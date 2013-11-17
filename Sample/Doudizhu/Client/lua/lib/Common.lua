

------------------------------------------------------------------
-- common.lua
-- Author     : ChenJM
-- Version    : 1.15
-- Date       :   
-- Description: 通用类，包含字体，颜色，适配位置等,
------------------------------------------------------------------


require("lib.ZyImage")

pWinSize=CCDirector:sharedDirector():getWinSize()
function ZyRequire()
	--local eLanguage = ScutUtility.CLocale:getLanguage()
	local strPath = nil
	if eLanguage == "zh_TW" then --繁体中文
		strPath2 = "config/language/ZH_TW/lib"
	else
		strPath1 = "config/language/ZH_CN/Language"
		strPath2 = "config/language/ZH_CN/lib"
	end
	require(strPath1)
	require(strPath2)
end

ZyRequire()

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
function Log(fmt,...)
	CCLuaLog(string.format(fmt,...))
end

--文件的路径--
--mode r w a b
--fmt... 同Log参数
function LogFile(fileName, mode, fmt,...)
	local f = io.open(ScutDataLogic.CFileHelper:getWritablePath(fileName), mode)
	f:write(string.format(fmt,...))
	f:write("\n")
	f:close()
end



--返回时间hh:mm:ss---
function formatTime(timeNum, format)
	if format == nil then
		format = "%02d:%02d:%02d"
	end
	return string.format(format, ZyCalculateTime(timeNum))
end


function ZyCalculateTime(timeNum)
	local nSec=timeNum
	local h=0
	local m=0
	local s=0
	h=math.floor(nSec/ 3600)
	m=math.floor((nSec%3600) /60)
	s=nSec%60
	return h,m,s
end

-- 解析服务器下发时间
function parseTime(strTime)
	local strMode = "(%d+)-(%d+)-(%d+)%s*(%d+)%:(%d+):(%d+)"
	local pos, leng, year, month, day, hour, min, sec = string.find(strTime, strMode)
	local time = {}
	time.year = year or 1970
	time.month = month or 1
	time.day = day or 1
	time.hour = hour or 0
	time.min = min or 0
	time.sec = sec or 0
	return time
end

-- 获取当前系统时间
function currentTime()
	return os.date("*t")
end

function isSameDay(t1, t2)
    if t1 == nil or t2 == nil then
        return false
    end
	if tonumber(t1.year) == tonumber(t2.year)
		and tonumber(t1.month) == tonumber(t2.month)
		and tonumber(t1.day) == tonumber(t2.day) then
		return true
	end
	return false
end

-- 事件用时间
function eventTime(ticks)
	local time = getTimeTable(ticks)
	local curTime = currentTime()
	if isSameDay(time, curTime) then
		return string.format("%02d:%02d", time.hour, time.min)
	else
		return string.format("%02d-%02d", time.month, time.day)
	end
end

--将服务器下发的Ticks时间转换成时间表	nTicks为nil时输出当前时间
--表格如下：
-- hour	2
-- min	20
-- wday	2
-- day	14
-- month	11
-- year	2011
-- sec	29
-- yday	318
-- isdst	false
function getTimeTable(nTicks)
	if nTicks then
		nTicks = nTicks - 3600 * 8
	else
		nTicks = os.time()
	end

	return os.date("*t", nTicks)
end

--将服务器下发的Ticks时间转换成时间字符串	nTicks为nil时输出当前时间
--输出格式：   2011-11-14 17:38:01
function getTimeString(nTicks)
	local tDate = getTimeTable(nTicks)

	return string.format("%04d-%02d-%02d %02d:%02d:%02d", tDate.year, tDate.month, tDate.day, tDate.hour, tDate.min, tDate.sec)
end

function Int64(number)
	return ScutDataLogic.CInt64:new_local(number)
end

function int64toNumber(int64)
    if int64 == nil then
        return nil
    else
	    return tonumber(int64:str())
	end
end

function compareFloat(float1, float2)
	local float21 = float2 - 0.000001
	local float22 = float2 + 0.000001
	if float1 >= float21 and float1 <= float22  then
		return 0
	elseif float1 < float21 then
		return -1
	elseif float1 > float22 then
		return 1
	end
end

function deleteFile(fileName)
	os.remove (ScutDataLogic.CFileHelper:getWritablePath(fileName))
end

function debugInfo()
	local info = debug.getinfo(3)
	local src = string.reverse(info.short_src)
	local pos = string.find(src, "\\")
	src = src.sub(src, 0, pos-1)
	src = string.reverse(src)
	local info2 = debug.getinfo(2)
	if info.name == nil then
	    info.name = "nil"
	end
	local str = string.format("%-25s %-25s %-20s line:%-10d %-30s", info2.name, src, info.name, info.currentline, os.date())
	return str
end

--获取节点在当前屏幕的位置
function getScreenPosition(node)
	local position = node:getPosition()
	while node:getParent() do
		node = node:getParent()
		local position2 = node:getPosition()
		position.x = position.x + position2.x
		position.y = position.y + position2.y
	end
	return position
end

--返回值为true表示保存成功，ITEMS数组TABLE，filename表示要保存的文件名
function saveItemsToFile(items,filename)
      local f = io.open(ScutDataLogic.CFileHelper:getWritablePath(filename), "w+")
      if f == nil or items == nil or #items <= 0 then
        return false
      end
      local key = ZyTable.keys(items[1])
      for i,x in ipairs(key) do
      	f:write(string.format("%s	",x))
      end
      f:write("\n")
      for i,x in ipairs(items) do
		  if x then
			  local itemId = nil;
			  local item = ""
			   for j,v in ipairs(key) do
					local data = x[v]
					if data ~= nil then
						if type(data) == "userdata" then
							item = item..string.format("%s	",data:str())
						else
							item = item..string.format("%s	",tostring(data))
						end
					end
			   end
			  f:write(item)
			  f:write("\n")
		  end
      end;
      f:flush()
      f:close()
      io.close();
end

--返回nil则表示失败
function getItemFromFile(filename)
	local f = io.open(ScutDataLogic.CFileHelper:getWritablePath(filename), "r")
    if f == nil then
      return nil
   	end
	local items = {}
	local keys = {}
	local itemIndex = 0
    for line in f:lines() do
        local index = 0
        local i = 0
        local j = 0
    	if #keys == 0 then
    		while true do
                i, j = string.find(line, "	", index)
                if i == nil then break end
                	ZyTable.push_back(keys,string.sub(line, index, i-1))
                	index = i+1;
            end
        else
            local elemIndex = 1
            items[itemIndex] = {}
    		while true do
                i, j = string.find(line, "	", index)
                if i == nil then break end
                local item = items[itemIndex]
                local key = keys[elemIndex]
                if key == nil then
                    break
                end
                item[key] = string.sub(line, index, i-1)
                elemIndex = elemIndex + 1
                index = i+1;
           end
    	end
    	itemIndex = itemIndex + 1
    end
    return items
end

-- 判断文件是否存在
function isFileExist(filename)
	local result = io.open(filename, "r")
	io.close()
	return result ~= nil
end

--是否包含非法字符，除数字与字母外的字符
function IsillegalString(inString)
	firstidx, lastidx = string.find(inString, "[%d%a]+")
	if firstidx ~= nil and lastidx == string.len(inString) then
	    return false
	end
	return true
end

--分割字符串
function Split(szFullString, szSeparator)
	local nSplitArray = {}
	local nFindStartIndex = 1
	local nSplitIndex = 1

	if szFullString == nil then
		return nSplitArray
	end

	while true do
		local nFindLastIndex = string.find(szFullString, szSeparator, nFindStartIndex)
		if not nFindLastIndex then
			nSplitArray[nSplitIndex] = string.sub(szFullString, nFindStartIndex, string.len(szFullString))
			break
   		end
		nSplitArray[nSplitIndex] = string.sub(szFullString, nFindStartIndex, nFindLastIndex - 1)
		nFindStartIndex = nFindLastIndex + string.len(szSeparator)
		nSplitIndex = nSplitIndex + 1
	end
	return nSplitArray
end


--场景进入退出动画
local time = 0.6
local sceneTable = {}

function SlideInLPushScene(scene)
   local beforeScene = CCDirector:sharedDirector():getRunningScene()
   sceneTable[#sceneTable+1] = beforeScene
   local s = CCTransitionFade:create(time,scene)
   CCDirector:sharedDirector():pushScene(s)
end

function SlideInLReplaceScene(scene,type)
	if type then
		MainScene.releaseResource()
		GuideLayer.releaseResource()
	end
   local s = CCTransitionFade:transitionWithDuration(time,scene)
   commReplaceScene(s)
end

function SlideInRPopScene(scene,type)
	local beforeScene = sceneTable[#sceneTable]
	local s = CCTransitionFade:create(time,beforeScene)
    	commReplaceScene(s)
	if type then
    		MainMenuLayer.init(type, beforeScene)
    	end
	sceneTable[#sceneTable] = nil
end

function popupScene()
	commPopScene()
end

function commPopScene()
	CCDirector:sharedDirector():popScene()
	CCTextureCache:sharedTextureCache():removeUnusedTextures()
end

function commReplaceScene(scene)
	CCDirector:sharedDirector():replaceScene(scene)
	CCTextureCache:sharedTextureCache():removeUnusedTextures()
end


--使用Lua遍历指定目录，获取所有文件，并使用自定义的函数处理每一个文件
--遍历目录，并对所有的文件进行处理
function fuc_dir_file(dirpath,func)
    os.execute('dir "'.. dirpath.. '" /s > temp.txt')
    io.input("temp.txt")
    local dirname = ""
    local filename = ""
    for line in io.lines() do
        local a,b,c
        --匹配目录
        a,b,c=string.find(line,"^%s*(.+)%s+的目录")
        if a then
         dirname = c
     end
     --匹配文件
        a,b,c=string.find(line,"^%d%d%d%d%-%d%d%-%d%d%s-%d%d:%d%d%s-[%d%,]+%s+(.+)%s-$")
        if a then
         filename = c
         func(dirname.. "\\".. filename)
        end
    end
end

function getUserDataPath()
	local path = string.format("userdata/%s/", accountInfo.mUser91Name)
	if not ScutDataLogic.CFileHelper:isDirExists(path) then
		ScutDataLogic.CFileHelper:createDir(path)
	end
	return path
end

function IMAGE(file)
	local texture = CCTextureCache:sharedTextureCache():addImage(P(file))
	return texture
end

function getMidPoint(startPoint, endPoint)
	if startPoint == nil or endPoint == nil then
		return PT(0, 0)
	end
	local midPoint = PT(0, 0)
	midPoint.x = startPoint.x + (endPoint.x - startPoint.x)/2
	midPoint.y = startPoint.y + (endPoint.y - startPoint.y)/2
	return midPoint
end

function transToLocalPosition(x, y)
	local pos = PT(0, 0)
	pos.x = x * TILE_SIZE.width - TILE_SIZE.width / 2
	pos.y = y * TILE_SIZE.height - TILE_SIZE.height / 2
	return pos
end

function transToServerPosition(x, y)
	local pos = PT(0, 0)
	pos.x = math.floor(x / TILE_SIZE.width)
	pos.y = math.floor(y / TILE_SIZE.height)
	return pos
end



--创建物品图片
function createItemImage(itemId,ImageId)
	local imagePath,itemAbility = getItemImageRoute(itemId,ImageId)

	local bgSprite = CCSprite:spriteWithTexture(IMAGE(mAbilityImagePath[itemAbility]))
	bgSprite:setAnchorPoint(PT(0, 0))
	bgSprite:setPosition(PT(0, 0))

	local itemImage = CCSprite:spriteWithTexture(IMAGE(imagePath))
	bgSprite:addChild(itemImage, 0)
	itemImage:setAnchorPoint(PT(0.5, 0.5))
	itemImage:setPosition(PT(bgSprite:getContentSize().width/2, bgSprite:getContentSize().height/2))
	return bgSprite
	
end

function createAction(texture, frameCount, delay)
	if not texture then
		return
	end
	delay = delay or 0.05
	local size = texture:getContentSize()
	local frameWidth = size.width / frameCount

	local animation = CCAnimation:animation()
	animation:setDelay(delay)
	for i=1, frameCount do
		animation:addFrameWithTexture(texture, CCRectMake((i-1)*frameWidth, 0, frameWidth, size.height))
	end
	local action = CCAnimate:actionWithAnimation(animation, false)
	return action
end

function timeToDate(ticks)
	local time = getTimeTable(ticks)
	local curTime = currentTime()
	local strTime = string.format("%04d-%02d-%02d %02d:%02d", time.year, time.month, time.day, time.hour, time.min)
	if time.year == curTime.year then
		if time.yday == curTime.yday then 			-- 今天
		    strTime =  GameString.IDS_TODAY.. string.format(" %02d:%02d", time.hour, time.min)
	    elseif time.yday == curTime.yday - 1 then 	-- 昨天
	        strTime =  GameString.IDS_YESTERDAY.. string.format(" %02d:%02d", time.hour, time.min)
	    elseif time.yday == curTime.yday - 2 then 	-- 前天
	        strTime =  GameString.IDS_DAY_BEFORE_YESTERDAY.. string.format(" %02d:%02d", time.hour, time.min)
	    end
	end
	return strTime;
end



--缩放控件
function setScale(item, scaleValue)
	itemSize = item:getContentSize()
	item:setScale(scaleValue)
	item:setContentSize(SZ(itemSize.width * scaleValue, itemSize.height * scaleValue))
end


--按平台
function createAndColor(msg, size, CCTextAlignment, fontName, fontsize, strokeColor, color)
		local lbl = CCLabelTTF:create(msg, size, CCTextAlignment, fontName, fontsize, strokeColor)
		lbl:setColor(color)
		return lbl
end


---算字符长度
function utfstrlen(str)
local len = #str;
local left = len;
local cnt = 0;
local arr={0,0xc0,0xe0,0xf0,0xf8,0xfc};
while left ~= 0 do

local tmp=string.byte(str,-left);

local i=#arr;
while arr[i] do
	if tmp>=arr[i] then 
		left=left-i;
		break;
	end
	i=i-1;
end

cnt=cnt+1;

end
return cnt;
end 


-----
function utfstrIndex(str,num)
local len = #str;
local left = len;
local cnt = 0;
local arr={0,0xc0,0xe0,0xf0,0xf8,0xfc};
local maxIndex=1
while cnt <num and  left~=0 do
local tmp=string.byte(str,-left);
local i=#arr;
while arr[i] do
	if tmp>=arr[i] then 
		left=left-i;
		break;
	end
	i=i-1;
end
cnt=cnt+1;
end
maxIndex=#str-left


local content=string.sub(str, 1,maxIndex)
if maxIndex<#str then
content=content.."..."
end
return content
end;

--------
function getQualityBg(qualityType, itemType)
	if itemType == 1 then--装备图标背景
		if qualityType == 1 then
			return "common/icon_8015_1.png"
		elseif qualityType == 2 then
			return "common/icon_8015_2.png"
		elseif qualityType == 3 then
			return "common/icon_8015_3.png"
		elseif qualityType == 4 then
			return "common/icon_8015_4.png"
		else
			return "common/icon_8015_3.png"
		end
	elseif itemType == 2 then--战斗背景
		if qualityType == 1 then
			return "common/icon_8016_1.png"
		elseif qualityType == 2 then
			return "common/icon_8016_2.png"
		elseif qualityType == 3 then
			return "common/icon_8016_3.png"
		elseif qualityType == 4 then
			return "common/icon_8016_4.png"
		else
			return "common/icon_8032.png"
		end	
	elseif itemType == 3 then--大图背景	
		if qualityType == 1 then
			return "common/icon_8017_1.png"
		elseif qualityType == 2 then
			return "common/icon_8017_2.png"
		elseif qualityType == 3 then
			return "common/icon_8017_3.png"
		elseif qualityType == 4 then
			return "common/icon_8017_4.png"
		else
			return  "common/icon_8017_3.png"
		end		
	end

end;


--获取数字图片
function getNumberSprite(nNumber,type)
	--1 减 2 加
	local imageFile = "common/panle_1042.png"
	if type== 2 then
		 imageFile = "common/panle_1043.png"
	end
	local texture = IMAGE(imageFile)
	if texture == nil then
		return nil
	end
	local txSize = texture:getContentSize()
	local strNumber = tostring(nNumber)
	strNumber=strNumber or 0
	strNumber=math.abs(strNumber)
	local nLength = string.len(strNumber)
	local pNode = CCNode:create()
	local nWidth = txSize.width/3
	local nHeight = txSize.height/4
	local subFrame = CCSpriteFrame:createWithTexture(texture, CCRectMake(0, 3*nHeight,nWidth,nHeight))
	local nLeft =-nWidth
		local subSprite = CCSprite:createWithSpriteFrame(subFrame)
		pNode:setPosition(PT(0, 0))
		pNode:addChild(subSprite, 0)
		subSprite:setAnchorPoint(PT(0,0))
		subSprite:setPosition(PT(0, 0 ))
		nLeft =0
	for i = 1,nLength do
		local nDig = tonumber(string.sub(strNumber, i, i))
		if nDig == 0 then
			subFrame = CCSpriteFrame:createWithTexture(texture, CCRectMake(nWidth, 3*nHeight,nWidth,nHeight))
		else
			subFrame = CCSpriteFrame:createWithTexture(texture, CCRectMake((nDig- 1)%3*nWidth,math.floor((nDig -1)/3)*nHeight,nWidth,nHeight))
		end
		subSprite = CCSprite:createWithSpriteFrame(subFrame)
		pNode:addChild(subSprite, 0)
		subSprite:setAnchorPoint(PT(0,0))
		nLeft = nLeft + nWidth
		subSprite:setPosition(PT(nLeft, 0 ))
	end
	pNode:setContentSize(SZ(nLeft, subSprite:getContentSize().height))
	return pNode
end

