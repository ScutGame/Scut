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


-- 资源编号    小头像    小半身像    大半身像	
--------------------------------------------------------主角
ROLEINFOS[1000]={TopHead="Icon_1000",HeadImg="Icon_2000",RoleImg="Icon_2000_1"}
ROLEINFOS[1001]={TopHead="Icon_1001",HeadImg="Icon_2001",RoleImg="Icon_2001_1"}
ROLEINFOS[1002]={TopHead="Icon_1002",HeadImg="Icon_2002",RoleImg="Icon_2002_1"}
ROLEINFOS[1003]={TopHead="Icon_1003",HeadImg="Icon_2003",RoleImg="Icon_2003_1"}
ROLEINFOS[1004]={TopHead="Icon_1004",HeadImg="Icon_2004",RoleImg="Icon_2004_1"}
ROLEINFOS[1005]={TopHead="Icon_1005",HeadImg="Icon_2005",RoleImg="Icon_2005_1"}
ROLEINFOS[1006]={TopHead="Icon_1006",HeadImg="Icon_2006",RoleImg="Icon_2006_1"}
ROLEINFOS[1007]={TopHead="Icon_1007",HeadImg="Icon_2007",RoleImg="Icon_2007_1"}





