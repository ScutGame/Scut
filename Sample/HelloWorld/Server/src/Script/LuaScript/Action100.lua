Action100 ={}

function Action100:getUrlElement(httpGet, parent)
    local urlParam = {}
    urlParam.Result = true
    return urlParam
end

function Action100:takeAction(urlParam, parent)
	CPrint("use lua do action...")
	local actionResult = {}
	actionResult.Result = true
	actionResult._content = "Hello World for Lua!"
    return actionResult
end


function Action100:buildPacket(writer, urlParam, actionResult)
	PushIntoStack(writer, actionResult._content)
    return true
end

