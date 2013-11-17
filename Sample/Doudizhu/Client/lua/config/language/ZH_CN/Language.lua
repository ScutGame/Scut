-------------------------------------------------------
-- Language.lua
-- Author     : JunM Chen
-- Version    : 1.0.0.0
-- Date       : 2013-3-6
-- Description: 存放工程所需的中文字符串变量
------------------------------------------------------------------

local strModuleName = "Language";
module(strModuleName, package.seeall);
--CCLuaLog("Module ".. strModuleName.. " loaded.");
strModuleName = nil;

-- 由于游戏显示的中文须采用UTF-8编码，\n因此将工程所有中文字符串都保存于本文件中，本\n文件采用无BOM的UTF-8编码格式。
-- 注意脑残Decoda不能支持本文件正常显示。其它常见编辑器均支持。
-- 同时任何人不得修改本文件的编码格式。
---
------------------------------------------------------------
GAME_NAME="真人斗地主"


 -- char size test
 
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
IDS_TOPUP="充值"
IDS_GAMECOIN="金豆:"
IDS_MENU="菜单"
IDS_ADMITTANCE="准入"
IDS_GAMECOIN_LEAST="当前金豆"
IDS_COMMA="，"
IDS_COLON="："
IDS_SENDCARDWARM="您没有选取卡牌"
IDS_CLOSED_APP="是否退出游戏？"


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


--登录

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
LAN_REGIST_WARM="4-12位字符"
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
LAN_NAME="输入昵称:"
LAN_REGIST="快速注册"
LAN_LOGIN="登陆游戏"


TIP_TOPUP="充值成功! 元宝将在1至2分钟内到账或者刷新页面！"
TIP_PLEASECHOICEGOLD="请选择金额"
--商店
SHOP_HEADSHOP="头像商店"
SHOP_GOLD="元宝"
SHOP_COMMODITYNAME="物品名称"
SHOP_PRICE="价格："
SHOP_VIPPRICE="vip价格："
SHOP_BUYSUCCESS="恭喜您购买成功！"
SHOP_FAILURE="您的元宝余额不足！"
SHOP_GAINGAMECOIN="获得金豆："
SHOP_BUZUCHONGZHI="元宝不足，是否立即充值？"

------服务端返回空值提示语
_ErrorStr = " is nil"

---聊天系统
CHAT_JISHICHAT="即时聊天"
CHAT_LIAOTIANRECORD="聊天记录"
CHAT_ME="我"
CHAT_SAY="说："
CHAT_FOR="对"
CHAT_ZANWEIKAIFANG="暂未开放"
--排名系统
RANKING_JINDOUPAIMING="金豆排名"
RANKING_SHENGLVPAIMING="胜率排名"
RANKING_SHUOMING="说明"
RANKING_MINGCI="名次"
RANKING_NICHEN="昵称"
RANKING_JINDOU="金豆"
RANKING_SHENGLV="胜率"
RANKING_ZANWUPAIMING="暂无排名"
RANKING_SHUOMING="1、金豆排行：玩家在游戏中拥有的金豆越多排名越高。"
RANKING_SHUOMING2="2、胜率排行：当玩家游戏局数超过500场后即可获得排行资格。"
-------
 IDS_FONT_12_TEST			= "损兵折将的准准备。兵折将的准备为一名征战四方的君主，怎能没有一个响亮的名号备aaaa好aaafgfgfgadfasdfasd损兵折将的准备"
 
 
 --任务
CHENGZHANGRENWU="成长任务"
RICHANGRENWU="日常任务"
TASKNAME="任务名称："
TASKJD="任务进度"
TASKMS="任务描述："
TASKJL="任务奖励"
JIANGLI="奖励"
JINDOU="金豆"
LINGQU="领取"
YILINGQU="已领取"
ZANWURENWU="暂无任务"
WEIWANCHENG="未完成"
YOUXIYINXIAO="游戏音效"
BEIJINGYINYUE="背景音乐"
SHOUJIZHENDONG="手机震动"
HUODE_1="您获得"
HUODE_2="金豆奖励！"



---主要功能
IDS_BASE="底注"
IDS_BEISHU="倍数"
IDS_EXITWARM="是否要退出游戏桌面？"


--个人档案
PersonalFile_BASIC="基本资料"
PersonalFile_SOCIAL="社交资料"
PersonalFile_GAME="游戏资料"

PersonalFile_GOLD="%s元宝"
PersonalFile_CHANGEPWD="修改密码"
PersonalFile_COMPLEMENTEDPWD="补全密码"
PERSONAL_GIRL="女孩"
PERSONAL_MAN="男孩"
PERSONAL_YEAR="年"
PERSONAL_MONTH="月"
PERSONAL_DAY="号"
PERSONAL_NAMEERROR="您输入的昵称有误(可输入4个字符)"
PERSONAL_HOBBYERROR="您输入的爱好有误(可输入4个字符)"
PERSONAL_PROFESSIONERROR="您输入的职业有误(可输入4个字符)"
PERSONAL_BIRTHDAYERROR=""
PERSONAL_SUCCESS="修改成功"
PERSONAL_BAOCUN="保存成功"
PERSONAL_CHANGE="是否更换头像？"
PERSONAL_TOSHOP="您暂无可更换的头像，可至 "
PERSONAL_SHOP="商店"
PERSONAL_BUY=" 购买"
PERSONA_ACHIEVEMENT="成就进度"

--密码
Password_COMPLEMENTED="确定补全"
Password_CHANGE="确定修改"
Password_TITLE="密码补全"
Password_CHANGEPASSWORD="修改密码:"
Password_COMPLEMENTEDPWD="补全密码:"
Password_CONFIRMPWD="确认输入:"
Password_NOT_BU_PASSWORD="未输入密码！"
Password_PLEASE_PASSWORD="请再次输入密码！"
Password_BUG_PASSWORD="两次输入密码不同！"
Password_BUQUAN_OK="密码补全成功"
Password_PASSWORD_OK="密码修改成功"

PersonalFile_DDZ="萌妹斗地主"


--充值
TOPUP_TITLE			= "充值"
TOPUP_RECORD			= "充值记录"
TOPUP_TOPUPNOW		= "立即充值"
TOP_TIP=
[[
如何充值元宝（1元=10元宝）

1、通过手机充值
     在【充值中心】页面就可以直接进行移动卡/联通卡/电信卡和支付宝的充值。

2、通过电脑充值
     登录充值网站，选择“银行卡/支付宝充值”，再选择“游戏类型”→“萌妹斗地主”充值。本服务支持银行卡直充、电话卡和支付宝等多种方式。
]]



TOP_TIP91=[[充值规则：
91豆：元宝=1：10
即1个91豆等于10元宝
点击“马上充值”在输入框内填写您要充值的元宝数量进入91充值页面按照提示步骤即可完成！
]]

TOP_TIPCHUKONG=[[充值规则：
1 充值比例（1元=6.5元宝）
2 点击“立即充值”在输入框内输入你所要充值的人民币数额进入充值页面按照提示步骤即可完成。（提示：您输入的人民币数额必须为10的整数倍，否则无法充值成功！）
]]



TOP_TIPNOCOMPUTERUC=
[[
如何充值元宝（1元=10元宝）
1、通过手机充值
在【充值中心】页面就可以直接进行移动卡/联通卡/电信卡的充值。
]]

TOP_TIPNOCOMPUTERQIHU=
[[
如何充值元宝（1元=10元宝）
在【支付中心】页面就可以进行支付宝/银联/充值卡/骏网卡/360币/储蓄卡的充值
]]

--转盘系统
ROLL_FREE="剩余免费次数"
TIP_ESCAPE="玩家%s已逃跑！"
TIP_ESCAPE1="您正处于上一局托管中,是否结束上一局？"

--广播
NOTICE_NAME = "【%s】"
NOTICE_SYSTEM="系统"
NOTICE_ZANWUGONGAO="暂无公告"

COMEBACK_TIP="正在返回上一局未结束牌局，请稍等..."
SET_ACCOUNT= "账号设置"
SET_PERSONAL="个人中心"
SET_FEEDBACK="意见反馈"
IDS_NETDRAGON="论坛专区"
TOPUP_TIP4="请输入十的倍数"
TOUUP_TIPS="您输入的额度必须为10的倍数！"
TIP_PLEASEINPUT="请输入数量："
LAN_ID_NOT_Z="请输入账号和密码"
LAN_ID_NOT_PASSWOED="未输入密码"
LAN_ONT_NAME="请输入玩家姓名！"
