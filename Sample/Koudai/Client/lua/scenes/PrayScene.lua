------------------------------------------------------------------
-- prayScene.lua.lua
-- Author     : yeyq

-- Version    : 1.0
-- Date       :
-- Description: 
------------------------------------------------------------------

module("prayScene", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

_scene = nil 		-- 场景
layerBG = nil
prayTable = {}  ---3301

DayTable ={
	"3",
	"5","7","9"
}


--
---------------公有接口(以下两个函数固定不用改变)--------
--

-- 函数名以小写字母开始，每个函数都应注释

-- 场景入口
function pushScene()
	initResource()
	init()
end

function close()
	if  showLayer~=nil  then
		showLayer:getParent():removeChild(showLayer,true)
		showLayer = nil 
	end;
	if  layerBG ~= nil  then
		layerBG:getParent():removeChild(layerBG,true)
		layerBG = nil
	end
end


-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
_scene = nil 		-- 场景
layerBG = nil
prayTable = {}  ---3301
showLayer=nil
DayTable ={
	"3",
	"5","7","9"
}

end

-- 释放资源
function releaseResource()
	close()
	_scene = nil 		-- 场景
	showLayer = nil
	layerBG = nil
	prayTable = {}  ---3301
end


-- 创建场景
function init(fatherScene,fatherLayer)
	_scene = fatherScene

	-- 添加背景
	layerBG = CCLayer:create()
	fatherLayer:addChild(layerBG, 0)
	-- 此处添加场景初始内容
	actionLayer.Action3301(_scene,false)
end
function showLayert()
	
	if  showLayer~=nil  then
		showLayer:getParent():removeChild(showLayer,true)
		
	end;
	
	
	
	showLayer = CCLayer:create()
	layerBG:addChild(showLayer, 0)



	local  dayNum = prayTable.PrayType 
	local nowDay= prayTable.IsPrayNum 
	local needDay= DayTable[dayNum]
	

	
	
	---半透明的背景
	local bgTouM = CCSprite:create(P(Image.image_toumingPath))
	local bgTouM_X = pWinSize.width*0.92
	bgTouM:setScaleX(bgTouM_X/bgTouM:getContentSize().width)
	bgTouM:setScaleY(pWinSize.height*0.705/bgTouM:getContentSize().height)
	bgTouM:setAnchorPoint(PT(0,0))
	bgTouM:setPosition(PT((pWinSize.width-bgTouM_X)/2,pWinSize.height*0.17+SY(9)))
	showLayer:addChild(bgTouM,0)	
	--------哆来A梦图
	
	local bImage = "common/list_1152.png"
	local aMengSprite = CCSprite:create(P(bImage))
	aMengSprite:setAnchorPoint(PT(0,0))
	aMengSprite:setPosition(PT(bgTouM:getPosition().x+SY(15),
					bgTouM:getPosition().y+SY(25)))
	showLayer:addChild(aMengSprite,0)
	
	------    祈祷提示语
	local prayString = Language.Pray_stirng
	local prayTxt = 	CCLabelTTF:create(prayString,FONT_NAME,FONT_SM_SIZE)
	showLayer:addChild(prayTxt,0)
	prayTxt:setAnchorPoint(PT(0,0))
	prayTxt:setPosition(PT(pWinSize.width*0.67,
				aMengSprite:getPosition().y+aMengSprite:getContentSize().height+SY(10)))
	
	local prayString = Language.Pray_stirng2
	local needStr = string.format(prayString,needDay or 0)
	local prayTxt = 	CCLabelTTF:create(needStr,FONT_NAME,FONT_SM_SIZE)
	showLayer:addChild(prayTxt,0)
	prayTxt:setAnchorPoint(PT(0,0))
	prayTxt:setPosition(PT(pWinSize.width*0.6,
							aMengSprite:getPosition().y+aMengSprite:getContentSize().height))
	
	
	
	local prayString = Language.Pray_stirng3
	local prayTxt = 	CCLabelTTF:create(prayString,FONT_NAME,FONT_SM_SIZE)
	showLayer:addChild(prayTxt,0)
	prayTxt:setAnchorPoint(PT(0,0))
	prayTxt:setPosition(PT(pWinSize.width*0.6,
								aMengSprite:getPosition().y+aMengSprite:getContentSize().height-SY(10)))

	---   祈祷按钮
	local pButtonImg = "common/list_1153.png"
	local pedButtonImg = "common/list_1153_1.png"
	local prayButton =ZyButton:new(pButtonImg,pButtonImg,pedButtonImg,nil,FONT_NAME,FONT_SM_SIZE)
	prayButton:setAnchorPoint(PT(0,0))
	prayButton:setPosition(PT(pWinSize.width*0.6,aMengSprite:getPosition().y+SY(2)))
	prayButton:addto(showLayer,0)
	prayButton:registerScriptHandler(onClick)
	
	local IsStatu  = prayTable.IsStatu 
	if  IsStatu == 1 then
		prayButton:setEnabled(false)	
	end 
	
	
		-----祈祷的次数 及需要祈祷的次数

	local prayNumString = Language.Pray_Num
	
	local  dayNum = prayTable.PrayType 
	local nowDay= prayTable.IsPrayNum 
	local needDay= DayTable[dayNum]
	
	
	if nowDay== nil   then 
		nowDay= 0
	end
	if  needDay == nil  then
		needDay= 0		
	end
	 
	
	local prayNum = string.format(prayNumString,nowDay,needDay)
	local prayTxt = 	CCLabelTTF:create(prayNum,FONT_NAME,FONT_SM_SIZE)
	showLayer:addChild(prayTxt,0)
	prayTxt:setAnchorPoint(PT(0,0))
	prayTxt:setPosition(PT(pWinSize.width*0.62,aMengSprite:getPosition().y-SY(8)))


end

function  onClick()
	actionLayer.Action3302(_scene,nil)
end


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
	local actionID = ZyReader:getActionID()
	if actionID == 3301 then 
		local serverInfo=actionLayer._3301Callback(pScutScene, lpExternalData)
		if serverInfo~=nil and serverInfo ~= " " then
			prayTable = serverInfo
			showLayert()				
		end
	end 
	if actionID == 3302 then
		local serverInfo1 = actionLayer._3302Callback(pScutScene, lpExternalData)
		if serverInfo1 ~= nil  then 
			local rewardStr	= serverInfo1.Cue
			if rewardStr~=nil and rewardStr ~= ""  then
				local rewardStr1 = string.format(Language.Pray_huojiang,rewardStr)
				ZyToast.show(pScutScene,rewardStr1,1,0.5)
			else
				ZyToast.show(pScutScene,Language.Pray_jiaoyou,1,0.5)
			end
			actionLayer.Action3301(_scene,nil)				
		end 
	end
	local userData = ScutRequestParam:getParamData(lpExternalData)
end