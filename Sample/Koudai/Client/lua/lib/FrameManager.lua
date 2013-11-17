FrameManager = {}

function FrameManager:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
end

function FrameManager:init()
    CCDirector:sharedDirector():getScheduler():scheduleScriptFunc(function()
        self:update()
    end, 0, false)
end

function FrameManager:update()
    ScutDataLogic.CDataRequest:Instance():PeekLUAData()
end