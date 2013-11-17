

------------------------------------------------------------------
-- accountInfo.lua
-- Author     :
-- Version    : 1.15
-- Date       :
-- Description: 接口请求
------------------------------------------------------------------


------------------------------------------------------------------

local strModuleName = "actionLayer";
module(strModuleName, package.seeall);
CCLuaLog("Module " .. strModuleName .. " loaded.");
strModuleName = nil;


function Action360(Scene,isLoading ,RefeshToken,RetailID,Scope,content)
	ZyWriter:writeString("ActionId",360)
	ZyWriter:writeString("RefeshToken",RefeshToken  )
	ZyWriter:writeString("RetailID",RetailID)
	ZyWriter:writeString("Scope",Scope)
	ZyExecRequest(Scene, content,isLoading)
end



--        1000_GM命令接口（ID=1000）
function Action1000(Scene,isLoading ,Cmd)
ZyWriter:writeString("ActionId",1000)
   	ZyWriter:writeString("Cmd",Cmd)
    ZyExecRequest(Scene, nil,isLoading)
end

--        1001_服务器列表协议接口（ID=1001）
function Action1001(Scene,isLoading ,GameID)
	ZyWriter:writeString("ActionId",1001)
   	ZyWriter:writeString("GameID",GameID)
	ZyExecRequest(Scene, nil,isLoading)
end

function _1001Callback(pScutScene, lpExternalData)
   	local Datatable=nil
    	if ZyReader:getResult() == eScutNetSuccess then
        	Datatable={}
	 	local RecordNums_1=ZyReader:getInt();
        	local Severtable={}
        	if RecordNums_1~=0 then
           	for k=1, RecordNums_1 do
               	local mSevertable={}
               	ZyReader:recordBegin()
               	mSevertable.ID=ZyReader:getInt();
               	mSevertable.Name=ZyReader:readString();
               	mSevertable.Status=ZyReader:readString();
               	mSevertable.BaseUrl=ZyReader:readString();
               	ZyReader:recordEnd()
               	ZyTable.push_back(Severtable,mSevertable)
           	end
    	end
     	Datatable.Severtable=Severtable;
    end
    return Datatable
end

--        1002_注册通行证ID获取接口（ID=1002）
function Action1002(Scene,isLoading ,MobileType,GameType,RetailID,ClientAppVersion,ScreenX,ScreenY,DeviceID)
	ZyWriter:writeString("ActionId",1002)
   	ZyWriter:writeString("MobileType",MobileType)
        ZyWriter:writeString("GameType",GameType)
        ZyWriter:writeString("RetailID",RetailID)
        ZyWriter:writeString("ClientAppVersion",ClientAppVersion)
        ZyWriter:writeString("ScreenX",ScreenX)
        ZyWriter:writeString("ScreenY",ScreenY)
        ZyWriter:writeString("DeviceID",DeviceID)
   	ZyExecRequest(Scene, nil,isLoading)
end

function _1002Callback(pScutScene, lpExternalData)
   local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.PassportID= ZyReader:readString()
        DataTabel.Password= ZyReader:readString()
    end
    return DataTabel
end

--        1004_用户登录（ID=1004）
function Action1004(Scene,isLoading ,
MobileType,
Pid,
Pwd,
DeviceID,
GameType,
ScreenX,
ScreenY,
RetailID,
ClientAppVersion,
ServerID,
RetailUser,
Code,
token
)
	ZyWriter:writeString("ActionId",1004)
   	ZyWriter:writeString("MobileType",MobileType)
   	ZyWriter:writeString("Pid",Pid)
       ZyWriter:writeString("Pwd",Pwd)
       ZyWriter:writeString("DeviceID",DeviceID)
       ZyWriter:writeString("GameType",GameType)
       ZyWriter:writeString("ScreenX",ScreenX)
       ZyWriter:writeString("ScreenY",ScreenY)
       ZyWriter:writeString("RetailID",RetailID)
       ZyWriter:writeString("ClientAppVersion",ClientAppVersion)
       ZyWriter:writeString("ServerID",ServerID)
       ZyWriter:writeString("RetailUser",RetailUser)
       ZyWriter:writeString("Code",Code)
       ZyWriter:writeString("token",token)
	ZyExecRequest(Scene, nil,isLoading)
end

function _1004Callback(pScutScene, lpExternalData)
   	local DataTabel=nil
     	if ZyReader:getResult() == eScutNetSuccess or ZyReader:getResult() == 1005 then
        	DataTabel={}
        	DataTabel.SessionID= ZyReader:readString()
        	DataTabel.UserID= ZyReader:readString()
        	DataTabel.UserType= ZyReader:getInt()
        	DataTabel.LoginTime= ZyReader:readString()
        	DataTabel.GuideID= ZyReader:getInt()
        	DataTabel.PassportId = ZyReader:readString()        	
        	DataTabel.AccessToken = ZyReader:readString()
        	DataTabel.RefeshToken = ZyReader:readString()
        	DataTabel.QihooUserID = ZyReader:readString()
        	DataTabel.Scope = ZyReader:readString()
        	DataTabel.StatusCode= ZyReader:getResult()
      else
      		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.2)
    	end
    	return DataTabel
end

--        1005_创建角色（ID=1005）
function Action1005(Scene,isLoading ,UserName,Sex,HeadID,CareerID,RetailID,Pid,MobileType,ScreenX,ScreenY,ClientAppVersion,GameID,ServerID,GeneralID)
	ZyWriter:writeString("ActionId",1005)
   	ZyWriter:writeString("UserName",UserName)
        ZyWriter:writeString("Sex",Sex)
        ZyWriter:writeString("HeadID",HeadID)
        ZyWriter:writeString("CareerID",CareerID)
        ZyWriter:writeString("RetailID",RetailID)
        ZyWriter:writeString("Pid",Pid)
        ZyWriter:writeString("MobileType",MobileType)
        ZyWriter:writeString("ScreenX",ScreenX)
        ZyWriter:writeString("ScreenY",ScreenY)
        ZyWriter:writeString("ClientAppVersion",ClientAppVersion)
        ZyWriter:writeString("GameID",GameID)
        ZyWriter:writeString("ServerID",ServerID)
        ZyWriter:writeString("GeneralID",GeneralID)
   	ZyExecRequest(Scene, nil,isLoading)
end

function _1005Callback(pScutScene, lpExternalData)
   	local DataTabel=nil
   	local aa=ZyReader:getResult()
    if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
	else
		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)
    end
    return DataTabel
end

--        1006_密码更新接口（ID=1006）
function Action1006(Scene,isLoading ,PassWord)
	ZyWriter:writeString("ActionId",1006)
   	ZyWriter:writeString("PassWord",PassWord)
    ZyExecRequest(Scene, nil,isLoading)
end

function _1006Callback(pScutScene, lpExternalData)
   	local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
	else
			ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)
    end
    return DataTabel
end

--        1008_用户角色详情接口（ID=1008）
function Action1008(Scene,isLoading, info )--info=1 跨服战结束退出答题；
   	ZyWriter:writeString("ActionId",1008)
	ZyExecRequest(Scene, info,isLoading)
end

function _1008Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.CityID= ZyReader:getInt()
		DataTabel.PointX= ZyReader:getWORD()
		DataTabel.PointY= ZyReader:getWORD()
		DataTabel.GeneralID= ZyReader:getInt()
		DataTabel.GuildID= ZyReader:readString()
		DataTabel.NickName= ZyReader:readString()
		DataTabel.UserLv= ZyReader:getWORD()
		DataTabel.CareerID= ZyReader:getWORD()
		DataTabel.Sex= ZyReader:getInt()
		DataTabel.HeadID= ZyReader:readString()
		DataTabel.GoldNum= ZyReader:getInt()
		DataTabel.GameCoin= ZyReader:getInt()
		DataTabel.LifeNum= ZyReader:getInt()
		DataTabel.MaxLifeNum= ZyReader:getInt()
		DataTabel.EnergyNum= ZyReader:getWORD()
		DataTabel.MaxEnergyNum= ZyReader:getWORD()
		DataTabel.CurrExperience= ZyReader:getInt()
		DataTabel.MaxExperience= ZyReader:getInt()
		DataTabel.VipLv= ZyReader:getWORD()
		DataTabel.CountryID= ZyReader:getWORD()
		DataTabel.ItemLiveNum= ZyReader:getInt()
		DataTabel.ItemLiveMaxNum= ZyReader:getInt()
		local RecordNums_1=ZyReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
				local mRecordTabel_1={}
				ZyReader:recordBegin()
				mRecordTabel_1.BlessingType= ZyReader:getWORD()
				mRecordTabel_1.BlessingNum= ZyReader:getInt()
				mRecordTabel_1.PropDate= ZyReader:getInt()
				mRecordTabel_1.PropHead= ZyReader:readString()
				mRecordTabel_1.PropDesc= ZyReader:readString()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.BlessTable = RecordTabel_1;
		DataTabel.UserLocation= ZyReader:getWORD()
		DataTabel.UserExp= ZyReader:getInt()
		DataTabel.UserStatus= ZyReader:getWORD()
		DataTabel.PlotID= ZyReader:getInt()
		DataTabel.IsUseup= ZyReader:getWORD()
		DataTabel.PictureID= ZyReader:readString()
		DataTabel.PictureTime= ZyReader:getInt()
		DataTabel.DemandGold= ZyReader:getInt()
		DataTabel.SurplusEnergy= ZyReader:getWORD()
		DataTabel.IsHelper= ZyReader:getWORD()
		DataTabel.PlotStatusID= ZyReader:getInt()
		DataTabel.MercenarySeq= ZyReader:getInt()
		DataTabel.CardUserID= ZyReader:readString()
		DataTabel.IsChampion= ZyReader:getInt()
		
		DataTabel.BattleNum= ZyReader:getInt()
		DataTabel.TotalBattle= ZyReader:getInt()
		DataTabel.Rstore= ZyReader:getInt()
		DataTabel.TotalRstore= ZyReader:getInt()
		DataTabel.HonourNum= ZyReader:getInt()
		DataTabel.NextHonourNum= ZyReader:getInt()
		
		DataTabel.CombatNum= ZyReader:getInt()
		DataTabel.TalPriority= ZyReader:getInt()
		DataTabel.IsLv= ZyReader:getWORD()
		
		local RecordNums_2=ZyReader:getInt()
		local RecordTabel_2={}
		if RecordNums_2~=0 then
			for k=1,RecordNums_2 do
				local mRecordTabel_2={}
				ZyReader:recordBegin()
				mRecordTabel_2.FunEnum=ZyReader:getWORD()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_2,mRecordTabel_2)
			end
		end
		DataTabel.FuncTabel = RecordTabel_2;

		
		local RecordNums_3=ZyReader:getInt()
		local RecordTabel_3={}
		if RecordNums_3~=0 then
			for k=1,RecordNums_3 do
				local mRecordTabel_3={}
				ZyReader:recordBegin()
				mRecordTabel_3.FunEnum= ZyReader:getWORD()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_3,mRecordTabel_3)
			end
		end
		DataTabel.OpenFuncTabel = RecordTabel_3;
		
		DataTabel.unReadCount  = ZyReader:getInt()
		DataTabel.WizardNum = ZyReader:getInt()
		
	elseif ZyReader:getResult() == 10001 then--账号过时 退出游戏 修改协议时候要加上该段代码
		local box = ZyMessageBoxEx:new()
		box:doPrompt(pScutScene, nil, ZyReader:readErrorMsg(),Language.IDS_QUITGAME,MainScene.makeSureExitGame)
	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.2)
	end
	return DataTabel
end

--3.1.1.8	用户开启功能列表接口（ID=1009）
function Action1009(Scene,isLoading)
	ZyWriter:writeString("ActionId",1009)
  	ZyExecRequest(Scene, nil,isLoading)
end

function _1009Callback(pScutScene, lpExternalData)
	local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
     	 	DataTabel={}
    	local RecordNums_1  = ZyReader:getInt()
	   	if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
                local mRecordTabel_1={}
                ZyReader:recordBegin()
                mRecordTabel_1.FunEnum = ZyReader:getWORD()
                ZyReader:recordEnd()
                ZyTable.push_back(DataTabel,mRecordTabel_1)
            end
	    end
	else
        	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)
   	end
    return DataTabel
end


--3.1.1.9	购买体力接口（ID=1010）
function Action1010(Scene,isLoading,PayType,Ops)
	ZyWriter:writeString("ActionId",1010)
	ZyWriter:writeString("PayType",PayType)
	ZyWriter:writeString("Ops",Ops)
  	ZyExecRequest(Scene, nil,isLoading)
end

--        1011_挖金矿接口（ID=1011）
function Action1011(Scene,isLoading ,PayType,Ops)
   	ZyWriter:writeString("ActionId",1011)
   	ZyWriter:writeString("PayType",PayType)
        ZyWriter:writeString("Ops",Ops)
	ZyExecRequest(Scene, nil,isLoading)
end

--3.1.1.11	界面消息通知接口（ID=1012）
function Action1012(Scene,isLoading)
	ZyWriter:writeString("ActionId",1012)
  	ZyExecRequest(Scene, nil,isLoading)
end

function _1012Callback(pScutScene, lpExternalData)
	local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
     	 	DataTabel={}
    	local RecordNums_1  = ZyReader:getInt()
	   	if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
                local mRecordTabel_1={}
                ZyReader:recordBegin()
                mRecordTabel_1.MsgType = ZyReader:getWORD()
                ZyReader:recordEnd()
                ZyTable.push_back(DataTabel,mRecordTabel_1)
            end
	    end
	elseif ZyReader:getResult() == 10001 then--账号过时 退出游戏 修改协议时候要加上该段代码
        local box = ZyMessageBoxEx:new()
        box:doPrompt(pScutScene, nil, ZyReader:readErrorMsg(),Language.QUITGAME,MainMenuLayer.makeSureExitGame)
        MainScene.setIsLoginOut(true)
	else
        	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)
   	end
    return DataTabel
end

--3.1.1.15	随机取玩家名字接口（ID=1016）
function Action1016(Scene,isLoading)
	ZyWriter:writeString("ActionId",1016)
  	ZyExecRequest(Scene, nil,isLoading)
end

--3.1.1.16	玩家退出接口（ID=1017）
function Action1017(Scene,isLoading)
	ZyWriter:writeString("ActionId",1017)
  	ZyExecRequest(Scene, nil,isLoading)
end


--        1022_补尝领取接口（ID=1022）
function Action1022(Scene,isLoading )
	ZyWriter:writeString("ActionId",1022)
	ZyExecRequest(Scene, nil,isLoading)
end

function _1022Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
	--	DataTabel.Content= ZyReader:readString()
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)
		Action1008(pScutScene,nil)
		MainMenuLayer.releasePresenBtn()
	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)
	end
	return DataTabel
end

--1023_人物改名接口（ID=1023）
function Action1023(Scene, isLoading, NickName)
	ZyWriter:writeString("ActionId",1023)
	ZyWriter:writeString("NickName",NickName)
	ZyExecRequest(Scene, NickName, isLoading)
end

--        1024_新手卡激活接口（ID=1024）
function Action1024(Scene,isLoading ,CardID)
	ZyWriter:writeString("ActionId",1024)
	ZyWriter:writeString("CardID",CardID)
	ZyExecRequest(Scene, nil,isLoading)
end

function _1024Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
	else          
        	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)	
	end
	return DataTabel
end

--        玩家升级提示接口（ID=1025）
function Action1025(Scene,isLoading)
	ZyWriter:writeString("ActionId",1025)
	ZyExecRequest(Scene, nil,isLoading)
end

function _1025Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.GoldNum= ZyReader:getInt()
	else          
        	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)	
	end
	return DataTabel
end

--        1026_可创建佣兵列表接口（ID=1026）
function Action1026(Scene,isLoading )

   	ZyWriter:writeString("ActionId",1026)
   	
	ZyExecRequest(Scene, nil,isLoading)
end

function _1026Callback(pScutScene, lpExternalData) 
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.GeneralID= ZyReader:getInt()
                mRecordTabel_1.GeneralName= ZyReader:readString()
                mRecordTabel_1.HeadID= ZyReader:readString()
                mRecordTabel_1.AbilityName= ZyReader:readString()
                mRecordTabel_1.GeneralDesc= ZyReader:readString()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;

       
    else          
         	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)	
    end
    return DataTabel
end


function Action1065(Scene,isLoading,OrderID,GameID,Server,ServiceName,PassportID)
	ZyWriter:writeString("ActionId",1065)
	 	ZyWriter:writeString("OrderID",OrderID)
	 	ZyWriter:writeString("GameID",GameID)
	 	ZyWriter:writeString("Server",Server)
	 	ZyWriter:writeString("ServiceName",ServiceName)
	 	ZyWriter:writeString("PassportID",PassportID)
  	ZyExecRequest(Scene, nil,isLoading)
end

function Action1069(Scene,isLoading,MObileType,GameID)
	ZyWriter:writeString("ActionId",1069)
	ZyWriter:writeString("MObileType",MObileType)
	ZyWriter:writeString("GameID",GameID)
	ZyExecRequest(Scene, nil,isLoading)
end

function _1069Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		local RecordNums_1=ZyReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
				local mRecordTabel_1={}
				ZyReader:recordBegin()
				mRecordTabel_1.Dollar= ZyReader:readString()
				mRecordTabel_1.product_id= ZyReader:readString()
				mRecordTabel_1.SilverPiece= ZyReader:getInt()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel = RecordTabel_1;
	else          
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)	
	end
	return DataTabel
end


--        1091_购买精力提示接口（ID=1091）
function Action1091(Scene,isLoading )
	ZyWriter:writeString("ActionId",1091)
	ZyExecRequest(Scene, nil,isLoading)
end

function _1091Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.CurrDayUseNum= ZyReader:getInt()
		DataTabel.CurrDayUseMaxNum= ZyReader:getInt()
		DataTabel.UseGold= ZyReader:getInt()
		DataTabel.RecoverEnergy= ZyReader:getInt()
	else          
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)	
	end
	return DataTabel
end


--        1092_新手引导进度（ID=1092）
function Action1092(Scene,isLoading )
	ZyWriter:writeString("ActionId",1092)
	ZyExecRequest(Scene, nil,isLoading)
end

function _1092Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.isPass= ZyReader:getInt()
		DataTabel.guideId= ZyReader:getInt()
	else          
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)	
	end
	return DataTabel
end


--        1093_新手引导完成任务（ID=1093）
function Action1093(Scene,isLoading ,GuideId,Ops)
	ZyWriter:writeString("ActionId",1093)
	ZyWriter:writeString("GuideId",GuideId)
	ZyWriter:writeString("Ops",Ops)
	ZyExecRequest(Scene, nil,isLoading)
end

function _1093Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
	else          
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)	
	end
	return DataTabel
end

--        1094_新手引导领取奖励（ID=1094）
function Action1094(Scene,isLoading ,GuideId)
   	ZyWriter:writeString("ActionId",1094)
   	ZyWriter:writeString("GuideId",GuideId)
	ZyExecRequest(Scene, nil,isLoading)
end

function _1094Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.isPass= ZyReader:getInt()
		DataTabel.GuideId= ZyReader:getInt()
		local RecordNums_1=ZyReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
				local mRecordTabel_1={}
				ZyReader:recordBegin()
				mRecordTabel_1.ItemName= ZyReader:readString()
				mRecordTabel_1.HeadID= ZyReader:readString()
				mRecordTabel_1.itemNum= ZyReader:getInt()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.RecordTabel = RecordTabel_1;
	else          
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)	
	end
	return DataTabel
end



--        1101_背包列表接口（ID=1101）
function Action1101(Scene,isLoading ,PageIndex,PageSize,PackType)
	ZyWriter:writeString("ActionId",1101)
   	ZyWriter:writeString("PageIndex",PageIndex)
        ZyWriter:writeString("PageSize",PageSize)
        ZyWriter:writeString("PackType",PackType)
  	ZyExecRequest(Scene, nil,isLoading)
end

function _1101Callback(pScutScene, lpExternalData)
	local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
      	DataTabel={}
 		DataTabel.PageCount = ZyReader:getInt()
  		DataTabel.GridNum   = ZyReader:getInt()
      	DataTabel.OccupyNum = ZyReader:getInt()
      	DataTabel.GameCoin= ZyReader:getInt()
        DataTabel.GoldNum= ZyReader:getInt()
    	local RecordNums_1  = ZyReader:getInt()
  		local RecordTabel_1={}
	   	if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
                local mRecordTabel_1={}
                ZyReader:recordBegin()
                mRecordTabel_1.UserItemID = ZyReader:readString()
                mRecordTabel_1.ItemID     = ZyReader:getInt()
                mRecordTabel_1.ItemType   = ZyReader:getWORD()
                mRecordTabel_1.Num        = ZyReader:getInt()
                mRecordTabel_1.HeadID     = ZyReader:readString()
                mRecordTabel_1.ProType = ZyReader:getWORD()
                mRecordTabel_1.ItemName = ZyReader:readString()
                mRecordTabel_1.ItemDesc = ZyReader:readString()
                mRecordTabel_1.SalePrice = ZyReader:getInt()
                mRecordTabel_1.IsUse = ZyReader:getInt()
                mRecordTabel_1.IsCostly = ZyReader:getInt()
                mRecordTabel_1.MaxHeadID= ZyReader:readString()
                mRecordTabel_1.IsKey = ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
            end
	    end
   		DataTabel.RecordTabel = RecordTabel_1;
	else
        ZyToast.show(pScutScene,ZyReader:readErrorMsg(),2,0.35)
   	end
    return DataTabel
end

--        1102_背包物品详情接口（ID=1102）
function Action1102(Scene,isLoading ,UserItemID,ToUserID,IsShow)
	ZyWriter:writeString("ActionId",1102)
	ZyWriter:writeString("UserItemID",UserItemID)
	ZyWriter:writeString("ToUserID",ToUserID)
	ZyWriter:writeString("IsShow",IsShow)	
	ZyExecRequest(Scene, nil,isLoading)
end

function _1102Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.HeadID      = ZyReader:readString()
        DataTabel.ItemName    = ZyReader:readString()
        DataTabel.ItemType    = ZyReader:getWORD()
        DataTabel.QualityType = ZyReader:getWORD()
        DataTabel.StengType   = ZyReader:getWORD()
        DataTabel.CurLevel    = ZyReader:getWORD()
        DataTabel.PreLevel    = ZyReader:getWORD()
        local RecordNums_1    = ZyReader:getInt()
        local RecordTabel_1={}
        if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
                local mRecordTabel_1={}
                ZyReader:recordBegin()
                mRecordTabel_1.AbilityType= ZyReader:getWORD()
                mRecordTabel_1.BaseNum= ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
        DataTabel.Ability = RecordTabel_1;
        local RecordNums_12   = ZyReader:getInt()
        local RecordTabel_12={}
        if RecordNums_12~=0 then
            for k=1,RecordNums_12 do
                local mRecordTabel_12={}
                ZyReader:recordBegin()
                mRecordTabel_12.CareerID   = ZyReader:getWORD()
                mRecordTabel_12.CareerName = ZyReader:readString()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_12,mRecordTabel_12)
              end
        end
        DataTabel.Career = RecordTabel_12;
        DataTabel.Price     = ZyReader:getInt()
        DataTabel.ItemDesc  = ZyReader:readString()
        local RecordNums_13 = ZyReader:getInt()
        local RecordTabel_13={}
        if RecordNums_13~=0 then
            for k=1,RecordNums_13 do
                local mRecordTabel_13={}
                ZyReader:recordBegin()
                mRecordTabel_13.MaterialsID = ZyReader:getInt()
                mRecordTabel_13.Name        = ZyReader:readString()
                mRecordTabel_13.Num         = ZyReader:getWORD()
                mRecordTabel_13.PackNum     = ZyReader:getWORD()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_13,mRecordTabel_13)
              end
        end
        DataTabel.Materials = RecordTabel_13;
    else
    		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)
    end
    return DataTabel
end

--      3.1.2.9	开启背包格子接口
function Action1110(Scene,isLoading ,BackpackType,Ops)
	ZyWriter:writeString("ActionId",1110)
	ZyWriter:writeString("BackpackType",BackpackType)
	ZyWriter:writeString("Ops",Ops)
	ZyExecRequest(Scene, nil,isLoading)
end

function Action1113(Scene,isLoading ,UserItemID,Ops)
	ZyWriter:writeString("ActionId",1113)
	ZyWriter:writeString("UserItemID",UserItemID)
	ZyWriter:writeString("Ops",Ops)
	ZyExecRequest(Scene, nil,isLoading)
end


function Action1114(Scene,isLoading ,UserItemID)
	ZyWriter:writeString("ActionId",1114)
	ZyWriter:writeString("UserItemID",UserItemID)
	ZyExecRequest(Scene, nil,isLoading)
end

function _1114Callback(pScutScene, lpExternalData)
   local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        local RecordNums_1=ZyReader:getInt()
        local RecordTabel_1={}
        if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
                local mRecordTabel_1={}
			ZyReader:recordBegin()
			mRecordTabel_1.Type= ZyReader:getInt()
			mRecordTabel_1.Num= ZyReader:getInt()
			mRecordTabel_1.ItemID= ZyReader:getInt()
			mRecordTabel_1.HeadID = ZyReader:readString()
			mRecordTabel_1.ItemName= ZyReader:readString()
			ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
        DataTabel.goods = RecordTabel_1;
      		
    else
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),2,0.35)
    end
    return DataTabel
end


--        1202_装备详情接口（ID=1202）
function Action1202(Scene,isLoading ,UserItemID,ToUserID, data)
	ZyWriter:writeString("ActionId",1202)
	ZyWriter:writeString("UserItemID",UserItemID)
	ZyWriter:writeString("ToUserID",ToUserID)
	ZyExecRequest(Scene, data, isLoading)
end

function _1202Callback(pScutScene, lpExternalData)
   local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.ItemName= ZyReader:readString()
        DataTabel.HeadID= ZyReader:readString()
        DataTabel.QualityType= ZyReader:getWORD()
        DataTabel.CurLevel = ZyReader:getWORD()
        DataTabel.UserLv= ZyReader:getWORD()
        DataTabel.StrongMoney= ZyReader:getInt()
        DataTabel.ColdTime= ZyReader:getInt()
        DataTabel.IsStrong= ZyReader:getWORD()
        local RecordNums_1=ZyReader:getInt()
        local RecordTabel_1={}
        if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
                local mRecordTabel_1={}
                ZyReader:recordBegin()
                mRecordTabel_1.CareerID= ZyReader:getInt()
                mRecordTabel_1.CareerName= ZyReader:readString()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
        DataTabel.Career = RecordTabel_1;
        --当前属性
        local RecordNums_12=ZyReader:getInt()
        local RecordTabel_12={}
        if RecordNums_12~=0 then
        for k=1,RecordNums_12 do
            local mRecordTabel_12={}
            ZyReader:recordBegin()
            mRecordTabel_12.AbilityType= ZyReader:getInt()
            mRecordTabel_12.BaseNum= ZyReader:getInt()
            ZyReader:recordEnd()
            ZyTable.push_back(RecordTabel_12,mRecordTabel_12)
        end
    end
        DataTabel.AbilityList = RecordTabel_12;
        --下一级属性
        local RecordNums_13=ZyReader:getInt()
        local RecordTabel_13={}
        if RecordNums_13~=0 then
        for k=1,RecordNums_13 do
            local mRecordTabel_13={}
            ZyReader:recordBegin()
            mRecordTabel_13.AbilityType= ZyReader:getInt()
            mRecordTabel_13.BaseNum= ZyReader:getInt()
            ZyReader:recordEnd()
            ZyTable.push_back(RecordTabel_13,mRecordTabel_13)
        end
    end
        DataTabel.AbilityList1 = RecordTabel_13;        
        DataTabel.ItemDesc =  ZyReader:readString()
	 DataTabel.Sellprice= ZyReader:getInt()
	 DataTabel.UserItemID= ZyReader:readString()
        DataTabel.MaxHeadID= ZyReader:readString()
	DataTabel.IsMaxLv= ZyReader:getInt()
	DataTabel.TenTimesStrongMoney= ZyReader:getInt()
    else
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),2,0.35)
    end
    return DataTabel
end


--        1203_佣兵装备更换接口（ID=1203）
function Action1203(Scene,isLoading ,GeneralID,UserItemID,Ops)
ZyWriter:writeString("ActionId",1203)
   	ZyWriter:writeString("GeneralID",GeneralID)
    ZyWriter:writeString("UserItemID",UserItemID)
    ZyWriter:writeString("Ops",Ops)
   ZyExecRequest(Scene, Ops,isLoading)
end

function _1203Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
    else
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)
    end
    return DataTabel
end

--        1204_装备强化接口（ID=1204）
function Action1204(Scene,isLoading ,UserItemID,Ops)
   	ZyWriter:writeString("ActionId",1204)
   	ZyWriter:writeString("UserItemID",UserItemID)
	ZyWriter:writeString("Ops",Ops)
	ZyExecRequest(Scene, nil,isLoading)
end

function _1204Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.ItemID      = ZyReader:getInt()
        DataTabel.ItemName    = ZyReader:readString()
        DataTabel.HeadID      = ZyReader:readString()
        DataTabel.CurLevel    = ZyReader:getWORD()
        DataTabel.QualityType = ZyReader:getWORD()
        DataTabel.StrongMoney = ZyReader:getInt()
        DataTabel.ColdTime    = ZyReader:getInt()
        DataTabel.IsStrong    = ZyReader:getWORD()
        local RecordNums_1=ZyReader:getInt()
        local RecordTabel_1={}
        if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
                local mRecordTabel_1={}
                ZyReader:recordBegin()
                mRecordTabel_1.AbilityType = ZyReader:getInt()
                mRecordTabel_1.BaseNum     = ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
        DataTabel.AbilityList = RecordTabel_1;

        local RecordNums_1=ZyReader:getInt()
        local RecordTabel_1={}
        if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
                local mRecordTabel_1={}
                ZyReader:recordBegin()
                mRecordTabel_1.StrengAbility= ZyReader:getInt()
                mRecordTabel_1.StengBaseNum= ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
        DataTabel.StrengAbilityList = RecordTabel_1;

        DataTabel.UserItemID= ZyReader:readString()
        DataTabel.StrongLv= ZyReader:getWORD()
    else
          ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)
    end
    return DataTabel
end


--        1205_装备列表接口（ID=1205）
function Action1205(Scene,isLoading ,EquParts, data)
   	ZyWriter:writeString("ActionId",1205)
   	ZyWriter:writeString("EquParts",EquParts)
	ZyExecRequest(Scene,data,isLoading)
end

function _1205Callback(pScutScene, lpExternalData)

    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
    	DataTabel={}
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
		mRecordTabel_1.UserItemID= ZyReader:readString()
		mRecordTabel_1.ItemID= ZyReader:getInt()
		mRecordTabel_1.ItemName= ZyReader:readString()
		mRecordTabel_1.HeadID= ZyReader:readString()
		mRecordTabel_1.CurLevel= ZyReader:getWORD()
		mRecordTabel_1.QualityType= ZyReader:getWORD()
		mRecordTabel_1.StrongMoney= ZyReader:getInt()
		mRecordTabel_1.IsStrong= ZyReader:getWORD()
		mRecordTabel_1.Sellprice= ZyReader:getInt()
		mRecordTabel_1.GeneralName= ZyReader:readString()
       	mRecordTabel_1.ItemDesc =  ZyReader:readString()	
		local RecordNums_2=ZyReader:getInt()
		local RecordTabel_2={}
          	if RecordNums_2~=0 then
			for k=1,RecordNums_2 do
			local mRecordTabel_2={}
			ZyReader:recordBegin()
			mRecordTabel_2.AbilityType= ZyReader:getInt()
			mRecordTabel_2.BaseNum= ZyReader:getInt()
			ZyReader:recordEnd()
			ZyTable.push_back(RecordTabel_2,mRecordTabel_2)
              	end
		end
		mRecordTabel_1.AbilityList = RecordTabel_2;	
		ZyReader:recordEnd()
		ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end

              
        end
		DataTabel.RecordTabel = RecordTabel_1;
    else
          ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)
    end
    return DataTabel
end


--        1208_未穿戴装备列表接口（ID=1208）
function Action1208(Scene,isLoading )
ZyWriter:writeString("ActionId",1208)
   	ZyExecRequest(Scene, nil,isLoading)
end

function _1208Callback(pScutScene, lpExternalData)
	local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
 		local RecordNums_2=ZyReader:getInt()
       	local RecordTabel_2={}
  		if RecordNums_2~=0 then
 			for m=1,RecordNums_2 do
          		local mRecordTabel_2={}
        		ZyReader:recordBegin()
     			mRecordTabel_2.UserItemID= ZyReader:readString()
			    mRecordTabel_2.ItemID= ZyReader:getInt()
			    mRecordTabel_2.ItemName= ZyReader:readString()
		     	mRecordTabel_2.HeadID= ZyReader:readString()
		     	mRecordTabel_2.CurLevel= ZyReader:getWORD()
		      	mRecordTabel_2.QualityType= ZyReader:getWORD()
		      	mRecordTabel_2.StrongMoney= ZyReader:getInt()
		     	mRecordTabel_2.ColdTime= ZyReader:getInt()
		     	mRecordTabel_2.IsStrong= ZyReader:getWORD()
		     	mRecordTabel_2.Sellprice= ZyReader:getInt()
		     	--第三层
		    	local RecordNums_3=ZyReader:getInt()
		       	local RecordTabel_3_1={}
		  		if RecordNums_3~=nil and RecordNums_3>0 then
			     	for i=1,RecordNums_3 do
			         	local mRecordTabel_3_1={}
					    ZyReader:recordBegin()
                        			mRecordTabel_3_1.AbilityType= ZyReader:getInt()
                        			mRecordTabel_3_1.BaseNum= ZyReader:getInt()
					    ZyReader:recordEnd()
		            	ZyTable.push_back(RecordTabel_3_1,mRecordTabel_3_1)
			     	end;
					mRecordTabel_2.AbilityList=RecordTabel_3_1
				end
		    	RecordNums_3=ZyReader:getInt()
		     	local RecordTabel_3_2={}
		    	if RecordNums_3~=nil and RecordNums_3>0 then
		    		for i=1,RecordNums_3 do
		        		local mRecordTabel_3_2={}
			        	ZyReader:recordBegin()
			     		mRecordTabel_3_2.StrengAbility= ZyReader:getInt()
				 		mRecordTabel_3_2.StengBaseNum= ZyReader:getInt()
			       		ZyReader:recordEnd()
			       		ZyTable.push_back(RecordTabel_3_2,mRecordTabel_3_2)
		          	end
		      		mRecordTabel_2.StrengAbilityList=RecordTabel_3_2
		    	end
		  		ZyReader:recordEnd()
          		ZyTable.push_back(RecordTabel_2,mRecordTabel_2)
        	end
  			DataTabel.GeneralEquiptList=RecordTabel_2
  		end
	else
 		 ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)
    end
    return DataTabel
end

--        1209_未穿戴部位装备列表（ID=1209）
function Action1209(Scene,isLoading ,EquParts,GeneralID)

   	ZyWriter:writeString("ActionId",1209)
   	ZyWriter:writeString("EquParts",EquParts)
    ZyWriter:writeString("GeneralID",GeneralID)

	ZyExecRequest(Scene, nil,isLoading)
end

function _1209Callback(pScutScene, lpExternalData)

	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		local RecordNums_1=ZyReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
				local mRecordTabel_1={}
				ZyReader:recordBegin()
				mRecordTabel_1.UserItemID= ZyReader:readString()
				mRecordTabel_1.ItemName= ZyReader:readString()
				mRecordTabel_1.HeadID= ZyReader:readString()
				mRecordTabel_1.CurLevel= ZyReader:getWORD()
				mRecordTabel_1.QualityType= ZyReader:getWORD()
				mRecordTabel_1.GeneralName= ZyReader:readString()
             
                
				local RecordNums_1_1=ZyReader:getInt()
				local RecordTabel_1_1={}
				if RecordNums_1_1~=0 then
					for k=1,RecordNums_1_1 do
						local mRecordTabel_1_1={}
						ZyReader:recordBegin()
						mRecordTabel_1_1.AbilityType= ZyReader:getInt()
						mRecordTabel_1_1.BaseNum= ZyReader:getInt()
						ZyReader:recordEnd()
						ZyTable.push_back(RecordTabel_1_1,mRecordTabel_1_1)
					end
				end
				mRecordTabel_1.AbilityList = RecordTabel_1_1;


				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.RecordTabel = RecordTabel_1;
				
				
	else
		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)
	end
	return DataTabel
end


--3.1.3.14	佣兵培养接口（ID=1217）
function Action1217(Scene,isLoading ,Ops,MultipleType,GeneralID,BringUpType)
	ZyWriter:writeString("ActionId",1217)
	ZyWriter:writeString("Ops",Ops)
	ZyWriter:writeString("MultipleType",MultipleType)
	ZyWriter:writeString("GeneralID",GeneralID)
	ZyWriter:writeString("BringUpType",BringUpType)
	ZyExecRequest(Scene, Ops,isLoading)
end


--        1301_命运水晶列表接口（ID=1301）
function Action1301(Scene,isLoading, ToUserID)
	ZyWriter:writeString("ActionId",1301)
	ZyWriter:writeString("ToUserID",ToUserID)	
	ZyExecRequest(Scene, nil,isLoading)
end

function _1301Callback(pScutScene, lpExternalData)
   local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.CrystalPackNum= ZyReader:getInt()
        local RecordNums_1=ZyReader:getInt()
        local RecordTabel_1={}
        if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
                local mRecordTabel_1={}
                ZyReader:recordBegin()
                mRecordTabel_1.GeneralID   = ZyReader:getInt()
                mRecordTabel_1.GeneralName = ZyReader:readString()
                mRecordTabel_1.GeneralLv   = ZyReader:getWORD()
                mRecordTabel_1.HeadID      = ZyReader:readString()
                mRecordTabel_1.OpenNum     = ZyReader:getWORD()
		mRecordTabel_1.GeneralQuality= ZyReader:getWORD()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
        DataTabel.RecordTabel = RecordTabel_1;
        DataTabel.PicyturesID= ZyReader:readString()
        local RecordNums_12=ZyReader:getInt()
        local RecordTabel_12={}
        if RecordNums_12~=0 then
            for k=1,RecordNums_12 do
                local mRecordTabel_12={}
                ZyReader:recordBegin()
                mRecordTabel_12.UserCrystalID = ZyReader:readString()
                mRecordTabel_12.CrystalID     = ZyReader:getInt()
                mRecordTabel_12.CrystalHeadID = ZyReader:readString()
                mRecordTabel_12.Position      = ZyReader:getWORD()
                mRecordTabel_12.CrystalName      = ZyReader:readString()
                mRecordTabel_12.CrystalLv      = ZyReader:getWORD()
                mRecordTabel_12.CrystalQuality      = ZyReader:getWORD()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_12,mRecordTabel_12)
              end
        end
        DataTabel.RecordTabel2 = RecordTabel_12;
        local RecordNums_13=ZyReader:getInt()
        local RecordTabel_13={}
        if RecordNums_13~=0 then
            for k=1,RecordNums_13 do
                local mRecordTabel_13={}
                ZyReader:recordBegin()
                mRecordTabel_13.UserCrystalID = ZyReader:readString()
                mRecordTabel_13.CrystalID     = ZyReader:getInt()
                mRecordTabel_13.CrystalHeadID = ZyReader:readString()
                mRecordTabel_13.CrystalName = ZyReader:readString()
                mRecordTabel_13.CrystalLv     = ZyReader:getWORD()
                mRecordTabel_13.CrystalQuality= ZyReader:getWORD()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_13,mRecordTabel_13)
              end
        end
        DataTabel.RecordTabel3 = RecordTabel_13;
        DataTabel.GeneralQuality= ZyReader:getWORD()
    else
  ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)
    end
    return DataTabel
end

--        1302_佣兵命运水晶列表接口（ID=1302）
function Action1302(Scene,isLoading ,GeneralID,ToUserID)
   	ZyWriter:writeString("ActionId",1302)
   	ZyWriter:writeString("GeneralID",GeneralID)
   	ZyWriter:writeString("ToUserID",ToUserID)	
	ZyExecRequest(Scene, nil,isLoading)
end

function _1302Callback(pScutScene, lpExternalData)
   local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.PicyturesID= ZyReader:readString()
        DataTabel.NumOpen    = ZyReader:getWORD()
        local RecordNums_1=ZyReader:getInt()
        local RecordTabel_1={}
        if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
                local mRecordTabel_1={}
                ZyReader:recordBegin()
                mRecordTabel_1.UserCrystalID = ZyReader:readString()
                mRecordTabel_1.CrystalID     = ZyReader:getInt()
                mRecordTabel_1.CrystalHeadID        = ZyReader:readString()
                mRecordTabel_1.Position      = ZyReader:getWORD()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
        DataTabel.RecordTabel = RecordTabel_1;
        DataTabel.MaxHeadID= ZyReader:readString()
 	DataTabel.GeneralQuality= ZyReader:getWORD()
    else
          ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)
    end
    return DataTabel
end

--        1303_猎取命运水晶列表接口（ID=1303）
function Action1303(Scene,isLoading )
ZyWriter:writeString("ActionId",1303)
   	ZyExecRequest(Scene, nil,isLoading)
end

function _1303Callback(pScutScene, lpExternalData)
   local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.GoldNum= ZyReader:getInt()
        DataTabel.GameCoin= ZyReader:getInt()
        DataTabel.FreeNum= ZyReader:getInt()
        DataTabel.IsSale= ZyReader:getInt()
        DataTabel.IsTelegrams= ZyReader:getInt()
        local RecordNums_1=ZyReader:getInt()
        local RecordTabel_1={}
        if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
                local mRecordTabel_1={}
                ZyReader:recordBegin()
                mRecordTabel_1.UserCrystalID = ZyReader:readString()
                mRecordTabel_1.CrystalID    = ZyReader:getInt()
                mRecordTabel_1.CrystalName  = ZyReader:readString()
                mRecordTabel_1.HeadID       = ZyReader:readString()
                mRecordTabel_1.CrystalQuality       = ZyReader:getWORD()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
        DataTabel.RecordTabel = RecordTabel_1;

        local RecordNums_12=ZyReader:getInt()
        local RecordTabel_12={}
        if RecordNums_12~=0 then
            for k=1,RecordNums_12 do
                local mRecordTabel_12={}
                ZyReader:recordBegin()
                mRecordTabel_12.HuntingID = ZyReader:getInt()
                mRecordTabel_12.Price     = ZyReader:getInt()
                mRecordTabel_12.IsLight   = ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_12,mRecordTabel_12)
              end
        end
        DataTabel.RecordTabel2 = RecordTabel_12;
    else
        ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)
    end
    return DataTabel
end

--        1304_命运水晶详情接口（ID=1304）
function Action1304(Scene,isLoading ,UserCrystalID,ToUserID)
	ZyWriter:writeString("ActionId",1304)
	ZyWriter:writeString("UserCrystalID",UserCrystalID)
	ZyWriter:writeString("ToUserID",ToUserID)	
	ZyExecRequest(Scene, nil,isLoading)
end

function _1304Callback(pScutScene, lpExternalData)
   local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.CrystalName       = ZyReader:readString()
        DataTabel.HeadID            = ZyReader:readString()
        DataTabel.CrystalLv         = ZyReader:getWORD()
        DataTabel.CrystalQuality    = ZyReader:getWORD()
        DataTabel.CrystalExperience = ZyReader:getInt()
        DataTabel.MaxExperience     = ZyReader:getInt()
        DataTabel.AbilityType       = ZyReader:getWORD()
        DataTabel.AttrNum           = ZyReader:readString()
        DataTabel.SalePrice         = ZyReader:getInt()
    else
          ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)
    end
    return DataTabel
end

--        1305_获取命运水晶接口（ID=1305）
function Action1305(Scene,isLoading ,HuntingID,Ops)
ZyWriter:writeString("ActionId",1305)
   	ZyWriter:writeString("HuntingID",HuntingID)
    ZyWriter:writeString("Ops",Ops)
   ZyExecRequest(Scene, nil,isLoading)
end

function _1305Callback(pScutScene, lpExternalData)
   local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.CrystalID= ZyReader:getInt()
        DataTabel.HeadID= ZyReader:readString()
        local RecordNums_1=ZyReader:getInt()
        local RecordTabel_1={}
        if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
                local mRecordTabel_1={}
                ZyReader:recordBegin()
                mRecordTabel_1.HuntingID= ZyReader:getInt()
                mRecordTabel_1.HuntingName= ZyReader:readString()
                mRecordTabel_1.IsLight= ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
        DataTabel.RecordTabel = RecordTabel_1;
    else
          ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)
    end
    return DataTabel
end

--        1306_卖出命运水晶接口（ID=1306）
function Action1306(Scene,isLoading ,UserCrystalID,Ops)
ZyWriter:writeString("ActionId",1306)
   	ZyWriter:writeString("UserCrystalID",UserCrystalID)
    ZyWriter:writeString("Ops",Ops)
	ZyExecRequest(Scene, Ops,isLoading)
end

--        1307_拾取命运水晶接口（ID=1307）
function Action1307(Scene,isLoading ,UserCrystalID,Ops)
ZyWriter:writeString("ActionId",1307)
   	ZyWriter:writeString("UserCrystalID",UserCrystalID)
    ZyWriter:writeString("Ops",Ops)
    ZyExecRequest(Scene, nil,isLoading)
end

--        1308_合成命运水晶接口（ID=1308）
function Action1308(Scene,isLoading ,UserCrystalID1,UserCrystalID2,Ops)
ZyWriter:writeString("ActionId",1308)
   	ZyWriter:writeString("UserCrystalID1",UserCrystalID1)
    ZyWriter:writeString("UserCrystalID2",UserCrystalID2)
    ZyWriter:writeString("Ops",Ops)
    ZyExecRequest(Scene, nil,isLoading)
end

--        1309_佣兵替换命运水晶接口（ID=1309）
function Action1309(Scene,isLoading ,GeneralID,UserCrystalID,Potion,Ops)
ZyWriter:writeString("ActionId",1309)
   	ZyWriter:writeString("GeneralID",GeneralID)
    ZyWriter:writeString("UserCrystalID",UserCrystalID)
    ZyWriter:writeString("Potion",Potion)
    ZyWriter:writeString("Ops",Ops)
    ZyExecRequest(Scene, nil,isLoading)
end

--        1310_命运背包格子开启接口（ID=1310）
function Action1310(Scene,isLoading ,Ops,LatticeNum)
ZyWriter:writeString("ActionId",1310)
   	ZyWriter:writeString("Ops",Ops)
    ZyWriter:writeString("LatticeNum",LatticeNum)
    ZyExecRequest(Scene, nil,isLoading)
end

function _1310Callback(pScutScene, lpExternalData)
   local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
    else
          ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)
    end
    return DataTabel
end

--        1311_命运背包水晶列表接口（ID=1311）
function Action1311(Scene,isLoading ,type)
ZyWriter:writeString("ActionId",1311)
   	ZyExecRequest(Scene, type,isLoading)
end

function _1311Callback(pScutScene, lpExternalData)
   local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.CrystalPackNum= ZyReader:getInt()
        local RecordNums_1=ZyReader:getInt()
        local RecordTabel_1={}
        if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
                local mRecordTabel_1={}
                ZyReader:recordBegin()
                mRecordTabel_1.UserCrystalID= ZyReader:readString()
                mRecordTabel_1.CrystalID= ZyReader:getInt()
                mRecordTabel_1.CrystalHeadID= ZyReader:readString()
                mRecordTabel_1.CrystalName= ZyReader:readString()
                mRecordTabel_1.CrystalLv= ZyReader:getWORD()
                mRecordTabel_1.CrystalQuality= ZyReader:getWORD()
                mRecordTabel_1.AbilityType = ZyReader:getWORD()
                mRecordTabel_1.AttrNum = ZyReader:readString()
                mRecordTabel_1.GeneralName = ZyReader:readString()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
        DataTabel.RecordTabel = RecordTabel_1;
    else
         ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)
    end
    return DataTabel
end


--        1401_玩家佣兵列表接口（ID=1401）
function Action1401(Scene,isLoading ,ToUserID,GeneralType)
	ZyWriter:writeString("ActionId",1401)
	ZyWriter:writeString("ToUserID",ToUserID)
	ZyWriter:writeString("GeneralType",GeneralType)
	ZyExecRequest(Scene, nil,isLoading)
end

function _1401Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.CurrNum= ZyReader:getInt()
		DataTabel.Unopened= ZyReader:getInt()
		local RecordNums_1=ZyReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
				local mRecordTabel_1={}
				ZyReader:recordBegin()
				mRecordTabel_1.GeneralID= ZyReader:getInt()
				mRecordTabel_1.GeneralName= ZyReader:readString()
				mRecordTabel_1.HeadID= ZyReader:readString()
				mRecordTabel_1.GeneralLv= ZyReader:getWORD()
				mRecordTabel_1.GeneralQuality= ZyReader:getWORD()
				mRecordTabel_1.CurrNum= ZyReader:getInt()
				mRecordTabel_1.WorseNum= ZyReader:getInt()
				mRecordTabel_1.IsRecruit= ZyReader:getWORD()
				mRecordTabel_1.IsBattle= ZyReader:getInt()
				mRecordTabel_1.DemandNum= ZyReader:getInt()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.RecordTabel = RecordTabel_1;	
	else          
		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)
	end
	return DataTabel
end

--        1402_招募佣兵界面接口（ID=1402）
function Action1402(Scene,isLoading )
	ZyWriter:writeString("ActionId",1402)
	ZyExecRequest(Scene, nil,isLoading)
end

function _1402Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.GoldNum= ZyReader:getInt()
		DataTabel.GameCoin= ZyReader:getInt()
		local RecordNums_1=ZyReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
				local mRecordTabel_1={}
				ZyReader:recordBegin()
				mRecordTabel_1.RecruitType= ZyReader:getWORD()
				mRecordTabel_1.Quality= ZyReader:readString()
				mRecordTabel_1.SurplusNum= ZyReader:getInt()
				mRecordTabel_1.TotalNum= ZyReader:getInt()
				mRecordTabel_1.DemandGold= ZyReader:getInt()
				mRecordTabel_1.ColdTime= ZyReader:getInt()
				mRecordTabel_1.IsFirst= ZyReader:getWORD()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.RecordTabel = RecordTabel_1;
	else          
		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)
	end
	return DataTabel
end

--        1403_佣兵详情接口（ID=1403）
function Action1403(Scene,isLoading ,GeneralID,ToUserID,data)
	ZyWriter:writeString("ActionId",1403)
	ZyWriter:writeString("GeneralID",GeneralID)
	ZyWriter:writeString("ToUserID",ToUserID)
	ZyExecRequest(Scene, data,isLoading)
end

function _1403Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.GeneralName= ZyReader:readString()
		DataTabel.HeadID= ZyReader:readString()
		DataTabel.PicturesID= ZyReader:readString()
		DataTabel.GeneralQuality= ZyReader:getWORD()
		DataTabel.Experience= ZyReader:getInt()
		DataTabel.MaxExperience= ZyReader:getInt()
		DataTabel.PowerNum= ZyReader:getWORD()
		DataTabel.SoulNum= ZyReader:getWORD()
		DataTabel.IntellectNum= ZyReader:getWORD()
		DataTabel.CareerID= ZyReader:getInt()
		DataTabel.CareerName= ZyReader:readString()
		DataTabel.LifeNum= ZyReader:getInt()
		DataTabel.LifeMaxNum= ZyReader:getInt()
		DataTabel.GeneralLv= ZyReader:getWORD()
		DataTabel.GeneralStatus= ZyReader:getWORD()
		DataTabel.GeneralDesc= ZyReader:readString()
		DataTabel.AttackNum= ZyReader:getInt()
		DataTabel.VitalityNum= ZyReader:getInt()
		DataTabel.TalentAbility = ZyReader:getInt()
		DataTabel.TalentName = ZyReader:readString()
		local RecordNums_1=ZyReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
				local mRecordTabel_1={}
				ZyReader:recordBegin()
				mRecordTabel_1.UserItemID= ZyReader:readString()
				mRecordTabel_1.ItemID= ZyReader:getInt()
				mRecordTabel_1.ItemName= ZyReader:readString()
				mRecordTabel_1.Position= ZyReader:getInt()
				mRecordTabel_1.HeadPic= ZyReader:readString()
				mRecordTabel_1.ItemLv= ZyReader:getWORD()
				mRecordTabel_1.IsSynthesis= ZyReader:getWORD()
				mRecordTabel_1.QualityType= ZyReader:getInt()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.RecordTabel = RecordTabel_1;

		local RecordNums_2=ZyReader:getInt()
		local RecordTabel_2={}
		if RecordNums_2~=0 then
			for k=1,RecordNums_2 do
				local mRecordTabel_2={}
				ZyReader:recordBegin()
				mRecordTabel_2.AbilityID= ZyReader:getInt()
				mRecordTabel_2.AbilityName= ZyReader:readString()
				mRecordTabel_2.AbilityLv= ZyReader:getWORD()
				mRecordTabel_2.AbilityDesc= ZyReader:readString()
				mRecordTabel_2.AbilityHead= ZyReader:readString()
				mRecordTabel_2.Position= ZyReader:getInt()
				mRecordTabel_2.UserItemID= ZyReader:readString()
				mRecordTabel_2.AbilityQuality= ZyReader:getInt()				
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_2,mRecordTabel_2)
			end
		end
		DataTabel.RecordTabel2 = RecordTabel_2;           
		local RecordNums_4=ZyReader:getInt()
		local RecordTabel_4={}
		if RecordNums_4~=0 then
			for k=1,RecordNums_4 do
				local mRecordTabel_4={}
				ZyReader:recordBegin()
				mRecordTabel_4.SkillID = ZyReader:getWORD()
				mRecordTabel_4.SkillNum = ZyReader:readString()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_4,mRecordTabel_4)
			end
		end
		DataTabel.RecordTabel4 = RecordTabel_4;
                    
		local RecordNums_3=ZyReader:getInt()
		local RecordTabel_3={}
		if RecordNums_3~=0 then
			for k=1,RecordNums_3 do
				local mRecordTabel_3={}
				ZyReader:recordBegin()
				mRecordTabel_3.FunctionID= ZyReader:getWORD()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_3,mRecordTabel_3)
			end
		end
		DataTabel.RecordTabel3 = RecordTabel_3;				
		DataTabel.Potential=ZyReader:getInt()
		DataTabel.SoulGrid= ZyReader:getInt()
		DataTabel.BattleHeadID = ZyReader:readString()
		DataTabel.AbilityNum= ZyReader:getInt()
		
		--- 缘分信息
		local RecordNums_4=ZyReader:getInt()
		local RecordTabel_4={}
		if RecordNums_4~=0 then
			for k=1,RecordNums_4 do
				local mRecordTabel_4={}
				ZyReader:recordBegin()
				mRecordTabel_4.KarmaName = ZyReader:readString()
				mRecordTabel_4.KarmaDesc  = ZyReader:readString()
				mRecordTabel_4.IsActive =  ZyReader:getInt()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_4,mRecordTabel_4)
			end
		end
		DataTabel.YFInforTabel = RecordTabel_4;
	else          
		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)
	end
	return DataTabel
end


--        1404_佣兵邀请接口【完成】（ID=1404）
function Action1404(Scene,isLoading ,RecruitType,SoulID,IsLead)
	ZyWriter:writeString("ActionId",1404)
	ZyWriter:writeString("RecruitType",RecruitType)
	ZyWriter:writeString("SoulID",SoulID)
	ZyWriter:writeString("IsLead",IsLead)
	ZyExecRequest(Scene, nil,isLoading)
end


function _1404Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		 DataTabel={}
	        DataTabel.GeneralName= ZyReader:readString()
	        DataTabel.PicturesID= ZyReader:readString()
	        DataTabel.GeneralLv= ZyReader:getWORD()
	        DataTabel.GeneralType= ZyReader:getWORD()
	        DataTabel.LifeNum= ZyReader:getInt()
	        DataTabel.PowerNum= ZyReader:getWORD()
	        DataTabel.SoulNum= ZyReader:getWORD()
	        DataTabel.IntellectNum= ZyReader:getWORD()
	        DataTabel.CurrSoulID= ZyReader:getInt()
	        DataTabel.GainNum= ZyReader:getInt()
	        DataTabel.GeneralQuality= ZyReader:getWORD()
	        DataTabel.Potential= ZyReader:getWORD()
	        DataTabel.AbilityID= ZyReader:getInt()
	        DataTabel.AbilityName= ZyReader:readString()
	        DataTabel.HeadID= ZyReader:readString()
	        DataTabel.AbilityDesc= ZyReader:readString()
	        DataTabel.AbilityQuality= ZyReader:getWORD()
	        DataTabel.AddLifeNum= ZyReader:readString()
	        DataTabel.AddPowerNum= ZyReader:readString()
	        DataTabel.AddSoulNum= ZyReader:readString()
	        DataTabel.AddIntellectNum= ZyReader:readString()
	        DataTabel.AddPotential= ZyReader:readString()
		DataTabel.CareerID= ZyReader:getWORD()
	        
	else          
		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)
	end
	return DataTabel
end

--        1416_传承界面接口（ID=1416）
function Action1416(Scene,isLoading ,Ops)

   	ZyWriter:writeString("ActionId",1416)
   	ZyWriter:writeString("Ops",Ops)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _1416Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.HeritageType= ZyReader:getWORD()
                mRecordTabel_1.GeneralID= ZyReader:getInt()
                mRecordTabel_1.GeneralName= ZyReader:readString()
                mRecordTabel_1.Head= ZyReader:readString()
                mRecordTabel_1.GeneralLv= ZyReader:getWORD()
                mRecordTabel_1.PowerNum= ZyReader:getWORD()
                mRecordTabel_1.SoulNum= ZyReader:getWORD()
                mRecordTabel_1.IntellectNum= ZyReader:getWORD()
                mRecordTabel_1.GeneralQuality= ZyReader:getWORD()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;
                
                
        local RecordNums_2=ZyReader:getInt()
         local RecordTabel_2={}
          if RecordNums_2~=0 then
            for k=1,RecordNums_2 do
             local mRecordTabel_2={}
             ZyReader:recordBegin()
                mRecordTabel_2.OpsType= ZyReader:getInt()
                mRecordTabel_2.VipLv= ZyReader:getWORD()
                mRecordTabel_2.UseGold= ZyReader:getInt()
                mRecordTabel_2.ItemID= ZyReader:getInt()
                mRecordTabel_2.ItemNum= ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_2,mRecordTabel_2)
              end
        end
                DataTabel.RecordTabel2 = RecordTabel_2;
                
        DataTabel.HeritageName= ZyReader:readString()
        DataTabel.HeritageLv= ZyReader:getWORD()
        DataTabel.DisGeneralName= ZyReader:readString()

       
    else          
		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)
    end
    return DataTabel
end

--        1417_传承与被传承人列表接口（ID=1417）
function Action1417(Scene,isLoading ,HeritageType)

   	ZyWriter:writeString("ActionId",1417)
   	ZyWriter:writeString("HeritageType",HeritageType)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _1417Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.GeneralID= ZyReader:getInt()
                mRecordTabel_1.GeneralName= ZyReader:readString()
                mRecordTabel_1.HeadID= ZyReader:readString()
                mRecordTabel_1.GeneralLv= ZyReader:getWORD()
                mRecordTabel_1.CareerName= ZyReader:readString()
                mRecordTabel_1.PowerNum= ZyReader:getWORD()
                mRecordTabel_1.SoulNum= ZyReader:getWORD()
                mRecordTabel_1.IntellectNum= ZyReader:getWORD()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;

       
    else          
          	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)	
    end
    return DataTabel
end

--        1418_传承人与被传承人选择接口（ID=1418）
function Action1418(Scene,isLoading ,GeneralID,HeritageType,userData)

   	ZyWriter:writeString("ActionId",1418)
   	ZyWriter:writeString("GeneralID",GeneralID)
        ZyWriter:writeString("HeritageType",HeritageType)
        
	ZyExecRequest(Scene, userData,isLoading)
end

function _1418Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}

       
    else          
       	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)	
    end
    return DataTabel
end

--        1419_传承接口（ID=1419）
function Action1419(Scene,isLoading ,Ops)

   	ZyWriter:writeString("ActionId",1419)
   	ZyWriter:writeString("Ops",Ops)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _1419Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}  
	else          
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)	
	end
	return DataTabel
end

--        1425_佣兵灵魂突破接口（ID=1425）
function Action1425(Scene,isLoading ,GeneralID,Ops)
	ZyWriter:writeString("ActionId",1425)
	ZyWriter:writeString("GeneralID",GeneralID)
	ZyWriter:writeString("Ops",Ops)
	ZyExecRequest(Scene, GeneralID, isLoading)
end

function _1425Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.GeneralName= ZyReader:readString()
		DataTabel.Potential= ZyReader:getInt()
	else          
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)	
	end
	return DataTabel
end



--        1441_佣兵升级界面接口（ID=1441）
function Action1441(Scene,isLoading ,GeneralID)

   	ZyWriter:writeString("ActionId",1441)
   	ZyWriter:writeString("GeneralID",GeneralID)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _1441Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		local RecordNums_1=ZyReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
				local mRecordTabel_1={}
				ZyReader:recordBegin()
				mRecordTabel_1.UserItemID= ZyReader:readString()
				mRecordTabel_1.ItemID= ZyReader:getInt()
				mRecordTabel_1.ItemName= ZyReader:readString()
				mRecordTabel_1.HeadID= ZyReader:readString()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.RecordTabel = RecordTabel_1;
		DataTabel.CurrGeneralName= ZyReader:readString()
		DataTabel.CurrLv= ZyReader:getWORD()
		DataTabel.NextLv= ZyReader:getWORD()
		DataTabel.CurrHead= ZyReader:readString()
		DataTabel.LiftNum= ZyReader:getInt()
		DataTabel.Experience= ZyReader:getInt()
		DataTabel.AbilityID= ZyReader:getInt()
		DataTabel.IsUp= ZyReader:getWORD()
--		DataTabel.ItemName= ZyReader:readString()
		DataTabel.UpExperience= ZyReader:getInt()
		DataTabel.Percent= ZyReader:readString()
		DataTabel.Status= ZyReader:getInt()

	else          
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)	
	end
	return DataTabel
end

--        1442_佣兵升级接口（ID=1442）
function Action1442(Scene,isLoading ,GeneralID)

   	ZyWriter:writeString("ActionId",1442)
   	ZyWriter:writeString("GeneralID",GeneralID)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _1442Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}

       
    else          
     	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)	
    end
    return DataTabel
end

--3.1.6.1	培养详细接口（ID=1443）
function Action1443(Scene,isLoading,data )
   	ZyWriter:writeString("ActionId",1443)       
	ZyExecRequest(Scene, data,isLoading)
end

function _1443Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		local RecordNums_1=ZyReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
				local mRecordTabel_1={}
				ZyReader:recordBegin()
				mRecordTabel_1.BringUpType= ZyReader:getInt()
				mRecordTabel_1.UseUpType= ZyReader:getWORD()
				mRecordTabel_1.UseUpNum= ZyReader:getInt()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.RecordTabel = RecordTabel_1;
		DataTabel.ItemNum = ZyReader:getInt();
	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)			
	end
	return DataTabel

end

--        1444_佣兵升级卡片列表接口（ID=1444）
function Action1444(Scene,isLoading ,PageIndex,PageSize)

   	ZyWriter:writeString("ActionId",1444)
   	ZyWriter:writeString("PageIndex",PageIndex)
        ZyWriter:writeString("PageSize",PageSize)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _1444Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.PageCount= ZyReader:getInt()
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.UserItemID= ZyReader:readString()
                mRecordTabel_1.ItemID= ZyReader:getInt()
                mRecordTabel_1.ItemName= ZyReader:readString()
                mRecordTabel_1.HeadID= ZyReader:readString()
                mRecordTabel_1.ItemNum= ZyReader:getInt()
                mRecordTabel_1.Experience= ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;

       
    else          
       	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)	
    end
    return DataTabel
end

--        1445_佣兵升级卡片选择接口（ID=1445）
function Action1445(Scene,isLoading ,GeneralCard,GeneralID,GeneralCardNum)

   	ZyWriter:writeString("ActionId",1445)
   	ZyWriter:writeString("GeneralCard",GeneralCard)
        ZyWriter:writeString("GeneralID",GeneralID)
        ZyWriter:writeString("GeneralCardNum",GeneralCardNum)
        
	ZyExecRequest(Scene, nil,isLoading)
end


function _1445Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}

       
    else          
      	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)	
    end
    return DataTabel
end


--        1446_传承界面接口[Python]（ID=1446）
function Action1446(Scene,isLoading ,Ops)

   	ZyWriter:writeString("ActionId",1446)
   	ZyWriter:writeString("Ops",Ops)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _1446Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.HeritageType= ZyReader:getWORD()
                mRecordTabel_1.GeneralID= ZyReader:getInt()
                mRecordTabel_1.GeneralName= ZyReader:readString()
                mRecordTabel_1.Head= ZyReader:readString()
                mRecordTabel_1.GeneralLv= ZyReader:getWORD()
                mRecordTabel_1.PowerNum= ZyReader:getWORD()
                mRecordTabel_1.SoulNum= ZyReader:getWORD()
                mRecordTabel_1.IntellectNum= ZyReader:getWORD()
                mRecordTabel_1.GeneralQuality= ZyReader:getWORD()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;
                
                
                
        local RecordNums_2=ZyReader:getInt()
         local RecordTabel_2={}
          if RecordNums_2~=0 then
            for k=1,RecordNums_2 do
             local mRecordTabel_2={}
             ZyReader:recordBegin()
                mRecordTabel_2.OpsType= ZyReader:getInt()
                mRecordTabel_2.VipLv= ZyReader:getWORD()
                mRecordTabel_2.UseGold= ZyReader:getInt()
                mRecordTabel_2.ItemID= ZyReader:getInt()
                mRecordTabel_2.ItemNum= ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_2,mRecordTabel_2)
              end
        end
                DataTabel.RecordTabel2 = RecordTabel_2;
                
                
        DataTabel.HeritageName= ZyReader:readString()
        DataTabel.HeritageLv= ZyReader:getWORD()
        DataTabel.DisGeneralName= ZyReader:readString()

       
    else          
     	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)	
    end
    return DataTabel
end

--        1447_传承与被传承人列表接口[Python]（ID=1447）
function Action1447(Scene,isLoading ,HeritageType)

   	ZyWriter:writeString("ActionId",1447)
   	ZyWriter:writeString("HeritageType",HeritageType)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _1447Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.GeneralID= ZyReader:getInt()
                mRecordTabel_1.GeneralName= ZyReader:readString()
                mRecordTabel_1.HeadID= ZyReader:readString()
                mRecordTabel_1.GeneralLv= ZyReader:getWORD()
                mRecordTabel_1.CareerName= ZyReader:readString()
                mRecordTabel_1.PowerNum= ZyReader:getWORD()
                mRecordTabel_1.SoulNum= ZyReader:getWORD()
                mRecordTabel_1.IntellectNum= ZyReader:getWORD()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;

       
    else          
       	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)	
    end
    return DataTabel
end

--        1448_传承人与被传承人选择接口[Python]（ID=1448）
function Action1448(Scene,isLoading ,GeneralID,HeritageType, userData)

   	ZyWriter:writeString("ActionId",1448)
   	ZyWriter:writeString("GeneralID",GeneralID)
        ZyWriter:writeString("HeritageType",HeritageType)
        
	ZyExecRequest(Scene, userData,isLoading)
end

function _1448Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}

       
    else          
	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)	
    end
    return DataTabel
end

--        1449_传承接口[Python]（ID=1449）
function Action1449(Scene,isLoading ,Ops)

   	ZyWriter:writeString("ActionId",1449)
   	ZyWriter:writeString("Ops",Ops)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _1449Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}

       
    else          
	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)	
    end
    return DataTabel
end

--        1481_魂技升级接口（ID=1481）
function Action1481(Scene,isLoading ,AbilityID,UserItemID,UserItemIDs)

   	ZyWriter:writeString("ActionId",1481)
   	ZyWriter:writeString("AbilityID",AbilityID)
        ZyWriter:writeString("UserItemID",UserItemID)
        ZyWriter:writeString("UserItemIDs",UserItemIDs)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _1481Callback(pScutScene, lpExternalData)
	
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.ExperienceNum= ZyReader:getInt()
		DataTabel.AbilityLv= ZyReader:getInt()
	
	
	else          
	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)	
	end
	return DataTabel
end

--        1482_魂技升级详细接口（ID=1482）
function Action1482(Scene,isLoading ,UserItemID)

   	ZyWriter:writeString("ActionId",1482)
   	ZyWriter:writeString("UserItemID",UserItemID)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _1482Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.AbilityName= ZyReader:readString()
        DataTabel.AbilityLv= ZyReader:getInt()
        DataTabel.IsExperienceNum= ZyReader:getInt()
        DataTabel.NextExperienceNum= ZyReader:getInt()
        DataTabel.HeadID= ZyReader:readString()
        DataTabel.IsMaxLv= ZyReader:getInt()
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.UserItemID= ZyReader:readString()
                mRecordTabel_1.AbilityID= ZyReader:getInt()
                mRecordTabel_1.HeadID= ZyReader:readString()
                mRecordTabel_1.AbilityName= ZyReader:readString()
                mRecordTabel_1.AbilityDesc= ZyReader:readString()
                mRecordTabel_1.ExperienceNum= ZyReader:getInt()
                mRecordTabel_1.AbilityLv= ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;

       
    else          
	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)	
    end
    return DataTabel
end

--        1483_魂技列表接口（ID=1483）
function Action1483(Scene,isLoading ,PageIndex,PageSize,Ops)
	ZyWriter:writeString("ActionId",1483)
	ZyWriter:writeString("PageIndex",PageIndex)
	ZyWriter:writeString("PageSize",PageSize)
	ZyWriter:writeString("Ops",Ops)    
	ZyExecRequest(Scene, nil,isLoading)
end

function _1483Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.PageCount= ZyReader:getInt()
		local RecordNums_1=ZyReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
				local mRecordTabel_1={}
				ZyReader:recordBegin()
				mRecordTabel_1.AbilityID= ZyReader:getInt()
				mRecordTabel_1.AbilityName= ZyReader:readString()
				mRecordTabel_1.HeadID= ZyReader:readString()
				mRecordTabel_1.AbilityDesc= ZyReader:readString()
				mRecordTabel_1.AbilityLv= ZyReader:getInt()
				mRecordTabel_1.GeneralID= ZyReader:getInt()
				mRecordTabel_1.GeneralName= ZyReader:readString()
				mRecordTabel_1.UserItemID= ZyReader:readString()
				mRecordTabel_1.AttackType= ZyReader:getInt()
				mRecordTabel_1.AbilityQuality= ZyReader:getInt()
				mRecordTabel_1.RatioNum= ZyReader:getInt()
				
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.RecordTabel = RecordTabel_1;
	else          
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)	
	end
	return DataTabel
end

--        1484_佣兵魂技更换接口（ID=1484）
function Action1484(Scene,isLoading ,Ops,AbilityID,UserItemID,GeneralID,Position)

   	ZyWriter:writeString("ActionId",1484)
   	ZyWriter:writeString("Ops",Ops)
        ZyWriter:writeString("AbilityID",AbilityID)
        ZyWriter:writeString("UserItemID",UserItemID)
        ZyWriter:writeString("GeneralID",GeneralID)
        ZyWriter:writeString("Position",Position)
        
	ZyExecRequest(Scene, Ops,isLoading)
end


function _1484Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}

       
    else          
	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)	
    end
    return DataTabel
end

--        1485_魂技详细接口（ID=1485）
function Action1485(Scene,isLoading ,UserItemID ,data)

   	ZyWriter:writeString("ActionId",1485)
   	ZyWriter:writeString("UserItemID",UserItemID )
        
	ZyExecRequest(Scene, data,isLoading)
end

function _1485Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.AbilityID= ZyReader:getInt()
        DataTabel.HeadID= ZyReader:readString()
        DataTabel.AbilityName= ZyReader:readString()
        DataTabel.AbilityDesc= ZyReader:readString()
        DataTabel.EffectDesc= ZyReader:readString()
        DataTabel.MaxHeadID= ZyReader:readString()
        DataTabel.FntHeadID= ZyReader:readString()
        DataTabel.AbilityLv= ZyReader:getInt()
        DataTabel.AbilityQuality= ZyReader:getInt()
          DataTabel.AttackType= ZyReader:getInt()
     
    else          
         ZyToast.show(pScutScene,ZyReader:readErrorMsg(),2,0.35)
    end
    return DataTabel
end


--        1501_魔术列表接口（ID=1501）
function Action1501(Scene,isLoading ,PageIndex,PageSize,MagicType)
    ZyWriter:writeString("ActionId",1501)
   	ZyWriter:writeString("PageIndex",PageIndex)
    ZyWriter:writeString("PageSize",PageSize)
    ZyWriter:writeString("MagicType",MagicType)
    ZyExecRequest(Scene, nil,isLoading)
end

function _1501Callback(pScutScene, lpExternalData)
   local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.PageCount= ZyReader:getInt()
        DataTabel.ExpNum   = ZyReader:getInt()
        DataTabel.QueueID  = ZyReader:readString()
        DataTabel.ColdTime = ZyReader:getInt()
        local RecordNums_1 = ZyReader:getInt()
        local RecordTabel_1={}
 		if RecordNums_1~=0 then
     		for k=1,RecordNums_1 do
             local mRecordTabel_1={}
                ZyReader:recordBegin()
                mRecordTabel_1.MagicID= ZyReader:getInt()
                mRecordTabel_1.MagicType= ZyReader:getInt()
                mRecordTabel_1.MagicName= ZyReader:readString()
                mRecordTabel_1.HeadID= ZyReader:readString()
                mRecordTabel_1.MagicLv= ZyReader:getWORD()
                mRecordTabel_1.IsUp= ZyReader:getWORD()


                mRecordTabel_1.IsEnabled= ZyReader:getInt()
                mRecordTabel_1.IsLv= ZyReader:getInt()

                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
   		DataTabel.RecordTabel = RecordTabel_1;
	else
  		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),2,0.35)
    end
    return DataTabel
end

--        1502_魔术详情接口（ID=1502）
function Action1502(Scene,isLoading ,MagicID)
    ZyWriter:writeString("ActionId",1502)
   	ZyWriter:writeString("MagicID",MagicID)
    ZyExecRequest(Scene, nil,isLoading)
end

function _1502Callback(pScutScene, lpExternalData)
   local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.MagicName = ZyReader:readString()
        DataTabel.MagicLv   = ZyReader:getWORD()
        DataTabel.MagicDesc = ZyReader:readString()
        DataTabel.UserLv    = ZyReader:getWORD()
        DataTabel.ExpNum    = ZyReader:getInt()
        DataTabel.ColdTime  = ZyReader:getInt()
    else
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),2,0.35)
    end
    return DataTabel
end

--        1503_魔术升级接口（ID=1503）
function Action1503(Scene,isLoading ,MagicID)
    ZyWriter:writeString("ActionId",1503)
   	ZyWriter:writeString("MagicID",MagicID)
    ZyExecRequest(Scene, nil,isLoading)
end

function _1503Callback(pScutScene, lpExternalData)
   local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.ColdTime= ZyReader:getInt()
        DataTabel.Description= ZyReader:getInt()
    else
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),2,0.35)
    end
    return DataTabel
end

--        1601_装备、丹药合成材料列表接口（ID=1601）
function Action1601(Scene,isLoading ,ItemID,UserItemID)

   	ZyWriter:writeString("ActionId",1601)
   	ZyWriter:writeString("ItemID",ItemID)
        ZyWriter:writeString("UserItemID",UserItemID)
        
	ZyExecRequest(Scene, nil,isLoading)
end


function _1601Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.SynthesisID = ZyReader:getInt()--合成ID
        DataTabel.ItemName    = ZyReader:readString()--名称
        DataTabel.HeadID      = ZyReader:readString()--头像
        DataTabel.QualityType = ZyReader:getWORD()--品质
        DataTabel.Level       = ZyReader:getWORD()--等级
        DataTabel.MedicineLv  = ZyReader:getWORD()--丹药品级
        DataTabel.AttrTypeID  = ZyReader:getWORD()--丹药属性 1力量 2魂力 3智力
        DataTabel.MedicineNum = ZyReader:getWORD()--数值
        local RecordNums_1=ZyReader:getInt()
        local RecordTabel_1={}
        if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
                local mRecordTabel_1={}
                ZyReader:recordBegin()
                mRecordTabel_1.AbilityType = ZyReader:getWORD()
                mRecordTabel_1.BaseNum     = ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
        DataTabel.RecordTabel = RecordTabel_1;
        local RecordNums_12    = ZyReader:getInt()
        local RecordTabel_12={}
        if RecordNums_12~=0 then
            for k=1,RecordNums_12 do
                local mRecordTabel_12={}
                ZyReader:recordBegin()
                mRecordTabel_12.MaterialsID   = ZyReader:getInt()
                mRecordTabel_12.MaterialsName = ZyReader:readString()
                mRecordTabel_12.HeadID        = ZyReader:readString()
                mRecordTabel_12.CurNum        = ZyReader:getInt()
                mRecordTabel_12.MaxNum        = ZyReader:getInt()
                mRecordTabel_12.PlotID         = ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_12,mRecordTabel_12)
            end
        end
        DataTabel.RecordTabel2 = RecordTabel_12;
    else
         ZyToast.show(pScutScene,ZyReader:readErrorMsg(),2,0.35)
    end
    return DataTabel
end

--        1603_装备、丹药合成接口（ID=1603）
function Action1603(Scene,isLoading ,UserItemID,UserEquID,Ops)
   	ZyWriter:writeString("ActionId",1603)
   	ZyWriter:writeString("UserItemID",UserItemID)
        ZyWriter:writeString("UserEquID",UserEquID)
        ZyWriter:writeString("Ops",Ops)
	ZyExecRequest(Scene, nil,isLoading)
end

function _1603Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
    else
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),2,0.35)
    end
    return DataTabel
end

--        1604_材料掉落副本接口（ID=1604）
function Action1604(Scene,isLoading ,MaterialsID)

   	ZyWriter:writeString("ActionId",1604)
   	ZyWriter:writeString("MaterialsID",MaterialsID)

	ZyExecRequest(Scene, nil,isLoading)
end

function _1604Callback(pScutScene, lpExternalData)

    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.CityID= ZyReader:getInt()
        DataTabel.PlotType= ZyReader:getWORD()
        DataTabel.PlotID= ZyReader:getInt()
    else
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),2,0.2)
    end
    return DataTabel
end


--        1605_药水加血使用接口（ID=1605）
function Action1605(Scene,isLoading ,UserItemID)

   	ZyWriter:writeString("ActionId",1605)
   	ZyWriter:writeString("UserItemID",UserItemID)

	ZyExecRequest(Scene, nil,isLoading)
end

--        1606_新手礼包使用接口【完成】（ID=1606）
function Action1606(Scene,isLoading ,UserItemID)

   	ZyWriter:writeString("ActionId",1606)
   	ZyWriter:writeString("UserItemID",UserItemID)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _1606Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.HasNextGift= ZyReader:getInt()
	 DataTabel.Content= ZyReader:readString()
       
    else          
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),2,0.2)
    end
    return DataTabel
end


--        1609_VIP礼包使用接口（ID=1609）
function Action1609(Scene,isLoading ,UserItemID)
	ZyWriter:writeString("ActionId",1609)
	ZyWriter:writeString("UserItemID",UserItemID)
	ZyExecRequest(Scene, nil,isLoading)
end

---集邮系统
function Action1610(Scene,isLoading,AlbumType,PageIndex,PageSize)
	ZyWriter:writeString("ActionId",1610)
	ZyWriter:writeString("AlbumType",AlbumType)
	ZyWriter:writeString("PageIndex",PageIndex)
	ZyWriter:writeString("PageSize",PageSize)
	ZyExecRequest(Scene, nil,isLoading)
end

function _1610Callback(pScutScene, lpExternalData)
	local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
 		DataTabel={}
 		DataTabel.PageCount = ZyReader:getInt()
 		local RecordNums_1 =ZyReader:getInt()
		local RecordTabel_1={}
  		if RecordNums_1~=0 then
     			for k=1,RecordNums_1 do
		      		local mRecordTabel_1={}
		     		ZyReader:recordBegin()
				mRecordTabel_1.HeadID = ZyReader:readString()
				mRecordTabel_1.Name = ZyReader:readString()
				mRecordTabel_1.Status = ZyReader:getInt()
				mRecordTabel_1.ID =   ZyReader:getInt()
				mRecordTabel_1.Quality =  ZyReader:getWORD()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
       		end
       	end

        DataTabel.StampTable = RecordTabel_1;
  	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        1611_集邮册卡牌详细信息显示接口（ID=1611）
function Action1611(Scene,isLoading ,AlbumType,CardID)

   	ZyWriter:writeString("ActionId",1611)
   	ZyWriter:writeString("AlbumType",AlbumType)
        ZyWriter:writeString("CardID",CardID)
        
	ZyExecRequest(Scene, AlbumType,isLoading)
end

function _1611Callback(pScutScene, lpExternalData)
   
local DataTabel=nil
if ZyReader:getResult() == eScutNetSuccess then
	DataTabel={}
	DataTabel.GeneralName= ZyReader:readString()
	DataTabel.HeadID= ZyReader:readString()
	DataTabel.PicturesID= ZyReader:readString()
	DataTabel.GeneralQuality= ZyReader:getWORD()
	DataTabel.PowerNum= ZyReader:getWORD()
	DataTabel.SoulNum= ZyReader:getWORD()
	DataTabel.IntellectNum= ZyReader:getWORD()
	DataTabel.CareerID= ZyReader:getWORD()
	DataTabel.LifeNum= ZyReader:getInt()
	DataTabel.GeneralLv= ZyReader:getWORD()
	DataTabel.GeneralDesc= ZyReader:readString()
	DataTabel.TalentAbility= ZyReader:getInt()
	DataTabel.TalentName= ZyReader:readString()
	DataTabel.TalentAbilityQuality= ZyReader:getInt()
	DataTabel.TalentAbilityHead= ZyReader:readString()
	local RecordNums_1=ZyReader:getInt()
	local RecordTabel_1={}
	if RecordNums_1~=0 then
		for k=1,RecordNums_1 do
			local mRecordTabel_1={}
			ZyReader:recordBegin()
			mRecordTabel_1.SkillID= ZyReader:getWORD()
			mRecordTabel_1.SkillNum= ZyReader:readString()
			ZyReader:recordEnd()
			ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
		end
	end
	DataTabel.RecordTabel = RecordTabel_1;


	DataTabel.ItemID= ZyReader:getInt()
	DataTabel.ItemName= ZyReader:readString()
	DataTabel.ItemHead= ZyReader:readString()
	DataTabel.QualityType= ZyReader:getInt()
	DataTabel.SalePrice= ZyReader:getInt()
	DataTabel.ItemDesc= ZyReader:readString()	
	local RecordNums_2=ZyReader:getInt()
	local RecordTabel_2={}
	if RecordNums_2~=0 then
		for k=1,RecordNums_2 do
			local mRecordTabel_2={}
			ZyReader:recordBegin()
			mRecordTabel_2.AbilityType= ZyReader:getInt()
			mRecordTabel_2.BaseNum= ZyReader:getInt()
			ZyReader:recordEnd()
			ZyTable.push_back(RecordTabel_2,mRecordTabel_2)
		end
	end
	DataTabel.RecordTabel2 = RecordTabel_2;

	DataTabel.AbilityID= ZyReader:getInt()
	DataTabel.AbilityName= ZyReader:readString()
	DataTabel.AbilityHead= ZyReader:readString()
	DataTabel.AbilityDesc= ZyReader:readString()
	DataTabel.AbilityQuality= ZyReader:getInt()

       
    else          
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end



--        1901_魔法阵列表接口（ID=1901）
function Action1901(Scene,isLoading )
ZyWriter:writeString("ActionId",1901)
   ZyExecRequest(Scene, nil,isLoading)
end

function _1901Callback(pScutScene, lpExternalData)
	local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
 		DataTabel={}
 		local i=ZyReader:getInt()
		local RecordTabel_1={}
  		if i~=0 then
     		for k=1,i do
	      		local mRecordTabel_1={}
	     		ZyReader:recordBegin()
	     		mRecordTabel_1.EmbattleID= ZyReader:getInt()
	         	mRecordTabel_1.EmbattleName= ZyReader:readString()
	       		mRecordTabel_1.EmbattleLv= ZyReader:getWORD()
	       		mRecordTabel_1.IsEnabled= ZyReader:getInt()
	      		mRecordTabel_1.EffectDesc= ZyReader:readString()
	        	local j=ZyReader:getInt()
	         	local RecordTabel_2={}
	          	if j~=0 then
	            	for k=1,j do
	             		local mRecordTabel_2={}
	             		ZyReader:recordBegin()
	             		mRecordTabel_2.IsStations= ZyReader:getWORD()
		                mRecordTabel_2.Location= ZyReader:getWORD()
		                mRecordTabel_2.EGeneralID= ZyReader:getInt()
		                mRecordTabel_2.GeneralHeadID= ZyReader:readString()
		                mRecordTabel_2.IsReplace=ZyReader:getWORD()
		                mRecordTabel_2.GeneralQuality= ZyReader:getWORD()
		                ZyReader:recordEnd()
		                ZyTable.push_back(RecordTabel_2,mRecordTabel_2)
	              	end
	            end
	        	mRecordTabel_1.Location_ = RecordTabel_2;
	   		ZyReader:recordEnd()
	          	ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
       		end
       	end
        DataTabel.EmbattleID_ = RecordTabel_1
		i=ZyReader:getInt()
   		local RecordTabel_1_2={}
     	if i~=0 then
    		for k=1,i do
         		local mRecordTabel_1_2={}
             	ZyReader:recordBegin()
                mRecordTabel_1_2.GeneralID= ZyReader:getInt()
                mRecordTabel_1_2.HeadID= ZyReader:readString()
                mRecordTabel_1_2.GeneralQuality= ZyReader:getWORD()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1_2,mRecordTabel_1_2)
     		end
     	end
        DataTabel.GeneralID_ = RecordTabel_1_2;
  	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

function Action1091(Scene,isLoading )
	ZyWriter:writeString("ActionId",1091)
   	ZyExecRequest(Scene, nil,isLoading)
end

function _1091Callback(pScutScene, lpExternalData)
   	local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.CurrDayUseNum =ZyReader:getInt()
        DataTabel.CurrDayUseMaxNum  =ZyReader:getInt()
        DataTabel.UseGold =ZyReader:getInt()
        DataTabel.RecoverEnergy  =ZyReader:getInt()
	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        1902_魔法阵设置接口（ID=1902）
function Action1902(Scene,isLoading ,MagicID,Ops,GeneralID,Location)
	ZyWriter:writeString("ActionId",1902)
	ZyWriter:writeString("MagicID",MagicID)
	ZyWriter:writeString("Ops",Ops)
	ZyWriter:writeString("GeneralID",GeneralID)
	ZyWriter:writeString("Location",Location)
	ZyExecRequest(Scene, nil,isLoading)
end

function _1902Callback(pScutScene, lpExternalData)
   	local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        1903_魔法阵启用接口（ID=1903）
function Action1903(Scene,isLoading ,MagicID)
	ZyWriter:writeString("ActionId",1903)
   	ZyWriter:writeString("MagicID",MagicID)
   	ZyExecRequest(Scene, nil,isLoading)
end

function _1903Callback(pScutScene, lpExternalData)
   	local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

-- 3009活动列表接口
function Action3009(Scene,isLoading)
	ZyWriter:writeString("ActionId",3009)
   	ZyExecRequest(Scene, nil,isLoading)
end

function _3009Callback(pScutScene, lpExternalData)
   	local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
	 local RecordNums_1= ZyReader:getInt()
         	local RecordTabel_1={}
		if RecordNums_1~=0 then
      		for k=1,RecordNums_1 do
       		local mRecordTabel_1={}
             		ZyReader:recordBegin()
			mRecordTabel_1.ActiveId = ZyReader:getInt()
			mRecordTabel_1.ActiveName = ZyReader:readString()
			mRecordTabel_1.ActiveType = ZyReader:getWORD()
			mRecordTabel_1.BeginDate = ZyReader:readString()
			mRecordTabel_1.EndDate = ZyReader:readString()
			mRecordTabel_1.EnableStatus = ZyReader:getWORD()
			mRecordTabel_1.BossLv = ZyReader:getWORD()
			mRecordTabel_1.HeadID = ZyReader:readString()
			mRecordTabel_1.Descp = ZyReader:readString()
			mRecordTabel_1.ActiveStyle = ZyReader:getWORD()
			mRecordTabel_1.GuildID = ZyReader:readString()
               	 ZyReader:recordEnd()
                	ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
     			end
      		  end
  		DataTabel.RecordTabel = RecordTabel_1
	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end
-- 3012活动信息列表接口
function Action3012(Scene,isLoading)
	ZyWriter:writeString("ActionId",3012)
   	ZyExecRequest(Scene, nil,isLoading)
end

function _3012Callback(pScutScene, lpExternalData)
   	local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
         local RecordNums_1= ZyReader:getInt()
         	local RecordTabel_1={}
		if RecordNums_1~=0 then
      		for k=1,RecordNums_1 do
       		local mRecordTabel_1={}
				ZyReader:recordBegin()
				mRecordTabel_1.FestivalID   = ZyReader:getInt()
				mRecordTabel_1.FestivalName   = ZyReader:readString()
				mRecordTabel_1.FestivalType   = ZyReader:getWORD()
				mRecordTabel_1.StartDate   = ZyReader:readString()
				mRecordTabel_1.EndDate   = ZyReader:readString()
				mRecordTabel_1.HeadID    = ZyReader:readString()
				mRecordTabel_1.RestrainNum   = ZyReader:getInt()
				mRecordTabel_1.FestivalDesc   = ZyReader:readString()
				mRecordTabel_1.IsReceive  = ZyReader:getWORD()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
     			end
      		  end
  		DataTabel.activeTable = RecordTabel_1
	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

----3013   精灵祝福
function Action3013(Scene,isLoading)
	ZyWriter:writeString("ActionId",3013)
   	ZyExecRequest(Scene, nil,isLoading)
end

function _3013Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.RewardInfo  = ZyReader:readString()
		DataTabel.Num   = ZyReader:getInt()
		DataTabel.HeadID = ZyReader:readString()
	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
	end
	return DataTabel
end

-- 3014活动奖励领取接口
function Action3014(Scene,isLoading,FestivalID)
	ZyWriter:writeString("ActionId",3014)
	ZyWriter:writeString("FestivalID",FestivalID)
   	ZyExecRequest(Scene, nil,isLoading)
end


function _3014Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
	else          
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
	end
	return DataTabel
end


----祈祷详细接口
function Action3301(Scene,isLoading )
   	ZyWriter:writeString("ActionId",3301)
	ZyExecRequest(Scene, nil,isLoading)
end


function _3301Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.PrayType = ZyReader:getInt()
		DataTabel.PrayNum  = ZyReader:getInt()
		DataTabel.IsPrayNum   = ZyReader:getInt()
		DataTabel.PrayDesc = ZyReader:readString()
		DataTabel.IsStatu = ZyReader:getInt()
	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
	end
	
	return DataTabel
end

---祈祷奖励的接口
function Action3302(Scene,isLoading )
   	ZyWriter:writeString("ActionId",3302)
	ZyExecRequest(Scene, nil,isLoading)
end


function _3302Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.Cue = ZyReader:readString()
	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
	end
	
	return DataTabel
end




--        4001_副本列表接口（ID=4001）
function Action4001(Scene,isLoading ,CityID,PlotType)

   	ZyWriter:writeString("ActionId",4001)
   	ZyWriter:writeString("CityID",CityID)
        ZyWriter:writeString("PlotType",PlotType)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _4001Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        --第一层
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
             	--第二层
        		local RecordNums_2=ZyReader:getInt()
        		local RecordTabel_2={}
		       if RecordNums_2~=0 then
		            for k=1,RecordNums_2 do
		             	local mRecordTabel_2={}
		              ZyReader:recordBegin()
                		mRecordTabel_2.PlotID = ZyReader:getInt()
                		mRecordTabel_2.PlotSeqNo = ZyReader:getWORD()
                		mRecordTabel_2.PlotName = ZyReader:readString()
                		mRecordTabel_2.BossHeadID = ZyReader:readString()
                		mRecordTabel_2.PlotStatus = ZyReader:getWORD()
                		mRecordTabel_2.ScoreNum  = ZyReader:getInt()
                		mRecordTabel_2.isKill  = ZyReader:getInt()
                		mRecordTabel_2.Experience   = ZyReader:getInt()
                		mRecordTabel_2.GameCoin   = ZyReader:getInt()
                		mRecordTabel_2.PlotDesc   = ZyReader:readString()
                		mRecordTabel_2.PlotLv    = ZyReader:getInt()
                		mRecordTabel_2.EnergyNum     = ZyReader:getInt()
                		mRecordTabel_2.ChallengeNum     = ZyReader:getInt()
                		mRecordTabel_2.MaxChallengeNum     = ZyReader:getInt()
                		mRecordTabel_2.PlotNum      = ZyReader:getInt()
                		
                		local RecordNums_3=ZyReader:getInt()
                        local RecordTabel_3={}
                        if RecordNums_3~=0 then
                            for k=1,RecordNums_3 do
                                local mRecordTabel_3={}
                                ZyReader:recordBegin()
                                mRecordTabel_3.ItemName= ZyReader:readString()
                                mRecordTabel_3.Num= ZyReader:getInt()
                                ZyReader:recordEnd()
                                ZyTable.push_back(RecordTabel_3,mRecordTabel_3)
                            end
                        end
                        mRecordTabel_2.DropItemTable=RecordTabel_3
                		
                         ZyReader:recordEnd()
                         ZyTable.push_back(RecordTabel_2,mRecordTabel_2)
              		 end
              	 end
                mRecordTabel_1.CityPlotTable=RecordTabel_2
                mRecordTabel_1.CityID = ZyReader:getInt()
                mRecordTabel_1.CityName = ZyReader:readString()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;
                DataTabel.ResetNum =ZyReader:getInt()
                DataTabel.BackpackType =ZyReader:getInt()
    else
     ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end



--        4002_副本地图加载接口（ID=4002）
function Action4002(Scene,isLoading ,PlotID,Ops)

   	ZyWriter:writeString("ActionId",4002)
   	ZyWriter:writeString("PlotID",PlotID)
        ZyWriter:writeString("Ops",Ops)

	ZyExecRequest(Scene, PlotID,isLoading)
end


function _4002Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    local result = ZyReader:getResult()
    if result == eScutNetSuccess then
        DataTabel={}
        DataTabel.PlotName= ZyReader:readString()
        DataTabel.BgScene= ZyReader:readString()
        DataTabel.FgScene= ZyReader:readString()
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.MercenaryID= ZyReader:getInt()
                mRecordTabel_1.MercenarySeq= ZyReader:getWORD()
                mRecordTabel_1.MercenaryName= ZyReader:readString()
                mRecordTabel_1.HeadID= ZyReader:readString()
                mRecordTabel_1.MercenaryTip= ZyReader:readString()
                mRecordTabel_1.PointX= ZyReader:getInt()
                mRecordTabel_1.PointY= ZyReader:getInt()
                mRecordTabel_1.PreStoryCode= ZyReader:readString()
                mRecordTabel_1.AftStoryCode= ZyReader:readString()

                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
        DataTabel.DemenInfo = RecordTabel_1;
        DataTabel.IsOverCombat = ZyReader:getWORD()   
    end
    return DataTabel
end


--------------------3.5.1.3	副本通关宝箱与评价接口（ID=4003）
function Action4003(Scene,isLoading ,PlotID)
   	ZyWriter:writeString("ActionId",4003)
   	ZyWriter:writeString("PlotID",PlotID)
	ZyExecRequest(Scene, nil,isLoading)
end

function _4003Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.AttackScore= ZyReader:getWORD()
        DataTabel.DefenseScore= ZyReader:getWORD()
        DataTabel.ScoreNum= ZyReader:getWORD()
        DataTabel.StarScore= ZyReader:getWORD()
        DataTabel.Experience= ZyReader:getInt()
        DataTabel.YueLiNum= ZyReader:getInt()
        DataTabel.PennyNum= ZyReader:getInt()
        DataTabel.GoldNum= ZyReader:getInt()
        DataTabel.ItemHeadID= ZyReader:readString()
        DataTabel.ItemName= ZyReader:readString()
        DataTabel.ItemQualityType= ZyReader:getWORD()

       local RecordNums_1=ZyReader:getInt()
	local RecordTabel_1={}
	 if RecordNums_1~=nil and RecordNums_1>0 then
		    		for k=1, RecordNums_1 do
		    			local mRecordTabel_1={}
		    			ZyReader:recordBegin()
		    			mRecordTabel_1.GeneralName=ZyReader:readString()
		    			ZyReader:recordEnd()
		    			 ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
		    		end
		    end
	DataTabel.GeneralName=RecordTabel_1

 DataTabel.BlessPennyNum= ZyReader:getInt()


		local RecordNums_2=ZyReader:getInt()
		local RecordTabel_2={}
		if RecordNums_2~=0 then
			for k=1,RecordNums_2 do
				local mRecordTabel_2={}
				ZyReader:recordBegin()
				mRecordTabel_2.Name= ZyReader:readString()
				mRecordTabel_2.HeadID= ZyReader:readString()
				mRecordTabel_2.Num= ZyReader:getInt()
				mRecordTabel_2.ItemID = ZyReader:getInt()
				mRecordTabel_2.MaxHeadID =  ZyReader:readString()
				mRecordTabel_2.ItemDesc =  ZyReader:readString()
				mRecordTabel_2.QualityType= ZyReader:getInt()
			
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_2,mRecordTabel_2)
			end
		end
		DataTabel.RecordTabel2 = RecordTabel_2;
		
		DataTabel.HonourNum= ZyReader:getInt()
		DataTabel.PlotSuccessType= ZyReader:getInt()
		DataTabel.PlotFailureType= ZyReader:getInt()
		DataTabel.PlotName= ZyReader:readString()
		DataTabel.MaxHonourNum= ZyReader:getInt()
		DataTabel.CurrentHonour= ZyReader:getInt()
		DataTabel.LastMaxHonourNum= ZyReader:getInt()
		DataTabel.IsUpgrade= ZyReader:getInt()
    else
    		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),0.8,0.3)
    end
    return DataTabel
end


--        4004_副本战斗详情接口（ID=4004）
function Action4004(Scene,isLoading ,PlotNpcID)

   	ZyWriter:writeString("ActionId",4004)
   	ZyWriter:writeString("PlotNpcID",PlotNpcID)

	ZyExecRequest(Scene, nil,isLoading)
end

function _4004Callback(pScutScene, lpExternalData)

	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.IsWin=ZyReader:getWORD()
		DataTabel.Experience=ZyReader:getInt()
		-----获得的物品
		local RecordNums_1=ZyReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=nil and RecordNums_1>0 then
			for k=1, RecordNums_1 do
				local mRecordTabel_1={}
				ZyReader:recordBegin()
				mRecordTabel_1.ItemName=ZyReader:readString()
				mRecordTabel_1.ItemHeadID=ZyReader:readString()
				mRecordTabel_1.ItemQualityType=ZyReader:getWORD()
				mRecordTabel_1.ItemNum=ZyReader:getInt()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.Getitems=RecordTabel_1
		    
		    
		-------------攻防阵型
		local RecordNums_2=ZyReader:getInt()
		local RecordTabel_2={}
		if RecordNums_2~=nil and RecordNums_2>0 then
		for  k=1, RecordNums_2 do
		local mRecordNums_2={}
		ZyReader:recordBegin()
		mRecordNums_2.AttGeneralID=ZyReader:getInt()
		mRecordNums_2.AttGeneralName=ZyReader:readString()
		mRecordNums_2.AttGeneralHeadID=ZyReader:readString()
		mRecordNums_2.AttPosition=ZyReader:getWORD()
		mRecordNums_2.LiveNum=ZyReader:getInt()
		mRecordNums_2.LiveMaxNum=ZyReader:getInt()
		mRecordNums_2.MomentumNum=ZyReader:getWORD()
		mRecordNums_2.MaxMomentumNum=ZyReader:getWORD()
		mRecordNums_2.AbilityID=ZyReader:getInt()
		mRecordNums_2.GeneralLv=ZyReader:getWORD()
		mRecordNums_2.IsAttReplace= ZyReader:getWORD()
		mRecordNums_2.AttGeneralQuality= ZyReader:getWORD()
		ZyReader:recordEnd()
		ZyTable.push_back(RecordTabel_2,mRecordNums_2)
		end
		end
		DataTabel.AgainstTable=RecordTabel_2
		    
		
		-------------防守阵型
		local RecordNums_3=ZyReader:getInt()
		local RecordTabel_3={}
		if RecordNums_3~=nil and RecordNums_3>0 then
			for  k=1, RecordNums_3 do
				local mRecordNums_3={}
				ZyReader:recordBegin()
				mRecordNums_3.AttGeneralID=ZyReader:getInt()
				mRecordNums_3.AttGeneralName=ZyReader:readString()
				mRecordNums_3.AttGeneralHeadID=ZyReader:readString()
				mRecordNums_3.AttPosition=ZyReader:getWORD()
				mRecordNums_3.LiveNum=ZyReader:getInt()
				mRecordNums_3.LiveMaxNum=ZyReader:getInt()
				mRecordNums_3.MomentumNum=ZyReader:getWORD()
				mRecordNums_3.MaxMomentumNum=ZyReader:getWORD()
				mRecordNums_3.AbilityID=ZyReader:getInt()
				mRecordNums_3.GeneralLv=ZyReader:getWORD()
				mRecordNums_3.IsAttReplace= ZyReader:getWORD()
				mRecordNums_3.AttGeneralQuality= ZyReader:getWORD()				
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_3,mRecordNums_3)
			end
		end
		DataTabel.DefendingTable=RecordTabel_3
		    
		    
		
		-----------------战斗过程
		local RecordNums_4=ZyReader:getInt()
		local RecordTabel_4={}
		if RecordNums_4~=nil and RecordNums_4>0 then
			for  k=1, RecordNums_4 do
				local mRecordNums_4={}
				ZyReader:recordBegin()
				mRecordNums_4.AttGeneralID=ZyReader:getInt()
				mRecordNums_4.AttGeneralLiveNum=ZyReader:getInt()
				mRecordNums_4.AttGeneralQishi=ZyReader:getWORD()
				mRecordNums_4.AttackTaget=ZyReader:getWORD()
				mRecordNums_4.AttackType=ZyReader:getWORD()
				mRecordNums_4.AbilityProperty=ZyReader:getWORD()
				mRecordNums_4.AttGeneralStatus=ZyReader:getWORD()
				mRecordNums_4.BackDamage=ZyReader:getInt()
				mRecordNums_4.AttEffectID=ZyReader:readString()
				mRecordNums_4.TargetEffectID=ZyReader:readString()
				mRecordNums_4.IsMove=ZyReader:getWORD()
				mRecordNums_4.Position=ZyReader:getWORD()
				mRecordNums_4.Role=ZyReader:getWORD()
			
				-----内嵌循环 中招效果开始
				local RecordNums_4_0=ZyReader:getInt()
				local RecordTabel_4_0={}
				if RecordNums_4_0~=nil and RecordNums_4_0>0 then
					for k=1, RecordNums_4_0 do
						local mRecordTabel_4_0={}
						ZyReader:recordBegin()
						mRecordTabel_4_0.GeneralEffect=ZyReader:getWORD()
						mRecordTabel_4_0.ConDamageNum=ZyReader:getInt()
						mRecordTabel_4_0.IsIncrease=ZyReader:getInt()
						ZyReader:recordEnd()
						ZyTable.push_back(RecordTabel_4_0,mRecordTabel_4_0)
					end
				end
				mRecordNums_4.GeneralEffects=RecordTabel_4_0

				-----内嵌循环 受到攻击的角色
				local RecordNums_4_1=ZyReader:getInt()
				local RecordTabel_4_1={}
				if RecordNums_4_1~=nil and RecordNums_4_1>0 then
					for k=1, RecordNums_4_1 do
						local mRecordTabel_4_1={}
						ZyReader:recordBegin()
						mRecordTabel_4_1.TargetGeneralID=ZyReader:getInt()
						mRecordTabel_4_1.TargetGeneralLiveNum=ZyReader:getInt()
						mRecordTabel_4_1.TargetGeneralQishi=ZyReader:getWORD()
						mRecordTabel_4_1.TargetDamageNum=ZyReader:getInt()
						mRecordTabel_4_1.IsShanBi=ZyReader:getWORD()
						mRecordTabel_4_1.IsGeDang=ZyReader:getWORD()
						mRecordTabel_4_1.IsFangji=ZyReader:getWORD()
						mRecordTabel_4_1.IsMove=ZyReader:getWORD()
						mRecordTabel_4_1.FangjiDamageNum=ZyReader:getInt()
						mRecordTabel_4_1.TargetStatus=ZyReader:getWORD()
						mRecordTabel_4_1.Position=ZyReader:getWORD()
						mRecordTabel_4_1.Role=ZyReader:getWORD()
						-----内嵌循环 中招效果开始
						local RecordNums_4_1_0=ZyReader:getInt()
						local RecordTabel_4_1_0={}
						if RecordNums_4_1_0~=nil and RecordNums_4_1_0>0 then
							for k=1, RecordNums_4_1_0 do
								local mRecordTabel_4_1_0={}
								ZyReader:recordBegin()
								mRecordTabel_4_1_0.GeneralEffect=ZyReader:getWORD()
								mRecordTabel_4_1_0.IsIncrease=ZyReader:getInt()
								ZyReader:recordEnd()
								ZyTable.push_back(RecordTabel_4_1_0,mRecordTabel_4_1_0)
							end
						end
						mRecordTabel_4_1.GeneralEffects=RecordTabel_4_1_0
						mRecordTabel_4_1.IsBaoji = ZyReader:getWORD()
				
				
						-----触发技能
						local RecordNums_4_1_1=ZyReader:getInt()
						local RecordTabel_4_1_1={}
						if RecordNums_4_1_1~=nil and RecordNums_4_1_1>0 then
							for k=1, RecordNums_4_1_1 do
								local mRecordTabel_4_1_1={}
								ZyReader:recordBegin()
								mRecordTabel_4_1_1.TrumpAbility=ZyReader:getWORD()
								mRecordTabel_4_1_1.TrumpNum=ZyReader:getInt()
								ZyReader:recordEnd()
								ZyTable.push_back(RecordTabel_4_1_1,mRecordTabel_4_1_1)
							end
						end                  
						mRecordTabel_4_1.TriggerTable=RecordTabel_4_1_1
						ZyReader:recordEnd()
						ZyTable.push_back(RecordTabel_4_1,mRecordTabel_4_1)
					end
				end
				mRecordNums_4.DefendFightTable=RecordTabel_4_1
				
				-----触发技能
				local RecordNums_4_2=ZyReader:getInt()
				local RecordTabel_4_2={}
				if RecordNums_4_2~=nil and RecordNums_4_2>0 then
					for k=1, RecordNums_4_2 do
						local mRecordTabel_4_2={}
						ZyReader:recordBegin()
						mRecordTabel_4_2.TrumpAbility=ZyReader:getWORD()
						mRecordTabel_4_2.TrumpNum=ZyReader:getInt()
						ZyReader:recordEnd()
						ZyTable.push_back(RecordTabel_4_2,mRecordTabel_4_2)
					end
				end                  
				mRecordNums_4.TriggerTable=RecordTabel_4_2
	
				mRecordNums_4.FntHeadID= ZyReader:readString()
				mRecordNums_4.AbilityID= ZyReader:getInt()


				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_4,mRecordNums_4)
			end
		end 
		DataTabel.FightProcessTable=RecordTabel_4
		DataTabel.BlessExperience=ZyReader:getInt()
		DataTabel.GotoNum=ZyReader:getInt()
		    
		    

	---------自身魂技效果 
		local RecordNums_5=ZyReader:getInt()
		local RecordTabel_5={}
		if RecordNums_5~=nil and RecordNums_5>0 then
			for  k=1, RecordNums_5 do
				local mRecordNums_5={}
				ZyReader:recordBegin()
				mRecordNums_5.AttGeneralID=ZyReader:getInt()
				mRecordNums_5.EffectID1=ZyReader:readString()
				mRecordNums_5.FntHeadID=ZyReader:readString()
				mRecordNums_5.IsIncrease=ZyReader:getWORD()
				mRecordNums_5.Position=ZyReader:getInt()
				mRecordNums_5.Role=ZyReader:getInt()
				
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_5,mRecordNums_5)
			end
		end
		DataTabel.FirstEffectTabel=RecordTabel_5	
		
		DataTabel.UserTalPriority= ZyReader:getInt()
		DataTabel.NpcTalPriority= ZyReader:getInt()
		DataTabel.CurrentHonour= ZyReader:getInt()

		
	else
		ZyLoading.releaseAll()
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
	end
	return DataTabel
end

--3.5.1.5	离开副本接口（ID=4005）
function Action4005(Scene,isLoading ,PlotID)
   	ZyWriter:writeString("ActionId",4005)
   	ZyWriter:writeString("PlotID",PlotID)
	ZyExecRequest(Scene, nil,isLoading)
end

function _4005Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
    	DataTabel={}
    	DataTabel.CityID=ZyReader:getInt()
    	DataTabel.PointX=ZyReader:getWORD()
    	DataTabel.PointY=ZyReader:getWORD()
    else
    	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),0.8,0.35)
    end
    return DataTabel
  end


--        5101_竞技场列表接口（ID=5101）
function Action5101(Scene,isLoading )
	ZyWriter:writeString("ActionId",5101)
   	ZyExecRequest(Scene, nil,isLoading)
end

function _5101Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.nickName     = ZyReader:readString()--用户名称
		DataTabel.SportsName   = ZyReader:readString()--称号
		DataTabel.ObtianNum    = ZyReader:getInt()--声望
		DataTabel.Ranking      = ZyReader:getInt()--排名
		DataTabel.VictoryNum   = ZyReader:getInt()--连胜次数
		DataTabel.RewardGoin   = ZyReader:getInt()--奖励金币
		DataTabel.RewardObtian = ZyReader:getInt()--奖励声望
		DataTabel.ReceiveDate  = ZyReader:getInt()--领取时间
		DataTabel.ChallengeNum = ZyReader:getInt()--当日挑战次数
		DataTabel.CodeTime     = ZyReader:getInt()--冷却时间
		local RecordNums_1=ZyReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
				local mRecordTabel_1={}
				ZyReader:recordBegin()
				mRecordTabel_1.ToUserID   = ZyReader:readString()--用户ID
				mRecordTabel_1.UserName   = ZyReader:readString()--用户名称
				mRecordTabel_1.UserHeadID = ZyReader:readString()--用户头像
				mRecordTabel_1.UserRank   = ZyReader:getInt()--用户排名
				mRecordTabel_1.UserLv     = ZyReader:getWORD()--用户等级
				
				
				
				local RecordNums_1_1=ZyReader:getInt()
				local RecordTabel_1_1={}
				if RecordNums_1_1~=0 then
					for k=1,RecordNums_1_1 do
						local mRecordTabel_1_1={}
						ZyReader:recordBegin()
						mRecordTabel_1_1.GeneralID= ZyReader:getInt()
						mRecordTabel_1_1.GeneralName= ZyReader:readString()
						mRecordTabel_1_1.PicturesID= ZyReader:readString()
						mRecordTabel_1_1.GeneralQuality= ZyReader:getInt()
						mRecordTabel_1_1.GeneralLv= ZyReader:getInt()
						ZyReader:recordEnd()
						ZyTable.push_back(RecordTabel_1_1,mRecordTabel_1_1)
					end
				end
				mRecordTabel_1.RecordTabel = RecordTabel_1_1;
				mRecordTabel_1.embatListCount= ZyReader:getInt()
				mRecordTabel_1.embattleListCount= ZyReader:getInt()
				mRecordTabel_1.Reward= ZyReader:getInt()


				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
			end
		end
   		DataTabel._ToUserID = RecordTabel_1;
   		
   		
   		
		local RecordNums_2=ZyReader:getInt()
		local RecordTabel_2={}
		if RecordNums_2~=0 then
			for k=1,RecordNums_2 do
				local mRecordTabel_2={}
				ZyReader:recordBegin()
				mRecordTabel_2.ChallengeDate   = ZyReader:readString()--挑战时间
				mRecordTabel_2.ChallengeName   = ZyReader:readString()--挑战者
				mRecordTabel_2.BeChallengeName = ZyReader:readString()--被挑战者
				mRecordTabel_2.IsVictory       = ZyReader:getWORD()--战斗情况--1：战胜 2：战败
				mRecordTabel_2.RankStatus      = ZyReader:getInt()--排名情况0不变
				mRecordTabel_2.CurrTopID       = ZyReader:getInt()
				mRecordTabel_2.UserSportsID       = ZyReader:readString()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_2,mRecordTabel_2)
			end
		end
		DataTabel._ChallengeData = RecordTabel_2;

		local RecordNums_3=ZyReader:getInt()
		local RecordTabel_3={}
		if RecordNums_3~=0 then
			for k=1,RecordNums_3 do
				local mRecordTabel_3={}
				ZyReader:recordBegin()
				mRecordTabel_3.UserRank= ZyReader:getInt()
				mRecordTabel_3.ToUserID= ZyReader:readString()
				mRecordTabel_3.UserName= ZyReader:readString()
				mRecordTabel_3.UserLv= ZyReader:getWORD()
				mRecordTabel_3.PowerNum= ZyReader:getInt()
				mRecordTabel_3.SportsName= ZyReader:readString()
				mRecordTabel_3.VictoryNum= ZyReader:getInt()
				local RecordNums_3_1=ZyReader:getInt()
				local RecordTabel_3_1={}
				if RecordNums_3_1~=0 then
					for k=1,RecordNums_3_1 do
						local mRecordTabel_3_1={}
						ZyReader:recordBegin()
						mRecordTabel_3_1.GeneralID= ZyReader:getInt()
						mRecordTabel_3_1.GeneralName= ZyReader:readString()
						mRecordTabel_3_1.PicturesID= ZyReader:readString()
						mRecordTabel_3_1.GeneralQuality= ZyReader:getInt()
						mRecordTabel_3_1.GeneralLv= ZyReader:getInt()
						ZyReader:recordEnd()
						ZyTable.push_back(RecordTabel_3_1,mRecordTabel_3_1)
					end
				end
				mRecordTabel_3.RecordTabel = RecordTabel_3_1;
				mRecordTabel_3.embatListCount= ZyReader:getInt()
				mRecordTabel_3.embattleListCount= ZyReader:getInt()
				mRecordTabel_3.Reward= ZyReader:getInt()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_3,mRecordTabel_3)
			end
		end
		DataTabel.TopTenTabel = RecordTabel_3;


		DataTabel.sportsIntegral= ZyReader:getInt()
	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
	end
	return DataTabel
end

--        5102_竞技英雄列表接口（ID=5102）
function Action5102(Scene,isLoading )
	ZyWriter:writeString("ActionId",5102)
   	ZyExecRequest(Scene, nil,isLoading)
end

function _5102Callback(pScutScene, lpExternalData)
   	local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        local RecordNums_1=ZyReader:getInt()
      	local RecordTabel_1={}
 		if RecordNums_1~=0 then
  			for k=1,RecordNums_1 do
             	local mRecordTabel_1={}
             	ZyReader:recordBegin()
                mRecordTabel_1.RankNum= ZyReader:getInt()
                mRecordTabel_1.HeroID= ZyReader:readString()
                mRecordTabel_1.HeroName= ZyReader:readString()
                mRecordTabel_1.HeroLv= ZyReader:getWORD()
                mRecordTabel_1.PowerNum= ZyReader:getInt()
                mRecordTabel_1.Trend= ZyReader:getWORD()
                mRecordTabel_1.SportsName= ZyReader:readString()
                mRecordTabel_1.VictoryNum= ZyReader:getInt()
				ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
       		end
        end
   		DataTabel.RecordTabel = RecordTabel_1;
	else
ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end


---挑战奖励接口
function Action5103(Scene,isLoading ,ToUser)
	ZyWriter:writeString("ActionId",5103)
   	ZyWriter:writeString("ToUser ",ToUser )
	ZyExecRequest(Scene, nil,isLoading)
end

function _5103Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.RewardGoin = ZyReader:getInt()
        DataTabel.RewardObtian = ZyReader:getInt()
    else
	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        5104_增加挑战次数接口（ID=5104）
function Action5104(Scene,isLoading ,Ops)

   	ZyWriter:writeString("ActionId",5104)
   	ZyWriter:writeString("Ops",Ops)

	ZyExecRequest(Scene, nil,isLoading)
end

--        5105_消除竞技冷却时间接口（ID=5105）
function Action5105(Scene,isLoading ,Ops)
   	ZyWriter:writeString("ActionId",5105)
   	ZyWriter:writeString("Ops",Ops)
	ZyExecRequest(Scene, nil,isLoading)
end

function _5105Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
    else
ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end


----5106   排名奖励领取接口
function Action5106(Scene,isLoading)
	ZyWriter:writeString("ActionId",5106)
  	ZyExecRequest(Scene, nil,isLoading)
end

--        5107_竞技战斗详情接口（ID=5107）
function Action5107(Scene,isLoading ,ToUserID,UserSportsID)
   	ZyWriter:writeString("ActionId",5107)
   	ZyWriter:writeString("ToUserID",ToUserID)
   	ZyWriter:writeString("UserSportsID",UserSportsID)
	ZyExecRequest(Scene, nil,isLoading)
end

function _5107Callback(pScutScene, lpExternalData)
 	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.IsWin= ZyReader:getWORD()
		DataTabel.GameCoin= ZyReader:getInt()
		DataTabel.Obtion= ZyReader:getInt()
--攻击方阵型
		local RecordNums_1=ZyReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 and RecordNums_1>0 then
			for k=1,RecordNums_1 do
			local mRecordTabel_1={}
			ZyReader:recordBegin()
			mRecordTabel_1.AttGeneralID= ZyReader:getInt()
			mRecordTabel_1.AttGeneralName= ZyReader:readString()
			mRecordTabel_1.AttGeneralHeadID= ZyReader:readString()
			mRecordTabel_1.AttPosition= ZyReader:getWORD()
			mRecordTabel_1.LiveNum= ZyReader:getInt()
			mRecordTabel_1.LiveMaxNum= ZyReader:getInt()
			mRecordTabel_1.MomentumNum= ZyReader:getWORD()
			mRecordTabel_1.MaxMomentumNum= ZyReader:getWORD()
			mRecordTabel_1.AbilityID= ZyReader:getInt()
			mRecordTabel_1.GeneralLv= ZyReader:getWORD()
		    	mRecordTabel_1.IsAttReplace= ZyReader:getWORD()
		    	mRecordTabel_1.AttGeneralQuality= ZyReader:getWORD()
			ZyReader:recordEnd()
			ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
		end
        end
                DataTabel.AgainstTable = RecordTabel_1;
    --防守方阵型
		local RecordNums_2=ZyReader:getInt()
		local RecordTabel_2={}
		if RecordNums_2~=0 and RecordNums_2>0 then
		for k=1,RecordNums_2 do
			local mRecordTabel_2={}
			ZyReader:recordBegin()
			mRecordTabel_2.AttGeneralID= ZyReader:getInt()
			mRecordTabel_2.AttGeneralName= ZyReader:readString()
			mRecordTabel_2.AttGeneralHeadID= ZyReader:readString()
			mRecordTabel_2.AttPosition= ZyReader:getWORD()
			mRecordTabel_2.LiveNum= ZyReader:getInt()
			mRecordTabel_2.LiveMaxNum= ZyReader:getInt()
			mRecordTabel_2.MomentumNum= ZyReader:getWORD()
			mRecordTabel_2.MaxMomentumNum= ZyReader:getWORD()
			mRecordTabel_2.AbilityID= ZyReader:getInt()
			mRecordTabel_2.GeneralLv= ZyReader:getWORD()
		    	mRecordTabel_2.IsAttReplace= ZyReader:getWORD()
		    	mRecordTabel_2.AttGeneralQuality= ZyReader:getWORD()
			ZyReader:recordEnd()
			ZyTable.push_back(RecordTabel_2,mRecordTabel_2)
              end
        end
		DataTabel.DefendingTable = RecordTabel_2;
		-----------------战斗过程
		local RecordNums_4=ZyReader:getInt()
		 local RecordTabel_4={}
		if RecordNums_4~=nil and RecordNums_4>0 then
		    		for  k=1, RecordNums_4 do
		    			local mRecordNums_4={}
		    			ZyReader:recordBegin()
		    			mRecordNums_4.AttGeneralID=ZyReader:getInt()
		    			mRecordNums_4.AttGeneralLiveNum=ZyReader:getInt()
		    			mRecordNums_4.AttGeneralQishi=ZyReader:getWORD()
		    			mRecordNums_4.AttackTaget=ZyReader:getWORD()
		    			mRecordNums_4.AttackType=ZyReader:getWORD()
		    			mRecordNums_4.AbilityProperty=ZyReader:getWORD()
		    			mRecordNums_4.AttGeneralStatus=ZyReader:getWORD()
		    			mRecordNums_4.BackDamage=ZyReader:getInt()
		    			mRecordNums_4.AttEffectID=ZyReader:readString()
		    			mRecordNums_4.TargetEffectID=ZyReader:readString()
		    			mRecordNums_4.IsMove=ZyReader:getWORD()
		    			mRecordNums_4.Position=ZyReader:getWORD()
		    			mRecordNums_4.Role=ZyReader:getWORD()

		    			-----内嵌循环 中招效果开始
		    			local RecordNums_4_0=ZyReader:getInt()
					local RecordTabel_4_0={}
					if RecordNums_4_0~=nil and RecordNums_4_0>0 then
			    			for k=1, RecordNums_4_0 do
			    			local mRecordTabel_4_0={}
			    			ZyReader:recordBegin()
			    			mRecordTabel_4_0.GeneralEffect=ZyReader:getWORD()
			    			mRecordTabel_4_0.ConDamageNum=ZyReader:getInt()
			    			mRecordTabel_4_0.IsIncrease=ZyReader:getInt()
			    			ZyReader:recordEnd()
			    			ZyTable.push_back(RecordTabel_4_0,mRecordTabel_4_0)
						end
					end
		    			mRecordNums_4.GeneralEffects=RecordTabel_4_0
		    			-----内嵌循环 受到攻击的角色
		    			local RecordNums_4_1=ZyReader:getInt()
					local RecordTabel_4_1={}
					if RecordNums_4_1~=nil and RecordNums_4_1>0 then
			    			for k=1, RecordNums_4_1 do
			    			local mRecordTabel_4_1={}
			    			ZyReader:recordBegin()
			    			mRecordTabel_4_1.TargetGeneralID=ZyReader:getInt()
			    			mRecordTabel_4_1.TargetGeneralLiveNum=ZyReader:getInt()
			    			mRecordTabel_4_1.TargetGeneralQishi=ZyReader:getWORD()
			    			mRecordTabel_4_1.TargetDamageNum=ZyReader:getInt()
			    			mRecordTabel_4_1.IsShanBi=ZyReader:getWORD()
			    			mRecordTabel_4_1.IsGeDang=ZyReader:getWORD()
			    			mRecordTabel_4_1.IsFangji=ZyReader:getWORD()
			    			mRecordTabel_4_1.IsMove=ZyReader:getWORD()
			    			mRecordTabel_4_1.FangjiDamageNum=ZyReader:getInt()
			    			mRecordTabel_4_1.TargetStatus=ZyReader:getWORD()
			    			mRecordTabel_4_1.Position=ZyReader:getWORD()
		    				mRecordTabel_4_1.Role=ZyReader:getWORD()
				    	-----内嵌循环 中招效果开始
			    			local RecordNums_4_1_0=ZyReader:getInt()
						local RecordTabel_4_1_0={}
						    if RecordNums_4_1_0~=nil and RecordNums_4_1_0>0 then
				    			for k=1, RecordNums_4_1_0 do
				    			    local mRecordTabel_4_1_0={}
				    			    ZyReader:recordBegin()
				    			    mRecordTabel_4_1_0.GeneralEffect=ZyReader:getWORD()
				    			    mRecordTabel_4_1_0.IsIncrease=ZyReader:getInt()
				    			    ZyReader:recordEnd()
				    			    ZyTable.push_back(RecordTabel_4_1_0,mRecordTabel_4_1_0)
							    end
						    end
			    			mRecordTabel_4_1.GeneralEffects=RecordTabel_4_1_0
			    			mRecordTabel_4_1.IsBaoji = ZyReader:getWORD()
			    			
			    	
						-----触发技能
						local RecordNums_4_1_1=ZyReader:getInt()
						local RecordTabel_4_1_1={}
						if RecordNums_4_1_1~=nil and RecordNums_4_1_1>0 then
							for k=1, RecordNums_4_1_1 do
								local mRecordTabel_4_1_1={}
								ZyReader:recordBegin()
								mRecordTabel_4_1_1.TrumpAbility=ZyReader:getWORD()
								mRecordTabel_4_1_1.TrumpNum=ZyReader:getInt()
								ZyReader:recordEnd()
								ZyTable.push_back(RecordTabel_4_1_1,mRecordTabel_4_1_1)
							end
						end                  
						mRecordTabel_4_1.TriggerTable=RecordTabel_4_1_1
			    			ZyReader:recordEnd()
			    			ZyTable.push_back(RecordTabel_4_1,mRecordTabel_4_1)
						end
					end
	                    mRecordNums_4.DefendFightTable=RecordTabel_4_1
	                    
				-----触发技能
				local RecordNums_4_2=ZyReader:getInt()
				local RecordTabel_4_2={}
				if RecordNums_4_2~=nil and RecordNums_4_2>0 then
					for k=1, RecordNums_4_2 do
						local mRecordTabel_4_2={}
						ZyReader:recordBegin()
						mRecordTabel_4_2.TrumpAbility=ZyReader:getWORD()
						mRecordTabel_4_2.TrumpNum=ZyReader:getInt()
						ZyReader:recordEnd()
						ZyTable.push_back(RecordTabel_4_2,mRecordTabel_4_2)
					end
				end                  
				mRecordNums_4.TriggerTable=RecordTabel_4_2
				mRecordNums_4.FntHeadID= ZyReader:readString()
		
	                    ZyReader:recordEnd()
	                    ZyTable.push_back(RecordTabel_4,mRecordNums_4)
                	end
		  end
		DataTabel.FightProcessTable = RecordTabel_4;
		
		local RecordNums_5=ZyReader:getInt()
		local RecordTabel_5={}
		if RecordNums_5~=0 then
			for k=1,RecordNums_5 do
				local mRecordTabel_5={}
				ZyReader:recordBegin()
				mRecordTabel_5.GeneralID= ZyReader:getInt()
				mRecordTabel_5.EffectID1= ZyReader:readString()
				mRecordTabel_5.FntHeadID= ZyReader:readString()
				mRecordTabel_5.IsIncrease= ZyReader:getWORD()
				mRecordTabel_5.Position= ZyReader:getInt()
				mRecordTabel_5.Role= ZyReader:getInt()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_5,mRecordTabel_5)
			end
		end
		DataTabel.FirstEffectTabel = RecordTabel_5;
		
		DataTabel.UserTalPriority= ZyReader:getInt()
		DataTabel.NpcTalPriority= ZyReader:getInt()
		DataTabel.SportsPrizeStr= ZyReader:readString()
		
	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
	end
    return DataTabel
end

--        5108_竞技战斗详情接口（ID=5108）
function Action5108(Scene,isLoading ,ToUserID,MailID)
   	ZyWriter:writeString("ActionId",5108)
   	ZyWriter:writeString("ToUserID",ToUserID)
   	ZyWriter:writeString("MailID",MailID)
	ZyExecRequest(Scene, nil,isLoading)
end

function _5108Callback(pScutScene, lpExternalData)
 	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.IsWin= ZyReader:getWORD()
--		DataTabel.GameCoin= ZyReader:getInt()
--		DataTabel.Obtion= ZyReader:getInt()
--攻击方阵型
		local RecordNums_1=ZyReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 and RecordNums_1>0 then
			for k=1,RecordNums_1 do
			local mRecordTabel_1={}
			ZyReader:recordBegin()
			mRecordTabel_1.AttGeneralID= ZyReader:getInt()
			mRecordTabel_1.AttGeneralName= ZyReader:readString()
			mRecordTabel_1.AttGeneralHeadID= ZyReader:readString()
			mRecordTabel_1.AttPosition= ZyReader:getWORD()
			mRecordTabel_1.LiveNum= ZyReader:getInt()
			mRecordTabel_1.LiveMaxNum= ZyReader:getInt()
			mRecordTabel_1.MomentumNum= ZyReader:getWORD()
			mRecordTabel_1.MaxMomentumNum= ZyReader:getWORD()
			mRecordTabel_1.AbilityID= ZyReader:getInt()
			mRecordTabel_1.GeneralLv= ZyReader:getWORD()
		    	mRecordTabel_1.IsAttReplace= ZyReader:getWORD()
		    	mRecordTabel_1.AttGeneralQuality= ZyReader:getWORD()
			ZyReader:recordEnd()
			ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
		end
        end
                DataTabel.AgainstTable = RecordTabel_1;
    --防守方阵型
		local RecordNums_2=ZyReader:getInt()
		local RecordTabel_2={}
		if RecordNums_2~=0 and RecordNums_2>0 then
		for k=1,RecordNums_2 do
			local mRecordTabel_2={}
			ZyReader:recordBegin()
			mRecordTabel_2.AttGeneralID= ZyReader:getInt()
			mRecordTabel_2.AttGeneralName= ZyReader:readString()
			mRecordTabel_2.AttGeneralHeadID= ZyReader:readString()
			mRecordTabel_2.AttPosition= ZyReader:getWORD()
			mRecordTabel_2.LiveNum= ZyReader:getInt()
			mRecordTabel_2.LiveMaxNum= ZyReader:getInt()
			mRecordTabel_2.MomentumNum= ZyReader:getWORD()
			mRecordTabel_2.MaxMomentumNum= ZyReader:getWORD()
			mRecordTabel_2.AbilityID= ZyReader:getInt()
			mRecordTabel_2.GeneralLv= ZyReader:getWORD()
		    	mRecordTabel_2.IsAttReplace= ZyReader:getWORD()
		    	mRecordTabel_2.AttGeneralQuality= ZyReader:getWORD()
			ZyReader:recordEnd()
			ZyTable.push_back(RecordTabel_2,mRecordTabel_2)
              end
        end
		DataTabel.DefendingTable = RecordTabel_2;
		-----------------战斗过程
		local RecordNums_4=ZyReader:getInt()
		 local RecordTabel_4={}
		if RecordNums_4~=nil and RecordNums_4>0 then
		    		for  k=1, RecordNums_4 do
		    			local mRecordNums_4={}
		    			ZyReader:recordBegin()
		    			mRecordNums_4.AttGeneralID=ZyReader:getInt()
		    			mRecordNums_4.AttGeneralLiveNum=ZyReader:getInt()
		    			mRecordNums_4.AttGeneralQishi=ZyReader:getWORD()
		    			mRecordNums_4.AttackTaget=ZyReader:getWORD()
		    			mRecordNums_4.AttackType=ZyReader:getWORD()
		    			mRecordNums_4.AbilityProperty=ZyReader:getWORD()
		    			mRecordNums_4.AttGeneralStatus=ZyReader:getWORD()
		    			mRecordNums_4.BackDamage=ZyReader:getInt()
		    			mRecordNums_4.AttEffectID=ZyReader:readString()
		    			mRecordNums_4.TargetEffectID=ZyReader:readString()
		    			mRecordNums_4.IsMove=ZyReader:getWORD()
		    			mRecordNums_4.Position=ZyReader:getWORD()
		    			mRecordNums_4.Role=ZyReader:getWORD()

		    			-----内嵌循环 中招效果开始
		    			local RecordNums_4_0=ZyReader:getInt()
					local RecordTabel_4_0={}
					if RecordNums_4_0~=nil and RecordNums_4_0>0 then
			    			for k=1, RecordNums_4_0 do
			    			local mRecordTabel_4_0={}
			    			ZyReader:recordBegin()
			    			mRecordTabel_4_0.GeneralEffect=ZyReader:getWORD()
			    			mRecordTabel_4_0.ConDamageNum=ZyReader:getInt()
			    			mRecordTabel_4_0.IsIncrease=ZyReader:getInt()
			    			ZyReader:recordEnd()
			    			ZyTable.push_back(RecordTabel_4_0,mRecordTabel_4_0)
						end
					end
		    			mRecordNums_4.GeneralEffects=RecordTabel_4_0
		    			-----内嵌循环 受到攻击的角色
		    			local RecordNums_4_1=ZyReader:getInt()
					local RecordTabel_4_1={}
					if RecordNums_4_1~=nil and RecordNums_4_1>0 then
			    			for k=1, RecordNums_4_1 do
			    			local mRecordTabel_4_1={}
			    			ZyReader:recordBegin()
			    			mRecordTabel_4_1.TargetGeneralID=ZyReader:getInt()
			    			mRecordTabel_4_1.TargetGeneralLiveNum=ZyReader:getInt()
			    			mRecordTabel_4_1.TargetGeneralQishi=ZyReader:getWORD()
			    			mRecordTabel_4_1.TargetDamageNum=ZyReader:getInt()
			    			mRecordTabel_4_1.IsShanBi=ZyReader:getWORD()
			    			mRecordTabel_4_1.IsGeDang=ZyReader:getWORD()
			    			mRecordTabel_4_1.IsFangji=ZyReader:getWORD()
			    			mRecordTabel_4_1.IsMove=ZyReader:getWORD()
			    			mRecordTabel_4_1.FangjiDamageNum=ZyReader:getInt()
			    			mRecordTabel_4_1.TargetStatus=ZyReader:getWORD()
			    			mRecordTabel_4_1.Position=ZyReader:getWORD()
		    				mRecordTabel_4_1.Role=ZyReader:getWORD()
				    	-----内嵌循环 中招效果开始
			    			local RecordNums_4_1_0=ZyReader:getInt()
						local RecordTabel_4_1_0={}
						    if RecordNums_4_1_0~=nil and RecordNums_4_1_0>0 then
				    			for k=1, RecordNums_4_1_0 do
				    			    local mRecordTabel_4_1_0={}
				    			    ZyReader:recordBegin()
				    			    mRecordTabel_4_1_0.GeneralEffect=ZyReader:getWORD()
				    			    mRecordTabel_4_1_0.IsIncrease=ZyReader:getInt()
				    			    ZyReader:recordEnd()
				    			    ZyTable.push_back(RecordTabel_4_1_0,mRecordTabel_4_1_0)
							    end
						    end
			    			mRecordTabel_4_1.GeneralEffects=RecordTabel_4_1_0
			    			mRecordTabel_4_1.IsBaoji = ZyReader:getWORD()
			    			
			    	
						-----触发技能
						local RecordNums_4_1_1=ZyReader:getInt()
						local RecordTabel_4_1_1={}
						if RecordNums_4_1_1~=nil and RecordNums_4_1_1>0 then
							for k=1, RecordNums_4_1_1 do
								local mRecordTabel_4_1_1={}
								ZyReader:recordBegin()
								mRecordTabel_4_1_1.TrumpAbility=ZyReader:getWORD()
								mRecordTabel_4_1_1.TrumpNum=ZyReader:getInt()
								ZyReader:recordEnd()
								ZyTable.push_back(RecordTabel_4_1_1,mRecordTabel_4_1_1)
							end
						end                  
						mRecordTabel_4_1.TriggerTable=RecordTabel_4_1_1
			    			ZyReader:recordEnd()
			    			ZyTable.push_back(RecordTabel_4_1,mRecordTabel_4_1)
						end
					end
	                    mRecordNums_4.DefendFightTable=RecordTabel_4_1
	                    
				-----触发技能
				local RecordNums_4_2=ZyReader:getInt()
				local RecordTabel_4_2={}
				if RecordNums_4_2~=nil and RecordNums_4_2>0 then
					for k=1, RecordNums_4_2 do
						local mRecordTabel_4_2={}
						ZyReader:recordBegin()
						mRecordTabel_4_2.TrumpAbility=ZyReader:getWORD()
						mRecordTabel_4_2.TrumpNum=ZyReader:getInt()
						ZyReader:recordEnd()
						ZyTable.push_back(RecordTabel_4_2,mRecordTabel_4_2)
					end
				end                  
				mRecordNums_4.TriggerTable=RecordTabel_4_2
				mRecordNums_4.FntHeadID= ZyReader:readString()
				DataTabel.AbilityID= ZyReader:getInt()
		
	                    ZyReader:recordEnd()
	                    ZyTable.push_back(RecordTabel_4,mRecordNums_4)
                	end
		  end
		DataTabel.FightProcessTable = RecordTabel_4;
		
		local RecordNums_5=ZyReader:getInt()
		local RecordTabel_5={}
		if RecordNums_5~=0 then
			for k=1,RecordNums_5 do
				local mRecordTabel_5={}
				ZyReader:recordBegin()
				mRecordTabel_5.GeneralID= ZyReader:getInt()
				mRecordTabel_5.EffectID1= ZyReader:readString()
				mRecordTabel_5.FntHeadID= ZyReader:readString()
				mRecordTabel_5.IsIncrease= ZyReader:getWORD()
				mRecordTabel_5.Position= ZyReader:getInt()
				mRecordTabel_5.Role= ZyReader:getInt()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_5,mRecordTabel_5)
			end
		end
		DataTabel.FirstEffectTabel = RecordTabel_5;
		
		DataTabel.UserTalPriority= ZyReader:getInt()
		DataTabel.NpcTalPriority= ZyReader:getInt()
--		DataTabel.SportsPrizeStr= ZyReader:readString()
		DataTabel.GameCoin= ZyReader:getInt()
		DataTabel.Obtion= ZyReader:getInt()
		
	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
	end
    return DataTabel
end

----boss战 参加接口
function Action5401(Scene,isLoading,PageIndex ,PageSize,ActiveId)
	ZyWriter:writeString("ActionId",5401)
	ZyWriter:writeString("PageIndex",PageIndex )
	ZyWriter:writeString("PageSize",PageSize)
	ZyWriter:writeString("ActiveId",ActiveId)
	ZyExecRequest(Scene, nil,isLoading)
end

function _5401Callback(pScutScene, lpExternalData)
	 local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.PlotID   =  ZyReader:getInt()
		DataTabel.BossLv   =  ZyReader:getWORD()
		DataTabel.ColdTime   =  ZyReader:getInt()
		DataTabel.RegNum   =  ZyReader:getInt()
		DataTabel.CombatStatus   =  ZyReader:getWORD()
		DataTabel.InspireNum     =  ZyReader:getInt()
		DataTabel.ReLiveNum   =  ZyReader:getInt()
		
		
		local RecordNums_1 =  ZyReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
			local mRecordTabel_1={}
			ZyReader:recordBegin()
			mRecordTabel_1.UserName    = ZyReader:readString()
			mRecordTabel_1.HeadID    = ZyReader:readString()
			mRecordTabel_1.UserStatus    = ZyReader:getInt()
			mRecordTabel_1.CodeTime    = ZyReader:getInt()
			mRecordTabel_1.IsWing    = ZyReader:getWORD()
			mRecordTabel_1.IsChampion    = ZyReader:getInt()
		
			ZyReader:recordEnd()
			ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.PlayerTabel = RecordTabel_1
		
		
		DataTabel.reliveInspirePercent   =  ZyReader:getInt()
		DataTabel.GlodNum =  ZyReader:getInt()
		DataTabel.InspirePercent =  ZyReader:getInt()
		DataTabel.BackGoldNum =  ZyReader:getInt()
		DataTabel.combatNum =  ZyReader:getInt()
		DataTabel.damageNum =  ZyReader:getInt()
		DataTabel.codeTime  =  ZyReader:getInt()
		
		DataTabel.BossLiftNum= ZyReader:getInt()
		DataTabel.BossMaxLift= ZyReader:getInt()

	end
    return DataTabel
end





---boss战鼓舞接口
function Action5402(Scene,isLoading,Ops,ActiveId)
	ZyWriter:writeString("ActionId",5402)
	ZyWriter:writeString("Ops",Ops )
	ZyWriter:writeString("ActiveId",ActiveId)
	ZyExecRequest(Scene, nil,isLoading)
end
function _5402Callback(pScutScene, lpExternalData)
	 local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.InspireNum    =  ZyReader:getInt()
	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
	end
    return DataTabel
end



---boss战复活接口
function Action5403(Scene,isLoading,Ops,ActiveId)
	ZyWriter:writeString("ActionId",5403)
	ZyWriter:writeString("Ops",Ops)
	ZyWriter:writeString("ActiveId",ActiveId)
	ZyExecRequest(Scene, nil,isLoading)
end
function _5403Callback(pScutScene, lpExternalData)
	 local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.ReliveInspirePercent =  ZyReader:getInt()
	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
	end
    return DataTabel
end


---boss战血量刷新接口
function Action5404(Scene,isLoading,ActiveId)
	ZyWriter:writeString("ActionId",5404)
	ZyWriter:writeString("ActiveId",ActiveId)
	ZyExecRequest(Scene, nil,isLoading)
end
function _5404Callback(pScutScene, lpExternalData)
	 local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.CurrLifeNum =  ZyReader:getInt()
		DataTabel.MaxLifeNum =  ZyReader:getInt()
	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
	end
    return DataTabel
end

----boss战战斗详情接口
function Action5405(Scene,isLoading,ActiveId)
	ZyWriter:writeString("ActionId",5405)
	ZyWriter:writeString("ActiveId",ActiveId)
	ZyExecRequest(Scene, nil,isLoading)
end
function _5405Callback(pScutScene, lpExternalData)
	 local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.IsWin  =  ZyReader:getWORD()
		DataTabel.DamageNum  =  ZyReader:getInt()
		DataTabel.ObtainNum  =  ZyReader:getInt()
		DataTabel.GameCoin  =  ZyReader:getInt()
		DataTabel.KillGameCoin  =  ZyReader:getInt()
		
		-------------攻防阵型
		local RecordNums_2=ZyReader:getInt()
		local RecordTabel_2={}
		if RecordNums_2~=nil and RecordNums_2>0 then
		for  k=1, RecordNums_2 do
		local mRecordNums_2={}
		ZyReader:recordBegin()
		mRecordNums_2.AttGeneralID=ZyReader:getInt()
		mRecordNums_2.AttGeneralName=ZyReader:readString()
		mRecordNums_2.AttGeneralHeadID=ZyReader:readString()
		mRecordNums_2.AttPosition=ZyReader:getWORD()
		mRecordNums_2.LiveNum=ZyReader:getInt()
		mRecordNums_2.LiveMaxNum=ZyReader:getInt()
		mRecordNums_2.MomentumNum=ZyReader:getWORD()
		mRecordNums_2.MaxMomentumNum=ZyReader:getWORD()
		mRecordNums_2.AbilityID=ZyReader:getInt()
		mRecordNums_2.GeneralLv=ZyReader:getWORD()
		mRecordNums_2.IsAttReplace= ZyReader:getWORD()
		mRecordNums_2.AttGeneralQuality= ZyReader:getWORD()
		ZyReader:recordEnd()
		ZyTable.push_back(RecordTabel_2,mRecordNums_2)
		end
		end
		DataTabel.AgainstTable=RecordTabel_2
		    
		
		-------------防守阵型
		local RecordNums_3=ZyReader:getInt()
		local RecordTabel_3={}
		if RecordNums_3~=nil and RecordNums_3>0 then
			for  k=1, RecordNums_3 do
				local mRecordNums_3={}
				ZyReader:recordBegin()
				mRecordNums_3.AttGeneralID=ZyReader:getInt()
				mRecordNums_3.AttGeneralName=ZyReader:readString()
				mRecordNums_3.AttGeneralHeadID=ZyReader:readString()
				mRecordNums_3.AttPosition=ZyReader:getWORD()
				mRecordNums_3.LiveNum=ZyReader:getInt()
				mRecordNums_3.LiveMaxNum=ZyReader:getInt()
				mRecordNums_3.MomentumNum=ZyReader:getWORD()
				mRecordNums_3.MaxMomentumNum=ZyReader:getWORD()
				mRecordNums_3.AbilityID=ZyReader:getInt()
				mRecordNums_3.GeneralLv=ZyReader:getWORD()
				mRecordNums_3.IsAttReplace= ZyReader:getWORD()
				mRecordNums_3.AttGeneralQuality= ZyReader:getWORD()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_3,mRecordNums_3)
			end
		end
		DataTabel.DefendingTable=RecordTabel_3
		    
		    
		
		-----------------战斗过程
		local RecordNums_4=ZyReader:getInt()
		local RecordTabel_4={}
		if RecordNums_4~=nil and RecordNums_4>0 then
			for  k=1, RecordNums_4 do
				local mRecordNums_4={}
				ZyReader:recordBegin()
				mRecordNums_4.AttGeneralID=ZyReader:getInt()
				mRecordNums_4.AttGeneralLiveNum=ZyReader:getInt()
				mRecordNums_4.AttGeneralQishi=ZyReader:getWORD()
				mRecordNums_4.AttackTaget=ZyReader:getWORD()
				mRecordNums_4.AttackType=ZyReader:getWORD()
				mRecordNums_4.AbilityProperty=ZyReader:getWORD()
				mRecordNums_4.AttGeneralStatus=ZyReader:getWORD()
				mRecordNums_4.BackDamage=ZyReader:getInt()
				mRecordNums_4.AttEffectID=ZyReader:readString()
				mRecordNums_4.TargetEffectID=ZyReader:readString()
				mRecordNums_4.IsMove=ZyReader:getWORD()
				mRecordNums_4.Position=ZyReader:getWORD()
				mRecordNums_4.Role=ZyReader:getWORD()
			
				-----内嵌循环 中招效果开始
				local RecordNums_4_0=ZyReader:getInt()
				local RecordTabel_4_0={}
				if RecordNums_4_0~=nil and RecordNums_4_0>0 then
					for k=1, RecordNums_4_0 do
						local mRecordTabel_4_0={}
						ZyReader:recordBegin()
						mRecordTabel_4_0.GeneralEffect=ZyReader:getWORD()
						mRecordTabel_4_0.ConDamageNum=ZyReader:getInt()
						mRecordTabel_4_0.IsIncrease=ZyReader:getInt()
						ZyReader:recordEnd()
						ZyTable.push_back(RecordTabel_4_0,mRecordTabel_4_0)
					end
				end
				mRecordNums_4.GeneralEffects=RecordTabel_4_0
			
				-----内嵌循环 受到攻击的角色
				local RecordNums_4_1=ZyReader:getInt()
				local RecordTabel_4_1={}
				if RecordNums_4_1~=nil and RecordNums_4_1>0 then
					for k=1, RecordNums_4_1 do
						local mRecordTabel_4_1={}
						ZyReader:recordBegin()
						mRecordTabel_4_1.TargetGeneralID=ZyReader:getInt()
						mRecordTabel_4_1.TargetGeneralLiveNum=ZyReader:getInt()
						mRecordTabel_4_1.TargetGeneralQishi=ZyReader:getWORD()
						mRecordTabel_4_1.TargetDamageNum=ZyReader:getInt()
						mRecordTabel_4_1.IsShanBi=ZyReader:getWORD()
						mRecordTabel_4_1.IsGeDang=ZyReader:getWORD()
						mRecordTabel_4_1.IsFangji=ZyReader:getWORD()
						mRecordTabel_4_1.IsMove=ZyReader:getWORD()
						mRecordTabel_4_1.FangjiDamageNum=ZyReader:getInt()
						mRecordTabel_4_1.TargetStatus=ZyReader:getWORD()
						mRecordTabel_4_1.Position=ZyReader:getWORD()
						mRecordTabel_4_1.Role=ZyReader:getWORD()
						-----内嵌循环 中招效果开始
						local RecordNums_4_1_0=ZyReader:getInt()
						local RecordTabel_4_1_0={}
						if RecordNums_4_1_0~=nil and RecordNums_4_1_0>0 then
							for k=1, RecordNums_4_1_0 do
								local mRecordTabel_4_1_0={}
								ZyReader:recordBegin()
								mRecordTabel_4_1_0.GeneralEffect=ZyReader:getWORD()
								mRecordTabel_4_1_0.IsIncrease=ZyReader:getInt()
								ZyReader:recordEnd()
								ZyTable.push_back(RecordTabel_4_1_0,mRecordTabel_4_1_0)
							end
						end
						mRecordTabel_4_1.GeneralEffects=RecordTabel_4_1_0
						mRecordTabel_4_1.IsBaoji = ZyReader:getWORD()
				
				
						-----触发技能
						local RecordNums_4_1_1=ZyReader:getInt()
						local RecordTabel_4_1_1={}
						if RecordNums_4_1_1~=nil and RecordNums_4_1_1>0 then
							for k=1, RecordNums_4_1_1 do
								local mRecordTabel_4_1_1={}
								ZyReader:recordBegin()
								mRecordTabel_4_1_1.TrumpAbility=ZyReader:getWORD()
								mRecordTabel_4_1_1.TrumpNum=ZyReader:getInt()
								ZyReader:recordEnd()
								ZyTable.push_back(RecordTabel_4_1_1,mRecordTabel_4_1_1)
							end
						end                  
						mRecordTabel_4_1.TriggerTable=RecordTabel_4_1_1
						ZyReader:recordEnd()
						ZyTable.push_back(RecordTabel_4_1,mRecordTabel_4_1)
					end
				end
				mRecordNums_4.DefendFightTable=RecordTabel_4_1
				
				-----触发技能
				local RecordNums_4_2=ZyReader:getInt()
				local RecordTabel_4_2={}
				if RecordNums_4_2~=nil and RecordNums_4_2>0 then
					for k=1, RecordNums_4_2 do
						local mRecordTabel_4_2={}
						ZyReader:recordBegin()
						mRecordTabel_4_2.TrumpAbility=ZyReader:getWORD()
						mRecordTabel_4_2.TrumpNum=ZyReader:getInt()
						ZyReader:recordEnd()
						ZyTable.push_back(RecordTabel_4_2,mRecordTabel_4_2)
					end
				end                  
				mRecordNums_4.TriggerTable=RecordTabel_4_2
	
				mRecordNums_4.FntHeadID= ZyReader:readString()


				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_4,mRecordNums_4)
			end
		end 
		DataTabel.FightProcessTable=RecordTabel_4

		    
		    

	---------自身魂技效果 
		local RecordNums_5=ZyReader:getInt()
		local RecordTabel_5={}
		if RecordNums_5~=nil and RecordNums_5>0 then
			for  k=1, RecordNums_5 do
				local mRecordNums_3={}
				local mRecordNums_5={}
				ZyReader:recordBegin()
				mRecordNums_5.AttGeneralID=ZyReader:getInt()
				mRecordNums_5.EffectID1=ZyReader:readString()
				mRecordNums_5.FntHeadID=ZyReader:readString()
				mRecordNums_5.IsIncrease=ZyReader:getWORD()
				mRecordNums_5.Position=ZyReader:getInt()
				mRecordNums_5.Role=ZyReader:getInt()
				
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_5,mRecordNums_5)
			end
		end
		DataTabel.FirstEffectTabel=RecordTabel_5	
		
		DataTabel.UserTalPriority= ZyReader:getInt()
		DataTabel.NpcTalPriority= ZyReader:getInt()

		
		
		
	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
	end
    return DataTabel
end

----boss战伤害排名接口

function Action5406(Scene,isLoading,ActiveId,IsCurr)	
	ZyWriter:writeString("ActionId",5406)
	ZyWriter:writeString("ActiveId",ActiveId)
	ZyWriter:writeString("IsCurr",IsCurr)
	ZyExecRequest(Scene, IsCurr,isLoading)
end


function _5406Callback(pScutScene, lpExternalData)
	 local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.RankingNo =  ZyReader:getInt()
		local RecordNums_1=ZyReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
			local mRecordTabel_1={}
			ZyReader:recordBegin()
			mRecordTabel_1.UserID  = ZyReader:readString()
			mRecordTabel_1.UserName  = ZyReader:readString()
			mRecordTabel_1.DamageNum  = ZyReader:getInt()
			mRecordTabel_1.UserLv= ZyReader:getWORD()
			ZyReader:recordEnd()
			ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.RecordTabel = RecordTabel_1
		DataTabel._damageNum  =  ZyReader:getInt()
		DataTabel._nickName   =  ZyReader:readString()
		DataTabel._userLv   =  ZyReader:getInt()
	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
	end
    return DataTabel
end 


----boss战退出接口
function Action5407(Scene,isLoading,ActiveId)
	ZyWriter:writeString("ActionId",5407)
	ZyWriter:writeString("ActiveId",ActiveId)
	ZyExecRequest(Scene, nil,isLoading)
end

--- 世界boss列表
function Action5408(Scene,isLoading,FunctionEnum)	
	ZyWriter:writeString("ActionId",5408)
	ZyWriter:writeString("FunctionEnum",FunctionEnum)
	ZyExecRequest(Scene, nil,isLoading)
end


function _5408Callback(pScutScene, lpExternalData)
	 local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		local RecordNums_1=ZyReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
			local mRecordTabel_1={}
			ZyReader:recordBegin()
			mRecordTabel_1.ActiveId = ZyReader:getInt()
			mRecordTabel_1.ActiveName = ZyReader:readString()
			mRecordTabel_1.HeadID = ZyReader:readString()
			mRecordTabel_1.Descption = ZyReader:readString()
			mRecordTabel_1.BossLv  = ZyReader:getWORD()
			mRecordTabel_1.EnablePeriod  = ZyReader:readString()
			ZyReader:recordEnd()
			ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.RecordTabel = RecordTabel_1
	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
	end
    return DataTabel
end 


--        7001_商店物品列表接口（ID=7001）
function Action7001(Scene,isLoading ,MallType,PageIndex,PageSize)
	ZyWriter:writeString("ActionId",7001)
	ZyWriter:writeString("MallType",MallType)
	ZyWriter:writeString("PageIndex",PageIndex)
	ZyWriter:writeString("PageSize",PageSize)
	ZyExecRequest(Scene, nil,isLoading)
end

function _7001Callback(pScutScene, lpExternalData)
    local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.PageCount= ZyReader:getInt()
		DataTabel.GameCoin= ZyReader:getInt()
		DataTabel.GoldNum= ZyReader:getInt()
		local RecordNums_1=ZyReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
			local mRecordTabel_1={}
			ZyReader:recordBegin()
			mRecordTabel_1.ItemID= ZyReader:getInt()
			mRecordTabel_1.ItemName= ZyReader:readString()
			mRecordTabel_1.HeadID= ZyReader:readString()
			mRecordTabel_1.MaxHeadID= ZyReader:readString()
			mRecordTabel_1.ItemDescribe = ZyReader:readString()
			mRecordTabel_1.QualityType = ZyReader:getWORD()
			mRecordTabel_1.ItemPrice = ZyReader:getInt()
			mRecordTabel_1.SpecialPrice = ZyReader:getInt()
			mRecordTabel_1.SeqNO = ZyReader:getInt()
			mRecordTabel_1.IsHot = ZyReader:getInt()
			mRecordTabel_1.HotSell=ZyReader:getBYTE()
			ZyReader:recordEnd()
			ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.RecordTabel = RecordTabel_1;
		DataTabel.ObtainNum= ZyReader:getInt()
	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
	end
    return DataTabel
end


--        7004_商店物品购买接口（ID=7004）
function Action7004(Scene,isLoading ,ItemID,MallType,Num)
    ZyWriter:writeString("ActionId",7004)
    ZyWriter:writeString("ItemID",ItemID)
    ZyWriter:writeString("MallType",MallType)
    ZyWriter:writeString("Num",Num)
    ZyExecRequest(Scene, nil,isLoading)
end

function _7004Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
    else
	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.15)
    end
    return DataTabel
end


--        7006_背包物品出售接口（ID=7006）
function Action7006(Scene,isLoading ,UserItemID, data)
   	ZyWriter:writeString("ActionId",7006)
   	ZyWriter:writeString("UserItemID",UserItemID)
	ZyExecRequest(Scene, data,isLoading)
end


--        7009_物品详情接口（ID=7009）
function Action7009(Scene,isLoading ,ItemID,MallType,CityID)
   	ZyWriter:writeString("ActionId",7009)
   	ZyWriter:writeString("ItemID",ItemID)
	ZyWriter:writeString("MallType",MallType)
	ZyWriter:writeString("CityID",CityID)
	ZyExecRequest(Scene, nil,isLoading)
end

function _7009Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.HeadID= ZyReader:readString()
        DataTabel.ItemName= ZyReader:readString()
        DataTabel.ItemType= ZyReader:getWORD()
        DataTabel.QualityType= ZyReader:getWORD()
        DataTabel.CurLevel= ZyReader:getWORD()
        DataTabel.PreLevel= ZyReader:getWORD()
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.AbilityType= ZyReader:getInt()
                mRecordTabel_1.BaseNum= ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.Ability = RecordTabel_1;
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.CareerID= ZyReader:getInt()
                mRecordTabel_1.CareerName= ZyReader:readString()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.Career = RecordTabel_1;
        DataTabel.QiShiName= ZyReader:readString()
        DataTabel.Price= ZyReader:getInt()
        DataTabel.ItemDesc= ZyReader:readString()
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.MaterialsID= ZyReader:getInt()
                mRecordTabel_1.Name= ZyReader:readString()
                mRecordTabel_1.Num= ZyReader:getWORD()
                mRecordTabel_1.PackNum= ZyReader:getWORD()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.Materials = RecordTabel_1;
    else
  ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        7011_竞技场道具列表（ID=7011）
function Action7011(Scene,isLoading ,PageIndex,PageSize)
	ZyWriter:writeString("ActionId",7011)
	ZyWriter:writeString("PageIndex",PageIndex)
	ZyWriter:writeString("PageSize",PageSize)
	ZyExecRequest(Scene, nil,isLoading)
end

function _7011Callback(pScutScene, lpExternalData)
	
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.PageCount= ZyReader:getInt()
		local RecordNums_1=ZyReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
				local mRecordTabel_1={}
				ZyReader:recordBegin()
				mRecordTabel_1.ItemID= ZyReader:getInt()
				mRecordTabel_1.ItemName= ZyReader:readString()
				mRecordTabel_1.HeadID= ZyReader:readString()
				mRecordTabel_1.MaxHeadID= ZyReader:readString()
				mRecordTabel_1.ItemDesc= ZyReader:readString()
				mRecordTabel_1.QualityType= ZyReader:getInt()
				mRecordTabel_1.Athletics= ZyReader:getInt()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.RecordTabel = RecordTabel_1;
		DataTabel.SportsIntegral= ZyReader:getInt()
	else          
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
	end
	return DataTabel
end

--        7012_竞技场购买道具（ID=7012）
function Action7012(Scene,isLoading ,ItemId,ItemNum)

   	ZyWriter:writeString("ActionId",7012)
   	ZyWriter:writeString("ItemId",ItemId)
        ZyWriter:writeString("ItemNum",ItemNum)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _7012Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
	DataTabel.SportsIntegral= ZyReader:getInt() 
    else          
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end


--        9003_充值礼包列表接口（ID=9003）
function Action9003(Scene,isLoading )
   	ZyWriter:writeString("ActionId",9003)
	ZyExecRequest(Scene, nil,isLoading)
end

function _9003Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.PacksID= ZyReader:getInt()
                mRecordTabel_1.PacksName= ZyReader:readString()
                mRecordTabel_1.IsRevice= ZyReader:getWORD()
                mRecordTabel_1.IsShow= ZyReader:getWORD()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;
    else
       ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        9004_充值礼包详情接口（ID=9004）
function Action9004(Scene,isLoading ,PacksID)
   	ZyWriter:writeString("ActionId",9004)
   	ZyWriter:writeString("PacksID",PacksID)
	ZyExecRequest(Scene, nil,isLoading)
end

function _9004Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.PacksName= ZyReader:readString()
        DataTabel.IsRevice= ZyReader:getWORD()
        DataTabel.PacksDesc= ZyReader:readString()
    else
ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        9005_充值礼包领取接口（ID=9005）
function Action9005(Scene,isLoading ,PacksID)
   	ZyWriter:writeString("ActionId",9005)
   	ZyWriter:writeString("PacksID",PacksID)
	ZyExecRequest(Scene, nil,isLoading)
end

function _9005Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}

    else
ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end



--首次充值奖励
function Action9006(Scene,isLoading )
   	ZyWriter:writeString("ActionId",9006)
	ZyExecRequest(Scene, nil,isLoading)
end

function _9006Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
	DataTabel={}
	local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.ItemID= ZyReader:getInt()
                mRecordTabel_1.Type= ZyReader:getInt()
                mRecordTabel_1.Num= ZyReader:getInt()
                mRecordTabel_1.HeadID= ZyReader:readString()
                mRecordTabel_1.ItemName= ZyReader:readString()
                mRecordTabel_1.ItemDesc= ZyReader:readString()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
       DataTabel= RecordTabel_1;
                
    else
	 ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        9101_好友列表接口（ID=9101）
function Action9101(Scene,isLoading ,FriendType,PageIndex,PageSize)
   	 ZyWriter:writeString("ActionId",9101)
   	 ZyWriter:writeString("FriendType",FriendType)
        ZyWriter:writeString("PageIndex",PageIndex)
        ZyWriter:writeString("PageSize",PageSize)
	ZyExecRequest(Scene, FriendType,isLoading)
end

function _9101Callback(pScutScene, lpExternalData)

    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.PageCount= ZyReader:getInt()
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.FriendID= ZyReader:readString()
                mRecordTabel_1.FriendName= ZyReader:readString()
                mRecordTabel_1.FriendLv= ZyReader:getWORD()
                mRecordTabel_1.FriendType= ZyReader:getWORD()
                mRecordTabel_1.UserID= ZyReader:readString()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;
    else
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)
    end
    return DataTabel
end

--        9102添加好友接口 （ID=9102）
function Action9102(Scene,isLoading ,PageIndex,PageSize)
   	ZyWriter:writeString("ActionId",9102)
   	ZyWriter:writeString("PageIndex",PageIndex)
        ZyWriter:writeString("PageSize",PageSize)
	ZyExecRequest(Scene, nil,isLoading)
end

function _9102Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.PageCount= ZyReader:getInt()
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.FriendID= ZyReader:readString()
                mRecordTabel_1.FriendName= ZyReader:readString()
                mRecordTabel_1.FriendLv= ZyReader:getWORD()
                mRecordTabel_1.FriendType = ZyReader:getWORD()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;
    else
 		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        9103_添加好友接口（ID=9103）
function Action9103(Scene,isLoading ,FriendID,FriendName)
	ZyWriter:writeString("ActionId",9103)
   	ZyWriter:writeString("FriendID",FriendID)
	ZyWriter:writeString("FriendName",FriendName)
    ZyExecRequest(Scene, nil,isLoading)
end


--        9104_删除好友接口（ID=9104）
function Action9104(Scene,isLoading ,FriendID)
	ZyWriter:writeString("ActionId",9104)
   	ZyWriter:writeString("FriendID",FriendID)
    ZyExecRequest(Scene, nil,isLoading)
end

function _9104Callback(pScutScene, lpExternalData)
   	local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
	else
ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        9105
function Action9105(Scene,isLoading ,FriendId,MailId,Ops)
	ZyWriter:writeString("ActionId",9105)
   	ZyWriter:writeString("FriendId",FriendId)
   	ZyWriter:writeString("MailId",MailId)
   	ZyWriter:writeString("Ops",Ops)
    ZyExecRequest(Scene, nil,isLoading)
end

function _9105Callback(pScutScene, lpExternalData)
	local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        9201_私聊发送接口（ID=9201）
function Action9201(Scene,isLoading ,ToUserID,Content)
	ZyWriter:writeString("ActionId",9201)
   	ZyWriter:writeString("ToUserID",ToUserID)
	ZyWriter:writeString("Content",Content)
    ZyExecRequest(Scene, nil,isLoading)
end

function _9201Callback(pScutScene, lpExternalData)
   	local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
	else
ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        9202_公告列表接口（ID=9202）
function Action9202(Scene,isLoading ,PageIndex,PageSize)
	ZyWriter:writeString("ActionId",9202)
   	ZyWriter:writeString("PageIndex",PageIndex)
	ZyWriter:writeString("PageSize",PageSize)
   	ZyExecRequest(Scene, nil,isLoading)
end

function _9202Callback(pScutScene, lpExternalData)
   	local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.PageCount= ZyReader:getInt()
        local RecordNums_1=ZyReader:getInt()
    	local RecordTabel_1={}
    	if RecordNums_1~=0 then
    		for k=1,RecordNums_1 do
             	local mRecordTabel_1={}
             	ZyReader:recordBegin()
                mRecordTabel_1.Title= ZyReader:readString()
                mRecordTabel_1.Content= ZyReader:readString()
                mRecordTabel_1.SendDate= ZyReader:readString()
                mRecordTabel_1.NoticesType =  ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
     		end
        end
		DataTabel.RecordTabel = RecordTabel_1;
		DataTabel.IsToday = ZyReader:getWORD()
	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        9203_聊天发送接口（ID=9203）
function Action9203(Scene,isLoading ,ChatType,Content)
	ZyWriter:writeString("ActionId",9203)
   	ZyWriter:writeString("ChatType",ChatType)
	ZyWriter:writeString("Content",Content)
    ZyExecRequest(Scene, nil,isLoading)
end

function _9203Callback(pScutScene, lpExternalData)
   	local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
	else
ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        9204_聊天列表接口（ID=9204）
function Action9204(Scene,isLoading )
	ZyWriter:writeString("ActionId",9204)
   	ZyExecRequest(Scene,1,isLoading)
end

function _9204Callback(pScutScene, lpExternalData)
    if ZyReader:getResult() == eScutNetSuccess then
        local ChatMaxNum= ZyReader:getInt()
        DataTabel=nil
        local recordNum=ZyReader:getInt()
        if recordNum~=nil and recordNum>0 then
        	DataTabel={}
            for k=1,recordNum do
             	local mRecordTabel_1={}
             	ZyReader:recordBegin()
                mRecordTabel_1.UserID= ZyReader:readString()
                mRecordTabel_1.UserName= ZyReader:readString()
                mRecordTabel_1.Content= ZyReader:readString()
                mRecordTabel_1.SendDate= ZyReader:readString()
                mRecordTabel_1.ChatType= ZyReader:getWORD()
                mRecordTabel_1.ReUser= ZyReader:readString()
                mRecordTabel_1.ReUserName= ZyReader:readString()
                mRecordTabel_1.IsFriend= ZyReader:getInt()
                mRecordTabel_1.UserVipLv= ZyReader:getWORD()
                mRecordTabel_1.ReUserVipLv= ZyReader:getWORD()
                ZyReader:recordEnd()
                ZyTable.push_back(DataTabel,mRecordTabel_1)

         	end
         	 	DataTabel.RecordTabel = RecordTabel_1;
        end

	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end



function Action9205(Scene,isLoading )
	ZyWriter:writeString("ActionId",9205)
   	ZyExecRequest(Scene, nil,isLoading)
end

--1系统广播  2游戏内部 3玩家
function _9205Callback(pScutScene, lpExternalData)
   	local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        local RecordNums_1=ZyReader:getInt()
    	if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             	local mRecordTabel_1={}
             	ZyReader:recordBegin()
                mRecordTabel_1.BroadcastType= ZyReader:getWORD()
                mRecordTabel_1.Content= ZyReader:readString()
                mRecordTabel_1.PlayTimes= ZyReader:getInt()
                mRecordTabel_1.UserName= ZyReader:readString()
                ZyReader:recordEnd()
                ZyTable.push_back(DataTabel,mRecordTabel_1)
         	end
        end

	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

---- 9301    好友发件接口
function Action9301(Scene,isLoading,Title,Content,ToUserID,ToUserName,IsGuide)
	ZyWriter:writeString("ActionId",9301)
	ZyWriter:writeString("Title",Title)
	ZyWriter:writeString("Content",Content)
	ZyWriter:writeString("ToUserID",ToUserID)
	ZyWriter:writeString("ToUserName",ToUserName)
	ZyWriter:writeString("IsGuide",IsGuide)
   	ZyExecRequest(Scene, nil,isLoading)
end

--        9302_玩家收件接口（ID=9302）
function Action9302(Scene,isLoading ,MailType)
	ZyWriter:writeString("ActionId",9302)
	ZyWriter:writeString("MailType",MailType)
	ZyExecRequest(Scene, nil,isLoading)
end

function _9302Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		local RecordNums_1=ZyReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
				local mRecordTabel_1={}
				ZyReader:recordBegin()
				mRecordTabel_1.UserId= ZyReader:getInt()
				mRecordTabel_1.MailType= ZyReader:getWORD()
				mRecordTabel_1.FromUserId= ZyReader:getInt()
				mRecordTabel_1.FromUserName= ZyReader:readString()
				mRecordTabel_1.Title= ZyReader:readString()
				mRecordTabel_1.Content= ZyReader:readString()
				mRecordTabel_1.SendDate= ZyReader:readString()
				mRecordTabel_1.IsGuide= ZyReader:getByte()
				mRecordTabel_1.GuideContent= ZyReader:readString()
				mRecordTabel_1.IsReply= ZyReader:getByte()
				mRecordTabel_1.ReplyStatus= ZyReader:getWORD()
				mRecordTabel_1.MailID= ZyReader:readString()
				mRecordTabel_1.CounterattackUserID= ZyReader:getInt()
				mRecordTabel_1.SendMailDate= ZyReader:readString()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.LetterTabel = RecordTabel_1;
	else          
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
	end
	return DataTabel
end

function Action10001(Scene,isLoading)
	ZyWriter:writeString("ActionId",10001)
	ZyExecRequest(Scene, nil,isLoading)
end

function _10001Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.DewNum= ZyReader:getInt()
             DataTabel.IsShow= ZyReader:getWORD()
             DataTabel.LandNum= ZyReader:getInt()
             local RecordNums_1= ZyReader:getInt()
         	local RecordTabel_1={}
		if RecordNums_1~=0 then
      		for k=1,RecordNums_1 do
       		local mRecordTabel_1={}
             		ZyReader:recordBegin()
               	 mRecordTabel_1.LandPsition= ZyReader:getInt()
               	 mRecordTabel_1.IsOpen= ZyReader:getWORD()
                 	 mRecordTabel_1.IsGain= ZyReader:getWORD()
               	 mRecordTabel_1.IsRedLand= ZyReader:getWORD()
               	 mRecordTabel_1.IsBlackLand= ZyReader:getWORD()
                	 mRecordTabel_1.ColdTime= ZyReader:getInt()
                	 mRecordTabel_1.GeneralID= ZyReader:getInt()
                	 mRecordTabel_1.PlantType= ZyReader:getWORD()
               	 ZyReader:recordEnd()
                	ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
     			end
      		  end
  		DataTabel.PlantingInfo = RecordTabel_1;
	else
		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
	end
	return DataTabel
end

function Action10002(Scene,isLoading)
	ZyWriter:writeString("ActionId",10002)
	ZyExecRequest(Scene, nil,isLoading)
end

function _10002Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
             local RecordNums_1= ZyReader:getInt()
		if RecordNums_1~=0 then
      		for k=1,RecordNums_1 do
       		local mRecordTabel_1={}
             		ZyReader:recordBegin()
               	 mRecordTabel_1.GeneralID= ZyReader:getInt()
               	 mRecordTabel_1.GeneralName= ZyReader:readString()
                	mRecordTabel_1.GeneralStatus= ZyReader:getWORD()
                	mRecordTabel_1.HeadID= ZyReader:readString()
               	 ZyReader:recordEnd()
                	ZyTable.push_back(DataTabel,mRecordTabel_1)
     			end
      		  end
	else
		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),0.5,0.35)
	end
	return DataTabel
end


function Action10003(Scene,isLoading,PlantType,GeneralID,LandPostion)
	ZyWriter:writeString("ActionId",10003)
	ZyWriter:writeString("PlantType",PlantType)
	ZyWriter:writeString("GeneralID",GeneralID)
	ZyWriter:writeString("LandPostion",LandPostion)
	ZyExecRequest(Scene, nil,isLoading)
end

function _10003Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.GeneralName=ZyReader:readString()
		DataTabel.GeneralLv=ZyReader:getWord()
		DataTabel.RewardNum=ZyReader:getInt()
		DataTabel.PlantQualityType=ZyReader:getWord()
	else
		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),0.5,0.35)
	end
	return DataTabel
end


function Action10004(Scene,isLoading,PlantType,GeneralID,PlantQualityType,LandPsition)
	ZyWriter:writeString("ActionId",10004)
	ZyWriter:writeString("PlantType",PlantType)
	ZyWriter:writeString("GeneralID",GeneralID)
	ZyWriter:writeString("PlantQualityType",PlantQualityType)
	ZyWriter:writeString("LandPsition",LandPsition)
	ZyExecRequest(Scene, nil,isLoading)
end


function Action10005(Scene,isLoading,PlantType,GeneralID,RefershID,Ops)
	ZyWriter:writeString("ActionId",10005)
	ZyWriter:writeString("PlantType",PlantType)
	ZyWriter:writeString("GeneralID",GeneralID)
	ZyWriter:writeString("RefershID",RefershID)
	ZyWriter:writeString("Ops",Ops)
	ZyExecRequest(Scene, nil,isLoading)
end


function Action10006(Scene,isLoading,PlantType,GeneralID,LandPositon)
	ZyWriter:writeString("ActionId",10006)
	ZyWriter:writeString("PlantType",PlantType)
	ZyWriter:writeString("GeneralID",GeneralID)
	ZyWriter:writeString("LandPositon",LandPositon)
	ZyExecRequest(Scene, nil,isLoading)
end





function Action10007(Scene,isLoading,LandPostion,Ops)
	ZyWriter:writeString("ActionId",10007)
	ZyWriter:writeString("LandPostion",LandPostion)
	ZyWriter:writeString("Ops",Ops)
	ZyExecRequest(Scene, nil,isLoading)
end


function Action10009(Scene,isLoading,Ops)
	ZyWriter:writeString("ActionId",10009)
	ZyWriter:writeString("Ops",Ops)
	ZyExecRequest(Scene, nil,isLoading)
end


function Action10008(Scene,isLoading,LandPostion,Ops)
	ZyWriter:writeString("ActionId",10008)
	ZyWriter:writeString("LandPostion",LandPostion)
	ZyWriter:writeString("Ops",Ops)
	ZyExecRequest(Scene, nil,isLoading)
end



function Action10010(Scene,isLoading,Ops)
	ZyWriter:writeString("ActionId",10010)
	ZyWriter:writeString("Ops",Ops)
	ZyExecRequest(Scene, nil,isLoading)
end



function Action10011(Scene,isLoading,Ops)
	ZyWriter:writeString("ActionId",10011)
	ZyWriter:writeString("Ops",Ops)
	ZyExecRequest(Scene, nil,isLoading)
end



--        11001_探险答题接口（ID=11001）
function Action11001(Scene,isLoading )
   	ZyWriter:writeString("ActionId",11001)
	ZyExecRequest(Scene, nil,isLoading)
end

function _11001Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.QuestionID= ZyReader:getInt()
        DataTabel.QuestionContent= ZyReader:readString()
        DataTabel.AnswerID1= ZyReader:getInt()
        DataTabel.AnserContent1= ZyReader:readString()
        DataTabel.AnswerID2= ZyReader:getInt()
        DataTabel.AnserContent2= ZyReader:readString()
        DataTabel.IsEnd= ZyReader:getInt()
        DataTabel.CodeTime= ZyReader:getInt()
        DataTabel.UseGold= ZyReader:getInt()
    else
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        11002_完成探险答题接口（ID=11002）
function Action11002(Scene,isLoading ,QuestionID,AnswerID,IsRemove)
   	ZyWriter:writeString("ActionId",11002)
   	ZyWriter:writeString("QuestionID",QuestionID)
        ZyWriter:writeString("AnswerID",AnswerID)
        ZyWriter:writeString("IsRemove",IsRemove)
	ZyExecRequest(Scene, nil,isLoading)
end

function _11002Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.CodeTime= ZyReader:getInt()
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.RewardType= ZyReader:getWORD()
                mRecordTabel_1.RewardNum= ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;
    else 
        --ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        11003_探险答题冷却加速接口（ID=11003）
function Action11003(Scene,isLoading ,Ops)
   	ZyWriter:writeString("ActionId",11003)
   	ZyWriter:writeString("Ops",Ops)
	ZyExecRequest(Scene, nil,isLoading)
end

function _11003Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
    else
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        12001_幸运转盘界面接口（ID=12001）
function Action12001(Scene,isLoading ,UserItemID )
   	ZyWriter:writeString("ActionId",12001)
   	ZyWriter:writeString("UserItemID",UserItemID)	 
	ZyExecRequest(Scene, nil,isLoading)
end

function _12001Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.IsFree= ZyReader:getWORD()
        DataTabel.FreeNum= ZyReader:getInt()
        DataTabel.ItemHead= ZyReader:readString()
        DataTabel.ItemContent= ZyReader:readString()
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.Postion= ZyReader:getInt()
                mRecordTabel_1.HeadID= ZyReader:readString()
                mRecordTabel_1.ItemDesc= ZyReader:readString()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;

       
    else          
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        12002_我的宝藏列表接口（ID=12002）
function Action12002(Scene,isLoading )

   	ZyWriter:writeString("ActionId",12002)
   	
	ZyExecRequest(Scene, nil,isLoading)
end

function _12002Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.RewardType= ZyReader:getWORD()
                mRecordTabel_1.RewardName= ZyReader:readString()
                mRecordTabel_1.RewardNum= ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;   
    else          
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        12003_探宝记录接口（ID=12003）
function Action12003(Scene,isLoading )
   	ZyWriter:writeString("ActionId",12003)  	
	ZyExecRequest(Scene, nil,isLoading)
end

function _12003Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.UseName= ZyReader:readString()
                mRecordTabel_1.RewardType= ZyReader:getWORD()
                mRecordTabel_1.RewardName= ZyReader:readString()
                mRecordTabel_1.RewardNum= ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;

       
    else          
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        12004_抽奖接口（ID=12004）
function Action12004(Scene,isLoading ,Ops)
   	ZyWriter:writeString("ActionId",12004)
   	ZyWriter:writeString("Ops",Ops)
	ZyExecRequest(Scene, nil,isLoading)
end

function _12004Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.Postion= ZyReader:getWORD()      
        DataTabel.HasNextBox= ZyReader:getInt()
    else          
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end



----- 13001 圣吉塔

function Action13001(Scene,isLoading)
   	ZyWriter:writeString("ActionId",13001)
	ZyExecRequest(Scene, nil,isLoading)
end

function _13001Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.BattleRount  = ZyReader:getInt()
		DataTabel.LastBattleRount  = ZyReader:getInt()
		DataTabel.MaxTierNum  = ZyReader:getInt()
		DataTabel.ScoreStar  = ZyReader:getInt()
		DataTabel.UserRank   = ZyReader:getInt()
		DataTabel.UserLv    = ZyReader:getInt()
    else          
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end


----- 13002 圣吉塔进入副本接口

function Action13002(Scene,isLoading,AddType)
   	ZyWriter:writeString("ActionId",13002)
   	ZyWriter:writeString("AddType",AddType)
	ZyExecRequest(Scene, nil,isLoading)
end

function _13002Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.LastIsRountStar  = ZyReader:getInt()
		DataTabel.AddNum  = ZyReader:getInt()
		DataTabel.IsRountStar  = ZyReader:getInt()
		DataTabel.LastScoreStar  = ZyReader:getInt()
		DataTabel.HeadID  = ZyReader:readString()
		DataTabel.MonsterName  = ZyReader:getInt()
		DataTabel.MonsterNum  = ZyReader:getInt()
		
		
		local RecordNums_1=ZyReader:getInt()
	     	local RecordTabel_1={}
			if RecordNums_1~=0 then
	    		for k=1,RecordNums_1 do
	             	local mRecordTabel_1={}
	             	ZyReader:recordBegin()
	                mRecordTabel_1.DifficultNum    = ZyReader:getInt()
	                mRecordTabel_1.Multiple    = ZyReader:getInt()
	                ZyReader:recordEnd()
	                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
	        	end
	       end
   		DataTabel.DifficultTabel = RecordTabel_1;
		
		
		
		DataTabel.LifeNum  = ZyReader:getInt()
		DataTabel.PhyNum  = ZyReader:getInt()
		DataTabel.MagNum  = ZyReader:getInt()
		DataTabel.AbiNum  = ZyReader:getInt()
		DataTabel.MaxTierNum  = ZyReader:getInt()
		DataTabel.ScoreStar   = ZyReader:getInt()
    else          
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        4401_圣吉塔活动界面（ID=4401）
function Action4401(Scene,isLoading )

   	ZyWriter:writeString("ActionId",4401)
   	
	ZyExecRequest(Scene, nil,isLoading)
end

function _4401Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.BattleRount= ZyReader:getInt()
        DataTabel.LastBattleRount= ZyReader:getInt()
        DataTabel.MaxTierNum= ZyReader:getInt()
        DataTabel.ScoreStar= ZyReader:getInt()
        DataTabel.UserRank= ZyReader:getInt()
        DataTabel.UserLv= ZyReader:getInt()
        DataTabel.SJTStatus= ZyReader:getWORD()

       
    else          
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        4402_圣吉塔属性加成界面接口（ID=4402）
function Action4402(Scene,isLoading )

   	ZyWriter:writeString("ActionId",4402)
   	
	ZyExecRequest(Scene, nil,isLoading)
end

function _4402Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.StarNum= ZyReader:getInt()
        DataTabel.EffNum= ZyReader:getInt()

       
    else          
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end


--        4403_圣吉塔选择属性接口（ID=4403）
function Action4403(Scene,isLoading ,PropertyType,EffNum)

   	ZyWriter:writeString("ActionId",4403)
   	ZyWriter:writeString("PropertyType",PropertyType)
        ZyWriter:writeString("EffNum",EffNum)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _4403Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}

       
    else          
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        4404_进入副本接口（ID=4404）
function Action4404(Scene,isLoading )

   	ZyWriter:writeString("ActionId",4404)
   	
	ZyExecRequest(Scene, nil,isLoading)
end

function _4404Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.PlotID= ZyReader:getInt()
        DataTabel.IsRountStar= ZyReader:getInt()
        DataTabel.LastScoreStar= ZyReader:getInt()
        DataTabel.HeadID= ZyReader:readString()
        DataTabel.MonsterName= ZyReader:readString()
        DataTabel.MonsterNum= ZyReader:getInt()
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.DifficultyType= ZyReader:getInt()
                mRecordTabel_1.DifficultNum= ZyReader:readString()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;
        DataTabel.LifeNum= ZyReader:readString()
        DataTabel.PhyNum= ZyReader:readString()
        DataTabel.MagNum= ZyReader:readString()
        DataTabel.AbiNum= ZyReader:readString()
        DataTabel.MaxTierNum= ZyReader:getInt()
        DataTabel.ScoreStar= ZyReader:getInt()

       
    else          
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end



-----13004 圣吉塔 属性兑换界面接口

function Action13004(Scene,isLoading)
   	ZyWriter:writeString("ActionId",13004)
	ZyExecRequest(Scene, nil,isLoading)
end


function _13004Callback(pScutScene, lpExternalData)
   	local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.IsTierNum      = ZyReader:getInt()
        DataTabel.IsRountStar    = ZyReader:getInt()
        DataTabel.LastScoreStar     = ZyReader:getInt()

        local RecordNums_1=ZyReader:getInt()
     	local RecordTabel_1={}
		if RecordNums_1~=0 then
    		for k=1,RecordNums_1 do
             	local mRecordTabel_1={}
             	ZyReader:recordBegin()
                mRecordTabel_1.AddName    = ZyReader:getInt()
                mRecordTabel_1.AddNum    = ZyReader:getInt()
                mRecordTabel_1.NeedStar    = ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
        	end
        end
   		DataTabel.AbilityNumTabel = RecordTabel_1;
   		
		DataTabel.LifeNum       = ZyReader:getInt()
		DataTabel.PhyNum     = ZyReader:getInt()
		DataTabel.MagNum      = ZyReader:getInt()	
		DataTabel.AbiNum     = ZyReader:getInt()
	else
	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end




-----13005 圣吉塔 属性兑换界面接口

function Action13005(Scene,isLoading,SJTID)
   	ZyWriter:writeString("ActionId",13005)
   	ZyWriter:writeString("SJTID",SJTID)
	ZyExecRequest(Scene, nil,isLoading)
end


function _13005Callback(pScutScene, lpExternalData)
   	local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.IsTierStar       = ZyReader:getInt()
        DataTabel.IsRountStar     = ZyReader:getInt()
        DataTabel.ScoreStar      = ZyReader:getInt()

        local RecordNums_1=ZyReader:getInt()
     	local RecordTabel_1={}
		if RecordNums_1~=0 then
    		for k=1,RecordNums_1 do
             	local mRecordTabel_1={}
             	ZyReader:recordBegin()
                mRecordTabel_1.ItemHeadID     = ZyReader:readString()
                mRecordTabel_1.ItemNum     = ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
        	end
        end
   		DataTabel.achGoodTabel = RecordTabel_1;

	else
	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end




-----13006  圣吉塔排行榜接口

function Action13006(Scene,isLoading,RankStype)
   	ZyWriter:writeString("ActionId",13006)
   	ZyWriter:writeString("RankStype",RankStype)
	ZyExecRequest(Scene, nil,isLoading)
end


function _13006Callback(pScutScene, lpExternalData)
   	local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        local RecordNums_1=ZyReader:getInt()
     	local RecordTabel_1={}
		if RecordNums_1~=0 then
    		for k=1,RecordNums_1 do
             	local mRecordTabel_1={}
             	ZyReader:recordBegin()
		mRecordTabel_1.RanKing      = ZyReader:getInt()
		mRecordTabel_1.NickName      = ZyReader:readString()
		mRecordTabel_1.UserLv      = ZyReader:getInt()
		mRecordTabel_1.MaxTierNum      = ZyReader:getInt()
		mRecordTabel_1.ScoreStar      = ZyReader:getInt()
		mRecordTabel_1.HaveRankNum      = ZyReader:getInt()
		mRecordTabel_1.UserID       = ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
        	end
        end
   		DataTabel.rankPlayTabel = RecordTabel_1;

	else
	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end


-----4405 圣吉塔战斗详情接口

function Action4405(Scene,isLoading,DiffcultyType,DifficultNum,PlotID)
   	ZyWriter:writeString("ActionId",4405)
   	ZyWriter:writeString("DiffcultyType",DiffcultyType)
       ZyWriter:writeString("DifficultNum",DifficultNum)
       ZyWriter:writeString("PlotID",PlotID)
	ZyExecRequest(Scene, nil,isLoading)
end


function _4405Callback(pScutScene, lpExternalData)
   	local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.IsWin     = ZyReader:getWORD()
        DataTabel.BgScene= ZyReader:readString()
--        DataTabel.Experience   = ZyReader:getInt()
	
			-------------攻防阵型
		local RecordNums_2=ZyReader:getInt()
		local RecordTabel_2={}
		if RecordNums_2~=nil and RecordNums_2>0 then
		for  k=1, RecordNums_2 do
		local mRecordNums_2={}
		ZyReader:recordBegin()
		mRecordNums_2.AttGeneralID=ZyReader:getInt()
		mRecordNums_2.AttGeneralName=ZyReader:readString()
		mRecordNums_2.AttGeneralHeadID=ZyReader:readString()
		mRecordNums_2.AttPosition=ZyReader:getWORD()
		mRecordNums_2.LiveNum=ZyReader:getInt()
		mRecordNums_2.LiveMaxNum=ZyReader:getInt()
		mRecordNums_2.MomentumNum=ZyReader:getWORD()
		mRecordNums_2.MaxMomentumNum=ZyReader:getWORD()
		mRecordNums_2.AbilityID=ZyReader:getInt()
		mRecordNums_2.GeneralLv=ZyReader:getWORD()
		mRecordNums_2.IsAttReplace= ZyReader:getWORD()
		mRecordNums_2.AttGeneralQuality= ZyReader:getWORD()
		ZyReader:recordEnd()
		ZyTable.push_back(RecordTabel_2,mRecordNums_2)
		end
		end
		DataTabel.AgainstTable=RecordTabel_2
		    
		
		-------------防守阵型
		local RecordNums_3=ZyReader:getInt()
		local RecordTabel_3={}
		if RecordNums_3~=nil and RecordNums_3>0 then
			for  k=1, RecordNums_3 do
				local mRecordNums_3={}
				ZyReader:recordBegin()
				mRecordNums_3.AttGeneralID=ZyReader:getInt()
				mRecordNums_3.AttGeneralName=ZyReader:readString()
				mRecordNums_3.AttGeneralHeadID=ZyReader:readString()
				mRecordNums_3.AttPosition=ZyReader:getWORD()
				mRecordNums_3.LiveNum=ZyReader:getInt()
				mRecordNums_3.LiveMaxNum=ZyReader:getInt()
				mRecordNums_3.MomentumNum=ZyReader:getWORD()
				mRecordNums_3.MaxMomentumNum=ZyReader:getWORD()
				mRecordNums_3.AbilityID=ZyReader:getInt()
				mRecordNums_3.GeneralLv=ZyReader:getWORD()
				mRecordNums_3.IsAttReplace= ZyReader:getWORD()
				mRecordNums_3.AttGeneralQuality= ZyReader:getWORD()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_3,mRecordNums_3)
			end
		end
		DataTabel.DefendingTable=RecordTabel_3
		    
		    
		
		-----------------战斗过程
		local RecordNums_4=ZyReader:getInt()
		local RecordTabel_4={}
		if RecordNums_4~=nil and RecordNums_4>0 then
			for  k=1, RecordNums_4 do
				local mRecordNums_4={}
				ZyReader:recordBegin()
				mRecordNums_4.AttGeneralID=ZyReader:getInt()
				mRecordNums_4.AttGeneralLiveNum=ZyReader:getInt()
				mRecordNums_4.AttGeneralQishi=ZyReader:getWORD()
				mRecordNums_4.AttackTaget=ZyReader:getWORD()
				mRecordNums_4.AttackType=ZyReader:getWORD()
				mRecordNums_4.AbilityProperty=ZyReader:getWORD()
				mRecordNums_4.AttGeneralStatus=ZyReader:getWORD()
				mRecordNums_4.BackDamage=ZyReader:getInt()
				mRecordNums_4.AttEffectID=ZyReader:readString()
				mRecordNums_4.TargetEffectID=ZyReader:readString()
				mRecordNums_4.IsMove=ZyReader:getWORD()
				mRecordNums_4.Position=ZyReader:getWORD()
				mRecordNums_4.Role=ZyReader:getWORD()
			
				-----内嵌循环 中招效果开始
				local RecordNums_4_0=ZyReader:getInt()
				local RecordTabel_4_0={}
				if RecordNums_4_0~=nil and RecordNums_4_0>0 then
					for k=1, RecordNums_4_0 do
						local mRecordTabel_4_0={}
						ZyReader:recordBegin()
						mRecordTabel_4_0.GeneralEffect=ZyReader:getWORD()
						mRecordTabel_4_0.ConDamageNum=ZyReader:getInt()
						mRecordTabel_4_0.IsIncrease=ZyReader:getInt()
						ZyReader:recordEnd()
						ZyTable.push_back(RecordTabel_4_0,mRecordTabel_4_0)
					end
				end
				mRecordNums_4.GeneralEffects=RecordTabel_4_0
			
				-----内嵌循环 受到攻击的角色
				local RecordNums_4_1=ZyReader:getInt()
				local RecordTabel_4_1={}
				if RecordNums_4_1~=nil and RecordNums_4_1>0 then
					for k=1, RecordNums_4_1 do
						local mRecordTabel_4_1={}
						ZyReader:recordBegin()
						mRecordTabel_4_1.TargetGeneralID=ZyReader:getInt()
						mRecordTabel_4_1.TargetGeneralLiveNum=ZyReader:getInt()
						mRecordTabel_4_1.TargetGeneralQishi=ZyReader:getWORD()
						mRecordTabel_4_1.TargetDamageNum=ZyReader:getInt()
						mRecordTabel_4_1.IsShanBi=ZyReader:getWORD()
						mRecordTabel_4_1.IsGeDang=ZyReader:getWORD()
						mRecordTabel_4_1.IsFangji=ZyReader:getWORD()
						mRecordTabel_4_1.IsMove=ZyReader:getWORD()
						mRecordTabel_4_1.FangjiDamageNum=ZyReader:getInt()
						mRecordTabel_4_1.TargetStatus=ZyReader:getWORD()
						mRecordTabel_4_1.Position=ZyReader:getWORD()
						mRecordTabel_4_1.Role=ZyReader:getWORD()
						-----内嵌循环 中招效果开始
						local RecordNums_4_1_0=ZyReader:getInt()
						local RecordTabel_4_1_0={}
						if RecordNums_4_1_0~=nil and RecordNums_4_1_0>0 then
							for k=1, RecordNums_4_1_0 do
								local mRecordTabel_4_1_0={}
								ZyReader:recordBegin()
								mRecordTabel_4_1_0.GeneralEffect=ZyReader:getWORD()
								mRecordTabel_4_1_0.IsIncrease=ZyReader:getInt()
								ZyReader:recordEnd()
								ZyTable.push_back(RecordTabel_4_1_0,mRecordTabel_4_1_0)
							end
						end
						mRecordTabel_4_1.GeneralEffects=RecordTabel_4_1_0
						mRecordTabel_4_1.IsBaoji = ZyReader:getWORD()
				
				
						-----触发技能
						local RecordNums_4_1_1=ZyReader:getInt()
						local RecordTabel_4_1_1={}
						if RecordNums_4_1_1~=nil and RecordNums_4_1_1>0 then
							for k=1, RecordNums_4_1_1 do
								local mRecordTabel_4_1_1={}
								ZyReader:recordBegin()
								mRecordTabel_4_1_1.TrumpAbility=ZyReader:getWORD()
								mRecordTabel_4_1_1.TrumpNum=ZyReader:getInt()
								ZyReader:recordEnd()
								ZyTable.push_back(RecordTabel_4_1_1,mRecordTabel_4_1_1)
							end
						end                  
						mRecordTabel_4_1.TriggerTable=RecordTabel_4_1_1
						ZyReader:recordEnd()
						ZyTable.push_back(RecordTabel_4_1,mRecordTabel_4_1)
					end
				end
				mRecordNums_4.DefendFightTable=RecordTabel_4_1
				
				-----触发技能
				local RecordNums_4_2=ZyReader:getInt()
				local RecordTabel_4_2={}
				if RecordNums_4_2~=nil and RecordNums_4_2>0 then
					for k=1, RecordNums_4_2 do
						local mRecordTabel_4_2={}
						ZyReader:recordBegin()
						mRecordTabel_4_2.TrumpAbility=ZyReader:getWORD()
						mRecordTabel_4_2.TrumpNum=ZyReader:getInt()
						ZyReader:recordEnd()
						ZyTable.push_back(RecordTabel_4_2,mRecordTabel_4_2)
					end
				end                  
				mRecordNums_4.TriggerTable=RecordTabel_4_2
	
				mRecordNums_4.FntHeadID= ZyReader:readString()
				mRecordNums_4.AbilityID= ZyReader:getInt()


				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_4,mRecordNums_4)
			end
		end 
		DataTabel.FightProcessTable=RecordTabel_4
--		DataTabel.BlessExperience=ZyReader:getInt()
--		DataTabel.GotoNum=ZyReader:getInt()
		    
		    

	---------自身魂技效果 
		local RecordNums_5=ZyReader:getInt()
		local RecordTabel_5={}
		if RecordNums_5~=nil and RecordNums_5>0 then
			for  k=1, RecordNums_5 do
				local mRecordNums_3={}
				ZyReader:recordBegin()
				mRecordNums_5.AttGeneralID=ZyReader:getInt()
				mRecordNums_5.EffectID1=ZyReader:readString()
				mRecordNums_5.FntHeadID=ZyReader:readString()
				mRecordNums_5.IsIncrease=ZyReader:getWORD()
				mRecordNums_5.Role=ZyReader:getInt()
				mRecordNums_5.Position=ZyReader:getInt()
				
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_5,mRecordNums_5)
			end
		end
		DataTabel.FirstEffectTabel=RecordTabel_5	
		
        	DataTabel.UserTalPriority= ZyReader:getInt()
        	DataTabel.NpcTalPriority= ZyReader:getInt()
        	DataTabel.Score= ZyReader:getWORD()
        	DataTabel.StarNum= ZyReader:getWORD()
        	
	else
	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end


--        4406_圣吉塔战斗结果奖励接口（ID=4406）
function Action4406(Scene,isLoading ,IsWin,Score,StarNum)

   	ZyWriter:writeString("ActionId",4406)
   	ZyWriter:writeString("IsWin",IsWin)
   	ZyWriter:writeString("Score",Score)
       ZyWriter:writeString("StarNum",StarNum)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _4406Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.Score= ZyReader:getWORD()
        DataTabel.StarNum= ZyReader:getWORD()
        DataTabel.CombatRound= ZyReader:getWORD()
        DataTabel.Exchange= ZyReader:getWORD()
        DataTabel.Receive= ZyReader:getWORD()
        DataTabel.CurrentLayer= ZyReader:getInt()
        DataTabel.MaxHonourNum= ZyReader:getInt()
        DataTabel.CurrentHonour= ZyReader:getInt()

       
    else          
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end



--        4407_圣吉塔兑换属性界面（ID=4407）
function Action4407(Scene,isLoading ,IsWin)

   	ZyWriter:writeString("ActionId",4407)
   	ZyWriter:writeString("IsWin",IsWin)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _4407Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.LayerNum= ZyReader:getInt()
        DataTabel.StarNum= ZyReader:getInt()
        DataTabel.SulplusNum= ZyReader:getInt()
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.PropertyType= ZyReader:getWORD()
                mRecordTabel_1.EffNum= ZyReader:readString()
                mRecordTabel_1.DemandNum= ZyReader:getInt()
                mRecordTabel_1.IsActive= ZyReader:getWORD()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;
        DataTabel.LifeNum= ZyReader:readString()
        DataTabel.WuLiNum= ZyReader:readString()
        DataTabel.MofaNum= ZyReader:readString()
        DataTabel.FunJiNum= ZyReader:readString()

       
    else          
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end




--        4408_圣吉塔属性兑换（ID=4408）
function Action4408(Scene,isLoading ,PropertyType,StarNum)

   	ZyWriter:writeString("ActionId",4408)
   	ZyWriter:writeString("PropertyType",PropertyType)
        ZyWriter:writeString("StarNum",StarNum)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _4408Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}

       
    else          
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        4409_圣吉塔奖励界面接口（ID=4409）
function Action4409(Scene,isLoading)

   	ZyWriter:writeString("ActionId",4409)
   	
	ZyExecRequest(Scene, nil,isLoading)
end

function _4409Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.MoreStar= ZyReader:getInt()
        DataTabel.IsTierStar= ZyReader:getInt()
        DataTabel.IsTierNum= ZyReader:getInt()
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.ItemID= ZyReader:getInt()
                mRecordTabel_1.SJTRewarType= ZyReader:getInt()
                mRecordTabel_1.RewardNum= ZyReader:getInt()
                mRecordTabel_1.HeadID= ZyReader:readString()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;
        DataTabel.Modulus= ZyReader:getInt()
        DataTabel.AdditionalGameCoin= ZyReader:getInt()

       
    else          
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end


--        4410_五层领取奖励接口（ID=4410）
function Action4410(Scene,isLoading )

   	ZyWriter:writeString("ActionId",4410)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _4410Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}

       
    else          
         ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end




--        4411_排行榜接口（ID=4411）
function Action4411(Scene,isLoading )

   	ZyWriter:writeString("ActionId",4411)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _4411Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.RankId= ZyReader:getInt()
                mRecordTabel_1.NickName= ZyReader:readString()
                mRecordTabel_1.UserLv= ZyReader:getWORD()
                mRecordTabel_1.MaxTierNum= ZyReader:getInt()
                mRecordTabel_1.ScoreStar= ZyReader:getInt()
                mRecordTabel_1.HaveRankNum= ZyReader:getInt()
                mRecordTabel_1.UserID= ZyReader:readString()
                mRecordTabel_1.SJTRankType= ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;

       
    else          
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        12051_九宫格怪物接口（ID=12051）
function Action12051(Scene,isLoading ,PlotID)

   	ZyWriter:writeString("ActionId",12051)
   	ZyWriter:writeString("PlotID",PlotID)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _12051Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.PlotNpcID= ZyReader:getInt()
                mRecordTabel_1.HasMapCount= ZyReader:getWORD()
                mRecordTabel_1.Position= ZyReader:getWORD()
                mRecordTabel_1.Name= ZyReader:readString()
                mRecordTabel_1.HeadID= ZyReader:readString()
                mRecordTabel_1.Quality= ZyReader:getWORD()
                mRecordTabel_1.IsBox= ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;
        DataTabel.CurrentMapCount= ZyReader:getWORD()
        DataTabel.FragmentNum= ZyReader:getInt()
        DataTabel.ConsumeEnergy= ZyReader:getWORD()
        DataTabel.CurrentEnergy= ZyReader:getWORD()
        DataTabel.MaxEnergy= ZyReader:getWORD()
        DataTabel.PlayAnimat= ZyReader:getInt()

       
    else          
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        12052_守卫 Boss 界面接口（ID=12052）
function Action12052(Scene,isLoading ,PlotID)

   	ZyWriter:writeString("ActionId",12052)
   	ZyWriter:writeString("PlotID",PlotID)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _12052Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.PlotNpcID= ZyReader:getInt()
        DataTabel.Name= ZyReader:readString()
        DataTabel.Picture= ZyReader:readString()
        DataTabel.NpcSeqNo= ZyReader:getWORD()
        DataTabel.Level= ZyReader:getWORD()
        DataTabel.Quality= ZyReader:getWORD()
        DataTabel.ConsumeEnergy= ZyReader:getWORD()
        DataTabel.CurrentEnergy= ZyReader:getWORD()
        DataTabel.MaxEnergy= ZyReader:getWORD()
        DataTabel.ChallengeCount= ZyReader:getWORD()
        DataTabel.MaxChallengeCount= ZyReader:getWORD()
        DataTabel.HasNextBoss= ZyReader:getWORD()
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.ItemName= ZyReader:readString()
                mRecordTabel_1.ItemHead= ZyReader:readString()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;
	DataTabel.IsWin= ZyReader:getWORD()
       
    else          
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

-----12053 考古战斗详情接口

function Action12053(Scene,isLoading,PlotNpcID,MonsterPosition)
   	ZyWriter:writeString("ActionId",12053)
   	ZyWriter:writeString("PlotNpcID",PlotNpcID)
   	ZyWriter:writeString("MonsterPosition",MonsterPosition)
	ZyExecRequest(Scene, nil,isLoading)
end


function _12053Callback(pScutScene, lpExternalData)
   	local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.IsWin     = ZyReader:getWORD()
        DataTabel.BgScene= ZyReader:readString()
--        DataTabel.Experience   = ZyReader:getInt()
	
			-------------攻防阵型
		local RecordNums_2=ZyReader:getInt()
		local RecordTabel_2={}
		if RecordNums_2~=nil and RecordNums_2>0 then
		for  k=1, RecordNums_2 do
		local mRecordNums_2={}
		ZyReader:recordBegin()
		mRecordNums_2.AttGeneralID=ZyReader:getInt()
		mRecordNums_2.AttGeneralName=ZyReader:readString()
		mRecordNums_2.AttGeneralHeadID=ZyReader:readString()
		mRecordNums_2.AttPosition=ZyReader:getWORD()
		mRecordNums_2.LiveNum=ZyReader:getInt()
		mRecordNums_2.LiveMaxNum=ZyReader:getInt()
		mRecordNums_2.MomentumNum=ZyReader:getWORD()
		mRecordNums_2.MaxMomentumNum=ZyReader:getWORD()
		mRecordNums_2.AbilityID=ZyReader:getInt()
		mRecordNums_2.GeneralLv=ZyReader:getWORD()
		mRecordNums_2.IsAttReplace= ZyReader:getWORD()
		mRecordNums_2.AttGeneralQuality= ZyReader:getWORD()
		ZyReader:recordEnd()
		ZyTable.push_back(RecordTabel_2,mRecordNums_2)
		end
		end
		DataTabel.AgainstTable=RecordTabel_2
		    
		
		-------------防守阵型
		local RecordNums_3=ZyReader:getInt()
		local RecordTabel_3={}
		if RecordNums_3~=nil and RecordNums_3>0 then
			for  k=1, RecordNums_3 do
				local mRecordNums_3={}
				ZyReader:recordBegin()
				mRecordNums_3.AttGeneralID=ZyReader:getInt()
				mRecordNums_3.AttGeneralName=ZyReader:readString()
				mRecordNums_3.AttGeneralHeadID=ZyReader:readString()
				mRecordNums_3.AttPosition=ZyReader:getWORD()
				mRecordNums_3.LiveNum=ZyReader:getInt()
				mRecordNums_3.LiveMaxNum=ZyReader:getInt()
				mRecordNums_3.MomentumNum=ZyReader:getWORD()
				mRecordNums_3.MaxMomentumNum=ZyReader:getWORD()
				mRecordNums_3.AbilityID=ZyReader:getInt()
				mRecordNums_3.GeneralLv=ZyReader:getWORD()
				mRecordNums_3.IsAttReplace= ZyReader:getWORD()
				mRecordNums_3.AttGeneralQuality= ZyReader:getWORD()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_3,mRecordNums_3)
			end
		end
		DataTabel.DefendingTable=RecordTabel_3
		    
		    
		
		-----------------战斗过程
		local RecordNums_4=ZyReader:getInt()
		local RecordTabel_4={}
		if RecordNums_4~=nil and RecordNums_4>0 then
			for  k=1, RecordNums_4 do
				local mRecordNums_4={}
				ZyReader:recordBegin()
				mRecordNums_4.AttGeneralID=ZyReader:getInt()
				mRecordNums_4.AttGeneralLiveNum=ZyReader:getInt()
				mRecordNums_4.AttGeneralQishi=ZyReader:getWORD()
				mRecordNums_4.AttackTaget=ZyReader:getWORD()
				mRecordNums_4.AttackType=ZyReader:getWORD()
				mRecordNums_4.AbilityProperty=ZyReader:getWORD()
				mRecordNums_4.AttGeneralStatus=ZyReader:getWORD()
				mRecordNums_4.BackDamage=ZyReader:getInt()
				mRecordNums_4.AttEffectID=ZyReader:readString()
				mRecordNums_4.TargetEffectID=ZyReader:readString()
				mRecordNums_4.IsMove=ZyReader:getWORD()
				mRecordNums_4.Position=ZyReader:getWORD()
				mRecordNums_4.Role=ZyReader:getWORD()
			
				-----内嵌循环 中招效果开始
				local RecordNums_4_0=ZyReader:getInt()
				local RecordTabel_4_0={}
				if RecordNums_4_0~=nil and RecordNums_4_0>0 then
					for k=1, RecordNums_4_0 do
						local mRecordTabel_4_0={}
						ZyReader:recordBegin()
						mRecordTabel_4_0.GeneralEffect=ZyReader:getWORD()
						mRecordTabel_4_0.ConDamageNum=ZyReader:getInt()
						mRecordTabel_4_0.IsIncrease=ZyReader:getInt()
						ZyReader:recordEnd()
						ZyTable.push_back(RecordTabel_4_0,mRecordTabel_4_0)
					end
				end
				mRecordNums_4.GeneralEffects=RecordTabel_4_0
			
				-----内嵌循环 受到攻击的角色
				local RecordNums_4_1=ZyReader:getInt()
				local RecordTabel_4_1={}
				if RecordNums_4_1~=nil and RecordNums_4_1>0 then
					for k=1, RecordNums_4_1 do
						local mRecordTabel_4_1={}
						ZyReader:recordBegin()
						mRecordTabel_4_1.TargetGeneralID=ZyReader:getInt()
						mRecordTabel_4_1.TargetGeneralLiveNum=ZyReader:getInt()
						mRecordTabel_4_1.TargetGeneralQishi=ZyReader:getWORD()
						mRecordTabel_4_1.TargetDamageNum=ZyReader:getInt()
						mRecordTabel_4_1.IsShanBi=ZyReader:getWORD()
						mRecordTabel_4_1.IsGeDang=ZyReader:getWORD()
						mRecordTabel_4_1.IsFangji=ZyReader:getWORD()
						mRecordTabel_4_1.IsMove=ZyReader:getWORD()
						mRecordTabel_4_1.FangjiDamageNum=ZyReader:getInt()
						mRecordTabel_4_1.TargetStatus=ZyReader:getWORD()
						mRecordTabel_4_1.Position=ZyReader:getWORD()
						mRecordTabel_4_1.Role=ZyReader:getWORD()
						-----内嵌循环 中招效果开始
						local RecordNums_4_1_0=ZyReader:getInt()
						local RecordTabel_4_1_0={}
						if RecordNums_4_1_0~=nil and RecordNums_4_1_0>0 then
							for k=1, RecordNums_4_1_0 do
								local mRecordTabel_4_1_0={}
								ZyReader:recordBegin()
								mRecordTabel_4_1_0.GeneralEffect=ZyReader:getWORD()
								mRecordTabel_4_1_0.IsIncrease=ZyReader:getInt()
								ZyReader:recordEnd()
								ZyTable.push_back(RecordTabel_4_1_0,mRecordTabel_4_1_0)
							end
						end
						mRecordTabel_4_1.GeneralEffects=RecordTabel_4_1_0
						mRecordTabel_4_1.IsBaoji = ZyReader:getWORD()
				
				
						-----触发技能
						local RecordNums_4_1_1=ZyReader:getInt()
						local RecordTabel_4_1_1={}
						if RecordNums_4_1_1~=nil and RecordNums_4_1_1>0 then
							for k=1, RecordNums_4_1_1 do
								local mRecordTabel_4_1_1={}
								ZyReader:recordBegin()
								mRecordTabel_4_1_1.TrumpAbility=ZyReader:getWORD()
								mRecordTabel_4_1_1.TrumpNum=ZyReader:getInt()
								ZyReader:recordEnd()
								ZyTable.push_back(RecordTabel_4_1_1,mRecordTabel_4_1_1)
							end
						end                  
						mRecordTabel_4_1.TriggerTable=RecordTabel_4_1_1
						ZyReader:recordEnd()
						ZyTable.push_back(RecordTabel_4_1,mRecordTabel_4_1)
					end
				end
				mRecordNums_4.DefendFightTable=RecordTabel_4_1
				
				-----触发技能
				local RecordNums_4_2=ZyReader:getInt()
				local RecordTabel_4_2={}
				if RecordNums_4_2~=nil and RecordNums_4_2>0 then
					for k=1, RecordNums_4_2 do
						local mRecordTabel_4_2={}
						ZyReader:recordBegin()
						mRecordTabel_4_2.TrumpAbility=ZyReader:getWORD()
						mRecordTabel_4_2.TrumpNum=ZyReader:getInt()
						ZyReader:recordEnd()
						ZyTable.push_back(RecordTabel_4_2,mRecordTabel_4_2)
					end
				end                  
				mRecordNums_4.TriggerTable=RecordTabel_4_2
	
				mRecordNums_4.FntHeadID= ZyReader:readString()
				mRecordNums_4.AbilityID= ZyReader:getInt()


				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_4,mRecordNums_4)
			end
		end 
		DataTabel.FightProcessTable=RecordTabel_4
--		DataTabel.BlessExperience=ZyReader:getInt()
--		DataTabel.GotoNum=ZyReader:getInt()
		    
		    

	---------自身魂技效果 
		local RecordNums_5=ZyReader:getInt()
		local RecordTabel_5={}
		if RecordNums_5~=nil and RecordNums_5>0 then
			for  k=1, RecordNums_5 do
				local mRecordNums_3={}
				ZyReader:recordBegin()
				mRecordNums_5.AttGeneralID=ZyReader:getInt()
				mRecordNums_5.EffectID1=ZyReader:readString()
				mRecordNums_5.FntHeadID=ZyReader:readString()
				mRecordNums_5.IsIncrease=ZyReader:getWORD()
				mRecordNums_5.Role=ZyReader:getInt()
				mRecordNums_5.Position=ZyReader:getInt()
				
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_5,mRecordNums_5)
			end
		end
		DataTabel.FirstEffectTabel=RecordTabel_5	
		
        	DataTabel.UserTalPriority= ZyReader:getInt()
        	DataTabel.NpcTalPriority= ZyReader:getInt()
        	DataTabel.PlotNpcID= ZyReader:getInt()
        	DataTabel.GameCoin= ZyReader:getInt()
		DataTabel.Gold= ZyReader:getInt()
		DataTabel.AgentNum= ZyReader:getInt()
		DataTabel.HonourNum= ZyReader:getInt()
		DataTabel.StarNum= ZyReader:getWORD()
		
		local RecordNums_1=ZyReader:getInt()
	         local RecordTabel_1={}
	          if RecordNums_1~=0 then
	            for k=1,RecordNums_1 do
	             local mRecordTabel_1={}
	             ZyReader:recordBegin()
	                mRecordTabel_1.RewardInfo= ZyReader:readString()
	                mRecordTabel_1.Num= ZyReader:getInt()
	                mRecordTabel_1.Picture= ZyReader:readString()
	                ZyReader:recordEnd()
	                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
	              end
	        end
	                DataTabel.RecordTabel = RecordTabel_1;
	                DataTabel.PlotName= ZyReader:readString()
	                DataTabel.MaxHonourNum= ZyReader:getInt()
	                DataTabel.CurrentHonour= ZyReader:getInt()
	                DataTabel.LastMaxHonourNum= ZyReader:getInt()
	                DataTabel.IsUpgrade= ZyReader:getInt()


	else
	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        12055_考古系统购买挑战次数接口（ID=12055）
function Action12055(Scene,isLoading ,PlotNpcID)

   	ZyWriter:writeString("ActionId",12055)
   	ZyWriter:writeString("PlotNpcID",PlotNpcID)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _12055Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}

       
    else          
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end


--        12056_考古系统开启宝箱接口（ID=12056）
function Action12056(Scene,isLoading ,PlotID,PlotNpcID)

   	ZyWriter:writeString("ActionId",12056)
   	ZyWriter:writeString("PlotID",PlotID)
        ZyWriter:writeString("PlotNpcID",PlotNpcID)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _12056Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.RewardInfo= ZyReader:readString()
        DataTabel.Num= ZyReader:getInt()
        DataTabel.Picture= ZyReader:readString()

       
    else          
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end


--        12101_龙穴取宝界面接口（ID=12101）
function Action12101(Scene,isLoading ,LairTreasureType)

   	ZyWriter:writeString("ActionId",12101)
   	ZyWriter:writeString("LairTreasureType",LairTreasureType)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _12101Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.Postion= ZyReader:getInt()
                mRecordTabel_1.HeadID= ZyReader:readString()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;
        DataTabel.LastNum= ZyReader:getInt()
        DataTabel.ActivityEndTime= ZyReader:readString()
        DataTabel.ExpendNum= ZyReader:getInt()
        DataTabel.HaveNum= ZyReader:getInt()
        DataTabel.UserType= ZyReader:getInt()
        DataTabel.MaxNum= ZyReader:getInt()

       
    else          
         ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

--        12102_龙穴取宝接口（ID=12102）
function Action12102(Scene,isLoading ,LairTreasureType)

   	ZyWriter:writeString("ActionId",12102)
   	ZyWriter:writeString("LairTreasureType",LairTreasureType)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _12102Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.postion= ZyReader:getInt()
        DataTabel.LairRewardName= ZyReader:readString()
        DataTabel.LairRewardNum= ZyReader:getInt()
        DataTabel.LairRewardHead= ZyReader:readString()
        DataTabel.LairRewardType= ZyReader:getInt()
        DataTabel.ItemId= ZyReader:getInt()

       
    else          
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end



--        12057_地图列表接口（ID=12057）
function Action12057(Scene,isLoading )

   	ZyWriter:writeString("ActionId",12057)
   	
	ZyExecRequest(Scene, nil,isLoading)
end

function _12057Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.PlotID= ZyReader:getInt()
                mRecordTabel_1.PlotMapName= ZyReader:readString()
                mRecordTabel_1.BossHeadID= ZyReader:readString()
                mRecordTabel_1.KgScene= ZyReader:readString()
                mRecordTabel_1.IsActive= ZyReader:getWORD()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;

       
    else          
          ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
    end
    return DataTabel
end

