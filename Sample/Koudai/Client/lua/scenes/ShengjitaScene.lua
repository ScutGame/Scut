
-- ShengjitaScene.lua.lua
-- Author     :Lysong
-- Version    : 1.0.0.0
-- Date       :
-- Description:
------------------------------------------------------------------

module("ShengjitaScene", package.seeall)

require("scenes.SbattleScene")

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local mScene= nil 
local mLayer=nil
local showLayer=nil
local list1={}
local list2={}
local list0={}
local list3={}
local list4={}
local list5={}
local mRecordTabel={}
local mRecordTabel2={}
local mRecordTabel5={}
local iswin=nil

local position={}
local fatherLayer
local bangtype=nil
local paihangzi=nil
local mtable={}
local jinbi=nil
local ifjinbi=nil


local mtype={
		[1]={pic="shengjita/list_3009_1.png"},--=""
		[2]={pic="shengjita/list_3009_2.png"},--=""
		[3]={pic="shengjita/list_3009_3.png"},--=""
		[4]={pic="shengjita/list_3009_4.png"},--=""
		}

function setLayer(scene, layer)
	fatherLayer = layer
	mScene = scene
end;
--
---------------公有接口(以下两个函数固定不用改变)--------
--

-- 函数名以小写字母开始，每个函数都应注释


-- 退出场景
function close()
	if showLayer ~= nil then
		showLayer:getParent():removeChild(showLayer, true)
		showLayer = nil
	end
	if mLayer then
		mLayer:getParent():removeChild(mLayer, true)
		mLayer = nil
	end
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
	BillboardLayer = nil
	m_infoLayer = nil
	shuxingLayer = nil
	dancenLayer = nil
	duihuanLayer = nil
	lingquLayer = nil
	mLayer = nil
	showLayer=nil
	weidadaoLayer=nil
	rankList=nil
end
-- 创建场景
function init()
	
	
	initResource()
	local layer = CCLayer:create()
	fatherLayer:addChild(layer, 0)
	mLayer= layer
	
	-- 此处添加场景初始内容                  
	local bg1 = "shengjita/map_1005.png"
	local imageBg = CCSprite:create(P(bg1));
	imageBg:setScaleX(pWinSize.width*0.92/imageBg:getContentSize().width)
	imageBg:setScaleY(pWinSize.height*0.7/imageBg:getContentSize().height)
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(pWinSize.width*0.04,pWinSize.height*0.2))
	mLayer:addChild(imageBg,0)
	
	--勇闯圣吉塔艺术字
	local titleBg = CCSprite:create(P("shengjita/list_3008.png"))
	titleBg:setAnchorPoint(PT(0,0))
	titleBg:setPosition(PT((pWinSize.width-titleBg:getContentSize().width)/2, pWinSize.height*0.77))
	mLayer:addChild(titleBg, 0)
	
	actionLayer.Action4401(mScene, nil)
--	showContent()

end
function gotoScene(scene)
    if scene then
        mScene=scene;
        mLayer=CCLayer:create()
        mScene:addChild(mLayer,2)
    end
end
function releseShowLayer()
	if showLayer ~= nil then
		showLayer:getParent():removeChild(showLayer, true)
		showLayer = nil
	end
end

function showContent()
	if showLayer ~= nil then
		showLayer:getParent():removeChild(showLayer, true)
		showLayer = nil
	end
	local layer = CCLayer:create()
	mLayer:addChild(layer, 0)
	showLayer = layer
	
	--立即前往按钮
	if list1.LastBattleRount==0 then
		local getBtn=ZyButton:new(Image.image_button_hui_c,Image.image_button_hui_c,nil,Language.RIGHTGO,FONT_NAME,FONT_SM_SIZE)
		getBtn:setAnchorPoint(PT(0,0))
		getBtn:setPosition(PT((pWinSize.width-getBtn:getContentSize().width)/2,pWinSize.height*0.63))
		getBtn:addto(showLayer, 0)  
	else
		local getBtn=ZyButton:new(Image.image_button_red_c_0,Image.image_button_red_c_1,nil,Language.RIGHTGO,FONT_NAME,FONT_SM_SIZE)
		getBtn:setAnchorPoint(PT(0,0))
		getBtn:setPosition(PT((pWinSize.width-getBtn:getContentSize().width)/2,pWinSize.height*0.63))
		getBtn:registerScriptHandler(goto)
		getBtn:addto(showLayer, 0)   	
	end
	
	--挑战、剩余挑战次数
	local tiaozhan=Language.YITIAOZHAN..list1.BattleRount..Language.CI..","..Language.SHENGYU..list1.LastBattleRount..Language.CIJIHUI
	local challenge = CCLabelTTF:create(tiaozhan,FONT_NAME,FONT_SM_SIZE)
	showLayer:addChild(challenge,1)
	challenge:setAnchorPoint(PT(0,0))
	challenge:setPosition(PT((pWinSize.width-challenge:getContentSize().width)/2,pWinSize.height*0.58))
	
	--今日最高
	local top = CCLabelTTF:create(Language.TODAYTOP_TIP..Language.CENGSHU,FONT_NAME,FONT_SM_SIZE)
	showLayer:addChild(top,1)
	top:setAnchorPoint(PT(0,0))
	top:setPosition(PT(pWinSize.width*0.1,pWinSize.height*0.46))
	--层数字
	local cengshu = CCLabelTTF:create(list1.MaxTierNum,FONT_NAME,FONT_SM_SIZE)
	showLayer:addChild(cengshu,1)
	cengshu:setAnchorPoint(PT(0,0))
	cengshu:setColor(ccGREEN)
	cengshu:setPosition(PT(top:getPosition().x+top:getContentSize().width+SX(7),pWinSize.height*0.46))
	--星星
	local starImg=CCSprite:create(P("mainUI/list_1036.png"))
	starImg:setAnchorPoint(PT(0,0))
	starImg:setPosition(PT(cengshu:getPosition().x+cengshu:getContentSize().width+SX(10),pWinSize.height*0.45))
	showLayer:addChild(starImg,0)
	--星星数字
	local xingshu = CCLabelTTF:create(list1.ScoreStar,FONT_NAME,FONT_SM_SIZE)
	showLayer:addChild(xingshu,1)
	xingshu:setAnchorPoint(PT(0,0))
	xingshu:setColor(ccGREEN)
	xingshu:setPosition(PT(starImg:getPosition().x+starImg:getContentSize().width+SX(7),pWinSize.height*0.46))
	
	--当前排名
	local dangqianpaiming = CCLabelTTF:create(Language.RIGHTNOW,FONT_NAME,FONT_SM_SIZE)
	showLayer:addChild(dangqianpaiming,1)
	dangqianpaiming:setAnchorPoint(PT(0,0))
	dangqianpaiming:setPosition(PT(pWinSize.width*0.1,pWinSize.height*0.41))
	--排名
	if list1.UserRank>20 then
		paiming = CCLabelTTF:create(Language.PAIMINGZHIWAI,FONT_NAME,FONT_SM_SIZE)
		showLayer:addChild(paiming,1)
		paiming:setAnchorPoint(PT(0,0))
		paiming:setColor(ccRED)
		paiming:setPosition(PT(dangqianpaiming:getPosition().x+dangqianpaiming:getContentSize().width,pWinSize.height*0.41))
	else
		paiming = CCLabelTTF:create(list1.UserRank,FONT_NAME,FONT_SM_SIZE)
		showLayer:addChild(paiming,1)
		paiming:setAnchorPoint(PT(0,0))
		paiming:setColor(ccRED)
		paiming:setPosition(PT(dangqianpaiming:getPosition().x+dangqianpaiming:getContentSize().width,pWinSize.height*0.41))
	end
	--奖励
	local jiangli = CCLabelTTF:create(Language.JINRUPAIHANG,FONT_NAME,FONT_SM_SIZE)
	showLayer:addChild(jiangli,1)
	jiangli:setAnchorPoint(PT(0,0))
	jiangli:setPosition(PT(paiming:getPosition().x+paiming:getContentSize().width,pWinSize.height*0.41))
	
	--排名按钮
	local paiBtn=ZyButton:new("shengjita/list_3002_1.png","shengjita/list_3002_2.png",nil,nil,FONT_NAME,FONT_SM_SIZE)
	paiBtn:setAnchorPoint(PT(0,0))
	paiBtn:setPosition(PT(pWinSize.width*0.7,pWinSize.height*0.25))
	paiBtn:registerScriptHandler(paimingAction)
	paiBtn:addto(showLayer, 0)  
--[[	if PersonalInfo.getPersonalInfo().Shengjita==1 then
		if PersonalInfo.getPersonalInfo().Exchange==0 and PersonalInfo.getPersonalInfo().Receive~=0 then
			actionLayer.Action4407(mScene, nil,1)
			PersonalInfo.getPersonalInfo().Shengjita=0
		elseif PersonalInfo.getPersonalInfo().Receive==0 then
			actionLayer.Action4409(mScene, nil)
			PersonalInfo.getPersonalInfo().Shengjita=0
		end
	end
	--]]
end

--
function paimingAction()
--	actionLayer.Action4411(mScene, nil,1)
	Billboard()
end

--排行榜
function removeBillboard()
    if BillboardLayer ~= nil then
        BillboardLayer:getParent():removeChild(BillboardLayer,true)
        BillboardLayer = nil
    end
end

function initBillboard()
    removeBillboard()
    BillboardLayer=CCLayer:create()
    BillboardLayer:setAnchorPoint(PT(0,0));
    BillboardLayer:setPosition(PT(0,0));
    fatherLayer:addChild(BillboardLayer,2)
end

function Billboard()
	initBillboard()
	--创建背景
	local imageSize = SZ(pWinSize.width,pWinSize.height*0.855)
	local imageBg = CCSprite:create(P(Image.ImageBackground))
	imageBg:setScaleX(imageSize.width/imageBg:getContentSize().width)
	imageBg:setScaleY(imageSize.height/imageBg:getContentSize().height)
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(0, pWinSize.height*0.145))
	BillboardLayer:addChild(imageBg, 0)
	
	--屏蔽按钮ceng
	local unTouchBtn =UIHelper.createActionRect(imageSize)
	unTouchBtn:setPosition(PT(0,imageBg:getPosition().y))
	BillboardLayer:addChild(unTouchBtn,0)
 

	--关闭按钮
	local topupBtn= ZyButton:new(Image.image_close)
	topupBtn:setAnchorPoint(PT(0,0))
	topupBtn:setPosition(PT(pWinSize.width*0.86,pWinSize.height*0.91))
	topupBtn:registerScriptHandler(closepaihang)
	topupBtn:addto(BillboardLayer,0)
	
	--排行榜艺术字
	local titleBg = CCSprite:create(P("shengjita/list_3017.png"))
	titleBg:setAnchorPoint(PT(0,0))
	titleBg:setPosition(PT((pWinSize.width-titleBg:getContentSize().width)/2, pWinSize.height*0.91))
	BillboardLayer:addChild(titleBg, 0)
	
	local  p_itemDataList={
    	 	{name=Language.QINGTONG},--1
    	 	{name=Language.BAIYIN},--2
    	 	{name=Language.HUANGJIN},--3
    	 }
    	 mtable=p_itemDataList
	local rowH=pWinSize.height*0.75/5
	for  k, v in pairs(p_itemDataList) do
		shuxingBtn= ZyButton:new("shengjita/list_3017_1.png", "shengjita/list_3017_2.png", nil,p_itemDataList[k].name,nil, FONT_SM_SIZE)
		shuxingBtn:setAnchorPoint(PT(0,0))
		local chax=(pWinSize.width*0.92/3-shuxingBtn:getContentSize().width)/2
--		shuxingBtn:setPosition(PT(pWinSize.width*0.1+rowH*(k-1)*1.2,pWinSize.height*0.8))
		shuxingBtn:setPosition(PT(pWinSize.width*0.04+chax+(k-1)*(chax*1.5+shuxingBtn:getContentSize().width),pWinSize.height*0.8))
		shuxingBtn:setTag(k)
		shuxingBtn:registerScriptHandler(paihangAction)
		shuxingBtn:addto(BillboardLayer,1)
		v.clickBtn=shuxingBtn
	--Index=v
    	end
	
	local bg1 = "shengjita/map_1005.png"
	local imageBg = CCSprite:create(P(bg1));
	imageBg:setScaleX(pWinSize.width*0.92/imageBg:getContentSize().width)
	imageBg:setScaleY(pWinSize.height*0.7/imageBg:getContentSize().height)
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(pWinSize.width*0.04,pWinSize.height*0.21))
	BillboardLayer:addChild(imageBg,0)
	
	local upBg = CCSprite:create(P("common/list_1038.9.png"))
	upBg:setScaleX(imageSize.width/upBg:getContentSize().width*0.8)
	upBg:setScaleY(imageSize.height/upBg:getContentSize().height*0.1)
	upBg:setAnchorPoint(PT(0,0))
	upBg:setPosition(PT(pWinSize.width*0.1, pWinSize.height*0.7))
	BillboardLayer:addChild(upBg, 0)
--	PAIHANGBANGTISHI
	local taskLabel=CCLabelTTF:create(Language.PAIHANGBANGTISHI,FONT_NAME,FONT_SM_SIZE)
	taskLabel:setAnchorPoint(PT(0,0))
	taskLabel:setPosition(PT(pWinSize.width*0.13, pWinSize.height*0.735))
	BillboardLayer:addChild(taskLabel,0)
	
	bangtype=1
	paihangzi=1
	
	list()
	actionLayer.Action4411(mScene, nil)
--	refreshList()
	mtable[1].clickBtn:selected()
end

function paihangAction(node)
	bangtype=node:getTag()
	for k, v in pairs(mtable) do
		if k==bangtype then
			v.clickBtn:selected()
		else
			v.clickBtn:unselected()
		end
	end
	paihangzi=1
	refreshList()
--	actionLayer.Action4411(mScene, nil,index)
end;

function removeinfo()
    if m_infoLayer ~= nil then
        m_infoLayer:getParent():removeChild(m_infoLayer,true)
        m_infoLayer = nil
    end
end

function initinfo()
    removeinfo()
    m_infoLayer=CCLayer:create()
    m_infoLayer:setAnchorPoint(PT(0,0));
    m_infoLayer:setPosition(PT(0,0));
    fatherLayer:addChild(m_infoLayer,2)
end

function list()
	initinfo()
	if rankList~=nil then
		rankList:clear()
	end
	local boxSize=SZ(pWinSize.width*0.8,pWinSize.height*0.5)
       local start_x=pWinSize.width*0.1
	local start_y=pWinSize.height*0.1
	rankList=ScutCxList:node(boxSize.width ,ccc4(25, 25, 25, 25),boxSize)
	m_infoLayer:addChild(rankList,0)
	rankList:setAnchorPoint(PT(0,0))
	rankList:setPosition(PT(start_x*1,start_y*2))
	rankList:setHorizontal(false)
	list:setTouchEnabled(true)
end



function  refreshList()
	if rankList~=nil then
		rankList:clear()
	end
	local  p_itemDataList={
    	 	{pic1="common/list_1203.png"},--1
    	 	{pic1="common/list_1204.png"},--2
    	 	{pic1="common/list_1205.png"},--3
    	 	{pic1="common/list_1206.png"},--4
    	 	{pic1="common/list_1207.png"},--5
    	 	{pic1="common/list_1208.png"},--6
    	 	{pic1="common/list_1209.png"},--7
    	 	{pic1="common/list_1210.png"},--8
    	 	{pic1="common/list_1211.png"},--9
    	 	{pic1="common/list_1203.png",pic2="common/list_1202.png"},--10
    	 	{pic1="common/list_1203.png",pic2="common/list_1203.png"},--11
    	 	{pic1="common/list_1203.png",pic2="common/list_1204.png"},--12
    	 	{pic1="common/list_1203.png",pic2="common/list_1205.png"},--13
    	 	{pic1="common/list_1203.png",pic2="common/list_1206.png"},--14
    	 	{pic1="common/list_1203.png",pic2="common/list_1207.png"},--15
    	 	{pic1="common/list_1203.png",pic2="common/list_1208.png"},--16
    	 	{pic1="common/list_1203.png",pic2="common/list_1209.png"},--17
    	 	{pic1="common/list_1203.png",pic2="common/list_1210.png"},--18
    	 	{pic1="common/list_1203.png",pic2="common/list_1211.png"},--19
    	 	{pic1="common/list_1204.png",pic2="common/list_1202.png"},--20
    	 }
    	 --]]
	for k, v in pairs(list0) do
--	for k, v in pairs(p_itemDataList) do
		local listItem=ScutCxListItem:itemWithColor(ccc3(42,28,13))
		listItem:setOpacity(0)
		listItem:setMargin(CCSize(0,0));
		local mlayout=CxLayout()
		mlayout.val_x.t = ABS_WITH_PIXEL
		mlayout.val_y.t = ABS_WITH_PIXEL
		mlayout.val_x.val.pixel_val =0
		mlayout.val_y.val.pixel_val =0
		local layer = CCLayer:create();
		if list0[k].SJTRankType==bangtype then
			--框
			local imageSize = SZ(pWinSize.width,pWinSize.height*0.855)
			local upBg = CCSprite:create(P("common/list_1038.9.png"))
			upBg:setScaleX(imageSize.width/upBg:getContentSize().width*0.8)
			upBg:setScaleY(imageSize.height/upBg:getContentSize().height*0.12)
			upBg:setAnchorPoint(PT(0,0))
			upBg:setPosition(PT(0,0))
			layer:addChild(upBg, 0)
			--排名数字
			local titleBg = CCSprite:create(P(p_itemDataList[paihangzi].pic1))
			titleBg:setAnchorPoint(PT(0,0))
			titleBg:setPosition(PT(pWinSize.width*0.05, pWinSize.height*0.025))
			layer:addChild(titleBg, 0)
			if paihangzi>9 then
				local titleBg1 = CCSprite:create(P(p_itemDataList[paihangzi].pic2))
				titleBg1:setAnchorPoint(PT(0,0))
				titleBg1:setPosition(PT(titleBg:getPosition().x+titleBg:getContentSize().width, pWinSize.height*0.025))
				layer:addChild(titleBg1, 0)
			end
			--字
			local taskLabel=CCLabelTTF:create(list0[k].NickName.."  "..Language.DENGJI..list0[k].UserLv,FONT_NAME,FONT_SM_SIZE)
	--		local taskLabel=CCLabelTTF:create("123456",FONT_NAME,FONT_SM_SIZE)
			taskLabel:setAnchorPoint(PT(0,0))
			taskLabel:setPosition(PT(pWinSize.width*0.18, pWinSize.height*0.07))
			layer:addChild(taskLabel,0)
			local taskLabel=CCLabelTTF:create(Language.DEXING..list0[k].ScoreStar,FONT_NAME,FONT_SM_SIZE)
			taskLabel:setAnchorPoint(PT(0,0))
			taskLabel:setPosition(PT(pWinSize.width*0.18, pWinSize.height*0.04))
			layer:addChild(taskLabel,0)
			local taskLabel=CCLabelTTF:create(Language.LIANXUJINBANG..list0[k].HaveRankNum..Language.TIAN,FONT_NAME,FONT_SM_SIZE)
			taskLabel:setAnchorPoint(PT(0,0))
			taskLabel:setPosition(PT(pWinSize.width*0.18, pWinSize.height*0.01))
			layer:addChild(taskLabel,0)
			
			--通关层数
			local paiBtn=ZyButton:new("mainUI/list_1147.png","mainUI/list_1147.png",nil,list0[k].MaxTierNum,FONT_NAME,FONT_SM_SIZE)
			paiBtn:setAnchorPoint(PT(0,0))
			paiBtn:setPosition(PT(pWinSize.width*0.45,pWinSize.height*0.02))
			paiBtn:addto(layer, 0) 
			
			--阵容按钮
			--local paiBtn=ZyButton:new(Image.image_button,Image.image_button,nil,nil,FONT_NAME,FONT_SM_SIZE)
			local paiBtn=ZyButton:new(Image.image_button,nil,nil,Language.ZHENGRONG,FONT_NAME,FONT_SM_SIZE)
			paiBtn:setAnchorPoint(PT(0,0))
			paiBtn:setPosition(PT(pWinSize.width*0.6,pWinSize.height*0.03))
			paiBtn:setTag(k)
			paiBtn:registerScriptHandler(zhenrong)
			paiBtn:addto(layer, 0)  
			
			paihangzi=paihangzi+1---排行数字
		    	
		       rankList:setRowHeight(paiBtn:getContentSize().height*2.05)
		       listItem:addChildItem(layer)
		       rankList:addListItem(listItem, false)
		end
	end
	
end

function zhenrong(pNode)
	
	local tag = pNode:getTag()
	local playerId = list0[tag].UserID 
    rankList=nil  
	HeroScene.pushScene(nil, playerId)
	
end;

function closepaihang()
	rankList=nil
	removeinfo()
	removeBillboard()
end

--属性加成界面
function goto()
	if list1.LastBattleRount~=0 then
		releseShowLayer()
		actionLayer.Action4402(mScene, nil)
	end
--	showshuxing()
end;

function releseshuxing()
	if shuxingLayer ~= nil then
		shuxingLayer:getParent():removeChild(shuxingLayer, true)
		shuxingLayer = nil
	end
end

function showshuxing()
	
	if shuxingLayer ~= nil then
		shuxingLayer:getParent():removeChild(shuxingLayer, true)
		shuxingLayer = nil
	end
	
	local layer = CCLayer:create()
	mLayer:addChild(layer, 0)
	
	shuxingLayer = layer
	
	--您上次单轮
	shangci=Language.NINSHANGCI..list2.StarNum
	local challenge = CCLabelTTF:create(shangci,FONT_NAME,FONT_SM_SIZE)
	shuxingLayer:addChild(challenge,1)
	challenge:setAnchorPoint(PT(0,0))
	challenge:setPosition(PT(pWinSize.width*0.13,pWinSize.height*0.7))
	--星星
	local starImg=CCSprite:create(P("mainUI/list_1036.png"))
	starImg:setAnchorPoint(PT(0,0))
	starImg:setPosition(PT(challenge:getPosition().x+challenge:getContentSize().width+SX(10),pWinSize.height*0.69))
	shuxingLayer:addChild(starImg,0)
	--星星数字
	local xingshu = CCLabelTTF:create(Language.KETIQIAN,FONT_NAME,FONT_SM_SIZE)
	shuxingLayer:addChild(xingshu,1)
	xingshu:setAnchorPoint(PT(0,0))
	xingshu:setColor(ccGREEN)
	xingshu:setPosition(PT(starImg:getPosition().x+starImg:getContentSize().width+SX(7),pWinSize.height*0.7))
	--翅膀
	local starImg=CCSprite:create(P("shengjita/list_3014.png"))
	starImg:setAnchorPoint(PT(0,0))
	starImg:setPosition(PT((pWinSize.width-starImg:getContentSize().width)/2,pWinSize.height*0.59))
	shuxingLayer:addChild(starImg,0)
	--加成
	local jiacheng = CCLabelTTF:create(Language.JIACHENG..list2.EffNum.."%",FONT_NAME,FONT_BIG_SIZE)
	shuxingLayer:addChild(jiacheng,1)
	jiacheng:setAnchorPoint(PT(0,0))
	jiacheng:setColor(ccBLACK)
	jiacheng:setPosition(PT((pWinSize.width-jiacheng:getContentSize().width)/2,pWinSize.height*0.6))
	--选择一项属性开始挑战
	local xuanzesx = CCLabelTTF:create(Language.XUANZEYIXIANG,FONT_NAME,FONT_SM_SIZE)
	shuxingLayer:addChild(xuanzesx,1)
	xuanzesx:setAnchorPoint(PT(0,0))
	xuanzesx:setPosition(PT((pWinSize.width-xuanzesx:getContentSize().width)/2,pWinSize.height*0.5))
	--属性按钮
	local rowH=pWinSize.height*0.75/5
	for  k=1, 4  do
		local shuxingBtn= ZyButton:new("shengjita/list_3007_1.png", "shengjita/list_3007_2.png", nil,nil,nil, FONT_SM_SIZE)
		local kong=(pWinSize.width/4-shuxingBtn:getContentSize().width)/2
		shuxingBtn:setAnchorPoint(PT(0,0))
--		shuxingBtn:setPosition(PT(pWinSize.width*0.08+rowH*(k-1)*1,pWinSize.height*0.3))
		shuxingBtn:setPosition(PT(kong*1.6+(kong*1.5+shuxingBtn:getContentSize().width)*(k-1),pWinSize.height*0.3))
		shuxingBtn:setTag(k)
		shuxingBtn:registerScriptHandler(shuxingAction)
		shuxingBtn:addto(shuxingLayer,1)
	--Index=v
    	end
	local  shuxingtu={
    	 	{pic="shengjita/list_3009_1.png"},
    	 	{pic="shengjita/list_3009_2.png"},
    	 	{pic="shengjita/list_3009_3.png"},
    	 	{pic="shengjita/list_3009_4.png"},
    	 }
	for k, v in pairs(shuxingtu) do
		local shuxingImg=CCSprite:create(P(v.pic))
		local kong=(pWinSize.width/4-shuxingImg:getContentSize().width)/2
		shuxingImg:setAnchorPoint(PT(0,0))
		shuxingImg:setPosition(PT(kong*1.3+(kong*1.75+shuxingImg:getContentSize().width)*(k-1),pWinSize.height*0.33))
		shuxingLayer:addChild(shuxingImg,1)
	--Index=v
    	end
	
end

--单层界面
function shuxingAction(node)
	releseshuxing()
	local index=node:getTag()
	PersonalInfo.getPersonalInfo().EffNum=list2.EffNum
	actionLayer.Action4403(mScene, nil,index,list2.EffNum)
--	showdancen()
end

function relesedancen()
	if dancenLayer ~= nil then
		dancenLayer:getParent():removeChild(dancenLayer, true)
		dancenLayer = nil
	end
end

function showdancen()
	
	if dancenLayer ~= nil then
		dancenLayer:getParent():removeChild(dancenLayer, true)
		dancenLayer = nil
	end
	
	local layer = CCLayer:create()
	mLayer:addChild(layer, 0)
	
	dancenLayer = layer
	
	--第几层
	ceng=Language.DI..list3.PlotID..Language.CENG
	local naceng = CCLabelTTF:create(ceng,FONT_NAME,FONT_SM_SIZE)
	dancenLayer:addChild(naceng,1)
	naceng:setAnchorPoint(PT(0,0))
	naceng:setPosition(PT(pWinSize.width*0.13,pWinSize.height*0.73))
	--得分
	ceng=Language.DEFEN..list3.IsRountStar
	local defen = CCLabelTTF:create(ceng,FONT_NAME,FONT_SM_SIZE)
	dancenLayer:addChild(defen,1)
	defen:setAnchorPoint(PT(0,0))
	defen:setPosition(PT(naceng:getPosition().x+naceng:getContentSize().width+SX(20),naceng:getPosition().y))
	--星星
	local starImg=CCSprite:create(P("mainUI/list_1036.png"))
	starImg:setAnchorPoint(PT(0,0))
	starImg:setPosition(PT(defen:getPosition().x+defen:getContentSize().width+SX(5),pWinSize.height*0.72))
	dancenLayer:addChild(starImg,0)
	--剩余
	sheng=Language.SHENGYU..list3.LastScoreStar
	local shengyu = CCLabelTTF:create(sheng,FONT_NAME,FONT_SM_SIZE)
	dancenLayer:addChild(shengyu,1)
	shengyu:setAnchorPoint(PT(0,0))
	shengyu:setPosition(PT(starImg:getPosition().x+starImg:getContentSize().width+SX(20),pWinSize.height*0.73))
	--星星
	local starImg=CCSprite:create(P("mainUI/list_1036.png"))
	starImg:setAnchorPoint(PT(0,0))
	starImg:setPosition(PT(shengyu:getPosition().x+shengyu:getContentSize().width+SX(5),pWinSize.height*0.72))
	dancenLayer:addChild(starImg,0)
	--每隔5层
	local meige = CCLabelTTF:create(Language.MEIGE,FONT_NAME,FONT_SM_SIZE)
	dancenLayer:addChild(meige,1)
	meige:setAnchorPoint(PT(0,0))
	meige:setPosition(PT(starImg:getPosition().x+starImg:getContentSize().width+SX(10),pWinSize.height*0.73))
	--中间块
	local starImg=CCSprite:create(P("shengjita/list_3015.png"))
	starImg:setAnchorPoint(PT(0,0))
	starImg:setPosition(PT((pWinSize.width-starImg:getContentSize().width)/2,pWinSize.height*0.57))
	dancenLayer:addChild(starImg,0)
	--中间左块
	local starImg1=CCSprite:create(P("common/list_1012.png"))
	starImg1:setAnchorPoint(PT(0,0))
	starImg1:setPosition(PT(pWinSize.width*0.25,pWinSize.height*0.59))
	dancenLayer:addChild(starImg1,0)
	--头像
	local image  = string.format("smallitem/%s.png",list3.HeadID)
	local starImg=CCSprite:create(P(image))
	starImg:setAnchorPoint(PT(0,0))
	starImg:setPosition(PT(pWinSize.width*0.26,pWinSize.height*0.6))
	dancenLayer:addChild(starImg,0)
	--名称
	local name = CCLabelTTF:create(list3.MonsterName,FONT_NAME,FONT_DEF_SIZE)
	dancenLayer:addChild(name,1)
	name:setAnchorPoint(PT(0,0))
--	name:setPosition(PT(,pWinSize.height*0.67))
	name:setPosition(PT(pWinSize.width*0.5,starImg1:getPosition().y+starImg1:getContentSize().height-SY(10)))
	--守关怪物数
	local name = CCLabelTTF:create(Language.SHOUGUANGUAIWU..list3.MonsterNum,FONT_NAME,FONT_DEF_SIZE)
	dancenLayer:addChild(name,1)
	name:setAnchorPoint(PT(0,0))
	name:setPosition(PT(pWinSize.width*0.46,pWinSize.height*0.61))
	local  zhandou={
    	 	{pic_1="shengjita/list_3003_1.png",pic_2="shengjita/list_3003_2.png"},
    	 	{pic_1="shengjita/list_3004_1.png",pic_2="shengjita/list_3004_2.png"},
    	 	{pic_1="shengjita/list_3005_1.png",pic_2="shengjita/list_3005_2.png"},
    	}
    	local rowH=pWinSize.height*0.75/5
	for k, v in pairs(zhandou) do
		local zhandouBtn= ZyButton:new(v.pic_1, v.pic_2, nil,nil,nil, FONT_SM_SIZE)
		zhandouBtn:setAnchorPoint(PT(0,0))
		zhandouBtn:setPosition(PT(pWinSize.width*0.3,pWinSize.height*0.5-rowH*(k-1)*0.6))
		zhandouBtn:setTag(k)
		zhandouBtn:registerScriptHandler(zhandouAction)
		zhandouBtn:addto(dancenLayer,1)
		--数值
		value=mRecordTabel[k].DifficultNum*100
		local shuxingzi=CCLabelTTF:create(value.."%"..Language.SHILI,FONT_NAME,FONT_SM_SIZE)
		shuxingzi:setAnchorPoint(PT(0,0))
		shuxingzi:setPosition(PT(pWinSize.width*0.49,pWinSize.height*0.55-rowH*(k-1)*0.6))
		dancenLayer:addChild(shuxingzi,1)
		local shuxingzi=CCLabelTTF:create(mRecordTabel[k].DifficultyType..Language.BEIDEFEN,FONT_NAME,FONT_SM_SIZE)
		shuxingzi:setAnchorPoint(PT(0,0))
		shuxingzi:setPosition(PT(pWinSize.width*0.49,pWinSize.height*0.52-rowH*(k-1)*0.6))
		dancenLayer:addChild(shuxingzi,1)
	--Index=v
    	end
    	--属性加成
	local shuxingjc = CCLabelTTF:create(Language.SHUXINGJIACHENG,FONT_NAME,FONT_SM_SIZE)
	dancenLayer:addChild(shuxingjc,1)
	shuxingjc:setAnchorPoint(PT(0,0))
	shuxingjc:setPosition(PT(pWinSize.width*0.1,pWinSize.height*0.27))
	--图
	local  shuxingtu={
    	 	{pic="shengjita/list_3010_1.png"},
    	 	{pic="shengjita/list_3010_2.png"},
    	 	{pic="shengjita/list_3010_3.png"},
    	 	{pic="shengjita/list_3010_4.png"},
    	 }
	for k, v in pairs(shuxingtu) do
		local shuxingImg=CCSprite:create(P(v.pic))
		shuxingImg:setAnchorPoint(PT(0,0))
		shuxingImg:setPosition(PT(pWinSize.width*0.25+rowH*(k-1)*0.6,pWinSize.height*0.265))
		dancenLayer:addChild(shuxingImg,1)
	--Index=v
    	end
    	--字
    	local  shuxingtu={
    	 	{shuzi=list3.LifeNum},
    	 	{shuzi=list3.PhyNum},
    	 	{shuzi=list3.MagNum},
    	 	{shuzi=list3.AbiNum},
    	 }
	for k, v in pairs(shuxingtu) do
		local shuxingzi=CCLabelTTF:create("+"..v.shuzi,FONT_NAME,FONT_SM_SIZE)
		shuxingzi:setAnchorPoint(PT(0,0))
		shuxingzi:setPosition(PT(pWinSize.width*0.31+rowH*(k-1)*0.6,pWinSize.height*0.27))
		dancenLayer:addChild(shuxingzi,1)
	--Index=v
    	end
    	--今日最高
    	local jinrizuigao = CCLabelTTF:create(Language.JINRIZUIGAO,FONT_NAME,FONT_SM_SIZE)
	dancenLayer:addChild(jinrizuigao,1)
	jinrizuigao:setAnchorPoint(PT(0,0))
	jinrizuigao:setPosition(PT(pWinSize.width*0.1,pWinSize.height*0.23))
	
	local xingshu = CCLabelTTF:create(list3.MaxTierNum,FONT_NAME,FONT_SM_SIZE)
	dancenLayer:addChild(xingshu,1)
	xingshu:setAnchorPoint(PT(0,0))
	xingshu:setPosition(PT(jinrizuigao:getPosition().x+jinrizuigao:getContentSize().width,pWinSize.height*0.23))
	xingshu:setColor(ccGREEN)
	
	local defen = CCLabelTTF:create(Language.DEFEN,FONT_NAME,FONT_SM_SIZE)
	dancenLayer:addChild(defen,1)
	defen:setAnchorPoint(PT(0,0))
	defen:setPosition(PT(pWinSize.width*0.42,pWinSize.height*0.23))
	
	local xingshu = CCLabelTTF:create(list3.ScoreStar,FONT_NAME,FONT_SM_SIZE)
	dancenLayer:addChild(xingshu,1)
	xingshu:setAnchorPoint(PT(0,0))
	xingshu:setPosition(PT(defen:getPosition().x+defen:getContentSize().width,pWinSize.height*0.23))
	xingshu:setColor(ccGREEN)
	
	--星星
	local starImg=CCSprite:create(P("mainUI/list_1036.png"))
	starImg:setAnchorPoint(PT(0,0))
	starImg:setPosition(PT(xingshu:getPosition().x+xingshu:getContentSize().width+SX(5),pWinSize.height*0.22))
	dancenLayer:addChild(starImg,0)
	
end

function zhandouAction(node)
	if node:getTag()==1 then
		actionLayer.Action4405(mScene, nil,mRecordTabel[1].DifficultyType,mRecordTabel[1].DifficultNum,list3.PlotID)
	elseif node:getTag()==2 then
		actionLayer.Action4405(mScene, nil,mRecordTabel[2].DifficultyType,mRecordTabel[2].DifficultNum,list3.PlotID)
	elseif node:getTag()==3 then
		actionLayer.Action4405(mScene, nil,mRecordTabel[3].DifficultyType,mRecordTabel[3].DifficultNum,list3.PlotID)
	end
end

--属性兑换界面
function releseduihuan()
	if duihuanLayer ~= nil then
		duihuanLayer:getParent():removeChild(duihuanLayer, true)
		duihuanLayer = nil
	end
end

function showduihuan()
	
	if duihuanLayer ~= nil then
		duihuanLayer:getParent():removeChild(duihuanLayer, true)
		duihuanLayer = nil
	end
	
	local layer = CCLayer:create()
	mLayer:addChild(layer, 0)
	
	duihuanLayer = layer
	
	--第几层
	ceng=Language.YINGCHUANGGUO.."  "..list4.LayerNum.."  "..Language.CENG
	local naceng = CCLabelTTF:create(ceng,FONT_NAME,FONT_SM_SIZE)
	duihuanLayer:addChild(naceng,1)
	naceng:setAnchorPoint(PT(0,0))
	naceng:setPosition(PT(pWinSize.width*0.13,pWinSize.height*0.7))
	
	--得分
	ceng=Language.DEFEN..list4.StarNum
	local defen = CCLabelTTF:create(ceng,FONT_NAME,FONT_SM_SIZE)
	duihuanLayer:addChild(defen,1)
	defen:setAnchorPoint(PT(0,0))
	defen:setPosition(PT(pWinSize.width*0.5,naceng:getPosition().y))
	--星星
	local starImg=CCSprite:create(P("mainUI/list_1036.png"))
	starImg:setAnchorPoint(PT(0,0))
	starImg:setPosition(PT(defen:getPosition().x+defen:getContentSize().width+SX(5),pWinSize.height*0.69))
	duihuanLayer:addChild(starImg,0)
	--剩余
	ceng=Language.SHENGYU..list4.SulplusNum
	local shengyu = CCLabelTTF:create(ceng,FONT_NAME,FONT_SM_SIZE)
	duihuanLayer:addChild(shengyu,1)
	shengyu:setAnchorPoint(PT(0,0))
	shengyu:setPosition(PT(starImg:getPosition().x+starImg:getContentSize().width+SX(5),naceng:getPosition().y))
	--星星
	local starImg=CCSprite:create(P("mainUI/list_1036.png"))
	starImg:setAnchorPoint(PT(0,0))
	starImg:setPosition(PT(shengyu:getPosition().x+shengyu:getContentSize().width+SX(5),pWinSize.height*0.69))
	duihuanLayer:addChild(starImg,0)
	
	--兑换属性
	local starImg=CCSprite:create(P("shengjita/list_3011.png"))
	starImg:setAnchorPoint(PT(0,0))
	starImg:setPosition(PT((pWinSize.width-starImg:getContentSize().width)/2,pWinSize.height*0.63))
	duihuanLayer:addChild(starImg,0)
	--选择属性兑换
	local shuxingdh = CCLabelTTF:create(Language.CHOOSESHUXING,FONT_NAME,FONT_SM_SIZE)
	duihuanLayer:addChild(shuxingdh,1)
	shuxingdh:setAnchorPoint(PT(0,0))
	shuxingdh:setColor(ccRED)
	shuxingdh:setPosition(PT((pWinSize.width-shuxingdh:getContentSize().width)/2,pWinSize.height*0.27))
	--框
	local imageSize = SZ(pWinSize.width,pWinSize.height*0.855)
	local kongge=pWinSize.width/3-imageSize.width*0.25
	local rowH=pWinSize.height*0.75/5
	local  shuxingtu={
    	 	{pic="shengjita/list_3016.9.png"},
    	 	{pic="shengjita/list_3016.9.png"},
    	 	{pic="shengjita/list_3016.9.png"},
    	 }
	for k, v in pairs(shuxingtu) do
		local upBg=CCSprite:create(P(v.pic))
		upBg:setScaleX(imageSize.width/upBg:getContentSize().width*0.25)
		upBg:setScaleY(imageSize.height/upBg:getContentSize().height*0.36)
		upBg:setAnchorPoint(PT(0,0))
--		upBg:setPosition(PT(pWinSize.width*0.11+rowH*(k-1)*1.2,pWinSize.height*0.3))
		upBg:setPosition(PT(kongge+(kongge/2+imageSize.width*0.25)*(k-1),pWinSize.height*0.3))
		duihuanLayer:addChild(upBg,1)
	--Index=v
    	end
    	--血物魔魂
    	local  image={
    	 	{pic=mtype[mRecordTabel2[1].PropertyType].pic},
    	 	{pic=mtype[mRecordTabel2[2].PropertyType].pic},
    	 	{pic=mtype[mRecordTabel2[3].PropertyType].pic},
    	 }
	for k, v in pairs(image) do
		local imageSize = SZ(pWinSize.width,pWinSize.height*0.855)
		local upBg=CCSprite:create(P(v.pic))
		upBg:setAnchorPoint(PT(0,0))
		upBg:setPosition(PT(kongge*1.7+(kongge/2+imageSize.width*0.25)*(k-1),pWinSize.height*0.5))
		duihuanLayer:addChild(upBg,1)
	--Index=v
    	end
    	local  ziti={
    	 	{name=mRecordTabel2[1].EffNum},
    	 	{name=mRecordTabel2[2].EffNum},
    	 	{name=mRecordTabel2[3].EffNum},
    	 }
	for k, v in pairs(ziti) do
		local upBg=CCLabelTTF:create("+"..v.name,FONT_NAME,FONT_BIG_SIZE)
		upBg:setAnchorPoint(PT(0,0))
		upBg:setPosition(PT(kongge*1.8+(kongge/2+imageSize.width*0.25)*(k-1),pWinSize.height*0.45))
		duihuanLayer:addChild(upBg,1)
	--Index=v
    	end
    	local  xing={
    	 	{name=mRecordTabel2[1].DemandNum},
    	 	{name=mRecordTabel2[2].DemandNum},
    	 	{name=mRecordTabel2[3].DemandNum},
    	 }
	for k, v in pairs(xing) do
		local upBg=CCLabelTTF:create("+"..v.name,FONT_NAME,FONT_BIG_SIZE)
		upBg:setAnchorPoint(PT(0,0))
		upBg:setPosition(PT(kongge*1.8+(kongge/2+imageSize.width*0.25)*(k-1),pWinSize.height*0.4))
		duihuanLayer:addChild(upBg,1)
		local upBg1=CCSprite:create(P("mainUI/list_1036.png"))
		upBg1:setAnchorPoint(PT(0,0))
		upBg1:setPosition(PT(upBg:getPosition().x+upBg:getContentSize().width+SX(5),pWinSize.height*0.39))
		duihuanLayer:addChild(upBg1,1)
	--Index=v
    	end
--    	mtype[].pic
    	--兑换按钮
    	local  zhandou={
    	 	{pic_1="shengjita/list_3006_1.png",pic_2="shengjita/list_3006_2.png"},
    	 	{pic_1="shengjita/list_3006_1.png",pic_2="shengjita/list_3006_2.png"},
    	 	{pic_1="shengjita/list_3006_1.png",pic_2="shengjita/list_3006_2.png"},
    	}
    	local rowH=pWinSize.height*0.75/5
	for k, v in pairs(zhandou) do
		if mRecordTabel2[k].DemandNum>list4.SulplusNum then
			image1="shengjita/list_3006_3.png"
			image2="shengjita/list_3006_3.png"
		else
			image1=v.pic_1
			image2=v.pic_2
		end
		local zhandouBtn= ZyButton:new(image1, image2, nil,Language.DUIHUAN,nil, FONT_SM_SIZE)
		zhandouBtn:setAnchorPoint(PT(0,0))
		zhandouBtn:setPosition(PT(kongge*1.5+(kongge/2+imageSize.width*0.25)*(k-1),pWinSize.height*0.32))
		zhandouBtn:setTag(k)
		zhandouBtn:registerScriptHandler(duihuanAction)
		zhandouBtn:addto(duihuanLayer,1)
	--Index=v
    	end
	--属性加成
	local shuxingjc = CCLabelTTF:create(Language.SHUXINGJIACHENG,FONT_NAME,FONT_SM_SIZE)
	duihuanLayer:addChild(shuxingjc,1)
	shuxingjc:setAnchorPoint(PT(0,0))
	shuxingjc:setPosition(PT(pWinSize.width*0.1,pWinSize.height*0.22))
	--图
	local  shuxingtu={
    	 	{pic="shengjita/list_3010_1.png"},
    	 	{pic="shengjita/list_3010_2.png"},
    	 	{pic="shengjita/list_3010_3.png"},
    	 	{pic="shengjita/list_3010_4.png"},
    	 }
	for k, v in pairs(shuxingtu) do
		local shuxingImg=CCSprite:create(P(v.pic))
		shuxingImg:setAnchorPoint(PT(0,0))
		shuxingImg:setPosition(PT(pWinSize.width*0.25+rowH*(k-1)*0.75,pWinSize.height*0.215))
		duihuanLayer:addChild(shuxingImg,1)
	--Index=v
    	end
    	--字
    	local  shuxingtu={
    	 	{shuzi=list4.LifeNum},
    	 	{shuzi=list4.WuLiNum},
    	 	{shuzi=list4.MofaNum},
    	 	{shuzi=list4.FunJiNum},
    	 }
	for k, v in pairs(shuxingtu) do
		local shuxingzi=CCLabelTTF:create("+"..v.shuzi,FONT_NAME,FONT_SM_SIZE)
		shuxingzi:setAnchorPoint(PT(0,0))
		shuxingzi:setPosition(PT(pWinSize.width*0.3+rowH*(k-1)*0.75,pWinSize.height*0.22))
		duihuanLayer:addChild(shuxingzi,1)
	--Index=v
    	end
end

function duihuanAction(node)
--	releseduihuan()
	local tag=node:getTag()
	if list4.SulplusNum>=mRecordTabel2[tag].DemandNum then
		actionLayer.Action4408(mScene, nil,mRecordTabel2[tag].PropertyType,mRecordTabel2[tag].DemandNum)
	end
end;

--奖励领取界面
function releselingqu()
	if lingquLayer ~= nil then
		lingquLayer:getParent():removeChild(lingquLayer, true)
		lingquLayer = nil
	end
end

function showlingqu()
	
	if lingquLayer ~= nil then
		lingquLayer:getParent():removeChild(lingquLayer, true)
		lingquLayer = nil
	end
	
	local layer = CCLayer:create()
	mLayer:addChild(layer, 0)
	
	lingquLayer = layer
	
	--每过5层可领取奖励
	ceng=Language.MEIGUCENG.."1"
	local naceng = CCLabelTTF:create(ceng,FONT_NAME,FONT_SM_SIZE)
	lingquLayer:addChild(naceng,1)
	naceng:setAnchorPoint(PT(0,0))
	naceng:setColor(ccBLACK)
	naceng:setPosition(PT(pWinSize.width*0.13,pWinSize.height*0.7))
	--星星
	local starImg=CCSprite:create(P("mainUI/list_1036.png"))
	starImg:setAnchorPoint(PT(0,0))
	starImg:setPosition(PT(naceng:getPosition().x+naceng:getContentSize().width+SX(5),pWinSize.height*0.69))
	lingquLayer:addChild(starImg,0)
	--=金币*1000
	jbceng="="..Language.JINBI.."*"..list5.Modulus
	local jinbi = CCLabelTTF:create(jbceng,FONT_NAME,FONT_SM_SIZE)
	lingquLayer:addChild(jinbi,1)
	jinbi:setAnchorPoint(PT(0,0))
	jinbi:setColor(ccBLACK)
	jinbi:setPosition(PT(starImg:getPosition().x+starImg:getContentSize().width+SX(5),pWinSize.height*0.7))
	--层得分
	dangqianceng=list5.IsTierNum-4
	cdfen=dangqianceng.."-"..list5.IsTierNum..Language.CENGDEFEN..list5.IsTierStar
	local cengdefen = CCLabelTTF:create(cdfen,FONT_NAME,FONT_SM_SIZE)
	lingquLayer:addChild(cengdefen,1)
	cengdefen:setAnchorPoint(PT(0,0))
	cengdefen:setColor(ccBLACK)
	cengdefen:setPosition(PT(pWinSize.width*0.255,pWinSize.height*0.65))
	--星星
	local starImg=CCSprite:create(P("mainUI/list_1036.png"))
	starImg:setAnchorPoint(PT(0,0))
	starImg:setPosition(PT(cengdefen:getPosition().x+cengdefen:getContentSize().width+SX(5),pWinSize.height*0.64))
	lingquLayer:addChild(starImg,0)
	--超出成绩
	chaochu=Language.CHAOCHUCHENGJI..list5.MoreStar..Language.KEXING
	local chaochucj = CCLabelTTF:create(chaochu,FONT_NAME,FONT_SM_SIZE)
	lingquLayer:addChild(chaochucj,1)
	chaochucj:setAnchorPoint(PT(0,0))
	chaochucj:setColor(ccBLACK)
	chaochucj:setPosition(PT((pWinSize.width-chaochucj:getContentSize().width)/2,pWinSize.height*0.58))
	--框
	local rowH=pWinSize.height*0.75/5
	if #mRecordTabel3>0 then
		for k, v in pairs(mRecordTabel3) do
			position[k]={}
			local shuxingImg=CCSprite:create(P("common/icon_8015_4.png"))
			shuxingImg:setAnchorPoint(PT(0,0))
			shuxingImg:setPosition(PT(pWinSize.width*0.14+rowH*(k-1)*0.85,pWinSize.height*0.45))
			lingquLayer:addChild(shuxingImg,1)
			if mRecordTabel3[k].SJTRewarType==1 then
				image="smallitem/icon_8010.png"
			elseif mRecordTabel3[k].SJTRewarType==2 then
				image="smallitem/icon_8012.png"
			elseif mRecordTabel3[k].SJTRewarType==3 then
				image  = string.format("smallitem/%s.png",mRecordTabel3[k].HeadID)
			end
			local shuxingImg=CCSprite:create(P(image))
			shuxingImg:setAnchorPoint(PT(0,0))
			shuxingImg:setPosition(PT(pWinSize.width*0.15+rowH*(k-1)*0.85,pWinSize.height*0.458))
			position[k].x=pWinSize.width*0.15+rowH*(k-1)*0.85+shuxingImg:getContentSize().width/2
			lingquLayer:addChild(shuxingImg,1)
			
			if mRecordTabel3[k].SJTRewarType==2 then
			    jinbi=mRecordTabel3[k].RewardNum+list5.AdditionalGameCoin
			    ifjinbi=1
			else
			    jinbi=mRecordTabel3[k].RewardNum
--			    ifjinbi=nil
			end
			local shuliang = CCLabelTTF:create(jinbi,FONT_NAME,FONT_SM_SIZE)
			lingquLayer:addChild(shuliang,1)
			shuliang:setAnchorPoint(PT(0,0))
			shuliang:setPosition(PT(position[k].x-shuliang:getContentSize().width/2,pWinSize.height*0.43))
			--
		--Index=v
	    	end
	    	
	    	if ifjinbi~=1 then
	    	    if list5.AdditionalGameCoin>0 then
                    local num=#mRecordTabel3
                    image="smallitem/icon_8012.png"
                    local shuxingImg=CCSprite:create(P(image))
                    shuxingImg:setAnchorPoint(PT(0,0))
                    shuxingImg:setPosition(PT(pWinSize.width*0.15+rowH*(num+1-1)*0.85,pWinSize.height*0.458))
                    local positionX=pWinSize.width*0.15+rowH*(num+1-1)*0.85+shuxingImg:getContentSize().width/2
                    lingquLayer:addChild(shuxingImg,1)
                    
                    local shuliang = CCLabelTTF:create(list5.AdditionalGameCoin,FONT_NAME,FONT_SM_SIZE)
                    lingquLayer:addChild(shuliang,1)
                    shuliang:setAnchorPoint(PT(0,0))
                    shuliang:setPosition(PT(positionX-shuliang:getContentSize().width/2,pWinSize.height*0.43))
                end
	    	end
	    	
	    	--领取奖励按钮
		local getBtn=ZyButton:new(Image.image_button_red_c_0,Image.image_button_red_c_1,nil,Language.LINGQUJIANGLI,FONT_NAME,FONT_SM_SIZE)
		getBtn:setAnchorPoint(PT(0,0))
		getBtn:setPosition(PT((pWinSize.width-getBtn:getContentSize().width)/2,pWinSize.height*0.35))
		getBtn:registerScriptHandler(lingquAction)
		getBtn:addto(lingquLayer,0)
	else
		local meijiangli = CCLabelTTF:create(Language.MEIJIANGLI,FONT_NAME,FONT_SM_SIZE)
		lingquLayer:addChild(meijiangli,1)
		meijiangli:setAnchorPoint(PT(0,0))
		meijiangli:setPosition(PT((pWinSize.width-meijiangli:getContentSize().width)/2,pWinSize.height*0.48))
		
		--继续挑战按钮
		local getBtn=ZyButton:new(Image.image_button_red_c_0,Image.image_button_red_c_1,nil,Language.JIXUTIAOZHAN,FONT_NAME,FONT_SM_SIZE)
		getBtn:setAnchorPoint(PT(0,0))
		getBtn:setPosition(PT((pWinSize.width-getBtn:getContentSize().width)/2,pWinSize.height*0.35))
		getBtn:registerScriptHandler(jixuAction)
		getBtn:addto(lingquLayer,0)
	end
    	
	--下方提示
	local chaochucj = CCLabelTTF:create(Language.TISHI_3,FONT_NAME,FONT_SM_SIZE)
	lingquLayer:addChild(chaochucj,1)
	chaochucj:setAnchorPoint(PT(0,0))
	chaochucj:setPosition(PT((pWinSize.width-chaochucj:getContentSize().width)/2,pWinSize.height*0.28))
	local chaochucj = CCLabelTTF:create(Language.TISHI_4,FONT_NAME,FONT_SM_SIZE)
	lingquLayer:addChild(chaochucj,1)
	chaochucj:setAnchorPoint(PT(0,0))
	chaochucj:setPosition(PT((pWinSize.width-chaochucj:getContentSize().width)/2,pWinSize.height*0.25))
	
end

function jixuAction()
	if list5.IsTierNum==50 then
	    releselingqu()
        actionLayer.Action4401(mScene, nil)
	else
        releselingqu()
        actionLayer.Action4404(mScene,nil)
	end
end;

function lingquAction()
	ifjinbi=nil
    actionLayer.Action4410(mScene, nil)
end

function afterAction()
	weidadao()
end

function weidadao()
	
	if weidadaoLayer ~= nil then
		weidadaoLayer:getParent():removeChild(weidadaoLayer, true)
		weidadaoLayer = nil
	end
	
	local layer = CCLayer:create()
	mLayer:addChild(layer, 0)
	
	weidadaoLayer = layer
	
	local naceng = CCLabelTTF:create(Language.DENGJIWEIDADAO,FONT_NAME,FONT_DEF_SIZE)
	weidadaoLayer:addChild(naceng,1)
	naceng:setAnchorPoint(PT(0,0))
	naceng:setPosition(PT((pWinSize.width-naceng:getContentSize().width)/2,pWinSize.height*0.6))
	
end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	
	if actionId==4401 then
		if ZyReader:getResult()== eScutNetSuccess then
			local serverInfo=actionLayer._4401Callback(pScutScene, lpExternalData)
	 		if serverInfo~=nil then
				list1=serverInfo
			end;
			if serverInfo.SJTStatus==1 then
				actionLayer.Action4402(mScene, nil)
			elseif serverInfo.SJTStatus==2 then
				actionLayer.Action4404(mScene,nil)
			elseif serverInfo.SJTStatus==3 then
				actionLayer.Action4407(mScene, nil,1)
			elseif serverInfo.SJTStatus==4 then
				actionLayer.Action4409(mScene, nil)
			elseif serverInfo.SJTStatus==5 then
				
			elseif serverInfo.SJTStatus==0 then
				showContent()
			end
		else
			local box = ZyMessageBoxEx:new()
			box:doPrompt(pScutScene, nil, ZyReader:readErrorMsg(),Language.IDS_SURE,afterAction)
		end
	elseif actionId==4402 then
		local serverInfo=actionLayer._4402Callback(pScutScene, lpExternalData)
 		if serverInfo~=nil then
			list2=serverInfo
		end;
		if list2.EffNum==0 then
			actionLayer.Action4404(mScene,nil)
		else
			showshuxing()
		end
	elseif actionId==4403 then
		local serverInfo=actionLayer._4403Callback(pScutScene, lpExternalData)
 		if serverInfo~=nil then
			local IsTireNum=serverInfo.IsTireNum
			actionLayer.Action4404(mScene,nil)
		end;
	elseif actionId==4404 then
		local serverInfo=actionLayer._4404Callback(pScutScene, lpExternalData)
 		if serverInfo~=nil then
			list3=serverInfo
			mRecordTabel=serverInfo.RecordTabel
		end;
		showdancen()
	elseif actionId==4405 then
		if ZyReader:getResult()== eScutNetSuccess then
			relesedancen()
			local serverInfo=actionLayer._4405Callback(pScutScene, lpExternalData)
			if serverInfo~=nil then
				PersonalInfo.getPersonalInfo().fightinfo=serverInfo
				local fightInfo=ZyTable.th_table_dup(serverInfo)			
				SbattleScene.init(fightInfo)
				iswin=serverInfo.IsWin
			end
		else
			ZyToast.show(mScene,ZyReader:readErrorMsg())
		end
	elseif actionId==4407 then
		releseShowLayer()
		local serverInfo=actionLayer._4407Callback(pScutScene, lpExternalData)
 		if serverInfo~=nil then
			list4=serverInfo
			mRecordTabel2=serverInfo.RecordTabel
		end;
		showduihuan()
	elseif actionId==4408 then
		if ZyReader:getResult()== eScutNetSuccess then
			releseduihuan()
			actionLayer.Action4404(mScene,nil)
		else
			ZyToast.show(mScene,ZyReader:readErrorMsg())
		end
	elseif actionId==4409 then
		releseShowLayer()
		local serverInfo=actionLayer._4409Callback(pScutScene, lpExternalData)
 		if serverInfo~=nil then
			list5=serverInfo
			mRecordTabel3=serverInfo.RecordTabel
		end;
		showlingqu()
	elseif actionId==4410 then
		if list5.IsTierNum==50 then
			releselingqu()
            actionLayer.Action4401(mScene, nil)
		else
			if PersonalInfo.getPersonalInfo().Exchange==0 then
				releselingqu()
				actionLayer.Action4407(mScene, nil,1)
			else
				releselingqu()
				actionLayer.Action4404(mScene,nil)
			end
		end
	elseif actionId==4411 then
		local serverInfo=actionLayer._4411Callback(pScutScene, lpExternalData)
 		if serverInfo~=nil then
			list0=serverInfo.RecordTabel
		end;
		refreshList()
	end
	
end
