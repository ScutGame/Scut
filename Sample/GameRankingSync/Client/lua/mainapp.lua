------------------------------------------------------------------
-- mainapp.lua
-- Author     : LZW
-- Version    : 1.0.0.0
-- Date       : 2014-1-20
-- Description:
------------------------------------------------------------------


local strModuleName = "mainapp"
CCLuaLog("Module ".. strModuleName.. " loaded.")
strModuleName = nil

function PushReceiverCallback(pScutScene, lpExternalData)
    testScene.netCallback(pScutScene, lpExternalData, true)
end

local function ScutMain()
    ------------------------------------------------------------------
    -- ↓↓ 初始化环境变量 开始 ↓↓
    ------------------------------------------------------------------
    
    local strRootDir = ScutDataLogic.CFileHelper:getPath("lua");
    local strTmpPkgPath = package.path;
    
    local strSubDirs =
    {
      
        "lib",       
        "SyncSDK"
        
        -- 在此添加新的目录
    };
    
    -- 逐个添加子文件夹
    for key, value in ipairs(strSubDirs) do
        local strOld = strTmpPkgPath;
        if(1 == key) then
            strTmpPkgPath = string.format("%s/%s/?.lua%s", strRootDir, value, strOld);
        else
            strTmpPkgPath = string.format("%s/%s/?.lua;%s", strRootDir, value, strOld);
        end
        strOld = nil;
    end
    
    package.path = string.format("%s/?.lua;%s", strRootDir, strTmpPkgPath);
    strTmpPkgPath = nil;

    ------------------------------------------------------------------
    -- ↑↑ 初始化环境变量 结束 ↑↑
    ------------------------------------------------------------------
    
    -- require必须在环境变量初始化之后，避免文件找不到的情况发生
    require("lib.lib")
    require("lib.ScutScene")
    require("lib.FrameManager")
    require("SyncSDK.ScutDataSync")
    require("testScene")
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

    end

    --注册服务器push回调
   ScutExt:getInstance():RegisterSocketPushHandler("ScutDataSync.PushReceiverCallback")
   ScutScene:registerNetDecodeEnd("netDecodeEnd");
    ------------------------------------------------------------------
    -- ↑↑ 协议解析函数注册 结束 ↑↑
    ------------------------------------------------------------------

end

function __G__TRACKBACK__(msg)
    print("----------------------------------------")
    print("LUA ERROR: " .. tostring(msg) .. "\n")
    print(debug.traceback())
    print("----------------------------------------")
end

local function main()
    ScutMain()
    testScene.init()
end

xpcall(main, __G__TRACKBACK__)

--ScutDataSync.registerSceneCallback(ScutEntityListener.NotifySceneLayer)