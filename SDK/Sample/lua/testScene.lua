module("testScene", package.seeall)
require("commonString")
------获取资源的路径---------------------
function P(fileName)
    if fileName then
        return ScutDataLogic.CFileHelper:getPath(fileName)
    else
        return nil
    end
end
local mRankLayer
local mLayer
local tip1 
local tip2 
local tip3 
local bgLayer
local submitLayer
local allTable={};
local mList;
local mNameEdit;
local mScoreEdit;
local isCanSubmit = true
local isCanGetRank = true 
local mNameStr;
local mScoreStr;

function closeBtnActon()
    if bgLayer then 
        mScene:removeChild(bgLayer,true);
    end 
end

function step3()
    if(tip3) then 
        tip3:getParent():removeChild(tip3, true)
        tip3 = nil
    end 
    --[[
    --透明背景
    local transparentBg=bg2;--背景图片
    local sprite = CCSprite:create(P(transparentBg));
    local sprite2 = CCSprite:create(P(bg1));
    sprite2:setAnchorPoint(PT(0.5,0.5));
    sprite2:setPosition(PT(sprite:getContentSize().width/2, sprite:getContentSize().height/2));
    sprite:addChild(sprite2,0);
    sprite:setAnchorPoint(PT(0.5,0.5));
    sprite:setPosition(PT(mScene:getContentSize().width/2, mScene:getContentSize().height/2));
    mScene:addChild(sprite,0);
   ]]   
end

function step2()
if(tip2) then 
        tip2:getParent():removeChild(tip2, true)
        tip2 = nil
    end 
     tip3 = CCLabelTTF:create(commonString.IDS_TIP3, "sdfsfe", 20)
    tip3:setPosition(PT(SX(240),SY(160)))
    mScene:addChild(tip3)
    local  action = CCSequence:createWithTwoActions(
    CCDelayTime:create(0.5),
    CCCallFuncN:create(step3));
    mScene:runAction(action)
end

function step1()
    if(tip1) then 
        tip1:getParent():removeChild(tip1, true)
        tip1 = nil
    end 
     tip2 = CCLabelTTF:create(commonString.IDS_TIP2, "sdfsfe", 20)
    tip2:setPosition(PT(SX(240),SY(160)))
    mScene:addChild(tip2)
    local action = CCSequence:createWithTwoActions(
    CCDelayTime:create(0.5),
    CCCallFuncN:create(step2));
    mScene:runAction(action)
end 

function showRank()
    if isCanGetRank == false then 
        return 
    end 

        ScutDataLogic.CNetWriter:getInstance():writeString("ActionId",1001)
    ScutDataLogic.CNetWriter:getInstance():writeString("PageIndex",1  )
    ScutDataLogic.CNetWriter:getInstance():writeString("PageSize",30)
    ZyExecRequest(mScene, nil,false,1)
end

function submitOK()    
        local name= mNameEdit:getText()
        local sorce= mScoreEdit:getText()
        if  name==""  or sorce == "" then
            --跳出的提示界面
            local box = ZyMessageBoxEx:new()
            box:doPrompt(mScene, nil, commonString.IDS_EMPTY_TIP,commonString.IDS_OK,messageCallback)
            mNameEdit:setVisible(false);
            mScoreEdit:setVisible(false);
        else
        ScutDataLogic.CNetWriter:getInstance():writeString("ActionId",1000)
        ScutDataLogic.CNetWriter:getInstance():writeString("UserName",name )
        ScutDataLogic.CNetWriter:getInstance():writeString("Score",sorce)    
        ZyExecRequest(mScene, nil,false,1) 
        end
end

function messageCallback()
    mNameEdit:setVisible(true);
    mScoreEdit:setVisible(true);
end

function submitCancle()
    closeSubmitLaye()
end 

function submit()
   --submitLayer = UIHelper.createUIBg(nil,nil,ZyColor:colorWhite(),nil,true);--没有背景框
    if isCanSubmit == false then
        return 
    end 
    isCanSubmit = false
    isCanGetRank = false
    local aa=nil
    local ww=288
    local hh=0
    local xx=(pWinSize.width-ww)/2
    --密码文字
    local imgSprite=CCSprite:create(P(Image.image_list_txt));
    local txt_h= imgSprite:getContentSize().height--输入框高
   submitLayer = CCLayer:create();
   submitLayer:setContentSize(CCSize(SX(240),SY(160)));
   mScene:addChild(submitLayer);
   submitLayer:setAnchorPoint(PT(0.5,0.5));
   submitLayer:setPosition(PT(mScene:getContentSize().width/2, mScene:getContentSize().height/2));
   local sprite = CCSprite:create(P("common/panel_1002_12.png"))
   sprite:setScaleX(SX(240)/sprite:getContentSize().width);
   sprite:setScaleY(SY(160)/sprite:getContentSize().height);
   submitLayer:addChild(sprite,0);
   local startY = 0
   local titleName1=CCLabelTTF:create(commonString.IDS_NICKNAME,FONT_NAME, FONT_DEF_SIZE);
    titleName1:setAnchorPoint(CCPoint(0,0))
    titleName1:setPosition(CCPoint(SX(-100),
        startY+titleName1:getContentSize().height - SY(30)))
    submitLayer:addChild(titleName1)
    titleName1:setColor(ccc3(0,0,0))
    --[文本输入框2底图
    local txt_x=titleName1:getPositionX()+SX(8)+titleName1:getContentSize().width--输入宽坐标
    local txt_ww=xx+ww-txt_x-SX(44)--输入框宽
    
    local bgEmCCPointy1= CCSprite:create(P(Image.image_list_txt))
  
    mNameEdit =  CCEditBox:create(CCSize(SX(120),bgEmCCPointy1:getContentSize().height), CCScale9Sprite:create(P(Image.image_list_txt)))
    mNameEdit:setPosition(CCPoint(titleName1:getPositionX()+ titleName1:getContentSize().width +  SX(60) ,titleName1:getPositionY()+SY(5)))
    mNameEdit:setFontColor(ccc3(0,0,0))
    submitLayer:addChild(mNameEdit)

    local titleName=CCLabelTTF:create(commonString.IDS_SORCE, "sfeew", FONT_DEF_SIZE);
    titleName:setColor(ccc3(0,0,0))
    titleName:setAnchorPoint(CCPoint(0,0))
    aa=(hh/2-titleName:getContentSize().height)/2--间距
    titleName:setPosition(CCPoint(titleName1:getPositionX(),titleName1:getPositionY()+txt_h+SY(10)))
    submitLayer:addChild(titleName)

    mScoreEdit = CCEditBox:create(CCSize(SX(120),bgEmCCPointy1:getContentSize().height), CCScale9Sprite:create(P(Image.image_list_txt)))
    mScoreEdit:setFontColor(ccc3(0,0,0))
    mScoreEdit:setPosition(CCPoint(titleName:getPositionX() + titleName:getContentSize().width + SX(60) ,titleName:getPositionY()+SY(5)))
    submitLayer:addChild(mScoreEdit)
    mScoreEdit:setVisible(true)--是否显示
    
    local button2 = ZyButton:new(P("button/button_1011.png"),P("button/button_1012.png"),nil,commonString.IDS_OK)
    button2:setPosition(PT(SX(-30) -button2:getContentSize().width,SY(-50)));
    button2:addto(submitLayer,0)
    button2:registerScriptTapHandler(submitOK);
    
    local button3 = ZyButton:new(P("button/button_1011.png"),P("button/button_1012.png"),nil,commonString.IDS_CANCLE)
    button3:setPosition(PT(SX(30) ,SY(-50)));
    button3:addto(submitLayer,0)
    button3:registerScriptTapHandler(submitCancle);
end 

function closeSubmitLaye()
     mScene:removeChild(submitLayer,true)
     isCanSubmit = true 
     isCanGetRank = true 
end 

function init()
    if mScene then
        return 
    end
    -- 注册网络回调
    local scene = ScutScene:new()
    mScene = scene.root
    scene:registerCallback(netCallback)
    
    CCDirector:sharedDirector():pushScene(mScene)
    pWinSize = mScene:getContentSize()
    
    mLayer = CCLayer:create()
    mLayer:setAnchorPoint(CCPoint(0,0))
    mLayer:setPosition(CCPoint(0,0))
    mScene:addChild(mLayer, 0)
    
    mRankLayer = CCLayer:create();
    mRankLayer:setAnchorPoint(PT(0.5, 0.5));
    mRankLayer:setPosition(PT(pWinSize.width/2, pWinSize.height/2));
    --mLayer:addChild(mRankLayer,0);
    
    CCDirector:sharedDirector():pushScene(mScene)
    
    local bgSprite=CCSprite:create(P("beijing.jpg"))
    bgSprite:setScaleX(pWinSize.width/bgSprite:getContentSize().width)
    bgSprite:setScaleY(pWinSize.height/bgSprite:getContentSize().height)
    bgSprite:setAnchorPoint(CCPoint(0.5,0.5))
    bgSprite:setPosition(CCPoint(pWinSize.width/2,pWinSize.height/2));
    mScene:addChild(bgSprite);
   ScutDataLogic.CNetWriter:setUrl("http://ph.scutgame.com/service.aspx")
 --   ScutDataLogic.CNetWriter:setUrl("http://115.29.163.76:8101/Service.aspx")
 
    local button = ZyButton:new(P("icon_1011.png"));
    button:addto(mScene,0);
    button:setPosition(PT(pWinSize.width - button:getContentSize().width - SX(10), SY(10)));
    button:registerScriptTapHandler(showRank)
    
    local button2 = ZyButton:new(P("button/button_1011.png"),P("button/button_1012.png"),nil,commonString.IDS_SUBMIT)
    button2:setPosition(PT(pWinSize.width/2 - button2:getContentSize().width/2 ,SY(10)));
    button2:addto(mScene,0)
    button2:registerScriptTapHandler(submit);
	
	
    --mLayer:addChild(bgSprite,0)
end 

function netCallback(pZyScene, lpExternalData)  
    local actionID = ZyReader:getActionID()
    local lpExternalData = lpExternalData or 0
    local userData = ZyRequestParam:getParamData(lpExternalData)
    if actionID==1001 then
        local table =  _1001Callback(pZyScene, lpExternalData);
        if table then
            bgLayer= UIHelper.createUIBg(nil,nil,ccc3(255,255,255),nil,true)--没有背景框
            mScene:addChild(bgLayer);   
         --返回按钮
            local closeBtn=ZyButton:new(P(Image.image_exit),P(Image.image_exit));
            closeBtn:setPosition(PT(bgLayer:getContentSize().width-closeBtn:getContentSize().width - SX(15),bgLayer:getContentSize().height-closeBtn:getContentSize().height - SY(5)));
            closeBtn:registerScriptTapHandler(closeBtnActon);
            closeBtn:addto(bgLayer,99);
             showLayout()
             update(table.RecordTabel)--更新数据
        end
    elseif actionID == 1000 then 
          _1000Callback(pZyScene, lpExternalData);
    end
end

function showLayout()
    if layoutLayer then
        mLayer:removeChild(layoutLayer,true)
        layoutLayer=nil;
    end
    layoutLayer=CCLayer:create()
    bgLayer:addChild(layoutLayer,1)
    local simpleW=(pWinSize.width*0.8)/3;  --每个框要多少宽度
    local Bg=CCSprite:create(P(Image.panle_1019_1_9));
    local scalex=simpleW/Bg:getContentSize().width;
    local scaleList={0.7,1.1,1.2}--竞技榜中的列表属性
    local cursor=pWinSize.width*0.1;
    local listBgStartY=pWinSize.height*0.75;
    allTable.ranKingTitlePos={};--位置
    allTable.biliSize={};--每一个标题的比例大小

    local table=nil;
     --名次--昵称--胜率
    table={commonString.IDS_ORDER,commonString.IDS_NICKNAME1,commonString.IDS_SORCE1}
   ----
   allTable.title={}
   for i,v in ipairs(table) do  
        local temp=scaleList[i];
        local textlabel=CCLabelTTF:create(v,FONT_NAME,FONT_SM_SIZE);
        textlabel:setAnchorPoint(PT(0.5,0));
       textlabel:setColor(ccRED1)
       textlabel:setPosition(PT(cursor+simpleW*temp/2,listBgStartY+SY(2)))
        allTable.ranKingTitlePos[i]=(textlabel:getPositionX());
       allTable.biliSize[i]=simpleW*scaleList[i]
       cursor=cursor+(simpleW*scaleList[i])
        layoutLayer:addChild(textlabel,1)
        allTable.title[i]=textlabel
    end
   mScrollView = CCScrollView:create()
   mScrollSize = SZ(pWinSize.width*0.8 , pWinSize.height*0.63)
   if nil ~= mScrollView then
      mScrollView:setViewSize(mScrollSize)
      mScrollView:setPosition(PT(pWinSize.width*0.1,listBgStartY - pWinSize.height*0.63))
      mScrollView:setScale(1.0)
      mScrollView:ignoreAnchorPointForPosition(true) 
      mScrollView:setDirection(kCCScrollViewDirectionVertical);
   end 
   layoutLayer:addChild(mScrollView) 
end

function update(mtable)
    local bgEmpty  = CCSprite:create(P(Image.panle_1019_1_9))
    local layer = CCLayer:create();
    --layer:setColor(ccc4(100,0,0,100))
    layer:setContentSize(CCSize(mScrollSize.width, bgEmpty:getContentSize().height*#mtable))
    layer:setPosition(PT(0, -bgEmpty:getContentSize().height*#mtable + mScrollSize.height))
    if layer and mScrollView then 
        mScrollView:setContainer(layer)
        mScrollView:updateInset()
    end 
    
    for i, v in ipairs(mtable) do 
        local bgEmpty  = CCSprite:create(P(Image.panle_1019_1_9))
        local bgLayer =  CCLayer:create();
        bgLayer:setContentSize(CCSize(mScrollSize.width, bgEmpty:getContentSize().height))
        layer:addChild(bgLayer,0)
        bgLayer:setPosition(PT(-SX(50),layer:getContentSize().height-i*bgEmpty:getContentSize().height))
        local table={i,v.UserName,v.Score }
        for k, v in ipairs(table) do 
            local bgEmpty= CCScale9Sprite:create(P(Image.panle_1019_1_9));
             --   bgEmpty:setScaleX(allTable.biliSize[k]*0.95/bgEmpty:getContentSize().width)--设置缩放比例
                bgEmpty:setContentSize(CCSize(allTable.biliSize[k]*0.95,bgEmpty:getContentSize().height));
                bgEmpty:setAnchorPoint(PT(0.5,0.5))--设置对其坐标
                bgEmpty:setPosition(PT(allTable.ranKingTitlePos[k]-layer:getPositionX(),pWinSize.height*0.63/4/2))--设置图片坐标
                bgLayer:addChild(bgEmpty,0)
                    local value=CCLabelTTF:create(v,FONT_NAME,FONT_SM_SIZE)
                    value:setAnchorPoint(PT(0.5,0.5));
                    value:setPosition(PT(allTable.ranKingTitlePos[k]-layer:getPositionX(),pWinSize.height*0.63/4/2));
                    bgLayer:addChild(value,0);
        end 
    end

end

function _1001Callback(pZyScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == 0 then
        DataTabel={}
        DataTabel.PageCount= ZyReader:getInt()
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.UserName= ZyReader:readString()
                mRecordTabel_1.Score= ZyReader:getInt()
                ZyReader:recordEnd()
                table.insert(RecordTabel_1,mRecordTabel_1)
              end
        end
        DataTabel.RecordTabel = RecordTabel_1;
    else          
          local box = ZyMessageBoxEx:new()
          box:doPrompt(pZyScene, nil, ZyReader:readErrorMsg(),commonString.IDS_OK)
    end
    return DataTabel
end

function _1000Callback(pZyScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == 0 then
        DataTabel={}
       closeSubmitLaye()
    else          
          local box = ZyMessageBoxEx:new()
          box:doPrompt(pZyScene, nil, ZyReader:readErrorMsg(),commonString.IDS_OK)
    end
    return DataTabel
end
