 module("ZyTable", package.seeall)

--适合索引从1开始的连续Table                                                                 \/]]
function reverse(self)
    local copy = table.copy(self);
    local len = #self;

    for i = 1, len do
        self[i] = copy[len];
        len = (len - 1);
    end;

    return self;
end;

--------------------------------------------------------------------------------
--适合索引从1开始的连续Table
function slice(self, from, len)
    if (from + 0) < 0 then
        from = (#self + 1 + from);
    elseif from == 0 then
        from = (from + 1);
    end;

    if not len then
        len = #self;
    elseif len <= 0 then
        len = (#self + len + 1);
    end;

    for i, v in ipairs(self) do
        if i < from or i > len then
            self[i] = nil;
        end;
    end;

    return self;
end;

--------------------------------------------------------------------------------
--适合索引从1开始的连续Table
function merge(self, ...)
    for i = 1, arg.n do
        for k, v in pairs(arg[i]) do
            self[type(k) == 'number' and (k + #self) or k] = v;
        end;
    end;

    return self;
end;

--------------------------------------------------------------------------------

function dump(self)
    local result = tostring(self)..' {\n';
    local count, scope = 1, {};
    local map = {[true] = 'true', [false] = 'false'};
    local function _dump(t)
        local id = tostring(t);
        local tab = ('    '):rep(count);

        if scope[id] then
            result = ('%s%s*RECURSION*\n'):format(result, tab);
            return;
        else
            scope[id] = true;
        end;

        for k, v in pairs(t) do
            if type(v) ~= 'table' then
                result = ('%s%s[%s] => (%s)%s\n'):format(result, tab, k, type(v), tostring(map[v] or v));
            else
                result = ('%s%s[%s] => %s {\n'):format(result, tab, k, id);
                count = (count + 1);
                _dump(v);
                result = ('%s%s}\n\n'):format(result, tab);
                count = (count - 1);
            end;
        end;
    end;

    _dump(self);
    return result..'}';
end;

--------------------------------------------------------------------------------

function keys(self)
    local result = {};

    for k, v in pairs(self) do
        table.insert(result, k);
    end;

    return result;
end;

--------------------------------------------------------------------------------

function values(self)
    local result = {};

    for k, v in pairs(self) do
        table.insert(result, v);
    end;

    return result;
end;

--------------------------------------------------------------------------------

function copy(self)
    local result = {};

    for k, v in pairs(self) do
        if type(v) == 'table' then
            result[k] = table.copy(v);
        else
            result[k] = v;
        end;
    end;

    return result;
end;


function th_table_dup(ori_tab)
    if (type(ori_tab) ~= "table") then
        return nil;
    end
    local new_tab = {};
    for i,v in pairs(ori_tab) do
        local vtyp = type(v);
        if (vtyp == "table") then
            new_tab[i] = th_table_dup(v);
        elseif (vtyp == "thread") then
            new_tab[i] = v;
        elseif (vtyp == "userdata") then
            new_tab[i] = v;
        else
            new_tab[i] = v;
        end
    end
    return new_tab;
end


--------------------------------------------------------------------------------

function each(self, callback)
    for k, v in pairs(self) do
        if type(v) == 'table' then
            self[k] = table.each(v, callback);
        else
            self[k] = callback(k, v);
        end;
    end;

    if #self > 0 then
        return self;
    end;
end;

--------------------------------------------------------------------------------

function search(self, value)
    for k, v in pairs(self) do
        if v == value then
            return k;
        end;
    end;
end;

--------------------------------------------------------------------------------

function first(self)
    local key = pairs(self);
    return self[key(self)];
end;

--------------------------------------------------------------------------------

function last(self)
	return self[#self]
end;

--------------------------------------------------------------------------------

function size(self)
    local count = 0;

    for k in pairs(self) do
        count = (count + 1);
    end;

    return count;
end;


--------------------------------------------------------------------------------

function insert(self, pos, value)
    table.insert(self, pos, value)
end

--------------------------------------------------------------------------------

--针对Key是数组的下标的情况
function push_back(self,value)
    table.insert(self, value)
end

--------------------------------------------------------------------------------

function remove(self, pos)
    table.remove(self, pos)
end

--------------------------------------------------------------------------------

function pop_back(self)
    table.remove(self)
end

--------------------------------------------------------------------------------

function clear(self)
	for k,v in pairs(self) do
	    table.remove(self, k)
	end
end

--为table t设置默认值
local key = {}
local mt = {__index = function(t) return t[key] end}
function setDefault(t, d)
	t[key] = d
	setmetatable(t, mt)
end

-- 跟踪table的访问
local index = {}
mt = {
	__index = function(t, k)
		print("*access to element " .. tostring(k))
		return t[index][k]
	end,

	__newindex = function(t, k, v)
		print("*update of element " .. tostring(k) .. " to " .. tostring(v))
		t[index][k] = v
	end
}

function track(t)
	local proxy = {}
	proxy[index] = t
	setmetatable(proxy, mt)
	return proxy
end

-- 只读的table
function readOnly(t)
	local proxy = {}
	local mt = {
		__index = t,
		__newindex = function(t, k, v)
			error("attempt to update a read-only table", 2)
		end
	}
	setmetatable(proxy, t)
	return proxy
end

-- 连接嵌套的字符串数组
function rconcat(t)
	if type(t) ~= "table" then return t end
	local res = {}
	for i = 1, #t do
		res[i] = rconcat(t[i])
	end
	return table.concat(res)
end


---
function GetPaging(pageSize,list)
	local pageCount=math.ceil(#list/pageSize)
	local count=0
	local lastList={}
	local sunList={}
	
	for k, v in pairs(list) do	

		sunList[#sunList+1]=v
		count=count+1
		if count>=pageSize then
			lastList[#lastList+1]=sunList
			sunList={}
			count=0
		end
	end
	return pageCount,lastList
end;


--排序
function orderBy(list,key)
	local length=#list
	for i=1 , length do --逐步扩大有序区
		j=i+1
		if list[j] then
		if   (list[j][key])<(list[i][key])   then
			list[0]=list[j] --存储待排序元素
			while ((list[0][key])<(list[i][key])) do --查找在有序区中的插入位置，同时移动元素
				list[i+1]=list[i] --移动
				i=i-1 --查找
			end;
			list[i+1]=list[0];--将元素插入
		end
		end
		i=j-1 --还原有序区指针
	end
	list[0]=nil
	return list
end;


function Exists(x,y,table)
	for k, v in pairs(table) do
		if x==v.x and y==v.y then
			return true
		end
	end
	return false
end;

function GetValue(x,y,table)
	for k, v in pairs(table) do
		if x==v.x and y==v.y then
			return v
		end
	end
	return nil
end;



