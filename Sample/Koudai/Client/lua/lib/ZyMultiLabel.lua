--多功能label空间
--朱晓敏
--2012.1.14
--

require ("common.XmlParser")

ZyMultiLabel={
	_layer=nil,
}

--xml字符串事例
--[[
<label color='0,255,0' userdata='aaaa' tag='11' class='Payment.rechargeByMessage' fontname='' fontsize=''>[MyName]</label>
<image src='temp/001.png' size='123,421'/>
]]
--说明
--label标签必须要有文本内容
--属性有:
--color 		颜色
--fontname 		字体
--fontname 		字号
--tag  			tag值
--class 		当class不为空时，label显示的是linklabel，触发该class的方法
--userdata 		当class不为空时，设置linklable的userdata

--标签值为文本内容，class为空时支持\r\n 或 \n 换行
--image标签 当isani属性为空或者不为true时，src必填，当isani为true时，创建控件必须传入获得动画的方法
--属性有：
--isani		是否是动画
--src 		图片路径
--size		格式：width,height 设置图片大小
--userdata

--参数(*的为必填)： xml格式类型的string(*)，宽度(*)，字体，字号，每行的间隔,获得动画的方法（触发回传userdata参数）
--是否有下滑线
function ZyMultiLabel:new(content,width,fontname,fontsize,nYSpace,getAniSpriteFunc,isLine)
	local instance = {}
	setmetatable(instance, self)
	self.__index = self
	if fontname == nil then	
		fontname = FONT_NAME;
	end
	
	if fontsize == nil then	
		fontsize = FONT_FMM_SIZE;
	end
	
	if nYSpace == nil then --各控件y位置的间隔
		nYSpace = SY(1)
	end
	
	local layer = CCLayer:create()
	layer:setContentSize(SZ(width,SY(1)))  --临时大小
	local xmlTable=XmlParser:ParseXmlText(content) --xml字符串解析
	if #xmlTable > 0 then
		local nWidth=0 --当前行的宽度
		local nControlTable={} --保存各行的控件
		local nLine = 1;		
		for key,value in pairs(xmlTable) do			
			--取得属性
			local color = nil;
			local fname = nil;
			local fsize = nil;
			local userdata = nil;
			local tag = nil;
			local class = nil;			
			local src = nil;
			local size = nil;
			local text = value.Value;
			local isani=nil;
			for k,v in pairs(value.Attributes) do 
				k = string.lower(k)
				if k == "color" then
					color = v;					
				elseif k == "fontname" then
					fname = v;					
				elseif k == "fontsize" then
					fsize = v;					
				elseif k == "userdata" then
					userdata = v; --ZyFont.Split( v,",");
				elseif k == "tag" then
					tag = v;
				elseif k == "class" then
					class = v;				
				elseif k == "src" then
					src = v;
				elseif k == "size" then
					size = v;
				elseif k == "isani" then
					isani = string.lower(v);
				end
			end
			--一些属性处理，无设置的定义默认值
			if color == nil or color == "" then
				color = 	ZyColor:colorWhite();
			else
				color = ZyFont.Split(color,",")
				color = ccc3(color[1],color[2],color[3]);
			end
			if fname == nil or fname == "" then
				fname = fontname
			end
			if fsize == nil or fsize == "" then
				fsize = fontsize
			end
			
			if nControlTable[nLine] == nil then				
				nControlTable[nLine]={}
				nControlTable[nLine].height=0;
				nControlTable[nLine].items={};
			end
			
			-- local control = nil;
			if value.Name == "label" then
				if class ~= nil and class ~= "" then  --如果class有值则为linklabel类型
					local linkLabel=ZyLinkLable:new(text,color,fname,fsize,width,isLine)
					local csize=linkLabel:getContentSize();
					if csize.width + nWidth > width  then
						nLine = nLine + 1;
						nControlTable[nLine]={}
						nControlTable[nLine].height=0;
						nControlTable[nLine].items={};
						nWidth = csize.width
					else
						nWidth = nWidth + csize.width
					end
					if csize.height > nControlTable[nLine].height then
						nControlTable[nLine].height = csize.height;
					end
					table.insert(nControlTable[nLine].items,{item=linkLabel,sacelX=1,sacelY=1})
				--	linkLabel:registerScriptHandler(class);
					
					if userdata then
						linkLabel:setUserData(ZyFont.Split(userdata,","))
					end					
					if tag then
						linkLabel:setTag(tag)
					end
					
					linkLabel:setPosition(PT(0,0))--先临时放一个位置 之后再算位置
					linkLabel:addto(layer)					
				else
					--/r/n /n处理	
                    if 				text then
					local textTable=ZyFont.Split(string.gsub(text,"\r\n","\n"),"\n")
					for index,strText in pairs(textTable) do
						while true do
							local labelStr,str=ZyFont.subString(strText,width - nWidth , fname, fsize);
							strText = str;
							if labelStr == nil or labelStr == "" then
								nLine = nLine + 1;
								nWidth = 0;
								nControlTable[nLine]={}
								nControlTable[nLine].height=0;
								nControlTable[nLine].items={};							
							else
								local label = CCLabelTTF:create(labelStr, fname, fsize)
								local csize=label:getContentSize();							
								nWidth = nWidth + csize.width							
								if csize.height > nControlTable[nLine].height then
									nControlTable[nLine].height = csize.height;
								end
								table.insert(nControlTable[nLine].items,{item=label,sacelX=1,sacelY=1})
								if tag then
									label:setTag(tag)
								end
								label:setColor(color);
								label:setAnchorPoint(PT(0,0))
								label:setPosition(PT(0,0))--先临时放一个位置 之后再算位置
								layer:addChild(label,0)	
							end	
							if strText == nil or strText == "" then
								break;
							end
						end
						if index<#textTable then
							nLine = nLine + 1;
							nWidth = 0;
							nControlTable[nLine]={}
							nControlTable[nLine].height=0;
							nControlTable[nLine].items={};
						end
					end
				end
				end
			elseif value.Name == "image" then
				local img=nil
				if isani == "true" then
					img = getAniSpriteFunc(userdata);
				else
					img = CCSprite:create(P(src));
				end
				local scalex=1
				local scaley=1
				if size then
					size=ZyFont.Split(size,",")
					scalex=tonumber(size[1])/img:getContentSize().width
					scaley=tonumber(size[2])/img:getContentSize().height
					img:setScaleX(scalex)
					img:setScaleY(scaley)
				end
				
				local csize=img:getContentSize();
				if csize.width*scalex + nWidth > width  then
					nLine = nLine + 1;
					nControlTable[nLine]={}
					nControlTable[nLine].height=0;
					nControlTable[nLine].items={};
					nWidth = csize.width*scalex
				else
					nWidth = nWidth + csize.width*scalex
				end
				if csize.height*scaley > nControlTable[nLine].height then
					nControlTable[nLine].height = csize.height*scaley;
				end
				if tag then
					img:setTag(tag)
				end
				table.insert(nControlTable[nLine].items,{item=img,sacelX=scalex,sacelY=scaley})
				img:setAnchorPoint(PT(0,0))
				img:setPosition(PT(0,0))--先临时放一个位置 之后再算位置
				layer:addChild(img,0)	
			end			
		end	
		--先遍历一次计算layer高度
		local layerHeight=0
		for key,value in pairs(nControlTable) do
			layerHeight = value.height + layerHeight
		end
		layerHeight = layerHeight + (#nControlTable-1)*nYSpace;
		local layerWidth=width
		if #nControlTable==1 then
			layerWidth=nWidth
		end
		layer:setContentSize(SZ(layerWidth,layerHeight))
		--遍历所有控件设置position
		local offy = layerHeight;
		for key,value in pairs(nControlTable) do
			local offx=0
			offy = offy - value.height/2;
			for k,v in pairs (value.items) do
				v.item:setPosition(PT(offx,offy-v.item:getContentSize().height*v.sacelY/2))
				offx = offx + v.item:getContentSize().width*v.sacelX;
			end			
			offy = offy - value.height/2 - nYSpace;
		end
	end	
	
	instance._layer = layer
	return instance
end
--设置位置

function ZyMultiLabel:setPosition(point)
	self._layer:setPosition(point)
end

function ZyMultiLabel:getPosition(point)
	return self._layer:getPosition()
end
function ZyMultiLabel:setAnchorPoint(point)
	self._layer:setAnchorPoint(point)
end
function ZyMultiLabel:addto(parent, param1, param2)
	if type(param1) == "userdata" then
		parent:addChildItem(self._layer, param1)
	else
		if param2 then
			parent:addChild(self._layer, param1, param2)
		elseif param1 then
			parent:addChild(self._layer, param1)
		else
			parent:addChild(self._layer, 0)
		end
	end
end
--设置是否隐藏
function ZyMultiLabel:setVisible(visible)
	self._layer:setVisible(visible)
end
--获得是否隐藏
function ZyMultiLabel:getIsVisible()
	return self._layer:getIsVisible()
end

function ZyMultiLabel:getLayer()
	return self._layer
end

function ZyMultiLabel:getContentSize()
	return self._layer:getContentSize()
end

function ZyMultiLabel:setContentSize(size)
	self._layer:setContentSize()
end

function ZyMultiLabel:addChild(child ,zorder)
	if zorder ~= nil then
		self._layer:addChild(child, zorder);
    else
		self._layer:addChild(child, 0);
    end
end

function ZyMultiLabel:setTag(tag)
	self._layer:setTag(tag)
end

function  ZyMultiLabel:removeLabel()
	self._layer:getParent():removeChild(self._layer,true)
end;

function ZyMultiLabel:getTag()
	return self._layer:getTag()
end