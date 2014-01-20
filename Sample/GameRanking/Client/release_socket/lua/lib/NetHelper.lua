

ZyRequestParam = {param = {}}
function ZyRequestParam:getParamData(nTag)
    return ZyRequestParam.param[nTag]
end
ZyRequestCounter = 1
ZyReader = ScutDataLogic.CNetReader:getInstance()

eScutNetSuccess = 0
eScutNetError = 10000

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
function ZyReader:readInt64()
    return ScutDataLogic.CInt64:new_local(ZyReader:getCInt64())
end

function ZyReader.readErrorMsg()
    return string.format("%s", ZyReader:getErrMsg():getCString())
end

ZyWriter = ScutDataLogic.CNetWriter:getInstance()


function ZyExecRequest(pScutScene, lpData, bShowLoading, addressPath)
    if  bShowLoading == true then
        --LoadingScene.init(pScutScene)
    end

    ZyRequestCounter = ZyRequestCounter + 1
    if  bShowLoading==nil then
        ZyLoading.show(pScutScene, ZyRequestCounter)
    end
    
    if lpData then
        table.insert(ZyRequestParam.param, ZyRequestCounter, lpData)
    end
    if addressPath == nil  then
        ScutDataLogic.CDataRequest:Instance():AsyncExecRequest(pScutScene, ZyWriter:generatePostData(), ZyRequestCounter, nil)
    else
        local lenth=string.len(ZyWriter:generatePostData())
        ScutDataLogic.CDataRequest:Instance():AsyncExecTcpRequest(pScutScene,addressPath, 1, nil, ZyWriter:generatePostData(), lenth)
        ScutDataLogic.CNetWriter:resetData()
    end

    ScutDataLogic.CNetWriter:resetData()
end

