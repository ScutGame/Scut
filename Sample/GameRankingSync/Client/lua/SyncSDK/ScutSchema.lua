------------------------------------------------------------------
-- ScutSchema.lua
-- Author     :
-- Version    : 1.0
-- Date       :
-- Description: The schema of entity
------------------------------------------------------------------

--The ScutSchemaField class
local _hasChildName = "_hasChild"
ScutSchemaField = {}
function ScutSchemaField:new(name, field)
	local instance = {}
	setmetatable(instance, self)
	self.__index = self
	
    instance.__name = name
    instance.__field = field or {}
    instance.__isTable = type(field) == type(table)
    return instance
end

function ScutSchemaField:hasChild()
    if self.__isTable then
        return self.__field[_hasChildName] or false
    end
    return false
end

function ScutSchemaField:getChild()
    if self:hasChild() then
        return ScutSchema:new(self.__name, self.__field)
    end
    return nil
end

function ScutSchemaField:getName()
    return self.__name
end

function ScutSchemaField:getIndex()
    if self.__isTable then
        return self.__field[1] or -1
    end
    return -1
end

function ScutSchemaField:getType()
    if self.__isTable then
        return self.__field[2]
    end
    return nil
end

function ScutSchemaField:isKey(fieldName)
    if self.__isTable then
        return self.__field[3] or false
    end
    return false
end

function ScutSchemaField:isList(fieldName)
    if self.__isTable then
        return self.__field["_list"] or false
    end
    return false
end

function ScutSchemaField:isDict(fieldName)
    if self.__isTable then
        return (self.__field["_key"] and self.__field["_value"]) or false
    end
    return false
end


--The ScutSchema class 
ScutSchema = {}
function ScutSchema:new(schemaName, schema)
	local instance = {}
	setmetatable(instance, self)
	self.__index = self
	
	instance.__name = schemaName
	if schema then
	    instance.__schema = schema
	else
        instance.__schema = ScutSchemaInfo.g__schema[schemaName]
    end
    if instance.__schema then
        --parse keys of index
        instance.__fields = {}
        instance.__keysIndex = {}
        for k,v in pairs(instance.__schema) do
            local field = ScutSchemaField:new(k, v)
            local index = field:getIndex()
            if index > 0 then
                instance.__fields[index] = field:getName()
            end
            if not field:hasChild() and field:isKey() then
                table.insert(instance.__keysIndex, field:getIndex())
            end
        end
    end
    
    return instance
end

function ScutSchema:getName()
    return  self.__name
end

function ScutSchema:isEmpty()
    return  self.__schema == nil
end

function ScutSchema:hasKey()
    return table.getn(self.__keysIndex) > 0
end

function ScutSchema:hasKeyIndex(index)
    for k,v in pairs(self.__keysIndex) do
        if v == index then
            return true
        end
    end
    return false
end

function ScutSchema:foreach(func)
    for k,name in pairs(self.__fields) do
        local val = self.__schema[name]
        if name ~= _hasChildName  then
            local field = ScutSchemaField:new(name, val)
            if func then 
                func(field)
            end
        end
    end
end

function ScutSchema:getField(fieldName)
    local field = self.__schema[fieldName]
    if field then
        return ScutSchemaField:new(fieldName, field)
    end
    return nil
end

function ScutSchema:getFieldIndex(fieldName)
    local field = self:getField(fieldName)
    if field then
        return field:getIndex()
    else
        return -1
    end
end

function ScutSchema:getChildSchema(fieldName)
    local field = self:getField(fieldName)
    if field then
        return field:getChild()
    else
        return nil
    end
end
