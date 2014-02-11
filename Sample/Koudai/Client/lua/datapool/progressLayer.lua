------------------------------------------------------------------
-- progressLayer.lua
-- Author     : ChenJM
-- Version    : 1.15
-- Date       :
-- Description: 登陆加载界面,
------------------------------------------------------------------

module("progressLayer", package.seeall)
require ("datapool.MusicManager")
require("scenes.MainScene")
require("scenes.ChatScene")
require("datapool.MenuBtnConfig")
require("scenes.PublicAnnouncementScene")
require("scenes.FirstLogin")
require("layers.GuideLayer")

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

_scene = nil 		-- 场景
_createConfig = true
_mProIndex  = nil
_mBgSprite  = nil
_mDownedCfg   = nil
local proSprite
_firstLogin=nil--是否第一次登录

EDownBasicConfig =
{
	eNoneDown = -1,
	eDownPersonalInfo = 1,
	eDownMenuConfig = 2,
	eDownChatInfo= 3,
	eDownNoticeInfo= 4,
	eDownGuideInfo = 5,
	eDownEnd =5,
}


--
---------------公有接口(以下两个函数固定不用改变)--------
--
-- 函数名以小写字母开始，每个函数都应注释


-- 替换场景
function replaceScene(isfirst)
	_firstLogin = isfirst
	initResource()
	createScene()
	CCDirector:sharedDirector():replaceScene(_scene)
end

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
	_mProIndex = 0
	_mDownedCfg =  false
end

-- 释放资源
function releaseResource()
	if layerBG ~= nil then
		layerBG:getParent():removeChild(layerBG, true)
		layerBG = nil
	end
	_mProIndex  = nil
	_mBgSprite  = nil
	_mDownedCfg = nil
	proSprite=nil
end

function onExit ()
	releaseResource()
end;

function nextScene()

	onExit ()
	
	MainScene.setNotice(true)
	if _firstLogin then
		FirstLogin.init()
	else	
		MainScene.init()
--		businessStore.pushScene()
	end

end;

-- 创建场景
function createScene()
	initResource()
	local scene = ScutScene:new()
	-- 注册网络回调
	_scene = scene.root
	scene:registerCallback(networkCallback)
--	_scene:registerOnExit("progressLayer.onExit")
	local size = CCDirector:sharedDirector():getWinSize()
	-- 添加背景
	layerBG = CCLayerGradient:create(ccc4(255, 0, 0, 255), ccc4(255, 0, 0, 255))
	_scene:addChild(layerBG, 0)
	
	-- 此处添加场景初始内容
	local mainBg = CCSprite:create(P("imageupdate/default.png"))
	mainBg:setScaleX(size.width/mainBg:getContentSize().width)
	mainBg:setScaleY(size.height/mainBg:getContentSize().height)
	layerBG:addChild(mainBg, 0)
	mainBg:setPosition(PT(size.width/2,size.height/2))

	
	local bgSprite = CCSprite:create(P("imageupdate/list_1116.png"))
	bgSprite:setPosition(PT(size.width/2,SY(10)))
	layerBG:addChild(bgSprite,0)
	_mBgSprite = bgSprite
	
	actionLayer.Action1008(_scene,nil)
	return _scene
end


function updateProSprite()
	if proSprite~=nil then
		proSprite:getParent():removeChild(proSprite,true)
		proSprite=nil
	end
	local imgStr = P("imageupdate/list_5003.9.png")
	local imgSize =ZyImage:imageSize(imgStr)
	
	local prog =_mProIndex/(EDownBasicConfig.eDownEnd)
	local maxLong=_mBgSprite:getContentSize().width*0.83
	local width=maxLong*prog
	proSprite = CCSprite:create(imgStr)
	proSprite:setScaleY(_mBgSprite:getContentSize().height*0.5/proSprite:getContentSize().height)
	proSprite:setScaleX(width/proSprite:getContentSize().width)
	local sz = _mBgSprite:getContentSize()
	_mBgSprite:removeAllChildrenWithCleanup(true)
	proSprite:setAnchorPoint(PT(0, 0.5))
	proSprite:setPosition(PT(_mBgSprite:getPosition().x-sz.width/2+sz.width*0.087,
					_mBgSprite:getPosition().y+_mBgSprite:getContentSize().height*0.08))
	_mBgSprite:getParent():addChild(proSprite,0)
	if prog>=1 then
		delayExec(nextScene,0.1)
	end
end




function netUpdate()
end

function actionNum(page)
	if  page == nil  then
		page = 1
	end
	
	actionLayer.Action9202(_scene,nil,page,20)
end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionID = ZyReader:getActionID()
	local result = ZyReader:getResult();
	----个人信息
	if actionID ==1008 then
		local personalInfo=actionLayer._1008Callback(pScutScene, lpExternalData)
		if personalInfo~=nil then
			PersonalInfo.setPersonalInfo(personalInfo)
		end
		 actionLayer.Action1009(pScutScene,nil)
	---聊天信息
	elseif actionID ==9204 then
		ChatScene.clearLocalMessage()
		ChatScene.networkCallback(pScutScene, lpExternalData)
		actionNum()
		

	elseif  actionID ==9202 then
		PublicAnnouncementScene.networkCallback(pScutScene, lpExternalData)
		 actionLayer.Action1092(pScutScene,nil)
	elseif actionID ==1012  then

  	elseif actionID ==1009 then
  		 local personalInfo=actionLayer._1009Callback(pScutScene, lpExternalData)
  		 if personalInfo~=nil then 
  		 	MenuBtnConfig.clearMenuItemTables()
	  	 	for k , v in ipairs(personalInfo) do
	  	 		if v ~=nil then
	  	 			MenuBtnConfig.setMenuItem(v.FunEnum)
	  	 		end
	  	 	end
		end
		actionLayer.Action9204(pScutScene,false)
	elseif actionID == 1092 then
		local serverInfo=actionLayer._1092Callback(pScutScene, lpExternalData)
		if serverInfo~=nil then
			GuideLayer.setIsGuide(serverInfo.isPass, serverInfo.guideId, 1)
		end
	end
	
	_mProIndex =  _mProIndex + 1
	updateProSprite()

end

---延迟进行方法
function delayExec(funName,nDuration)
    local  action = CCSequence:createWithTwoActions(
    CCDelayTime:create(nDuration),
    CCCallFunc:create(funName));
    layerBG:runAction(action)
end


function closeCallback()
	CCDirector:sharedDirector():endToLua()
end;

