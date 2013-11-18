
-- RBattleResult.lua.lua
-- Author     :Lysong
-- Version    : 1.0.0.0
-- Date       :
-- Description:
------------------------------------------------------------------

module("RBattleResult", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

mScene = nil 		-- 场景

local mExpBg=nil
local long=nil
local type=nil
local list1={}

-- 场景入口
function init()
	initResource()
	createScene()
	
end

-- 退出场景
function popScene()
	releaseResource()
end


--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()

end

-- 释放资源
function releaseResource()
	rewardLayer =nil
	
end

-- 创建场景
function createScene(info)
	local scene = ScutScene:new()

	scene:registerCallback(networkCallback)
    mScene = scene.root 
	
	mScene:registerScriptHandler(SpriteEase_onEnterOrExit)
	
	SlideInLReplaceScene(mScene,1)
	
	list1=info
	
	-- 添加主层
	mLayer= CCLayer:create()
	mScene:addChild(mLayer, 1)
	
	score=Score
	starnum=StarNum

	--创建背景
--	local bgLayer = UIHelper.createUIBg(nil, nil, nil,"BattleResult.popScene")
--	mLayer:addChild(bgLayer,0)

	local bgLayer = creatBg(tabStr)
	mLayer:addChild(bgLayer, 0)	


	-- 此处添加场景初始内容
	--[[
	--再来一次
	local topBtn=ZyButton:new("button/list_1023.png",nil,nil,Language.REWARD_AGAIN)
	topBtn:registerScriptHandler(againAction)
	topBtn:setAnchorPoint(PT(0,0))
	topBtn:setPosition(PT(pWinSize.width*0.45-topBtn:getContentSize().width, pWinSize.height*0.225))	
	topBtn:addto(mLayer, 0)	
	--]]
	--关闭
	local topBtn=ZyButton:new("button/list_1023.png",nil,nil,Language.IDS_COLSE)
	topBtn:registerScriptHandler(pass)
	topBtn:setAnchorPoint(PT(0,0))
	topBtn:setPosition(PT((pWinSize.width-topBtn:getContentSize().width)/2, pWinSize.height*0.2))
--	topBtn:setPosition(PT(pWinSize.width*0.55, pWinSize.height*0.225))
	topBtn:addto(mLayer, 0)
	

	MainMenuLayer.init(1, mScene)	
	MainMenuLayer.refreshWin()
	
	showReward()
--	actionLayer.Action4406(mScene, nil,1,Score,StarNum)

end


function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        releaseResource()
    end
end


function pass()
	actionLayer.Action12057(mScene,nil)
end;

--创建背景
function creatBg(tabStr)
	local layer = CCLayer:create()
	
	local boxSize = SZ(pWinSize.width, pWinSize.height*0.7)
	local pos_x = 0
	local pos_y = pWinSize.height*0.145
	
	local bgSprite=CCSprite:create(P("common/list_3048.png"))
	bgSprite:setAnchorPoint(PT(0,0))
	bgSprite:setPosition(PT(pos_x,pos_y))
	bgSprite:setScaleX(boxSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(boxSize.height/bgSprite:getContentSize().height)
	layer:addChild(bgSprite,0)
	
	--标题
	local titleSprite=CCSprite:create(P("title/list_1098.png"))
	titleSprite:setAnchorPoint(PT(0.5,0))
	titleSprite:setPosition(PT(pos_x+boxSize.width*0.5,pos_y+boxSize.height*0.9-titleSprite:getContentSize().height*1.6))
	layer:addChild(titleSprite,0)
	

	layer:setContentSize(boxSize)

	
	return layer
end;

--显示奖励
function showReward()

	local layer = CCLayer:create()
	mLayer:addChild(layer, 0)
	
	rewardLayer = layer	
	

	local titleSize = FONT_DEF_SIZE
	
	local startX = pWinSize.width*0.05
	local height = pWinSize.height*0.72
	
	--副本名称
	local label = CCLabelTTF:create(list1.PlotName, FONT_NAME, titleSize)
	label:setAnchorPoint(PT(0.5,0))
	label:setPosition(PT(pWinSize.width/2, height))
	layer:addChild(label, 0)
	--星级图
	if list1.StarNum==1 then
		image="mainUI/list_3043.png"
	elseif list1.StarNum==2 then
		image="mainUI/list_3044.png"
	elseif list1.StarNum==3 then
		image="mainUI/list_3045.png"
	end
	local starImg=CCSprite:create(P(image))
	starImg:setAnchorPoint(PT(0.5,0))
	starImg:setPosition(PT(pWinSize.width/2,label:getPosition().y-starImg:getContentSize().height*1.1))
	layer:addChild(starImg,0)	
	--获得金币
	local label1 = CCLabelTTF:create(Language.HUODEJINBI, FONT_NAME, titleSize)
	label1:setAnchorPoint(PT(0,0))
	label1:setPosition(PT(starImg:getPosition().x-starImg:getContentSize().width/2+SX(20), starImg:getPosition().y-label1:getContentSize().height*1.2))
	if list1.GameCoin~=0 then
		layer:addChild(label1, 0)
	end
	local label = CCLabelTTF:create(list1.GameCoin, FONT_NAME, titleSize)
	label:setAnchorPoint(PT(0,0))
	label:setPosition(PT(starImg:getPosition().x+starImg:getContentSize().width/2-label:getContentSize().width-SX(20), label1:getPosition().y))
	if list1.GameCoin~=0 then
		layer:addChild(label, 0)
	end
	--获得荣誉值
	local label1 = CCLabelTTF:create(Language.HUODERYZ, FONT_NAME, titleSize)
	label1:setAnchorPoint(PT(0,0))
	label1:setPosition(PT(starImg:getPosition().x-starImg:getContentSize().width/2+SX(20),label:getPosition().y-label1:getContentSize().height*1.2))
	layer:addChild(label1, 0)
	local label = CCLabelTTF:create(list1.HonourNum, FONT_NAME, titleSize)
	label:setAnchorPoint(PT(0,0))
	label:setPosition(PT(starImg:getPosition().x+starImg:getContentSize().width/2-label:getContentSize().width-SX(20), label1:getPosition().y))
	layer:addChild(label, 0)
	
	if list1.Gold and list1.Gold > 0 then
		--获得晶石
		local label1 = CCLabelTTF:create(Language.HUODEJINGSHI, FONT_NAME, titleSize)
		label1:setAnchorPoint(PT(0,0))
		label1:setPosition(PT(starImg:getPosition().x-starImg:getContentSize().width/2+SX(20),label:getPosition().y-label1:getContentSize().height*1.2))
		layer:addChild(label1, 0)
		label = CCLabelTTF:create(list1.Gold, FONT_NAME, titleSize)
		label:setAnchorPoint(PT(0,0))
		label:setPosition(PT(starImg:getPosition().x+starImg:getContentSize().width/2-label:getContentSize().width-SX(20), label1:getPosition().y))
		layer:addChild(label, 0)
	end
	
	--荣誉进度条
	xuetiao=CCSprite:create(P("mainUI/list_3049_1.png"));
	xuetiao:setAnchorPoint(PT(0,0))
	xuetiao:setPosition(PT((pWinSize.width-xuetiao:getContentSize().width)/2,label:getPosition().y-label:getContentSize().height*2))
	layer:addChild(xuetiao,0)
	
	mExpBg = xuetiao
	
	type=1
	if list1.IsUpgrade==1 then
		--当前经验
		local dangqian=((list1.LastMaxHonourNum-list1.HonourNum+list1.CurrentHonour)/list1.LastMaxHonourNum)*xuetiao:getContentSize().width*0.93
		ExpBar = CCSprite:create(P("mainUI/list_3049_2.9.png"));
		ExpBar:setAnchorPoint(PT(0,0))
		if dangqian/ExpBar:getContentSize().width<0.5 then
			ExpBar:setScaleX(pWinSize.width*0.05/ExpBar:getContentSize().width)
		else
			ExpBar:setScaleX(dangqian/ExpBar:getContentSize().width)
		end
--		ExpBar:setScaleX(dangqian/ExpBar:getContentSize().width)
		long=dangqian/ExpBar:getContentSize().width
		ExpBar:setPosition(PT(xuetiao:getPosition().x+xuetiao:getContentSize().width*0.04, xuetiao:getPosition().y+SY(1)))
	--	ExpBar:setScaleY(ExpBg:getContentSize().height*0.5/ExpBar:getContentSize().height)
		layer:addChild(ExpBar, 0)
	else
		--当前经验
		local dangqian=((list1.CurrentHonour-list1.HonourNum)/list1.MaxHonourNum)*xuetiao:getContentSize().width*0.93
		ExpBar = CCSprite:create(P("mainUI/list_3049_2.9.png"));
		ExpBar:setAnchorPoint(PT(0,0))
		if dangqian/ExpBar:getContentSize().width<0.5 then
			ExpBar:setScaleX(pWinSize.width*0.05/ExpBar:getContentSize().width)
		else
			ExpBar:setScaleX(dangqian/ExpBar:getContentSize().width)
		end
--		ExpBar:setScaleX(dangqian/ExpBar:getContentSize().width)
		long=dangqian/ExpBar:getContentSize().width
		ExpBar:setPosition(PT(xuetiao:getPosition().x+xuetiao:getContentSize().width*0.04, xuetiao:getPosition().y+SY(1)))
	--	ExpBar:setScaleY(ExpBg:getContentSize().height*0.5/ExpBar:getContentSize().height)
		layer:addChild(ExpBar, 0)
	end
	--[[
	local SkillExp=list1.CurrentHonour
	local iSkillExp=list1.MaxHonourNum
	local HpWidth = pWinSize.width*0.78
	local HpImage = "mainUI/list_3049_2.9.png"
	pos = PT(xuetiao:getPosition().x-xuetiao:getContentSize().width/2+SX(17) ,xuetiao:getPosition().y+SY(1))
	createBar(HpWidth, HpImage, SkillExp, iSkillExp,pos, layer)
	--]]
	mIndex = 0
	
	scaleTo(ExpBar)
	--培养丹
	if list1.AgentNum and list1.AgentNum  > 0 then	
		local name = Language.PEIYANGDAN
		local image = "smallitem/icon_4094.png"
		local num = list1.AgentNum
		local quality = 1
		local item = creatItem(image, name, num, quality)	
		item:setAnchorPoint(PT(0,0))
		local pos_x = pWinSize.width*0.1
		item:setPosition(PT(pos_x,xuetiao:getPosition().y-pWinSize.height*0.12 ))
		layer:addChild(item, 0)
	end
	
	--物品
	local pos_y = nil
	if list1.RecordTabel1==nil then
	    list1.RecordTabel1={}
	end
	for k,v in ipairs(list1.RecordTabel) do
		local name = v.RewardInfo 
		local image = string.format("smallitem/%s.png",v.Picture)
		local num = v.Num
		local quality = 1
		local item = creatItem(image, name, num, quality)	
		item:setAnchorPoint(PT(0,0))
		if list1.AgentNum and list1.AgentNum  > 0 then
			pos_x = pWinSize.width*0.1+item:getContentSize().width*1.2*(k)
		else
			pos_x = pWinSize.width*0.1+item:getContentSize().width*1.2*(k-1)
		end
		item:setPosition(PT(pos_x,xuetiao:getPosition().y-pWinSize.height*0.12 ))
		layer:addChild(item, 0)
	end
	
	--晶石
--	local pos_x = pWinSize.width*0.1
--	local pos_y = nil
--	if list1.Gold and list1.Gold > 0 then
--		local item,itemSize = imageMoney("mainUI/list_1006.png", list1.Gold)
--		if not pos_y then
--			pos_y = height-itemSize.height*1.5
--			height = pos_y
--		end
--		item:setPosition(PT(pos_x+item:getContentSize().width*1.2, xuetiao:getPosition().y-pWinSize.height*0.12 ))
--		layer:addChild(item, 0)
--	end
	
end

function scaleTo(pNode)
	mIndex = mIndex+1
	
	if list1.IsUpgrade == 1 then
		if mIndex > 5 then
			if  type==1 then
				long=nil
				type=2
				mIndex=1
				pNode:setScaleX(pWinSize.width*0.05/pNode:getContentSize().width)
			elseif type==2 then
				
				return
			end
		end
		if type==1 then
			baifen=(list1.HonourNum-list1.CurrentHonour)/list1.LastMaxHonourNum
		elseif type==2 then
			baifen=list1.CurrentHonour/list1.MaxHonourNum
		end
		local sacle=xuetiao:getContentSize().width*0.93*baifen/5*mIndex/pNode:getContentSize().width
		if sacle<0.5 then
		    if list1.IsUpgrade == 1 then
		        long=nil
				type=2
				mIndex=1
                pNode:setScaleX(pWinSize.width*0.05/pNode:getContentSize().width)
		    else
		        return
		    end
		end
		if long==nil then
		    pNode:setScaleX(sacle)
		else
		    pNode:setScaleX(long+sacle)	
		end
		local action2 = CCCallFuncN:create(RBattleResult.scaleTo)	
		local action1 = CCDelayTime:create(0.2)
		local action = CCSequence:createWithTwoActions(action1, action2)
		if mExpBg then
			pNode:runAction(action)
		end
	else
		if mIndex > 5 then
			
			return
		end
		local baifen=list1.HonourNum/list1.MaxHonourNum
		local sacle=xuetiao:getContentSize().width*0.93*baifen/5*mIndex/pNode:getContentSize().width
		if sacle<0.5 then
		    return
		end
		pNode:setScaleX(long+sacle)	
		local action2 = CCCallFuncN:create(RBattleResult.scaleTo)	
		local action1 = CCDelayTime:create(0.2)
		local action = CCSequence:createWithTwoActions(action1, action2)
		if mExpBg then
			pNode:runAction(action)
		end
	end
	
end

function  createBar(width,picPath,currentValue,maxValue,pos,father)
    local  proBar = CCSprite:create(P(picPath))
	if not maxValue or maxValue < 0 then
		maxValue=100
	end
	if not currentValue then
		currentValue = 0
	end
	
	local scale=currentValue/maxValue 
	if scale>1 then
		scale=1
	end
	draw_w=width*scale  --实际绘制长度
	
	if draw_w< proBar:getContentSize().width then
		draw_w= proBar:getContentSize().width
	end
	proBar:setScaleX(draw_w/proBar:getContentSize().width)
	proBar:setAnchorPoint(PT(0,0))
	proBar:setPosition(PT(pos.x,pos.y))
	father:addChild(proBar,1)
	if currentValue==0 then
		proBar:setVisible(false)
	end
	
	return proBar
end;

--图片做单位  + 价格
function imageMoney(image, num)

	local layer = CCLayer:create()
		local sprite = CCSprite:create(P(image));
		sprite:setAnchorPoint(PT(0,0))
		sprite:setPosition(PT(0,0))
		layer:addChild(sprite, 0)

		local numLabel = CCLabelTTF:create("+"..num, FONT_NAME, FONT_SM_SIZE)
		numLabel:setAnchorPoint(PT(0,0))
		numLabel:setPosition(PT(sprite:getPosition().x+sprite:getContentSize().width*1.25, (sprite:getContentSize().height-numLabel:getContentSize().height)*0.5))
		layer:addChild(numLabel, 0)


		local itemSize = SZ(numLabel:getPosition().x+numLabel:getContentSize().width, sprite:getContentSize().height)

	return layer,itemSize
end

--获得物品图标
function creatItem(image, name, num, quality)
	local imageBg = getQualityBg(quality, 1)
	local menuItem = CCMenuItemImage:create(P(imageBg), P(imageBg))
	local btn = CCMenu:createWithItem(menuItem)
	
	menuItem:setAnchorPoint(PT(0,0))
	menuItem:registerScriptHandler(menberCallBack)
	if tag then
		menuItem:setTag(tag)
	end
	btn:setContentSize(SZ(menuItem:getContentSize().width, menuItem:getContentSize().height))
	
	--佣兵头像
	if image then
		local imageLabel = CCMenuItemImage:create(P(image),P(image))
		if imageLabel == nil then
			 return btn 
		end
		imageLabel:setAnchorPoint(CCPoint(0.5, 0.5))
		imageLabel:setPosition(PT(menuItem:getContentSize().width*0.5,menuItem:getContentSize().height*0.5))
		menuItem:addChild(imageLabel,0)
	end
	if num then
		local numLabel=CCLabelTTF:create("x"..num,FONT_NAME,FONT_FMM_SIZE)
		numLabel:setAnchorPoint(PT(0.5,0))
		numLabel:setPosition(PT(menuItem:getContentSize().width*0.5, menuItem:getContentSize().height*0.05))
		menuItem:addChild(numLabel,0)
	end
	if name then
		local nameLabel = CCLabelTTF:create(name, FONT_NAME, FONT_FMM_SIZE)
		nameLabel:setAnchorPoint(PT(0.5,0))
		nameLabel:setPosition(PT(menuItem:getContentSize().width*0.5, menuItem:getContentSize().height*0.5-menuItem:getContentSize().height/2-nameLabel:getContentSize().height*1.2))
		menuItem:addChild(nameLabel, 0)	
	end
	
	return btn
end;

--创建文字
function creatLabel(fitstName, secondName)
	local layer = CCLayer:create()
	
	if fitstName == nil then
		fitstName = ""
	end
	local firstLabel=CCLabelTTF:create(fitstName..":",FONT_NAME,FONT_SM_SIZE)
	firstLabel:setAnchorPoint(PT(0,0))
	firstLabel:setPosition(PT(0, 0))
	layer:addChild(firstLabel,0)
	
	if secondName == nil then
		secondName = ""
	end
	local secondLabel=CCLabelTTF:create(secondName,FONT_NAME,FONT_SM_SIZE)
	secondLabel:setAnchorPoint(PT(0,0))
	secondLabel:setPosition(PT(firstLabel:getPosition().x+firstLabel:getContentSize().width, firstLabel:getPosition().y))
	layer:addChild(secondLabel,0)	
	local color = ZyColor:colorGreen()
	if color~=nil then
		secondLabel:setColor(color)
	end
	
	local layerSize = SZ(secondLabel:getPosition().x+secondLabel:getContentSize().width, firstLabel:getContentSize().height)
	
	return layer,layerSize
end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	
	if actionId==12057 then
		local serverInfo=actionLayer._12057Callback(pScutScene, lpExternalData)
 		if serverInfo~=nil then
			mRecordTabel=serverInfo.RecordTabel
		end
		popScene()
		RelicArchaeology.init(mRecordTabel)
	else
		commonCallback.networkCallback(pScutScene, lpExternalData)
	end
	
end