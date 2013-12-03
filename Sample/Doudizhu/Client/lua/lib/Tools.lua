------------------------------------------------------------------
-- Tools.lua
-- Author     : 
-- Version    : 1.15
-- Date       :   
-- Description: ,
------------------------------------------------------------------
module("Tools", package.seeall)

--两矩形是否碰到
function is_meet(xx,yy,ww,hh,x,y,w,h)
	local ok=false
	if x+w>xx and x-ww<xx  and y+h>yy and y-hh<yy then
		ok=true;--碰到
	end;
	return ok
end

--给出中文字符串宽
function get_String_w(ss,FONT,SIZE)
	local textlabel = CCLabelTTF:create(ss,FONT,SIZE)
	return textlabel:getContentSize().width
end

--单个中文字符宽
function get_String_1_w(FONT,SIZE)
	local textlabel = CCLabelTTF:create( Language.NIN,FONT,SIZE)
	return textlabel:getContentSize().width
end

--单个中文字符高
function get_String_1_h(FONT_NAME,SIZE)
	local textlabel = CCLabelTTF:create(Language.NIN,FONT_NAME,SIZE)
	return textlabel:getContentSize().height
end

function get_image_w(bgEmptyFile)
	local bgEmpty= CCSprite:create(P(bgEmptyFile));
	return bgEmpty:getContentSize().width
end

function get_image_h(bgEmptyFile)
	local bgEmpty= CCSprite:create(P(bgEmptyFile));
	return bgEmpty:getContentSize().height
end

function get_String_w(ss,FONT,SIZE)
	local textlabel = CCLabelTTF:create(ss,FONT,SIZE)
	return textlabel:getContentSize().width
end