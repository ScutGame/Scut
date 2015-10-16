ScutWriter ={}
function ScutWriter:new()
	local instance = {}
	setmetatable(instance, self)
	self.__index = self
end

function ScutWriter:writeString(name, value)
    return ScutWriter_writeString(name, value)
end

function ScutWriter:writeInt32(name, value)
    return ScutWriter_writeInt32(name, value)
end

function ScutWriter:writeWord(name, value)
    return ScutWriter_writeWord(name, value)
end

function ScutWriter:writeHead(sid, uid)
    return ScutWriter_writeHead(sid, uid)
end