------------------------------------------------------------------
-- RoleInfoConfig.lua
-- Author : ChenJM
-- Version : 1.15
-- Date :
-- Description: 角色配置,
------------------------------------------------------------------

module("RoleInfoConfig", package.seeall)
ROLEINFOS ={}

function  getRoleInfo(id)
 id=tonumber(id)
	if ROLEINFOS[id]~=nil then
		return ROLEINFOS[id]
	end
end;
