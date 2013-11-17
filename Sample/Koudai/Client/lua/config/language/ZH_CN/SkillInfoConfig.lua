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

SKILLINFOS[1001]={AbilityID=1001,AbilityName="野球拳",AttackUnit=1,AttackTaget=2,EffectCount=0,IsMove=0,EffectDesc="",AbilityDesc="对敌方单体造成伤害",EffectID1="skill_1001",EffectID2="",HeadID="icon_7000",MaxHeadID="icon_7000_1",FntHeadID="list_2000"}

SKILLINFOS[1002]={AbilityID=1002,AbilityName="爆裂火球",AttackUnit=1,AttackTaget=2,EffectCount=0,IsMove=0,EffectDesc="",AbilityDesc="对敌方单体造成伤害",EffectID1="skill_1002",EffectID2="",HeadID="icon_7001",MaxHeadID="icon_7001_1",FntHeadID="list_2001"}

SKILLINFOS[1003]={AbilityID=1003,AbilityName="流云刺",AttackUnit=1,AttackTaget=2,EffectCount=0,IsMove=0,EffectDesc="",AbilityDesc="对敌方单体造成伤害",EffectID1="skill_1003",EffectID2="",HeadID="icon_7002",MaxHeadID="icon_7002_1",FntHeadID="list_2002"}

SKILLINFOS[1004]={AbilityID=1004,AbilityName="破甲弹",AttackUnit=1,AttackTaget=2,EffectCount=0,IsMove=0,EffectDesc="",AbilityDesc="对敌方单体造成伤害",EffectID1="skill_1004",EffectID2="",HeadID="icon_7003",MaxHeadID="icon_7003_1",FntHeadID="list_2003"}

SKILLINFOS[1005]={AbilityID=1005,AbilityName="蓄劲",AttackUnit=1,AttackTaget=1,EffectCount=2,IsMove=0,EffectDesc="提高物理攻击",AbilityDesc="提高自己物理攻击，持续1回合",EffectID1="skill_1005",EffectID2="",HeadID="icon_7004",MaxHeadID="icon_7004_1",FntHeadID="list_2004"}

SKILLINFOS[1006]={AbilityID=1006,AbilityName="强体",AttackUnit=1,AttackTaget=1,EffectCount=0,IsMove=0,EffectDesc="提高自身生命值上限",AbilityDesc="提高自身生命值上限",EffectID1="skill_1006",EffectID2="",HeadID="icon_7005",MaxHeadID="icon_7005_1",FntHeadID="list_2005"}

SKILLINFOS[1007]={AbilityID=1007,AbilityName="力拔",AttackUnit=1,AttackTaget=1,EffectCount=0,IsMove=0,EffectDesc="提高自身力量",AbilityDesc="提高自身力量",EffectID1="skill_1007",EffectID2="",HeadID="icon_7006",MaxHeadID="icon_7006_1",FntHeadID="list_2006"}

SKILLINFOS[1008]={AbilityID=1008,AbilityName="冥想",AttackUnit=1,AttackTaget=1,EffectCount=0,IsMove=0,EffectDesc="提高自身智力",AbilityDesc="提高自身智力",EffectID1="skill_1008",EffectID2="",HeadID="icon_7007",MaxHeadID="icon_7007_1",FntHeadID="list_2007"}

SKILLINFOS[1009]={AbilityID=1009,AbilityName="入梦",AttackUnit=1,AttackTaget=1,EffectCount=0,IsMove=0,EffectDesc="提高自身魂力",AbilityDesc="提高自身魂力",EffectID1="skill_1009",EffectID2="",HeadID="icon_7008",MaxHeadID="icon_7008_1",FntHeadID="list_2008"}

SKILLINFOS[1010]={AbilityID=1010,AbilityName="光之庇护",AttackUnit=4,AttackTaget=3,EffectCount=0,IsMove=0,EffectDesc="全体友方回复10点气势",AbilityDesc="全体友方回复10点气势",EffectID1="skill_1010",EffectID2="",HeadID="icon_7009",MaxHeadID="icon_7009_1",FntHeadID="list_2009"}

SKILLINFOS[1011]={AbilityID=1011,AbilityName="恩惠之雨",AttackUnit=4,AttackTaget=3,EffectCount=0,IsMove=0,EffectDesc="回复友方全体目标生命",AbilityDesc="回复友方全体目标生命",EffectID1="skill_1011",EffectID2="",HeadID="icon_7010",MaxHeadID="icon_7010_1",FntHeadID="list_2010"}

SKILLINFOS[1012]={AbilityID=1012,AbilityName="罪恶诅咒",AttackUnit=1,AttackTaget=2,EffectCount=0,IsMove=0,EffectDesc="降低目标10点气势",AbilityDesc="对敌方单体造成伤害，同时降低目标10点气势",EffectID1="skill_1012",EffectID2="",HeadID="icon_7011",MaxHeadID="icon_7011_1",FntHeadID="list_2011"}

SKILLINFOS[1013]={AbilityID=1013,AbilityName="惩戒之刃",AttackUnit=2,AttackTaget=2,EffectCount=0,IsMove=0,EffectDesc="",AbilityDesc="对敌方横向造成伤害",EffectID1="skill_1013",EffectID2="",HeadID="icon_7012",MaxHeadID="icon_7012_1",FntHeadID="list_2012"}

SKILLINFOS[1014]={AbilityID=1014,AbilityName="冷冻冰柱",AttackUnit=3,AttackTaget=2,EffectCount=0,IsMove=0,EffectDesc="",AbilityDesc="对敌方纵向造成伤害",EffectID1="skill_1014",EffectID2="",HeadID="icon_7013",MaxHeadID="icon_7013_1",FntHeadID="list_2013"}

SKILLINFOS[1015]={AbilityID=1015,AbilityName="战争咆哮",AttackUnit=4,AttackTaget=2,EffectCount=0,IsMove=0,EffectDesc="",AbilityDesc="对敌方全体造成伤害",EffectID1="skill_1015",EffectID2="",HeadID="icon_7014",MaxHeadID="icon_7014_1",FntHeadID="list_2014"}

SKILLINFOS[1016]={AbilityID=1016,AbilityName="眩晕打击",AttackUnit=1,AttackTaget=2,EffectCount=1,IsMove=0,EffectDesc="增加眩晕",AbilityDesc="对敌方单体造成伤害，有概率眩晕目标，持续1回合",EffectID1="skill_1016",EffectID2="",HeadID="icon_7015",MaxHeadID="icon_7015_1",FntHeadID="list_2015"}

SKILLINFOS[1017]={AbilityID=1017,AbilityName="眠袭",AttackUnit=1,AttackTaget=2,EffectCount=1,IsMove=0,EffectDesc="增加昏睡",AbilityDesc="对敌方单体造成伤害，有概率昏睡目标，持续1回合",EffectID1="skill_1017",EffectID2="",HeadID="icon_7016",MaxHeadID="icon_7016_1",FntHeadID="list_2016"}

SKILLINFOS[1018]={AbilityID=1018,AbilityName="铁壁",AttackUnit=1,AttackTaget=2,EffectCount=3,IsMove=0,EffectDesc="提高所有防御",AbilityDesc="对敌方单体造成伤害，同时提高自己的所有防御，持续3回合",EffectID1="skill_1018",EffectID2="",HeadID="icon_7017",MaxHeadID="icon_7017_1",FntHeadID="list_2017"}

SKILLINFOS[1019]={AbilityID=1019,AbilityName="圣光复苏",AttackUnit=1,AttackTaget=1,EffectCount=0,IsMove=0,EffectDesc="回复大量生命",AbilityDesc="回复自己大量生命",EffectID1="skill_1019",EffectID2="",HeadID="icon_7018",MaxHeadID="icon_7018_1",FntHeadID="list_2018"}

SKILLINFOS[1020]={AbilityID=1020,AbilityName="雷霆震击",AttackUnit=1,AttackTaget=2,EffectCount=1,IsMove=0,EffectDesc="增加眩晕",AbilityDesc="对敌方单体造成伤害，有概率眩晕目标，持续1回合",EffectID1="skill_1020",EffectID2="",HeadID="icon_7019",MaxHeadID="icon_7019_1",FntHeadID="list_2019"}

SKILLINFOS[1021]={AbilityID=1021,AbilityName="月神之箭",AttackUnit=2,AttackTaget=2,EffectCount=0,IsMove=0,EffectDesc="",AbilityDesc="对敌方横向造成伤害",EffectID1="skill_1021",EffectID2="",HeadID="icon_7020",MaxHeadID="icon_7020_1",FntHeadID="list_2020"}

SKILLINFOS[1022]={AbilityID=1022,AbilityName="野性呼唤",AttackUnit=1,AttackTaget=2,EffectCount=0,IsMove=0,EffectDesc="回复50点气势",AbilityDesc="对敌方单体造成伤害，同时回复自己50点气势",EffectID1="skill_1022",EffectID2="",HeadID="icon_7021",MaxHeadID="icon_7021_1",FntHeadID="list_2021"}

SKILLINFOS[1023]={AbilityID=1023,AbilityName="破碎践踏",AttackUnit=1,AttackTaget=2,EffectCount=2,IsMove=0,EffectDesc="降低目标物理防御力",AbilityDesc="对敌方单体造成伤害，同时降低目标物理防御力，持续2回合",EffectID1="skill_1023",EffectID2="",HeadID="icon_7022",MaxHeadID="icon_7022_1",FntHeadID="list_2022"}

SKILLINFOS[1024]={AbilityID=1024,AbilityName="风暴之锤",AttackUnit=1,AttackTaget=2,EffectCount=1,IsMove=0,EffectDesc="增加眩晕",AbilityDesc="对敌方单体造成伤害，有概率眩晕目标，持续1回合",EffectID1="skill_1024",EffectID2="",HeadID="icon_7023",MaxHeadID="icon_7023_1",FntHeadID="list_2023"}

SKILLINFOS[1025]={AbilityID=1025,AbilityName="幽光之魂",AttackUnit=1,AttackTaget=2,EffectCount=1,IsMove=0,EffectDesc="对目标造成迷失状态，持续1回合",AbilityDesc="对敌方单体造成伤害，有概率迷失目标，持续1回合",EffectID1="skill_1025",EffectID2="",HeadID="icon_7024",MaxHeadID="icon_7024_1",FntHeadID="list_2024"}

SKILLINFOS[1026]={AbilityID=1026,AbilityName="弹幕冲击",AttackUnit=3,AttackTaget=2,EffectCount=0,IsMove=0,EffectDesc="",AbilityDesc="对敌方纵向造成伤害",EffectID1="skill_1026",EffectID2="",HeadID="icon_7025",MaxHeadID="icon_7025_1",FntHeadID="list_2025"}

SKILLINFOS[1027]={AbilityID=1027,AbilityName="火焰气息",AttackUnit=1,AttackTaget=2,EffectCount=1,IsMove=0,EffectDesc="增加暴击",AbilityDesc="对敌方单体造成伤害，同时提高自己的暴击概率，持续1回合",EffectID1="skill_1027",EffectID2="",HeadID="icon_7026",MaxHeadID="icon_7026_1",FntHeadID="list_2026"}

SKILLINFOS[1028]={AbilityID=1028,AbilityName="死亡旋风",AttackUnit=1,AttackTaget=2,EffectCount=0,IsMove=0,EffectDesc="",AbilityDesc="对敌方单体造成伤害",EffectID1="skill_1028",EffectID2="",HeadID="icon_7027",MaxHeadID="icon_7027_1",FntHeadID="list_2027"}

SKILLINFOS[1029]={AbilityID=1029,AbilityName="狂暴",AttackUnit=1,AttackTaget=3,EffectCount=2,IsMove=0,EffectDesc="增加暴击",AbilityDesc="对敌方单体造成伤害，同时提高自己的暴击概率，持续2回合",EffectID1="skill_1029",EffectID2="",HeadID="icon_7028",MaxHeadID="icon_7028_1",FntHeadID="list_2028"}

SKILLINFOS[1030]={AbilityID=1030,AbilityName="黑暗之门",AttackUnit=1,AttackTaget=2,EffectCount=1,IsMove=0,EffectDesc="对目标造成混乱状态，持续1回合",AbilityDesc="对敌方单体造成伤害，有概率混乱目标，持续1回合",EffectID1="skill_1030",EffectID2="",HeadID="icon_7029",MaxHeadID="icon_7029_1",FntHeadID="list_2029"}

SKILLINFOS[1031]={AbilityID=1031,AbilityName="腐蚀瘟疫",AttackUnit=1,AttackTaget=2,EffectCount=2,IsMove=0,EffectDesc="对目标造成中毒状态，持续2回合",AbilityDesc="对敌方单体造成伤害，同时造成中毒状态，持续2回合",EffectID1="skill_1031",EffectID2="",HeadID="icon_7030",MaxHeadID="icon_7030_1",FntHeadID="list_2030"}

SKILLINFOS[1032]={AbilityID=1032,AbilityName="暗影冲击",AttackUnit=1,AttackTaget=2,EffectCount=0,IsMove=0,EffectDesc="",AbilityDesc="对敌方单体造成伤害",EffectID1="skill_1032",EffectID2="",HeadID="icon_7031",MaxHeadID="icon_7031_1",FntHeadID="list_2031"}

SKILLINFOS[1033]={AbilityID=1033,AbilityName="散弹",AttackUnit=4,AttackTaget=2,EffectCount=0,IsMove=0,EffectDesc="",AbilityDesc="对敌方全体造成伤害",EffectID1="skill_1033",EffectID2="",HeadID="icon_7032",MaxHeadID="icon_7032_1",FntHeadID="list_2032"}

SKILLINFOS[1034]={AbilityID=1034,AbilityName="幻影剑舞",AttackUnit=3,AttackTaget=2,EffectCount=0,IsMove=0,EffectDesc="",AbilityDesc="对敌方纵向造成伤害",EffectID1="skill_1034",EffectID2="",HeadID="icon_7033",MaxHeadID="icon_7033_1",FntHeadID="list_2033"}

SKILLINFOS[1035]={AbilityID=1035,AbilityName="闪烁突袭",AttackUnit=1,AttackTaget=2,EffectCount=3,IsMove=0,EffectDesc="增加闪避",AbilityDesc="对敌方单体造成伤害，同时提高自己的闪避概率，持续3回合",EffectID1="skill_1035",EffectID2="",HeadID="icon_7034",MaxHeadID="icon_7034_1",FntHeadID="list_2034"}

SKILLINFOS[1036]={AbilityID=1036,AbilityName="热血战魂",AttackUnit=4,AttackTaget=3,EffectCount=2,IsMove=0,EffectDesc="提高全体友方物理攻击",AbilityDesc="提高全体友方物理攻击，持续2回合",EffectID1="skill_1036",EffectID2="",HeadID="icon_7035",MaxHeadID="icon_7035_1",FntHeadID="list_2035"}

SKILLINFOS[1037]={AbilityID=1037,AbilityName="灵魂灼烧",AttackUnit=1,AttackTaget=2,EffectCount=2,IsMove=0,EffectDesc="降低目标魂技防御力",AbilityDesc="对敌方单体造成伤害，同时降低目标魂技防御力，持续2回合",EffectID1="skill_1037",EffectID2="",HeadID="icon_7036",MaxHeadID="icon_7036_1",FntHeadID="list_2036"}

SKILLINFOS[1038]={AbilityID=1038,AbilityName="魂之挽歌",AttackUnit=3,AttackTaget=2,EffectCount=0,IsMove=0,EffectDesc="",AbilityDesc="对敌方纵向造成伤害",EffectID1="skill_1038",EffectID2="",HeadID="icon_7037",MaxHeadID="icon_7037_1",FntHeadID="list_2037"}

SKILLINFOS[1039]={AbilityID=1039,AbilityName="剧毒新星",AttackUnit=1,AttackTaget=2,EffectCount=2,IsMove=0,EffectDesc="对目标造成中毒状态，持续2回合",AbilityDesc="对敌方单体造成伤害，同时造成中毒状态，持续2回合",EffectID1="skill_1039",EffectID2="",HeadID="icon_7038",MaxHeadID="icon_7038_1",FntHeadID="list_2038"}

SKILLINFOS[1040]={AbilityID=1040,AbilityName="屠戮",AttackUnit=1,AttackTaget=2,EffectCount=2,IsMove=0,EffectDesc="对目标造成出血状态，持续2回合",AbilityDesc="对敌方单体造成伤害，同时造成出血状态，持续2回合",EffectID1="skill_1040",EffectID2="",HeadID="icon_7039",MaxHeadID="icon_7039_1",FntHeadID="list_2039"}

SKILLINFOS[1041]={AbilityID=1041,AbilityName="自然之怒",AttackUnit=2,AttackTaget=2,EffectCount=0,IsMove=0,EffectDesc="",AbilityDesc="对敌方横向造成伤害",EffectID1="skill_1041",EffectID2="",HeadID="icon_7040",MaxHeadID="icon_7040_1",FntHeadID="list_2040"}

SKILLINFOS[1042]={AbilityID=1042,AbilityName="神圣救赎",AttackUnit=4,AttackTaget=3,EffectCount=1,IsMove=0,EffectDesc="增加全体友方物理、魂技、魔法攻击力并回复生命值，持续一回合",AbilityDesc="增加全体友方物理、魂技、魔法攻击力并回复生命值，持续一回合",EffectID1="skill_1042",EffectID2="",HeadID="icon_7041",MaxHeadID="icon_7041_1",FntHeadID="list_2041"}

SKILLINFOS[1043]={AbilityID=1043,AbilityName="神灭斩",AttackUnit=1,AttackTaget=2,EffectCount=0,IsMove=0,EffectDesc="",AbilityDesc="对敌方单体造成伤害",EffectID1="skill_1043",EffectID2="",HeadID="icon_7042",MaxHeadID="icon_7042_1",FntHeadID="list_2042"}

SKILLINFOS[1044]={AbilityID=1044,AbilityName="冰冻之箭",AttackUnit=1,AttackTaget=2,EffectCount=1,IsMove=0,EffectDesc="对目标造成冰冻状态，持续1回合",AbilityDesc="对敌方单体造成伤害，有概率冰冻目标，持续1回合",EffectID1="skill_1044",EffectID2="",HeadID="icon_7043",MaxHeadID="icon_7043_1",FntHeadID="list_2043"}

SKILLINFOS[1045]={AbilityID=1045,AbilityName="虚弱诅咒",AttackUnit=1,AttackTaget=2,EffectCount=1,IsMove=0,EffectDesc="降低对方所有气势",AbilityDesc="对敌方单体造成伤害，同时降低对方所有气势，持续1回合",EffectID1="skill_1045",EffectID2="",HeadID="icon_7044",MaxHeadID="icon_7044_1",FntHeadID="list_2044"}

SKILLINFOS[1046]={AbilityID=1046,AbilityName="噩梦侵蚀",AttackUnit=1,AttackTaget=2,EffectCount=1,IsMove=0,EffectDesc="增加混乱",AbilityDesc="对敌方单体造成伤害，有概率混乱目标，持续1回合",EffectID1="skill_1046",EffectID2="",HeadID="icon_7045",MaxHeadID="icon_7045_1",FntHeadID="list_2045"}

SKILLINFOS[1047]={AbilityID=1047,AbilityName="嗜血撕裂",AttackUnit=1,AttackTaget=2,EffectCount=2,IsMove=0,EffectDesc="对目标造成出血状态，持续2回合",AbilityDesc="对敌方单体造成伤害，同时造成出血状态，持续2回合",EffectID1="skill_1047",EffectID2="",HeadID="icon_7046",MaxHeadID="icon_7046_1",FntHeadID="list_2046"}

SKILLINFOS[1048]={AbilityID=1048,AbilityName="英勇打击",AttackUnit=1,AttackTaget=2,EffectCount=0,IsMove=0,EffectDesc="",AbilityDesc="对敌方单体造成伤害",EffectID1="skill_1048",EffectID2="",HeadID="icon_7047",MaxHeadID="icon_7047_1",FntHeadID="list_2047"}

SKILLINFOS[1049]={AbilityID=1049,AbilityName="新月之痕",AttackUnit=1,AttackTaget=2,EffectCount=1,IsMove=0,EffectDesc="增加眩晕",AbilityDesc="对敌方单体造成伤害，有概率眩晕目标，持续1回合",EffectID1="skill_1049",EffectID2="",HeadID="icon_7048",MaxHeadID="icon_7048_1",FntHeadID="list_2048"}

SKILLINFOS[1050]={AbilityID=1050,AbilityName="幻像法球",AttackUnit=2,AttackTaget=2,EffectCount=1,IsMove=0,EffectDesc="增加眩晕",AbilityDesc="对敌方横向造成伤害，有概率眩晕目标，持续1回合",EffectID1="skill_1050",EffectID2="",HeadID="icon_7049",MaxHeadID="icon_7049_1",FntHeadID="list_2049"}

SKILLINFOS[1051]={AbilityID=1051,AbilityName="疾风步",AttackUnit=1,AttackTaget=2,EffectCount=2,IsMove=0,EffectDesc="增加闪避",AbilityDesc="对敌方单体造成伤害，同时提高自己的闪避概率，持续2回合",EffectID1="skill_1051",EffectID2="",HeadID="icon_7050",MaxHeadID="icon_7050_1",FntHeadID="list_2050"}

SKILLINFOS[1052]={AbilityID=1052,AbilityName="星落",AttackUnit=1,AttackTaget=2,EffectCount=2,IsMove=0,EffectDesc="增加暴击",AbilityDesc="对敌方单体造成伤害，同时提高自己的暴击概率，持续2回合",EffectID1="skill_1052",EffectID2="",HeadID="icon_7051",MaxHeadID="icon_7051_1",FntHeadID="list_2051"}

SKILLINFOS[1053]={AbilityID=1053,AbilityName="灵魂之矛",AttackUnit=2,AttackTaget=2,EffectCount=0,IsMove=0,EffectDesc="",AbilityDesc="对敌方横向造成伤害",EffectID1="skill_1053",EffectID2="",HeadID="icon_7052",MaxHeadID="icon_7052_1",FntHeadID="list_2052"}

SKILLINFOS[1054]={AbilityID=1054,AbilityName="月刃",AttackUnit=1,AttackTaget=2,EffectCount=1,IsMove=0,EffectDesc="增加眩晕",AbilityDesc="对敌方单体造成伤害，有概率眩晕目标，持续1回合",EffectID1="skill_1054",EffectID2="",HeadID="icon_7053",MaxHeadID="icon_7053_1",FntHeadID="list_2053"}

SKILLINFOS[1055]={AbilityID=1055,AbilityName="幽冥一击",AttackUnit=1,AttackTaget=2,EffectCount=1,IsMove=0,EffectDesc="增加眩晕",AbilityDesc="对敌方单体造成伤害，有概率眩晕目标，持续1回合",EffectID1="skill_1055",EffectID2="",HeadID="icon_7054",MaxHeadID="icon_7054_1",FntHeadID="list_2054"}

SKILLINFOS[1056]={AbilityID=1056,AbilityName="伤残恐惧",AttackUnit=1,AttackTaget=2,EffectCount=1,IsMove=0,EffectDesc="对目标造成混乱状态，持续1回合",AbilityDesc="对敌方单体造成伤害，有概率混乱目标，持续1回合",EffectID1="skill_1056",EffectID2="",HeadID="icon_7055",MaxHeadID="icon_7055_1",FntHeadID="list_2055"}

SKILLINFOS[1057]={AbilityID=1057,AbilityName="无光之盾",AttackUnit=1,AttackTaget=2,EffectCount=2,IsMove=0,EffectDesc="免疫所有攻击",AbilityDesc="免疫所有攻击，持续2回合",EffectID1="skill_1057",EffectID2="",HeadID="icon_7056",MaxHeadID="icon_7056_1",FntHeadID="list_2056"}

SKILLINFOS[1058]={AbilityID=1058,AbilityName="先祖之魂",AttackUnit=1,AttackTaget=1,EffectCount=2,IsMove=0,EffectDesc="增加暴击",AbilityDesc="提高自己暴击概率，持续2回合",EffectID1="skill_1058",EffectID2="",HeadID="icon_7057",MaxHeadID="icon_7057_1",FntHeadID="list_2057"}

SKILLINFOS[1059]={AbilityID=1059,AbilityName="刚毅不屈",AttackUnit=1,AttackTaget=1,EffectCount=2,IsMove=0,EffectDesc="增加格挡",AbilityDesc="提高自己格挡概率，持续2回合",EffectID1="skill_1059",EffectID2="",HeadID="icon_7058",MaxHeadID="icon_7058_1",FntHeadID="list_2058"}

SKILLINFOS[1060]={AbilityID=1060,AbilityName="死亡盛宴",AttackUnit=1,AttackTaget=2,EffectCount=1,IsMove=0,EffectDesc="对目标造成迷失状态，持续1回合",AbilityDesc="对敌方单体造成伤害，有概率迷失目标，持续1回合",EffectID1="skill_1060",EffectID2="",HeadID="icon_7059",MaxHeadID="icon_7059_1",FntHeadID="list_2059"}

SKILLINFOS[1061]={AbilityID=1061,AbilityName="狩猎",AttackUnit=1,AttackTaget=2,EffectCount=1,IsMove=0,EffectDesc="增加眩晕",AbilityDesc="对敌方单体造成伤害，有概率眩晕目标，持续1回合",EffectID1="skill_1061",EffectID2="",HeadID="icon_7060",MaxHeadID="icon_7060_1",FntHeadID="list_2060"}

SKILLINFOS[1062]={AbilityID=1062,AbilityName="霜之哀伤",AttackUnit=1,AttackTaget=2,EffectCount=1,IsMove=0,EffectDesc="对目标造成冰冻状态，持续1回合",AbilityDesc="对敌方单体造成伤害，有概率冰冻目标，持续1回合",EffectID1="skill_1062",EffectID2="",HeadID="icon_7061",MaxHeadID="icon_7061_1",FntHeadID="list_2061"}

SKILLINFOS[1063]={AbilityID=1063,AbilityName="混乱之箭",AttackUnit=1,AttackTaget=2,EffectCount=1,IsMove=0,EffectDesc="对目标造成混乱状态，持续1回合",AbilityDesc="对敌方单体造成伤害，有概率混乱目标，持续1回合",EffectID1="skill_1063",EffectID2="",HeadID="icon_7062",MaxHeadID="icon_7062_1",FntHeadID="list_2062"}

SKILLINFOS[1064]={AbilityID=1064,AbilityName="冥火暴击",AttackUnit=1,AttackTaget=2,EffectCount=1,IsMove=0,EffectDesc="增加眩晕",AbilityDesc="对敌方单体造成伤害，有概率眩晕目标，持续1回合",EffectID1="skill_1064",EffectID2="",HeadID="icon_7063",MaxHeadID="icon_7063_1",FntHeadID="list_2063"}

SKILLINFOS[1065]={AbilityID=1065,AbilityName="撕裂大地",AttackUnit=4,AttackTaget=2,EffectCount=1,IsMove=0,EffectDesc="增加眩晕",AbilityDesc="对敌方全体造成伤害，有概率眩晕目标，持续1回合",EffectID1="skill_1065",EffectID2="",HeadID="icon_7064",MaxHeadID="icon_7064_1",FntHeadID="list_2064"}



