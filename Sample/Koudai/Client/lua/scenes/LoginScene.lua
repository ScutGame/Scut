------------------------------------------------------------------
-- LoginScene.lua
-- Author     : Zonglin Liu
-- Version    : 
-- Date       :   
-- Description: 注册新用户界面
------------------------------------------------------------------

module("LoginScene", package.seeall)
require("scenes.CreatNewMan")

mScene = nil 		-- 场景



-- 场景入口
function pushScene()
	initResource()
	createScene()
	
end

-- 退出场景
function popScene()
	releaseResource()
	
end

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()

end

-- 释放资源
function releaseResource()
	if showLayer ~= nil then
		showLayer:getParent():removeChild(showLayer,true)
		showLayer=nil
	end
	if m_pidEditTxt~=nil then
		m_pidEditTxt:release()
		m_pidEditTxt=nil
	end
	if m_pswEditTxt~=nil then
		m_pswEditTxt:release()
		m_pswEditTxt=nil
	end
	
	mScene = nil
end
function onExit()
	releaseResource()
end;

-- 创建场景
function init()
	local scene  = ScutScene:new()
    mScene = scene.root
	runningScene = CCDirector:sharedDirector():getRunningScene()
	if runningScene == nil then
		CCDirector:sharedDirector():runWithScene(mScene)
	else
		 commReplaceScene(mScene)
	end
	
	-- 注册网络回调
	mScene = scene.root 
	scene:registerCallback(networkCallback)
		mScene:registerScriptHandler(SpriteEase_onEnterOrExit)
	-- 添加主层
	mLayer= CCLayer:create()
	mScene:addChild(mLayer, 0)

	--背景
	m_bgImage= CCSprite:create(P(Image.image_bg));
	m_bgImage:setScaleX(pWinSize.width/m_bgImage:getContentSize().width)
	m_bgImage:setScaleY(pWinSize.height/m_bgImage:getContentSize().height)
	m_bgImage:setAnchorPoint(PT(0.5,0.5))
	m_bgImage:setPosition(PT(pWinSize.width/2,pWinSize.height/2))
	mLayer:addChild(m_bgImage,-1)


	-- 此处添加场景初始内容
	showContent()
end

function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        onExit()
    end
end


-- 去登陆界面
function showContent()
	if showLayer ~= nil then
		showLayer:getParent():removeChild(showLayer,true)
		showLayer=nil
	end
	
	local layer=CCLayer:create();
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(0,0))
	mLayer:addChild(layer,0)
	
	showLayer = layer
	
	--半透明背景图
	--LOGO
	local logoImg=CCSprite:create(P(Image.image_logo))
	logoImg:setScale(pWinSize.width*0.8/logoImg:getContentSize().width)
	logoImg:setAnchorPoint(PT(0.5,0))
	logoImg:setPosition(PT(pWinSize.width/2,pWinSize.height-logoImg:getContentSize().height*1.1))
	layer:addChild(logoImg,0)
	
	--背景框
	local bgSprite=CCSprite:create(P("common/list_1168.png"))
	local scale = pWinSize.width/bgSprite:getContentSize().width
	bgSprite:setScale(scale)
	bgSprite:setAnchorPoint(PT(0.5,1))
	bgSprite:setPosition(PT(pWinSize.width/2,logoImg:getPosition().y-SY(5)))
	layer:addChild(bgSprite,0)
	
	---输入框底图
	local aa=nil
	local ww=SX(288)
	local hh=0
	local xx=(pWinSize.width-ww)/2
	
	--[返回按钮  
	local startY=bgSprite:getPosition().y-bgSprite:getContentSize().height*scale*0.8
	ChoiceBtn2=ZyButton:new(Image.image_button_red_c_0,Image.image_button_red_c_1,nil, Language.IDS_BACK,FONT_NAME,FONT_SM_SIZE)
	ChoiceBtn2:setColorNormal(ZyColor:colorYellow())
	ChoiceBtn2:setAnchorPoint(PT(0.5,0))
	ChoiceBtn2:setPosition(PT(pWinSize.width/2,startY))
	
	ChoiceBtn2:registerScriptHandler(key_back)
	ChoiceBtn2:addto(layer, 0)
	
	--[快速进入按钮
	ChoiceBtn=ZyButton:new(Image.image_button_red_c_0,Image.image_button_red_c_1,nil, Language.LAN_SUPER_IN,FONT_NAME,FONT_SM_SIZE)
	ChoiceBtn:setColorNormal(ZyColor:colorYellow())
	ChoiceBtn:setAnchorPoint(PT(1,0))
	ChoiceBtn:setPosition(PT(ChoiceBtn2:getPosition().x-ChoiceBtn2:getContentSize().width/2-SX(5),ChoiceBtn2:getPosition().y))
	ChoiceBtn:registerScriptHandler(key_kuaisu)
	if not _DOWNJOY then
		ChoiceBtn:addto(layer, 0)
	else
		ChoiceBtn2:setPosition(PT(ChoiceBtn:getPosition().x,startY))
	end

	--[登陆按钮
	ChoiceBtn1=ZyButton:new(Image.image_button_red_c_0,Image.image_button_red_c_1,nil, Language.LAN_IN,FONT_NAME,FONT_SM_SIZE)
	ChoiceBtn1:setColorNormal(ZyColor:colorYellow())
	ChoiceBtn1:setAnchorPoint(PT(0,0))
	ChoiceBtn1:setPosition(PT(ChoiceBtn2:getPosition().x+ChoiceBtn2:getContentSize().width/2+SX(5),ChoiceBtn2:getPosition().y))
	ChoiceBtn1:registerScriptHandler(key_in)
	ChoiceBtn1:addto(layer, 0)
	--密码文字
	local imgSprite=CCSprite:create(P(Image.image_list_txt_2));
	local txt_h= imgSprite:getContentSize().height--输入框高
	if txt_h< Tools.get_String_1_h(FONT_NAME,FONT_SM_SIZE) then
		txt_h=Tools.get_String_1_h(FONT_NAME,FONT_SM_SIZE)
	end;
	local titleName1=CCLabelTTF:create(Language.LAN_PASSWOED,FONT_NAME,FONT_SM_SIZE);
	titleName1:setAnchorPoint(PT(0,0))
	titleName1:setPosition(PT(xx+SX(22),startY+ChoiceBtn1:getContentSize().height+SX(8)+txt_h/2-titleName1:getContentSize().height/2))
	layer:addChild(titleName1, 0)
	--[文本输入框2底图
	local txt_x=titleName1:getPosition().x+SX(8)+titleName1:getContentSize().width--输入宽坐标
	local txt_ww=xx+ww-txt_x-SX(44)--输入框宽
	local bgEmpty1= CCSprite:create(P(Image.image_list_txt_2))
	bgEmpty1:setScaleX(txt_ww/bgEmpty1:getContentSize().width)
	bgEmpty1:setAnchorPoint(PT(0,0))
	bgEmpty1:setPosition(PT(txt_x,titleName1:getPosition().y+titleName1:getContentSize().height/2-bgEmpty1:getContentSize().height/2))
	layer:addChild(bgEmpty1, 0)
	--[文本编辑
	if m_pswEditTxt~=nil then
		m_pswEditTxt:release()
		m_pswEditTxt=nil
	end
	--文本编辑
	m_pswEditTxt = CScutEdit:new()
	m_pswEditTxt:init(false, true)
	m_pswEditTxt:setRect(CCRect(bgEmpty1:getPosition().x+SX(8),pWinSize.height-(bgEmpty1:getPosition().y+bgEmpty1:getContentSize().height)+SX(4),txt_ww-SX(16),bgEmpty1:getContentSize().height-SX(8)))
	m_pswEditTxt:setVisible(true)--是否显
	
	--[账号文字
	local titleName=CCLabelTTF:create(Language.LAN_Z,FONT_NAME,FONT_SM_SIZE);
	titleName:setAnchorPoint(PT(0,0))
	aa=(hh/2-titleName:getContentSize().height)/2--间距
	titleName:setPosition(PT(titleName1:getPosition().x,titleName1:getPosition().y+txt_h+SX(8)))
	layer:addChild(titleName, 0)
	
	--[文本输入框1底图
	local bgEmpty= CCSprite:create(P(Image.image_list_txt_2));
	bgEmpty:setScaleX(txt_ww/bgEmpty:getContentSize().width)
	bgEmpty:setAnchorPoint(PT(0,0))
	bgEmpty:setPosition(PT(txt_x,titleName:getPosition().y+titleName:getContentSize().height/2-bgEmpty:getContentSize().height/2))
	layer:addChild(bgEmpty, 0)
	if m_pidEditTxt~=nil then
		m_pidEditTxt:release()
		m_pidEditTxt=nil
	end
	m_pidEditTxt = CScutEdit:new()
	m_pidEditTxt:init(false, false)
	m_pidEditTxt:setRect(CCRect(bgEmpty:getPosition().x+SX(8),pWinSize.height-(bgEmpty:getPosition().y+bgEmpty:getContentSize().height)+SX(4),txt_ww-SX(16),bgEmpty:getContentSize().height-SX(8)))
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
	--多账户按钮
	local moreAccount=ZyButton:new("button/list_1166.png")
	moreAccount:setAnchorPoint(PT(0,0))
	moreAccount:setPosition(PT(txt_x+txt_ww+SX(5),titleName:getPosition().y))
	moreAccount:registerScriptHandler(showAccountList)
	moreAccount:addto(layer, 0)
	
	listWidth=txt_ww
	listHeight=bgEmpty:getContentSize().height*3
	list_x=bgEmpty:getPosition().x
	list_y=bgEmpty:getPosition().y-listHeight
	isClick=false
end


function showPlayerNameEidt()
	if m_playerNameEdit~=nil then
		m_playerNameEdit:setVisible(true)
	end
end;


--------多账号登陆
function showAccountList()
	if showAccountListLayer~=nil then
		HASChoiceBtn(true)
		showAccountListLayer:getParent():removeChild(showAccountListLayer,true)
		showAccountListLayer=nil
		m_pswEditTxt:setVisible(true)
		return
	else
		HASChoiceBtn(false)
		m_pswEditTxt:setVisible(false)
	end
	
	local itemHeight=listHeight/3
	showAccountListLayer=CCLayer:create()
	showLayer:addChild(showAccountListLayer,0)
	
	local backGround=CCSprite:create(P("common/list_1002_1.9.png"))
	backGround:setScaleX(listWidth/backGround:getContentSize().width)
	backGround:setScaleY(listHeight/backGround:getContentSize().height)
	backGround:setAnchorPoint(PT(0,0));
	backGround:setPosition(PT(list_x,list_y));
	showAccountListLayer:addChild(backGround,0)
    
      m_accountList=ScutCxList:node(itemHeight,ccc4(124, 124, 124, 255),SZ(listWidth,listHeight))
	m_accountList:setAnchorPoint(PT(0,0))
	m_accountList:setPosition(PT(list_x,list_y))
	m_accountList:registerItemClickListener(LoginScene.onChoiceAccount) 
	showAccountListLayer:addChild(m_accountList,0)
	m_accountList:setTouchEnabled(true)
	getCurrentAccount()
	refleshAccountList()
end

function refleshAccountList()
       m_accountList:clear()
	local layout=CxLayout()
	layout.val_x.t = ABS_WITH_PIXEL
	layout.val_y.t = ABS_WITH_PIXEL
	local itemHeight=m_accountList:getRowHeight ()
	for index, message in ipairs(acountInfoTable) do
		if message~=nil and message.PassportID~=nil  then
			local listItem=ScutCxListItem:itemWithColor(ccc3(0,0,0))
			listItem:setDrawBottomLine(true)
			listItem:setOpacity(140)
			listItem:setMargin(CCSize(0,0));

			--名称数据
			local label = CCLabelTTF:create(Language.LAN_Z .. message.PassportID, FONT_NAME, FONT_SM_SIZE)
			layout.val_x.val.pixel_val =SX(8)
			local nOffsetY =(itemHeight-label:getContentSize().height)/2
			layout.val_y.val.pixel_val = nOffsetY
			listItem:addChildItem(label,layout)
			
			--删除按钮
			local deletBtn=ZyButton:new("button/closebottom.png","button/closebottom_1.png")
			local width=listWidth-deletBtn:getContentSize().width*1.2
			layout.val_x.val.pixel_val =width
			layout.val_y.val.pixel_val =(itemHeight-deletBtn:getContentSize().height)/2
			deletBtn._menuItem:setTag(index)
			deletBtn:registerScriptHandler(deleteAccount)
			listItem:addChildItem(deletBtn._menu,layout)
			m_accountList:addListItem(listItem,false)
		end
	end
end;

function HASChoiceBtn(visiable)
if not _DOWNJOY then
ChoiceBtn:setVisible(visiable)
end
ChoiceBtn1:setVisible(visiable)
ChoiceBtn2:setVisible(visiable)
end

--
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

--选中账号
function onChoiceAccount(index,item)
	if not isClick then
            local messageIndex=index+1
            local message = acountInfoTable[messageIndex]
            if message~=nil then
            		if message.PassportID~=nil then
            		    m_currentPsw=message.pwd
			            m_pidEditTxt:setText(message.PassportID)
				        if m_currentPsw~=nil and m_currentPsw~="error" then
					        m_pswEditTxt:setText("******")
                        end
                   end
            end
            showAccountList()
            m_showAccount=false
	end
end;

--删除账号
function  deleteAccount(item)
	isClick=true
	local tag=item:getTag()

	if string.len(m_pidEditTxt:GetEditText())~=0 then
	    local pid=m_pidEditTxt:GetEditText()
	    if acountInfoTable[tag].PassportID==pid then
	        m_pidEditTxt:setText("")
	        m_pswEditTxt:setText("")
	    end
	end

	local tabel={}
	for k, v in ipairs(acountInfoTable) do
		if k~=tag then
			tabel[#tabel+1]=v
		end
	end
	acountInfoTable=nil
	acountInfoTable=tabel
	refleshAccountList()
	saveCurrentAccount(acountInfoTable)
	isClick=false
	if m_accountList:getChildCount()<=0 then
        showAccountList()
        m_showAccount=false
	end
end;

function  isNoAccout(num)
 	if num==nil then
 		num="error"
 	end
 	return num
end;

function  getRemenberAccount(accountId)
	local PassportID= accountInfo.getConfig("sys/account.ini",accountId,"PassportID")
	local pwd= accountInfo.getConfig("sys/account.ini",accountId,"pw")
	if PassportID~=nil and PassportID~=0 and PassportID~="1" and PassportID~=1 and pwd~=nil and PassportID~="error" and pwd~="error" then
		return PassportID,pwd
	end
		 return false
end;
------------------------------------------------------------------------------------------------
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
        table[1].pwd=ScutDataLogic.CFileHelper:encryptPwd(psw, nil):getCString()
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
-----------------------------------------------------------
--返回按钮响应
function key_back()
	LanScenes.init(2)
end;

--注册按钮响应
function key_kuaisu()
	sendAction(1002)
end

--登入按钮响应
function key_in()
	if isClick then
		return
	end
	
	m_isNormalLogin=true
	pid=m_pidEditTxt:GetEditText()
	password=m_pswEditTxt:GetEditText()

	if pid==""   then
		--跳出的提示界面
		local box = ZyMessageBoxEx:new()
		box:doPrompt(mScene, nil, Language.LAN_ID_NOT_Z,Language.IDS_SURE,messageCallback)
		m_pswEditTxt:setVisible(false);
		m_pidEditTxt:setVisible(false);
	elseif password=="" then
		--跳出的提示界面
		local box = ZyMessageBoxEx:new()
		box:doPrompt(mScene, nil, Language.LAN_ID_NOT_PASSWOED,Language.IDS_SURE,messageCallback)
		m_pswEditTxt:setVisible(false);
		m_pidEditTxt:setVisible(false)
	else
	
	
		if password~="******" then
			password=ScutDataLogic.CFileHelper:encryptPwd( password, nil):getCString()	
			m_currentPsw=password
		elseif  m_currentPsw~=nil then
			password=m_currentPsw
		end
		
		
		state=2

		local mMobileType ,mGameType,mRetailID=accountInfo.readMoble()
		local mMac=accountInfo.mMac
		local mServerID,mServerPath,mServerName,mServerState=accountInfo.readServerId()
		local ScreenX=pWinSize.width
		local ScreenY=pWinSize.height
		setInputText(false)
		if _DOWNJOY then 
			if downJoyPid then
				actionLayer.Action1004(mScene,nil,mMobileType,pid,password,mMac,mGameType,ScreenX,ScreenY,mRetailID,nil,mServerID);
			else            --普通登录
				actionLayer.Action1004(mScene,nil,mMobileType,"0",password,mMac,mGameType,ScreenX,ScreenY,mRetailID,nil,mServerID,pid);
			end
		else
			actionLayer.Action1004(mScene,nil,mMobileType,pid,password,mMac,mGameType,
			ScreenX ,ScreenY,mRetailID,accountInfo.ClientAppVersion,mServerID)
		end
	
	end
	
end;

function  setInputText(isEnabel)
	if m_pidEditTxt~=nil then
		m_pidEditTxt:setVisible(isEnabel)
	end
	if m_pswEditTxt~=nil then
		m_pswEditTxt:setVisible(isEnabel)
	end
end;


function sendAction(actionId)
	if actionId == 1002 then
		accountInfo.readMoble()
		actionLayer.Action1002(mScene,false,
		accountInfo.mMobileType,
		accountInfo.mGameType,
		accountInfo.mRetailID,
		accountInfo.ClientAppVersion,
		pWinSize.width,
		pWinSize.height,
		accountInfo.getMac())
	elseif actionId == 1004 then
		local mMobileType ,mGameType,mRetailID=accountInfo.readMoble()
		local mPassportID, mPassWord=accountInfo.readAccount()
			setInputText(false)
			actionLayer.Action1004(mScene,nil,
			mMobileType,
			mPassportID,
			mPassWord,
			accountInfo.mMac,mGameType,
			pWinSize.width,
			pWinSize.height,mRetailID,
			accountInfo.ClientAppVersion,
			accountInfo.mServerID)
	end
end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if actionId == 1002 then
		local LoginResponse=actionLayer._1002Callback(pScutScene, lpExternalData)
		if LoginResponse~=nil then
      		if LoginResponse.PassportID~=nil and LoginResponse.Password~=nil then
      				isQuiklyPid=true
      				local Pid=LoginResponse.PassportID 
				local Pwd=LoginResponse.Password	
				m_currentPsw=nil
				accountInfo.mPassWord=LoginResponse.Password
				accountInfo.mPassportID=LoginResponse.PassportID 	
				if  checkAccount(Pid,Pwd) then
					orderAccount(Pid,Pwd,1)
				end              
		              --登陆
   				sendAction(1004)
 			end
 		else
		--	isClick=false
			setInputText(false)
			local box = ZyMessageBoxEx:new()
			box:doPrompt(pScutScene, nil, ZyReader:readErrorMsg(),Language.IDS_SURE,messageCallback)
  		end		
	elseif actionId == 1004 then
  		ScutScene:registerNetErrorFunc("MainScene.netConnectError");
		local StatusCode=ZyReader:getResult()
		local LoginResponse=actionLayer._1004Callback(pScutScene, lpExternalData)
		if LoginResponse~=nil then
			if not Login91 and not _UCGAME  then
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
			end
			
			accountInfo.saveServerId();
			ScutDataLogic.CNetWriter:setSessionID(LoginResponse.SessionID);
			ScutDataLogic.CNetWriter:setUserID( Int64(LoginResponse.UserID));
			local personalInfo=PersonalInfo.getPersonalInfo()
			personalInfo._userID=LoginResponse.UserID
			personalInfo._UserType=LoginResponse.UserType
			if loginType==8 then
		                   	  accountInfo.setPassportID(LoginResponse.PassportId);
			               accountInfo.SaveAccountInfo();
			end
			if  LoginResponse.PassportId~=nil then
			    personalInfo._Pid=LoginResponse.PassportId
				if  Login91 or _UCGAME    then
		                   	  accountInfo.setPassportID(LoginResponse.PassportId);
			               accountInfo.SaveAccountInfo();
			       end
		       end
		       
		       if m_pidEditTxt~=nil then
	      			m_pidEditTxt:release()
	         		m_pswEditTxt:release()
			end
			if LoginResponse.StatusCode==1005 then
				CreatNewMan.pushScene()
			else
				progressLayer.replaceScene()
			end
		else
			local box = ZyMessageBoxEx:new()
			box:doPrompt(pScutScene, nil, ZyReader:readErrorMsg(),Language.IDS_SURE,messageCallback)
    		end
		m_isNormalLogin=false
		isClick=false
		-------------------------创建角色接口	
	
	end	
end

function gotoServer()
	LanScenes.init(2)
end;

function messageCallback()
	--clearEdit()
	setInputText(true)
end;

function clearEdit()
	m_pidEditTxt:setText("")
	m_pswEditTxt:setText("")
end

