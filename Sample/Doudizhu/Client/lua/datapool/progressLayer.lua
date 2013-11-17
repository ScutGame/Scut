------------------------------------------------------------------
-- progressLayer.lua
-- Author     : ChenJM
-- Version    : 1.15
-- Date       :
-- DescriCCPointion: 登陆加载界面,
------------------------------------------------------------------

module("progressLayer", package.seeall)
require ("datapool.MusicManager")
--require("chatFunction.PrivateChatLayer")
--require("chatFunction.ChatBoxLayer")
--require("layers.HelpLayer")
-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

_scene = nil 		-- 场景
_createConfig = true
_mProIndex  = nil
_mBgSprite  = nil
_mDownedCfg   = nil
local proSprite
local isFirstTcpReg=nil


EDownBasicConfig =
{
	eNoneDown = -1,
	eDownPersonalInfo = 1,
	eDownMenuConfig = 2,
	eDownChatInfo= 3,
	eDownNoticeInfo= 4,
	eDownEnd =2,
}


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

-- 替换场景
function replaceScene()
	initResource()
	createScene()
	CCDirector:sharedDirector():pushScene(_scene)
end

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
    _mProIndex =0
    _mDownedCfg =  false
end
-- 释放资源
function releaseResource()
    _mProIndex  = nil
    _mBgSprite  = nil
    _mDownedCfg   = nil
    proSprite=nil
    isFirstTcpReg=nil
end

function onExit ()
	 releaseResource()
end;

-- 创建场景
function createScene()
     initResource()
	local scene = ScutScene:new()
	_scene = scene.root 
	-- 注册网络回调
	scene:registerCallback(networkCallback)
	--_scene:registerOnExit("progressLayer.onExit")
	local size = CCDirector:sharedDirector():getWinSize()
	-- 添加背景
	local layerBG = CCLayer:create()
	_scene:addChild(layerBG, 0)

	-- 此处添加场景初始内容
	local mainBg = CCSprite:create(P("imageupdate/default.jpg"))
	mainBg:setScaleX(pWinSize.width/mainBg:getContentSize().width)
	mainBg:setScaleY(pWinSize.height/mainBg:getContentSize().height)
	layerBG:addChild(mainBg, 0)
	mainBg:setPosition(CCPoint(size.width/2,size.height/2))

    local bgSprite = CCSprite:create(P("imageupdate/backprogress.png"))
    bgSprite:setPosition(CCPoint(size.width/2,bgSprite:getContentSize().height))
    layerBG:addChild(bgSprite,0)
    _mBgSprite = bgSprite
    actionLayer.Action100(_scene,nil)
   --actionLayer.Action1008(_scene,nil)
	return _scene
end
function nextScene()
	 releaseResource()
	-- playMusic(EnumMusicType.bgMusic)
	MainScene.releaseResource()
 	MainScene.initScene()
 	ScutScene:registerNetErrorFunc("LoginScene.netConnectError")	
end;

function updateProSprite()
	if proSprite~=nil then
		proSprite:getParent():removeChild(proSprite,true)
		proSprite=nil
	end
    local imgStr = P("imageupdate/list_5003.9.png")
    local imgSize =ZyImage:imageSize(imgStr)

   local prog =_mProIndex/(EDownBasicConfig.eDownEnd)
   local maxLong=_mBgSprite:getContentSize().width*0.96
   local width=maxLong*prog
     proSprite = CCSprite:create(imgStr)
    proSprite:setScaleY(_mBgSprite:getContentSize().height*0.5/proSprite:getContentSize().height)
    proSprite:setScaleX(width/proSprite:getContentSize().width)
    local sz = _mBgSprite:getContentSize()
    _mBgSprite:removeAllChildrenWithCleanup(true)
    proSprite:setAnchorPoint(CCPoint(0, 0.5))
    proSprite:setPosition(CCPoint(_mBgSprite:getPosition().x-sz.width*0.48,_mBgSprite:getPosition().y+_mBgSprite:getContentSize().height*0.06))
    _mBgSprite:getParent():addChild(proSprite,0)
    if prog>=1 then
    	delayExec(nextScene,0.05, _mBgSprite)
--   	nextScene()
    end
end



function netUpdate()

end

--下载配置文件
function downConfig()
	requireConfig(_scene)
end

--下载背包数据
function downPackage()
	Packages.RequestPackage(_scene,false)
end

---延迟进行方法
function delayExec(funName,nDuration, parent)
	local  action = CCSequence:createWithTwoActions(
	CCDelayTime:create(nDuration),
	CCCallFuncN:create(funName));
	parent:runAction(action)
end



-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionID = ZyReader:getActionID()
	local result = ZyReader:getResult();
	----个人信息
	
    if actionID ==100 then
    	if ZyReader:getResult() == eScutNetSuccess then
    		actionLayer.Action1008(pScutScene,nil)
    	else
    		if not isFirstTcpReg then
    			isFirstTcpReg=true
    			actionLayer.Action100(pScutScene,nil)
    		else
    			actionLayer.Action1008(pScutScene,nil)
    		end
    	end	
   elseif     actionID ==1008 then
        local personalInfo=actionLayer._1008Callback(pScutScene, lpExternalData)
  	 	if personalInfo~=nil then
  	 		PersonalInfo.getPersonalInfo()._Wings=nil
  	 		PersonalInfo.setPersonalInfo(personalInfo)
  	 		if personalInfo.RoomId >0 then
  	 			MainScene.setGotoType(2)
  	 		end
  	 	end
    end
        _mProIndex =  _mProIndex + 1
         updateProSprite()

end

function makeIsWing(info)
	local isWing = false	
	for k, v in  pairs(info) do
		if v.MsgType==12 then
			isWing = true
		end
	end
	return isWing
end

function closeCallback()
	CCDirector:sharedDirector():endToLua()
end;

function downRoleInfo()
	-- 请求用户角色详细信息(1504)
	ZyWriter:writeInt32("ActionId", NetActionID.EServerType.eGetUserDetail)
	ZyWriter:writeInt64("UserId", accountInfo:getUserId())
	ZyExecRequest(_scene, nil,false)
end


--【 1504 - 请求用户角色详细信息(同5008)】
function Callback_1504(pScutScene, lpExternalData)
	if ZyReader:getResult() == eScutNetSuccess then
		local Response_1504 = nil
		if ZyReader:getInt() ~= 0 then
			Response_1504 = {}
			ZyReader:recordBegin()
			Response_1504.IsVIP = ZyReader:getBYTE() == 1
			ZyReader:recordEnd()
			--建筑等级信息
			accountInfo.setDetailData(Response_1504)
			NewUserHelp.initData()
		end
	end
end
