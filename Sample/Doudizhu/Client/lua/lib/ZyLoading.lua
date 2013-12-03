module("ZyLoading", package.seeall)


mlstLoading = {}
local mCount = 0
local mLoadingSprite = nil
local mLoadingTag = 100001
local mbUiLoading = false;
local isClick=nil

function getLoadingInfo(pScene)
	for k, v in pairs(mlstLoading) do
		if v.scene == pScene then
			return v
		end
	end
	return nil
end

function removeListData(lpScene)
	local key = nil
	for k, v in pairs(mlstLoading) do
		if v.scene == lpScene then
			key = k
			break
		end
	end
	if key ~= nil then
	    ZyTable.remove(mlstLoading,key)
	end
end


function getLoadingSprite()
	return mLoadingSprite
end

function show(pScene, nTagId,strTips)
	if not isClick then 
	    isClick=true
		local itemInfo = getLoadingInfo(pScene);
		if(itemInfo == nil) then
			if(not mbUiLoading) then
				createUI(pScene,strTips);
			end
			local item = {scene = pScene, nCounter = 1, sprite = mLoadingSprite}
			ZyTable.push_back(mlstLoading, item)
		else
			itemInfo.nCounter = itemInfo.nCounter + 1
		end
	end
end

function hide(lpScene)
      isClick=false
	local itemInfo = getLoadingInfo(lpScene)
	if itemInfo ~= nil then
		itemInfo.nCounter = itemInfo.nCounter - 1
		if(itemInfo.nCounter <= 0) then
	--	    lpScene:removeChild(itemInfo.sprite, true)
	--	    itemInfo.sprite=nil	
			if(not mbUiLoading) and (itemInfo.sprite) then
				lpScene:removeChild(itemInfo.sprite, true)
				mLoadingSprite = nil;
			end
			removeListData(lpScene)
		end
	end
	--.CCTextureCache:sharedTextureCache():removeAllTextures() 
	
end

function ShowForUiLoading(pScene)
	  if not isClick then
	      isClick=true
		local itemInfo = getLoadingInfo(pScene);
		if((itemInfo == nil) and (not mbUiLoading)) then
			createUI(pScene);
		end
		mbUiLoading = true;
		isClick=false
	  end
end

function HideForUiLoading(pScene)
	local itemInfo = getLoadingInfo(pScene);	
	mbUiLoading = false;
	isClick=false
	if(itemInfo == nil) and (mLoadingSprite) and pScene then
		pScene:removeChild(mLoadingSprite, true);
		mLoadingSprite = nil;
	end
end

function createUI(pScene, pos)
    if pScene then
	local item = CCLayer:create()
	local winSize = CCDirector:sharedDirector():getWinSize()
	item:setContentSize(winSize)
	--loading动画--
	
	local sprite=ScutAnimation.CScutAnimationManager:GetInstance():LoadSprite("loading")
	sprite:play()
	sprite:setPosition(CCPoint(pWinSize.width/2, pWinSize.height*0.4))
	
	local tranBg = CCSprite:create(P("common/ground_2018.9.png"))
	tranBg:setPosition(PT(0, 0))
	tranBg:setAnchorPoint(PT(0, 0))
	tranBg:setOpacity(120)
	tranBg:setScaleX(winSize.width / tranBg:getContentSize().width)
	tranBg:setScaleY(winSize.height / tranBg:getContentSize().height)
	item:addChild(tranBg, 0)
	item:addChild(sprite,100)


		--添加Loading下面的提示信息
	if strTips then
		local lbTips = CCLabelTTF:create(strTips, FONT_NAME, FONT_SM_SIZE)
		
		local lbSz = lbTips:getContentSize()
		local tipsBg = CCSprite:create(P("common/list_2004.9.png"))
		
		lbSz.width = lbSz.width + SX(8) + SX(20)
		lbSz.height = lbSz.height + SY(8) + SY(10)
		tipsBg:setAnchorPoint(PT(0.5, 0.5))
		tipsBg:setScaleX(lbSz.width/ tipsBg:getContentSize().width)
		tipsBg:setScaleY(lbSz.height/ tipsBg:getContentSize().height)
		local pt = PT(item:getContentSize().width/2,item:getContentSize().height/2)
		pt.y = pt.y - spriteloading:getContentSize().height/2 - SY(2) - lbSz.height/2
		item:addChild(tipsBg, 0)
		tipsBg:setPosition(pt)
		
		item:addChild(lbTips, 0)
		lbTips:setPosition(pt)
	end
	
	
	local menuItem = CCMenuItemSprite:create(item, nil)
	mLoadingSprite = CCMenu:createWithItem(menuItem)
--	mLoadingSprite:setTouchesDispatchPriority(-999)
	pScene:addChild(mLoadingSprite, 9, mLoadingTag)

	return mLoadingSprite
	end
end
--]]
function  releaseAll()
	if mLoadingSprite~=nil and mbUiLoading then
	   mLoadingSprite:getParent():removeChild(mLoadingSprite,true)
	end
     	for k, v in pairs(mlstLoading) do
		if v.scene~=nil then
			v.scene:removeChild(v.sprite, true)
		end
	end
	mLoadingSprite=nil
	mlstLoading = {}
	mbUiLoading = false;
	isClick=false
end;
--]]
