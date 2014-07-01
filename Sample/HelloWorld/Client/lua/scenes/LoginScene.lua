------------------------------------------------------------------
--LoginScene.lua
-- Author     : JMChen
-- Version    : 1.0.0.0
-- Date       : 2011-10-15
-- DescriCCPointion：登陆界面 
------------------------------------------------------------------


module("LoginScene", package.seeall)




-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写
 
local mScene = nil 		-- 场景
local mLayer = nil 		-- 层

--
---------------公有接口(以下两个函数固定不用改变)--------
--


-- 退出场景
function closeScene()
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
	mScene=nil
	mLayer=nil
end

-- 创建场景
function init(type)
--注册服务器push回调
	initResource()

	-- 注册网络回调
	local scene = ScutScene:new()	
	scene:registerCallback(netCallback)	
	mScene = scene.root;	
	mLayer = CCLayer:create()
	mLayer:setAnchorPoint(CCPoint(0,0))
	mLayer:setPosition(CCPoint(0,0))
	mScene:addChild(mLayer, 0)
	
	CCDirector:sharedDirector():pushScene(mScene)
	CCDirector:sharedDirector():RegisterBackHandler("MainScene.closeApp")
	
	m_CurrentType=type
	if isFirstLogin==nil then
		isFirstLogin=true
	end;
	

    local spriteNor = CCSprite:create(P("button/button_1013.png"))
    local spriteDown = CCSprite:create(P("button/button_1013.png")) 
    spriteDown:setScale(0.94)
    
    local label = CCLabelTTF:create("send", FONT_NAME,FONT_SM_SIZE); 
    label:setPosition(CCPoint(spriteNor:getContentSize().width / 2, spriteNor:getContentSize().height / 2))
    spriteNor:addChild(label, 0)  
    
    local label = CCLabelTTF:create("send", FONT_NAME,FONT_SM_SIZE); 
    label:setPosition(CCPoint(spriteNor:getContentSize().width / 2, spriteNor:getContentSize().height / 2))
    spriteDown:addChild(label, 0) 
    
    spriteDown:setPosition(CCPoint(spriteNor:getContentSize().width * 0.03, spriteNor:getContentSize().height * 0.03))
    local menuItem = CCMenuItemSprite:create(spriteNor, spriteDown)
    local menu = CCMenu:createWithItem(menuItem)
    menu:setContentSize(menuItem:getContentSize())
    menu:setAnchorPoint(CCPoint(0, 0))
    mLayer:addChild(menu,0)
    menu:setPosition(PT(pWinSize.width/2,pWinSize.height*0.2))
    menuItem:registerScriptTapHandler(LoginScene.callAction)
    
    

end


function callAction()
   	ZyWriter:writeString("ActionId",100)
	local lenth=string.len(ZyWriter:generatePostData())
	local addressPath=("127.0.0.1:9001")
	ScutDataLogic.CDataRequest:Instance():AsyncExecTcpRequest(mScene,addressPath, 1, nil, ZyWriter:generatePostData(), lenth);
	ScutDataLogic.CNetWriter:resetData();		
	ScutDataLogic.CNetWriter:resetData() 	
end


---网络回调
function netCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID();
	local userData = ZyRequestParam:getParamData(lpExternalData)
    if actionId ==100 then
        local content=ZyReader:readString();
        local label=CCLabelTTF:create(content,FONT_NAME,FONT_SM_SIZE); 
        mScene:addChild(label,1)
        label:setPosition(CCPoint(pWinSize.width/2,pWinSize.height/2))   
        local secondAction = CCSequence:createWithTwoActions(CCFadeOut:create(0.55),
        CCCallFuncN:create(LoginScene.hide))
        local  action = CCSequence:createWithTwoActions(CCDelayTime:create(1),secondAction)
        label:runAction(action)   
    end
end


ZyWriter = ScutDataLogic.CNetWriter:getInstance()
ZyReader = ScutDataLogic.CNetReader:getInstance()
ZyRequestParam = {param = {}}
function ZyRequestParam:getParamData(nTag)
	return ZyRequestParam.param[nTag]
end

function ZyReader.readString()
	local nLen = ZyReader:getInt()
	local strRet = nil
	if nLen ~= 0
	then
        local str = ScutDataLogic.CLuaString:new("")
        ZyReader:getString(str, nLen)
        strRet = string.format("%s", str:getCString())
        str:delete()
	end
	return strRet
end

function hide(node)
	if node~= nil then
	    node:getParent():removeChild(node, true)
	    node=nil
	end
end