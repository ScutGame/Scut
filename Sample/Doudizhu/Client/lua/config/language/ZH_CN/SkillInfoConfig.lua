----------------------------------------------------------------
-- SkillInfoConfig.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 技能配置,
------------------------------------------------------------------
module("SkillInfoConfig", package.seeall)

SKILLINFOS={}


function  getSkillInfo(id)
	if SKILLINFOS[id]~=nil then
		return SKILLINFOS[id]
	end
	return false
end;