------------------------------------------------------------------
-- HeroAccessory.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 佣兵附属界面
------------------------------------------------------------------

module("HeroAccessory", package.seeall)

local g_index = nil 
local shicitype=nil
function releaseResource()
	m_current_Info = nil
	dataLayer=nil
	g_index = nil 
	mAnimationLayer =nil
	mSkillDetailLayer =nil
	mDetailLayer = nil
	mStrenLayer=nil
end;


function setData(info)
	m_current_Info = info
end

----------------------------------------------
--佣兵属性信息
function  releaseDataLayer()
	if dataLayer~=nil then
--		dataLayer:getParent():removeChild(dataLayer,true)
--		dataLayer=nil
	end
end;

function  refreshPlayerDate(fatherLayer)
	releaseDataLayer()
	
	mLayer = fatherLayer
	

	dataLayer=CCLayer:create()
	dataLayer:setAnchorPoint(PT(0,0))
	dataLayer:setPosition(PT(0, pWinSize.height*0.225))	
	
	
	mLayer:addChild(dataLayer,0)
	
	
	--文字背景
	
	local labelBg = CCSprite:create(P("common/list_1052.9.png"))
	local boxSize=SZ(labelBg:getContentSize().width,pWinSize.height*0.15)
	labelBg:setScaleY(boxSize.height*1.1/labelBg:getContentSize().height)
	labelBg:setAnchorPoint(PT(0,0))
	labelBg:setPosition(PT(pWinSize.width*0.5-labelBg:getContentSize().width*0.5, 0))
	dataLayer:addChild(labelBg, 0)	
	
	
	local row=6
	local colW=labelBg:getContentSize().width*0.3
	local rowH=boxSize.height/row
	local posX=labelBg:getPosition().x+labelBg:getContentSize().width*0.085
	local posY=pWinSize.height*0.16
	local dateTable={Language.USERHP,"","",Language.LILIANG,Language.ZHILI,
	Language.HUILI,Language.WULIGONGJI,Language.FASHUGONGJI,
	Language.HUNJIGONGJI,Language.WULIFANGYU,Language.FASHUFANGYU,
	Language.HUNJIFANGYU,Language.BAOJI,Language.SHANBI,
	Language.GEDANG,Language.BISHA,Language.MINGZHONG,Language.POJI,
	}
	local numDate=nil
--[[	1生命回复   2	物理攻击     3	魂技攻击     4	魔法攻击     5	物理防御     6	魂技防御     7	魔法防御
    8	暴击    9	命中  10	破击         11	韧性         12	闪避         13	格挡         14	必杀         15	眩晕         16	昏睡         17	冰冻         18	迷失
  19	定身         20	中毒         21	出血         22	气势         23	混乱         24	绝对防御      25 个人先攻     26 力量      27魂力     28智力--]]

--	if playerSeverData~=nil then
	numDate={
	m_current_Info.LifeMaxNum,"","",
	m_current_Info.PowerNum,m_current_Info.IntellectNum,m_current_Info.SoulNum,
	getDataNum(2),getDataNum(4),getDataNum(3),
	getDataNum(5),getDataNum(7),getDataNum(6),
	percentStr(getDataNum(8)),percentStr(getDataNum(12)),percentStr(getDataNum(13)),
	percentStr(getDataNum(14)),percentStr(getDataNum(9)),percentStr(getDataNum(10)),
		}
	--end
	local row=3
	for k , v in ipairs(dateTable) do
			local str=v
			if numDate~=nil then
				str=v .. numDate[k]
			end
			local label=createLabel(str,dataLayer)
			if rowH==nil then
				rowH=label:getContentSize().height*1.2
			end
			label:setAnchorPoint(PT(0,0))
			local rowIndex=(k-1)%row
			local colIndex=math.floor((k-1)/row)+1
			label:setPosition(PT(posX+rowIndex*colW,posY-colIndex*rowH))
	end

end;

function  percentStr(num)
	if num~=0 then
		return num*100 ..  "%"
	end
	return num
end;

function  getDataNum(id)
	for k , v in pairs(m_current_Info.RecordTabel4) do
		if v.SkillID ==id then
			return v.SkillNum
		end
	end
	return 0
end;

function  createLabel(name,parent,color)
	local label=CCLabelTTF:create(name,FONT_NAME,FONT_SMM_SIZE)
	label:setAnchorPoint(PT(0,0))
	if color~=nil then
		label:setColor(color)
	end
	parent:addChild(label,0)
	return label
end;

---------------------------------------

--详情
function setScene(scene)
	mScene = scene
	if  scene.Scene then
		mScene = scene.Scene
		g_index = scene.index
	end
end


function  releaseEquipDetailLayer()
	if mDetailLayer then
		mDetailLayer:getParent():removeChild(mDetailLayer,true)
		mDetailLayer=nil
	end
	if GuideLayer.judgeIsGuide(8) then
		GuideLayer.close()
	end
end;

--创建详细层
function  createEquipDetailLayer(item)
	local index = nil 
	if  item ~= nil   then 
		index = item.index
		mScene = item.Scene
	end
	releaseEquipDetailLayer()
	local layer=CCLayer:create()
	mScene:addChild(layer,2)
	mDetailLayer=layer
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(0,0))
	
	
	local info = m_current_Info
	-----------------
-----------------
	local sprite=CCSprite:create(P("mainUI/list_1014.png"))
	local boxSize=SZ(pWinSize.width,pWinSize.height-sprite:getContentSize().height)

	for k=1 ,2 do
		local pingBiBtn=UIHelper.createActionRect(boxSize)
		pingBiBtn:setPosition(PT(0,pWinSize.height-boxSize.height))
		layer:addChild(pingBiBtn,0)
	end

	local bgSprite=CCSprite:create(P("common/list_1024.png"))
	bgSprite:setAnchorPoint(PT(0.5,0))
	bgSprite:setScaleX(pWinSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(pWinSize.height*0.855/bgSprite:getContentSize().height)
	bgSprite:setPosition(PT(pWinSize.width/2,pWinSize.height*0.145))
	layer:addChild(bgSprite,0)
	
	--标题
	local titleLabel=CCSprite:create(P("title/list_1101.png"))
	titleLabel:setAnchorPoint(PT(0.5,0))
	titleLabel:setPosition(PT(pWinSize.width/2,pWinSize.height*0.96-titleLabel:getContentSize().height))
	layer:addChild(titleLabel,0)
	
	local closeBtn=ZyButton:new("button/list_1046.png")
	closeBtn:setAnchorPoint(PT(0,0))
	closeBtn:setPosition(PT(pWinSize.width-closeBtn:getContentSize().width*1.2,
							pWinSize.height-closeBtn:getContentSize().height*1.2))
	closeBtn:addto(layer,0)
	closeBtn:registerScriptHandler(releaseEquipDetailLayer)
	local startY=closeBtn:getPosition().y-titleLabel:getContentSize().height
	

	--简介
	local sizeBox=SZ(boxSize.width*0.9,pWinSize.height*0.55)
	local contentBg=CCSprite:create(P("common/list_1052.9.png"))
	contentBg:setAnchorPoint(PT(0.5,0))
	contentBg:setScaleY(sizeBox.height/contentBg:getContentSize().height)
	contentBg:setPosition(PT(pWinSize.width/2,
								startY-sizeBox.height))
	layer:addChild(contentBg,0)
	local startX=pWinSize.width/2-contentBg:getContentSize().width*0.48
	
	---	物品图片
	local bgPic = getQualityBg(info.QualityType, 3) 
	local goodBg=CCSprite:create(P(bgPic))
	goodBg:setAnchorPoint(PT(0,0))
	goodBg:setPosition(PT(startX,
				contentBg:getPosition().y+sizeBox.height*0.9-goodBg:getContentSize().height))
	layer:addChild(goodBg,0)
	if info.MaxHeadID and info.MaxHeadID~= "" then
		local imgPath=string.format("bigitem/%s.png",info.MaxHeadID )
		local goodSprite=CCSprite:create(P(imgPath))
		goodSprite:setAnchorPoint(PT(0.5,0.5))
		goodSprite:setPosition(PT(goodBg:getContentSize().width/2,
										goodBg:getContentSize().height/2))
		goodBg:addChild(goodSprite,0)
	end
	
	---名称
	local name = info.ItemName
	if name then
		local nameLabel=CCLabelTTF:create(name,FONT_NAME,FONT_SM_SIZE)
		nameLabel:setAnchorPoint(PT(0,0))
		nameLabel:setPosition(PT(goodBg:getContentSize().width*0.15,
					goodBg:getContentSize().height*0.99-nameLabel:getContentSize().height))
		goodBg:addChild(nameLabel,0)
	end

	--品质
	local str=nil
	if  genrealQuality[info.QualityType ] then
		str=genrealQuality[info.QualityType ]
	end	
	if str then
		local qualityLabel=CCLabelTTF:create(str,FONT_NAME,FONT_SMM_SIZE)
		qualityLabel:setAnchorPoint(PT(0,0))
		qualityLabel:setPosition(PT(goodBg:getContentSize().width*0.65,
									goodBg:getContentSize().height*0.98-qualityLabel:getContentSize().height))
		goodBg:addChild(qualityLabel,0)
	end
	
	--等级
	if info.CurLevel then
		local goodLv=CCLabelTTF:create(info.CurLevel .. Language.IDS_LEV,FONT_NAME,FONT_SMM_SIZE)
		goodLv:setAnchorPoint(PT(0,0))
		goodLv:setPosition(PT(goodBg:getContentSize().width*0.08, goodBg:getContentSize().height*0.03))
		goodBg:addChild(goodLv,0)
	end
	
	--增加属性值
	local tableInfo=info.AbilityList or {}
	local startX=goodBg:getContentSize().width*0.08
	local startY=goodBg:getContentSize().height*0.12
	local colW=goodBg:getContentSize().width/2
	local rowH=nil
	local col=1
	for k, v in pairs(tableInfo) do
		local str=Language.BAG_TYPE_[v.AbilityType] .. ":+" .. v.BaseNum
		local lAttribute=CCLabelTTF:create(str,FONT_NAME,FONT_SMM_SIZE)
		if not rowH then
			rowH=lAttribute:getContentSize().height+SY(1)
		end
		lAttribute:setAnchorPoint(PT(0,0))
		local posX=startX+((k-1)%col)*colW
		local posY=startY+math.floor((k-1)/col)*rowH
		lAttribute:setPosition(PT(posX,posY))
		goodBg:addChild(lAttribute,0)
	end
		
	--说明文字
	local titleLabel=CCLabelTTF:create(Language.BAG_INTRO .. ":",FONT_NAME,FONT_BIG_SIZE)
	titleLabel:setAnchorPoint(PT(0,0))
	titleLabel:setPosition(PT(goodBg:getPosition().x+goodBg:getContentSize().width+SX(5),
						goodBg:getPosition().y+goodBg:getContentSize().height-titleLabel:getContentSize().height))
	layer:addChild(titleLabel,0)
	local contentStr=string.format("<label>%s</label>",info.ItemDesc or Language.IDS_NONE)
	local contentLabel=ZyMultiLabel:new(contentStr,sizeBox.width*0.35,FONT_NAME,FONT_DEF_SIZE);
	contentLabel:setAnchorPoint(PT(0,0))
	contentLabel:setPosition(PT(titleLabel:getPosition().x,
						titleLabel:getPosition().y-contentLabel:getContentSize().height-SY(2)))
	contentLabel:addto(layer,0)
	
	--售价
	local sellLabel=CCLabelTTF:create(Language.EQUIP_PRICE .. ":",FONT_NAME,FONT_BIG_SIZE)
	sellLabel:setAnchorPoint(PT(0,0))
	sellLabel:setPosition(PT(titleLabel:getPosition().x,
						contentLabel:getPosition().y-sellLabel:getContentSize().height*2))
	layer:addChild(sellLabel,0)
	
	local priceLabel=CCLabelTTF:create(info.Sellprice .. Language.IDS_GOLD,FONT_NAME,FONT_DEF_SIZE)
	priceLabel:setAnchorPoint(PT(0,0))
	priceLabel:setPosition(PT(titleLabel:getPosition().x,
						sellLabel:getPosition().y-sellLabel:getContentSize().height*1.1))
	layer:addChild(priceLabel,0)
	
	
	
	
	--装备更换
	local buttonStr = Language.ROLE_CHANGEEQUIP
	local colW=pWinSize.width/2
	local changeBtn=ZyButton:new("button/list_1023.png",nil,nil,buttonStr)
	changeBtn:setAnchorPoint(PT(0,0))
	changeBtn:setPosition(PT(colW/2-changeBtn:getContentSize().width/2,
							contentBg:getPosition().y-changeBtn:getContentSize().height*1.2))
	changeBtn:addto(layer,0)	
	changeBtn:registerScriptHandler(changeEquip)
	
	
	
	--武器强化
	local buttonStr = Language.ROLE_EQUIPSTRONG 
	local strenBtn=ZyButton:new("button/list_1023.png",nil,nil,buttonStr)
	strenBtn:setAnchorPoint(PT(0,0))
	strenBtn:setPosition(PT(colW*1.5-changeBtn:getContentSize().width/2 ,
							changeBtn:getPosition().y))
	strenBtn:addto(layer,0)
	strenBtn:registerScriptHandler(equipLvUp)
	
	
	
	btnTable = {}
	btnTable.strenBtn = strenBtn
	if  index == 1 then 
			changeBtn:setVisible(false)
			strenBtn:setVisible(false)
			closeBtn:setVisible(false)
				
					--关闭按钮 
			local name=Language.IDS_BACK
			local fun=releaseEquipDetailLayer
			local closeBtn=ZyButton:new("button/list_1023.png",nil,nil,name)
			closeBtn:setAnchorPoint(PT(0,0))
			closeBtn:setPosition(PT(pWinSize.width*0.5-closeBtn:getContentSize().width/2,changeBtn:getPosition().y))
			if  pWinSize.height == 800   then 
				closeBtn:setPosition(PT(pWinSize.width*0.5-closeBtn:getContentSize().width/2,pWinSize.height*0.24))
			end
			closeBtn:addto(layer,0)
			closeBtn:registerScriptHandler(fun)
	end
	
	if GuideLayer.judgeIsGuide(8) then
		GuideLayer.setScene(mScene)
		GuideLayer.init()
	end		
	
end;

function getBtnTable()
	return btnTable
end

----------------------------------------------------------------------------------
function changeEquip()
	releaseEquipDetailLayer()

	local oldItemInfo = m_current_Info.UserItemID 
	ItemListLayer.setInfo(mScene, oldItemInfo)
	
	local equipType = HeroScene.getItemPosition()
	local generalId = HeroScene.getGeneralId()
	ItemListLayer.init(1, equipType, nil, generalId)
end;

function equipLvUp()
	releaseEquipDetailLayer()
	
	HeroScene.sendAction(1202,2)
end;


-------------------------------------------------------------

function  releaseSkillDetailLayer()
	if mSkillDetailLayer then
		mSkillDetailLayer:getParent():removeChild(mSkillDetailLayer,true)
		mSkillDetailLayer=nil
	end
end;

--创建详细层
function  showSkillDetailLayer(isNochangeBtn, openType)
	releaseSkillDetailLayer()
	
	local info = m_current_Info
	
	local layer=CCLayer:create()
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(0,0))	
	mScene:addChild(layer,2)
	
	mSkillDetailLayer=layer
	mOpenType = openType
	
		--创建背景
	local imageSize = SZ(pWinSize.width,pWinSize.height*0.855)
	local imageBg = CCSprite:create(P(Image.ImageBackground))
	imageBg:setScaleX(imageSize.width/imageBg:getContentSize().width)
	imageBg:setScaleY(imageSize.height/imageBg:getContentSize().height)
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(0, pWinSize.height*0.145))
	layer:addChild(imageBg, 0)
	
	--屏蔽按钮ceng
	local unTouchBtn =  ZyButton:new(Image.image_transparent)
	unTouchBtn:setScaleX(imageSize.width/unTouchBtn:getContentSize().width)
	unTouchBtn:setScaleY(imageSize.height/unTouchBtn:getContentSize().height)
	unTouchBtn:setAnchorPoint(PT(0,0))
	unTouchBtn:setPosition(PT(0,imageBg:getPosition().y))
	unTouchBtn:addto(layer, 0)

	---文字标题
	local titleLabel=CCLabelTTF:create(Language.SKILL_DETAIL,FONT_NAME,FONT_BIG_SIZE)
	titleLabel:setAnchorPoint(PT(0.5,0))
	titleLabel:setPosition(PT(pWinSize.width/2,pWinSize.height*0.96-titleLabel:getContentSize().height))
	layer:addChild(titleLabel,0)


	local closeBtn=ZyButton:new("button/list_1046.png")
	closeBtn:setAnchorPoint(PT(0,0))
	closeBtn:setPosition(PT(pWinSize.width-closeBtn:getContentSize().width*1.2,
							pWinSize.height-closeBtn:getContentSize().height*1.2))
	closeBtn:addto(layer,0)
	closeBtn:registerScriptHandler(releaseSkillDetailLayer)
	local startY=closeBtn:getPosition().y-titleLabel:getContentSize().height
	
	if  g_index == 1 then
		closeBtn:setVisible(false)
	
	end
	
	
	
	--简介
	local sizeBox=SZ(pWinSize.width*0.9,pWinSize.height*0.55)
	local contentBg=CCSprite:create(P("common/list_1052.9.png"))
	contentBg:setAnchorPoint(PT(0.5,0))
	contentBg:setScaleY(sizeBox.height/contentBg:getContentSize().height)
	contentBg:setPosition(PT(pWinSize.width/2,
								startY-sizeBox.height))
	layer:addChild(contentBg,0)
	local startX=pWinSize.width/2-contentBg:getContentSize().width*0.48
	
	---	物品图片
	local bgPic = getQualityBg(info.AbilityQuality, 3) 
	local goodBg=CCSprite:create(P(bgPic))
	goodBg:setAnchorPoint(PT(0,0))
	goodBg:setPosition(PT(startX,
				contentBg:getPosition().y+sizeBox.height*0.9-goodBg:getContentSize().height))
	layer:addChild(goodBg,0)
	local imgPath=string.format("bigitem/%s.png",info.MaxHeadID)
	local goodSprite=CCSprite:create(P(imgPath))
	goodSprite:setAnchorPoint(PT(0.5,0.5))
	goodSprite:setPosition(PT(goodBg:getContentSize().width/2,
									goodBg:getContentSize().height/2))
	goodBg:addChild(goodSprite,0)
	
	---名称
	local name = info.AbilityName
	if name then
		local nameLabel=CCLabelTTF:create(name,FONT_NAME,FONT_SM_SIZE)
		nameLabel:setAnchorPoint(PT(0,0))
		nameLabel:setPosition(PT(goodBg:getContentSize().width*0.15,
					goodBg:getContentSize().height*0.99-nameLabel:getContentSize().height))
		goodBg:addChild(nameLabel,0)
	end
	
	--品质
	local str=""
	if  genrealQuality[info.AbilityQuality ] then
		str=str .. genrealQuality[info.AbilityQuality ]
	end	
	local qualityLabel=CCLabelTTF:create(str,FONT_NAME,FONT_SMM_SIZE)
	qualityLabel:setAnchorPoint(PT(0,0))
	qualityLabel:setPosition(PT(goodBg:getContentSize().width*0.65,
								goodBg:getContentSize().height*0.98-qualityLabel:getContentSize().height))
	goodBg:addChild(qualityLabel,0)
	
		
	--说明文字
	local titleLabel=CCLabelTTF:create(Language.BAG_INTRO .. ":",FONT_NAME,FONT_BIG_SIZE)
	titleLabel:setAnchorPoint(PT(0,0))
	titleLabel:setPosition(PT(goodBg:getPosition().x+goodBg:getContentSize().width+SX(5),
						goodBg:getPosition().y+goodBg:getContentSize().height-titleLabel:getContentSize().height))
	layer:addChild(titleLabel,0)
	local contentStr=string.format("<label>%s</label>",info.AbilityDesc or Language.IDS_NONE)
	local contentLabel=ZyMultiLabel:new(contentStr,sizeBox.width*0.38,FONT_NAME,FONT_DEF_SIZE);
	contentLabel:setAnchorPoint(PT(0,0))
	contentLabel:setPosition(PT(titleLabel:getPosition().x,
						titleLabel:getPosition().y-contentLabel:getContentSize().height-SY(2)))
	contentLabel:addto(layer,0)
	
	
	
	--魂技类型
	local AttackType = AttackTypetTable[info.AttackType]
	if AttackType then
		local nameLabel=CCLabelTTF:create(AttackType,FONT_NAME,FONT_SM_SIZE)
		nameLabel:setAnchorPoint(PT(0,0))
		nameLabel:setPosition(PT(goodBg:getContentSize().width*0.10,
					goodBg:getContentSize().height*0.10))
		goodBg:addChild(nameLabel,0)
	end

	--魂技等级
	local level = info.AbilityLv
	if level then
		local nameLabel=CCLabelTTF:create(level..Language.IDS_LEV,FONT_NAME,FONT_SM_SIZE)
		nameLabel:setAnchorPoint(PT(0,0))
		nameLabel:setPosition(PT(goodBg:getContentSize().width*0.1,
					goodBg:getContentSize().height*0.03))
		goodBg:addChild(nameLabel,0)
	end
	
	
	--魂技更换
	local colW=pWinSize.width/2
	local buttonStr = Language.ROLE_CHANGESKILL
	local actionPath = changeSkill
	
	if m_current_Info.Position == 1 or isNochangeBtn then--退出
		buttonStr = Language.IDS_BACK
		actionPath = releaseSkillDetailLayer
	end
	local strenBtn=ZyButton:new("button/list_1039.png",nil,nil,buttonStr)
	strenBtn:setAnchorPoint(PT(0,0))
	strenBtn:setPosition(PT(colW/2-strenBtn:getContentSize().width/2,
							contentBg:getPosition().y-strenBtn:getContentSize().height*1.2))
	strenBtn:addto(layer,0)	
	strenBtn:registerScriptHandler(actionPath)
	
	
	--魂技升级
	local buttonStr = Language.IDS_LEVELUP 
	local closeBtn=ZyButton:new("button/list_1039.png",nil,nil,buttonStr)
	closeBtn:setAnchorPoint(PT(0,0))
	closeBtn:setPosition(PT(colW*1.5-closeBtn:getContentSize().width/2 ,
							contentBg:getPosition().y-closeBtn:getContentSize().height*1.2))
	closeBtn:addto(layer,0)
	closeBtn:registerScriptHandler(skillLvUp)
	
	if  g_index == 1 then
		closeBtn:setVisible(false)
		strenBtn:setVisible(false)
		
					
		
					--关闭按钮 
		local name=Language.IDS_BACK
		local fun=releaseSkillDetailLayer
		local popBtn=ZyButton:new("button/list_1023.png",nil,nil,name)
		popBtn:setAnchorPoint(PT(0,0))
		popBtn:setPosition(PT(pWinSize.width*0.5-popBtn:getContentSize().width/2,
									contentBg:getPosition().y-closeBtn:getContentSize().height*1.2))
		if  pWinSize.height == 800 then
			popBtn:setPosition(PT(pWinSize.width*0.5-popBtn:getContentSize().width/2,
									pWinSize.height*0.25))
		end
		popBtn:addto(layer,0)
		popBtn:registerScriptHandler(fun)
	end
end;

--魂技更换
function changeSkill()
	releaseSkillDetailLayer()
	

	local position =m_current_Info.Position
	local generalId =m_current_Info.GeneralID
	
	local oldItemInfo = m_current_Info
	ItemListLayer.setInfo(mScene,oldItemInfo)	
	
	ItemListLayer.init(2, position, nil, generalId)
end;

--魂技升级
function skillLvUp()
	MainMenuLayer.setIsShow(true, false)
 	HeroScene.setBgIsVisible(true)
 	RoleBagScene.setBgIsVisible(true)

	releaseSkillDetailLayer()
	local AbilityID = m_current_Info.AbilityID--技能id
	local UserItemID = m_current_Info.UserItemID --	
	
	SoulSkillScene.setAbilityID(AbilityID, UserItemID, mScene, mOpenType)
	SoulSkillScene.init()
end

----------------------------------------------------
--释放强化层
function  releaseStrenLayer()
	if mStrenLayer then
		mStrenLayer:getParent():removeChild(mStrenLayer,true)
		mStrenLayer=nil
	end
end;

--创建强化层
function createStrenLayer(info)
	mDetailInfo = info
	MainMenuLayer.setIsShow(false, true)
	HeroScene.setBgIsVisible(true)
	

	releaseStrenLayer()
	local layer=CCLayer:create()
	mStrenLayer=layer
	mScene:addChild(layer,2)
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(0,0))
	
	local sprite=CCSprite:create(P("mainUI/list_1014.png"))
	local boxSize=SZ(pWinSize.width,pWinSize.height-sprite:getContentSize().height)
	for k=1 ,2 do
		local pingBiBtn=UIHelper.createActionRect(boxSize)
		pingBiBtn:setPosition(PT(0,pWinSize.height-boxSize.height))
		layer:addChild(pingBiBtn,0)
	end	
	
	local titleBg=CCSprite:create(P("common/list_1047.png"))
	
	
	local path="common/list_1043.png"
	local bgSprite=CCSprite:create(P(path))
	local boxSize=SZ(pWinSize.width,pWinSize.height*0.75)
	bgSprite:setAnchorPoint(PT(0.5,0))
	bgSprite:setScaleX(boxSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(boxSize.height/bgSprite:getContentSize().height)
	local posY=pWinSize.height*0.145--pWinSize.height-boxSize.height-titleBg:getContentSize().height
	bgSprite:setPosition(PT(pWinSize.width/2,posY))
	layer:addChild(bgSprite,0)
	
	
	local tabStr={Language.EQUIP_STREN}
	local tabBar=createTabBar(tabStr,layer)
	local startY=tabBar:getPosition().y  

	--背景图片
	local sizeBox=SZ(pWinSize.width*0.8,pWinSize.height*0.5)

	local contentBg=CCSprite:create(P("common/list_1052.9.png"))
	contentBg:setAnchorPoint(PT(0.5,0))
	contentBg:setScaleY(sizeBox.height/contentBg:getContentSize().height)
	contentBg:setPosition(PT(pWinSize.width/2,
								startY-sizeBox.height-SY(5)))
	layer:addChild(contentBg,0)
	local startX=pWinSize.width*0.5-contentBg:getContentSize().width*0.48
	---	物品图片
	local bgPic = getQualityBg(info.QualityType, 3)
	local goodBg=CCSprite:create(P(bgPic))
	goodBg:setAnchorPoint(PT(0,0))
	goodBg:setPosition(PT(startX,
				contentBg:getPosition().y+(sizeBox.height-goodBg:getContentSize().height)/2))
	layer:addChild(goodBg,0)
	local imgPath=string.format("bigitem/%s.png",info.MaxHeadID )
	local goodSprite=CCSprite:create(P(imgPath))   
	goodSprite:setAnchorPoint(PT(0.5,0.5))
	goodSprite:setPosition(PT(goodBg:getContentSize().width/2,
									goodBg:getContentSize().height/2))
	goodBg:addChild(goodSprite,0)
	
	--等级
	local goodLv=CCLabelTTF:create(info.CurLevel .. Language.IDS_LEV,FONT_NAME,FONT_SMM_SIZE)
	goodLv:setAnchorPoint(PT(0,0))
	goodLv:setPosition(PT(goodBg:getContentSize().width*0.08,goodBg:getContentSize().height*0.09-goodLv:getContentSize().height))
	goodBg:addChild(goodLv,0)
	--属性
	local tableInfo=info.AbilityList or {}
	local startX=goodLv:getPosition().x
	local startY=goodBg:getContentSize().height*0.1
	local colW=goodBg:getContentSize().width/2
	local rowH=nil
	local col=1
	for k, v in pairs(tableInfo) do
		local str=Language.BAG_TYPE_[v.AbilityType] .. "+" .. v.BaseNum
		local lAttribute=CCLabelTTF:create(str,FONT_NAME,FONT_SMM_SIZE)
		if not rowH then
			rowH=lAttribute:getContentSize().height+SY(1)
		end
		lAttribute:setAnchorPoint(PT(0,0))
		local posX=startX+((k-1)%col)*colW
		local posY=startY+math.floor((k-1)/col)*rowH
		lAttribute:setPosition(PT(posX,posY))
		goodBg:addChild(lAttribute,0)
	end		
	---名称
	local nameLabel=CCLabelTTF:create(info.ItemName,FONT_NAME,FONT_SM_SIZE)
	nameLabel:setAnchorPoint(PT(0,0))
	nameLabel:setPosition(PT(goodBg:getContentSize().width*0.15,
				goodBg:getContentSize().height*0.99-nameLabel:getContentSize().height))
	goodBg:addChild(nameLabel,0)
	--品质
	local str="" 
	if  genrealQuality[info.QualityType ] then
		str=str .. genrealQuality[info.QualityType ]
	end	
	local qualityLabel=CCLabelTTF:create(str,FONT_NAME,FONT_SMM_SIZE)
	qualityLabel:setAnchorPoint(PT(0,0))
	qualityLabel:setPosition(PT(goodBg:getContentSize().width*0.65,
								nameLabel:getPosition().y))
	goodBg:addChild(qualityLabel,0)
	
	--强化标题
	local titleLabel=CCLabelTTF:create(Language.EQUIP_AFTER .. ":",FONT_NAME,FONT_DEF_SIZE)
	titleLabel:setAnchorPoint(PT(0,0))
	titleLabel:setPosition(PT(goodBg:getPosition().x+goodBg:getContentSize().width*1.075,
								goodBg:getPosition().y+goodBg:getContentSize().height-titleLabel:getContentSize().height))
	layer:addChild(titleLabel,0)
		
	--增加的属性
	local tableInfo=info.AbilityList or {}
	local rowH=titleLabel:getContentSize().height
	local startX=titleLabel:getPosition().x
	local startY=titleLabel:getPosition().y-rowH*1.5
	local colW=pWinSize.width*0.2
	local afterInfo=info.AbilityList1 or {}
	for k, v in pairs(tableInfo) do
		local str=Language.BAG_TYPE_[v.AbilityType] .. ":"
		local lAttribute=CCLabelTTF:create(str,FONT_NAME,FONT_SM_SIZE)
		lAttribute:setAnchorPoint(PT(0,0))
		lAttribute:setPosition(PT(startX,startY-(k-1)*rowH*2))
		layer:addChild(lAttribute,0)
		local baseNum=CCLabelTTF:create(v.BaseNum,FONT_NAME,FONT_SM_SIZE)
		baseNum:setAnchorPoint(PT(0,0))
		baseNum:setPosition(PT(lAttribute:getPosition().x,
								lAttribute:getPosition().y-rowH))
		layer:addChild(baseNum,0)
		---
		
		local toSprite=CCSprite:create(P("common/list_1189.png"))
		toSprite:setAnchorPoint(PT(0,0.5))
		toSprite:setPosition(PT(baseNum:getPosition().x+baseNum:getContentSize().width,
								baseNum:getPosition().y+baseNum:getContentSize().height/2))	
		layer:addChild(toSprite,0)
		
		local afterNum=CCLabelTTF:create(afterInfo[k].BaseNum,FONT_NAME,FONT_SM_SIZE)
		afterNum:setAnchorPoint(PT(0,0))
		afterNum:setPosition(PT(toSprite:getPosition().x+toSprite:getContentSize().width,
								baseNum:getPosition().y))
		layer:addChild(afterNum,0)
	end
	startY=startY-(#tableInfo-1)*rowH*2
	
	--等级变化
	local levelTile=CCLabelTTF:create(Language.IDS_LEVEL,FONT_NAME,FONT_DEF_SIZE)
	levelTile:setAnchorPoint(PT(0,0))
	levelTile:setPosition(PT(startX,startY-rowH*3))
	layer:addChild(levelTile,0)
	local levelNum=info.CurLevel
		

	local baseNum=CCLabelTTF:create(string.format(Language.IDS_LVSTR,levelNum),FONT_NAME,FONT_SM_SIZE)
	baseNum:setAnchorPoint(PT(0,0))
	baseNum:setPosition(PT(levelTile:getPosition().x,
							levelTile:getPosition().y-rowH))
	layer:addChild(baseNum,0)
	---

	if info.IsMaxLv ~= 1 then--是否已经强化到最大等级   0：否1：是 
		local levAfter=levelNum+1	
		local toSprite=CCSprite:create(P("common/list_1189.png"))
		toSprite:setAnchorPoint(PT(0,0.5))
		toSprite:setPosition(PT(baseNum:getPosition().x+baseNum:getContentSize().width,
								baseNum:getPosition().y+baseNum:getContentSize().height/2))	
		layer:addChild(toSprite,0)
		
		local afterNum=CCLabelTTF:create(string.format(Language.IDS_LVSTR,levAfter),
													FONT_NAME,FONT_SM_SIZE)
		afterNum:setAnchorPoint(PT(0,0))
		afterNum:setPosition(PT(toSprite:getPosition().x+toSprite:getContentSize().width,
								baseNum:getPosition().y))
		layer:addChild(afterNum,0)
	else
		local afterNum=CCLabelTTF:create(Language.ROLE_LVMAX,
													FONT_NAME,FONT_SM_SIZE)
		afterNum:setColor(ZyColor:colorRed())
		afterNum:setAnchorPoint(PT(0,0))
		afterNum:setPosition(PT(baseNum:getPosition().x,
								baseNum:getPosition().y-baseNum:getContentSize().height))
		layer:addChild(afterNum,0)			
	end
	
---需求
	startY=baseNum:getPosition().y-rowH*2
	local needLabel=CCLabelTTF:create(Language.EQUIP_YICI .. ":" .. info.StrongMoney..Language.IDS_GOLD ,FONT_NAME,FONT_SM_SIZE)
	needLabel:setAnchorPoint(PT(0,0))
	needLabel:setPosition(PT(startX,startY))
	layer:addChild(needLabel,0)
	local needLabel1=CCLabelTTF:create(Language.EQUIP_SHICI .. ":" .. info.TenTimesStrongMoney..Language.IDS_GOLD ,FONT_NAME,FONT_SM_SIZE)
	needLabel1:setAnchorPoint(PT(0,0))
	needLabel1:setPosition(PT(startX,needLabel:getPosition().y-needLabel1:getContentSize().height-SY(5)))
	layer:addChild(needLabel1,0)
	
	
---关闭按钮
	local colW=pWinSize.width/2
	local closeBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.IDS_COLSE)
	closeBtn:setAnchorPoint(PT(0,0))
	closeBtn:setPosition(PT(colW/2-closeBtn:getContentSize().width/2,
							contentBg:getPosition().y-closeBtn:getContentSize().height*1.2))
	closeBtn:addto(layer,0)
	closeBtn:registerScriptHandler(clsoeStren)
	
	--强化按钮一次
	local strenBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.EQUIP_YICI_1,nil,FONT_SMM_SIZE)
	strenBtn:setAnchorPoint(PT(0,0))
	strenBtn:setPosition(PT(colW-strenBtn:getContentSize().width/2,
							closeBtn:getPosition().y))
	strenBtn:addto(layer,0)	
	strenBtn:registerScriptHandler(strenWeaponAction)
	
	--强化按钮十次
	local strenTenBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.EQUIP_SHICI_1,nil,FONT_SMM_SIZE)
	strenTenBtn:setAnchorPoint(PT(0,0))
	strenTenBtn:setPosition(PT(colW*1.5-strenTenBtn:getContentSize().width/2,
							closeBtn:getPosition().y))
	strenTenBtn:addto(layer,0)	
	strenTenBtn:registerScriptHandler(strenWeaponAction10)
	
	
	btnTable = {}
	btnTable.strenBtn = strenBtn
	if GuideLayer.judgeIsGuide(9) then
		GuideLayer.setScene(mScene)
		GuideLayer.init()
	end	
	
end;

--创建Tab
function createTabBar(tabName, layer)
	local tabBar = ZyTabBar:new(Image.image_top_button_0, Image.image_top_button_1, tabName, FONT_NAME, FONT_SM_SIZE, 4, Image.image_LeftButtonNorPath, Image.image_rightButtonNorPath);
	tabBar:setCallbackFun(callbackTabBar); -----点击启动函数
--	tabBar:addto(layer,0) ------添加
	tabBar:setColor(ZyColor:colorYellow())  ---设置颜色
	
	tabBar:setPosition(PT(pWinSize.width*0.035,pWinSize.height*0.83))  -----设置坐标
	return tabBar
end

--关闭强化界面
function clsoeStren()
 	releaseStrenLayer()
 	MainMenuLayer.setIsShow(false, false)
 	HeroScene.setBgIsVisible(false)
 	HeroScene.refreshWin()
end;


--请求强化接口
--一次
function strenWeaponAction()
	if not isClick then
		isClick =true			
		shicitype=1
		actionLayer.Action1204(mScene,nil,mDetailInfo.UserItemID,1)
	end
end;
--十次
function strenWeaponAction10()
	if not isClick then
		isClick =true		
		shicitype=10		
		actionLayer.Action1204(mScene,nil,mDetailInfo.UserItemID,10 )
	end
end;


--强化动画
-------------------------------------------------------------
function releaseAnimationLayer()
	if mAnimationLayer then
		mAnimationLayer:getParent():removeChild(mAnimationLayer,true)
		mAnimationLayer=nil
	end	 
end;
--
function animationLayer(serverData)
	releaseAnimationLayer()
	local layer=CCLayerColor:create(ccc4(0,0,0,255))
	mAnimationLayer=layer
	for k=1 ,2 do
		local actionBtn=UIHelper.createActionRect(pWinSize)
		actionBtn:setPosition(PT(0,0))
		layer:addChild(actionBtn,0)
	end
	 mScene:addChild(layer,2)
	 
	 local sprite=Sprite:new("donghua_1001")
	 sprite:setPosition(pWinSize.width/2,pWinSize.height/2)
	 sprite:addto(layer,0)
	 
	 if shicitype==1 then
		 bgPath = "common/list_8006_1.png"
		 if serverData.StrongLv > 1 then
		 	bgPath = "common/list_8006.png"
		 end	
	elseif shicitype==10 then
		bgPath = "common/list_3040.png"
		 if serverData.StrongLv > 10 then
		 	bgPath = "common/list_3041.png"
		 end
	end
	 local str=string.format(Language.EQUIP_TIP2,mDetailInfo.CurLevel+1 or 1)
	 local bgSprite=CCSprite:create(P(bgPath))
	 bgSprite:setAnchorPoint(PT(0.5,0.5))
	 bgSprite:setPosition(PT(pWinSize.width/2,pWinSize.height/2)) 
	 layer:addChild(bgSprite,0)
	 local numTable={
	 "list_8000", "list_8001","list_8002", "list_8003","list_8004", "list_8005", }
--	 local path=string.format("common/%s.png",numTable[serverData.StrongLv])
--	 local numSprite=CCSprite:create(P(path))
	 numSprite=WeaponScene.getNumberSprite(serverData.StrongLv)
	 numSprite:setAnchorPoint(PT(0,0.5))
	 numSprite:setPosition(PT(bgSprite:getContentSize().width*0.72,bgSprite:getContentSize().height/2)) 
	 bgSprite:addChild(numSprite,0)

	bgSprite:setScale(0.5)
	local point=PT(pWinSize.width/2,pWinSize.height*0.3)
	local action=CCScaleTo:create(0.5,1)
	local funcName = CCCallFuncN:create(HeroAccessory.animationOver)	
	local action2 = CCSequence:createWithTwoActions(CCDelayTime:create(0.5),funcName);	
	local action1 = CCMoveTo:create(0.3, point) 
	local actionHarm=CCSequence:createWithTwoActions(action1,action2)
	local lastAction=CCSpawn:createWithTwoActions(action,actionHarm)
	bgSprite:runAction(lastAction)
	
end;

--获取数字图片
function getNumberSprite(nNumber,type)
	local imageFile = "common/list_3042.png"
	local texture = IMAGE(imageFile)
	if texture == nil then
		return nil
	end
	local txSize = texture:getContentSize()
	local strNumber = tostring(nNumber)
	strNumber=strNumber or 0
	strNumber=math.abs(strNumber)
	local nLength = string.len(strNumber)
	local pNode = CCNode:create()
	local nWidth = txSize.width/3
	local nHeight = txSize.height/4
	local subFrame = CCSpriteFrame:createWithTexture(texture, CCRectMake(0, 3*nHeight,nWidth,nHeight))
	local nLeft =-nWidth
	if type ~=1 then
		local subSprite = CCSprite:createWithSpriteFrame(subFrame)
		pNode:setPosition(PT(0, 0))
		pNode:addChild(subSprite, 0)
		subSprite:setAnchorPoint(PT(0,0))
		subSprite:setPosition(PT(0, 0 ))
		nLeft =0
	end
	for i = 1,nLength do
		local nDig = tonumber(string.sub(strNumber, i, i))
		if nDig == 0 then
			subFrame = CCSpriteFrame:createWithTexture(texture, CCRectMake(nWidth, 3*nHeight,nWidth,nHeight))
		else
			subFrame = CCSpriteFrame:createWithTexture(texture, CCRectMake((nDig- 1)%3*nWidth,math.floor((nDig -1)/3)*nHeight,nWidth,nHeight))
		end
		subSprite = CCSprite:createWithSpriteFrame(subFrame)
		pNode:addChild(subSprite, 0)
		subSprite:setAnchorPoint(PT(0,0))
		nLeft = nLeft + nWidth
		subSprite:setPosition(PT(nLeft, 0 ))

	end
	pNode:setContentSize(SZ(nLeft, subSprite:getContentSize().height))
	return pNode
end

function animationOver()
	releaseAnimationLayer()
	refreshStreng()

end;

function refreshStreng()
	actionLayer.Action1202(mScene,false,mDetailInfo.UserItemID,nil, 3)
end


---延迟进行方法
function delayExec(funName,nDuration)
	local action = CCSequence:createWithTwoActions(
	CCDelayTime:create(nDuration),
	CCCallFunc:create(funName));
	if mStrenLayer then
		mStrenLayer:runAction(action)
	end
end
------------------------------------------------------------




function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)

	if actionId==1202 then
		local serverInfo=actionLayer._1202Callback(pScutScene,lpExternalDate)
		if serverInfo then
			if userData == 1 then--详情
				HeroAccessory.setData(serverInfo)				
				HeroAccessory.createEquipDetailLayer()	
			elseif userData == 2 then--强化
				mDetailInfo = serverInfo
				HeroAccessory.createStrenLayer(serverInfo)
			elseif userData == 3 then--强化完成刷新
				createStrenLayer(serverInfo)			
			end
		end	
	elseif actionId==1204 then
    		if ZyReader:getResult() == eScutNetSuccess then	
    			isStrenAction=true
    			MainMenuLayer.refreshWin()
				local serverData=actionLayer._1204Callback(pScutScene,lpExternalDate)
				animationLayer(serverData)
    		else
				ZyToast.show(pScutScene, ZyReader:readErrorMsg())
    		end	
	end    
	isClick = false

end;

