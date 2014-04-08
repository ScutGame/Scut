------------------------------------------------------------------
-- ChangePwd.lua
-- Author     : chenjp

-- Version    : 1.0
-- Date       :密码修改
-- Description: 
------------------------------------------------------------------

module("ChangePwd", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

mScene = nil 		-- 场景
mLayer=nil
local layer_1=nil
local passwordEdit=nil;
local passwordEdit_2=nil;
local m_personInfo=nil 	
--
---------------公有接口(以下两个函数固定不用改变)--------
--


-- 函数名以小写字母开始，每个函数都应注释

-- 场景入口
function pushScene()
	initResource()
	createScene()
	CCDirector:sharedDirector():pushScene(mScene)
end
-- 退出场景
function popScene()
	releaseResource()
	CCDirector:sharedDirector():popScene()
end

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
	m_personInfo=PersonalInfo.getPersonalInfo()
end
-- 释放资源
function releaseResource()
end
-- 创建场景
function createScene()
	 initResource()
	local scene = ScutScene:new()
	-- 注册网络回调
	scene:registerCallback(networkCallback)
	mScene = scene.root
	-- 添加背景
	mLayer = CCLayer:create()
	mScene:addChild(mLayer, 0)
	
	--透明背景
	local transparentBg=Image.tou_ming;--背景图片
	local menuItem =CCMenuItemImage:create(P(transparentBg), P(transparentBg))
	local menuBg=CCMenu:createWithItem(menuItem);
	menuBg:setContentSize(menuItem:getContentSize());
	menuBg:setAnchorPoint(CCPoint(0,0));
	menuBg:setScaleX(pWinSize.width/menuBg:getContentSize().width*2)
	menuBg:setScaleY(pWinSize.height/menuBg:getContentSize().height*2);
	menuBg:setPosition(CCPoint(0,0));
	mLayer:addChild(menuBg,0);
	


	-- 此处添加场景初始内容
	layer_1= CCLayer:create();
	mLayer:addChild(layer_1, 0)
	showLayer()
	SlideInLPushScene(mScene)
end
function showLayer()
	-- 添加背景
	layer_1= CCLayer:create();
	mLayer:addChild(layer_1, 0)
	
	local bgSprite=CCSprite:create(P("imageupdate/default.jpg"))
	bgSprite:setScaleX(pWinSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(pWinSize.height/bgSprite:getContentSize().height);
	bgSprite:setAnchorPoint(PT(0.5,0.5))
	bgSprite:setPosition(PT(pWinSize.width/2,pWinSize.height/2))
	layer_1:addChild(bgSprite,0)
	
	local touming_bg=CCSprite:create(P("common/ground_2018.9.png"))
	touming_bg:setScaleX(pWinSize.width/touming_bg:getContentSize().width)
	touming_bg:setScaleY(pWinSize.height/touming_bg:getContentSize().height)
	touming_bg:setAnchorPoint(PT(0.5,0.5))
	touming_bg:setPosition(PT(pWinSize.width/2,pWinSize.height/2))
	layer_1:addChild(touming_bg,0)
	
	local touming_bg1=CCSprite:create(P("common/panle_1012_5.png"))
	touming_bg1:setScaleX(pWinSize.width*0.97/touming_bg1:getContentSize().width)
	touming_bg1:setScaleY(pWinSize.height*0.97/touming_bg1:getContentSize().height)
	touming_bg1:setAnchorPoint(PT(0.5,0.5))
	touming_bg1:setPosition(PT(pWinSize.width/2,pWinSize.height/2))
	layer_1:addChild(touming_bg1,0)
	
	local btnStr=nil
	local pwdStr=nil
	local secPwdStr=nil
	
	--返回按钮
--	local exitText=CCSprite:create(P("title/panle_1039.png"))
--	exitText:setAnchorPoint(PT(0,0))
--	exitText:setPosition(PT(pWinSize.width-exitText:getContentSize().width-SY(20),SY(10)))
--	mLayer:addChild(exitText)
	local exitText=ZyButton:new("title/panle_1039.png",nil,nil, nil,FONT_NAME,FONT_SM_SIZE)
	exitText:setAnchorPoint(PT(0,0))
	exitText:setPosition(PT(pWinSize.width-exitText:getContentSize().width-SY(20),SY(17)))
	exitText:registerScriptTapHandler(ChangePwd.exit)
	exitText:addto(mLayer)
	
	local exitBtn=ZyButton:new(Image.image_exit,nil,nil, nil,FONT_NAME,FONT_SM_SIZE)
	exitBtn:setAnchorPoint(PT(0,0))
	exitBtn:setPosition(PT(exitText:getPosition().x-exitBtn:getContentSize().width,SY(10)))
	exitBtn:registerScriptTapHandler(ChangePwd.exit)
	exitBtn:addto(mLayer)

	if accountInfo.UserType==1 then
		titleStr="title/panle_1012_1.png"
		btnStr="title/panle_1012_3.png"
		pwdStr="title/panle_1012_2.png"
		secPwdStr="title/panle_1013_4.png"
		
		exitBtn:setIsVisible(false)
		exitText:setIsVisible(false)
	elseif accountInfo.UserType==0 then 
		titleStr="title/panle_1013_1.png"
		btnStr="title/panle_1013_5.png"
		pwdStr="title/panle_1013_3.png"
		secPwdStr="title/panle_1013_4.png"
		exitBtn:setIsVisible(true)
		exitText:setIsVisible(true)
	end;
	
	--保存按钮
	local saveBtn=ZyButton:new("button/button_1013.png","button/button_1014.png",nil, nil,FONT_NAME,FONT_SM_SIZE)
	saveBtn:setAnchorPoint(PT(0.5,0))
	saveBtn:addImage(btnStr)
	saveBtn:registerScriptTapHandler(ChangePwd.saveNewPwd)
	saveBtn:setPosition(PT(pWinSize.width/2,SY(15)))
	saveBtn:addto(layer_1)
	
	local bgEmptyt=nil
	if not bgEmptyt then
		bgEmptyt={}
		for  i =1,2  do
		
			pwdText=nil
			if i==1 and secPwdStr~=nil then
				pwdText=CCSprite:create(P(secPwdStr));
			else
				pwdText=CCSprite:create(P(pwdStr));
			end;
		
			pwdText:setAnchorPoint(PT(0,0))
			layer_1:addChild(pwdText)
		
			bgEmptyt[i]= CCSprite:create(P(Image.image_list_txt))
			bgEmptyt[i]:setScaleX(pWinSize.width*0.25/bgEmptyt[1]:getContentSize().width)
			bgEmptyt[i]:setAnchorPoint(PT(0,0))
			layer_1:addChild(bgEmptyt[i], 0)
			
			local contentStr=string.format("<label fontsize='10'>%s</label>",Language.LAN_INPUT_TXT)
			local warmText=CCSprite:create(P("title/panle_1012_4.png"));
			warmText:setAnchorPoint(PT(0,0))
			layer_1:addChild(warmText)
			
			local warmY=nil
			if i==1 then
				warmY=saveBtn:getPosition().y+saveBtn:getContentSize().height+SY(20)
			elseif i==2 then
				warmY=bgEmptyt[1]:getPosition().y+pwdText:getContentSize().height+SY(20)
			end;
			pwdText:setPosition(PT(pWinSize.width/2-pwdText:getContentSize().width,
					warmY+warmText:getContentSize().height+SY(10)))
			bgEmptyt[i]:setPosition(PT( pWinSize.width/2,pwdText:getPosition().y+(pwdText:getContentSize().height-bgEmptyt[i]:getContentSize().height)/2))	
			warmText:setPosition(PT(bgEmptyt[i]:getPosition().x+(pWinSize.width*0.25-warmText:getContentSize().width)/2,warmY))
		end;
	end;
	
	--密码编辑框
	passwordEdit_2 = CScutEdit:new()
	passwordEdit_2:init(false, true, ccc4(235,197, 151, 255),ccc4(0,39,61,200))
	passwordEdit_2:setRect(CCRect(bgEmptyt[1]:getPosition().x+SY(8),
		pWinSize.height-(bgEmptyt[1]:getPosition().y+pwdText:getContentSize().height)-SY(5) ,
		pWinSize.width*0.25-SY(15), pwdText:getContentSize().height))
	
	passwordEdit = CScutEdit:new()
	passwordEdit:init(false, true, ccc4(235,197, 151, 255),ccc4(0,39,61,200))
	passwordEdit:setRect(CCRect(bgEmptyt[2]:getPosition().x+SY(8),
		pWinSize.height-(bgEmptyt[2]:getPosition().y+pwdText:getContentSize().height)-SY(5) ,
		pWinSize.width*0.25-SY(15), pwdText:getContentSize().height))
	
	--账号
	local pidText=CCSprite:create(P("title/panle_1013_2.png"));
	pidText:setAnchorPoint(PT(0,0))
	pidText:setPosition(PT(pWinSize.width/2-pidText:getContentSize().width,
		bgEmptyt[2]:getPosition().y+bgEmptyt[2]:getContentSize().height+SY(20)))
	layer_1:addChild(pidText)
	
	local nameBg= CCSprite:create(P(Image.image_list_txt))
	nameBg:setScaleX(pWinSize.width*0.25/nameBg:getContentSize().width)
	nameBg:setAnchorPoint(PT(0,0))
	nameBg:setPosition(PT( pWinSize.width/2,pidText:getPosition().y+(pidText:getContentSize().height-nameBg:getContentSize().height)/2))
	layer_1:addChild(nameBg, 0)
	
	local userPid=CCLabelTTF:create(m_personInfo._Pid,FONT_NAME,FONT_SM_SIZE);
	userPid:setAnchorPoint(PT(0,0.5))
	userPid:setColor(ccRED1)
	userPid:setPosition(PT(nameBg:getPosition().x+SY(5),pidText:getPosition().y+pidText:getContentSize().height/2))
	layer_1:addChild(userPid)
	
	local titleSprite=CCSprite:create(P(titleStr));
	titleSprite:setAnchorPoint(PT(0.5,0.5))
	titleSprite:setPosition(PT(pWinSize.width/2,nameBg:getPosition().y+pidText:getContentSize().height+SY(35)))
	layer_1:addChild(titleSprite,0)
	
end;

function exit()
	CCDirector:sharedDirector():popScene()
    SlideInRPopScene()
	if mLayer~=nil then
		mLayer:getParent():removeChild(mLayer,true)
		mLayer=nil
	end
	PersonalInfo.getPersonalInfo()._UserType=1
	PersonalFileScene.changeBtnImg()
	setSureEdit(false)
end;

function saveNewPwd()
	password=passwordEdit:GetEditText()
	local password_2=passwordEdit_2:GetEditText()
	if password=="" then
		setSureEdit(false)
		local box = ZyMessageBoxEx:new()
		box:doPrompt( mScene, nil, Language.Password_NOT_BU_PASSWORD,Language.IDS_SURE,AfterPwdBox)
	elseif password_2=="" then
		setSureEdit(false)
		local box = ZyMessageBoxEx:new()
		box:doPrompt( mScene, nil, Language.Password_PLEASE_PASSWORD,Language.IDS_SURE,AfterPwdBox)
	elseif password_2~=password then
		setSureEdit(false)
		local box = ZyMessageBoxEx:new()
		box:doPrompt( mScene, nil, Language.Password_BUG_PASSWORD,Language.IDS_SURE,AfterPwdBox)
	else
		close_txt()
		password=ScutDataLogic.CFileHelper:encryptPwd( password, nil):getCString()
		actionLayer.Action1006(mScene,nil,password)
	end
end;
function AfterPwdBox()
	setSureEdit(true)
end;
function  setSureEdit(isVisible)
	if passwordEdit~=nil then
		passwordEdit:setVisible(isVisible)
		passwordEdit_2:setVisible(isVisible)
	end
end;
function close_txt()
	if passwordEdit~=nil then
		passwordEdit:setVisible(false)
		passwordEdit=nil
	end
	if passwordEdit_2~=nil then
		passwordEdit_2:setVisible(false)
		passwordEdit_2=nil
	end
end;
--点击确定密码修改成功按钮
function key_password_ok()
	releasedraw_3()
	showLayer()
end;
-- 清楚账号管理界面
function releasedraw_3()
	if passwordEdit~=nil then
		passwordEdit:release()
		passwordEdit_2:release()
	end
	if layer_1~=nil then
		layer_1:getParent():removeChild(layer_1,true)
		layer_1=nil
	end
end;
function askEidt()
	setClearEdit()
	setSureEdit(true)
end;
function setClearEdit()
	passwordEdit:setText("")
	passwordEdit_2:setText("")
end;

-- 触屏按下
function touchBegan(e)
end
-- 触屏移动
function touchMove(e)
end
-- 触屏弹起
function touchEnd(e)
end
-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID();
	 local userData = ZyRequestParam:getParamData(lpExternalData)
	if actionId == 1006 then
  		if ZyReader:getResult() == eScutNetSuccess then
			accountInfo.saveConfig("sys/account.ini", "Account", "PassportID" , isNoAccout(accountInfo.read_PassportID()))
			accountInfo.saveConfig("sys/account.ini", "Account", "pw" , isNoAccout(password))
			accountInfo.UserType=0
			PersonalInfo.getPersonalInfo()._UserType=0
			accountInfo.UserType=0
			if m_personInfo._UserType==1 then
				local box = ZyMessageBoxEx:new()
				box:doPrompt( pScutScene, nil,  Language.Password_BUQUAN_OK,Language.IDS_SURE,key_password_ok)
			else
				local box = ZyMessageBoxEx:new()
				box:doPrompt( pScutScene, nil,  Language.Password_PASSWORD_OK,Language.IDS_SURE,key_password_ok)
			end
		else
			local box = ZyMessageBoxEx:new()
			box:doPrompt( pScutScene, nil,ZyReader:readErrorMsg(),Language.IDS_SURE,key_password_ok)
		end
	
	end
end
function  isNoAccout(num)
 	if num==nil then
 		num="error"
 	end
 	return num
end;