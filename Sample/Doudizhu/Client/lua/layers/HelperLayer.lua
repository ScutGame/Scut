------------------------------------------------------------------
--HelperLayer.lua
-- Author     : JMChen
-- Version    : 1.0.0.0
-- Date       : 2011-10-15
-- Description:
------------------------------------------------------------------


module("HelperLayer", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local mScene = nil 		-- 场景
local mLayer = nil 		-- 层
local mList=nil
local mColW=nil
local helpTable=nil
--
---------------公有接口(以下两个函数固定不用改变)--------
--



-- 退出场景
function closeScene()
	if mLayer then
		mLayer:getParent():removeChild(mLayer,true)
		mLayer=nil
	end
	mScene=nil
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
	mScene=nil
	mLayer=nil
	mList=nil
	helpTable=nil
end

function createOther()
	for k=4, 6 do
		local info=helpTable[k]
		createItems(info)	
	end
	 delayExec(createOther1,0.001, mLayer)	
end;

function createOther1()
	for k=7, #helpTable do
		local info=helpTable[k]
		createItems(info)	
	end
end
-- 创建场景
function initScene(parent)
	if mScene then
		return 
	end
	mScene=parent

	
	mLayer = CCLayer:create()
	mScene:addChild(mLayer, 10)
	
	local transparentBg=Image.tou_ming;--背景图片
	local bgSprite=CCSprite:create(P(transparentBg))
	bgSprite:setAnchorPoint(CCPoint(0,0));
	bgSprite:setScaleX(pWinSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(pWinSize.height/bgSprite:getContentSize().height);
	bgSprite:setPosition(CCPoint(0,0));
	mLayer:addChild(bgSprite,0);
	
	local actionBtn=UIHelper.createActionRect(pWinSize)
	actionBtn:setAnchorPoint(CCPoint(0,0));
	mLayer:addChild(actionBtn,0);
	
	
	--背景
	local detailBg= CCSprite:create(P("common/panle_1072.png"));
	detailBg:setAnchorPoint(PT(0,0))
	detailBg:setScaleX(pWinSize.width/detailBg:getContentSize().width)
	detailBg:setScaleY(pWinSize.height/detailBg:getContentSize().height)
	detailBg:setPosition(PT(0,0))
	mLayer:addChild(detailBg,0)
	--离开按钮
	local exitBtn=ZyButton:new("button/panle_1014_3.png",nil,nil, nil,FONT_NAME,FONT_SM_SIZE)
	exitBtn:setAnchorPoint(PT(0,0))
	exitBtn:setPosition(PT(pWinSize.width-exitBtn:getContentSize().width,pWinSize.height-exitBtn:getContentSize().height-SY(3)))
	exitBtn:registerScriptTapHandler(closeScene)
	exitBtn:addto(mLayer)
     
	local listSize = SZ(pWinSize.width,pWinSize.height*0.75)
	local list_x=detailBg:getPosition().x+SY(20)
	local list_y=detailBg:getPosition().y+SY(10)
	local listRowH=listSize.height/5
	mColW=listSize.width*0.87
	
	local list = ScutCxList:node(listRowH, ccc4(24, 24, 24, 0), listSize)
	list:setAnchorPoint(PT(0, 0))
	list:setPosition(PT(pWinSize.width*0.05, pWinSize.height*0.1))
	mLayer:addChild(list,0)	
	mList=list
	mList:setTouchEnabled(true)
	helpTable=HelpInfoConfig.getHelpInfo()
	for k=1, 3 do
		local info=helpTable[k]
		createItems(info)		
	end
	 delayExec(createOther,0.001, mLayer)
   
end;




function createItems(info)
	local layout=CxLayout()
	layout.val_x.t = ABS_WITH_PIXEL
	layout.val_y.t = ABS_WITH_PIXEL
	layout.val_x.val.pixel_val =0
	layout.val_y.val.pixel_val =0
	local listItem = ScutCxListItem:itemWithColor(ccc3(32, 24, 3))
	listItem:setOpacity(0)
	local layer=CCLayer:create()	
	local str=info.content
	local label=createRowLabel(str,layer,nil,mColW)
	listItem:addChildItem(layer, layout)
	mList:setRowHeight(label:getContentSize().height+SY(1))
	mList:addListItem(listItem, false)
end;


function  createRowLabel(content,parent,color,width)
	local label= ZyMultiLabel:new(content,width,FONT_NAME,FONT_SM_SIZE)
	label:addto(parent,0)
	return label
end;

---延迟进行方法
function delayExec(funName,nDuration, parent)
	local  action = CCSequence:createWithTwoActions(
	CCDelayTime:create(nDuration),
	CCCallFuncN:create(funName));
	parent:runAction(action)
end

