------------------------------------------------------------------
-- LetterScenes.lua.lua
-- Author     : yeyq

-- Version    : 1.0
-- Date       :
-- Description: 信件
------------------------------------------------------------------

module("LetterScenes", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local g_scene = nil 		-- 场景
local g_LayerBG = nil 
local g_mTabBar = nil 
local g_mCurrentTab = nil 
local g_List = nil 
local g_LetterTabel = nil 
local g_LeaveLayer = nil 
local g_mInputEdit = nil 

local 	g_MenuId = {
			eSuoYou = 1,
			eZhanDou = 2,
		
			eHaoYou = 3,
			eXiTong = 4,
		}
local 	g_LetterType = {
			eSuoYou = 0,
			eZhanDou = 1,
			eHaoYou = 2,
			eXiTong = 3,
		}
	
local	g_TabName = {
			[g_MenuId.eSuoYou] = Language.MAILL_ALL,
			[g_MenuId.eZhanDou] = Language.MAILL_FIGHT,
		--	[g_MenuId.eHaoYou] = Language.MAILL_FRIEND,
			
		[g_MenuId.eXiTong] = Language.MAILL_SYSTEM,
			
	
			[g_MenuId.eHaoYou] = Language.MAILL_FRIEND,
		
		}
local  g_TabNameTo = {}
local g_ButtonStrTabel = {
		[g_MenuId.eZhanDou] = Language.MAILL_FIGHTSTR,
		[g_MenuId.eHaoYou] = Language.MAILL_FRIENDSTR,
		[g_MenuId.eXiTong] = Language.MAILL_SYSTEMSTR,
	  }	
local	g_goTabel = {}

--
---------------公有接口(以下两个函数固定不用改变)--------
--

-- 函数名以小写字母开始，每个函数都应注释

function OnEnter()
	MainMenuLayer.refreshWin()
end;

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
	if not g_mCurrentTab then
		g_mCurrentTab = 1 
	end
	
	g_MenuId = {
			eSuoYou = 1,
			eZhanDou = 2,
	
			eHaoYou = 3,
			eXiTong = 4,
		}
 	g_LetterType = {
			eSuoYou = 0,
			eZhanDou = 1,
			eHaoYou = 2,
			eXiTong = 3,
		}
	g_TabName = {
			[g_MenuId.eSuoYou] = Language.MAILL_ALL,
			[g_MenuId.eZhanDou] = Language.MAILL_FIGHT,
			
		[g_MenuId.eXiTong] = Language.MAILL_SYSTEM,
			
	
			[g_MenuId.eHaoYou] = Language.MAILL_FRIEND,
		
		}
	 g_ButtonStrTabel = {
		[g_MenuId.eZhanDou] = Language.MAILL_FIGHTSTR,
		[g_MenuId.eHaoYou] = Language.MAILL_FRIENDSTR,
		[g_MenuId.eXiTong] = Language.MAILL_SYSTEMSTR,
	  }	
	g_TabNameTo = {}
end
-- 释放资源
function releaseResource()
		closeAllLayer()
		g_scene = nil 		-- 场景
		g_LayerBG = nil 
		g_TabName = nil 
		g_mTabBar = nil 
		g_MenuId = nil 
		g_List = nil 
		g_ButtonStrTabel = nil
		g_goTabel = nil
		g_buttonClose = nil 
		g_TabNameTo =nil 
		g_mInputEdit = nil
		nilString=nil
end

-- 创建场景
function init()
	local scene = ScutScene:new()
	-- 注册网络回调
	initResource()
	scene:registerCallback(networkCallback)
	g_scene = scene.root 
	g_scene:registerScriptHandler(SpriteEase_onEnterOrExit)
	--g_scene:registerOnEnter("LetterScenes.OnEnter")
	SlideInLReplaceScene(g_scene,1)
	
	-- 添加背景
	 local layerBg = CCLayer:create()
	 g_LayerBG = layerBg
	g_scene:addChild(layerBg, 1)
	
	-- 注册触屏事件
	--layerBg.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHBEGAN, "LetterScenes.touchBegan")
	--layerBg.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHMOVED, "LetterScenes.touchMove")
	--layerBg.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHENDED, "LetterScenes.touchEnd")
	-- 此处添加场景初始内容
	local bgImgH = pWinSize.height*0.68
	local bgImg = CCSprite:create(P("common/list_1040.png"))
	bgImg:setScaleX(pWinSize.width/bgImg:getContentSize().width)
	bgImg:setScaleY(bgImgH/bgImg:getContentSize().height)
	layerBg:addChild(bgImg,0)
	bgImg:setAnchorPoint(PT(0,0))
	bgImg:setPosition(PT(0,pWinSize.height*0.14))
	RankFuc()
	
	local  tabBarRate= 49/960
	local tabBar=ZyTabBar:new(nil,nil,g_TabNameTo,FONT_NAME,FONT_SM_SIZE,4)
	g_mTabBar=tabBar
	tabBar:addto(layerBg,0)
	tabBar:setColor(ZyColor:colorYellow())
	tabBar:setAnchorPoint(PT(0,0))
	tabBar:selectItem(g_mCurrentTab);				--点击哪个按钮
	tabBar:setCallbackFun(callbackTabBarFunc);		--点击响应的事件
	tabBar:setPosition(PT(pWinSize.width*0.04
						,bgImg:getPosition().y+bgImgH -pWinSize.height*tabBarRate))
	local listH = pWinSize.height*(537/960)
	local listW = pWinSize.width*(591/640)	
	local listY = pWinSize.height*(62/960)+bgImg:getPosition().y
	local listSize = SZ(listW,listH)
	local list = ScutCxList:node(listH/2.5,ccc4(24,24,24,0),listSize)
	list:setTouchEnabled(true)
	list:setAnchorPoint(PT(0,0))
	list:setPosition(PT((pWinSize.width-listW)/2,listY))
	layerBg:addChild(list,1)
	
	g_List = list	
	
	nilString = CCLabelTTF:create("",FONT_NAME,FONT_SM_SIZE)	
	layerBg:addChild(nilString,0)
	nilString:setAnchorPoint(PT(0.5,0.5))
	nilString:setPosition(PT(pWinSize.width*0.5,pWinSize.height*0.55))	
	
       MainMenuLayer.init(1, g_scene)
	showAction()
end


function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        releaseResource()
    end
end



--- tab响应事件
function callbackTabBarFunc(bar,pNode)---- 本身，按钮
	local index=pNode:getTag(); ----按钮的标号
	if index~=g_mCurrentTab then
	    g_mCurrentTab = index; 
	    showAction()
	end
end
function RankFuc()
	local num = nil 
	for i=1,5 do 
		num = g_TabName[i]
		g_TabNameTo[i] =  num
	end
end

--tab切换界面
function showAction()
	g_List:clear();
	if  g_mCurrentTab == g_MenuId.eSuoYou then
		actionLayer.Action9302(g_scene,nil,0)
	elseif g_mCurrentTab==g_MenuId.eZhanDou then
		actionLayer.Action9302(g_scene,nil,1)
	elseif g_mCurrentTab==g_MenuId.eHaoYou then
		actionLayer.Action9302(g_scene,nil,2)
	elseif g_mCurrentTab==g_MenuId.eXiTong then
		actionLayer.Action9302(g_scene,nil,3)
	end
end


 ----list 显示
function showShop()
	if #g_LetterTabel > 0 then
		nilString:setString("")
	else
		nilString:setString(Language.MAILL_NONE)
	end

	for  i=1,#g_LetterTabel do
		local item = ScutCxListItem:itemWithColor(ccc3(25,57,45))	
		item:setOpacity(0)
		local layout = CxLayout()
		layout.val_x.t = ABS_WITH_PIXEL
		layout.val_y.t = ABS_WITH_PIXEL	
		layout.val_x.val.pixel_val =0
		layout.val_y.val.pixel_val =0
		local itemLayer = CCLayer:create()
		local listHeight =  g_List:getRowHeight()
		
		
		
		--- item 背景
		local itemBgImgH = listHeight*0.98
		local itemBgImgW = pWinSize.width*(580/640)
		local itemBgImg = CCSprite:create(P("common/list_1038.9.png"))
		itemBgImg:setScaleY(itemBgImgH/itemBgImg:getContentSize().height)
		itemBgImg:setScaleX(itemBgImgW/itemBgImg:getContentSize().width)
		itemLayer:addChild(itemBgImg,0)
		itemBgImg:setAnchorPoint(PT(0,0))
		itemBgImg:setPosition(PT((pWinSize.width*(591/640)-itemBgImgW)/2,(listHeight - itemBgImgH)/2))
		
		
		
		
		local itemAllContentX  = pWinSize.width*(12/640)
		local itemButtonY = pWinSize.height*(10/960) 
		----时间
		local itemDateStr =g_LetterTabel[i].SendDate or 0 
		local itemDate = CCLabelTTF:create(itemDateStr,FONT_NAME,FONT_SM_SIZE)	
		itemBgImg:addChild(itemDate,0)
		itemDate:setAnchorPoint(PT(0,0))
		itemDate:setPosition(PT(itemAllContentX,itemButtonY))
		
		----按钮
		local buttonStr = nil 
		local buttonStrOr = g_LetterTabel[i].MailType 
		if buttonStrOr  == g_LetterType.eZhanDou  then 
			buttonStr  = g_ButtonStrTabel[g_MenuId.eZhanDou]
		elseif buttonStrOr  == g_LetterType.eHaoYou  then 
			buttonStr = g_ButtonStrTabel[g_MenuId.eHaoYou]
	--	elseif  buttonStrOr  == g_LetterType.eXiTong  then 
	--		buttonStr = g_ButtonStrTabel[g_MenuId.eXiTong]
		end
	
		if g_LetterTabel[i].IsReply ==1    then	
			if g_LetterTabel[i].ReplyStatus~=1 then
				local agreeBtn = ZyButton:new("button/list_1039.png",nil,nil,Language.MAILL_AGREE)
				agreeBtn:addto(itemLayer,1)
				agreeBtn:setAnchorPoint(PT(0,0))
				agreeBtn:setTag(i)
				agreeBtn:setPosition(PT(pWinSize.width*(470/640),itemButtonY))
				local noAgreeBtn = ZyButton:new("button/list_1039.png",nil,nil,Language.MAILL_NOAGREE)
				noAgreeBtn:addto(itemLayer,1)
				noAgreeBtn:setAnchorPoint(PT(0,0))
				noAgreeBtn:setTag(i)
				noAgreeBtn:setPosition(PT(agreeBtn:getPosition().x-agreeBtn:getContentSize().width*1.5,
											itemButtonY))		
				noAgreeBtn:registerScriptHandler(noAgreeFirendAction)
				agreeBtn:registerScriptHandler(agreeFirendAction)
			end
		elseif  buttonStrOr == 1 or buttonStrOr == 2  then 	
			local itemButton = ZyButton:new("button/list_1039.png",nil,nil,buttonStr)
			itemButton:addto(itemLayer,1)
			itemButton:setAnchorPoint(PT(0,0))
			itemButton:setTag(i)
			itemButton:setPosition(PT(pWinSize.width*(470/640),itemButtonY))
			itemButton:registerScriptHandler(itemButtonFunc)
			
			if buttonStrOr == 1 then
				local itemButton1 = ZyButton:new("button/list_1039.png",nil,nil,Language.MAILL_HUIFANG)
				itemButton1:addto(itemLayer,1)
				itemButton1:setAnchorPoint(PT(0,0))
				itemButton1:setTag(i)
				itemButton1:setPosition(PT(itemButton:getPosition().x-itemButton1:getContentSize().width*1.1,itemButton:getPosition().y))
				itemButton1:registerScriptHandler(huifangAction)
--				g_LetterTabel[i].CounterattackUserID     g_LetterTabel[i].SendMailDate
			end
		end

		
		
		
			----内容
		local contentStrW = pWinSize.width*(550/640)
		local contentStrTo = g_LetterTabel[i].Content or 0
		local contentStr1 = string.format("<label>%s</label>",contentStrTo)
		local contentLabel = ZyMultiLabel:new(contentStr1,contentStrW,FONT_NAME,FONT_SM_SIZE)
		contentLabel:addto(itemBgImg,0)
		contentLabel:setAnchorPoint(PT(0,0))
		contentLabel:setPosition(PT(itemAllContentX,itemBgImgH - contentLabel:getContentSize().height - SY(15) ))
		
		local contentMaxHeight = itemBgImgH- SY(10)  - itemButtonY - SY(15) 
		

		
		-----写信人
		local itemTitleString = g_LetterTabel[i].FromUserName or 0
		local itemTitle = CCLabelTTF:create(itemTitleString,FONT_NAME,FONT_SM_SIZE)	
		itemBgImg:addChild(itemTitle,0)
		itemTitle:setAnchorPoint(PT(0,0))
		itemTitle:setPosition(PT(itemAllContentX,itemBgImgH  - SY(10)))

		----计算信件内容 如果大于 特定高点的信件内容 就设置高度
		local contentHeight =contentLabel:getContentSize().height
		if  contentHeight > contentMaxHeight then
			listHeight = (contentHeight-contentMaxHeight)+listHeight
			itemBgImgH = listHeight*0.98
			itemBgImg:setScaleY(itemBgImgH/itemBgImg:getContentSize().height)
			itemBgImg:setScaleX(itemBgImgW/itemBgImg:getContentSize().width)
			itemLayer:addChild(itemBgImg,0)
			itemBgImg:setAnchorPoint(PT(0,0))
			itemBgImg:setPosition(PT((pWinSize.width*(591/640)-itemBgImgW)/2,(listHeight - itemBgImgH)/2))
			
			contentLabel:setAnchorPoint(PT(0,0))
			contentLabel:setPosition(PT(itemAllContentX,itemBgImgH - contentLabel:getContentSize().height - SY(10) ))
		
			itemTitle:setAnchorPoint(PT(0,0))
			itemTitle:setPosition(PT(itemAllContentX,itemBgImgH  - SY(10)))
			g_List:setRowHeight(listHeight)
		end

		
		item:addChildItem(itemLayer,layout)
		g_List:addListItem(item,false)
	end
end	

function huifangAction(node)
	tag=node:getTag()
	actionLayer.Action5108(g_scene,nil,g_LetterTabel[tag].CounterattackUserID,g_LetterTabel[tag].MailID)
end;

--同意
function agreeFirendAction(node)
	local tag=node:getTag()
	if g_LetterTabel[tag] then
		actionLayer.Action9105(g_scene,nil ,g_LetterTabel[tag].FromUserId,g_LetterTabel[tag].MailID,1)
	end
end;


--不同意
function noAgreeFirendAction(node)
	local tag=node:getTag()
	if g_LetterTabel[tag] then
		actionLayer.Action9105(g_scene,nil ,g_LetterTabel[tag].FromUserId,g_LetterTabel[tag].MailID,0)
	end
end;



--- 按钮点击前往
function  itemButtonFunc(item)
	local i = item:getTag()
	local typeAge = g_LetterTabel[i].MailType
	if typeAge  == g_LetterType.eZhanDou  then--反击
			local id = g_LetterTabel[i].CounterattackUserID 
			
			actionLayer.Action5107(g_scene,nil,id,nil)
	elseif typeAge  == g_LetterType.eHaoYou  then --留言
			leaveStrFunc(i)
--	elseif  typeAge  == g_LetterType.eXiTong  then 
---			businessStore.pushScene()
	end
end

-----留言框
function leaveStrFunc(i)

	g_LeaveLayer = CCLayer:create()
	g_LayerBG:addChild(g_LeaveLayer,1)
	
	---屏蔽按钮

	local closeLayerButton = ZyButton:new(Image.image_toumingPath,Image.image_toumingPath,Image.image_toumingPath,nil,FONT_NAME,FONT_SM_SIZE)
	closeLayerButton:addto(g_LeaveLayer,0)
	closeLayerButton:setScaleX(pWinSize.width*1.2/closeLayerButton:getContentSize().width)
	closeLayerButton:setScaleY(pWinSize.height*1.2/closeLayerButton:getContentSize().height)
	closeLayerButton:setAnchorPoint(PT(0,0))
	closeLayerButton:setPosition(PT(0,0))

	
	---发送按钮
	local gotoButton = ZyButton:new("button/list_1023.png",nil,nil,Language.MAILL_FASONG)
	gotoButton:addto(g_LeaveLayer,2)
	gotoButton:setAnchorPoint(PT(0,0))
	gotoButton:setPosition(PT(pWinSize.width*(360/640),pWinSize.height*(280/930)))
	gotoButton:registerScriptHandler(goActionFunc)
	
	----取消按钮
	local closeButton = ZyButton:new("button/list_1023.png",nil,nil,Language.close_string)
	closeButton:addto(g_LeaveLayer,2)
	closeButton:setAnchorPoint(PT(0,0))
	closeButton:setPosition(PT(pWinSize.width*(84/640),gotoButton:getPosition().y))
	closeButton:registerScriptHandler(closeLayerFunc)

	--- 留言背景图
	local leaveStrBgImg = CCSprite:create(P("common/list_1054.png"))
	leaveStrBgImg:setScaleY(pWinSize.height*0.4/leaveStrBgImg:getContentSize().height)
	leaveStrBgImg:setScaleX(pWinSize.width/leaveStrBgImg:getContentSize().width)
	g_LeaveLayer:addChild(leaveStrBgImg,1)
	leaveStrBgImg:setAnchorPoint(PT(0,0.5))
	leaveStrBgImg:setPosition(PT(0,pWinSize.height*0.5))
	
	--- 留言背景图底部
	local leaveStrBgImg2 = CCSprite:create(P("common/list_1047.png"))
	leaveStrBgImg2:setScaleX(pWinSize.width/leaveStrBgImg2:getContentSize().width)
	g_LeaveLayer:addChild(leaveStrBgImg2,1)
	leaveStrBgImg2:setAnchorPoint(PT(0,0))
	leaveStrBgImg2:setPosition(PT(0,pWinSize.height*0.27))
	
	---留言背景框
	local leaveStrBrImg= CCSprite:create(P("common/list_1052.9.png"))
	leaveStrBrImg:setScaleY(pWinSize.height*(196/930)/leaveStrBrImg:getContentSize().height)
	g_LeaveLayer:addChild(leaveStrBrImg,2)
	leaveStrBrImg:setAnchorPoint(PT(0,0))
	leaveStrBrImg:setPosition(PT((pWinSize.width-leaveStrBrImg:getContentSize().width)/2,pWinSize.height*0.38))
	-----给XX留言
	-- 数据
	---
	local giveStr1 = string.format(Language.MAILL_LEAVESTRING,g_LetterTabel[i].FromUserName)
	local giveStr = CCLabelTTF:create(giveStr1,FONT_NAME,FONT_SM_SIZE)	
	g_LeaveLayer:addChild(giveStr,2)
	giveStr:setAnchorPoint(PT(0.5,0))
	giveStr:setPosition(PT(pWinSize.width*0.5,pWinSize.height*(570/930)))
	g_goTabel={}
	g_goTabel.Title = giveStr1
	g_goTabel.ToUserID = g_LetterTabel[i].FromUserId 
	g_goTabel.ToUserName = g_LetterTabel[i].FromUserName
	g_goTabel.IsGuide  = 1

	
	
	
	----留言内容
	local editSize=SZ(leaveStrBrImg:getContentSize().width*0.98,pWinSize.height*(186/930))
	local startY=pWinSize.height*(386/930)
	g_mInputEdit= CScutEdit:new();
	g_mInputEdit:init(true, false)
	g_mInputEdit:setRect(CCRect((pWinSize.width-leaveStrBrImg:getContentSize().width)/2+pWinSize.width*(13/960),startY,editSize.width,editSize.height))

	
end
---关掉屏蔽按钮
function closeLayerFunc()
	setEditVisible(false)
	if g_LeaveLayer then 
		g_LeaveLayer:getParent():removeChild(g_LeaveLayer,true)
		g_LeaveLayer = nil 
	end
end
---发送留言
function  goActionFunc()
local content=g_mInputEdit:GetEditText()

	if content and string.len(content)>0 then
		actionLayer.Action9301(g_scene,nil,g_goTabel.Title,content,g_goTabel.ToUserID,g_goTabel.ToUserName,g_goTabel.IsGuide)
	end
end

--设置输入框可见
function  setEditVisible(value)
		if  value == nil   then
			value = true	
		end
		if  g_mInputEdit then
			g_mInputEdit:setVisible(value)
		end
end

 --  全部层移除
function closeAllLayer()
	closeLayerFunc()
	if   g_LayerBG ~= nil then
		g_LayerBG:getParent():removeChild(g_LayerBG,true)	
		g_LayerBG = nil 
	end
end
---延迟进行方法
function delayExec(funName,nDuration)
    local  action = CCSequence:createWithTwoActions(
    CCDelayTime:create(nDuration),
    CCCallFunc:create(funName));
    g_LayerBG:runAction(action)
end

-- 触屏按下
function touchBegan(e)
end
-- 触屏移动
function touchMove(e)
end
-- 触屏弹起
function touchEnd(e)
end

function refreshWin()
	showAction(nil, mCurrentTab)
end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionID = ZyReader:getActionID()
	if  actionID == 9302  then
		local serverInfo=actionLayer._9302Callback(pScutScene, lpExternalData) ----返回整个表的数据
		if serverInfo~=nil  and serverInfo ~= " " then
			g_LetterTabel = serverInfo.LetterTabel 
			showShop()
		end
	elseif actionID == 9301  then
		if ZyReader:getResult() == eScutNetSuccess then
       		closeLayerFunc()
       		ZyToast.show(pScutScene,Language.MAILL_goLetterStr,1,0.5)
		else          
			setEditVisible(false)
			ZyToast.show(pScutScene,ZyReader:readErrorMsg(),0.5,0.15)	
			delayExec("LetterScenes.setEditVisible",0.7)			
		end
	elseif actionID == 5107  then
		local serverInfo=actionLayer._5107Callback(pScutScene, lpExternalData)
		if  serverInfo ~= nil then
			local battleType = 2
			CompetitiveBattle.setBattleInfo(serverInfo, g_scene, battleType)
			CompetitiveBattle.pushScene()					
		end	
	elseif  actionID==5108 then
	--战斗数据返回
		local attackData=actionLayer._5108Callback(pScutScene, lpExternalData)
		if attackData then
			local battleType = 7
			CompetitiveBattle.setBattleInfo(attackData, g_scene, battleType)
			CompetitiveBattle.pushScene()
		end
	elseif  	actionID == 9105  then
		if ZyReader:getResult() == eScutNetSuccess then
			showAction()
		else          
			ZyToast.show(pScutScene,ZyReader:readErrorMsg(),0.5,0.15)			
		end	

	else
		commonCallback.networkCallback(pScutScene, lpExternalData)

	end
	
end