------------------------------------------------------------------
-- LvUpNoticeLayer.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 
------------------------------------------------------------------

module("LvUpNoticeLayer", package.seeall)


mScene = nil 		-- 场景

function setVisible(value)
	if lvUpLayer then
		lvUpLayer:setVisible(value)
	end
end

function setOpenTable(info)
	_OpenFuncTabel = info
end;

function releaseResource()
	mInfo = nil
	isGotoLv = false
	lvUpLayer = nil
end

function releaseLvUplayer()
	if lvUpLayer then
		lvUpLayer:getParent():removeChild(lvUpLayer, true)
		lvUpLayer = nil
	end
	releaseResource()

	
	if _OpenFuncTabel and #_OpenFuncTabel > 0 then
		guideOpenNewFunc()
	else
		MainMenuLayer.setIsGotoLv(false)
		MainMenuLayer.refreshWin()
		GuideLayer.show()
		BattleResult.startGuide()
		_OpenFuncTabel=nil		
	end
end

function setInfo(scene, layer , info)
	mScene = scene
	mInfo = info
	mLayer = layer
end


function lvUpNotice()
	if not mInfo then
		return
	end
	if lvUpLayer then
		lvUpLayer:getParent():removeChild(lvUpLayer, true)
		lvUpLayer = nil
	end

	

	local layer = CCLayer:create()
	mScene:addChild(layer, 15)
	
	lvUpLayer = layer
	

	--屏蔽按钮ceng
	local unTouchBtn =  ZyButton:new("common/transparentBg.png", "common/transparentBg.png", "common/transparentBg.png")
	unTouchBtn:setScaleX(pWinSize.width/unTouchBtn:getContentSize().width)
	unTouchBtn:setScaleY(pWinSize.height/unTouchBtn:getContentSize().height)
	unTouchBtn:setAnchorPoint(PT(0,0))
	unTouchBtn:setPosition(PT(0,0))
	unTouchBtn:addto(layer, 0)		
	
	
	local boxSize = SZ(pWinSize.width*0.75, pWinSize.height*0.35)
	local imageBg = CCSprite:create(P("common/list_1187.png"))
--	imageBg:setScaleX(boxSize.width/imageBg:getContentSize().width)
--	imageBg:setScaleY(boxSize.height/imageBg:getContentSize().height)
	boxSize = SZ(imageBg:getContentSize().width, imageBg:getContentSize().height)
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(pWinSize.width*0.5-boxSize.width*0.5, pWinSize.height*0.35))
	layer:addChild(imageBg, 0)
	
	
	
	local titlel = CCLabelTTF:create(Language.HERO_GOODGET, FONT_NAME, FONT_BIG_SIZE)
	titlel:setAnchorPoint(PT(0.5, 1))
	titlel:setPosition(PT(boxSize.width*0.5, boxSize.height*0.95))
	imageBg:addChild(titlel, 0)
	
	

	local _personalInfo = PersonalInfo.getPersonalInfo()
	local pos_x = pWinSize.width*0.1
	local pos_y = boxSize.height*0.6
	--等级
	local levelLabel = CCLabelTTF:create(Language.HERO_CURRLV..": ".._personalInfo._UserLv, FONT_NAME, FONT_SM_SIZE)
	levelLabel:setAnchorPoint(PT(0,0))
	levelLabel:setPosition(PT(pos_x,pos_y))
	imageBg:addChild(levelLabel, 0)
	pos_y = pos_y-levelLabel:getContentSize().height*1.5
      
      --上阵佣兵
	local numLabel = CCLabelTTF:create(Language.HERO_GROUPNUM..": ".._personalInfo._BattleNum.."/".._personalInfo._TotalBattle, FONT_NAME, FONT_SM_SIZE)
	numLabel:setAnchorPoint(PT(0,0))
	numLabel:setPosition(PT(pos_x,pos_y))
	imageBg:addChild(numLabel, 0)

       
       --晶石
       local item,itemSize = imageMoney("mainUI/list_1006.png", mInfo.GoldNum)
	pos_y =  pos_y-itemSize.height-numLabel:getContentSize().height*0.5
	local numLabel = CCLabelTTF:create(Language.HERO_GOLDPRICE..": ", FONT_NAME, FONT_SM_SIZE)
	numLabel:setAnchorPoint(PT(0,0))
	numLabel:setPosition(PT(pos_x,pos_y+itemSize.height*0.5-numLabel:getContentSize().height*0.5))
	imageBg:addChild(numLabel, 0)
	pos_x = pos_x+numLabel:getContentSize().width
	
	item:setPosition(PT(pos_x, pos_y))
	imageBg:addChild(item, 0)
		



	--确认按钮
	local topBtn=ZyButton:new(Image.image_button,nil,nil,Language.IDS_SURE)
	topBtn:registerScriptHandler(releaseLvUplayer)
	topBtn:setAnchorPoint(PT(0,0))
	topBtn:setPosition(PT(boxSize.width*0.5-topBtn:getContentSize().width*0.5, boxSize.height*0.05))	
	topBtn:addto(imageBg, 0)	
	
	

end

--图片做单位  + 价格
function imageMoney(image, num)

	local layer = CCLayer:create()
		local sprite = CCSprite:create(P(image));
		sprite:setAnchorPoint(PT(0,0))
		sprite:setPosition(PT(0,0))
		layer:addChild(sprite, 0)

		local numLabel = CCLabelTTF:create(num or "", FONT_NAME, FONT_SM_SIZE)
		numLabel:setAnchorPoint(PT(0,0))
		numLabel:setPosition(PT(sprite:getPosition().x+sprite:getContentSize().width+pWinSize.width*0.025, (sprite:getContentSize().height-numLabel:getContentSize().height)*0.5))
		layer:addChild(numLabel, 0)

		local itemSize = SZ(numLabel:getPosition().x+numLabel:getContentSize().width, sprite:getContentSize().height)

	return layer,itemSize
end



function closeGuide()
	closeTouchLayer()
	if guideLayer then
		guideLayer:getParent():removeChild(guideLayer, true)
		guideLayer = nil
	end
	MainMenuLayer.setIsGotoLv(false)
	MainMenuLayer.refreshWin()	
	GuideLayer.show()
	BattleResult.startGuide()
	_OpenFuncTabel=nil
end

function guideOpenNewFunc()
	if guideLayer then
		guideLayer:getParent():removeChild(guideLayer, true)
		guideLayer = nil
	end

	

	local layer = CCLayer:create()
	mScene:addChild(layer, 15)
	
	guideLayer = layer
	
	m_functionList = {}
	m_functionList[2]={titleName=Language.GUIDE_TITLE[2]}--魔法阵
	m_functionList[3]={titleName=Language.GUIDE_TITLE[3]}--魔术
	m_functionList[4]={titleName=Language.GUIDE_TITLE[4]}--强化
	m_functionList[9]={titleName=Language.GUIDE_TITLE[9]}--好友
	m_functionList[11]={titleName=Language.GUIDE_TITLE[11]}--竞技场
	m_functionList[12]={titleName=Language.GUIDE_TITLE[12]}--庄园种植经验树
	m_functionList[14]={titleName=Language.GUIDE_TITLE[14]}--日常活动
	m_functionList[15]={titleName=Language.GUIDE_TITLE[15]}--金矿洞
	m_functionList[16]={titleName=Language.GUIDE_TITLE[16]}--命运水晶
	m_functionList[19]={titleName=Language.GUIDE_TITLE[19]}--世界BOSS挑战
	m_functionList[21]={titleName=Language.GUIDE_TITLE[21]}--每日探险
	m_functionList[22]={titleName=Language.GUIDE_TITLE[22]}--庄园种植
	m_functionList[24]={titleName=Language.GUIDE_TITLE[24]}--圣吉塔挑战
	m_functionList[27]={titleName=Language.GUIDE_TITLE[27]}--背包
	m_functionList[37]={titleName=Language.GUIDE_TITLE[37]}--商城 
	m_functionList[38]={titleName=Language.GUIDE_TITLE[38]}--传承
	m_functionList[47]={titleName=Language.GUIDE_TITLE[47]}--佣兵
	m_functionList[48]={titleName=Language.GUIDE_TITLE[48]}--装备
	m_functionList[49]={titleName=Language.GUIDE_TITLE[49]}--魂技    
	m_functionList[50]={titleName=Language.GUIDE_TITLE[50]}--升级
	m_functionList[51]={titleName=Language.GUIDE_TITLE[51]}--副本    
	m_functionList[52]={titleName=Language.GUIDE_TITLE[52]}--菜单
	m_functionList[53]={titleName=Language.GUIDE_TITLE[53]}--信件
	m_functionList[54]={titleName=Language.GUIDE_TITLE[54]}--设置
	m_functionList[57]={titleName=Language.GUIDE_TITLE[57]}--圣吉塔
	m_functionList[58]={titleName=Language.GUIDE_TITLE[58]}--考古	
	m_functionList[59]={titleName=Language.GUIDE_TITLE[59]}--精英副本	

	
	local str = ""
	for k,v in ipairs(_OpenFuncTabel) do
		if m_functionList[v.FunEnum] then
			if str ~= "" then
				str = str..","
			end
			str = str..m_functionList[v.FunEnum].titleName

		end
	end
	if str ~= "" then
		local str = string.format(Language.GUIDE_OPENMESSAGE, str)
		
		creatTouchLayer()
		GuideLayer.createGuideNpc(guideLayer, nil, nil, nil, str)
	else
		closeGuide()
	end
end;

function closeTouchLayer()
	if touchLayer then
		touchLayer:getParent():removeChild(touchLayer, true)
		touchLayer = nil	
	end
end;

function creatTouchLayer(type, func)
	if touchLayer then
		touchLayer:getParent():removeChild(touchLayer, true)
		touchLayer = nil	
	end

	local layer = CCLayer:create()
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(0,0))
	mScene:addChild(layer, 15)

	
	
	touchLayer = layer
	
	local funcName = LvUpNoticeLayer.closeGuide
	
	local button=ZyButton:new(Image.image_transparent,Image.image_transparent)
	button:setScaleX(pWinSize.width/button:getContentSize().width)
	button:setScaleY(pWinSize.height/button:getContentSize().height)
	button:registerScriptHandler(funcName)
	button:setAnchorPoint(PT(0,0))
	button:setPosition(PT(0,0))
	button:addto(layer, 0)	
	
end

