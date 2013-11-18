------------------------------------------------------------------
-- PlotFightLayer.lua
-- Author     : Zonglin Liu 
-- Version    : 1.0
-- Date       :   
-- Description: 战斗播放
------------------------------------------------------------------

module("PlotFightLayer", package.seeall)
require("battle.BattleResult")--副本通关评价
require("scenes.SBattleResult")--圣吉塔通关评价
---------------------------------------


_scene = nil 		-- 场景

--每秒的速度
local fightSpeed=pWinSize.height
--血条长度
local hpBar_LongLenth = ZyImage:imageSize("common/list_1032.png").width*0.95


--_battleType   战斗类型  nil普通副本， 1竞技场,  2信件反击，3世界boss , 4好友界面战斗 ,5圣吉塔,6考古

function setFightInfo(info, isOverCombat, battleType)
	_AllFightInfoTable=info
	IsOverCombat = isOverCombat
	
	_battleType = battleType
end

-- 释放资源
function releaseResource()
_overBtn = nil
isNotNpc = nil--是否npc 怪物  ， 判断是否有先攻值
_battleType=nil
ScutAnimation.CScutAnimationManager:GetInstance():ReleaseAllAniGroup()
end

--初始数据
function initResource()
	stopFightTag = false
	personalInfo=PersonalInfo.getPersonalInfo()
	if _AllFightInfoTable~=nil then
		_FiguresGeneralInfo=_AllFightInfoTable.AgainstTable
		_EnemyGeneralInfo=_AllFightInfoTable.DefendingTable
		_FightInfo=_AllFightInfoTable.FightProcessTable	
	end
	if _battleType == 1 or _battleType == 2 or _battleType == 4 then
		isNotNpc = true
	end
	
end;


-- 初始化
function init(mainScene)

	initResource()
	_scene = mainScene
	_layer=CCLayer:create()
	-- 注册触屏事件
--	_layer.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHBEGAN, "PlotFightLayer.touchBegan")
--	_layer.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHMOVED, "PlotFightLayer.touchMove")
--	_layer.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHENDED, "PlotFightLayer.touchEnd")
--	_layer:setIsTouchEnabled(true)
	_scene:addChild(_layer,3)
-----------------------------------------------精灵层
	_SpriteLayer=CCLayer:create()
	_SpriteLayer:setAnchorPoint(PT(0,0))
	_SpriteLayer:setPosition(PT(0,0))
	_layer:addChild(_SpriteLayer,0)
----立即结束按钮
	quickOver()

----开始战斗循环
	_fightState = EnumFightState.EFightBegin
	fightEngine()	
end

----立即结束按钮
function quickOver()
	-----竞技场 和普通副本 有跳过战斗接口
	if IsOverCombat ==1 then
		local overBtn = ZyButton:new(Image.image_button, nil, nil, Language.PLOT_JUMP, FONT_NAME, FONT_SM_SIZE)
		overBtn:setAnchorPoint(PT(0,0))
		overBtn:setPosition(PT(pWinSize.width*0.9975-overBtn:getContentSize().width, pWinSize.height*0.0025))
		overBtn:registerScriptHandler(JumpFightAction)
		overBtn:addto(_layer, 0)
		_overBtn = overBtn
	end
end


--跳过战斗
function JumpFightAction()
	_overBtn:setEnabled(false)
	stopFight()
end


----------停止战斗
function stopFight()
	_fightState = EnumFightState.ERoundEnd
	_curState = EnumAttackProcess.eAttackEnd
	stopFightTag = true
	_layer:stopAllActions()
	if _battleType == nil then
		battleScene.isQuickEnd(_AllFightInfoTable)
	else
		fightOver()
	end
end


---战斗引擎
EnumFightState = {
	EFightBegin		= 0,	---战斗开始
	EFightDataInit 	= 1,	---数据初始化
	EShowEffect		= 2, ---初始特效
	ERoundBegin 	= 3,	---回合开始
	ERoundEnd		= 4,	---回合结束
	EFightEnd 		= 5, ---战斗结束
}

function fightEngine()
	_fightState = _fightState + 1
	if _fightState == EnumFightState.EFightDataInit then
		---数据初始化----主角现身
		fightDataInit()
		initPlayerPosition()
	elseif _fightState == EnumFightState.EShowEffect then
		showFirstEffect()--初始特效
	elseif _fightState == EnumFightState.ERoundBegin then
		playNextRound()
	elseif _fightState == EnumFightState.ERoundEnd then
		---一场战斗播放结束
		fightOver()
	end
end;


---战斗数据初始化
function  fightDataInit()
	_FightAroundIndex=0
end;

---初始化人物
function  initPlayerPosition()
	--如果没有己方  或 敌方信息 战斗播放结束
	if _FiguresGeneralInfo==nil or _EnemyGeneralInfo == nil then
		_fightState = EnumFightState.ERoundBegin
		fightEngine()
		return
	end
	
	--创建己阵型
	for k, v in ipairs(_FiguresGeneralInfo) do
		if v and v.LiveNum>0 then
			v.Role = 0
		--	local headImg = string.format("smallitem/%s.png", v.AttGeneralHeadID)
			local headImg = string.format("battleHead/%s.png", v.AttGeneralHeadID)
			local name = v.AttGeneralName
			local level = nil
			local memberCallBack = ""
			local position = v.AttPosition		
			local pos=getProsPosition(position,true)
			local tag = k
			local quality = v.AttGeneralQuality
	
			local FiguresSprite=HumanCard:new()
			FiguresSprite:createPlayer(_SpriteLayer, headImg, speed, name, layerTag,Tag, quality)
			FiguresSprite:setPosition(pos.x, pos.y)				

			v.sprite=FiguresSprite
			if v.MomentumNum>=100 then
				FiguresSprite:MomentumMax()		
			end
			local hgBar=createHpLabel(v.LiveNum,v.LiveMaxNum)
			v.hgBar=hgBar
			hgBar:setPosition(PT(pos.x+FiguresSprite:getContentSize().width/2-hpBar_LongLenth/2,
			pos.y-hgBar:getContentSize().height))
			_SpriteLayer:addChild(hgBar,0)
		end
	end
	
	--创建敌方阵型
	for k, v in ipairs(_EnemyGeneralInfo) do
		if  v and v.LiveNum>0 then
			v.Role = 1
	--		local headImg = string.format("smallitem/%s.png", v.AttGeneralHeadID)
			local headImg = string.format("battleHead/%s.png", v.AttGeneralHeadID)
			local name = v.AttGeneralName
			local level = nil
			local memberCallBack = ""
			local position = v.AttPosition
			local pos=getProsPosition(position,false)	
			local tag = k
			local layerTag = 0
			local quality = v.AttGeneralQuality
			
			local FiguresSprite=HumanCard:new()
			FiguresSprite:createPlayer(_SpriteLayer, headImg, speed, name, layerTag,Tag,quality)
			FiguresSprite:setPosition(pos.x, pos.y)				
			v.sprite=FiguresSprite				
			if v.MomentumNum>=100 then
				FiguresSprite:MomentumMax()		
			end
			local hgBar=createHpLabel(v.LiveNum,v.LiveMaxNum)
			v.hgBar=hgBar
			hgBar:setPosition(PT(pos.x+FiguresSprite:getContentSize().width/2-hpBar_LongLenth/2,
			pos.y-hgBar:getContentSize().height))
			_SpriteLayer:addChild(hgBar,0)
		end
	end

	local cardSize=CCSprite:create(P("common/list_1032.png"))
	---己方先攻
	local userItem,itemSize = creatPriorityItem(_AllFightInfoTable.UserTalPriority)
	userItem:setAnchorPoint(PT(0,0))
	local pos = getProsPosition(1, true)
	userItem:setPosition(PT(0, pos.y+cardSize:getContentSize().height+itemSize.height*0.2))
	_SpriteLayer:addChild(userItem,0)
	
	
	--敌方先攻
	local npcPriority = "???" 
	if isNotNpc then
		npcPriority = _AllFightInfoTable.NpcTalPriority
	end
	local npcItem,itemSize = creatPriorityItem(npcPriority)
	npcItem:setAnchorPoint(PT(0,0))
	local pos = getProsPosition(1, false)	
	npcItem:setPosition(PT(0, pos.y-itemSize.height*1.2))
	_SpriteLayer:addChild(npcItem,0)	

	delayExec(PlotFightLayer.fightEngine, 0.5)
end;

--创建先攻值
function creatPriorityItem(num)
	local layer = CCLayer:create()
	local sprite = CCSprite:create(P("battle/list_1136.png"))
	sprite:setAnchorPoint(PT(0,0))
	sprite:setPosition(PT(0,0))
	layer:addChild(sprite, 0)
	local numStr = ""
	if num then
		numStr = num
	end
	local numLabel = CCLabelTTF:create(numStr, FONT_NAME, FONT_SM_SIZE)
	numLabel:setAnchorPoint(PT(0,0.5))
	numLabel:setPosition(PT(sprite:getContentSize().width, sprite:getContentSize().height*0.5))
	layer:addChild(numLabel, 0)
	
	layerSize = SZ(sprite:getContentSize().width+numLabel:getContentSize().width, sprite:getContentSize().height)
	
	return layer,layerSize
end

--创建血条
function  createHpLabel(Hp,maxHp)
	local scale=Hp/maxHp
	if scale>1 then
		scale=1
	elseif scale>0 and scale < 0.05 then
		scale = 0.05 
	end
	local longLenth=hpBar_LongLenth*Hp/maxHp
	local sprite=CCSprite:create(P("common/list_5003.9.png"))
	sprite:setAnchorPoint(PT(0,0))
	sprite:setScaleX(longLenth/sprite:getContentSize().width)
	return sprite
end;
	
--获取精灵的位置
function getProsPosition(location, bEnd)
	local startX=0
	local startY=SY(3)	

	startX = pWinSize.width*0.1+pWinSize.width*0.3*((location-1)%3)
	
	if bEnd then---   true  or false
		startY=pWinSize.height*0.25-math.floor((location-1)/3)*pWinSize.height*0.18
	else
		startY=pWinSize.height*0.615+math.floor((location-1)/3)*pWinSize.height*0.18
	end
	return PT(startX,startY)
end

--初始特效显示
function showFirstEffect()
---[[
	_showFirstEffectNum = 0
	_fristEffectNum = 0
	
	for k, v in pairs(_AllFightInfoTable.FirstEffectTabel) do
		_fristEffectNum = _fristEffectNum+1
	end

	_AllFightInfoTable.FirstEffectTabel = resetData(_AllFightInfoTable.FirstEffectTabel)


	_FirstEffectTabel = {}
	if _fristEffectNum > 0 then
		_isFirstShow = true
		for k, v in pairs(_AllFightInfoTable.FirstEffectTabel) do
			for n,m in ipairs(v.RecordTabel) do
				local actionLayer = CCLayer:create()
				actionLayer:setTag(#_FirstEffectTabel+1)
				_SpriteLayer:addChild(actionLayer, 0)
				m.lalyer = actionLayer
				
				_FirstEffectTabel[#_FirstEffectTabel+1]=m	
				
				local delayNum = 0.01+(n-1)*0.99
				delayExec(PlotFightLayer.showSingleFirstEffect, delayNum, actionLayer)
				
			end	
		end	
	else
		fightEngine()
	end
	
	
--]]
--fightEngine()
end

function resetData(info)
	local mRecordTabel = {}
	for k,v in ipairs(info) do
		if v.RecordTabel == nil then
			v.RecordTabel = {}
			v.RecordTabel[1]  = v
		end
	end
	
	
	for k,v in ipairs(info) do
		local isHave = true
		for m,n in ipairs(mRecordTabel) do
			if v.Position == n.Position and v.Role == n.Role then
				n.RecordTabel[#n.RecordTabel+1] = v
				isHave = false
			end
		end
		
		if isHave then
			mRecordTabel[#mRecordTabel+1] = v
		end
	end
	return mRecordTabel
end




function showSingleFirstEffect(pNode, index)
	local tag = nil
	if index then
		tag = index
	else
		tag = pNode:getTag()
	end
	v = _FirstEffectTabel[tag]
	local sprite,info = getSpirte(v.Role ,v.Position)
	local strImg = string.format("skilltitle/%s.png", v.FntHeadID)
	showContent(nil,sprite, strImg)
	local attribuAni=ScutAnimation.CScutAnimationManager:GetInstance():LoadSprite(v.EffectID1)
	attribuAni:registerFrameCallback("PlotFightLayer.finishAnimation")
--	attribuAni:setAnchorPoint(PT(0.5,0.5))
	local defPos = sprite:getPosition()
	attribuAni:setPosition(PT(defPos.x+sprite:getContentSize().width*0.5, defPos.y))	
	_SpriteLayer:addChild(attribuAni, 1)
	attribuAni:play()
	
end


--动画不移动
---[[
--动画播放完成后 
function finishAnimation(pSprite, curAniIndex, curFrameIndex, nPlayingFlag)
	if nPlayingFlag == 2 then
		pSprite:registerFrameCallback("")
		delayRemove(pSprite)
	end
end

function delayRemove(sprite)
	if sprite ~= nil then
		local delayAct = CCDelayTime:create(0.1)
		local funcName = CCCallFuncN:create(PlotFightLayer.removeTmpSprite)
		local act = CCSequence:createWithTwoActions(delayAct,funcName)
		sprite:runAction(act)
		sprite = nil
	end
	_showFirstEffectNum=_showFirstEffectNum+1
	if _showFirstEffectNum >= _fristEffectNum then
		_FirstEffectTabel=nil
		fightEngine()
	end
end

--魂技动画播放完成
function removeTmpSprite(sprite)
	local tag = sprite:getTag()
	if sprite ~= nil then
		sprite:removeFromParentAndCleanup(true)
		sprite = nil
	end
--	local currentFightSprite=_fighterInfo.sprite
--	if _CurrentAroundInfo.MomentumNum>=100 then
--		currentFightSprite:MomentumMax()
--	else
--		currentFightSprite:MomentumNotMax()
--	end
--		
--  	DefInjury()
end
--]]




--进入下一回合判断
function  playNextRound()

	if _fightState == EnumFightState.EFightEnd then
		return false
	end
	_FightAroundIndex=_FightAroundIndex+1
	_CurrentAroundInfo=_FightInfo[_FightAroundIndex]
	if _FightInfo and _FightAroundIndex > #_FightInfo then
		--下一流程
		fightEngine()
	else
		--播放当前回合动画
		playCurRoundAnimate()
	end;
end;




--攻击流程
EnumAttackProcess = {
	eAttackBegin 		= 0,--攻击回合开始
	eShowEffect			= 1,--显示debuff效果
	eTriggerEffect		= 2,--出发技能
	eMoveToRival 		= 3,--移动
	eAttackAnimate 		= 4,--攻击动画
	eDefTrigger			= 5,--防守方出发技能
	eAttackBackMove	= 6,--反击移动
	eAttackBackAnimate	= 7,--反击动画
	eAttackBackReplace	= 8,	--死于反击  替补佣兵上场
	eAttackEnd			= 9,--攻击回合结束
}


local fightProcessDefend={}
local fightProcessFighter={}



---播放当前回合动画
function playCurRoundAnimate()
	initCurRoundData()
	normalAttackProcess()
end;

----初始化战斗数据
function initCurRoundData()
	_fighterInfo={}
	_defendInfo={}
	
	effectTimes=0--效果播放次数
	_curState=0--战斗回合阶段
	
	
	---状态为一回合战斗的开始
	_curState=EnumAttackProcess.eAttackBegin
	
	---获取攻击者 精灵 和信息
	local sprite, info =getSpirte(_CurrentAroundInfo.Role,_CurrentAroundInfo.Position)
	_fighterInfo = info

	---判断防守方是否有数据 没有数据将跳过回合
	if _CurrentAroundInfo.DefendFightTable~=nil and #_CurrentAroundInfo.DefendFightTable >0  then
		--	1	单体	 --2	横向攻击3	纵向攻击 4	全体攻击
		---防守方可能是多个 ，做个数组存起来
		for k, v in pairs(_CurrentAroundInfo.DefendFightTable) do
			local sprite, info=getSpirte(v.Role, v.Position)
			if sprite and info then
				local num = #_defendInfo+1 			
				_defendInfo[num]={}
				_defendInfo[num].spriteInfo = info
				_defendInfo[num].defInfo = v
			end
		end
	else
		--没有debuff则回合直接结束
		if _CurrentAroundInfo.GeneralEffects~=nil and #_CurrentAroundInfo.GeneralEffects>0 then	
		else
			_curState= EnumAttackProcess.eAttackEnd
		end
	end	
end;

--攻击阶段流程
function normalAttackProcess()
	if stopFightTag then
		return
	end
	_curState = _curState + 1 ---进入下一状态
	if _curState == EnumAttackProcess.eShowEffect then--显示debuff效果---------------判断 效果 如中毒等 状态
		local currentSprite=_fighterInfo.sprite
		local currentFightInfo=_fighterInfo
		_hpNum = 0--扣血的特效个数
		_isSHowHpNum = 0--已显示个数
		for k, v in pairs(_CurrentAroundInfo.GeneralEffects) do
			if v.ConDamageNum and v.ConDamageNum > 0 then
				_hpNum = _hpNum+1
			end
		end
		
		local delayNum = 0
		currentSprite:releaseEffectImage()
		if _CurrentAroundInfo.GeneralEffects and #_CurrentAroundInfo.GeneralEffects>0 then
			for k, v in pairs(_CurrentAroundInfo.GeneralEffects) do
				UpAttribute(v.GeneralEffect,v.IsIncrease,currentSprite)
				currentSprite:addEffectImage(v.GeneralEffect)
				if v.ConDamageNum and v.ConDamageNum > 0 then
					delayNum = delayNum+1
					local fun = "PlotFightLayer.afterShowAttribute"
					showHurtHp(v.ConDamageNum, currentFightInfo, currentSprite, fun, nil, delayNum)		
				end
			end
		end
		if _hpNum > 0 then
			
		else
			delayExec(PlotFightLayer.normalAttackProcess, 0.1)
		end
	elseif _curState == EnumAttackProcess.eTriggerEffect then--攻击方触发技能
		delayExec(PlotFightLayer.normalAttackProcess, 0.1)
   	elseif _curState == EnumAttackProcess.eMoveToRival then--佣兵攻击动做
   		--是否有被攻击方， 有则 播放攻击动作   
   		if #_defendInfo > 0 then
   			_fighterInfo.sprite:attack()
   			delayExec(PlotFightLayer.normalAttackProcess, 0.1)
   		else
			--如果没有攻击目标直接进入下一回合
			delayExec(PlotFightLayer.playNextRound, 0.1)
   		end
	elseif _curState == EnumAttackProcess.eAttackAnimate then--攻击动画
		if #_defendInfo > 0 then
			---播放攻击动画
		   	playNormalAttackAnimate()
		else
			--如果没有攻击目标直接进入下一回合
			delayExec(PlotFightLayer.playNextRound, 0.1)
		end			
	elseif _curState == EnumAttackProcess.eDefTrigger then--被攻击方触发
		delayExec(PlotFightLayer.normalAttackProcess, 0.01)
	elseif _curState == EnumAttackProcess.eAttackBackMove then--被攻击方反击
		local fangJiAction= fightBackAction()--反击动作，提示问题文字

		if not fangJiAction  then
			_curState = EnumAttackProcess.eAttackEnd	
			delayExec(PlotFightLayer.normalAttackProcess, 0.01)
		end
	elseif _curState == EnumAttackProcess.eAttackBackAnimate then----反击动画
		_curState = EnumAttackProcess.eAttackEnd
		delayExec(PlotFightLayer.normalAttackProcess, 0.01)
	elseif  _curState == EnumAttackProcess.eAttackBackReplace then--被反击方 替换佣兵
		delayExec(PlotFightLayer.normalAttackProcess, 0.1)		
	elseif _curState == EnumAttackProcess.eAttackEnd then--战斗结束
		delayExec(PlotFightLayer.playNextRound, 0.1)
	else
		delayExec(PlotFightLayer.playNextRound, 0.1)
	end
end

function UpAttribute(state,upOrdown,sprite)

--[[2物理攻击    3魂技攻击     4魔法攻击     5物理防御     6魂技防御     7魔法防御
8暴击     9命中    10破击    11韧性    12闪避      13格挡      14必杀       22气势
--]]
	local attributeTable={[2]="icon_7220",[3]="icon_7219",[4]="icon_7222",
	[5]="icon_7214",[6]="icon_7221",[7]="icon_7223",[8]="icon_7213",[9]="icon_7211",
--		[10]="icon_7220",[11]="icon_7220",--10破击    11韧性没有
	[12]="icon_7212",[13]="icon_7210",
	--	[14]="icon_7210",-- 14必杀没有
	[22]="icon_7224"
	}
	---1下降  2 上升
	local stateTable={"battle/icon_7209.png","battle/icon_7208.png"}
	if not attributeTable[state] or not stateTable[upOrdown+1] then
		return
	end
	
	local attribuImg = string.format("battle/%s.png", attributeTable[state])
	local attribuAni = CCSprite:create(P(attribuImg))
	attribuAni:setAnchorPoint(PT(0,0))
	attribuAni:setPosition(PT(sprite:getPosition().x+sprite:getContentSize().width*0.5,sprite:getPosition().y+sprite:getContentSize().height*0.6))
	sprite:getParent():addChild(attribuAni, 0)
	

	local stateAni= CCSprite:create(P(stateTable[upOrdown+1]))
	stateAni:setAnchorPoint(PT(0,0))
	local position = PT(attribuAni:getContentSize().width*1.1,-stateAni:getContentSize().height*0.3)
	stateAni:setPosition(position)
	attribuAni:addChild(stateAni,0)
	
	position.y = attribuAni:getContentSize().height*0.5
	
	local action1 = CCMoveTo:create(0.5, position)
	local action2 = CCFadeOut:create(0.5)--淡出
	local funcName = CCCallFuncN:create(PlotFightLayer.UpAttributeOver)
	local action3 = CCSequence:createWithTwoActions(action2,funcName)
	local actionHarm=CCSequence:createWithTwoActions(action1,action3)
	stateAni:runAction(actionHarm)		

end

function UpAttributeOver(stateAni)
	if stateAni then
		attribuAni = stateAni:getParent()		
		stateAni:getParent():removeChild(stateAni, true)
		stateAni = nil	
		attribuAni:getParent():removeChild(attribuAni, true)
		attribuAni = nil
	end
	if _isFirstShow then
		_showFirstEffectNum = _showFirstEffectNum+1
		if _fristEffectNum >= _showFirstEffectNum then
			_isFirstShow = false
			fightEngine()
		end
	end
end

--效果扣血后 死亡判断
function afterShowAttribute(pNode)
	if pNode then
		pNode:getParent():removeChild(pNode, true)
		pNode = nil
	end
	_isSHowHpNum = _isSHowHpNum+1
	if _isSHowHpNum >=_hpNum then
		if _fighterInfo.LiveNum <= 0 then
			_fighterInfo.sprite:setVisible(false)
			local pos = _fighterInfo.sprite:getPosition()
			
			local tombstoneImg = CCSprite:create(P("battle/list_1163.png"))
			tombstoneImg:setAnchorPoint(PT(0,0))
			tombstoneImg:setPosition(pos)
			_layer:addChild(tombstoneImg,0)		
		end
		delayExec(PlotFightLayer.normalAttackProcess, 0.1)
	end
end

function nextAction()
	if _curState > 0 and _curState < 9 then
		delayExec(PlotFightLayer.normalAttackProcess, 0.1)
	else
		delayExec(PlotFightLayer.playNextRound, 0.1)
	end
end;

-----------------------
--攻击
function attackMove(sprite)
	sprite:attack()	
end;

-------------------

-----播放攻击动画
function playNormalAttackAnimate()
	local currentFightSprite=_fighterInfo.sprite
	local pos=PT(currentFightSprite:getPosition().x+currentFightSprite:getContentSize().width/2,currentFightSprite:getPosition().y+currentFightSprite:getContentSize().height/2)
	_CurrentAroundInfo.MomentumNum=_CurrentAroundInfo.AttGeneralQishi
	defendCount = 0
		local skillSpr = "skill_1000"--普通攻击
		if _CurrentAroundInfo.AttEffectID then--魂技攻击
			skillSpr = _CurrentAroundInfo.AttEffectID	
			--技能名称图片
			local strImg = string.format("skilltitle/%s.png", _CurrentAroundInfo.FntHeadID)
			showContent(nil,currentFightSprite, strImg)
		end
	
		local tag=1
		if _defendInfo and  #_defendInfo>1 then
			tag=2
		end
		local defSprite = _defendInfo[tag].spriteInfo.sprite
		local defPos=PT(defSprite:getPosition().x+defSprite:getContentSize().width/2,defSprite:getPosition().y)		

		--创建动画
		local skillSprite=Human:new()		
		skillSprite:createNpc(_SpriteLayer,skillSpr,nil,nil,pos.x,pos.y, 1)
		if _fighterInfo.Role == 1 then
			skillSprite:setScaleY(-1)
			skillSprite:setAnchorPoint(0, 1)
		end
		_skillSprite = skillSprite		
		--动画移动
		skillMove(skillSprite, defSprite)

end;

--魂技名称显示
function  showContent(str,sprite,pic)
	if sprite~=nil and (str or pic) then
		local label = nil
		if str~=nil then
			label=CCLabelTTF:create(str,FONT_NAME,FONT_SM_SIZE)
			label:setColor(ZyColor:colorYellow())
		else
			label = CCSprite:create(P(pic))
		end
		label:setAnchorPoint(PT(0.5,0))
		label:setOpacity(255)
		_SpriteLayer:addChild(label,sprite:getZOrder()+1)
		local position = CCPoint(sprite:getPosition().x+sprite:getContentSize().width*0.5, sprite:getPosition().y+sprite:getContentSize().height*0.05)
		
		label:setPosition(position)
		label:setVisible(true)
		position.y = position.y + sprite:getContentSize().height*0.8
		local action1 = CCMoveTo:create(0.5, position)
		local action3 = CCFadeOut:create(1)
		local funcName = CCCallFuncN:create(PlotFightLayer.finishLabelShow)
		local action3 = CCSequence:createWithTwoActions(action3,funcName)
		local actionHarm=CCSequence:createWithTwoActions(action1,action3)
		label:runAction(actionHarm)
	end
end;

--魂技名称显示结束
function  finishLabelShow(sprite)
    if sprite ~= nil then
        sprite:removeFromParentAndCleanup(true)
        sprite = nil
    end
end;


--动画移动

--攻击移动
function skillMove(skillSprite, defSprite)	

	local currentDefSprite=defSprite
	local pos=currentDefSprite:getPosition()

--		---判断是从哪里过去

	pos.x = pos.x+currentDefSprite:getContentSize().width/2
	pos.y = pos.y
	if _fighterInfo.Role == 1 then
		pos.y = pos.y+currentDefSprite:getContentSize().height/2
	end
	
	local oldPos=_fighterInfo.sprite:getPosition()
	
	--
	local time=math.abs(pos.y-oldPos.y)/fightSpeed
   	local actionBy = CCMoveTo:create(time, pos)
	local funcName = CCCallFuncN:create(PlotFightLayer.skillMoveEnd)
	local act = CCSequence:createWithTwoActions(actionBy,funcName)
-----	设置精灵的显示层级
	skillSprite:runAction(act)

	
end;

--攻击移动结束
function skillMoveEnd()

	_skillSprite:remove()

	local currentFightSprite=_fighterInfo.sprite
	if _CurrentAroundInfo.MomentumNum>=100 then
		currentFightSprite:MomentumMax()
	else
		currentFightSprite:MomentumNotMax()
	end
		
  	DefInjury()
  	
end;

---防守方受伤
function DefInjury()
	--防守方被攻击信息

	for k,v in ipairs(_defendInfo) do
		local defInfo= v.defInfo
		local spriteInfo = v.spriteInfo
		local sprite = spriteInfo.sprite	
		
		if defInfo.IsShanBi == 1 then--是否闪避
			local func = PlotFightLayer.afterDefend
			sprite:dodge(func)
			showContent(nil,sprite, "battle/icon_7212.png")
		else
			if defInfo.IsGeDang==1 then--是否格挡
				showContent(nil,sprite, "battle/icon_7210.png")
			elseif defInfo.IsBaoji==1  then--是否暴击
				showContent(nil,sprite, "battle/icon_7213.png")
			end
			
			local LiveNum = defInfo.TargetGeneralLiveNum
			local hurtNum = defInfo.TargetDamageNum
			local labelHarm = defInfo._labelHarm
			


			local funcName =PlotFightLayer.JudgeDefendIfDead
			local tag = k			
	
			if hurtNum and  hurtNum ~= 0 then
				showHurtHp(hurtNum, spriteInfo, sprite, funcName, tag)
			else
				JudgeDefendIfDead(nil, tag)
			end

	--		--debuff
			if defInfo.GeneralEffects and #defInfo.GeneralEffects >0 then
				sprite:releaseEffectImage()
				for k,v in ipairs (defInfo.GeneralEffects) do
					sprite:addEffectImage(v.GeneralEffect)
					UpAttribute(v.GeneralEffect,v.IsIncrease,sprite)
				end
			end
		end

	end
end

--扣血效果
function showHurtHp(hurtNum, currentInfo, sprite, fun, tag, delayNum)

	currentInfo.LiveNum=currentInfo.LiveNum-hurtNum
	if currentInfo.LiveNum> currentInfo.LiveMaxNum then
		currentInfo.LiveNum=currentInfo.LiveMaxNum
	end
	if currentInfo.LiveNum<0 then
	     currentInfo.LiveNum=0
	end
	
	local labelHarm = nil--加血，扣血 文字
	if hurtNum >= 0 then
		labelHarm = getNumberSprite(hurtNum, 1)
	else
		labelHarm = getNumberSprite(hurtNum, 2)
	end
	local position = CCPoint(sprite:getPosition().x, sprite:getPosition().y+sprite:getContentSize().height*0.5)
	labelHarm:setPosition(position)
	labelHarm:setVisible(true)
	_SpriteLayer:addChild(labelHarm,sprite:getZOrder()+1)
	
	
	if tag then
		labelHarm:setTag(tag)
	end
	sprite:defend()
	position.y =sprite:getPosition().y+sprite:getContentSize().height*0.8
	local action1 = CCMoveTo:create(0.35, position)
	local action2 = CCFadeOut:create(0.1)--淡出
	local funcName = CCCallFuncN:create(fun)
	local action3 = CCSequence:createWithTwoActions(action2,funcName)
	local actionHarm=CCSequence:createWithTwoActions(action1,action3)
	
	if delayNum == nil then
		delayNum = 0
	end
	local delayTime = 0.01+delayNum*0.2
	
	local action = CCSequence:createWithTwoActions(CCFadeOut:create(delayTime), actionHarm)
	labelHarm:runAction(action)	

	local hgBar = currentInfo.hgBar
	if hgBar then		
		local nScale = currentInfo.LiveNum/currentInfo.LiveMaxNum
		if nScale > 0 then
			if nScale>0 and nScale < 0.05 then--值太小了 缩放会不行
				nScale = 0.05
			elseif nScale > 1 then
				nScale = 1
			end
			hgBar:setScaleX(nScale*hpBar_LongLenth/hgBar:getContentSize().width)
		else
			hgBar:setVisible(false)
		end
	end
end


--判断被攻击方是否死亡
function JudgeDefendIfDead(pNode, index)
	local tag = nil
	if index then
		tag = index
	else
		tag = pNode:getTag()
	end
	if tag then
		if _defendInfo[tag].defInfo.TargetGeneralLiveNum <= 0 then
			_defendInfo[tag].spriteInfo.sprite:setVisible(false)
			local pos = _defendInfo[tag].spriteInfo.sprite:getPosition()
			
			local tombstoneImg = CCSprite:create(P("battle/list_1163.png"))
			tombstoneImg:setAnchorPoint(PT(0,0))
			tombstoneImg:setPosition(pos)
			_layer:addChild(tombstoneImg,0)	
			
		end	
		
		if _defendInfo[tag].defInfo.TargetGeneralQishi >= 100 then
			_defendInfo[tag].spriteInfo.sprite:MomentumMax()
		else
			_defendInfo[tag].spriteInfo.sprite:MomentumNotMax()		
		end
	end
	
	
	if pNode then
		pNode:getParent():removeChild(pNode, true)
		pNode = nil
	end
	afterDefend()
end


--被攻击之后  判断是否进入下一阶段
function afterDefend()
	defendCount = defendCount+1
	
	--所有被攻击玩家播放完成
	if defendCount >= #_defendInfo then
		delayExec(PlotFightLayer.normalAttackProcess, 0.1)
	end
end


---延迟进行方法
function delayExec(funName,nDuration,parent)
	local  action = CCSequence:createWithTwoActions(
	CCDelayTime:create(nDuration),
	CCCallFuncN:create(funName));
	local layer = _layer
	if parent then
		layer=parent
	end
	if layer then
		layer:runAction(action)
	end	
end

-----获取精灵
function  getSpirte(role,position)
	if role ~=nil then 
		local generalInfo=_FiguresGeneralInfo
		if role==1 then
			generalInfo=_EnemyGeneralInfo
		end
		for k, v in ipairs(generalInfo) do
			if v and v.AttPosition==position and v.LiveNum > 0 then
				return v.sprite,v
			end
		end
	end
	return false
end;


--战斗结束
function fightOver()
	if _battleType == nil then
		battleScene.roundOver(_AllFightInfoTable)
	elseif _battleType == 1 or _battleType == 2  or _battleType == 4  or _battleType==7 then--竞技场
		CompetitiveBattle.battleOver(_AllFightInfoTable)
	elseif _battleType == 3 then--世界boss
		WorldBossToScene.battleOver(_AllFightInfoTable)
	elseif _battleType == 5 then--圣吉塔
		SbattleScene.roundOver(_AllFightInfoTable)
	elseif _battleType == 6 then--考古
		RbattleScene.roundOver(_AllFightInfoTable)
	end
end

--反击动作播放
function fightBackAction()
	local fangJiAction = false
	local defSprite = _fighterInfo.sprite
	
	_fightBackSkillSprite = {}
	_attackBackNum = 0
	for k, v in pairs(_defendInfo) do
		if v.defInfo.IsFangji==1 then
			fangJiAction=true
			_attackBackNum=_attackBackNum+1	
			
			
			local sprite=v.spriteInfo.sprite
			sprite:attack()
			showContent(nil,sprite, "battle/icon_7225.png")
		
			local pos = sprite:getPosition()
			
				--创建动画
			local skillSpr = "skill_1000"--普通攻击
			local skillSprite=Human:new()		
			skillSprite:createNpc(_SpriteLayer,skillSpr,nil,nil,pos.x,pos.y, 1)
			if v.spriteInfo.Role == 1 then
				skillSprite:setScaleY(-1)
				skillSprite:setAnchorPoint(0, 1)
			end
			skillSprite:setSpriteTag(k)
			
			_fightBackSkillSprite[k] = skillSprite
		
			--动画移动
			AttackBackMove(skillSprite, defSprite)
		
		end
	end
	
	return fangJiAction
end

--反击动画移动
function AttackBackMove(skillSprite, defSprite)	

	local currentDefSprite=defSprite
	local pos=currentDefSprite:getPosition()

--		---判断是从哪里过去

	pos.x = pos.x+currentDefSprite:getContentSize().width/2
	pos.y = pos.y
--	if _fighterInfo.Role == 0 then
--		pos.y = pos.y
--	end
	
	local oldPos=skillSprite:getPosition()
	
	--
	local time=math.abs(pos.y-oldPos.y)/fightSpeed
   	local actionBy = CCMoveTo:create(time, pos)
	local funcName = CCCallFuncN:create(PlotFightLayer.endForAttackBack)
	local act = CCSequence:createWithTwoActions(actionBy,funcName)
-----	设置精灵的显示层级
	skillSprite:runAction(act)

end

function endForAttackBack(pNode)
	local tag = pNode:getTag()

	_fightBackSkillSprite[tag]:remove()
		
	local hurtNum = _defendInfo[tag].defInfo.FangjiDamageNum
	showHurtHp(hurtNum, _fighterInfo, _fighterInfo.sprite, PlotFightLayer.judgeFightBackDeath, tag, delayNum)
end

--反击死亡判断
function judgeFightBackDeath(pNode)
	if pNode then
		pNode:getParent():removeChild(pNode, true)
		pNode = nil
	end
	_attackBackNum = _attackBackNum-1
	if  _attackBackNum <= 0 then
		if _fighterInfo.LiveNum <= 0 then
			_fighterInfo.sprite:setVisible(false)
			local pos = _fighterInfo.sprite:getPosition()
			
			local tombstoneImg = CCSprite:create(P("battle/list_1163.png"))
			tombstoneImg:setAnchorPoint(PT(0,0))
			tombstoneImg:setPosition(pos)
			_layer:addChild(tombstoneImg,0)		
		end
		delayExec(PlotFightLayer.normalAttackProcess, 0.01)
	end
end

function judgeFangji()
	local isFangji = false
	for k,v in ipairs(_AllFightInfoTable.FightProcessTable) do
		for n,m in ipairs(v.DefendFightTable) do
			if m.IsFangji == 1 then
				isFangji = true
			end
		end
	end
	
	if isFangji then
		ZyToast.show(_layer, "fangji", 1, 0.5)
	else
		ZyToast.show(_layer, "no fangji", 1, 0.5)
	end
end
