------------------------------------------------------------------
-- RoleDetailScene.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 
------------------------------------------------------------------

module("RoleDetailScene", package.seeall)


mScene = nil 		-- 场景



-- 场景入口
function pushScene()
	initResource()
	createScene()
	
end

-- 退出场景
function popScene()
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
	mLayer = nil
	mScene = nil
end

-- 创建场景
function createScene()
	local scene = ScutScene:new()
	mScene = scene.root
		mScene:registerScriptHandler(SpriteEase_onEnterOrExit)
	runningScene = CCDirector:sharedDirector():getRunningScene()
	if runningScene == nil then
		CCDirector:sharedDirector():runWithScene(mScene)
	else
		 SlideInLReplaceScene(mScene)
	end
	MainScene.releaseResource()

	-- 注册网络回调
	scene:registerCallback(networkCallback)
	
	-- 添加主层
	mLayer= CCLayer:create()
	mScene:addChild(mLayer, 0)


	--创建背景
	local bgLayer = UIHelper.createUIBg(nil,"",ZyColor:colorYellow(),"RoleDetailScene.popScene")
	mLayer:addChild(bgLayer,0)

	-- 此处添加场景初始内容
	
end


function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        releaseResource()
    end
end


--发送请求
function sendActionId(actionId)
	if actionId == 1 then


	elseif actionId == 2 then


	end

end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if actionId == 1 then


	elseif actionId == 2 then


	end
end