--
-- TypeData.lua.lua
-- 91War
--
-- Created by LinKequn on 8/30/2011.
-- Copyright 2008 ND, Inc. All rights reserved.
--

module("TypeData", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写


--
---------------公有接口(以下两个函数固定不用改变)--------
--

-- 函数名以小写字母开始，每个函数都应注释

-- 场景入口
function pushScene()
	initResource()
	createScene()
	CCDirector:sharedDirector():pushScene(_scene)
end
-- 退出场景
function popScene()
	releaseResource()
	CCDirector:sharedDirector():popScene()
end

--
-------------------------私有接口------------------------'
---附加属性类型
function AttrType(type)
    if type then
 local table={
		[1]={name=Language.NATT},--="内攻"
		[2]={name=Language.NDEF},--="内防"
		[3]={name=Language.WATT},--="外攻"
		[4]={name=Language.WDEF},--="外攻"
		[5]={name=Language.HATT},--="会攻"
		[6]={name=Language.HDEF},--="会防"
		[7]={name=Language.HIT},--="命中"
		[8]={name=Language.EVA},--="闪避"
		[9]={name=Language.IATT},--="冰攻"
		[10]={name=Language.IDEF},--="冰防"
		[11]={name=Language.FATT},--="火攻"
		[12]={name=Language.FDEF},--="火防"
		[13]={name=Language.TATT},--="雷攻"
		[14]={name=Language.TDEF},--="雷防"
		[15]={name=Language.PATT},--="毒攻"
		[16]={name=Language.PDEF},--="毒防"
		[17]={name=Language.STR},--="力量"
		[18]={name=Language.SPR},--="灵力"
		[19]={name=Language.VIT},--="体力"
		[20]={name=Language.CRE},--="定力"
		[21]={name=Language.AGI},--="身法"
		[22]={name=Language.VITALITY},--="活力"
		[23]={name=Language.ENERGY},--="精力"
		[24]={name=Language.HP},--="血"
		[25]={name=Language.AIR},--="血"
		[26]={name=Language.FRUY},--="怒"
		[27]={name=Language.HPCEL},--="血上限"
		[28]={name=Language.AIRCEL},--="气上限"
		[29]={name=Language.REVIVE},--="复活"
		[30]={name=Language.EXP},--="经验"
		[31]={name=Language.FJTYPE_JSBKX},--="减少冰抗性"
        [32]={name=Language.FJTYPE_JDBKXXX},--="降低冰抗性下限"
        [33]={name=Language.FJTYPE_JSHKX},--="减少火抗性"
        [34]={name=Language.FJTYPE_JDHKXXX},--="降低火抗性下限"
        [35]={name=Language.FJTYPE_JSLKX},--="减少雷抗性"
        [36]={name=Language.FJTYPE_JDLXXXX},--="降低雷抗性下限"
        [37]={name=Language.FJTYPE_JSDKX},--="减少毒抗性"
        [38]={name=Language.FJTYPE_JDDKXXX},--="降低毒抗性下限"
        [39]={name=Language.SHUANGBEIJINQIAN},--="双倍金钱"
        [40]={name=Language.SHUANGBEIJINGYAN},--="双倍道具"

	}
		return table[type].name;
	end
end
--宝石类型
function GemType(type)
if type then
    local table={};
    table={
    [1]={name=Language.BAOSHI_XIWANGBAOSHI},--希望宝石
    [2]={name=Language.BAOSHI_ZHIHUIBAOHSI},--智慧宝石
    [3]={name=Language.BAOSHI_XUANWEIBAOSHI},--玄微宝石
    [4]={name=Language.BAOSHI_HUANMINGBAOSHI},--幻冥宝石
    [5]={name=Language.BAOSHI_XIWANGBAOSHI},--希望宝石
    [6]={name=Language.BAOSHI_MEILIBAOSHI},--魅力宝石
    [7]={name=Language.BAOSHI_MINGYUNBAOSHI},--命运宝石
    [8]={name=Language.BAOSHI_TIANJIBAOSHI},--天机宝石
    [9]={name=Language.BAOSHI_SHENGLIBAOSHI},--胜利宝石
           }
     return table[type].name
end
end
--属相类型
function ElementType(type)
    if type then
        local table={};
        table={
        [1]={type=Language.WUHUN_FENG,kz=Language.WUHUN_DI,bkz=Language.WUHUN_HUO},--风--克制地    被火克制
        [2]={type=Language.WUHUN_DI,kz=Language.WUHUN_HUO,bkz=Language.WUHUN_FENG},--地--克制水    被风克制
        [3]={type=Language.WUHUN_SHUI,kz=Language.WUHUN_HUO,bkz=Language.WUHUN_DI},--水--克制火    被地克制
        [4]={type=Language.WUHUN_HUO,kz=Language.WUHUN_FENG,bkz=Language.WUHUN_SHUI},--火--克制风    被水克制
                }
    return table[type];
    end
end
--门派类型
function ProfessionType(type)
    if type then
        local table={};
        table={[1]={type=Language.LOGIN_DAWU},--大巫
                [2]={type=Language.LOGIN_SANQING},--三清
                [3]={type=Language.LOGIN_SHENNONG},--神农
                [4]={type=Language.LOGIN_XUANYUAN},--轩辕
                [5]={type=Language.LOGIN_GUIMEI},--鬼魅
                }
     return table[type];
    end
end
--去门派
function gotoMenPai(type)
	if type then
		local table={[1]={name=Language.Task_GotoMengPai_[type],Id=2001},--去大巫门派
					[2]={name=Language.Task_GotoMengPai_[type],Id=2002},--去三清门派
					[3]={name=Language.Task_GotoMengPai_[type],Id=2003},--去神农门派
					[4]={name=Language.Task_GotoMengPai_[type],Id=2004},--去轩辕门派
					[5]={name=Language.Task_GotoMengPai_[type],Id=2005},--去鬼魅门派
					}
		return table[type]
	end
end
function TrendType(type)
    if type then
        local table={[0]={type=Language.SPORT_BUBIAN,color=ccWHITE},--不变--白色
                    [1]={type=Language.SPORT_SHANGSHENG,color=ccGREEN},--上升--绿色
                    [2]={type=Language.SPORT_XIAJIANG,color=ccRED},--下降--红色
                    }
        return table[type]
    end
end
--活跃度枚举
function DailyTaskType(type)
	if type then
		local table={[1]={type=Language.HUOYUDU_[type]},--登录
					[2]={type=Language.HUOYUDU_[type]},--活动
					[3]={type=Language.HUOYUDU_[type]},--副本
					[4]={type=Language.HUOYUDU_[type]},--任务
					[5]={type=Language.HUOYUDU_[type]},--竞技
					}
		return table[type]
	end
end