------------------------------------------------------------------
-- PlotConfig.lua
-- Author     : ChenJM
-- Version    : 1.15
-- Date       :
-- Description: 副本配置,8-17更新1.2数据
------------------------------------------------------------------
module("PlotConfig", package.seeall)

PLOTINFO={}


function  getPlotInfo(id)
	if PLOTINFO[id]~=nil then
		return PLOTINFO[id]
	end
	return false
end;
