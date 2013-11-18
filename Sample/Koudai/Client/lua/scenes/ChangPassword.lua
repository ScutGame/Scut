------------------------------------------------------------------
-- ChangPassword.lua
-- Author     : Zonglin Liu
-- Version    : 1.25
-- Date       :   
-- Description: 密码不全，修改密码
------------------------------------------------------------------

module("ChangPassword", package.seeall)


mScene = nil 		-- 场景



-- 场景入口
function pushScene()
	initResource()
	createScene()
	
end

-- 退出场景
function popScene()
	releaseResource()
	MainScene.init()
end

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()

end

-- 释放资源
function releaseResource()
	releasedraw_3()
end

-- 创建场景
function createScene()
	local scene = ScutScene:new()
    mScene = scene.root 
	runningScene = CCDirector:sharedDirector():getRunningScene()
	if runningScene == nil then
		CCDirector:sharedDirector():runWithScene(mScene)
	else
		 SlideInLReplaceScene(mScene)
	end
	MainScene.releaseResource()

	-- 注册网络回调
	scene:registerCallback(networkCallback)
	
	-- 添加主层
	mLayer= CCLayer:create()
	mScene:addChild(mLayer, 0)


--	--创建背景
	local bgLayer = UIHelper.createUIBg(nil, Language.LAN_ID_MANAGE,ZyColor:colorYellow(),ChangPassword.popScene)
	mLayer:addChild(bgLayer,0)

	-- 此处添加场景初始内容
	draw_3()
	
end

--账号管理界面       修改，不全密码
function draw_3()
	if layer_1~=nil then
		layer_1:getParent():removeChild(layer_1,true)
		layer_1=nil
	end
	layer_1= CCLayer:create();
	layer_1:setAnchorPoint(PT(0,0))
	layer_1:setPosition(PT(0,0))
	mLayer:addChild(layer_1, 0)

	--账号框
	local ww=SX(99)--输入框宽
	local bgEmpty= CCSprite:create(P(Image.image_list_txt))
	bgEmpty:setScaleX(ww/bgEmpty:getContentSize().width)
	bgEmpty:setAnchorPoint(PT(0,0))
	bgEmpty:setPosition(PT(( pWinSize.width-ww)/2,pWinSize.height/22*17))
	layer_1:addChild(bgEmpty, 0)
	--账号文字
	local titleName1=CCLabelTTF:create(Language.LAN_Z,FONT_NAME,FONT_SM_SIZE);
	titleName1:setAnchorPoint(PT(0,0))
	titleName1:setPosition(PT(bgEmpty:getPosition().x-titleName1:getContentSize().width-4,bgEmpty:getPosition().y+bgEmpty:getContentSize().height/2-titleName1:getContentSize().height/2))
	layer_1:addChild(titleName1, 0)
	--账号
	local titleName3=CCLabelTTF:create(accountInfo.read_PassportID(),FONT_NAME,FONT_SM_SIZE);
	titleName3:setAnchorPoint(PT(0,0))
	titleName3:setPosition(PT(bgEmpty:getPosition().x+2,bgEmpty:getPosition().y+bgEmpty:getContentSize().height/2-titleName3:getContentSize().height/2))
	layer_1:addChild(titleName3, 0)
	--补全密码框
	local password_w=SX(111)--账号密码框宽
	local imgSprite=CCSprite:create(P(Image.image_list_txt));
	local password_h= imgSprite:getContentSize().height--账号密码框高
	xilianBtn_=nil
	if xilianBtn_==nil then
		xilianBtn_={}
		for i=1,2 do
			local bgEmptyt= CCSprite:create(P(Image.image_list_txt))
			bgEmptyt:setScaleX(password_w/bgEmptyt:getContentSize().width)
			bgEmptyt:setAnchorPoint(PT(0,0))
			layer_1:addChild(bgEmptyt, 0)
			--
			xilianBtn_[i]=ZyButton:new(Image.image_transparent,Image.image_transparent)
			xilianBtn_[i]:setScaleX(password_w/xilianBtn_[i]:getContentSize().width)
			xilianBtn_[i]:setScaleY(bgEmptyt:getContentSize().height/xilianBtn_[i]:getContentSize().height)
			xilianBtn_[i]:setAnchorPoint(PT(0,0))
			if i==1 then
				xilianBtn_[i]:setPosition(PT(bgEmpty:getPosition().x,bgEmpty:getPosition().y-password_h-11))
				bgEmptyt:setPosition(PT(xilianBtn_[i]:getPosition().x,xilianBtn_[i]:getPosition().y))
			--	xilianBtn_[i]:registerScriptHandler(LanScenes.key_up)
			else
				xilianBtn_[i]:setPosition(PT(bgEmpty:getPosition().x,xilianBtn_[1]:getPosition().y-password_h-11))
				bgEmptyt:setPosition(PT(xilianBtn_[i]:getPosition().x,xilianBtn_[i]:getPosition().y))
			--	xilianBtn_[i]:registerScriptHandler(LanScenes.key_down)
			end;
			xilianBtn_[i]:addto(layer_1, 0)
			--4-12位数字或字母组成
			local titleName2=nil
			titleName2=CCLabelTTF:create(Language.LAN_INPUT_TXT,FONT_NAME,FONT_SM_SIZE);
			titleName2:setAnchorPoint(PT(0,0))
			titleName2:setPosition(PT(xilianBtn_[i]:getPosition().x+password_w+4,xilianBtn_[i]:getPosition().y+password_h/2-titleName2:getContentSize().height/2))
			layer_1:addChild(titleName2, 0)
		end;
	end;
	--密码
	m_pswEditTxt=CScutEdit:new()
	m_pswEditTxt:init(false, true)
	m_pswEditTxt:setRect(CCRect(xilianBtn_[1]:getPosition().x,pWinSize.height-xilianBtn_[1]:getPosition().y-password_h,password_w,password_h))
	--密码确认
	m_pswSureTxt = CScutEdit:new()
	m_pswSureTxt:init(false, true)
	m_pswSureTxt:setRect(CCRect(xilianBtn_[2]:getPosition().x,pWinSize.height-xilianBtn_[2]:getPosition().y-password_h,password_w,password_h))
	--
	titleName2=nil
	local type=PersonalInfo.getPersonalInfo()._UserType
		bu_or_xiu_boolean=false
	if type~=0 then
		bu_or_xiu_boolean=true
	end
	if bu_or_xiu_boolean then
		--补全密码文字
		titleName2=CCLabelTTF:create(Language.LAN_ADD_PASSWOED,FONT_NAME,FONT_SM_SIZE);
	else
		--修改密码文字
		titleName2=CCLabelTTF:create(Language.LAN_MODI_PASSWOED,FONT_NAME,FONT_SM_SIZE);
	end;
	titleName2:setAnchorPoint(PT(0,0))
	titleName2:setPosition(PT(xilianBtn_[1]:getPosition().x-titleName2:getContentSize().width-4,xilianBtn_[1]:getPosition().y+password_h/2-titleName2:getContentSize().height/2))
	layer_1:addChild(titleName2, 0)
	--再次输入文字
	local titleName3=CCLabelTTF:create(Language.LAN_AGAIN_INPUT,FONT_NAME,FONT_SM_SIZE);
	titleName3:setAnchorPoint(PT(0,0))
	titleName3:setPosition(PT(xilianBtn_[2]:getPosition().x-titleName3:getContentSize().width-4,xilianBtn_[2]:getPosition().y+password_h/2-titleName3:getContentSize().height/2))
	layer_1:addChild(titleName3, 0)
	--[
	local imgSprite=CCSprite:create(P(Image.image_button_red_c_0));
	local ww=imgSprite:getContentSize().width
	local hh=imgSprite:getContentSize().height
	local aa=(pWinSize.width- ww*2)/3
	local yy=xilianBtn_[2]:getPosition().y/2-hh/2
	local xilianBtn=nil
	if bu_or_xiu_boolean then
		--确定补全按钮
		xilianBtn=ZyButton:new(Image.image_button_red_c_0,Image.image_button_red_c_1,nil,Language.LAN_OK_ADD,FONT_NAME,FONT_SM_SIZE)
		xilianBtn:setColorNormal(ZyColor:colorYellow())
		xilianBtn:setAnchorPoint(PT(0,0))
		xilianBtn:setPosition(PT((pWinSize.width-xilianBtn:getContentSize().width)/2,yy))
		xilianBtn:registerScriptHandler(key_bu_ok)--设置触响应时调用的函数
		xilianBtn:addto(layer_1, 0)
	else
		--确定修改按钮
		xilianBtn=ZyButton:new(Image.image_button_red_c_0,Image.image_button_red_c_1,nil,Language.LAN_OK_MODI,FONT_NAME,FONT_SM_SIZE)
		xilianBtn:setColorNormal(ZyColor:colorYellow())
		xilianBtn:setAnchorPoint(PT(0,0))
		xilianBtn:setPosition(PT(aa,yy))
		xilianBtn:registerScriptHandler(key_bu_ok)--设置触响应时调用的函数
		xilianBtn:addto(layer_1, 0)
		--选择服务器按钮
		xilianBtn1=ZyButton:new(Image.image_button_red_c_0,Image.image_button_red_c_1,nil,Language.LAN_SELECT_SEVER,FONT_NAME,FONT_SM_SIZE)
		xilianBtn1:setColorNormal(ZyColor:colorYellow())
		xilianBtn1:setAnchorPoint(PT(0,0))
		xilianBtn1:setPosition(PT(xilianBtn:getPosition().x+xilianBtn:getContentSize().width+aa,yy))
		xilianBtn1:registerScriptHandler(key_select_serve_ok)--设置触响应时调用的函数
		xilianBtn1:addto(layer_1, 0)
	end;
end

-- 清楚账号管理界面
function releasedraw_3()
	if m_pswEditTxt~=nil then
		m_pswEditTxt:release()
		m_pswSureTxt:release()
	end
	if layer_1~=nil then
		layer_1:getParent():removeChild(layer_1,true)
		layer_1=nil
	end
end;

-- 确定补全按钮
function key_bu_ok()
	--跳出的提示界面
	local t1=m_pswEditTxt:GetEditText()
	password=m_pswSureTxt:GetEditText()
	if t1=="" then
		setSureEdit(false)
		local box = ZyMessageBoxEx:new()
		box:doPrompt( mScene, nil, Language.LAN_NOT_BU_PASSWORD,Language.IDS_SURE,AfterPwdBox)
	elseif password=="" then
		setSureEdit(false)
		local box = ZyMessageBoxEx:new()
		box:doPrompt( mScene, nil, Language.LAN_PLEASE_PASSWORD,Language.IDS_SURE,AfterPwdBox)
	elseif t1~=password then
		setSureEdit(false)
		local box = ZyMessageBoxEx:new()
		box:doPrompt( mScene, nil, Language.LAN_BUG_PASSWORD,Language.IDS_SURE,AfterPwdBox)
	else
		setSureEdit(false)
		password=ScutDataLogic.CFileHelper:encryptPwd( password, nil):getCString()
		actionLayer.Action1006(mScene,nil,password)
	end
end

function AfterPwdBox()
	setSureEdit(true)
end;

function  setSureEdit(isVisible)
	if m_pswEditTxt~=nil then
		m_pswEditTxt:setVisible(isVisible)
		m_pswSureTxt:setVisible(isVisible)
	end
end;

function setClearEdit()
	m_pswEditTxt:setText("")
	m_pswSureTxt:setText("")
end;

--点击确定密码修改成功按钮
function key_password_ok()
	releasedraw_3()
	draw_3(mLayer)
end;

function close_txt()
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
end;

--选择服务器按钮  选择服务器 切换账号 
function key_select_serve_ok()
----


	local runningScene = CCDirector:sharedDirector():getRunningScene()
	actionLayer.Action1017(runningScene,false)



	
end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if actionId == 1006 then
  		if ZyReader:getResult() == eScutNetSuccess then
			accountInfo.saveConfig("sys/account.ini", "Account", "PassportID" , isNoAccout(accountInfo.read_PassportID()))
			accountInfo.saveConfig("sys/account.ini", "Account", "pw" , isNoAccout(password))
			accountInfo.UserType=0
			PersonalInfo.getPersonalInfo()._UserType=0
			close_txt()
			
			ZyToast.show(pScutScene, Language.LAN_PASSWORD_OK, 1, 0.35)--密码修改成功
			key_password_ok()
		else
			ZyToast.show(pScutScene, ZyReader:readErrorMsg(), 1, 0.35)
			askEidt()
		end
	elseif actionId == 1017 then
			releasedraw_3()
			LanScenes.init(2)
	elseif actionId == 1001 then
	
	end
end

function  isNoAccout(num)
 	if num==nil then
 		num="error"
 	end
 	return num
end;

function askEidt()
	setClearEdit()
	setSureEdit(true)
end;
