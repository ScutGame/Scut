
------------------------------------------------------------------
-- MenuBtnConfig.lua
-- Author     : ChenJM
-- Version    : 1.15
-- Date       :   
-- Description:按钮管理 ,
------------------------------------------------------------------
module("MenuBtnConfig", package.seeall)

--[[
酒馆招募佣兵
魔法阵
魔术
强化
培养
魂技学习
修炼
购买精力
好友
开启第二个强化队列
竞技场
庄园种植经验树
公会
日常任务
金矿洞
命运水晶
神秘商店
领土战
世界BOSS挑战
送花
每日探险
庄园种植金钱树
多人副本
圣吉塔挑战
扫荡
角色
背包
任务
世界地图
--]]
MenuId = {
	eJiuGuan	 = 1,--酒馆
	eMoFaZheng	 = 2,--魔法阵
	eMoShu	     = 3,--魔术
	eQiangHua	 = 4,--强化
	ePeiYang	 = 5,--培养
	eXiuLian     = 7,--修炼
	eHaoYou 	 = 9,--好友
	eJingJiChang = 11,--竞技场
	eZhuangYuan  = 12,--庄园种植经验树
	eGongHui     = 13,--公会
	eDailyTast   = 14,--日常任务
	eGoldHole	 = 15,--金矿洞
	eMingYunShuijin	= 16,--命运水晶
	eDailyAdventure	= 17,--神秘商店
	eJueSe 	= 26,--角色
	eSaoDang  = 25,--扫荡
	eBeiBao = 27,--背包
	eRenWu 	= 28,--任务
	eWorldMap     = 29,--世界地图
	eGuoJia  = 30,  --国家选择
	eAllMenu  = 100,  --
	eFengLin = 36,--封灵
	eTianDiJie = 35,--天地劫
	eMall = 37,--商城
	eChuanCheng = 38,--传承
	eKuaFu = 39,--天下第一（跨服战）
	eLaXin = 40,--拉新卡

	eYongBin = 47,--佣兵
	eEquip = 48,--装备
	eHunji= 49,--魂技
	eYongBLevel= 50,--升级
	ePlot = 51,--副本
	eChaDang = 100,--A--菜单
	eXinJian = 102,--A --信件
	eSheZhi = 103,--A	--设置
	eShangZhen = 104,--我的阵营  ---A		
}


local MenuItemTables={
	[MenuId.eMoFaZheng]=0,
	[MenuId.eMoShu]=0,
	[MenuId.eQiangHua]=0,
	[MenuId.ePeiYang]=0,
	[MenuId.eXiuLian]=0,
	[MenuId.eHaoYou]=0,
	[MenuId.eJingJiChang]=0,
	[MenuId.eZhuangYuan]=0,
	[MenuId.eGongHui]=0,
	[MenuId.eDailyTast]=0,
	[MenuId.eGoldHole]=0,
	[MenuId.eMingYunShuijin]=0,
	[MenuId.eDailyAdventure]=0,
	[MenuId.eSaoDang]=0,
	[MenuId.eJueSe]=0,
	[MenuId.eBeiBao]=0,
	[MenuId.eRenWu]=0,
	[MenuId.eWorldMap]=0,
	[MenuId.eGuoJia]=0,
	[MenuId.eJiuGuan]=0,
	[MenuId.eFengLin]=0,
	[MenuId.eTianDiJie]=0,


	}
	
	function clearMenuItemTables ()
		MenuItemTables=nil
		MenuItemTables={}
	end;

function  getMenuItem(id)
	if  MenuItemTables[id]==1 then
		return true
	end
	return false
end;

function  setMenuItem(key)
	MenuItemTables[key]=1
end;
