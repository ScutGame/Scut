------------------------------------------------------------------
--LoginScene.lua
-- Author     : JMChen
-- Version    : 1.0.0.0
-- Date       : 2011-10-15
-- DescriCCPointion：登陆界面 
------------------------------------------------------------------


module("LoginScene", package.seeall)
require("action.actionLayer")
require("scenes.MainScene")
require("datapool.accountInfo")
require("datapool.PersonalInfo")
require("datapool.Image")
--require("payment.channelEngine")
require("datapool.progressLayer")

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写
 
local mScene = nil 		-- 场景
local mLayer = nil 		-- 层
local layer_1=nil--选择服务器界面

local server_button_=nil--选择服务器按钮
local m_currentPsw=nil--多账户登陆,记录密码
local m_pswEditTxt
local m_pidEditTxt
local isClick=nil
local m_isNormalLogin=nil
local m_createRoleLayer=nil
-----创建角色
local m_playerNameEdit=nil

local head_ok=nil--点选头像打钩图
local head_image=nil

local m_serverType=2   ---显示服务器层
local m_loginType=3    ---显示登录界面
local m_editPsw=4         --显示修改密码

local isFirstLogin=nil
--
---------------公有接口(以下两个函数固定不用改变)--------
--



-- 退出场景
function closeScene()
	releaseResource()
end

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
	accountTable={}
	accountTable.mMobileType,accountTable.mGameType,accountTable.mRetailID=accountInfo.readMoble()
end

function setFirstLogin()
	isFirstLogin=false
end

-- 释放资源
function releaseResource()
	mScene=nil
	mLayer=nil
end
--点击头像响应
function key_head(tag)
	head_ID=tag
	local sprite=head_image[head_ID]
	head_ok:setPosition(CCPoint(sprite:getPosition().x,sprite:getPosition().y))
end;
-- 创建场景
function init(type)
--注册服务器push回调
	initResource()
	if mScene then
		return 
	end
	-- 注册网络回调
	local scene = ScutScene:new()	
	scene:registerCallback(netCallback)	
	mScene = scene.root;
	--mScene:registerOnExit("LoginScene.onExit")
	
	mLayer = CCLayer:create()
	mLayer:setAnchorPoint(CCPoint(0,0))
	mLayer:setPosition(CCPoint(0,0))
	mScene:addChild(mLayer, 0)
	
	CCDirector:sharedDirector():pushScene(mScene)
	CCDirector:sharedDirector():RegisterBackHandler("MainScene.closeApp")
	
	m_CurrentType=type
	if isFirstLogin==nil then
		isFirstLogin=true
	end;
	
	-- 添加背景
	local bgSprite=CCSprite:create(P("imageupdate/default.jpg"))
	bgSprite:setScaleX(pWinSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(pWinSize.height/bgSprite:getContentSize().height)
	bgSprite:setAnchorPoint(CCPoint(0.5,0.5))
	bgSprite:setPosition(CCPoint(pWinSize.width/2,pWinSize.height/2))
	mLayer:addChild(bgSprite,0)
	
     if   m_CurrentType==nil then 
		if _DOWNJOY then
			local sprite=CCSprite:create(P("Default/downjoy.jpg"))
			ChannelImg=sprite
			sprite:setScaleX(pWinSize.width/sprite:getContentSize().width)
			sprite:setScaleY(pWinSize.height/sprite:getContentSize().height)
			sprite:setPosition(CCPoint(pWinSize.width/2,pWinSize.height/2));
			mLayer:addChild(sprite,2);
			delayExec("LanScenes.loginGoSwitch",2)
		else	
			loginGoSwitch()
		end
	else
		loginGoSwitch(m_CurrentType)
	end
end

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
			   --360
	elseif accountTable.mRetailID=="0021" then
			   showLayer(2)
		
    	--UC游戏
    	elseif accountTable.mRetailID=="0036" then
    		if nType ~= ScutUtility.CCPointAndroid then
			  LOGINFOR91SDK= ND91SDK.Login91:CreateLogin91();
		end
    		 showLayer(2)
    	---当乐
    	elseif accountTable.mRetailID=="0037" then
    		 showLayer(2)
    	--360渠道
    	elseif accountTable.mRetailID=="2008" then
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
			if mPassportID and mServerPath then
				ScutDataLogic.CNetWriter:setUrl(mServerPath)
				send(1004)
			else
				showLayer(2)				
			end	
    		end
       end;
        --]]
end;

function netConnectError(pScutScene)

	if pScutScene == nil then
		pScutScene = CCDirector:sharedDirector():getRunningScene()
	end;
	ZyLoading.releaseAll()
	
	ZyToast.show(pScutScene, Language.TIP_NONET, 1.7, 0.3)
	
end;

function netConnectError2(pScutScene)

	if pScutScene == nil then
		pScutScene = CCDirector:sharedDirector():getRunningScene()
	end;
	ZyLoading.releaseAll()
	
	ZyToast.show(pScutScene, Language.TIP_NONET, 1.7, 0.3)
	
	goto_main()
end;

function  showLayer(type)
	if type==2 then
		send(1001)
	elseif type==3 then
		goto_login()
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
	logoImg:setAnchorPoint(CCPoint(0.5,0))
	logoImg:setScale(0.7)
	logoImg:setPosition(CCPoint(pWinSize.width/2,pWinSize.height-logoImg:getContentSize().height*1.1*0.7))
	layer_1:addChild(logoImg,0)
	
	--背景,z框
	local bgSprite=CCSprite:create(P("common/list_9998_3.png"))
	bgSprite:setAnchorPoint(CCPoint(0.5,1))

	local scaleBox=SZ(pWinSize.width,pWinSize.height*0.7)
	bgSprite:setScaleX(scaleBox.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(scaleBox.height/bgSprite:getContentSize().height)
	bgSprite:setPosition(CCPoint(pWinSize.width/2,pWinSize.height*0.7))
	
	
--	bgSprite:setPosition(CCPoint(pWinSize.width/2,logoImg:getPosition().y-SY(5)))
	layer_1:addChild(bgSprite,0)
	
	--------
	server_button_={}
	local col=3
	local sprite=CCSprite:create(P("button/list_2001.png"))
	local spriteW=math.floor(scaleBox.width*0.8/col)
	local spriteH=sprite:getContentSize().height
	local startX= (pWinSize.width-scaleBox.width*0.8)/2
	local startY=bgSprite:getPosition().y-spriteH
	for k , v in pairs(serverTabel) do
		local severBtn=ZyButton:new("button/list_2001.png",nil,nil,v.Name,FONT_NAME,FONT_SM_SIZE)
		severBtn:setTag(k)
		severBtn:registerScriptTapHandler(LoginScene.key_serve_button_ok)
		severBtn:setAnchorPoint(CCPoint(0.5,1))
		local colsW=(k-1)%col*spriteW
		local rowsH=math.floor((k-1)/col)*spriteH
		severBtn:setPosition(CCPoint(startX+spriteW/2+colsW,
								startY-rowsH))
		severBtn:addto(layer_1,0)
		server_button_[#server_button_+1]=severBtn
	end
	mLayer:addChild(layer_1,0)
	isClick=false
end;

function onExit()
end;


 --选择服务器按钮
function key_serve_button_ok(index)
	--local tag=item:getTag()
	
	--local serverInfo=serverTabel[index]
	accountInfo.setServerID(1)
	accountInfo.setServerName(1)
	accountInfo.setServerState(1)
	accountInfo.setServerPath("ddz.scutgame.com:9700")
	
	accountInfo.saveServerId()
	ScutDataLogic.CNetWriter:setUrl(accountInfo.mServerPath)

--    goto_login()
	goto_main()
end;

--主界面
function goto_main()
	if layer_1~=nil then
		mLayer:removeChild(layer_1,true)
		layer_1=nil
	end
	if m_pidEditTxt~=nil then
		m_pidEditTxt:release()
		m_pswEditTxt:release()
	end
	layer_1=CCLayer:create();
	layer_1:setAnchorPoint(CCPoint(0,0))
	layer_1:setPosition(CCPoint(0,0))
	mLayer:addChild(layer_1)
	
	--LOGO
	local logoImg=CCSprite:create(P("Image/logo1.png"))
	logoImg:setScaleX(pWinSize.width*0.5/logoImg:getContentSize().width)
	logoImg:setScaleY(pWinSize.height*0.4/logoImg:getContentSize().height)
	logoImg:setAnchorPoint(CCPoint(0.5,0.5))
	logoImg:setPosition(CCPoint(pWinSize.width/2,pWinSize.height/2-pWinSize.height*0.4/2 ))
	layer_1:addChild(logoImg,0)
	local posY=	logoImg:getPosition().y-pWinSize.height*0.4/2

	--[快速注册按钮
	registBtn=ZyButton:new(Image.image_button_0,Image.image_button_1,nil,nil,FONT_NAME,FONT_SM_SIZE)
	registBtn:setAnchorPoint(CCPoint(0,0))
	registBtn:setPosition(CCPoint(pWinSize.width/2-registBtn:getContentSize().width*1.5,
					posY-registBtn:getContentSize().height))
	registBtn:registerScriptTapHandler(
function(pSender)
	 if not isClick then
		isClick=true
		m_currentPsw=nil
		send(1002)
		if m_pidEditTxt~=nil then
			m_pidEditTxt:release()
			m_pswEditTxt:release()
		end
	  --goto_new_man()
	end
end)
	registBtn:addImage("title/panle_1055.png")
	registBtn:addto(layer_1)
	
	--[登录按钮
	loginBtn=ZyButton:new(Image.image_button_0,Image.image_button_1,nil, nil,FONT_NAME,FONT_SM_SIZE)
	loginBtn:setAnchorPoint(CCPoint(0.5,0))
	loginBtn:setPosition(CCPoint(pWinSize.width/2+loginBtn:getContentSize().width,posY-loginBtn:getContentSize().height))
	loginBtn:registerScriptTapHandler( 
 function (pSender)
 	    	local nType = ScutUtility.ScutUtils:GetPlatformType();
   	if accountInfo.mRetailID=="2008"  then
        	 channelEngine.login()
        elseif accountInfo.mRetailID=="0021"  then     
        	loginType=8
        	 channelEngine.login()        	
        elseif accountInfo.mRetailID=="0036"  then
        	if nType == ScutUtility.CCPointAndroid then	
        		 channelEngine.login()
        	else
        		loginUC()
        	end
        elseif accountInfo.mRetailID=="0037"  then
        	if nType == ScutUtility.CCPointAndroid then
        		 channelEngine.login()
        	else
			loginDangLe()
        	end
        elseif accountInfo.mRetailID=="0001"  then
        	if nType == ScutUtility.CCPointAndroid then
        		 channelEngine.login()
        	else
        		logining91()
        	end
	else
		goto_login()
	end
 end)
	loginBtn:addImage("title/panle_1056.png")
	loginBtn:addto(layer_1)
	if     accountTable.mRetailID=="0001" or    accountTable.mRetailID=="0036" or  accountTable.mRetailID=="0021" then
		registBtn:setIsVisible(false)
		loginBtn:setPosition(CCPoint(pWinSize.width/2,posY-loginBtn:getContentSize().height))	
	end
	
	getCurrentAccount()
  
	isClick=false	
end;


 -- 点击开始游戏
function key_start_game()
	local name=m_playerNameEdit:GetEditText()
	m_playerNameEdit:setVisible(false)
	if name==nil or name=="" then
		local box = ZyMessageBoxEx:new()
		box:doPromCCPoint(mScene, nil, Language.LAN_ONT_NAME,Language.IDS_OK,setNewManInput)
	else
		send(1005)
	end;
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
	    	  loginType=8
	    	  actionLayer.Action1004(mScene,nil,mMobileType,"0","0",DeviceID,GameType,ScreenX,ScreenY,RetailID,nil,mServerId,loginUin);
  --]]
end;


function getLogin91Sdk()
  return LOGINFOR91SDK
end
 
 
----------登陆回调
function loginCallBack(tData)
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
		if  channelEngine.getType()  == ChannelConfig.uc then
			  loginType=8
			  ScutDataLogic.CNetWriter:setSessionID(sid);
			  actionLayer.Action1004(mScene,nil,mMobileType,"0","0",DeviceID,GameType,ScreenX,ScreenY,RetailID,nil,mServerId);		  
		elseif  channelEngine.getType()  == ChannelConfig.qihoo360 then
			  actionLayer.Action1004(mScene,nil,mMobileType,"0","0",DeviceID,GameType,ScreenX,ScreenY,
			  				RetailID,nil,mServerId,nil,sid);
		elseif  channelEngine.getType()  == ChannelConfig.downjoy then
		    		loginType=8
			  	actionLayer.Action1004(mScene,nil,mMobileType,userid,"0",DeviceID,GameType,ScreenX,ScreenY,RetailID,nil,mServerId,nil,nil,sid);
		elseif  channelEngine.getType()  == ChannelConfig.sdk91 then
				loginType=8
		          if  sid ~=nil then
		               ScutDataLogic.CNetWriter:setSessionID(sid);
		          end		          
				actionLayer.Action1004(mScene,nil,mMobileType,"0","0",DeviceID,GameType,ScreenX,ScreenY,RetailID,nil,mServerId,userid);
		end
	end
end



 
function goto_login()
	if layer_1~=nil then
		mLayer:removeChild(layer_1,true)
		layer_1=nil
	end
	layer_1=CCLayer:create();
	layer_1:setAnchorPoint(CCPoint(0,0))
	layer_1:setPosition(CCPoint(0,0))
	mLayer:addChild(layer_1)
	
	--半透明背景图
	local touming_bg=CCSprite:create(P("common/ground_2018.9.png"))
	touming_bg:setScaleX(pWinSize.width/touming_bg:getContentSize().width)
	touming_bg:setScaleY(pWinSize.height/touming_bg:getContentSize().height)
	touming_bg:setAnchorPoint(CCPoint(0.5,0.5))
	touming_bg:setPosition(CCPoint(pWinSize.width/2,pWinSize.height/2))
	layer_1:addChild(touming_bg,0)
	
	--LOGO
	local logoImg=CCSprite:create(P(Image.image_logo))
	logoImg:setScaleX(pWinSize.width*0.5/logoImg:getContentSize().width)
	logoImg:setScaleY(pWinSize.height*0.4/logoImg:getContentSize().height)
	logoImg:setAnchorPoint(CCPoint(0.5,0))
	logoImg:setPosition(CCPoint(pWinSize.width/2,pWinSize.height-pWinSize.height*0.4))
	layer_1:addChild(logoImg,1)
	--背景框
	local bgSprite=CCSprite:create(P("common/panle_1052.png"))
	bgSprite:setAnchorPoint(CCPoint(0.5,0.5))
	bgSprite:setPosition(CCPoint(pWinSize.width/2,pWinSize.height/2))
	layer_1:addChild(bgSprite,0)
	
	--输入框底图
	local aa=nil
	local ww=288
	local hh=0
	local xx=(pWinSize.width-ww)/2
	
	--[快速进入按钮
	local startY=bgSprite:getPosition().y-bgSprite:getContentSize().height/2
	
	--返回按钮
	ChoiceBtn2=ZyButton:new(Image.image_button_0,Image.image_button_1,nil, nil,FONT_NAME,FONT_SM_SIZE)
	ChoiceBtn2:setAnchorPoint(CCPoint(0.5,0))
	ChoiceBtn2:setPosition(CCPoint(pWinSize.width/2-ChoiceBtn2:getContentSize().width/2-SX(20),startY-ChoiceBtn2:getContentSize().height-SY(5)))
	ChoiceBtn2:registerScriptTapHandler(goto_main)
	ChoiceBtn2:addImage("title/button_1009.png")
	ChoiceBtn2:addto(layer_1)

	--[登陆按钮
	ChoiceBtn=ZyButton:new(Image.image_button_0,Image.image_button_1,nil,nil,FONT_NAME,FONT_SM_SIZE)
	ChoiceBtn:setAnchorPoint(CCPoint(0.5,0))
	ChoiceBtn:setPosition(CCPoint(pWinSize.width/2+ChoiceBtn:getContentSize().width/2+SX(20),startY-ChoiceBtn:getContentSize().height-SY(5)))
	ChoiceBtn:registerScriptTapHandler(key_in)
	ChoiceBtn:addImage("title/button_1007.png")
	ChoiceBtn:addto(layer_1)
	
	--密码文字
	local imgSprite=CCSprite:create(P(Image.image_list_txt));
	local txt_h= imgSprite:getContentSize().height--输入框高
	--[[
	if txt_h< Tools.get_String_1_h(FONT_NAME,FONT_SM_SIZE) then
		txt_h=Tools.get_String_1_h(FONT_NAME,FONT_SM_SIZE)
	end;
	]]
	local titleName1=CCSprite:create(P("title/panle_1053.png"));
	titleName1:setAnchorPoint(CCPoint(0,0))
	titleName1:setPosition(CCPoint(pWinSize.width/2-titleName1:getContentSize().width*2,
		startY+titleName1:getContentSize().height+SY(10)))
	layer_1:addChild(titleName1)
	--[文本输入框2底图
	local txt_x=titleName1:getPosition().x+SX(8)+titleName1:getContentSize().width--输入宽坐标
	local txt_ww=xx+ww-txt_x-SX(44)--输入框宽
	local bgEmCCPointy1= CCSprite:create(P(Image.image_list_txt))
	bgEmCCPointy1:setScaleX(pWinSize.width*0.25/bgEmCCPointy1:getContentSize().width)
	bgEmCCPointy1:setScaleY(titleName1:getContentSize().height/bgEmCCPointy1:getContentSize().height)
	bgEmCCPointy1:setAnchorPoint(CCPoint(0,0))
	bgEmCCPointy1:setPosition(CCPoint(txt_x,titleName1:getPosition().y+SY(5)))
	layer_1:addChild(bgEmCCPointy1)
	--[文本编辑
	if m_pswEditTxt~=nil then
		m_pswEditTxt:release()
		m_pswEditTxt=nil
	end
	--文本编辑
	m_pswEditTxt = CScutEdit:new()
--	m_pswEditTxt:init(false, true)
	m_pswEditTxt:init(false, true, ccc4(235,197, 151, 255),ccc4(0,39,61,200))
	m_pswEditTxt:setRect(CCRect(bgEmCCPointy1:getPosition().x+SX(8),
		pWinSize.height-(bgEmCCPointy1:getPosition().y+titleName1:getContentSize().height)+SY(4),
		pWinSize.width*0.24,titleName1:getContentSize().height))
	m_pswEditTxt:setVisible(true)--是否显
	--[账号文字
	local titleName=CCSprite:create(P("title/panle_1054.png"));
	titleName:setAnchorPoint(CCPoint(0,0))
	aa=(hh/2-titleName:getContentSize().height)/2--间距
	titleName:setPosition(CCPoint(pWinSize.width/2-titleName:getContentSize().width*2,titleName1:getPosition().y+txt_h+SY(10)))
	layer_1:addChild(titleName)
	--[文本输入框1底图
	local bgEmCCPointy= CCSprite:create(P(Image.image_list_txt));
	bgEmCCPointy:setScaleX(pWinSize.width*0.25/bgEmCCPointy:getContentSize().width)
	bgEmCCPointy:setScaleY(titleName:getContentSize().height/bgEmCCPointy:getContentSize().height)
	bgEmCCPointy:setAnchorPoint(CCPoint(0,0))
	bgEmCCPointy:setPosition(CCPoint(txt_x,titleName:getPosition().y+SY(5)))
	layer_1:addChild(bgEmCCPointy)
	if m_pidEditTxt~=nil then
		m_pidEditTxt:release()
		m_pidEditTxt=nil
	end
	m_pidEditTxt = CScutEdit:new()
--	m_pidEditTxt:init(false, false)
	m_pidEditTxt:init(false, false, ccc4(235,197, 151, 255),ccc4(0,39,61,200))
	m_pidEditTxt:setRect(CCRect(bgEmCCPointy:getPosition().x+SX(8),
		pWinSize.height-(bgEmCCPointy:getPosition().y+titleName:getContentSize().height)+SY(4),
		pWinSize.width*0.24,titleName:getContentSize().height))
	m_pidEditTxt:setVisible(true)--是否显示
	
	getCurrentAccount()
	if acountInfoTable ~= nil then
	    local message = acountInfoTable[1]
        if message~=nil then
            if message.PassportID~=nil then
                m_pidEditTxt:setText(message.PassportID)
                if message.pwd~=nil and message.pwd~="error" then
                    m_currentPsw=message.pwd
                    m_pswEditTxt:setText("******")
                end
            end
        end
    end
	
	listWidth=txt_ww
	listHeight=bgEmCCPointy:getContentSize().height*3
	list_x=bgEmCCPointy:getPosition().x
	list_y=bgEmCCPointy:getPosition().y-listHeight
	isClick=false
end

--注册按钮
function regist()
	setInputText(false)
	
	local box = ZyMessageBoxEx:new()
	box:doPromCCPoint(mScene, nil, Language.LAN_WARM,Language.IDS_OK,setLoginInput)	
end;

function setLoginInput()
	setInputText(true)
end;

-- 返回按钮
function key_back()
	if not isClick then
		isClick=true
		if m_pidEditTxt~=nil then
			m_pidEditTxt:release()
			m_pidEditTxt=nil
		end
		if m_pswEditTxt~=nil then
			m_pswEditTxt:release()
			m_pswEditTxt=nil
		end
		if m_pswSureTxt~=nil then
			m_pswSureTxt:release()
			m_pswSureTxt=nil
		end
		goto_server()
	end
end;



--登陆按钮
function key_in()
	 if not isClick then
	 
		m_isNormalLogin=true
		pid=m_pidEditTxt:GetEditText()
		password=m_pswEditTxt:GetEditText()
		if password~="******" then
		    password=ScutDataLogic.CFileHelper:encryptPwd( password, nil):getCString()	
		    m_currentPsw=password
		elseif  m_currentPsw~=nil then
		    password=m_currentPsw
		end
	
		state=2
		if  pid==""   then
			--跳出的提示界面
			local box = ZyMessageBoxEx:new()
			box:doPrompt(mScene, nil, Language.LAN_ID_NOT_Z,Language.IDS_OK,messageCallback)
			m_pswEditTxt:setVisible(false);
			m_pidEditTxt:setVisible(false);
		elseif password=="" then
			--跳出的提示界面
			local box = ZyMessageBoxEx:new()
			box:doPromCCPoint(mScene, nil, Language.LAN_ID_NOT_PASSWOED,Language.IDS_OK,messageCallback)
			m_pswEditTxt:setVisible(false);
			m_pidEditTxt:setVisible(false)
		else
			local    mMobileType ,mGameType,mRetailID=accountInfo.readMoble()
			local 	mMac=accountInfo.mMac
			local  mServerID,mServerPath,mServerName,mServerState=accountInfo.readServerId()
			local ScreenX=pWinSize.width
			local ScreenY=pWinSize.height
			setInputText(false)
			--if _DOWNJOY then 
			--	if downJoyPid then
	        	 --   		 actionLayer.Action1004(mScene,nil,mMobileType,pid,password,mMac,mGameType,ScreenX,ScreenY,mRetailID,nil,mServerID);
	        	--	else            --普通登录
	        	 --		 actionLayer.Action1004(mScene,nil,mMobileType,"0",password,mMac,mGameType,ScreenX,ScreenY,mRetailID,nil,mServerID,pid);
	        	--	end
			--else
				actionLayer.Action1004(mScene,nil,mMobileType,pid,password,mMac,mGameType,
				ScreenX ,ScreenY,mRetailID,accountInfo.ClientAppVersion,mServerID)
			--ends
	
		end;
	end
--	if m_pidEditTxt~=nil then
--		m_pidEditTxt:release()
--		m_pswEditTxt:release()
--	end
--	MainScene.releaseResource()
--	MainScene.initScene()
--	progressLayer.replaceScene()
end;

function messageCallback()
    if m_pswEditTxt then
	m_pswEditTxt:setVisible(true);
	m_pidEditTxt:setVisible(true);
	end
end;

function  getCurrentAccount()
	local accountTabel={
					{name="Account"},
					{name="Account1"},
					{name="Account2"}}
	acountInfoTable={}
	for k, v in ipairs(accountTabel) do
		if getRemenberAccount(v.name) then
			local pid,pwd=getRemenberAccount(v.name)
				local tag=#acountInfoTable+1
				acountInfoTable[tag]={}
				acountInfoTable[tag].PassportID=pid
				acountInfoTable[tag].pwd=pwd
		end
	end
end;

function  getRemenberAccount(accountId)
	local PassportID= accountInfo.getConfig("sys/account.ini",accountId,"PassportID")
	local pwd= accountInfo.getConfig("sys/account.ini",accountId,"pw")
	if PassportID~=nil and PassportID~=0 and PassportID~="1" and PassportID~=1 and pwd~=nil and PassportID~="error" and pwd~="error" then
		return PassportID,pwd
	end
		 return false
end;

function  setInputText(isEnabel)
	if m_pidEditTxt~=nil then
		m_pidEditTxt:setVisible(isEnabel)
	end
	if m_pswEditTxt~=nil then
		m_pswEditTxt:setVisible(isEnabel)
	end
end;

--去创建角色界面
function goto_new_man()
    if m_createRoleLayer then
        return
    end
    if layer_1 then
        layer_1:getParent():removeChild(layer_1,true)
        layer_1=nil
    end
	local layer_1=CCLayer:create();
	m_createRoleLayer=layer_1
	layer_1:setAnchorPoint(CCPoint(0,0))
	layer_1:setPosition(CCPoint(0,0))
	mLayer:addChild(layer_1)
	
	--半透明背景图
	local touming_bg=CCSprite:create(P("common/ground_2018.9.png"))
	touming_bg:setScaleX(pWinSize.width/touming_bg:getContentSize().width)
	touming_bg:setScaleY(pWinSize.height/touming_bg:getContentSize().height)
	touming_bg:setAnchorPoint(CCPoint(0.5,0.5))
	touming_bg:setPosition(CCPoint(pWinSize.width/2,pWinSize.height/2))
	layer_1:addChild(touming_bg,0)
	--LOGO
	
	--底边框
	local dibianImg=CCSprite:create(P("common/panel_1001_1.png"))
	dibianImg:setAnchorPoint(CCPoint(0,0))
	dibianImg:setPosition(CCPoint(0,0))
	layer_1:addChild(dibianImg,0)
	
	--开始游戏按钮
	local  ChoiceBtn=ZyButton:new(Image.image_button_start_1,Image.image_button_start_0,nil,nil,FONT_NAME,FONT_SM_SIZE)
	ChoiceBtn:setAnchorPoint(CCPoint(0.5,0))
	ChoiceBtn:setPosition(CCPoint(pWinSize.width/2,dibianImg:getContentSize().height+SY(5)))
	ChoiceBtn:registerScriptTapHandler(key_start_game)
	ChoiceBtn:addto(layer_1)

	--创建角色框尺寸
	local ww=ChoiceBtn:getContentSize().width
	local hh=pWinSize.height/4--链条框高度
	local xx=(pWinSize.width-ww)/2
	local txt_hh=hh/2-SY(10)--输入框高度
	
	--名字字数限制
	local contentLabel=CCLabelTTF:create(Language.LAN_REGIST_WARM,FONT_NAME,FONT_SM_SIZE);
	contentLabel:setAnchorPoint(CCPoint(0.5,0))
	contentLabel:setPosition(CCPoint(pWinSize.width/2,dibianImg:getContentSize().height+SY(5)+ChoiceBtn:getContentSize().height+SY(10)))
	layer_1:addChild(contentLabel)
	
	---文本输入框1底图
	local txt_w=ww
	local bgEmCCPointy= CCSprite:create(P(Image.image_list_txt));
	bgEmCCPointy:setScaleX(txt_w/bgEmCCPointy:getContentSize().width)
	bgEmCCPointy:setAnchorPoint(CCPoint(0,0))
	bgEmCCPointy:setPosition(CCPoint((pWinSize.width-txt_w)/2,contentLabel:getPosition().y+contentLabel:getContentSize().height+SY(2)))
	layer_1:addChild(bgEmCCPointy,0)
	----------文本编辑
	m_playerNameEdit = CScutEdit:new()
	m_playerNameEdit:SetTextSize(FONT_SM_SIZE)
	m_playerNameEdit:init(false, false,ccc4(235,197, 151,255), ccc4(0,39,61,200))
	m_playerNameEdit:setRect(CCRect(bgEmCCPointy:getPosition().x+SX(8),pWinSize.height-(bgEmCCPointy:getPosition().y+bgEmCCPointy:getContentSize().height)+SY(6) ,txt_w-SY(16), bgEmCCPointy:getContentSize().height-SY(10)))
	m_playerNameEdit:setVisible(true)--是否显示
	--名字文字
	local titleName=CCSprite:create(P("title/panle_1066.png"));
	titleName:setAnchorPoint(CCPoint(0,0))
	titleName:setPosition(CCPoint(bgEmCCPointy:getPosition().x-titleName:getContentSize().width-SY(4),
		bgEmCCPointy:getPosition().y+bgEmCCPointy:getContentSize().height/2-titleName:getContentSize().height/2))
	layer_1:addChild(titleName)
	--点选头像打钩图标
  	Sex=0
	head_ok= CCSprite:create(P("common/font_1015.png"));
  	 head_ok:setAnchorPoint(CCPoint(0.5,0.5))
	local sizeHeadClick=head_ok:getContentSize()
	layer_1:addChild(head_ok,1)
	
  	---------创建主角头像
  	head_image={}
  	local col=5
  	local head_ww=sizeHeadClick.width*col
  	local colW=sizeHeadClick.width+SY(2)
  	local rowH=sizeHeadClick.height*0.8
	local head_xx=(pWinSize.width-head_ww)/2
	local head_yy=bgEmCCPointy:getPosition().y+bgEmCCPointy:getContentSize().height+SY(2)
  	headImgTable={"head_1001","head_1002","head_1003",
  			"head_1004","head_1005"}
  	head_ID=1
  	
  	local dibianImg=CCSprite:create(P("common/panle_1011_1.png"))
  	dibianImg:setScaleX(pWinSize.width/dibianImg:getContentSize().width)
  	dibianImg:setScaleY(pWinSize.height*0.4/dibianImg:getContentSize().height)
	dibianImg:setAnchorPoint(CCPoint(0.5,0.5))
	dibianImg:setPosition(CCPoint(pWinSize.width/2,head_yy+dibianImg:getContentSize().height/2))
	layer_1:addChild(dibianImg,0)
  	
  	for k , v in pairs(headImgTable) do
  		local myHead=string.format("headImg/%s.png",v)
  		local headBg=ZyButton:new("common/panel_1001_6.png")
		headBg:addImage(myHead)
  		headBg:setAnchorPoint(CCPoint(0.5,0.5))
  		headBg:registerScriptTapHandler(key_head)
		headBg:setTag(k)
  		local headColW=(k-1)%col*colW*1.03
		headBg:setPosition(CCPoint(head_xx+colW/2+headColW-SY(10),
  					dibianImg:getPosition().y))
  		head_image[#head_image+1]=headBg
  		headBg:addto(layer_1,1)
  		if k==1 then
		head_ok:setScaleX(headBg:getContentSize().width*1.2/head_ok:getContentSize().width)
		head_ok:setScaleY(headBg:getContentSize().height*1.2/head_ok:getContentSize().height)
		head_ok:setPosition(headBg:getPosition())
		end
  	end	 
	 isClick=false
end

function setNewManInput()
	m_playerNameEdit:setVisible(true)
end;

--点击返回按钮
function key_new_man_brack()
    if not isClick then
    isClick=true
	if m_createRoleLayer~=nil then
		m_playerNameEdit:release()
		m_playerNameEdit=nil
		m_createRoleLayer:getParent():removeChild(m_createRoleLayer,true)
		m_createRoleLayer=nil
	end
	goto_login()
	end
end






--对比账号
function checkAccount(pid,psw)
	for k, v in ipairs(acountInfoTable) do
		if v~=nil then
			if v.PassportID==pid then
				return true
			end
		end
	end
	local tag=#acountInfoTable+1
	acountInfoTable[tag]={}
	acountInfoTable[tag].PassportID=pid
	acountInfoTable[tag].pwd=ScutDataLogic.CFileHelper:encryptPwd(psw, nil):getCString()
	orderAccount(acountInfoTable[tag].PassportID,acountInfoTable[tag].pwd)
end

--对账号进行排序
function  orderAccount(pid,psw,type)
	local table={}
	table[1]={}
	table[1].PassportID=pid
	table[1].pwd=psw
	if type~=nil then
        table[1].pwd=ScutDataLogic.CFileHelper:encryCCPointPwd(psw, nil):getCString()
	end
    if m_currentPsw~=nil then
        table[1].pwd=m_currentPsw
        m_currentPsw=nil
    end
	for k, v in ipairs(acountInfoTable) do
		if v~=nil then
			if v.PassportID~=pid then
				local tag=#table+1
				table[tag]={}
				table[tag].PassportID=v.PassportID
				table[tag].pwd=v.pwd
			end
		end
	end
	acountInfoTable=nil
	acountInfoTable=table
	saveCurrentAccount(acountInfoTable)
end;

--保存账号信息
function  saveCurrentAccount(table)
local accountTabel={{name="Account"},
					{name="Account1"},
					{name="Account2"}}
		for k, v in ipairs(accountTabel) do
		    if table[k]==nil then
		        table[k]={}
		    end
			accountInfo.saveConfig("sys/account.ini", v.name, "PassportID" , isNoAccout(table[k].PassportID))
			accountInfo.saveConfig("sys/account.ini", v.name, "pw" , isNoAccout(table[k].pwd))
		end
end;

function  isNoAccout(num)
 	if num==nil then
 		num="error"
 	end
 	return num
end;

--发送请求
function send(actionID)
--请求服务器列表
	if  actionID==1001 then
	
		 key_serve_button_ok()
		--[[
    		ScutDataLogic.CNetWriter:setUrl("http://dir.scutgame.com/Service.aspx")
    		local mMobileType ,mGameType,mRetailID= accountInfo.readMoble()
    		actionLayer.Action1001(mScene,false,mGameType)
    		--]]
	elseif  actionID==1002 then
		accountInfo.readMoble()
		actionLayer.Action1002(mScene,false,
			accountInfo.mMobileType,
			accountInfo.mGameType,
			accountInfo.mRetailID,
			accountInfo.ClientAppVersion,
			pWinSize.width,
			pWinSize.height,
			accountInfo.getMac(),accountInfo.mServerID)
	elseif  actionID==1004 then
		local mMobileType ,mGameType,mRetailID=accountInfo.readMoble()
		local   mPassportID, mPassWord=accountInfo.readAccount()
		setInputText(false)
		actionLayer.Action1004(mScene,nil, mMobileType ,mPassportID,
			mPassWord,accountInfo.mMac,mGameType,
			pWinSize.width , pWinSize.height,mRetailID,
			accountInfo.ClientAppVersion,accountInfo.mServerID)
	elseif  actionID==1005 then
		local userName=m_playerNameEdit:GetEditText()
		accountInfo.readMoble()
		local serverID=accountInfo.readServerId()
		local   mPassportID, mPassWord=accountInfo.readAccount()
		if PersonalInfo.getPersonalInfo()._Pid~=nil then
			mPassportID=PersonalInfo.getPersonalInfo()._Pid
		end
		actionLayer.Action1005(mScene,nil,
			userName,Sex,
			headImgTable[head_ID] ,
			accountInfo.mRetailID,
			mPassportID,
			accountInfo.mMobileType,
			pWinSize.width,
			pWinSize.height,
			accountInfo.ClientAppVersion,
			accountInfo.mGameType,
			serverID)
	end
end

function setLoginInput()
	setInputText(true)
end;

---网络回调
function netCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID();
	local userData = ZyRequestParam:getParamData(lpExternalData)
    if actionId ==1001 then
		local serverInfo=actionLayer._1001Callback(pScutScene, lpExternalData)
   		if serverInfo~=nil then
       		serverTabel={}
       		serverTabel=serverInfo.Severtable
       		key_serve_button_ok(1)
       		--goto_server()
       	else
       	    	isClick=false
       	end
       
	elseif actionId ==1002 then
		local LoginResponse=actionLayer._1002Callback(pScutScene, lpExternalData)
		if LoginResponse~=nil then
      		if LoginResponse.PassportID~=nil and LoginResponse.Password~=nil then
      				isQuiklyPid=true
      				local Pid=LoginResponse.PassportID 
				local Pwd=LoginResponse.Password	
				accountInfo.mPassWord=LoginResponse.Password
				accountInfo.mPassportID=LoginResponse.PassportID 	
				if  checkAccount(Pid,Pwd) then
							orderAccount(Pid,Pwd,1)
				end         
				--ZyToast.show(pScutScene, Pid,1,0.2)				
			--	accountInfo.SaveAccountInfo()
		              --登陆
   				send(1004)
 			end
 		else
 		    isClick=false
			setInputText(false)
      			local box = ZyMessageBoxEx:new()
			box:doPromCCPoint(pScutScene, nil, ZyReader:readErrorMsg(),Language.IDS_OK,messageCallback)
  		end
	elseif actionId ==1004 then	
		local StatusCode=ZyReader:getResult()
		local LoginResponse=actionLayer._1004Callback(pScutScene, lpExternalData)
		if LoginResponse~=nil then
			--if not Login91 and not _UCGAME  then
					accountInfo.UserType=LoginResponse.UserType
					if not    isQuiklyPid  then
						if m_pswEditTxt~=nil and string.len(m_pswEditTxt:GetEditText())>0 then
							local Pid=m_pidEditTxt:GetEditText()
							local Pwd=m_pswEditTxt:GetEditText()
							if acountInfoTable==nil then
								getCurrentAccount()
							end
							if  checkAccount(Pid,Pwd) then
								orderAccount(Pid,Pwd,1)
					              end	
			             	 	end
			              end
			--end
			accountInfo.saveServerId();
			
			ScutDataLogic.CNetWriter:setSessionID(LoginResponse.SessionID);
			ScutDataLogic.CNetWriter:setUserID( Int64(LoginResponse.UserID));
			local personalInfo=PersonalInfo.getPersonalInfo()
			personalInfo._userID=LoginResponse.UserID
			personalInfo._UserType=LoginResponse.UserType

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
		       
		       if m_pidEditTxt~=nil then
	      			m_pidEditTxt:release()
	         		m_pswEditTxt:release()
			end
			if StatusCode==1005 then
			    goto_new_man()
			else
				progressLayer.replaceScene()
			end
		else
			if isFirstLogin==true then
				showLayer( m_serverType)
				isFirstLogin=false
			else
				local box = ZyMessageBoxEx:new()
				box:doPromCCPoint(pScutScene, nil, ZyReader:readErrorMsg(),Language.IDS_OK,setLoginInput)
			end
    		end
		    m_isNormalLogin=false
		    isClick=false
			-------------------------创建角色接口
	elseif actionId ==1005 then
		if ZyReader:getResult() == eScutNetSuccess then
			m_playerNameEdit:release()
			--MainMapLayer.setFirstPos()
			if m_createRoleLayer~=nil then
				m_createRoleLayer:getParent():removeChild(m_createRoleLayer,true)
				m_createRoleLayer=nil
			end
			m_playerNameEdit=nil
			progressLayer.replaceScene()
			accountInfo.setfirstPlot(true)
		else      
             local box = ZyMessageBoxEx:new()
	      box:doPrompt(pScutScene, nil, ZyReader:readErrorMsg(),Language.IDS_OK,setNewManInput)		
		end
    end
end