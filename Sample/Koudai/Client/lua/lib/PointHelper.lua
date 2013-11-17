 module("PointHelper", package.seeall)
mTouchDistance = SX(15)
 ---------convert tilemap tileCoordinate to View Pt-----
 --input CCTMXTiledMap* tiledObj
 --input layerName const char*
 --input tiledCoordinate tPt
 --return ViewPoint

 function tiledPtToViewPt(tileMapObj, layerName, tPt)
   local layer = tileMapObj:layerNamed(layerName)
   --左下点
   local ptOut = layer:positionAt(CCPointMake(tPt.x, tPt.y))
   local tileHeight = tileMapObj:getTileSize().height;
   ptOut = CCPointMake(ptOut.x, ptOut.y-tileHeight/2);
   local tmpSz = tileMapObj:getContentSizeInPixels();
   local mapCenterPt = CCPointMake(tmpSz.width/2,tmpSz.height/2);
   ptOut = ccpSub( ptOut  , mapCenterPt);
   return ptOut
 end

function touchPtToMapPt(tileMapObj, touchPt)
    local pos = tileMapObj:convertToNodeSpace(touchPt);
	local halfMapWidth = (tileMapObj:getMapSize().width)*0.5;
	local mapHeight = tileMapObj:getMapSize().height
	local tileWidth = tileMapObj:getTileSize().width
	local tileHeight = tileMapObj:getTileSize().height

	local tilePosDiv = CCPointMake(pos.x / tileWidth, pos.y / tileHeight)
	local inverseTileY = mapHeight - tilePosDiv.y;

	local posX = math.floor(inverseTileY + tilePosDiv.x - halfMapWidth);
	local posY = math.floor(inverseTileY - tilePosDiv.x + halfMapWidth)
	local isOn = false
	if 0>posX or posX> tileMapObj:getMapSize().width - 1 or 0>posY or
        posY> tileMapObj:getMapSize().height - 1
    then
		isOn = false;
	else
		isOn = true;
	end
	posX = math.max(0, posX)
	posX = math.min(tileMapObj:getMapSize().width - 1, posX)
	posY = math.max(0, posY)
	posY = math.min(tileMapObj:getMapSize().height - 1, posY)
	return isOn, CCPointMake(posX, posY)
end

function touchPtToMapPt2(tileMapObj, touchPt)
	local pos = tileMapObj:convertToNodeSpace(touchPt);
	local halfMapWidth = (tileMapObj:getMapSize().width)*0.5;
	local mapHeight = tileMapObj:getMapSize().height
	local tileWidth = tileMapObj:getTileSize().width
	local tileHeight = tileMapObj:getTileSize().height

	local tilePosDiv = CCPointMake(pos.x / tileWidth, pos.y / tileHeight)
	local inverseTileY = mapHeight - tilePosDiv.y;

	local posX = math.floor(inverseTileY + tilePosDiv.x - halfMapWidth);
	local posY = math.floor(inverseTileY - tilePosDiv.x + halfMapWidth)
	local isOn = false
	if 0>posX or posX> tileMapObj:getMapSize().width - 1 or 0>posY or
        posY> tileMapObj:getMapSize().height - 1
    then
		isOn = false;
	else
		isOn = true;
	end

	return isOn, CCPointMake(posX, posY)
end


function touchPtToMapPt3(tileMapObj, touchPt)
    	local pos = tileMapObj:convertToNodeSpace(touchPt);
	local halfMapWidth = (tileMapObj:getMapSize().width)*0.5;
	local mapHeight = tileMapObj:getMapSize().height
	local tileWidth = tileMapObj:getTileSize().width
	local tileHeight = tileMapObj:getTileSize().height
	
	
	
	

	local tilePosDiv = CCPointMake(pos.x / tileWidth, pos.y / tileHeight)
	local inverseTileY = mapHeight - tilePosDiv.y;

	local posX = math.ceil(tilePosDiv.x);
	local posY = math.ceil(inverseTileY)
	
	local isOn = false
	if 0>posX or posX> tileMapObj:getMapSize().width - 1 or 0>posY or
        posY> tileMapObj:getMapSize().height - 1
      then
		isOn = false;
	else
		isOn = true;
	end
	posX = math.max(0, posX)
	posX = math.min(tileMapObj:getMapSize().width - 1, posX)
	posY = math.max(0, posY)
	posY = math.min(tileMapObj:getMapSize().height , posY)
	return isOn, CCPointMake(posX, posY)
	
end

--获取多点触摸的距离
function getDistance(touches, isPer)
	if touches ~= nil and #touches >=2
	then
		local first = touches[1]
		local second = touches[2]
		local pt1 = nil
		local pt2 = nil
		if isPer
		then
			pt1 = first:previousLocationInView(first:view())
			pt2 = second:previousLocationInView(second:view())
		else
			pt1 = first:locationInView(first:view())
			pt2 = second:locationInView(second:view())
		end
		return ccpDistance(pt1, pt2)
	end
end

function normalMove(e, bglayer, bgSpriite, nCurrentScaleValue, bUseBgSprite)
 for k,v in ipairs(e) do
        touchLocation =  v:locationInView(v:view() )
	    prevLocation = v:previousLocationInView(v:view())
	    touchLocation = CCDirector:sharedDirector():convertToGL(touchLocation );
	    prevLocation = CCDirector:sharedDirector():convertToGL( prevLocation );

        local diff = ccpSub(touchLocation, prevLocation)

		local winSize = CCDirector:sharedDirector():getWinSize()
        local currentPos = bglayer:getPosition()

        local pos = ccpAdd(currentPos, diff)

		if bUseBgSprite == false then
		else
			local nSpriteWidthHalf = bgSpriite:getContentSize().width /2
			local nSpriteHeightHalf = bgSpriite:getContentSize().height /2
			local nMoveDisH =nSpriteWidthHalf * nCurrentScaleValue - winSize.width /2
			local nMoveDisV = nSpriteHeightHalf*nCurrentScaleValue - winSize.height/2

			local leftTopPoint = CCPoint(-nMoveDisH, nMoveDisV)
			local rightBottomPt = CCPoint(nMoveDisH, -nMoveDisV)

			if pos.x < leftTopPoint.x
			then
			pos.x = leftTopPoint.x
			end
			if pos.x > rightBottomPt.x
			then pos.x = rightBottomPt.x
			end

			if pos.y < rightBottomPt.y
			then
			pos.y = rightBottomPt.y
			end

			if pos.y > leftTopPoint.y
			then
			pos.y = leftTopPoint.y
			end
		end
		 --CCLuaLog(string.format("(%f, %f)", pos.x, pos.y))
        bglayer:setPosition( pos );
    end
end

--根据当前的缩放系数判断 缩放后是否会越界
function isCanScale(scale, bglayer, bgSpriite)

		local winSize = CCDirector:sharedDirector():getWinSize()
		local nSpriteWidthHalf = bgSpriite.width /2 * scale
		local nSpriteHeightHalf = bgSpriite.height /2 * scale

		local spritePt	= bglayer:getPosition()
		spritePt.x = spritePt.x + winSize.width /2
		spritePt.y = spritePt.y + winSize.height /2


		local bCanScale = true
		if nSpriteWidthHalf -  spritePt.x < 0 or spritePt.x + nSpriteWidthHalf < winSize.width then
			bCanScale = false
		end
		if nSpriteHeightHalf - spritePt.y < 0 or spritePt.y + nSpriteHeightHalf < winSize.height  then
			bCanScale = false
		end
		return bCanScale
end

--多点組ove事件
function multiTouches(e, nCurrentScaleValue, nMaxScale, nMinScale, bglayer, bgSpriteSize)
	local curDis = PointHelper.getDistance(e, false)
	local perDis = PointHelper.getDistance(e, true)
	local scale = nil
	if perDis > 0 then
		scale = curDis * 1.0/ perDis
	end

	scale = nCurrentScaleValue - (1- scale)

	if scale <= nMaxScale and scale > nMinScale then
		if PointHelper.isCanScale(scale,bglayer, bgSpriteSize) == true then
			nCurrentScaleValue = scale
			bglayer:setScale(nCurrentScaleValue)
		end
	end
	return nCurrentScaleValue
end
