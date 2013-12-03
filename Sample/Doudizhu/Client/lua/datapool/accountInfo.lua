------------------------------------------------------------------
-- accountInfo.lua
-- Author     :
-- Version    : 1.15
-- Date       :
-- Description: 账号信息
------------------------------------------------------------------



module("accountInfo",package.seeall);
mUserID=nil;
mServerID=nil;
mServerPath=nil;
mServerName=nil;
mServerState=nil;
mPassportID=nil;
mPassWord=nil;
mMobileType = ScutUtility.ScutUtils:GetPlatformType();--手机类型
mMac = ScutUtility.ScutUtils:getMacAddress();--设备ID
mGameType=7;--游戏类型
ClientAppVersion="1.0"--客户端版本号
mRetailID="0000"
mCID = 0;
mCV = 0;
mIMSI = "462814541231564";
mLang = 0;
mNickName = nil;
mGender = 1;
mCountry = 1;
mHeadid = 0;
mServerTable=nil
UserType=nil--账号类型
firstPlot=nil
askActivity=nil

local version=0
local helpShow=nil

local fightInfo={}

function  setActivity(value)
	askActivity=value
end;

function  getActivity()
	return askActivity
end;

function  getHelpShow()
	return helpShow
end;

function  setHelpShow(value)
		if value==1 then
			if helpShow~=100 and helpShow~=101 then
				helpShow=value
			end
		else
			helpShow=value
		end
end;

function  clearHelpShow()
	helpShow=nil
end;

function  setFightProcess(info)
	fightInfo=info
end;

function getFightProcess ()
	return fightInfo
end;

function  getfirstPlot()
return firstPlot
end;

function  setfirstPlot(value)
firstPlot=value
end;

--服务器
function setServerID(serverId)
    mServerID=serverId;
end
function getServerID()
   return mServerID;
end

function  getRetailId()
   return  mRetailID
end;

function getPassportID()
    return mPassportID
end

function setServerPath(ServerPath)
	mServerPath = ServerPath;
end

function getServerPath()
	return mServerPath;
end

function setServerName(ServerName)
	mServerName = ServerName;
end

function getServerName()
	return mServerName;
end

function setServerState(ServerState)
	mServerState = ServerState;
end

function getServerState()
	return mServerState;
end


function setServerTable(serverTable)
	mServerTable = serverTable;
end

function getServerTable()
	return mServerTable;
end
----

function getGameId()
 return mGameType;
end
function getMac()
    mMac = ScutUtility.ScutUtils:getMacAddress();
    return mMac
end
--
function setUserID(UserID)
    mUserID = UserID;
end

function getUserID()
    return mUserID;
end

function setPassportID(PassportID)
    mPassportID = PassportID;
end

function setPassWord( passWord)
    mPassWord  = passWord;
end
--
function setMobileType(type)
   mMobileType = type;
end

function getMobileType()
    return mMobileType
end

function setCID( cid)
    mCID = cid;
end

function getCID()
    return mCID;
end

function setIMSI(imsi)
    mIMSI = imsi;
end
function getIMSI()
    return mIMSI;
end

function setLang(lang)
    mLang = lang;
end

function getLang()
    return mLang
end

function setNickName(nickName)
    mNickName = nickName;
end

function getNickName()
    return mNickName;
end

function setGender(gender)
     mGender = gender;
end

function getGender()
    return mGender;
end

function setCountry(country)
    mCountry = country;
end

function getCountry()
    return mCountry;
end

function setHeadid(headid)
    mHeadid = headid;
end

function getHeadid()
    return mHeadid;
end

function setCV(cv)
    mCV = cv;
end

function getCV()
    return mCV;
end

function saveServerId()
	if mServerID ~= nil then
		pIni = ScutDataLogic.CLuaIni:new();
		bIsRead = pIni:Load("sys/server.ini");
		pIni = ScutDataLogic.CLuaIni:new();
		pIni:Set("Server", "ServerId", tostring(mServerID));
		pIni:Set("Server", "ServerPath", tostring(mServerPath));
		pIni:Set("Server", "ServerName", tostring(mServerName));
		pIni:Set("Server", "ServerState", tostring(mServerState));
		pIni:Save("sys/server.ini");
		pIni:delete();
		return true
	else
	      return false;
	end
end

function readServerId()
	pIni = ScutDataLogic.CLuaIni:new();
	bIsRead = pIni:Load("sys/server.ini");
	if bIsRead == false then
	pIni:delete();
	else
	   mServerID = pIni:Get("Server", "ServerId");
	   mServerPath = pIni:Get("Server", "ServerPath");
	   mServerName = pIni:Get("Server", "ServerName");
	   mServerState = pIni:Get("Server", "ServerState");
	end
	return mServerID,mServerPath,mServerName,mServerState;
end

function SaveAccountInfo()
    if mPassportID ~= nil  and mPassWord~=nil then
		pIni = ScutDataLogic.CLuaIni:new();
		bIsRead = pIni:Load("sys/account.ini");
		pIni = ScutDataLogic.CLuaIni:new();
		pIni:Set("Account", "PassportID",mPassportID);
		pIni:Set("Account", "pw", ScutDataLogic.CFileHelper:encryptPwd(mPassWord, nil):getCString());
		pIni:Save("sys/account.ini");
		pIni:delete();
		return true
    elseif mPassportID ~= nil  then
       	pIni = ScutDataLogic.CLuaIni:new();
		bIsRead = pIni:Load("sys/account.ini");
		pIni = ScutDataLogic.CLuaIni:new();
		pIni:Set("Account", "PassportID",mPassportID);
		pIni:Save("sys/account.ini");
		pIni:delete();
		return true
    elseif mPassWord~=nil then

    --[[   	pIni = ScutDataLogic.CLuaIni:new();
		bIsRead = pIni:Load("sys/account.ini");
		pIni = ScutDataLogic.CLuaIni:new();
		pIni:Set("Account", "pw", ScutDataLogic.CFileHelper:encryptPwd(mPassWord, nil):getCString());
		pIni:Save("sys/account.ini");
		pIni:delete();
		return true--]]
	else
	    return false;
	end
end


function readAccount()
	pIni       = ScutDataLogic.CLuaIni:new();
	bIsRead    = pIni:Load("sys/account.ini");
	if bIsRead == false then
		pIni:delete();
	else
	   	mPassportID   = pIni:Get("Account","PassportID");
	   	mPassWord = pIni:Get("Account", "pw");
	   	if mPassportID =="error" then
	      		mPassportID=nil
       	end
       	if mPassWord=="error" then
          		mPassWord=nil
       	end
	end
	return mPassportID, mPassWord;
end

function saveMoble()
  	if mGameType~=nil then
  		pIni = ScutDataLogic.CLuaIni:new();
  		bIsRead = pIni:Load("sys/read.ini");
  		pIni = ScutDataLogic.CLuaIni:new();
    		pIni:Set("MobileInfo", "GameType",mGameType);
    		pIni:Save("sys/read.ini");
		pIni:delete();
		return true
	else
		return false
  	end
end

--写入账号密码
function savePassportID_and_Password(z,pw)
  	pIni = ScutDataLogic.CLuaIni:new();
	bIsRead = pIni:Load("sys/account.ini");
	pIni = ScutDataLogic.CLuaIni:new();
	pIni:Set("Account", "PassportID",z);
	pIni:Set("Account", "pw", pw);
	pIni:Save("sys/account.ini");
	pIni:delete();
end

--给出账号
function read_PassportID()
  	pIni = ScutDataLogic.CLuaIni:new();
	bIsRead = pIni:Load("sys/account.ini");
	mPassportID=nil
	if bIsRead==false then
		pIni:delete();
	else
		mPassportID = pIni:Get("Account", "PassportID");
	   	return mPassportID
	end
end

function readAccount()
	pIni       = ScutDataLogic.CLuaIni:new();
	bIsRead    = pIni:Load("sys/account.ini");
	if bIsRead == false then
		pIni:delete();
	else
	   	mPassportID   = pIni:Get("Account","PassportID");
	   	mPassWord = pIni:Get("Account", "pw");
	   	if mPassportID =="error" then
	      		mPassportID=nil
       	end
       	if mPassWord=="error" then
          		mPassWord=nil
       	end
	end
	return mPassportID, mPassWord;
end



--给出密码
function read_Password()
  	pIni = ScutDataLogic.CLuaIni:new();
	bIsRead = pIni:Load("sys/account.ini");
	mPassWord=nil
	if bIsRead==false then
		pIni:delete();
	else
		mPassWord = pIni:Get("Account", "pw");
	   	return mPassWord
	end
end


function  readVersion()
	pIni =ScutDataLogic.CLuaIni:new()
    	bIsRead =pIni:Load("sys/system.ini")
    	if bIsRead == false then
 		pIni:delete();
    	else
    	       local versions=pIni:Get("systemInfo","version");
       	if versions~=nil and  versions~="error" then
         		 version=versions
       	end
    	end
    	return version
end;

function readMoble()
    	pIni =ScutDataLogic.CLuaIni:new()
    	bIsRead =pIni:Load("sys/system.ini")
    	if bIsRead == false then
 		pIni:delete();
    	else
    	       local id=pIni:Get("android","RetailID");
       	if id~=nil and  id~="error" then
         		 mRetailID=string.sub(id,1,4)
       	end
      --[[ 	mRetailID=pIni:Get("android","RetailID");
       	if mRetailID=="error" or mRetailID==nil then
       		mRetailID="0000"
       	end
       	--]]
    	end
    	return mMobileType ,mGameType,mRetailID;
end

function readRetailID()
    	pIni =ScutDataLogic.CLuaIni:new()
    	bIsRead =pIni:Load("sys/system.ini")
    	local aa=nil
    	if bIsRead == false then
 		pIni:delete();
    	else
       	aa=pIni:Get("MobileInfo","RetailID");
    	end
    	return aa
end

-- 获取配置：文件名、标题、key
function getConfig(sFileName, sTitle, sKey)
	local pIni = ScutDataLogic.CLuaIni:new()
	local bIsRead = pIni:Load(sFileName)
	if bIsRead == false then
		pIni:Set(sTitle, sKey, 1)
		pIni:Save(sFileName)
		pIni:delete()
		return 1
	end
	return pIni:Get(sTitle, sKey)
end


-- 保存配置：文件名、标题、key、value
function saveConfig(sFileName, sTitle, key , value)
	if value ~= nil then
		local pIni = ScutDataLogic.CLuaIni:new()
		local bIsRead = pIni:Load(sFileName)
		pIni:Set(sTitle, key, value)
		pIni:Save(sFileName)
		pIni:delete()
		return true
	else
	    return false
	end
end
