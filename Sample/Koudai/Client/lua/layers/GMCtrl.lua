------------------------------------------------------------------
-- GMCtrl.lua
-- Author     : WRS
-- Version    : 1.15
-- Date       :   
-- Description: GM命令,
------------------------------------------------------------------


module ("GMCtrl",package.seeall);

local m_Layer = nil
local mScene = nil
local m_ShowTraning_Width = nil
local m_ShowTraning_Height = nil
local m_startX = nil
local m_startY = nil

function releaseResource()
    if m_Layer~=nil then
        m_Layer:getParent():removeChild(m_Layer,true) 
    end
    
    m_Layer = nil
    mScene = nil
    m_ShowTraning_Width = nil
    m_ShowTraning_Height = nil
    m_startX = nil
    m_startY = nil
end

function init(mainLayer)
    m_Layer=CCLayer:create();
    --设置界面的大小和移动
    m_Layer:setContentSize(SZ(pWinSize.width,pWinSize.height))
    m_Layer:setPosition(PT(SX(0),SY(0)))
    mainLayer:addChild(m_Layer,1)

    ----平铺整个界面透明图片 禁止点击上一层
    local menuItem=CCMenuItemImage:create(P("common/transparentBg.png"), P("common/transparentBg.png"));
    local menuBG=CCMenu:createWithItem(menuItem);
    menuBG:setContentSize(menuItem:getContentSize())
    menuBG:setAnchorPoint(PT(0, 0))
    menuBG:setScaleX(pWinSize.width/menuItem:getContentSize().width)
    menuBG:setScaleY(pWinSize.height/menuItem:getContentSize().height)
    m_Layer:addChild(menuBG,0)
    
    m_EditReply = ZyMessageBoxEx:new()
    m_EditReply:doModify(MainScene.pMainScene,Language.GM_TITLE,Language.GM_MESSAGE,Language.IDS_CANCEL,Language.IDS_SURE,sendAction)

end


function sendAction(clickedButtonIndex,content,tag)
   if clickedButtonIndex==ID_MBOK then--确认
        if content~=nil and string.len(content)>0 then
            actionLayer.Action1000(MainScene.pMainScene,nil,content)
        else
            ZyToast.show(MainScene.pMainScene,Language.GM_MESSAGE2,2,0.35)     
        end
          
    elseif clickedButtonIndex == ID_MBCANCEL  then--取消   
    end
     releaseResource()
end
