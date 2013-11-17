------------------------------------------------------------------
-- commonCallback.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 
------------------------------------------------------------------

module("commonCallback", package.seeall)



-- ÍøÂç»Øµ÷
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if actionId == 1008 or actionId == 1025  then
		MainMenuLayer.networkCallback(pScutScene, lpExternalData)
	elseif actionId == 1093 or actionId == 1094 then
		GuideLayer.networkCallback(pScutScene, lpExternalData)
	elseif actionId == 9205 then
		BroadcastLayer.networkCallback(pScutScene, lpExternalData)	
	end
end