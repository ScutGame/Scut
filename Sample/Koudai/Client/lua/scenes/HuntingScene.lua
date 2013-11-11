------------------------------------------------------------------
-- HuntingHotelScene.lua
-- Author     :
-- Version    : 1.0
-- Date       :
-- Description: ,
------------------------------------------------------------------
module("HuntingScene", package.seeall)

require("layers.HuntSparLayer")

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local mScene = nil 		-- 场景
local mLayer=nil
local mContentLayer=nil
local mToUserID=nil
local bottomY=nil
local mServerData=nil
local mCurrentSparTable=nil
local isClick=nil
local mCurrentGeneral=nil
local mChoicePosition=nil
local mShowSparInfo=nil
local clickBottomIndex=nil
local firshClickCryId=nil
local mChoiceLayer=nil
local isfusion=false
local isGotoHunt=nil

local mOpenIndex=nil
local m_choiceImge=nil
local mChoiceIndex=nil
local m_listTable=nil
local mEquipLayer=nil
local mEquipChoiceIndex=nil
local mEquipTable=nil

--
---------------公有接口(以下两个函数固定不用改变)--------
--

-- 函数名以小写字母开始，每个函数都应注释

-- 退出场景
function closeScene()
	releaseResource()
	MainScene.init()
end

--设置查看
function setTouserID(value)
	mToUserID=value
end;

---
function init()
	if mScene then
		return
	end	
	initResource()
	local scene  = ScutScene:new()
	-- 注册网络回调
	scene:registerCallback(networkCallback)
	mScene = scene.root 
		mScene:registerScriptHandler(SpriteEase_onEnterOrExit)
	SlideInLReplaceScene(mScene,1)
	
	-- 添加背景
	mLayer = CCLayer:create()
	mScene:addChild(mLayer, 0)

	--创建背景
	local bgLayer=createBgLayer()
	bgLayer:setPosition(PT(0,0))
	mLayer:addChild(bgLayer,0)
--	MainMenuLayer.init(2, mScene)
	actionLayer.Action1301(mScene,nil,mToUserID)
end;


function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        releaseResource()
    end
end


--创建背景层
function  createBgLayer()
	local layer=CCLayer:create()
	layer:setAnchorPoint(PT(0,0))
	--大背景
	local bgSprite=CCSprite:create(P("common/list_1076.png"))
	bgSprite:setAnchorPoint(PT(0.5,0.5))
	bgSprite:setScaleX(pWinSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(pWinSize.height/bgSprite:getContentSize().height)
	bgSprite:setPosition(PT(pWinSize.width/2,pWinSize.height/2))
	layer:addChild(bgSprite,0)
	--中间层
	local midSprite=CCSprite:create(P("common/list_1074.png"))
	midSprite:setAnchorPoint(PT(0.5,0.5))
	midSprite:setScaleX(pWinSize.width/midSprite:getContentSize().width)
	midSprite:setScaleY(pWinSize.height/midSprite:getContentSize().height)
	
	midSprite:setPosition(PT(pWinSize.width/2,pWinSize.height/2))
	layer:addChild(midSprite,0)
	--]]
	return layer
end;

--佣兵选择
function  createTopLayer()
	if mServerData.RecordTabel then
		local info=mServerData.RecordTabel
		local layer=CCLayer:create()
		 ---LIST 控件
		local startY=pWinSize.height*0.95
		local listBg=CCSprite:create(P("common/list_1052.9.png"))
		local listSize=SZ(listBg:getContentSize().width*0.95,pWinSize.height*0.12)
		listBg:setAnchorPoint(PT(0.5,0))
		listBg:setScaleY(listSize.height/listBg:getContentSize().height)
		listBg:setPosition(PT(pWinSize.width/2,startY-listSize.height))
		layer:addChild(listBg,0)
			
		local lSprite=CCSprite:create(P("button/list_1069.png"))
		lSprite:setAnchorPoint(PT(1,0.5))
		lSprite:setPosition(PT((pWinSize.width-listBg:getContentSize().width)/2,startY-listSize.height/2))
		layer:addChild(lSprite,0)
		local rSprite=CCSprite:create(P("button/list_1068.png"))
		rSprite:setAnchorPoint(PT(0,0.5))
		rSprite:setPosition(PT(pWinSize.width-(pWinSize.width-listBg:getContentSize().width)/2,
							lSprite:getPosition().y))
		layer:addChild(rSprite,0)
		local listY=listBg:getPosition().y+listSize.height*0.05
		listSize=SZ(listSize.width,listSize.height*0.9)
		local listX=(pWinSize.width-listSize.width)/2
		local mListColWidth=listSize.width/4
		local list = ScutCxList:node(mListColWidth, ccc4(24, 24, 24, 0), listSize)
		list:setAnchorPoint(PT(0,0))
		list:setPosition(PT(listX,listY))
		list:setRecodeNumPerPage(1)
		layer:addChild(list,1)
		list:setTouchEnabled(true)
		list:setHorizontal(true)	
		local layout = CxLayout()
		layout.val_x.t = ABS_WITH_PIXEL
		layout.val_y.t = ABS_WITH_PIXEL
		layout.val_x.val.pixel_val = 0
		layout.val_y.val.pixel_val = 0
		
		if not mChoiceIndex then
			mChoiceIndex=1
		end
		for k, v in pairs(info) do
		 	local listItem = ScutCxListItem:itemWithColor(ccc3(24,24,24))
			listItem:setOpacity(0)
			local layer=CCLayer:create()		
						--物品图片
						
			local mBgTecture=getQualityBg(v.GeneralQuality, 1)	
			local spriteBg=CCSprite:createWithTexture(IMAGE(mBgTecture))
			spriteBg:setAnchorPoint(PT(0.5,0.5))
			spriteBg:setPosition(PT(mListColWidth/2,listSize.height/2))
			layer:addChild(spriteBg,0)
		--	local roleInfo=RoleInfoConfig.getRoleInfo(v.HeadID)
			local imagePath=string.format("smallitem/%s.png",v.HeadID )
			local headSprite=CCSprite:create(P(imagePath))
			headSprite:setAnchorPoint(PT(0.5,0.5))
			headSprite:setPosition(PT(spriteBg:getContentSize().width/2,spriteBg:getContentSize().height/2))
			spriteBg:addChild(headSprite,0)
			
			local actionBtn,menuItemHead=UIHelper.createActionRect(spriteBg:getContentSize(),HuntingScene.changeSoldier,k)
			actionBtn:setPosition(PT(0,0))
			spriteBg:addChild(actionBtn,0)
			if k==mChoiceIndex then
				  refreshClickAction(menuItemHead)
			end
			listItem:addChildItem(layer, layout)
			list:addListItem(listItem, false)
		end

		list:selectChild(0)
		mCurrentGeneral=mServerData.RecordTabel[1]
		mContentLayer:addChild(layer,0)
	end	
end;

function  refreshClickAction(node)
	local actionNode=node
	if m_choiceImge ~= nil then
		m_choiceImge:getParent():removeChild(m_choiceImge,true)
		m_choiceImge=nil
	end
	if m_choiceImge == nil then
		m_choiceImge = CCSprite:create(P(Image.Image_choicebox))
		m_choiceImge:setAnchorPoint(CCPoint(0.5, 0.5))
		local bosSize=SZ(actionNode:getContentSize().width,actionNode:getContentSize().height)
		m_choiceImge:setPosition(PT(actionNode:getContentSize().width*0.5,actionNode:getContentSize().height*0.5))
		actionNode:getParent():addChild(m_choiceImge,0)
	end
end;

--选择佣兵
function changeSoldier(node)
	local tag=node:getTag()
	mChoiceIndex=tag
		
	refreshClickAction(node)
	if mServerData.RecordTabel[tag] then
		local GeneralID= mServerData.RecordTabel[tag].GeneralID
		mCurrentGeneral=mServerData.RecordTabel[tag]
		if not isClick then
		isClick =true
		actionLayer.Action1302(mScene,false,GeneralID,mToUserID)
		end
	end
end;

--刷新中间信息
function releaseMidLayer()
	if mMidLayer then
		mMidLayer:getParent():removeChild(mMidLayer,true)
		mMidLayer=nil
	end
end;

function  refreshMidLayer(info,path,GeneralQuality)
	mCurrentSparTable=info
	releaseMidLayer()
	local layer=CCLayer:create()
	mMidLayer=layer
	mContentLayer:addChild(layer,0)
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(0,0))

	local startY=pWinSize.height*0.85
	
	--中间显示佣兵
	local bgPic =  getQualityBg(GeneralQuality, 3)
	local bgSprite=CCSprite:create(P(bgPic))
	bgSprite:setAnchorPoint(PT(0.5,0))
	bgSprite:setPosition(PT(pWinSize.width/2,startY-bgSprite:getContentSize().height-SY(5)))
	layer:addChild(bgSprite,0)
	
	local imagePath=string.format("bigitem/%s.png",path   )
	local generalSprite=CCSprite:create(P(imagePath))	
	generalSprite:setAnchorPoint(PT(0.5,0.5))
	generalSprite:setPosition(PT(bgSprite:getContentSize().width/2,bgSprite:getContentSize().height/2))
	bgSprite:addChild(generalSprite,0)
	
	--
	local colW=(pWinSize.width-bgSprite:getContentSize().width)/2
	local rowH=bgSprite:getContentSize().height*0.3
	local startY=bgSprite:getPosition().y+bgSprite:getContentSize().height-rowH/2
	local startX=colW/2
	local positionTable={}
	local mBgTecture=IMAGE("common/list_1088.png")
	local mLockTecture=IMAGE("common/list_1088_1.png")
	local maxNum=mCurrentGeneral.OpenNum
	
	for k=1, 6 do
		local headSprite=CCSprite:createWithTexture(mBgTecture)
		headSprite:setAnchorPoint(PT(0.5,0.5))
		local posX=startX+((k-1)%2)*(colW+bgSprite:getContentSize().width)
		local posY=startY-math.floor((k-1)/2)*rowH
		
		headSprite:setPosition(PT(posX,posY))
		layer:addChild(headSprite,0)
		local fun=HuntingScene.equipSparAction
		if k >maxNum then
			fun=HuntingScene.LockAction
			local lockSprite=CCSprite:createWithTexture(mLockTecture)
			lockSprite:setAnchorPoint(PT(0.5,0.5))
			lockSprite:setPosition(PT(headSprite:getContentSize().width/2,
							headSprite:getContentSize().height/2))
			headSprite:addChild(lockSprite,0)				
		end
		local actionBtn,menuItemHead=UIHelper.createActionRect(headSprite:getContentSize(),fun,k)
		actionBtn:setPosition(PT(0,0))
		headSprite:addChild(actionBtn,0)
		positionTable[k]={}
		positionTable[k].headSprite=headSprite
		positionTable[k].actionBtn=menuItemHead	
	end
	--
	for k, v in pairs(info) do	
		local pImage = string.format("smallitem/%s.png",v.CrystalHeadID)
		local sparSprite=CCSprite:create(P(pImage))
		sparSprite:setAnchorPoint(PT(0.5,0.5))
		local positionInfo=positionTable[v.Position]
		if positionInfo then
			local parent=positionInfo.headSprite
			parent:addChild(sparSprite,0)
			sparSprite:setPosition(PT(parent:getContentSize().width/2,
							parent:getContentSize().height/2))
			positionInfo.actionBtn:registerScriptHandler(sparInfoAction)
			positionInfo.actionBtn:setTag(k)
			
		end
	end	
	local startY=bgSprite:getPosition().y
	bottomY=startY
	--
end;


function LockAction(node)
	local tag=node:getTag()
	if tag and tag >0 then
		local baseNum=(tag+1)*10
		ZyToast.show(mScene,string.format(Language.LIFECELL_TIP5,baseNum))
	end
end;

--释放详细表
function releaseDetailLayer()
	if mDetailLayer then
		mDetailLayer:getParent():removeChild(mDetailLayer,true)
		mDetailLayer=nil
	end
end;

--点击空格子响应
function equipSparAction(node)
	local tag=node:getTag()
	mChoicePosition=tag
	clickBottomIndex=nil
	releaseDetailLayer()
   	actionLayer.Action1311(mScene,nil,1)	

end;


function releaseEquipLayer()
	MainMenuLayer.releaseResource()
	mEquipChoiceIndex=nil
	mEquipTable={}
	mChoiceIndex=nil
	if mEquipLayer then
		mEquipLayer:getParent():removeChild(mEquipLayer,true)
		mEquipLayer=nil
	end
end;


function createEquipLayer(serverData)
	 releaseEquipLayer()
	mEquipTable=serverData
	mEquipChoiceIndex=nil
	local layer=CCLayer:create()
	mEquipLayer=layer
	mContentLayer:addChild(layer,2)		
	local bgSprite=CCSprite:create(P("common/list_1076.png"))
	bgSprite:setScaleX(pWinSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(pWinSize.height/bgSprite:getContentSize().height)
	bgSprite:setAnchorPoint(PT(0.5,0))
	bgSprite:setPosition(PT(pWinSize.width/2,0))
	layer:addChild(bgSprite,0)
		
	local eSprite=CCSprite:create(P("common/list_1074.png"))
	eSprite:setScaleX(pWinSize.width/eSprite:getContentSize().width)
	eSprite:setScaleY(pWinSize.height/eSprite:getContentSize().height)
	eSprite:setAnchorPoint(PT(0.5,0))
	eSprite:setPosition(PT(pWinSize.width/2,0))
	layer:addChild(eSprite,0)
	
	local bosSize=SZ(pWinSize.width*0.9,pWinSize.height*0.9)
	local actionBtn=UIHelper.createActionRect(pWinSize)
	actionBtn:setPosition(PT(0,0))
	layer:addChild(actionBtn,0)
	
	
	MainMenuLayer.init(2, mEquipLayer)
	local closeBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.LIFECELL_TIP6)
	closeBtn:setAnchorPoint(PT(0,0))
	closeBtn:setPosition(PT(pWinSize.width*0.1,pWinSize.height*0.94-closeBtn:getContentSize().height))
	closeBtn:addto(layer,0)
	closeBtn:registerScriptHandler(releaseEquipLayer)
	
	local sureBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.IDS_OK)
	sureBtn:setAnchorPoint(PT(0,0))
	sureBtn:setPosition(PT(pWinSize.width*0.9-sureBtn:getContentSize().width,
				closeBtn:getPosition().y))
	sureBtn:addto(layer,0)
	sureBtn:registerScriptHandler(makeEquipAction)	

	if serverData  and #serverData>0 then
		local listSize=SZ(bosSize.width,bosSize.height*0.8)
		local listY=sureBtn:getPosition().y-listSize.height-SY(2)
		local listX=(pWinSize.width-listSize.width)/2
		local mListRowHeight=listSize.height/5
		local list = ScutCxList:node(mListRowHeight, ccc4(24, 24, 24, 0), listSize)
		list:setTouchEnabled(true)
		list:setAnchorPoint(PT(0,0))
		list:setPosition(PT(listX,listY))
		layer:addChild(list,1)
		
		local layout = CxLayout()
		layout.val_x.t = ABS_WITH_PIXEL
		layout.val_y.t = ABS_WITH_PIXEL
		layout.val_x.val.pixel_val = 0
		layout.val_y.val.pixel_val = 0
		local mBgTecture=IMAGE("common/list_1088.png")
		for k, v in pairs(serverData) do
			if  not v.empty  then
		 	local listItem = ScutCxListItem:itemWithColor(ccc3(24,24,24))
			listItem:setOpacity(0)
			local sonBox=SZ(list:getContentSize().width*0.96,mListRowHeight)
			local posX=list:getContentSize().width*0.02
			
			local layer=CCLayer:create()
			local bgSprite=CCSprite:create(P("common/list_1038.9.png"))
			bgSprite:setScaleX(sonBox.width/bgSprite:getContentSize().width)
			bgSprite:setScaleY(sonBox.height/bgSprite:getContentSize().height)
			bgSprite:setAnchorPoint(PT(0,0.5))
			bgSprite:setPosition(PT(posX,sonBox.height/2))
			layer:addChild(bgSprite,0)
			
			local headBg=CCSprite:createWithTexture(mBgTecture)
			headBg:setAnchorPoint(PT(0,0.5))
			headBg:setPosition(PT(sonBox.width*0.06,sonBox.height/2))
			layer:addChild(headBg,0)
			
			local pImage = string.format("smallitem/%s.png",v.CrystalHeadID)
			local sparSprite=CCSprite:create(P(pImage))
			sparSprite:setAnchorPoint(PT(0.5,0.5))
			sparSprite:setPosition(PT(headBg:getContentSize().width/2,
									headBg:getContentSize().height/2))
			headBg:addChild(sparSprite,0)
			
			local rowH=sonBox.height/5
			local posX=headBg:getPosition().x+headBg:getContentSize().width+SX(3)
			
			----名称
			local nameLabel=CCLabelTTF:create(v.CrystalName,FONT_NAME,FONT_SM_SIZE)
			nameLabel:setAnchorPoint(PT(0,0))
			local color = ZyColor:getCrystalColor(v.CrystalQuality)
			nameLabel:setColor(color)
			nameLabel:setPosition(PT(posX,sonBox.height*0.9-rowH))
			layer:addChild(nameLabel,1)
			
			if v.GeneralName and string.len(v.GeneralName)>0 then
				local generalLabel=CCLabelTTF:create(string.format(Language.EQUIP_EQUIPMAN,v.GeneralName),FONT_NAME,FONT_SM_SIZE)
				generalLabel:setAnchorPoint(PT(0,0))
				generalLabel:setPosition(PT(sonBox.width*0.6,nameLabel:getPosition().y))
				layer:addChild(generalLabel,1)
			end
			
			--
			local str=  qualityText[v.CrystalQuality]
			local quilityLabel=CCLabelTTF:create(Language.EQUIP_PIN .. ":" .. str,FONT_NAME,FONT_SM_SIZE)
			quilityLabel:setAnchorPoint(PT(0,0))
			quilityLabel:setPosition(PT(nameLabel:getPosition().x,
									nameLabel:getPosition().y-rowH))
			layer:addChild(quilityLabel,1)
			
			
			local levelLabel=CCLabelTTF:create(Language.IDS_LEVEL .. ":" .. v.CrystalLv,FONT_NAME,FONT_SM_SIZE)
			levelLabel:setAnchorPoint(PT(0,0))
			levelLabel:setPosition(PT(nameLabel:getPosition().x+sonBox.width*0.25,
									quilityLabel:getPosition().y))
			layer:addChild(levelLabel,1)
			
			---属性
			local abilityName=""
			if Language.BAG_TYPE_[v.AbilityType] then
				abilityName=Language.BAG_TYPE_[v.AbilityType] .. ":+"  .. v.AttrNum
			end
			local abilityLabel=CCLabelTTF:create(abilityName,FONT_NAME,FONT_SM_SIZE)
			abilityLabel:setAnchorPoint(PT(0,0))
			abilityLabel:setPosition(PT(nameLabel:getPosition().x,
									levelLabel:getPosition().y-rowH))
			layer:addChild(abilityLabel,1)
			
			
			--选择按钮	
			local chioceBtn = ZyButton:new(Image.image_button_hook_0, Image.image_button_hook_1,Image.image_button_hook_0)
			chioceBtn:setAnchorPoint(PT(0,0))
			chioceBtn:setPosition(PT(sonBox.width*0.9-chioceBtn:getContentSize().width, sonBox.height/2-chioceBtn:getContentSize().height*0.5))
			chioceBtn:addto(layer, 0)
			chioceBtn:setTag(k)
		--	chioceBtn:registerScriptHandler(choiceEquipSparAction)
			mEquipTable[k].chioceBtn=chioceBtn
			
			local actionBtn=UIHelper.createActionRect(sonBox,HuntingScene.choiceEquipSparAction,k)
			actionBtn:setPosition(PT(bgSprite:getPosition().x,
										bgSprite:getPosition().y-	actionBtn:getContentSize().height/2))
			layer:addChild(actionBtn, 0)	
			listItem:addChildItem(layer, layout)
			list:addListItem(listItem, false)
			end
		end
	else
		local label=CCLabelTTF:create(Language.LIFECELL_TIP7,FONT_NAME,FONT_DEF_SIZE)
		label:setAnchorPoint(PT(0.5,0.5))
		label:setPosition(PT(pWinSize.width/2,pWinSize.height/2))
		layer:addChild(label,0)
	end 
end;


function choiceEquipSparAction(node)
	local tag=node:getTag()
	if mEquipChoiceIndex and  mEquipChoiceIndex==tag then
		mEquipChoiceIndex=nil
	else
		mEquipChoiceIndex=tag
	end
    for k, v in pairs(mEquipTable) do
        v.chioceBtn:unselected()
        if k==mEquipChoiceIndex then
            v.chioceBtn:selected()	
        end
    end
end;


function makeEquipAction()
	if mEquipChoiceIndex and mEquipTable[mEquipChoiceIndex] then
		if not isClick then
		isClick=true
		--装备
		local UserCrystalID=mEquipTable[mEquipChoiceIndex].UserCrystalID 
		actionLayer.Action1309(mScene,false ,mCurrentGeneral.GeneralID,UserCrystalID,mChoicePosition,1)
		end
	else
		releaseEquipLayer()
	end
end;


--点击查看已有的响应
function sparInfoAction(node)
local tag=node:getTag()
if mCurrentSparTable[tag] then
	clickBottomIndex=nil
	mShowSparInfo=mCurrentSparTable[tag]
	local UserCrystalID=mCurrentSparTable[tag].UserCrystalID
	firshClickCryId=UserCrystalID
	mChoicePosition=mCurrentSparTable[tag].Position
	if not isClick then
	isClick =true
	actionLayer.Action1304(mScene,false,UserCrystalID,mToUserID)
	end	
end
end;

--创建选择列表
function releaseChoiceLayer()
	isfusion=nil
	if mChoiceLayer then
		mChoiceLayer:getParent():removeChild(mChoiceLayer,true)
		mChoiceLayer=nil
	end
end;

function createChoiceSparLayer(info,tag)
releaseDetailLayer()
local layer=CCLayer:create()
mLayer:addChild(layer,2)
mChoiceLayer=layer
layer:setAnchorPoint(PT(0,0))
layer:setPosition(PT(0,0))
for k=1 ,2 do
	local pingbiBtn=UIHelper.createActionRect(pWinSize)
	pingbiBtn:setPosition(PT(0,0))
	layer:addChild(pingbiBtn,0)
end

local midBox=SZ(pWinSize.width,pWinSize.height*0.3)
local midSprite=CCSprite:create(P("common/list_1038.9.png"))
midSprite:setScaleX(midBox.width/midSprite:getContentSize().width)
midSprite:setScaleY(midBox.height/midSprite:getContentSize().height)
midSprite:setAnchorPoint(PT(0.5,0.5))
midSprite:setPosition(PT(pWinSize.width/2,pWinSize.height/2))
layer:addChild(midSprite,0)


 ---LIST 控件
local listSize=SZ(pWinSize.width*0.9,pWinSize.height*0.2)
local listX=(pWinSize.width-listSize.width)/2
local listY=pWinSize.height/2-listSize.height/2
local mListColWidth=listSize.width/4
local list = ScutCxList:node(mListColWidth, ccc4(24, 24, 24, 0), listSize)
list:setTouchEnabled(true)
list:setAnchorPoint(PT(0,0))
list:setHorizontal(true)	
list:setPosition(PT(listX,listY))
layer:addChild(list,1)

local layout = CxLayout()
layout.val_x.t = ABS_WITH_PIXEL
layout.val_y.t = ABS_WITH_PIXEL
layout.val_x.val.pixel_val = 0
layout.val_y.val.pixel_val = 0
local mBgTecture=IMAGE("common/list_1088.png")
for k, v in pairs(info) do
	if k~=tag and not v.empty then
 	local listItem = ScutCxListItem:itemWithColor(ccc3(24,24,24))
	listItem:setOpacity(0)
	local layer=CCLayer:create()
	local headBg=CCSprite:createWithTexture(mBgTecture)
	headBg:setAnchorPoint(PT(0.5,0.5))
	headBg:setPosition(PT(mListColWidth/2,listSize.height/2))
	layer:addChild(headBg,0)
	local pImage = string.format("smallitem/%s.png",v.CrystalHeadID)
	local sparSprite=CCSprite:create(P(pImage))
	sparSprite:setAnchorPoint(PT(0.5,0.5))
	sparSprite:setPosition(PT(mListColWidth/2,listSize.height/2))
	layer:addChild(sparSprite,0)
	local str=v.CrystalName .. string.format(Language.IDS_LVSTR,v.CrystalLv)
	local nameLabel=CCLabelTTF:create(str,FONT_NAME,FONT_SMM_SIZE)
	local color = ZyColor:getCrystalColor(v.CrystalQuality)
	nameLabel:setColor(color)
	nameLabel:setAnchorPoint(PT(0.5,1))
	nameLabel:setPosition(PT(mListColWidth/2,
								headBg:getPosition().y-headBg:getContentSize().height/2))
	layer:addChild(nameLabel,0)
	local actionBtn,menuItemHead=UIHelper.createActionRect(SZ(mListColWidth,listSize.height),HuntingScene.choiceSparAction,k)
	actionBtn:setPosition(PT(0,0))
	layer:addChild(actionBtn,0)
	listItem:addChildItem(layer, layout)
	list:addListItem(listItem, false)
	end
end
	list:selectChild(0)	
	-----
	local closeBtn=ZyButton:new("button/list_1046.png")
	closeBtn:setAnchorPoint(PT(0,0))
	closeBtn:setPosition(PT(pWinSize.width/2+midBox.width/2-closeBtn:getContentSize().width*1.1,
							midSprite:getPosition().y+midBox.height/2-closeBtn:getContentSize().height*1.2))
	closeBtn:addto(layer,1)
	closeBtn:registerScriptHandler(releaseChoiceLayer)
end;

----装备 合成水晶
function choiceSparAction(node)
	local tag=node:getTag()
	if mServerData.RecordTabel3[tag] then 
		local UserCrystalID= mServerData.RecordTabel3[tag].UserCrystalID	
		if not isClick then
			isClick=true
			---合成
			if isfusion then
				actionLayer.Action1308(mScene,false ,firshClickCryId,UserCrystalID,1)
			else
			--装备
				actionLayer.Action1309(mScene,false ,mCurrentGeneral.GeneralID,UserCrystalID,mChoicePosition,1)
			end
		end	
	end
end;

--详细信息
function createDetailLayer(info)
releaseDetailLayer()
local layer=CCLayer:create()
mLayer:addChild(layer,2)
mDetailLayer=layer
layer:setAnchorPoint(PT(0,0))
layer:setPosition(PT(0,0))
for k=1 ,2 do
	local pingbiBtn=UIHelper.createActionRect(pWinSize,HuntingScene.releaseDetailLayer)
	pingbiBtn:setPosition(PT(0,0))
	layer:addChild(pingbiBtn,0)
end

-----
local midSize=SZ(pWinSize.width*0.65,pWinSize.height*0.4)
local midBgSprite=CCSprite:create(P("common/list_1038.9.png"))
midBgSprite:setScaleX(midSize.width/midBgSprite:getContentSize().width)
midBgSprite:setScaleY(midSize.height/midBgSprite:getContentSize().height)
midBgSprite:setAnchorPoint(PT(0.5,0.5))
midBgSprite:setPosition(PT(pWinSize.width/2,pWinSize.height*0.6))
layer:addChild(midBgSprite,0)

----
local startX=pWinSize.width/2-midSize.width*0.45
local startY=pWinSize.height*0.6+midSize.height*0.38

----
local mBgTecture=IMAGE("common/list_1088.png")
local headBg=CCSprite:createWithTexture(mBgTecture)
headBg:setAnchorPoint(PT(0,0))
headBg:setPosition(PT(startX,startY-headBg:getContentSize().height))
layer:addChild(headBg,0)

---
local imagePath = string.format("smallitem/%s.png",info.HeadID)
local headSprite=CCSprite:create(P(imagePath))
headSprite:setAnchorPoint(PT(0.5,0.5))
headSprite:setPosition(PT(headBg:getContentSize().width/2,headBg:getContentSize().height/2))
headBg:addChild(headSprite,0)

----名称
startX=startX+headBg:getContentSize().width*1.2
local nameLabel=CCLabelTTF:create(info.CrystalName,FONT_NAME,FONT_DEF_SIZE)
nameLabel:setAnchorPoint(PT(0,0))
local color = ZyColor:getCrystalColor(info.CrystalQuality)
nameLabel:setColor(color)
nameLabel:setPosition(PT(startX,
						headBg:getPosition().y+headBg:getContentSize().height-nameLabel:getContentSize().height))
layer:addChild(nameLabel,1)

--
local str=  qualityText[info.CrystalQuality]
local quilityLabel=CCLabelTTF:create(Language.EQUIP_PIN .. ":" .. str,FONT_NAME,FONT_SM_SIZE)
quilityLabel:setAnchorPoint(PT(0,0))
quilityLabel:setPosition(PT(startX,
						nameLabel:getPosition().y-nameLabel:getContentSize().height*1.5))
layer:addChild(quilityLabel,1)


local levelLabel=CCLabelTTF:create(Language.IDS_LEVEL .. ":" .. info.CrystalLv,FONT_NAME,FONT_SM_SIZE)
levelLabel:setAnchorPoint(PT(0,0))
levelLabel:setPosition(PT(startX,
						quilityLabel:getPosition().y-quilityLabel:getContentSize().height*1.5))
layer:addChild(levelLabel,1)

---命格经验
local str=Language.HUNT_EXP .. ":" .. string.format("%d/%d",info.CrystalExperience,info.MaxExperience)
local expLabel=CCLabelTTF:create(str,FONT_NAME,FONT_SM_SIZE)
expLabel:setAnchorPoint(PT(0,0))
expLabel:setPosition(PT(startX,
						levelLabel:getPosition().y-levelLabel:getContentSize().height*1.5))
layer:addChild(expLabel,1)

---属性
local abilityName=""
if Language.BAG_TYPE_[info.AbilityType] then
	abilityName=Language.BAG_TYPE_[info.AbilityType] .. ":+"  .. info.AttrNum
end
local abilityLabel=CCLabelTTF:create(abilityName,FONT_NAME,FONT_SM_SIZE)
abilityLabel:setAnchorPoint(PT(0,0))
abilityLabel:setPosition(PT(startX,
						expLabel:getPosition().y-expLabel:getContentSize().height*1.5))
layer:addChild(abilityLabel,1)

-----关闭按钮
local closeBtn=ZyButton:new("button/list_1046.png")
closeBtn:setAnchorPoint(PT(0,0))
closeBtn:setPosition(PT(pWinSize.width/2+midSize.width/2-closeBtn:getContentSize().width*1.1,
						midBgSprite:getPosition().y+midSize.height/2-closeBtn:getContentSize().height*1.2))
closeBtn:addto(layer,1)
closeBtn:registerScriptHandler(releaseDetailLayer)

--合成按钮
local  fusionBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.HUNT_FUSION)
fusionBtn:setAnchorPoint(PT(0,0))
fusionBtn:addto(layer,1)
fusionBtn:setPosition(PT(pWinSize.width/2-fusionBtn:getContentSize().width/2,
							midBgSprite:getPosition().y-midSize.height*0.45))
fusionBtn:registerScriptHandler(fusionAction)

--卸下按钮
local unloadBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.HUNT_UNLOAD)
unloadBtn:setAnchorPoint(PT(0,0))
unloadBtn:addto(layer,1)
unloadBtn:registerScriptHandler(unloadAction)
unloadBtn:setPosition(PT(pWinSize.width/2+midSize.width/2-fusionBtn:getContentSize().width*1.1,
							fusionBtn:getPosition().y))
if clickBottomIndex then
	fusionBtn:setPosition(PT(unloadBtn:getPosition().x,
								midBgSprite:getPosition().y-midSize.height*0.45))
	unloadBtn:setVisible(false)
end


end;

--合成
function fusionAction(node)
local tag=node:getTag()
isfusion=true
releaseDetailLayer()
createChoiceSparLayer(mServerData.RecordTabel3,clickBottomIndex)
end;

----卸下
function unloadAction(node)
	local UserCrystalID=mShowSparInfo.UserCrystalID
	if not isClick then
		isClick=true
		actionLayer.Action1309(mScene,false ,mCurrentGeneral.GeneralID,UserCrystalID,mChoicePosition,0)
	end	
end;

--创建玩家拥有的晶石层
function releaseBottomLayer()
	if mBottomLayer then
		mBottomLayer:getParent():removeChild(mBottomLayer,true)
		mBottomLayer=nil
	end
end;

--刷新页码
function refreshPage()
	if m_listTable.pageLabel then
	local str=string.format("%d/%d",m_listTable.currentPage,m_listTable.maxPage)
	m_listTable.pageLabel:setString(str)
	end
end;

--list翻页
function callbackListview(page)
	if tonumber(page) <0 then
		page=0
	end
	m_listTable.currentPage=tonumber(page)+1	
	refreshPage()
end

--底下的晶石表
function refreshBottomLayer()
	releaseBottomLayer()
	local layer=CCLayer:create()
	mBottomLayer=layer
	mContentLayer:addChild(layer,0)
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(0,0))
	
	local topSprite=CCSprite:create(P("common/list_1085.png"))
	local startY=bottomY
	
	local bottomBox=SZ(pWinSize.width*0.9,pWinSize.height*0.34)
	local bSprite=CCSprite:create(P("common/list_1038.9.png"))
	bSprite:setScaleX(bottomBox.width/bSprite:getContentSize().width)
	bSprite:setScaleY(bottomBox.height/bSprite:getContentSize().height)
	bSprite:setAnchorPoint(PT(0,0))
	bSprite:setPosition(PT(pWinSize.width*0.05,startY-bottomBox.height))
	layer:addChild(bSprite,0)
	
	
	--
	local mergerBtn=ZyButton:new("button/list_1023.png",nil,nil,Language.HUNT_MER)
	mergerBtn:setAnchorPoint(PT(0,0))
	mergerBtn:setPosition(PT(bSprite:getPosition().x+bottomBox.width/4-mergerBtn:getContentSize().width/2,
								bSprite:getPosition().y+bottomBox.height/20))
	mergerBtn:addto(layer,0)
	mergerBtn:registerScriptHandler(fusionAllAction)
	--前往猎命
	local huntingBtn=ZyButton:new("button/list_1023.png",nil,nil,Language.HUNT_ING)
	huntingBtn:setAnchorPoint(PT(0,0))
	huntingBtn:setPosition(PT(bSprite:getPosition().x+bottomBox.width/4*3-huntingBtn:getContentSize().width/2,
								mergerBtn:getPosition().y))
	huntingBtn:addto(layer,0)
	huntingBtn:registerScriptHandler(gotoHunting)	
	--
	
	local listY=huntingBtn:getPosition().y+huntingBtn:getContentSize().height
	local listX=bSprite:getPosition().x+bottomBox.width*0.02
	local listSize=SZ(bottomBox.width*0.96,bottomBox.height-mergerBtn:getContentSize().height*1.2)
	local list = ScutCxList:node(listSize.width, ccc4(24, 24, 24, 0), listSize)	
	list:setAnchorPoint(PT(0,0))
	list:setPosition(PT(listX,listY))
	list:setRecodeNumPerPage(1)
	layer:addChild(list,1)
	list:setHorizontal(true)
	list:setPageTurnEffect(true)
	list:registerLoadEvent("HuntingScene.callbackListview")
	
	m_listTable={}
	m_listTable.currentPage=1
	m_listTable.maxPage=1
	
	local rows=50;	
	local pageSize=10
	local sparTable=mServerData.RecordTabel3 or {}
	rows=rows>#sparTable and rows or #sparTable;
	if rows>#sparTable then
		for i=1,rows-#sparTable do
		        sparTable[#sparTable+1]={empty=true}
		end
	end
	
	local pageCount,pageTable=ZyTable.GetPaging(pageSize,sparTable)
	
	 ---页码
	 m_listTable.maxPage=pageCount

	local str=string.format("%d/%d",m_listTable.currentPage,m_listTable.maxPage)
	local pageLabel=CCLabelTTF:create(str,FONT_NAME,FONT_FM_SIZE)
	pageLabel:setAnchorPoint(PT(0.5,0))
	pageLabel:setPosition(PT(pWinSize.width/2,list:getPosition().y-SY(5)))
	layer:addChild(pageLabel,0)
	m_listTable.pageLabel=pageLabel

	
	local layout = CxLayout()
	layout.val_x.t = ABS_WITH_PIXEL
	layout.val_y.t = ABS_WITH_PIXEL
	layout.val_x.val.pixel_val = 0
	layout.val_y.val.pixel_val = 0
	local rowH=bottomBox.height*0.37
	local col=5
	local colW=listSize.width/5
	local startY=listSize.height-rowH/2
	local startX=colW/2
	local mBgTecture=IMAGE("common/list_1088.png")
	local mLockTecture=IMAGE("common/list_1088_1.png")
	
	for k, v in pairs(pageTable) do
			local listItem = ScutCxListItem:itemWithColor(ccc3(24,24,24))
			listItem:setOpacity(0)
			local layer=CCLayer:create()	
			for m, n in pairs(v) do
				local index=(k-1)*pageSize+m
				local sparBgSprite=CCSprite:createWithTexture(mBgTecture)
				sparBgSprite:setAnchorPoint(PT(0.5,0.5))
				local posX=startX+((m-1)%col)*colW
				local posY=startY-math.floor((m-1)/col)*rowH
				sparBgSprite:setPosition(PT(posX,posY))
				layer:addChild(sparBgSprite,0)
				local fun=nil
				if not n.empty then
					local pImage = string.format("smallitem/%s.png",n.CrystalHeadID)
					local sparSprite=CCSprite:create(P(pImage))
					sparSprite:setAnchorPoint(PT(0.5,0.5))
					sparSprite:setPosition(PT(sparBgSprite:getContentSize().width/2,
											sparBgSprite:getContentSize().height/2))
					sparBgSprite:addChild(sparSprite,0)	
					local levelLabel=CCLabelTTF:create(string.format(Language.IDS_LVSTR,n.CrystalLv),FONT_NAME,FONT_SMM_SIZE)	
					levelLabel:setAnchorPoint(PT(1,0))
					levelLabel:setPosition(PT(sparSprite:getContentSize().width,0))
					sparSprite:addChild(levelLabel,0	)
				
					local actionBtn=UIHelper.createActionRect(sparBgSprite:getContentSize(),HuntingScene.choiceBottomSpar,index)
					actionBtn:setPosition(PT(0,0))
					sparBgSprite:addChild(actionBtn,0)
			
					local nameLabel=CCLabelTTF:create(n.CrystalName,FONT_NAME,FONT_SMM_SIZE)	
					nameLabel:setAnchorPoint(PT(0.5,1))
					local color = ZyColor:getCrystalColor(n.CrystalQuality)
					nameLabel:setColor(color)
					nameLabel:setPosition(PT(sparSprite:getPosition().x,0))
					sparBgSprite:addChild(nameLabel,0	)
					fun=HuntingScene.choiceBottomSpar	
				end
		
				if index>mServerData.CrystalPackNum then
					fun=HuntingScene.openBoxAction
					local lockSprite=CCSprite:createWithTexture(mLockTecture)
					lockSprite:setAnchorPoint(PT(0.5,0.5))
					lockSprite:setPosition(PT(sparBgSprite:getContentSize().width/2,
									sparBgSprite:getContentSize().height/2))
					sparBgSprite:addChild(lockSprite,0)		
				end
				if  fun then
					local actionBtn=UIHelper.createActionRect(sparBgSprite:getContentSize(),fun,index)
					actionBtn:setPosition(PT(0,0))
					sparBgSprite:addChild(actionBtn,0)
				end
			end	
			listItem:addChildItem(layer, layout)
			list:addListItem(listItem, false)
	end
end;


function  createSinglePage(info)
	local layer=CCLayer:create()

end;

function openBoxAction(node)
	local tag=node:getTag()
	if not isClick then
	isClick=true
	mOpenIndex=tag
	actionLayer.Action1310(mScene,nil,1,tag)
	end
end;


--是否确认扩容
function openBoxMakeSure(clickedButtonIndex,content,tag)
	if clickedButtonIndex == 1 then--确认
		actionLayer.Action1310(mScene,nil,2,mOpenIndex)
	end
end

--去猎命界面
function gotoHunting()
	if not isClick then
	isClick=true
	actionLayer.Action1303(mScene,nil)
	end
end;

--合并所有
function fusionAllAction()
	local box = ZyMessageBoxEx:new()
	box:doQuery(mScene, Language.TIP_TIP,Language.LIFECELL_COMBINEMSG,Language.IDS_SURE,Language.IDS_CANCEL,makeConfirm)
end;


function makeConfirm(clickedButtonIndex,content,tag)
	if clickedButtonIndex == 1 then--确认
	if not isClick then
		isClick=true
		actionLayer.Action1308(mScene,false,0,0,2)
	end
	end
end;


--选择底下的晶石
function choiceBottomSpar(node)
local tag=node:getTag()
if mServerData.RecordTabel3[tag] then
clickBottomIndex=tag
mShowSparInfo=mServerData.RecordTabel3[tag]
local UserCrystalID=mShowSparInfo.UserCrystalID
firshClickCryId=UserCrystalID
if not isClick then
	isClick =true
	actionLayer.Action1304(mScene,false,UserCrystalID,mToUserID)
end	
end

end;

--释放全部的层
function releaseContentLayer()
	releaseBottomLayer()
	releaseMidLayer()
	releaseDetailLayer()
	if mContentLayer then
		mContentLayer:getParent():removeChild(mContentLayer,true)
		mContentLayer=nil
	end
end;

--创建 所有的层 
function  createContentLayer()
	releaseContentLayer()
	local layer=CCLayer:create()
	mContentLayer=layer
	mLayer:addChild(layer,0)
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(0,0))
	--
	local closeBtn=ZyButton:new("button/list_1046.png")
	closeBtn:setAnchorPoint(PT(0,0))
	closeBtn:setPosition(PT(pWinSize.width-closeBtn:getContentSize().width,
						pWinSize.height-closeBtn:getContentSize().height))
	closeBtn:addto(layer,1)
	closeBtn:registerScriptHandler(closeScene)
	
	createTopLayer()
	refreshMidLayer(mServerData.RecordTabel2,mServerData.PicyturesID, mServerData.GeneralQuality)
	refreshBottomLayer()
	--	
end;
--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
end

-- 释放资源
function releaseResource()
	HuntSparLayer.releaseResource()
	mLayer=nil
	mScene=nil
	mContentLayer=nil
	mBottomLayer=nil
	mMidLayer=nil
	isClick=nil
	m_choiceImge=nil
	mChoiceIndex=nil
	m_listTable=nil
	mEquipLayer=nil
end

function  setGotoHunt(value)
	isGotoHunt=value
end;

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionID = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	--水晶列表 
	if actionID==1301 then
		local serverData=actionLayer._1301Callback(pScutScene, lpExternalData)
		if serverData then
			isGotoHunt=nil
			HuntSparLayer.closeScene()
			mServerData=serverData
			 createContentLayer()
		end
		--
	elseif  actionID==1302 then
		local serverData=actionLayer._1302Callback(pScutScene, lpExternalData)
		if serverData then
			mCurrentSparTable=serverData.RecordTabel
			refreshMidLayer(mCurrentSparTable,serverData.MaxHeadID,serverData.GeneralQuality)
		end
	elseif actionID==1304 then
		if isGotoHunt then
			HuntSparLayer._1304Callback(pScutScene, lpExternalData)
		else
			local serverData=actionLayer._1304Callback(pScutScene, lpExternalData)
			if serverData then
				 createDetailLayer(serverData)
			end
		end
	elseif actionID==1305  then
			HuntSparLayer._1305Callback(pScutScene, lpExternalData)	
	elseif actionID==1306  then
			HuntSparLayer._1306Callback(pScutScene, lpExternalData)	
	elseif actionID==1307  then
			HuntSparLayer._1307Callback(pScutScene, lpExternalData)	
	elseif actionID==1309 then
   		 if ZyReader:getResult() == eScutNetSuccess then
   		 	actionLayer.Action1302(pScutScene,false,mCurrentGeneral.GeneralID)
   		 	actionLayer.Action1311(pScutScene,nil)	
   		 	releaseEquipLayer()
		   	releaseChoiceLayer()
   		 	releaseDetailLayer()
   		 else
          		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)
   		 end
	elseif actionID==1308 then
   		 if ZyReader:getResult() == eScutNetSuccess then
          		ZyToast.show(pScutScene, Language.HUNT_FUSIONTIP) 	
   		 	clickBottomIndex=nil
   		 	isfusion=false	 	
   		 	releaseChoiceLayer()
   		 	actionLayer.Action1302(pScutScene,false,mCurrentGeneral.GeneralID)
			actionLayer.Action1311(mScene,nil)	 	
   		 else
          		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)
   		 end 
   		 --刷新未放入格子的水晶
   	elseif  actionID==1311 then
			local serverData=actionLayer._1311Callback(pScutScene, lpExternalData)
			if serverData then
				if userData==1 then
					createEquipLayer(serverData.RecordTabel)
				else
					mServerData.CrystalPackNum=serverData.CrystalPackNum
					mServerData.RecordTabel3=serverData.RecordTabel
					refreshBottomLayer()
				end
			end  
		--获取命运水晶列表接口  前往猎命界面
   	elseif actionID==1303 then
		local serverData=actionLayer._1303Callback(pScutScene, lpExternalData)
		if serverData then
		--	releaseContentLayer()
			
			HuntSparLayer.closeScene()	
			isGotoHunt=true
			HuntSparLayer.setServerData(serverData)	
			HuntSparLayer.init(pScutScene)
		end
		--背包扩容
	elseif actionID == 1310 then
		local type = ZyReader:getResult()
		if type == 1 then
			local box = ZyMessageBoxEx:new()
			box:doQuery(mLayer,nil,ZyReader:readErrorMsg(),Language.IDS_SURE,Language.IDS_CANCEL,openBoxMakeSure)
		elseif type ==0 or type == 2 then
			mOpenIndex=nil
			actionLayer.Action1311(pScutScene,nil)--刷新背包列表
		else
      			ZyToast.show(pScutScene, ZyReader:readErrorMsg())
		end
		
	end
	isClick=false
	
	commonCallback.networkCallback(pScutScene, lpExternalData)
end





