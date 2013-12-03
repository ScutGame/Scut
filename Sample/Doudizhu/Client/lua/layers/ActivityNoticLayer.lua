--
-- ActivityNoticLayer.lua.lua
-- Author     : Chensy
-- Version    : 1.0
-- Date       :
-- Description: 活动公告
--

module("ActivityNoticLayer", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local mScene = nil 		-- 场景
local noMsg=nil;
local activityNoticMlist=nil;--list
--
---------------公有接口(以下两个函数固定不用改变)--------
--

-- 函数名以小写字母开始，每个函数都应注释

-- 场景入口
function pushScene()
	initResource()
	createScene()
	CCDirector:sharedDirector():pushScene(_scene)
end
-- 退出场景
function popScene()
	releaseResource()
	CCDirector:sharedDirector():popScene()
end

---关闭界面
function closeAction()
  	if activityNoticMlist then
  	    activityNoticMlist:clear();
  	end
    noMsg=nil;
    if mLayer then
        mScene:removeChild(mLayer,true)
        mLayer=nil;
    end
    mScene=nil;
end

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
end
-- 释放资源
function releaseResource()
end
-- 创建场景
function createScene(scene)
    if scene then
        mScene=scene;
    end
   mLayer=CCLayer:create();
   mScene:addChild(mLayer,9)
	-- 此处添加场景初始内容
	local transparentBg=Image.tou_ming;--背景图片
	local menuItem =CCMenuItemImage:create(P(transparentBg),P(transparentBg));
	local menuBg=CCMenu:createWithItem(menuItem);
	menuBg:setContentSize(menuItem:getContentSize());
	menuBg:setAnchorPoint(CCPoint(0,0));
	menuBg:setScaleX(pWinSize.width/menuBg:getContentSize().width*2)
	menuBg:setScaleY(pWinSize.height/menuBg:getContentSize().height*2);
	menuBg:setPosition(CCPoint(0,0));
	mLayer:addChild(menuBg,0);
    ----
	--背景
	local detailBg= CCSprite:create(P("common/panle_1072.png"));
	detailBg:setAnchorPoint(PT(0,0))
	detailBg:setScaleX(pWinSize.width/detailBg:getContentSize().width)
	detailBg:setScaleY(pWinSize.height/detailBg:getContentSize().height)
	detailBg:setPosition(PT(0,0))
	mLayer:addChild(detailBg,0)
	--离开按钮
	local exitBtn=ZyButton:new("button/panle_1014_3.png",nil,nil, nil,FONT_NAME,FONT_SM_SIZE)
	exitBtn:setAnchorPoint(PT(0,0))
	exitBtn:setPosition(PT(pWinSize.width-exitBtn:getContentSize().width,pWinSize.height-exitBtn:getContentSize().height-SY(3)))
	exitBtn:registerScriptTapHandler(closeAction)
	exitBtn:addto(mLayer)
    ---
  	if activityNoticMlist then
  	    mLayer:removeChild(activityNoticMlist,true)
  	    activityNoticMlist=nil;
  	end
	local listSize = SZ(pWinSize.width*0.9,pWinSize.height*0.75)

	local listRowH=listSize.height/5
	local colW=listSize.width*0.9
	
	activityNoticMlist = ScutCxList:node(listSize.height, ccc4(24, 24, 24, 0), listSize)
	activityNoticMlist:setAnchorPoint(PT(0, 0))
	activityNoticMlist:setTouchEnabled(true)
	activityNoticMlist:setPosition(PT(pWinSize.width*0.05, pWinSize.height*0.1))
	mLayer:addChild(activityNoticMlist,0)
	
    ---
    
    noMsg=CCLabelTTF:create("",FONT_NAME,FONT_SM_SIZE);
    noMsg:setAnchorPoint(PT(0.5,0.5));
    noMsg:setPosition(PT(activityNoticMlist:getContentSize().width/2,activityNoticMlist:getContentSize().height/2));
    activityNoticMlist:addChild(noMsg,1);
    
    actionLayer.Action9203(mScene,nil,1,100)
   -- updateActivityNoticMlist();--更新数据
end
function updateActivityNoticMlist(mtable)
    --[[local mtable={[1]={Title="facai",SendDate="2013-12-01",Content="CCXXOO"},
        [2]={Title="facai",SendDate="2013-12-01",Content="CCXXOO"},
        [3]={Title="facai",SendDate="2013-12-01",Content="CCXXOO"},}]]
    
    activityNoticMlist:clear();
	local layout = CxLayout()
	layout.val_x.t = ABS_WITH_PIXEL
	layout.val_y.t = ABS_WITH_PIXEL
	layout.val_x.val.pixel_val = 0
	layout.val_y.val.pixel_val = 0
    if #mtable>0 then
        noMsg:setString("")
        for k,v in ipairs(mtable) do
            local listItem = ScutCxListItem:itemWithColor(ccc3(0,0,0))
            listItem:setOpacity(0);
            ----
            local  content = createNoticeMessage(v);--创建聊天公告消息
            activityNoticMlist:setRowHeight(content:getContentSize().height + SY(10))
            listItem:addChildItem(content, layout)
            activityNoticMlist:addListItem(listItem, false)
        end
    else
        noMsg:setString(Language.NOTICE_ZANWUGONGAO)--暂无公告
    end
    
end
--创建公会通用界面
function createNoticeMessage(v)
	local layer = CCLayer:create()
	local layerSize=SZ(0,0)
	layerSize.width=activityNoticMlist:getContentSize().width
	--local title=string.format(Language.CHAT_KUANG,)..string.format("(%s)",message.SendDate)
    local title=string.format(Language.NOTICE_NAME,v.Title)..string.format("(%s)",v.SendDate)
--[[	local label=CCLabelTTF:create(title,FONT_NAME,FONT_SM_SIZE)
	label:setAnchorPoint(PT(0,1))
	label:setColor(ccYELLOW)]]
--	local str=v.Content
	local xmlContent=string.format("<label color='%d,%d,%d'>%s</label>",255,255,0,title.."\r\n")..string.format("<label >%s</label>",v.Content)
	local ndMultiLabe=ZyMultiLabel:new(xmlContent,layerSize.width*0.98,FONT_NAME,FONT_SM_SIZE)
			    --横线图片
	local bgEmpty= CCSprite:create(P(Image.panle_1016_1));
	layerSize.height=ndMultiLabe:getContentSize().height+SY(5)+bgEmpty:getContentSize().height
	bgEmpty:setScaleX(layerSize.width/bgEmpty:getContentSize().width)--设置缩放比例
	-- bgEmpty:setScaleY(0.5)--设置缩放比例
	bgEmpty:setAnchorPoint(PT(0,0))--设置对其坐标
	bgEmpty:setPosition(PT(0,0))--设置图片坐标
	layer:addChild(bgEmpty,0)

	layer:setContentSize(layerSize)
	--label:setPosition(PT(0,layerSize.height))
	ndMultiLabe:setPosition(PT(0,layerSize.height-ndMultiLabe:getContentSize().height))
	ndMultiLabe:addto(layer,0)
--	layer:addChild(label,0)
	return layer
end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionID = ZyReader:getActionID()
	local userData = ZyRequestParam:getParamData(lpExternalData)
	if actionID==9203 then
        local table=actionLayer._9203Callback(pScutScene, lpExternalData)
    --[[  local table={[1]={Title="facai",CreateDate="2013-12-01",Content="CCXXOO"},
        [2]={Title="facai",CreateDate="2013-12-01",Content=Language.TOP_TIP},
        [3]={Title="facai",CreateDate="2013-12-01",Content="CCXXOO"},}]]
        if table then
            updateActivityNoticMlist(table.RecordTabel)
        end
	end
end