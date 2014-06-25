Action2001 ={}

function Action2001:getUrlElement(httpGet, parent)
	local urlParam = {}
	urlParam.Result = true
	urlParam.GameType = ReadNumberParam(httpGet, "GameType")
    urlParam.ServerID = ReadNumberParam(httpGet, "ServerID")
    urlParam.PageIndex = ReadNumberParam(httpGet, "PageIndex")
    urlParam.PageSize = ReadNumberParam(httpGet, "PageSize")
    return urlParam
end

function Action2001:takeAction(urlParam, parent)
	CPrint("lua print test...")
	local actionResult = {}
	actionResult.PageCount = 0
	actionResult.ItemCount = 0
	actionResult.dsItemCollect = nil
	actionResult.Result = true
	local result =  GetNoticeCache(urlParam.PageIndex, urlParam.PageSize)
	if result == nil then
		actionResult.Result = false
		return actionResult
	end
	actionResult.PageCount = result[0]
	actionResult.dsItemCollect = result[1]
	actionResult.ItemCount = actionResult.dsItemCollect.Count
    return actionResult
end


function Action2001:buildPacket(writer, urlParam, actionResult)
	PushIntoStack(writer, actionResult.PageCount)
    PushIntoStack(writer, actionResult.ItemCount)
	local len = actionResult.ItemCount
    for i=1,len,1 do
		local info = actionResult.dsItemCollect[i-1]
        dsItem = CreateDataStruct()
        PushIntoStack(dsItem, info.Title)
        PushIntoStack(dsItem, info.Content)
        PushIntoStack(dsItem, FormatDateString(info.CreateDate, "yyyy-MM-dd HH:mm:ss"))
        PushIntoStack(writer, dsItem)
	end
    return true
end

