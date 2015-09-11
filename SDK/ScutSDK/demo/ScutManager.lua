-------------------------------------------------------------------
--文件：ScutManager.lua
--作者：wangsheng
--日期：2015年6月30日
--描述：服务器接口管理器
-------------------------------------------------------------------
require "NetHelper"

function OnHandleData(pScene, nTag, nNetRet, lpData, lpExternal)
    ScutManager:execCallback(nTag, nNetRet, lpData,lpExternal)
end

ScutManager = class("ScutManager")

local loadingCache = {}--loading缓存
local callbackCache = {}--回调函数
local callTag = 0--调用的id递增
ScutManager.url = "192.168.2.98:9001"--服务器地址

--初始化
function ScutManager:init()
    ScutDataLogic.CNetWriter:setUrl(ScutManager.url)
    
    cc.Director:getInstance():getScheduler():scheduleScriptFunc(function()
        ScutDataLogic.CDataRequest:Instance():PeekLUAData()
    end, 0, false)
end

--执行网络回调函数。
function ScutManager:execCallback(nTag, nNetState, lpData,lpExternal)
    local actionID = ZyReader:getActionID()
    local lpExternalData = lpExternalData or 0
    local userData = ZyRequestParam:getParamData(lpData)
    if 2 == nNetState then
        local reader = ScutDataLogic.CDataRequest:Instance()
        local bValue = reader:LuaHandlePushDataWithInt(lpData)
        if callbackCache[nTag] then
            callbackCache[nTag]()
            callbackCache[nTag]=nil
        end
        self:netDecodeEnd(nTag)
    end
end
--调用接口
--callBack回调函数
--url地址
--lpData为用户附加数据，该数据将被直接传回。
function ScutManager:doHandler(callBack,url,lpData)
    ZyExecRequest(cc.Director:getInstance():getRunningScene(),lpData,url or ScutManager.url)
    if callBack then
        callbackCache[ZyRequestTag] = callBack
    end
    --添加遮罩
--    local loading = ClassManager.createInstance("LoadingLayer")
--    loadingCache[ZyRequestTag] = loading
--    UIManager.push(loading)
end
--执行完毕
function ScutManager:netDecodeEnd(nTag)
    --调用完毕，如有loading 移除loading
--    local loading = loadingCache.nTag
--    if loading then
--    	UIManager.pop(loading:getId())
--    	loadingCache.nTag = nil
--    end
end

ScutManager:init()