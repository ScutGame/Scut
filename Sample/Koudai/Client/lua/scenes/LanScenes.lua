
------------------------------------------------------------------
-- LanScenes.lua
-- Author     : ZongLin Liu
-- Version    : 
-- Date       :   
-- Description: 登陆界面,
------------------------------------------------------------------

module("LanScenes", package.seeall)

require("scenes.LoginScene")
require("scenes.CreatNewMan")
require("datapool.progressLayer")
--require("payment.channelEngine")
local accountTable={}
local loginType=nil
LOGINFOR91SDK=nil
-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写
--1列出服务器地址
--2判断输入的账号密码是否正确
--3

function  quitGame ()
	CCDirector:sharedDirector():endToLua();
end;

function quiteGame()
	CCDirector:sharedDirector():endToLua();
end


function  loginDangLe()
	local mMobileType,GameType,RetailID=accountInfo.readMoble();
	local mServerId=accountInfo.getServerID()
	local mServerPath=accountInfo.getServerPath()
	local mServerState=accountInfo.getServerState()
	local mServerName=accountInfo.getServerName()
	LOGINFOR91SDK= ND91SDK.Login91:CreateLogin91();
	local sid = LOGINFOR91SDK:getSessionId();
	local loginUin= LOGINFOR91SDK:getLoginUin();
     if  sid ==nil then
     	 startSDK()
     	 return
      end
      	local ScreenX=pWinSize.width
	    local ScreenY=pWinSize.height
	    local DeviceID=accountInfo.getMac()
	    if mServerPath~=nil and mServerPath~="error" then
	 	    ScutDataLogic.CNetWriter:setUrl(mServerPath)
	    end
	loginType=8
	actionLayer.Action1004(mScene,nil,mMobileType,loginUin,"0",DeviceID,GameType,ScreenX,ScreenY,RetailID,nil,mServerId,nil,nil,sid);
end;

function loginUC()
    local mMobileType,GameType,RetailID=accountInfo.readMoble();
    local mServerId=accountInfo.getServerID()
    local mServerPath=accountInfo.getServerPath()
    local mServerState=accountInfo.getServerState()
    local mServerName=accountInfo.getServerName()
    local sid = LOGINFOR91SDK:getSessionId();
     if  sid ~=nil then
         ScutDataLogic.CNetWriter:setSessionID(sid);
      end
      	local ScreenX=pWinSize.width
	    local ScreenY=pWinSize.height
	    local DeviceID=accountInfo.getMac()
	    if mServerPath~=nil and mServerPath~="error" then
	 	    ScutDataLogic.CNetWriter:setUrl(mServerPath)
	     --	  ScutDataLogic.CNetWriter:setUrl("http://192.168.1.55:8001/Service.aspx")
	    end
	    loginType=8
	    actionLayer.Action1004(mScene,nil,mMobileType,"0","0",DeviceID,GameType,ScreenX,ScreenY,RetailID,nil,mServerId);
end;


----------登陆回调
function loginCallBack(tData)
	ZyLoading.HideForUiLoading(mScene)
	if tData then
		local sid = nil;
		local serverid = nil;
		local gameid = nil;
		local userid = nil;
		local accessToken = nil;
		local name = nil;
		local DeviceID=accountInfo.getMac()
		local mMobileType,GameType,RetailID=accountInfo.readMoble();
		local mServerId=accountInfo.getServerID()
		local ScreenX=pWinSize.width
		local ScreenY=pWinSize.height
		mLoginCallBackData = tData
		for k, v in pairs(tData) do
			 if k == "sid" then
				sid = v;
			 end
			 if k == "serverid" then
				serverid = v;
			 end
			 if k == "gameid" then
				gameid = v
			 end
			 if k == "userid" then
				userid = v;
			 end
			 if k == "accesstoken" then
				accessToken = v
			 end
			 if k == "weiboname" then
				CCLuaLog(v);
			 end
		end
		accountInfo.saveConfig("sys/log.ini", "channelEngine", "channelEngine" , channelEngine.getType())
		if  channelEngine.getType()  == ChannelConfig.uc then
			  loginType=8
			  ScutDataLogic.CNetWriter:setSessionID(sid);
			  actionLayer.Action1004(mScene,nil,mMobileType,"0","0",DeviceID,GameType,ScreenX,ScreenY,RetailID,nil,mServerId);		  
		elseif  channelEngine.getType()  == ChannelConfig.qihoo360 then
			  actionLayer.Action1004(mScene,nil,mMobileType,"0","0",DeviceID,GameType,ScreenX,ScreenY,
			  				RetailID,nil,mServerId,nil,sid);
		elseif  channelEngine.getType()  == ChannelConfig.xiaomi then
			  loginType=8
			  ScutDataLogic.CNetWriter:setSessionID(sid);
			  actionLayer.Action1004(mScene,nil,mMobileType,"0","0",DeviceID,GameType,ScreenX,ScreenY,RetailID,nil,mServerId,userid);	
		elseif  channelEngine.getType()  == ChannelConfig.downjoy then
			
		    		loginType=8
			  	actionLayer.Action1004(mScene,nil,mMobileType,"0","0",DeviceID,GameType,ScreenX,ScreenY,RetailID,nil,mServerId,userid,nil,sid);
		elseif  channelEngine.getType()  == ChannelConfig.sdk91 then
				loginType=8
		          if  sid ~=nil then
		               ScutDataLogic.CNetWriter:setSessionID(sid);
		          end
				actionLayer.Action1004(mScene,nil,mMobileType,"0","0",DeviceID,GameType,ScreenX,ScreenY,RetailID,nil,mServerId,userid);
		end
	end
end



function  logining91()
    local mMobileType,GameType,RetailID=accountInfo.readMoble();
    local mServerId=accountInfo.getServerID()
    local mServerPath=accountInfo.getServerPath()
    local mServerState=accountInfo.getServerState()
    local mServerName=accountInfo.getServerName()
     LOGINFOR91SDK= ND91SDK.Login91:CreateLogin91(); 
    	     local sid = LOGINFOR91SDK:getSessionId();
    	      local loginUin= LOGINFOR91SDK:getLoginUin();
          if  sid ~=nil then
               ScutDataLogic.CNetWriter:setSessionID(sid);
          end
	    local ScreenX=pWinSize.width
	    local ScreenY=pWinSize.height
	    local DeviceID=accountInfo.getMac()
	    if mServerPath~=nil and mServerPath~="error" then
	        ScutDataLogic.CNetWriter:setUrl(mServerPath)
	    end
	    if RetailID~="2008" then
	    	  loginType=8
	    	  actionLayer.Action1004(mScene,nil,mMobileType,"0","0",DeviceID,GameType,ScreenX,ScreenY,RetailID,nil,mServerId,loginUin);
	    else
	    	  actionLayer.Action1004(mScene,nil,mMobileType,sid,"0",DeviceID,GameType,ScreenX,ScreenY,RetailID,nil,mServerId,"");
	    	  
	    end
  --]]
end;


function getLogin91Sdk()
  	return LOGINFOR91SDK
end

--初始化数据
function initResource()
    accountTable={}
    accountTable.mMobileType,accountTable.mGameType,accountTable.mRetailID=accountInfo.readMoble() 
end

function releaseResource()
	mScene = nil
end

function onExit()
	releaseResource()
end;

function init(type)
	if mScene then
		return 
	end	
	initResource()
	local scene =ScutScene:new()
	scene:registerCallback(netCallback)--设置网络回调监听
	mScene = scene.root;
    	mLayer=CCLayer:create();
    	mLayer:setAnchorPoint(PT(0,0))
	mLayer:setPosition(PT(0,0))
    	mScene:addChild(mLayer,0)
       local runningScene = CCDirector:sharedDirector():getRunningScene()
	if runningScene == nil then
		CCDirector:sharedDirector():runWithScene(mScene)
	else
		CCDirector:sharedDirector():replaceScene(mScene)
	end
	
   	m_CurrentType=type
	--底图
	local RetailId=accountInfo.getRetailId()
	if RetailId == "0036" then
		delayExec("LanScenes.creatBgImage",0.1)
	else
		creatBgImage()
	end
	playMusic(EnumMusicType.bgMusic)
	 ----------------------------------------
	if   m_CurrentType==nil then 
		if _DOWNJOY then
			local sprite=CCSprite:create(P("Default/downjoy.jpg"))
			ChannelImg=sprite
			sprite:setScaleX(pWinSize.width/sprite:getContentSize().width)
			sprite:setScaleY(pWinSize.height/sprite:getContentSize().height)
			sprite:setPosition(PT(pWinSize.width/2,pWinSize.height/2));
			mLayer:addChild(sprite,2);
			delayExec("LanScenes.loginGoSwitch",2)
		else	
			loginGoSwitch()
		end
	else
		loginGoSwitch(m_CurrentType)
	end
	
end



--通用底图
function creatBgImage()
	m_bgImage= CCSprite:create(P(Image.image_bg));
	m_bgImage:setScaleX(pWinSize.width/m_bgImage:getContentSize().width)
	m_bgImage:setScaleY(pWinSize.height/m_bgImage:getContentSize().height)
	m_bgImage:setAnchorPoint(PT(0.5,0.5))
	m_bgImage:setPosition(PT(pWinSize.width/2,pWinSize.height/2))
	mLayer:addChild(m_bgImage,-1)
end;

---------------登陆选择 去往不同路线
function  loginGoSwitch(layertype)
	---渠道图片
	if ChannelImg~=nil then
		ChannelImg:getParent():removeChild(ChannelImg,true)
		ChannelImg=nil
	end
	
	local nType = ScutUtility.ScutUtils:GetPlatformType();
   	---91SDK
    	if accountTable.mRetailID=="0001" then
			   showLayer(2)
    	--UC游戏
    	elseif accountTable.mRetailID=="0036" then
    		if nType ~= ScutUtility.ptAndroid then
			  LOGINFOR91SDK= ND91SDK.Login91:CreateLogin91();
		end
    		 showLayer(2)
    	---当乐
    	elseif accountTable.mRetailID=="0037" then
    		 showLayer(2)
    	--360渠道
    	elseif accountTable.mRetailID=="2008" then
    		 showLayer(2)
    	elseif accountTable.mRetailID=="0070" then
    		 showLayer(2)
    	--其他
    	else
	--tag 用于判断从其他界面中进入时要显示的界面
	--1.为服务器选择界面

    		if layertype then
    			 showLayer(layertype)
    		else
    			local mPassportID, mPassWord=accountInfo.readAccount()
			local mServerID,mServerPath,mServerName,mServerState=accountInfo.readServerId()
			if mPassportID and mServerPath and mPassportID~="1" then
				ScutDataLogic.CNetWriter:setUrl(mServerPath)
				send(1004)
			else
				showLayer(2)
			end	
    		end
       end;
        --]]
end;

function  showLayer(type)
	if type==2 then
		send(1001)
	elseif type==3 then
		goto_lan()
	end
end;


--进入服务器界面
function goto_server()
	if m_pidEditTxt~=nil then
		m_pidEditTxt:release()
	end
	if m_pswEditTxt~=nil then
		m_pswEditTxt:release()
	end
	if layer_1~=nil then
		mLayer:removeChild(layer_1,true)
		layer_1=nil
	end;
	layer_1=CCLayer:create();
	
	--LOGO
	local logoImg=CCSprite:create(P(Image.image_logo))
	logoImg:setAnchorPoint(PT(0.5,0))
	logoImg:setScale(pWinSize.width*0.5/logoImg:getContentSize().width)
	logoImg:setPosition(PT(pWinSize.width/2,pWinSize.height-logoImg:getContentSize().height*1.1*0.7))
	layer_1:addChild(logoImg,0)
	
	--背景,z框
	local bgSprite=CCSprite:create(P("common/list_1168.png"))
	bgSprite:setAnchorPoint(PT(0.5,1))

	local scaleBox=SZ(pWinSize.width,pWinSize.height*0.6)
	bgSprite:setScaleX(scaleBox.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(scaleBox.height/bgSprite:getContentSize().height)
	bgSprite:setPosition(PT(pWinSize.width/2,pWinSize.height*0.7))
	layer_1:addChild(bgSprite,0)
	
	----------
	server_button_={}
	local col=2
	local sprite=CCSprite:create(P("button/list_2001.png"))
	local spriteW=math.floor(scaleBox.width*0.8/col)
	local spriteH=sprite:getContentSize().height
	local startX= (pWinSize.width-scaleBox.width*0.8)/2
	local startY=bgSprite:getPosition().y-spriteH
	---bgSprite:getContentSize().height/2
	--[[
	if math.ceil(#serverTabel/col)>1 then
		startY=bgSprite:getPosition().y-bgSprite:getContentSize().height/3
	end
	--]]
	for k , v in pairs(serverTabel) do
		local severBtn=ZyButton:new("button/list_2001.png",nil,nil,v.Name,FONT_NAME,FONT_SM_SIZE)
		severBtn:setTag(k)
		severBtn:registerScriptHandler(choiceServer)
		severBtn:setAnchorPoint(PT(0.5,1))
		local colsW=(k-1)%col*spriteW
		local rowsH=math.floor((k-1)/col)*spriteH
		severBtn:setPosition(PT(startX+spriteW/2+colsW,
								startY-rowsH))
		severBtn:addto(layer_1,0)
		server_button_[#server_button_+1]=severBtn
	end
	mLayer:addChild(layer_1,0)
	isClick=false
end



--点击服务器列表按钮
function choiceServer(pNode)
  --  local index = pNode:getTag()
   -- if index and  index>0 then
      -- local choiceServerInfo=serverTabel[index]
       accountInfo.setServerID(1)
       accountInfo.setServerName(1)
       accountInfo.setServerState(1)
       accountInfo.setServerPath("http://kd1.scutgame.com/Service.aspx")
	       
    --  accountInfo.mServerPath="http://kd1.scutgame.com/Service.aspx"
       accountInfo.saveServerId()
       ScutDataLogic.CNetWriter:setUrl(accountInfo.mServerPath)

     	 	goto_lan()

  --  end
end

function goto_lan()
	LoginScene.init()
end

function send(actionId)
	if actionId == 1001 then--获取服务器列表
				choiceServer()
    		--ScutDataLogic.CNetWriter:setUrl("http://dir.scutgame.com/Service.aspx")
    		--[[
    		local mMobileType, mGameType, mRetailID= accountInfo.readMoble()
    		actionLayer.Action1001(mScene,false,mGameType)	
    		--]]
	elseif actionId == 1004 then--已有账号快速登录
		local mMobileType, mGameType, mRetailID = accountInfo.readMoble()
		local mPassportID, mPassWord = accountInfo.readAccount()
		---[[
		actionLayer.Action1004(mScene,nil, mMobileType ,mPassportID,
			mPassWord,accountInfo.mMac,mGameType,
			pWinSize.width , pWinSize.height,mRetailID,
			accountInfo.ClientAppVersion,accountInfo.mServerID)	
			--]]
	end
end


---延迟进行方法
function delayExec(funName,nDuration)
	local action = CCSequence:createWithTwoActions(
	CCDelayTime:create(nDuration),
	CCCallFunc:create(funName));
	if mLayer then
		mLayer:runAction(action)
	end
end

function netCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	
	if actionId == 1001 then
		ScutScene:registerNetErrorFunc("MainScene.netConnectError");
		local serverInfo=actionLayer._1001Callback(pScutScene, lpExternalData)
   		if serverInfo~=nil then
       		serverTabel={}
       		serverTabel=serverInfo.Severtable
       		goto_server()
       	else
       	    	isClick=false
       	end	
	elseif actionId == 1004 then----已有账号快速登录
		accountInfo.saveConfig("sys/log.ini", "actionId", "actionId" , "1004")
		local LoginResponse = actionLayer._1004Callback(pScutScene, lpExternalData)
   		if LoginResponse ~= nil then
   			accountInfo.UserType = LoginResponse.UserType
			accountInfo.saveServerId();
			ScutDataLogic.CNetWriter:setSessionID(LoginResponse.SessionID);
			ScutDataLogic.CNetWriter:setUserID( Int64(LoginResponse.UserID));
			local personalInfo = PersonalInfo.getPersonalInfo()
			personalInfo._userID = LoginResponse.UserID
			personalInfo._UserType = LoginResponse.UserType	
			if LoginResponse.PassportId ~= nil then
				personalInfo._Pid = LoginResponse.PassportId
				if loginType==8 then
			                   	  accountInfo.setPassportID(LoginResponse.PassportId);
				               accountInfo.SaveAccountInfo();
				end
		       end
		       
		       if accountInfo.mRetailID=="0021"  then
		       	personalInfo.AccessToken=LoginResponse.AccessToken
		       	personalInfo.RefeshToken=LoginResponse.RefeshToken
		       	personalInfo.QihooUserID=LoginResponse.QihooUserID
		       	personalInfo.Scope=LoginResponse.Scope
		       end 
		       
		       if LoginResponse.StatusCode==1005 then
				CreatNewMan.pushScene()
			else
			    onExit()
				progressLayer.replaceScene()
			end
		else
		--	showLayer(2)
		end
	end
end

function netConnectError(pScutScene, nTag)
	LoadingScene.hide();
	ZyLoading.releaseAll()
	local box =ZyMessageBoxEx:new()
	box:doQuery(pScutScene, nil, Language.TIP_NONET, Language.IDS_RETRY, Language.IDS_QUIT, stopServerMessageBoxCallback);
end

function stopServerMessageBoxCallback(clickedButtonIndex, content, tag)
	if clickedButtonIndex == ID_MBOK then
		--正常路线

				local mPassportID, mPassWord=accountInfo.readAccount()
				local mServerID,mServerPath,mServerName,mServerState=accountInfo.readServerId()
				if mPassportID and mServerPath then
					ScutDataLogic.CNetWriter:setUrl(mServerPath)
					send(1004)
				else
					send(1001)
				end

	else
		CCDirector:sharedDirector():endToLua();
	end
end

