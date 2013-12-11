ScutScene = {}
g_scenes = {}

function ScutScene:new(o)
    o = o or {}
    if o.root == nil then
        o.root = CCScene:create()
        print(o.root)
    end
    setmetatable(o, self)
    self.__index = self
    g_scenes[o.root] = o
    return o
end

function ScutScene:registerScriptHandler(func)
end

function ScutScene:registerCallback(func)
    func = func or function()end
    self.mCallbackFunc = func
end

function ScutScene:registerNetErrorFunc(func)
    func = func or function()end
    self.mNetErrorFunc = func
end

function ScutScene:registerNetCommonDataFunc(func)
    func = func or function()end
    self.mNetCommonDataFunc = func
end

function ScutScene:registerNetDecodeEnd()
    func = func or function()end
    self.NetDecodeEndFunc = func
end

function ScutScene:execCallback(nTag, nNetState, pData)
    if 2 == nNetState then
        local reader = ScutDataLogic.CDataRequest:Instance()
        local bValue = reader:LuaHandlePushDataWithInt(pData)
      --  if not bValue then return end
        if self.mCallbackFunc then
            self.mCallbackFunc(self.root)
        end

        if self.mNetCommonDataFunc then
            self.mNetCommonDataFunc()
        end

        netDecodeEnd(self.root, nTag)

        if self.mNetErrorFunc then
            self.mNetErrorFunc()
        end
    end
end