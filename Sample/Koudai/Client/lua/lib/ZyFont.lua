
module("ZyFont", package.seeall)

-- 记忆池
mWideWordWidth = {}
mUnwideWordWidth = {}
mUnwideWordWidth2 = {}
mSpaceWidth = {}
mLineHeight = {}
mCharSize={};

-- 获取字体行高
function lineHeightOfFont(fontName, fontSize)
	local key = generateKey(fontName, fontSize)
	if mLineHeight[key] == nil then
		local label = CCLabelTTF:create(Language.IDS_SURE, FONT_NAME, fontSize)
		--label:create(Language.IDS_SURE, FONT_NAME, fontSize)
		mLineHeight[key] = label:getContentSize().height
		label:delete()
	end
	return mLineHeight[key]
end

-- 获取非宽字符的宽度
function wordWidth(fontName, fontSize)
	local key = generateKey(fontName, fontSize)
	if mUnwideWordWidth[key] == nil then
		local str = "ABCDEFGHIGKLMNopqrstuvwxyz1234567890"
		local label = CCLabelTTF:create(str, fontName, fontSize)
		--label:create(str, fontName, fontSize)
		mUnwideWordWidth[key] = label:getContentSize().width / string.len(str)
		label:delete()
	end
	return mUnwideWordWidth[key]
end
--
function wordWidth2(word, fontName, fontSize)
	local key = generateKey(fontName, fontSize)
	if mUnwideWordWidth2[key] == nil then
		mUnwideWordWidth2[key] = {}
	end
	if mUnwideWordWidth2[key][word] == nil then
		local label = CCLabelTTF:new()
		label:create(word, fontName, fontSize)
		mUnwideWordWidth2[key][word] = label:getContentSize().width
		label:delete()
	end
	return mUnwideWordWidth2[key][word]
end

-- 获取宽字符的宽度
function wideWordWidth(fontName, fontSize)
	local key = generateKey(fontName, fontSize)
	if mWideWordWidth[key] == nil then
		local str = Language.IDS_FONT_12_TEST
		local label = CCLabelTTF:create(str, fontName, fontSize)
		--label:create(str, fontName, fontSize)
		mWideWordWidth[key] = label:getContentSize().width / (string.len(str) / 3)
		label:delete()
	end
	return mWideWordWidth[key]
end



-- 获取单行字符串的宽度
function stringWidth(str, fontName, fontSize)
	if string.len(str) > 255 then
		return SX(1500)
	else
		local label = CCLabelTTF:create(str, FONT_NAME, FONT_SM_SIZE)
		--label:create(str, FONT_NAME, FONT_SM_SIZE)
		local width = label:getContentSize().width
		label:delete()
		return width
	end
end



-- 获取多行字符串的区域大小
function stringSize(str, width, fontName, fontSize)
	local nWidthWide = wideWordWidth(fontName, fontSize)
	local nWidthUnwide = wordWidth(fontName, fontSize)
	local nLine = 1
	local nLen = string.len(str)
	local nPos = 0
	local nWidth = 0
	for i = 1, nLen do
		if i > nPos then
			if ZyBit.bit:_and(string.byte(str, i), 0x80) ~= 0 then
				nWidth = nWidth + nWidthWide
				nPos = i + 2
			else
				if string.sub(str, i, i) == "\n" then
					nWidth = 0
					nLine = nLine + 1
				else
					nWidth = nWidth + nWidthUnwide
				end
				nPos = i
			end

			if nWidth + nWidthWide > width then
				nWidth = 0
				nLine = nLine + 1
			end
		end
	end

	if nLine == 1 then
		width = stringWidth(str)
	end

	return CCSize(width, nLine * lineHeightOfFont(fontName, fontSize))

end


-- 获取非宽字符的宽度
function spaceWidth(fontName, fontSize)
	local key = generateKey(fontName, fontSize)
	if mSpaceWidth[key] == nil then
		local str = "                    "
		local label = CCLabelTTF:new()
		label:create(str, fontName, fontSize)
		mSpaceWidth[key] = label:getContentSize().width / string.len(str)
		label:delete()
	end
	return mSpaceWidth[key]
end




-- 给定宽度、字符串，返回相应子字符串及长度
function stringWithWidth(str, width, fontName, fontSize)
	local nWidthWide = wideWordWidth(fontName, fontSize)
	local nWidthUnwide = wordWidth(fontName, fontSize)
	local nLen = string.len(str)
	local nPos = 0
	local nWidth = 0
	for i = 1, nLen do
		if i > nPos then
			if ZyBit.bit:_and(string.byte(str, i), 0x80) ~= 0 then
				nPos = i + 2
				nWidth = nWidth + nWidthWide
			else
				nPos = i
				if string.sub(str, i, i) == "\n" then
					break
				else
					nWidth = nWidth + nWidthUnwide
				end
			end

			if nWidth + nWidthWide > width then
				break
			end
		end
	end

	return string.sub(str, 1, nPos), nPos
end




-- 给定宽度，返回相应个数的空格字符串
function spaceStringWithWidth(width, fontName, fontSize)
	local w = spaceWidth(fontName, fontSize)
	return string.rep(" ",  math.ceil(width / w)+2)
end
--返回一个空格的  为了防止把以前的逻辑改了。
function spaceStringWithWidth2(width, fontName, fontSize)
	local w = spaceWidth(fontName, fontSize)
	return string.rep(" ",  math.ceil(width / w)+1)
end

-- 生成key
function generateKey(fontName, fontSize)
	return string.format("%s%d", fontName, fontSize)
end

--
function stringLength(str)
	local nLen = string.len(str)
	local nPos = 0
	local nRet = 0
	for i = 1, nLen do
		if i > nPos then
			if ZyBit.bit:_and(string.byte(str, i), 0x80) ~= 0 then
				nPos = i + 2
			else
				nPos = i
			end

			nRet = nRet + 1
		end
	end
	return nRet
end
--将str分解成 width宽的字符串 和  剩余的字符串
function subString(str , width , fontName, fontSize)
	if str == "" or str	== nil then
		return nil;
	end
	local nWidth=0;
	local nLine="";
	local j = 0
	for i = 1, string.len(str) do
	    if j < i then
            local ancii=string.byte(str,i)
            local strChar="";
            if ancii > 128 and ZyBit.bit:_and(ancii, 0x40) ~= 0 then
                local l = ZyBit.bit:_and(ancii, 0xF0) / 16
                if l == 0xF then
                    strChar=string.sub(str,i,i+3);
                    j=i+3
                elseif l == 0xE then
                    strChar=string.sub(str,i,i+2);
                    j=i+2
                elseif l == 0xC then
                    strChar=string.sub(str,i,i+1);
                    j=i+1
                else--xx
                    strChar=string.sub(str,i,i);
                end
            else
                strChar=string.sub(str,i,i);
            end
            local charWidth=charSize(strChar,fontName, fontSize).width;
            if nWidth + charWidth > width then			
                return nLine,string.sub(str,i,string.len(str))
            else
                nLine=nLine..strChar
                nWidth=nWidth+charWidth
            end
        end
	end
 	return nLine
end

--获得单个字符的宽度
function charSize(char,fontName, fontSize)
	if fontName == nil then
		fontName = FONT_NAME;
	end
	
	if fontSize == nil then
		fontSize=FONT_SM_SIZE;
	end
	
	local key = generateKey(fontName, fontSize)	
	local ancii=string.byte(char)
	--中文等字符
	if  ancii > 128 then
		return getCharSize(key,"chinese",char, fontName, fontSize)
	--大写
	elseif ancii >= 65 and ancii <= 90 then
		if char == "I" then
			return getCharSize(key,"I","I", fontName, fontSize)
		elseif char == "M" then
			return getCharSize(key,"M","M", fontName, fontSize)
		elseif char == "W" then
			return getCharSize(key,"W","W", fontName, fontSize)
		else
			return getCharSize(key,"A","A", fontName, fontSize)
		end
	--其他
	else
		if char == "i" then
			return getCharSize(key,"i","i", fontName, fontSize)
		elseif char == "m" then
			return getCharSize(key,"m","m", fontName, fontSize)
		elseif char == "w" then
			return getCharSize(key,"w","w", fontName, fontSize)
		elseif char == "l" then
			return getCharSize(key,"l","l", fontName, fontSize)
		else
			return getCharSize(key,"a","a", fontName, fontSize)
		end
	end
end

--获得长度
function getCharSize(key,word,char, fontName, fontSize)
	if mCharSize[key] and mCharSize[key][word] then
		return mCharSize[key][word];
	else
	    if mCharSize[key] == nil then
	        mCharSize[key]={}
	    end
		local label = CCLabelTTF:create(char, fontName, fontSize)
	--	label:create(char, fontName, fontSize)
		local size=label:getContentSize()
		mCharSize[key][word]=size;
		label:delete()
		return size;			
	end
end

--字符串分割
function Split(szFullString, szSeparator)
	local nFindStartIndex = 1
	local nSplitIndex = 1
	local nSplitArray = {}
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
