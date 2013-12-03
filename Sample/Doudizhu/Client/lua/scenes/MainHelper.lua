------------------------------------------------------------------
--MainHelper.lua
-- Author     : JMChen
-- Version    : 1.0.0.0
-- Date       : 2011-10-15
-- DescriCCPointion:
------------------------------------------------------------------


module("MainHelper", package.seeall)

local mTopLayer=nil
local mBttomLayer=nil
local isMove=nil
local isMoving=nil
--

local sendIndex=nil
local mSendLayer=nil
local mSendInfo=nil
local sendTable={}
local mGoldLabel=nil
--2 左 3 右
local cardNumTable={}
local mLandLordLayer=nil
local oldBeishu=nil
local mSetState={}
function  creaetPalyerHead(type,info,posY)
	local layer=CCLayer:create()
	local path=nil
	if info.HeadIcon then
	path=string.format("headImg/%s.png",info.HeadIcon )
	end
	local headSprite=createHeadSprite(path)
	layer:addChild(headSprite,1)
	
	local numSprite=CCSprite:create(P("common/panle_1021_2.png"))
	numSprite:setAnchorPoint(CCPoint(0,0))
	layer:addChild(numSprite,1)
	
	local numLabel=CCLabelTTF:create(0,FONT_NAME,FONT_FMM_SIZE)
	numLabel:setAnchorPoint(CCPoint(0.5,0.5))
	numLabel:setPosition(CCPoint(numSprite:getContentSize().width/2,numSprite:getContentSize().height/2))
	numSprite:addChild(numLabel,0)
	if info.NickName  then
		local nameLabel=CCLabelTTF:create(info.NickName,FONT_NAME,FONT_FMM_SIZE)
		nameLabel:setAnchorPoint(CCPoint(0.5,0))
		nameLabel:setPosition(CCPoint(headSprite:getContentSize().width/2,0))
		headSprite:addChild(nameLabel,0)
	end
	
	local pos=CCPoint(0,posY)
	local numPosY=posY
	local numPos=CCPoint(headSprite:getContentSize().width,numPosY)
	cardNumTable[3]=numLabel
	if type==2 then
		pos=CCPoint(pWinSize.width-headSprite:getContentSize().width,posY)
	       numPos=CCPoint(pos.x-numSprite:getContentSize().width,
	       				numPosY)
	       cardNumTable[2]=numLabel
	end
	info.numLabel=numLabel
	headSprite:setPosition(pos)
	numSprite:setPosition(numPos)
	return layer,numLabel,headSprite
end;


function createHeadSprite(path)
	local bgSprite=CCSprite:create(P("common/panel_1001_6.png"))
	bgSprite:setAnchorPoint(CCPoint(0,0))	
	---
	if path then
	local headSprite=CCSprite:create(P(path))
	headSprite:setAnchorPoint(CCPoint(0.5,0.5))
	headSprite:setPosition(CCPoint(bgSprite:getContentSize().width/2,bgSprite:getContentSize().height/2))
	bgSprite:addChild(headSprite,0)
	end
	return bgSprite
end;

function topLayerMove()
	if mTopLayer and not isMoving then
	local bgSprite=CCSprite:create(P("common/panel_1007_2.9.png"))
	isMove =not isMove
	local movePos=CCPoint(0,pWinSize.height-bgSprite:getContentSize().height*1.3)
	if isMove then
		movePos=CCPoint(0,pWinSize.height-bgSprite:getContentSize().height*0.4)
	end	
	local moveAct = CCMoveTo:create(0.3,movePos);
	moveAct=CCSequence:createWithTwoActions(moveAct, CCCallFuncN:create(moveEnd))
	mTopLayer:runAction(moveAct)
	isMoving=true
	end
end;


--创建单张
function createSigleCard(info,index,type,scale,cardBack)	
	local cardInfo=CardConfig.getCardInfoByid(info.CardId)
	if cardInfo then
	local imgPath=string.format("card/%s.png",cardInfo.HeadIcon)
	if cardBack then
		imgPath="common/card_1055.png"
	end
	local sprite=CCSprite:create(P(imgPath))
	sprite:setAnchorPoint(CCPoint(0,0))
	if scale then
		sprite:setScale(scale)
	end

	--
	if not type then
--		local actionBtn=UIHelper.createActionRect(sprite:getContentSize(),"MainDesk.choiceCardAction",index)
--		actionBtn:setPosition(CCPoint(0,0))
--		sprite:addChild(actionBtn,0)
	end
	return sprite
	end
end;
--功能键
function buttonAction(node)
	local tag=node
	local mScene = CCDirector:sharedDirector():getRunningScene()
	--托管
	if tag==1 then
		if not MainDesk.getTuoGuanLayer() then
			MainDesk.setTuoGuanID(1)
			actionLayer.Action2011(mScene,nil,1)	
		else
			MainDesk.setTuoGuanID(2)
			actionLayer.Action2011(mScene,nil,2)	
		end
	elseif tag==2 then
		chatBtnAction()--聊天按钮响应事件
	elseif tag==3 then
       	local setLayer = SetScene.createScene()
		mScene:addChild(setLayer,1)
    elseif tag ==4 then
		local box = ZyMessageBoxEx:new()
		box:doQuery(mScene,nil,Language.IDS_EXITWARM,Language.IDS_SURE,Language.IDS_CANCEL,IsExit)
	end
end;

function relaeseBottomLayer()
	if mBttomLayer then
		mBttomLayer:getParent():removeChild(mBttomLayer,true)
		mBttomLayer=nil
	end
end;

--创建底下控件
function createBottomLayer(info,roomID)
	 relaeseBottomLayer()
	 local layer=CCLayer:create()
	 mBttomLayer=layer
	 --
	 --
	 local goldSprite=CCSprite:create(P("common/icon_1031.png"))
	 goldSprite:setAnchorPoint(CCPoint(0,0))
	 goldSprite:setPosition(CCPoint(pWinSize.width*0.8,0))
	 layer:addChild(goldSprite,0)
	 local goldLabel=CCLabelTTF:create(info._GameCoin,FONT_NAME,FONT_SM_SIZE)
	 goldLabel:setAnchorPoint(CCPoint(0,0.5))
	 goldLabel:setPosition(CCPoint(goldSprite:getPosition().x+goldSprite:getContentSize().width,
	 			goldSprite:getPosition().y+goldSprite:getContentSize().height/2))
	 layer:addChild(goldLabel,0)
	 mGoldLabel=goldLabel
	 		
	local roomInfo=nil
	for k ,v in pairs(info._roomTabel) do
		if v.RoomId ==roomID then
			roomInfo=v
			break
		end
	end
	local str=Language.IDS_BASE.. ":".. roomInfo.AnteNum 
	local numLabel=CCLabelTTF:create(str,FONT_NAME,FONT_SM_SIZE)
	numLabel:setAnchorPoint(CCPoint(0,0.5))
	numLabel:setPosition(CCPoint(pWinSize.width*0.5-numLabel:getContentSize().width-SX(2),
								goldLabel:getPosition().y))
	 layer:addChild(numLabel,0)
	
	local str=Language.IDS_BEISHU.. ":".. roomInfo.RoomMultiple  
	local beishuLabel=CCLabelTTF:create(str,FONT_NAME,FONT_SM_SIZE)
	beishuLabel:setAnchorPoint(CCPoint(0,0.5))
	beishuLabel:setPosition(CCPoint(pWinSize.width*0.5+SX(2),
								numLabel:getPosition().y))
	 layer:addChild(beishuLabel,0)	
	return layer,beishuLabel,roomInfo,roomInfo.RoomMultiple 
end;


function setGoldeNum(num)
		mGoldLabel:setString(num)		 
end;


function releaseTopLayer()
	if mTopLayer then
		mTopLayer:getParent():removeChild(mTopLayer,true)
		mTopLayer=nil
	end
		isMove=nil
		isMoving=nil
end;



function  createTopMenu(mLayer)
	local layer =CCLayer:create()
	mTopLayer=layer
	mLayer:addChild(layer,3)

	local bgSprite=CCSprite:create(P("common/panel_1007_2.9.png"))
	local btnSprite=CCSprite:create(P("common/panel_1007_1.png"))
	
	local boxSize=CCSize(pWinSize.width*0.5,bgSprite:getContentSize().height)
	bgSprite:setScaleX(boxSize.width/bgSprite:getContentSize().width)
	bgSprite:setAnchorPoint(CCPoint(0.5,0))
	bgSprite:setPosition(CCPoint(pWinSize.width/2,btnSprite:getContentSize().height*0.8))
	layer:addChild(bgSprite,0)
	layer:setPosition(CCPoint(0,pWinSize.height-bgSprite:getContentSize().height*1.3))
	btnSprite:setAnchorPoint(CCPoint(0.5,0))
	btnSprite:setPosition(CCPoint(pWinSize.width/2,0))
	layer:addChild(btnSprite,0)
	
	local btnTable={{image="icon_1022",},{image="icon_1024",},{image="icon_1025",},{image="icon_1026",},
	}
	local colW=boxSize.width/(#btnTable+1)
	--
	local startX=pWinSize.width/2-boxSize.width/2+colW
	local startY=bgSprite:getPosition().y+boxSize.height*0.5
	for k, v in pairs(btnTable) do
		local path=string.format("common/%s.png",v.image)
		local btn=ZyButton:new(path)
		btn:setTag(k)
		btn:registerScriptTapHandler(buttonAction)
		btn:setPosition(CCPoint(startX-btn:getContentSize().width/2+colW*(k-1),startY-btn:getContentSize().height/2))
		btn:addto(layer,0)
	end
	
	local actionSize=CCSize(pWinSize.height*0.1,pWinSize.height*0.1)
	local actionBtn=UIHelper.createActionRect(actionSize,topLayerMove)
	actionBtn:setPosition(CCPoint(pWinSize.width/2-actionSize.width/2,0))
	layer:addChild(actionBtn,0)
	return layer	
end;


function releaseReportLayer()
	if mReportLayer then
		mReportLayer:getParent():removeChild(mReportLayer,true)
		mReportLayer=nil
	end
end;

--续局
function againGame(node)
	releaseReportLayer()
	MainDesk.releaseClock()
	local mScene = CCDirector:sharedDirector():getRunningScene()
	MainDesk.releaseLandLordLayer()
	MainDesk.releaseMingLayer()
	MainDesk.releaseControlLayer()
	MainDesk.releaseLandLoarCard()		
	MainDesk.releaseOutLayer()
	MainDesk.releaseTuoGuan()
	MainDesk.releaseCardLayer()	
	local roomId=MainDesk.getRoomID()
	actionLayer.Action2001(mScene,nil,roomId,2)
end;

--离开
function leaveAction(node)
	local mScene = CCDirector:sharedDirector():getRunningScene()
	actionLayer.Action2002(mScene,nil)
end;

function pushLandloardEnd(info,type)
	MainDesk.releaseOutLayer()
	releaseReportLayer()
	local layer=CCLayer:create()
	mReportLayer=layer
	mScene=MainDesk.getLayer()
	mScene:addChild(layer,9)	
	
	--
	local pernalInfo=PersonalInfo.getPersonalInfo()
	local actionBtn=UIHelper.createActionRect(pWinSize)
	actionBtn:setPosition(CCPoint(0,0))
	layer:addChild(actionBtn,0)
	
	--
	local midBgSprite=CCSprite:create(P("common/panle_1015.9.png"))
	midBgSprite:setAnchorPoint(CCPoint(0.5,0.5))
	local boxSize=SZ(pWinSize.width*0.6,pWinSize.height*0.7)
	midBgSprite:setScaleX(boxSize.width/midBgSprite:getContentSize().width)
	midBgSprite:setScaleY(boxSize.height/midBgSprite:getContentSize().height)	
	midBgSprite:setPosition(CCPoint(pWinSize.width/2,pWinSize.height/2))
	layer:addChild(midBgSprite,0)
	--local boxSize=midBgSprite:getContentSize()
	
	--
	local closeBtn=ZyButton:new("button/panle_1014_3.png")
	closeBtn:setPosition(CCPoint(pWinSize.width/2+boxSize.width*0.44-closeBtn:getContentSize().width,
							pWinSize.height/2+boxSize.height*0.46-closeBtn:getContentSize().width))
--	closeBtn:addto(layer,0)
	closeBtn:registerScriptTapHandler(leaveAction)
	
	--
	local startX=pWinSize.width/2-boxSize.width*0.45
	local startY=pWinSize.height/2+boxSize.height*0.48
	local winTable={[0]="panle_1022",[1]="panle_1024"}
	local numType={[0]=2,[1]=1}	
	local winSprite=nil
	if not type then
	if info.IsLandlord ==0 then
		numType={[0]=1,[1]=2}
		winTable={[0]="panle_1025",[1]="panle_1023"}
	end

	winSprite=CCSprite:create(P(string.format("common/%s.png",winTable[info.IsLandlordWin])))
	winSprite:setAnchorPoint(CCPoint(0,0))
	winSprite:setPosition(CCPoint(startX,startY-winSprite:getContentSize().height))
	layer:addChild(winSprite,0)
	--输赢音效
	end
	--
	local headSprite=MainHelper.createHeadSprite(string.format("headImg/%s.png",pernalInfo._HeadIcon))
	headSprite:setAnchorPoint(CCPoint(0,0))
	local posY=pWinSize.height/2+boxSize.height*0.28-headSprite:getContentSize().height-SY(2)
	if winSprite then
		 posY=winSprite:getPosition().y-headSprite:getContentSize().height-SY(2)
	end	
	headSprite:setPosition(CCPoint(startX,posY))
	layer:addChild(headSprite,0)
	
	local plaeyrName=CCLabelTTF:create(pernalInfo._NickName,FONT_NAME,FONT_SM_SIZE)
	plaeyrName:setAnchorPoint(CCPoint(0,0))
	plaeyrName:setColor(ccc3(241,176,63))
	
	plaeyrName:setPosition(CCPoint(headSprite:getPosition().x+headSprite:getContentSize().width+SX(2),
					headSprite:getPosition().y+headSprite:getContentSize().height-plaeyrName:getContentSize().height))
	layer:addChild(plaeyrName,0)
	


	local numSprite=getNumberSprite(info.CoinNum,numType[info.IsLandlordWin])
	numSprite:setAnchorPoint(CCPoint(0,0))
	numSprite:setPosition(CCPoint(plaeyrName:getPosition().x,
					plaeyrName:getPosition().y-numSprite:getContentSize().height-plaeyrName:getContentSize().height/2))
	layer:addChild(numSprite,0)
	
	local str=Language.IDS_GAMECOIN_LEAST.. info.GameCoin	
	PersonalInfo.getPersonalInfo()._GameCoin=info.GameCoin
	
	local leasetNum=CCLabelTTF:create(str,FONT_NAME,FONT_SM_SIZE)
	leasetNum:setAnchorPoint(CCPoint(0,1))
	leasetNum:setColor(ccc3(241,176,63))
	leasetNum:setPosition(CCPoint(numSprite:getPosition().x,
					numSprite:getPosition().y-numSprite:getContentSize().height*0.2))
	layer:addChild(leasetNum,0)
	if mGoldLabel then
		mGoldLabel:setString(info.GameCoin)
	end
	--
	
	local againBtn=ZyButton:new("button/panle_1026_2.png","button/panle_1026_3.png")
	againBtn:addImage("button/panle_1049.png")
	againBtn:addto(layer,0)
	againBtn:registerScriptTapHandler(againGame)						
	againBtn:setPosition(CCPoint(pWinSize.width/2+boxSize.width/4-againBtn:getContentSize().width/2,
							midBgSprite:getPosition().y-boxSize.height*0.45))
	--
	local leaveBtn=ZyButton:new("button/panle_1026_2.png","button/panle_1026_3.png")
	leaveBtn:addImage("button/panle_1044.png")
	leaveBtn:addto(layer,0)
	leaveBtn:registerScriptTapHandler(leaveAction)
	leaveBtn:setPosition(CCPoint( pWinSize.width/2-leaveBtn:getContentSize().width/2-boxSize.width/4,
							againBtn:getPosition().y))	
							
	MainDesk.setFirstCallLandlord(true)
end;

function IsExit(index, content, tag)
	local mScene = CCDirector:sharedDirector():getRunningScene()
	if index == 1 then
		MainDesk.setFirstCallLandlord(true)
		actionLayer.Action2002(mScene,nil)
	end
end;

--聊天按钮响应事件
function chatBtnAction()
	if not isClick then
	    isClick=true
        local mScene = CCDirector:sharedDirector():getRunningScene()
        ChatLayer.createScene(mScene)
    end
end

-------
function ChatState()
    isClick=false;
end


function moveEnd()
	isMoving=false
end;

function topMoveUp()
	if mTopLayer and not isMoving then
		if not isMove then
			isMove=true
			local bgSprite=CCSprite:create(P("common/panel_1007_2.9.png"))
			movePos=CCPoint(0,pWinSize.height-bgSprite:getContentSize().height*0.4)
			local moveAct = CCMoveTo:create(0.3,movePos);
			moveAct=CCSequence:createWithTwoActions(moveAct, CCCallFuncN:create(moveEnd))
			mTopLayer:runAction(moveAct)
			isMoving=true
		end
	end
end;



--发牌动作
function  sendCardAction(layer,info,singleW,startY)
	mSendInfo=info
	sendIndex=1
	mSendLayer=CCLayer:create()
	layer:addChild(mSendLayer,0)
	 refreshCrad()
end;

function  refreshCrad()
	mSendLayer:removeAllChildrenWithCleanup(true)
	local cardInfo=mSendInfo.PlayerCardTable
	for k=1,#cardInfo-sendIndex do
		local sprite=CCSprite:create(P("common/card_1055.png"))
		sprite:setAnchorPoint(CCPoint(0.5,0.5))
		sprite:setPosition(CCPoint(pWinSize.width/2,pWinSize.height*0.57-(k-1)*(sprite:getContentSize().height/108)))
		mSendLayer:addChild(sprite,0)
	end
	sendTable.shunxu=1
	runSendAction()
end;
function sendEnd(node)
	node:getParent():removeChild(node,true)
	if sendIndex<=#mSendInfo.PlayerCardTable then	
	if sendTable.shunxu==1 then
		 addSendPic()
		 
	else
		cardNumTable[sendTable.shunxu]:setString(sendIndex)
	end	
	if sendTable.shunxu>=3 then
		  sendIndex=sendIndex+1
		  refreshCrad()
	else
		sendTable.shunxu=sendTable.shunxu+1
		runSendAction()
	end
	--发牌结束
	else
		MainDesk.releaseSendLayer()
		MainDesk.setCardTable(mSendInfo.PlayerCardTable,mSendInfo.OpenTable)
		MainDesk.refreshCardLayer(1)
		MainDesk.createLandLordCard(mSendInfo.OpenTable,1)
		MainDesk.createClock()
		topMoveUp()
		MainDesk.callLandlord(mSendInfo.LandlordId ,mSendInfo.LandlordName )
	end
end;
--
function runSendAction(type)
	local sprite=CCSprite:create(P("common/card_1055.png"))
	sprite:setPosition(CCPoint(pWinSize.width/2,pWinSize.height/2))
	mSendLayer:addChild(sprite,0)
	local movePos=CCPoint(pWinSize.width*0.9,pWinSize.height*0.1)
	local speedLenth= pWinSize.width*5
	local speed=math.abs(movePos.y-sprite:getPosition().y)/speedLenth
	movePos.x=(pWinSize.width*0.9-sendIndex*(pWinSize.width/#mSendInfo.PlayerCardTable))
	if sendTable.shunxu==2 then
		movePos=CCPoint(pWinSize.width*0.9,pWinSize.height*0.5)
		speed=math.abs(movePos.x-sprite:getPosition().x)/speedLenth
	elseif sendTable.shunxu==3 then
		movePos=CCPoint(pWinSize.width*0.1,pWinSize.height*0.5)
		speed=math.abs(movePos.x-sprite:getPosition().x)/speedLenth
	end
	spriteMoveAction(sprite,movePos,speed,sendEnd)
end;

---
function  spriteMoveAction(sprite,movePos,speed,fun)
		--音效（发牌）
	local moveAct = CCMoveTo:create(speed,movePos);
	moveAct=CCSequence:createWithTwoActions(moveAct, CCCallFuncN:create(fun))
	sprite:runAction(moveAct)
end;

function addSendPic()
	local info=mSendInfo.PlayerCardTable[sendIndex]
	local sprite=createSigleCard(info,1,1)	
	local parent=mSendLayer:getParent()
	parent:addChild(sprite,(#mSendInfo.PlayerCardTable-sendIndex))
	local singleW=(pWinSize.width-sprite:getContentSize().width)/19
	local startX=pWinSize.width-sprite:getContentSize().width
	sprite:setPosition(CCPoint(startX-(sendIndex-1)*singleW,pWinSize.height*0.1))
end;

--


---延迟进行方法
function delayExec(funName,nDuration, parent)
	local  action = CCSequence:createWithTwoActions(
	CCDelayTime:create(nDuration),
	CCCallFuncN:create(funName));
	parent:runAction(action)
end

---
function showAnimation(path)
--[[
        local mScene = CCDirector:sharedDirector():getRunningScene()

	local spr=ScutAnimation.CScutAnimationManager:GetInstance():LoadSprite(path)
	spr:setCurAni(0)
	spr:play()
	mScene:addChild(spr,5)
	spr:setPosition(CCPoint(pWinSize.width/2,pWinSize.height*0.3))
	
	spr:registerFrameCallback("MainHelper.showEffecOver")
		]]
end;

--显示完技能特效之后
function showEffecOver(pSprite, curAniIndex, curFrameIndex, nPlayingFlag)
	if nPlayingFlag==2 then
		pSprite:registerFrameCallback("")
		local  action = CCSequence:actionOneTwo(
		    CCDelayTime:actionWithDuration(0.01),
		    CCCallFuncN:actionWithScriCCPointFuncName("MainHelper.removeSprite"));
		pSprite:runAction(action)
	end
end;

--移除动作精灵
function  removeSprite(node)
		node:removeFromParentAndCleanup(true)	
end;

--
function  showPngEffect(sprite,path,fun,index,parent)
	local actionSprite=CCSprite:create(P(path))
	parent:addChild(actionSprite,11)
	actionSprite:setAnchorPoint(CCPoint(0.5,0))
	if index then
		actionSprite:setTag(index)
	end
	actionSprite:setPosition(CCPoint(sprite:getPosition().x+sprite:getContentSize().width/2,sprite:getPosition().y))
	local movePos=CCPoint(actionSprite:getPosition().x,sprite:getPosition().y+sprite:getContentSize().height)
	local moveTime=0.5
	local moveto = CCMoveTo:actionWithDuration(moveTime, movePos);
	fun=fun or "MainHelper.removeSprite"
	local callfunc = CCCallFuncN:actionWithScriCCPointFuncName(fun);
	local sequence = CCSequence:actionOneTwo(moveto, callfunc);
	actionSprite:runAction(sequence)
end;




--
function  showFondEffect(pos,str,parent,time)
	local actionSprite=CCLabelTTF:create(str,FONT_NAME,FONT_SM_SIZE)
	parent:addChild(actionSprite,2)
	actionSprite:setAnchorPoint(CCPoint(0.5,0))
	actionSprite:setPosition(pos)
	
	
	local delayTime=time
	local action1 = CCSequence:actionOneTwo(CCFadeOut:actionWithDuration(delayTime),
			CCCallFuncN:actionWithScriCCPointFuncName("MainHelper.removeSprite"))

	actionSprite:runAction(action1)
end;


function play(mScene)
	--背景音乐
	mSetState.musicState2=tonumber(accountInfo.getConfig("sys/config.ini", "SETTING2", "musicState2"))
	if mScene~=nil then
		if mSetState.musicState2==1 then
			local index=EnumMusicType.bgMusic
			resetMusicInit()
		    	playMusic(index)
		end
	end
	--
end;



--判断是否有点击中牌组
function isTouchInCard(touchLocation,sprite,singleW)
	local pt= sprite:getParent():convertToNodeSpace(touchLocation)	
    	local 	Position =sprite:getPosition()
	local	ContentSize =sprite:getContentSize()
	local	AnchorPoint =sprite:getAnchorPoint()
	local rc=CCRectMake( Position.x,Position.y,singleW, 
						ContentSize.height);
	if CCRect.containsPoint(rc, pt) then
		return true
	end
end;

