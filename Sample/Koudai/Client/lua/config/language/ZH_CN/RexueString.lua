-------------------------------------------------------
-- RexueString.lua
-- Author     : Xin Zhang
-- Version    : 1.0.0.0
-- Date       : 2011-10-17
-- Description: 存放工程所需的中文字符串变量
------------------------------------------------------------------

local strModuleName = "RexueString";
module(strModuleName, package.seeall);
CCLuaLog("Module " .. strModuleName .. " loaded.");
strModuleName = nil;

-- 由于游戏显示的中文须采用UTF-8编码，\n因此将工程所有中文字符串都保存于本文件中，本\n文件采用无BOM的UTF-8编码格式。
-- 注意脑残Decoda不能支持本文件正常显示。其它常见编辑器均支持。
-- 同时任何人不得修改本文件的编码格式。
---
---产品名称
GAME_NAME = "天界"

TIP_PLEASEINPUT="请输入数量："
 -- char size test
 IDS_FONT_12_TEST			= "损兵折将的准准备。兵折将的准备为一名征战四方的君主，怎能没有一个响亮的名号备aaaa好aaafgfgfgadfasdfasd损兵折将的准备"
IDS_BACK="返回"
IDS_OK= "确定";
IDS_COMFIRM="确认"
IDS_CANCEL="取消"
IDS_CLOSED_APP="是否退出游戏？"
GM_TITLE = "GM命令"
GM_MESSAGE = "请输入命令:"
GM_MESSAGE2 = "请求GM命令为空，发送失败"
NIN="您"
TIP_TIP="提示"
TIP_YES="是"
TIP_NO="否"
TIP_NONET="网络连接失败"
TIP_NONET1="网络连接失败,请重新登录游戏"
IDS_RETRY="重试"
IDS_QUIT="退出"
IDS_MESSAGE1 = "您的装备强化已经重新开启，时间不等人，赶紧进入天界行中继续奋战吧！"
IDS_MESSAGE2 = "您的魔术强化已经重新开启，时间不等人，赶紧进入天界行中继续奋战吧！"

--主界面
MAIN_JINGSHINO="晶石"
MAIN_JINBINO="金币"
MAIN_BLOODVOL="血量"
MAIN_PHYSICAL="体力"
MAIN_WAN="万"
MAIN_YI="亿"

GUIDE_MESSAGE ={}
GUIDE_MESSAGE[1] = "点击自动寻路"
GUIDE_MESSAGE[2] = "点击对话"
GUIDE_MESSAGE[3] = "当前任务"
GUIDE_MESSAGE[4] = "点击选择"
GUIDE_MESSAGE[5] = "点击招募"
GUIDE_MESSAGE[6] = "点击关闭"
GUIDE_MESSAGE[7] = "点击拖动图像"
GUIDE_MESSAGE[8] = "拖动到该位置"
GUIDE_MESSAGE[9] = "点击强化"
GUIDE_MESSAGE[10] = "点击保存阵法"
GUIDE_MESSAGE[11] = "点击启用阵法"
GUIDE_MESSAGE[12] = "请先选择要招募的佣兵"
GUIDE_MESSAGE[13] = "请先点击招募佣兵"
GUIDE_MESSAGE[14] = "请先选择要强化的装备"
GUIDE_MESSAGE[15] = "请先点击强化装备"
GUIDE_MESSAGE[16] = "请先选择佣兵拖放到阵法中"
GUIDE_MESSAGE[17] = "请先保存阵法"
GUIDE_MESSAGE[18] = "请先启用阵法"
GUIDE_MESSAGE[19] = "打开背包可以使用绷带进行补血"
GUIDE_MESSAGE[20] = "确定打开"
GUIDE_MESSAGE[21] = "点击打开礼包"
GUIDE_MESSAGE[22] = "点击使用"
GUIDE_MESSAGE[23] = "点击使用绷带"
GUIDE_OPENMESSAGE = "恭喜你开启了%s功能"
GUIDE_TITLE = {}
GUIDE_TITLE[1] = "酒馆"
GUIDE_TITLE[2] = "魔法阵"
GUIDE_TITLE[3] = "魔术"
GUIDE_TITLE[4] = "装备强化"
GUIDE_TITLE[7] = "修炼"
GUIDE_TITLE[9] = "好友"
GUIDE_TITLE[11] = "竞技场"
GUIDE_TITLE[12] = "庄园"
GUIDE_TITLE[13] = "公会"
GUIDE_TITLE[14] = "活动"
GUIDE_TITLE[15] = "金矿洞"
GUIDE_TITLE[16] = "命运水晶"
GUIDE_TITLE[17] = "神秘商店"
GUIDE_TITLE[18] = "领土战"
GUIDE_TITLE[19] = "世界BOSS挑战"
GUIDE_TITLE[20] = "送花"
GUIDE_TITLE[21] = "每日探险"
GUIDE_TITLE[22] = "庄园种植金钱树"
GUIDE_TITLE[23] = "多人副本"
GUIDE_TITLE[24] = "圣吉塔挑战"
GUIDE_TITLE[25] = "扫荡"
GUIDE_TITLE[28] = "任务"
GUIDE_OK = "打开"

--队列
ARRAY_POEN_QIANGHUA="开启更多强化队列"
ARRAY_TYPE_={}
ARRAY_TYPE_[1]="强化队列等待中"
ARRAY_TYPE_[2]="魔术队列等待中"
ARRAY_TYPE_[6]="每日探险队列等待中"
ARRAY_TYPE_[7]="庄园种植队列等待中"
ARRAY_TYPE_TIME_={}
ARRAY_TYPE_TIME_[1]="强化队列冷却中"
ARRAY_TYPE_TIME_[2]="魔术队列冷却中"
ARRAY_TYPE_TIME_[6]="探险队列冷却中"
ARRAY_TYPE_TIME_[7]="种植队列冷却中"
ARRAY_NOT="当前没有可选择的队列"
ARRAY_OK="消除冷却时间需花费%s"
ARRAY_ONT_GOLD="晶石不足"
--修炼
EXP_IN="练功修行中..."
EXP_GET="累计获得:%s经验"
EXP_TIME="练功时间:%s"
EXP_SWORD="队伍战斗力:%s"
--装备强化
EQUIP_ELIMINATE_TIME="强化同时消除冷却时间"
EQUIP_STRONG="装备强化"
EQUIP_COMPOSE="装备合成"
EQUIP_WEICHUAIDAI="未穿戴"
EQUIP_TIME="冷却时间"
EQUIP_STRONGMSG="是否使用%s直接进行强化?"
EQUIP_ZHUQUEQIANG="朱雀枪"
EQUIP_LEVEL="暗金%d级"
EQUIP_COMMONATTACT="普通攻击"
EQUIP_POWERATTACT="绝技攻击"
EQUIP_NOT_EQUIP="当前无装备"
EQUIP_NOT_EQUIP_TXT="当前无强化信息"
EQUIP_MESSAGE = {}
EQUIP_MESSAGE[1] = "当前无可强化装备"
EQUIP_MESSAGE[2] = "自动消除冷却时间将自动扣除晶石，是否确定强化同时消除冷却时间？"
EQUIP_MONEY="升级费用:"
EQUIP_STRONGOVER="强化升级属性"
EQUIP_STRONG="强化"
EQUIP_BAG="未穿戴"
EQUIP_NO_MONEY="铜钱不足"
EQUIP_NOT="不可强化"
EQUIP_COPPER="金币"
EQUIP_NOTGOLD="金币不足！"
EQUIP_NOTLEVEL="等级不足！"
EQUIP_STRONG_LEVEL = "强化等级"
EQUIP_LEVEL_={}--等级
EQUIP_LEVEL_[1]="青铜%d级"
EQUIP_LEVEL_[2]="玄铁%d级"
EQUIP_LEVEL_[3]="秘银%d级"
EQUIP_LEVEL_[4]="白金%d级"
EQUIP_LEVEL_[5]="鸣阳%d级"
EQUIP_LEVEL_[6]="天衍%d级"
EQUIP_LEVEL_[7]="神炽%d级"
EQUIP_LEVEL_[8]="神光%d级"
EQUIP_LEVEL_[9]="圣佑%d级"
EQUIP_LEVEL_[10]="魔炼%d级"
--------------登陆
LAN_Z="账号:"
LAN_PASSWOED="密码:"
LAN_QU_={}
LAN_QU_[1]="1"
LAN_QU_[2]="2"
LAN_QU_[3]="3"
LAN_QU_[4]="4"
LAN_IN="登录"
LAN_SUPER_IN="注册"
LAN_ADD_PASSWOED="补全密码:"
LAN_AGAIN_INPUT="再次输入:"
LAN_MODI_PASSWOED="修改密码:"
LAN_OK_ADD="确定补全"
LAN_OK_MODI="确定修改"
LAN_SELECT_SEVER="选择服务器"
LAN_INPUT_TXT="4-12位数字或字母组成"
LAN_NAME_LONG="名字太长，请重新输入"
LAN_NEW_INPUT_NAME="改名字已有玩家注册，请重新输入"
LAN_MAN="男性"
LAN_WOMAN="女性"
LAN_NAME="名字:"
LAN_START_GAME="开始游戏"
LAN_ID_MANAGE="账号管理"
LAN_ID_NOT_PASSWOED="未输入密码"
LAN_ID_NOT_Z="请输入账号和密码"
LAN_NOT_BU_PASSWORD="未输入补全密码！"
LAN_PLEASE_PASSWORD="请再次输入密码！"
LAN_BUG_PASSWORD="两次输入密码不同！"
LAN_PASSWORD_OK="密码修改成功"
LAN_ONT_NAME="请输入玩家姓名！"

LAN_CAR_={}--职业名称
LAN_CAR_[1]="格斗家"
LAN_CAR_[2]="火枪手"
LAN_CAR_[3]="鬼剑士"
LAN_CAR_DES_={}--职业描述
LAN_CAR_DES_[1]="高格挡，拳法缤纷，干劲威猛"
LAN_CAR_DES_[2]="高暴击，枪法精准，杀人无形"
LAN_CAR_DES_[3]="高闪避，挥剑如虹，轻巧如灵"
LAN_CARTXT="职业:"
LAN_CARTXT_DES="说明:"
----------------聊天
CHAT_TITLE="标题"
CHAT_TITLE_={}
CHAT_TITLE1="综合"
CHAT_TITLE2="世界"
CHAT_TITLE3="公会"
CHAT_TITLE4="私聊"
CHAT_TITLE5="公告"
CHAT_TITLE6="系统"
CHAT_LEFT="【"
CHAT_RIGHT="】"

CHAT_TYPE_BUTTOM_={}
CHAT_TYPE_BUTTOM_[1]="世界"
CHAT_TYPE_BUTTOM_[2]="公会"
CHAT_TYPE_BUTTOM_[3]="私聊"
CHAT_ADD_MAN="加友"
CHAT_ADD_MAN_OK="好友添加成功"
CHAT_SEND="发送"
CHAT_FIRENDNAME="请输入内容！"
CHAT_NOT_MAN="请选择玩家！"
CHAT__CANCEL="取消"
CHAT_SURE="确定"
CHAT_SAY="说:"
CHAT_NOT_GuildID="您还未加入公会，无法使用公会频道。"
CHAT_ME="我"
CHAT_FOR="对"
CHAT_ADDSUC="添加成功！"
CHAT_DRAW = "图纸"
--------------任务界面
Task_AGAIN="刷新任务"
Task_JINSHI_OK="晶石完成"
Task_NOT_FINDROAD="当前场景不可进行该操作！"
Task_title="任务:"
Task_show_task_button="显示任务跟踪"
TASK_SYMBOLS="【%s】"
Task_type_={}
Task_type_[1]="主线"
Task_type_[2]="支线"
Task_type_[3]="精英"
Task_type_[4]="日常"
Task_type_[5]="活动"
Task_in="接受任务"
Task_give_up="放弃任务"
Task_win="领取奖励"
Task_en="任务需求"
Task_name="名称"
Task_grade="星级:"
Task_txt="描述"
Task_aim="目标"
Task_good="奖励"
Task_ok="完成"
Task_STAR="星"
Task_SEE="阅历+"
Task_Status_={}
Task_Status_[0]="(不可接)"
Task_Status_[1]="(可接)"
Task_Status_[2]="(已接)"
Task_Status_[3]="(完成)"
Task_Status_[4]="(结束)"
Task_Status_[5]="(禁用)"
Task_SHOW_TOP_BUTTON_={}
Task_SHOW_TOP_BUTTON_[1]="当前任务"
Task_SHOW_TOP_BUTTON_[2]="可接任务"
Task_NEW_NPC="发布任务NPC:"
Task_END_NPC="完成任务NPC:"
Task_TO="去"
Task_TO_SEEK="去找"
Task_TALK="对话"
Task_COLLECT="收集"
Task_KILL="杀"
Task_DEFEAT="打败"
Task_WIN_GOOD="获得物品"
Task_O="。"
Task_BUTTON_TASK_END="完成任务"
Task_NOT_TASK="当前无任务"
Task_NOT_TASK_TXT="当前无任务描述"
Task_OK="好。"
Task_CommandType_={}
Task_CommandType_[1]="装备"
Task_CommandType_[2]="道具"
Task_CommandType_[3]="奇石"
Task_CommandType_[4]="黑市"
Task_CommandType_[5]="酒馆"
Task_CommandType_[6]="仓库"
Task_CommandType_[7]="命运"
Task_CommandType_[8]="公会祈祷"
Task_CommandType_[9]="激活"
TASK_MESSAGE = {}
TASK_MESSAGE[1] = "（%s级可接，修炼或副本战斗可获得经验）"
Task_NOTASK="当前没有正在进行的活动"

--限时活动
TASK_NAME = "活动名称"
TASK_TIME = "活动时间"
TASK_CONTENT = "活动内容"

---------------魔法布阵
EMBATTLE_00="【%s】"
EMBATTLE_TITLE="布阵"
EMBATTLE_SELECT="选择阵法"
EMBATTLE_SELECT_SPEED="显示出手顺序"
EMBATTLE_CLASS="级"

EMBATTLE_STAR_ZHENFA="启用阵法"
EMBATTLE_TIME="冷却时间:%s"
EMBATTLE_UPGRADE="升级"
EMBATTLE_SAVE_ZHENFA="保存阵法"
EMBATTLE_MESSAGE1= "(已启用)"
EMBATTLE_OPENSUC= "启用成功"
EMBATTLE_SAVESUC= "保存成功"
-----------------------公会战界面
MERCENARY_FACTION="公会战"
MERCENARY_APPLY="报名"
MERCENARY_LOOK="查看报名"
MERCENARY_APPLY_OK="已报名公会"
MERCENARY_LIST="公会战对战列表"
MERCENARY_OGTO_WAR="进入战场"
MERCENARY_OGTO_WIN="冠军"
MERCENARY_F="帮"
MERCENARY_TXT_={}
MERCENARY_TXT_[1]="1.公会战每周日21:00开战，需要公会等级达到2级。"
MERCENARY_TXT_[2]="2.每周一由帮主或副班主进行报名，周日20:50截止。"
MERCENARY_TXT_[3]="3.当周累计贡献前16强的公会可以晋级公会战。"
MERCENARY_TXT_[4]="4.公会战开始后退出战场将视为放弃公会战。"
MERCENARY_TXT_[5]="5.冠军和亚军公会将获得3个礼包，由帮主分配。"
MERCENARY_JIANGLI="公会奖励："
MERCENARY_BAOMING="报名公会："
MERCENARY_OK="您的公会已成功报名。请于周日21:00参加帮战。"
MERCENARY_NO="您没有报名权限，可以让帮主或副帮主来报名。"
MERCENARY_YET="您的公会已经报名。"
MERCENARY_TXT_APPLY={}
MERCENARY_TXT_APPLY[1]="排名"
MERCENARY_TXT_APPLY[2]="公会"
MERCENARY_TXT_APPLY[3]="会长"
MERCENARY_TXT_APPLY[4]="今日公会贡献"
MERCENARY_TXT_APPLY[5]="本周公会贡献"

---------------------------------------------------------------------------------
--装备封灵
EQUIP_SOUL="封灵"
EQUIP_WEAPONSOUL="武器封灵"
EQUIP_BAGSOUL="灵器背包"
EQUIP_UNKNOWNAME="名称"
EQUIP_XILIAN="洗炼"
EQUIP_ATTRIBUTE="属性"
EQUIP_LOCK="锁定"
EQUIP_UNLOCK="解锁"
EQUIP_ACTIVATION="激活"
EQUIP_SOULXILIAN="灵器洗炼"
EQUIP_XILIANORDER="洗炼花费"
EQUIP_XILIANCOST="%d灵石 %d铜币"
EQUIP_ACTIVATIONMSG="是否使用%d晶石激活新属性?"
EQUIP_XILIANMSG="是否使用%d晶石%d铜币洗炼以下属性?"
EQUIP_DECOMPBTN="分解"
EQUIP_DECOMPTEXT="分解可得"
EQUIP_DECOMPMONEY="%d灵石%d铜币"
EQUIP_DECOMPMSG="是否确认分解灵器"
EQUIP_DISBOARD="卸下"

--装备合成
PROP_SYNTHESISEQUIP="装备合成"
PROP_SYNTHESISEPILL="丹药合成"
PROP_MAKE="普通制作"
PROP_JINMAKE="晶石制作"
PROP_MAKEMSG="是否使用%d晶石直接进行制作?"
PROP_PILLSYNTHESIS="合成丹药"
PROP_EQUIPSYNTHESIS="合成装备"
PROP_PILLATTRIBUTE="丹药属性"
PROP_EQUIPATTRIBUTE="装备属性"
PROP_PILLSYNTHESISNUM="合成数量:%d"

PRODUCT_MESSAGE = "所需材料"
PRODUCT_MESSAGE2= "制作成功"
PRODUCT_MESSAGE3 = "该功能暂未开放"
PRODUCT_NAME = "名称"
PRODUCT_NUM = "可合成数量"

--猎命
LIFECELL_HUNTING="命运"
LIFECELL_GOLD="晶石"
LIFECELL_COPPER="金币"
LIFECELL_TEXT="今日剩余%d次免费猎命。"
LIFECELL_SALE="卖出命运"
LIFECELL_SALEKEY="一键卖出"
LIFECELL_BAG="命运背包"
LIFECELL_PICKUP="一键拾取"
LIFECELL_PICKUPFULL="命运背包已满，无法继续拾取命运！"
LIFECELL_COMBINE="合并闲置"
LIFECELL_TOHUNTING="前往猎命"
LIFECELL_COMBINEMSG="是否确认合并背包中的所有闲置命运?"
LIFECELL_EXP = "命格经验"
LIFECELL_MONEY = "价格"
LIFECELL_BUTTON1 = "卸下"
LIFECELL_BUTTON2 = "合成"
LIFECELL_BUTTON3 = "卖出"
LIFECELL_BUTTON4 = "拾取"
LIFECELL_BUTTON5 = "晶石猎命"
LIFECELL_SUCCESS = "合成成功"
LIFECELL_SALE_SUCCESS = "卖出成功"
LIFECELL_MESSAGE = {}
LIFECELL_MESSAGE[1] = "等级达到%d级解除封印"
LIFECELL_MESSAGE[2] = "当前没有可使用的命运水晶。"
LIFECELL_MESSAGE[3] = "是否花费100晶石进行猎命?"
LIFECELL_ONEGET = "一键猎命"
LIFECELL_ISLIGHT = "当前人物已点亮！"
LIFECELL_BAGFULL = "猎命空间已满"


--招募
ZHAOMU="招募"
BACKTEAM = "归队"
RECRUIT_POSSENTIAL="职业"
RECRUIT_CONDITION = "招募条件"
RECRUIT_REQUESTMONEY = "金币"
RECRUIT_SUCCESSMESSAGE = "招募成功"
RECRUIT_SUCCESSMESSAGE2 = "归队成功"
RECRUIT_PROFESSION1 = "格斗家"
RECRUIT_PROFESSION2 = "神枪手"
RECRUIT_PROFESSION3 = "鬼剑士"
RECRUIT_PROFESSION4 = "圣骑士"
RECRUIT_PROFESSION5 = "魔法师"
RECRUIT_STARTLIFE= "初始生命"
RECRUIT_POPULARITY="声望"
RECRUIT_CURRENTPOPULARITY = "当前声望"
RECRUIT_STARTSOULSKILL="魂技"
RECRUIT_SKILLINTRO="魂技说明"
RECRUIT_STARTPOWER="初始力量"
RECRUIT_STARTSOULPOWER="初始魂力"
RECRUIT_STARTSMART="初始智力"
RECRUIT_TEAM = "队伍人数"

--奇术
MAGIC_TITLE="奇术"
MAGIC_GONGFA="功法"
MAGIC_ZHENFA="阵法"
MAGIC_LIFEVALUE="生命值"
MAGIC_NORMALATTACK="普通攻击"
MAGIC_SPELLATTACK="法术攻击"
MAGIC_NORMALDEFENSE="普通防御"
MAGIC_STUNTATTACK="绝技攻击"
MAGIC_APELLDEFENSE="法术防御"
MAGIC_STUNTDEFENSE="绝技防御"
MAGIC_LEVEL="%d级"
MAGIC_FUNCDESCG="功法说明："
MAGIC_FUNCDESCZ="阵法说明："
MAGIC_CONSUMPEXP="消耗阅历："
MAGIC_COOLINGTIME="冷却时间："
MAGIC_LEVNEEDS="等级需求："
MAGIC_MESSAGE = {}
MAGIC_MESSAGE[1] = "点"
MAGIC_MESSAGE[2] = "分"
MAGIC_MESSAGE[3] = "级"
MAGIC_MESSAGE[4] = "冷却时间未到"
MAGIC_MESSAGE[5] = "秒"
MAGIC_MESSAGE[6] = "(已满级)"
MAGIC_UPGRADE="升级"
MAGIC_SURPLUSEXP="剩余阅历：%d"
MAGIC_COOLING="魔术冷却：%s"
MAGIC_TIANMENZ="天门阵"
MAGIC_HEYIZ="鹤翼阵"
MAGIC_ZHENGQIZ="正奇阵"
MAGIC_TIANGANGZ="天罡阵"
MAGIC_CIHUNZ="刺魂阵"
MAGIC_FEIYUZ="飞鱼阵"
MAGIC_QIXINGZ="七星阵"
MAGIC_JIANXINGZ="剑形阵"
MAGIC_ISCANUPMESSAGE = "当前可以升级魔术"

--角色
ROLE_MENU1 = "查看"
ROLE_MENU2 = "装备"
ROLE_MENU3 = "卸下"
ROLE_MENU4 = "展示"
ROLE_MENU5 = "使用"
ROLE_MENU6 = "丢弃"
ROLE_LEVELUP="升级"
ROLE_NULL ="无"
ROLE_NAME="名称"
ROLE_LEVEL="等级"
ROLE_DETAIL="详情"
ROLE_PILL="药剂"
ROLE_TRAIN="培养"
ROLE_LEAVE = "离队"
ROLE_LIFE="生命"
ROLE_EXPERIENCE = "当前经验"
ROLE_UPEXPERIENCE = "升级经验"
ROLE_POPULARITY="声望"
ROLE_DRAFT="经验"
ROLE_DUTY="职业"
ROLE_SOULSKILL="魂技"
ROLE_SOULSKILL_DETAIL = "魂技说明"
ROLE_POWER="力量"
ROLE_SOULPOWER="魂力"
ROLE_SMART="智力"
ROLE_POTENTIAL="潜力"
ROLE_UNIQUESKILL="绝技"
ROLE_LEFEPOWER="命力"
ROLE_GODKING="神王"
ROLE_FORCENUM="武力值"
ROLE_USKILLNUM="绝技值"
ROLE_MAGICNUM="法术值"
ROLE_LIFE="生命"
ROLE_HUN="魂"
ROLE_WU="武"
ROLE_FU="符"
ROLE_TOU="头"
ROLE_PAO="袍"
ROLE_XUE="靴"
ROLE_FORCE="武力"
ROLE_USKILL="绝技"
ROLE_MAGIC="法术"
ROLE_NOWATTRIBUTE="当前属性"
ROLE_NEWATTRIBUTE="新属性"
ROLE_SAVE="保存"
ROLE_CANCLE="取消"
ROLE_WULIPILL="武力丹"
ROLE_JUEJIPILL="绝技丹"
ROLE_FASHUPILL="法术丹"
ROLE_CULTURETYPE_1 = "普通培养"
ROLE_CULTURETYPE_2="加强培养"
ROLE_CULTURETYPE_3="白金培养"
ROLE_CULTURETYPE_4="钻石培养"
ROLE_CULTURETYPE_5="至尊培养"
ROLE_CURRENTATTR = "当前属性"
ROLE_NEWATTR = "新属性"
ROLE_USEMEDICINE = "使用药剂"
ROLE_VIPMEDICINE="晶石药剂"
ROLE_GETMEDICINE="摘取药剂"
ROLE_GETSUCCESS="摘取药剂成功"
ROLE_MESSAGE = {}
ROLE_MESSAGE[1] = "当前没有可使用的装备"
ROLE_MESSAGE[2] = "当前没有可使用的药剂"
ROLE_MESSAGE[3] = "是否确定让【%s】离队"

ROLE_MEDLEV = {}
ROLE_MEDLEV[1]="一级"
ROLE_MEDLEV[2]="二级"
ROLE_MEDLEV[3]="三级"
ROLE_MEDLEV[4]="四级"
ROLE_MEDLEV[5]="五级"
ROLE_MEDLEV[6]="六级"



--副本
FB_ZHUXIAN="主线副本"
FB_DEKARONPATTERN="挑战模式"
FB_SWEEPPATTERN="扫荡模式"
FB_SWEEP="扫荡"
FB_YINLONGKU="隐龙窟"
FB_SHUSHANMIDAO="蜀山秘道"
FB_SWEEPDETAIL="扫荡详情"
FB_SWEEPNAME="副本名称"
FB_BAGCELL="背包空格"
FB_SWEEPOBJECT="扫荡对象"
FB_NUMCHOICE="次数选择"
FB_BEGINSWEEP="开始扫荡"
FB_YUANBAOSWEEP="晶石扫荡"
FB_SWEEPCHOICE1="扫荡至精力用尽为止"
FB_SWEEPCHOICE2="30分钟扫荡%d次，消耗%d点精力"
FB_SWEEPCHOICE3="60分钟扫荡%d次，消耗%d点精力"
FB_YBSWEEPMSG="是否花费%d个晶石直接完成扫荡?"
FB_SWEEPOVER="扫荡完成"
FB_SWEEPTURN="第%d轮扫荡"
FB_BATTLETURN="第%d次战斗"
FB_BATTELING="战斗中......"
FB_GET="战利品"
FB_PRIZE="副本奖励"
FB_PINGJIAJIANGLI="副本评价奖励"
FB_LEVEL="级"
FB_CANCEL="取消"
FB_QUICKEN="加速"
FB_EXP="经验"
FB_GAMECOIN="金币"
FB_GOLD="晶石"
FB_EXPNUM="阅历"
FB_STOPTIME="扫荡结束剩余时间"
FB_STOPSWEEP="是否停止扫荡并退出"
LEAVE_PLOTSCENE="离开副本"
TOTAL_SORCE="总分: "
FB_REFRESH="刷新"
FB_REFRESHSUCCESS="刷新成功"
FB_REFRESHFAILED="刷新失败"
LASTREFRESH="今日剩余刷新次数: "
ASKFORAGAIN="是否继续挑战？ "
FB_HERO="英雄"

--竞技场
ARENA_AFTERMINS="%d分钟后"
ARENA_TITLE="竞技场"
ARENA_NULL = "无"
ARENA_GPTITLE="称号"
ARENA_PRESTIGE="声望"
ARENA_RANKING="排名"
ARENA_WINNINGSTREAK="连胜"
ARENA_HEROES="英雄榜"
ARENA_LIFECELL="命运"
ARENA_EMBATTLE="布阵"
ARENA_STRENGTHEN="强化"
ARENA_SOUL="封灵"
ARENA_NOTICE="竞技场公告"
ARENA_NOTICETXT_={}
ARENA_NOTICETXT_[1]="排名%d"
ARENA_NOTICETXT_[2]="的奖励:%d"
ARENA_NOTICETXT_[3]="金币,%d声望"
ARENA_NOTICETXT_[4]="领取时间%s"
ARENA_NOTICETXT_[5]="后天"
ARENA_NOTICETXT_[6]="明天"
ARENA_NOTICETXT_[7]="%d小时后"
ARENA_NOTICETXT_[8]="挑战了"
ARENA_NOTICETXT_[9]="，你%s了！"
ARENA_NOTICETXT_[10]="排名上升至%d名!"
ARENA_NOTICETXT_[11]="排名下降至%d名!"
ARENA_NOTICETXT_[12]="排名不变!"
ARENA_NOTICETXT_[13]="战胜"
ARENA_NOTICETXT_[14]="失败"
ARENA_NOTICETXT_[15]="【查看】"
ARENA_NOTICETXT_[16]="挑战时间冷却中"
ARENA_ZHANBAO_={}
ARENA_ZHANBAO_[1]="挑战了"
ARENA_ZHANBAO_[2]="，战"
ARENA_ZHANBAO_[3]="了！排名"
ARENA_WIN="胜"
ARENA_LOSE="败"
ARENA_LEVSTRING="等级%d"
ARENA_DAYTIMES="今天还可挑战"
ARENA_TIMES = "次"
ARENA_ADDTIMES="增加"
ARENA_ISCOOLINGTIME="是否消耗%s晶石清除冷却时间"
ARENA_ADDTIMESMSG="是否消耗%d晶石增加1次挑战次数"
ARENA_HEROLISTTITLE="英雄榜"
ARENA_HLTITLERANK="排名"
ARENA_HLTITLEGPLAYER="玩家"
ARENA_HLTITLELEVEL="等级"
ARENA_HLTITLECAPABILITY="战力"
ARENA_HLTITLETREND="趋势"
ARENA_TITLE_={}
ARENA_TITLE_[1]="排名"
ARENA_TITLE_[2]="玩家"
ARENA_TITLE_[3]="等级"
ARENA_TITLE_[4]="战力"
ARENA_TITLE_[5]="称号"
ARENA_TITLE_[6]="连胜"
ARENA_TITLE_[7]="趋势"
ARENA_UP="上升"
ARENA_DOWN="下降"
ARENA_NOT="不变"
ARENA_WIN="战胜了"
ARENA_GETWIN=",获得了胜利。"
--背包
BAG_TITLE="背包"
BAG_ARRANGEBAG="整理背包"
BAG_OPENBAG="您确定消耗%s开启以上背包吗？"
BAG_SPARNUM="晶石"
BAG_GOLDCOIN="金币"
BAG_DETAILTITLE="详情"
BAG_DETAILBACK="返回"
BAG_DETAILSALE="出售"
BAG_DETAILSYNTH="合成"
BAG_SAVE = "存入"
BAG_GET = "取出"
BAG_XIANGMOJIAN="降魔剑"
BAG_IS_OPEN_1="是否消耗"
BAG_IS_OPEN_2="个晶石开启所选择的"
BAG_IS_OPEN_3="个新格"
BAG_NO="取消"
BAG_OK="确定"
EQUIP_DECS="装备说明:"
BAG_Z_={}
BAG_Z_[1]="强化等级:"
BAG_Z_[2]="物理攻击"
BAG_Z_[3]="魂攻击"
BAG_Z_[4]="穿戴需求:"
BAG_Z_[5]="角色等级:"
BAG_Z_[6]="角色职业:"
BAG_Z_[7]="价格:"
BAG_TYPE_={}--装备属性
BAG_TYPE_[1]="生命"
BAG_TYPE_[2]="物理攻击"
BAG_TYPE_[3]="魂技攻击"
BAG_TYPE_[4]="魔法攻击"
BAG_TYPE_[5]="物理防御"
BAG_TYPE_[6]="魂技防御"
BAG_TYPE_[7]="魔法防御"
BAG_TYPE_[8]="暴击"
BAG_TYPE_[9]="命中"
BAG_TYPE_[10]="破击"
BAG_TYPE_[11]="韧性"
BAG_TYPE_[12]="闪避"
BAG_TYPE_[13]="格挡"
BAG_TYPE_[14]="必杀"
BAG_TYPE_[15]="眩晕"
BAG_TYPE_[16]="昏睡"
BAG_TYPE_[17]="冰冻"
BAG_TYPE_[18]="迷失"
BAG_TYPE_[19]="定身"
BAG_TYPE_[20]="中毒"
BAG_TYPE_[21]="出血"
BAG_TYPE_[22]="气势"
BAG_PRICE = "金币"
BAG_SPARESALE = "出售装备后灵件将随装备一起出售，确定要出售装备吗？"
--公会
CROPS_TITLE="公会"
CROPS_RANKING="排名"
CROPS_WANG="帮主"
CROPS_LEVEL="等级"
CROPS_NUMBER="人数"
CROPS_CHECK="查看"
CROPS_BTNCROPS="创建公会"
CROPS_ENTERNAME="输入名称"
CROPS_PROMPT="花费500000金币"
CROPS_CONFIRM="确认"
CROPS_CLOSE="关闭"
CROPS_CHECKCROPS="查看公会"
CROPS_NAME="名称"
CROPS_COLONEL="会长"
CROPS_EXPERIENCE="经验"
CROPS_MEMBERLIST="成员列表"
CROPS_POST="职务"
CROPS_COMRANKING="竞技排名"
CROPS_APPLYJOIN="申请加入"
CROPS_CANCELJOIN="取消申请"
CROPS_OTHERCROPS="其他公会"
CROPS_CROPSPLACE="集会所"
CROPS_LOOKCROPS="查看公会"
CROPS_LOOKMEMBER="查看成员"
CROPS_RETURN="返回"

--我的公会
MYCROPS_TITLE="我的公会"
MYCROPS_INFORMATION="公会信息"
MYCROPS_MEMBER="成员"
MYCROPS_JOURNAL="日志"
MYCROPS_ACTIVITY="活动"
MYCROPS_EXAMINE="审核"
MYCROPS_NAME="名称"
MYCROPS_LEVEL="等级"
MYCROPS_RANKING="排名"
MYCROPS_NUMBER="人数"
MYCROPS_EXPERIENCE="经验"
MYCROPS_WEEKEXPER="本周累计贡献%s经验"
MYCROPS_GIVETO="转让"
MYCROPS_QUIT="退出"
MYCROPS_OVER="解散公会"
MYCROPS_SET_INFO="设置介绍"
MYCROPS_SET_NOTICE="设置公告"
MYCROPS_MODIFICATIOON="修改"
MYCROPS_NOTICETITLE="公会公告"
MYCROPS_INTRUCTION="公会介绍"
MYCROPS_OTHERCROPS="其它公会"
MYCROPS_ASSEMBLY="集会所"
MYCROPS_RETURNCITY="返回城市"
MYCROPS_QUITMSG="是否退出公会"
MYCROPS_GIVETOOTHER="转让会长"
MYCROPS_GIVECROPS="是否将会长转让给%s"
MYCROPS_NOGIVECROPS="公会没有副会长,无法转让"
MYCROPS_CONFIRM="确认"
MYCROPS_CLOSE="关闭"
MYCROPS_MEMBERLIST="成员列表"
MYCROPS_MEMBERLEVEL="等级"
MYCROPS_WORK="职务"
MYCROPS_ARENALEVEL="竞技排名"
MYCROPS_DAYCONTRIBUTION="今日贡献"
MYCROPS_TOTALCONTRIBUTION="总贡献"
MYCROPS_LOGINTIME="登录"
MYCROPS_NOW="当前"
MYCROPS_APPOINTMENT="任命"
MYCROPS_CHEXIAO="撤销"
MYCROPS_CHECK="查看"
MYCROPS_NOTRECORD="当前无成员贡献记录"
MYCROPS_NOTAPPLY="当前无申请记录"
MYCROPS_EXPEL="驱逐"
MYCROPS_NOTICE="公告"
MYCROPS_EDIT="修改"
MYCROPS_APPOINTMSG="是否任命%s为副会长"
MYCROPS_DISAPPOINTMSG="是否取消%s的副会长职位"
MYCROPS_NOCHOOSEMSG="请选择一位成员"
MYCROPS_CHAIRMAN="会长"
MYCROPS_VICECHAIRMAN="副会长"
MYCROPS_MEMCONTRI="成员贡献记录"
MYCROPS_TIME="时间"
MYCROPS_POPULARITY="声望"
MYCROPS_EXP="经验"
MYCROPS_GET="获得"
MYCROPS_GIVE=", 为公会贡献了"
MYCROPS_APPLYLIST="申请列表"
MYCROPS_APPLYTIME="申请时间"
MYCROPS_OPERATION="操作"
MYCROPS_AGREE="同意"
MYCROPS_REFUSEAPPLY="忽略所有"
MYCROPS_NOWLEVEL="当前公会级别 %d级"
MYCROPS_NOWNEEDEXP="还需要%d经验,您的公会将升到%d级"

MYCROPS_BUILD="恭喜您成功创建公会！"

--公会BOSS
CROPSEBOSS_SET="设置时间"
CROPSEBOSS_LOOK="查看时间"
CROPSEBOSS_ISSET="是否设置"
CROPSEBOSS_DAY_ONE="本周一21:00开启"
CROPSEBOSS_DAY_TWO="本周二21:00开启"
CROPSEBOSS_DAY_THREE="本周三21:00开启"
CROPSEBOSS_DAY_FOUR="本周四21:00开启"
CROPSEBOSS_DAY_FIVE="本周五21:00开启"
CROPSEBOSS_DAY_SIX="本周六21:00开启"
CROPSEBOSS_DAY_SEVEN="本周日16:00开启"
CROPSEBOSS_DAY_EIGHT="本周日22:00开启"
CROPSEBOSS_CHECKED="已选择"
CROPSEBOSS_SUCCESS="公会BOSS时间设置成功"
CROPSEBOSS_HAVESET="本周已设置"
CROPSEBOSS_NOSET="本周公会BOSS时间未设置"

--公会技能
CORPSSKILL_SKILL		= "公会技能"
CORPSSKILL_SKILLUP	= "升级"
CORPSSKILL_JINGSHI	= "晶石捐献"
CORPSSKILL_GOLD		= "金币捐献"
CORPSSKILL_COUNT		= "公会技能点"
CORPSSKILL_LEVEL		= "玩家等级"
CORPSSKILL_GOLDNUM	= "可捐献金币"
CORPSSKILL_JINGSHINUM	= "可捐献晶石"
CORPSSKILL_MAKEUP	= "加成值:%s"
CORPSSKILL_NEED		= "升级需要:%s技能点"
CORPSSKILL_HAVEGIVE	= "已捐献数量"
CORPSSKILL_GIVE		= "捐献"
CORPSSKILL_MAX		= "最大"
CORPSSKILL_GOLDOVER	= "您输入的数值大于当日可捐献最大金额，请重新输入"
CORPSSKILL_JINGSHIOVER	= "您输入的数值大于当日可捐献最大晶石，请重新输入"
CORPSSKILL_INPUTGOLD		= "请输入分配金额"
CORPSSKILL_INPUTJINGSHI		= "请输入分配晶石"

CORPSSKILL_SKILLLEVEL		= "等级"
CORPSSKILL_SCALE		= "捐献比例"
CORPSSKILL_GIVEGOLD	= "是否捐献%s%s？"
CORPSSKILL_GIVESUCCESS	= "捐献成功"
CORPSSKILL_LEVELUP	= "技能升级成功"
CORPSSKILL_NOLEVELUP		= "当前没有可升级技能"
CORPSSKILL_FUNCTION			= "功能"
CORPSSKILL_LEVELTOP		= "已满级"

--庄园种植
PLANT_FORTUNETREE="发财树"
PLANT_EXPERIENCETREE="经验树"
PLANT_CHOOSEPARTNER="选择伙伴"
PLANT_LEVELNUM="%d级"
PLANT_PUTONG="普通"
PLANT_YOUXIU="优秀"
PLANT_JINGLIANG="精良"
PLANT_CHUANQI="传奇"
PLANT_SHENHUA="神话"
PLANT_QUALITY="种植品质"
PLANT_REFRESH="刷新"
PLANT_REFRESHMSG="是否消耗%d晶石刷新品质"
PLANT_FULLLEVEL="满级"
PLANT_FULLLEVMSG="是否消耗%d晶石直接满级"
PLANT_PARTNERMSG="伙伴等级不能超过主角"
PLANT_BUTTON="种植"
PLANT_CHOOSE="选择种植类型"
PLANT_PFORTUNETREE="发财树"
PLANT_PEXPERIENCETREE="经验树"
PLANT_COLDTIME="是否消耗%d晶石消除冷却时间"
PLANT_BUY="购买"
PLANT_HARVEST="收获"
PLANT_UPDATE="升级土地"
MERCENARIES_LV="佣兵等级: "
PLANT_QUALITY="种植品质: "
PLANT_EXP="收获经验: "
PLANT_GOLD="收获金币: "
NO_MERCENARIES="您还没有佣兵！"
LV="Lv"


--活动界面
ACTIVITY_TITLE="活动列表"
ACTIVITY_JOINBTN="参加"
ACTIVITY_NOBEGIN="未开始"
ACTIVITY_NOOPEN="未开启"
ACTIVITY_ISOVER="已结束"
ACTIVITY_MESSAGE = {}
ACTIVITY_MESSAGE[1] = "当前没有可参与的活动"
ACTIVITY_MESSAGE[2] = "日常活动"
ACTIVITY_MESSAGE[3] = "公会活动"

PERINFO="个人信息"
HIGHESTWIN="最高连胜"
CURWIN="当前连胜"
WIN="赢"
LOSS="输"
GETPROSE="获得声望"
GETMONEY="获得铜钱"
PER_BILLBOARD="个人战报"
ALL_BILLBOARD="所有战报"

--好友界面
FRIEND_TITLE_={}
FRIEND_TITLE_[1]="最近联系"
FRIEND_TITLE_[2]="好友"
FRIEND_TITLE_[3]="粉丝"
FRIEND_TITLE_[4]="黑名单"
FRIEND_LIST_TITLE_={}
FRIEND_LIST_TITLE_[1]="好友列表"
FRIEND_LIST_TITLE_[2]="等级"
FRIEND_LIST_TITLE_[3]="竞技排名"
FRIEND_LIST_TITLE_[4]="通关副本"
FRIEND_LIST_TITLE_[5]="最近登录"
FRIEND_LIST_TITLE_[6]="操作"
FRIEND_BOTTOM_BUTTON_={}
FRIEND_BOTTOM_BUTTON_[1]="私聊"
FRIEND_BOTTOM_BUTTON_[2]="删除"
FRIEND_ISDELETE="是否删除好友"
FRIEND_ADDTITLE="添加好友"
---------------

--金矿洞
GETMONEYTIP="进入金矿洞可以获得大量金币"
GETMONEY_OK="使用"
--领土战
LINGTUZHAN="领土战"
PARTINSUC="参战成功"
PARTINHERO="参战英雄"
PARTIN="参战"
GUWU="晶石鼓舞"
YUELI="阅历鼓舞"
SHUSHAN="护卫队积分: "
KUNLUN="先攻队积分: "
HUWEI="护卫队"
XIANGONG="先攻队"
ENDTIME="领土战结束时间: "
WINPA="最高连杀: "
USE200TIP="消耗200阅历有几率增加20%战斗力"
USE20TIP="消耗20晶石加20%战斗力"
SHUSHANBATTLE="莫根玛英雄"
KUNLUNBATTLE="哈斯德尔英雄"
PERSONRAL="个人信息"
STR_STRAKWIN="连胜"
STR_MaxWinNum="最高连胜: %d场"
STR_CurWinNum="当前连胜: %d场"
STR_WINNUM="获得胜利: %d场"
STR_LOSTNUM="战斗失败: %d场"
STR_OBTAINNUM="获得声望: %d"
STR_COLDNUM="获得金币: %d"
STR_BEAT="击败"
STR_FINISHSTEAK="完成%d连杀"
STR_FINISHLOSHSTEAK="终结了%s的%s连杀"
STR_WINNUMS="获得%d金币,%d声望!"
STR_GET="获得 "
LEVELPART1="20~40级 "
LEVELPART2="40级以上 "
GENERALNAME="名称"
GENERALLEVEL="等级"
GENERALWin="胜利场次 "
GENERALLOSTN="失败场次 "
GENERALCWIN="当前连胜 "
GENERALMAXWIN="最高连胜 "
ENCORANGE="鼓舞增加战斗力: "
ENCORANGESUC="鼓舞成功! "
--boss战
BOSSACTIVITY_NAME="BOSS战"
BOSS_NAME="擎天木"
CUR_TOTLENUM="当前参与人数"
OUT_BATTLE="离开副本"
BOSS_PHOENIX="浴火重生"
BOSS_PHOENIX_PAY="是否消耗%d晶石直接进入战斗"
BOSS_PHOENIX_UNALLOW="挑战还未开始,不可以使用浴火重生"
BOSS_START="挑战开始"
BOSS_HURT="伤害排名"
BOSS_HURT_NUM="造成%d伤害,"
BOSS_RANK="您当前排%d名"
BOSS_GUWU="鼓舞战斗力"
BOSS_DEADTH="已击杀"
--客栈
LLONN_ALL="全部"
LLONN_ORDINARY="普通佣兵"
LLONN_PRESTIGE="声望佣兵"
LLONN_TITLE="酒馆"
LLONN_PRESTIGE = "声望"
LLONN_NOYONG="当前无可招募佣兵"
LLONN_NOINFO="当前无佣兵信息"

---每日探险
DAILY_REWARD="获得"
DAILY_COOL_MOVE="花费%d晶石清除冷却时间"
DAILY_REWARDTYPE_={}
DAILY_REWARDTYPE_[1]="金币"
DAILY_REWARDTYPE_[2]="声望"
DAILY_REWARDTYPE_[3]="阅历"
DAILY_REWARDTYPE_[4]="精力"
DAILY_REWARDTYPE_[5]="经验"
DAILY_REWARDTYPE_[6]="晶石"
DAILY_JINGBIN="金币"
DAILY_SHENGWANG="声望"
--战斗
LABLE_HP="血量: "
LABEL_QISHI="气势: "
LABLE_HUNJI="魂技: "
NOHP="血量不足，请使用绷带"

---商店
SHOP_SALE="买卖"
SHOP_BUYBACK="购回"
SHOP_BUY="购买"
SHOP_LOOK="查看"
SHOP_PLAYER_NAME="角色等级"
SHOP_PLAYER_WORK="角色职业"
SHOP_PRICE="价格"
SHOP_BUYSUCCESS="购买成功"
SHOP_NOSOLD="当前没有可回购的物品"
SHOP_QISHI="奇石"
SHOP_SHENGWANG="声望"
SHOP_QISHITYPE_WHITE="白奇石"
SHOP_QISHITYPE_GREEN="绿奇石"
SHOP_QISHITYPE_BLUE="蓝奇石"
SHOP_QISHITYPE_PURPLE="紫奇石"
----神秘商店
SHOP_NOTE="商店物品每六小时刷新一次"
SHOP_TOPNOTE="本次刷新物品"
SHOP_SM_MAN="神秘商人"
SHOP_HAVEBUY="已购买"
EQUIP_CHOICEEQUIP="选择装备"
SHOP_REFRESH="晶石刷新"
SHOP_REFRESHSUCCESS="刷新商品成功"
--国家选择
COUNTRY_ONE="莫根玛"
COUNTRY_TWO="哈斯德尔"
COUNTRY_JOIN="加入"
COUNTRY_JOIN_ONE="恭喜您加入国家莫根马"
COUNTRY_JOIN_TWO="恭喜您加入国家哈斯德尔"
--多人副本
GROOP_NAME="多人副本"
GROUP_LOOT="掉落"
GROUP_GROUPLIST="组队列表"
GROUP_EXPNUM="阅历"
GROUP_CAPTAIN="队长"
GROUP_PEOPLE_NUM="人数"
GROUP_NAME = "名称"
GROUP_GAREER = "职业"
GROUP_LEVEL = "等级"
GROUP_MESSAGE="人数已满"
GROUP_MESSAGE2="次数已满"
GROUP_MESSAGE3="当前没有可加入的队伍"
GROUP_MESSAGE4="当前没有可创建副本"
--GROUP_NOTINTIME="多人副本已结束"
GROUP_FASTIN="快速进入"
GROUP_CREATPLOT="创建副本"
GROUP_CREATE = "创建"
GROUP_RETURN="返回"
GROUP_LEAVE="离开"
GROUP_START="开始挑战"
GROUP_JOIN="加入"
GROUP_OUT = "踢出"
GROUP_SURE = "确认"
GROUP_EXPERINECE="经验"
RROUP_GETMESSAGE       = "获得"
RROUP_GETEXPERIENCE = "获得经验"
GROUP_INGORMATION="组队信息"
GROUP_INGORMATION_CAPTAIN="(队长)"

GROUP_INGORMATION_LEVEL="%d级"

STRING="你"

----------------副本剧情

--系统设置
SYSTEMCONFIG="个人信息"
MUSICCONFIG="声音设置: "
MUSICTURNON="开启"
MUSICTURNDOWN="关闭"
SHOWPLAYERS="同屏显示人数: "
SHOWPLAYERS1="全部屏蔽 "
SHOWPLAYERS3="全部显示 "
SHOWPLAYERS2="显示15人"
SHOWPLAYERS4="显示30人"
ACCOUTCONFIG="账号设置"
DEFULTCONFIG="默认设置"
SEVERNAME="您所在的游戏大区: "
GAMEACOOUNT="您的账号: "
PLAYERNAME="昵称: "
PLAYERLV="等级: "
PLAYEREXP="阅历: "
PLAYERVIPLV="您当前VIP等级: "
NOVIP="您还没有VIP权限! "
NEXTVIP1="继续充值"
NEXTVIP2="晶石可以提升至"
NEXTVIP="继续充值%d晶石可以提升至Lv.6 "
CHECKDETAIL="查看特权"
TOPUPNOW="立即充值"
SCIENCE="静音: "
TAB_ACCOUNTT="账号信息"
TAB_PERSONRAL="个人信息"
NOTUSE="功能暂未开放"
GEDOUJIA="格斗家"
SHENQIANGSHOU="神枪手"
GUIJIANSHI="鬼剑士"
SHENQISHI="圣骑士"
MOFASHI="魔法师"
STATES="拥有状态: "
USERHP="生命: "
WULIGONGJI="物理攻击: "
HUNJIGONGJI="魂技攻击: "
FASHUGONGJI="法术攻击: "
HUNJIFANGYU="魂技防御: "
FASHUFANGYU="法术防御: "
GERENXIANGONG="个人先攻: "
WULIFANGYU="物理防御: "
BAOJI="暴击: "
MINGZHONG="命中: "
POJI="破击: "
BISHA="必杀: "
RENXING="韧性: "
SHANBI="闪避: "
GEDANG="格挡: "
QUITGAME="退出游戏"
ASKQUITGAME="是否退出游戏？"

--战斗界面
GETITEMS="战利品: "
VICTORY="胜利"
EVATION="通关评价"
GETEXP="获得经验: "

LEVELFIGHTTIP1="您当前剩余跳过次数: %s"
--LEVELFIGHTTIP2="VIP3以上用户可无限使用"
LEVELFIGHTTIP3="无限制"
LEVELFIGHTTIP4="等级达到40级每天可使用60次跳过功能"
LEVELFIGHTTIP5="VIP3以上用户可无限制使用跳过功能"

---充值界面
TITLE_TOPUP={}
TITLE_TOPUP1="充值"
TITLE_TOPUP2="VIP功能"
TITLE_TOPUP3="充值礼包"
TITLE_TOPUP4="充值记录"
--充值礼包
GIFT_BAG_DESCRIBE="礼包描述"
GIFT_BAG_INFOMATION="礼包信息"
GIFT_BAG_GET="领取"
GIFT_CAN_GET="可领取"
GIFT_CANNOT_GET="不可领取"
GIFT_GETSUCCESS="领取成功"
YUAN="元"
TOP_TITLE="充值中心"
TOP_TIP=
[[
如何充值晶石（1元=10晶石）

1、通过手机充值
     在【充值中心】页面就可以直接进行移动卡/联通卡/电信卡和支付宝的充值。

2、通过电脑充值
     登录充值网站，选择“银行卡/支付宝充值”，再选择“游戏类型”→“天界”充值。本服务支持银行卡直充、电话卡和支付宝等多种方式。

]]

TOP_TIP91=[[充值规则：
91豆：晶石=1：10
即1个91豆等于10晶石
点击“马上充值”在输入框内填写您要充值的晶石数量进入91充值页面按照提示步骤即可完成！
]]

TOP_TIPCHUKONG=[[充值规则：
1 充值比例（1元=6.5晶石）
2 点击“立即充值”在输入框内输入你所要充值的人民币数额进入充值页面按照提示步骤即可完成。（提示：您输入的人民币数额必须为10的整数倍，否则无法充值成功！）
]]

TOP_TIP911=[[充值规则：
91豆：晶石=1：6.5
即1个91豆等于6.5晶石
点击“马上充值”在输入框内填写您要充值的金额进入91充值页面按照提示步骤即可完成！
]]

TOP_TIPNOCOMPUTERUC=
[[
如何充值晶石（1元=10晶石）
1、通过手机充值
在【充值中心】页面就可以直接进行移动卡/联通卡/电信卡的充值。
]]

PERSONRAL_CENTER="个人中心"
--VIP界面
VIP_TITLE="VIP功能"
VIP_MORE="更多VIP功能"
VIP_1="当前VIP等级:"
VIP_2="升到"
VIP_3="级还需充值:"
VIP_4="晶石"
VIP_5="可享受功能项目:"
VIP_CURRENTFUNCTION="当前等级可享受的项目: "
VIP_MAX="当前VIP等级已达到上限,后续将开放更多功能。"

VIP_MIAOSHU="  从开服当日起，只要玩家的角色是第一次在游戏里进行充值，不限制任何金额，除了能获得相应的晶石之外，还都将会获得一份“首次充值礼包”，礼包内容包含:首次充值额外10%晶石，二品武力药剂、魂技药剂、智力药剂各一瓶。"
VIP_CHUKONG="VIP规则"

VIP_LEVEL1="1、 每天可以进入金矿洞寻宝3次。\n2、 开启晶石增加竞技场挑战次数。\n3、 公会成员可以进行经典祈祷。"
VIP_LEVEL2="1、 开启每日任务刷新。\n2、 每天可以进入金矿洞寻宝5次。\n3、 开启第三个装备强化队列。\n4、 公会成员可以进行豪华祈祷。\n5、 修炼时间延长至24小时。\n6、 开启晶石增加竞技场挑战次数。"
VIP_LEVEL3="1、 角色培养增加白金培养，更容易获得高属性。\n2、 每天可以进入金矿洞寻宝7次。\n3、 开启晶石刷新神秘商店物品功能。\n4、 开启晶石重置英雄副本功能。\n5、 开启BOSS战浴火重生功能，将带来战斗力的加成。\n6、 开启BOSS战和领土战晶石鼓舞功能。\n7、 可以自动参加领土战。\n8、 可以消费晶石购买圣水进行庄园种植。\n9、 包含VIP1~VIP2所有内容。"
VIP_LEVEL4="1、 每日任务可使用晶石直接完成功能。\n2、 每天可以购买5次精力。\n3、 每天可以进入金矿洞寻宝9次。\n4、 装备强化同时可使用晶石消除冷却时间。\n5、 可使用晶石直接代替图纸合成时缺失的材料。\n6、 可以花费晶石升级庄园土地，升级后种植获得更高产量。\n7、 包含VIP1~VIP3所有内容。"
VIP_LEVEL5="1、 庄园种植可以一键刷新出满级种子。\n2、 每天可以进入金矿洞寻宝30次。\n3、 猎命可以使用晶石召唤“先知”，可以高概率获得高品质的命运水晶。\n4、 培养开启钻石级培养，更容易获得高属性。\n5、 可以直接使用晶石制作药剂，省去获取材料的麻烦。\n6、 每天可以使用晶石重置英雄副本2次。\n7、 包含VIP1~VIP4所有内容。"
VIP_LEVEL6="1、 每天可以购买20次精力。\n2、 角色开启至尊培养，高概率获得高属性。\n3、 包含VIP1~VIP5所有内容。"
VIP_LEVEL7="1、 每天可以进入金矿洞寻宝50次。\n2、 开启自动进入金矿洞功能，直至用完当天的可用次数。\n3、 副本扫荡精力不足时自动购买精力。\n4、 在人物药剂界面可以直接使用6种品质的药剂，省去合成带来的麻烦。\n5、 每天可以购买20次精力。\n6、 包含VIP1~VIP6所有内容。"
VIP_LEVEL8="1、 每天可以购买45次精力。\n2、 每天可以进入金矿洞寻宝50次。\n3、 包含VIP1~VIP7所有内容。"
VIP_LEVEL9="1、 每天可以进入金矿洞寻宝100次。\n2、 每天可以购买45次精力。\n3、 包含VIP1~VIP8所有内容。"
VIP_LEVEL10="1、 每天可以购买80次精力。\n2、 每天可以进入金矿洞寻宝200次。\n3、 包含VIP1~VIP9所有内容。"

HELP_TITEL="帮助"

--公会祈祷
PRAY_STATUESLV="祭坛等级"
PRAY_CURRAURA="祭坛当前经验"
PRAY_MAXAURA="祭坛升级经验"
PRAY_DO="祈祷"
PRAY_NORMAL="入门祈祷"
PRAY_SILVER="经典祈祷"
PRAY_GOLD="豪华祈祷"
PRAY_GAINOBTION="获得声望"
PRAY_GAINBLESSING="获得祝福"
PRAY_GAINAURA="获得灵气"
PRAY_USEEXPNUM="消耗阅历"
PRAY_USEGOLD="消耗晶石"
PRAY_TIMES="次"
PRAY_LEVEL="级"
PRAY_DONE="进行"
PRAY_GET=",祭坛"
PRAY_TIME="10分钟前"
PRAY_ANIMA="灵气"
PRAY_TITLE="公会祈祷"
PRAY_NOTPRAY="当前无祈祷信息"

--公会朝圣
HADJ_CALL="召唤"
HADJ_JOIN="加入"
HADJ_ISHELP="协助"
HADJ_CALL_UP="召集成员"
HADJ_ENOUGH="七星朝圣还需要公会成员：%s人"
HADJ_FOLLOWER="先知"
HADJ_NOTICE="公会成员满七人就可以完成朝圣。完成后每位参与成员将可以获得%s点声望。每天每个成员只能获得1次声望奖励。但是每天协助公会成员完成朝圣的次数没有限制。"
HADJ_TITLE="七星朝圣"
HADJ_CALLSUCCESS="召唤成功"
HADJ_SUCCESS="您成功进行七星朝圣，获得声望：+%s"
HADJ_SUCCESSNOADD="朝圣成功"
HADJ_MESSAGE = {}
HADJ_MESSAGE[1] = "发布召集信息成功"
HADJ_MESSAGE[2] = "您已朝圣过,协助朝圣无法获得声望"
-------主界面
LEASTHP="当前剩余绷带血量: %d"
LEASTBLESS="当前剩余祝福次数: %d"
LEASTBLESS="当前剩余祝福次数: %d"
LEASTDOUBLEBLESS="剩余副本双倍掉落材料次数%d次"
LEASTENERGY="当前精力: %d"
NOSTR="无"
LEASTCHANGSHAPE="当前剩余变身时间: %s分钟"

--排行榜
COUNTRY="国家"
RANK_VIPLEVEL="VIP等级"
RANK_ACTION="操作"
--
GUANGBO="【系统】：%s"
---gm反馈
GMCALLBACK="反馈"
NOFUNCTION="功能暂未开放"
TWOLIENCONTENT="一共要有十六个字算出十六个字占得高"
---

IDS_DANGLELOGIN="当乐通行证"
IDS_DOWNJOYNAME="当乐用户名"
IDS_NETDRAGON="论坛专区"
MENU_LIST="菜单列表"

----排行榜
RANK_LEVEL="等级"
RANK_FIGHTPOINT="战斗力"
RANK_POPULARITY="声望"
RANK_WEALTH="财富"
RANK_LIST="排名"
RANK_NAME="名称"
RANK_LEVEL="等级"
RANK_COUNTRY="国家"
RANK_VIPLEVEL="VIP等级"
RANK_OPERATION="操作"
RANK_NOTINRANK="您当前未进入排行,请继续努力!"
RANK_CURRENTLEVEL="您当前排行第%d名!"
VESION="当前版本: 1.58"

 --appstore msg
 IDS_APPSTORE_CLIENT_INVALID 		= "客户端无效"
 IDS_APPSTORE_PAYMENT_INVALID 		= "购买无效"
IDS_APPSTORE_PAYMENT_CENCELED		= "操作已取消"
 IDS_APPSTORE_PAYMENT_NOT_ALLOWED 	= "不允许购买"
 IDS_APPSTORE_UNKOWN_ERROR 			= "未知错误"
 IDS_APPSTORE_TRANSACTION_STATE_PURCHASING 	= "正在购买"
 IDS_APPSTORE_TRANSACTION_STATE_PURCHASED 	= "购买成功"
 IDS_APPSTORE_TRANSACTION_STATE_FAILED 		= "交易失败"
 IDS_APPSTORE_TRANSACTION_STATE_RESTORED 	= "已经在购买列表中"
 IDS_APPSTORE_REQUEST_ITEMS_EMPTY	= "商品列表失败"
 IDS_APPSTORE_REQUEST_DEVICE		= "设备不支持支付"
 IDS_APPSTORE_REQUEST_FAILED		= "请求连接失败"

 --装备封灵
ENCHANT_WEAPON 	= "武器"
ENCHANT_HORCURX 	= "魂器"
ENCHANT_CLOTHES 		= "衣服"
ENCHANT_NECKLACE 	= "项链"
ENCHANT_SHOULDERPAD = "护肩"
ENCHANT_SHOES 		= "鞋子"
ENCHANT_WASHING 	= "洗涤"
ENCHANT_JINGSHIWASHING = "晶石洗涤"
ENCHANT_ATTRIBUTE 	= "属性"
ENCHANT_WASHINGATTRIBUTE = "洗涤属性"
ENCHANT_LIMIT 		= "变动范围"
ENCHANT_LOCK 		= "锁定"
ENCHANT_UNLOCK 		= "解锁"
ENCHANT_STONE 		= "灵石"
ENCHANT_COAST 		= "本次消耗"
ENCHANT_LOCKNUM 	= "当前只能锁定%s个属性"
ENCHANT_UNACTIVED 	= "未激活"
ENCHANT_TITLE 		= "装备封灵"
ENCHANT_ACTIVED 		= "激活属性"
ENCHANT_FREETIME 	= "今日免费洗涤次数还剩余%s次"
ENCHANT_PRICE 		= "本次消耗"
ENCHANT_NOEQUIP 	= "当前部位没有装备"
ENCHANT_ASKSALE 		= "您确定要出售此物品吗？"

ENCHANT_SECONDLOCK		= "通关苍穹奇峰并且等级达到60级开启。"
ENCHANT_THIRDLOCK		= "通关开房宫殿并且等级达到70级开启。"
ENCHANT_FORTHLOCK		= "通关海贼之乱并且等级达到80级开启。"
ENCHANT_FIFTHLOCK		= "通关火神殿堂并且等级达到90级开启。"
ENCHANT_SIXTH			= "通关心灵幻境并且等级达到100级开启。"


--天地劫
TIANDI_STR 			= "天地劫"
TIANDI_TITLE 		= "天地劫轮回"
TIANDI_TIANDIJIE	= "天地劫之"
TIANDI_POSITION 	= "当前位置"
TIANDI_GATENUM 	= "第%s关"
TIANDI_MONSTER 	= "当前怪物"
TIANDI_REWARD 	= "挑战奖励"
TIANDI_START 		= "开始挑战"
TIANDI_REFRESH 	= "是否重置天地劫副本？"
TIANDI_RETURN 	= "回到上层"
TIANDI_CURREFRESH	= "刷新本层"

---赛跑游戏
RACE_STR="赛跑"
RACE_TITLE="宠物赛跑"
RACE_LANJIE="拦截"
RACE_DAY_RACETIMES="今日剩余可赛跑次数: "
RACE_DAY_STOPTIMES="今日剩余可拦截次数: "
RACE_DAY_HELPRACE="帮助好友赛跑次数: "
RACE_TIME="剩余时间"

RACE_CHOICE 	= "宠物选择"
RACE_TORTOISE = "乌龟"
RACE_RABBIT 	= "兔子"
RACE_GIRAFFE 	= "长颈鹿"
RACE_HORSE 	= "白马"
RACE_LION 		= "狮子"
RACE_ANIMAL 	= "当前比赛宠物"
RACE_TIME 		= "剩余时间"
RACE_MINUTE 	= "%s分钟"
RACE_PRICE 	= "赛跑奖励"
RACE_GET 		= "%s金币,%s声望"
RACE_FRIEND 	= "守护好友"
RACE_HELP 		= "邀请好友"
RACE_REFRESH  = "刷新宠物"
RACE_START 	= "开始比赛"
RACE_STOPLIST = "拦截列表"
RACE_STOPPRICE 	= "拦截奖励"
RACE_PROTECTFRIEND 	= "护送好友"
RACE_STOP 		= "您确定要拦截%s的宠物吗？"
RACE_OVER 		= "%s的宠物已完成赛跑，不可拦截。"
FRIEND_LIST="好友列表"
FRIEND_LEVEL="等级"
FRIEND_RANKINGNUM="竞技排名"
FRIEND_PASTPLOT="通关副本"
FRIEND_LASTLOGIN="最近登录"
NO_FRIEND="您暂时还未添加好友"
RACE_NAME="昵称"
RACE_STOPPLAYER="您确定要拦截玩家：%s吗？"
RACE_NOPLAYER="当前没有可拦截勇士!"
FRIEND_OPERATION="操作"
RACE_LEASETIME="剩余时间"




----星辰系统
STAR_TITLE = "星光普照"
STAR_RULETITLE = "规则说明"
STAR_RULE = "1. 每一张纸牌都显示6个不同星座图标。\n2. 其中只有天平星座图标可以带来好运。\n3. 六张纸牌抽到的天平纸牌越多可以获得的奖励越丰厚。\n4. 抽到三张以上就可以获得星辰点数。"
STAR_YUELI = "剩余阅历"
STAR_COUNT = "星辰点数"
STAR_TIME = "今日摇动次数"
STAT_CHANCE ="剩余免费更换次数"
STAR_RESULT = "最终结果"
STAR_NUM = "%s天平座"
STAR_PRICE = "获得奖励"
STAR_START = "摇动机关"
STAR_TOCHANE = "免费更换"
STAR_JINGSHI = "逆天更换"



TIP_PLEASECHOICEGOLD="请选择金额"

 ----
DAILY_TAST="系统公告"
MYINFOMATION="日常任务"
GOTOGET="前往"
DAILY_ENERGY="今日还可购买精力"
DAILY_GOLDHOLE="今日还可进入金矿洞"
DAILY_FENGLU="今日还可领取俸禄"
DAILY_GONGHUIQIDAO="今日还可公会祈祷"
DAILY_MINGYUNTIMES="今日还可免费猎命"
DAILY_ARENATIMES="今日还可竞技场挑战"
DAILY_ADVANTURETIMES="今日剩余探险"
DAILY_PLANTTIMES="当前剩余种植次数"
DAILY_RACETIMES="当前剩余赛跑次数"
TIMES="次"

NO_NOTICEMESSAGE="暂无公告内容"
ACTIVITI_ENRYGY="精力大作战"
ACTIVITI_MONEY="金币大作战"
ASKPARYINACTIVITY="%s活动已经开启了，是否前往参加？"

--晶石商城
MALL_TITLE				= "商城"
MALL_RECOMMEND	= "推荐"
MALL_DISCOUNT		= "打折促销"
MALL_TOTALPRICE		= "总价"
MALL_NOPROMOTION	= "当前无推荐商品"
MALL_NODISCOUNT	= "当前无打折促销商品"

--晶石商城道具
BAG_CORPSERENAME 	= "更改公会名称"
BAG_OLDNAME 			= "原名"
BAG_NEWNAME 		= "改名"
BAG_RENAME 			= "更改昵称"
BAG_RENAMESUCCESS	= "改名成功"
BAG_NOCORPSE			= "阁下未加入公会，无法使用该道具"
BAG_USESUCCESS		= "使用成功"
BAG_NONAME			= "请输入名称"
BAG_CHAIRMANUSE		= "您不是会长没有权限使用"

--表情道具展示
SHOW_NOVIP			= "VIP1级开启此功能"

 --公会晨练
 ANSWER_TITLE			= "公会晨练"
 ANSWER_JINSSHI		= "晶石答题"
 ANSWER_AUTOMATIC	= "自动答题"
 ANSWER_LEVEL			= "等级"

 --天下第一
BEST_TITLE				= "天下第一"
BEST_RULETITLE			= "规则说明"
BEST_REWARD			= "冠军奖励"
BEST_BET				= "下注"
BEST_MYRECORD		= "我的战绩"
BEST_GAMERULE		= "赛会规则"
BEST_MYBET			= "我的下注"
BEST_WORTH			= "身价排行"
BEST_MYRECORDLIST	= "我的战绩表"
BEST_SCREENINGS		= "场次"
BEST_OPPONENT		= "对手"
BEST_SCORE				= "比分"
BEST_LOOKRECORD		= "查看战绩"
BEST_VOTEDOFF			= "您已经被淘汰"
BEST_BETRECORD		= "下注详情"
BEST_SKYLIST			= "天榜"
BEST_MAN				= "人榜"
BEST_RANKING			= "%s强"
BEST_BETINFO			= "您已下注%s场，总额%s万金币，获利%s万金币。"
BEST_NOBET			= "您当前还未下注任何比赛。"
BEST_RANKLIST			= "身价榜"
BEST_ISAPPLY			= "已报名"
BEST_APPLYRULE		= "报名规则"
BEST_LOOK				= "查看报名"
BEST_HISTORY			= "历史战绩"
BEST_RANK				= "名次"
BEST_GROUP			= "组别"
BEST_TIMES				= "届数"
BEST_SERVER			= "服务器"
BEST_WIN				= "（胜利）"
BEST_ROUND			= "第%s局"
BEST_DEFEAT			= "战胜了"
BEST_FIGHTRECORD		= "战报"
BEST_NORECORD		= "当前没有您的相关战绩！"
BEST_PLAYER			= "玩家"
BEST_AREA				= "所在大区"
BEST_PLAYERWORTH	= "身价"
BEST_FIGHTOVER		= "战斗结束"
BEST_KING				= "冠军"
BEST_SECOND			= "亚军"
BEST_THIRD				= "季军"
BEST_SEMIFINALS		= "半决赛"
BEST_FINALS			= "决赛"

BEST_RULE = [[
1 开服后一周开启天下第一的报名，开服未满一周的服务器不参加该活动。
2 玩家通过完成主线任务“冰雪试炼1”开启天下第一功能。
3 比赛分为两个组别，40-59级玩家为人榜组，60级以上玩家为天榜组。
4 比赛采取淘汰赛的规则，先在本服务器内淘汰产生天榜，人榜各16强。最后淘汰产生赛区天榜，人榜32强。
5 每一次天下第一时间跨度为2周，一周为报名周，一周为比赛周。
6 比赛采用导入玩家数据的方法，在比赛周的每天11:30分导入玩家数据。参赛玩家需要及时调整好阵型和装备，保证最强战斗力！
7 玩家可以在赛区淘汰赛阶段进行下注。
8 玩家下注成功后将获得与下注金额对等的奖励，下注失败将扣除一半的下注金额。
9 比赛结束后，对于天榜，人榜前32名次的玩家将给予不同额度的奖励。
]]

BUFF_MOON = "战斗力加成剩余时间：%s分钟"

--传承系统
INHERIT_TITLE			= "传承"
INHERIT_CHOICE		= "选择"
INHERIT_NORMAL		= "普通传承"
INHERIT_JINGSHI		= "晶石传承"
INHERIT_TOP			= "至尊传承"
INHERIT_FREE			= "免费"
INHERIT_OLD			= "传承人"
INHERIT_NEW			= "被传承人"
INHERIT_SUCCESS		= "传承成功"
INHERIT_NOMAN		= "当前无可选择的佣兵"

FIRSTQUESTION = "第%s题"


--礼物系统
GIFT_TITLE		= "礼物"
GIFT_ADD		= "增加属性"
GIFT_NOW		= "当前好感度"
GIFT_NEXT		= "下一好感度"
GIFT_LEARN		= "学习技能"
GIFT_SKILLNAME	= "技能名称"
GIFT_SKILLNEED		= "需要好感度等级"
GIFT_INTRODUCE	= "技能介绍"
GIFT_LEVEL			= "好感度等级"
GIFT_LEVELUP		= "升级需要"
GIFT_LIKE			= "喜欢物品"
GIFT_JINGSHI		= "晶石赠送"
GIFT_GIVE			= "赠送礼物"
GIFT_CLEAR			= "清除饱食度"
GIFT_DEGREE		= "当前饱食度"
GIFT_SENDTIME		= "今日剩余晶石赠送次数"
GIFT_NOGIFT		= "您当前背包没有礼物，可到商城购买精品礼物宝箱！"
GIFT_TYPE1			= "食物"
GIFT_TYPE2			= "厨具"
GIFT_TYPE3			= "机械"
GIFT_TYPE4			= "书籍"
GIFT_TYPE5			= "乐器"
GIFT_NOWATER		= "您当前背包没有龙神圣水，可到商城购买！"

--公会攻城战
CITY_TITLE			= "公会攻城"
CITY_APPLYINFO		= "报名情况"
CITY_RULE			= "相关规则"
CITY_NAME			= "城市名称"
CITY_GET			= STAR_PRICE--"获得奖励"
CITY_APPLYTME		= "报名截止倒计时"
CITY_APPLY			= MERCENARY_APPLY--“报名
CITY_WINNER		= "获胜方"
CITY_WIN 			= ARENA_WIN--"战胜了"
CITY_APPLTITLE		= "公会攻城战报名"
CITY_OCCUPY		= "占领公会"
CITY_DAY			= "%s天"
CITY_CROPSENAME		= "公会名称"
CITY_CROPSELEVEL		= "公会等级"
CITY_CAPTAIN			= "会长名称"
CITY_APPLYCITY			= "报名城市"
CITY_SITUATION			= "战况"
CITY_SETFLAGE			= "设置旗帜"

--拉新卡
CARD_INPUT		= "输入卡号"
CARD_NUM			= "拉新卡号"

--法宝
HALIDOM_TITLE			= "法宝"
HALIDOM_EXP			= "法宝修炼"
HALIDOM_INFO			= "法宝信息"
HALIDOM_ATTRIBUTE	= "法宝属性"
HALIDOM_SIGNS		= "法宝属相"
HALIDOM_SKILL			= "法宝技能"
HALIDOM_MAKE		= "修炼"
HALIDOM_SKILLSTATUS	= "技能状态"
HALIDOM_CONDITION	= "开启条件"
HALIDOM_LIFE			= "寿命"
HALIDOM_COAST		= "升级需要消耗"
HALIDOM_SKILLLEVEL	= "技能等级"
HALIDOM_SKILLDESCRIBE = "技能描述"
HALIDOM_NOWLEVEL	= "当前技能等级"
HALIDOM_GOTO		= "前往"
HALIDOM_SKILLS		= "技能"
HALIDOM_ACCESSORY	= "附加属性"
HALIDOM_GROW		= "成长率"
HALIDOM_ADD			= "加成人物属性"
HALIDOM_LIFT			= "延寿"
HALIDOM_WASH		= "洗炼"
HALIDOM_NOTICE		= "完成以上任务可修炼法宝，修炼法宝之后可开启更多法宝功能。"
HALIDOM_NORMAL		= "普通"
HALIDOM_CHANGE		= "易相"
HALIDOM_EXPLAIN=[[说明：法宝总共4种属相，地，水，火，风。地克制水，水克制火，火克制风，风克制地。相应克制的属相战斗有伤害加成。
]]
HALIDOM_EARTH		= "地"
HALIDOM_WATER		= "水"
HALIDOM_WIND		= "风"
HALIDOM_FIRE			= "火"
HALIDOM_NONE		= "无"
HALIDOM_EMELENT		= "属相"
HALIDOM_RESTRAIN		= "克制属相"
HALIDOM_BERESTAIN	= "被克制属相"
HALIDOM_HURT			= "%s属相    伤害加成%s%%"
HALIDOM_FATELEVEL	= "当前祭祀等级"
HALIDOM_ATTRIBUTELEVEL	= "当前属性等级"
HALIDOM_NEXTFATE	= "下一等级祭祀消耗"
HALIDOM_NEXTCOST	= "升级需消耗"
HALIDOM_THING		= "物品"
HALIDOM_FATE			= "祭祀"
HALIDOM_LEARN		= "学习"
HALIDOM_COLLECT		= "收集"
HALIDOM_SUCCESSNUM= "成功率"
HALIDOM_GROWNUM	= "成长值"
HALIDOM_ORDINARY	= "普通"
HALIDOM_SURPERIOR	= "精良"
HALIDOM_GOOD		= "优秀"
HALIDOM_EXCELLENT	= "卓越"
HALIDOM_PERFECT		= "完美"
HALIDOM_NEED			= "法宝等级%s级"
HALIDOM_SKILLLV		= "当前技能等级"

--武器附魔
WEAPON_TITLE			= "武器附魔"
WEAPON_ONEKEY		= "一键合成"
WEAPON_STONE		= "魔晶"
WEAPON_TRAIN			= "附魔符培养"
WEAPON_BASENUM		= "基础属性值"
WEAPON_FINALNUM	= "最终值"
WEAPON_COST			= "（花费%s提升%s点成长值，成功率%s%%）"
WEAPON_NORMAL		= "普通培养"
WEAPON_JINGSHI		= "晶石培养"
WEAPON_TOP			= "至尊培养"
WEAPON_NOWEAPON	= "当前人物没有装备武器"
WEAPON_SALENOTICE	= "出售后附魔符将永久消失，是否确认出售？"

WEAPON_BOXOPEN		= "武器强化至%s开启（该凹槽可镶嵌%s附魔符）。"
WEAPON_BOXCONTENT	= ""

WEAPON_PUTIN			= "该凹槽可镶嵌%s附魔符："
WEAPON_GREEN		= "绿色"
WEAPON_BLUE			= "蓝色"
WEAPON_YELLOW		= "黄色"
WEAPON_COLOR		= "彩色"


--查看其它玩家
LOOK_CRYSTAL			= "水晶"
LOOK_SPAREPART		= "灵件"
LOOK_ENCHANT		= "附魔"

--1.55
--将星录
KNIGHT_TITLE			= "将星录"
KNIGHT_INVITE			= "完成以上任务可以邀请%s"
KNIGHT_GET			= "领奖"			
KNIGHT_RECRUIT		= "已招募"

--替补佣兵
REPLACE_NAME			= "替"
REPLACE_TITLE			= "替补佣兵"
REPLACE_NOTICE		= "该位置只能放置佣兵！"

--幸运转盘
ROLL_LUCKROTARY			= "幸运大转盘"
ROLL_STORAGEBOX			= "储物箱"
ROLL_LUCKGET				= "在幸运转盘中获得"
ROLL_ROLLTIME				= "本日剩余免费转盘次数"
ROLL_MYROLL				= "<label>我的宝藏</label>"
ROLL_RECORD				= "探宝记录"
ROLL_TIMES					= "抽奖%s次"
ROLL_GET					= "在幸运大转盘中获得"
ROLL_FREE					= "免费抽奖"

--1.58
--公会城市争夺战
CITY_SKILLNUM				= "技能点数"
CITY_FLAGENOTICE			= "请设置公会旗帜名称："
 --
 TIPFORVETION="您当前不是最新版本，请到官网下载最新客户端"
 TOUUP_TIPS="您输入的额度必须为10的倍数！"
TIP_TOPUP="充值成功后晶石将在1至2分钟内到账或者刷新页面！"

--VIP特权
VIP                                           = "VIP特权"
VIP_YUJU						="您当前是VIP%s再充值%s晶石享受VIP%s"


--
VIP1 = [[1.可购买VIP1礼包
2.强化出现暴击,随机提高装备等级2~3等级
]]

VIP2= [[1.可购买VIP2礼包
2.强化出现暴击,随机提高装备等级2~4等级
3.每次初始竞技场6次
4.最多可关注10人
]]

VIP3 =[[1.可购买VIP3礼包
2.强化可出现暴击，随机提高装备等级2-5级
3.精英，BOSS关卡可以使用晶石清除战斗次数
4.每日初始竞技场次数7次
5.最多可关注15人
6.好友数量增加至55人
7.每日可使用宝物许愿次数1次
]]
VIP4 =[[1.可购买VIP4礼包
2.强化可出现暴击，随机提高装备等级2-6级
3.每日初始竞技场次数8次
4.最多可关注20人
5.好友上限数量增加至60人
6.弟子受到点播后，最多有1个弟子获得额外经验
7.每日可以、使用宝物许愿次数2次
]]
VIP5 =[[1.可购买VIP5礼包
2.每日体力可使用普通回复次数5次
3.每日元气可使用普通回复次数5次 
4.每日初始竞技场次数9次 
5.最多可关注25人
6.好友上限数量增至65人
7.每日可使用宝物许愿3次 
8.包含VIP1-4所有功能
]]
VIP6 =[[1.可购买VIP6礼包
2.每日初始竞技场次数6次
3.最多可关注30人
4.好友上限数量增加至70人
5.弟子受到点播后，最多有2个弟子获得额外经验
6.每日可使用宝物许愿4次
7.包含VIP1-5所有功能
]]
VIP7 =[[1.可购买VIP7礼包
2.每日体力可使用高级回复次数15次
3.每日元气可使用高级回复次数15次
4.每日初始竞技场次数11次
5.最多可关注35人
6.好友上限增加至75人
7.弟子受到点播后，最多有3个弟子获得额外经验
8.每日可使用宝物许愿5次
9.包含VIP1-6所有功能
]]
VIP8 =[[1.可购买VIP8礼包
2.每日初始竞技场次数12次
3.最多可关注40人
4.好友数量上限增加至80人
5.弟子受到点播后，最多有4个弟子获得额外经验
6.每日可使用宝物许愿6次
7.包含VIP1-7所有功能
]]
VIP9 =[[1.可购买VIP9礼包
2.每日体力可使用至尊回复次数25次
3.每日元气可使用至尊回复次数25次
4.每日初始竞技场次数13次
5.最多可关注45人
6.好友上限数量增加至85人
7.弟子受到点播后，最多有5个弟子获得额外经验
8.每日可使用宝物许愿8次
9.包含VIP1-8所有功能
]]
VIP10 =[[1.可购买VIP10礼包
2.每日初始竞技场次数14次
3.最多可关注50人
4.好友数量上限增加至90人
5.弟子受到点播后，最多有6个弟子获得额外经验
6.每日可使用宝物许愿10次
7.包含VIP1-9所有功能
]]
VIP11 =[[1.可购买VIP11礼包
2.每日初始竞技场次数15次
3.最多可关注55人
4.好友上限增加到95人
5.每日可使用宝物许愿12次
6.包含VIP1-10所有功能
]]
VIP12 =[[1.可购买VIP12礼包
2.每日初始竞技场次数16次
3.最多可关注60人
4.好友上限增加至100人
5.每日可使用宝物许愿15次
6.包含VIP1-11所有功能
]]
VIPOver = "您当前的VIP等级为12级"
-----------假数据

doString = "是否支付?"
 doString_OK= "确定"

tobe  = "特价晶石:%s"
bgNumber = "原价晶石:%s"
PriceValue = "<label color='%s'>%s</label>"


goodsString = "您购买%s的数量"
Sum = "总价%s晶石"
CloseButton = "取消"
QueDingButton = "确定"

 Shopping ="购买"

spendMoney = "花费%s晶石清除冷却时间"
spendTime ="探险冷却中"

--------------------------------------

mailStyle = {"所有","战斗","好友","系统"}
businessStore = {"促销","道具","礼包"}
------------------------------------------------------------
--口袋天界
ACCOUNT				= "账号管理"

BASICE_TITLE			= "基础信息"
BASICE_NAME			= "勇士名称"
BASICE_LEVEL			= "等级"
BASICE_LEADER			= "领导力"
BASICE_INGROUP		= "上阵佣兵"
BASICE_NEXT			= "下一点体力恢复"

BASICE_ALL				= "全部体力恢复"
----------------------------------------------------------------
--庄园


Manor_grow_button1 =" 满级"
Manor_grow_button2 =" 刷新"
Manor_grow_button3 =" 种植"



Manor_grow_whiteTree =  "普通"
Manor_grow_greenTree = "优秀"
Manor_grow_blueTree =  "精良"
Manor_grow_purpleTree = "传奇"
Manor_grow_yellowTree =  "神话"

Manor_grow_stringTree =  "%s"




Manor_gorw_stringLevel = "佣兵等级:%s"
Manor_gorw_stringPlantType  = "种植品种:%s"
Manor_gorw_stringGrowExperience = "收获经验:%s"


----------------------------------------
Box = "获得物品:"
