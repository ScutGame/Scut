------------------------------------------------------------------
-- PlotListScene.lua
-- Author     :JunM Chen
-- Version    : 1.0
-- Date       :
-- Description: 副本列表
------------------------------------------------------------------
module("PlotListScene", package.seeall)
require("battle.battleScene")
require("battle.PlotFightLayer")
require("layers.AddSpriteLayer")



-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local mScene = nil 		-- 场景
local mLayer=nil
local mCurrentTab=nil
local mTabBar=nil
local mListTable={}
local mList=nil
mChoiceIndex=nil
local isClick=nil
local personalInfo=nil
----------临时
local isAction=nil
local topHeight=nil
local mServerData=nil
local mCityPlotData=nil
local mBackpackType =nil
--
local remenberChoice={}
local mEnergyLayer=nil
local plotType=nil
local dropLayer=nil
--
---------------公有接口(以下两个函数固定不用改变)--------
--
-- 函数名以小写字母开始，每个函数都应注释]

function getButtonTable()
	return buttonTable
end

function getPlotType()
	return plotType
end

-- 退出场景
function closeScene()
	releaseResource()
	
end

--初始化场景入口
function  init()
	if not plotType then
		plotType=1
	else
		local isTrue,index = GuideLayer.guidePlotIndex()
		if isTrue then
			mCurrentTab = 1
			mChoiceIndex = index
			plotType = 1
		else
			if mLastPlotType then
				plotType = mLastPlotType
			end
		end
	end
	if mScene then
		return
	end
	initResource()
	local scene = ScutScene:new()
    mScene = scene.root
	
		mScene:registerScriptHandler(SpriteEase_onEnterOrExit)
		
	-- 注册网络回调
	scene:registerCallback(networkCallback)	
	SlideInLReplaceScene(mScene,1)


	
	-- 添加背景
	mLayer = CCLayer:create()
	mScene:addChild(mLayer, 0)
	-- 此处添加场景初始内容
	
	if not  mCurrentTab then
		mCurrentTab=1
	end	

	topHeight=pWinSize.height*0.17

	local bgLayer=createBglayer(titleName)
	bgLayer:setPosition(PT(0,0))
	mLayer:addChild(bgLayer,0)	
	MainMenuLayer.init(1, mScene)
	send(4001,plotType)
end;


function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        releaseResource()
    end
end



function createMList(titleName)
        local tabBar=createTabBar(titleName)
        mTabBar=tabBar
        mListTable={}
        mListTable.maxPage=5
    	local listSize=SZ(pWinSize.width*0.9,pWinSize.height*0.55)
        local listX=(pWinSize.width-listSize.width)/2
        local listY=tabBar:getPosition().y-listSize.height-SY(2)
        local mListRowHeight=listSize.height/mListTable.maxPage
        local list = ScutCxList:node(mListRowHeight, ccc4(24, 24, 24, 0), listSize)
        list:setAnchorPoint(PT(0,0))
        list:setPosition(PT(listX,listY))
        list:setRecodeNumPerPage(1)
        mLayer:addChild(list,0)
		list:setTouchEnabled(true)
        mList=list
end


function  createContentLayer()
--创建通用list
	local titleName={}
	for k, v in pairs(mServerData) do
		titleName[#titleName+1]=v.CityName
	end
	createMList(titleName)
	
	local touchLayer=CCLayer:create()
	mLayer:addChild(touchLayer, 0)	
	
	touchLayer:setTouchEnabled(true)
--	touchLayer.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHBEGAN, "PlotListScene.touchBegan")
--	touchLayer.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHMOVED, "PlotListScene.touchMove")
--	touchLayer.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHENDED, "PlotListScene.touchEnd")	
	
	
	if not mChoiceIndex then
		mChoiceIndex=1
	end
	showLayer()
	
end;

function touchBegan(e)
	if isGuide then
		GuideLayer.close()
	end
end

---创建背景层
function  createBglayer(tabStr)
	local layer=CCLayer:create()
	layer:setAnchorPoint(PT(0,0))
	--大背景
	local bgSprite=CCSprite:create(P("common/list_1015.png"))
	bgSprite:setAnchorPoint(PT(0.5,0.5))
	bgSprite:setScaleX(pWinSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(pWinSize.height/bgSprite:getContentSize().height)
	bgSprite:setPosition(PT(pWinSize.width/2,pWinSize.height/2))
	layer:addChild(bgSprite,0)
	--中间层
	local midSprite=CCSprite:create(P("common/list_1040.png"))

	local boxSize=SZ(pWinSize.width, pWinSize.height*0.68)	
	midSprite:setAnchorPoint(PT(0.5,0))
	midSprite:setScaleX(boxSize.width/midSprite:getContentSize().width)
	midSprite:setScaleY(boxSize.height/midSprite:getContentSize().height)
	midSprite:setPosition(PT(pWinSize.width/2,pWinSize.height*0.145))
	layer:addChild(midSprite,0)
	
	local headImage = CCSprite:create(P("mainUI/list_3018_1.png"))
	headImage:setAnchorPoint(PT(0,0))
	headImage:setScaleX(pWinSize.width/headImage:getContentSize().width)
	headImage:setPosition(PT(0,pWinSize.height*0.145))
	layer:addChild(headImage, 1)
	--精英副本前往按钮
	eliteBtn=ZyButton:new("button/list_3018_2.png",nil,nil,
						nil,FONT_NAME,FONT_SM_SIZE)
	eliteBtn:setPosition(PT(pWinSize.width*0.45,pWinSize.height*0.145))
	eliteBtn:registerScriptHandler(goToElite)
	eliteBtn:addto(layer,1)
	
	
	--精英副本离开按钮
	nomalBtn=ZyButton:new("button/list_3018_3.png",nil,nil,
						nil,FONT_NAME,FONT_SM_SIZE)
	nomalBtn:setPosition(PT(pWinSize.width*0.45,pWinSize.height*0.145))
	nomalBtn:registerScriptHandler(exitElite)
	
	nomalBtn:addto(layer,1)
	
	if personalInfo._UserLv<10  then
		headImage:setVisible(false)
		eliteBtn:setVisible(false)
		nomalBtn:setVisible(false)
	elseif plotType==1 then
		eliteBtn:setVisible(true)
		nomalBtn:setVisible(false)
	elseif plotType==2 then
		nomalBtn:setVisible(true)
		eliteBtn:setVisible(false)
	end;
	
	
	local topSprite=CCSprite:create(P("mainUI/list_1000.png"))
	return layer
end;

--前往精英副本
function goToElite()
    mCurrentTab=nil
    mChoiceIndex=nil
    if  mTabBar then
    	mTabBar:remove()
    end;
	if mList then
		mList:clear()
	end;
	
	mLastTab = nil
	mChoiceIndex = nil
	
	plotType=2
	send(4001,plotType)
	
	if eliteBtn then
		eliteBtn:setVisible(false)
	end;
	if nomalBtn then
		nomalBtn:setVisible(true)
	end;
end;
--离开精英副本
function exitElite()
	 mCurrentTab=nil
   	 mChoiceIndex=nil
	if  mTabBar then
		mTabBar:remove()
	end;
	if mList then
		mList:clear()
	end;
	plotType=1
	mLastTab = nil
	mChoiceIndex = nil	
	send(4001,plotType)
	eliteBtn:setVisible(true)
	nomalBtn:setVisible(false)
end;

function createTabBar(tabStr)
	if tabStr then
		local tabBar=nil
	  	tabBar=ZyTabBar:new(nil,nil,tabStr,FONT_NAME,FONT_SM_SIZE,4)
		tabBar:selectItem(mCurrentTab)	
		local tabBar_X=pWinSize.width*0.04
		local tabBar_Y=pWinSize.height*0.77
		tabBar:setAnchorPoint(PT(0,0))
		tabBar:setPosition(PT(tabBar_X,tabBar_Y))
		tabBar:setCallbackFun(callbackTabBar)
		tabBar:addto(mLayer,1)
		
	    if mCurrentTab>=6 then
	        local chengshi=6
	        tabBar._list:turnToPage(chengshi-1)
        else
            local chengshi=mCurrentTab
            tabBar._list:turnToPage(chengshi-1)
	    end

		return tabBar
	end
end;

----tabbar响应
function callbackTabBar(bar,pNode)
    local index =pNode:getTag();
    if index ~=mCurrentTab then    	
    	   mChoiceIndex=0
    	   if remenberChoice[index] then
    	   	mChoiceIndex=remenberChoice[index]
    	   end   
	    mCurrentTab=index
	    showLayer()
    end
end;

function creatGuide()
	if GuideLayer.judgeIsGuide(2) then
		isGuide = true
		
		local isTrue,index = GuideLayer.guidePlotIndex()
		if mCurrentTab == 1 and index == mChoiceIndex then
			GuideLayer.setScene(mScene)
			GuideLayer.init()
		else
			GuideLayer.close()
		end
	else
		isGuide = nil
	end	
end
--层级管理器
function  showLayer()
    if not mCurrentTab then
    mCurrentTab=1
    mChoiceIndex=1
    end
	mCityPlotData=mServerData[mCurrentTab].CityPlotTable 
	refreshListPage()
end;

--刷新列表
function  refreshListPage()
	buttonTable = {}
	if mCityPlotData and mList then
		mList:clear()
		local layout = CxLayout()
		layout.val_x.t = ABS_WITH_PIXEL
		layout.val_y.t = ABS_WITH_PIXEL
		layout.val_x.val.pixel_val = 0
		layout.val_y.val.pixel_val = 0
		local type=false
		for k, v in pairs(mCityPlotData) do
		 	local listItem = ScutCxListItem:itemWithColor(ccc3(24,24,24))
			listItem:setOpacity(0)
			local plotInfoLayer,listHeight=createListItem(v,k)
			listItem:addChildItem(plotInfoLayer, layout)
			mList:setRowHeight(listHeight)
			if mChoiceIndex and  mChoiceIndex>#mCityPlotData-3 then
				type=true
			end
			mList:addListItem(listItem, type)
		end
		
		if mChoiceIndex and not type then
			local page=mChoiceIndex-1
			mList:turnToPage(page)
		end
		
		
	end

	delayExec(creatGuide, 0.3, mLayer)
end;




---延迟进行方法
function delayExec(funName,nDuration, parent)
	local  action = CCSequence:createWithTwoActions(
	CCDelayTime:create(nDuration),
	CCCallFuncN:create(funName));
	if parent then
		parent:runAction(action)
	end
end


--创建一个listItem项目
function  createListItem(info,index)
	local layer=CCLayer:create()
	local layerHeight=0
	local boxSize=SZ(mList:getContentSize().width,pWinSize.height*0.1)
	local bgSprite=CCSprite:create(P("common/list_1038.9.png"))
	bgSprite:setAnchorPoint(PT(0,0))
	bgSprite:setPosition(PT(0,0))
	layer:addChild(bgSprite,0)

	--
	local startX=boxSize.width*0.05
	--info.BossHeadID="Icon_1002"
	local mBgTecture=IMAGE("common/list_1012.png")
	if info.BossHeadID then	
		--物品图片
		local headBg=CCSprite:createWithTexture(mBgTecture)
		boxSize.height=headBg:getContentSize().height*1.4
		headBg:setAnchorPoint(PT(0,0.5))
		headBg:setPosition(PT(startX,boxSize.height/2))
		bgSprite:addChild(headBg,0)	
		local path=string.format("smallitem/%s.png",info.BossHeadID)
		local headSprite=CCSprite:create(P(path))
		headSprite:setAnchorPoint(PT(0.5,0.5))
		headSprite:setPosition(PT(headBg:getContentSize().width/2,
								headBg:getContentSize().height/2))	
		startX=startX+headBg:getContentSize().width+SX(10)
		headBg:addChild(headSprite,0)			
	end
	
	bgSprite:setScaleX(boxSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(boxSize.height/bgSprite:getContentSize().height)
	layerHeight=boxSize.height
	
	--副本名称
	local plotName=CCLabelTTF:create(info.PlotName,FONT_NAME,FONT_DEF_SIZE)
	plotName:setAnchorPoint(PT(0,0.5))
	layer:addChild(plotName,1)
	plotName:setColor(ccC1)
		
	--前往按钮
	local gotoBtn=ZyButton:new("button/list_1039.png",nil,nil,
							Language.PLOT_GOTO,FONT_NAME,FONT_SM_SIZE)
	buttonTable[#buttonTable+1] = gotoBtn
	--前往按钮
	local actionBtn2,node=UIHelper.createActionRect(gotoBtn:getContentSize(),enterPlotAction,index)
	layer:addChild(actionBtn2,0)	
	--选中项目按钮
	local actionBtn=UIHelper.createActionRect(boxSize,selectItemAction,index)
	layer:addChild(actionBtn,0)
	gotoBtn:addto(layer,1)
	gotoBtn:setTag(index)
	if info.PlotStatus==0 then
		gotoBtn:setVisible(false)
		node:setEnabled(false)
	end	
	--如果被选中 则创建不同的项目
	if info.isSelected then
		local contentWidth=	boxSize.width*0.94
		--总高度
		local contentHeight=0
		local startX=boxSize.width*0.06
		local rowH=SY(2)
		
		--说明文字
		local contentStr=string.format("<label>%s</label>",info.PlotDesc or Language.IDS_NONE)
		local contentLabel=ZyMultiLabel:new(contentStr,contentWidth*0.9,FONT_NAME,FONT_DEF_SIZE);
		contentLabel:setAnchorPoint(PT(0,0))
		contentLabel:addto(layer,1)
		contentHeight=contentLabel:getContentSize().height
		
		if plotType==1 then
			contentLabel:setVisible(true)
		else
			contentLabel:setVisible(false)
		end;
		
		--体力消耗
		local str=Language.PLOT_PHYTIP .. ":" .. (info.EnergyNum or 0)
		local physicalLabel=CCLabelTTF:create(str,FONT_NAME,FONT_DEF_SIZE)
		physicalLabel:setAnchorPoint(PT(0,0))
		layer:addChild(physicalLabel,1)
		contentHeight=contentHeight+physicalLabel:getContentSize().height+rowH		
		--建议等级
		local str=Language.PLOT_ADVISELV .. ":" .. string.format(Language.IDS_LVSTR,(info.PlotLv or 0))
		local levelLabel=CCLabelTTF:create(str,FONT_NAME,FONT_DEF_SIZE)
		levelLabel:setAnchorPoint(PT(0,0))
		layer:addChild(levelLabel,1)
		contentHeight=contentHeight+levelLabel:getContentSize().height+rowH	

		--最大挑战次数
		local str=Language.PLOT_TIP2 .. ":" ..string.format("%d/%d", info.PlotNum, info.MaxChallengeNum)
	--	local str=Language.PLOT_TIP2 .. ":" .. info.MaxChallengeNum
		local timeLabel=CCLabelTTF:create(str,FONT_NAME,FONT_DEF_SIZE)
		timeLabel:setAnchorPoint(PT(0,0))
		layer:addChild(timeLabel,1)
		
--		contentHeight=contentHeight+levelLabel:getContentSize().height+rowH	
		
		--胜利奖励
		local str=Language.PLOT_WINREWARD .. ":" 
		local rewardLabel=CCLabelTTF:create(str,FONT_NAME,FONT_DEF_SIZE)
		rewardLabel:setAnchorPoint(PT(0,0))
		layer:addChild(rewardLabel,1)
		contentHeight=contentHeight+rewardLabel:getContentSize().height+rowH
		local rewardHeight=rewardLabel:getContentSize().height+rowH
		local reward={
		{type=1,num=info.Experience  },{type=2,num=info.GameCoin},
		--{type=3,num=info.Ascended},
		}
		local height=math.ceil(#reward/2)*rewardHeight
		contentHeight=contentHeight+rowH+height
		--设置位置
		if plotType==1  then
			contentLabel:setPosition(PT(startX,contentHeight-contentLabel:getContentSize().height))
			physicalLabel:setPosition(PT(startX,contentLabel:getPosition().y-rowH-physicalLabel:getContentSize().height))
		else	
			physicalLabel:setPosition(PT(startX,contentHeight-physicalLabel:getContentSize().height))
		end;
		levelLabel:setPosition(PT(startX,physicalLabel:getPosition().y-rowH-levelLabel:getContentSize().height))
		timeLabel:setPosition(PT(levelLabel:getPosition().x+levelLabel:getContentSize().width+SX(10),levelLabel:getPosition().y))	
		rewardLabel:setPosition(PT(startX,levelLabel:getPosition().y-rowH-rewardLabel:getContentSize().height))	
		--循环创建奖励
		startX=startX+rewardLabel:getContentSize().width+SX(2)
		local posY=rewardLabel:getPosition().y
		if reward and #reward>0 then
			--"经验" "金币"
			local rewardType={[1]=Language.PLOT_HONOUR,[2]=Language.IDS_GOLD,
			}
			for k, v in pairs(reward ) do
				local pX=startX+((k-1)%2)*contentWidth*0.3
				local pY=posY-math.floor((k-1)/2)*rewardHeight
				local str=rewardType[v.type] .. " +" .. v.num
				local label=CCLabelTTF:create(str,FONT_NAME,FONT_DEF_SIZE)
				label:setAnchorPoint(PT(0,0))
				label:setPosition(PT(pX,pY))
				layer:addChild(label,1)
			end
		end
		
		--查看掉落物
		local contentStr=string.format("<label class='PlotListScene.seeDrop' color='71, 239,32' userdata='%d'>%s</label>",index,Language.PLOT_SEEDROPOUT )
		local contentLabel=ZyMultiLabel:new(contentStr,boxSize.width*0.9,FONT_NAME,FONT_SM_SIZE,nil,nil,true);
		contentLabel:setAnchorPoint(PT(0,0))
		contentLabel:setPosition(PT(boxSize.width*0.8,bgSprite:getPosition().y+rowH*2))
									--rewardLabel:getPosition().y-rowH-contentLabel:getContentSize().height-SY(10)))
		contentLabel:addto(layer,1)
		
		--
		local contentBg=CCSprite:create(P("common/list_1038.9.png"))
		contentHeight=contentHeight+rowH
		contentBg:setScaleX(contentWidth/contentBg:getContentSize().width)
		contentBg:setScaleY(contentHeight/contentBg:getContentSize().height)
		contentBg:setAnchorPoint(PT(0,0))
		contentBg:setPosition(PT(boxSize.width*0.03,0))
		layer:addChild(contentBg,0)	
		---重新设置 任务名称的位置		
		bgSprite:setPosition(PT(0,contentHeight))
		layerHeight=layerHeight+contentHeight
	end
		actionBtn:setPosition(PT(0,bgSprite:getPosition().y))
		plotName:setPosition(PT(startX,bgSprite:getPosition().y+boxSize.height/2))
		gotoBtn:setPosition(PT(boxSize.width*0.8,plotName:getPosition().y-gotoBtn:getContentSize().height/2))
		actionBtn2:setPosition(gotoBtn:getPosition())
							
	return layer,layerHeight
end;

--查看掉落物
function seeDrop(pNode)

    local linklabel=ZyLinkLable.getLingLabel(pNode)
    local index = linklabel:getUserData()[1]
    
   -- mCityPlotData=mCityPlotData[index].CityPlotTable
    local plotID=mCityPlotData[tonumber(index)].PlotID
    local dropItemTable= mCityPlotData[tonumber(index)].DropItemTable
    
	if #dropItemTable>0 then
		
		dropLayer=CCLayer:create()
		mScene:addChild(dropLayer,9)	
		
		--掉落物背景
		local dropBg=CCSprite:create(P("common/list_1054.png"))
		dropBg:setScaleX(pWinSize.width*0.7/dropBg:getContentSize().width)
		dropBg:setScaleY(pWinSize.height*0.5/dropBg:getContentSize().height)
		dropBg:setAnchorPoint(PT(0.5,0))
		dropBg:setPosition(PT(pWinSize.width/2,(pWinSize.height-dropBg:getContentSize().height)/2))
		dropLayer:addChild(dropBg,0)	
		
		--标题
		local titleStr=string.format("<label  color='71, 239,32' fontsize='30'>%s</label>",Language.PLOT_SEEDROPOUT )
		local titleLabel=ZyMultiLabel:new(titleStr,dropBg:getContentSize().width*0.9,FONT_NAME,FONT_SM_SIZE);
		titleLabel:setAnchorPoint(PT(0,0))
		titleLabel:setPosition(PT((pWinSize.width-titleLabel:getContentSize().width)/2,dropBg:getPosition().y+dropBg:getContentSize().height-SY(20)))
		titleLabel:addto(dropLayer,1)
	
		local colW=dropBg:getContentSize().width*0.35
		local posX=(dropBg:getPosition().x)/2-SY(10)
		local posY=dropBg:getPosition().y+dropBg:getContentSize().height-titleLabel:getContentSize().height-SY(10)
		local row=2
		for k , v in ipairs(dropItemTable) do
			local str=v.ItemName.."*"..v.Num
			local label=createLabel(str,dropLayer)
			if rowH==nil then
				rowH=label:getContentSize().height*1.5
			end
			label:setAnchorPoint(PT(0,0))
			local rowIndex=(k-1)%row
			local colIndex=math.floor((k-1)/row)+1
			label:setPosition(PT(posX+rowIndex*colW,posY-colIndex*rowH))
		end
	
		--离开按钮
		local exitBtn=ZyButton:new("button/list_1039.png",nil,nil,
						Language.IDS_QUIT,FONT_NAME,FONT_SM_SIZE)
		exitBtn:setPosition(PT(0,0))
		exitBtn:registerScriptHandler(exitDrops)
		exitBtn:addto(dropLayer,1)
		
		exitBtn:setPosition(PT((pWinSize.width-exitBtn:getContentSize().width)/2,dropBg:getPosition().y+SY(2)))
		
	end;
end;

function exitDrops()
	if dropLayer  then
		mScene:removeChild(dropLayer,true)
	end;
end;

function  createLabel(name,parent,color)
	local label=CCLabelTTF:create(name,FONT_NAME,FONT_SM_SIZE)
	label:setAnchorPoint(PT(0,0))
	if color~=nil then
		label:setColor(color)
	end
	parent:addChild(label,0)
	return label
end;

--前往按钮点击响应
function  enterPlotAction(node, index)

	isAction=true
	local tag=nil
	if index then
		tag = index
	else
		tag = node:getTag()
	end
--	if isGuide then
--		GuideLayer.judgeIsEnterRight(tag)
--	end	
    mChoiceIndex = tag
	if mBackpackType and mBackpackType~=0 then----背包满了不能进行下去
		local box = ZyMessageBoxEx:new()
		box:setTag(tag)
		box:doQuery(mScene,nil,Language.PLOT_TIP1,Language.IDS_SURE,Language.IDS_CANCEL,adkGotoBag)
	else
		enterPlotAction1(tag)
	end
end;

-- 前往副本
function enterPlotAction1(tag)
	if mCityPlotData[tag] then
		if mCityPlotData[tag].ChallengeNum>=mCityPlotData[tag].MaxChallengeNum then
			if isShowVip() then
				if PersonalInfo.getPersonalInfo()._VipLv < 3 then--VIP等级不足3级挑战			

				else
					actionLayer.Action4002(mScene, nil , mCityPlotData[tag].PlotID, 0)
				end
			else
				ZyToast.show(mScene, Language.PLOT_TIP9,1.5,0.35)
			end
		else
			if not isClick then
			isClick =true
			actionLayer.Action4002(mScene, nil , mCityPlotData[tag].PlotID, 0)
			end
		end
	end
end;

--是否使用晶石进行副本
function  adkUseGold(clickedButtonIndex,content,tag) 
	if clickedButtonIndex==ID_MBOK then
		actionLayer.Action4002(mScene, nil , _PlotID, 1)	
	end
end;



--[[
1	装备背包2	佣兵背包3	魂技背包4	普通背包
--]]
--背包满了 跳转
function  adkGotoBag(clickedButtonIndex,content,tag) 
	if clickedButtonIndex==ID_MBOK then
		local MenuId=MainMenuLayer.MenuId
		closeScene()
		local typeTable={[1]=MenuId.eZhuangBei,
		[2]=MenuId.eYongBing,[3]=MenuId.eHunJi,
		[4]=MenuId.eBeiBao,
		}
		local moveId=typeTable[mBackpackType]
		MainMenuLayer.funcAction(nil,moveId)
	else
		enterPlotAction1(tag)
	end
end;


--选中一个副本
function selectItemAction(node, index)
	if isGuide then
		GuideLayer.close()
	end

	if not isAction then
		local tag=nil
		if tag then
			tag = index
		else
			tag = node:getTag()
		end
		for k, v in pairs(mCityPlotData) do
		--for k,v in pairs(eliteData) do
			if k==tag then
				if mChoiceIndex==tag and v.isSelected then
						v.isSelected=nil
				else
						v.isSelected=true
				end	
			else
				v.isSelected=nil
			end	
		end
		mChoiceIndex=tag
		remenberChoice[mCurrentTab] =mChoiceIndex
		refreshListPage()
		--goToElite()
	end
	isAction=false
end;


--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
mListTable={}
--mChoiceIndex=nil
isClick=nil
personalInfo=PersonalInfo.getPersonalInfo()
isFirst=nil--第一次请求4001
eliteBtn=nil
nomalBtn=nil
end

-- 释放资源
function releaseResource()
	AddSpriteLayer.releaseResource()
	mLayer=nil
	mScene=nil
	mListTable=nil
	mList=nil
	mCityPlotData=nil
	mServerData=nil
	mTabBar=nil
--	mChoiceIndex=0
	isClick=nil
	remenberChoice={}
	isFirst=nil--第一次请求4001
	mEnergyLayer=nil
	isGuide=nil
end

function send(actionId,plotType)
	if  actionId ==4001 and plotType then
		actionLayer.Action4001(mScene,nil,nil,plotType)
	end;
end;

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionID = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if actionID == 4002 then
		_PlotID = userData
    		if ZyReader:getResult() == 1  then--返回值1：提示挑战次数达到是否XX晶石开启当前挑战。2：提示晶石不足返回充值页面
--    			local serverInfo=actionLayer._4002Callback(pScutScene, lpExternalData)
--    			local str = string.format(Language.PLOT_COAST, serverInfo.PlotNum)
		 	local box = ZyMessageBoxEx:new()
			box:doQuery(pScutScene,nil,ZyReader:readErrorMsg(),Language.IDS_SURE,Language.IDS_CANCEL,adkUseGold)			
		elseif ZyReader:getResult() == 2  then

		elseif ZyReader:getResult() == 3  then
			--精力不足
			actionLayer.Action1091(mScene,false)			
    		else
			local serverInfo=actionLayer._4002Callback(pScutScene, lpExternalData)
			if serverInfo~=nil then
				remberLastFightInfo()
				
				local PlotID = userData
				battleScene.setPlotMapInfo(serverInfo, PlotID)
				releaseResource()
				battleScene.pushScene()
			--	playMusic(EnumMusicType.fightMusic)
			else
				ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
			end
    		end
	elseif actionID == 1091 then
		local serverInfo=actionLayer._1091Callback(pScutScene, lpExternalData)
		if serverInfo then
			AddSpriteLayer.setInfo(mScene)
			AddSpriteLayer.createEnergyLayer(serverInfo)
		end
	elseif actionID == 4001 then
		local serverInfo=actionLayer._4001Callback(pScutScene, lpExternalData)
		if serverInfo then
			mServerData=serverInfo.RecordTabel
			mBackpackType=serverInfo.BackpackType
			--mCurrentTab=#mServerData
			
			judgeNowItem(serverInfo)
			
			if #mServerData > 0 then 
			    createContentLayer()
			end
		end
	elseif actionID == 1010 then
		AddSpriteLayer.networkCallback(pScutScene, lpExternalData)
	else
		commonCallback.networkCallback(pScutScene, lpExternalData)
	end
	isClick=false
end


function judgeNowItem(serverInfo)


	if mLastTab and mLastIndex then
		mCurrentTab = mLastTab
		mChoiceIndex = mLastIndex
		plotType = mLastPlotType
	else
		if serverInfo.RecordTabel then
			for k,v in ipairs(serverInfo.RecordTabel) do
				for k1,v1 in ipairs(v.CityPlotTable) do
					if v1.PlotStatus ~= 0 then
						mCurrentTab = k
						mChoiceIndex = k1
					end
				end
			end
		end	
	end

	local isTrue,index = GuideLayer.guidePlotIndex()
	if isTrue then
		mCurrentTab = 1
		mChoiceIndex = index
	end
end

function remberLastFightInfo()
	mLastTab = mCurrentTab
	mLastIndex = mChoiceIndex
	mLastPlotType = plotType
end

