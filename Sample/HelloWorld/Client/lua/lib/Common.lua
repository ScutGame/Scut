

------------------------------------------------------------------
-- common.lua
-- Author     : ChenJM
-- Version    : 1.15
-- Date       :   
-- Description: 通用类，包含字体，颜色，适配位置等,
------------------------------------------------------------------




pWinSize=CCDirector:sharedDirector():getWinSize()




function PT(x,y)
    return CCPoint(x,y)
end


function SZ(width, height)
    return CCSize(width, height)
end 

function SCALEX(x)
    return CCDirector:sharedDirector():getWinSize().width/480*x
end
function SCALEY(y)
    return CCDirector:sharedDirector():getWinSize().height /320*y
end

FONT_NAME     = "黑体"



FONT_SM_SIZE  = SCALEX(30)






---


------获取资源的路径---------------------
function P(fileName)
	if fileName then
		return ScutDataLogic.CFileHelper:getPath(fileName)
	else
		return nil
	end
end

function SX(x)
    return SCALEX(x)
end 

function SY(y)
    return SCALEY(y)
end

