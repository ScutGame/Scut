------------------------------------------------------------------
--MainDesk.lua
-- Author     : JMChen
-- Version    : 1.0.0.0
-- Date       : 2011-10-15
-- DescriCCPointion:
------------------------------------------------------------------


module("MainDesk", package.seeall)
require("scenes.MainHelper")
require("layers.AiLayer") 
-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

--
local mScene = nil 		-- 场景
local mLayer = nil 		-- 层
local mColW=nil
local mCardSize=nil
local mPersonalInfo=nil
local bottomHeight=nil
local mHeadSprite=nil

local isRun=nil
local isRun=nil
local mTimes=nil
local mRoomID=nil
local mSendServerData=nil
local mCardTable={}
local mPalyerTable={}
local mCurrentLandLord=nil
local mCurrentLandUserID=nil
local isFrstCallLandlord=true

--倍数控件
local mBeiLabel=nil

--房间信息
local roomInfo=nil 
--
local sendState=nil;----用来判断现在是否在聊天界面--true未不在聊天界面
local chatTable={};--存放聊天说要的数据

local oldOutCard={}
local isOuting=nil
local isAgain=nil
local mTouchCCPointBegin
local mTouchCCPointMove
local mBeginIndex
local mLastOutInfo=nil
local beishuNum=nil

--
local mingCardTable=nil
local isClick=nil
local endInfo=nil

local mMingBtn=nil
local mLandLordLayer=nil

local waitingSprite=nil
local currentUserid=nil

local mTuoGuanID=nil
local firstGetCoin=nil
-----
local haveWhere=nil;--在哪里

local mPutDownBtn=nil
local mDoBtn=nil

local onetime=nil
---------------公有接口(以下两个函数固定不用改变)--------
--

function setCardTable(table,loadCard)
	mCardTable=table
	mLandLordCardTable=loadCard
end;

function setRoomID(value)
mRoomID=value
end;

--是否明牌
function callMingAction(node)
	local tag=node
	if not isClick then
	isClick=true
	actionLayer.Action2007(mScene,nil,tag)
	end
end;
-- 退出场景
function closeScene()
	releaseResource()
end

--过牌
function passAction(node)
	isOuting=true
	putDownAction()
	MainHelper.topMoveUp()
	actionLayer.Action2009(mScene,nil,"")
end;

--提示出牌
function noticeAction()
 	if mLastOutInfo and mLastOutInfo.UserId~= tonumber(mPersonalInfo._userID) then
 		local noticeTable=AiLayer.getCards(mCardTable,mLastOutInfo.DeckType ,mLastOutInfo.RecordTabel,mLastOutInfo.CardSize)
 		putDownAction()
		for k , v in pairs(noticeTable) do
			if mCardTable[v] then
				local info=mCardTable[v]
				info.isChoice= true				
				info.cardSprite:setPosition(CCPoint(info.cardSprite:getPosition().x,bottomHeight+SY(15)))
			end
		end		
		if #noticeTable==0 then
			passAction()
		else
			setBtnEnabled(true)	
		end;
 	end	
end
--


--放下
function  putDownAction()
	local startY=bottomHeight
	setBtnEnabled(false)
	for k, v in pairs(mCardTable) do
		v.isChoice=false
		v.cardSprite:setPosition(CCPoint(v.cardSprite:getPosition().x,startY))	
	end
end;


--叫地主
function callLandLordAction(node)
	
	local tag=node
	
	if tag==2 and noCallPath =="button/button_1002.png" then
		isFrstCallLandlord=true
	end;
	
	if tag>0 then
		actionLayer.Action2005(mScene,nil,tag)
	end
end;

----点击出牌
function sendCardAction()
	local outTable={}
	local cards=""
	local index=1
	MainHelper.topMoveUp()
	for k, v in pairs(mCardTable) do
		if v.isChoice then
			if index~=1 then
				cards=cards.. ","
			end	
			index=index+1
			ZyTable.push_back(outTable,v)
			cards=cards.. v.CardId
		end
	end
--	createOutLayer(type,outTable)
	if cards~="" then
		actionLayer.Action2009(mScene,nil,cards)

	else
	      ZyToast.show(mScene,Language.IDS_SENDCARDWARM,1,0.3)
	end;
end;
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
    haveWhere=true;--在哪里
	--初始化一张牌的
	local sprite=	CCSprite:create(P("card/card_1064.png"))
	mCardSize=sprite:getContentSize()
	mColW=sprite:getContentSize().width
	mPersonalInfo=PersonalInfo.getPersonalInfo()
	bottomHeight=pWinSize.height*0.08
	isRun=nil
	oldOutCard={}
	isOuting=nil
	---
	isClick=nil
    	chatTable={};--存放聊天说要的数据
    	waitingSprite=nil
end
--在哪里
function getWherer()
    return haveWhere;
end
-- 释放资源
function releaseResource()
    haveWhere=nil;--在哪里
	mCurrentLandLord=nil
	mCurrentLandUserID=nil
	isClick=nil
	currentUserid=nil
	currentIsReNew=nil
	firstGetCoin=nil
	mTuoGuanID=nil
		mControlLayer=nil
	--公告
	BroadcastLayer.releaseResource()
	--加载分配动画
    	if waitingSprite then
    		waitingSprite:removeFromParentAndCleanup(true)
    		waitingSprite=nil
    		ScutAnimation.CScutAnimationManager:GetInstance():UnLoadSprite("loading")
    	end	

    	--删除托管层
    	releaseTuoGuan()
    	
    	releaseMingLayer()
	--释放地主三张牌
	releaseLandLoarCard()
	--释放打出去的牌
	releaseOutLayer()
	--释放叫地主操作层
	releaseLandLordLayer()
	--释放结束的时候的结束框
	MainHelper.releaseReportLayer()
	--结束计时器
--	CCScheduler:sharedScheduler():unscheduleScriCCPointFunc("MainDesk.timeElapse")
	CCDirector:sharedDirector():getScheduler():unscheduleScriptEntry(schedulerEntry1)
	--释放牌组
	releaseCardLayer()
	sendState=nil;----用来判断现在是否在聊天界面--true未不在聊天界面
	chatTable=nil;--存放聊天说要的数据
	--释放玩家
	releasePlayerLayer()
	--释放发牌动作
	releaseSendLayer()
	--释放底下的提示框
	MainHelper.relaeseBottomLayer()
	--释放上部的按钮
	MainHelper.releaseTopLayer()
	mScene=nil
	mClockSprite=nil
	mLayer=nil
	isCall=nil
	isOuting=nil
	oldOutCard={}
end	


function getRoomID()
	return mRoomID
end;

--跳出牌局结果
function showReport()
	local index=nil;
	MainHelper.pushLandloardEnd(endInfo)	
end;

--刷新明牌的牌组
function refreshMingCardTable()
		local linshiTable={}
		for k, v in pairs(mingCardTable) do
			if not v.isChoice then
			 	ZyTable.push_back(linshiTable,v)
			end
		end
		mingCardTable=linshiTable	
end

function onTouch(eventType , x, y)
    if eventType == "began" then 
        return touchBegan(x,y)
    elseif eventType == "moved" then
        return touchMoved(x,y)
    elseif eventType == "ended" then 
        return touchEnd(x,y)
    end
end
-- 创建场景
function initScene()
	initResource()
	if mScene then
		return 
	end
	local scene = ScutScene:new()	
	-- 注册网络回调
	scene:registerCallback(networkCallback)	
	mScene = scene.root
	-- 添加背景
	mLayer = CCLayer:create()
	mScene:addChild(mLayer, 0)
	MainScene.releaseResource()
	-- 注册触屏事件
	mLayer:setTouchEnabled(true)	
	mLayer:registerScriptTouchHandler(onTouch)
	--
      local runningScene = CCDirector:sharedDirector():getRunningScene()
	if runningScene == nil then
		CCDirector:sharedDirector():runWithScene(mScene)
	else
		CCDirector:sharedDirector():replaceScene(mScene)
	end
	---创建背景
	local bgLayer=UIHelper.createMainDestBg(bottomHeight)
	mLayer:addChild(bgLayer,0)
	
	--创建底下 信息
	local bottomLayer,label,info=MainHelper.createBottomLayer(mPersonalInfo,1001)
	mBeiLabel=label
	roomInfo=info	
	mLayer:addChild(bottomLayer,0)
	
	onetime=1
	--创建头部组件
	local topLayer=MainHelper.createTopMenu(mLayer)
    --CCScheduler:sharedScheduler():scheduleScriCCPointFunc("MainDesk.timeElapse", 1, false)
	schedulerEntry1 = CCDirector:sharedDirector():getScheduler():scheduleScriptFunc(timeElapse, 1, false)
   	--
	sendState=true;--用来判断是否不在聊天界面的
	
	--等待动画
		--[[
	waitingSprite=ScutAnimation.CScutAnimationManager:GetInstance():LoadSprite("pipei")
	waitingSprite:play()
	waitingSprite:setPosition(CCPoint(pWinSize.width/2, pWinSize.height*0.5))
	mLayer:addChild(waitingSprite,1)
	]]
		--[[
	 if firstGetCoin then
	 	local pos=CCPoint(pWinSize.width/2,waitingSprite:getPosition().y+waitingSprite:getContentSize().height/2)
	 	MainHelper.showFondEffect(pos,firstGetCoin,mLayer,3)
	 	firstGetCoin=nil
	 end
         ]]
	--BroadcastLayer.init(mScene,1)
	playmusic()

end

function playmusic()
	if mScene then
		MainHelper.play(mScene)
	end
end;

function setFirstCallLandlord(flag)
	isFrstCallLandlord=flag
end


-- 触屏按下
function touchBegan(x, y)
if not mTuoGuanLayer  and mLayer and mCardTable then
	mTouchCCPointBegin=CCPoint(x,y)
	local startY=bottomHeight
	for k=#mCardTable,1,-1 do
		local v=mCardTable[k]
		if v then
		if MainHelper.isTouchInCard(mTouchCCPointBegin,v.cardSprite,mColW) then
			 v.isChoice= not  v.isChoice
			 mBeginIndex=k
			if   v.isChoice then
				v.cardSprite:setPosition(CCPoint(v.cardSprite:getPosition().x,
                                v.cardSprite:getPosition().y+SY(15)))
			else
				v.cardSprite:setPosition(CCPoint(v.cardSprite:getPosition().x,startY))	
			end
			break
		end	
		end
  end
  end
  return 1
end

-- 触屏移动
function touchMoved(e)
		mTouchCCPointMove=true
		 return 1
end

-- 触屏弹起
function touchEnd(x,y)
if mTouchPtMove  and mCardTable then

 	local touchPtEnd= PT(x,y)
	
	local endIndex=nil
	for k=#mCardTable,1,-1 do
		local v=mCardTable[k]
		if MainHelper.isTouchInCard(touchPtEnd,v.cardSprite,mColW) then
			endIndex=k
			break
		end
	end
	if endIndex and mBeginIndex and endIndex~=mBeginIndex then
			local begin=mBeginIndex<endIndex and mBeginIndex or endIndex
			local mEnd=mBeginIndex>endIndex and mBeginIndex  or endIndex
			for k=begin ,mEnd do
				local v=mCardTable[k]
				v.isChoice= true				
				v.cardSprite:setPosition(PT(v.cardSprite:getPosition().x,bottomHeight+SY(15)))
			end	
	end
	mBeginIndex=nil
	mTouchPtBegin=nil
	mTouchPtMove=nil
end


	local count=0
	if mCardTable then
        for k ,v in pairs(mCardTable) do
             if v.isChoice then
                count=count+1
                break
             end
        end
	end
	if count>0 then
		setBtnEnabled(true)
	else
		setBtnEnabled(false)
	end	

end

--
function firstGetGameCoin(value)
	firstGetCoin=value
end;

--初始化玩家
function releasePlayerLayer()
	if mPlayerLayer then
		mPlayerLayer:getParent():removeChild(mPlayerLayer,true)
		mPlayerLayer=nil
	end
end;

--初始化玩家位置等
function  initPlayerLayer(playTable)
    if mLayer and  #playTable>1  then
	releasePlayerLayer()
	local layer=CCLayer:create()
	mPlayerLayer=layer
	mLayer:addChild(layer,0)
	mPalyerTable=playTable
	local posY=bottomHeight+mCardSize.height+SY(15)
	local height=pWinSize.height*0.25
	mCardTable={}
	--创建玩家自己
	local playerIndex=1	
	for k, v in pairs(playTable) do
		if v.UserId ==tonumber(mPersonalInfo._userID) then
			playerIndex=k
			local headSprite=MainHelper.createHeadSprite(string.format("headImg/%s.png",v.HeadIcon))
			headSprite:setPosition(CCPoint(0,posY))
			layer:addChild(headSprite,1)
			mHeadSprite=headSprite
			v.headSprite=headSprite
			chatTable[k]=v;
			if v.NickName  then
				local nameLabel=CCLabelTTF:create(v.NickName,FONT_NAME,FONT_FM_SIZE)
				nameLabel:setAnchorPoint(CCPoint(0.5,0))
				nameLabel:setPosition(CCPoint(headSprite:getContentSize().width/2,0))
				headSprite:addChild(nameLabel,0)
			end
			break
		end
	end	
		
	--创建下家
	local index=(playerIndex+1)%3 ==0 and 3 or (playerIndex+1)%3
	local info=playTable[index]
	local headSprite,nameLabel,headBg=MainHelper.creaetPalyerHead(2,info,posY+height)
	layer:addChild(headSprite,0)
	playTable[index].headSprite=headBg
	chatTable[index]=playTable[index];

	--创建上家
	local index=(playerIndex-1)%3 ==0 and 3 or (playerIndex-1)%3
	local info=playTable[index]
	local headSprite,nameLabel,headBg=MainHelper.creaetPalyerHead(1,info,posY+height)
	layer:addChild(headSprite,0)
	playTable[index].headSprite=headBg
	chatTable[index]=playTable[index];


	local mingBtn=ZyButton:new("button/button_1011.png","button/button_1012.png")
	mingBtn:addImage("button/button_1010.png")
	mingBtn:addto(layer,0)
	mingBtn:setPosition(CCPoint(pWinSize.width-mingBtn:getContentSize().width,
	                                pWinSize.height*0.48))
	mingBtn:setIsVisible(false)				
	mingBtn:registerScriptTapHandler(refreshMingCard)
	mMingBtn=mingBtn
end	
end;

--获取聊天玩家的位置
function getChatPlayerTable()
   return chatTable;
end

--发牌
function  releaseSendLayer()
	if mSendLayer then
		mSendLayer:getParent():removeChild(mSendLayer,true)
		mSendLayer=nil
	end
end;

--
function sendCardLayer(info)
    if mLayer then
    releaseLandLoarCard()
    releaseLandLordLayer()
    mLastOutInfo=nil
    isFrstCallLandlord=true
    	mCardTable={}
    	if waitingSprite then
    		waitingSprite:removeFromParentAndCleanup(true)
    		waitingSprite=nil
    		ScutAnimation.CScutAnimationManager:GetInstance():UnLoadSprite("pipei")
    	end
	releaseSendLayer()
	mSendServerData=info	
	local layer=CCLayer:create()
	mSendLayer=layer
	mLayer:addChild(layer,0)	
	local singleW=(pWinSize.width-mColW)/19
	local startY=bottomHeight
	MainHelper.sendCardAction(layer,info,singleW,startY)
	end
end;

----创建玩家的牌组
function releaseCardLayer()
	if mCardLayer then
		mCardLayer:getParent():removeChild(mCardLayer,true)
		mCardLayer=nil
	end
end;



--刷新玩家拥有的牌组
function refreshCardLayer(type)
	releaseCardLayer()
	local layer=CCLayer:create()
	mCardLayer=layer
	mLayer:addChild(layer,0)	
	
	table.sort(mCardTable,  sortComps)
	local linshiTable={}
	if  not type  then
	for k, v in pairs(mCardTable) do
		if not v.isChoice then
		 	ZyTable.push_back(linshiTable,v)
		end
	end
	mCardTable=linshiTable
	end
	
	local singleW=(pWinSize.width-mColW)/19
	local startX=pWinSize.width/2-((#mCardTable-1)*singleW+mColW)/2
	local startY=bottomHeight
	local index=0

	for k, v in pairs(mCardTable) do
			local cardSprite= MainHelper.createSigleCard(v,k)	
			cardSprite:setPosition(CCPoint(startX+index*singleW,startY))
			v.cardSprite=cardSprite
			v.isChoice=false
			index=index+1
			layer:addChild(cardSprite,0)
	end
end;

--排序算法
function sortComps(a,b)
	local  result = (a.CardId % 100) - (b.CardId % 100);
	if result==0 then
		result=(a.CardId / 100) - (b.CardId / 100);
	end	
	if result>0 then
		return true
	end
	return false
end;

--选择一张卡
function choiceCardAction(node)
	if not  isOuting then
	local tag=node:getTag()
	local startY=bottomHeight
	if mCardTable[tag] then
		local sprite=mCardTable[tag].cardSprite
		 mCardTable[tag].isChoice= not  mCardTable[tag].isChoice
		if   mCardTable[tag].isChoice then
			sprite:setPosition(CCPoint(sprite:getPosition().x,sprite:getPosition().y+SY(15)))
		else
			sprite:setPosition(CCPoint(sprite:getPosition().x,startY))	
		end
	end
	end
end;

--释放控制层
function  releaseControlLayer()
	isRun=false
	mPutDownBtn=nil
	mDoBtn=nil
	if mControlLayer then
		mControlLayer:getParent():removeChild(mControlLayer,true)
		mControlLayer=nil
	end
end;
function noNotice()
	if noticeSprite ~=nil then
		noticeSprite:getParent():removeChild(noticeSprite,true)
	end;
end;
--玩家控制层   
function createControlLayer(IsReNew)
    ChatLayer.closeBtnActon();--释放聊天场景
	local layer=CCLayer:create()
	mControlLayer=layer
	mLayer:addChild(layer,0)
	
	local bgSprite=CCSprite:create(P("common/panel_1001_6.png"))
	local passBtn=ZyButton:new("button/panle_1009_3.png","button/panle_1009_4.png","button/panle_1009_5.png")
	local posY=mHeadSprite:getPosition().y
	passBtn:addImage("button/panle_1033.png")	
	local cha=(pWinSize.width-mHeadSprite:getContentSize().width-passBtn:getContentSize().width*4.7)/2
	passBtn:setPosition(CCPoint(mHeadSprite:getPosition().x+mHeadSprite:getContentSize().width+cha,
						posY))
	passBtn:addto(layer,0)
	passBtn:registerScriptTapHandler(passAction)

	
	local clockSprite=CCSprite:create(P("button/icon_1032.png"))
	clockSprite:setAnchorPoint(CCPoint(0,0.5))
	clockSprite:setPosition(CCPoint(passBtn:getPosition().x+passBtn:getContentSize().width+SX(2),
								passBtn:getPosition().y+passBtn:getContentSize().height/2))
	layer:addChild(clockSprite,0)
	if mSendServerData.CodeTime  then
		mTimes=mSendServerData.CodeTime
		local coldLabel=CCLabelTTF:create(mSendServerData.CodeTime,FONT_NAME,FONT_FM_SIZE)
		mTimeLabel=coldLabel
		coldLabel:setAnchorPoint(CCPoint(0.5,0.5))
		coldLabel:setPosition(CCPoint(clockSprite:getContentSize().width/2,clockSprite:getContentSize().height/2))
		clockSprite:addChild(coldLabel,0)
		isRun=true
	end
	
	local noticeBtn=ZyButton:new("button/panle_1009_3.png","button/panle_1009_4.png","button/panle_1009_5.png")
	noticeBtn:addImage("button/panle_1047.png")	
	noticeBtn:setPosition(CCPoint(clockSprite:getPosition().x+clockSprite:getContentSize().width*1.1,
						clockSprite:getPosition().y-noticeBtn:getContentSize().height/2))
	noticeBtn:addto(layer,0)
	noticeBtn:registerScriptTapHandler(noticeAction)
	if IsReNew ==1 then
		passBtn:setIsEnabled(false)
		noticeBtn:setIsEnabled(false)
		passBtn:addImage("button/panle_1047_1.1.png")	
		noticeBtn:addImage("button/panle_1033_1.1.png")	
	end
	local putDownBtn=ZyButton:new("button/panle_1009_3.png","button/panle_1009_4.png","button/panle_1009_5.png")
	putDownBtn:addImage("button/panle_1040.png")	
	putDownBtn:setPosition(CCPoint(noticeBtn:getPosition().x+noticeBtn:getContentSize().width*1.1,
						noticeBtn:getPosition().y))
	putDownBtn:addto(layer,0)
	putDownBtn:registerScriptTapHandler(putDownAction)
--	putDownBtn:setIsEnabled(false)	
	mPutDownBtn=putDownBtn
	local doBtn=ZyButton:new("button/panle_1009_1.png","button/panle_1009_2.png","button/panle_1009_5.png")
	doBtn:addImage("button/panle_1038.png")
	doBtn:setPosition(CCPoint(putDownBtn:getPosition().x+putDownBtn:getContentSize().width*1.1,
						putDownBtn:getPosition().y))
	doBtn:addto(layer,0)
	--doBtn:setIsEnabled(false)	
	doBtn:registerScriptTapHandler(sendCardAction)
	mDoBtn=doBtn
	local count=0
	for k ,v in pairs(mCardTable) do
		 if v.isChoice then
		 	count=count+1
		 	break
		 end
	end
	if count>0 then
		setBtnEnabled(true)
	else
		setBtnEnabled(false)
	end	
	noticeSprite=nil
	if mLastOutInfo then
	local noticeTable=AiLayer.getCards(mCardTable,mLastOutInfo.DeckType ,mLastOutInfo.RecordTabel,mLastOutInfo.CardSize )	
		if #noticeTable==0 and IsReNew ~=1 then
			noticeSprite=CCSprite:create(P("title/font_1013.png"))
			noticeSprite:setAnchorPoint(CCPoint(0.5,0))
			noticeSprite:setPosition(CCPoint(pWinSize.width/2,bottomHeight+(mCardTable[1].cardSprite:getContentSize().height-noticeSprite:getContentSize().height)/2))
			layer:addChild(noticeSprite,0)
			 delaySprite(noticeSprite,noNotice,3)
		end;
	end
end;

--延迟处理
function  delaySprite(sprite,funName,nDuration) 
        local  action = CCSequence:createWithTwoActions(
            CCDelayTime:create(nDuration),
            CCCallFunc:create(funName));
            sprite:runAction(action)
end;






function setBtnEnabled(value)
			if mPutDownBtn and  mDoBtn then
				if value then
					mPutDownBtn:addImage("button/panle_1040.png")	
					mDoBtn:addImage("button/panle_1038.png")			
				else
					mPutDownBtn:addImage("button/panle_1040_2.png")	
					mDoBtn:addImage("button/panle_1038_2.png")
				end
				mPutDownBtn:setIsEnabled(value)
				mDoBtn:setIsEnabled(value)
			end
end;



--释放打出去的牌
function releaseOutLayer()
	if mOutLayer and mLayer then
		mLayer:removeChild(mOutLayer,true)
		mOutLayer=nil
	end
end;



--刷新明牌层
function refreshMingCard()
	if mingCardTable and #mingCardTable>0 then
		releaseMingLayer()
		local layer=CCLayer:create()
		mMingLayer=layer
		mLayer:addChild(layer,2)
		--
		local actionBtn=UIHelper.createActionRect(pWinSize,"MainDesk.releaseMingLayer")
		actionBtn:setPosition(CCPoint(0,0))
		layer:addChild(actionBtn,0)
		--
		local bgSprite=CCSprite:create(P("common/ground_2018.9.png"))
		bgSprite:setScaleX(pWinSize.width/bgSprite:getContentSize().width)
		bgSprite:setScaleY(pWinSize.height/bgSprite:getContentSize().height)
		bgSprite:setAnchorPoint(CCPoint(0.5,0.5))
		bgSprite:setPosition(CCPoint(pWinSize.width/2,pWinSize.height/2))
		layer:addChild(bgSprite,0)
		
		local kuangSprite=CCSprite:create(P("common/panle_1070.9.png"))
		layer:addChild(kuangSprite,0)
		
		--	
		local linshiTable={}
		for k, v in pairs(mingCardTable) do
			if not v.isChoice then
			 	ZyTable.push_back(linshiTable,v)
			end
		end
		mingCardTable=linshiTable	
		local scale=0.6
		local singleW=(pWinSize.width-mColW)/19*scale
		local colW=(#mingCardTable-1)*singleW+mColW*scale
		local startX=pWinSize.width/2-colW/2
		local startY=bottomHeight+mCardSize.height*1.5
		
		kuangSprite:setScaleX((colW+SX(10))/kuangSprite:getContentSize().width)
		kuangSprite:setScaleY(mCardSize.height*0.8/kuangSprite:getContentSize().height)
		kuangSprite:setAnchorPoint(CCPoint(0,0))
		kuangSprite:setPosition(CCPoint(startX-SX(5),startY-mCardSize.height*0.1))
		
		local index=0	
		for k=#mingCardTable,1,-1 do
			local v=mingCardTable[k]
			local cardSprite= MainHelper.createSigleCard(v,nil,nil,scale)	
			cardSprite:setPosition(CCPoint(startX+index*singleW,startY))
			index=index+1
			layer:addChild(cardSprite,0)
		end
	end
end;

--创建打出去的牌组
function  createOutLayer(type,outTable,info)
    if mLayer then
    	isMing=false
    	isCall=false
    	releaseLandLordLayer()
    	releaseMingLayer()
	releaseOutLayer()
	releaseControlLayer()
	local layer=CCLayer:create()
	mOutLayer=layer
	mLayer:addChild(layer,1)	
	if #outTable>0 then
	local scale=0.7
	local singleW=(pWinSize.width-mColW)/19*scale
	local colW=(#outTable-1)*singleW+mColW*scale
	local startX=pWinSize.width/2-colW/2-mColW/2*scale
	local startY=pWinSize.height*0.52
	local index=0
	
	if mLastOutInfo and mLastOutInfo.UserId~=info.UserId then
		local i=math.random(1,3)
 		local yaobuqiTable={[1]=EnumMusicType.dani1,[2]=EnumMusicType.dani2,[3]=EnumMusicType.dani3}
 		local index=yaobuqiTable[i]
	    	playEffect(index)  	
    	end	
    	
 	if info.UserId~= tonumber(mPersonalInfo._userID) then
		for k, v in pairs(mPalyerTable) do
			if v.UserId==info.UserId then
				v.cardNum=v.cardNum-#outTable
				v.numLabel:setString(v.cardNum)
				local posX=v.headSprite:getPosition().x+v.headSprite:getContentSize().width*1.2
				if posX>=pWinSize.width*0.8 then	
					posX=v.headSprite:getPosition().x-colW-mColW
				end
				startX=posX
				break
			end
		end	
		--扣除明牌的牌
		if info.UserId== mCurrentLandUserID    and mingCardTable and  #mingCardTable>0 then
			for k, v in pairs(outTable) do
				for m, n in pairs(mingCardTable) do
					if v.CardId==n.CardId then
						n.isChoice=true
						break
					end
				end
			end
			if mMingLayer then
			    refreshMingCard()
			else
			    refreshMingCardTable()
			end	 
		end
	else
		startY=pWinSize.height*0.45
		for m, n in pairs(mCardTable) do
			n.isChoice=false
		end
		for k, v in pairs(outTable) do
			for m, n in pairs(mCardTable) do
				if v.CardId==n.CardId then
					n.isChoice=true
					break
				end
			end
		end
		local index=EnumMusicType.chupai
		playEffect(index)	
		refreshCardLayer()	 
 	end
	mLastOutInfo=info
	oldOutCard=outTable
 	oldOutX=startX
	for k, v in pairs(outTable) do
		local cardSprite= MainHelper.createSigleCard(v,nil,nil,scale)	
		index=index+1
		cardSprite:setPosition(CCPoint(startX+index*singleW,startY))
		layer:addChild(cardSprite,0)
	end
	--效果
	if info.DeckType ==7 then
		MainHelper.showAnimation("font_1004")
		local index=EnumMusicType.zhadan
	    	playEffect(index)
	elseif info.DeckType ==3 then
		MainHelper.showAnimation("font_1003")
		local index=EnumMusicType.wangzha
	    	playEffect(index)
	 elseif info.DeckType==5 then 
	 	local index=EnumMusicType.sandaiyi
	    	playEffect(index)
	 elseif info.DeckType==6 then
		 local index=EnumMusicType.sandaiyidui
	    	playEffect(index)
	elseif info.DeckType==11 then
		 local index=EnumMusicType.feiji
	    	playEffect(index)
	elseif info.DeckType==10 then
		 local index=EnumMusicType.liandui
	    	playEffect(index)
	elseif info.DeckType==8 then
		 local index=EnumMusicType.shunzi
	    	playEffect(index)
	end	
	if mBeiLabel and  info.MultipleNum   then
		local str=Language.IDS_BEISHU.. ":".. info.MultipleNum 
		mBeiLabel:setString(str)
	end
		
 	else
 		--如果之前有牌的话 记住
 		if oldOutCard and #oldOutCard>0  and mLastOutInfo and mLastOutInfo.UserId~=tonumber(mPersonalInfo._userID)  then
			local scale=0.7
			local singleW=(pWinSize.width-mColW)/19*scale
			local colW=(#oldOutCard-1)*singleW+mColW*scale
			local startY=pWinSize.height*0.52
			local index=0
			for k, v in pairs(oldOutCard) do
				local cardSprite= MainHelper.createSigleCard(v,nil,nil,scale)	
				index=index+1
				cardSprite:setPosition(CCPoint(oldOutX+index*singleW,startY))
				layer:addChild(cardSprite,0)
			end
 		end
		for k, v in pairs(mPalyerTable) do
			if v.UserId==info.UserId  then
				local passSprite=CCSprite:create(P("common/font_1010.png"))
				--音效（要不起）	
				local id=math.random(1,3)
		 		local yaobuqiTable={[1]=EnumMusicType.pass,[2]=EnumMusicType.yaobuqi,[3]=EnumMusicType.buyao}
		 		local index=yaobuqiTable[id]
			    	 playEffect(index)
	    	    --
				passSprite:setAnchorPoint(CCPoint(0,0.5))
				layer:addChild(passSprite,0)
				local posX=v.headSprite:getPosition().x+v.headSprite:getContentSize().width*1.2
				if posX>=pWinSize.width*0.8 then	
					posX=v.headSprite:getPosition().x-passSprite:getContentSize().width*1.5
				end
				local clockPos=CCPoint(posX,v.headSprite:getPosition().y+v.headSprite:getContentSize().height/2)
				passSprite:setPosition(clockPos)
			end
		end
 	end
 	isOuting=false
 	end
end;

--叫地主
function releaseLandLordLayer()
	 if mLandLordLayer then
	 	mLandLordLayer:getParent():removeChild(mLandLordLayer,true)
	 	mLandLordLayer=nil
	 end
end;

--移除闹钟精灵
function releaseClock()
	if mClockSprite then
		mClockSprite:getParent():removeChild(mClockSprite,true)
		mClockSprite=nil
	end
	mTimeLabel=nil
	isCall=nil
end;

--创建闹钟
function  createClock(type)
	if mSendServerData and mSendServerData.CodeTime  then
		if mClockSprite then
			mClockSprite:getParent():removeChild(mClockSprite,true)
			mClockSprite=nil
		end
		mTimes=mSendServerData.CodeTime
		local clockSprite=CCSprite:create(P("button/icon_1032.png"))
		clockSprite:setAnchorPoint(CCPoint(0,0))
		mClockSprite=clockSprite
		mLayer:addChild(clockSprite,0)
		local coldLabel=CCLabelTTF:create(mSendServerData.CodeTime,FONT_NAME,FONT_FM_SIZE)
		mTimeLabel=coldLabel
		coldLabel:setAnchorPoint(CCPoint(0.5,0.5))
		coldLabel:setPosition(CCPoint(clockSprite:getContentSize().width/2,clockSprite:getContentSize().height/2))
		clockSprite:addChild(coldLabel,0)
		if type==1 then
			isRun=true
		elseif type==2 then
			isMing=true	
		else
			isCall=true
		end
	end
end;

--闹钟移动结束
function clockMoveEnd(node)
	mTimes=mSendServerData.CodeTime
	mTimeLabel:setString(mTimes)
	isCall=true
end;
--叫地主时  闹钟移动
function moveClock(movePos)
	if mClockSprite then
		local moveAct = CCMoveTo:create(0.2,movePos);
		moveAct=CCSequence:createWithTwoActions(moveAct, CCCallFuncN:create(clockMoveEnd))
		mClockSprite:runAction(moveAct)
	end
end;



--叫地主 操作框
function callLandlord(LandlordId,LandlordName,lastState,isRob)
	releaseLandLordLayer()
	local layer=CCLayer:create()
	mLayer:addChild(layer,0)
	mLandLordLayer=layer
	local landlordSprite
	local type=nil
	if not mClockSprite then
	    type=1
	    createClock()
	end
	--
	if LandlordId ==tonumber(mPersonalInfo._userID) then
         ChatLayer.closeBtnActon();--释放聊天场景
		landlordSprite=CCSprite:create(P("common/font_1002.png"))
		landlordSprite:setAnchorPoint(CCPoint(0.5,0.5))
		landlordSprite:setPosition(CCPoint(pWinSize.width/2,pWinSize.height*0.55))
		layer:addChild(landlordSprite,0)
		--
		local callBtn=ZyButton:new("button/button_1011.png","button/button_1012.png",nil)
		local path="button/panle_1046.png"
		if (not lastState or lastState ==0) and  ( not isRob or isRob==0)  then 
			path="button/button_1001.png"
		end
		callBtn:addImage(path)
		
		callBtn:addto(layer,0)
		callBtn:setPosition(CCPoint(pWinSize.width/2-callBtn:getContentSize().width*1.5,pWinSize.height*0.45-callBtn:getContentSize().height))
		callBtn:setTag(1)
		callBtn:registerScriptTapHandler(callLandLordAction)
		--
		local noCallBtn=ZyButton:new("button/button_1011.png","button/button_1012.png",nil)
		noCallPath="button/panle_1035.png"	
		if  (not lastState or lastState ==0) and  ( not isRob or isRob==0) then 
			 noCallPath="button/button_1002.png"
		end
		noCallBtn:addImage(noCallPath)
		noCallBtn:addto(layer,0)
		noCallBtn:setPosition(CCPoint(pWinSize.width/2+noCallBtn:getContentSize().width*0.5,callBtn:getPosition().y))
		noCallBtn:setTag(2)
		noCallBtn:registerScriptTapHandler(callLandLordAction)
	else
	 	landlordSprite=CCLabelTTF:create(LandlordName or 0,FONT_NAME,FONT_BIG_SIZE)
		landlordSprite:setAnchorPoint(CCPoint(0.5,0.5))
		landlordSprite:setPosition(CCPoint(pWinSize.width/2,pWinSize.height*0.58))
		layer:addChild(landlordSprite,0)
		
	end
	
	local leftSprite=CCSprite:create(P("common/font_1001.png"))
	leftSprite:setAnchorPoint(CCPoint(1,0.5))
	leftSprite:setPosition(CCPoint(landlordSprite:getPosition().x-landlordSprite:getContentSize().width/2,landlordSprite:getPosition().y))
	layer:addChild(leftSprite,0)
	
	local rightImg=nil
	if   (not lastState or lastState ==0) and  ( not isRob or isRob==0) then
		rightImg="common/font_1003.png"
	else	
		rightImg="common/font_1008.png"
	end;
	
	
	local rightSprite=CCSprite:create(P(rightImg))
	rightSprite:setAnchorPoint(CCPoint(0,0.5))
	rightSprite:setPosition(CCPoint(landlordSprite:getPosition().x+landlordSprite:getContentSize().width/2,landlordSprite:getPosition().y))
	layer:addChild(rightSprite,0)
				
	local id=findPlayerByUserID(LandlordId)
	local info=mPalyerTable[id]	
	local posX=info.headSprite:getPosition().x+info.headSprite:getContentSize().width
	if posX>=pWinSize.width*0.8 then	
		    posX=info.headSprite:getPosition().x-mClockSprite:getContentSize().width
	end
	
	local clockPos=CCPoint(posX,info.headSprite:getPosition().y+mClockSprite:getContentSize().height*0.8)
								
	if mCurrentLandLord and lastState then	
		local info=mPalyerTable[mCurrentLandLord]
		local pathTable={[0]="font_1009",[1]="font_1008"}
		if  not isRob or  isRob==0   then
			pathTable={[0]="font_1007",[1]="font_1003"}
		else
			if lastState==1 then
				MainHelper.showAnimation("font_1002")
			end
		end
		local sprite=CCSprite:create(P(string.format("common/%s.png",pathTable[lastState])))
		sprite:setAnchorPoint(CCPoint(0,0))
		local posX=info.headSprite:getPosition().x+info.headSprite:getContentSize().width
		if posX>=pWinSize.width*0.8 then
			posX=info.headSprite:getPosition().x-sprite:getContentSize().width
		end
		sprite:setPosition(CCPoint(posX,info.headSprite:getPosition().y+info.headSprite:getContentSize().height*0.3))
		layer:addChild(sprite,0)
        if type then
             mClockSprite:setPosition(clockPos)
        else
            moveClock(clockPos)
        end
		
	else
		if mClockSprite then
		    mClockSprite:setPosition(clockPos)
		end
	end
	
	isFrstCallLandlord=false
	--找到斗地主
	mCurrentLandLord=findPlayerByUserID(LandlordId)
end;

--根据userid找到玩家数据
function findPlayerByUserID(id)
	--找到斗地主
	if #mPalyerTable>0 then
	for k, v in pairs(mPalyerTable) do
		if v.UserId ==id then
			return k
		end
	end
	end
end;



--叫地主回调 判断是否结束叫地主
function pushBackCallLandLord(info)
    if mLayer then
	--叫地主 
	local str=Language.IDS_BEISHU.. ":".. info.MultipleNum 
	mBeiLabel:setString(str)
	beishuNum=info.MultipleNum 
	mCurrentLandUserID=info.LandlordId
	if info.IsEnd ==0 then
		isCall=false
		callLandlord(info.LandlordId,info.LandlordName,info.IsCall ,info.IsRob )
		--isFrstCallLandlord=false
	else
		--叫地主结束
		
		local index=EnumMusicType.start
	    	playEffect(index)
	    	
		releaseLandLordLayer()
		for k, v in pairs(mPalyerTable) do
			local spritePath="common/button_1015.png"
			if v.UserId ==info.LandlordId then
				spritePath="common/button_1016.png"
				if info.LandlordId==tonumber(mPersonalInfo._userID) then
					for k, v in pairs(mLandLordCardTable) do
						mCardTable[#mCardTable+1]=v
					end
					refreshCardLayer(1)
				else
					v.cardNum=20
					v.numLabel:setString(20)
				end	
			else
				v.cardNum=17
			end
			local sprite=CCSprite:create(P(spritePath))
			sprite:setAnchorPoint(CCPoint(1,1))	
			sprite:setPosition(CCPoint(v.headSprite:getContentSize().width,v.headSprite:getContentSize().height))
			v.headSprite:addChild(sprite,0)
		end
        
		releaseClock()
		createLandLordCard(mLandLordCardTable)	
	
        if mCurrentLandUserID ==tonumber(mPersonalInfo._userID) then
            careateMingPaiLayer()
        else
            controlChoice(info.LandlordId)	
        end	
	end
	end
end;

--释放明牌层
function releaseMingLayer()
    isMing=false
    releaseClock()
	if mMingLayer then
		mMingLayer:getParent():removeChild(mMingLayer,true)
		mMingLayer=nil
	end
end;

--创建明牌的玩家
function careateMingPaiLayer()
	if mCurrentLandUserID ==tonumber(mPersonalInfo._userID) then
		local layer=CCLayer:create()
		mMingLayer=layer
		mLayer:addChild(layer,1)
		local callBtn=ZyButton:new("button/button_1011.png","button/button_1012.png",nil)
		callBtn:addImage("button/button_1010.png")	
		callBtn:addto(layer,0)
		callBtn:setPosition(CCPoint(pWinSize.width/2-callBtn:getContentSize().width*1.5,
						pWinSize.height*0.45-callBtn:getContentSize().height))
		callBtn:setTag(1)
		callBtn:registerScriptTapHandler(callMingAction)
		
		--
		local noCallBtn=ZyButton:new("button/button_1011.png","button/button_1012.png",nil)
		noCallBtn:addImage("button/button_1017.png")
		noCallBtn:addto(layer,0)
		noCallBtn:setPosition(CCPoint(pWinSize.width/2+noCallBtn:getContentSize().width*0.5,callBtn:getPosition().y))
		noCallBtn:setTag(2)
		noCallBtn:registerScriptTapHandler(callMingAction)
		 createClock(2)
		 mTimes=10
		 local pos=CCPoint(pWinSize.width/2-mClockSprite:getContentSize().width/2,noCallBtn:getPosition().y+noCallBtn:getContentSize().height)
		 mClockSprite:setPosition(pos)
		 
	else
		
	end
end;



--创建明牌按钮
function createMingBtn(serverTable)
    if mBeiLabel then
        local str=Language.IDS_BEISHU.. ":".. serverTable.MultipleNum 
        mBeiLabel:setString(str)
    end
	if mCurrentLandUserID ~=tonumber(mPersonalInfo._userID) then
		mingCardTable=serverTable.RecordTabel
		local info=mPalyerTable[findPlayerByUserID(mCurrentLandUserID)]	
		mMingBtn:setIsVisible(true)
		MainHelper.showPngEffect(info.headSprite,"button/font_1011.png",nil,nil,mLayer)	
	end	
end;


--释放三张牌
function releaseLandLoarCard()
	if mLandLordCardLayer then
		mLandLordCardLayer:getParent():removeChild(mLandLordCardLayer,true)
		mLandLordCardLayer=nil
	end
end;

--创建地主的三张牌 两种状态 nil 为中间 有值为左上角
function createLandLordCard(info,type)
    if info then
	releaseLandLoarCard()
	local layer=CCLayer:create()
	mLandLordCardLayer=layer	
	mLayer:addChild(layer,2)
	local scale=0.5
	local startX=0
	local startY=pWinSize.height-mCardSize.height*scale
	---
	local singleW=(pWinSize.width-mColW)/19*scale	
	if type then
		 singleW=mColW*scale*1.1
		 startX=pWinSize.width/2-singleW*1.5
		 startY=pWinSize.height*0.7
	end	
	for k, v in pairs(info) do
		local cardSprite= MainHelper.createSigleCard(v,nil,nil,scale,type)		
		cardSprite:setPosition(CCPoint(startX+(k-1)*singleW,startY))
		layer:addChild(cardSprite,k)
	end
	end
end;

--判断是否创建 操作框还是创建闹钟
function  controlChoice(LandlordId,IsReNew )
if not mLayer    then
	return
end
releaseClock()
releaseControlLayer()
if LandlordId ==tonumber(mPersonalInfo._userID) then
	currentUserid=LandlordId
	currentIsReNew=IsReNew
	if  not mTuoGuanLayer then
	 createControlLayer(IsReNew)
	 end
else
	currentUserid=nil
	currentIsReNew=nil
	createClock(1)
	local id=findPlayerByUserID(LandlordId)
	local info=mPalyerTable[id]	
	local posX=info.headSprite:getPosition().x+info.headSprite:getContentSize().width	
	if posX>=pWinSize.width*0.8 then	
		posX=info.headSprite:getPosition().x-mClockSprite:getContentSize().width
	end
	local clockPos=CCPoint(posX,info.headSprite:getPosition().y+mClockSprite:getContentSize().height*0.8)
	mClockSprite:setPosition(clockPos)
end
end;

--计时器
function timeElapse(dt)
	if isRun  and mTimes and mTimes>0 then
		mTimes=math.ceil(mTimes-dt)
		if mTimeLabel then
			mTimeLabel:setString(mTimes)
		end
		if mTimes<=0 then
			isRun=false
			releaseClock()
			releaseControlLayer()
		end
	end
	--明牌
	if isMing  and mTimes and mTimes>0 then
		mTimes=math.ceil(mTimes-dt)
		if mTimeLabel then
			mTimeLabel:setString(mTimes)
		end
		if mTimes<=0 then
			isMing=false
			actionLayer.Action2007(mScene,nil,2)
		end
	end
	
	--叫地主倒计时
	if isCall and mTimes and mTimes>0 then
		mTimes=math.ceil(mTimes-dt)
		if mTimes<=0 then
		   mTimes=0 
		end
		if mTimeLabel then
			mTimeLabel:setString(mTimes)
		end
		if mTimes<=0 then
			isCall=false
			if mCurrentLandUserID== tonumber(mPersonalInfo._userID) then
				actionLayer.Action2005(mScene,nil,2)
			end
            		ChatLayer.closeBtnActon();--释放聊天场景
		end
	end
	onetime=onetime+1
	if onetime==10 then
		actionLayer.Action100(mScene,false,1)
		onetime=0
	end
end;

--获取玩家打牌时候的层
function getLayer()
    return mLayer;
end

--获取玩家发送状态
function getSendState()
    return sendState;
end

function setSendState(state)
    if state then
        sendState=nil;
    end
end

function setIsAgain(value)
	isAgain=value
end;

--创建托管层 
function releaseTuoGuan()
	if mTuoGuanLayer then
		mTuoGuanLayer:getParent():removeChild(mTuoGuanLayer,true)
		mTuoGuanLayer=nil
	end
end;

function getTuoGuanLayer()
	return mTuoGuanLayer
end;

---系统下发托管
function createChocieTuoGuan(serverTable)
	if mPersonalInfo and serverTable.UserId ==tonumber(mPersonalInfo._userID) then
		createTuoGuan()
	end
end;

---
function puchReleaseTuoGuan()
	 releaseTuoGuan()
	 
	 if currentUserid then
	 	controlChoice(currentUserid,currentIsReNew)
	 end
end;

--取消托管
function cancelTuoAction()
	mTuoGuanID=2
	actionLayer.Action2011(mScene,nil,2)
end;

--创建托管精灵
function createTuoGuan()	
    releaseTuoGuan()
	local layer=CCLayer:create()
	mTuoGuanLayer=layer
	mLayer:addChild(layer,2)
	
	local tuoBtn=ZyButton:new("common/button_1043.png")
	tuoBtn:setAnchorPoint(CCPoint(0,0))
	tuoBtn:registerScriptTapHandler(cancelTuoAction)
	tuoBtn:setPosition(CCPoint(pWinSize.width/2-tuoBtn:getContentSize().width/2,
							bottomHeight))
	tuoBtn:addto(layer,0)
end;

function  setTuoGuanID(value)
	mTuoGuanID=value
end;

--显示其他玩家的牌组
function showOtherCard()
	releaseOutLayer()
	local layer=CCLayer:create()
	mOutLayer=layer
	mLayer:addChild(layer,1)	
	local scale=0.4
	local singleW=(pWinSize.width-mColW)/19*scale
	local startY=pWinSize.height*0.52
	local mPersonalInfo=PersonalInfo.getPersonalInfo()
	--
	for k, v in pairs(endInfo.RecordTabel) do
		 if v.UserId~=tonumber(mPersonalInfo._userID) then
		 		local index=0
		 		local id=findPlayerByUserID(v.UserId)
		 		local info=mPalyerTable[id]
		 		if info.numLabel then
				info.numLabel:setString(0)		 
				end				
		 		local colW=(#v.cardTable-1)*singleW+mColW*scale
		 		local posY=info.headSprite:getPosition().y+info.headSprite:getContentSize().height-mCardSize.height*scale
				local posX=info.headSprite:getPosition().x+info.headSprite:getContentSize().width
				if posX>=pWinSize.width*0.8 then	
				       posY=info.headSprite:getPosition().y	
					posX=info.headSprite:getPosition().x-colW-mColW*scale
				end 
				for m=#v.cardTable,1,-1 do
				local n=v.cardTable[m]
					local cardSprite= MainHelper.createSigleCard(n,nil,nil,scale)	
					index=index+1
					cardSprite:setPosition(CCPoint(posX+index*singleW,posY))
					layer:addChild(cardSprite,0)
				end				
		 end		 
	end
	
	if endInfo.IsLandlord ==0 then
	index=EnumMusicType.lose
	if  endInfo.IsLandlordWin==0 then
		index=EnumMusicType.win
	end;
	elseif  endInfo.IsLandlord ==1 then
	index=EnumMusicType.win
	if  endInfo.IsLandlordWin==0 then
		index=EnumMusicType.lose
	end;
	end;
	playEffect(index)
	
	MainHelper.delayExec(showReport,3, mLayer)
end;
--显示报告
function createRepor(info)
    if mLayer then
	endInfo=info
	releaseOutLayer()
	isRun=false
	releaseClock()
	mCardTable=nil
	local layer=CCLayer:create()
	mOutLayer=layer
	mLayer:addChild(layer,1)	



	if mBeiLabel then
		local str=Language.IDS_BEISHU.. ":".. info.MultipleNum 
		mBeiLabel:setString(str)
	end
	
	local scale=0.7
	local singleW=(pWinSize.width-mColW)/19*scale
	local startY=pWinSize.height*0.52
	local mPersonalInfo=PersonalInfo.getPersonalInfo()
	--
	local colW=(#info.lastCardTable-1)*singleW+mColW*scale
	local startX=pWinSize.width/2-colW/2-mColW/2*scale
	local startY=pWinSize.height*0.52
	local index=0
	if info.LastUserId~=tonumber(mPersonalInfo._userID) then
		 		local id=findPlayerByUserID(info.LastUserId)
		 		local info=mPalyerTable[id]	
		 		local posY=startY
		 		if info.numLabel then
					info.numLabel:setString(0)		 
				end		
				local posX=info.headSprite:getPosition().x+info.headSprite:getContentSize().width*1.2
				if posX>=pWinSize.width*0.8 then	
					posX=info.headSprite:getPosition().x-colW-mColW			
				end
				for m, n in pairs(endInfo.lastCardTable) do
					local cardSprite= MainHelper.createSigleCard(n,nil,nil,scale)	
					index=index+1
					cardSprite:setPosition(CCPoint(posX+index*singleW,posY))
					layer:addChild(cardSprite,0)
				end		
	else
		releaseCardLayer()		
		for m, n in pairs(endInfo.lastCardTable) do
			local cardSprite= MainHelper.createSigleCard(n,nil,nil,scale)	
			index=index+1
			cardSprite:setPosition(CCPoint(startX+index*singleW,startY))
			layer:addChild(cardSprite,0)
		end			
	end
	MainHelper.delayExec(showOtherCard,2, mLayer)
	end
end;





--返回大厅
function baskToMainHome()
				resetMusicInit()
			   	PrivateChatLayer.clearMessage();--消除聊天记录
				closeScene()
				MainScene.initScene()	
end;

--游戏退出通知
local escapeInfo=nil
function gameOverCallBack(serverInfo)
			--玩家跑	
			escapeInfo=serverInfo
			PersonalInfo.getPersonalInfo()._GameCoin=serverInfo.GameCoin 
			ChatLayer.closeBtnActon()
			if serverInfo.FleeUserId ~=0  and serverInfo.FleeUserId~=tonumber(PersonalInfo.getPersonalInfo()._userID) then		
		          	local box = ZyMessageBoxEx:new()         	
		          	local str=string.format(Language.TIP_ESCAPE,serverInfo.FleeNickName )
			       box:doPromCCPoint(mScene, nil, str,Language.IDS_OK,showEscapeReport)
			--系统解散
			else
				baskToMainHome()
			end
end;

--显示逃跑的结局
function showEscapeReport()
	escapeInfo.IsLandlordWin=1
	escapeInfo.CoinNum=escapeInfo.InsCoinNum
	MainHelper.pushLandloardEnd(escapeInfo,1)
end;

--去商城
function gotoshop(index, content, tag)
	local mScene = CCDirector:sharedDirector():getRunningScene()
	if index == 1 then
				resetMusicInit()
			   	PrivateChatLayer.clearMessage();--消除聊天记录
				closeScene()	
				MainScene.initScene(1)	
	else
		baskToMainHome()	
	end
end;

---
function continuGame(serverInfo)
    	if waitingSprite then
    		waitingSprite:removeFromParentAndCleanup(true)
    		waitingSprite=nil
    		ScutAnimation.CScutAnimationManager:GetInstance():UnLoadSprite("pipei")
    	end
	if mBeiLabel then
		local str=Language.IDS_BEISHU.. ":".. serverInfo.MultipleNum  
		mBeiLabel:setString(str)
	end 
    	mSendServerData=serverInfo
	 initPlayerLayer(serverInfo.playerTable)
	 for k, v in pairs(serverInfo.playerTable) do

	 	local id=findPlayerByUserID(v.UserId)
		local info=mPalyerTable[id]	
		info.cardNum=#v.CardTable
	 	if v.UserId ==tonumber(mPersonalInfo._userID) then
	 		mCardTable=v.CardTable
	 	else
	 	    if info.numLabel then
	 	        info.numLabel:setString(info.cardNum)
	 	    end
	 	end	
	 	if serverInfo.LandlordId ~=0 then
	 		mCurrentLandUserID=serverInfo.LandlordId 
	 		local spritePath="common/button_1015.png"
	 		if mCurrentLandUserID==v.UserId then
	 			mingCardTable=v.CardTable
	 			spritePath="common/button_1016.png"
	 			
	 		end
			local sprite=CCSprite:create(P(spritePath))
			sprite:setAnchorPoint(CCPoint(1,1))	
			sprite:setPosition(CCPoint(info.headSprite:getContentSize().width,info.headSprite:getContentSize().height))
			info.headSprite:addChild(sprite,0)	
	 	end
	 end
	 


			
	 refreshCardLayer(1)
	 --是否托管
	 if serverInfo.IsAI ==1 then
	 	createTuoGuan()
	 end
	 if serverInfo.IsShow==1 and  mCurrentLandUserID~=tonumber(mPersonalInfo._userID)  then
	 	if mMingBtn then
	 		mMingBtn:setIsVisible(true)
	 	end
	 end
	 if serverInfo.GameCoin then
		  mPersonalInfo._GameCoin=serverInfo.GameCoin
		  MainHelper.setGoldeNum(serverInfo.GameCoin)
	 end
	 --
	 if serverInfo.RecordTabel and #serverInfo.RecordTabel>0 then
	 	createOutLayer(type,serverInfo.RecordTabel,serverInfo)
	 end 
	 
	 for k, v in pairs(serverInfo.playerTable) do

	 	local id=findPlayerByUserID(v.UserId)
		local info=mPalyerTable[id]	
		info.cardNum=#v.CardTable
	 	if v.UserId ==tonumber(mPersonalInfo._userID) then
	 		mCardTable=v.CardTable
	 	else
	 	    if info.numLabel then
	 	        info.numLabel:setString(info.cardNum)
	 	    end
	 	end	
	 end
	 
	if serverInfo.OutCardUserId and serverInfo.OutCardUserId~=0 then
		controlChoice(serverInfo.OutCardUserId,serverInfo.IsReNew )
	end
	 mLandLordCardTable=serverInfo.LandLordCard
	 createLandLordCard(mLandLordCardTable)
end;

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionID = ZyReader:getActionID()
	local userData = ZyRequestParam:getParamData(lpExternalData)
	--房间离开接口
	if actionID==2002 then
		if ZyReader:getResult() == eScutNetSuccess then
			baskToMainHome()	
		else
       		 ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.2)	
		end
		--进入房间 用于续局用
	elseif  actionID==2001 then
	    	if ZyReader:getResult() == eScutNetSuccess then
	    		    	local GameCoin = ZyReader:getInt(); 
	    		    	currentUserid=nil
				PersonalInfo.getPersonalInfo()._GameCoin=GameCoin   
				MainHelper.setGoldeNum(GameCoin)
				--[[
				--等待动画
				waitingSprite=ScutAnimation.CScutAnimationManager:GetInstance():LoadSprite("pipei")
				waitingSprite:play()
				waitingSprite:setPosition(CCPoint(pWinSize.width/2, pWinSize.height*0.5))
				mLayer:addChild(waitingSprite,1)
				]]
				if mBeiLabel then
					local str=Language.IDS_BEISHU.. ":".. roomInfo.RoomMultiple 
					mBeiLabel:setString(str)
				end
		elseif ZyReader:getResult() ==1 then
		--	local box = ZyMessageBoxEx:new()
		--	box:doQuery(pScutScene,nil,Language.TIP_ESCAPE1,Language.IDS_SURE,Language.IDS_CANCEL,leaveRoom)		
		else	
			local box = ZyMessageBoxEx:new()
			box:doQuery(pScutScene,nil, ZyReader:readErrorMsg(),Language.IDS_SURE,Language.IDS_CANCEL,gotoshop)	
		end
	--明牌接口
	elseif  actionID==2007 then
	    	if ZyReader:getResult() == eScutNetSuccess then
			releaseMingLayer()
			controlChoice(mCurrentLandUserID,1)	
		else
       		 ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.2)	
		end
	elseif actionID==2008 then
		 local serverTable=actionLayer._2008Callback(pScutScene, lpExternalData)
		 --自己不是地主的时候
		 if mCurrentLandUserID~=tonumber(mPersonalInfo._userID) then
		 	mingCardTable=serverTable.RecordTabel	
		 end
	--叫地主
	elseif  actionID==2005 then
		if ZyReader:getResult() == eScutNetSuccess then
			isCall=false
			releaseLandLordLayer()	
		else
       		 ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.2)	
		end
		--托管接口
	elseif  actionID==2011 then
		if ZyReader:getResult() == eScutNetSuccess then
			if mTuoGuanID==1 then
				 createTuoGuan()	
			elseif mTuoGuanID==2 then
				puchReleaseTuoGuan()
			end	
		elseif ZyReader:getResult() == 1 then
			baskToMainHome()
		else
       		 ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.2)	
		end
		mTuoGuanID=nil
		--出牌接口
	elseif  actionID==2009 then
		if ZyReader:getResult() == eScutNetSuccess then		
			releaseControlLayer()
			releaseClock()
		else
			isOuting=nil
       		 ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.2)	
		end	
		--
	elseif actionID ==9002 then----9002_聊天发送接口（ID=9002）
        if ZyReader:getResult() == eScutNetSuccess then
            PrivateChatLayer.getMessage(pScutScene)
            ChatLayer.setIsClick(true)
            ChatLayer.closeBtnActon();
            sendState=true
        else         
		ChatLayer.setIsClick(true) 
		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.2)
        end
     elseif actionID ==9003 then--9003_聊天记录列表接口【完成】（ID=9003）
           local table=actionLayer._9003Callback(pScutScene, lpExternalData)
             if ChatLayer.getIsClick2() then----用来判断是否是切换Tab的时候到聊天记录那边
                ChatLayer.setIsClick2(false);
                ChatLayer.updateChatTypeLayer()
             else
           end
	elseif actionID==9202 then-- 9003_聊天记录列表接口【完成】（ID=9003）
		BroadcastLayer.networkCallback(pScutScene, lpExternalData)
	 end
	 isClick=nil
end


