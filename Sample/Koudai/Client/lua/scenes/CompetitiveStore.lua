------------------------------------------------------------------
-- CompetitiveStore.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 
------------------------------------------------------------------

module("CompetitiveStore", package.seeall)

local numberAge = 1--数量




function setInfo(info, scene)
	mData = info
	mScene = scene
end;

function numChoice()----- 按键击发

	ButtonLayer =  CCLayer:create()
	ButtonLayer:setAnchorPoint(PT(0,0))
	ButtonLayer:setPosition(PT(0,0))	
	mScene:addChild(ButtonLayer,4)

	--背景
	createBg()
	

	--您兑换“”的数量
	local goodsString_1 = string.format(Language.COMPETI_EXCHANGE, mData.ItemName )
	local titleRecharge = CCLabelTTF:create(goodsString_1,FONT_NAME,FONT_SM_SIZE)
	titleRecharge:setColor(ccc3(255,255,255))
	ButtonLayer:addChild(titleRecharge,1)
	titleRecharge:setAnchorPoint(PT(0.5,0.5))
	titleRecharge:setPosition(PT(pWinSize.width*0.5,pWinSize.height*0.575))
	
	

	--关闭
	local touming1=ZyButton:new("button/list_1039.png",nil,nil,Language.CloseButton,FONT_NAME,FONT_SM_SIZE);
	touming1:addto(ButtonLayer,100)
	touming1:setAnchorPoint(PT(0.5,0))
	touming1:setPosition(PT(pWinSize.width*0.5-SX(100),pWinSize.height*0.35))
	touming1:registerScriptHandler(closeButton_keyAction)

	--确定
	local touming2=ZyButton:new("button/list_1039.png",nil,nil,Language.QueDingButton,FONT_NAME,FONT_SM_SIZE);
	touming2:addto(ButtonLayer,100)
	touming2:setAnchorPoint(PT(0.5,0))
	touming2:setPosition(PT(pWinSize.width*0.5+SX(100),pWinSize.height*0.35))
	touming2:registerScriptHandler(OKButton_keyAction) 

	addWenzi()
end

--背景
function createBg()
	--屏蔽按钮
	local touming=ZyButton:new(Image.image_toumingPath,Image.image_toumingPath,Image.image_toumingPath);
	touming:setAnchorPoint(PT(0,0))
	touming:setPosition(PT(0,0))
	touming:setScaleX(pWinSize.width/touming:getContentSize().width)
	touming:setScaleY(pWinSize.height/touming:getContentSize().height)
	touming:addto(ButtonLayer,0)

	--背景				
	local buyBigImg = CCSprite:create(P("common/list_1054.png"));
	local scale = pWinSize.width/buyBigImg:getContentSize().width
	buyBigImg:setScale(scale)
	buyBigImg:setAnchorPoint(PT(0,0))
	buyBigImg:setPosition(PT(0,pWinSize.height*0.5-buyBigImg:getContentSize().height*0.5*scale))
	ButtonLayer:addChild(buyBigImg,0)  
	
	
	local imageFile =   "mainUI/list_1022_2.png"							
	local buyBigBaseImg = CCSprite:create(P(imageFile));
	buyBigBaseImg:setAnchorPoint(PT(0,0))
	buyBigBaseImg:setScaleX(pWinSize.width/buyBigBaseImg:getContentSize().width)
	buyBigBaseImg:setPosition(PT(0,buyBigImg:getPosition().y))
	ButtonLayer:addChild(buyBigBaseImg,0)  
	
	---中间透明图片
	local imageFile =   "common/list_1052.9.png"							
	local buyBigBetweenImg = CCSprite:create(P(imageFile));
	buyBigBetweenImg:setScaleY(buyBigImg:getContentSize().height*scale*0.7/buyBigBetweenImg:getContentSize().height)
	buyBigBetweenImg:setAnchorPoint(PT(0,0))
	buyBigBetweenImg:setPosition(PT((pWinSize.width-buyBigBetweenImg:getContentSize().width)/2,
					buyBigImg:getPosition().y+buyBigImg:getContentSize().height*0.1*scale))
	ButtonLayer:addChild(buyBigBetweenImg,0)  
end;

function sumValue() 
	local perCost = mData.Athletics --单价
	local totalNum = numberAge*perCost
	local much = string.format(Language.COMPETI_TOTAL, totalNum)
	return much
end



function addWenzi()

	local pos_y = pWinSize.height*0.5
	
	local imageFile =   "common/list_1049.png"							----白色图片
	local requestBg = CCSprite:create(P(imageFile));
	requestBg:setScaleX(pWinSize.width*0.25/requestBg:getContentSize().width)
	requestBg:setAnchorPoint(PT(0.5,0.5))
	requestBg:setPosition(PT(pWinSize.width*0.5,pos_y))
	ButtonLayer:addChild(requestBg,1) 	
	
	numberAge1= CCLabelTTF:create(numberAge,FONT_NAME,FONT_SM_SIZE)
	ButtonLayer:addChild(numberAge1,1)
	numberAge1:setColor(ccc3(0,0,0))
	numberAge1:setAnchorPoint(PT(0.5,0.5))
	numberAge1:setPosition(PT(pWinSize.width*0.5, pos_y))
	
	
	sum = sumValue()
	
	titleRecharge2 = CCLabelTTF:create(sum,FONT_NAME,FONT_SM_SIZE)
	titleRecharge2:setColor(ccc3(255,255,255))
	ButtonLayer:addChild(titleRecharge2,1)
	titleRecharge2:setAnchorPoint(PT(0.5,0))
	titleRecharge2:setPosition(PT(pWinSize.width*0.5,pWinSize.height*0.42)) 	
	
	local buttonLImg = "button/list_1069.png"
	local buttonRImg = "button/list_1068.png"

	buttonLeft = ZyButton:new(buttonRImg,nil,nil,nil,FONT_NAME,FONT_SM_SIZE);
	buttonLeft:addto(ButtonLayer,2)
	buttonLeft:setTag(1)
	buttonLeft:setAnchorPoint(PT(0.5,0.5))
	buttonLeft:setPosition(PT(pWinSize.width*0.5+SX(100), pos_y))
	buttonLeft:registerScriptHandler(UpdateAdd)
	buttonLeft:setVisible(true)
	
	buttonRight = ZyButton:new(buttonLImg,nil,nil,nil,FONT_NAME,FONT_SM_SIZE);
	buttonRight:addto(ButtonLayer,2)
	buttonRight:setTag(1)
	buttonRight:setAnchorPoint(PT(0.5,0.5))
	buttonRight:setPosition(PT(pWinSize.width*0.5-SX(100), pos_y))
	buttonRight:registerScriptHandler(UpdateReduce)
	buttonRight:setVisible(false)
	

	
end



------增加数量
function UpdateAdd(item)
	local addTo = item:getTag()
	numberAge = numberAge+addTo
	if  tonumber(numberAge) >1     then
		buttonRight:setVisible(true)
	end
	if tonumber(numberAge) > 98 then 
		buttonLeft:setVisible(false)
	end
	sum = sumValue()	
	titleRecharge2:setString(sum)
	numberAge1:setString(numberAge)
end

--减少数量
function UpdateReduce(item)
	local reduceTo = item:getTag()
	numberAge = numberAge-reduceTo
	if  numberAge <= 0  then 
		numberAge =1
	end
	if tonumber(numberAge)<=1 then
		buttonRight:setVisible(false)
	end;
	if tonumber(numberAge)<=99 then 
		buttonLeft:setVisible(true)
	end
	sum = sumValue()
	titleRecharge2:setString(sum)
	numberAge1:setString(numberAge)
end;
	


function closeButton_keyAction()  ------  取消按钮
	if ButtonLayer then
		ButtonLayer:getParent():removeChild(ButtonLayer,true)
		ButtonLayer = nil
	end	
	numberAge = 1		
end

function OKButton_keyAction()  ---- 确认按钮
	actionLayer.Action7012(mScene,nil, mData.ItemID, numberAge)
	CompetitiveStore.closeButton_keyAction()
end


