	
module("TestScene", package.seeall)
	

local mScene
local mLayer

function init()
	local scene = ScutScene:new()
	mLayer = CCLayer:create()
	mScene = scene.root 
	mScene:addChild(mLayer,0)
	scene:registerCallback(netCallBack)
	local runningScene = CCDirector:sharedDirector():getRunningScene()
	if runningScene == nil then
		CCDirector:sharedDirector():runWithScene(mScene)
	else
		CCDirector:sharedDirector():replaceScene(mScene)
	end
end



function action1004()
	ZyWriter:writeString("ActionId",1004)
	ZyWriter:writeString("MobileType",2)
	ZyWriter:writeString("Pid","z10041")
	ZyWriter:writeString("Pwd",ScutDataLogic.CFileHelper:encryptPwd("123456", nil):getCString())
	ZyWriter:writeString("DeviceID","234e4r6dfg")
	ZyWriter:writeString("GameType",5)
	ZyWriter:writeString("ServerID",3)
	ZyWriter:writeString("ScreenX",100)
	ZyWriter:writeString("ScreenY",100)
	ZyWriter:writeString("RetailID","0000")
	ZyExecRequest(mScene, nil,true,1)
end





-------ÍøÂç»Øµ÷
function netCallBack(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID();
	if actionId==1004 then
		if ZyReader:getResult() == eScutNetSuccess then
			ZyToast.show(mScene,"Login Sucess!")
		else
			ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)
		end
	else
		ZyToast.show(mScene,"Action".. actionId)
	end
end



--return button and LableText
function Button(normalPic, downPic, listtener, strText, TextAlign, fontSize, color, tag, bCheck, disablePic)

	menuItem = MenuItem(normalPic, downPic, listtener, strText, TextAlign, fontSize, color, bCheck, disablePic)
	if tag ~= nil then
		menuItem:setTag(tag)
	end

	local Btn = CCMenu:menuWithItem(menuItem)
	Btn:setContentSize(menuItem:getContentSize())
	return Btn, menuItem
end


function MenuItem(normalPic, downPic, listtener, strText, TextAlign, fontSize, color, bCheck, disablePic)
	local strNor     = nil
	local strDown    = nil
	local strDisable = nil
	if normalPic ~= nil then
		strNor = P(normalPic)
	end

	if downPic ~= nil then
		strDown = P(downPic)
	end

	if disablePic ~= nil then
		strDisable = P(disablePic)
	end

    local menuItem
	if disablePic == nil then
		menuItem = CCMenuItemImage:itemFromNormalImage(strNor, strDown)
	else
		menuItem = CCMenuItemImage:itemFromNormalImage(strNor, strDown, strDisable)
	end
	
	if bCheck then
		menuItem:selected()
	else
		menuItem:unselected()
	end

	local pLable = nil
	if strText ~= nil then
		pLable  = CCLabelTTF:create(strText, FONT_NAME, (fontSize))
	end

    local szMenu = menuItem:getContentSize()


	if listtener ~= nil then
		menuItem:registerScriptTapHandler(listtener)
	end
	menuItem:setAnchorPoint(CCPoint(0, 0))


	--TextAlign--
	if pLable ~= nil then
		if TextAlign == 0 then  --AlignLeft
			pLable:setAnchorPoint(CCPoint(0, 0.5))
			pLable:setPosition(CCPoint(0, szMenu.height/2))
		elseif TextAlign == 1 then --AlignCenter
			pLable:setPosition(CCPoint(szMenu.width/2, szMenu.height/2))
		else --ALignRight
		   pLable:setAnchorPoint(CCPoint(1, 0.5))
		   pLable:setPosition(CCPoint(szMenu.width/2, szMenu.height/2))
		end
		if color ~= nil then
			pLable:setColor(color)
		end
		menuItem:addChild(pLable, 0, 0)
	end
	return menuItem, pLable
end
