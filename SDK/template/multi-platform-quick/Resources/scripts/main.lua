-- for CCLuaEngine
function __G__TRACKBACK__(errorMessage)
    print("----------------------------------------")
    print("LUA ERROR: "..tostring(errorMessage).."\n")
    print(debug.traceback("", 2))
    print("----------------------------------------")
end

local strModuleName = "mainapp"
CCLuaLog("Module ".. strModuleName.. " loaded.")
strModuleName = nil

function PushReceiverCallback(pScutScene, lpExternalData)
end

local function ScutMain()
    local strRootDir = ScutDataLogic.CFileHelper:getPath("scripts")
    local strTmpPkgPath = package.path
    
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
    }

    for key, value in ipairs(strSubDirs) do
        local strOld = strTmpPkgPath;
        if(1 == key) then
            strTmpPkgPath = string.format("%s/%s/?.lua%s", strRootDir, value, strOld)
        else
            strTmpPkgPath = string.format("%s/%s/?.lua;%s", strRootDir, value, strOld)
        end
        strOld = nil
    end
    
    package.path = string.format("%s/?.lua;%s", strRootDir, strTmpPkgPath)
    strTmpPkgPath = nil;

    require("lib.lib")
    require("lib.ScutScene")
    require("lib.FrameManager")
    require("datapool.Image")
    require("testScene")
    g_frame_mgr = FrameManager:new()
    g_frame_mgr:init()

    function OnHandleData(pScene, nTag, nNetRet, pData)
        pScene = tolua.cast(pScene, "CCScene")
        g_scenes[pScene]:execCallback(nTag, nNetRet, pData)
    end

    math.randomseed(os.time())
    __NETWORK__=true
    
    function processCommonData(lpScene)
        return true
    end
    function netDecodeEnd(pScutScene, nTag)
    end

    ScutScene:registerNetDecodeEnd("netDecodeEnd")

    function err_handler(str)
        ZyRequestCounter = ZyRequestCounter + 1
        ZyWriter:writeString("ActionId",404 )
        ZyWriter:writeString("ErrorInfo", str)
        ZyExecRequest(ScutScene, nil,isLoading)
    end
end

local function main()
    ScutMain()
    --testScene.init()
end

xpcall(main, __G__TRACKBACK__)
require("app.MyApp").new():run()
