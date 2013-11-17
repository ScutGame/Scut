/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using System;
using System.Data;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// ActionID定义描述
    /// </summary>
    public class ActionIDDefine
    {
        ///<summary>
        ///GET 360TOKEN 
        ///</summary>
        public const Int16 Cst_Action360 = 360;
        ///<summary>
        ///错误日志
        ///</summary>
        public const Int16 Cst_Action404 = 404;
        ///<summary>
        ///测试用
        ///</summary>
        public const Int16 Cst_Action10000 = 10000;
        ///<summary>
        ///GM命令接口
        ///</summary>
        public const Int16 Cst_Action1000 = 1000;

        ///<summary>
        ///服务器列表协议接口
        ///</summary>
        public const Int16 Cst_Action1001 = 1001;

        ///<summary>
        ///注册通行证ID获取接口
        ///</summary>
        public const Int16 Cst_Action1002 = 1002;

        ///<summary>
        ///用户注册
        ///</summary>
        public const Int16 Cst_Action1003 = 1003;

        ///<summary>
        ///用户登录
        ///</summary>
        public const Int16 Cst_Action1004 = 1004;

        ///<summary>
        ///创建角色
        ///</summary>
        public const Int16 Cst_Action1005 = 1005;

        ///<summary>
        ///密码更新接口
        ///</summary>
        public const Int16 Cst_Action1006 = 1006;

        ///<summary>
        ///用户检测接口
        ///</summary>
        public const Int16 Cst_Action1007 = 1007;

        ///<summary>
        ///用户角色详情接口
        ///</summary>
        public const Int16 Cst_Action1008 = 1008;

        ///<summary>
        ///用户开启功能列表接口
        ///</summary>
        public const Int16 Cst_Action1009 = 1009;

        ///<summary>
        ///购买体力接口
        ///</summary>
        public const Int16 Cst_Action1010 = 1010;

        ///<summary>
        ///挖金矿接口
        ///</summary>
        public const Int16 Cst_Action1011 = 1011;

        ///<summary>
        ///界面消息通知接口
        ///</summary>
        public const Int16 Cst_Action1012 = 1012;

        ///<summary>
        ///新手活动在线得精力领取接口
        ///</summary>
        public const Int16 Cst_Action1013 = 1013;

        ///<summary>
        ///新手活动登录奖励领取接口
        ///</summary>
        public const Int16 Cst_Action1014 = 1014;

        ///<summary>
        ///新手活动新手卡领取接口
        ///</summary>
        public const Int16 Cst_Action1015 = 1015;

        ///<summary>
        ///随机取玩家名字接口
        ///</summary>
        public const Int16 Cst_Action1016 = 1016;

        ///<summary>
        ///玩家退出接口
        ///</summary>
        public const Int16 Cst_Action1017 = 1017;

        ///<summary>
        ///新手活动领取阅历和声望接口
        ///</summary>
        public const Int16 Cst_Action1018 = 1018;

        ///<summary>
        ///排行榜列表接口
        ///</summary>
        public const Int16 Cst_Action1019 = 1019;

        ///<summary>
        ///领取俸禄接口
        ///</summary>
        public const Int16 Cst_Action1020 = 1020;

        ///<summary>
        ///小助手接口
        ///</summary>
        public const Int16 Cst_Action1021 = 1021;

        ///<summary>
        ///补尝领取接口
        ///</summary>
        public const Int16 Cst_Action1022 = 1022;

        ///<summary>
        ///玩家改名接口
        ///</summary>
        public const Int16 Cst_Action1023 = 1023;

        ///<summary>
        ///激活新手卡接口
        ///</summary>
        public const Int16 Cst_Action1024 = 1024;

        ///<summary>
        ///佣兵升级界面
        ///</summary>
        public const Int16 Cst_Action1025 = 1025;

        ///<summary>
        ///可创建佣兵列表接口
        ///</summary>
        public const Int16 Cst_Action1026 = 1026;

        ///<summary>
        ///91SDK充值接口
        ///</summary>
        public const Int16 Cst_Action1065 = 1065;

        ///<summary>
        ///AppStore充值接口
        ///</summary>
        public const Int16 Cst_Action1066 = 1066;

        ///<summary>
        ///AppStore充值详情接口
        ///</summary>
        public const Int16 Cst_Action1069 = 1069;

        ///<summary>
        ///触控android充值接口
        ///</summary>
        public const Int16 Cst_Action1080 = 1080;

        ///<summary>
        ///配置数据版本检测接口
        ///</summary>
        public const Int16 Cst_Action1090 = 1090;

        ///<summary>
        ///背包列表接口
        ///</summary>
        public const Int16 Cst_Action1101 = 1101;

        ///<summary>
        ///背包物品详情接口
        ///</summary>
        public const Int16 Cst_Action1102 = 1102;

        ///<summary>
        ///背包整理接口
        ///</summary>
        public const Int16 Cst_Action1103 = 1103;

        ///<summary>
        ///背包格子开启接口
        ///</summary>
        public const Int16 Cst_Action1104 = 1104;

        ///<summary>
        ///背包物品丢弃接口
        ///</summary>
        public const Int16 Cst_Action1105 = 1105;

        ///<summary>
        ///仓库物品列表接口
        ///</summary>
        public const Int16 Cst_Action1106 = 1106;

        ///<summary>
        ///仓库物品放入取出接口
        ///</summary>
        public const Int16 Cst_Action1107 = 1107;

        ///<summary>
        ///仓库格子开启接口
        ///</summary>
        public const Int16 Cst_Action1108 = 1108;

        ///<summary>
        ///背包物品分类列表
        ///</summary>
        public const Int16 Cst_Action1109 = 1109;

        ///<summary>
        ///佣兵装备列表接口
        ///</summary>
        public const Int16 Cst_Action1201 = 1201;

        ///<summary>
        ///装备详情接口
        ///</summary>
        public const Int16 Cst_Action1202 = 1202;

        ///<summary>
        ///佣兵装备更换接口
        ///</summary>
        public const Int16 Cst_Action1203 = 1203;

        ///<summary>
        ///装备强化接口
        ///</summary>
        public const Int16 Cst_Action1204 = 1204;

        ///<summary>
        ///装备列表接口
        ///</summary>
        public const Int16 Cst_Action1205 = 1205;

        ///<summary>
        ///未穿戴装备列表接口
        ///</summary>
        public const Int16 Cst_Action1208 = 1208;

        ///<summary>
        ///未穿戴部位装备列表
        ///</summary>
        public const Int16 Cst_Action1209 = 1209;

        ///<summary>
        ///装备封灵界面接口
        ///</summary>
        public const Int16 Cst_Action1210 = 1210;

        ///<summary>
        ///装备灵件背包列表接口
        ///</summary>
        public const Int16 Cst_Action1211 = 1211;

        ///<summary>
        ///装备镶嵌灵件列表接口
        ///</summary>
        public const Int16 Cst_Action1212 = 1212;

        ///<summary>
        ///装备镶嵌与卸下接口
        ///</summary>
        public const Int16 Cst_Action1213 = 1213;

        ///<summary>
        ///灵件属性洗涤接口
        ///</summary>
        public const Int16 Cst_Action1214 = 1214;

        ///<summary>
        ///灵件背包开启接口
        ///</summary>
        public const Int16 Cst_Action1215 = 1215;

        ///<summary>
        ///灵件属性开启接口
        ///</summary>
        public const Int16 Cst_Action1216 = 1216;

        ///<summary>
        ///佣兵培养接口
        ///</summary>
        public const Int16 Cst_Action1217 = 1217;

        ///<summary>
        ///装备附魔界面接口
        ///</summary>
        public const Int16 Cst_Action1251 = 1251;

        ///<summary>
        ///佣兵武器附魔符列表接口
        ///</summary>
        public const Int16 Cst_Action1252 = 1252;

        ///<summary>
        ///背包附魔符列表接口
        ///</summary>
        public const Int16 Cst_Action1253 = 1253;

        ///<summary>
        ///附魔符详情接口
        ///</summary>
        public const Int16 Cst_Action1254 = 1254;

        ///<summary>
        ///卖出附魔符接口
        ///</summary>
        public const Int16 Cst_Action1255 = 1255;

        ///<summary>
        ///合成附魔符接口
        ///</summary>
        public const Int16 Cst_Action1256 = 1256;

        ///<summary>
        ///附魔符培养界面接口
        ///</summary>
        public const Int16 Cst_Action1257 = 1257;

        ///<summary>
        ///附魔符培养接口
        ///</summary>
        public const Int16 Cst_Action1258 = 1258;

        ///<summary>
        ///武器替换附魔符接口
        ///</summary>
        public const Int16 Cst_Action1259 = 1259;

        ///<summary>
        ///附魔背包格子开启接口
        ///</summary>
        public const Int16 Cst_Action1260 = 1260;

        ///<summary>
        ///可装备附魔符列表接口
        ///</summary>
        public const Int16 Cst_Action1261 = 1261;

        ///<summary>
        ///可合成附魔符列表接口
        ///</summary>
        public const Int16 Cst_Action1262 = 1262;

        ///<summary>
        ///命运水晶列表接口
        ///</summary>
        public const Int16 Cst_Action1301 = 1301;

        ///<summary>
        ///佣兵命运水晶列表接口
        ///</summary>
        public const Int16 Cst_Action1302 = 1302;

        ///<summary>
        ///猎取命运水晶列表接口
        ///</summary>
        public const Int16 Cst_Action1303 = 1303;

        ///<summary>
        ///命运水晶详情接口
        ///</summary>
        public const Int16 Cst_Action1304 = 1304;

        ///<summary>
        ///获取命运水晶接口
        ///</summary>
        public const Int16 Cst_Action1305 = 1305;

        ///<summary>
        ///卖出命运水晶接口
        ///</summary>
        public const Int16 Cst_Action1306 = 1306;

        ///<summary>
        ///拾取命运水晶接口
        ///</summary>
        public const Int16 Cst_Action1307 = 1307;

        ///<summary>
        ///合成命运水晶接口
        ///</summary>
        public const Int16 Cst_Action1308 = 1308;

        ///<summary>
        ///佣兵替换命运水晶接口
        ///</summary>
        public const Int16 Cst_Action1309 = 1309;

        ///<summary>
        ///命运背包格子开启接口
        ///</summary>
        public const Int16 Cst_Action1310 = 1310;

        ///<summary>
        ///命运背包水晶列表接口
        ///</summary>
        public const Int16 Cst_Action1311 = 1311;

        ///<summary>
        ///一键获取命运水晶接口
        ///</summary>
        public const Int16 Cst_Action1312 = 1312;

        ///<summary>
        ///玩家佣兵列表接口
        ///</summary>
        public const Int16 Cst_Action1401 = 1401;

        ///<summary>
        ///酒馆佣兵列表接口
        ///</summary>
        public const Int16 Cst_Action1402 = 1402;

        ///<summary>
        ///佣兵详情接口
        ///</summary>
        public const Int16 Cst_Action1403 = 1403;

        ///<summary>
        ///佣兵邀请接口
        ///</summary>
        public const Int16 Cst_Action1404 = 1404;

        ///<summary>
        ///佣兵离队接口
        ///</summary>
        public const Int16 Cst_Action1405 = 1405;

        ///<summary>
        ///丹药列表接口
        ///</summary>
        public const Int16 Cst_Action1406 = 1406;

        ///<summary>
        ///丹药服用接口
        ///</summary>
        public const Int16 Cst_Action1407 = 1407;

        ///<summary>
        ///培养属性详情接口
        ///</summary>
        public const Int16 Cst_Action1408 = 1408;

        ///<summary>
        ///佣兵属性培养接口
        ///</summary>
        public const Int16 Cst_Action1409 = 1409;

        ///<summary>
        ///佣兵属性保存接口
        ///</summary>
        public const Int16 Cst_Action1410 = 1410;

        ///<summary>
        ///佣兵修炼接口
        ///</summary>
        public const Int16 Cst_Action1411 = 1411;

        ///<summary>
        ///佣兵修炼获得经验接口
        ///</summary>
        public const Int16 Cst_Action1412 = 1412;

        ///<summary>
        ///佣兵可服用丹药列表接口
        ///</summary>
        public const Int16 Cst_Action1413 = 1413;

        ///<summary>
        ///Vip用户佣兵可服用丹药列表接口
        ///</summary>
        public const Int16 Cst_Action1414 = 1414;

        ///<summary>
        ///药剂摘除接口
        ///</summary>
        public const Int16 Cst_Action1415 = 1415;

        ///<summary>
        ///传承界面接口
        ///</summary>
        public const Int16 Cst_Action1416 = 1416;

        ///<summary>
        ///传承与被传承人列表接口
        ///</summary>
        public const Int16 Cst_Action1417 = 1417;

        ///<summary>
        ///传承与被传承人选择接口
        ///</summary>
        public const Int16 Cst_Action1418 = 1418;

        ///<summary>
        ///传承接口
        ///</summary>
        public const Int16 Cst_Action1419 = 1419;

        ///<summary>
        ///礼物界面接口
        ///</summary>
        public const Int16 Cst_Action1420 = 1420;

        ///<summary>
        ///玩家礼物列表接口
        ///</summary>
        public const Int16 Cst_Action1421 = 1421;

        ///<summary>
        ///赠送礼物接口
        ///</summary>
        public const Int16 Cst_Action1422 = 1422;

        ///<summary>
        ///消除饱食度接口
        ///</summary>
        public const Int16 Cst_Action1423 = 1423;

        ///<summary>
        ///将星佣兵列表接口
        ///</summary>
        public const Int16 Cst_Action1431 = 1431;

        ///<summary>
        ///将星佣兵招募详情接口
        ///</summary>
        public const Int16 Cst_Action1432 = 1432;

        ///<summary>
        ///将星佣兵任务奖励领取接口
        ///</summary>
        public const Int16 Cst_Action1433 = 1433;

        ///<summary>
        ///将星佣兵招募接口
        ///</summary>
        public const Int16 Cst_Action1434 = 1434;

        ///<summary>
        ///佣兵升级界面接口
        ///</summary>
        public const Int16 Cst_Action1441 = 1441;

        ///<summary>
        ///佣兵升级接口
        ///</summary>
        public const Int16 Cst_Action1442 = 1442;

        ///<summary>
        ///佣兵升级卡片列表接口
        ///</summary>
        public const Int16 Cst_Action1444 = 1444;

        ///<summary>
        ///佣兵升级卡片选择接口
        ///</summary>
        public const Int16 Cst_Action1445 = 1445;

        ///<summary>
        ///法宝修炼界面接口
        ///</summary>
        public const Int16 Cst_Action1451 = 1451;

        ///<summary>
        ///法宝修炼接口
        ///</summary>
        public const Int16 Cst_Action1452 = 1452;

        ///<summary>
        ///法宝界面接口
        ///</summary>
        public const Int16 Cst_Action1453 = 1453;

        ///<summary>
        ///法宝详细信息接口
        ///</summary>
        public const Int16 Cst_Action1454 = 1454;

        ///<summary>
        ///法宝升级物品列表接口
        ///</summary>
        public const Int16 Cst_Action1455 = 1455;

        ///<summary>
        ///法宝升级接口
        ///</summary>
        public const Int16 Cst_Action1456 = 1456;

        ///<summary>
        ///法宝延寿接口
        ///</summary>
        public const Int16 Cst_Action1457 = 1457;

        ///<summary>
        ///法宝洗练接口
        ///</summary>
        public const Int16 Cst_Action1458 = 1458;

        ///<summary>
        ///法宝技能列表接口
        ///</summary>
        public const Int16 Cst_Action1459 = 1459;

        ///<summary>
        ///法宝技能洗涤接口
        ///</summary>
        public const Int16 Cst_Action1460 = 1460;

        ///<summary>
        ///法宝技能升级消耗接口
        ///</summary>
        public const Int16 Cst_Action1461 = 1461;

        ///<summary>
        ///法宝技能升级接口
        ///</summary>
        public const Int16 Cst_Action1462 = 1462;

        ///<summary>
        ///法宝附加属性列表接口
        ///</summary>
        public const Int16 Cst_Action1463 = 1463;

        ///<summary>
        ///法宝附加属性祭祀接口
        ///</summary>
        public const Int16 Cst_Action1464 = 1464;

        ///<summary>
        ///法宝附加属性学习属性列表接口
        ///</summary>
        public const Int16 Cst_Action1465 = 1465;

        ///<summary>
        ///法宝附加属性学习接口
        ///</summary>
        public const Int16 Cst_Action1466 = 1466;

        ///<summary>
        ///法宝附加属性学习接口
        ///</summary>
        public const Int16 Cst_Action1467 = 1467;

        ///<summary>
        ///法宝附加属性升级消耗接口
        ///</summary>
        public const Int16 Cst_Action1468 = 1468;

        ///<summary>
        ///法宝附加属性升级接口
        ///</summary>
        public const Int16 Cst_Action1469 = 1469;

        ///<summary>
        ///法宝属相接口
        ///</summary>
        public const Int16 Cst_Action1470 = 1470;

        ///<summary>
        ///法宝属相易相接口
        ///</summary>
        public const Int16 Cst_Action1471 = 1471;

        ///<summary>
        ///魔术列表接口
        ///</summary>
        public const Int16 Cst_Action1501 = 1501;

        ///<summary>
        ///魔术详情接口
        ///</summary>
        public const Int16 Cst_Action1502 = 1502;

        ///<summary>
        ///魔术升级接口
        ///</summary>
        public const Int16 Cst_Action1503 = 1503;

        ///<summary>
        ///魔术配置信息下发接口
        ///</summary>
        public const Int16 Cst_Action1504 = 1504;

        ///<summary>
        ///魂技配置信息下发接口
        ///</summary>
        public const Int16 Cst_Action1505 = 1505;

        ///<summary>
        ///装备、丹药合成材料列表接口
        ///</summary>
        public const Int16 Cst_Action1601 = 1601;

        ///<summary>
        ///装备、丹药合成材料详情接口
        ///</summary>
        public const Int16 Cst_Action1602 = 1602;

        ///<summary>
        ///装备、丹药合成接口
        ///</summary>
        public const Int16 Cst_Action1603 = 1603;

        ///<summary>
        ///材料掉落副本接口
        ///</summary>
        public const Int16 Cst_Action1604 = 1604;

        ///<summary>
        ///药水加血使用接口
        ///</summary>
        public const Int16 Cst_Action1605 = 1605;

        ///<summary>
        ///新手礼包使用接口
        ///</summary>
        public const Int16 Cst_Action1606 = 1606;

        ///<summary>
        ///领取气血包接口
        ///</summary>
        public const Int16 Cst_Action1607 = 1607;

        ///<summary>
        ///变身卡使用接口
        ///</summary>
        public const Int16 Cst_Action1608 = 1608;

        ///<summary>
        ///VIP礼包使用接口
        ///</summary>
        public const Int16 Cst_Action1609 = 1609;

        ///<summary>
        ///队列列表接口
        ///</summary>
        public const Int16 Cst_Action1701 = 1701;

        ///<summary>
        ///队列加速接口
        ///</summary>
        public const Int16 Cst_Action1702 = 1702;

        ///<summary>
        ///队列开启接口
        ///</summary>
        public const Int16 Cst_Action1703 = 1703;

        ///<summary>
        ///魔法阵列表接口
        ///</summary>
        public const Int16 Cst_Action1901 = 1901;

        ///<summary>
        ///魔法阵设置接口
        ///</summary>
        public const Int16 Cst_Action1902 = 1902;

        ///<summary>
        ///魔法阵启用接口
        ///</summary>
        public const Int16 Cst_Action1903 = 1903;

        ///<summary>
        ///世界图片加载接口
        ///</summary>
        public const Int16 Cst_Action2001 = 2001;

        ///<summary>
        ///城市地图加载接口
        ///</summary>
        public const Int16 Cst_Action2002 = 2002;

        ///<summary>
        ///城市玩家刷新接口
        ///</summary>
        public const Int16 Cst_Action2003 = 2003;

        ///<summary>
        ///选择国家接口
        ///</summary>
        public const Int16 Cst_Action2004 = 2004;

        ///<summary>
        ///城市地图配置下发接口
        ///</summary>
        public const Int16 Cst_Action2005 = 2005;

        ///<summary>
        ///剧情任务列表接口
        ///</summary>
        public const Int16 Cst_Action3001 = 3001;

        ///<summary>
        ///剧情任务领取放弃接口
        ///</summary>
        public const Int16 Cst_Action3002 = 3002;

        ///<summary>
        ///剧情任务跟踪接口
        ///</summary>
        public const Int16 Cst_Action3003 = 3003;

        ///<summary>
        ///NPC任务列表接口
        ///</summary>
        public const Int16 Cst_Action3004 = 3004;

        ///<summary>
        ///日常任务列表接口
        ///</summary>
        public const Int16 Cst_Action3005 = 3005;

        ///<summary>
        ///日常任务领取放弃接口
        ///</summary>
        public const Int16 Cst_Action3006 = 3006;

        ///<summary>
        ///剧情任务交付接口
        ///</summary>
        public const Int16 Cst_Action3007 = 3007;

        ///<summary>
        ///剧情任务配置信息下发接口
        ///</summary>
        public const Int16 Cst_Action3008 = 3008;

        ///<summary>
        ///活动列表接口
        ///</summary>
        public const Int16 Cst_Action3009 = 3009;

        ///<summary>
        ///限时活动列表接口
        ///</summary>
        public const Int16 Cst_Action3010 = 3010;

        ///<summary>
        ///限时活动详情接口
        ///</summary>
        public const Int16 Cst_Action3011 = 3011;

        ///<summary>
        ///赛跑界面接口
        ///</summary>
        public const Int16 Cst_Action3201 = 3201;

        ///<summary>
        ///宠物配置接口
        ///</summary>
        public const Int16 Cst_Action3202 = 3202;

        ///<summary>
        ///宠物刷新接口
        ///</summary>
        public const Int16 Cst_Action3203 = 3203;

        ///<summary>
        ///赛跑开始接口
        ///</summary>
        public const Int16 Cst_Action3204 = 3204;

        ///<summary>
        ///拦截界面接口
        ///</summary>
        public const Int16 Cst_Action3205 = 3205;

        ///<summary>
        ///宠物拦截接口
        ///</summary>
        public const Int16 Cst_Action3206 = 3206;

        ///<summary>
        ///副本列表接口
        ///</summary>
        public const Int16 Cst_Action4001 = 4001;

        ///<summary>
        ///副本地图加载接口
        ///</summary>
        public const Int16 Cst_Action4002 = 4002;

        ///<summary>
        ///副本通关宝箱与评价接口
        ///</summary>
        public const Int16 Cst_Action4003 = 4003;

        ///<summary>
        ///副本战斗详情接口
        ///</summary>
        public const Int16 Cst_Action4004 = 4004;

        ///<summary>
        ///离开副本接口
        ///</summary>
        public const Int16 Cst_Action4005 = 4005;

        ///<summary>
        ///扫荡界面接口
        ///</summary>
        public const Int16 Cst_Action4006 = 4006;

        ///<summary>
        ///扫荡开始接口
        ///</summary>
        public const Int16 Cst_Action4007 = 4007;

        ///<summary>
        ///加速或晶石扫荡接口
        ///</summary>
        public const Int16 Cst_Action4008 = 4008;

        ///<summary>
        ///扫荡详情接口
        ///</summary>
        public const Int16 Cst_Action4009 = 4009;

        ///<summary>
        ///扫荡取消接口
        ///</summary>
        public const Int16 Cst_Action4010 = 4010;

        ///<summary>
        ///副本配置信息下发接口
        ///</summary>
        public const Int16 Cst_Action4011 = 4011;

        ///<summary>
        ///重置精英副本接口
        ///</summary>
        public const Int16 Cst_Action4012 = 4012;

        ///<summary>
        ///跳过副本战斗接口
        ///</summary>
        public const Int16 Cst_Action4013 = 4013;

        ///<summary>
        ///重置英雄副本接口
        ///</summary>
        public const Int16 Cst_Action4014 = 4014;

        ///<summary>
        ///多人副本组队列表接口
        ///</summary>
        public const Int16 Cst_Action4202 = 4202;

        ///<summary>
        ///可创建副本列表接口
        ///</summary>
        public const Int16 Cst_Action4203 = 4203;

        ///<summary>
        ///副本队伍详情接口
        ///</summary>
        public const Int16 Cst_Action4204 = 4204;

        ///<summary>
        ///创建副本接口
        ///</summary>
        public const Int16 Cst_Action4205 = 4205;

        ///<summary>
        ///加入队伍接口
        ///</summary>
        public const Int16 Cst_Action4206 = 4206;

        ///<summary>
        ///队伍位置调整接口
        ///</summary>
        public const Int16 Cst_Action4207 = 4207;

        ///<summary>
        ///踢出玩家接口
        ///</summary>
        public const Int16 Cst_Action4208 = 4208;

        ///<summary>
        ///玩家离开接口
        ///</summary>
        public const Int16 Cst_Action4209 = 4209;

        ///<summary>
        ///开始战斗接口
        ///</summary>
        public const Int16 Cst_Action4210 = 4210;

        ///<summary>
        ///多人副本战斗详情接口
        ///</summary>
        public const Int16 Cst_Action4211 = 4211;

        ///<summary>
        ///天地劫副本接口
        ///</summary>
        public const Int16 Cst_Action4301 = 4301;

        ///<summary>
        ///刷新天地劫副本接口
        ///</summary>
        public const Int16 Cst_Action4302 = 4302;

        ///<summary>
        ///天地劫副本战斗详情接口
        ///</summary>
        public const Int16 Cst_Action4303 = 4303;

        ///<summary>
        ///竞技场列表接口
        ///</summary>
        public const Int16 Cst_Action5101 = 5101;

        ///<summary>
        ///竞技英雄列表接口
        ///</summary>
        public const Int16 Cst_Action5102 = 5102;

        ///<summary>
        ///挑战奖励接口
        ///</summary>
        public const Int16 Cst_Action5103 = 5103;

        ///<summary>
        ///增加挑战次数接口
        ///</summary>
        public const Int16 Cst_Action5104 = 5104;

        ///<summary>
        ///消除竞技冷却时间接口
        ///</summary>
        public const Int16 Cst_Action5105 = 5105;

        ///<summary>
        ///排名奖励领取接口
        ///</summary>
        public const Int16 Cst_Action5106 = 5106;

        ///<summary>
        ///竞技战斗详情接口
        ///</summary>
        public const Int16 Cst_Action5107 = 5107;

        ///<summary>
        ///领土战列表接口
        ///</summary>
        public const Int16 Cst_Action5201 = 5201;

        ///<summary>
        ///领土战鼓舞接口
        ///</summary>
        public const Int16 Cst_Action5202 = 5202;

        ///<summary>
        ///领土战英雄列表接口
        ///</summary>
        public const Int16 Cst_Action5203 = 5203;

        ///<summary>
        ///领土战参加接口
        ///</summary>
        public const Int16 Cst_Action5204 = 5204;

        ///<summary>
        ///领土战退出接口
        ///</summary>
        public const Int16 Cst_Action5205 = 5205;

        ///<summary>
        ///公会战报名页面接口
        ///</summary>
        public const Int16 Cst_Action5301 = 5301;

        ///<summary>
        ///公会战报名接口
        ///</summary>
        public const Int16 Cst_Action5302 = 5302;

        ///<summary>
        ///公会战已报名列表接口
        ///</summary>
        public const Int16 Cst_Action5303 = 5303;

        ///<summary>
        ///公会战进入战场接口
        ///</summary>
        public const Int16 Cst_Action5304 = 5304;

        ///<summary>
        ///公会战晶石鼓舞接口
        ///</summary>
        public const Int16 Cst_Action5305 = 5305;

        ///<summary>
        ///公会战参战人员列表接口
        ///</summary>
        public const Int16 Cst_Action5306 = 5306;

        ///<summary>
        ///Boss战参加接口
        ///</summary>
        public const Int16 Cst_Action5401 = 5401;

        ///<summary>
        ///Boss战报鼓舞接口
        ///</summary>
        public const Int16 Cst_Action5402 = 5402;

        ///<summary>
        ///Boss战欲火重生接口
        ///</summary>
        public const Int16 Cst_Action5403 = 5403;

        ///<summary>
        ///Boss战血量刷新接口
        ///</summary>
        public const Int16 Cst_Action5404 = 5404;

        ///<summary>
        ///Boss战战斗详情接口
        ///</summary>
        public const Int16 Cst_Action5405 = 5405;

        ///<summary>
        ///Boss战伤害排名接口
        ///</summary>
        public const Int16 Cst_Action5406 = 5406;

        ///<summary>
        ///Boss战退出接口
        ///</summary>
        public const Int16 Cst_Action5407 = 5407;

        ///<summary>
        ///公会列表接口
        ///</summary>
        public const Int16 Cst_Action6001 = 6001;

        ///<summary>
        ///公会成员列表接口
        ///</summary>
        public const Int16 Cst_Action6002 = 6002;

        ///<summary>
        ///公会日志列表接口
        ///</summary>
        public const Int16 Cst_Action6003 = 6003;

        ///<summary>
        ///公会活动列表接口
        ///</summary>
        public const Int16 Cst_Action6004 = 6004;

        ///<summary>
        ///公会详情接口
        ///</summary>
        public const Int16 Cst_Action6005 = 6005;

        ///<summary>
        ///公会申请接口
        ///</summary>
        public const Int16 Cst_Action6006 = 6006;

        ///<summary>
        ///公会申请审核列表接口
        ///</summary>
        public const Int16 Cst_Action6007 = 6007;

        ///<summary>
        ///公会职务任命转让接口
        ///</summary>
        public const Int16 Cst_Action6008 = 6008;

        ///<summary>
        ///公会公会设置接口
        ///</summary>
        public const Int16 Cst_Action6009 = 6009;

        ///<summary>
        ///公会退出接口
        ///</summary>
        public const Int16 Cst_Action6010 = 6010;

        ///<summary>
        ///公会祭神列表接口
        ///</summary>
        public const Int16 Cst_Action6011 = 6011;

        ///<summary>
        ///公会成员祭神上香接口
        ///</summary>
        public const Int16 Cst_Action6012 = 6012;

        ///<summary>
        ///公会封魔列表接口
        ///</summary>
        public const Int16 Cst_Action6013 = 6013;

        ///<summary>
        ///公会成员加入封魔接口
        ///</summary>
        public const Int16 Cst_Action6014 = 6014;

        ///<summary>
        ///公会成员召唤散仙接口
        ///</summary>
        public const Int16 Cst_Action6015 = 6015;

        ///<summary>
        ///清空会员申请接口
        ///</summary>
        public const Int16 Cst_Action6016 = 6016;

        ///<summary>
        ///创建公会接口
        ///</summary>
        public const Int16 Cst_Action6017 = 6017;

        ///<summary>
        ///驱除会员接口
        ///</summary>
        public const Int16 Cst_Action6018 = 6018;

        ///<summary>
        ///公会申请审核通过接口
        ///</summary>
        public const Int16 Cst_Action6019 = 6019;

        ///<summary>
        ///公会申请取消接口
        ///</summary>
        public const Int16 Cst_Action6020 = 6020;

        ///<summary>
        ///公会副会长列表接口
        ///</summary>
        public const Int16 Cst_Action6021 = 6021;

        ///<summary>
        ///封魔召集接口
        ///</summary>
        public const Int16 Cst_Action6022 = 6022;

        ///<summary>
        ///公会改名接口
        ///</summary>
        public const Int16 Cst_Action6023 = 6023;

        ///<summary>
        ///公会增加成员上限
        ///</summary>
        public const Int16 Cst_Action6024 = 6024;

        ///<summary>
        ///Boss战报参加接口
        ///</summary>
        public const Int16 Cst_Action6101 = 6101;

        ///<summary>
        ///Boss战报鼓舞接口
        ///</summary>
        public const Int16 Cst_Action6102 = 6102;

        ///<summary>
        ///Boss战欲火重生接口
        ///</summary>
        public const Int16 Cst_Action6103 = 6103;

        ///<summary>
        ///Boss战血量刷新接口
        ///</summary>
        public const Int16 Cst_Action6104 = 6104;

        ///<summary>
        ///Boss战战斗详情接口
        ///</summary>
        public const Int16 Cst_Action6105 = 6105;

        ///<summary>
        ///Boss战报伤害排名接口
        ///</summary>
        public const Int16 Cst_Action6106 = 6106;

        ///<summary>
        ///Boss战退出接口
        ///</summary>
        public const Int16 Cst_Action6107 = 6107;

        ///<summary>
        ///Boss战时间列表接口
        ///</summary>
        public const Int16 Cst_Action6108 = 6108;

        ///<summary>
        ///Boss战时间设定接口
        ///</summary>
        public const Int16 Cst_Action6109 = 6109;

        /// <summary>
        /// 公会技能列表接口
        /// </summary>
        public const Int16 Cst_Action6201 = 6201;

        /// <summary>
        /// 
        /// </summary>
        public const Int16 Cst_Action6202 = 6202;

        /// <summary>
        /// 
        /// </summary>
        public const Int16 Cst_Action6203 = 6203;

        /// <summary>
        /// 
        /// </summary>
        public const Int16 Cst_Action6204 = 6204;

        /// <summary>
        /// 
        /// </summary>
        public const Int16 Cst_Action6205 = 6205;

        /// <summary>
        /// 
        /// </summary>
        public const Int16 Cst_Action6206 = 6206;

        ///<summary>
        ///参加工会晨练
        ///</summary>
        public const Int16 Cst_Action6301 = 6301;

        ///<summary>
        ///获取题目
        ///</summary>
        public const Int16 Cst_Action6302 = 6302;

        ///<summary>
        ///获取所有玩家结果接口
        ///</summary>
        public const Int16 Cst_Action6303 = 6303;

        ///<summary>
        ///自动答题
        ///</summary>
        public const Int16 Cst_Action6304 = 6304;

        ///<summary>
        ///答题接口
        ///</summary>
        public const Int16 Cst_Action6305 = 6305;

        ///<summary>
        ///报名城市列表接口
        ///</summary>
        public const Int16 Cst_Action6401 = 6401;

        ///<summary>
        ///报名城市需求接口
        ///</summary>
        public const Int16 Cst_Action6402 = 6402;

        ///<summary>
        ///已报名公会列表接口
        ///</summary>
        public const Int16 Cst_Action6403 = 6403;

        ///<summary>
        ///公会报名接口
        ///</summary>
        public const Int16 Cst_Action6404 = 6404;

        ///<summary>
        ///设置旗帜接口
        ///</summary>
        public const Int16 Cst_Action6405 = 6405;

        ///<summary>
        ///公会战报界面接口
        ///</summary>
        public const Int16 Cst_Action6406 = 6406;

        ///<summary>
        ///详细战报列表接口
        ///</summary>
        public const Int16 Cst_Action6407 = 6407;

        ///<summary>
        ///详细战报列表接口
        ///</summary>
        public const Int16 Cst_Action6408 = 6408;

        ///<summary>
        ///争夺战成员列表接口
        ///</summary>
        public const Int16 Cst_Action6409 = 6409;

        ///<summary>
        ///争夺战鼓舞接口
        ///</summary>
        public const Int16 Cst_Action6410 = 6410;

        ///<summary>
        ///争夺战退出接口
        ///</summary>
        public const Int16 Cst_Action6411 = 6411;

        ///<summary>
        ///争夺战参战接口
        ///</summary>
        public const Int16 Cst_Action6412 = 6412;

        ///<summary>
        ///报名界面接口
        ///</summary>
        public const Int16 Cst_Action6501 = 6501;

        ///<summary>
        ///报名接口
        ///</summary>
        public const Int16 Cst_Action6502 = 6502;

        ///<summary>
        ///对战列表接口
        ///</summary>
        public const Int16 Cst_Action6503 = 6503;

        ///<summary>
        ///战绩表
        ///</summary>
        public const Int16 Cst_Action6504 = 6504;

        ///<summary>
        ///报名界面接口
        ///</summary>
        public const Int16 Cst_Action6505 = 6505;

        ///<summary>
        ///报名接口
        ///</summary>
        public const Int16 Cst_Action6506 = 6506;

        ///<summary>
        ///对战列表接口
        ///</summary>
        public const Int16 Cst_Action6507 = 6507;

        ///<summary>
        ///战绩表
        ///</summary>
        public const Int16 Cst_Action6508 = 6508;

        ///<summary>
        ///战绩表
        ///</summary>
        public const Int16 Cst_Action6509 = 6509;

        ///<summary>
        ///战绩表
        ///</summary>
        public const Int16 Cst_Action6510 = 6510;

        ///<summary>
        ///报名列表
        ///</summary>
        public const Int16 Cst_Action6511 = 6511;

        ///<summary>
        ///历史列表
        ///</summary>
        public const Int16 Cst_Action6512 = 6512;

        ///<summary>
        ///8强列表
        ///</summary>
        public const Int16 Cst_Action6513 = 6513;

        ///<summary>
        ///商店物品列表接口
        ///</summary>
        public const Int16 Cst_Action7001 = 7001;

        ///<summary>
        ///神秘商店物品列表接口
        ///</summary>
        public const Int16 Cst_Action7002 = 7002;

        ///<summary>
        ///背包售出物品列表接口
        ///</summary>
        public const Int16 Cst_Action7003 = 7003;

        ///<summary>
        ///商店物品购买接口
        ///</summary>
        public const Int16 Cst_Action7004 = 7004;

        ///<summary>
        ///神秘商店物品购买接口
        ///</summary>
        public const Int16 Cst_Action7005 = 7005;

        ///<summary>
        ///背包物品出售接口
        ///</summary>
        public const Int16 Cst_Action7006 = 7006;

        ///<summary>
        ///神秘商店晶石刷新接口
        ///</summary>
        public const Int16 Cst_Action7007 = 7007;

        ///<summary>
        ///玩家售出物品购买接口
        ///</summary>
        public const Int16 Cst_Action7008 = 7008;

        ///<summary>
        ///物品详情接口
        ///</summary>
        public const Int16 Cst_Action7009 = 7009;

        ///<summary>
        ///物品配置信息下发接口
        ///</summary>
        public const Int16 Cst_Action7010 = 7010;

        ///<summary>
        ///竞技场道具列表
        ///</summary>
        public const Int16 Cst_Action7011 = 7011;

        ///<summary>
        ///竞技场道具购买
        ///</summary>
        public const Int16 Cst_Action7012 = 7012;

        ///<summary>
        ///充值礼包列表接口
        ///</summary>
        public const Int16 Cst_Action9003 = 9003;

        ///<summary>
        ///充值礼包详情接口
        ///</summary>
        public const Int16 Cst_Action9004 = 9004;

        ///<summary>
        ///充值礼包领取接口
        ///</summary>
        public const Int16 Cst_Action9005 = 9005;

        ///<summary>
        ///好友列表接口
        ///</summary>
        public const Int16 Cst_Action9101 = 9101;

        ///<summary>
        ///最近联系列表接口
        ///</summary>
        public const Int16 Cst_Action9102 = 9102;

        ///<summary>
        ///添加好友接口
        ///</summary>
        public const Int16 Cst_Action9103 = 9103;

        ///<summary>
        ///删除好友接口
        ///</summary>
        public const Int16 Cst_Action9104 = 9104;

        ///<summary>
        ///加入黑名单列表接口
        ///</summary>
        public const Int16 Cst_Action9105 = 9105;

        ///<summary>
        ///删除最近联系人接口
        ///</summary>
        public const Int16 Cst_Action9106 = 9106;

        ///<summary>
        ///私聊发送接口
        ///</summary>
        public const Int16 Cst_Action9201 = 9201;

        ///<summary>
        ///公告列表接口
        ///</summary>
        public const Int16 Cst_Action9202 = 9202;

        ///<summary>
        ///聊天发送接口
        ///</summary>
        public const Int16 Cst_Action9203 = 9203;

        ///<summary>
        ///聊天列表接口
        ///</summary>
        public const Int16 Cst_Action9204 = 9204;

        ///<summary>
        ///系统广播接口
        ///</summary>
        public const Int16 Cst_Action9205 = 9205;

        ///<summary>
        ///庄园土地列表接口
        ///</summary>
        public const Int16 Cst_Action10001 = 10001;

        ///<summary>
        ///庄园种植佣兵列表接口
        ///</summary>
        public const Int16 Cst_Action10002 = 10002;

        ///<summary>
        ///庄园种植植物品质列表接口
        ///</summary>
        public const Int16 Cst_Action10003 = 10003;

        ///<summary>
        ///庄园植物种植接口
        ///</summary>
        public const Int16 Cst_Action10004 = 10004;

        ///<summary>
        ///庄园植物品质刷新接口
        ///</summary>
        public const Int16 Cst_Action10005 = 10005;

        ///<summary>
        ///庄园种植收获接口
        ///</summary>
        public const Int16 Cst_Action10006 = 10006;

        ///<summary>
        ///庄园冷却加速接口
        ///</summary>
        public const Int16 Cst_Action10007 = 10007;

        ///<summary>
        ///开启土地接口
        ///</summary>
        public const Int16 Cst_Action10008 = 10008;

        ///<summary>
        ///购买仙露接口
        ///</summary>
        public const Int16 Cst_Action10009 = 10009;

        ///<summary>
        ///升级红土地接口
        ///</summary>
        public const Int16 Cst_Action10010 = 10010;

        ///<summary>
        ///升级黑土地接口
        ///</summary>
        public const Int16 Cst_Action10011 = 10011;

        ///<summary>
        ///探险答题接口
        ///</summary>
        public const Int16 Cst_Action11001 = 11001;

        ///<summary>
        ///完成探险答题接口
        ///</summary>
        public const Int16 Cst_Action11002 = 11002;

        ///<summary>
        ///探险答题冷却加速接口
        ///</summary>
        public const Int16 Cst_Action11003 = 11003;

        ///<summary>
        ///幸运转盘界面接口
        ///</summary>
        public const Int16 Cst_Action12001 = 12001;

        ///<summary>
        ///我的宝藏列表接口
        ///</summary>
        public const Int16 Cst_Action12002 = 12002;

        ///<summary>
        ///探宝记录接口
        ///</summary>
        public const Int16 Cst_Action12003 = 12003;

        ///<summary>
        ///抽奖接口
        ///</summary>
        public const Int16 Cst_Action12004 = 12004;

        ///<summary>
        ///新手礼包列表接口
        ///</summary>
        public const Int16 Cst_Action13001 = 13001;

        ///<summary>
        ///领取新手礼包接口
        ///</summary>
        public const Int16 Cst_Action13002 = 13002;


    }
}