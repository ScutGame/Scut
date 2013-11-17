-------------------------------------------------------
-- Language.lua
-- Author     : JunM Chen
-- Version    : 1.0.0.0
-- Date       : 2013-3-6
-- Description: 存放工程所需的中文字符串变量
------------------------------------------------------------------

local strModuleName = "Language";
module(strModuleName, package.seeall);
CCLuaLog("Module " .. strModuleName .. " loaded.");
strModuleName = nil;

-- 由于游戏显示的中文须采用UTF-8编码，\n因此将工程所有中文字符串都保存于本文件中，本\n文件采用无BOM的UTF-8编码格式。
-- 注意脑残Decoda不能支持本文件正常显示。其它常见编辑器均支持。
-- 同时任何人不得修改本文件的编码格式。
---
------------------------------------------------------------
GAME_NAME="口袋天界"

 -- char size test
 IDS_FONT_12_TEST			= "损兵折将的准准备。兵折将的准备为一名征战四方的君主，怎能没有一个响亮的名号备aaaa好aaafgfgfgadfasdfasd损兵折将的准备"
 
--通用
IDS_NONE="无"
IDS_LVSTR="Lv.%d"
IDS_EXP="经验"
IDS_GOLD="金币"
IDS_NOFUNCTION="功能暂未开放"
IDS_OPEN="开启"
IDS_COLSE="关闭"
IDS_SETING="设置"
IDS_INPUT="输入"
IDS_OPEN1="打开"
IDS_JINGSHI="晶石"
IDS_LEVEL="等级"
IDS_LEV="级"
IDS_BACK="返回"
IDS_CANCEL="取消"
IDS_SURE="确定"
IDS_REFRESH="刷新"
IDS_CHECK="查看"
IDS_NUM="数量"
IDS_USE="使用"
IDS_DELETE="删除"
IDS_NEXT="下一步"
IDS_LEVELUP="升级"
IDS_QUIT="退出"
IDS_QUITGAME="退出游戏"
TIP_TIP="提示"
TIP_STOP="停止"
TIP_FUNCTION="功能即将开放，尽请期待！"
TIP_YES="是"
TIP_NO="否"
IDS_SPRITE="精力"
IDS_CLOSED_APP="是否退出游戏？"
IDS_MESSAGE1 = "您的装备强化已经重新开启，时间不等人，赶紧进入天界中继续奋战吧！"
IDS_MESSAGE2 = "您的魔术强化已经重新开启，时间不等人，赶紧进入天界中继续奋战吧！"
NIN="您"
TIP_NONET="网络连接失败"
TIP_NONET1="网络连接失败,请重新登录游戏"
IDS_RETRY="重试"
IDS_TIMES="次"
IDS_GOON="继续"
IDS_OK="确定"
IDS_COMMA="，"
IDS_COLON="："

IDS_DANGLELOGIN="当乐通行证"
IDS_DOWNJOYNAME="当乐用户名"
IDS_NETDRAGON="论坛专区"
MENU_LIST="菜单列表"
IDS_PRICE="奖励"

--gm
GM_TITLE = "GM命令"
GM_MESSAGE = "请输入命令:"
GM_MESSAGE2 = "请求GM命令为空，发送失败"


--口袋天界
ACCOUNT				= "账号管理"


--登录
LOGIN_CAREER1="杀破狼"--"格斗家"
LOGIN_CAREER2="暮月"--"神枪手"
LOGIN_CAREER3="苍天穹"--"鬼剑士"
LOGIN_CAREER4="卡卡西"--"圣骑士"
LOGIN_CAREER5="安娜苏"--"魔法师"


LOGIN_DES1="高格挡，拳法缤纷，干劲威猛"
LOGIN_DES2="高暴击，枪法精准，杀人无形"
LOGIN_DES3="高闪避，挥剑如虹，轻巧如灵"
LOGIN_DES4="高血量，英勇无敌，救死扶伤"
LOGIN_DES5="高伤害，魔力无穷，杀人千里"
LOGIN_TITLE1="选择初始英雄"
LOGIN_TITLE2="请输入昵称"
LOGIN_IN="进入游戏"

--------------登陆
LAN_Z="账号:"
LAN_PASSWOED="密码:"
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
LAN_START_GAME="开始游戏"
LAN_ID_MANAGE="账号管理"
LAN_ID_NOT_PASSWOED="未输入密码"
LAN_ID_NOT_Z="请输入账号和密码"
LAN_NOT_BU_PASSWORD="未输入密码！"
LAN_PLEASE_PASSWORD="请再次输入密码！"
LAN_BUG_PASSWORD="两次输入密码不同！"
LAN_PASSWORD_OK="密码修改成功"
LAN_ONT_NAME="请输入玩家姓名！"

---佣兵基本系统
BASICE_TITLE			= "基础信息"
BASICE_NAME			= "会长名称"
BASICE_LEVEL			= "等级"
BASICE_LEADER			= "领导力"
BASICE_INGROUP		= "上阵佣兵"
BASICE_NEXT			= "下一点精力恢复"
BASICE_ALL				= "全部精力恢复"
BASICE_HENOUR		= "荣誉"
BASICE_YUELI			= "阅历"
BASICE_DAY			= "%s天"

--口袋天界副本字段
PLOT_CITYNAME="德兰亚"
PLOT_PHYTIP="精力消耗"
PLOT_ADVISELV="建议等级"
PLOT_WINREWARD="胜利奖励"
PLOT_GOTO="前往"
PLOT_TIP2="每日挑战次数"
PLOT_TIP3="年轻人，闯荡江湖也要记得修身养性啊，我看你身子很虚弱了。吃点精力药剂补补吧！能恢复8点精力哦."
PLOT_TIP1="背包已满是否进行整理？"
PLOT_TIP4="玩竞技场去"
PLOT_TIP5="恢复精力"
PLOT_TIP6="精力药剂"
PLOT_TIP7="今日已服用%d瓶，（当前每天最多使用%d瓶）"
PLOT_JUMP="跳过"
PLOT_WIN="战斗胜利"
PLOT_FALSE="战斗失败"
PLOT_HONOUR="荣誉"
PLOT_PASSNUM=""
PLOT_COAST="您当天最大挑战次数已用完，是否花费%s晶石增加一次挑战次数（VIP3开启）?"
PLOT_ROUND="当前战斗"
PLOT_TIP8="您当前最大挑战次数已用完，VIP3级可用晶石增加挑战次数！是否立即充值？"
PLOT_ASCENDED="职业进阶书"
PLOT_ELITE="精英副本"
PLOT_NOMAL="普通副本"
PLOT_SEEDROPOUT="查看掉落"
PLOT_TIP9="您当前最大挑战次数已用完"


---游戏设置
GAME_SET="设置"
SET_MUSIC="音乐"
SET_ACTIVITY="活动"
SET_CHATTIP="聊天提示"
SET_HELP="帮助"
SET_NOTICE="设置通知"
SET_CDKEY="输入兑换礼品CD-KEY"
SET_MAXPHY="体力满"
SET_BEATTACK="被攻击"
SET_NOTICE1="提醒全部"
SET_NOTICE2="每天一次"
SET_NOTICE3="每周一次"
SET_NOTICE4="不提醒"
SET_INPUTTITLE="请勇士输入需要兑换的CD-KEY:"
SET_CDTILE="兑换CD-KEY"
SET_ACCOUNT= "账号设置"
SET_PERSONAL="个人中心"
SET_FEEDBACK="意见反馈"


---聊天
CHAT_TITLE="聊天"
CHAT_SEND="发言"
CHAT_TITLETIP="聊天（最多60个字符）"
----------------聊天
CHAT_TITLE_={}
CHAT_TITLE1="综合"
CHAT_TITLE2="世界"
CHAT_TITLE3="公会"
CHAT_TITLE4="私聊"
CHAT_TITLE5="公告"
CHAT_TITLE6="系统"
CHAT_LEFT="【"
CHAT_RIGHT="】"
CHAT_SURE="确定"
CHAT_SAY="说:"
CHAT_ME="我"
CHAT_FOR="对"
CHAT_TIPS="发言内容不能为空！"
CHAT_GOODNUM="（拥有道具数量）"


----------竞技场
COMPETI_TITLE="竞技场"
COMPETI_ATTACK="挑战"
COMPETI_GETREWARD="领奖"
COMPETI_TIP="击败所有对手，让他们颤抖去吧！"
COMPETI_TIP1="排名越靠前，积分奖励越多！"
COMPETI_RANK="当前排名"
COMPETI_TIMES="今日剩余挑战次数"
COMPETI_SORCE="积分"
COMPETI_SOIDIER="上阵佣兵"
COMPETI_CURRENTSORCE="当前积分"
COMPETI_CHANGE="兑换积分"
COMPETI_GET="兑换"
COMPETI_GETSORCE="保持此排名每天获得%d积分"
COMPETI_RETIP="竞技积分每天刷新一次，每天0点发放竞技排名积分！"


--人物信息
ROLE_NAME="姓名"
ROLE_LEVEL="等级"
ROLE_INHERIT="传承"
ROLE_TRAIN="培养"
ROLE_SOULSKILL="魂技"
ROLE_LIFE="生命"
ROLE_POWER="力量"
ROLE_SOULPOWER="魂力"
ROLE_INTELLECT="智力"
ROLE_POTENTIAL="潜力"
ROLE_CUREXP="当前经验"
ROLE_MAXEXP="升级经验"
ROLE_TIP1="普通培养"
ROLE_TIP2="晶石培养"
ROLE_TIP3="白金培养"
ROLE_TIP4="至尊培养"
ROLE_TIP5="请选择默认培养次数"
ROLE_TIP6="当前默认培养次数:%d次"
ROLE_TIP7="原属性"
ROLE_TIP8="新属性"
ROLE_TIP9="（晶石*%d）"
ROLE_TIP10="（培养药剂*%d）"


ROLE_QUALITY_I="白色"
ROLE_QUALITY_II="绿色"
ROLE_QUALITY_III="蓝色"
ROLE_QUALITY_IV="紫色"
ROLE_QUALITY_V="黄色"
ROLE_QUALITY_VI="橙色"


ROLE_COLOR_I="白"
ROLE_COLOR_II="绿"
ROLE_COLOR_III="蓝"
ROLE_COLOR_IV="紫"
ROLE_COLOR_V="黄"
ROLE_COLOR_VI="橙"

USERHP="生命: "
LILIANG="力量: "
ZHILI="智力: "
HUILI="魂力: "
WULIGONGJI="物理攻击: "
HUNJIGONGJI="魂技攻击: "
FASHUGONGJI="魔法攻击: "
HUNJIFANGYU="魂技防御: "
FASHUFANGYU="魔法防御: "
GERENXIANGONG="佣兵先攻: "
WULIFANGYU="物理防御: "
BAOJI="暴击: "
MINGZHONG="命中: "
POJI="破击: "
BISHA="必杀: "
RENXING="韧性: "
SHANBI="闪避: "
GEDANG="格挡: "


ROLE_EQUIPCHOICE="装备选择"
ROLE_NOEQUIP="没有可选择装备"
ROLE_LVUP="升级"
ROLE_SOULNOTICE="您当前已拥有该佣兵，将自动转换为灵魂获得灵魂数量："
ROLE_CHANGEEQUIP="更换装备"
ROLE_CHANGESKILL="更换魂技"
ROLE_SKILLLVUP="魂技升级"
ROLE_EQUIPSTRONG="装备强化"
ROLE_NOEXPCARD="您当前没有佣兵经验卡片"
ROLE_NOCHOICEHERO="您当前没有可选择佣兵"
ROLE_NOSOULCARD="您当前没有灵魂碎片"
ROLE_NOHEROCARD="您当前没有佣兵"
ROLE_POINTCHOICE="点击选择佣兵"
ROLE_SOUL="灵魂"
ROLE_CARDLIST="经验卡列表"
ROLE_SKILLNOTICE="该位置后续将安排开放，尽请期待！"
ROLE_SKILLCHOICE="魂技选择"
ROLE_NOSKILL="没有可选的魂技"
ROLE_UPEXP="升级经验"
ROLE_LVMAX="(已达到最级)"

HERO_NAME="佣兵名称"
HERO_CURRLV="当前等级"
HERO_NEXTLV="可提升等级至"
HERO_SHANGXIAN="已达到等级上限"
HERO_TISHI="选择的经验卡已超出当前等级上限，超出部分佣兵无法获得，是否确认升级？"
HERO_GETEXP="获得经验"
HERO_LIFENUM="生命"
HERO_PLEASECHOICE="请先选择佣兵卡片"
HERO_GROUPNUM="上阵佣兵数量"
HERO_GOLDPRICE="奖励晶石"
HERO_GOODGET="给力升级"

HERO_CAREER_I="格"
HERO_CAREER_II="枪"
HERO_CAREER_III="剑"
HERO_CAREER_IV="骑"
HERO_CAREER_V="魔"

--背包系统


BAG_TITLE="背包"
BAG_PROPS="道具"
BAG_MATERIAL="材料"
BAG_PAPER="图纸"
BAG_INTRO="简介"
BAG_MORE="扩充"
BAG_DETAIL="物品信息"
BAG_TIP1="还未拥有此类物品"
BAG_TIP2="是否花费%d晶石开启位置？"
BAG_TIP3="晶石不足是否前往充值？"
BAG_TIP4="出售成功"
BAG_TIP5="请输入想要修改的昵称："
BAG_TIP6="修改成功！"
BAG_TIP7="您出售的物品为贵重物品，是否确定出售？"
BAG_TIP8="您当前已经拥有该佣兵，使用后会提升佣兵一个突破等级，无法获得佣兵，确定使用吗？"
BAG_TIP9="获得"
BAG_TIP10="获得潜能点"
BAG_TIP11="出售成功,获得%d金币"
BAG_TIP12="%s钥匙不足！"
BAG_PRICE="单价"

BAG_TIP="当前容量:%d/%d"
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
--充值系统
TOPUPNOW="立即充值"
TOPUP_VIPLOOK="查看VIP特权"
TOPUP="充值"
TOPUP_VIP="当前VIP等级"
TOPUP_DETAIL="充值记录"
TOPUP_TIP="充值额度越高越实惠！"
TOPUP_TIP1="充值100晶石即可享受"
TOPUP_TIP2="升级到下一个VIP等级还需充值：%d晶石!"
TOPUP_TIP3="请输入整数！"
TOPUP_FIRST="首次充值奖励"
TIP_PLEASEINPUT="请输入数量："
TOPUP_TIP4="请输入十的倍数"
TIP_TOPUP="充值成功后元宝将在1至2分钟内到账或者刷新页面！"
TIP_PLEASECHOICEGOLD="请选择金额"
IDS_NETDRAGON="论坛专区"
TOPUP_INFO=[[
1.如何充值晶石？（1元=10晶石）
在【充值中心】页面就可以直接进行移动卡/联通卡/电信卡和支付宝的充值。
2.通过电脑充值
登录充值网站，选择“银行卡/支付宝充值”，再选择“游戏类型”→“口袋天界”充值，本服务支持银行卡直充、电话卡和支付宝等多种方式。
]]


TOP_TIP91=[[
充值规则：
91豆：晶石=1：10
即1个91豆等于10晶石
点击“立即充值”在输入框内填写您要充值的晶石数量进入91充值页面按照提示步骤即可完成！
]]

TOP_TIPCHUKONG=[[充值规则：
1 充值比例（1元=6.5晶）
2 点击“立即充值”在输入框内输入你所要充值的人民币数额进入充值页面按照提示步骤即可完成。（提示：您输入的人民币数额必须为10的整数倍，否则无法充值成功！）
]]

TOP_TIPNOCOMPUTERUC=
[[
如何充值晶石（1元=10晶石）
1、通过手机充值
在【充值中心】页面就可以直接进行移动卡/联通卡/电信卡的充值。
]]

TOP_TIPNOCOMPUTERQIHU=
[[
如何充值晶石（1元=10晶石）
在【支付中心】页面就可以进行支付宝/银联/充值卡/骏网卡/360币/储蓄卡的充值
]]


----信件
MAILL_ALL="所有"
MAILL_FIGHT="战斗"
MAILL_FRIEND="好友"
MAILL_SYSTEM="系统"
MAILL_ATTACK="反击"
MAILL_NONE="尊敬的会长，暂时没有收到伊妹儿！"
MAILL_FIGHTSTR = "反击"
MAILL_HUIFANG="回放"
MAILL_FRIENDSTR ="留言"
MAILL_SYSTEMSTR = "前往"
MAILL_LEAVESTRING = "给%s留言"
MAILL_GIVESTRING =  "请输入您的留言！"
MAILL_FASONG = "发送"
MAILL_goLetterStr = "发送成功！"
MAILL_AGREE= "同意"
MAILL_NOAGREE= "不同意"
---集邮系统
COLLECTION_SOLDIER="佣兵"
COLLECTION_EQUIP="装备"
COLLECTION_SKILL="魂技"

--好友系统
FIREND_TITLE="好友"
FIREND_ENEMY="仇敌"
FIREND_CARE="关注"
FIREND_ADD="加好友"
FIREND_MESSAGE="留言"
FIREND_MESSAGE1="给%s留言"
FIREND_DELETET="删除成功！"
FIREND_NONE1="您还未添加好友！"
FIREND_NONE2="您还没有仇敌！"
FIREND_NONE3="您还未关注好友！"
FIREND_NONE4="没有可添加的好友！"
FIREND_INVITE="邀请"
FIREND_FIGHT="竞技"
FIREND_FIGHT1="竞技场仇敌"
FIREND_FIGHT2="该玩家为您的仇敌，添加好友后将解除仇敌关系，确定要添加好友吗？"
FIREND_CANVEL2="取消仇敌"
FIREND_CANVEL3="取消关注"


--合成
PROP_TITLE="道具合成"
PROP_TIP="合成预览"
PROP_NAME="道具名称"
PROP_CHARTER="道具属性"
PROP_MAKE="制作"
PROP_ENOUGH="材料不足,vip4级开启晶石合成"
PROP_ENOUGHNOVIP="材料不足"


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

--装备
EQUIP_ALL="全部"
EQUIP_WEA="武器"
EQUIP_CLO="服饰"
EQUIP_GUS="魂器"
EQUIP_TIP="强化装备可提升佣兵血量,攻防属性"
EQUIP_SELL="出售"
EQUIP_PIN="品质"
EQUIP_PRICE="售价"
EQUIP_STREN="强化"
EQUIP_AFTER="强化预览"
EQUIP_WU="物品信息"
EQUIP_NEED="需求"
EQUIP_YICI="单次强化"
EQUIP_YICI_1="强化一次"
EQUIP_SHICI="十次强化"
EQUIP_SHICI_1="强化十次"
EQUIP_EQUIPMAN="装备于%s"
EQUIP_SELLTILE="出售装备"
EQUIP_SELLTIP="请选择要出售的装备"
EQUIP_NONE="暂无可出售的装备"
EQUIP_QUALITY1="白板"
EQUIP_QUALITY2="蓝色"
EQUIP_QUALITY3="紫色"
EQUIP_QUALITY4="橙色"
EQUIP_QUALITY5="金色"
EQUIP_STRENTIP="强化成功!"
EQUIP_STRENTIP1="您暂时还没有装备!"
EQUIP_TIP2="已强化至%d级!"
EQUIP_TIP3="出售装备成功，获得%d金币!"



--猎命
HUNT_MER="合并闲置"
HUNT_ING="前往猎命"
HUNT_EXP = "命格经验"
HUNT_FUSION = "合成"
HUNT_FUSIONTIP = "合成成功!"
HUNT_UNLOAD = "卸下"
HUNT_TIMES = "今日剩余免费次数"
HUNT_TITLE= "猎命"
HUNT_AUTO= "一键猎命"
HUNT_AUTOSELL= "一键卖出"
HUNT_MONEY= "晶石猎命"
HUNT_AUTOG= "一键拾取"
HUNT_SELLTIP= "获得"
HUNT_GETTIP= "拾取成功"
HUNT_SELL= "卖出"
HUNT_GET= "拾取"
LIFECELL_COMBINEMSG="是否确认合并背包中的所有闲置命运?"
LIFECELL_TIP3 = "是否花费100晶石进行猎命?"
LIFECELL_TIP4 = "格子暂未开启！"
LIFECELL_TIP5 = "此命格位置在佣兵等级%d级时候开启！"
LIFECELL_TIP6 = "更换佣兵"
LIFECELL_TIP7= "暂无可用的命运水晶!"
HUNT_VIPLIMIT= "尊敬的会长，VIP5才可以晶石猎命哦！"
HUNT_ONEGETLIMIT= "尊敬的会长，VIP5才可以一键猎命哦！"
HUNT_PRICE="价格"
HUNT_OPEN="此命格位置在佣兵等级%s级时候开启！"
HUNT_ASKSELL="确定要卖出%s水晶吗？"
HUNT_WARMING="当前人物未点亮！"

--培养
TRAIN_HEROTRAIN = "佣兵培养"
TRAIN_NOTICE="消耗培养药剂或晶石调整佣兵属性，可以将佣兵潜能点变为属性!"
TRAIN_QUALITY="品质"
TRAIN_SETTIME="设置次数"
TRAIN_COUNT="潜能点"
TRAIN_DRUG="培养药剂"
TRAIN_EXIT="返回"
TRAIN_TITLE="培养"
TRAIN_SUCCESS="培养成功"
TRAIN_TIME="次数(%s)按钮"

--酒馆
PUB_CAREER="职业"
PUB_LEVEL="初始等级"
PUB_LIFE="初始生命"
PUB_POWER="初始力量"
PUB_SOUL="初始魂力"
PUB_INTELLECT="初始智力"
PUB_ABILITY="天赋魂技"
PUB_ABILITYDES="魂技说明"
PUB_REQUIREMENT="招募条件"
PUB_REPUTATION="声望"
PUB_CURRGOIN="当前金币"
PUB_CURRREP="当前声望"
PUB_GROUP="队伍人数"
PUB_RECRIUIT="招募"
PUB_BACKTEAM="归队"
PUB_INITIAL="初始"
PUB_GROW="成长"

HOTAL_DESC="可发现的佣兵品质"
HOTAL_FREE="后免费"
HOTAL_FREETIME="今日免费次数"

PUB_INITIAL="初始"
PUB_GROW="成长"

--传承
INHERIT_TITLE="传承"
INHERIT_CHOICE="请选择传承佣兵"
INHERIT_DISAPPEAR="佣兵传承之后会消失"
INHERIT_RESULT="传承结果"
INHERIT_UP="等级提升至"
INHERIT_ISAWAY="消失"
INHERIT_NOTICE="将佣兵的经验传承给另一个佣兵，使其快速升级\r\n传承的佣兵等级至少要大于被传承佣兵3级以上"
INHERIT_QUALITY="品质"
INHERIT_NORMAL="普通传承（传承宝珠*1）"
INHERIT_TOP="至尊传承"
INHERIT_NOCHOICE="当前没有可传承的佣兵"


--魂技升级
SKILL_LEVELUP="魂技升级"
SKILL_COST="消耗魂技"
SKILL_CHOICE="请选择消耗魂技"
SKILL_ADD="增加经验值"
SKILL_LIST="魂技列表"
SKILL_EQUIP="装备于"
SKILL_TYPE_I="单体"
SKILL_TYPE_II="纵向攻击"
SKILL_TYPE_III="横向攻击"
SKILL_TYPE_IV="全体攻击"
SKILL_NOSKILL="当前没有魂技"
SKILL_EXP="经验值"
SKILL_PROWER = "强度"
SKILL_DETAIL="技能信息"
SKILL_ADDEXP="吞噬成功，获得%s经验值！"
SKILL_FULL="会长，当前最多可以放置5张魂技卡片！"
SKILL_UP="吞噬成功，获得%s经验值，恭喜您的魂技成功升级到%s级！"

--副本评价
ASSESS_TITLE="副本评价"

--佣兵卡片背包
BAG_HERO="佣兵列表"
BAG_SOUL="碎片"
BAG_QUALITY="品质"
BAG_DES="简介"
BAG_LACK="差%s可招募"
BAG_CURRNUM="当前数量"
BAG_EXP="经验"
BAG_INGROUP="已上阵"
BAG_RECRUITSUCCESS="招募成功"
BAG_BREAK="突破"
BAG_BREAKNUM="%s突破成功，获得%s点潜能点"
BAG_BREAKLACK="差%s可突破"

--公告界面
NOTICE_GOTO="立即前往"


----------------祈祷界面
Pray_stirng ="哆啦A梦的祝福" 
Pray_stirng2 = "祈祷%s天,我的百变兜兜"
Pray_stirng3 = "奖励丰盛哦!"

Pray_Num = " 已祈祷了%s/%s次"
Pray_jiaoyou = "坚持，坚持呀，多啦A梦的口袋等着你，坚持就是胜利！"
Pray_huojiang = "恭喜你完成了祈祷，获得多啦A梦的奖励：%s"

---------------   开宝箱
start_string = "开始"
close_string = "关闭"


------------------- 庄园佣兵
Ranger_Img = "Figure_1004_1.png"

------------------商城界面
businessStore1 = "促销"
businessStore2 = "道具"
businessStore3 = "礼包"

--广播
NOTICE_NAME = "【%s】"
NOTICE_SYSTEM="系统"


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
EMBATTLE_MESSAGE1= "已启用"
EMBATTLE_MESSAGE2= "会长等级达到%s级开启该阵法"
EMBATTLE_OPENSUC= "启用成功"
EMBATTLE_SAVESUC= "保存成功"
REPLACE_NOTICE		= "该位置只能放置佣兵！"

---------商城
businessStirng =  {"道具","礼包","促销"}
businessStirng_menu = "您购买%s的数量"
businessStirng_czString = "您当前晶石不足，购买失败!是否立即充值?"

SHOP_PROP="道具"
SHOP_GIFTBAG="礼包"
SHOP_HOTAL="酒馆"
okString = "是"
noString = "否"



----首充返利
TopUpRebate_TitleStr = "充值额度越高,越实惠" 
TopUpRebate_ButtonStr = "查看VIP权利" 
TopUpRebate_ListStr = "充值%s人民币="
TopUpRebate_ListStr2 = "%s 送 %s"
---升级有惊喜
UpGrade_Str1 = "<label>亲爱的会长，您在游戏中达到指定的等级时可以领取晶石,并享受VIP待遇哦</label>" 
UpGrade_VIP_Str = "<label>会长等级达到%s级送%s晶石享受%s待遇</label>"
UpGrade_Str1NOVIP = "<label>亲爱的会长，您在游戏中达到指定的等级时可以领取晶石哦</label>" 
UpGrade_VIP_StrNOVIP = "<label>会长等级达到%s级送%s晶石</label>"
UpGrade_Str = "<label>会长等级达到%s级送%s晶石</label>"
UpGrade_ButtonStr = "领取"
--VIP特权
VIP                                           = "VIP特权"
VIP_YUJU						="您当前是"
VIP_Str = "再充值%s晶石可享受"
VIPOver = "您当前的VIP等级为10级"
VIP_OStr = "你需要充值%s晶石就可享受"
--
VIP1 = [[1.每日进入金矿洞3次
2.强化可以出现暴击，随机提高装备等级1-2级
]]

VIP2= [[1.每日进入金矿洞5次
2.强化可出现暴击，随机提高装备等级1-3级
]]

VIP3 =[[1.角色培养增加白金级培养：更容易洗出高属性
2.每日进入金矿洞7次
3.强化可出现暴击，随机提高装备等级1-4级
4.可以消耗晶石购买圣水进行庄园种植
]]
VIP4 =[[1.每日可以使用精力药剂3次
2.每日进入金矿洞9次
3.庄园土地升级：玩家可消费晶石对庄园土地进行升级，升级后的土地，将获得更多的收益
4.装备强化出现暴击，随机提高装备2-5级
]]
VIP5 =[[1.每日进入金矿洞20次
2.每日可以使用精力药剂5次
3.包含VIP1-4功能
4.增加一键猎命功能
5.增加晶石猎命，晶石直接召唤第四个NPC
6.角色培养增加至尊培养
]]
VIP6 =[[1.每日可进入金矿洞30次
2.每日可以使用精力药剂10次
3.包含VIP1-5所有功能
]]
VIP7 =[[1.每日可进入金矿洞40次
2.每日可以使用精力药剂15次
3.包含VIP1-6内容
]]
VIP8 =[[1.每日可以进入金矿洞50次
2.每日可以使用精力药剂20次
3.包含VIP1-7所有内容
]]
VIP9 =[[1.每日可以使用精力药剂25次
2.包含VIP1-8内容
]]
VIP10 =[[1.每日可以使用精力药剂30次
2.包含VIP1-9内容
]] 

-----------假数据
doString = "是否支付?"
doString_OK= "确定"
tobe  = "特价:%s"
bgNumber = "价格:%s"
PriceValue = "<label color='%s'>%s</label>"
goodsString = "您购买\"%s\"的数量"
Sum = "总价%s晶石"
CloseButton = "取消"
QueDingButton = "确定"
Shopping ="购买"
spendMoney = "花费%s晶石清除冷却时间"
spendTime ="探险冷却中"
Business_Str = "您的晶石不足，购买失败！是否立即充值"
Business_KONGBAI = "当前没有可出售的物品"
Shoped = " 购买成功！"

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

ACTIVE_NOACTIVE="当前没有可参加的活动"
----------------------------------------
Box = "获得物品:"

--魔术界面
MAGIC_TITLE="奇术"
MAGIC_GONGFA="功法"
MAGIC_ZHENFA="阵法"
MAGIC_NAME="名称"
MAGIC_LIFEVALUE="生命值"
MAGIC_NORMALATTACK="普通攻击"
MAGIC_SPELLATTACK="法术攻击"
MAGIC_NORMALDEFENSE="普通防御"
MAGIC_STUNTATTACK="绝技攻击"
MAGIC_APELLDEFENSE="法术防御"
MAGIC_STUNTDEFENSE="绝技防御"
MAGIC_LEVEL="%d级"
MAGIC_FUNCDESCG="功法说明"
MAGIC_FUNCDESCZ="阵法说明"
MAGIC_CONSUMPEXP="消耗阅历"
MAGIC_COOLINGTIME="冷却时间："
MAGIC_LEVNEEDS="等级需求"
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
MAGIC_ISCANUPMESSAGE = "当前可以升级魔术"
MAGIC_OPEN="会长等级达到%s级开启该阵法"
MAGIC_UNOPEN="未开启"

-----------送精力

SEND_LOVE_ONE ="中午(12点)"
SEND_LOVE_TO ="下午(6点)"


-------每日探险
TODAY_Str  = "今天题目已答完"
TODAY_Str2 = "恭喜获得"
TODAY_Str1 = "%s*%s "


toLabel ={
[1]="金币",
[2]=" 声望",
[3]="阅历",
[4]=" 精力",
[5]=" 经验",
[6]="晶石",

[7]="命运水晶品质概率奖励",
[8]="命运水晶",[10]="灵件",
[9]=" 附魔符",
[10]="再来一次",
[11]="心情愉快",
[12]="充值返还",
}


---------------公告
 Public_String = "暂无公告"
 Public_MaxPage = "当前是最后一页"
 Public_SmallPage = "当前是第一页"
 Public_ButtonStr = "立即前往"

--战斗奖励
REWARD_THING="战利品"
REWARD_EVALUATE="评价"
REWARD_ROUND="战斗回合"
REWARD_PRICE="人物奖励"
REWARD_AGAIN="再来一次"
REWARD_HONOUR="荣誉值"
REWARD_YUELI="阅历"
REWARD_NAME="副本名称"
HUODEJINBI="获得金币"
HUODEYUELI="获得阅历"
HUODERYZ="获得荣誉值"
HUODEJINGSHI="获得晶石"




DAILY_REWARDTYPE_={}
DAILY_REWARDTYPE_[1]="金币"
DAILY_REWARDTYPE_[2]="声望"
DAILY_REWARDTYPE_[3]="阅历"
DAILY_REWARDTYPE_[4]="精力"
DAILY_REWARDTYPE_[5]="经验"
DAILY_REWARDTYPE_[6]="晶石"



-----招募送好礼
RecruitScene_buttonStr = "立即前往"

--假数据
RecruitScene_Str = "[活动时间]：6月15日"
Active_ContentStr = "[活动内容]：%s"

-----商城打折促销
Active_Date  = "[活动时间]：%s~%s"
BSDiscount_Str1 = "[活动内容]：活动期间,商城内销售的所有物品,包括招募佣兵享受5折待遇."

---------双倍收益
--DoubleIncome_str = "[活动时间]：6月23日~6月24日"
DoubleIncome_str1 = "[活动内容]：玩家在当天的前50次闯关副本可以获得双倍经验"
DoubleIncome_str2 = "[剩余有效次数]：%s"


-----礼包预览
BAG_nameStr ="数量:"
BAG_button = "确定"
Gift_titleStr ="物品预览"
Gift_title2Str ="使用后可以获得以下物品:"



-------世界BOSS挑战
WORLDBOSS_TITLE1 = "击退BOSS可以获得大量声望和金币"
WORLDBOSS_TITLE2 = "%s".." ".." ".."%s级"
WORLDBOSS_STARTDAY = "每日%s开战"
WORLDBOSS_LASTTIMERANKING = "上次排名情况："
WORLDBOSS_RankContentStr1 = "第一名：%s"
WORLDBOSS_RankContentStr2 = "第二名：%s"
WORLDBOSS_RankContentStr3 = "第三名：%s"
WORLDBOSS_BUTTONStr = "上次战况"
WORLDBOSS_ATTACKBOSS = "<label>%s级世界BOSS%s，伤害前10名，可以获得礼包奖励</label>"
WORLDBOSS_ATTACKRANKING = "最终击杀"
WORLDBOSS_ATTACKVALUE = "，被%s击杀"
WORLDBOSS_ATTACKVALUE1 = "伤害血量：%s"
WORLDBOSS_ATTACKVALUE2 = "等级：%s"
WORLDBOSS_BUTTONStr1 = "查".." ".."看"
WORLDBOSS_REVIVESTR = "勇士您已经攻击%s次，共造成伤害："
WORLDBOSS_RESPAWN = "大战一场有点累，等待复活："
WORLDBOSS_Close = "退出"
WORLDBOSS_Revive = "立即复活"
WORLDBOSS_Inspire = "晶石鼓舞"
WORLDBOSS_Challenge = "立即挑战"
WORLDBOSS_AttackRanking = "伤害排名"
WORLDBOSS_AttackLV = " 等级：%s"
WORLDBOSS_ButtonStr1 = "退出"
WORLDBOSS_ButtonStr2 = "晶石鼓舞"
WORLDBOSS_ButtonStr3 = "立即复活"
WORLDBOSS_attackListStr = "伤害排名"
WORLDBOSS_IMGNum = "mainUI/list_1201_%s.png"
WORLDBOSS_NOKILL = "，未被击杀"

------服务端返回空值提示语
_ErrorStr = " is nil"


---- 活动界面无活动提示语
_activeStr = "当前没有活动"


--功能未开启提示语
PROMPT_CRYSTAL="20级将开启此功能"

--精灵祝福
SPRITE_REWARDS="恭喜您获得了："
SPRITE_REWARDS1="恭喜您获得"

----活动 首充值
ACTIVE_TOPUPSTR = "<label>尊敬的会长阁下,您在口袋天界中第一次充值,不管额度多少将获得以下奖励:</label>"
ACTIVE_BUTTONSTR = "立即前往"
ACTIVE_BUTTONSTR2 = "领取"
ACTIVE_SuccessStr = "恭喜你获得%s"
ACTIVE_FailStr = "尊敬的会长，您还没有一次充值进入啊，是否立即充值获得奖励？"
--金矿洞
GOLD_NOTICE="传说中的宝藏之地，丰富的金银财宝等着你！"
GOLD_IN="进入"

----------
--功能开启提示
GUIDE_OPENMESSAGE = "恭喜你开启了%s功能"
GUIDE_TITLE = {}
GUIDE_TITLE[2] = "魔法阵"
GUIDE_TITLE[3] = "魔术"
GUIDE_TITLE[4] = "装备强化"
GUIDE_TITLE[9] = "恩怨"
GUIDE_TITLE[11] = "竞技场"
GUIDE_TITLE[12] = "庄园"
GUIDE_TITLE[14] = "活动"
GUIDE_TITLE[15] = "金矿洞"
GUIDE_TITLE[16] = "命运水晶"
GUIDE_TITLE[19] = "世界BOSS挑战"
GUIDE_TITLE[21] = "每日探险"
GUIDE_TITLE[22] = "金钱树种植"
GUIDE_TITLE[24] = "圣吉塔挑战"
GUIDE_TITLE[27] = "背包"
GUIDE_TITLE[37] = "商城"
GUIDE_TITLE[38] = "传承"
GUIDE_TITLE[47] = "佣兵"
GUIDE_TITLE[48] = "装备"
GUIDE_TITLE[49] = "魂技"
GUIDE_TITLE[50] = "升级"
GUIDE_TITLE[51] = "副本"
GUIDE_TITLE[52] = "菜单"
GUIDE_TITLE[53] = "信件"
GUIDE_TITLE[54] = "设置"
GUIDE_TITLE[57] = "圣吉塔"
GUIDE_TITLE[58] = "考古"
GUIDE_TITLE[59] = "精英副本"


----集邮系统
Stamp_YONGBIN = "佣兵"
Stamp_ZHUANGBEI = "装备"
Stamp_HUNJI = "魂技"


--------------------------------
--圣吉塔
DENGJIWEIDADAO="暂无活动"
RIGHTGO="立即前往"
YITIAOZHAN="已挑战"
CI="次"
SHENGYU="剩余"
CIJIHUI="次机会"
TODAYTOP_TIP="【今日最高】："
CENGSHU="层数"
RIGHTNOW="【当前排名】："
PAIMINGZHIWAI="20名之外"
JINRUPAIHANG="（进入排行榜有额外的奖励）"
PAIHANGBANGTISHI="只要您进入排行榜，就能在第二天获得大量金币奖励。"
QINGTONG="青铜榜"
BAIYIN="白银榜"
HUANGJIN="黄金榜"
ZHENGRONG="阵容"
BIAOJIANGZI="表酱紫"
DENGJI="等级："
DEXING="得星："
LIANXUJINBANG="连续进榜"
TIAN="天"
NINSHANGCI="您上次单轮挑战最高得分"
KETIQIAN="可提前获得一项属性"
JIACHENG="加成"
XUANZEYIXIANG="选择一项属性开始挑战"
YONGCHUANG="勇闯圣吉塔第"
DI="第"
CENG="层"
DEFEN="得分"
MEIGE="每隔5层发放奖励"
MEIJIANGLI="没有奖励可领取！"
JIXUTIAOZHAN="继续挑战"
SHOUGUANGUAIWU="守关怪物数："
SHILI="实力"
BEIDEFEN="倍得分"
SHUXINGJIACHENG="属性加成："
JINRIZUIGAO="今日最高：层数"
YINGCHUANGGUO="已闯过"
CHOOSESHUXING="选择一项属性进行兑换，临时加强佣兵属性"
DUIHUAN="兑换"
MEIGUCENG="每过5层可领取奖励："
JINBI="金币"
CENGDEFEN="层得分："
CHAOCHUCHENGJI="超出今天最佳成绩"
KEXING="颗星"
LINGQUJIANGLI="领取奖励"
TISHI_3="1-5首次达到5、15、30、45星时，"
TISHI_4="可额外获得海量银两、宝箱、钥匙、晶石。"
ZHANDOU_1="再闯"
ZHANDOU_2="层可兑换属性加成"
ZHANDOU_3="层可领取丰厚奖励"
ZHANDOUHUIFANG="战斗回放"



--活动
ACTIVE_GETNOTICE = "领取成功"
ACTIVE_NOLASTBOSS = "当前还没有可查看的上次近况！"
BOSS_HURT_NUM="造成%d伤害"
ACTIVE_HAVEGET="已领取"
ACTIVE_HAVERECEIVE="已领"
ACTIVE_VIPOPEN="亲爱的会长，VIP%d级才开启此功能哦！"

--竞技场
COMPETI_ADD = "增加"
COMPETI_ADDTIMESMSG="是否消耗%d晶石增加1次挑战次数？"
COMPETI_EXCHAGNESUCCESS = "兑换成功"
COMPETI_TOTAL = "总价%s积分"
COMPETI_EXCHANGE = "您兑换\"%s\"的数量"

--龙穴取宝
STARTQUBAO="开始取宝"
SHENGYUCISHU="今日剩余次数："
SHENGYUCISHUBUZU="剩余次数不足"
ACTIVEENDTIME="活动结束时间："
JINTIAN="今天"
GONGXI="恭喜您！"
SHENTOU="神偷帮您窃取了："
QUBAOXIAOHAO="取宝消耗："
XIANYOU="现有："
JIESHAO="委托神偷“埃布尔”帮您到龙穴中窃取一件宝物，由于潜行的时间有限，能窃取到何种宝物完全取决于您的运气，对于贪财的神偷来说，您的委托金决定着他所使用的偷窃技能等级。"
ZHUANJIA="专家级窃取"
DASHI="大师级窃取"
ZONGSHI="宗师级窃取"

--遗迹考古
STARTKAOGU="开始考古"
KAOGUJIESHAO="在亚鲁尼斯历史中曾出现过许多辉煌的种族文明，相传在这些文明遗迹中隐藏这珍贵的历史碎片，佣兵们通过对各个遗迹的考古探索，寻获所有的碎片就可以拼接出宝图，找出传说中的失落宝藏。"
LIKAIKAOGU="离开考古"
YISHOUJI="已收集宝图碎片："
TISHI_0="每次战斗消耗"
TISHI_1="点精力，当前精力："
TISHI_2="战胜守卫怪物即可寻获藏宝图碎片，拼接藏宝图进入遗迹宝库即可挑战守卫BOSS获取失落宝物。"
SHENGYUTIAOZHAN="剩余挑战次数："
DIAOLUO="掉落："
GONGXIHUODE="恭喜您获得了："
TIAOZHANBUZU="剩余挑战次数不足，是否花费10晶石增加1次？"
ZUIHOU="宝藏已被发现，请等待下周重置"
BIANHAO="号boss"
JINLIBUZU="精力不足，请等待精力回复后继续挑战"
PEIYANGDAN="培养药剂"
TONGGUAN="通关悬空城考古地图才可以进入！"

-------------------------------------------
--1.28
GUIDE_JUMP="跳过"
GUIDE_NEXT="下一步"

CHAT_ADDFRIEND="添加好友成功"

