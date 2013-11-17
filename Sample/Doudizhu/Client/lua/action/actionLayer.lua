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
--CCLuaLog("Module ".. strModuleName.. " loaded.");
strModuleName = nil;


function Action360(Scene,isLoading ,RefeshToken,RetailID,Scope)
	ZyWriter:writeString("ActionId",360)
	ZyWriter:writeString("RefeshToken",RefeshToken  )
	ZyWriter:writeString("RetailID",RetailID)
	ZyWriter:writeString("Scope",Scope)
	ZyExecRequest(Scene, nil,isLoading)
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
	ZyExecRequest(Scene, nil,isLoading,1)
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
function Action1002(Scene,isLoading ,MobileType,GameType,RetailID,ClientAppVersion,ScreenX,ScreenY,DeviceID,ServerID )
	ZyWriter:writeString("ActionId",1002)
   	ZyWriter:writeString("MobileType",MobileType)
        ZyWriter:writeString("GameType",GameType)
        ZyWriter:writeString("RetailID",RetailID)
        ZyWriter:writeString("ClientAppVersion",ClientAppVersion)
        ZyWriter:writeString("ScreenX",ScreenX)
        ZyWriter:writeString("ScreenY",ScreenY)
        ZyWriter:writeString("DeviceID",DeviceID)
        ZyWriter:writeString("ServerID",ServerID )
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
Code
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

    	end
    	return DataTabel
end

--        1005_创建角色（ID=1005）
function Action1005(Scene,isLoading ,UserName,Sex,HeadID,RetailID,Pid,MobileType,ScreenX,ScreenY,ClientAppVersion,GameID,ServerID)

   	ZyWriter:writeString("ActionId",1005)
   	ZyWriter:writeString("UserName",UserName)
        ZyWriter:writeString("Sex",Sex)
        ZyWriter:writeString("HeadID",HeadID)
        ZyWriter:writeString("RetailID",RetailID)
        ZyWriter:writeString("Pid",Pid)
        ZyWriter:writeString("MobileType",MobileType)
        ZyWriter:writeString("ScreenX",ScreenX)
        ZyWriter:writeString("ScreenY",ScreenY)
        ZyWriter:writeString("ClientAppVersion",ClientAppVersion)
        ZyWriter:writeString("GameID",GameID)
        ZyWriter:writeString("ServerID",ServerID)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _1005Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
    else          
         ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.2)
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
          local box = ZyMessageBoxEx:new()
	      box:doPrompt(pScutScene, nil, ZyReader:readErrorMsg(),Language.IDS_OK)
    end
    return DataTabel
end

--        1008_主界面接口（ID=1008）
function Action1008(Scene,isLoading )
   	ZyWriter:writeString("ActionId",1008)
	ZyExecRequest(Scene, nil,isLoading)
end

function Action100(Scene,isLoading,ops )
   	ZyWriter:writeString("ActionId",100)
   	ZyWriter:writeString("ops",ops)
	ZyExecRequest(Scene, nil,isLoading)
end


function _1008Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.Pid= ZyReader:readString()
        DataTabel.HeadIcon= ZyReader:readString()
        DataTabel.NickName= ZyReader:readString()
        DataTabel.GameCoin= ZyReader:getInt()
        DataTabel.Gold= ZyReader:getInt()
        DataTabel.VipLv= ZyReader:getInt()
        DataTabel.WinNum= ZyReader:getInt()
        DataTabel.FailNum= ZyReader:getInt()
        DataTabel.TitleName= ZyReader:readString()
        DataTabel.ScoreNum= ZyReader:getInt()
        DataTabel.WinRate= ZyReader:getInt()
        local RecordNums_1=ZyReader:getInt()
         local roomTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRoomTabel_1={}
             ZyReader:recordBegin()
                mRoomTabel_1.RoomId= ZyReader:getInt()
                mRoomTabel_1.RoomName= ZyReader:readString()
                mRoomTabel_1.RoomMultiple= ZyReader:getWORD()
                mRoomTabel_1.MinCion= ZyReader:getInt()
                mRoomTabel_1.GiffCion= ZyReader:getInt()
                mRoomTabel_1.Description= ZyReader:readString()
                mRoomTabel_1.AnteNum= ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(roomTabel_1,mRoomTabel_1)
              end
        end
                DataTabel.roomTabel = roomTabel_1;
                DataTabel.RoomId  = ZyReader:getInt();
    else          
          local box = ZyMessageBoxEx:new()
	      box:doPrompt(pScutScene, nil, ZyReader:readErrorMsg(),Language.IDS_OK)
    end
    return DataTabel
end

--        1009_社交资料接口【完成】（ID=1009）
function Action1009(Scene,isLoading )

   	ZyWriter:writeString("ActionId",1009)
   	
	ZyExecRequest(Scene, nil,isLoading)
end

function _1009Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.Name= ZyReader:readString()
        DataTabel.Sex= ZyReader:getByte()
        DataTabel.Birthday= ZyReader:readString()
        DataTabel.Hobby= ZyReader:readString()
        DataTabel.Profession= ZyReader:readString()
    else          
          local box = ZyMessageBoxEx:new()
	      box:doPrompt(pScutScene, nil, ZyReader:readErrorMsg(),Language.IDS_OK)
    end
    return DataTabel
end

--        1010_更换头像接口【完成】（ID=1010）
function Action1010(Scene,isLoading ,HeadIcon)

   	ZyWriter:writeString("ActionId",1010)
   	ZyWriter:writeString("HeadIcon",HeadIcon)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _1010Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}

       
    else          
          local box = ZyMessageBoxEx:new()
	      box:doPrompt(pScutScene, nil, ZyReader:readErrorMsg(),Language.IDS_OK)
    end
    return DataTabel
end

--        1011_社交资料保存接口【完成】（ID=1011）
function Action1011(Scene,isLoading ,Name,Sex,Birthday,Profession,Hobby)

   	ZyWriter:writeString("ActionId",1011)
   	ZyWriter:writeString("Name",Name)
        ZyWriter:writeString("Sex",Sex)
        ZyWriter:writeString("Birthday",Birthday)
        ZyWriter:writeString("Profession",Profession)
        ZyWriter:writeString("Hobby",Hobby)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _1011Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}

       
    else          
          local box = ZyMessageBoxEx:new()
	      box:doPrompt(pScutScene, nil, ZyReader:readErrorMsg(),Language.IDS_OK)
    end
    return DataTabel
end

--        1013_背包物品列表通知接口【未完成】（ID=1013）
function Action1013(Scene,isLoading ,PageIndex,PageSize)

   	ZyWriter:writeString("ActionId",1013)
   	ZyWriter:writeString("PageIndex",PageIndex)
        ZyWriter:writeString("PageSize",PageSize)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _1013Callback(pScutScene, lpExternalData)
   
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
                mRecordTabel_1.ItemType= ZyReader:getWORD()
                mRecordTabel_1.Num= ZyReader:getInt()
                mRecordTabel_1.HeadID= ZyReader:readString()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;

       
    else          
          local box = ZyMessageBoxEx:new()
	      box:doPrompt(pScutScene, nil, ZyReader:readErrorMsg(),Language.IDS_OK)
    end
    return DataTabel
end

--        1014_用户信息变更通知接口【未完成】（ID=1014）
function Action1014(Scene,isLoading ,GameType,ServerID)

   	ZyWriter:writeString("ActionId",1014)
   	ZyWriter:writeString("GameType",GameType)
        ZyWriter:writeString("ServerID",ServerID)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _1014Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.HeadIcon= ZyReader:readString()
        DataTabel.GameCoin= ZyReader:getInt()
        DataTabel.Gold= ZyReader:getInt()
        DataTabel.VipLv= ZyReader:getInt()
        DataTabel.WinNum= ZyReader:getInt()
        DataTabel.FailNum= ZyReader:getInt()
        DataTabel.TitleName= ZyReader:readString()
        DataTabel.ScoreNum= ZyReader:getInt()

       
    else          
          local box = ZyMessageBoxEx:new()
	      box:doPrompt(pScutScene, nil, ZyReader:readErrorMsg(),Language.IDS_OK)
    end
    return DataTabel
end


--        1019_排行榜接口【完成】（ID=1019）
function Action1019(Scene,isLoading ,RankType,PageIndex,PageSize)

   	ZyWriter:writeString("ActionId",1019)
   	ZyWriter:writeString("RankType",RankType)
        ZyWriter:writeString("PageIndex",PageIndex)
        ZyWriter:writeString("PageSize",PageSize)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _1019Callback(pScutScene, lpExternalData)
   
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
                mRecordTabel_1.RankID= ZyReader:getInt()
                mRecordTabel_1.UserID= ZyReader:getInt()
                mRecordTabel_1.NickName= ZyReader:readString()
                mRecordTabel_1.GameCoin= ZyReader:getInt()
                mRecordTabel_1.Wining= ZyReader:readString()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;
    else          
        ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.2)
    end
    return DataTabel
end

--        1069_AppStore充值详情接口【完成】（ID=1069）
function Action1069(Scene,isLoading ,MobileType,GameID)

   	ZyWriter:writeString("ActionId",1069)
   	ZyWriter:writeString("MobileType",MobileType)
        ZyWriter:writeString("GameID",GameID)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _1069Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
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
                DataTabel.RecordTabel = RecordTabel_1;

       
    else          
          local box = ZyMessageBoxEx:new()
	      box:doPrompt(pScutScene, nil, ZyReader:readErrorMsg(),Language.IDS_OK)
    end
    return DataTabel
end



--     
function Action2001(Scene,isLoading,RoomId, Op )
   	ZyWriter:writeString("ActionId",2001)
   	ZyWriter:writeString("RoomId",RoomId)
   	ZyWriter:writeString("Op",Op)
   	local userdata=string.format('%d',RoomId)
	ZyExecRequest(Scene, userdata,isLoading)
end


function Action2002(Scene,isLoading )
   	ZyWriter:writeString("ActionId",2002)
	ZyExecRequest(Scene, nil,isLoading)
end

--下发玩家数据
function _2003Callback(pScutScene, lpExternalData)
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
                mRecordTabel_1.NickName= ZyReader:readString()
                mRecordTabel_1.HeadIcon= ZyReader:readString()
                mRecordTabel_1.PosId= ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.PlayerTable = RecordTabel_1;
    else          
        ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.2)
    end
    return DataTabel
end


function _2004Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.LandlordId= ZyReader:getInt()
        DataTabel.LandlordName= ZyReader:readString()
        DataTabel.CodeTime= ZyReader:getInt()
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
	if RecordNums_1~=0 then
		for k=1,RecordNums_1 do
		local mRecordTabel_1={}
		ZyReader:recordBegin()
		mRecordTabel_1.CardId= ZyReader:getInt()
		ZyReader:recordEnd()
		ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
		end
	end
	DataTabel.OpenTable = RecordTabel_1;
	local RecordNums_2=ZyReader:getInt()
	local RecordTabel_2={}
	if RecordNums_2~=0 then
		for k=1,RecordNums_2 do
		local mRecordTabel_2={}
		ZyReader:recordBegin()
		mRecordTabel_2.CardId= ZyReader:getInt()
		ZyReader:recordEnd()
		ZyTable.push_back(RecordTabel_2,mRecordTabel_2)
		end
	end
                DataTabel.PlayerCardTable = RecordTabel_2;  
    else          
        ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.2)
    end
    return DataTabel
end


function Action2005(Scene,isLoading ,op)
   	ZyWriter:writeString("ActionId",2005)
   	ZyWriter:writeString("op",op)
	ZyExecRequest(Scene, op,isLoading)
end


function _2006Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.IsEnd= ZyReader:getByte()
        DataTabel.LandlordId= ZyReader:getInt()
        DataTabel.LandlordName= ZyReader:readString()
        DataTabel.MultipleNum= ZyReader:getInt()
        DataTabel.AnteNum= ZyReader:getInt()
        DataTabel.IsCall= ZyReader:getByte()    
        DataTabel.IsRob= ZyReader:getByte()         
    else          
        ZyToast.show(pScutScene, "2006".. ZyReader:readErrorMsg(),1,0.2)
    end
    return DataTabel  
end

function Action2007(Scene,isLoading ,Op)
   	ZyWriter:writeString("ActionId",2007)
   	ZyWriter:writeString("Op",Op )
	ZyExecRequest(Scene, nil,isLoading)
end


--
function _2008Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.CardId= ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;     
                DataTabel.MultipleNum  =ZyReader:getInt()
                DataTabel.AnteNum  = ZyReader:getInt()
    else          
		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.2)
    end
    return DataTabel
end


function Action2009(Scene,isLoading ,Cards)
   	ZyWriter:writeString("ActionId",2009)
   	ZyWriter:writeString("Cards",Cards)
	ZyExecRequest(Scene, nil,isLoading)
end

function _2010Callback(pScutScene, lpExternalData)  
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.UserId= ZyReader:getInt()
		DataTabel.NextUserId= ZyReader:getInt()
		DataTabel.DeckType= ZyReader:getWORD()
		DataTabel.CardSize= ZyReader:getInt()
		local RecordNums_1=ZyReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
			local mRecordTabel_1={}
			ZyReader:recordBegin()
			mRecordTabel_1.CardId= ZyReader:getInt()
			ZyReader:recordEnd()
			ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
			end
		end
                DataTabel.RecordTabel = RecordTabel_1;      
                DataTabel.IsReNew  = ZyReader:getByte();      
                DataTabel.MultipleNum   = ZyReader:getInt();      
                DataTabel.AnteNum   = ZyReader:getInt();   
    else          
		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.2)
    end
    return DataTabel
end



function Action2011(Scene,isLoading ,Op)
   	ZyWriter:writeString("ActionId",2011)
   	ZyWriter:writeString("Op",Op )
	ZyExecRequest(Scene, nil,isLoading)
end



--        3001_成就界面接口【完成】（ID=3001）
function Action3001(Scene,isLoading ,AchieveType,PageIndex,PageSize)

   	ZyWriter:writeString("ActionId",3001)
   	ZyWriter:writeString("AchieveType",AchieveType)
        ZyWriter:writeString("PageIndex",PageIndex)
        ZyWriter:writeString("PageSize",PageSize)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _3001Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.CompleteNum= ZyReader:getInt()
        DataTabel.AchieveNum= ZyReader:getInt()
        DataTabel.PageCount= ZyReader:getInt()
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.AchieveID= ZyReader:getInt()
                mRecordTabel_1.AchieveName= ZyReader:readString()
                mRecordTabel_1.HeadID= ZyReader:readString()
                mRecordTabel_1.IsGain= ZyReader:getWORD()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;

       
    else          
          local box = ZyMessageBoxEx:new()
	      box:doPrompt(pScutScene, nil, ZyReader:readErrorMsg(),Language.IDS_OK)
    end
    return DataTabel
end

--        3002_成就详情接口【完成】（ID=3002）
function Action3002(Scene,isLoading ,AchieveID)

   	ZyWriter:writeString("ActionId",3002)
   	ZyWriter:writeString("AchieveID",AchieveID)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _3002Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.AchieveName= ZyReader:readString()
        DataTabel.AchieveType= ZyReader:getWORD()
        DataTabel.HeadID= ZyReader:readString()
        DataTabel.AchieveDesc= ZyReader:readString()
        DataTabel.IsComplete= ZyReader:getWORD()
        DataTabel.CompleteNum= ZyReader:getInt()
        DataTabel.AchieveNum= ZyReader:getInt()

       
    else          
          local box = ZyMessageBoxEx:new()
	      box:doPrompt(pScutScene, nil, ZyReader:readErrorMsg(),Language.IDS_OK)
    end
    return DataTabel
end


function _2012Callback(pScutScene, lpExternalData)
    local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.IsLandlord= ZyReader:getByte()
		DataTabel.IsLandlordWin= ZyReader:getByte()
		DataTabel.ScoreNum= ZyReader:getInt()
		DataTabel.CoinNum= ZyReader:getInt()
		DataTabel.GameCoin= ZyReader:getInt()
		local RecordNums_1=ZyReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
				local mRecordTabel_1={}
				ZyReader:recordBegin()
				mRecordTabel_1.UserId= ZyReader:getInt()
				local RecordNums_2=ZyReader:getInt()
				local RecordTabel_2={}
				if RecordNums_2~=0 then
					for k=1,RecordNums_2 do
						local mRecordTabel_2={}
						ZyReader:recordBegin()
						mRecordTabel_2.CardId= ZyReader:getInt()
						ZyReader:recordEnd()
						ZyTable.push_back(RecordTabel_2,mRecordTabel_2)
					end
				end
				mRecordTabel_1.cardTable = RecordTabel_2;
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.RecordTabel = RecordTabel_1;
		
		local RecordNums_3=ZyReader:getInt()
		local RecordTabel_3={}
		if RecordNums_3~=0 then
		for k=1,RecordNums_3 do
			local mRecordTabel_3={}
			ZyReader:recordBegin()
			mRecordTabel_3.CardId= ZyReader:getInt()
			ZyReader:recordEnd()
			ZyTable.push_back(RecordTabel_3,mRecordTabel_3)
		end
		end	
		DataTabel.lastCardTable=RecordTabel_3
		DataTabel.LastUserId= ZyReader:getInt()
		DataTabel.MultipleNum= ZyReader:getInt()
		DataTabel.AnteNum= ZyReader:getInt()	 
    else          
		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.2)
    end
    return DataTabel
end



function _2013Callback(pScutScene, lpExternalData)

    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.FleeUserId= ZyReader:getInt()
        DataTabel.FleeNickName= ZyReader:readString()
        DataTabel.GameCoin= ZyReader:getInt()
        DataTabel.ScoreNum= ZyReader:getInt()
        DataTabel.InsScoreNum= ZyReader:getInt()
        DataTabel.InsCoinNum= ZyReader:getInt()  
    else          
		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.2)
    end
    return DataTabel
end


function _2014Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.UserId= ZyReader:getInt()
        DataTabel.Status= ZyReader:getByte()
    else          
		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.2)
    end
    return DataTabel
end



function _2015Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.MultipleNum= ZyReader:getInt()
        DataTabel.AnteNum= ZyReader:getInt()
        DataTabel.IsAI= ZyReader:getByte()
	DataTabel.IsShow= ZyReader:getByte()
	DataTabel.LandlordId= ZyReader:getInt()
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
         if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
		local mRecordTabel_1={}
		ZyReader:recordBegin()
		mRecordTabel_1.UserId= ZyReader:getInt()
		mRecordTabel_1.NickName= ZyReader:readString()
		mRecordTabel_1.HeadIcon= ZyReader:readString()
		mRecordTabel_1.PosId= ZyReader:getInt()

		local RecordNums_2=ZyReader:getInt()
		local RecordTabel_2={}
		if RecordNums_2~=0 then
			for k=1,RecordNums_2 do
				local mRecordTabel_2={}
				ZyReader:recordBegin()
				mRecordTabel_2.CardId= ZyReader:getInt()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_2,mRecordTabel_2)
			end
		end
		mRecordTabel_1.CardTable = RecordTabel_2;
		ZyReader:recordEnd()
		ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
             end
        end
         DataTabel.playerTable = RecordTabel_1;
        local RecordNums_3=ZyReader:getInt()
         local RecordTabel_3={}
          if RecordNums_3~=0 then
            for k=1,RecordNums_3 do
             local mRecordTabel_3={}
             ZyReader:recordBegin()
                mRecordTabel_3.CardId= ZyReader:getInt()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_3,mRecordTabel_3)
              end
        end
               DataTabel.LandLordCard = RecordTabel_3;
               DataTabel.CodeTime =ZyReader:getInt()
               DataTabel.OutCardUserId =ZyReader:getInt()
               DataTabel.IsReNew =ZyReader:getByte()
               DataTabel.GameCoin =ZyReader:getInt()
		local RecordNums_4=ZyReader:getInt()
		 local RecordTabel_4={}
		  if RecordNums_4~=0 then
		    for k=1,RecordNums_4 do
		     local mRecordTabel_4={}
		     ZyReader:recordBegin()
		        mRecordTabel_4.CardId= ZyReader:getInt()
		        ZyReader:recordEnd()
		        ZyTable.push_back(RecordTabel_4,mRecordTabel_4)
		      end
		end
              DataTabel.RecordTabel = RecordTabel_4;
		DataTabel.DeckType= ZyReader:getWORD()
		DataTabel.CardSize= ZyReader:getInt()
		DataTabel.UserId = ZyReader:getInt()
               
    else          
		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.2)
    end
    return DataTabel
end


--        3003_任务界面接口【未完成】（ID=3003）
function Action3003(Scene,isLoading ,TaskType,PageIndex,PageSize)

   	ZyWriter:writeString("ActionId",3003)
   	ZyWriter:writeString("TaskType",TaskType)
        ZyWriter:writeString("PageIndex",PageIndex)
        ZyWriter:writeString("PageSize",PageSize)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _3003Callback(pScutScene, lpExternalData)
   
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
                mRecordTabel_1.TaskID= ZyReader:getInt()
                mRecordTabel_1.TaskName= ZyReader:readString()
                mRecordTabel_1.CompleteNum= ZyReader:getInt()
                mRecordTabel_1.TaskNum= ZyReader:getInt()
                mRecordTabel_1.TaskDesc= ZyReader:readString()
                mRecordTabel_1.GameCoin= ZyReader:getInt()
                mRecordTabel_1.IsReceive= ZyReader:getWORD()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;

       
    else          
          local box = ZyMessageBoxEx:new()
	      box:doPrompt(pScutScene, nil, ZyReader:readErrorMsg(),Language.IDS_OK)
    end
    return DataTabel
end


--        3004_任务奖励领取接口【未完成】（ID=3004）
function Action3004(Scene,isLoading ,TaskID)

   	ZyWriter:writeString("ActionId",3004)
   	ZyWriter:writeString("TaskID",TaskID)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _3004Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}

       
    else          
          local box = ZyMessageBoxEx:new()
	      box:doPrompt(pScutScene, nil, ZyReader:readErrorMsg(),Language.IDS_OK)
    end
    return DataTabel
end


--        7001_商店物品列表接口【未完成】（ID=7001）
function Action7001(Scene,isLoading ,ShopType,PageIndex,PageSize)

   	ZyWriter:writeString("ActionId",7001)
   	ZyWriter:writeString("ShopType",ShopType)
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
                mRecordTabel_1.ItemPrice= ZyReader:getInt()
                mRecordTabel_1.VipPrice= ZyReader:getInt()
                mRecordTabel_1.GainGameCoin= ZyReader:getInt()
                mRecordTabel_1.ShopDesc= ZyReader:readString()
                mRecordTabel_1.SeqNO= ZyReader:getWORD()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;

       
    else          
          local box = ZyMessageBoxEx:new()
	      box:doPrompt(pScutScene, nil, ZyReader:readErrorMsg(),Language.IDS_OK)
    end
    return DataTabel
end

--        7002_商店物品购买接口【完成】（ID=7002）
function Action7002(Scene,isLoading ,ItemID)

   	ZyWriter:writeString("ActionId",7002)
   	ZyWriter:writeString("ItemID",ItemID)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _7002Callback(pScutScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
    else          
		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.2)
    end
    return DataTabel

end

--        9001_即时聊天列表接口（ID=9001）
function Action9001(Scene,isLoading )
   	ZyWriter:writeString("ActionId",9001)
	ZyExecRequest(Scene, nil,isLoading)
end




function _9001Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.ChatID= ZyReader:getInt()
                mRecordTabel_1.ChatContent= ZyReader:readString()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;
    else          
        ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.2)
    end
    return DataTabel
end
--        9002_聊天发送接口（ID=9002）
function Action9002(Scene,isLoading ,ChatID)

   	ZyWriter:writeString("ActionId",9002)
   	ZyWriter:writeString("ChatID",ChatID)
	ZyExecRequest(Scene, nil,isLoading)
end

function _9002Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
    else          
        ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.2)
    end
    return DataTabel
end
--        9003_聊天记录列表接口【完成】（ID=9003）
function Action9003(Scene,isLoading )

   	ZyWriter:writeString("ActionId",9003)
   	
	ZyExecRequest(Scene, nil,isLoading)
end

function _9003Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        DataTabel.ChatMaxNum= ZyReader:getInt()
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
         local tNum={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.UserID= ZyReader:getInt()
                mRecordTabel_1.UserName= ZyReader:readString()
                mRecordTabel_1.ChatID= ZyReader:getInt()
                mRecordTabel_1.Content= ZyReader:readString()
                mRecordTabel_1.SendDate= ZyReader:readString()
                ZyReader:recordEnd()
                ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
                ----1 是全部
                PrivateChatLayer.appendLocalMessage(1, mRecordTabel_1)	
                   ---单个频道进入多少条数据
                   if tNum[1] then
                        tNum[1] = tNum[1]+1
                    else
                        tNum[1] = 1;
                    end
              end
        end
        -------------------
        if RecordNums_1>0 then
            local tMessage = PrivateChatLayer.getLocalMessage(1)
            if #tMessage > 0 then 
                if RecordNums_1 > 100 then
                    RecordNums_1 = 100
                 end
            end
  			 ------
			 for index=#tMessage-RecordNums_1+1 , #tMessage  do
			   --[[ if index==#tMessage then
 			        MainMenuLayer.rebuildMessage()--添加主界面聊天消息
			    end]]
			 	if ChatLayer.getIsClick2() then
					ChatLayer.addMsg(1,index)
			    else
                        if MainDesk.getSendState() and MainDesk.getLayer() and MainDesk.getChatPlayerTable() then
                            local table=MainDesk.getChatPlayerTable()
                            local layer=MainDesk.getLayer()
                             local mtable=PrivateChatLayer.getLocalMessage(1)[index]
                            for j,v1 in pairs(table) do 
                              if mtable.UserID==v1.UserId then
                                local pos =v1.headSprite:getPosition().x+v1.headSprite:getContentSize().width
                                local layer2=nil
                                 if pos >pWinSize.width/2 then
                                    layer2=ZyToast.createToast2(nil,0.8,0.35,mtable.Content,true) 
                                    layer2:setPosition(PT(v1.headSprite:getPosition().x,v1.headSprite:getPosition().y))
                                    layer:addChild(layer2,3)
                                 else
                                    layer2=ZyToast.createToast2(nil,0.8,0.35,mtable.Content) 
                                    layer2:setPosition(PT(v1.headSprite:getPosition().x+v1.headSprite:getContentSize().width,v1.headSprite:getPosition().y))
                                    layer:addChild(layer2,3)
                                 end

                              end
                            end 
                        end
				end
			 end
        end
        DataTabel.RecordTabel = RecordTabel_1;
    else          
        ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.2)
    end
    return DataTabel
end
--        9202_公告广播通知接口【未完成】（ID=9202）
function Action9202(Scene,isLoading ,GameType,ServerID)

   	ZyWriter:writeString("ActionId",9202)
   	ZyWriter:writeString("GameType",GameType)
        ZyWriter:writeString("ServerID",ServerID)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _9202Callback(pScutScene, lpExternalData)
   
    local DataTabel=nil
    if ZyReader:getResult() == eScutNetSuccess then
        DataTabel={}
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.BroadcastType= ZyReader:getWORD()
                mRecordTabel_1.Content= ZyReader:readString()
                mRecordTabel_1.PlayTimes= ZyReader:getInt()
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

--        9203_公告列表接口【未完成】（ID=9203）
function Action9203(Scene,isLoading ,PageIndex,PageSize)

   	ZyWriter:writeString("ActionId",9203)
   	ZyWriter:writeString("PageIndex",PageIndex)
        ZyWriter:writeString("PageSize",PageSize)
        
	ZyExecRequest(Scene, nil,isLoading)
end

function _9203Callback(pScutScene, lpExternalData)
   
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


--        12001_转盘界面接口【完成】（ID=12001）
function Action12001(Scene,isLoading )
	ZyWriter:writeString("ActionId",12001)
	ZyExecRequest(Scene, nil,isLoading)
end

function _12001Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.IsFree= ZyReader:getWORD()
		DataTabel.FreeNum= ZyReader:getInt()
		local RecordNums_1=ZyReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
				local mRecordTabel_1={}
				ZyReader:recordBegin()
				mRecordTabel_1.Postion= ZyReader:getInt()
				mRecordTabel_1.HeadID= ZyReader:readString()
				mRecordTabel_1.Probability= ZyReader:readString()
				mRecordTabel_1.ItemDesc= ZyReader:readString()
				mRecordTabel_1.GameCoin= ZyReader:getInt()
				ZyReader:recordEnd()
				ZyTable.push_back(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.RecordTabel = RecordTabel_1; 
		DataTabel.UserCoin= ZyReader:getInt()
		DataTabel.UserGold= ZyReader:getInt()

	else          
		local box = ZyMessageBoxEx:new()
		box:doPrompt(pScutScene, nil, ZyReader:readErrorMsg(),Language.IDS_OK)
	end
	return DataTabel
end

--        12002_抽奖接口【完成】（ID=12002）
function Action12002(Scene,isLoading ,Ops)
	ZyWriter:writeString("ActionId",12002)
	ZyWriter:writeString("Ops",Ops)
	ZyExecRequest(Scene, nil,isLoading)
end

function _12002Callback(pScutScene, lpExternalData)
	local DataTabel=nil
	if ZyReader:getResult() == eScutNetSuccess then
		DataTabel={}
		DataTabel.Postion= ZyReader:getWORD()
		DataTabel.RewardContent= ZyReader:readString()
		DataTabel.FreeNum= ZyReader:getInt()
		DataTabel.UserCoin= ZyReader:getInt()
		DataTabel.UserGold= ZyReader:getInt()		
	else          
		local box = ZyMessageBoxEx:new()
		box:doPrompt(pScutScene, nil, ZyReader:readErrorMsg(),Language.IDS_OK)
	end
	return DataTabel
end
