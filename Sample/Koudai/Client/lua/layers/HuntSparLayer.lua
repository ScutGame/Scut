------------------------------------------------------------------
-- HuntSparLayer.lua
-- Author     :
-- Version    : 1.0
-- Date       :
-- Description: ,
------------------------------------------------------------------
module("HuntSparLayer", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local mScene = nil 		-- 场景
local mLayer=nil
local mServerData=nil
local mDetailLayer=nil
local mBottomLayer=nil
local mBigWheel=nil
local mCurrentShowSpar=nil
local isClick=nil
local rotationValue={}
local mCurrentId=nil
local gotoNpcID=1
local isFirst=nil
--
local isOneGet=nil
local mAutoBtn=nil
local mGoldBtn=nil
local mSellBtn=nil


local m_listTable=nil
--
---------------公有接口(以下两个函数固定不用改变)--------
--

-- 函数名以小写字母开始，每个函数都应注释

-- 场景入口
function setServerData(value)
	mServerData=value
end;

function init(scene)
 	releaseContentLayer()
	mScene=scene
	mLayer = CCLayer:create()
	mScene:addChild(mLayer,0)
	for k=1, 2 do
		local actionBtn=UIHelper.createActionRect(pWinSize)
		actionBtn:setPosition(PT(0,0))
		mLayer:addChild(actionBtn,0)
	end
	initResource()
	createContentLayer()
	createBigWheel()
end

---
function releaseContentLayer()
	releaseBottomLayer()
	if mContentLayer then
		mContentLayer:getParent():removeChild(mContentLayer,true)
		mContentLayer=nil
	end
	if mLayer then
		mLayer:getParent():removeChild(mLayer,true)
		mLayer=nil
	end
end;


--创建背景层
function  createBgLayer()
	local layer=CCLayer:create()
	layer:setAnchorPoint(PT(0,0))
	--大背景
	local bgSprite=CCSprite:create(P("activeBg/list_1122.jpg"))
	bgSprite:setAnchorPoint(PT(0.5,0))
	bgSprite:setScaleX(pWinSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(pWinSize.height/bgSprite:getContentSize().height)
	
	bgSprite:setPosition(PT(pWinSize.width/2,0))
	layer:addChild(bgSprite,0)
	--中间层
	local midSprite=CCSprite:create(P("common/list_1074.png"))
	midSprite:setAnchorPoint(PT(0.5,0))
	midSprite:setPosition(PT(pWinSize.width/2,0))
	midSprite:setScaleX(pWinSize.width/midSprite:getContentSize().width)
	midSprite:setScaleY(pWinSize.height/midSprite:getContentSize().height)
	
	layer:addChild(midSprite,0)
	--]]
	return layer
end;

---创建通用层
function  createContentLayer()
	local layer=CCLayer:create()
	mContentLayer=layer
	mLayer:addChild(layer,0)
	local bgLayer= createBgLayer()
	layer:addChild(bgLayer,0)
	-----
	local startX=pWinSize.width*0.05
	local startY=pWinSize.height*0.96

	--免费次数
	local timeLabel=CCLabelTTF:create(Language.HUNT_TIMES .. ":" .. mServerData.FreeNum ,FONT_NAME,FONT_DEF_SIZE)
	timeLabel:setAnchorPoint(PT(0.5,0))
	timeLabel:setPosition(PT(pWinSize.width*0.5,
					startY-timeLabel:getContentSize().height))
	layer:addChild(timeLabel,0)
	
	---
	startY=timeLabel:getPosition().y
	local sparLabel=CCLabelTTF:create(Language.IDS_JINGSHI .. ":" .. mServerData.GoldNum ,FONT_NAME,FONT_SM_SIZE)
	sparLabel:setAnchorPoint(PT(0,0))
	sparLabel:setPosition(PT(startX,startY-sparLabel:getContentSize().height))
	layer:addChild(sparLabel,0)
	local goldLabel=CCLabelTTF:create(Language.IDS_GOLD .. ":" .. mServerData.GameCoin ,FONT_NAME,FONT_SM_SIZE)
	goldLabel:setAnchorPoint(PT(0,0))
	goldLabel:setPosition(PT(startX,sparLabel:getPosition().y-sparLabel:getContentSize().height*1.2))
	layer:addChild(goldLabel,0)
	-----
	local closeBtn=ZyButton:new("button/list_1046.png")
	closeBtn:setAnchorPoint(PT(0,0))
	closeBtn:setPosition(PT(pWinSize.width-closeBtn:getContentSize().width*1.1,
							pWinSize.height-closeBtn:getContentSize().height*1.2))
	closeBtn:addto(layer,1)
	closeBtn:registerScriptHandler(gobackHunting)
	
	 refreshBottomLayer()
end;

--一键猎命
function autoHuntAction()
	if PersonalInfo.getPersonalInfo()._VipLv < 5 then
		ZyToast.show(mScene, Language.HUNT_ONEGETLIMIT,1,0.35)
		return		
	end
	isOneGet=true
	mSellBtn:setEnabled(false)
	mGoldBtn:setEnabled(false)
	mAutoBtn:setString(Language.TIP_STOP)
	mAutoBtn:registerScriptHandler(stopAutoHunt)
	oneGetAction()
end;

function oneGetAction()
	if not isClick then
		isClick=true
		local HuntId=mServerData.RecordTabel2[mCurrentId].HuntingID
		actionLayer.Action1305(mScene,false,HuntId,1)
	end
end;

function  stopAutoHunt()
	isOneGet=false
	mSellBtn:setEnabled(true)
	mGoldBtn:setEnabled(true)
	mAutoBtn:setString(Language.HUNT_AUTO)
	mAutoBtn:registerScriptHandler(autoHuntAction)	
end;

---一键卖出
function autoSellAction()
	if not isClick then
	isClick =true
	actionLayer.Action1306(mScene,false,nil,2)
	end
end;

--晶石猎命
function coinHuntAction()
	if PersonalInfo.getPersonalInfo()._VipLv < 5 then
		ZyToast.show(mScene, Language.HUNT_VIPLIMIT,1,0.35)
		return		
	end
	if not isOneGet then
		local box = ZyMessageBoxEx:new()
		box:doQuery(mScene, nil,Language.LIFECELL_TIP3,Language.IDS_SURE,Language.IDS_CANCEL,confirmGoldGet)
	end
end;

--
function confirmGoldGet(clickedButtonIndex,content,tag)
	if clickedButtonIndex == 1 then--确认
		if not isClick then
			isClick=true
			local HuntId= 1006  --mServerData.RecordTabel2[4].HuntingID
			actionLayer.Action1305(mScene,false,HuntId,2)
		end
	end;
end

--返回装备晶石界面
function gobackHunting()
		isFirst=nil
		isClick =false
		isOneGet=nil
		releaseContentLayer()
		HuntingScene.setGotoHunt(false)
		actionLayer.Action1311(mScene,nil)
end;

--释放大转盘
function  releaseBigWheel()
	if mBigWheel then
		mBigWheel:getParent():removeChild(mBigWheel,true)
		mBigWheel=nil
	end
	mWheelBg=nil
end;

--大转盘
function createBigWheel()
	releaseBigWheel()
	local layer=CCLayer:create()
	mBigWheel=layer
	--------
	mLayer:addChild(layer,0)
	local startY=pWinSize.height*0.96
	local bgSprite=CCSprite:create(P("common/list_1120.png"))
	bgSprite:setAnchorPoint(PT(0.5,0.5))
	bgSprite:setPosition(PT(pWinSize.width/2,startY-bgSprite:getContentSize().height/2))
	layer:addChild(bgSprite,0)
	mWheelBg=bgSprite
	local col=11
	local rowH=bgSprite:getContentSize().height
	local colW=bgSprite:getContentSize().width
	startY=rowH/2
	local startX=colW/2
	rotationValue={
	[1]={rotation=0,pos=PT(colW/2,rowH*0.15),image="smallitem/icon_8000.png"},
	[2]={rotation=-72,pos=PT(colW*0.83,rowH*0.4),image="smallitem/icon_8001.png"},
	[3]={rotation=-144,pos=PT(colW*0.7,rowH*0.75),image="smallitem/icon_8002.png"},
	[4]={rotation=144,pos=PT(colW*0.3,rowH*0.75),image="smallitem/icon_8003.png"},
	[5]={rotation=72,pos=PT(colW*0.17,rowH*0.4),image="smallitem/icon_8004.png"},
	}
	sortTable()
	for k, v in pairs(mServerData.RecordTabel2) do		
		if v.IsLight==1  then
			gotoNpcID=k
		end
	end	
	if not mCurrentId then
		mCurrentId=gotoNpcID
	end
	
	--如果猎到第五个停止
	if gotoNpcID>=5 then
		 stopAutoHunt()		
	end
	
	--
	local mBgTecture=IMAGE("common/list_1088.png")
	mWheelBg:setRotation((mCurrentId-1)*72)
	for k, v in pairs(mServerData.RecordTabel2) do				
		local path=rotationValue[k].image
		local headSprite=CCSprite:create(P(path))
		local fun=HuntSparLayer.huntSparAction
		if v.IsLight~=1  then
		     local imageTex = CImageHelper:getImageGray(P(path))
		    headSprite = CCSprite:createWithTexture(imageTex)
		    fun="HuntSparLayer.warmingAction"
		end
		v.headSprite=headSprite
		local posInfo=rotationValue[k]
		headSprite:setAnchorPoint(PT(0.5,0.5))
		headSprite:setPosition(posInfo.pos)
		headSprite:setRotation(-(mCurrentId-1)*72)
		bgSprite:addChild(headSprite,0)	
		--人物背景
		local spriteBg=CCSprite:createWithTexture(mBgTecture)
		spriteBg:setAnchorPoint(PT(0.5,0.5))
		spriteBg:setPosition(PT(headSprite:getContentSize().width/2,
									headSprite:getContentSize().height/2))
		headSprite:addChild(spriteBg,-1)	
		
		--钱
		local moneyLabel=CCLabelTTF:create((v.Price or 0) .. Language.IDS_GOLD ,FONT_NAME,FONT_SM_SIZE)
		moneyLabel:setAnchorPoint(PT(0.5,1))
		moneyLabel:setPosition(PT(headSprite:getContentSize().width/2,-SY(5)))
		headSprite:addChild(moneyLabel,-1)	
		
									
		local actionBtn=UIHelper.createActionRect(headSprite:getContentSize(),fun,k)
		actionBtn:setPosition(PT(0,0))
		headSprite:addChild(actionBtn,0)
	end
	rotationAction(gotoNpcID)
end;

--灰头像提示语
function  warmingAction()
	ZyToast.show(mScene, Language.HUNT_WARMING,1,0.35)
	
end;

---转动大转盘
function  rotationAction(id)
	local wheelNum=(id-mCurrentId)*72
	local speedTime=0.5*math.abs(id-mCurrentId)
	mCurrentId=id
	local funName="HuntSparLayer.wheelMoveEnd"
	local rotationAt = CCRotateBy:create(speedTime,wheelNum);
   	rotationAt = CCSequence:createWithTwoActions(rotationAt,CCCallFunc:create(funName));
	mWheelBg:runAction(rotationAt)
	for k , v in pairs(mServerData.RecordTabel2) do
		local num=-wheelNum
		local  rotationAt = CCRotateBy:create(speedTime,num);
		v.headSprite:runAction(rotationAt)			
	end
end;

--大转盘移动结束
function wheelMoveEnd()
	isClick=false
	if isOneGet then
		if #mServerData.RecordTabel>=50 then
			if mServerData.IsSale==1 then
				autoSellAction()
			else
				stopAutoHunt()
			end
		else
			oneGetAction()
		end		
	end
end;

--点击人物进行猎命
function huntSparAction(node)
	local tag=node:getTag()
	if mServerData.RecordTabel2[tag] then
		if not isClick then
		isClick=true
		local HuntId=mServerData.RecordTabel2[tag].HuntingID
		actionLayer.Action1305(mScene,false,HuntId,1)
		end
	end
end;

--创建玩家拥有的晶石层
function releaseBottomLayer()
	if mBottomLayer then
		mBottomLayer:getParent():removeChild(mBottomLayer,true)
		mBottomLayer=nil
	end
end;



--刷新页码
function refreshPage()
	if m_listTable.pageLabel then
	local str=string.format("%d/%d",m_listTable.currentPage,m_listTable.maxPage)
	m_listTable.pageLabel:setString(str)
	end
end;

--list翻页
function callbackListview(page)
	if tonumber(page) <0 then
		page=0
	end
	m_listTable.currentPage=tonumber(page)+1	
	refreshPage()
end


--底下的晶石表
function refreshBottomLayer()
	releaseBottomLayer()
	local layer=CCLayer:create()
	mBottomLayer=layer
	mContentLayer:addChild(layer,0)
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(0,0))
	
	local startY=pWinSize.height*0.02
	local bottomBox=SZ(pWinSize.width*0.9,pWinSize.height*0.35)
	local bSprite=CCSprite:create(P("common/list_1038.9.png"))
	bSprite:setScaleX(bottomBox.width/bSprite:getContentSize().width)
	bSprite:setScaleY(bottomBox.height/bSprite:getContentSize().height)
	bSprite:setAnchorPoint(PT(0,0))
	bSprite:setPosition(PT(pWinSize.width*0.05,startY))
	layer:addChild(bSprite,0)

	startY=startY+bottomBox.height+SY(3)
	local colW=bottomBox.width/3
	
	--一键猎命
	local autoHuntBtn=ZyButton:new("button/list_1023.png",nil,nil,Language.HUNT_AUTO)
	autoHuntBtn:setPosition(PT(pWinSize.width/2-autoHuntBtn:getContentSize().width/2,startY))
	autoHuntBtn:addto(layer,0)
	autoHuntBtn:registerScriptHandler(autoHuntAction)
	if isOneGet then
		autoHuntBtn:setString(Language.TIP_STOP)
		autoHuntBtn:registerScriptHandler(stopAutoHunt)	
	end
	mAutoBtn=autoHuntBtn

	
	--一键卖出
	local autoSellBtn=ZyButton:new("button/list_1023.png",nil,nil,Language.HUNT_AUTOSELL)
	autoSellBtn:setPosition(PT(autoHuntBtn:getPosition().x-colW,startY))
	autoSellBtn:addto(layer,0)	
	autoSellBtn:registerScriptHandler(autoSellAction)
	mSellBtn=autoSellBtn
	
	--晶石猎命
	local coinHuntBtn=ZyButton:new("button/list_1023.png",nil,nil,Language.HUNT_MONEY)
	coinHuntBtn:setPosition(PT(autoHuntBtn:getPosition().x+colW,startY))
	coinHuntBtn:addto(layer,0)
	coinHuntBtn:registerScriptHandler(coinHuntAction)
	mGoldBtn=coinHuntBtn

	if not isShowVip() and PersonalInfo.getPersonalInfo()._VipLv < 5 then
		autoHuntBtn:setVisible(false)
		coinHuntBtn:setVisible(false)	
	end
	----------------------
	--
	local autoBtn=ZyButton:new("button/list_1023.png",nil,nil,Language.HUNT_AUTOG)
	autoBtn:setAnchorPoint(PT(0,0))
	autoBtn:setPosition(PT(pWinSize.width/2-autoBtn:getContentSize().width/2,
								bSprite:getPosition().y+SY(3)))
	autoBtn:addto(layer,0)
	autoBtn:registerScriptHandler(autoGetAction)
	

	
	local listY=autoBtn:getPosition().y+autoBtn:getContentSize().height
	local listX=bSprite:getPosition().x+bottomBox.width*0.02
	local listSize=SZ(bottomBox.width*0.96,bottomBox.height-autoBtn:getContentSize().height*1.2)
	
	local list = ScutCxList:node(listSize.width, ccc4(24, 24, 24, 0), listSize)	
	list:setAnchorPoint(PT(0,0))
	list:setPosition(PT(listX,listY))
	list:setRecodeNumPerPage(1)
	layer:addChild(list,1)
	list:setHorizontal(true)
	list:setPageTurnEffect(true)
	list:setTouchEnabled(true)
	list:registerLoadEvent("HuntSparLayer.callbackListview")
	
	m_listTable={}
	m_listTable.currentPage=1
	m_listTable.maxPage=1
	

	local rows=50;
	local pageSize=10
	local sparTable=ZyTable.th_table_dup(mServerData.RecordTabel)

	rows=rows>#sparTable and rows or #sparTable;
	if rows>#sparTable then
		for i=1,rows-#sparTable do
		        sparTable[#sparTable+1]={empty=true}
		end
	end
	local pageCount,pageTable=ZyTable.GetPaging(pageSize,sparTable)
	if isOneGet  and #mServerData.RecordTabel then
		m_listTable.currentPage=math.ceil(#mServerData.RecordTabel/pageSize)
	end

	 ---页码
	 m_listTable.maxPage=pageCount
	local str=string.format("%d/%d",m_listTable.currentPage,m_listTable.maxPage)
	local pageLabel=CCLabelTTF:create(str,FONT_NAME,FONT_FM_SIZE)
	pageLabel:setAnchorPoint(PT(0.5,0))
	pageLabel:setPosition(PT(pWinSize.width/2,list:getPosition().y))
	layer:addChild(pageLabel,0)
	m_listTable.pageLabel=pageLabel
	--]]
	
	
	local layout = CxLayout()
	layout.val_x.t = ABS_WITH_PIXEL
	layout.val_y.t = ABS_WITH_PIXEL
	layout.val_x.val.pixel_val = 0
	layout.val_y.val.pixel_val = 0
	local col=5
	local rowH=bottomBox.height*0.38
	local colW=listSize.width/col
	local startY=listSize.height-rowH*0.4
	local startX=colW/2
	local mBgTecture=IMAGE("common/list_1088.png")
	
	
	for m,n in pairs(pageTable) do
	local listItem = ScutCxListItem:itemWithColor(ccc3(24,24,24))
	listItem:setOpacity(0)
	local sunLayer=CCLayer:create()	
	for k, v in pairs(n) do
		local index=(m-1)*pageSize+k
		local sparBgSprite=CCSprite:createWithTexture(mBgTecture)
		sparBgSprite:setAnchorPoint(PT(0.5,0.5))
		local posX=startX+((k-1)%col)*colW
		local posY=startY-math.floor((k-1)/col)*rowH
		sparBgSprite:setPosition(PT(posX,posY))
		sunLayer:addChild(sparBgSprite,0)
		if not v.empty then
			local pImage = string.format("smallitem/%s.png",v.HeadID)
			local sparSprite=CCSprite:create(P(pImage))
			sparSprite:setAnchorPoint(PT(0.5,0.5))
			sparSprite:setPosition(PT(sparBgSprite:getContentSize().width/2,
									sparBgSprite:getContentSize().height/2))
			sparBgSprite:addChild(sparSprite,0)
			
			local actionBtn=UIHelper.createActionRect(sparBgSprite:getContentSize(),HuntSparLayer.choiceBottomSpar,index)
			actionBtn:setPosition(PT(0,0))
			sparBgSprite:addChild(actionBtn,0)
			local nameLabel=CCLabelTTF:create(v.CrystalName,FONT_NAME,FONT_SMM_SIZE)	
			nameLabel:setAnchorPoint(PT(0.5,1))
			nameLabel:setPosition(PT(sparSprite:getPosition().x,0))
			sparBgSprite:addChild(nameLabel,0)
			nameLabel:setColor(ZyColor:getCrystalColor(v.CrystalQuality))		
		end
	end
	listItem:addChildItem(sunLayer, layout)
	list:addListItem(listItem, false)
	end
	
	list:turnToPage(m_listTable.currentPage-1)

end;

--一键拾取
function autoGetAction()
	if not isClick then
	isClick =true
	actionLayer.Action1307(mScene,false,nil,2)
	end
end;

--点击单个晶石
function choiceBottomSpar(node)
	local tag=node:getTag()
	if mServerData.RecordTabel[tag] then
			mCurrentShowSpar=mServerData.RecordTabel[tag]
			local UserCrystalID=mServerData.RecordTabel[tag].UserCrystalID
			if not isClick then
			isClick =true
					actionLayer.Action1304(mScene,false,UserCrystalID)
			end	
	end
end;

--详细信息
function createDetailLayer(info)
local layer=CCLayer:create()
mLayer:addChild(layer,2)
mDetailLayer=layer
layer:setAnchorPoint(PT(0,0))
layer:setPosition(PT(0,0))
for k=1 ,2 do
	local pingbiBtn=UIHelper.createActionRect(pWinSize,HuntingScene.releaseDetailLayer)
	pingbiBtn:setPosition(PT(0,0))
	layer:addChild(pingbiBtn,0)
end

-----
local midSize=SZ(pWinSize.width*0.65,pWinSize.height*0.4)
local midBgSprite=CCSprite:create(P("common/list_1038.9.png"))
midBgSprite:setScaleX(midSize.width/midBgSprite:getContentSize().width)
midBgSprite:setScaleY(midSize.height/midBgSprite:getContentSize().height)
midBgSprite:setAnchorPoint(PT(0.5,0.5))
midBgSprite:setPosition(PT(pWinSize.width/2,pWinSize.height*0.6))
layer:addChild(midBgSprite,0)

----
local startX=pWinSize.width/2-midSize.width*0.45
local startY=pWinSize.height*0.6+midSize.height*0.38
----
local mBgTecture=IMAGE("common/list_1088.png")
local headBg=CCSprite:createWithTexture(mBgTecture)
headBg:setAnchorPoint(PT(0,0))
headBg:setPosition(PT(startX,startY-headBg:getContentSize().height))
layer:addChild(headBg,0)
---
local imagePath = string.format("smallitem/%s.png",info.HeadID)
local headSprite=CCSprite:create(P(imagePath))
headSprite:setAnchorPoint(PT(0.5,0.5))
headSprite:setPosition(PT(headBg:getContentSize().width/2,headBg:getContentSize().height/2))
headBg:addChild(headSprite,0)

----名称
startX=startX+headBg:getContentSize().width*1.2
local nameLabel=CCLabelTTF:create(info.CrystalName,FONT_NAME,FONT_DEF_SIZE)
nameLabel:setAnchorPoint(PT(0,0))
local color = ZyColor:getCrystalColor(info.CrystalQuality)
nameLabel:setColor(color)
nameLabel:setPosition(PT(startX,
						headBg:getPosition().y+headBg:getContentSize().height-nameLabel:getContentSize().height))
layer:addChild(nameLabel,1)

---命格经验
local str=Language.HUNT_EXP .. ":" .. string.format("%d/%d",info.CrystalExperience,info.MaxExperience)
local expLabel=CCLabelTTF:create(str,FONT_NAME,FONT_SM_SIZE)
expLabel:setAnchorPoint(PT(0,0))
expLabel:setPosition(PT(startX,
						nameLabel:getPosition().y-nameLabel:getContentSize().height*1.5))
layer:addChild(expLabel,1)

---属性
local abilityName=""
if Language.BAG_TYPE_[info.AbilityType] then
	abilityName=Language.BAG_TYPE_[info.AbilityType] .. ":"  .. info.AttrNum
end
local abilityLabel=CCLabelTTF:create(abilityName,FONT_NAME,FONT_SM_SIZE)
abilityLabel:setAnchorPoint(PT(0,0))
abilityLabel:setPosition(PT(startX,
						expLabel:getPosition().y-expLabel:getContentSize().height*1.5))
layer:addChild(abilityLabel,1)

--出售价格
local str=Language.HUNT_PRICE .. ":" .. info.SalePrice 
local priceLabel=CCLabelTTF:create(str,FONT_NAME,FONT_SM_SIZE)
priceLabel:setAnchorPoint(PT(0,0))
priceLabel:setPosition(PT(startX,
						abilityLabel:getPosition().y-priceLabel:getContentSize().height*1.5))
layer:addChild(priceLabel,1)

-----
local closeBtn=ZyButton:new("button/list_1046.png")
closeBtn:setAnchorPoint(PT(0,0))
closeBtn:setPosition(PT(pWinSize.width/2+midSize.width/2-closeBtn:getContentSize().width*1.1,
						midBgSprite:getPosition().y+midSize.height/2-closeBtn:getContentSize().height*1.2))
closeBtn:addto(layer,1)
closeBtn:registerScriptHandler(releaseDetailLayer)

local  sellBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.HUNT_SELL)
sellBtn:setAnchorPoint(PT(0,0))
sellBtn:addto(layer,1)
sellBtn:setPosition(PT(pWinSize.width/2-sellBtn:getContentSize().width/2,
							midBgSprite:getPosition().y-midSize.height*0.45))
sellBtn:registerScriptHandler(sellAction)
local getBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.HUNT_GET)
getBtn:setAnchorPoint(PT(0,0))
getBtn:addto(layer,1)
getBtn:registerScriptHandler(getSparAction)
getBtn:setPosition(PT(pWinSize.width/2+midSize.width/2-sellBtn:getContentSize().width*1.1,
							sellBtn:getPosition().y))
if info.CrystalQuality==1  then
	sellBtn:setPosition(PT(getBtn:getPosition().x,
								midBgSprite:getPosition().y-midSize.height*0.45))
	getBtn:setVisible(false)
end
end;

--卖出按钮响应
function sellAction(node)
	releaseDetailLayer()
	if mCurrentShowSpar.CrystalQuality > 3 then--紫色品质以上增加询问
		local box = ZyMessageBoxEx:new()
		local contentStr=string.format(Language.HUNT_ASKSELL, mCurrentShowSpar.CrystalName )
		box:doQuery(mScene, nil, contentStr , Language.IDS_SURE, Language.IDS_CANCEL,askIsSell) 		
	else
		sellIt()
	end
end;

--询问是否卖出
function askIsSell(index, content, tag)
	if index == 1 then
		sellIt()
	else
		isClick=false
	end
end;

--卖出
function sellIt()
	local UserCrystalID=mCurrentShowSpar.UserCrystalID
	if not isClick then
		isClick=true
		actionLayer.Action1306(mScene,false,UserCrystalID,1)
	end
end


---拾取
function getSparAction(node)
	local    UserCrystalID=mCurrentShowSpar.UserCrystalID
	if not isClick then
	isClick=true
	actionLayer.Action1307(mScene,false,UserCrystalID,1)
	end
end;

--释放详细表
function releaseDetailLayer()
	if mDetailLayer then
		mDetailLayer:getParent():removeChild(mDetailLayer,true)
		mDetailLayer=nil
	end
end;

-- 退出场景
function closeScene()
	releaseContentLayer()
	releaseResource()
	HuntingScene.setGotoHunt(false)
end

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
isClick=nil
end
-- 释放资源
function releaseResource()
	mLayer=nil
	mScene=nil
	mContentLayer=nil
	mBigWheel=nil
	mBottomLayer=nil
end

--详细信息
function _1304Callback(pScutScene, lpExternalData)
			local serverData=actionLayer._1304Callback(pScutScene, lpExternalData)
			if serverData then
				 createDetailLayer(serverData)
			end
			isClick=false
end;

--获取
function _1305Callback(pScutScene, lpExternalData)
	    if ZyReader:getResult() == eScutNetSuccess  or ZyReader:getResult() ==1  then
			actionLayer.Action1303(pScutScene,nil)			
	    else
	    		if isOneGet then
	    			stopAutoHunt()
	    		end
          		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)	    
	    end
	    isClick=false
end;

--卖出
function _1306Callback(pScutScene, lpExternalData)
	    if ZyReader:getResult() == eScutNetSuccess then
			actionLayer.Action1303(pScutScene,nil)			
	    else
          		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)	    
	    end
			isClick=false
end;

--拾取
function _1307Callback(pScutScene, lpExternalData)
	    if ZyReader:getResult() == eScutNetSuccess or  ZyReader:getResult()  == 1 then
          		ZyToast.show(pScutScene, Language.HUNT_GETTIP )	    	
			actionLayer.Action1303(pScutScene,nil)			
	    else
          		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)	    
	    end
			isClick=false
end;


---
function  sortTable()
	if mServerData.RecordTabel2  and #mServerData.RecordTabel2>0  then
		local table=mServerData.RecordTabel2
		local lenth=#table
		table[0]={}
		for i=1 , lenth do
			local j=i+1
			if table[j] then
				if  table[j].HuntingID<table[i].HuntingID then
					--存储待排序元素
					table[0]=table[j]					
					--查找在有序区中的插入位置，同时移动元素
					while (table[0].HuntingID<table[i].HuntingID) do
						table[i+1]=table[i]
						i=i-1
					end
					--将元素插入
					table[i+1]=table[0]		
				end
			end
			--还原有序区指针
			i=j-1
		end	
		table[0]=nil
		mServerData.RecordTabel2=table
	end
end;







