ScutReader ={}
function ScutReader:new()
	local instance = {}
	setmetatable(instance, self)
	self.__index = self
	
end
function ScutReader:readAction()
    return ScutReader_readAction()
end

function ScutReader:getInt()
    return ScutReader_getInt()
end
function ScutReader:getWORD()
    return ScutReader_getWORD()
end
function ScutReader:getByte()
    return ScutReader_getByte()
end
function ScutReader:readString()
    return ScutReader_readString()
end

function ScutReader:recordBegin()
    return ScutReader_recordBegin()
end

function ScutReader:recordEnd()
    return ScutReader_recordEnd()
end

function ScutReader:readErrorMsg()
    return ScutReader_readErrorMsg()
end

function ScutReader:readErrorCode()
    return ScutReader_readErrorCode()
end


function ScutReader:getResult()
    return ScutReader_getResult()
end