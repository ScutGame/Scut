------------------------------------------------------------------
-- ScutEntity.lua
-- Author     :
-- Version    : 1.0
-- Date       :
-- Description: The entity manager
------------------------------------------------------------------

--The ScutEntity class
ScutEntity = {}
g__entitydata = {}

function ScutEntityFactory(schemaName)

    local schema = ScutSchema:new(schemaName)
    if not schema:isEmpty() then
        data = g__entitydata[schemaName]
        if data == nil then
            g__entitydata[schemaName] = {}
            data = g__entitydata[schemaName]
        end
        return ScutEntity:new(schema, data)
    end
    return nil
end

--[[Get ScutEntity a instance.
    @schema: This a required for schema object parameter;
    @data: This a required for source data;
    @key: The entity's key;
]]--
function ScutEntity:new(schema, data, key)
	local instance = {}
	setmetatable(instance, self)
	self.__index = self
	
    instance.__root = root
    instance.__schema = schema
    instance.__data = data
    instance.__key = key
    instance.__keyTemp = nil
    instance.__pos = 0
    return instance
end

function ScutEntity:getKey()
    return self.__key
end

function ScutEntity:getVal()
    return self.__data
end

function ScutEntity:getSchema()
    return self.__schema
end

function ScutEntity:create()
    return ScutEntity:new(self.__schema, {})
end

function ScutEntity:addBegin()
    self.__keyTemp = {}
end 

function ScutEntity:add(val)
    local index = self.__pos + 1
    if self.__schema:hasKeyIndex(index) then
        table.insert(self.__keyTemp, val) --push key
    end
    table.insert(self.__data, val)
    self.__pos = index
end

function ScutEntity:addEnd()
    local count = table.getn(self.__keyTemp)
    if count == 1 then
        self.__key = self.__keyTemp[1]
    elseif count > 1 then
        self.__key = table.concat(self.__keyTemp, "-")
    end
    self.__keyTemp = nil
end 

function ScutEntity:addDict(pair)
    local index = self.__pos + 1
    local data = pair:getVal()
    if table.getn(data) == 2 then
        local key = data[1]
        local value = data[2]
        self.__data[key] = value
    end
    self.__pos = index
end

function ScutEntity:addEntity(entity)
    local index = self.__pos + 1
    local key = entity:getKey()
    local data = entity:getVal()
    if key ~=nil and key ~= "" then
        self.__data[key] = data
    else 
        table.insert(self.__data, data)
    end
    self.__pos = index
end

function ScutEntity:hasChild(name)
    local field = self.__schema:getField(name)
    if field then
        return field:hasChild()
    end
    return false
end

function ScutEntity:get(name)
    local field = self.__schema:getField(name)
    if field then
        local index = field:getIndex()
        if field:hasChild() then
            local schema = field:getChild()
            if schema then
                return ScutEntity:new(schema, self.__data[index])
            end
        else
            return self.__data[index]
        end
    end
    return nil
end

function ScutEntity:first()
    local data = self.__data
    for k,v in pairs(data) do
        return ScutEntity:new(self.__schema, v, k)
    end
end

function ScutEntity:foreach(func)
    local data = self.__data
    for k,v in pairs(data) do
        local child = ScutEntity:new(self.__schema, v, k)
        if func then 
            func(child)
        end
    end
end

function ScutEntity:findAll(match, sortFunc)
    local recordList = {}
    self:foreach(function(record)
        if match == nil or match(record) then
            table.insert(recordList, record)
        end
    end)
    if sortFunc then
        table.sort(recordList, sortFunc)
    end
    return recordList
end