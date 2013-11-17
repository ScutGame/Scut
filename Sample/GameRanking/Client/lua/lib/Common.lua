

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
function Half_Float(x)
    return x*0.5;
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


FONT_DEF_SIZE = SCALEX(18)
FONT_SM_SIZE  = SCALEX(15)
FONT_BIG_SIZE = SCALEX(23)
FONT_M_BIG_SIZE = SCALEX(63)
FONT_SMM_SIZE  = SCALEX(13)
FONT_FM_SIZE=SCALEX(11)
FONT_FMM_SIZE=SCALEX(12)
FONT_FMMM_SIZE=SCALEX(9)



ccBLACK = ccc3(0,0,0);
ccWHITE = ccc3(255,255,255);
ccYELLOW = ccc3(255,255,0);
ccBLUE = ccc3(0,0,255);
ccGREEN = ccc3(0,255,0);
ccRED = ccc3(255,0,0);
ccMAGENTA = ccc3(255,0,255);
ccPINK = ccc3(228,56,214);		-- 粉色
ccORANGE = ccc3(206, 79, 2)	  -- 橘红色
ccGRAY = ccc3(166,166,166);
ccC1=ccc3(45,245,250);
---通用颜色
ccRED1= ccc3(86,26,0)
ccYELLOW2=ccc3(241,176,63)

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





















































