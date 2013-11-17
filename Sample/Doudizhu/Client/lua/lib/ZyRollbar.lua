------------------------------------------------------------------
-- ZyRollbar.lua.lua
-- Author     : WangTong
-- Version    : 1.0
-- Date       :
-- Description: 
------------------------------------------------------------------

--
-- ZyButton.lua
-- 91War
--
-- Created by LinKequn on 8/12/2011.
-- Copyright 2008 ND, Inc. All rights reserved.
--

 ZyRollbar = {
	rolllayer = nil,
      mBar={},
    sBar={},
    bBar=nil,
    value=1,
    height=0,
    
 }
 function ZyRollbar:new(parent,orgX,orgY,strroll,strbar,width,count,height)
 	local instance = {}
	setmetatable(instance, self)
	self.__index = self
     if strroll==nil then
        strroll="common/list_1020_3.png"
    end
    if strbar==nil then
        strbar="common/list_1020_1.9.png"
    end
     local rolllayer=CCLayer:create()
     instance.rolllayer= rolllayer
    local offsetY=-SY(10)

    instance.mBar={}
    instance.sBar={}
    instance.bBar=nil
    instance.value=count
    instance.height=0
	
    for index=1 ,count ,1 do
        local menuItem = CCSprite:create(P(strroll))
        menuItem:setAnchorPoint(PT(0.2, 0.5))
        instance.mBar[index]=menuItem
       --背景框
        local bgBar=CCSprite:create(P(strbar))
        bgBar:setAnchorPoint(PT(0,0))
        bgBar:setPosition(PT(0,offsetY))
        local sizeOld = bgBar:getContentSize()
        
        bgBar:setScaleX(width / sizeOld.width)
        rolllayer:addChild(bgBar,0)
        instance.bBar=bgBar
       -- bgBar:setContentSize(sizeOld)
        --变化的进度条
        local sbar=CCSprite:create(P("common/list_1020_2.9.png"))
        sbar:setAnchorPoint(PT(0,0.5))
        sbar:setPosition(PT(SX(1),offsetY+bgBar:getContentSize().height/2))
        local sizeOld = sbar:getContentSize()
        instance.sBar[index]=sbar
        menuItem:setPosition(PT(0,bgBar:getPosition().y+sizeOld.height/10*8-SY(1)))
        rolllayer:addChild(menuItem,1) 
        rolllayer:addChild(sbar,0)
        rolllayer:setPosition(PT(orgX,orgY))
        offsetY=offsetY+instance.height
	end
	rolllayer:setContentSize(CCSize(width,height))
       rolllayer:setIsTouchEnabled(true)
	parent:addChild(rolllayer,0)
	
	return instance,instance.rolllayer,instance.mBar,bBar,instance.sBar

 end
 
 
 
function ZyRollbar:tBegan(e,nowvalue,max,nowindex,moveheight)
        local rolllayer=self.rolllayer
        local mBar=self.mBar
        local sBar=self.sBar
    if e ~= nil then
        if moveheight==nil then
            moveheight=0
        end

        for k,v in ipairs(e) do
            local pointBegin = v:locationInView(v:view())
            pointBegin = CCDirector:sharedDirector():convertToGL(pointBegin)
            pointBegin=ccpSub(pointBegin,rolllayer:getPosition())
            local spriteBarsz={}
            local spriteSize={}
            local positionCurrent={}
            local long={}
            for index=1,self.value,1 do
                long[index]={}
                spriteBarsz[index]=self.mBar[index]:getTextureRect();
                spriteSize[index]=spriteBarsz[index].size;
                positionCurrent[index] = mBar[index]:getPosition()   
                if pointBegin.x>positionCurrent[index].x-SX(5) and pointBegin.x<positionCurrent[index].x+spriteSize[index].width+SX(5) and
                    pointBegin.y+moveheight<positionCurrent[index].y+spriteSize[index].height-SY(3) and  pointBegin.y+moveheight>positionCurrent[index].y-SY(10) then           
                    self.mMouseBeginPt = pointBegin  
                    self.curindex=index
                elseif pointBegin.y+moveheight<=positionCurrent[index].y+spriteSize[index].height-SY(3)  and  pointBegin.y+moveheight>=positionCurrent[index].y-SY(10) then
                    local pos=pointBegin
                    if pos.x>-mBar[index]:getContentSize().width and pos.x <rolllayer:getContentSize().width then
                        if pos.x>rolllayer:getContentSize().width-mBar[index]:getContentSize().width/2 then
                            pos.x=rolllayer:getContentSize().width-mBar[index]:getContentSize().width/2
                        end
                        if pos.x<0 then
                            pos.x=0
                        end
                        sBar[index]:setScaleX(pos.x/ sBar[index]:getContentSize().width)
                        mBar[index]:setPosition(CCPoint(pos.x,positionCurrent[index].y))
                        long[index].value=pos.x
                        long[index].isvalue=true
                        return long,rolllayer:getContentSize().width- mBar[index]:getContentSize().width/2                       
                    end
                end                 
            end
        end
    elseif self.value~=nil then
        if mBar[nowindex]~=nil then
            if max~=0 then
                local pos=nowvalue/max*(rolllayer:getContentSize().width- mBar[nowindex]:getContentSize().width/2)
                sBar[nowindex]:setScaleX(pos/ (sBar[nowindex]:getContentSize().width))
                mBar[nowindex]:setPosition(CCPoint(pos,mBar[nowindex]:getPosition().y))
                else 
                	local image=CCSprite:create(P("common/list_1020_3.png"))
                	sBar[nowindex]:setScaleX((image:getContentSize().width*0.3/sBar[nowindex]:getContentSize().width))
               	mBar[nowindex]:setPosition(CCPoint(0,mBar[nowindex]:getPosition().y))
            end
        end
    end
end

--
function ZyRollbar:tMoved(e)
    if e ~= nil then
        local long
        local mBar=self.mBar
        local rolllayer=self.rolllayer
        local sBar=self.sBar
        local curindex= self.curindex
        if self.mMouseBeginPt ~=nil and curindex~=nil then
            for k,v in ipairs(e) do
                local long={}
                local index=nil
                local touchLocation =  v:locationInView(v:view() )
                local pointBegin = CCDirector:sharedDirector():convertToGL(touchLocation)
                local prevLocation = v:previousLocationInView(v:view())
                for i=1,self.value,1 do
                    long[i]={}
                end
                local diff = ccpSub(touchLocation, prevLocation)
                local currentPos = mBar[curindex]:getPosition()
                local pos = ccpAdd(currentPos, diff)
                if pos.x< 0 then
                    pos.x = 0
                end
                if pos.x > rolllayer:getContentSize().width-mBar[curindex]:getContentSize().width*0.5 then
                    pos.x = rolllayer:getContentSize().width-mBar[curindex]:getContentSize().width*0.5
                end
                sBar[curindex]:setScaleX(pos.x/ sBar[curindex]:getContentSize().width)
                mBar[curindex]:setPosition(CCPoint(pos.x,mBar[curindex]:getPosition().y))
                long[curindex].value=pos.x
                long[curindex].isvalue=true
                return long,rolllayer:getContentSize().width- mBar[curindex]:getContentSize().width/2
            end
        end
    end
end

function ZyRollbar:tEnd(e)
    if e ~= nil then
        self.mMouseBeginPt=nil
        self.curindex=nil
    end
end


