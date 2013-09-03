
--ScutReader = ScutReader:new()

function Action100(Scene,isLoading,ops )
   	ScutWriter:writeString("ActionId",100)
   	ScutWriter:writeString("ops",ops)
	
end

function Action1004(token)
	ScutWriter:writeString("MobileType", token.MobileType)
	ScutWriter:writeString("Pid", token.Pid)
	ScutWriter:writeString("Pwd", token.Pwd)
	ScutWriter:writeString("DeviceID", token.DeviceID)
	ScutWriter:writeString("GameType", token.GameType)
	ScutWriter:writeString("ScreenX", "400")
	ScutWriter:writeString("ScreenY", "300")
	ScutWriter:writeString("RetailID", token.RetailID)
	ScutWriter:writeString("ServerID", token.ServerID)
	ScutWriter:writeString("RetailUser", "")
	ScutWriter:writeString("ClientAppVersion", "1.0")
	ScutWriter:writeString("Code","")

end

function _1004Callback(token)
    local DataTabel=nil

    if ScutReader:getResult() then
        DataTabel={}
        DataTabel.SessionID= ScutReader:readString()
        DataTabel.UserID= ScutReader:readString()
        DataTabel.UserType= ScutReader:getInt()
        DataTabel.LoginTime= ScutReader:readString()
        DataTabel.GuideID= ScutReader:getInt()
        DataTabel.PassportId= ScutReader:readString()
        DataTabel.RefeshToken= ScutReader:readString()
        DataTabel.QihooUserID= ScutReader:readString()
        DataTabel.Scope= ScutReader:readString()
        
        token.Uid = DataTabel.UserID
        token.Sid = DataTabel.SessionID
    else
        LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
    end
    return DataTabel
end

--        1005_创建角色接口【完成】（ID=1005）
function Action1005(token)
	ScutWriter:writeString("ActionId",1005)
	local num = math.random(99,99999999)
	ScutWriter:writeString("UserName","AA"..num)
	ScutWriter:writeString("Sex",0)
	ScutWriter:writeString("HeadID","head_1001")
	ScutWriter:writeString("RetailID",token.RetailID)
	ScutWriter:writeString("Pid", token.MobileType)
	ScutWriter:writeString("MobileType", token.MobileType)
	ScutWriter:writeString("ScreenX","400")
	ScutWriter:writeString("ScreenY","300")
	ScutWriter:writeString("ClientAppVersion","1.0")
	ScutWriter:writeString("GameID", token.GameType)
	ScutWriter:writeString("ServerID", token.ServerID)

end

function _1005Callback(token)
	local DataTabel=nil
	if ScutReader:getResult() then
		DataTabel={}
	else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
	end
	return DataTabel
end


--        1006_密码更新接口（ID=1006）
function Action1006(token)
   	ScutWriter:writeString("ActionId",1006)
   	ScutWriter:writeString("PassWord","1111")
end

function _1006Callback(token)
	local DataTabel=nil
	if ScutReader:getResult() then
		DataTabel={}
	else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
	end
	return DataTabel
end

--        1008_主界面接口（ID=1008）
function Action1008(Scene,isLoading )
   	ScutWriter:writeString("ActionId",1008)
	
end


function _1008Callback(token)
   
	local DataTabel=nil
	if ScutReader:getResult() then
		DataTabel={}
		DataTabel.Pid= ScutReader:readString()
		DataTabel.HeadIcon= ScutReader:readString()
		DataTabel.NickName= ScutReader:readString()
		DataTabel.GameCoin= ScutReader:getInt()
		DataTabel.Gold= ScutReader:getInt()
		DataTabel.VipLv= ScutReader:getInt()
		DataTabel.WinNum= ScutReader:getInt()
		DataTabel.FailNum= ScutReader:getInt()
		DataTabel.TitleName= ScutReader:readString()
		DataTabel.ScoreNum= ScutReader:getInt()
		DataTabel.WinRate= ScutReader:getInt()
		local RecordNums_1=ScutReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
				local mRoomTabel_1={}
				ScutReader:recordBegin()
				mRoomTabel_1.RoomId= ScutReader:getInt()
				mRoomTabel_1.RoomName= ScutReader:readString()
				mRoomTabel_1.RoomMultiple= ScutReader:getWORD()
				mRoomTabel_1.MinCion= ScutReader:getInt()
				mRoomTabel_1.GiffCion= ScutReader:getInt()
				mRoomTabel_1.Description= ScutReader:readString()
				mRoomTabel_1.AnteNum= ScutReader:getInt()
				ScutReader:recordEnd()
				table.insert(RecordTabel_1, mRoomTabel_1)
			end
		end
		
		DataTabel.RecordTabel  = RecordTabel_1;
		
		DataTabel.RecordTabelNum = #RecordTabel_1
		DataTabel.RoomId  = ScutReader:getInt();
	else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
	end
	return DataTabel
end

--        1009_社交资料接口【完成】（ID=1009）
function Action1009(Scene,isLoading )

   	ScutWriter:writeString("ActionId",1009)
   	
	
end

function _1009Callback(token)
	
	local DataTabel=nil
	if ScutReader:getResult() then
		DataTabel={}
		DataTabel.Name= ScutReader:readString()
		DataTabel.Sex= ScutReader:getByte()
		DataTabel.Birthday= ScutReader:readString()
		DataTabel.Hobby= ScutReader:readString()
		DataTabel.Profession= ScutReader:readString()
	else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
	end
	return DataTabel
end

--        1010_更换头像接口【完成】（ID=1010）
function Action1010(Scene,isLoading ,HeadIcon)

   	ScutWriter:writeString("ActionId",1010)
   	ScutWriter:writeString("HeadIcon","head_1002")
        
	
end

function _1010Callback(token)
   
    local DataTabel=nil
    if ScutReader:getResult() then
        DataTabel={}

       
    else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
    end
    return DataTabel
end

--        1011_社交资料保存接口【完成】（ID=1011）
function Action1011(token)

	ScutWriter:writeString("ActionId",1011)
	ScutWriter:writeString("Name","name")
	ScutWriter:writeString("Sex",0)
	ScutWriter:writeString("Birthday","2000.1.1")
	ScutWriter:writeString("Profession","eatBoy")
	ScutWriter:writeString("Hobby","eat")

	
end

function _1011Callback(token)
   
	local DataTabel=nil
	if ScutReader:getResult() then
		DataTabel={}
	
	
	else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
	end
	return DataTabel
end

--        1013_背包物品列表通知接口【未完成】（ID=1013）
function Action1013()

	ScutWriter:writeString("ActionId",1013)
	ScutWriter:writeString("PageIndex",1)
	ScutWriter:writeString("PageSize",12)
        
	
end

function _1013Callback(token)
   
	local DataTabel=nil
	if ScutReader:getResult() then
		DataTabel={}
		DataTabel.PageCount= ScutReader:getInt()
		local RecordNums_1=ScutReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
				local mRecordTabel_1={}
				ScutReader:recordBegin()
				mRecordTabel_1.UserItemID= ScutReader:readString()
				mRecordTabel_1.ItemID= ScutReader:getInt()
				mRecordTabel_1.ItemName= ScutReader:readString()
				mRecordTabel_1.ItemType= ScutReader:getWORD()
				mRecordTabel_1.Num= ScutReader:getInt()
				mRecordTabel_1.HeadID= ScutReader:readString()
				ScutReader:recordEnd()
			--	table.insert(RecordTabel_1,mRecordTabel_1)
				table.insert(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.RecordTabel = RecordTabel_1;
	else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
	end
	return DataTabel
end

--        1014_用户信息变更通知接口【未完成】（ID=1014）
function Action1014(Scene,isLoading ,GameType,ServerID)
	ScutWriter:writeString("ActionId",1014)
	ScutWriter:writeString("GameType",GameType)
	ScutWriter:writeString("ServerID",ServerID)
end

function _1014Callback(token)
	local DataTabel=nil
		if ScutReader:getResult() then
		DataTabel={}
		DataTabel.HeadIcon= ScutReader:readString()
		DataTabel.GameCoin= ScutReader:getInt()
		DataTabel.Gold= ScutReader:getInt()
		DataTabel.VipLv= ScutReader:getInt()
		DataTabel.WinNum= ScutReader:getInt()
		DataTabel.FailNum= ScutReader:getInt()
		DataTabel.TitleName= ScutReader:readString()
		DataTabel.ScoreNum= ScutReader:getInt()  
	else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
	end
	return DataTabel
end


--        1019_排行榜接口【完成】（ID=1019）
function Action1019()

   	ScutWriter:writeString("ActionId",1019)
   	ScutWriter:writeString("RankType",1)
        ScutWriter:writeString("PageIndex",1)
        ScutWriter:writeString("PageSize",5)
        
	
end

function _1019Callback(token)
	
	local DataTabel=nil
	if ScutReader:getResult() then
		DataTabel={}
		DataTabel.PageCount= ScutReader:getInt()
		local RecordNums_1=ScutReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
				local mRecordTabel_1={}
				ScutReader:recordBegin()
				mRecordTabel_1.RankID= ScutReader:getInt()
				mRecordTabel_1.UserID= ScutReader:getInt()
				mRecordTabel_1.NickName= ScutReader:readString()
				mRecordTabel_1.GameCoin= ScutReader:getInt()
				mRecordTabel_1.Wining= ScutReader:readString()
				ScutReader:recordEnd()
		--		table.insert(RecordTabel_1,mRecordTabel_1)
				table.insert(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.RecordTabel = RecordTabel_1;
	else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
	end
	return DataTabel
end

--        1069_AppStore充值详情接口【完成】（ID=1069）
function Action1069(token)

	ScutWriter:writeString("ActionId",1069)
	ScutWriter:writeString("MobileType",token.MobileType)
	ScutWriter:writeString("GameID",token.GameType)
        
	
end

function _1069Callback(token)
   
	local DataTabel=nil
	if ScutReader:getResult() then
		DataTabel={}
		local RecordNums_1=ScutReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
				local mRecordTabel_1={}
				ScutReader:recordBegin()
				mRecordTabel_1.Dollar= ScutReader:readString()
				mRecordTabel_1.product_id= ScutReader:readString()
				mRecordTabel_1.SilverPiece= ScutReader:getInt()
				ScutReader:recordEnd()
				table.insert(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.RecordTabel = RecordTabel_1;  
	else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
	end
	return DataTabel
end



--     
function Action2001( )
   	ScutWriter:writeString("ActionId",2001)
   	ScutWriter:writeString("RoomId",1001)
   	ScutWriter:writeString("Op",1)
   	
end

function _2001Callback()
   
	local DataTabel=nil
	if ScutReader:getResult() then
		DataTabel={}
		DataTabel.GameCoin= ScutReader:getInt()
	else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
	end
	return DataTabel
end



function Action2002(Scene,isLoading )
   	ScutWriter:writeString("ActionId",2002)
	
end

--下发玩家数据
function _2003Callback(token)
	local DataTabel=nil
	if ScutReader:getResult() then
		DataTabel={}
		local RecordNums_1=ScutReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
				local mRecordTabel_1={}
				ScutReader:recordBegin()
				mRecordTabel_1.UserId= ScutReader:getInt()
				mRecordTabel_1.NickName= ScutReader:readString()
				mRecordTabel_1.HeadIcon= ScutReader:readString()
				mRecordTabel_1.PosId= ScutReader:getInt()
				ScutReader:recordEnd()
				table.insert(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.PlayerTable = RecordTabel_1;
	else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
	end
	return DataTabel
end


function _2004Callback(token)
   
	local DataTabel=nil
	if ScutReader:getResult() then
		DataTabel={}
		DataTabel.LandlordId= ScutReader:getInt()
		DataTabel.LandlordName= ScutReader:readString()
		DataTabel.CodeTime= ScutReader:getInt()
		local RecordNums_1=ScutReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
				local mRecordTabel_1={}
				ScutReader:recordBegin()
				mRecordTabel_1.CardId= ScutReader:getInt()
				ScutReader:recordEnd()
				table.insert(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.OpenTable = RecordTabel_1;
		local RecordNums_2=ScutReader:getInt()
		local RecordTabel_2={}
		if RecordNums_2~=0 then
			for k=1,RecordNums_2 do
				local mRecordTabel_2={}
				ScutReader:recordBegin()
				mRecordTabel_2.CardId= ScutReader:getInt()
				ScutReader:recordEnd()
				table.insert(RecordTabel_2,mRecordTabel_2)
			end
		end
		DataTabel.PlayerCardTable = RecordTabel_2;  
	else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
	end
	return DataTabel
end


function Action2005(Scene,isLoading ,op)
   	ScutWriter:writeString("ActionId",2005)
   	ScutWriter:writeString("op",1)

end

function _2005Callback()
   
	local DataTabel=nil
	if ScutReader:getResult() then
		DataTabel={}
	else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
	end
	return DataTabel
end

--        2006_叫地主通知接口【完成】（ID=2006）
function _2006Callback(token)
    local DataTabel=nil
    if ScutReader:getResult() then
        DataTabel={}
        DataTabel.IsEnd= ScutReader:getByte()
        DataTabel.LandlordId= ScutReader:getInt()
        DataTabel.LandlordName= ScutReader:readString()
        DataTabel.MultipleNum= ScutReader:getInt()
        DataTabel.AnteNum= ScutReader:getInt()
        DataTabel.IsCall= ScutReader:getByte()    
        DataTabel.IsRob= ScutReader:getByte()         
    else          
        ZyToast.show(pNdScene, "2006" .. ScutReader:readErrorMsg(),1,0.2)
    end
    return DataTabel  
end

function Action2007(Scene,isLoading ,Op)
   	ScutWriter:writeString("ActionId",2007)
   	ScutWriter:writeString("Op",1 )
	
end

function _2007Callback()
   
	local DataTabel=nil
	if ScutReader:getResult() then
		DataTabel={}
	else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
	end
	return DataTabel
end

--        2008_明牌通知接口【完成】（ID=2008）
function _2008Callback(token)
    local DataTabel=nil
    if ScutReader:getResult() then
        DataTabel={}
        local RecordNums_1=ScutReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ScutReader:recordBegin()
                mRecordTabel_1.CardId= ScutReader:getInt()
                ScutReader:recordEnd()
                table.insert(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;     
                DataTabel.MultipleNum  =ScutReader:getInt()
                DataTabel.AnteNum  = ScutReader:getInt()
    else          
		ZyToast.show(pNdScene, ScutReader:readErrorMsg(),1,0.2)
    end
    return DataTabel
end

--        2009_出牌接口【完成】（ID=2009）
function Action2009(Scene,isLoading ,Cards)
   	ScutWriter:writeString("ActionId",2009)
   	ScutWriter:writeString("Cards","103")
	
end

function _2009Callback()
	local DataTabel=nil
	if ScutReader:getResult() then
		DataTabel={}
	else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
	end
	return DataTabel
end

--        2010_出牌通知接口【完成】（ID=2010）
function _2010Callback(token)  
    local DataTabel=nil
    if ScutReader:getResult() then
		DataTabel={}
		DataTabel.UserId= ScutReader:getInt()
		DataTabel.NextUserId= ScutReader:getInt()
		DataTabel.DeckType= ScutReader:getWORD()
		DataTabel.CardSize= ScutReader:getInt()
		local RecordNums_1=ScutReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
			local mRecordTabel_1={}
			ScutReader:recordBegin()
			mRecordTabel_1.CardId= ScutReader:getInt()
			ScutReader:recordEnd()
			table.insert(RecordTabel_1,mRecordTabel_1)
			end
		end
                DataTabel.RecordTabel = RecordTabel_1;      
                DataTabel.IsReNew  = ScutReader:getByte();      
                DataTabel.MultipleNum   = ScutReader:getInt();      
                DataTabel.AnteNum   = ScutReader:getInt();   
    else          
		ZyToast.show(pNdScene, ScutReader:readErrorMsg(),1,0.2)
    end
    return DataTabel
end



function Action2011(Scene,isLoading ,Op)
   	ScutWriter:writeString("ActionId",2011)
   	ScutWriter:writeString("Op",Op )
	
end

function _2011Callback()
	local DataTabel=nil
	if ScutReader:getResult() then
		DataTabel={}
	else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
	end
	return DataTabel
end

--        3001_成就界面接口【完成】（ID=3001）
function Action3001()

   	ScutWriter:writeString("ActionId",3001)
   	ScutWriter:writeString("AchieveType",0)
        ScutWriter:writeString("PageIndex",1)
        ScutWriter:writeString("PageSize",20)

end

function _3001Callback(token)
   
	local DataTabel=nil
	if ScutReader:getResult() then
		DataTabel={}
		DataTabel.CompleteNum= ScutReader:getInt()
		DataTabel.AchieveNum= ScutReader:getInt()
		DataTabel.PageCount= ScutReader:getInt()
		local RecordNums_1=ScutReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
				local mRecordTabel_1={}
				ScutReader:recordBegin()
				mRecordTabel_1.AchieveID= ScutReader:getInt()
				mRecordTabel_1.AchieveName= ScutReader:readString()
				mRecordTabel_1.HeadID= ScutReader:readString()
				mRecordTabel_1.IsGain= ScutReader:getWORD()
				ScutReader:recordEnd()
				table.insert(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.RecordTabel = RecordTabel_1;
	else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
	end
	return DataTabel
end

--        3002_成就详情接口【完成】（ID=3002）
function Action3002(Scene,isLoading ,AchieveID)
   	ScutWriter:writeString("ActionId",3002)
   	ScutWriter:writeString("AchieveID","1001")
end

function _3002Callback(token)
   
	local DataTabel=nil
	if ScutReader:getResult() then
		DataTabel={}
		DataTabel.AchieveName= ScutReader:readString()
		DataTabel.AchieveType= ScutReader:getWORD()
		DataTabel.HeadID= ScutReader:readString()
		DataTabel.AchieveDesc= ScutReader:readString()
		DataTabel.IsComplete= ScutReader:getWORD()
		DataTabel.CompleteNum= ScutReader:getInt()
		DataTabel.AchieveNum= ScutReader:getInt() 
	else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
	end
	return DataTabel
end


function _2012Callback(token)
    local DataTabel=nil
	if ScutReader:getResult() then
		DataTabel={}
		DataTabel.IsLandlord= ScutReader:getByte()
		DataTabel.IsLandlordWin= ScutReader:getByte()
		DataTabel.ScoreNum= ScutReader:getInt()
		DataTabel.CoinNum= ScutReader:getInt()
		DataTabel.GameCoin= ScutReader:getInt()
		local RecordNums_1=ScutReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
				local mRecordTabel_1={}
				ScutReader:recordBegin()
				mRecordTabel_1.UserId= ScutReader:getInt()
				local RecordNums_2=ScutReader:getInt()
				local RecordTabel_2={}
				if RecordNums_2~=0 then
					for k=1,RecordNums_2 do
						local mRecordTabel_2={}
						ScutReader:recordBegin()
						mRecordTabel_2.CardId= ScutReader:getInt()
						ScutReader:recordEnd()
						table.insert(RecordTabel_2,mRecordTabel_2)
					end
				end
				mRecordTabel_1.cardTable = RecordTabel_2;
				ScutReader:recordEnd()
				table.insert(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.RecordTabel = RecordTabel_1;
		
		local RecordNums_3=ScutReader:getInt()
		local RecordTabel_3={}
		if RecordNums_3~=0 then
		for k=1,RecordNums_3 do
			local mRecordTabel_3={}
			ScutReader:recordBegin()
			mRecordTabel_3.CardId= ScutReader:getInt()
			ScutReader:recordEnd()
			table.insert(RecordTabel_3,mRecordTabel_3)
		end
		end	
		DataTabel.lastCardTable=RecordTabel_3
		DataTabel.LastUserId= ScutReader:getInt()
		DataTabel.MultipleNum= ScutReader:getInt()
		DataTabel.AnteNum= ScutReader:getInt()	 
    else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
    end
    return DataTabel
end



function _2013Callback(token)

	local DataTabel=nil
	if ScutReader:getResult() then
		DataTabel={}
		DataTabel.FleeUserId= ScutReader:getInt()
		DataTabel.FleeNickName= ScutReader:readString()
		DataTabel.GameCoin= ScutReader:getInt()
		DataTabel.ScoreNum= ScutReader:getInt()
		DataTabel.InsScoreNum= ScutReader:getInt()
		DataTabel.InsCoinNum= ScutReader:getInt()  
	else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
	end
	return DataTabel
end


function _2014Callback(token)
   
	local DataTabel=nil
	if ScutReader:getResult() then
		DataTabel={}
		DataTabel.UserId= ScutReader:getInt()
		DataTabel.Status= ScutReader:getByte()
	else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
	end
	return DataTabel
end



function _2015Callback(token)
   
	local DataTabel=nil
	if ScutReader:getResult() then
		DataTabel={}
		DataTabel.MultipleNum= ScutReader:getInt()
		DataTabel.AnteNum= ScutReader:getInt()
		DataTabel.IsAI= ScutReader:getByte()
		DataTabel.IsShow= ScutReader:getByte()
		DataTabel.LandlordId= ScutReader:getInt()
		local RecordNums_1=ScutReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
		for k=1,RecordNums_1 do
		local mRecordTabel_1={}
		ScutReader:recordBegin()
		mRecordTabel_1.UserId= ScutReader:getInt()
		mRecordTabel_1.NickName= ScutReader:readString()
		mRecordTabel_1.HeadIcon= ScutReader:readString()
		mRecordTabel_1.PosId= ScutReader:getInt()
		
		local RecordNums_2=ScutReader:getInt()
		local RecordTabel_2={}
		if RecordNums_2~=0 then
		for k=1,RecordNums_2 do
		local mRecordTabel_2={}
		ScutReader:recordBegin()
		mRecordTabel_2.CardId= ScutReader:getInt()
		ScutReader:recordEnd()
		table.insert(RecordTabel_2,mRecordTabel_2)
		end
		end
		mRecordTabel_1.CardTable = RecordTabel_2;
		ScutReader:recordEnd()
		table.insert(RecordTabel_1,mRecordTabel_1)
		end
		end
		DataTabel.playerTable = RecordTabel_1;
		local RecordNums_3=ScutReader:getInt()
		local RecordTabel_3={}
		if RecordNums_3~=0 then
			for k=1,RecordNums_3 do
				local mRecordTabel_3={}
				ScutReader:recordBegin()
				mRecordTabel_3.CardId= ScutReader:getInt()
				ScutReader:recordEnd()
				table.insert(RecordTabel_3,mRecordTabel_3)
			end
		end
		DataTabel.LandLordCard = RecordTabel_3;
		DataTabel.CodeTime =ScutReader:getInt()
		DataTabel.OutCardUserId =ScutReader:getInt()
		DataTabel.IsReNew =ScutReader:getByte()
		DataTabel.GameCoin =ScutReader:getInt()
		local RecordNums_4=ScutReader:getInt()
		local RecordTabel_4={}
		if RecordNums_4~=0 then
			for k=1,RecordNums_4 do
				local mRecordTabel_4={}
				ScutReader:recordBegin()
				mRecordTabel_4.CardId= ScutReader:getInt()
				ScutReader:recordEnd()
				table.insert(RecordTabel_4,mRecordTabel_4)
			end
		end
		DataTabel.RecordTabel = RecordTabel_4;
		DataTabel.DeckType= ScutReader:getWORD()
		DataTabel.CardSize= ScutReader:getInt()
		DataTabel.UserId = ScutReader:getInt()
		
	else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
	end
	return DataTabel
end


--        3003_任务界面接口【未完成】（ID=3003）
function Action3003()

	ScutWriter:writeString("ActionId",3003)
	ScutWriter:writeString("TaskType",1)
	ScutWriter:writeString("PageIndex",1)
	ScutWriter:writeString("PageSize",20)
        
	
end

function _3003Callback(token)
   
	local DataTabel=nil
	if ScutReader:getResult() then
		DataTabel={}
		DataTabel.PageCount= ScutReader:getInt()
		local RecordNums_1=ScutReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
				local mRecordTabel_1={}
				ScutReader:recordBegin()
				mRecordTabel_1.TaskID= ScutReader:getInt()
				mRecordTabel_1.TaskName= ScutReader:readString()
				mRecordTabel_1.CompleteNum= ScutReader:getInt()
				mRecordTabel_1.TaskNum= ScutReader:getInt()
				mRecordTabel_1.TaskDesc= ScutReader:readString()
				mRecordTabel_1.GameCoin= ScutReader:getInt()
				mRecordTabel_1.IsReceive= ScutReader:getWORD()
				ScutReader:recordEnd()
				table.insert(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.RecordTabel = RecordTabel_1;

	
	else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
	end
	return DataTabel
end


--        3004_任务奖励领取接口【未完成】（ID=3004）
function Action3004(Scene,isLoading ,TaskID)

   	ScutWriter:writeString("ActionId",3004)
   	ScutWriter:writeString("TaskID",1002)
        
	
end

function _3004Callback(token)
   
    local DataTabel=nil
    if ScutReader:getResult() then
        DataTabel={}

       
    else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
    end
    return DataTabel
end


--        7001_商店物品列表接口【未完成】（ID=7001）
function Action7001(Scene,isLoading ,ShopType,PageIndex,PageSize)

   	ScutWriter:writeString("ActionId",7001)
   	ScutWriter:writeString("ShopType",2)
        ScutWriter:writeString("PageIndex",1)
        ScutWriter:writeString("PageSize",10)
        
	
end

function _7001Callback(token)
   
    local DataTabel=nil
    if ScutReader:getResult() then
        DataTabel={}
        DataTabel.PageCount= ScutReader:getInt()
        DataTabel.GameCoin= ScutReader:getInt()
        DataTabel.GoldNum= ScutReader:getInt()
        local RecordNums_1=ScutReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ScutReader:recordBegin()
                mRecordTabel_1.ItemID= ScutReader:getInt()
                mRecordTabel_1.ItemName= ScutReader:readString()
                mRecordTabel_1.HeadID= ScutReader:readString()
                mRecordTabel_1.ItemPrice= ScutReader:getInt()
                mRecordTabel_1.VipPrice= ScutReader:getInt()
                mRecordTabel_1.GainGameCoin= ScutReader:getInt()
                mRecordTabel_1.ShopDesc= ScutReader:readString()
                mRecordTabel_1.SeqNO= ScutReader:getWORD()
                ScutReader:recordEnd()
                table.insert(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;

       
    else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
    end
    return DataTabel
end

--        7002_商店物品购买接口【完成】（ID=7002）
function Action7002(Scene,isLoading ,ItemID)

   	ScutWriter:writeString("ActionId",7002)
   	ScutWriter:writeString("ItemID",16)
        
	
end

function _7002Callback(token)
    local DataTabel=nil
    if ScutReader:getResult() then
        DataTabel={}
    else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
    end
    return DataTabel

end

--        9001_即时聊天列表接口（ID=9001）
function Action9001(Scene,isLoading )
   	ScutWriter:writeString("ActionId",9001)
	
end




function _9001Callback(token)
   
    local DataTabel=nil
    if ScutReader:getResult() then
        DataTabel={}
        local RecordNums_1=ScutReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ScutReader:recordBegin()
                mRecordTabel_1.ChatID= ScutReader:getInt()
                mRecordTabel_1.ChatContent= ScutReader:readString()
                ScutReader:recordEnd()
                table.insert(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;
    else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
    end
    return DataTabel
end
--        9002_聊天发送接口（ID=9002）
function Action9002(Scene,isLoading ,ChatID)

   	ScutWriter:writeString("ActionId",9002)
   	ScutWriter:writeString("ChatID",1002)
	
end

function _9002Callback(token)
   
    local DataTabel=nil
    if ScutReader:getResult() then
        DataTabel={}
    else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
    end
    return DataTabel
end
--        9003_聊天记录列表接口【完成】（ID=9003）
function Action9003(Scene,isLoading )

   	ScutWriter:writeString("ActionId",9003)
   	
	
end

function _9003Callback(token)
   
    local DataTabel=nil
    if ScutReader:getResult() then
        DataTabel={}
        DataTabel.ChatMaxNum= ScutReader:getInt()
        local RecordNums_1=ScutReader:getInt()
         local RecordTabel_1={}
         local tNum={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ScutReader:recordBegin()
                mRecordTabel_1.UserID= ScutReader:getInt()
                mRecordTabel_1.UserName= ScutReader:readString()
                mRecordTabel_1.ChatID= ScutReader:getInt()
                mRecordTabel_1.Content= ScutReader:readString()
                mRecordTabel_1.SendDate= ScutReader:readString()
                ScutReader:recordEnd()
                table.insert(RecordTabel_1,mRecordTabel_1)
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
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
    end
    return DataTabel
end
--        9202_公告广播通知接口【未完成】（ID=9202）
function Action9202(token)

   	ScutWriter:writeString("ActionId",9202)
   	ScutWriter:writeString("GameType",token.GameType)
        ScutWriter:writeString("ServerID",token.ServerID)
        
	
end

function _9202Callback(token)
   
    local DataTabel=nil
    if ScutReader:getResult() then
        DataTabel={}
        local RecordNums_1=ScutReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ScutReader:recordBegin()
                mRecordTabel_1.BroadcastType= ScutReader:getWORD()
                mRecordTabel_1.Content= ScutReader:readString()
                mRecordTabel_1.PlayTimes= ScutReader:getInt()
                ScutReader:recordEnd()
                table.insert(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;

       
    else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
    end
    return DataTabel
end

--        9203_公告列表接口【未完成】（ID=9203）
function Action9203(Scene,isLoading ,PageIndex,PageSize)

   	ScutWriter:writeString("ActionId",9203)
   	ScutWriter:writeString("PageIndex",1)
        ScutWriter:writeString("PageSize",20)
        
	
end

function _9203Callback(token)
   
    local DataTabel=nil
    if ScutReader:getResult() then
        DataTabel={}
        DataTabel.PageCount= ScutReader:getInt()
        local RecordNums_1=ScutReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ScutReader:recordBegin()
                mRecordTabel_1.Title= ScutReader:readString()
                mRecordTabel_1.Content= ScutReader:readString()
                mRecordTabel_1.SendDate= ScutReader:readString()
                ScutReader:recordEnd()
                table.insert(RecordTabel_1,mRecordTabel_1)
              end
        end
                DataTabel.RecordTabel = RecordTabel_1;
    else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
    end
    return DataTabel
end


--        12001_转盘界面接口【完成】（ID=12001）
function Action12001(Scene,isLoading )
	ScutWriter:writeString("ActionId",12001)
	
end

function _12001Callback(token)
	local DataTabel=nil
	if ScutReader:getResult() then
		DataTabel={}
		DataTabel.IsFree= ScutReader:getWORD()
		DataTabel.FreeNum= ScutReader:getInt()
		local RecordNums_1=ScutReader:getInt()
		local RecordTabel_1={}
		if RecordNums_1~=0 then
			for k=1,RecordNums_1 do
				local mRecordTabel_1={}
				ScutReader:recordBegin()
				mRecordTabel_1.Postion= ScutReader:getInt()
				mRecordTabel_1.HeadID= ScutReader:readString()
				mRecordTabel_1.Probability= ScutReader:readString()
				mRecordTabel_1.ItemDesc= ScutReader:readString()
				mRecordTabel_1.GameCoin= ScutReader:getInt()
				ScutReader:recordEnd()
				table.insert(RecordTabel_1,mRecordTabel_1)
			end
		end
		DataTabel.RecordTabel = RecordTabel_1; 
		DataTabel.UserCoin= ScutReader:getInt()
		DataTabel.UserGold= ScutReader:getInt()

	else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
	end
	return DataTabel
end

--        12002_抽奖接口【完成】（ID=12002）
function Action12002(Scene,isLoading ,Ops)
	ScutWriter:writeString("ActionId",12002)
	ScutWriter:writeString("Ops",1)
	
end

function _12002Callback(token)
	local DataTabel=nil
	if ScutReader:getResult() then
		DataTabel={}
		DataTabel.Postion= ScutReader:getWORD()
		DataTabel.RewardContent= ScutReader:readString()
		DataTabel.FreeNum= ScutReader:getInt()
		DataTabel.UserCoin= ScutReader:getInt()
		DataTabel.UserGold= ScutReader:getInt()		
	else          
		LogWriteLine("请求Action:"..ScutReader:readAction()..",出错:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
	end
	return DataTabel
end

--
--
--function _2009Callback(token)
--    local DataTabel={}
--    if ScutReader.getResult() then
--
--    else
--        log.WriteLine('Error:'+ScutReader:getErrorCode() + ','+ScutReader:readErrorMsg())
--    end
--    return DataTabel
--end
--
--function _2010Callback(token, log)
--    local DataTabel={}
--    if ScutReader.getResult() then
--        DataTabel.UserId= ScutReader:readInt()
--        DataTabel.NextUserId= ScutReader:readInt()
--        DataTabel.DeckType= ScutReader:readShort()
--        DataTabel.CardSize= ScutReader:readInt()
--        
--        local RecordNums_1=ScutReader:readInt()
--        local RecordTabel_1={}
--        if RecordNums_1~=0 then
--            for k=1,RecordNums_1 do
--                local mRecordTabel_1={}
--                ScutReader:recordBegin()
--                mRecordTabel_1.CardId= ScutReader:readInt()
--                ScutReader:recordEnd()
--                table.insert(RecordTabel_1, mRecordTabel_1)
--              end
--        end
--        DataTabel.RecordTabel = RecordTabel_1;
--        DataTabel.IsReNew= ScutReader:readByte()
--        DataTabel.MultipleNum= ScutReader:readInt()
--        DataTabel.AnteNum= ScutReader:readInt()
--
--    else
--        log.WriteLine('Error:'+ScutReader:getErrorCode() + ','+ScutReader:readErrorMsg())
--
--    end
--    return DataTabel
--end
--
