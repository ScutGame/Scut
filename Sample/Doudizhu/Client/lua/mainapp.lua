------------------------------------------------------------------
-- mainapp.lua
-- Author     : Xin Zhang
-- Version    : 1.0.0.0
-- Date       : 2011-10-15
-- Description:
------------------------------------------------------------------

local strModuleName = "mainapp";
--CCLuaLog("Module ".. strModuleName.. " loaded.");
strModuleName = nil;

------------------------------------------------------------------
-- ↓↓ 初始化环境变量 开始 ↓↓
------------------------------------------------------------------

local strRootDir = ScutDataLogic.CFileHelper:getPath("lua");
local strTmpPkgPath = package.path;

local strSubDirs =
{
	"scenes",
	"layers",
	"datapool",
	"config",
	"action",
	"lib",
	"commupdate",
	"payment",
	
	-- 在此添加新的目录
};

--package.path = string.format("%s/?.lua;%s/lib/?.lua;%s/action/?.lua;%s/common/?.lua;%s/datapool/?.lua;%s/Global/?.lua;%s/layers/?.lua;%s/LuaClass/?.lua;%s/scenes/?.lua;%s/titleMap/?.lua;%s",strRootDir,strRootDir,strRootDir,strRootDir,strRootDir,strRootDir,strRootDir,strRootDir,strRootDir,strRootDir, strTmpPkgPath);

-- 逐个添加子文件夹
for key, value in ipairs(strSubDirs) do
	local strOld = strTmpPkgPath;
	if(1 == key) then
		strTmpPkgPath = string.format("%s/%s/?.lua%s", strRootDir, value, strOld);
	else
		strTmpPkgPath = string.format("%s/%s/?.lua;%s", strRootDir, value, strOld);
	end
--	CCLuaLog(value.. " added.");
	strOld = nil;
end

package.path = string.format("%s/?.lua;%s", strRootDir, strTmpPkgPath);
strTmpPkgPath = nil;

------------------------------------------------------------------
-- ↑↑ 初始化环境变量 结束 ↑↑
------------------------------------------------------------------

-- require必须在环境变量初始化之后，避免文件找不到的情况发生
require("lib.lib")
require("scenes.LoginScene")
require("datapool.PushReceiverLayer")
require("scenes.MainScene")

 g_frame_mgr = FrameManager:new()
    g_frame_mgr:init()

    function OnHandleData(pScene, nTag, nNetRet, pData)
        pScene = tolua.cast(pScene, "CCScene")
        g_scenes[pScene]:execCallback(nTag, nNetRet, pData)
    end

math.randomseed(os.time());
__NETWORK__=true
------------------------------------------------------------------
-- ↓↓ 协议解析函数注册 开始 ↓↓
------------------------------------------------------------------

function processCommonData(lpScene)
   return true;
end

function netDecodeEnd(pScutScene, nTag)
	ZyLoading.hide(pScutScene, nTag)
end
--注册服务器push回调
CCDirector:sharedDirector():RegisterSocketPushHandler("PushReceiverLayer.PushReceiverCallback");
--NDFixSDK.FixCocos2dx:CreateFixCocos2dx():RegisterSocketPushHandler("PushReceiverLayer.PushReceiverCallback")
--ScutScene:registerNetCommonDataFunc("processCommonData");
ScutScene:registerNetErrorFunc("LoginScene.netConnectError2");
ScutScene:registerNetDecodeEnd("netDecodeEnd");
--NdUpdate.CUpdateEngine:getInstance():registerResPackageUpdateLuaHandleFunc("CommandDataResove.resourceUpdated")

CCDirector:sharedDirector():RegisterBackHandler("MainScene.closeApp")
--注册crash log回调
CCDirector:sharedDirector():RegisterErrorHandler("err_handler")

--
function err_handler(str)
	ZyRequestCounter = ZyRequestCounter + 1
	ZyWriter:writeString("ActionId",404 );
	ZyWriter:writeString("ErrorInfo", str)
	ZyExecRequest(ScutScene, nil,isLoading)
--	ScutDataLogic.CDataRequest:Instance():AsyncExecRequest(ScutScene, ZyWriter:generatePostData(), ZyRequestCounter, nil);
--	ScutDataLogic.CNetWriter:resetData()
end

------------------------------------------------------------------
-- ↑↑ 协议解析函数注册 结束 ↑↑
------------------------------------------------------------------


LoginScene.init()

