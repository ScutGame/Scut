--ZyReader.recordBegin()：结构体中，开始解析二进制流
--ZyReader.recordEnd()：结构体闭合，结束解析二进制流
--ZyReader.getBYTE()：解析unsigned char（byte，1字节）方法。
--ZyReader.getWORD()：解析unsigned short（WORD，2字节）方法。
--ZyReader.getDWORD()：解析unsigned int（DWORD，4字节）方法。
--ZyReader.getFloat()：解析float方法。
--ZyReader.getDouble()：解析double方法。
--ZyReader.readString()：解析String方法。
--ZyReader.readInt64()：解析Int64方法。
--ZyReader.getInt()：解析Int32方法。
--ZyReader.readErrorMsg()：解析错误消息。该错误消息需要与服务端约定。当网络请求发生异常，或者业务逻辑出现错误时候，打印的消息。
--ZyReader.getResult()：获取当前网络请求状态，服务端返回错误码，该错误码需要与客户端约定。
--ZyReader.getActionID ()：获取当前请求的ActionID。

ZyRequestParam = {param = {}}
function ZyRequestParam:getParamData(nTag)
    return ZyRequestParam.param[nTag]
end
ZyRequestTag = 0
ZyReader = ScutDataLogic.CNetReader:getInstance()
ZyWriter = ScutDataLogic.CNetWriter:getInstance()

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

--参数pScutScene 为请求接口发起的scene对象，此对象为CCScene类型。
--参数lpData为用户附加数据，该数据将被直接传回。
function ZyExecRequest(pScutScene, lpData, addressPath)
    ZyRequestTag = ZyRequestTag + 1
    if lpData then
        table.insert(ZyRequestParam.param, ZyRequestTag, lpData)
    end
    if addressPath == nil  then
        ScutDataLogic.CDataRequest:Instance():AsyncExecRequest(pScutScene, ZyWriter:generatePostData(), ZyRequestTag, nil)
    else
        local lenth=string.len(ZyWriter:generatePostData())
        ScutDataLogic.CDataRequest:Instance():AsyncExecTcpRequest(pScutScene,addressPath, ZyRequestTag, nil, ZyWriter:generatePostData(), lenth)
        ScutDataLogic.CNetWriter:resetData()
    end
    ScutDataLogic.CNetWriter:resetData()
end

