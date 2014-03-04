------------------------------------------------------------------
--PushReceiverLayer.lua
-- Author     : JMChen
-- Version    : 1.0.0.0
-- Date       : 2011-10-15
-- Description:
------------------------------------------------------------------


module("PushReceiverLayer", package.seeall)


--服务器推送通知回调
function PushReceiverCallback(pScutScene, lpExternalData)

	local actionId = ScutDataLogic.CNetReader:getInstance():getActionID()
	local result = ScutDataLogic.CNetReader:getInstance():getResult()
	pScutScene= CCDirector:sharedDirector():getRunningScene()

	if actionId==2003 then
		local serverInfo=actionLayer._2003Callback(pScutScene, lpExternalData)
		if serverInfo and serverInfo.PlayerTable then
			MainDesk.initPlayerLayer(serverInfo.PlayerTable)
		end
	elseif actionId==2004 then
		local serverInfo=actionLayer._2004Callback(pScutScene, lpExternalData)
		if serverInfo then
		       MainDesk.releaseClock()
		      MainDesk.releaseCardLayer()
			MainDesk.sendCardLayer(serverInfo)
		end
	elseif actionId==2006 then
		local serverInfo=actionLayer._2006Callback(pScutScene, lpExternalData)
		if serverInfo then
			MainDesk.pushBackCallLandLord(serverInfo)
		end
		--
	elseif actionId==2012 then
		local serverInfo=actionLayer._2012Callback(pScutScene, lpExternalData)
		if serverInfo then
			 MainDesk.createRepor(serverInfo)	
		end
	elseif actionId==2008 then
		local serverInfo=actionLayer._2008Callback(pScutScene, lpExternalData)
		if serverInfo then
			 MainDesk.createMingBtn(serverInfo)	
		end
	elseif actionId==2013 then
		local serverInfo=actionLayer._2013Callback(pScutScene, lpExternalData)
		if serverInfo then
			MainDesk.gameOverCallBack(serverInfo)
		end
	elseif actionId==2014 then
		local serverInfo=actionLayer._2014Callback(pScutScene, lpExternalData)
		if serverInfo then
			 if serverInfo.Status ==1 then
				 MainDesk.releaseControlLayer()
				 MainDesk.createChocieTuoGuan(serverInfo)	
			 else
				 MainDesk.releaseTuoGuan() 
			 end
		end
	elseif actionId==9003 then
		local serverInfo=actionLayer._9003Callback(pScutScene, lpExternalData)
	elseif actionId==2010 then
		local serverInfo=actionLayer._2010Callback(pScutScene, lpExternalData)
		if serverInfo then
			MainDesk.createOutLayer(type,serverInfo.RecordTabel,serverInfo)
			MainDesk.controlChoice(serverInfo.NextUserId, serverInfo.IsReNew,serverInfo)
		end	
	elseif actionId==1014 then
		local serverInfo=actionLayer._1014Callback(pScutScene, lpExternalData)
  	 	if serverInfo~=nil then
  	 		PersonalInfo.getPersonalInfo()._HeadIcon=serverInfo.HeadIcon
  	 		PersonalInfo.getPersonalInfo()._GameCoin=serverInfo.GameCoin
  	 		PersonalInfo.getPersonalInfo()._Gold=serverInfo.Gold
  	 		PersonalInfo.getPersonalInfo()._WinNum=serverInfo.WinNum
  	 		PersonalInfo.getPersonalInfo()._FailNum=serverInfo.FailNum
  	 		PersonalInfo.getPersonalInfo()._TitleName=serverInfo.TitleName
  	 		PersonalInfo.getPersonalInfo()._ScoreNum=serverInfo.ScoreNum
  	 		PersonalInfo.getPersonalInfo()._VipLv=serverInfo.VipLv
  	 	end
  	 	MainScene.setjindou()
    elseif actionId==9202 then--公告接口
        local serverInfo=actionLayer._9202Callback(pScutScene, lpExternalData)
        if serverInfo then
      --  serverInfo={}
      --  serverInfo.RecordTabel={[1]={Content=Language.ddz}}
              if MainScene.getWherer() then--在主界面
                    BroadcastLayer.init(pScutScene,nil,serverInfo.RecordTabel)
              elseif MainDesk.getWherer() then--在打牌界面
                    BroadcastLayer.init(pScutScene,1,serverInfo.RecordTabel)
              end
         end
	elseif actionId==2015 then
		local serverInfo=actionLayer._2015Callback(pScutScene, lpExternalData)
  	 	if serverInfo~=nil then
  	 	    MainDesk.initScene()
			MainDesk.continuGame(serverInfo)
		end	 	
	end
	--]]
end