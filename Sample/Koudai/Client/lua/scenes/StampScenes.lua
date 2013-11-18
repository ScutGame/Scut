------------------------------------------------------------------
-- StampScenes.lua.lua
-- Author     : yeyq

-- Version    : 1.0
-- Date       :
-- Description:    集邮
------------------------------------------------------------------

module("StampScenes", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local g_scene = nil 		-- 场景
local g_mCurrentTab = nil 
local g_List = nil 
local g_StampTabel = nil 
local g_detailLayer = nil  
local g_LayerBG = nil 
local	g_StampType = {
		YongBin = 1, 
		ZhuangBei = 2, 
		HunJi = 3 ,
	}
local 	g_StampTable = {
		[g_StampType.YongBin] = Language.Stamp_YONGBIN, 
		[g_StampType.ZhuangBei] = Language.Stamp_ZHUANGBEI,
		[g_StampType.HunJi] =Language.Stamp_HUNJI,
	}
local  g_SceneTabel = {
			index = 1,
				Scene = nil,
}
--
---------------公有接口(以下两个函数固定不用改变)--------

-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
	if not g_mCurrentTab then
		g_mCurrentTab = 1 
	end
end
-- 释放资源
function releaseResource()
	g_scene=nil
end


-- 创建场景
function init()
	if g_scene then
		return
	end
	initResource()
	local scene = ScutScene:new()
	-- 注册网络回调
	scene:registerCallback(networkCallback)
	g_scene = scene.root
	initResource()
		g_scene:registerScriptHandler(SpriteEase_onEnterOrExit)
	SlideInLReplaceScene(g_scene,1)
	
	
	
	
	
	-- 添加背景
	
	local layerBG = CCLayer:create()
	g_scene:addChild(layerBG, 0)

	g_LayerBG = layerBG
	-- 此处添加场景初始内容
	
	local bgImgW =pWinSize.width 
	local bgImgH =pWinSize.height*(800/930)
	local bgImgY = pWinSize.height-bgImgH
	local bgImg = CCSprite:create(P("common/list_1024.png"))
	bgImg:setScaleX(bgImgW/bgImg:getContentSize().width)
	bgImg:setScaleY(bgImgH/bgImg:getContentSize().height)
	layerBG:addChild(bgImg,0)
	bgImg:setAnchorPoint(PT(0,0))
	bgImg:setPosition(PT(0,bgImgY))
	MainMenuLayer.init(2, g_scene)
	
	----tab 组件
	local  tabBarY = nil
	if pWinSize.height == 800 then 
		tabBarY = pWinSize.height*(875/960)
	else
		tabBarY = pWinSize.height*(865/960)
	end

	local tabBarX = pWinSize.width*(38/620)
	local tabBar=ZyTabBar:new(nil,nil,g_StampTable,FONT_NAME,FONT_SM_SIZE,3)
	tabBar:addto(layerBG,0)
	tabBar:setColor(ZyColor:colorYellow())
	tabBar:setAnchorPoint(PT(0,0))
	tabBar:selectItem(g_mCurrentTab);				--点击哪个按钮
	tabBar:setCallbackFun(callbackTabBarFunc);		--点击响应的事件
	tabBar:setPosition(PT(tabBarX,tabBarY))
	
	
	
	--list 背景
	local listBBIY =  nil
	local listBBIW =  pWinSize.width*(549/621)
	local listBBIH = nil
	if pWinSize.height == 800  then 
		listBBIH = pWinSize.height*(635/930)
		listBBIY =  pWinSize.height*(60/960)+bgImg:getPosition().y
	else
		listBBIH = pWinSize.height*(615/930)
		listBBIY =  pWinSize.height*(73/960)+bgImg:getPosition().y
	end
	
	local listBIgBgImg = CCSprite:create(P("common/list_1038.9.png"))
	listBIgBgImg:setScaleX(listBBIW/listBIgBgImg:getContentSize().width)
	listBIgBgImg:setScaleY(listBBIH/listBIgBgImg:getContentSize().height)
	layerBG:addChild(listBIgBgImg,0)
	listBIgBgImg:setAnchorPoint(PT(0,0))
	listBIgBgImg:setPosition(PT((pWinSize.width-listBBIW)/2,listBBIY))
	
	---list
	local listH = pWinSize.height*(605/930)
	local listW = pWinSize.width*(553/640)	
	local listY = nil
	local listH = nil
	if pWinSize.height == 800  then 
		listY = pWinSize.height*(68/960)+bgImg:getPosition().y	
		listH = pWinSize.height*(620/930)
	else
		listY = pWinSize.height*(80/960)+bgImg:getPosition().y	
		listH = pWinSize.height*(605/930)
	end
	local listSize = SZ(listW,listH)
	local list = ScutCxList:node(listH/4,ccc4(24,24,24,0),listSize)
	list:setTouchEnabled(true)
	list:setAnchorPoint(PT(0,0))
	list:setPosition(PT((pWinSize.width-listW)/2,listY))
	layerBG:addChild(list,0)
	g_List = list	
	showAction()
end

function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        releaseResource()
    end
end



--点击响应
function  callbackTabBarFunc(bar,pNode)
	local index=pNode:getTag(); ----按钮的标号
	if index~=g_mCurrentTab then
	    g_mCurrentTab = index; 
	    showAction()
	end
end
function showAction()
--	local  index  = nil
--	if  index ~= g_mCurrentTab  and  pNode ~= nil  then
--		local index = pNode:getTag()
--		g_mCurrentTab = index
--	end
	if g_mCurrentTab == g_StampType.YongBin then 
		g_List:clear();
		actionLayer.Action1610(g_scene,nil,1,1,800)
	elseif  g_mCurrentTab == g_StampType.ZhuangBei then 
		g_List:clear();
		actionLayer.Action1610(g_scene,nil,2,1,800)
	elseif  g_mCurrentTab == g_StampType.HunJi  then 
		g_List:clear();
		actionLayer.Action1610(g_scene,nil,3,1,800)
	end
end
function showLayer()

	local layerNum  = math.ceil(#g_StampTabel/5)	
	
	for  i=1,layerNum do
		local item = ScutCxListItem:itemWithColor(ccc3(25,57,45))	
		item:setOpacity(0)
		local layout = CxLayout()
		layout.val_x.t = ABS_WITH_PIXEL
		layout.val_y.t = ABS_WITH_PIXEL	
		layout.val_x.val.pixel_val =0
		layout.val_y.val.pixel_val =0
		local itemLayer = CCLayer:create()
		local listHeight =  g_List:getRowHeight()
		
		---卡牌背景
		--宽
		local  CardImgC = CCSprite:create(P("common/list_1012.png"))
		local listW = pWinSize.width*(553/640)
		local RateW = (listW-CardImgC:getContentSize().width*5)/6
		local CardImgW = (listW-RateW*6)/5
		-- 高
		local listH = pWinSize.height*(605/930)
		local RateH =  (listH-CardImgC:getContentSize().height*4)/5
		local CardImgH = (listH-RateH*5)/4
		
		--卡牌本身
		--宽
		local CardImgToC = CCSprite:create(P("smallitem/Icon_1001.png"))
		local RateToW = (listW-CardImgToC:getContentSize().width*5)/6
		local CardImgToCW = (listW-RateToW*6)/5
		--高
		local RateToH = (listH-CardImgToC:getContentSize().height*4)/5
		local CardImgToCH = (listH-RateToH*5)/4
	

		----列 放置
		for k=1,5 do
			local index =  k + (i-1)*5 
			if index > #g_StampTabel then 
			    index = #g_StampTabel
			end 
			if g_StampTabel[index].HeadID then
				local  CardImg= CCSprite:create(P("common/list_1012.png"))
				itemLayer:addChild(CardImg,0)
				CardImg:setAnchorPoint(PT(0,0))
				CardImg:setPosition(PT(RateW+(RateW+CardImgW)*(k-1),(listHeight-CardImg:getContentSize().height)/2))	
				if  g_StampTabel[index].Status  ==  1 then 
					if  g_StampTabel[index].Quality ~= 0 and g_StampTabel[index].Quality ~= nil    then
						local CardImgStr =getQualityBg(g_StampTabel[index].Quality, 1)
						CardImg= CCSprite:create(P(CardImgStr))
						itemLayer:addChild(CardImg,0)
						CardImg:setAnchorPoint(PT(0,0))
						CardImg:setPosition(PT(RateW+(RateW+CardImgW)*(k-1),(listHeight-CardImg:getContentSize().height)/2))	
					end
				
					local ImgStr = string.format("smallitem/%s.png",g_StampTabel[index].HeadID)
					local  OpenCardImg= CCSprite:create(P(ImgStr))
					CardImg:addChild(OpenCardImg,2)
					OpenCardImg:setAnchorPoint(PT(0.5,0.5))
					OpenCardImg:setPosition(PT(CardImg:getContentSize().width/2,
										CardImg:getContentSize().height/2))	
					
					
					local OpenCardStr = CCLabelTTF:create(g_StampTabel[index].Name,FONT_NAME,FONT_SM_SIZE)	
					itemLayer:addChild(OpenCardStr,2)
					OpenCardStr:setAnchorPoint(PT(0.5,0))
					OpenCardStr:setPosition(PT(RateToW+(RateToW+CardImgToCW)*(k-1)+OpenCardImg:getContentSize().width/2,0))
					
					
					local actionBtn=UIHelper.createActionRect(OpenCardImg:getContentSize(),StampScenes.toType, index)
					actionBtn:setAnchorPoint(PT(0,0))
					actionBtn:setPosition(PT(0,0))
					OpenCardImg:addChild(actionBtn,0)
				else
					local ImgStr = "common/list_2070.png"
					local  OpenCardImg= CCSprite:create(P(ImgStr))
					CardImg:addChild(OpenCardImg,2)
					OpenCardImg:setAnchorPoint(PT(0.5,0.5))
					OpenCardImg:setPosition(PT(CardImg:getContentSize().width/2,
										CardImg:getContentSize().height/2))
				end
				 
			end
		end

		item:addChildItem(itemLayer,layout)
		g_List:addListItem(item,false)
	end
end

----- 品质框小的
function qualityImgFunc(qNum)
	if qNum ==1  then
		CardImg= "common/icon_8015_1.png"
	elseif  qNum ==2 then
		CardImg= "common/icon_8015_2.png"
	elseif  qNum ==3 then
		CardImg= "common/icon_8015_3.png"
	elseif  qNum ==4 then
		CardImg= "common/icon_8015_4.png"
	end
	return CardImg
end
---卡牌类型
function toType(pNode, index)
	if  pNode  then 
		index = pNode:getTag()
	end
	g_SceneTabel.Scene = g_scene
	 cardID = g_StampTabel[index].ID
	local AlbumTypeTable={
	[ g_StampType.YongBin ]=1,
	[ g_StampType.ZhuangBei ]=2,
	[ g_StampType.HunJi ]=3,	
	}
	actionLayer.Action1611(g_scene,nil,AlbumTypeTable[g_mCurrentTab],cardID)
	--[[
	if    g_mCurrentTab   ==  g_StampType.YongBin  then
		actionLayer.Action1403(g_scene,nil,actionId,nil)
	elseif    g_mCurrentTab == g_StampType.ZhuangBei   then
		actionLayer.Action1202(g_scene,nil,actionId,nil)
	elseif    g_mCurrentTab == g_StampType.HunJi   then
		actionLayer.Action1485(g_scene,nil,actionId)
	end
	--]]
end


-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionID = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	
	if   actionID == 1610 then
		local serverInfo=actionLayer._1610Callback(pScutScene, lpExternalData) ----返回整个表的数据
		if serverInfo~=nil  and serverInfo ~= {} then
			g_StampTabel = serverInfo.StampTable 
			if  g_StampTabel ~={} and g_StampTabel ~= nil  then 
				showLayer()
			else
				local nilString = CCLabelTTF:create(Language.MAILL_NONE,FONT_NAME,FONT_SM_SIZE)	
				g_LayerBG:addChild(nilString,0)
				nilString:setAnchorPoint(PT(0.5,0.5))
				nilString:setPosition(PT(pWinSize.width*0.5,pWinSize.height*0.5))
			end
		else
			ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.5)					
		end
	elseif actionID == 1611  then
		local serverInfo=actionLayer._1611Callback(pScutScene, lpExternalData) ----返回整个表的数据
		if  serverInfo then
			--人物  装备 魂技
			local info = resetSkillCard(serverInfo)
			if userData==1 then
			
				actionLayer.Action1403(g_scene,nil,cardID,actionId,nil)
				--showHeroDetailLayer.setData(info.GeneralInfo, 1, g_scene)
				--showHeroDetailLayer.init(g_SceneTabel)	
			elseif userData==2 then
				HeroAccessory.setData(info.EquipInfo)
				HeroAccessory.createEquipDetailLayer(g_SceneTabel)
			elseif  userData==3 then
				HeroAccessory.setScene(g_SceneTabel)
				HeroAccessory.setData(info.SkillInfo)
				local isNochangeBtn = true--是否没有更换按钮		
				HeroAccessory.showSkillDetailLayer(isNochangeBtn)
			end
		end		
	elseif  actionID == 1403  then
		local serverInfo=actionLayer._1403Callback(pScutScene, lpExternalData) ----佣兵
		if serverInfo~=nil then
			if  serverInfo ~={} and serverInfo ~= nil  then
				local Skill = {}
				for k,v in ipairs(serverInfo.RecordTabel2) do
					local postion = v.Position
					Skill[postion] = v
				end
				serverInfo.Skill = Skill			
				showHeroDetailLayer.setData(serverInfo, 1, g_scene)
				showHeroDetailLayer.init(g_SceneTabel)	
			end
		else
			ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.5)					
		end
	elseif actionID == 1485 then
		local serverInfo=actionLayer._1485Callback(pScutScene, lpExternalData) ----魂技
		if serverInfo~=nil  and serverInfo ~={} then
			if serverInfo ~= nil then 
				HeroAccessory.setScene(g_SceneTabel)
				HeroAccessory.setData(serverInfo)
				local isNochangeBtn = true--是否没有更换按钮		
				HeroAccessory.showSkillDetailLayer(isNochangeBtn)
			end
		else
			ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.5)					
		end
	elseif  actionID == 1202  then
		local serverInfo=actionLayer._1202Callback(pScutScene, lpExternalData) ----装备
		if serverInfo~=nil  and serverInfo ~={} then
			if  serverInfo ~=" " and serverInfo ~= nil  then 
				HeroAccessory.setData(serverInfo)
				HeroAccessory.createEquipDetailLayer(g_SceneTabel)
			end
		else
			ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.5)					
		end
	end
end


function resetSkillCard(info)
 
	local DataTabel={}
	DataTabel.isStemp=true
	
	DataTabel.GeneralName= info.GeneralName
	DataTabel.HeadID= info.HeadID
	DataTabel.PicturesID=info.PicturesID
	DataTabel.GeneralQuality=info.GeneralQuality
	DataTabel.PowerNum=info.PowerNum
	DataTabel.SoulNum= info.SoulNum
	DataTabel.IntellectNum= info.IntellectNum
	DataTabel.CareerID=info.CareerID
	DataTabel.LifeNum= info.LifeNum
	DataTabel.GeneralLv=info.GeneralLv
	DataTabel.GeneralDesc= info.GeneralDesc
	DataTabel.UserItemID= info.TalentAbility
	DataTabel.TalentName=info.TalentName
	DataTabel.TalentAbilityQuality=info.TalentAbilityQuality
	DataTabel.TalentAbilityHead= info.TalentAbilityHead
	
	
	DataTabel.RecordTabel2={}
	DataTabel.RecordTabel2[1]={	}
	DataTabel.RecordTabel2[1].AbilityHead= info.TalentAbilityHead
	DataTabel.RecordTabel2[1].AbilityQuality= info.TalentAbilityQuality
	DataTabel.RecordTabel2[1].UserItemID=info.TalentAbility

	DataTabel.LifeMaxNum=info.LifeNum

	DataTabel.RecordTabel4=info.RecordTabel
		DataTabel.RecordTabel4[#DataTabel.RecordTabel4+1]={}
		DataTabel.RecordTabel4[#DataTabel.RecordTabel4].SkillID=101
		DataTabel.RecordTabel4[#DataTabel.RecordTabel4].SkillNum= info.PowerNum
		
		DataTabel.RecordTabel4[#DataTabel.RecordTabel4+1]={ }
		DataTabel.RecordTabel4[#DataTabel.RecordTabel4].SkillID=102
		DataTabel.RecordTabel4[#DataTabel.RecordTabel4].SkillNum= info.SoulNum
		
		DataTabel.RecordTabel4[#DataTabel.RecordTabel4+1]={ }
		DataTabel.RecordTabel4[#DataTabel.RecordTabel4].SkillID=103
		DataTabel.RecordTabel4[#DataTabel.RecordTabel4].SkillNum= info.IntellectNum
		

		
--		DataTabel.RecordTabel4[2]={ [SkillID]=101,  [SkillNum]=info.PowerNum  }
--		DataTabel.RecordTabel4[3]={ [SkillID]=102,  [SkillNum]=info.SoulNum  }
--		DataTabel.RecordTabel4[4]={ [SkillID]=103,  [SkillNum]=info.IntellectNum  }
--		DataTabel.RecordTabel4[1]={ [SkillID]=,  [SkillNum]= },
--		DataTabel.RecordTabel4[1]={ [SkillID]=,  [SkillNum]= },	
	

	
	
	
	
	
	
	local DataTabel2={}
	DataTabel2.ItemID= info.ItemID
	DataTabel2.ItemName= info.ItemName
	DataTabel2.MaxHeadID= info.ItemHead
	DataTabel2.QualityType= info.QualityType
	DataTabel2.Sellprice= info.SalePrice 
	DataTabel2.AbilityList = info.RecordTabel2;	
	DataTabel2.ItemDesc = info.ItemDesc
	
	local DataTabel3={}
	DataTabel3.AbilityID=info.AbilityID
	DataTabel3.AbilityName= info.AbilityName
	DataTabel3.MaxHeadID= info.AbilityHead
	DataTabel3.AbilityDesc=info.AbilityDesc
	DataTabel3.AbilityQuality= info.AbilityQuality
        
	table={}
	table.GeneralInfo = DataTabel
	table.EquipInfo = DataTabel2
	table.SkillInfo = DataTabel3
	
	return table

end



