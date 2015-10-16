
Action1001 ={}

function Action1001:getUrlElement(httpGet)
	local urlParam = {}
	urlParam.Result = true
	--urlParam.PageIndex = ScutReaderReadString(httpGet, "PageIndex")
    return urlParam
end

function Action1001:takeAction(urlParam)
	local actionResult = {}
	actionResult.Result = true
	--actionResult.Table = GetUserRankingList()
    return actionResult
end


function Action1001:buildPacket(writer, urlParam, actionResult)
    return true
end