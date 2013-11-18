------------------------------------------------------------------
-- NoticeLayer.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 公告界面        层
------------------------------------------------------------------

module("NoticeLayer", package.seeall)


mScene = nil 		-- 场景



-- 退出场景
function popScene()
	releaseResource()
	
end

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()

end

-- 释放资源
function releaseResource()

	if showLayer ~= nil then
		showLayer:getParent():removeChild(showLayer, true)
		showLayer = nil
	end
	mLayer = nil
	mScene = nil

end

-- 创建场景
function init(fatherLayer, scene)
	initResource()


	mScene = scene
	
	-- 添加主层
	mLayer = CCLayer:create()
	mScene:addChild(mLayer, 0)	
		mScene:registerScriptHandler(SpriteEase_onEnterOrExit)
	local unTouchBtn =  ZyButton:new(Image.image_transparent)
	unTouchBtn:setScaleX(pWinSize.width/unTouchBtn:getContentSize().width)
	unTouchBtn:setScaleY(pWinSize.height/unTouchBtn:getContentSize().height)
	unTouchBtn:setAnchorPoint(PT(0,0))
	unTouchBtn:setPosition(PT(0,0))
	unTouchBtn:addto(mLayer, 0)
	
	
	
	-- 此处添加场景初始内容
	showContent()
end

function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        releaseResource()
    end
end


function showContent()
	if showLayer ~= nil then
		showLayer:getParent():removeChild(showLayer, true)
		showLayer = nil
	end
	layer = CCLayer:create()
	mLayer:addChild(layer, 0)
	showLayer = layer
	
	--背景
	local imageBg = CCSprite:create(P("common/list_1043.png"))
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(pWinSize.width*0.5-imageBg:getContentSize().width*0.5, pWinSize.height*0.2))
	layer:addChild(imageBg, 0)
	
	--关闭按钮
	local closeBtn =  ZyButton:new(Image.image_close)
	closeBtn:setAnchorPoint(PT(0,0))
	closeBtn:setPosition(PT(0,0))
	closeBtn:addto(mLayer, 0)	
	closeBtn:registerScriptHandler(popScene)

	
	
	--创建list
	
	local list_width = pWinSize.width*0.9
	local list_height = pWinSize.height*0.52
	local list_x = pWinSize.width*0.5-list_width*0.5
	local list_y = pWinSize.height*0.24
	
	local listSize = SZ(list_width, list_height)
	list = ScutCxList:node(listSize.height*0.8, ccc4(24, 24, 24, 0), listSize)
	list:setAnchorPoint(PT(0, 0))
	list:setPosition(PT(list_x, list_y))
	layer:addChild(list,0)
	list:setTouchEnabled(true)
	table = {1, 2}
	for k,v in ipairs(table) do
		
		local listItem = ScutCxListItem:itemWithColor(ccc3(32, 24, 3))
		listItem:setOpacity(50)	
		
		local title = "afsdgah"
		local content = "fdassssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss"
		local item = creatItem(title, content, tag)
		
		local layout=CxLayout()
		layout.val_x.t = ABS_WITH_PIXEL
		layout.val_y.t = ABS_WITH_PIXEL
		layout.val_x.val.pixel_val =0
		layout.val_y.val.pixel_val =0

		listItem:addChildItem(item, layout)
		list:addListItem(listItem, false) 	
	
	end
	
end

--创建单条信息
function creatItem(title, content, tag)
	local layer = CCLayer:create()
	
	local itemSize = SZ(list:getContentSize().width, list:getRowHeight())
	
	local imageBg = CCSprite:create(P("common/list_1020.9.png"))
	imageBg:setScaleX(itemSize.width*0.99/imageBg:getContentSize().width)
	imageBg:setScaleY(itemSize.height*0.95/imageBg:getContentSize().height)
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(itemSize.width*0.005, itemSize.height*0.025))
	layer:addChild(imageBg, 0)

	if title then
		local titleLabel = CCLabelTTF:create(title, FONT_NAME, FONT_SM_SIZE)
		titleLabel:setAnchorPoint(PT(0.5,1))
		titleLabel:setPosition(PT(itemSize.width*0.5, itemSize.height*0.9))
		layer:addChild(titleLabel, 0)
	end
	
	if content then
		local contentStr=string.format("<label>%s</label>", content)
		local contentLabel=ZyMultiLabel:new(contentStr, itemSize.width*0.9, FONT_NAME, FONT_SM_SIZE);
		contentLabel:setPosition(PT(itemSize.width*0.05, itemSize.height*0.7-contentLabel:getContentSize().height))
		contentLabel:addto(layer, 0)
		
	end
	
	local funcBtn = ZyButton:new(Image.image_button, nil, nil, Language.NOTICE_GOTO, FONT_NAME, FONT_SM_SIZE)
	funcBtn:registerScriptHandler(gotoAction)
	funcBtn:setAnchorPoint(PT(0,0))
	funcBtn:setPosition(PT(itemSize.width*0.5-funcBtn:getContentSize().width*0.5, itemSize.height*0.1))
	funcBtn:addto(layer, 0)
	if tag ~= nil then
		funcBtn:setTag(tag)
	end

	return layer
end

function gotoAction(pNode)
	local index = pNode:getTag()
	if index == 1 then
		
	elseif index == 2 then
		
	end
	

end;
