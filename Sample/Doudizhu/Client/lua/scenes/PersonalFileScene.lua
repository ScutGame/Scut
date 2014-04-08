------------------------------------------------------------------
-- PersonalInfoScene
-- Author     : chenjp

-- Version    : 1.0
-- Date       :
-- Description: 个人档案
------------------------------------------------------------------

module("PersonalFileScene", package.seeall)

require("scenes.ChangePwd")

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local mScene = nil 		-- 场景
local mLayer=nil		--层
local layer_1=nil
local tabBar=nil

local mCurrentTab=nil;--tab

local m_personInfo=nil 	
local m_socialInfo=nil
local newhead=nil
local shoptype=nil
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
--修改头像
function changeHead()
	if mList~=nil and achievementBtn._isSelected  then
		mList:clear()
		mList=nil;
	end;
	personalFileBtn:unselected()
	achievementBtn:unselected()
	changeHeadBtn:selected()
	
	m_goldText=nil
	if  layer_1 and layer_1:getParent()~=nil then
		layer_1:getParent():removeChild(layer_1, true)
		layer_1 = nil
	end;

	layer_1= CCLayer:create()
	mScene:addChild(layer_1, 0)
	
	actionLayer.Action1013(mScene,nil ,1,100)
	
end;
--成就详细
function achievementDetail(index)
	    --透明背景
	local tag=index
	--[[
	if index~=nil then
		tag=index:getTag()
	end;
	]]
	local achievementDetailInfo=achievementTable[tag]
	
	actionLayer.Action3002(mScene,nil,achievementDetailInfo.AchieveID)
	
end;
--
--成就
function achievement()
	if bagList~=nil and changeHeadBtn._isSelected then
		bagList:clear()
		bagList=nil;
	end;
	if mList then
		mList:clear()
		mList=nil;
	end
	achievementBtn:selected()
	personalFileBtn:unselected()
	changeHeadBtn:unselected()
	
	if  layer_1 and layer_1:getParent()~=nil then
		layer_1:getParent():removeChild(layer_1, true)
		layer_1 = nil
	end;
	layer_1= CCLayer:create()
	mScene:addChild(layer_1, 0)

	--背景
	local tabName={"title/panel_1003_9.png"}
	if not mCurrentTab or mCurrentTab>#tabName then
	    mCurrentTab=1;
	end
	createTabBar(tabName,1);--创建标题按钮
	showLayer(mCurrentTab);	
end;

--修改密码
function changePwd()
	ChangePwd.createScene()
end;
--返回
function exit()
	if  m_nameEdit ~=nil and m_professionEdit ~=nil and  m_hobbyEdit ~=nil then
		m_nameEdit:setVisible(false)
		m_professionEdit:setVisible(false)
		m_hobbyEdit:setVisible(false)
		m_yearEdit:setVisible(false)
		m_monthEdit:setVisible(false)
		m_dateEdit:setVisible(false)
	end;
	if mList~=nil then
		mList:clear()
		mList=nil;
	end;
	
	CCDirector:sharedDirector():popScene()
	if isRollIn then 	
		RollScene.popScene()
	end
	MainScene.touxiang()
--	MainScene.releaseResource()
--	MainScene.initScene()
end;
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
	tabBar=nil
	m_personInfo=PersonalInfo.getPersonalInfo()
end
-- 释放资源
function releaseResource()
end
-- 创建场景
function createScene(type)
	isRollIn = type
	initResource()
	local scene = ScutScene:new()
	-- 注册网络回调
	scene:registerCallback(networkCallback)
	mScene = scene.root
--	mScene:registerOnEnter("PersonalFileScene.onEnter")
	-- 添加背景
	mLayer = CCLayer:create()
	mScene:addChild(mLayer, 0)

	-- 此处添加场景初始内容
	--透明背景
	local transparentBg=Image.tou_ming;--背景图片
	local menuItem =CCMenuItemImage:create(P(transparentBg),P(transparentBg));
	local menuBg=CCMenu:createWithItem(menuItem);
	menuBg:setContentSize(menuItem:getContentSize());
	menuBg:setAnchorPoint(CCPoint(0,0));
	menuBg:setScaleX(pWinSize.width/menuBg:getContentSize().width*2)
	menuBg:setScaleY(pWinSize.height/menuBg:getContentSize().height*2);
	menuBg:setPosition(CCPoint(0,0));
	mLayer:addChild(menuBg,0);
	
	-- 添加背景
	  local bgLayer = UIHelper.createUIBg(nil,nil,ZyColor:colorWhite(),nil,false);--没有背景框
  	  mLayer:addChild(bgLayer);
	
	--标题
	titleImage= CCSprite:create(P("title/panel_1003_7.png"));
	titleImage:setAnchorPoint(PT(0,0))
	titleImage:setPosition(PT(SY(8),pWinSize.height-titleImage:getContentSize().height-SY(10)))
	mLayer:addChild(titleImage,1)
	
	--左边背景
	local leftSize=SZ(pWinSize.width*0.25,pWinSize.height)
	leftBg= CCSprite:create(P("common/panel_1003_2.png"));
	leftBg:setScaleX(leftSize.width/leftBg:getContentSize().width)
	leftBg:setScaleY(leftSize.height/leftBg:getContentSize().height)
	leftBg:setAnchorPoint(PT(0,0.5))
	leftBg:setPosition(PT(0,pWinSize.height/2-SY(1)))
	mLayer:addChild(leftBg,0)
	
	--头像背景框
	headBg= ZyButton:new("common/panel_1001_6.png","common/panel_1001_6.png")
	headBg:setAnchorPoint(PT(0,0))
	headBg:setPosition(PT(leftBg:getPosition().x+(pWinSize.width*0.25-headBg:getContentSize().width)/2+SX(5),
			pWinSize.height/2+headBg:getContentSize().height/2-SY(20)))
	headBg:addto(mLayer)
	
	--头像
	local Head=string.format("headImg/%s.png",m_personInfo._HeadIcon)
--	headBg:addImage(myHead)
	local myHead= CCSprite:create(P(Head));
	myHead:setAnchorPoint(PT(0.5,0.5))
	mMyHead=myHead
	myHead:setPosition(PT(headBg:getPosition().x+headBg:getContentSize().width/2,headBg:getPosition().y+headBg:getContentSize().height/2))
	mLayer:addChild(myHead,0)
--]]
	--更换头像
	changeHeadBtn=ZyButton:new(Image.image_explain,Image.image_explain_1,nil, nil,FONT_NAME,FONT_SM_SIZE)
	changeHeadBtn:setAnchorPoint(PT(0,0))
	changeHeadBtn:setPosition(PT(leftBg:getPosition().x+(pWinSize.width*0.25-changeHeadBtn:getContentSize().width)/2+SX(5),pWinSize.height/2-changeHeadBtn:getContentSize().height))
	changeHeadBtn:registerScriptTapHandler(changeHead)
	changeHeadBtn:addImage("title/button_1003.png")
	changeHeadBtn:addto(mLayer)
	
	--个人档案按钮
	personalFileBtn=ZyButton:new(Image.image_button_small_0,Image.image_button_small_1,nil, nil,FONT_NAME,FONT_SM_SIZE)
--	personalFileBtn:setScaleX(leftSize.width*0.75/personalFileBtn:getContentSize().width)
--	personalFileBtn:setScaleY(pWinSize.height*0.1/personalFileBtn:getContentSize().height)
	personalFileBtn:setAnchorPoint(PT(0,0))
	
--	local chay=(changeHeadBtn:getPosition().y-leftBg:getPosition().y-leftSize.height/2)/2
--	local starty=leftBg:getPosition().y-leftSize.height/2
--	local bucha=(chay-personalFileBtn:getContentSize().height)/2

	personalFileBtn:setPosition(PT(leftBg:getPosition().x+(pWinSize.width*0.25-personalFileBtn:getContentSize().width)/2+SX(5),changeHeadBtn:getPosition().y-personalFileBtn:getContentSize().height*1.4))
--	personalFileBtn:setPosition(PT(headBg:getPosition().x+(pWinSize.width*0.14-leftSize.width*0.75)/2,starty+chay+bucha))
	personalFileBtn:registerScriptTapHandler(personalFile)
	personalFileBtn:addImage("title/button_1004.png")
	personalFileBtn:addto(mLayer)
	
	--成就榜按钮
	achievementBtn=ZyButton:new(Image.image_button_small_0,Image.image_button_small_1,nil, nil,FONT_NAME,FONT_SM_SIZE)
--	achievementBtn:setScaleX(leftSize.width*0.75/achievementBtn:getContentSize().width)
--	achievementBtn:setScaleY(pWinSize.height*0.1/achievementBtn:getContentSize().height)
	achievementBtn:setAnchorPoint(PT(0,0))
--	achievementBtn:setPosition(PT(personalFileBtn:getPosition().x,personalFileBtn:getPosition().y-achievementBtn:getContentSize().height*1.25))
	achievementBtn:setPosition(PT(leftBg:getPosition().x+(pWinSize.width*0.25-achievementBtn:getContentSize().width)/2+SX(5),leftBg:getPosition().y-leftSize.height/2+SY(55)))
	achievementBtn:registerScriptTapHandler(achievement)
	achievementBtn:addImage("title/button_1005.png")
	achievementBtn:addto(mLayer)

	--返回按钮
	local exitBtn=ZyButton:new(Image.image_exit,nil,nil, nil,FONT_NAME,FONT_SM_SIZE)
	exitBtn:setAnchorPoint(PT(0,0))
	exitBtn:setPosition(PT(SY(7),SY(2)))
	exitBtn:registerScriptTapHandler(exit)
	exitBtn:addto(mLayer)
--[[
	local exitText=CCSprite:create(P("title/panle_1039.png"));
	exitText:setAnchorPoint(PT(0,0))
	exitText:setPosition(PT(exitBtn:getPosition().x+exitBtn:getContentSize().width+SY(2),SY(10)))
	mLayer:addChild(exitText)
--]]	
	local exitText=ZyButton:new("title/panle_1039.png")
	exitText:setAnchorPoint(PT(0,0))
	exitText:setPosition(PT(exitBtn:getPosition().x+exitBtn:getContentSize().width+SY(2),SY(10)))
	exitText:registerScriptTapHandler(exit)
	exitText:addto(mLayer)
	
	layer_1= CCLayer:create()
	mScene:addChild(layer_1, 0)
	
	personalFile()
	--]]

	CCDirector:sharedDirector():pushScene(mScene)
end





--个人档案
function personalFile()
	if mList~=nil and achievementBtn._isSelected  then
		mList:clear()
		mList=nil;
	end;
	personalFileBtn:selected()
	achievementBtn:unselected()
	changeHeadBtn:unselected()
	--背景
	if not mCurrentTab then
	    mCurrentTab=1;
	end
	--Tab 
	local tabName={"title/panle_1021_14.png","title/panle_1021_15.png","title/panle_1021_16.png"}
	createTabBar(tabName,3);--创建标题按钮
	showLayer(mCurrentTab);
	
	--资料背景
	infoBgSize=SZ(pWinSize.width*0.7,pWinSize.height*0.75)
	infoBgImg=CCSprite:create(P("common/panel_1003_3.png"));
	infoBgImg:setScaleX(pWinSize.width*0.7/infoBgImg:getContentSize().width)
	infoBgImg:setScaleY(pWinSize.height*0.75/infoBgImg:getContentSize().height)
	infoBgImg:setAnchorPoint(PT(0,0))
	infoBgImg:setPosition(PT(tabBar:getPosition().x-SX(15),tabBar:getPosition().y-infoBgSize.height))
	mLayer:addChild(infoBgImg,0)	
	
end;





--基本资料
function basicInfo()
	
	--我的账号
	bgSize=SZ(pWinSize.width*0.65,pWinSize.width*0.06)
	local pidBdImg=CCSprite:create(P("common/panle_1026_1.9.png"));
	pidBdImg:setScaleX(bgSize.width/pidBdImg:getContentSize().width)
	pidBdImg:setScaleY(bgSize.height/pidBdImg:getContentSize().height)
	pidBdImg:setAnchorPoint(PT(0,0))
	pidBdImg:setPosition(PT(tabBar:getPosition().x-SY(15)+(pWinSize.width*0.7-bgSize.width)/2,
		tabBar:getPosition().y-bgSize.height-SY(10)))
	layer_1:addChild(pidBdImg,0)	
	
	local pidImg=CCSprite:create(P("title/panle_1026_5.png"));
	pidImg:setAnchorPoint(PT(0,0))
	pidImg:setPosition(PT(pidBdImg:getPosition().x+SY(20),
		pidBdImg:getPosition().y+(bgSize.height-pidImg:getContentSize().height)/2))
	layer_1:addChild(pidImg,0)	
	
	local pidText=CCLabelTTF:create(m_personInfo._Pid,FONT_NAME,FONT_SM_SIZE);
	pidText:setAnchorPoint(PT(0,0))
	pidText:setColor(ccRED1)
	pidText:setPosition(PT(pidImg:getPosition().x+pidImg:getContentSize().width+SY(2),
		pidBdImg:getPosition().y+(bgSize.height-pidText:getContentSize().height)/2))
	layer_1:addChild(pidText)
	
	--我的昵称
	local nickNameBg=CCSprite:create(P("common/panle_1026_1.9.png"));
	nickNameBg:setScaleX(bgSize.width/nickNameBg:getContentSize().width)
	nickNameBg:setScaleY(bgSize.height/nickNameBg:getContentSize().height)
	nickNameBg:setAnchorPoint(PT(0,0))
	nickNameBg:setPosition(PT(pidBdImg:getPosition().x,
		pidBdImg:getPosition().y-bgSize.height-SY(5)))
	layer_1:addChild(nickNameBg,0)	
	
	local nickNameImg=CCSprite:create(P("title/panle_1026_6.png"));
	nickNameImg:setAnchorPoint(PT(0,0))
	nickNameImg:setPosition(PT(nickNameBg:getPosition().x+SY(20),
		nickNameBg:getPosition().y+(bgSize.height-nickNameImg:getContentSize().height)/2))
	layer_1:addChild(nickNameImg,0)	
	
	local nickNameText=CCLabelTTF:create(m_personInfo._NickName,FONT_NAME,FONT_SM_SIZE);
	nickNameText:setAnchorPoint(PT(0,0))
	nickNameText:setColor(ccRED1)
	nickNameText:setPosition(PT(nickNameImg:getPosition().x+nickNameImg:getContentSize().width+SY(2),
		nickNameBg:getPosition().y+(bgSize.height-nickNameText:getContentSize().height)/2))
	layer_1:addChild(nickNameText)
	
	--我的元宝
	local goldBg=CCSprite:create(P("common/panle_1026_1.9.png"));
	goldBg:setScaleX(bgSize.width/goldBg:getContentSize().width)
	goldBg:setScaleY(bgSize.height/goldBg:getContentSize().height)
	goldBg:setAnchorPoint(PT(0,0))
	goldBg:setPosition(PT(nickNameBg:getPosition().x,
		nickNameBg:getPosition().y-bgSize.height-SY(5)))
	layer_1:addChild(goldBg,0)	
	
	local goldImg=CCSprite:create(P("title/panle_1026_7.png"));
	goldImg:setAnchorPoint(PT(0,0))
	goldImg:setPosition(PT(goldBg:getPosition().x+SY(20),
		goldBg:getPosition().y+(bgSize.height-goldImg:getContentSize().height)/2))
	layer_1:addChild(goldImg,0)	
	
	local myGold=string.format(Language.PersonalFile_GOLD,m_personInfo._Gold)
	local goldText=CCLabelTTF:create(myGold,FONT_NAME,FONT_SM_SIZE);
	goldText:setAnchorPoint(PT(0,0))
	goldText:setColor(ccRED1)
	goldText:setPosition(PT(goldImg:getPosition().x+goldImg:getContentSize().width+SY(2),
		goldBg:getPosition().y+(bgSize.height-goldText:getContentSize().height)/2))
	layer_1:addChild(goldText)
	
	--充值按钮
	local rechargeBtn=ZyButton:new(Image.image_explain,Image.image_explain_1,nil,nil,FONT_NAME,FONT_SM_SIZE)
	rechargeBtn:setAnchorPoint(PT(0,0))
	rechargeBtn:setPosition(PT(goldText:getPosition().x+goldText:getContentSize().width+SY(20),
		goldBg:getPosition().y+(bgSize.height-rechargeBtn:getContentSize().height)/2))
	rechargeBtn:registerScriptTapHandler(topUp)
	rechargeBtn:addImage("title/panle_1037_1.png")
	--rechargeBtn:addto(layer_1)
	
	m_goldText = goldText
	m_rechargeBtn = rechargeBtn
	
	
	--我的金豆
	local gameCoinBg=CCSprite:create(P("common/panle_1026_1.9.png"));
	gameCoinBg:setScaleX(bgSize.width/gameCoinBg:getContentSize().width)
	gameCoinBg:setScaleY(bgSize.height/gameCoinBg:getContentSize().height)
	gameCoinBg:setAnchorPoint(PT(0,0))
	gameCoinBg:setPosition(PT(goldBg:getPosition().x,
		goldBg:getPosition().y-bgSize.height-SY(5)))
	layer_1:addChild(gameCoinBg,0)	
	
	local gameCoinImg=CCSprite:create(P("title/panle_1026_8.png"));
	gameCoinImg:setAnchorPoint(PT(0,0))
	gameCoinImg:setPosition(PT(gameCoinBg:getPosition().x+SY(20),
		gameCoinBg:getPosition().y+(bgSize.height-gameCoinImg:getContentSize().height)/2))
	layer_1:addChild(gameCoinImg,0)	
	
	local gameCoinText=CCLabelTTF:create(m_personInfo._GameCoin,FONT_NAME,FONT_SM_SIZE);
	gameCoinText:setAnchorPoint(PT(0,0))
	gameCoinText:setColor(ccRED1)
	gameCoinText:setPosition(PT(gameCoinImg:getPosition().x+gameCoinImg:getContentSize().width+SY(2),
		gameCoinBg:getPosition().y+(bgSize.height-gameCoinText:getContentSize().height)/2))
	layer_1:addChild(gameCoinText)
	
	--账号安全
	safeBg=CCSprite:create(P("common/panle_1026_1.9.png"));
	safeBg:setScaleX(bgSize.width/safeBg:getContentSize().width)
	safeBg:setScaleY(bgSize.height/safeBg:getContentSize().height)
	safeBg:setAnchorPoint(PT(0,0))
	safeBg:setPosition(PT(gameCoinBg:getPosition().x,
		gameCoinBg:getPosition().y-bgSize.height-SY(5)))
	layer_1:addChild(safeBg,0)	
	
	safeImg=CCSprite:create(P("title/panle_1026_9.png"));
	safeImg:setAnchorPoint(PT(0,0))
	safeImg:setPosition(PT(safeBg:getPosition().x+SY(20),
		safeBg:getPosition().y+(bgSize.height-safeImg:getContentSize().height)/2))
	layer_1:addChild(safeImg,0)	
	
	--修改密码按钮
	local pwdStr="panle_1032"
	--[[
	if m_personInfo._UserType==1 then
		pwdStr="panle_1032"
	elseif m_personInfo._UserType==0 then 
		pwdStr="panle_1048"
	end;
	-]]
	if accountInfo.UserType==1 then
		pwdStr="panle_1032"
	elseif accountInfo.UserType==0 then 
		pwdStr="panle_1048"
	end;
	
	local pwdImg=string.format("title/%s.png",pwdStr)
	
	changePwdBtn=ZyButton:new(Image.image_explain,Image.image_explain_1,nil,nil,FONT_NAME,FONT_SM_SIZE)
	changePwdBtn:setAnchorPoint(PT(0,0))
	changePwdBtn:addImage(pwdImg)
	changePwdBtn:setPosition(PT(safeImg:getPosition().x+safeImg:getContentSize().width+SY(20),
		safeBg:getPosition().y+(bgSize.height-changePwdBtn:getContentSize().height)/2))
	changePwdBtn:registerScriptTapHandler(changePwd)
	changePwdBtn:addto(layer_1)
	
	
	local mMobileType ,mGameType,mRetailID=accountInfo. readMoble()
	local retailTable=nil
	local btnTable={}
	if mRetailID=="0001"  or  mRetailID=="0036"  then
		--账号设置
		btnTable[#btnTable+1]={btnName=Language.SET_PERSONAL}
	end
	
	--论坛专区
    	 if  mRetailID=="0001" then	
    	 	safeBg:setIsVisible(false)
    	 	changePwdBtn:setIsVisible(false)
    	 	safeImg:setIsVisible(false)
    	 	pidImg:setIsVisible(false)
    	 	pidText:setIsVisible(false)
		btnTable[#btnTable+1]={btnName=Language.IDS_NETDRAGON}
	  end
	  
	  --问题反馈
    	 if  mRetailID=="0001" then
		btnTable[#btnTable+1]={btnName=Language.SET_FEEDBACK}
	  end

    	 if  mRetailID=="0021" then
    	 	safeBg:setIsVisible(false)
    	 	changePwdBtn:setIsVisible(false)
    	 	safeImg:setIsVisible(false)
    	 	pidImg:setIsVisible(false)
    	 	pidText:setIsVisible(false)
	 end
	 
	  for k, v in pairs(btnTable) do
		local btn=ZyButton:new(Image.image_explain,Image.image_explain_1,nil,v.btnName,FONT_NAME,FONT_FMM_SIZE)
		btn:setPosition(PT(pidImg:getPosition().x+(k-1)*btn:getContentSize().width*1.2,
			pidBdImg:getPosition().y))
		btn:registerScriptTapHandler(PersonalFileScene.gotoPersonalAction)
		btn:setTag(k)
		btn:addto(layer_1) 	 
	  end
	  
end;


function gotoPersonalAction(node)
	local tag=node:getTag()
	if tag==1 then
		netdragonCenter()
	elseif  tag==2 then
		gotoBBS()
	elseif  tag==3 then
		netdragonFAQ()
	end
end;

function  gotoBBS()
    	local nType = ScutUtility.ScutUtils:GetPlatformType();
	if nType == ScutUtility.ptAndroid then
        	channelEngine.command("openForum")	
	else
        		LoginScene.getLogin91Sdk():openByCommand("enterPlatForm",1)
		--[[
		local sURL="http://bbs.18183.com/forum-koudaitianjie-1.html"
		mWebVies=NdWebView:new()
		local startY=pWinSize.height*0.085
		mWebVies:init(sURL, CCRectMake(0, startY,pWinSize.width, pWinSize.height*0.75),"", nil, nil)
		--]]
	end
end;
--
function netdragonCenter()
    	local nType = ScutUtility.ScutUtils:GetPlatformType();
	if nType == ScutUtility.ptAndroid then
        		channelEngine.enterUserCenter()
	else
        		LoginScene.getLogin91Sdk():showMessageBox()
	end
end;

function netdragonFAQ()
    	local nType = ScutUtility.ScutUtils:GetPlatformType();
	if nType == ScutUtility.ptAndroid then
        	channelEngine.sendSeed("",1)
	else
        	LoginScene.getLogin91Sdk():LoginEnv()
	end
end;

--社交资料
function showSocialInfo()
	local infoBgImg=nil
	if not infoBgImg then
		infoBgImg={}
		local bgSize=SZ(pWinSize.width*0.65,pWinSize.height*0.09)
		for i=1,5 do
			infoBgImg[i]=ZyButton:new("common/panle_1026_1.9.png",nil,nil, nil,FONT_NAME,FONT_SM_SIZE)
			infoBgImg[i]:setScaleX(bgSize.width/infoBgImg[i]:getContentSize().width)
			infoBgImg[i]:setScaleY(bgSize.height/infoBgImg[i]:getContentSize().height)
			infoBgImg[i]:setAnchorPoint(PT(0,0))
			infoBgImg[i]:registerScriptTapHandler(PersonalFileScene.toWriteIn)
			infoBgImg[i]:setTag(i)
			infoBgImg[i]:addto(layer_1)
			if i==1 then
				infoBgImg[i]:setPosition(PT(tabBar:getPosition().x-SY(15)+(pWinSize.width*0.7-bgSize.width)/2,
					tabBar:getPosition().y-bgSize.height-SY(10)))
			else	
				infoBgImg[i]:setPosition(PT(infoBgImg[1]:getPosition().x,
					infoBgImg[i-1]:getPosition().y-bgSize.height-SY(5)))
			end;
		end;
		
		--姓名
		nameImg= CCSprite:create(P("title/panle_1021_9.png"));
		nameImg:setAnchorPoint(PT(0,0))
		nameImg:setPosition(PT(infoBgImg[1]:getPosition().x+SY(40),
			infoBgImg[1]:getPosition().y+(bgSize.height-nameImg:getContentSize().height)/2))
		layer_1:addChild(nameImg,1)
		 
		if m_socialInfo.Name==nil then
			m_socialInfo.Name=""
		end;
		--姓名
		nameText= CCLabelTTF:create(m_socialInfo.Name,FONT_NAME,FONT_SM_SIZE);
		nameText:setAnchorPoint(PT(0,0))
		nameText:setColor(ccRED1)
		nameText:setPosition(PT(nameImg:getPosition().x+nameImg:getContentSize().width+SY(8),
			nameImg:getPosition().y-SY(2)))
		layer_1:addChild(nameText,1)
		
		--姓名编辑框
		m_nameEdit = CScutEdit:new()
		m_nameEdit:init(false, false,ccc4(235,197, 151,255), ccc4(0,39,61,200))
		m_nameEdit:setRect(CCRect(nameImg:getPosition().x+nameImg:getContentSize().width+SY(8),
			pWinSize.height-(nameImg:getPosition().y+nameImg:getContentSize().height+SY(2) ),
			pWinSize.width*0.25, nameImg:getContentSize().height+SY(6)))
		m_nameEdit:setText(m_socialInfo.Name)
		m_nameEdit:setVisible(false)
		
		--性别
		local sexText= CCSprite:create(P("title/panle_1021_10.png"));
		sexText:setAnchorPoint(PT(0,0))
		sexText:setPosition(PT(infoBgImg[2]:getPosition().x+SY(40),
			infoBgImg[2]:getPosition().y+(bgSize.height-sexText:getContentSize().height)/2))
		layer_1:addChild(sexText,2)
		
		girlBtn=ZyButton:new("common/panle_1021_2.png",nil,nil, nil,FONT_NAME,FONT_SM_SIZE)
		girlBtn:setAnchorPoint(PT(0,0))
		girlBtn:setPosition(PT(sexText:getPosition().x+sexText:getContentSize().width+SY(10),
			infoBgImg[2]:getPosition().y+(bgSize.height-girlBtn:getContentSize().height)/2))
		girlBtn:registerScriptTapHandler(PersonalFileScene.selectGirl)
		girlBtn:addto(layer_1)
	
		local girltxt=CCSprite:create(P("title/panle_1021_17.png"));
		girltxt:setAnchorPoint(PT(0,0))
		girltxt:setPosition(PT(girlBtn:getPosition().x+girlBtn:getContentSize().width+SY(5),
			infoBgImg[2]:getPosition().y+(bgSize.height-girltxt:getContentSize().height)/2))
		layer_1:addChild(girltxt)
		
		manBtn=ZyButton:new("common/panle_1021_2.png",nil,nil, nil,FONT_NAME,FONT_SM_SIZE)
		manBtn:setAnchorPoint(PT(0,0))
		manBtn:setPosition(PT(girltxt:getPosition().x+girltxt:getContentSize().width+SY(25),
			infoBgImg[2]:getPosition().y+(bgSize.height-manBtn:getContentSize().height)/2))
		manBtn:registerScriptTapHandler(PersonalFileScene.selectMan)
		manBtn:addto(layer_1)
	
		local mantxt=CCSprite:create(P("title/panle_1021_18.png"));
		mantxt:setAnchorPoint(PT(0,0))
		mantxt:setPosition(PT(manBtn:getPosition().x+manBtn:getContentSize().width+SY(5),
			infoBgImg[2]:getPosition().y+(bgSize.height-mantxt:getContentSize().height)/2))
		layer_1:addChild(mantxt)
		
		--打钩图标
		selectImg= CCSprite:create(P("common/panle_1021_1.png"));
		selectImg:setAnchorPoint(PT(0,0))
		layer_1:addChild(selectImg,2)
		
		--生日
		local birthdayText= CCSprite:create(P("title/panle_1021_11.png"));
		birthdayText:setAnchorPoint(PT(0,0))
		birthdayText:setPosition(PT(infoBgImg[3]:getPosition().x+SY(40),
			infoBgImg[3]:getPosition().y+(bgSize.height-birthdayText:getContentSize().height)/2))
		layer_1:addChild(birthdayText,1)
		
		m_yearEdit = CScutEdit:new()
		m_yearEdit:init(false, false,ccc4(235,197, 151,255), ccc4(0,39,61,200))
		m_yearEdit:setRect(CCRect(birthdayText:getPosition().x+birthdayText:getContentSize().width+SY(8),
			pWinSize.height-(birthdayText:getPosition().y+birthdayText:getContentSize().height ),
			pWinSize.width*0.1, birthdayText:getContentSize().height))
		m_yearEdit:setVisible(false)
		
		local yearImg=CCSprite:create(P("title/panle_1059.png"));
		yearImg:setAnchorPoint(PT(0,0))
		yearImg:setPosition(PT(birthdayText:getPosition().x+birthdayText:getContentSize().width+SY(10)+pWinSize.width*0.1,
			infoBgImg[3]:getPosition().y+(bgSize.height-mantxt:getContentSize().height)/2))
		layer_1:addChild(yearImg)
		
		m_monthEdit = CScutEdit:new()
		m_monthEdit:init(false, false,ccc4(235,197, 151,255), ccc4(0,39,61,200))
		m_monthEdit:setRect(CCRect(yearImg:getPosition().x+yearImg:getContentSize().width+SY(10),
				pWinSize.height-(infoBgImg[3]:getPosition().y+infoBgImg[3]:getContentSize().height)+SY(15) ,
			pWinSize.width*0.1, infoBgImg[3]:getContentSize().height-SY(20)))
		m_monthEdit:setVisible(false)
		
		local monthImg=CCSprite:create(P("title/panle_1058.png"));
		monthImg:setAnchorPoint(PT(0,0))
		monthImg:setPosition(PT(yearImg:getPosition().x+yearImg:getContentSize().width+SY(10)+pWinSize.width*0.1,
			infoBgImg[3]:getPosition().y+(bgSize.height-mantxt:getContentSize().height)/2))
		layer_1:addChild(monthImg)
		
		m_dateEdit = CScutEdit:new()
		m_dateEdit:init(false, false,ccc4(235,197, 151,255), ccc4(0,39,61,200))
		m_dateEdit:setRect(CCRect(monthImg:getPosition().x+monthImg:getContentSize().width+SY(10),
				pWinSize.height-(infoBgImg[3]:getPosition().y+infoBgImg[3]:getContentSize().height)+SY(15) ,
			pWinSize.width*0.1, infoBgImg[3]:getContentSize().height-SY(20)))
		m_dateEdit:setVisible(false)
		
		local dateImg=CCSprite:create(P("title/panle_1057.png"));
		dateImg:setAnchorPoint(PT(0,0))
		dateImg:setPosition(PT(monthImg:getPosition().x+monthImg:getContentSize().width+SY(10)+pWinSize.width*0.1,
			infoBgImg[3]:getPosition().y+(bgSize.height-mantxt:getContentSize().height)/2))
		layer_1:addChild(dateImg)
		
		if m_socialInfo.Birthday~=nil then
			local year,month,day=yearMonthDay(m_socialInfo.Birthday)
			
			yearText=CCLabelTTF:create(year,FONT_NAME,FONT_SM_SIZE);
			yearText:setColor(ccRED1)
			yearText:setAnchorPoint(PT(0,0))
			yearText:setPosition(PT(birthdayText:getPosition().x+birthdayText:getContentSize().width+SY(8),
				birthdayText:getPosition().y))
			m_yearEdit:setText(year)
			layer_1:addChild(yearText)
			
			monthText=CCLabelTTF:create(month,FONT_NAME,FONT_SM_SIZE);
			monthText:setColor(ccRED1)
			monthText:setAnchorPoint(PT(0,0))
			monthText:setPosition(PT(yearImg:getPosition().x+yearImg:getContentSize().width+SY(10),
				birthdayText:getPosition().y))
			m_monthEdit:setText(month)
			layer_1:addChild(monthText)
			
			dayText=CCLabelTTF:create(day,FONT_NAME,FONT_SM_SIZE);
			dayText:setColor(ccRED1)
			dayText:setAnchorPoint(PT(0,0))
			dayText:setPosition(PT(monthImg:getPosition().x+monthImg:getContentSize().width+SY(10),
				birthdayText:getPosition().y))
			m_dateEdit:setText(day)
			layer_1:addChild(dayText)
			
		end;
		
		--爱好
		local hobbyImg= CCSprite:create(P("title/panle_1021_12.png"));
		hobbyImg:setAnchorPoint(PT(0,0))
		hobbyImg:setPosition(PT(infoBgImg[4]:getPosition().x+SY(40),
			infoBgImg[4]:getPosition().y+(bgSize.height-hobbyImg:getContentSize().height)/2))
		layer_1:addChild(hobbyImg,1)
		
		if m_socialInfo.Hobby==nil then
			m_socialInfo.Hobby=""
		end;
		--爱好
		hobbyText= CCLabelTTF:create(m_socialInfo.Hobby,FONT_NAME,FONT_SM_SIZE);
		hobbyText:setColor(ccRED1)
		hobbyText:setAnchorPoint(PT(0,0))
		hobbyText:setPosition(PT(hobbyImg:getPosition().x+hobbyImg:getContentSize().width+SY(8),
			hobbyImg:getPosition().y))
		layer_1:addChild(hobbyText,1)
		
		--爱好编辑框
		m_hobbyEdit = CScutEdit:new()
		m_hobbyEdit:init(false, false,ccc4(235,197, 151,255), ccc4(0,39,61,200))
		m_hobbyEdit:setRect(CCRect(hobbyImg:getPosition().x+hobbyImg:getContentSize().width+SY(8),
			pWinSize.height-(hobbyImg:getPosition().y+hobbyImg:getContentSize().height+SY(2) ),
			pWinSize.width*0.25, hobbyImg:getContentSize().height+SY(6)))
		m_hobbyEdit:setText(m_socialInfo.Hobby)
		m_hobbyEdit:setVisible(false)

		--职业
		local professionImg= CCSprite:create(P("title/panle_1021_13.png"));
		professionImg:setAnchorPoint(PT(0,0))
		professionImg:setPosition(PT(infoBgImg[5]:getPosition().x+SY(40),
			infoBgImg[5]:getPosition().y+(bgSize.height-professionImg:getContentSize().height)/2))
		layer_1:addChild(professionImg,1)
		
		if m_socialInfo.Profession==nil then
			m_socialInfo.Profession=""
		end;
		--职业
		professionText= CCLabelTTF:create(m_socialInfo.Profession,FONT_NAME,FONT_SM_SIZE);
		professionText:setColor(ccRED1)
		professionText:setAnchorPoint(PT(0,0))
		professionText:setPosition(PT(professionImg:getPosition().x+professionImg:getContentSize().width+SY(8),
			professionImg:getPosition().y))
		layer_1:addChild(professionText,1)
		
		--职业编辑框
		m_professionEdit = CScutEdit:new()
		m_professionEdit:init(false, false,ccc4(235,197, 151,255), ccc4(0,39,61,200))
		m_professionEdit:setRect(CCRect(professionImg:getPosition().x+professionImg:getContentSize().width+SY(8),
			pWinSize.height-(professionImg:getPosition().y+professionImg:getContentSize().height+SY(2) ),
			pWinSize.width*0.25, professionImg:getContentSize().height+SY(6)))
		m_professionEdit:setText(m_socialInfo.Profession)
		m_professionEdit:setVisible(false)

		--保存按钮
		local saveBtn=ZyButton:new(Image.image_button_small_0,Image.image_button_small_1,nil, nil,FONT_NAME,FONT_SM_SIZE)
		saveBtn:setAnchorPoint(PT(0,0))
		saveBtn:setPosition(PT(infoBgImg[5]:getPosition().x+(bgSize.width-saveBtn:getContentSize().width)/2,
			infoBgImg[5]:getPosition().y-saveBtn:getContentSize().height-SY(20)))
		saveBtn:registerScriptTapHandler(PersonalFileScene.save)
		saveBtn:addImage("title/panle_1026_4.png")
		saveBtn:addto(layer_1)
		
		if m_socialInfo.Sex==0 then
			selectMan()
		else
			selectGirl()
		end;
	end;
end;

--日期字符串拆分
function yearMonthDay(birthday)
	local year_i,year_j=string.find(birthday,"-")
	local year=string.sub(birthday,1,year_i-1)
	local month_i,month_j=string.find(birthday,"-",year_i+1)
	local month=string.sub(birthday,year_i+1,month_i-1)
	local day=string.sub(birthday,month_i+1)
	return year,month,day
end;

function selectGirl()
	if m_nameEdit~=nil and m_hobbyEdit~=nil and m_professionEdit~=nil then
		m_nameEdit:setVisible(false)
		m_hobbyEdit:setVisible(false)
		m_professionEdit:setVisible(false)
		m_yearEdit:setVisible(false)
		m_monthEdit:setVisible(false)
		m_dateEdit:setVisible(false)
		
		if m_nameEdit:GetEditText()~=nil and m_nameEdit:GetEditText()~="" then
			m_socialInfo.Name=m_nameEdit:GetEditText()
			nameText:setString(m_socialInfo.Name)
		end;
		if m_hobbyEdit:GetEditText()~=nil and m_hobbyEdit:GetEditText()~="" then
			m_socialInfo.Hobby=m_hobbyEdit:GetEditText()
			hobbyText:setString(m_socialInfo.Hobby)
		end;
		if m_professionEdit:GetEditText()~=nil and m_professionEdit:GetEditText()~="" then
			m_socialInfo.Profession=m_professionEdit:GetEditText()
			professionText:setString(m_socialInfo.Profession)
		end;
		if m_yearEdit:GetEditText()~=nil and m_yearEdit:GetEditText()~="" then
			yearText:setString(m_yearEdit:GetEditText())
		end;
		if m_monthEdit:GetEditText()~=nil and m_monthEdit:GetEditText()~="" then
			monthText:setString(m_monthEdit:GetEditText())
		end;
		if m_dateEdit:GetEditText()~=nil and m_dateEdit:GetEditText()~="" then
			dayText:setString(m_dateEdit:GetEditText())
		end;
		m_socialInfo.Birthday=m_yearEdit:GetEditText().."-"..m_monthEdit:GetEditText().. "-".. m_dateEdit:GetEditText()
	end;
		
	girlBtn:selected()
	manBtn:unselected()
	
	selectImg:setPosition(PT(girlBtn:getPosition().x+(girlBtn:getContentSize().width-selectImg:getContentSize().width)/2,
		girlBtn:getPosition().y+(girlBtn:getContentSize().height-selectImg:getContentSize().height)/2))
	manBtn:unselected()
end;

function selectMan()

	if m_nameEdit~=nil and m_hobbyEdit~=nil and m_professionEdit~=nil then
		m_nameEdit:setVisible(false)
		m_hobbyEdit:setVisible(false)
		m_professionEdit:setVisible(false)
		m_yearEdit:setVisible(false)
		m_monthEdit:setVisible(false)
		m_dateEdit:setVisible(false)
		
		if m_nameEdit:GetEditText()~=nil and m_nameEdit:GetEditText()~="" then
			m_socialInfo.Name=m_nameEdit:GetEditText()
			nameText:setString(m_socialInfo.Name)
		end;
		if m_hobbyEdit:GetEditText()~=nil and m_hobbyEdit:GetEditText()~="" then
			m_socialInfo.Hobby=m_hobbyEdit:GetEditText()
			hobbyText:setString(m_socialInfo.Hobby)
		end;
		if m_professionEdit:GetEditText()~=nil and m_professionEdit:GetEditText()~="" then
			m_socialInfo.Profession=m_professionEdit:GetEditText()
			professionText:setString(m_socialInfo.Profession)
		end;
		if m_yearEdit:GetEditText()~=nil and m_yearEdit:GetEditText()~="" then
		    local year=m_yearEdit:GetEditText()
			yearText:setString(year)
		end;
		if m_monthEdit:GetEditText()~=nil and m_monthEdit:GetEditText()~="" then
			monthText:setString(m_monthEdit:GetEditText())
		end;
		if m_dateEdit:GetEditText()~=nil and m_dateEdit:GetEditText()~="" then
			dayText:setString(m_dateEdit:GetEditText())
		end;
		m_socialInfo.Birthday=m_yearEdit:GetEditText().."-"..m_monthEdit:GetEditText().. "-".. m_dateEdit:GetEditText()
	end;
	
	girlBtn:unselected()
	manBtn:selected()
	
	selectImg:setPosition(PT(manBtn:getPosition().x+(manBtn:getContentSize().width-selectImg:getContentSize().width)/2,
		manBtn:getPosition().y+(manBtn:getContentSize().height-selectImg:getContentSize().height)/2))
	girlBtn:unselected()
end;

--游戏资料
function gameInfo()
	--欢乐斗地主
	local bgSize=SZ(pWinSize.width*0.65,pWinSize.width*0.06)
	local ddzImg=CCSprite:create(P("common/panle_1026_1.9.png"));
	ddzImg:setScaleX(bgSize.width/ddzImg:getContentSize().width)
	ddzImg:setScaleY(bgSize.height/ddzImg:getContentSize().height)
	ddzImg:setAnchorPoint(PT(0,0))
	ddzImg:setPosition(PT(tabBar:getPosition().x-SY(15)+(pWinSize.width*0.7-bgSize.width)/2,
		tabBar:getPosition().y-bgSize.height-SY(10)))
	layer_1:addChild(ddzImg,0)	
	
	local ddzText=CCSprite:create(P("title/panle_1026_10.png"));
	ddzText:setAnchorPoint(PT(0,0))
	ddzText:setPosition(PT(ddzImg:getPosition().x+(bgSize.width-ddzText:getContentSize().width)/2,
		ddzImg:getPosition().y+(bgSize.height-ddzText:getContentSize().height)/2))
	layer_1:addChild(ddzText,0)
	
	--积分称号
	local bgImg=CCSprite:create(P("common/panle_1026_1.9.png"));
	bgImg:setScaleX(bgSize.width/bgImg:getContentSize().width)
	bgImg:setScaleY(bgSize.height/bgImg:getContentSize().height)
	bgImg:setAnchorPoint(PT(0,0))
	bgImg:setPosition(PT(ddzImg:getPosition().x,
		ddzImg:getPosition().y-bgSize.height-SY(10)))
	layer_1:addChild(bgImg,0)
	
	local scoreImg=CCSprite:create(P("title/panle_1026_11.png"));
	scoreImg:setAnchorPoint(PT(0,0))
	scoreImg:setPosition(PT(bgImg:getPosition().x+SY(20),
		bgImg:getPosition().y+(bgSize.height-scoreImg:getContentSize().height)/2))
	layer_1:addChild(scoreImg,0)	
	
	local scoreText=CCLabelTTF:create(m_personInfo._ScoreNum,FONT_NAME,FONT_SM_SIZE);
	scoreText:setAnchorPoint(PT(0,0))
	scoreText:setColor(ccRED1)
	scoreText:setPosition(PT(scoreImg:getPosition().x+scoreImg:getContentSize().width+SY(2),
		bgImg:getPosition().y+(bgSize.height-scoreText:getContentSize().height)/2))
	layer_1:addChild(scoreText)
	
	local titleNameImg=CCSprite:create(P("title/panle_1026_12.png"));
	titleNameImg:setAnchorPoint(PT(0,0))
	titleNameImg:setPosition(PT(bgImg:getPosition().x+bgSize.width/2-SY(5),
		bgImg:getPosition().y+(bgSize.height-titleNameImg:getContentSize().height)/2))
	layer_1:addChild(titleNameImg,0)	
	
	local titleNameText=CCLabelTTF:create(m_personInfo._TitleName,FONT_NAME,FONT_SM_SIZE);
	titleNameText:setAnchorPoint(PT(0,0))
	titleNameText:setColor(ccRED1)
	titleNameText:setPosition(PT(titleNameImg:getPosition().x+titleNameImg:getContentSize().width+SY(2),
		bgImg:getPosition().y+(bgSize.height-titleNameText:getContentSize().height)/2))
	layer_1:addChild(titleNameText)
	
	--局数
	local countBgImg=CCSprite:create(P("common/panle_1026_1.9.png"));
	countBgImg:setScaleX(bgSize.width/countBgImg:getContentSize().width)
	countBgImg:setScaleY(bgSize.height/countBgImg:getContentSize().height)
	countBgImg:setAnchorPoint(PT(0,0))
	countBgImg:setPosition(PT(bgImg:getPosition().x,
		bgImg:getPosition().y-bgSize.height-SY(10)))
	layer_1:addChild(countBgImg,0)	
	
	local countImg=CCSprite:create(P("title/panle_1026_13.png"));
	countImg:setAnchorPoint(PT(0,0))
	countImg:setPosition(PT(countBgImg:getPosition().x+SY(20),
		countBgImg:getPosition().y+(bgSize.height-countImg:getContentSize().height)/2))
	layer_1:addChild(countImg,0)	
	
	local totalCount=m_personInfo._WinNum+m_personInfo._FailNum
	local totalCountText=CCLabelTTF:create(totalCount,FONT_NAME,FONT_SM_SIZE);
	totalCountText:setAnchorPoint(PT(0,0))
	totalCountText:setColor(ccRED1)
	totalCountText:setPosition(PT(countImg:getPosition().x+countImg:getContentSize().width+SY(2),
		countBgImg:getPosition().y+(bgSize.height-totalCountText:getContentSize().height)/2))
	layer_1:addChild(totalCountText)
	
	local winCountImg=CCSprite:create(P("title/panle_1026_14.png"));
	winCountImg:setAnchorPoint(PT(0,0))
	winCountImg:setPosition(PT(countBgImg:getPosition().x+bgSize.width/2-SY(5),
		countBgImg:getPosition().y+(bgSize.height-winCountImg:getContentSize().height)/2))
	layer_1:addChild(winCountImg,0)	
	
	local winCountText=CCLabelTTF:create(m_personInfo._WinNum,FONT_NAME,FONT_SM_SIZE);
	winCountText:setAnchorPoint(PT(0,0))
	winCountText:setColor(ccRED1)
	winCountText:setPosition(PT(winCountImg:getPosition().x+winCountImg:getContentSize().width+SY(2),
		countBgImg:getPosition().y+(bgSize.height-winCountText:getContentSize().height)/2))
	layer_1:addChild(winCountText)
	
	--胜率
	local winRateBg=CCSprite:create(P("common/panle_1026_1.9.png"));
	winRateBg:setScaleX(bgSize.width/winRateBg:getContentSize().width)
	winRateBg:setScaleY(bgSize.height/winRateBg:getContentSize().height)
	winRateBg:setAnchorPoint(PT(0,0))
	winRateBg:setPosition(PT(countBgImg:getPosition().x,
		countBgImg:getPosition().y-bgSize.height-SY(10)))
	layer_1:addChild(winRateBg,0)	
	
	local winRateImg=CCSprite:create(P("title/panle_1026_15.png"));
	winRateImg:setAnchorPoint(PT(0,0))
	winRateImg:setPosition(PT(winRateBg:getPosition().x+SY(20),
		winRateBg:getPosition().y+(bgSize.height-winRateImg:getContentSize().height)/2))
	layer_1:addChild(winRateImg,0)	
	
	local winRateText=CCLabelTTF:create(m_personInfo._WinRate.."%",FONT_NAME,FONT_SM_SIZE);
	winRateText:setAnchorPoint(PT(0,0))
	winRateText:setColor(ccRED1)
	winRateText:setPosition(PT(winRateImg:getPosition().x+winRateImg:getContentSize().width+SY(2),
		winRateBg:getPosition().y+(bgSize.height-winRateText:getContentSize().height)/2))
	layer_1:addChild(winRateText)
	
end;

--展示所有成就
function showAchievement()
	
	local listSize = SZ(pWinSize.width*0.67, pWinSize.height*0.71)
	local list_x=tabBar:getPosition().x-SX(7)
	local list_y=tabBar:getPosition().y-pWinSize.height*0.75+pWinSize.height*0.03
	local listRowH=listSize.height/2
	
	mList = ScutCxList:node(listRowH, ccc4(24, 24, 24, 0), listSize)
	mList:setAnchorPoint(PT(0, 0))
	mList:setPosition(PT(list_x, list_y))
	mLayer:addChild(mList,0)
	
	local layout=CxLayout()
	layout.val_x.t = ABS_WITH_PIXEL
	layout.val_y.t = ABS_WITH_PIXEL
	layout.val_x.val.pixel_val =0
	layout.val_y.val.pixel_val =0

	local length=#achievementTable
	local row=nil
	local col=5
	if length%5==0 then
		row = math.floor(length/5)
	else
		row=math.floor(length/5)+1
	end;

	local colW=listSize.width/col
	local startX=SX(2)
	for index=1,row do
		local listItem = ScutCxListItem:itemWithColor(ccc3(32, 24, 3))
		listItem:setOpacity(0)
		
		if index==row then
			if length%5==0 then
				col=5
			else	
				col=length%5
			end;
		else
			col=5
		end;
		
		for  k =1,col do		
			local i=k+(index-1)*5
			local layer=CCLayer:create()
			local path=achievementTable[i].HeadID
			if achievementTable[i].IsGain==1 then
				path=path.."_1"
			end;
			local headStr=string.format("chengjiu/%s.png",path)
			--物品背景
			local headColW=(k-1)%col*colW
			local achievementBg= ZyButton:new(headStr,nil,nil, nil,FONT_NAME,FONT_SM_SIZE)
	  		achievementBg:setAnchorPoint(PT(0,0))
	  		
	  		local bgSprite=CCSprite:create(P("common/panel_1003_6.png"))
	  		bgSprite:setAnchorPoint(PT(0.5,0.5))	
	  		layer:addChild(bgSprite,0)

			achievementBg:setPosition(PT(startX+headColW,0))
			bgSprite:setPosition(PT(achievementBg:getPosition().x+achievementBg:getContentSize().width/2,
								achievementBg:getPosition().y+achievementBg:getContentSize().height/2))
			achievementBg:registerScriptTapHandler(achievementDetail)
			achievementBg:setTag(i)
			achievementBg:addto(layer)
			listItem:addChildItem(layer, layout)
			mList:setRowHeight(achievementBg:getContentSize().height+SY(5))
			
		end
		mList:addListItem(listItem, false)
	end;
	
	
end;



function showDetail()

	achievementLayer = CCLayer:create()
	mScene:addChild(achievementLayer, 0)
	
	local transparentBg=Image.tou_ming;--背景图片
	local menuItem =CCMenuItemImage:create(P(transparentBg),P(transparentBg));
	local menuBg=CCMenu:createWithItem(menuItem);
	menuBg:setContentSize(menuItem:getContentSize());
	menuBg:setAnchorPoint(CCPoint(0,0));
	menuBg:setScaleX(pWinSize.width/menuBg:getContentSize().width*2)
	menuBg:setScaleY(pWinSize.height/menuBg:getContentSize().height*2);
	menuBg:setPosition(CCPoint(0,0));
	achievementLayer:addChild(menuBg,0);
	
	--背景
	local detailBg= CCSprite:create(P("common/panle_1015.9.png"));
	detailBg:setAnchorPoint(PT(0,0))
	detailBg:setScaleX(pWinSize.width*0.5/detailBg:getContentSize().width)
	detailBg:setScaleY(pWinSize.height*0.6/detailBg:getContentSize().height);
	detailBg:setPosition(PT((pWinSize.width-pWinSize.width*0.5)/2,(pWinSize.height-pWinSize.height*0.6)/2))
	achievementLayer:addChild(detailBg,0)
	
	--物品图标框
	local detailImg= ZyButton:new("common/panle_1014_2.png","common/panle_1014_2.png",nil, nil,FONT_NAME,FONT_SM_SIZE)
	detailImg:setAnchorPoint(PT(0,0))
	detailImg:setPosition(PT(detailBg:getPosition().x+SY(20),
		detailBg:getPosition().y+(pWinSize.height*0.6-detailImg:getContentSize().height)/2+SY(20)))
	detailImg:addto(achievementLayer)
	
	if m_achievementDetailInfo.HeadID then
		local headStr=string.format("chengjiu/%s.png",m_achievementDetailInfo.HeadID)
		detailImg:addImage(headStr)
	end;
	
	--成就名称背景
	local nameBg= CCSprite:create(P("common/panle_1051.png"));
	nameBg:setAnchorPoint(PT(0,0))
	nameBg:setPosition(PT(detailImg:getPosition().x,detailImg:getPosition().y-nameBg:getContentSize().height-SY(5)))
	achievementLayer:addChild(nameBg,0)
	
	--商品名称
	local nameLabel=CCLabelTTF:create(m_achievementDetailInfo.AchieveName,FONT_NAME,20);
	nameLabel:setAnchorPoint(PT(0,0))
	nameLabel:setPosition(PT(nameBg:getPosition().x+(nameBg:getContentSize().width-nameLabel:getContentSize().width)/2,
			nameBg:getPosition().y+(nameBg:getContentSize().height-nameLabel:getContentSize().height)/2))
	achievementLayer:addChild(nameLabel)
	
	--成就详细
	local detailStr = string.format("<label color='%d,%d,%d'>%s</label>",241,176,63,m_achievementDetailInfo.AchieveDesc)
	local detailLabel=ZyMultiLabel:new(detailStr,pWinSize.width*0.5*0.5, FONT_NAME, FONT_SM_SIZE, nil, nil,false)
	detailLabel:setAnchorPoint(PT(0,0))
	detailLabel:setPosition(PT(detailImg:getPosition().x+detailImg:getContentSize().width+SY(10),
			detailImg:getPosition().y+detailImg:getContentSize().height-detailLabel:getContentSize().height))
	detailLabel:addto(achievementLayer,0)
	
	if m_achievementDetailInfo.IsComplete ==1 then
		
		local numLabel=CCLabelTTF:create(Language.PERSONA_ACHIEVEMENT,FONT_NAME,FONT_SM_SIZE);
		numLabel:setAnchorPoint(PT(0,0))
		numLabel:setPosition(PT(detailImg:getPosition().x+detailImg:getContentSize().width+(pWinSize.width*0.5*0.5-numLabel:getContentSize().width)/2,
				detailImg:getPosition().y+detailImg:getContentSize().height-numLabel:getContentSize().height))
		achievementLayer:addChild(numLabel)
	
		local rateStr = string.format("<label >%s/%s</label>",m_achievementDetailInfo.CompleteNum,m_achievementDetailInfo.AchieveNum)
		local rateLabel=CCLabelTTF:create(m_achievementDetailInfo.CompleteNum.."/"..m_achievementDetailInfo.AchieveNum,FONT_NAME,20);
		rateLabel:setAnchorPoint(PT(0,0))
		rateLabel:setPosition(PT(detailImg:getPosition().x+detailImg:getContentSize().width+(pWinSize.width*0.5*0.5-rateLabel:getContentSize().width)/2,
			numLabel:getPosition().y-pWinSize.height*0.031-rateLabel:getContentSize().height))
		achievementLayer:addChild(rateLabel)
		
		detailLabel:setPosition(PT(detailImg:getPosition().x+detailImg:getContentSize().width+SY(10),
			rateLabel:getPosition().y-pWinSize.height*0.0375-detailLabel:getContentSize().height))
	end;
	
	--离开按钮
	local exitBtn=ZyButton:new("button/panle_1014_3.png",nil,nil, nil,FONT_NAME,FONT_SM_SIZE)
	exitBtn:setAnchorPoint(PT(0,0))
	exitBtn:setPosition(PT(detailBg:getPosition().x+pWinSize.width*0.5-exitBtn:getContentSize().width-SY(15),
		detailBg:getPosition().y+pWinSize.height*0.6-exitBtn:getContentSize().height-SY(10)))
	exitBtn:registerScriptTapHandler(PersonalFileScene.exitDetail)
	exitBtn:addto(achievementLayer)
end;

function exitDetail()
	if  achievementLayer then
		mScene:removeChild(achievementLayer,true)
	end;
end

--背包
function showBag()
	
	local listSize = SZ(infoBgSize.width, pWinSize.height*0.69)
	local list_x=tabBar:getPosition().x-SY(25)
	local list_y=infoBgImg:getPosition().y+SY(13)
	local listRowH=listSize.height/2
	
	bagList = ScutCxList:node(listRowH, ccc4(24, 24, 24, 0), listSize)
	bagList:setAnchorPoint(PT(0, 0))
	bagList:setPosition(PT(list_x, list_y))
	layer_1:addChild(bagList,0)
	
	local layout=CxLayout()
	layout.val_x.t = ABS_WITH_PIXEL
	layout.val_y.t = ABS_WITH_PIXEL
	layout.val_x.val.pixel_val =0
	layout.val_y.val.pixel_val =0
	
	if #m_bagTable==0 then
		local toShopText=CCLabelTTF:create(Language.PERSONAL_TOSHOP,FONT_NAME,FONT_SM_SIZE);
		toShopText:setAnchorPoint(PT(0.5,0.5))
		toShopText:setColor(ccRED1)
		toShopText:setPosition(PT(pWinSize.width/1.8,pWinSize.height/2.2))
		layer_1:addChild(toShopText)
		local shopStr=string.format("<label class='PersonalFileScene.toShop'  >%s</label>",Language.PERSONAL_SHOP )
		local shopLabel=ZyMultiLabel:new(shopStr,pWinSize.width*0.9,FONT_NAME,FONT_SM_SIZE,nil,nil,true);
		shopLabel:setPosition(PT(toShopText:getPosition().x+toShopText:getContentSize().width/2,
							pWinSize.height/2.2-shopLabel:getContentSize().height/2))
		shopLabel:addto(layer_1,1)
		
		local buyText=CCLabelTTF:create(Language.PERSONAL_BUY,FONT_NAME,FONT_SM_SIZE);
		buyText:setAnchorPoint(PT(0,0.5))
		buyText:setColor(ccRED1)
		buyText:setPosition(PT(toShopText:getPosition().x+toShopText:getContentSize().width/2+SY(32),pWinSize.height/2.2))
		layer_1:addChild(buyText)
	end;	
	
	local length=#m_bagTable
	local row=nil
	local col=4
	if length%4==0 then
		row = math.floor(length/4)
	else
		row=math.floor(length/4)+1
	end;
	
	for index=1,row do
		local listItem = ScutCxListItem:itemWithColor(ccc3(32, 24, 3))
		listItem:setOpacity(0)
		
		if index==row then
			if length%4==0 then
				col=4
			else	
				col=length%4
			end;
		else
			col=4
		end;
		
		for  k =1,col do
			local i=k+(index-1)*4
			local layer=CCLayer:create()
			local headStr=string.format("headImg/%s.png",m_bagTable[i].HeadID)
			--物品背景
			local headBgImg=ZyButton:new("common/panle_1014_2.png",nil,nil, nil,FONT_NAME,FONT_SM_SIZE)
--			headBgImg:setScaleX(pWinSize.width*0.1/headBgImg:getContentSize().width)
--			headBgImg:setScaleY(pWinSize.height*0.13/headBgImg:getContentSize().height)
	  		headBgImg:setAnchorPoint(PT(0,0))
	  		local headColW=(k-1)%col*(pWinSize.width*0.1)*1.65
			headBgImg:setPosition(PT(SY(25)+headColW,0))
			headBgImg:registerScriptTapHandler(PersonalFileScene.toChange)
			headBgImg:setTag(i)
			headBgImg:addto(layer)
			
			--物品
			local headImg= CCSprite:create(P(headStr));
--			headImg:setScaleX(pWinSize.width*0.09/headImg:getContentSize().width)
--			headImg:setScaleY(pWinSize.height*0.12/headImg:getContentSize().height)
	  		headImg:setAnchorPoint(PT(0,0))
			headImg:setPosition(PT(headBgImg:getPosition().x+SY(7),
				headBgImg:getPosition().y+SY(7)))
	  		layer:addChild(headImg,0)
			
			listItem:addChildItem(layer, layout)
			bagList:setRowHeight(pWinSize.height*0.2+SY(10))
		end
		bagList:addListItem(listItem, false)
	end;
end;

function toShop()
	ShopScene.createScene()
	shoptype=1
end;

function toChange(index)
	local tag=index
	--[[
	if index then
		tag=index:getTag()
	end;
	]]
	local box = ZyMessageBoxEx:new()
	box:setTag(tag)
	box:doQuery(mScene, nil,Language.PERSONAL_CHANGE , Language.IDS_SURE, Language.IDS_CANCEL,makeSellConfirm) 
end;

function makeSellConfirm(clickedButtonIndex,content,tag)
	if clickedButtonIndex == 1 then--确认
		newhead=m_bagTable[tag].HeadID
		actionLayer.Action1010(mScene,nil ,newhead)
	end
end;

function toWriteIn(index)
	local tag=index
	--[[
	if index ~=nil then
		tag=index:getTag()
	end;
	]]
	if tag==1  then
		m_nameEdit:setVisible(true)
		m_hobbyEdit:setVisible(false)
		m_professionEdit:setVisible(false)
		m_yearEdit:setVisible(false)
		m_monthEdit:setVisible(false)
		m_dateEdit:setVisible(false)
		m_nameEdit:setText(m_socialInfo.Name)
		nameText:setString("")
		
		if m_hobbyEdit:GetEditText()~=nil and m_hobbyEdit:GetEditText()~="" then
			m_socialInfo.Hobby=m_hobbyEdit:GetEditText()
			hobbyText:setString(m_socialInfo.Hobby)
		end;
		if m_professionEdit:GetEditText()~=nil and m_professionEdit:GetEditText()~="" then
			m_socialInfo.Profession=m_professionEdit:GetEditText()
			professionText:setString(m_socialInfo.Profession)
		end;
		if m_yearEdit:GetEditText()~=nil and m_yearEdit:GetEditText()~="" then
			yearText:setString(m_yearEdit:GetEditText())
		end;
		if m_monthEdit:GetEditText()~=nil and m_monthEdit:GetEditText()~="" then
			monthText:setString(m_monthEdit:GetEditText())
		end;
		if m_dateEdit:GetEditText()~=nil and m_dateEdit:GetEditText()~="" then
			dayText:setString(m_dateEdit:GetEditText())
		end;
		m_socialInfo.Birthday=m_yearEdit:GetEditText().."-"..m_monthEdit:GetEditText().. "-".. m_dateEdit:GetEditText()
	elseif tag==3 then
		m_nameEdit:setVisible(false)
		m_hobbyEdit:setVisible(false)
		m_professionEdit:setVisible(false)
		m_yearEdit:setVisible(true)
		m_monthEdit:setVisible(true)
		m_dateEdit:setVisible(true)
		local year,month,day=yearMonthDay(m_socialInfo.Birthday)
		
		m_yearEdit:setText(year)
		m_monthEdit:setText(month)
		m_dateEdit:setText(day)
		
		yearText:setString("")
		monthText:setString("")
		dayText:setString("")
		
		if m_nameEdit:GetEditText()~=nil and m_nameEdit:GetEditText()~="" then
			m_socialInfo.Name=m_nameEdit:GetEditText()
			nameText:setString(m_socialInfo.Name)
		end;
		if m_hobbyEdit:GetEditText()~=nil and m_hobbyEdit:GetEditText()~="" then
			m_socialInfo.Hobby=m_hobbyEdit:GetEditText()
			hobbyText:setString(m_socialInfo.Hobby)
		end;
		if m_professionEdit:GetEditText()~=nil and m_professionEdit:GetEditText()~="" then
			m_socialInfo.Profession=m_professionEdit:GetEditText()
			professionText:setString(m_socialInfo.Profession)
		end;
	elseif tag==4 then
		m_nameEdit:setVisible(false)
		m_hobbyEdit:setVisible(true)
		m_professionEdit:setVisible(false)
		m_yearEdit:setVisible(false)
		m_monthEdit:setVisible(false)
		m_dateEdit:setVisible(false)
		m_hobbyEdit:setText(m_socialInfo.Hobby)
		hobbyText:setString("")
		
		if m_nameEdit:GetEditText()~=nil and m_nameEdit:GetEditText()~="" then
			m_socialInfo.Name=m_nameEdit:GetEditText()
			nameText:setString(m_socialInfo.Name)
		end;
		if m_professionEdit:GetEditText()~=nil and m_professionEdit:GetEditText()~="" then
			m_socialInfo.Profession=m_professionEdit:GetEditText()
			professionText:setString(m_socialInfo.Profession)
		end;
		if m_yearEdit:GetEditText()~=nil and m_yearEdit:GetEditText()~="" then
			yearText:setString(m_yearEdit:GetEditText())
		end;
		if m_monthEdit:GetEditText()~=nil and m_monthEdit:GetEditText()~="" then
			monthText:setString(m_monthEdit:GetEditText())
		end;
		if m_dateEdit:GetEditText()~=nil and m_dateEdit:GetEditText()~="" then
			dayText:setString(m_dateEdit:GetEditText())
		end;
		m_socialInfo.Birthday=m_yearEdit:GetEditText().."-"..m_monthEdit:GetEditText().. "-".. m_dateEdit:GetEditText()
	elseif tag==5 then
		m_nameEdit:setVisible(false)
		m_hobbyEdit:setVisible(false)
		m_professionEdit:setVisible(true)
		m_yearEdit:setVisible(false)
		m_monthEdit:setVisible(false)
		m_dateEdit:setVisible(false)
		
		m_professionEdit:setText(m_socialInfo.Profession)
		professionText:setString("")
		
		if m_nameEdit:GetEditText()~=nil and m_nameEdit:GetEditText()~="" then
			m_socialInfo.Name=m_nameEdit:GetEditText()
			nameText:setString(m_socialInfo.Name)
		end;
		if m_hobbyEdit:GetEditText()~=nil and m_hobbyEdit:GetEditText()~="" then
			m_socialInfo.Hobby=m_hobbyEdit:GetEditText()
			hobbyText:setString(m_socialInfo.Hobby)
		end;
		if m_yearEdit:GetEditText()~=nil and m_yearEdit:GetEditText()~="" then
			yearText:setString(m_yearEdit:GetEditText())
		end;
		if m_monthEdit:GetEditText()~=nil and m_monthEdit:GetEditText()~="" then
			monthText:setString(m_monthEdit:GetEditText())
		end;
		if m_dateEdit:GetEditText()~=nil and m_dateEdit:GetEditText()~="" then
			dayText:setString(m_dateEdit:GetEditText())
		end;
		m_socialInfo.Birthday=m_yearEdit:GetEditText().."-"..m_monthEdit:GetEditText().. "-".. m_dateEdit:GetEditText()
	else
		m_nameEdit:setVisible(false)
		m_hobbyEdit:setVisible(false)
		m_professionEdit:setVisible(false)
		m_yearEdit:setVisible(false)
		m_monthEdit:setVisible(false)
		m_dateEdit:setVisible(false)
		
		if m_nameEdit:GetEditText()~=nil and m_nameEdit:GetEditText()~="" then
			m_socialInfo.Name=m_nameEdit:GetEditText()
			nameText:setString(m_socialInfo.Name)
		end;
		if m_hobbyEdit:GetEditText()~=nil and m_hobbyEdit:GetEditText()~="" then
			m_socialInfo.Hobby=m_hobbyEdit:GetEditText()
			hobbyText:setString(m_socialInfo.Hobby)
		end;
		if m_professionEdit:GetEditText()~=nil and m_professionEdit:GetEditText()~="" then
			m_socialInfo.Profession=m_professionEdit:GetEditText()
			professionText:setString(m_socialInfo.Profession)
		end;
		if m_yearEdit:GetEditText()~=nil and m_yearEdit:GetEditText()~="" then
			yearText:setString(m_yearEdit:GetEditText())
		end;
		if m_monthEdit:GetEditText()~=nil and m_monthEdit:GetEditText()~="" then
			monthText:setString(m_monthEdit:GetEditText())
		end;
		if m_dateEdit:GetEditText()~=nil and m_dateEdit:GetEditText()~="" then
			dayText:setString(m_dateEdit:GetEditText())
		end;
		m_socialInfo.Birthday=m_yearEdit:GetEditText().."-"..m_monthEdit:GetEditText().. "-".. m_dateEdit:GetEditText()
	end;
end;

--充值
function topUp()
	TopUpScene.init()
end;

--保存按钮
function save()
	local name=m_nameEdit:GetEditText()
	local hobby=m_hobbyEdit:GetEditText()
	local profession=m_professionEdit:GetEditText()
	
	local year=m_yearEdit:GetEditText()
	local month=m_monthEdit:GetEditText()
	local day=m_dateEdit:GetEditText()
	local birthday=year.."-"..month.."-"..day
	local sex=nil
	if girlBtn._isSelected then
		sex=1
	elseif manBtn._isSelected then
		sex=0
	end;
	
	toWriteIn()
	
	actionLayer.Action1011(mScene,nil ,name,sex,birthday,profession,hobby)
end;
--Tab
function createTabBar(tabName,tabNum)
	if tabBar then
		tabBar:remove()
	end;
	tabBar = ZyTabBar:new(Image.image_shop,Image.image_shop_1,tabName, FONT_NAME, FONT_SM_SIZE,tabNum);
	tabBar:addto(mLayer)
	tabBar:setCallbackFun(callbackTabBar);  --点击响应的事件
	tabBar:selectItem(mCurrentTab);           --点击哪个按钮
	tabBar:setPosition(PT(leftBg:getPosition().x+pWinSize.width*0.25+SY(28),titleImage:getPosition().y-SY(5)))
end
--tabbar
function callbackTabBar(bar,pNode,tag)
	local index=pNode:getTag();
	if index ~=mCurrentTab then
		mCurrentTab=index;
		showLayer(mCurrentTab);
	end
end

function showLayer(index)
	if index then
	    mCurrentTab =index
	end
	if  layer_1 then
		layer_1:getParent():removeChild(layer_1, true)
		layer_1 = nil
	end;
	
	layer_1= CCLayer:create()
	mScene:addChild(layer_1, 0)
	
	if  m_nameEdit ~=nil and m_professionEdit ~=nil and  m_hobbyEdit ~=nil then
		m_nameEdit:setVisible(false)
		m_professionEdit:setVisible(false)
		m_hobbyEdit:setVisible(false)
		m_yearEdit:setVisible(false)
		m_monthEdit:setVisible(false)
		m_dateEdit:setVisible(false)
	end;
	
	if mCurrentTab==1  then
		if personalFileBtn._isSelected then
			basicInfo()
		elseif achievementBtn._isSelected then
			--showAchievement()
			actionLayer.Action3001(mScene,nil,0,1,100)
		else
			showBag()
		end;
	elseif mCurrentTab==2 then
		actionLayer.Action1009(mScene,nil)
	elseif mCurrentTab==3 then
		gameInfo()
	end
end

function changeBtnImg()
    if changePwdBtn then
		changePwdBtn:addImage("title/panle_1048.png")
	end
end

function onEnter()
	actionLayer.Action1008(mScene,nil)
	if shoptype==1 then
    	actionLayer.Action1013(mScene,nil ,1,100)
    	shoptype=0
    end
end;
-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID();
	local userData = ZyRequestParam:getParamData(lpExternalData)
	 if actionId ==1008 then
     	 	local personalInfo=actionLayer._1008Callback(pScutScene, lpExternalData)
  	 	if personalInfo~=nil then
  	 		PersonalInfo.getPersonalInfo()._Wings=nil
  	 		PersonalInfo.setPersonalInfo(personalInfo)
  	 		m_personInfo=PersonalInfo.getPersonalInfo()
  	 		
			if m_goldText then
			local myGold=string.format(Language.PersonalFile_GOLD,m_personInfo._Gold)
			m_goldText:setString(myGold)
			local pos = m_rechargeBtn:getPosition()
			m_rechargeBtn:setPosition(PT(m_goldText:getPosition().x+m_goldText:getContentSize().width+SY(20),
			pos.y))
			end
			
			
  	 	end
	elseif actionId == 1009 then
		m_socialInfo =actionLayer._1009Callback(pScutScene, lpExternalData)
		if m_socialInfo~=nil then
			
			showSocialInfo()
		else
			ZyToast.show(mScene, ZyReader:readErrorMsg(), 1, 0.35)
		end
	elseif actionId == 1010 then
		if ZyReader:getResult() == eScutNetSuccess then
			
			local box = ZyMessageBoxEx:new()
			box:doPrompt(mScene, nil,Language.PERSONAL_SUCCESS,Language.IDS_OK,refreshWin)	
			PersonalInfo.getPersonalInfo()._HeadIcon=newhead
			
	
			--头像
			local Head=string.format("headImg/%s.png",newhead)
			local myHead= CCSprite:create(P(Head));
			myHead:setAnchorPoint(PT(0.5,0.5))
			local pos=mMyHead:getPosition()
			myHead:setPosition(pos)			
			mMyHead:getParent():addChild(myHead,0)
			mMyHead:getParent():removeChild(mMyHead,true)
			mMyHead=myHead
			
		else
			ZyToast.show(mScene, ZyReader:readErrorMsg(), 1, 0.35)
		end
	elseif actionId == 1011 then
		if ZyReader:getResult() == eScutNetSuccess then
			
			ZyToast.show(mScene, Language.PERSONAL_BAOCUN, 1, 0.35)--修改成功
		else
			ZyToast.show(mScene, ZyReader:readErrorMsg(), 1, 0.35)
		end
	elseif actionId == 1013 then
		m_bagInfo =actionLayer._1013Callback(pScutScene, lpExternalData)
		if m_bagInfo~=nil then
			m_bagTable=m_bagInfo.RecordTabel
			if  layer_1 then
				layer_1:getParent():removeChild(layer_1, true)
				layer_1 = nil
			end;
			
			layer_1= CCLayer:create()
			mScene:addChild(layer_1, 0)

			
			local tabName={"title/panle_1041.png"}
			if not mCurrentTab or mCurrentTab>#tabName then
			    mCurrentTab=1;
			end
			createTabBar(tabName,1);--创建标题按钮
			showLayer(mCurrentTab)
			

		else
			ZyToast.show(mScene, ZyReader:readErrorMsg(), 1, 0.35)
		end
	elseif actionId == 3001 then
		m_achievementInfo =actionLayer._3001Callback(pScutScene, lpExternalData)
		if m_achievementInfo~=nil then
			achievementTable=m_achievementInfo.RecordTabel
			showAchievement()
		else
			ZyToast.show(mScene, ZyReader:readErrorMsg(), 1, 0.35)
		end
	elseif actionId == 3002 then
		m_achievementDetailInfo =actionLayer._3002Callback(pScutScene, lpExternalData)
		if m_achievementDetailInfo~=nil then
			showDetail()
		else
			ZyToast.show(mScene, ZyReader:readErrorMsg(), 1, 0.35)
		end
	end
end