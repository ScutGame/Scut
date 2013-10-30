SceneManager = {}

function SceneManager:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
end

function SceneManager:netDataDispatch(pScene, nTag, nNet, lpData, lpExternal)
    assert(pScene)
    --find scene and then exec callback
    pScene:execCallback(nTag, nNet, lpData, lpExternal)
end