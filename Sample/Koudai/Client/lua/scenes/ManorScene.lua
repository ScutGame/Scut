------------------------------------------------------------------
-- ManorScene.lua.lua
-- Author     : yeyq

-- Version    : 1.0
-- Date       :
-- Description: 
------------------------------------------------------------------


------------------------------------------------------------------

module("ManorScene", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local mScene = nil 		            -- 场景
local mLayer=nil
---------------公有接口(以下两个函数固定不用改变)--------
--

-- 函数名以小写字母开始，每个函数都应注释
local mServerData=nil
local _plantInfo={{},{},{},{},{},{},{},{},{}}
local bgTexSize=nil
local LineLayer=nil
local touchLand=nil
local DewNumLable=nil
local lastPositionTag=nil
local landLayer=nil
local choiceLayer=nil
local detailLayer=nil
--
local timeBegin=nil
local GeneralInfo=nil
local mCurrentIndex=1

local mCurrentId=1

--当前的是经验还是金钱
local currentTag=nil

--当前土地位置
local currentLandPosition=nil
local currentRefreshType=nil
local mContentLayer=nil
local PlantQualityTable={
Language.PLANT_PUTONG,Language.PLANT_YOUXIU,
Language.PLANT_JINGLIANG,Language.PLANT_CHUANQI,
Language.PLANT_SHENHUA,
}
local isClick=nil
local currentQuality
local clearLandPosition=nil
local rewardIndex=nil
local mImgSize=nil
local showLabel=nil
local gotoPlantId=nil
local mBigWheel=nil
local mChoiceSprite=nil
local mCurrentSprite=nil

local mMidBgSprite=nil
--
-------------------------私有接口------------------------
--

-- 释放资源
function releaseResource()

timeBegin=false
showLabel=nil

mContentLayer=nil
detailLayer=nil
choiceLayer=nil
buyFairyBtn=nil
landLayer=nil
mLayer=nil
mScene = nil 		    -- 场景

_plantInfo={{},{},{},{},{},{},{},{},{}}
bgTexSize=nil
 LineLayer=nil

 DewNumLable=nil
 landLayer=nil
 choiceLayer=nil
 detailLayer=nil
 mContentLayer=nil
 isClick=nil
 mCurrentIndex=1
 gotoPlantId=nil
mCurrentSprite=nil
mChoiceSprite=nil
mBigWheel=nil
--CCScheduler:sharedScheduler():unscheduleScriptFunc("ManorScene.timeElapse")
	
--	 CCDirector:sharedDirector():getScheduler():unscheduleScriptEntry(schedulerEntry1)
end



function close()

	if  mContentLayer ~=nil   then
			mContentLayer:getParent():removeChild(mContentLayer,true)
			mContentLayer =nil
	end
	if  detailLayer ~= nil    then
			detailLayer:getParent():removeChild(detailLayer,true)
			detailLayer =nil
	end
	
	if  choiceLayer ~= nil   then 
		choiceLayer:getParent():removeChild(choiceLayer,true)
		choiceLayer =nil
	end
	if  buyFairyBtn ~= nil   then 
		buyFairyBtn:getParent():removeChild(buyFairyBtn,true)
		buyFairyBtn =nil
	end

	if  landLayer~=nil   then
		landLayer:getParent():removeChild(landLayer,true) 
		landLayer = nil 
	end
	
	if   mLayer~=nil  then 
		mLayer:getParent():removeChild(mLayer,true)
		mLayer = nil 
	end
	
	releaseResource()
end

--------------------------点击响应  画矩形
function  DrewBox(position)
	if mImgSize and landLayer and position then
	releaseDrawBox()
	local layer=CCNode:create()
	LineLayer=layer
	layer:setContentSize(mImgSize)
	 --左边的线
        local lineLeft=NdLineNode:lineWithPoint(
        PT(0,mImgSize.height/2),
        PT(mImgSize.width/2+SX(2),0),
        1,ccc4(255,255, 255, 0))
        layer:addChild(lineLeft,0)
        
        -- 底下的线
        local lineDown=NdLineNode:lineWithPoint(
        PT(mImgSize.width/2+SX(2),0),
        PT(mImgSize.width,mImgSize.height/2),
        1,ccc4(255,255, 255, 0))
        layer:addChild(lineDown,0)
        
	 -- 右边的线
        local lineRight=NdLineNode:lineWithPoint(
        PT(mImgSize.width,mImgSize.height/2),
        PT(mImgSize.width/2,mImgSize.height),
        1,ccc4(255,255, 255, 0))
        layer:addChild(lineRight,0)
         
        --上面的线
        local lineUp=NdLineNode:lineWithPoint(
        PT(mImgSize.width/2,mImgSize.height),
        PT(0,mImgSize.height/2),
        1,ccc4(255,255, 255, 0))
        layer:addChild(lineUp,0)   
        layer:setAnchorPoint(PT(0,0))
        layer:setPosition(PT(0,0))
        layer:setPosition(position)
        landLayer:addChild(layer,2)    
	end	
end;

function  releaseDrawBox()
		if LineLayer~=nil then
			LineLayer:getParent():removeChild(LineLayer,true)
			LineLayer=nil
		end
end;
function onTouch(eventType , x, y)
    if eventType == "began" then 
        return touchBegan(x,y)
    elseif eventType == "moved" then
       -- return touchMove(x,y)
    elseif eventType == "ended" then 
        return touchEnd(x,y)
    elseif eventType == "cancelled" then 
        return touchEnd(x,y)
    end
end 
---初始化
function initUI(MScene,fLayer)
	
	mScene = MScene
	local layer=CCLayer:create()
	fLayer:addChild(layer, 0)	
	mLayer = layer
	
	mLayer:setTouchEnabled(true)
	 mLayer:registerScriptTouchHandler(onTouch)
	--mLayer.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHBEGAN, "ManorScene.touchBegan")
	--mLayer.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHMOVED, "ManorScene.touchMove")
	--mLayer.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHENDED, "ManorScene.touchEnd")

	--背景
	
	local bgSprite=CCSprite:create(P("zhuanyuan/list_1124_2.png"))

	bgSprite:setAnchorPoint(PT(0.5,0))
	bgSprite:setScaleX(pWinSize.width*0.92/bgSprite:getContentSize().width)
	bgSprite:setScaleY(pWinSize.height*0.69/bgSprite:getContentSize().height)
	bgSprite:setPosition(PT(pWinSize.width*0.5,pWinSize.height*0.215))
	mLayer:addChild(bgSprite,0)
	
	timeBegin=false
	actionLayer.Action10001(mScene,false)
end

--关闭按扭响应
-- 触屏按下
function touchBegan(x,y)
	if showLabel then
		  showLabel:stopAllActions()
		  showLabel:removeFromParentAndCleanup(true)
		  showLabel=nil
	end
	touchLand=false
	if _plantInfo then 
	     --for k,v in ipairs(e) do	
			--local   pointBegin = v:locationInView(v:view())
			--pointBegin = CCDirector:sharedDirector():convertToGL(pointBegin)
			pointBegin = PT(x,y)
			for k=1, 9 do    
		     		local pos , rect=getPlantPosition(k)
		     		if pos and rect and CCRect.containsPoint(rect, pointBegin) then
						if _plantInfo[k].IsOpen==1 and _plantInfo[k].IsGain~=1  and _plantInfo[k].ColdTime<=0   then
							DrewBox(pos)
							touchLand=true
							currentLandPosition=k
							return true	
							
					       end
				    end				  
		        end
	    --end
     end    
     return 1 
end

-- 触屏弹起
function touchEnd(x,y)
	releaseDrawBox()
	    --判断Begin跟End 是否点击同一物体--
        if   touchLand  then
		showChoiceLayer()		
        end
       touchLand=false
end

-----------------------种植土地层
function  releaseLandLayer()
	if landLayer~=nil then
		landLayer:getParent():removeChild(landLayer,true)
		landLayer=nil
	end
end;


---显示土地背景  刷新界面
function showPlantLayer()
    	--添加背景图片 	
	isClick=true
	releaseLandLayer()
	landLayer=CCLayer:create()
	landLayer:setAnchorPoint(PT(0,0))
	landLayer:setPosition(PT(0,0))
	mLayer:addChild(landLayer, 0)
	local midSprite=CCSprite:create(P("zhuanyuan/list_1124_1.png"))
	midSprite:setAnchorPoint(PT(0.5,0.5))
	midSprite:setPosition(PT(pWinSize.width*0.5,pWinSize.height*0.6))
	mMidBgSprite=midSprite
	landLayer:addChild(midSprite,0)
	bgTexSize =midSprite:getContentSize()
	
	local mBgTecture=IMAGE("zhuanyuan/list_1127.png")
	local sprite=CCSprite:createWithTexture(mBgTecture)    
	mImgSize=sprite:getContentSize()
	local lastPosition=nil	
	for k, v in ipairs(_plantInfo) do
	----绘制地表图片
			--获取位置
		local  pos=getPlantPosition(k)
		local landSprite=nil
		--如果没有开启
		if not  v.IsOpen then	
			landSprite=CCSprite:createWithTexture(mBgTecture)
			landSprite:setAnchorPoint(PT(0,0))
			landSprite:setPosition(pos)
			landLayer:addChild(landSprite, 0)
			if lastPosition==nil then
				lastPosition=pos
				lastPositionTag=k
			end	
		else
			--判断开启的是什么类型的土地
			local path=nil
			if v.IsBlackLand==1 then
				path="zhuanyuan/list_1130.png"
			elseif v.IsRedLand==1 then
				path="zhuanyuan/list_1129.png"
			else
				path="zhuanyuan/list_1128.png"
              	end
      			landSprite = CCSprite:create(P(path))
			landSprite:setAnchorPoint(PT(0,0))
			landSprite:setPosition(pos)
			landLayer:addChild(landSprite,1)
		end

		--判断是否可以收获了
		if v.IsGain==1 then
			--收获的是什么类型
			local spritePath="zhuanyuan/list_1125.png"
			if v.PlantType==2 then
				spritePath="zhuanyuan/list_1126.png"
			end
			local getResultTree=ZyButton:new(spritePath)
			v.resultPos=PT(pos.x+landSprite:getContentSize().width/2,
							pos.y+landSprite:getContentSize().height/10*6)
			getResultTree:setAnchorPoint(PT(0.5,0))
			getResultTree:setPosition(PT(pos.x+landSprite:getContentSize().width/2,pos.y+landSprite:getContentSize().height*0.2))
			getResultTree:addto(landLayer,9)
			getResultTree:setTag(k)
			getResultTree:registerScriptHandler(getResultAction)
		end
		--如果有时间则显示时间
		 v.ColdTime = v.ColdTime  or 0
		if  v.ColdTime>0 then
			local label,btn= createTimeLabel(v.ColdTime)
			v.timeLabel=label
			 local boxSize=SZ((label:getContentSize().width+btn:getContentSize().width),btn:getContentSize().height)
       		 local actionBtn=UIHelper.createActionRect(boxSize,ManorScene.clearTimeAction,k)
        
			label:setPosition(PT(pos.x+landSprite:getContentSize().width*0.4,pos.y+landSprite:getContentSize().height/10*6))
			btn:setPosition(PT(label:getPosition().x+label:getContentSize().width/2,
										label:getPosition().y))
			actionBtn:setPosition(PT(label:getPosition().x-label:getContentSize().width/2,
											label:getPosition().y-label:getContentSize().height/2))						
			landLayer:addChild(btn,9)
			landLayer:addChild(label, 9)
			landLayer:addChild(actionBtn, 9)
		end
		
	end
	
	--可以开启的位置显示一棵树
	if lastPosition ~=nil then	
		local treeButton=ZyButton:new("zhuanyuan/list_1131.png")
		treeButton:setAnchorPoint(PT(0.5,0))
		treeButton:setPosition(PT(lastPosition.x+mImgSize.width/2,lastPosition.y+mImgSize.height/4))
		treeButton:addto(landLayer,3)
		treeButton:registerScriptHandler(expansionLand)
	end	

	--底下的操作按钮
	local bgImg = CCSprite:create(P("common/list_1052.9.png"))
	bgImg:setScaleY(pWinSize.height*0.15/bgImg:getContentSize().height)
	bgImg:setAnchorPoint(PT(0.5,0))
	bgImg:setPosition(PT(pWinSize.width*0.5,pWinSize.height*0.22))
	landLayer:addChild(bgImg,0)
	
	local colW=bgImg:getContentSize().width/3
	local startY=pWinSize.height*0.24
	--仙露
	if mServerData.IsShow==1 then
		local buyFairyBtn=ZyButton:new("zhuanyuan/list_1134.png")
		buyFairyBtn:setAnchorPoint(PT(0.5,0))
		buyFairyBtn:setPosition(PT(pWinSize.width*0.5,startY))
		buyFairyBtn:addto(landLayer,3)
		buyFairyBtn:registerScriptHandler(buyFairyAction)		
		mServerData.DewNum =mServerData.DewNum or 0
		local  label=CCLabelTTF:create("" .. mServerData.DewNum,FONT_NAME,FONT_SM_SIZE)
		DewNumLable=label
		label:setAnchorPoint(PT(0,1))
		label:setPosition(PT(buyFairyBtn:getContentSize().width,buyFairyBtn:getContentSize().height))
		buyFairyBtn:addChild(label,0)
	end

	if PersonalInfo.getPersonalInfo()._VipLv >= 4 then
		local LevelUpBtn=ZyButton:new("zhuanyuan/list_1133.png")
		LevelUpBtn:setAnchorPoint(PT(0.5,0))
		LevelUpBtn:setPosition(PT(pWinSize.width*0.5+colW,startY))
		LevelUpBtn:addto(landLayer,3)
		LevelUpBtn:registerScriptHandler(levelUpRedLand)
		
		local LevelUpBlackBtn=ZyButton:new("zhuanyuan/list_1132.png")
		LevelUpBlackBtn:setAnchorPoint(PT(0.5,0))
		LevelUpBlackBtn:setPosition(PT(pWinSize.width*0.5-colW,startY))
		LevelUpBlackBtn:addto(landLayer,3)
		LevelUpBlackBtn:registerScriptHandler(levelUpBlackLand)
	end
	isClick=false
	
	
	timeBegin=true
	schedulerEntry1 = 	CCDirector:sharedDirector():getScheduler():scheduleScriptFunc(timeElapse, 1, false)
	--CCScheduler:sharedScheduler():scheduleScriptFunc("ManorScene.timeElapse", 1, false)  
	--]]
end

--
function  createTimeLabel(times,fun,tag)
	  --时间
        local strTime = formatTime(times)
      	 local   lbText = CCLabelTTF:create(strTime, FONT_NAME, FONT_SM_SIZE)
        lbText:setColor(ccc3(255,255,255)) 
        lbText:setAnchorPoint(PT(0.5,0.5)); 
        local sprite=CCSprite:create(P(Image.image_button_jiantou))
        sprite:setAnchorPoint(PT(0,0.5))
        return lbText,sprite
end

--
function getResultAction(node)
	if not isClick then
		isClick=true
	local pos=node:getTag()
	rewardIndex=nil
	rewardIndex=pos
	local landInfo=_plantInfo[rewardIndex]
	actionLayer.Action10006(mScene,false,landInfo.PlantType,landInfo.GeneralID,rewardIndex)
	end
end

---
function  showRewardNum(num)
	if num~=nil  and _plantInfo[rewardIndex] and _plantInfo[rewardIndex].resultPos then
	local landInfo=_plantInfo[rewardIndex]
	local pos=landInfo.resultPos
	local str=Language.PLANT_EXP
	if landInfo.PlantType==2 then
		str=Language.PLANT_GOLD
	end
	local label=CCLabelTTF:create(str .. "+" .. num,FONT_NAME,FONT_SM_SIZE)
	label:setPosition(pos)
	label:setAnchorPoint(PT(0.5,0))
	mLayer:addChild(label,9)
	label:setColor(ccc3(0,255,0))
	local point=PT(pos.x,pos.y+SY(10))
	local    action1 = CCMoveTo:create(0.3, point)
	local    action2 = CCFadeOut:create(0.5)
	local 	funcName = CCCallFuncN:create(ManorScene.showRewordFinish)
	local 	action3 = CCSequence:createWithTwoActions(action2,funcName)
	local    actionHarm=CCSequence:createWithTwoActions(action1,action3)
	label:runAction(actionHarm)
	showLabel=label
	end
end;

----
function showRewordFinish(label)
    if label ~= nil then
        label:removeFromParentAndCleanup(true)
        label = nil
        showLabel=nil
    end   
end;

----时间控制
function  clearTimeAction(item)
	if not isClick then
		isClick=true
		local tag=item:getTag()
		clearLandPosition=tag
		actionLayer.Action10007(mScene,nil,tag,1)
 	end
end;


function timeElapse(dt)
  if  timeBegin and _plantInfo and #_plantInfo>0 then
    for k, v in ipairs(_plantInfo) do
          if v.ColdTime~=nil  and v.ColdTime>0  and landLayer~=nil then
                    v.ColdTime = v.ColdTime- tonumber(dt)
                   if v.ColdTime<=0 then
                        timeBegin=false
			    actionLayer.Action10001(mScene,nil)
                   else
                         if  landLayer~=nil and v.timeLabel~=nil then
		                        v.timeLabel:setString(formatTime(v.ColdTime))
		           end  
                   end
          end
    end
   end
 end

-----------选择要种植的树 选择界面
function closeChoiceLayer()
	if choiceLayer~=nil then
		choiceLayer:getParent():removeChild(choiceLayer,true)
		choiceLayer=nil
	end
end;

function  showChoiceLayer()
	local time= _plantInfo[currentLandPosition].ColdTime
	clearLandPosition=currentLandPosition
	if time~=nil and time>0 then
		actionLayer.Action10007(mScene,nil,currentLandPosition,1)
	else
	if choiceLayer  then
		return
	end
	choiceLayer=CCLayer:create()
	choiceLayer:setAnchorPoint(PT(0,0))
	choiceLayer:setPosition(PT(0,0))
	mLayer:addChild(choiceLayer,2)
	
	local boxSize=SZ(pWinSize.width/3,pWinSize.height/3)
	local bottomBtn=ZyButton:new("common/tou_ming.9.png")
	bottomBtn:setScaleX(pWinSize.width/bottomBtn:getContentSize().width)
	bottomBtn:setScaleY(pWinSize.height/bottomBtn:getContentSize().height)
	bottomBtn:setAnchorPoint(PT(0,0))
	bottomBtn:setPosition(PT(0,0))
	bottomBtn:addto(choiceLayer,0)
	bottomBtn:registerScriptHandler(closeChoiceLayer)	
	--背景
	local bgSprite=CCSprite:create(P("common/List_2008.9.png"))
	bgSprite:setScaleY(pWinSize.height*0.15/bgSprite:getContentSize().height)
	bgSprite:setScaleX(pWinSize.width*0.4/bgSprite:getContentSize().width)
	
	bgSprite:setAnchorPoint(PT(0.5,0.5))
	bgSprite:setPosition(PT(pWinSize.width/2,pWinSize.height/2))
	choiceLayer:addChild(bgSprite,1)	

	--
	local colW=boxSize.width/2
	local startX=pWinSize.width/2
	local startY=pWinSize.height/2
	local personralInfo=PersonalInfo.getPersonalInfo()
	local table={{tag=1,pic="zhuanyuan/bottom_1013_1.png"}}
	if mServerData.IsShow==1 then
		 table={{tag=1,pic="zhuanyuan/bottom_1013_1.png"},{tag=2,pic="zhuanyuan/bottom_1014.png"}}
		 startX=pWinSize.width/2-boxSize.width/4
	end
	for k, v in pairs(table) do
		local button=ZyButton:new(v.pic)
		button:setAnchorPoint(PT(0.5,0.5))
		button:setPosition(PT(startX+(k-1)*colW,startY))
		button:setTag(v.tag)
		button:addto(choiceLayer,1)
		button:registerScriptHandler(choiceTreeAction)
	end
	end	
	
end;

--选择要种植的树
function  choiceTreeAction(item)
	if not isClick then
		isClick=true
		local tag=item:getTag()
		currentTag=tag
		actionLayer.Action10002(mScene,nil)
	end
end;

---详细界面
function  releaseDetailLayer()
	mChoiceSprite=nil
	 releaseContentLayer()
	if detailLayer~=nil then
		detailLayer:getParent():removeChild(detailLayer,true)
		detailLayer=nil
	end
end;

function  showDetailLayer()
	if detailLayer then
		return
	end
	closeChoiceLayer()
	detailLayer=CCLayer:create()
	mScene:addChild(detailLayer,3)
	--
	local startY=pWinSize.height*0.145
	local boxSize=SZ(pWinSize.width,pWinSize.height*0.855)
	local bottomBtn=UIHelper.createActionRect(boxSize)
	bottomBtn:setPosition(PT(0,startY))
	detailLayer:addChild(bottomBtn,0)
	
	--背景
	local bgSprite=CCSprite:create(P("common/list_1024.png"))
	bgSprite:setScaleX(boxSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(boxSize.height/bgSprite:getContentSize().height)
	bgSprite:setAnchorPoint(PT(0.5,0))
	bgSprite:setPosition(PT(pWinSize.width/2,startY))
	detailLayer:addChild(bgSprite,0)
	
	
	local midSize=SZ(boxSize.width*0.92,boxSize.height*0.8)
	local FgSprite=CCSprite:create(P("common/list_1038.9.png"))
	FgSprite:setAnchorPoint(PT(0.5,0))
	FgSprite:setScaleX(midSize.width/FgSprite:getContentSize().width)
	FgSprite:setScaleY(midSize.height/FgSprite:getContentSize().height)
	FgSprite:setPosition(PT(pWinSize.width*0.5,startY+boxSize.height*0.08))
	detailLayer:addChild(FgSprite,0)
		
	----佣兵框
	local bgImg = CCSprite:create(P("common/list_1052.9.png"))
	
	local listBgBox=SZ(bgImg:getContentSize().width,pWinSize.height*0.12)
	bgImg:setScaleY(listBgBox.height/bgImg:getContentSize().height)
	bgImg:setAnchorPoint(PT(0.5,0))
	bgImg:setPosition(PT(pWinSize.width/2,pWinSize.height*0.86))
	detailLayer:addChild(bgImg,0)

	 ---  两个左右按钮
	local startX=pWinSize.width/2-listBgBox.width/2
	local lSprite=CCSprite:create(P("button/list_1069.png"))
	lSprite:setAnchorPoint(PT(1,0.5))
	lSprite:setPosition(PT(startX,bgImg:getPosition().y+listBgBox.height/2))
	detailLayer:addChild(lSprite,0)
	
	local rSprite=CCSprite:create(P("button/list_1068.png"))
	rSprite:setAnchorPoint(PT(0,0.5))
	rSprite:setPosition(PT(bgImg:getPosition().x+listBgBox.width/2,
							lSprite:getPosition().y))
	detailLayer:addChild(rSprite,0)
	

	local listSize = SZ(listBgBox.width*0.98,listBgBox.height)
	local listX= startX+(listBgBox.width-listSize.width)/2
	local listY= bgImg:getPosition().y
	
	local mBgTecture=IMAGE("common/list_1012.png")
	local widthSprite=CCSprite:createWithTexture(mBgTecture)
	
	local listColW=widthSprite:getContentSize().width*1.2
	local list = ScutCxList:node(listColW,ccc4(24,24,24,0),listSize)
	list:setHorizontal(true)
	list:setTouchEnabled(true)
	list:setPosition(PT(listX,listY))
	mList=list
	detailLayer:addChild(list,0)
	
	local layout=CxLayout()
	layout.val_x.t = ABS_WITH_PIXEL
	layout.val_y.t = ABS_WITH_PIXEL
	layout.val_x.val.pixel_val =0
	layout.val_y.val.pixel_val =0
	local mBgTecture=IMAGE("common/list_1012.png")
	for k , v in pairs (GeneralInfo) do
			if v then
			local item = ScutCxListItem:itemWithColor(ccc3(25,57,45))
			item:setOpacity(0)

			local layer=CCLayer:create()
			local sparBgSprite=CCSprite:createWithTexture(mBgTecture)
			sparBgSprite:setAnchorPoint(PT(0,0))
			sparBgSprite:setPosition(PT((listColW/2-sparBgSprite:getContentSize().width/2),
								(list:getContentSize().height/2-sparBgSprite:getContentSize().height/2)
			))
			layer:addChild(sparBgSprite,0)
			
			local actionBtn,pNode=UIHelper.createActionRect(sparBgSprite:getContentSize(),ManorScene.rangerButtonFunc,k)
			actionBtn:setPosition(PT(0,0))
			sparBgSprite:addChild(actionBtn,0)
			
			local imagePath=string.format("smallitem/%s.png",v.HeadID)
			local headSprite=CCSprite:create(P(imagePath))
			headSprite:setAnchorPoint(PT(0.5,0.5))
			headSprite:setPosition(PT(sparBgSprite:getContentSize().width/2,sparBgSprite:getContentSize().height/2))
			sparBgSprite:addChild(headSprite,0)
			if mCurrentIndex==k then
				cretaeChoiceSprite(pNode)
			end

			item:addChildItem(layer, layout)
			list:addListItem(item,false)	
			end
	end
	actionLayer.Action10003(mScene,false,currentTag,GeneralInfo[mCurrentIndex].GeneralID,currentLandPosition)
end

function releaseChoiceSprite()
	if mChoiceSprite then
		mChoiceSprite:getParent():removeChild(mChoiceSprite,true)
		mChoiceSprite=nil
	end
end;

function  cretaeChoiceSprite(pNode)
	releaseChoiceSprite()
	mChoiceSprite=CCSprite:create(P(Image.Image_choicebox))
	mChoiceSprite:setAnchorPoint(CCPoint(0.5, 0.5))
	mChoiceSprite:setPosition(PT(pNode:getContentSize().width*0.5,pNode:getContentSize().height*0.5))
	mChoiceSprite:setScaleX((pNode:getContentSize().width)/mChoiceSprite:getContentSize().width)
	mChoiceSprite:setScaleY((pNode:getContentSize().height)/mChoiceSprite:getContentSize().height)
	pNode:addChild(mChoiceSprite,0)
end;


	---- 种植界面的佣兵点击
function rangerButtonFunc(pNode)
	local index = pNode:getTag()
	if index~=mCurrentIndex then
		cretaeChoiceSprite(pNode)
		mCurrentIndex=index
		releaseContentLayer()
		local generalID = GeneralInfo[index].GeneralID 
	      actionLayer.Action10003(mScene,nil,currentTag,generalID,currentLandPosition)
	end
end

----单个佣兵详细信息  刷10003
function  releaseContentLayer()
	releaseBigWheel()
	if mContentLayer~=nil then
		mContentLayer:getParent():removeChild(mContentLayer,true)
		mContentLayer=nil
	end
end;

function  showDetailContent(info)
	if  not detailLayer then
		return
	end
	releaseContentLayer()
	local layer =CCLayer:create()
	mContentLayer=layer
	detailLayer:addChild(layer,2)
	currentQuality=info.PlantQualityType or  1

	local bgSprite=CCSprite:create(P("common/list_1052.9.png"))
	bgSprite:setAnchorPoint(PT(0.5,0))
	local infoBox=SZ(bgSprite:getContentSize().width,pWinSize.height*0.15)
	bgSprite:setScaleY(infoBox.height/bgSprite:getContentSize().height)
	bgSprite:setPosition(PT(pWinSize.width/2,pWinSize.height*0.22))
	mContentLayer:addChild(bgSprite,0)

	---种植品质
	local str=Language.PLANT_EXP 
	if currentTag==2 then
		str=Language.PLANT_GOLD 
	end
		---佣兵等级  品质   获取的经验
	local dateTable={
	Language.MERCENARIES_LV .. info.GeneralLv,
	Language.PLANT_QUALITY .. PlantQualityTable[currentQuality],
	str .. info.RewardNum or 0
	}
	local startX=bgSprite:getPosition().x-pWinSize.width*0.1
	local startY=bgSprite:getPosition().y+infoBox.height*0.7
	local rowH=infoBox.height/6 
	for k, v in pairs(dateTable) do
		local label=CCLabelTTF:create(v,FONT_NAME,FONT_SM_SIZE)
		label:setAnchorPoint(PT(0,0))
		label:setPosition(PT(startX,startY-rowH*(k-1)))
		mContentLayer:addChild(label,0)
	end

	local PlantBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.PLANT_BUTTON)
	PlantBtn:setAnchorPoint(PT(0,0))
	PlantBtn:setPosition(PT(bgSprite:getPosition().x-infoBox.width*0.4,
							bgSprite:getPosition().y+infoBox.height/2-PlantBtn:getContentSize().height/2))
	PlantBtn:addto(mContentLayer,1)
	PlantBtn:registerScriptHandler(plantAction)
	
	local CloseButton = ZyButton:new("button/list_1039.png",nil,nil,Language.IDS_BACK)
	CloseButton:setAnchorPoint(PT(0,0))
	CloseButton:setPosition(PT(bgSprite:getPosition().x+infoBox.width*0.4-CloseButton:getContentSize().width,
									PlantBtn:getPosition().y))	
	CloseButton:addto(mContentLayer,1)
	CloseButton:registerScriptHandler(closeDetailLayer)
	
	local startY=bgSprite:getPosition().y+infoBox.height+SY(2)
	
	local refreshBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.PLANT_REFRESH)
	refreshBtn:setAnchorPoint(PT(0,0))
	refreshBtn:setPosition(PT(pWinSize.width*0.18,startY))   
	refreshBtn:addto(mContentLayer,1)
	refreshBtn:setTag(1)
	refreshBtn:registerScriptHandler(refreshAction)
	
	if isShowVip() then
		local fullLevelBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.PLANT_FULLLEVEL)
		fullLevelBtn:setAnchorPoint(PT(0,0))
		fullLevelBtn:setPosition(PT(pWinSize.width*0.68,startY))   
		fullLevelBtn:addto(mContentLayer,1)
		fullLevelBtn:setTag(2)
		fullLevelBtn:registerScriptHandler(refreshAction)
	end
	
	 createBigWheel(info) 
end;

--释放大转盘
function  releaseBigWheel()
	releaseCurrentSprite()
	if mBigWheel then
		mBigWheel:getParent():removeChild(mBigWheel,true)
		mBigWheel=nil
	end
	mWheelBg=nil
end;

--大转盘
function createBigWheel(info)
	releaseBigWheel()
	local layer=CCLayer:create()
	mBigWheel=layer
	--------
	mContentLayer:addChild(layer,0)
	local startY=pWinSize.height*0.65
	local bgSprite=CCSprite:create(P("common/list_1120.png"))
	bgSprite:setAnchorPoint(PT(0.5,0.5))
	bgSprite:setPosition(PT(pWinSize.width/2,startY))
	layer:addChild(bgSprite,0)
	mWheelBg=bgSprite
	local col=11
	local rowH=bgSprite:getContentSize().height
	local colW=bgSprite:getContentSize().width
	startY=rowH/2
	local startX=colW/2
	rotationValue={
	[1]={rotation=0,pos=PT(colW/2,rowH*0.15),image="smallitem/icon_8005.png"},
	[2]={rotation=-72,pos=PT(colW*0.83,rowH*0.4),image="smallitem/icon_8006.png"},
	[3]={rotation=-144,pos=PT(colW*0.7,rowH*0.75),image="smallitem/icon_8007.png"},
	[4]={rotation=144,pos=PT(colW*0.3,rowH*0.75),image="smallitem/icon_8008.png"},
	[5]={rotation=72,pos=PT(colW*0.17,rowH*0.4),image="smallitem/icon_8009.png"},
	}
	gotoPlantId=info.PlantQualityType or 1
	if not mCurrentId then
		mCurrentId=gotoPlantId
	end
	
	--
	local mBgTecture=IMAGE("common/list_1088.png")
	mWheelBg:setRotation((mCurrentId-1)*72)
	for k, v in pairs(rotationValue) do				
		local path=v.image
		local headSprite=CCSprite:create(P(path))
		local fun=ManorScene.huntSparAction	
		v.headSprite=headSprite
		headSprite:setAnchorPoint(PT(0.5,0.5))
		headSprite:setPosition(v.pos)
		headSprite:setRotation(-(mCurrentId-1)*72)
		bgSprite:addChild(headSprite,0)	
		--人物背景
		local spriteBg=CCSprite:createWithTexture(mBgTecture)
		spriteBg:setAnchorPoint(PT(0.5,0.5))
		spriteBg:setPosition(PT(headSprite:getContentSize().width/2,
									headSprite:getContentSize().height/2))
		headSprite:addChild(spriteBg,-1)			
		--品质
		local moneyLabel=CCLabelTTF:create(PlantQualityTable[k] ,FONT_NAME,FONT_SM_SIZE)
		moneyLabel:setAnchorPoint(PT(0.5,1))
		moneyLabel:setPosition(PT(headSprite:getContentSize().width/2,-SY(5)))
		headSprite:addChild(moneyLabel,-1)							
		--local actionBtn=UIHelper.createActionRect(headSprite:getContentSize(),fun,k)
		--actionBtn:setPosition(PT(0,0))
		--headSprite:addChild(actionBtn,0)
	end
	rotationAction(gotoPlantId)

end;

---转动大转盘
function  rotationAction(id)
	isClick=true
	releaseCurrentSprite()
	local wheelNum=(id-mCurrentId)*72
	local speedTime=0.5*math.abs(id-mCurrentId)
	mCurrentId=id
	local funName="ManorScene.wheelMoveEnd"
	local rotationAt = CCRotateBy:create(speedTime,wheelNum);
   	rotationAt = CCSequence:createWithTwoActions(rotationAt,CCCallFunc:create(funName));
	mWheelBg:runAction(rotationAt)
	for k , v in pairs(rotationValue) do
		local num=-wheelNum
		local  rotationAt = CCRotateBy:create(speedTime,num);
		v.headSprite:runAction(rotationAt)			
	end
end;

--大转盘移动结束
function wheelMoveEnd()
	isClick=false
	 craeteCurrentSprite()
end;

--选中精灵
function releaseCurrentSprite()
	if mCurrentSprite then
		mCurrentSprite:getParent():removeChild(mCurrentSprite,true)
		mCurrentSprite=nil
	end
end;

function craeteCurrentSprite()
	releaseCurrentSprite()
	local parent=rotationValue[mCurrentId].headSprite
	local currentSprite=CCSprite:create(P("common/list_1121.png"))
	currentSprite:setAnchorPoint(PT(0.5,0.5))
	mCurrentSprite=currentSprite
	currentSprite:setPosition(PT(parent:getContentSize().width/2,parent:getContentSize().height/2))
	parent:addChild(currentSprite,0)		
end;


---种植
function  plantAction()
	if not isClick then
	isClick=true
	actionLayer.Action10004(mScene,nil,currentTag,GeneralInfo[mCurrentIndex].GeneralID,currentQuality,currentLandPosition)
	end
end;

--刷新
function  refreshAction(item)
	if not isClick then
	 isClick=true
	local tag=item:getTag()
	currentRefreshType=tag
	actionLayer.Action10005(mScene,nil,currentTag,GeneralInfo[mCurrentIndex].GeneralID,tag,1)
	end
end


-----改变佣兵
function callbackChangeMan (bar, pNode)
    local index =pNode:getTag();
    if index ~= mCurrentIndex then   
        mCurrentIndex=index;
        actionLayer.Action10003(mScene,false,currentTag,GeneralInfo[mCurrentIndex].GeneralID,currentLandPosition)
    else  
    		return     
    end  
end;

----关闭详细层
function closeDetailLayer ()
	if not isClick then
		releaseDetailLayer()
	end
end;

--升级土地
function  levelUpRedLand()
	if not isClick then
	isClick=true
	actionLayer.Action10010(mScene,false,1)
	end
end;

--升级土地
function  levelUpBlackLand()
	if not isClick then
	isClick=true
	actionLayer.Action10011(mScene,false,1)
	end
end;

--购买响应
function  buyFairyAction()
	if not isClick then
	isClick=true
	actionLayer.Action10009(mScene,false,1)
	end
end;

---扩建土地
function  expansionLand(item)	
	if not isClick then
	isClick=true
	local tag=item:getTag()
	actionLayer.Action10008(mScene,false,lastPositionTag,1)
	end
end;

---获取位置
function  getPlantPosition(position)
	local x=pWinSize.width*0.5-bgTexSize.width*0.46
	local y=mMidBgSprite:getPosition().y-bgTexSize.height*0.145	
	local xPos=x
	local yPos =y
	local colW= bgTexSize.width/100*16
	local colW1= bgTexSize.width/100*16
	local rowH= bgTexSize.height/100*15
	local rowH1 = bgTexSize.height/100*15
	
	local row=(position-1)%3
	local col=math.floor((position-1)/3)
	xPos=xPos+colW*row+col*colW1
	yPos=yPos+rowH*row-col*rowH1
	--	
	return PT(xPos,yPos), CCRectMake( xPos+mImgSize.width/4,yPos+mImgSize.height/4,
						mImgSize.width/2, mImgSize.height/2);						
end;

---购买仙露询问
function  askBuyDrew(clickedButtonIndex,content,tag) 
    if clickedButtonIndex==ID_MBOK then
		actionLayer.Action10009(mScene,nil,2)
    end
end;

---扩建土地询问
function  askExpansionLand(clickedButtonIndex,content,tag) 
    if clickedButtonIndex==ID_MBOK then
		actionLayer.Action10008(mScene,nil,lastPositionTag,2)
    end
end;

---刷新
function  askRefleshData(clickedButtonIndex,content,tag) 
    if clickedButtonIndex==ID_MBOK then
	 actionLayer.Action10005(mScene,nil,currentTag,GeneralInfo[mCurrentIndex].GeneralID,currentRefreshType,2)
    end
end;


---询问是否消去时间
function  askClearTimeAction(clickedButtonIndex,content,tag) 
    if clickedButtonIndex==ID_MBOK then
		actionLayer.Action10007(mScene,nil,clearLandPosition,2)
    end
end;

---
function  askLevelRedAction(clickedButtonIndex,content,tag) 
    if clickedButtonIndex==ID_MBOK then
		actionLayer.Action10010(mScene,nil,2)
    end
end;

---
function  askLevelBlackAction(clickedButtonIndex,content,tag) 
    if clickedButtonIndex==ID_MBOK then
		actionLayer.Action10011(mScene,nil,2)
    end
end;



----网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionID = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	isClick=false
	if actionID==10001 then
		local plantInfo=actionLayer._10001Callback(pScutScene, lpExternalData)
		if plantInfo then
		       releaseDrawBox()	       
		       mServerData=plantInfo	 
			 _plantInfo={{},{},{},{},{},{},{},{},{}}
			 
			 for k, v in ipairs(plantInfo.PlantingInfo) do
			 	if v then
			 		_plantInfo[v.LandPsition]=v
			 	end
			 end
			 
			 showPlantLayer()
		else
			ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.5)					
		end
		--种植佣兵列表
	elseif actionID==10002 then
		local generalInfo=actionLayer._10002Callback(pScutScene,lpExternalData)
		if generalInfo~=nil  and  generalInfo ~= " " then	
			GeneralInfo=generalInfo
			showDetailLayer()
		else
			ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.5)				
		end
		--品质列表
	elseif actionID==10003 then
		local detailInfo=actionLayer._10003Callback(pScutScene,lpExternalData)
		if detailInfo~=nil and detailInfo ~= " " then	
			releaseContentLayer()
			showDetailContent(detailInfo)
		else
			ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.5)						
		end
	elseif actionID==10004 then
          if ZyReader:getResult() == eScutNetSuccess then
			releaseDetailLayer()
			mCurrentId=1
			actionLayer.Action10001(mScene,nil)
	    else
	    		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),0.8,0.35)
	    end
    elseif actionID==10005 then
          if ZyReader:getResult() == eScutNetSuccess then
			 actionLayer.Action10003(mScene,nil,currentTag,GeneralInfo[mCurrentIndex].GeneralID,currentLandPosition)
	    elseif ZyReader:getResult() ==1 then
		      local box = ZyMessageBoxEx:new()
		      box:doQuery(pScutScene,nil,ZyReader:readErrorMsg(),Language.TIP_YES,Language.TIP_NO,askRefleshData)
		elseif ZyReader:getResult() == 2 then--晶石不足提示充值
	
	    else
	    		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),0.8,0.35)
	     end
	elseif actionID==10009 then
		  if ZyReader:getResult() == eScutNetSuccess then
		  	ZyToast.show(pScutScene,Language.SHOP_BUYSUCCESS,0.8,0.3)
		  	local DewNum=ZyReader:getInt()
		  	if DewNum~=nil and  DewNum>0 then
				DewNumLable:setString("" .. DewNum)
			end
		   elseif ZyReader:getResult() ==1 then
		      local box = ZyMessageBoxEx:new()
		      box:doQuery(pScutScene,nil,ZyReader:readErrorMsg(),Language.TIP_YES,Language.TIP_NO,askBuyDrew)
		   else
		   	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),0.8,0.35)
		  end
	elseif actionID==10006 then
		    isClick=false
		    if ZyReader:getResult() == eScutNetSuccess then
		    	local RewardNum =ZyReader:getInt()
		    	 showRewardNum(RewardNum)
			     actionLayer.Action10001(mScene,nil)
		    else
		   	 ZyToast.show(pScutScene,ZyReader:readErrorMsg(),0.8,0.35)
		   end
	elseif actionID==10007 then
		  if ZyReader:getResult() == eScutNetSuccess then
		  	timeBegin=false
		  	actionLayer.Action10001(mScene,nil)
		 elseif ZyReader:getResult() ==1 then
		      local box = ZyMessageBoxEx:new()
		      box:doQuery(pScutScene,nil,ZyReader:readErrorMsg(),Language.TIP_YES,Language.TIP_NO,askClearTimeAction)
		elseif ZyReader:getResult() == 2 then--晶石不足提示充值

		 else
		   	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),0.8,0.35)
		 end	
	elseif actionID==10008 then
		  if ZyReader:getResult() == eScutNetSuccess then
		  	timeBegin=false
		  	actionLayer.Action10001(mScene,nil)
		 elseif ZyReader:getResult() ==1 then
		      local box = ZyMessageBoxEx:new()
		      box:doQuery(pScutScene,nil,ZyReader:readErrorMsg(),Language.TIP_YES,Language.TIP_NO,askExpansionLand)
		elseif ZyReader:getResult() == 2 then--晶石不足提示充值
		      
		 else
		   	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),0.8,0.35)
		 end			 
	elseif actionID==10010 then
		  if ZyReader:getResult() == eScutNetSuccess then
		  	timeBegin=false
		  	actionLayer.Action10001(mScene,nil)
		 elseif ZyReader:getResult() ==1 then
		      local box = ZyMessageBoxEx:new()
		      box:doQuery(pScutScene,nil,ZyReader:readErrorMsg(),Language.TIP_YES,Language.TIP_NO,askLevelRedAction)
		elseif ZyReader:getResult() == 2 then--晶石不足提示充值
 		      
		 else
		   	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),0.8,0.35)
		 end
	elseif actionID==10011 then
		  if ZyReader:getResult() == eScutNetSuccess then
		  	timeBegin=false
		  	actionLayer.Action10001(mScene,nil)
		 elseif ZyReader:getResult() ==1 then
		      local box = ZyMessageBoxEx:new()
		      box:doQuery(pScutScene,nil,ZyReader:readErrorMsg(),Language.TIP_YES,Language.TIP_NO,askLevelBlackAction)
		elseif ZyReader:getResult() == 2 then--晶石不足提示充值
      
		 else
		   	ZyToast.show(pScutScene,ZyReader:readErrorMsg(),0.8,0.35)
		 end		 
	end
end