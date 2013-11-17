------------------------------------------------------------------
-- Global.lua
-- Author     : Xin Zhang
-- Version    : 1.0.0.0
-- Date       : 2011-10-12
-- Description: 
------------------------------------------------------------------

local strModuleName = "Global";
CCLuaLog("Module " .. strModuleName .. " loaded.");
strModuleName = nil;

g_nWinSize = CCDirector:sharedDirector():getWinSize();
g_nWinWidth = g_nWinSize.width;
g_nWinHeight = g_nWinSize.height;


