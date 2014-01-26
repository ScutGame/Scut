------------------------------------------------------------------
-- ScutDataSync.lua
-- Author     :
-- Version    : 1.0
-- Date       :
-- Description: The data sync manager
------------------------------------------------------------------
require("datapool.PushReceiverLayer")
require("SyncSDK.ScutSchemaInfo")
require("SyncSDK.ScutSchema")
require("SyncSDK.ScutEntity")

module("ScutDataSync", package.seeall)
g__syncCallbackFunc = nil
local g__syncActionId = 200

function registerSceneCallback(func)
    g__syncCallbackFunc = func
end

function PushErrorCallback(pScutScene, lpExternalData)
	PushReceiverLayer.PushErrorCallback(pScutScene, lpExternalData)
end

function NotifyScene(pScutScene, lpExternalData, schemaName)
    if g__syncCallbackFunc then
        g__syncCallbackFunc(pScutScene, lpExternalData, schemaName)
    end
end

function PushReceiverCallback(pScutScene, lpExternalData)
	local actionId = ScutDataLogic.CNetReader:getInstance():getActionID()
	pScutScene= CCDirector:sharedDirector():getRunningScene()
            
    if actionId == g__syncActionId then
        local result = ScutDataLogic.CNetReader:getInstance():getResult()
	    if result then
	        PushParseEntity(pScutScene, lpExternalData)
	    end
    else
        PushReceiverLayer.PushReceiverCallback(pScutScene, lpExternalData)
    end
end

function PushParseEntity(pScutScene, lpExternalData)
    local entityNum = ZyReader:getInt()
    for k=1, entityNum do
        ZyReader:recordBegin()
        
        local schemaName = ZyReader:readString()
        local root = ScutEntityFactory(schemaName)
        
        if root then
            local entity = root:create()
            entity:addBegin()
            PushReadEntity(entity)
            entity:addEnd()
            root:addEntity(entity)
            
            NotifyScene(pScutScene, lpExternalData, schemaName)
        end
        ZyReader:recordEnd()
        
    end
end

function PushReadEntity(entity, schema)
    local schema = entity:getSchema()
    schema:foreach(function(field)
        if field:hasChild() then
            local schemaName = field:getName()
            local childSchema = field:getChild()
            local rowNumber = ZyReader:getInt()
            
            if field:isDict() then
                local keyField = childSchema:getField("_key")
                local valueField = childSchema:getField("_value")
                local child = nil
                if valueField:hasChild() then
                    child = ScutEntity:new(valueField:getChild(), {})
                else
                    child = ScutEntity:new(childSchema, {})
                end
                
                child:addBegin()
                for k =1, rowNumber do
                    local pair = child:create()
                    ZyReader:recordBegin()
                    pair:addBegin()
                    PushReadEntity(pair) 
                    pair:addEnd()
                    ZyReader:recordEnd()
                    child:addDict(pair)
                end
                child:addEnd()
                entity:addEntity(child)
            
            elseif field:isList() then
                local listField = childSchema:getField("_list")
                local child = nil
                if listField:hasChild() then
                    child = ScutEntity:new(listField:getChild(), {})
                else
                    child = ScutEntity:new(childSchema, {})
                end

                child:addBegin()
                for k =1, rowNumber do
                    ZyReader:recordBegin()
                    PushReadEntity(child)
                    ZyReader:recordEnd()
                end
                child:addEnd()
                entity:addEntity(child)
            else
                --single child
                local child = ScutEntity:new(childSchema, {})
                child:addBegin()
                ZyReader:recordBegin()
                PushReadEntity(child)
                ZyReader:recordEnd()
                child:addEnd()
                entity:addEntity(child)
            end
        else
            local fieldval = PushReaderField(field:getType())
            entity:add(fieldval)
        end
    end)
        
end

function PushReaderField(fieldType)
    local val = nil
    if fieldType == "int" then --4 byte size
        val = ZyReader:getInt()
    elseif fieldType == "short" then --2 byte size
        val = ZyReader:getWORD()
    elseif fieldType == "string" then
        val = ZyReader:readString()
    elseif fieldType == "byte" then --1 byte size
        val = ZyReader:getByte()
    elseif fieldType == "long" then --8 byte size
        val = ZyReader:readInt64()
    elseif fieldType == "float" then --4 byte size
        val = ZyReader:getFloat()
    elseif fieldType == "double" then --8 byte size
        val = ZyReader:getDouble()
    elseif fieldType == "bool" then
        val = ZyReader:getByte() == 1
    elseif fieldType == "decimal" then
        val = tonumber(ZyReader:readString())
    elseif fieldType == "datetime" then
        val = ZyReader:readString()
    else
        --not suport type
    end
    return val
end