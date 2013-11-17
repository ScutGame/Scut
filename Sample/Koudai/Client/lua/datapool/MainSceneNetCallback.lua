------------------------------------------------------------------
-- MainSceneNetCallback.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 主界面接口返回       中转
------------------------------------------------------------------

module("MainSceneNetCallback", package.seeall)


-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if actionId == 1401 or actionId == 3009 then
		MainMenuLayer.networkCallback(pScutScene, lpExternalData)
	elseif actionId ==9202  then 
		PublicAnnouncementScene.networkCallback(pScutScene, lpExternalData)
	elseif actionId ==9204  then
		ChatScene._9204Callback(pScutScene, lpExternalData)	
	end
	commonCallback.networkCallback(pScutScene, lpExternalData)
end