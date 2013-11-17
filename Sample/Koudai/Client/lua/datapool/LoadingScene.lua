
------------------------------------------------------------------
-- LoadingScene.lua
-- Author     :
-- Version    : 1.15
-- Date       :   
-- Description: 
------------------------------------------------------------------
module("LoadingScene",package.seeall)

local mSprite
local mLayer=nil
function init(pMainScene)

     mLayer=CCLayer:create();
     pMainScene:addChild(mLayer,0)
     
     local bgSprite=CCSprite:create(P("common/list_1015.png"));
     mLayer:addChild(bgSprite,0)
     bgSprite:setPosition(PT(mLayer:getContentSize().width/2,mLayer:getContentSize().height/2));
      
end


function hide()
	if mLayer~= nil then
	    mLayer:getParent():removeChild(mLayer, true)
	    mLayer=nil
	end
end