------------------------------------------------------------------
-- BattleResult.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 战斗后界面  （副本评价，转跳判断）
------------------------------------------------------------------

module("BattleResult", package.seeall)


mScene = nil 		-- 场景
local mExpBg=nil
local long=nil
local type=nil

-- 场景入口
function init()
	initResource()
	createScene()
	
end

-- 退出场景
function popScene()
	releaseResource()
	proSprite=nil
	PlotListScene.init(PlotListScene.getPlotType())
end


function setData(PlotID,roundNum)
_plotID = PlotID
_roundNum = roundNum
end;

function getBtn()
	return closeBtn
end;

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
	isGuide=nil
end

-- 释放资源
function releaseResource()
	AddSpriteLayer.releaseResource()
	rewardLayer =nil
	closeBtn=nil
	mDetailLayer=nil
	isGuide=nil
	mExpBg=nil
end

-- 创建场景
function createScene()
	local scene = ScutScene:new()

	scene:registerCallback(networkCallback)
	mScene = scene.root;
--	mScene:registerOnExit("BattleResult.releaseResource")	
	SlideInLReplaceScene(mScene,1)
	
	-- 添加主层
	mLayer= CCLayer:create()
	mScene:addChild(mLayer, 0)


	--创建背景
--	local bgLayer = UIHelper.createUIBg(nil, nil, nil,"BattleResult.popScene")
--	mLayer:addChild(bgLayer,0)

	local bgLayer = creatBg(tabStr)
	mLayer:addChild(bgLayer, 0)	


	-- 此处添加场景初始内容
	
	if PlotListScene.getPlotType() ==1 then
		--再来一次
		local topBtn=ZyButton:new("button/list_1023.png",nil,nil,Language.REWARD_AGAIN)
		topBtn:registerScriptHandler(againAction)
		topBtn:setAnchorPoint(PT(0,0))
		topBtn:setPosition(PT(pWinSize.width*0.45-topBtn:getContentSize().width, pWinSize.height*0.2))	
		topBtn:addto(mLayer, 0)	
	end;
	
	--关闭
	local topBtn=ZyButton:new("button/list_1023.png",nil,nil,Language.IDS_COLSE)
	topBtn:registerScriptHandler(popScene)
	topBtn:setAnchorPoint(PT(0,0))
	topBtn:setPosition(PT(pWinSize.width*0.55, pWinSize.height*0.2))	
	topBtn:addto(mLayer, 0)

	closeBtn = topBtn
	
	if GuideLayer.judgeIsGuide(3) then
		isGuide = true
	end
	

	MainMenuLayer.init(1, mScene)	
	MainMenuLayer.refreshWin()
	
	sendAction(4003)


	
end

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
	titleSprite:setAnchorPoint(PT(0.5,0.5))
	titleSprite:setPosition(PT(pos_x+boxSize.width*0.5,pos_y+boxSize.height*0.915))
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
	local label = CCLabelTTF:create(rewardInfo.PlotName, FONT_NAME, titleSize)
	label:setAnchorPoint(PT(0.5,0.5))
	label:setPosition(PT(pWinSize.width/2, height))
	layer:addChild(label, 0)
	--星级图
	if rewardInfo.StarScore==1 then
		image="mainUI/list_3043.png"
	elseif rewardInfo.StarScore==2 then
		image="mainUI/list_3044.png"
	elseif rewardInfo.StarScore==3 then
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
	layer:addChild(label1, 0)
	local label = CCLabelTTF:create(rewardInfo.PennyNum, FONT_NAME, titleSize)
	label:setAnchorPoint(PT(0,0))
	label:setPosition(PT(starImg:getPosition().x+starImg:getContentSize().width/2-label:getContentSize().width-SX(20), label1:getPosition().y))
	layer:addChild(label, 0)
	--获得阅历
	local label1 = CCLabelTTF:create(Language.HUODEYUELI, FONT_NAME, titleSize)
	label1:setAnchorPoint(PT(0,0))
	label1:setPosition(PT(starImg:getPosition().x-starImg:getContentSize().width/2+SX(20),label:getPosition().y-label1:getContentSize().height*1.2))
	layer:addChild(label1, 0)
	local label = CCLabelTTF:create(rewardInfo.YueLiNum, FONT_NAME, titleSize)
	label:setAnchorPoint(PT(0,0))
	label:setPosition(PT(starImg:getPosition().x+starImg:getContentSize().width/2-label:getContentSize().width-SX(20), label1:getPosition().y))
	layer:addChild(label, 0)
	--获得荣誉值
	local label1 = CCLabelTTF:create(Language.HUODERYZ, FONT_NAME, titleSize)
	label1:setAnchorPoint(PT(0,0))
	label1:setPosition(PT(starImg:getPosition().x-starImg:getContentSize().width/2+SX(20),label:getPosition().y-label1:getContentSize().height*1.2))
	layer:addChild(label1, 0)
	local label = CCLabelTTF:create(rewardInfo.HonourNum, FONT_NAME, titleSize)
	label:setAnchorPoint(PT(0,0))
	label:setPosition(PT(starImg:getPosition().x+starImg:getContentSize().width/2-label:getContentSize().width-SX(20), label1:getPosition().y))
	layer:addChild(label, 0)
	--荣誉进度条
	xuetiao=CCSprite:create(P("mainUI/list_3049_1.png"));
	xuetiao:setAnchorPoint(PT(0,0))
	xuetiao:setPosition(PT((pWinSize.width-xuetiao:getContentSize().width)/2,label:getPosition().y-label:getContentSize().height*2))
	layer:addChild(xuetiao,0)
	mExpBg = xuetiao
	
	type=1
	if rewardInfo.IsUpgrade==1 then
		--当前经验
		local dangqian=((rewardInfo.LastMaxHonourNum-rewardInfo.HonourNum+rewardInfo.CurrentHonour)/rewardInfo.LastMaxHonourNum)*xuetiao:getContentSize().width*0.9
		ExpBar = CCSprite:create(P("mainUI/list_3049_2.9.png"));
		ExpBar:setAnchorPoint(PT(0,0))
		if dangqian/ExpBar:getContentSize().width<0.5 then
			ExpBar:setScaleX(pWinSize.width*0.05/ExpBar:getContentSize().width)
			long=pWinSize.width*0.05/ExpBar:getContentSize().width
		else
			ExpBar:setScaleX(dangqian/ExpBar:getContentSize().width)
			long=dangqian/ExpBar:getContentSize().width
		end
--		long=dangqian/ExpBar:getContentSize().width
		ExpBar:setPosition(PT(xuetiao:getPosition().x+xuetiao:getContentSize().width*0.04, xuetiao:getPosition().y+SY(1)))
	--	ExpBar:setScaleY(ExpBg:getContentSize().height*0.5/ExpBar:getContentSize().height)
		layer:addChild(ExpBar, 0)
	else
		--当前经验
		local dangqian=((rewardInfo.CurrentHonour-rewardInfo.HonourNum)/rewardInfo.MaxHonourNum)*xuetiao:getContentSize().width*0.9
		ExpBar = CCSprite:create(P("mainUI/list_3049_2.9.png"));
		ExpBar:setAnchorPoint(PT(0,0))
		if dangqian/ExpBar:getContentSize().width<0.5 then
			ExpBar:setScaleX(pWinSize.width*0.05/ExpBar:getContentSize().width)
			long=pWinSize.width*0.05/ExpBar:getContentSize().width
		else
			ExpBar:setScaleX(dangqian/ExpBar:getContentSize().width)
			long=dangqian/ExpBar:getContentSize().width
		end
--		ExpBar:setScaleX(dangqian/ExpBar:getContentSize().width)
--		long=dangqian/ExpBar:getContentSize().width
		ExpBar:setPosition(PT(xuetiao:getPosition().x+xuetiao:getContentSize().width*0.04, xuetiao:getPosition().y+SY(1)))
	--	ExpBar:setScaleY(ExpBg:getContentSize().height*0.5/ExpBar:getContentSize().height)
		layer:addChild(ExpBar, 0)
	end
	--[[ 
	local SkillExp=rewardInfo.CurrentHonour
	local iSkillExp=rewardInfo.MaxHonourNum
	local HpWidth = pWinSize.width*0.78
	local HpImage = "mainUI/list_3049_2.9.png"
	pos = PT(xuetiao:getPosition().x-xuetiao:getContentSize().width/2+SX(17) ,xuetiao:getPosition().y+SY(1))
	createBar(HpWidth, HpImage, SkillExp, iSkillExp,pos, layer)
	--]]
	mIndex = 0
	
	scaleTo(ExpBar)
--	scaleTo(proBar)
	
--	updateProSprite()
	--物品
--	local pos_y = nil
	for k,v in ipairs(rewardInfo.RecordTabel2) do
		local name = v.Name 
		local image = string.format("smallitem/%s.png",v.HeadID)
		local num = v.Num
		local quality = v.	QualityType
		local item = creatItem(image, name, num, quality)	
		item:setAnchorPoint(PT(0,0))
		local pos_x = pWinSize.width*0.1+item:getContentSize().width*1.2*(k-1)
--		if pos_y == nil then
--			pos_y = height-item:getContentSize().height
--			height = pos_y
--		end
		item:setPosition(PT(pos_x,xuetiao:getPosition().y-pWinSize.height*0.12 ))
		layer:addChild(item, 0)
	end
	if rewardInfo.IsUpgrade~=1 then
		startGuide()
	end
end

function scaleTo(pNode)
	mIndex = mIndex+1
	
	if rewardInfo.IsUpgrade == 1 then
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
			baifen=(rewardInfo.HonourNum-rewardInfo.CurrentHonour)/rewardInfo.LastMaxHonourNum
		elseif type==2 then
			baifen=rewardInfo.CurrentHonour/rewardInfo.MaxHonourNum
		end
		local sacle=xuetiao:getContentSize().width*0.9*baifen/5*mIndex/pNode:getContentSize().width
		if sacle<0.5 then
		    if rewardInfo.IsUpgrade == 1 then
				long=nil
				type=2
				mIndex=1
                		pNode:setScaleX(pWinSize.width*0.05/pNode:getContentSize().width)
                		return
		    else
		        	return
		    end
		end
		if long==nil then
		    pNode:setScaleX(sacle)
		else
		    pNode:setScaleX(long+sacle)	
		end
		local action2 = CCCallFuncN:create(BattleResult.scaleTo)	
		local action1 = CCDelayTime:create(0.2)
		local action = CCSequence:createWithTwoActions(action1, action2)
		if mExpBg then
			pNode:runAction(action)
		end
	else
		if mIndex > 5 then
			
			return
		end
		local baifen=rewardInfo.HonourNum/rewardInfo.MaxHonourNum
		local sacle=xuetiao:getContentSize().width*0.9*baifen/5*mIndex/pNode:getContentSize().width
		if sacle<0.5 then
		    return
		end
		pNode:setScaleX(long+sacle)	
		local action2 = CCCallFuncN:create(BattleResult.scaleTo)	
		local action1 = CCDelayTime:create(0.2)
		local action = CCSequence:createWithTwoActions(action1, action2)
		if mExpBg then
			pNode:runAction(action)
		end
	end
	
end

function  createBar(width,picPath,currentValue,maxValue,pos,father)
    proBar = CCSprite:create(P(picPath))
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
	if menberCallBack then 
	    menuItem:registerScriptHandler(menberCallBack)
	end 
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
		name = judgeNameLen(name)
	
		local nameLabel = CCLabelTTF:create(name, FONT_NAME, FONT_FMM_SIZE)
		nameLabel:setAnchorPoint(PT(0.5,0))
		nameLabel:setPosition(PT(menuItem:getContentSize().width*0.5, menuItem:getContentSize().height*0.5-menuItem:getContentSize().height/2-nameLabel:getContentSize().height*1.2))
		menuItem:addChild(nameLabel, 0)	
	end
	
	return btn
end;

function judgeNameLen(str)
	local len = utfstrlen(str)
	local content = ""
	if len > 5 then
		content = utfstrIndex(str,1)..utfstrCutOut(str, len-1, len)
	else
		content = str
	end
	return content
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

--再来一次  按钮响应
function againAction()
	sendAction(4002)
end

-----------------------------------------------------
function sendAction(actionId)
	if actionId == 4002 then
		actionLayer.Action4002(mScene, nil , _plotID, 1)
	elseif actionId == 4003 then	
		actionLayer.Action4003(mScene,nil,_plotID)
	end
end;

---延迟进行方法
function delayExec(funName,nDuration)
    local  action = CCSequence:createWithTwoActions(
    CCDelayTime:create(nDuration),
    CCCallFunc:create(funName));
    mLayer:runAction(action)
end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if actionId == 4002 then
		_PlotID = userData
    		if ZyReader:getResult() == 1  then--返回值1：提示挑战次数达到是否XX晶石开启当前挑战。2：提示晶石不足返回充值页面
		 	local box = ZyMessageBoxEx:new()
			box:doQuery(pScutScene,nil,ZyReader:readErrorMsg(),Language.IDS_SURE,Language.IDS_CANCEL,adkUseGold)			
		elseif ZyReader:getResult() == 2  then
		 	local box = ZyMessageBoxEx:new()
			box:doQuery(pScutScene,nil, Language.BAG_TIP3,Language.IDS_SURE,Language.IDS_CANCEL,gotoTopUp)  
		elseif ZyReader:getResult() == 3  then
			--精力不足
			actionLayer.Action1091(pScutScene,false)
    		else
			local serverInfo=actionLayer._4002Callback(pScutScene, lpExternalData)
			if serverInfo~=nil then
				local PlotID = userData
				battleScene.setPlotMapInfo(serverInfo, PlotID)
				releaseResource()
				battleScene.pushScene()
			else
				ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
			end
    		end
	elseif actionId == 4003 then
		local serverInfo = actionLayer._4003Callback(pScutScene, lpExternalData)
		if serverInfo ~= nil then
			rewardInfo = serverInfo

--[[
		local RecordNums_2=1
		local RecordTabel_2={}
		if RecordNums_2~=0 then
			for k=1,RecordNums_2 do
				local mRecordTabel_2={}
				ZyReader:recordBegin()
				mRecordTabel_2.Name= "dsafgah"
				mRecordTabel_2.HeadID= "icon_8005"
				mRecordTabel_2.Num= 1
				mRecordTabel_2.ItemID = "fsddddddd"
				mRecordTabel_2.MaxHeadID =  "Icon_2000_1"
				mRecordTabel_2.ItemDesc =  "afdssddddddddddddddddddddddddddddddddddddddddddddddddddddddddd"
			mRecordTabel_2.QualityType = 3
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_2,mRecordTabel_2)
			end
		end
		rewardInfo.RecordTabel2 = RecordTabel_2;
		--]]
			
			showReward() 	 
		end
	elseif actionId == 1091 then
		local serverInfo=actionLayer._1091Callback(pScutScene, lpExternalData)
		if serverInfo then
			AddSpriteLayer.setInfo(pScutScene)
			AddSpriteLayer.createEnergyLayer(serverInfo)
		end
	elseif actionId == 1010 then
		AddSpriteLayer.networkCallback(pScutScene, lpExternalData)
	else
		commonCallback.networkCallback(pScutScene, lpExternalData)
	end	
end



--是否使用晶石进行副本
function  adkUseGold(clickedButtonIndex,content,tag) 
	if clickedButtonIndex==ID_MBOK then
		actionLayer.Action4002(mScene, nil , _PlotID, 1)	
	end
end;

function gotoTopUp(clickedButtonIndex, content, tag)
	if clickedButtonIndex ==ID_MBOK then
		TopUpScene.init()
	end
end

function startGuide()
	if mLayer and GuideLayer.judgeIsGuide(3) then
		isGuide = true
		GuideLayer.setScene(mScene)	
		GuideLayer.init()
		if MainMenuLayer.getIsGotoLv() then
			GuideLayer.hide()
		end
	else
		isGuide = nil
	end
end;
