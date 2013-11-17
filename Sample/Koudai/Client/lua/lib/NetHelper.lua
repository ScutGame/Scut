------------------------------------------------------------------
-- Author     : 
-- Version    : 1.15
-- Date       :   
-- Description: ,
------------------------------------------------------------------

require("lib.ZyLoading")
require ("scenes.LoadingScene")

ScutRequestParam = {param = {}}
function ScutRequestParam:getParamData(nTag)
	return ScutRequestParam.param[nTag]
end
ZyRequestCounter = 1
ZyReader = ScutDataLogic.CNetReader:getInstance()

--默认跟网络交互处理成功的标记
eScutNetSuccess = 0;
eNdNetError = 10000;

function ZyReader.readString()
	local nLen = ZyReader:getInt()
	local strRet = nil
	if nLen ~= 0
	then
        local str = ScutDataLogic.CLuaString:new("")
        ZyReader:getString(str, nLen)
        strRet = string.format("%s", str:getCString())
        str:delete()
	end
	return strRet
end
function ZyReader:readInt64()
	return ScutDataLogic.CInt64:new_local(ZyReader:getCInt64())
end

function ZyReader.readErrorMsg()
	return string.format("%s", ZyReader:getErrMsg():getCString())
end

ZyWriter = ScutDataLogic.CNetWriter:getInstance()


ScutDataLogic.CNetWriter:setUrl("http://dir.scutgame.com/Service.aspx")


function ZyExecRequest(pScutScene, lpData, bShowLoading)

	if  bShowLoading == true then
		--LoadingScene.init(pScutScene)
	end
	--如果为True 显示Loading图标--
	ZyRequestCounter = ZyRequestCounter + 1
	if  bShowLoading==nil then
	    ZyLoading.show(pScutScene, ZyRequestCounter)
	end
	if lpData then
	    table.insert(ScutRequestParam.param, ZyRequestCounter, lpData)
	end
	ScutDataLogic.CDataRequest:Instance():AsyncExecRequest(pScutScene, ZyWriter:generatePostData(), ZyRequestCounter, nil);	
	
	ScutDataLogic.CNetWriter:resetData()
end

function ZyReader.readGeneral()
	local v_item52 = WDGeneral:new()
	ZyReader:recordBegin()
	v_item52._id = ZyReader:readInt64()
	v_item52._headID = ZyReader:readInt64()
	v_item52._name = ZyReader:readString()
	v_item52._nature = ZyReader:getInt()
	v_item52._level = ZyReader:getInt()
	v_item52._experience = ZyReader:readInt64()
	v_item52._health = ZyReader:getInt()
	v_item52._exploit = ZyReader:getInt()
	v_item52._quality = ZyReader:getInt()
	v_item52._occupation = ZyReader:getInt()
	v_item52._grow = ZyReader:getFloat()
	v_item52._dantiao = ZyReader:getInt()
	if ZyReader:getInt() ~= 0 then
		ZyReader:recordBegin()
		v_item52._defence = ZyReader:getInt()
		v_item52._attack = ZyReader:getInt()
		v_item52._command = ZyReader:getInt()
		v_item52._power = ZyReader:getInt()
		v_item52._nimble = ZyReader:getInt()
		ZyReader:recordEnd()
	end
	if ZyReader:getInt() ~= 0 then
		ZyReader:recordBegin()
		v_item52._defenceAdd = ZyReader:getInt()
		v_item52._attackAdd = ZyReader:getInt()
		v_item52._commandAdd = ZyReader:getInt()
		v_item52._powerAdd = ZyReader:getInt()
		v_item52._nimbleAdd = ZyReader:getInt()
		ZyReader:recordEnd()
	end
	v_item52._official = WDOfficial:new(ZyReader:getInt())
	v_item52._state = ZyReader:getInt()
	local m_ArmsMatching = nil
	if ZyReader:getInt() ~= 0 then
		m_ArmsMatching = {}
		v_item52._suitability = m_ArmsMatching
		ZyReader:recordBegin()
		m_ArmsMatching[GENERAL_SUITABILITY_BU] = ZyReader:getInt()
		m_ArmsMatching[GENERAL_SUITABILITY_QI] = ZyReader:getInt()
		m_ArmsMatching[GENERAL_SUITABILITY_XIE] = ZyReader:getInt()
		m_ArmsMatching[GENERAL_SUITABILITY_GONG] = ZyReader:getInt()
		m_ArmsMatching[GENERAL_SUITABILITY_CHE] = ZyReader:getInt()
		ZyReader:recordEnd()
	end
	local m_Equipements = {}
	v_item52._equipments = m_Equipements
	local nNum54 = ZyReader:getInt()
	for idx53 = 1, nNum54 do
		local v_item53 = WDEquipment:new()
		ZyReader:recordBegin()
		v_item53._id = ZyReader:readInt64()
		v_item53._strengthenLever = ZyReader:getInt()
		local m_GemstoneIds = {}
		v_item53._jemstones = m_GemstoneIds
		local nNum55 = ZyReader:getInt()
		for idx54 = 1, nNum55 do
			local v_item54 = {}
			v_item54 = ZyReader:readInt64()
			ZyTable.push_back(m_GemstoneIds,v_item54)
		end
		if ZyReader:getInt() ~= 0 then
			ZyReader:recordBegin()
			v_item53._defence = ZyReader:getInt()
			v_item53._attack = ZyReader:getInt()
			v_item53._command = ZyReader:getInt()
			v_item53._power = ZyReader:getInt()
			v_item53._nimble = ZyReader:getInt()
			ZyReader:recordEnd()
		end
		v_item53._uniqueID = ZyReader:readInt64()
		ZyReader:recordEnd()
		ZyTable.push_back(m_Equipements,v_item53)
	end
	ZyReader:recordEnd()
	return v_item52
end
