------------------------------------------------------------------
-- SettingData.lua
-- Author     : ChenJM
-- Version    : 1.15
-- Date       :   
-- Description: 保存音乐配置,
------------------------------------------------------------------

module("SettingData", package.seeall)

-- 用户设置信息
local SettingData = {}

--本地设置信息
local LocalSettingData = {
   {EnumValue = 1,IsInUse  = 1},  -- 操作音效
   {EnumValue = 2,IsInUse  = 1},       -- 背景音乐
 }
 
 local LocalPeopleData = {
   {EnumValue = 1,IsInUse  = 0},  
   {EnumValue = 2,IsInUse  = 15},       
 }
 
 -------------显示人数
 function  saveShowPlayer(state)
	  if state ~= nil then
		return saveConfig("sys/config.ini", "system", "ShowState" , state)
	end
end;

function getShowPlayer()
	local result = getConfig("sys/config.ini", "system", "ShowState")
	if not tonumber(result) or tonumber(result) == 0 then
		return false
	else
		return result
	end
end

 --------
function saveMusicConfig(nConfig)
    if nConfig ~= nil then
		return saveConfig("sys/config.ini", "system", "isPlayMusic" , nConfig)
	end
end

function getMusicConfig()
	local result = getConfig("sys/config.ini", "system", "isPlayMusic")
	if not tonumber(result) or tonumber(result) == 0 then
		return false
	else
		return true
	end
end


-- 保存配置：文件名、标题、key、value
function saveConfig(sFileName, sTitle, key , value)
	if value ~= nil then
		local pIni = ScutDataLogic.CLuaIni:new()
		local bIsRead = pIni:Load(sFileName)
		pIni:Set(sTitle, key, value)
		pIni:Save(sFileName)
		pIni:delete()
		return true
	else
	       return false
	end
end


-- 获取配置：文件名、标题、key
function getConfig(sFileName, sTitle, sKey)
	local pIni = ScutDataLogic.CLuaIni:new()
	local bIsRead = pIni:Load(sFileName)
	if bIsRead == false then
		pIni:Set(sTitle, sKey, 1)
		pIni:Save(sFileName)
		pIni:delete()
		return 1
	end
	return pIni:Get(sTitle, sKey)
end


function initLocalOptions()
	   LocalSettingData[1].IsInUse = tonumber( getConfig("sys/config.ini", "system", "isPlayMusic"))
	   LocalSettingData[2].IsInUse = tonumber(getConfig("sys/config.ini", "system", "isPlayMusic"))
end

 
function getSettingData()
   return SettingData
end

function getLocalData()
   return LocalSettingData
end

-- 保存服务端返回的数据
function setSettingData(Response_1508)
    SettingData = {}
    initLocalOptions()
    for k,v in pairs(LocalSettingData) do
	   SettingData[#SettingData + 1] = v
    end
    local data = Response_1508.Items
	for k,v in pairs(data) do
	   v.EnumValue = v.EnumValue + #LocalSettingData
	   SettingData[#SettingData + 1] = v
    end
end


function  release()
   SettingData = {}
end





