--
-- SynthesisLayer.lua
-- Author     : JunMing Chen
-- Version    : 1.1.0.0
-- Date       : 2013-3-8
-- Description:合成系统
--

module("SynthesisLayer", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local mScene = nil 		-- 场景
local mLayer = nil 		
local mCurrentTab=nil
local mItemID=nil
local mData=nil
--
---------------公有接口(以下两个函数固定不用改变)--------
--

-- 函数名以小写字母开始，每个函数都应注释


-- 退出场景
function closeLayer()
	if mLayer then
		mLayer:getParent():removeChild(mLayer,true)
		mLayer=nil
	end
	releaseResource()
end


function setItemID(value, equipId)
	mItemID=value
	mEquipItemID = equipId
end;
function setDataInfo(value, bgType)
	mData=value
	mBgType = bgType
end;

-- 场景入口 -- 创建场景
function init(scene)
	if mScene then
		return
	end
	mScene = scene
	-- 添加背景
	mLayer =CCLayer:create()
	mScene:addChild(mLayer, 0)

			mScene:registerScriptHandler(SpriteEase_onEnterOrExit)
	
	-----------------
	local bgSprite=CCSprite:create(P("common/list_1024.png"))
	local boxSize=SZ(pWinSize.width,pWinSize.height*0.855)
	bgSprite:setScaleX(boxSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(boxSize.height/bgSprite:getContentSize().height)
	bgSprite:setAnchorPoint(PT(0.5,0))
	bgSprite:setPosition(PT(pWinSize.width/2,pWinSize.height*0.145))
	mLayer:addChild(bgSprite,0)
	local startX=pWinSize.width*0.05
	--背景
	for k=1 ,2 do
		local pingBiBtn=UIHelper.createActionRect(boxSize)
		pingBiBtn:setPosition(PT(0,bgSprite:getPosition().y))
		mLayer:addChild(pingBiBtn,0)
	end
	
	--标题
	--local titleLabel=CCLabelTTF:create(Language.PROP_TITLE,FONT_NAME,FONT_BIG_SIZE)
	local titleLabel=CCSprite:create(P("title/list_1114.png"))
	titleLabel:setAnchorPoint(PT(0.5,0))
	titleLabel:setPosition(PT(pWinSize.width/2,
						pWinSize.height*0.965-titleLabel:getContentSize().height))
	mLayer:addChild(titleLabel,0)
	-- 关闭按钮
	local closeBtn=ZyButton:new("button/list_1046.png")
	closeBtn:setAnchorPoint(PT(0,0))
	closeBtn:setPosition(PT(pWinSize.width-closeBtn:getContentSize().width*1.2,
							pWinSize.height-closeBtn:getContentSize().height*1.2))
	closeBtn:addto(mLayer,0)
	closeBtn:registerScriptHandler(closeLayer)
	

	----------------------------------定义六个固定位置

	local midSize=SZ(pWinSize.width*0.925,boxSize.height*0.8)
	local midSprite=CCSprite:create(P("common/list_1038.9.png"))
	midSprite:setScaleX(midSize.width/midSprite:getContentSize().width)
	midSprite:setScaleY(midSize.height/midSprite:getContentSize().height)
	midSprite:setAnchorPoint(PT(0.5,0))
	local startY=bgSprite:getPosition().y+pWinSize.height*0.073
	midSprite:setPosition(PT(pWinSize.width/2, startY))
	mLayer:addChild(midSprite,0)
	
	---
	local midBgSprite=CCSprite:create(P("common/list_1084.png"))
	startY=midSprite:getPosition().y+midSize.height*0.95
	local midBgSize=SZ(midBgSprite:getContentSize().width*1.3,pWinSize.height)
	local colW=midBgSize.width/3
	local rowH=boxSize.height*0.65/7
	local startX=(pWinSize.width-midBgSize.width)/2+colW/2
	local startY=startY-rowH/2
	local posTable={2,4,6,10,12,14}
	local mBgTecture=IMAGE(Image.Image_normalItemBg)
	local mateTable=mData.RecordTabel2
	for k, v in ipairs(posTable) do
		--物品图片
		if mateTable[k] then
			local goodBg=createItem(mBgTecture,mateTable[k].HeadID,mateTable[k].MaterialsName
								,mateTable[k].CurNum,mateTable[k].MaxNum)
			local posX=startX+((v-1)%3)*colW
			local posY=startY-math.floor((v-1)/3)*rowH
			goodBg:setPosition(PT(posX,posY))
			mLayer:addChild(goodBg,1)
		end
	end
	
	------------------------合成的物品位置  中间位置
	local itemSprite=createItem(mBgTecture,mData.HeadID,mData.ItemName)
	local posX=startX+((8-1)%3)*colW
	local posY=startY-math.floor((8-1)/3)*rowH
	itemSprite:setPosition(PT(posX,posY))
	mLayer:addChild(itemSprite,1)
	---
	midBgSprite:setAnchorPoint(PT(0.5,0.5))
	midBgSprite:setPosition(PT(itemSprite:getPosition().x,
								itemSprite:getPosition().y))
	mLayer:addChild(midBgSprite,0)
	
	-----------------------合成预览
	startY=midSprite:getPosition().y+SY(2)
	local tipSize=SZ(pWinSize.width,pWinSize.height*0.18)
	local tipBg=CCSprite:create(P("common/list_1052.9.png"))
	tipBg:setScaleY(tipSize.height/tipBg:getContentSize().height)	
	tipBg:setAnchorPoint(PT(0.5,0))
	tipBg:setPosition(PT(pWinSize.width/2,startY))
	mLayer:addChild(tipBg,0)
	startX=pWinSize.width/2-tipBg:getContentSize().width*0.48
	startY=startY-SY(2)+tipSize.height
	local rowH=tipSize.height/6
	local tipLabel=CCLabelTTF:create(Language.PROP_TIP..":",FONT_NAME,FONT_SM_SIZE)
	tipLabel:setAnchorPoint(PT(0,0))
	tipLabel:setPosition(PT(startX,
							startY-rowH))
	mLayer:addChild(tipLabel,0)
	--------合成的物品名字 
	local nameLabel=CCLabelTTF:create(Language.PROP_NAME ..":" .. mData.ItemName,
								FONT_NAME,FONT_SM_SIZE)
	nameLabel:setAnchorPoint(PT(0,0))
	nameLabel:setPosition(PT(tipLabel:getPosition().x,
							tipLabel:getPosition().y-rowH))
	mLayer:addChild(nameLabel,0)
	----获得的属性 
	local charLabel=CCLabelTTF:create(Language.PROP_CHARTER ..":",
								FONT_NAME,FONT_SM_SIZE)
	charLabel:setAnchorPoint(PT(0,0))
	charLabel:setPosition(PT(nameLabel:getPosition().x,
							nameLabel:getPosition().y-rowH))
	mLayer:addChild(charLabel,0)
	local startX=nameLabel:getPosition().x+charLabel:getContentSize().width+SX(2)
	local startY=charLabel:getPosition().y
	local colW=boxSize.width*0.3
	if mData.RecordTabel and #mData.RecordTabel>0 then
	for k, v in pairs(mData.RecordTabel) do
		local str=(Language.BAG_TYPE_[v.AbilityType] or "") .. "+" .. v.BaseNum	
		local label=CCLabelTTF:create(str,FONT_NAME,FONT_SM_SIZE)
		label:setAnchorPoint(PT(0,0))
		local posX=startX+colW*((k-1)%2)
		local posY=startY-rowH*math.floor((k-1)/2)
		label:setPosition(PT(posX,posY))
		mLayer:addChild(label,0)	
	end
	else
		local noneLabel=CCLabelTTF:create(Language.IDS_NONE,
									FONT_NAME,FONT_SM_SIZE)
		noneLabel:setAnchorPoint(PT(0,0))
		noneLabel:setPosition(PT(charLabel:getPosition().x+charLabel:getContentSize().width,
								charLabel:getPosition().y))
		mLayer:addChild(noneLabel,0)								
	end
	
--最后的按钮
	startY=charLabel:getPosition().y-math.floor(#mData.RecordTabel/2)*rowH
	local makeBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.PROP_MAKE	)
	makeBtn:setAnchorPoint(PT(0,0))
	makeBtn:setPosition(PT(pWinSize.width/2-makeBtn:getContentSize().width/2,
							tipBg:getPosition().y+SY(2)))
	makeBtn:addto(mLayer,0)
	makeBtn:registerScriptHandler(makeAction)
end

function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        releaseResource()
    end
end


--制作
function makeAction()
	if not isClick then
		isClick=true
		sendAction(1603, 1)--1：普通合成
	end
end;

function jingshiMake(clickedButtonIndex, content, tag)
	if clickedButtonIndex == 1 then
		sendAction(1603, 2)--2：晶石合成
	end
end

function AskIsMake(clickedButtonIndex, content, tag)
	if clickedButtonIndex == 1 then
		sendAction(1603, 3)--3：确认晶石合成
	end
end;


function sendAction(actionId, ops)
	if actionId == 1603 then
		local ItemID = nil--卷轴ID
		local UserItemID = nil--佣兵穿戴装备ID
		if mEquipItemID then
			UserItemID = mEquipItemID
		else
		 	ItemID = mItemID
		end
		actionLayer.Action1603(mScene,nil,ItemID,UserItemID,ops)--   ops 1：普通合成2：晶石合成3：确认晶石合成
	end
end;

function  createItem(mBgTecture,image,name,curNum ,maxNum)
		local goodBg=CCSprite:createWithTexture(mBgTecture)
		goodBg:setAnchorPoint(PT(0.5,0.5))
		if image then
		local path=string.format("smallitem/%s.png",image)
		local imageSprite=CCSprite:create(P(path))
		imageSprite:setAnchorPoint(PT(0.5,0.5))
		imageSprite:setPosition(PT(goodBg:getContentSize().width/2,
									goodBg:getContentSize().height/2))
		goodBg:addChild(imageSprite,0)	
		local goodName=CCLabelTTF:create(name,FONT_NAME,FONT_SM_SIZE)
		goodName:setAnchorPoint(PT(0.5,1))
		goodName:setPosition(PT(imageSprite:getPosition().x,0))
		goodBg:addChild(goodName,0)
			if curNum then
				local numStr=string.format("%d/%d",curNum,maxNum)
				local numLabel=CCLabelTTF:create(numStr,
								FONT_NAME,FONT_SM_SIZE)
				numLabel:setAnchorPoint(PT(1,0))
				numLabel:setPosition(PT(goodBg:getContentSize().width*0.94,goodBg:getContentSize().height*0.06))
				goodBg:addChild(numLabel,0)
			end
		end
		return goodBg
end;

--
-------------------------私有接口------------------------


-- 初始化资源、成员变量
function initResource()

end

-- 释放资源
function releaseResource()
 	mScene = nil 		-- 场景
 	mLayer = nil 	
	mData=nil
	mEquipItemID=nil	
end

---晶石不足 充值界面
function  czAction(clickedButtonIndex,content,tag)
	if clickedButtonIndex == ID_MBOK then
		TopUpScene.init()
	end
end


-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionID = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if actionID==1603 then
		if ZyReader:getResult() == eScutNetSuccess then
			closeLayer()
			if mBgType then
				HeroScene.showCreatSuccess()
				HeroScene.refreshWin()
			else
				BagScene.showLayer()
				BagScene.showCreatSuccess()
			end
		elseif ZyReader:getResult() == 1 then--晶石不足返回1	
			local box = ZyMessageBoxEx:new()
			box:doQuery(pScutScene, nil, Language.Business_Str,Language.TIP_YES,Language.TIP_NO,czAction)				
		elseif ZyReader:getResult() == 2 then--确认晶石返回2
				local box = ZyMessageBoxEx:new()
				box:doQuery(pScutScene, nil, ZyReader:readErrorMsg(),Language.IDS_SURE,Language.IDS_CANCEL,AskIsMake)
		elseif  ZyReader:getResult() == 3 then--材料不足CODE返回3
			if isShowVip() then
				ZyToast.show(pScutScene,Language.PROP_ENOUGH)
			else
				ZyToast.show(pScutScene,Language.PROP_ENOUGHNOVIP)
			end
		else
			ZyToast.show(pScutScene,ZyReader:readErrorMsg())
		end		
	end
	isClick=false
	
end

