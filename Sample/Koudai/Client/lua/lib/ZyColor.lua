

ZyColor = {
	_color = nil
}


--
-------------------------静态函数------------------------
--
-- 黄色
function ZyColor:colorYellow()
	return ccc3(255, 255, 0)
end

-- 白色
function ZyColor:colorWhite()
	return ccc3(255, 255, 255)
end

-- 红色
function ZyColor:colorRed()
	return ccc3(255, 0, 0)
end

-- 绿色
function ZyColor:colorGreen()
	return ccc3(0, 255, 0)
end

-- 蓝色
function ZyColor:colorBlue()
	return ccc3(55, 133, 255)
end

-- 紫色
function ZyColor:colorPurple()
	return ccc3(139, 0, 255)
end

-- 黑色
function ZyColor:colorBlack()
	return ccc3(0, 0, 0)
end

-- 橙色
function ZyColor:colorOrange()
	return ccc3(255, 170, 0)
end

-- 橙红色
function ZyColor:colorOrangeRed()
	return ccc3(255, 88, 0)
end

-- 灰色
function ZyColor:colorGray()
	return ccc3(111, 111,111)
end

-- 标签颜色
function ZyColor:colorLabel()
	return ccc3(168, 179, 214)
end
--
function ZyColor:colorBlueDark()
	return ccc3(12, 162, 171)
end
--
function ZyColor:colorYellowDark()
	return ccc3(174, 124, 16)
end
--
--
function ZyColor:colorBlueLight()
	return ccc3(0, 191, 255)
end


function ZyColor:getRoleQualityColor(nQuality)
	if nQuality == 1 then--    1	白板
		return ZyColor:colorWhite()
	elseif nQuality == 2 then--2	绿色
		return ZyColor:colorGreen()
	elseif nQuality == 3 then--3	蓝色
		return ZyColor:colorBlue()
	elseif nQuality == 4 then--4	紫色
		return ZyColor:colorPurple()
	elseif nQuality == 5 then--5	黄色
        	return ZyColor:colorYellow()	
	else
		return ZyColor:colorWhite()
	end       
end;

function ZyColor:getEquiptQualityColor(nQuality)
    if nQuality == 1 then--    1	白板
        return ZyColor:colorWhite()
    elseif nQuality == 2 then--2	蓝色
        return ZyColor:colorBlue()
    elseif nQuality == 3 then--3	紫色
        return ZyColor:colorPurple()
    elseif nQuality == 4 then--4	橙色
        return ZyColor:colorOrange()
    elseif nQuality == 5 then--5	黄色
        return ZyColor:colorYellow()
--    elseif nQuality == 6 then--6 红色
--    	  return ZyColor:colorRed()
    else
        return ZyColor:colorWhite()
    end
end

function ZyColor:getCrystalColor(nQuality)
    if nQuality == 1 then--    1	灰色
	return ccc3(255, 255, 255)--灰的看不清     白的
    elseif nQuality == 2 then--2	绿色
        return ccc3(0, 255, 0)
    elseif nQuality == 3 then--3	蓝色
        return ccc3(0, 0, 255)
    elseif nQuality == 4 then--4	紫色
        return ccc3(139, 0, 255)
    elseif nQuality == 5 then--5	黄色
        return ccc3(255, 255, 0)
    elseif nQuality == 6 then--6	橙色
        return ccc3(255, 170, 0)
    else
        return ccc3(128,128,128)--灰色
    end
end

function ZyColor:worldBlueColor()
	return ccc3(125, 0, 0)
end

