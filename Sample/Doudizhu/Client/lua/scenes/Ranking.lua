--
-- Ranking.lua.lua
-- Author     : Chensy
-- Version    : 1.0
-- Date       :
-- Description: 排名系统

module("Ranking", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写
local mScene = nil 		-- 场景
local mCurrentTab=nil;--tab
local allTable={};
local noMsg=nil;--是否有消息
local layoutLayer=nil;--通用层
local exPlainLayer=nil;--说明层
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

--说明按钮想要关闭事件
function explainCloseBtnAction()
    if exPlainLayer then
        mLayer:removeChild(exPlainLayer,true)
        exPlainLayer=nil;
    end
end
--关闭按钮想要事件
function closeBtnActon()
    releaseResource()--释放资源
    SlideInRPopScene()
    if mLayer then
        mScene:removeChild(mLayer,true);
        mLayer=nil;
    end
	mScene=nil
end
--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
allTable={} 
end
-- 释放资源
function releaseResource()
    allTable=nil;
    if mList then
        mList:clear();
        mList=nil;
    end
    releaseLayer()--释放掉层
end
-- 创建场景
function createScene()
    initResource()
	local scene  = ScutScene:new()
	-- 注册网络回调
	scene:registerCallback(networkCallback)
    mScene = scene.root
	-- 此处添加场景初始内容
    mLayer = CCLayer:create()
	mScene:addChild(mLayer, 0)
	----
    --透明背景
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
    local bgLayer = UIHelper.createUIBg(nil,nil,ZyColor:colorWhite(),nil,true);--没有背景框
    mLayer:addChild(bgLayer);	
    --返回按钮
    local closeBtn=ZyButton:new(P(Image.image_exit));
    closeBtn:setAnchorPoint(PT(1,0.5));
    closeBtn:setPosition(PT(pWinSize.width-closeBtn:getContentSize().width/2,pWinSize.height-closeBtn:getContentSize().height*0.8));
    closeBtn:registerScriptTapHandler(closeBtnActon);
    closeBtn:addto(mLayer);
    --说明按钮
    local explainBtn=ZyButton:new(P(Image.image_help))
    explainBtn:setAnchorPoint(PT(0.5,0.5));
    explainBtn:setPosition(PT(pWinSize.width*0.8,closeBtn:getPosition().y));
   -- explainBtn:setScaleX(0.8)
    explainBtn:registerScriptTapHandler(explainBtnAction)
   -- explainBtn:addImage(Image.button_1006);--说明图标
    explainBtn:addto(mLayer,1);
    ---
    if not mCurrentTab then
        mCurrentTab=1;
    end
    --Tab --金豆排名,胜率排名
   -- local tabName={Language.RANKING_JINDOUPAIMING,Language.RANKING_SHENGLVPAIMING}
    local tabName={Image.button_1028,Image.button_1029}  
 --   createTabBar(tabName);--创建标题按钮
    showLayer(mCurrentTab);
    ----
   	 SlideInLPushScene(mScene)
   --[[ MainScene.releseMainScene();
    local runningScene = CCDirector:sharedDirector():getRunningScene()
    if runningScene == nil then
		CCDirector:sharedDirector():runWithScene(mScene);
    else
		CCDirector:sharedDirector():replaceScene(mScene);
    end]]
end
--Tab
function createTabBar(tabName)
    tabBar = ZyTabBar:new(Image.image_shop,Image.image_shop_1,tabName, FONT_NAME, FONT_SM_SIZE,2);
    tabBar:addto(mLayer)
    tabBar:setCallbackFun(callbackTabBar);  --点击响应的事件
    tabBar:selectItem(mCurrentTab);           --点击哪个按钮
--    tabBar:setPosition(PT(SX(20),pWinSize.height-tabBar:getContentSize().height-SY(8)))   
    tabBar:setPosition(PT(SX(23),pWinSize.height*0.86-SY(1)))   
end
--tabbar
function callbackTabBar(bar,pNode,tag)
	local index=pNode:getTag();
	if index ~=mCurrentTab then
		mCurrentTab=index;
		showLayer(mCurrentTab);
	end
end
function showLayer(index)
    if index then
        mCurrentTab =index
    end
    releaseLayer();--释放层
    if mCurrentTab ==1 then 
        showLayout();
        actionLayer.Action1019(mScene,nil,mCurrentTab,1,200)
    --    update()--更新数据
    elseif mCurrentTab ==2 then
        showLayout();
        actionLayer.Action1019(mScene,nil,mCurrentTab,1,200)
       -- update()--更新数据
    end
end
--释放层
function  releaseLayer()
    allTable={};
    if layoutLayer then
        mLayer:removeChild(layoutLayer,true)
        layoutLayer=nil;
    end
end
function showLayout()
    -------
    if layoutLayer then
        mLayer:removeChild(layoutLayer,true)
        layoutLayer=nil;
    end
    layoutLayer=CCLayer:create()
    mLayer:addChild(layoutLayer,1)
    local simpleW=(pWinSize.width*0.8)/3;  --每个框要多少宽度
    local Bg=CCSprite:create(P(Image.panle_1019_1_9));
    local scalex=simpleW/Bg:getContentSize().width;
    local scaleList={0.7,1.1,1.2}--竞技榜中的列表属性
    local cursor=pWinSize.width*0.1;
    local listBgStartY=pWinSize.height*0.75;
    allTable.ranKingTitlePos={};--位置
    allTable.biliSize={};--每一个标题的比例大小

    local table=nil;
    if mCurrentTab==1 then
        --名次--昵称--金豆
        table={Language.RANKING_MINGCI,Language.RANKING_NICHEN,Language.RANKING_JINDOU}
    elseif mCurrentTab ==2 then
        --名次--昵称--胜率
        table={Language.RANKING_MINGCI,Language.RANKING_NICHEN,Language.RANKING_SHENGLV}
    end
   ----
   allTable.title={}
   for i,v in ipairs(table) do  
       local temp=scaleList[i];
       local textlabel=CCLabelTTF:create(v,FONT_NAME,FONT_SM_SIZE);
       textlabel:setAnchorPoint(PT(0.5,0));
       textlabel:setColor(ccRED1)
       textlabel:setPosition(PT(cursor+simpleW*temp/2,listBgStartY+SY(2)))
       allTable.ranKingTitlePos[i]=(textlabel:getPosition().x);
       allTable.biliSize[i]=simpleW*scaleList[i]
       cursor=cursor+(simpleW*scaleList[i])
        layoutLayer:addChild(textlabel,1)
        allTable.title[i]=textlabel
    end
    --mlils
    if mList then
        layoutLayer:removeChild(mList,true)
        mList=nil;
    end
    
    mList=ScutCxList:node(pWinSize.height*0.63/4,ccc4(124, 124, 124, 255),SZ(pWinSize.width*0.8,pWinSize.height*0.63));
	mList:setPosition(PT(pWinSize.width*0.1,listBgStartY-pWinSize.height*0.63));
    mList:setSelectedItemColor(ccc3(0, 70, 65), ccc3(0, 70, 65))
    --mList:registerItemClickListener(SportScene.gotoPlayer); --列表监听
    mList:setTouchEnabled(true)
    layoutLayer:addChild(mList,1);
    
    ---消息
    noMsg=CCLabelTTF:create("",FONT_NAME,FONT_SM_SIZE)
    noMsg:setAnchorPoint(PT(0.5,0.5));
    noMsg:setPosition(PT(pWinSize.width*0.5,pWinSize.height*0.46))
    layoutLayer:addChild(noMsg,1);

end
--更新数据
function update(mtable)
   --[[ mtable={};
    mtable={[1]={mc=1,name="cxx",money="777777"},[2]={mc=2,name="cxx",money="777777"},
[3]={mc=3,name="cxx",money="777777"},[4]={mc=4,name="cxx",money="7778777"},
[5]={mc=5,name="cxx",money="7778777"},}]]
 
    mList:clear();
    if #mtable<=0 then
        noMsg:setString(Language.RANKING_ZANWUPAIMING);--暂无排名
        noMsg:setColor(ccRED1)
        ---
        for k,v in ipairs(allTable.title) do 
            v:getParent():removeChild(v, true)
            v = nil
        end
    else
        noMsg:setString("")
    end
	local layout = CxLayout()
	layout.val_x.t = ABS_WITH_PIXEL
	layout.val_y.t = ABS_WITH_PIXEL
	layout.val_x.val.pixel_val = 0
	layout.val_y.val.pixel_val = 0
	----
       for i,v in ipairs(mtable) do 
            local listItem=ScutCxListItem:itemWithColor(ccc3(0,0,0))
            listItem:setDrawTopLine(false)
            listItem:setOpacity(0)
            mList:addListItem(listItem,false)

            local table=nil
            if mCurrentTab==1 then
                --名次--昵称--金豆
                table={v.RankID,v.NickName,v.GameCoin }
            elseif mCurrentTab ==2 then
                --名次--昵称--胜率
                table={v.RankID,v.NickName,v.Wining }
            end
            for k,v in ipairs(table) do 
                --背景框
                local bgEmpty= CCSprite:create(P(Image.panle_1019_1_9));
                bgEmpty:setScaleX(allTable.biliSize[k]*0.95/bgEmpty:getContentSize().width)--设置缩放比例
                bgEmpty:setAnchorPoint(PT(0.5,0.5))--设置对其坐标
                bgEmpty:setPosition(PT(allTable.ranKingTitlePos[k]-mList:getPosition().x,pWinSize.height*0.63/4/2))--设置图片坐标
                listItem:addChild(bgEmpty,0)
             --[[   if k==1 and v==1 or v==2 or v==3 then
                    local successImg=CCSprite:create(P(Image.icon_1015));
                    successImg:setAnchorPoint(PT(0.5,0.5))--设置对其坐标
                    successImg:setPosition(PT(allTable.ranKingTitlePos[k]-mList:getPosition().x,pWinSize.height*0.63/4/2))--设置图片坐标
                    listItem:addChild(successImg,0)
                else]]
                    local value=CCLabelTTF:create(v,FONT_NAME,FONT_SM_SIZE)
                    value:setAnchorPoint(PT(0.5,0.5));
                    value:setPosition(PT(allTable.ranKingTitlePos[k]-mList:getPosition().x,pWinSize.height*0.63/4/2));
                    listItem:addChild(value,0);
              --  end
            end
            --------
            
        end
        
end
--说明按钮想要事件
--说明按钮想要事件
function explainBtnAction()
    if exPlainLayer then
        mLayer:removeChild(exPlainLayer,true)
        exPlainLayer=nil;
    end
    exPlainLayer=CCLayer:create();
    mLayer:addChild(exPlainLayer,2)
    -----
    --透明背景
    local transparentBg=Image.tou_ming;--背景图片
    local menuItem =CCMenuItemImage:itemFromNormalImage(P(transparentBg),P(transparentBg));
    local menuBg=CCMenu:menuWithItem(menuItem);
    menuBg:setContentSize(menuItem:getContentSize());
    menuBg:setAnchorPoint(CCPoint(0,0));
    menuBg:setScaleX(pWinSize.width/menuBg:getContentSize().width*2)
    menuBg:setScaleY(pWinSize.height/menuBg:getContentSize().height*2);
    menuBg:setPosition(CCPoint(0,0));
    exPlainLayer:addChild(menuBg,0);
    ---
   --[[ local explainBg=CCSprite:create(P(Image.panle_1014_1));
    explainBg:setAnchorPoint(PT(0.5,0.5))
    explainBg:setPosition(PT(pWinSize.width/2,pWinSize.height/2));
    exPlainLayer:addChild(explainBg,1)]]
	--背景
	local explainBg= CCSprite:create(P("common/panle_1015.9.png"));
	explainBg:setAnchorPoint(PT(0,0))
	explainBg:setScaleX(pWinSize.width*0.5/explainBg:getContentSize().width)
	explainBg:setScaleY(pWinSize.height*0.6/explainBg:getContentSize().height);
	explainBg:setPosition(PT((pWinSize.width-pWinSize.width*0.5)/2,(pWinSize.height-pWinSize.height*0.6)/2))
	exPlainLayer:addChild(explainBg,0)
    ----关闭按钮
    local explainCloseBtn=ZyButton:new(P(Image.image_button_small_0),P(Image.image_button_small_1),nil,Language.IDS_OK,FONT_NAME,FONT_SM_SIZE)
    explainCloseBtn:setAnchorPoint(PT(0.5,0))
    explainCloseBtn:setPosition(PT(explainBg:getPosition().x+pWinSize.width*0.25,explainBg:getPosition().y+pWinSize.height*0.04));
    explainCloseBtn:registerScriptTapHandler(Ranking.explainCloseBtnAction)
    explainCloseBtn:addto(exPlainLayer,1);
    ----
    local list = ScutCxList:node(pWinSize.height*0.54*0.7, ccc4(24, 24, 24, 255), SZ(pWinSize.width*0.5*0.8,pWinSize.height*0.54*0.7))
	list:setPosition(PT(explainBg:getPosition().x+pWinSize.width*0.5*0.1,explainCloseBtn:getPosition().y+explainCloseBtn:getContentSize().height+SY(5)));
	list:setSelectedItemColor(ccc3(5, 93, 183), ccc3(8, 85, 163));
	exPlainLayer:addChild(list,1);
    ---说明内容
    local content=string.format("<label color='%d,%d,%d'>%s</label>",241,176,63,Language.RANKING_SHUOMING.."\r".."\r\n")
                   ..string.format("<label color='%d,%d,%d'>%s</label>",241,176,63,Language.RANKING_SHUOMING2.."\r\n")
    local explainContent=ZyMultiLabel:new(content,pWinSize.width*0.5*0.8,FONT_NAME,FONT_SM_SIZE,nil,nil);
    local listItem = ScutCxListItem:itemWithColor(ccc3(45,85,89));
    listItem:setOpacity(0);
    listItem:setDrawSelected(true);
    list:setRowHeight(explainContent:getContentSize().height)
    explainContent:addto(listItem,0)
	list:addListItem(listItem, false)  
	list:setTouchEnabled(true)
end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionID = ZyReader:getActionID()
	local userData = ZyRequestParam:getParamData(lpExternalData)
	if actionID==1019 then
	    local table=actionLayer._1019Callback(pScutScene, lpExternalData)
	    if table then
	        update(table.RecordTabel)--更新数据
	    end
	end
end