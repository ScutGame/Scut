------------------------------------------------------------------
-- MainMenuLayer.lua
-- Author     : Zonglin Liu
-- Version    :
-- Date       :
-- Description:
------------------------------------------------------------------

module("MainMenuLayer", package.seeall)

require("scenes.ChangPassword")--修改密码界面
require("scenes.VIPScene")--vip界面
require("layers.BasicInfoLayer")--基本信息界面
require("scenes.MagicScene")--魔术界面
require("scenes.PlotListScene")--副本界面
require("scenes.GameSettingScene")--游戏设置
require("scenes.HeroScene")--佣兵界面
require("scenes.ChatScene")--聊天
require("scenes.CompetitiveScene")--竞技场
require("scenes.BagScene")--背包
require("scenes.MailScene")--信件
require("scenes.CollectionScene")--集邮
require("scenes.FirendScene")--好友
require("scenes.WeaponScene")--装备
require("scenes.HuntingScene")--猎命
require("scenes.BossScene")--boss战斗
require("scenes.SoulSkillScene")--魂技
require("scenes.businessStore")--商城
require("scenes.RoleBagScene")--佣兵背包
require("scenes.EmbattleScene")--布阵界面
require("layers.BroadcastLayer")--广播栏
require("layers.ActiveBarLayer")--活动条
require("layers.GMCtrl")--活动条
require("scenes.BoxToScene")
require("scenes.TodayToScene") ------每日探险
require("scenes.PrayScene") ------------ 祈祷界面
require("scenes.ManorScene")
require("scenes.SoulSkillList")--魂技列表
require("scenes.HeroLvUp")--佣兵升级界面
require("scenes.sendToScene")----送精力界面
require("scenes.PublicAnnouncementScene") ------公告系统
require("scenes.RecruitScene") ---- 招募送好礼
require("scenes.BSDiscountScene")
require("scenes.DoubleIncome")
require("layers.LvUpNoticeLayer")
require("scenes.StampScenes")  -----集邮界面
require("scenes.LetterScenes") ---- 信件系统
require("scenes.ActiveAllScene")---
require("scenes.WorldBossToScene") --- 世界boss
require("scenes.TopUpRebate")--- 充值返利
require("scenes.UpGradeScene") ---- 升级有惊喜
require("scenes.TrialScene")--试练
require("layers.spriteBlessLayer")--精灵祝福界面

local mMessageLayer=nil
local mMessageTable=nil
local topLayerTable={}
 _count = 0
 
 function getBtnTable()
 	return btnTable
 end
 
 function getCurrentId()
 	return _currentId
 end

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
	endContent_width = pWinSize.width
	endContent_height = pWinSize.height*0.135
	endContent_x = 0
	endContent_y = 0

	topContent_width = pWinSize.width
	topContent_height = pWinSize.height*0.16
	topContent_x = 0
	topContent_y = pWinSize.height-topContent_height

	middleContent_width = pWinSize.width
	middleContent_height = pWinSize.height*0.29
	middleContent_x = (pWinSize.width-middleContent_width)/2
	middleContent_y = pWinSize.height*0.2025

	groupContent_width = pWinSize.width*0.9
	groupContent_height = pWinSize.height*0.16
	groupContent_x = (pWinSize.width-groupContent_width)/2
	groupContent_y =  pWinSize.height*0.505


	miniContent_height = pWinSize.height*0.08
	miniContent_width = pWinSize.width
	miniContent_x = 0
	miniContent_y = pWinSize.height-miniContent_height

	MenuId = {

		eChaDang = 52,--A 100
		eBeiBao =27,
		eFuBen = 51,--
		eZhenXing = 2,
		eShangDian = 37,
		eJingJi = 11,

		eYongBing = 47,--A
		eZhuangBei = 48,
		eHunJi = 49,
		eShuiJing = 16,
		eMoShu = 3,
		eXinJian = 53,--A   102
		eSheZhi = 54,--A  103
		eHuoDong = 14,--活动
		eShangZhen = 104,--我的阵营  ---A
		eShengJi = 50,--佣兵升级
		eEnYuan = 9,
		eJiYou = 55,--集邮
	

		ePaiYang = 5,
		eChuanCheng=38,
		
		eLiLian = 56,--历练
		
		eChaKan = 105,--查看
	}	
	_personalInfo = PersonalInfo.getPersonalInfo()
	
	isGotoLv=false
	isAllowToBasic = true
	
	btnTable = {}
	topLayerTable={}
end

function setMainSceneId()
	setCurrentScene(MenuId.eChaDang)
	setSelect(MenuId.eChaDang)
end

--点击顶部区域响应
function key_top()
	if isAllowToBasic then
		BasicInfoLayer.pushScene()
	end
end

--点击佣兵头像图片响应
function key_hero(pNode, index)

	local tag = pNode
	--[[
	if index then
		tag = index
	else
	--	tag = pNode:getTag()
	    tag = 1
	end 
	]]
	local hero_GeneralID = heroInfo.RecordTabel[tag].GeneralID


	setCurrentScene(MenuId.eShangZhen)
	HeroScene.pushScene(hero_GeneralID)
end

function moveTopList(page)
	_nowPage = page+1
end

--左按钮点击
function leftAction()
	if _nowPage > 1 then
		topList:turnToPage(_nowPage-2)
		
	end
end

--右按钮点击
function rightAction()
	if _nowPage < _totalPage then
		topList:turnToPage(_nowPage)
	end
end;

-- 释放资源
function releaseResource()
_isGetPlotItem=nil

topShowLayer=nil
miniShowLayer=nil

topLayer=nil
groupLayer=nil

endLayer=nil
middleLayer=nil

miniLayer=nil
mLayer=nil
LvUpNoticeLayer.releaseResource()
BroadcastLayer.releaseResource()
bcLayer=nil
top_value=nil
mini_value=nil
mMessageTable=nil
mMessageLayer=nil
isAllowToBasic=nil
_quickNumLabel=nil
_fightNumLabel=nil
isGuide=nil
topLayerTable={}

   	if schedulerEntry1 then
	 CCDirector:sharedDirector():getScheduler():unscheduleScriptEntry(schedulerEntry1)
	 end
--CCScheduler:sharedScheduler():unscheduleScriptFunc("MainMenuLayer.refreshAll")
end

function setIsShow(topValue, miniValue)
	top_value = topValue
	mini_value = miniValue
	if topLayer then
		topShowLayer:setVisible(top_value)
		topLayer:setVisible(top_value)
		topLayerTable.topBtn:setVisible(top_value)
		isAllowToBasic = top_value
	end
	if miniLayer then
		miniShowLayer:setVisible(mini_value)	
		miniLayer:setVisible(mini_value)
	end
end

-- 创建场景
function init(type, fatherScene)
	initResource()
	mShowType = type
	mScene = fatherScene
	-- 添加主层
	mLayer= CCLayer:create()--CCLayer:create()
	mLayer:setAnchorPoint(PT(0,0))
	mLayer:setPosition(PT(0,0))
	mScene:addChild(mLayer, 0)
	--mLayer:registerOnEnter("MainMenuLayer.refreshWin")
	
	local touchLayer=CCLayer:create()
	mLayer:addChild(touchLayer, 0)	
	
	--touchLayer:setIsTouchEnabled(true)
--	touchLayer.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHBEGAN, "MainMenuLayer.touchBegan")
--	touchLayer.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHMOVED, "MainMenuLayer.touchMove")
--	touchLayer.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHENDED, "MainMenuLayer.touchEnd")	
	if mShowType == nil then
		topContent()
		groupContent()
		middleContent()
		endContent()
		broadcast()
	elseif mShowType == 2 then
		endContent()
		
		broadcast()
	elseif mShowType == 1 then
		topContent()
		endContent()
		
		broadcast()
	elseif mShowType == 3 then
		topMiniInfo()
		endContent()
		
		broadcast()
	elseif mShowType == 4 then
		topMiniInfo()
		topContent()
		setIsShow(false, false)		
		broadcast()
		endContent()
	end
   schedulerEntry1 = 	CCDirector:sharedDirector():getScheduler():scheduleScriptFunc(refreshAll, 1, false)
--	CCScheduler:sharedScheduler():scheduleScriptFunc("MainMenuLayer.refreshAll",1,false)--启动计时器
	--]]
end
function gotoChatScene()
	_currentId=nil
	ChatScene.init()
end

function touchBegan(e)
	if isGuide then
		GuideLayer.close()
	end
end


function refreshWin()
	_count = 0
	sendAction(1008)
end;

function  refreshAll()
	_count = _count+1
	if _count%30 == 0 then
		sendAction(1008)
		if MainScene.getMainScene() then
			sendAction(9204)
		end	
	end
end;
function moveEndList(page)
	page = page+1
	if not isNoTurn then
		
		isNoTurn = true
		if page > endPage then
			endPage = 2
			isTurnRight = true
			endList:turnToPage(endPage)
		elseif page <= endPage then
			endPage = 1
			isTurnLeft = true
			endList:turnToPage(endPage-1)
		end
	else
		if ( isTurnRight and page == endPage+1 ) or ( isTurnLeft and page == endPage) then
			isNoTurn = false
			isTurnRight = nil
			isTurnLeft = nil
		end
	end
end
--功能按钮响应
function funcAction(pNode,index)
	local tag =index 
	if not index then
		 tag=pNode:getTag()
	end
	if tag == _currentId then--判断点击的是否是当前界面
		setSelect(_currentId)
		return
	end
	local runningScene = CCDirector:sharedDirector():getRunningScene()	

	if not MenuBtnConfig.getMenuItem(tag) then
		if tag == MenuId.eShuiJing then
			ZyToast.show(runningScene,Language.PROMPT_CRYSTAL)
		else
			ZyToast.show(runningScene,Language.TIP_FUNCTION)
		end
		return	
	end

	if tag == MenuId.eYongBing then
		RoleBagScene.pushScene()
	elseif tag == MenuId.eZhuangBei then
		WeaponScene.init()
	elseif tag == MenuId.eHunJi then
		SoulSkillList.pushScene()
	elseif tag == MenuId.eMoShu then
		MagicScene.init()		
	elseif tag == MenuId.eShuiJing then
		HuntingScene.init()
	elseif tag == MenuId.eShengJi then
		HeroLvUp.pushScene()
	elseif tag == MenuId.eXinJian then
		LetterScenes.init()
	elseif tag == MenuId.eEnYuan then
		FireScutScene.init()

	elseif tag == MenuId.eSheZhi then
		GameSettingScene.init()
	elseif tag == MenuId.eJiYou then
		StampScenes.init()
-------------------------------------------------------------		
		
	elseif tag == MenuId.eChaDang then
        	MainScene.init()
	elseif tag == MenuId.eBeiBao then
		--spriteBlessLayer.setParent()
		BagScene.init()
	elseif tag == MenuId.eFuBen then
		PlotListScene.init(1)
	--	StampScenes.init()
	elseif tag == MenuId.eZhenXing then
    	EmbattleScene.pushScene()
	---	WorldBossToScene.init()
	elseif tag == MenuId.eShangDian then
		businessStore.pushScene()
	elseif tag == MenuId.eJingJi then
		CompetitiveScene.init()
	elseif tag == MenuId.eHuoDong then
		ActiveBarLayer.init()
	elseif tag == MenuId.eLiLian then
		TrialScene.init()
	end

	setCurrentScene(tag)
	
	setSelect(_currentId)

end

--关闭所有页面
function closeAllLayer()
	if topLayer ~= nil then
		topLayer:getParent():removeChild(topLayer, true)
		topLayer = nil
	end
	if endLayer ~= nil then
		endLayer:getParent():removeChild(endLayer, true)
		endLayer = nil
	end
	if middleLayer ~= nil then
		middleLayer:getParent():removeChild(middleLayer, true)
		middleLayer = nil
	end
	if groupLayer ~= nil then
		groupLayer:getParent():removeChild(groupLayer, true)
		groupLayer = nil
	end

	if miniLayer ~= nil then
		miniLayer:getParent():removeChild(miniLayer, true)
		miniLayer = nil
	end
	
	BroadcastLayer.releaseResource()
	

	if mLayer ~= nil then
		mLayer:getParent():removeChild(mLayer, true)
		mLayer = nil
	end	
	
end

----------
--顶部广播显示区
function broadcast()
	BroadcastLayer.releaseResource()
	local endLine = CCSprite:create(P(Image.image_under_Line));	
	local layer = BroadcastLayer.init(mScene)
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(pWinSize.width/2-layer:getContentSize().width/2, 
						pWinSize.height*0.97))
	mLayer:addChild(layer, 1)

	bcLayer = layer
end;

---------------------------------
--顶部区域
function topContent()
	if topLayer ~= nil then
		topLayer:getParent():removeChild(topLayer, true)
		topLayer = nil
	end
	local layer = CCLayer:create()
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(topContent_x, topContent_y))
	mLayer:addChild(layer, 1)
	topLayer = layer
	

	
	local imageBg = CCSprite:create(P("mainUI/list_1000.png"));
	imageBg:setScaleX(topContent_width/imageBg:getContentSize().width)
	imageBg:setScaleY(topContent_height/imageBg:getContentSize().height)
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(0, 0))
	layer:addChild(imageBg, 0)

	local topBtn=ZyButton:new(Image.image_transparent,Image.image_transparent)
	topBtn:setScaleX(topContent_width/topBtn:getContentSize().width)
	topBtn:setScaleY(topContent_height/topBtn:getContentSize().height)
	topBtn:registerScriptHandler(key_top)
	topBtn:setAnchorPoint(PT(0,0))
	topBtn:setPosition(PT(0,0))
	topBtn:addto(layer, 0)
	topLayerTable.topBtn=topBtn
	if top_value then
		topBtn:setVisible(top_value)
		topLayer:setVisible(top_value)

	end

	local topLine = CCSprite:create(P(Image.image_top_line));
	topLine:setScaleX(topContent_width/topLine:getContentSize().width)	
	topLine:setAnchorPoint(PT(0,1))
	topLine:setPosition(PT(0, 0))
	layer:addChild(topLine, 0)


	topShow()

end

function topShow()
	if  topShowLayer then
		topShowLayer:getParent():removeChild(topShowLayer, true)
		topShowLayer = nil
	end
	local layer = CCLayer:create()
	topLayer:addChild(layer, 0)
	topShowLayer = layer	


	if top_value then
		topShowLayer:setVisible(top_value)
	end
	
	local startX = pWinSize.width*0.05
	--姓名背景
	local nameBg = CCSprite:create(P("mainUI/list_1001_2.png"));
	local startY=topContent_height-nameBg:getContentSize().height-pWinSize.height*0.03
	nameBg:setAnchorPoint(PT(0,0))
	nameBg:setPosition(PT(startX,startY))
	layer:addChild(nameBg, 0)
	local nameLabel = nil 
	if _personalInfo._NickName then
		nameLabel = CCLabelTTF:create(_personalInfo._NickName, FONT_NAME, FONT_SM_SIZE)
		nameLabel:setAnchorPoint(PT(0,0))
		nameLabel:setPosition(PT(nameBg:getPosition().x+(nameBg:getContentSize().width-nameLabel:getContentSize().width)*0.5, nameBg:getPosition().y+(nameBg:getContentSize().height-nameLabel:getContentSize().height)*0.5))
		layer:addChild(nameLabel, 0)
	end
	
	
	----玩家VIP图标显示
	if _personalInfo._VipLv ~= nil and _personalInfo._VipLv ~= 0 and isShowVip() then 
			VIPImg = string.format("button/vip_%s.png",_personalInfo._VipLv) 
			local VipNowImg = CCSprite:create(P(VIPImg))
			nameLabel:setPosition(PT(nameBg:getPosition().x+(nameBg:getContentSize().width-nameLabel:getContentSize().width-VipNowImg:getContentSize().width-SX(5))*0.5, nameBg:getPosition().y+(nameBg:getContentSize().height-nameLabel:getContentSize().height)*0.5))
			
			VipNowImg:setAnchorPoint(PT(0,0))
			VipNowImg:setPosition(PT(nameLabel:getPosition().x+nameLabel:getContentSize().width+SX(5),nameLabel:getPosition().y+nameLabel:getContentSize().height*0.5-VipNowImg:getContentSize().height*0.5))
			layer:addChild(VipNowImg,0)
	end
	startY=nameBg:getPosition().y
	--等级背景
	local levelBg = CCSprite:create(P("mainUI/list_1001_1.png"));
	levelBg:setAnchorPoint(PT(0,0))
	levelBg:setPosition(PT(nameBg:getPosition().x+nameBg:getContentSize().width*0.9, nameBg:getPosition().y))
	layer:addChild(levelBg, 0)
	if _personalInfo._UserLv then
		local levelStr = Language.ROLE_LEVEL..":".._personalInfo._UserLv
		local levelLabel = CCLabelTTF:create(levelStr, FONT_NAME, FONT_SM_SIZE)
		levelLabel:setAnchorPoint(PT(0,0))
		levelLabel:setPosition(PT(levelBg:getPosition().x+levelBg:getContentSize().width*0.25, levelBg:getPosition().y+(levelBg:getContentSize().height-levelLabel:getContentSize().height)*0.5))
		layer:addChild(levelLabel, 0)
	end


	local barBg = CCSprite:create(P(Image.image_exp_bar));
	
	--经验背景
	local expBg = CCSprite:create(P("mainUI/list_1001_3.png"));
	expBg:setAnchorPoint(PT(0,0))
	expBg:setPosition(PT(startX, startY-expBg:getContentSize().height-SY(1)))
	layer:addChild(expBg, 0)
	startY=expBg:getPosition().y
	--经验文字图片
	local expBar = CCSprite:create(P("mainUI/list_1008.png"));
	expBar:setAnchorPoint(PT(0,0))
	expBar:setPosition(PT(startX+expBg:getContentSize().width*0.075, expBg:getPosition().y+expBg:getContentSize().height*0.1))
	layer:addChild(expBar, 0)
	--经验条
	local pos = PT(expBg:getPosition().x+expBg:getContentSize().width*0.22, expBg:getPosition().y+expBg:getContentSize().height*0.11)
	createBar("mainUI/list_1004.9.png", _personalInfo._HonourNum, _personalInfo._NextHonourNum, pos, layer)



	--精力背景
	local energyBg = CCSprite:create(P("mainUI/list_1001_3.png"));
	energyBg:setAnchorPoint(PT(0,0))
	energyBg:setPosition(PT(startX, startY-energyBg:getContentSize().height-SY(1)))
	layer:addChild(energyBg, 0)
	--经验文字图片
	local expBar = CCSprite:create(P("mainUI/list_1009.png"));
	expBar:setAnchorPoint(PT(0,0))
	expBar:setPosition(PT(startX+energyBg:getContentSize().width*0.075, energyBg:getPosition().y+energyBg:getContentSize().height*0.1))
	layer:addChild(expBar, 0)
	--精力条
	local pos = PT(energyBg:getPosition().x+energyBg:getContentSize().width*0.22, energyBg:getPosition().y+energyBg:getContentSize().height*0.11)
	createBar("mainUI/list_1005.9.png", _personalInfo._EnergyNum, _personalInfo._MaxEnergyNum, pos, layer)




	--晶石背景
	
	local goldBg = CCSprite:create(P("mainUI/list_1002.png"));
	startY=topContent_height-goldBg:getContentSize().height-pWinSize.height*0.03
	goldBg:setAnchorPoint(PT(0,0))
	goldBg:setPosition(PT(pWinSize.width*0.7, startY))
	layer:addChild(goldBg, 0)
		local item,itemSize = imageMoney("mainUI/list_1006.png", _personalInfo._GoldNum)
		item:setPosition(PT(goldBg:getPosition().x+goldBg:getContentSize().width*0.1, goldBg:getPosition().y+(goldBg:getContentSize().height-itemSize.height)*0.5))
		layer:addChild(item, 0)

	--金币背景
	local goinBg = CCSprite:create(P("mainUI/list_1002.png"));
	goinBg:setAnchorPoint(PT(0,0))
	goinBg:setPosition(PT(goldBg:getPosition().x, energyBg:getPosition().y))
	layer:addChild(goinBg, 0)
		local item,itemSize = imageMoney("mainUI/list_1007.png", _personalInfo._GameCoin)
		item:setPosition(PT(goinBg:getPosition().x+goinBg:getContentSize().width*0.1, goinBg:getPosition().y+(goinBg:getContentSize().height-itemSize.height)*0.5))
		layer:addChild(item, 0)

end

--图片做单位  + 价格
function imageMoney(image, num)

	local layer = CCLayer:create()
		local sprite = CCSprite:create(P(image));
		sprite:setAnchorPoint(PT(0,0))
		sprite:setPosition(PT(0,0))
		layer:addChild(sprite, 0)

		local numLabel = CCLabelTTF:create(num or "", FONT_NAME, FONT_SMM_SIZE)
		numLabel:setAnchorPoint(PT(0,0))
		numLabel:setPosition(PT(sprite:getPosition().x+sprite:getContentSize().width, (sprite:getContentSize().height-numLabel:getContentSize().height)*0.5))
		layer:addChild(numLabel, 0)

		local itemSize = SZ(numLabel:getPosition().x+numLabel:getContentSize().width, sprite:getContentSize().height)

	return layer,itemSize
end

---创建经验条
function  createBar(picPath,currentValue,maxValue,pos,father)
	local barBg = CCSprite:create(P(Image.image_exp_bar));
	barBg:setAnchorPoint(PT(0,0))
	barBg:setPosition(PT(pos.x, pos.y))
	father:addChild(barBg, 0)

	local  proBar = CCSprite:create(P(picPath))
	if maxValue==0 then
		maxValue=100
	end
    currentValue=currentValue or 0
    maxValue=maxValue or 1 
	local scale=currentValue/maxValue
	if scale>1 then
		scale=1
	end

	local width = barBg:getContentSize().width-SX(2)
	draw_w=width*scale  --实际绘制长度

	if draw_w< proBar:getContentSize().width then
		draw_w= proBar:getContentSize().width
	end
	proBar:setScaleX(draw_w/proBar:getContentSize().width)
	proBar:setAnchorPoint(PT(0,0))
	proBar:setPosition(PT(pos.x+SX(1),pos.y))
	father:addChild(proBar,0)
	if currentValue==0 then
		proBar:setVisible(false)
	end


	local numStr = currentValue.."/"..maxValue
	local numLabel = CCLabelTTF:create(numStr, FONT_NAME, FONT_SM_SIZE)
	numLabel:setAnchorPoint(PT(0.5,0))
	numLabel:setPosition(PT(pos.x+barBg:getContentSize().width*0.5, pos.y+barBg:getContentSize().height))
	father:addChild(numLabel, 0)
		

	return proBar
end;



-------------------------------
--我的阵营
function groupContent()
	if groupLayer ~= nil then
		groupLayer:getParent():removeChild(groupLayer, true)
		groupLayer = nil
	end

	local layer = CCLayer:create()
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(0, groupContent_y))
	mLayer:addChild(layer, 1)

	groupLayer = layer

	local imageBg = CCSprite:create(P("mainUI/list_1010.png"));
	local scale = pWinSize.width/imageBg:getContentSize().width
	imageBg:setScale(scale)
--	imageBg:setScaleY(groupContent_height/imageBg:getContentSize().height)	
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(0,0))
	layer:addChild(imageBg, 0)


	groupContent_height = imageBg:getContentSize().height*scale*0.48
	--我的阵营列表
	local heroListSize = SZ(groupContent_width, groupContent_height)
	list = ScutCxList:node(heroListSize.width/5, ccc4(24, 24, 24, 0), heroListSize)
	list:setHorizontal(true)
	list:setAnchorPoint(PT(0, 0))
	list:setTouchEnabled(true)
	local pos_y = imageBg:getContentSize().height*scale*0.225
	list:setPosition(PT(groupContent_x, pos_y))
	layer:addChild(list,0)
	heroList = list
	
	
	local item_y = imageBg:getContentSize().height*scale*0.1
	local item,label = imageAndLabel("title/list_1013.png", _personalInfo._TalPriority)
	item:setPosition(PT(pWinSize.width*0.1, item_y))
	layer:addChild(item, 0)
	_quickNumLabel = label


	local item,label = imageAndLabel("title/list_1025.png", _personalInfo._CombatNum)
	item:setPosition(PT(pWinSize.width*0.5, item_y))
	layer:addChild(item, 0)
	_fightNumLabel = label	
	
end

--图片做单位  + 价格
function imageAndLabel(image, num)

	local layer = CCLayer:create()
	local titleSprite = CCSprite:create(P(image));
	titleSprite:setAnchorPoint(PT(0,0))
	titleSprite:setPosition(PT(0,0))
	layer:addChild(titleSprite, 0)
	
	local bgSize = SZ(pWinSize.width*0.15, pWinSize.height*0.05)
	local numBg = CCSprite:create(P("common/list_1037.9.png"));
	numBg:setScaleX(bgSize.width/numBg:getContentSize().width)
	numBg:setAnchorPoint(PT(0,0))
	numBg:setPosition(PT(titleSprite:getPosition().x+titleSprite:getContentSize().width+pWinSize.width*0.015,titleSprite:getContentSize().height*0.5-numBg:getContentSize().height*0.5))
	layer:addChild(numBg, 0)	

	local numLabel = CCLabelTTF:create(num or "", FONT_NAME, FONT_SM_SIZE)
	numLabel:setAnchorPoint(PT(0.5,0.5))
	numLabel:setPosition(PT(numBg:getPosition().x+bgSize.width*0.5, numBg:getPosition().y+numBg:getContentSize().height*0.5))
	layer:addChild(numLabel, 0)


	return layer,label
end

--我的阵营信息
function showHero()
	local list = heroList
	list:clear()
	local row = 5
	local nAmount = _personalInfo._TotalBattle

    if not nAmount then
        return 
    end
	btnTable.hero={}
	for i=1,nAmount do
	  	local listItem = ScutCxListItem:itemWithColor(ccc3(25,57,45))
		listItem:setOpacity(0)
		list:addListItem(listItem,false)

		local layer = CCLayer:create()
		local image = nil
		local name = nil
		local level = nil
		local tag = i
		local actionPath = MainMenuLayer.key_embattle
		local qualityType = nil
		local ishero  = false

		local heroDetail = heroInfo.RecordTabel[tag]
		if heroDetail then
			image = string.format("smallitem/%s.png", heroDetail.HeadID)
			level = heroDetail.GeneralLv
			actionPath = key_hero
			ishero = true
			qualityType = heroDetail.GeneralQuality 
		end

		local headItem,menuItem = creatHeadItem(image, level, name, qualityType,tag, actionPath)
		local pos_x = list:getRowWidth()*0.5-headItem:getContentSize().width*0.5
		local pos_y = list:getContentSize().height*0.18
		headItem:setAnchorPoint(PT(0,0))
		headItem:setPosition(PT(pos_x, pos_y))
		layer:addChild(headItem, 0)
		
		if ishero then
			--btnTable.hero[#btnTable.hero+1]=headItem
		end
		if heroDetail and heroDetail.GeneralID == _personalInfo._GeneralID then
			btnTable.guideHero	= menuItem
		end
		

		local layout = CxLayout()
		layout.val_x.t = ABS_WITH_PIXEL
		layout.val_y.t = ABS_WITH_PIXEL
		layout.val_x.t  = 0
		layout.val_y.t  = 0
		listItem:addChildItem(layer, layout)
	end
	
		--精灵祝福系统
	spriteBlessLayer.createActionLayer(mScene,PersonalInfo:getPersonalInfo()._WizardNum)
	
	judgeIsGuide()
	
end

function judgeIsGuide()
	if GuideLayer.judgeIsGuide(1) and not PublishNoitc then
		isGuide = true
		GuideLayer.setScene(mScene)
		GuideLayer.init()
	else
		isGuide = nil
	end
end

function setIsPublish(value)
	PublishNoitc = value
end

--创建佣兵图片
function creatHeadItem(image, level, name, qualityType, tag, actionPath)
	-- 背景
	local bgPic = Image.image_zhenfa_beijing
	if qualityType then
		bgPic = getQualityBg(qualityType, 1)
	end
	local menuItem = CCMenuItemImage:create(P(bgPic), P(bgPic))
	local btn = CCMenu:createWithItem(menuItem)
	btn:setAnchorPoint(PT(0,0))
	menuItem:setAnchorPoint(PT(0,0))
	if tag~= nil and tag~=-1 then
		menuItem:setTag(tag)
	end

	--设置缩放
	if nScale == nil then
		nScale = 1
	end
	menuItem:setScale(nScale)
	btn:setContentSize(SZ(menuItem:getContentSize().width*nScale,menuItem:getContentSize().height*nScale))

	--设置回调
	if actionPath ~= nil then
		menuItem:registerScriptTapHandler(actionPath)
	end


	-- 缩略图
	if image ~= nil and image ~= "" then
		local imageLabel = CCSprite:create(P(image))
		imageLabel:setAnchorPoint(CCPoint(0.5, 0.5))
		imageLabel:setPosition(PT(menuItem:getContentSize().width*0.5,menuItem:getContentSize().height*0.5))
		menuItem:addChild(imageLabel,0)
	end
	--等级
	if level ~= nil then
		--等级背景
		local levelBg = CCSprite:create(P("common/list_1011.png"))
		levelBg:setAnchorPoint(CCPoint(0.5, 0.5))
		levelBg:setPosition(PT(menuItem:getContentSize().width*0.5, -menuItem:getContentSize().height*0.1))
		menuItem:addChild(levelBg,0)



		local levelLabel = CCLabelTTF:create("Lv"..level, FONT_NAME, FONT_SM_SIZE-2)
		levelLabel:setAnchorPoint(CCPoint(0.5, 0.5))
		levelLabel:setPosition(PT(levelBg:getPosition().x, levelBg:getPosition().y))
		menuItem:addChild(levelLabel, 0)
	end


	if name ~= nil then
		local nameLabel = CCLabelTTF:create(name, FONT_NAME, FONT_SM_SIZE-2)
		nameLabel:setAnchorPoint(CCPoint(0, 0))
		nameLabel:setPosition(PT(SX(3),menuItem:getContentSize().height-nameLabel:getContentSize().height-SY(3)))
		menuItem:addChild(nameLabel,1)
	end




	return btn,menuItem

end;



--点击空白佣兵
function key_embattle()

	setCurrentScene(MenuId.eZhenXing)

	EmbattleScene.pushScene()
end;



---------------------------
--中间按钮区域
function middleContent()
	if middleLayer ~= nil then
		middleLayer:getParent():removeChild(middleLayer, true)
		middleLayer = nil
	end

	local layer = CCLayer:create()
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(middleContent_x, middleContent_y))
	mLayer:addChild(layer, 1)

	middleLayer = layer

	--按钮列表
	local topListSize = SZ(middleContent_width*0.8, middleContent_height)
	list = ScutCxList:node(topListSize.width, ccc4(24, 24, 24, 0), topListSize)
	list:setHorizontal(true)
	list:setPageTurnEffect(true)
	list:setAnchorPoint(PT(0, 0))
	list:setPosition(PT((middleContent_width-topListSize.width)*0.5, 0))
	list:registerLoadEvent(moveTopList)
	layer:addChild(list,0)
	list:setTouchEnabled(true)
	topList = list
	


	funcTabel ={
		{ id = MenuId.eYongBing, strPic = "title/list_1016.png"},--佣兵
		{ id = MenuId.eZhuangBei, strPic = "title/list_1017.png"},--装备
		{ id = MenuId.eHunJi, strPic = "title/list_1018.png"},--魂技
		{ id = MenuId.eMoShu, strPic = "title/list_1021.png"},--魔术
		{ id = MenuId.eShuiJing, strPic = "title/list_1019.png"},--水晶
		{ id = MenuId.eShengJi, strPic = "title/list_1020.png"},--升级
		{ id = MenuId.eEnYuan, strPic = "title/list_1162.png"},--恩怨
		{ id = MenuId.eSheZhi, strPic = "title/list_1093.png"},--设置
	--	{ id = MenuId.eXinJian, strPic = "title/list_1092.png"},--信件	
		{ id = MenuId.eJiYou, strPic = "title/list_1212.png"},--集邮
		
	}
		
		MenuBtnConfig.setMenuItem(MenuId.eYongBing)
		MenuBtnConfig.setMenuItem(MenuId.eSheZhi)
		
		local line = 4
		local row = 2		
		local nAmount = math.ceil(#funcTabel/(line*row))
		btnTable.middle = {}
		

		if not _nowPage then
			_nowPage = 1
		end
		local isGuide,GuideID,step = GuideLayer.getIsGuide()
		if GuideID == 1009 and (step == 1 or step == 2) and _nowPage ~= 1 then
			_nowPage = 1
		end
		
		_totalPage = nAmount
	
		for k=1,nAmount do
		  	local listItem = ScutCxListItem:itemWithColor(ccc3(25,57,45))
			listItem:setOpacity(0)
			list:addListItem(listItem,false)
			local funcBtnLayer = CCLayer:create()

			--按钮 排序 从上到下   从左到右   先上下 后左右
			for i=1,line*row do
				local num = (k-1)*(line*row)+i
				if funcTabel[num] then
					local funcBtn = ZyButton:new("mainUI/list_1026_1.png","mainUI/list_1026_2.png")
					local tag = funcTabel[num].id
					funcBtn:setTag(tag)
					funcBtn:registerScriptHandler(funcAction)
					funcBtn:setAnchorPoint(PT(0,0))
	
					local image = funcTabel[num].strPic
					local strImage = CCSprite:create(P(image));
					strImage:setAnchorPoint(PT(0,0))
					strImage:setPosition(PT((funcBtn:getContentSize().width-strImage:getContentSize().width)*0.5, (funcBtn:getContentSize().height-strImage:getContentSize().height)*0.5))
					funcBtn:addChild(strImage, 0)
	
					local startX = topList:getContentSize().width/row*(math.floor((i-1)/line)+0.5)-funcBtn:getContentSize().width*0.5
					local startY = topList:getContentSize().height/line*(line-(i-1)%line-0.5)-funcBtn:getContentSize().height*0.5
					funcBtn:setPosition(PT(startX, startY))
					funcBtn:addto(funcBtnLayer, 0)
					
					
					btnTable.middle[#btnTable.middle+1] = funcBtn
				end
			end
			--[[
			local pageLabel = CCLabelTTF:create(k.."/"..nAmount, FONT_NAME, FONT_SM_SIZE)
			pageLabel:setAnchorPoint(PT(0.5,0))
			pageLabel:setPosition(PT(topList:getContentSize().width*0.5, topList:getContentSize().height*0.05))
			funcBtnLayer:addChild(pageLabel, 0)
			--]]
	
			local layout = CxLayout()
			layout.val_x.t = ABS_WITH_PIXEL
			layout.val_y.t = ABS_WITH_PIXEL
			layout.val_x.t  = 0
			layout.val_y.t  = 0
			listItem:addChildItem(funcBtnLayer, layout)
		
		end
		
		
		if nAmount > 1 then
			local leftBtn = ZyButton:new(Image.image_LeftButtonNorPath)
			leftBtn:registerScriptHandler(leftAction)
			leftBtn:setAnchorPoint(PT(0,0))
			leftBtn:setPosition(PT(list:getPosition().x-leftBtn:getContentSize().width, list:getPosition().y+list:getContentSize().height*0.5-leftBtn:getContentSize().height*0.5))	
			leftBtn:addto(layer, 0)
			
			local rightBtn = ZyButton:new(Image.image_rightButtonNorPath)
			rightBtn:registerScriptHandler(rightAction)
			rightBtn:setAnchorPoint(PT(0,0))
			rightBtn:setPosition(PT(list:getPosition().x+list:getContentSize().width, list:getPosition().y+list:getContentSize().height*0.5-leftBtn:getContentSize().height*0.5))	
			rightBtn:addto(layer, 0)
			
		end
	
	 creatLetter()
	 
	 topList:turnToPage(_nowPage-1)
end





--信件
function creatLetter()
		---   信件按钮接入
		local layer = middleLayer
		

		local letterBtn = ZyButton:new("button/list_1213.png")
		local pos =  PT(pWinSize.width*0.98-letterBtn:getContentSize().width, pWinSize.height*0.25)		
		local tag = MenuId.eXinJian
		letterBtn:setTag(tag)
		letterBtn:registerScriptHandler(funcAction)
		letterBtn:setAnchorPoint(PT(0,0))
		letterBtn:setPosition(pos)
		letterBtn:addto(layer, 1)
		
		
		
		if _personalInfo._unReadCount and _personalInfo._unReadCount > 0 then
			letterBtn:setVisible(false)
			
			--闪烁
			local mailSprite=Human:new()		
			mailSprite:createNpc(layer,"xinfeng",nil,nil,pos.x+letterBtn:getContentSize().width*0.5,pos.y, 1)
			
			--透明按钮
			local unTouchBtn =  ZyButton:new(Image.image_transparent,Image.image_transparent)
			unTouchBtn:setScaleX(letterBtn:getContentSize().width/unTouchBtn:getContentSize().width)
			unTouchBtn:setScaleY(letterBtn:getContentSize().height/unTouchBtn:getContentSize().height)
			unTouchBtn:setAnchorPoint(PT(0,0))
			unTouchBtn:setPosition(PT(pos.x, pos.y))
			unTouchBtn:addto(layer, 0)
			unTouchBtn:setTag(tag)
			unTouchBtn:registerScriptHandler(funcAction)			
			

		end

end

-------------------------
--底部区域  菜单按钮
function endContent()
	if endLayer ~= nil then
		endLayer:getParent():removeChild(endLayer, true)
		endLayer = nil
	end

	local layer = CCLayer:create()
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(endContent_x, endContent_y))
	mLayer:addChild(layer, 1)
	endLayer = layer

	--底部背景
	local imageBg = CCSprite:create(P("mainUI/list_1014.png"));
	imageBg:setScaleY(endContent_height/imageBg:getContentSize().height)
	imageBg:setScaleX(endContent_width/imageBg:getContentSize().width)
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(0, 0))
	layer:addChild(imageBg, 0)
	
	--上边框
	local endLine = CCSprite:create(P(Image.image_under_Line));
	endLine:setScaleX(endContent_width/endLine:getContentSize().width)	
	endLine:setAnchorPoint(PT(0,0))
	endLine:setPosition(PT(0, endContent_height))
	layer:addChild(endLine, 0)


	local leftImage = CCSprite:create(P("mainUI/list_1142_1.png"));
	leftImage:setAnchorPoint(PT(1,1))
	leftImage:setPosition(PT(endContent_width/6, endContent_height))
	layer:addChild(leftImage, 0)
	
	local rightImage = CCSprite:create(P("mainUI/list_1142_2.png"));
	rightImage:setAnchorPoint(PT(1,1))
	rightImage:setPosition(PT(endContent_width, endContent_height))
	layer:addChild(rightImage, 0)



	menuTabel ={
		{ id = MenuId.eChaDang, strPic = "mainUI/list_1027_1.png",downPic="mainUI/list_1027_2.png"},
		{ id = MenuId.eBeiBao, strPic = "mainUI/list_1031_1.png",downPic="mainUI/list_1031_2.png"},
		{ id = MenuId.eFuBen, strPic = "mainUI/list_1029_1.png",downPic="mainUI/list_1029_2.png"},
		{ id = MenuId.eZhenXing, strPic = "mainUI/list_1028_1.png",downPic="mainUI/list_1028_2.png"},
		{ id = MenuId.eShangDian, strPic = "mainUI/list_1030_1.png",downPic="mainUI/list_1030_2.png"},
		{ id = MenuId.eHuoDong, strPic = "mainUI/list_1139_1.png",downPic="mainUI/list_1139_2.png"},
		{ id = MenuId.eLiLian, strPic = "mainUI/list_2065_1.png",downPic="mainUI/list_2065_2.png"},	--历练		
		{ id = MenuId.eJingJi, strPic = "mainUI/list_1091_1.png",downPic="mainUI/list_1091_2.png"},	
	}
	MenuBtnConfig.setMenuItem(MenuId.eChaDang)
	

	btnTable.menu={}
	
	local menuNum = 6--
	for i=1,1 do
		local image = menuTabel[i].strPic
		local imageDown = menuTabel[i].downPic
		local munuBtn=ZyButton:new(image,imageDown,imageDown)
		munuBtn:setTag(menuTabel[i].id)
		munuBtn:registerScriptHandler(funcAction)
		munuBtn:setAnchorPoint(PT(0,0))
		local startX = endContent_width/menuNum*(i-0.5)-munuBtn:getContentSize().width*0.5
		local startY = (endContent_height-munuBtn:getContentSize().height)*0.5
		munuBtn:setPosition(PT(startX, startY))
		munuBtn:addto(layer, 0)
		
		btnTable.menu[#btnTable.menu+1]=munuBtn
	end
	
	--按钮列表
	local endListSize = SZ(endContent_width/6*5, endContent_height)
	list = ScutCxList:node(endListSize.width/5, ccc4(24, 24, 24, 0), endListSize)
	list:setHorizontal(true)
	list:setAnchorPoint(PT(0, 0))
	list:setPosition(PT(endContent_width/6, 0))
	layer:addChild(list,0)
	list:registerLoadEvent(moveEndList)
	list:setPageTurnEffect(true)
	list:setRecodeNumPerPage (1) 
    list:setTouchEnabled(true)
	endList = list

	for i=2,#menuTabel do
	  	local listItem = ScutCxListItem:itemWithColor(ccc3(25,57,45))
		listItem:setOpacity(0)
		list:addListItem(listItem,false)		
	
		local layer = CCLayer:create()
		
		local image = menuTabel[i].strPic
		local imageDown = menuTabel[i].downPic
		local munuBtn=ZyButton:new(image,imageDown,imageDown)
		munuBtn:setTag(menuTabel[i].id)
		munuBtn:registerScriptHandler(funcAction)
		munuBtn:setAnchorPoint(PT(0,0))
		local startX = list:getRowWidth()*0.5-munuBtn:getContentSize().width*0.5
		local startY = (endContent_height-munuBtn:getContentSize().height)*0.5
		munuBtn:setPosition(PT(startX, startY))
		munuBtn:addto(layer, 0)		
		
		local layout = CxLayout()
		layout.val_x.t = ABS_WITH_PIXEL
		layout.val_y.t = ABS_WITH_PIXEL
		layout.val_x.t  = 0
		layout.val_y.t  = 0
		listItem:addChildItem(layer, layout)

		btnTable.menu[#btnTable.menu+1]=munuBtn
	end
	if not endPage then
		endPage = 1
	else
		if endPage > 1 then
			isTurnRight = true
			isNoTurn = true
			endList:turnToPage(endPage)
		end
	end
end





function setCurrentScene(id)
	_currentId = id
end

function setSelect(id)
	for k,v in ipairs(menuTabel) do
		if id == v.id then
			btnTable.menu[k]:selected()
		else
			btnTable.menu[k]:unselected()
		end
	end
end

--顶部缩略内容
function topMiniInfo()
	if miniLayer ~= nil then
		miniLayer:getParent():removeChild(miniLayer, true)
		miniLayer = nil
	end
	local layer = CCLayer:create()
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(miniContent_x, miniContent_y))	
	mLayer:addChild(layer, 0)
	miniLayer = layer
	
	if mini_value then
		miniLayer:setVisible(mini_value)
	end
	local imageBg = CCSprite:create(P("common/list_1047.png"));
	imageBg:setScaleX(miniContent_width/imageBg:getContentSize().width)	
	imageBg:setScaleY(miniContent_height/imageBg:getContentSize().height)
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(0, 0))
	layer:addChild(imageBg, 0)

	local topBtn=ZyButton:new(Image.image_transparent,Image.image_transparent)
	topBtn:setScaleX(pWinSize.width/topBtn:getContentSize().width)
	topBtn:setScaleY(miniContent_height/topBtn:getContentSize().height)
	topBtn:registerScriptHandler(key_top)
	topBtn:setAnchorPoint(PT(0,0))
	topBtn:setPosition(PT(miniContent_x, miniContent_y))
	topBtn:addto(layer, 0)
	


	showTopMini()
end

function showTopMini()
	if miniShowLayer then
		miniShowLayer:getParent():removeChild(miniShowLayer, true)
		miniShowLayer = nil	
	end
	
	local layer = CCLayer:create()
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(0, 0))	
	miniLayer:addChild(layer, 0)
	
	miniShowLayer = layer
	
	
	if mini_value then
		miniShowLayer:setVisible(mini_value)
	end	

	--姓名背景
	local startX =0
	local nameBg = CCSprite:create(P("mainUI/list_1001_2.png"));
	nameBg:setAnchorPoint(PT(0,0))
	local height = miniContent_height*0.5-nameBg:getContentSize().height*0.5
	nameBg:setPosition(PT(startX, height))
	layer:addChild(nameBg, 0)
	if _personalInfo._NickName then
		local nameLabel = CCLabelTTF:create(_personalInfo._NickName, FONT_NAME, FONT_SM_SIZE)
		nameLabel:setAnchorPoint(PT(0,0))
		nameLabel:setPosition(PT(nameBg:getPosition().x+(nameBg:getContentSize().width-nameLabel:getContentSize().width)*0.5, nameBg:getPosition().y+(nameBg:getContentSize().height-nameLabel:getContentSize().height)*0.5))
		layer:addChild(nameLabel, 0)
	end

	--等级背景
	local levelBg = CCSprite:create(P("mainUI/list_1001_1.png"));
	levelBg:setAnchorPoint(PT(0,0))
	levelBg:setPosition(PT(nameBg:getPosition().x+nameBg:getContentSize().width*0.9, height))
	layer:addChild(levelBg, 0)
	if _personalInfo._UserLv then
		local levelStr = Language.ROLE_LEVEL..":".._personalInfo._UserLv
		local levelLabel = CCLabelTTF:create(levelStr, FONT_NAME, FONT_SM_SIZE)
		levelLabel:setAnchorPoint(PT(0,0))
		levelLabel:setPosition(PT(levelBg:getPosition().x+levelBg:getContentSize().width*0.25, levelBg:getPosition().y+(levelBg:getContentSize().height-levelLabel:getContentSize().height)*0.5))
		layer:addChild(levelLabel, 0)
	end

	--晶石背景
	local goldBg = CCSprite:create(P("mainUI/list_1051.png"));
	goldBg:setAnchorPoint(PT(0,0))
	goldBg:setPosition(PT(pWinSize.width-goldBg:getContentSize().width-pWinSize.width*0.01, height))
	layer:addChild(goldBg, 0)
		local item1,itemSize = imageMoney("mainUI/list_1006.png", _personalInfo._GoldNum)
		item1:setPosition(PT(goldBg:getPosition().x+goldBg:getContentSize().width*0.01, 
				goldBg:getPosition().y+(goldBg:getContentSize().height-itemSize.height)*0.5))
		layer:addChild(item1, 0)
		
		local width=itemSize.width+pWinSize.width*0.01
	--金币背景
	local item,itemSize = imageMoney("mainUI/list_1007.png", _personalInfo._GameCoin)
	item:setPosition(PT(goldBg:getPosition().x+goldBg:getContentSize().width*0.45, goldBg:getPosition().y+(goldBg:getContentSize().height-itemSize.height)*0.5))
	layer:addChild(item, 0)
end


-------------聊天
function releaseMessageLayer()

	if mMessageLayer then
		mMessageLayer:getParent():removeChild(mMessageLayer,true)
		mMessageLayer=nil
	end
	mMessageTable=nil
end;

function createMessageLayer()
	if mMessageLayer then
		return
	end
	local layer=CCLayer:create()
	mMessageLayer=layer
	local runningScene = CCDirector:sharedDirector():getRunningScene()	
	mLayer:addChild(layer,3)
	mMessageTable={}
	local chatBtn=ZyButton:new("button/list_1161.png")
	chatBtn:setAnchorPoint(PT(0,0))
	chatBtn:setPosition(PT(pWinSize.width*0.02,endContent_height*1.1))
	chatBtn:addto(layer,0)
	chatBtn:registerScriptHandler(gotoChatScene)
	mMessageTable.chatBtn=chatBtn
	refreshSingleMessage()
end;



function refreshSingleMessage()

	local messageTable=ChatScene.getLocalMessage(1)
	local chatState=accountInfo.getConfig("sys/config.ini", "SETTING", "chatState")
       chatState=tonumber(chatState)
	if chatState~=0 and #messageTable> 0 then
		if mMessageTable.label then
			mMessageTable.label:getParent():removeChild(mMessageTable.label,true)
			mMessageTable.label=nil
		end
		local chatBtn=mMessageTable.chatBtn
		local info=messageTable[#messageTable]
		if info.Content then
			local layer,layerH=ChatScene.createListItem(info,1,1)
			
		--[[
			local cotentStr=utfstrIndex(info.Content,15)
			contentStr=cotentStr or "" .. "("..info.SendDate..")"
			local label=CCLabelTTF:create(contentStr,FONT_NAME,FONT_SM_SIZE)
			label:setAnchorPoint(PT(0,0.5)
			--]]
			layer:setPosition(PT(chatBtn:getPosition().x+chatBtn:getContentSize().width,
									chatBtn:getPosition().y+chatBtn:getContentSize().height/2-layerH/2))
			mMessageLayer:addChild(layer,0)
			mMessageTable.label=layer
		end
	end

end;



function rebuildMessage()	
	if mMessageLayer   and  mMessageTable then	
		local chatState=accountInfo.getConfig("sys/config.ini", "SETTING", "chatState")
       	chatState=tonumber(chatState)
		if chatState~=0  then	
			 refreshSingleMessage()
		end
	end
end


function sendAction(actionId)
	if actionId == 3009 then
		actionLayer.Action3009(mScene, nil)
	elseif actionId == 1401 then
	
	elseif actionId == 1008 then
		actionLayer.Action1008(mScene, false)

	elseif actionId == 1025 then
		actionLayer.Action1025(mScene, false)

	elseif actionId == 9204 then
		actionLayer.Action9204(mScene, false)	
	elseif actionId == 1902 then
		actionLayer.Action1902(mScene, false)			
	end
end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if actionId == 1401 then
		local serverInfo = actionLayer._1401Callback(pScutScene, lpExternalData)
		if serverInfo ~= nil then
			heroInfo = serverInfo
			showHero()
		end
	elseif actionId == 3009 then
		local serverInfo = actionLayer._3009Callback(pScutScene, lpExternalData)
		if serverInfo ~= nil then
			ActiveBarLayer.init(mScene,serverInfo)
		end
	elseif actionId == 1008 then
		local serverInfo = actionLayer._1008Callback(pScutScene, lpExternalData)
		if serverInfo ~= nil then
			PersonalInfo.setPersonalInfo(serverInfo)
			
			if serverInfo.FuncTabel~=nil then 
				MenuBtnConfig.clearMenuItemTables()
				for k , v in ipairs(serverInfo.FuncTabel) do
					if v ~=nil then
						MenuBtnConfig.setMenuItem(v.FunEnum)
					end
				end
			end
			
			if _quickNumLabel and _fightNumLabel then
				_quickNumLabel:setString(_personalInfo._TalPriority)
				_fightNumLabel:setString(_personalInfo._CombatNum)
			end
		
			if isGotoLv then
				return
			end
			if miniShowLayer then
				showTopMini()
			end
			if topShowLayer then
				topShow()
			end
            
	            if  MainScene.getMainScene() then
	            		--精灵祝福系统
				    spriteBlessLayer.createActionLayer(mScene,serverInfo.WizardNum)
	            end;

			if serverInfo.IsLv == 1 and not isGotoLv then
				isGotoLv = true
				local OpenFuncTabel = nil
				if serverInfo.OpenFuncTabel and #serverInfo.OpenFuncTabel > 0 then
					OpenFuncTabel = serverInfo.OpenFuncTabel			
				end	
				--OpenFuncTabel={ [1]={ FunEnum=11 }}
				LvUpNoticeLayer.setOpenTable(OpenFuncTabel)	
				sendAction(1025)
			end
		end
	elseif actionId == 1025 then
		local serverInfo = actionLayer._1025Callback(pScutScene, lpExternalData)
		if serverInfo ~= nil then
			GuideLayer.hide()
			LvUpNoticeLayer.setInfo(mScene, mLayer , serverInfo)
			if  not _isGetPlotItem then
				LvUpNoticeLayer.lvUpNotice()
				--LvUpNoticeLayer.setVisible(false)
			end
		end
	end
end

function setIsGotoLv(value)
	isGotoLv = value
end

function getIsGotoLv()
	return isGotoLv
end


function isGetPlotItem(value)	
	_isGetPlotItem = value
end


