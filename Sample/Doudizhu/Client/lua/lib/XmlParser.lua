-----------------------------------------------------------------------------------------
-- LUA only XmlParser from Alexander Makeev
-----------------------------------------------------------------------------------------
XmlParser = {};

function XmlParser:ToXmlString(value)
	--value = string.gsub (value, "&", "&amp;");		-- '&' -> "&amp;"
	value = string.gsub (value, "<", "&lt;");		-- '<' -> "&lt;"
	value = string.gsub (value, ">", "&gt;");		-- '>' -> "&gt;"
	--value = string.gsub (value, "'", "&apos;");	-- '\'' -> "&apos;"
	--value = string.gsub (value, "\"", "&quot;");	-- '"' -> "&quot;"
	-- replace non printable char -> "&#xD;"
	--[[
   	value = string.gsub(value, "([^%w%&%;%p%\t% ])",
       	function (c) 
       		return string.format("&#x%X;", string.byte(c)) 
       		--return string.format("&#x%02X;", string.byte(c)) 
       		--return string.format("&#%02d;", string.byte(c)) 
       	end);
		]]
	return value;
end

function XmlParser:FromXmlString(value)

  	value = string.gsub(value, "&#x([%x]+)%;",
      	function(h) 
      		return string.char(tonumber(h,16)) 
      	end);
  	value = string.gsub(value, "&#([0-9]+)%;",
      	function(h) 
      		return string.char(tonumber(h,10)) 
      	end);
      	--[[
	value = string.gsub (value, "&quot;", "\"");
	value = string.gsub (value, "&apos;", "'");
	value = string.gsub (value, "&gt;", ">");
	value = string.gsub (value, "&lt;", "<");
	value = string.gsub (value, "&amp;", "&");
	--]]
	value = string.gsub (value, "&quot;", "\"");
	value = string.gsub (value, "&apos;", "'");
	value = string.gsub (value, "&gt;", ">");
	value = string.gsub (value, "&lt;", "<");
	value = string.gsub (value, "&amp;", "&");
	
	return value;
end
   
function XmlParser:ParseArgs(s)
	local arg = {}
	string.gsub(s, "(%w+)=([\"'])(.-)%2", function (w, _, a)
    	arg[w] = self:FromXmlString(a);
  	end)
	return arg
end

function XmlParser:ParseXmlText(xmlText)
	local stack = {}
	local top = {Name=nil,Value=nil,Attributes={},ChildNodes={}}
	
	table.insert(stack, top)
	local ni,c,label,xarg, empty
	local i, j = 1, 1
	while true do
		ni,j,c,label,xarg, empty = string.find(xmlText, "<(%/?)([%w:]+)(.-)(%/?)>", i)
		if not ni then 
        break
        end
		local text = string.sub(xmlText, i, ni-1);
		if not string.find(text, "^%s*$") then
			top.Value=(top.Value or "")..self:FromXmlString(text);
		end
		if empty == "/" then  -- empty element tag
			table.insert(top.ChildNodes, {Name=label,Value=nil,Attributes=self:ParseArgs(xarg),ChildNodes={}})
		elseif c == "" then   -- start tag
			top = {Name=label, Value=nil, Attributes=self:ParseArgs(xarg), ChildNodes={}}
			table.insert(stack, top)   -- new level
			--log("openTag ="..top.Name);
		else  -- end tag
			local toclose = table.remove(stack)  -- remove top
			--log("closeTag="..toclose.Name);
			top = stack[#stack]
			if #stack < 1 then
				error("XmlParser: nothing to close with "..label)
			end
			if toclose.Name ~= label then
				error("XmlParser: trying to close "..toclose.Name.." with "..label)
			end
			table.insert(top.ChildNodes, toclose)
		end
		i = j+1
	end
	local text = string.sub(xmlText, i);
	if not string.find(text, "^%s*$") then
		stack[#stack].Value=(stack[#stack].Value or "")..self:FromXmlString(text);
	end
	if #stack > 1 then
	--	error("XmlParser: unclosed "..stack[#stack].Name)
	end
	return stack[1].ChildNodes;
end

