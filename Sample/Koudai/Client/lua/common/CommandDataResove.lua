--
-- CommandDataResove.lua.lua
-- 
--


module("CommandDataResove", package.seeall)



local progressLabel = nil
local unzipLabel = nil	

--资源包更新
function resourceUpdated(pScutScene, nTag)
   
end

function mbo_resourceUpdated()
    CCDirector:sharedDirector():endToLua();
end