------------------------------------------------------------------
-- AddSpriteLayer.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 精力购买界面
------------------------------------------------------------------

module("AddSpriteLayer", package.seeall)


mScene = nil 		-- 场景

local kaogu=nil

function setInfo(scene)
	mScene = scene

end

----------精力不足
function  releaseMenergyLayer()
	if mEnergyLayer then
		mEnergyLayer:getParent():removeChild(mEnergyLayer,true)
		mEnergyLayer=nil
	end
end;

function releaseResource()
mEnergyLayer=nil

end;
--

function createEnergyLayer(serverInfo,type)
	releaseMenergyLayer()
	kaogu=type
	local layer=CCLayer:create()
	mEnergyLayer=layer
	mScene:addChild(layer,3)
	local bgSprite=CCSprite:create(P("common/list_1024.png"))
	bgSprite:setAnchorPoint(PT(0.5,0))
	local boxSize=SZ(pWinSize.width,pWinSize.height*0.855)
	bgSprite:setPosition(PT(pWinSize.width/2,pWinSize.height-boxSize.height))
	bgSprite:setScaleX(boxSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(boxSize.height/bgSprite:getContentSize().height)
	layer:addChild(bgSprite,0)
	--
	local sprite=CCSprite:create(P("mainUI/list_1014.png"))
	for k=1 , 2 do
		local actionBtn=UIHelper.createActionRect(boxSize)
		actionBtn:setPosition(PT(0,bgSprite:getPosition().y))
		layer:addChild(actionBtn,0)	
	end
	
	local titleSprite=CCSprite:create(P("title/list_1155.png"))
	titleSprite:setAnchorPoint(PT(0.5,0))
	titleSprite:setPosition(PT(pWinSize.width/2,pWinSize.height*0.96-titleSprite:getContentSize().height))
	layer:addChild(titleSprite,0)	
	
	local startY=titleSprite:getPosition().y-SY(5)
	
	--
	local boxSize=SZ(pWinSize.width*0.9,pWinSize.height*0.68)
	local blackSprite=CCSprite:create(P("common/list_1038.9.png"))
	blackSprite:setScaleX(boxSize.width/blackSprite:getContentSize().width)
	blackSprite:setScaleY(boxSize.height/blackSprite:getContentSize().height)
	blackSprite:setAnchorPoint(PT(0.5,0))
	blackSprite:setPosition(PT(pWinSize.width/2,startY-boxSize.height))
	layer:addChild(blackSprite,0)
	
	
	local headSprite=CCSprite:create(P("mainUI/list_1154.png"))
	headSprite:setAnchorPoint(PT(0,0))
	headSprite:setPosition(PT(pWinSize.width*0.08,startY-headSprite:getContentSize().height-SY(5)))
	layer:addChild(headSprite,0)	
	
	local talkBox=SZ(boxSize.width*0.9-headSprite:getContentSize().width,headSprite:getContentSize().height)
	local talkSprite=CCSprite:create(P("common/list_1165.9.png"))
	talkSprite:setScaleX(talkBox.width/talkSprite:getContentSize().width)
	talkSprite:setScaleY(talkBox.height/talkSprite:getContentSize().height)
	talkSprite:setAnchorPoint(PT(0,0))
	talkSprite:setPosition(PT(headSprite:getPosition().x+headSprite:getContentSize().width+SX(2),
								headSprite:getPosition().y))
	layer:addChild(talkSprite,0)
	
	local tipStr=string.format("<label>%s</label>",Language.PLOT_TIP3)
	local tipLabel=ZyMultiLabel:new(tipStr,talkBox.width*0.8,FONT_NAME,FONT_SM_SIZE);
	tipLabel:setPosition(PT(talkSprite:getPosition().x+talkBox.width*0.15,
								talkSprite:getPosition().y+talkBox.height*0.9-tipLabel:getContentSize().height))
	tipLabel:addto(layer,0)
	
	
	--底下的部分
	startY=headSprite:getPosition().y-SY(5)
	local midBg=CCSprite:create(P("common/list_1052.9.png"))
	local midBox=SZ(midBg:getContentSize().width,pWinSize.height*0.2)
	midBg:setScaleY(midBox.height/midBg:getContentSize().height)
	midBg:setAnchorPoint(PT(0.5,0))
	midBg:setPosition(PT(pWinSize.width/2,startY-midBox.height))
	layer:addChild(midBg,0)
	local startX=midBg:getPosition().x-midBox.width*0.45
	local headBg=CCSprite:create(P(Image.Image_normalItemBg))
	headBg:setAnchorPoint(PT(0,0.5))
	headBg:setPosition(PT(startX,midBg:getPosition().y+midBox.height/2))
	layer:addChild(headBg,0)
	local headSprite=CCSprite:create(P("smallitem/icon_4094.png"))
	headSprite:setAnchorPoint(PT(0.5,0.5))
	headSprite:setPosition(PT(headBg:getContentSize().width/2,
							headBg:getContentSize().height/2))
	headBg:addChild(headSprite,0)
	
	local nameLabel=CCLabelTTF:create(Language.PLOT_TIP6,FONT_NAME,FONT_SM_SIZE)
	nameLabel:setAnchorPoint(PT(0,1))
	nameLabel:setPosition(PT(headBg:getPosition().x+headBg:getContentSize().width,
							headBg:getPosition().y+headBg:getContentSize().height*0.4))
	layer:addChild(nameLabel,0)
	
	
	--已用几瓶  可用几瓶
	local labelWidth = pWinSize.width-startX-nameLabel:getPosition().x
	local str=string.format("<label>%s</label>", string.format(Language.PLOT_TIP7,serverInfo.CurrDayUseNum,serverInfo.CurrDayUseMaxNum))
	local xmlLabel=ZyMultiLabel:new(str,labelWidth,FONT_NAME,FONT_SM_SIZE);
	xmlLabel:setPosition(PT(nameLabel:getPosition().x,
							nameLabel:getPosition().y-SY(2)-nameLabel:getContentSize().height-xmlLabel:getContentSize().height))
	xmlLabel:addto(layer,0)
	
	
	
	startY=midBg:getPosition().y-SY(5)
	
	local competiveBtn=ZyButton:new("button/list_1023.png",nil,nil,Language.PLOT_TIP4)
	competiveBtn:setAnchorPoint(PT(0.5,0))
	competiveBtn:setPosition(PT(pWinSize.width/4,startY-SY(5)-competiveBtn:getContentSize().height))
	competiveBtn:addto(layer,0)
	competiveBtn:setVisible(false)
	competiveBtn:registerScriptHandler(competiveAction)
	
	local recoverBtn=ZyButton:new("button/list_1023.png",nil,nil,Language.PLOT_TIP5)
	recoverBtn:setAnchorPoint(PT(0.5,0))
	recoverBtn:setPosition(PT(pWinSize.width/4*3,competiveBtn:getPosition().y))
	recoverBtn:addto(layer,0)
	recoverBtn:registerScriptHandler(recoverAction)
	
end;

function recoverAction()
	actionLayer.Action1010(mScene,nil,1,1)
	releaseMenergyLayer()
end;

function competiveAction()

end;


---------------------------------------购买精力
function askBugAction(clickedButtonIndex,content,tag) 
        if clickedButtonIndex==ID_MBOK then
           	actionLayer.Action1010(mScene,nil,1,2)
        end
end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if actionId == 1010 then
		if ZyReader:getResult() == eScutNetSuccess or ZyReader:getResult() == 2 then
			MainMenuLayer.refreshWin()
			if kaogu==1 then
				RelicArchaeology.refreshgou()
				kaogu=0
			end
			ZyToast.show(pScutScene,Language.SHOP_BUYSUCCESS,1,0.3)
		elseif ZyReader:getResult() == 1 then
			local box = ZyMessageBoxEx:new()
			box:doQuery(pScutScene, nil, ZyReader:readErrorMsg(),Language.IDS_SURE  ,Language.IDS_CANCEL,askBugAction)
		else
			ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.3)
		end 
	end
end





