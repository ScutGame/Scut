Action1001 ={}

function Action1001:getUrlElement(httpGet, parent)
	local urlParam = {}
	urlParam.Result = true
    return urlParam
end

function Action1001:takeAction(urlParam, parent)
	local actionResult = {}
	actionResult.Result = true
    return actionResult
end


function Action1001:buildPacket(writer, urlParam, actionResult)
    return true
end

