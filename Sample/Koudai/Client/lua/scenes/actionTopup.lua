------------------------------------------------------------------
-- 
-- Author     : 
-- Version    : 1.15
-- Date       :   
-- Description: 91 £¨uc ≥‰÷µ∑¥¿°
------------------------------------------------------------------

function action1065()
	   local mMobileType,GameType,RetailID=accountInfo.readMoble();
	   local mServerId,mServerPath,mServerName,mServerState=accountInfo.readServerId()
	   local scene = CCDirector:sharedDirector():getRunningScene()
          accountInfo.readAccount()
         	   local PassportID=accountInfo.getPassportID();
            	   local RetailId=accountInfo.getRetailId()
            	   local orderId=0
          local orderInfo=nil
          --91
	   if  RetailId=="0001" or  RetailId=="2006" then   
	        orderId=TopUpScene.getOrder()
	    ---UC
	    elseif RetailId=="2008" then
	    		 orderId=TopUpScene.getOrder()
	   elseif RetailId=="0036" then
	   	 	  local sdk=LanScenes.getLogin91Sdk()
	   	 	  orderId=sdk:getOrderdiD()
	   elseif RetailId=="2012" then
			orderInfo=TopUpScene.getTopUp()
			local sdk=TopUpScene.getPounBoxSdk()
			local   orderId=TopUpScene.getOrder()
			ZyWriter:writeString("ActionId",1081);
			ZyWriter:writeString("GameID",orderInfo.mGameType);	   		 
			ZyWriter:writeString("ServerID",orderInfo.mServerID);
			ZyWriter:writeString("ServerName",accountInfo.mServerName);
			ZyWriter:writeString("OrderNo",orderId);
			ZyWriter:writeString("PassportID",accountInfo.PassportID);
			ZyWriter:writeString("Currency","CNY");
			ZyWriter:writeString("RetailID",RetailId);
			ZyWriter:writeString("DeviceID",accountInfo.getMac());
			ZyWriter:writeString("PayType", "2012");
			ZyWriter:writeString("ProductID",  "10025500000001100255");
			ZyWriter:writeString("GameName",  Language.GAME_NAME);
			ZyWriter:writeString("Amount",orderInfo.Amount);
			ZyExecRequest(scene, nil,false);	   	       
	   elseif GameType==1003 then
	   		orderInfo=TopUpScene.getTopUp()
	   		local sdk=TopUpScene.getPounBoxSdk()
   	 		local    orderId=sdk:getSessionId()
	   		 ZyWriter:writeString("ActionId",1081);
		        ZyWriter:writeString("Orderno",orderId);
		        ZyWriter:writeString("GameID",orderInfo.mGameType);
		        ZyWriter:writeString("ServerID",orderInfo.mServerID);
		        ZyWriter:writeString("DeviceID",accountInfo.getMac());
		        ZyWriter:writeString("Amount",orderInfo.Amount);
		        ZyWriter:writeString("Gameconis",orderInfo.Gameconis);
		        ZyExecRequest(scene, nil,false);
	   end
	    
	   if orderId~=0 then
         	 actionLayer.Action1065(TopUpScene.getScene(),nil ,orderId,GameType,mServerId,mServerName,PassportID)
         	-- TopUpScene.clearOrder()
         end
         
end;

action1065()