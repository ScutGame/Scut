------------------------------------------------------------------
-- Author     : 
-- Version    : 1.15
-- Date       :   
-- Description: ,
------------------------------------------------------------------

--require("lib.ZyLoading")

ZyRequestParam = {param = {}}
function ZyRequestParam:getParamData(nTag)
	return ZyRequestParam.param[nTag]
end
ZyRequestCounter = 1
ZyReader = ScutDataLogic.CNetReader:getInstance()

--默认跟网络交互处理成功的标记
eScutNetSuccess = 0;
eScutNetError = 10000;



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


function ZyExecRequest(pScutScene, lpData, bShowLoading,type)

	if  bShowLoading == true then
		--LoadingScene.init(pScutScene)
	end
	--如果为True 显示Loading图标--
	ZyRequestCounter = ZyRequestCounter + 1
	if  bShowLoading==nil then
	    ZyLoading.show(pScutScene, ZyRequestCounter)
	end
	
	if lpData then
	    table.insert(ZyRequestParam.param, ZyRequestCounter, lpData)
	end
	if type then
		ScutDataLogic.CDataRequest:Instance():AsyncExecRequest(pScutScene, ZyWriter:generatePostData(), ZyRequestCounter, nil);
	else
		local lenth=string.len(ZyWriter:generatePostData())
		local addressPath="ddz.36you.net:9700"
		ScutDataLogic.CDataRequest:Instance():AsyncExecTcpRequest(pScutScene,addressPath, 1, nil, ZyWriter:generatePostData(), lenth);
	--	accountInfo.saveConfig("sys/read.ini", "URL", "URL" ,ZyWriter:generatePostData())
		ScutDataLogic.CNetWriter:resetData();		
	end

	ScutDataLogic.CNetWriter:resetData()
end

