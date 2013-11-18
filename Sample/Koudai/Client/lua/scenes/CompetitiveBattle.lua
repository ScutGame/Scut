------------------------------------------------------------------
-- CompetitiveBattle.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 
------------------------------------------------------------------

module("CompetitiveBattle", package.seeall)





function setBattleInfo(info, scene, battleType)
	mBattleInfo = info
	mScene = scene
	_battleType = battleType
end

-- 场景入口
function pushScene()
	initResource()
	createScene()

end

-- 退出场景
function closeAction()
	closeFightLayer()
	if mLayer then
		mLayer:getParent():removeChild(mLayer, true)
		mLayer = nil
	end
	releaseResource()

	afterClose()
end

function afterClose()
	MainMenuLayer.refreshWin()
	if _battleType == 4 then--好友

	elseif _battleType == 1 then--竞技场
		CompetitiveScene.refreshWin()
	elseif _battleType == 2 then--信件
		MainMenuLayer.refreshWin()	
	elseif _battleType == 7 then--信件回放
		MainMenuLayer.refreshWin()
	end
end

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
	
end

-- 释放资源
function releaseResource()
	mLayer = nil
	fightLayer=nil
	mBattleInfo=nil
end

-- 创建场景
function createScene()	
	-- 添加主层
	mLayer= CCLayer:create()
	mScene:addChild(mLayer, 1)
	
	local actionBtn=UIHelper.createActionRect(pWinSize)
	actionBtn:setPosition(PT(0,0))
	mLayer:addChild(actionBtn,0)


	creatBg()
	
	startFight(mBattleInfo)
	
end

function creatBg()
	local imageBg = CCSprite:create(P("map/map_1001.jpg"))
	imageBg:setScaleX(pWinSize.width/imageBg:getContentSize().width)
	imageBg:setScaleY(pWinSize.height/imageBg:getContentSize().height)
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(0,0))
	mLayer:addChild(imageBg, 0)
end;


----------------开始战斗
function startFight(info)
	if fightLayer then
		fightLayer:getParent():removeChild(fightLayer, true)
		fightLayer = nil
	end
	local layer = CCLayer:create()
	mLayer:addChild(layer, 0)
	
	fightLayer = layer

	local battleType = _battleType
	if battleType==7 then
		IsOverCombat = 1
	else
		IsOverCombat = nil--1跳过 
	end
	PlotFightLayer.setFightInfo(info,IsOverCombat,battleType)

	PlotFightLayer.init(fightLayer)

end;


function battleOver()
	if mBattleInfo.SportsPrizeStr and mBattleInfo.SportsPrizeStr ~= "" then
		showMysteria()--神秘事件
	else
		showResult()
	end
end

function showMysteria()
	local box = ZyMessageBoxEx:new()
	box:isGrilShow(true)
	box:doPrompt(mScene, nil, mBattleInfo.SportsPrizeStr , Language.IDS_SURE,showResult) 
end;

function showResult()
	local resultStr = ""
	if mBattleInfo.IsWin == 1 then
		resultStr = resultStr..Language.PLOT_WIN
	else
		resultStr = resultStr..Language.PLOT_FALSE
	end
	

	resultStr = resultStr..Language.IDS_COMMA..Language.IDS_PRICE..Language.IDS_COLON

	if mBattleInfo.GameCoin ~= nil and mBattleInfo.GameCoin > 0 then

		resultStr = resultStr..mBattleInfo.GameCoin..Language.IDS_GOLD 
	end
--	 IDS_COMMA
	local box = ZyMessageBoxEx:new()
	box:isGrilShow(true)
	box:doPrompt(mScene, nil, resultStr,Language.IDS_OK,closeAction)
end;


function closeFightLayer()
	if fightLayer then
		fightLayer:getParent():removeChild(fightLayer, true)
		fightLayer = nil
	end
	PlotFightLayer.releaseResource()
end;




