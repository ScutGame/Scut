
/*create databases*/
DROP database IF EXISTS `snscenter`;
create database `snscenter` DEFAULT CHARACTER SET gbk COLLATE gbk_chinese_ci;
DROP database IF EXISTS `paydb`;
create database `paydb` DEFAULT CHARACTER SET gbk COLLATE gbk_chinese_ci;

select 'create databases:snscenter,paydb successfull!';

/*grant user*/
Delete FROM mysql.user Where User='game_user';
grant select,insert,update,delete,create,alter,drop on *.* to game_user@"%" Identified by "123";

select 'grant user successfull!';

use paydb;
/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50168
Source Host           : localhost:3306
Source Database       : paydb

Target Server Type    : MYSQL
Target Server Version : 50168
File Encoding         : 65001

Date: 2013-11-22 16:01:01
*/

set names utf8;
SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `configretailerinfo`
-- ----------------------------
DROP TABLE IF EXISTS `configretailerinfo`;
CREATE TABLE `configretailerinfo` (
  `RetailerId` varchar(20) NOT NULL,
  `RetailerName` varchar(50) DEFAULT NULL,
  `Percentage` decimal(8,4) DEFAULT NULL,
  `DeveloperID` int(11) DEFAULT NULL,
  `DeveloperName` varchar(50) DEFAULT NULL,
  `DeveloperDate` datetime DEFAULT NULL,
  `MerchandiserID` int(11) DEFAULT NULL,
  `MerchandiserName` varchar(50) DEFAULT NULL,
  `MerchandiserDate` datetime DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `TaxRate` decimal(8,4) DEFAULT NULL,
  `PayPercentage` decimal(8,4) DEFAULT NULL,
  PRIMARY KEY (`RetailerId`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of configretailerinfo
-- ----------------------------

-- ----------------------------
-- Table structure for `gameinfo`
-- ----------------------------
DROP TABLE IF EXISTS `gameinfo`;
CREATE TABLE `gameinfo` (
  `GameID` int(11) NOT NULL,
  `GameName` varchar(20) DEFAULT NULL,
  `Currency` varchar(20) DEFAULT NULL,
  `Multiple` decimal(8,2) DEFAULT NULL,
  `GameWord` varchar(100) DEFAULT NULL,
  `AgentsID` varchar(20) DEFAULT NULL,
  `IsRelease` tinyint(4) DEFAULT NULL,
  `ReleaseDate` datetime DEFAULT NULL,
  `PayStyle` varchar(255) DEFAULT NULL,
  `SocketServer` varchar(100) DEFAULT NULL,
  `SocketPort` int(11) DEFAULT NULL,
  PRIMARY KEY (`GameID`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of gameinfo
-- ----------------------------
INSERT INTO `gameinfo` VALUES ('6', '口袋天界', '晶石', '10.00', 'KDTJ', '0000', '1', '1753-01-01 00:00:00', null, null, '0');
INSERT INTO `gameinfo` VALUES ('7', '斗地主', '元宝', '10.00', 'DDZ', '0000', '1', '1753-01-01 00:00:00', null, null, '0');

-- ----------------------------
-- Table structure for `gmfeedback`
-- ----------------------------
DROP TABLE IF EXISTS `gmfeedback`;
CREATE TABLE `gmfeedback` (
  `GMID` int(11) NOT NULL AUTO_INCREMENT,
  `UId` int(11) DEFAULT NULL,
  `GameID` int(11) DEFAULT NULL,
  `ServerID` int(11) DEFAULT NULL,
  `GMType` int(11) DEFAULT NULL,
  `content` varchar(255) DEFAULT NULL,
  `SubmittedTime` datetime DEFAULT NULL,
  `RContent` varchar(255) DEFAULT NULL,
  `ReplyTime` datetime DEFAULT NULL,
  `ReplyID` int(11) DEFAULT NULL,
  `Pid` varchar(20) DEFAULT NULL,
  `NickName` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`GMID`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of gmfeedback
-- ----------------------------

-- ----------------------------
-- Table structure for `orderinfo`
-- ----------------------------
DROP TABLE IF EXISTS `orderinfo`;
CREATE TABLE `orderinfo` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `OrderNO` varchar(100) DEFAULT NULL,
  `MerchandiseName` varchar(100) DEFAULT NULL,
  `PayType` varchar(20) DEFAULT NULL,
  `Amount` decimal(16,4) DEFAULT NULL,
  `Currency` varchar(10) DEFAULT NULL,
  `Expand` varchar(200) DEFAULT NULL,
  `SerialNumber` varchar(200) DEFAULT NULL,
  `PassportID` varchar(100) DEFAULT NULL,
  `ServerID` int(11) DEFAULT NULL,
  `GameID` int(11) DEFAULT NULL,
  `gameName` varchar(100) DEFAULT NULL,
  `ServerName` varchar(100) DEFAULT NULL,
  `PayStatus` int(11) DEFAULT NULL,
  `Signature` varchar(100) DEFAULT NULL,
  `Remarks` longtext,
  `GameCoins` int(11) DEFAULT NULL,
  `SendState` int(11) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `SendDate` datetime DEFAULT NULL,
  `RetailID` varchar(50) DEFAULT NULL,
  `DeviceID` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of orderinfo
-- ----------------------------

-- ----------------------------
-- Table structure for `payinfo`
-- ----------------------------
DROP TABLE IF EXISTS `payinfo`;
CREATE TABLE `payinfo` (
  `PayId` varchar(20) NOT NULL,
  `PayName` varchar(50) DEFAULT NULL,
  `Percentage` decimal(8,4) DEFAULT NULL,
  `ParentID` int(11) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`PayId`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of payinfo
-- ----------------------------

-- ----------------------------
-- Table structure for `sensitiveword`
-- ----------------------------
DROP TABLE IF EXISTS `sensitiveword`;
CREATE TABLE `sensitiveword` (
  `Code` int(11) NOT NULL,
  `Word` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Code`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of sensitiveword
-- ----------------------------

-- ----------------------------
-- Table structure for `serverinfo`
-- ----------------------------
DROP TABLE IF EXISTS `serverinfo`;
CREATE TABLE `serverinfo` (
  `ID` int(11) NOT NULL,
  `GameID` int(11) NOT NULL,
  `ServerName` varchar(20) DEFAULT NULL,
  `BaseUrl` varchar(200) DEFAULT NULL,
  `ActiveNum` int(11) DEFAULT NULL,
  `Weight` int(11) DEFAULT NULL,
  `IsEnable` tinyint(4) DEFAULT NULL,
  `TargetServer` int(11) DEFAULT NULL,
  `EnableDate` datetime DEFAULT NULL,
  `IntranetAddress` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`ID`,`GameID`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of serverinfo
-- ----------------------------
INSERT INTO `serverinfo` VALUES ('1', '6', '口袋一服', 'http://kd1.scutgame.com/Service.aspx', '0', '0', '1', '1', '1753-01-01 00:00:00', 'http://kd1.scutgame.com/Service.aspx');
INSERT INTO `serverinfo` VALUES ('1', '7', '斗地主一服', 'ddz.scutgame.com:9700', '0', '0', '1', '1', '1753-01-01 00:00:00', 'ddz.scutgame.com:9700');

-- ----------------------------
-- Table structure for `settlement`
-- ----------------------------
DROP TABLE IF EXISTS `settlement`;
CREATE TABLE `settlement` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `RetailID` varchar(50) DEFAULT NULL,
  `Amount` decimal(16,4) DEFAULT NULL,
  `GameID` int(11) DEFAULT NULL,
  `CreatYear` int(11) DEFAULT NULL,
  `CreatMouth` int(11) DEFAULT NULL,
  `SettlementState` int(11) DEFAULT NULL,
  `SettlementTime` datetime DEFAULT NULL,
  `Settlementer` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of settlement
-- ----------------------------



use snscenter;
/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50168
Source Host           : localhost:3306
Source Database       : snscenter

Target Server Type    : MYSQL
Target Server Version : 50168
File Encoding         : 65001

Date: 2013-11-22 16:01:11
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `limitdevice`
-- ----------------------------
DROP TABLE IF EXISTS `limitdevice`;
CREATE TABLE `limitdevice` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `DeviceID` varchar(50) DEFAULT NULL,
  `AppTime` datetime DEFAULT NULL,
  `Remark` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of limitdevice
-- ----------------------------

-- ----------------------------
-- Table structure for `loguserlogin`
-- ----------------------------
DROP TABLE IF EXISTS `loguserlogin`;
CREATE TABLE `loguserlogin` (
  `LogID` bigint(20) NOT NULL AUTO_INCREMENT,
  `UserID` bigint(20) DEFAULT NULL,
  `LogTime` datetime DEFAULT NULL,
  `IPAddr` varchar(15) DEFAULT NULL,
  `LogType` smallint(6) DEFAULT NULL,
  PRIMARY KEY (`LogID`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of loguserlogin
-- ----------------------------

-- ----------------------------
-- Table structure for `passportloginlog`
-- ----------------------------
DROP TABLE IF EXISTS `passportloginlog`;
CREATE TABLE `passportloginlog` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `DeviceID` varchar(50) DEFAULT NULL,
  `PassportID` varchar(50) DEFAULT NULL,
  `LoginTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of passportloginlog
-- ----------------------------

-- ----------------------------
-- Table structure for `snspassportlog`
-- ----------------------------
DROP TABLE IF EXISTS `snspassportlog`;
CREATE TABLE `snspassportlog` (
  `PassportID` int(11) NOT NULL AUTO_INCREMENT,
  `CreateTime` datetime DEFAULT NULL,
  `Mark` int(11) DEFAULT NULL,
  `RegPushTime` datetime DEFAULT NULL,
  `RegTime` datetime DEFAULT NULL,
  PRIMARY KEY (`PassportID`)
) ENGINE=InnoDB AUTO_INCREMENT=10000 DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of snspassportlog
-- ----------------------------

-- ----------------------------
-- Table structure for `snsuserinfo`
-- ----------------------------
DROP TABLE IF EXISTS `snsuserinfo`;
CREATE TABLE `snsuserinfo` (
  `UserId` int(11) NOT NULL AUTO_INCREMENT,
  `PassportID` varchar(32) DEFAULT NULL,
  `PassportPwd` varchar(50) DEFAULT NULL,
  `DeviceID` varchar(50) DEFAULT NULL,
  `RegType` smallint(6) DEFAULT NULL,
  `RegTime` datetime DEFAULT NULL,
  `RetailID` varchar(50) DEFAULT NULL,
  `RetailUser` varchar(50) DEFAULT NULL,
  `Mobile` varchar(12) DEFAULT NULL,
  `Mail` varchar(50) DEFAULT NULL,
  `PwdType` int(11) DEFAULT NULL,
  `RealName` varchar(20) DEFAULT NULL,
  `IDCards` varchar(20) DEFAULT NULL,
  `ActiveCode` char(10) DEFAULT NULL,
  `SendActiveDate` datetime DEFAULT NULL,
  `ActiveDate` datetime DEFAULT NULL,
  `WeixinCode` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=1380000 DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of snsuserinfo
-- ----------------------------

-- ----------------------------
-- Table structure for `userloginlog`
-- ----------------------------
DROP TABLE IF EXISTS `userloginlog`;
CREATE TABLE `userloginlog` (
  `SessionID` bigint(20) NOT NULL AUTO_INCREMENT,
  `UserID` bigint(20) DEFAULT NULL,
  `AddTime` datetime DEFAULT NULL,
  `Md5Hash` varchar(50) DEFAULT NULL,
  `Stat` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`SessionID`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of userloginlog
-- ----------------------------

select 'create center table successfully!';



/*create databases*/
DROP database IF EXISTS `ddzconfig`;
create database `ddzconfig` DEFAULT CHARACTER SET gbk COLLATE gbk_chinese_ci;

DROP database IF EXISTS `ddz1data`;
create database `ddz1data` DEFAULT CHARACTER SET gbk COLLATE gbk_chinese_ci;

DROP database IF EXISTS `ddz1log`;
create database `ddz1log` DEFAULT CHARACTER SET gbk COLLATE gbk_chinese_ci;

select 'create databases:ddzconfig,ddz1data,ddz1log successfull!';

use ddzconfig;
set names utf8;

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `achievementinfo`
-- ----------------------------
DROP TABLE IF EXISTS `achievementinfo`;
CREATE TABLE `achievementinfo` (
  `Id` int(11) NOT NULL,
  `Name` varchar(20) DEFAULT NULL,
  `Type` int(11) NOT NULL,
  `TargetNum` int(11) DEFAULT NULL,
  `HeadIcon` varchar(50) DEFAULT NULL,
  `Description` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of achievementinfo
-- ----------------------------
INSERT INTO `achievementinfo` VALUES ('1001', '萌妹铜牌', '2', '50', 'icon_1034', '成就描述：赢得50场萌妹斗地主胜利');
INSERT INTO `achievementinfo` VALUES ('1002', '萌妹银牌', '2', '150', 'icon_1035', '成就描述：赢得150场萌妹斗地主胜利');
INSERT INTO `achievementinfo` VALUES ('1003', '萌妹金牌', '2', '300', 'icon_1036', '成就描述：赢得300场萌妹斗地主胜利');
INSERT INTO `achievementinfo` VALUES ('1004', '萌妹白金', '2', '600', 'icon_1037', '成就描述：赢得600场萌妹斗地主胜利');
INSERT INTO `achievementinfo` VALUES ('1005', '萌妹铂金', '2', '1000', 'icon_1038', '成就描述：赢得1000场萌妹斗地主胜利');
INSERT INTO `achievementinfo` VALUES ('1006', '萌妹钻石', '2', '2000', 'icon_1039', '成就描述：赢得2000场萌妹斗地主胜利');
INSERT INTO `achievementinfo` VALUES ('1007', '萌妹大神', '2', '3500', 'icon_1040', '成就描述：赢得3500场萌妹斗地主胜利');
INSERT INTO `achievementinfo` VALUES ('1008', '一星至尊', '5', '500', 'icon_1041', '成就描述：累计充值500元宝');
INSERT INTO `achievementinfo` VALUES ('1009', '二星至尊', '5', '1500', 'icon_1042', '成就描述：累计充值1500元宝');
INSERT INTO `achievementinfo` VALUES ('1010', '三星至尊', '5', '3000', 'icon_1043', '成就描述：累计充值3000元宝');
INSERT INTO `achievementinfo` VALUES ('1011', '四星至尊', '5', '6000', 'icon_1044', '成就描述：累计充值6000元宝');
INSERT INTO `achievementinfo` VALUES ('1012', '五星至尊', '5', '8000', 'icon_1045', '成就描述：累计充值8000元宝');
INSERT INTO `achievementinfo` VALUES ('1013', '六星至尊', '5', '10000', 'icon_1046', '成就描述：累计充值10000元宝');
INSERT INTO `achievementinfo` VALUES ('1014', '七星至尊', '5', '20000', 'icon_1047', '成就描述：累计充值20000元宝');

-- ----------------------------
-- Table structure for `chatinfo`
-- ----------------------------
DROP TABLE IF EXISTS `chatinfo`;
CREATE TABLE `chatinfo` (
  `Id` int(11) NOT NULL,
  `Content` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of chatinfo
-- ----------------------------
INSERT INTO `chatinfo` VALUES ('1001', '苍天啊，给我一个出牌的机会吧！');
INSERT INTO `chatinfo` VALUES ('1002', '哎，我说你丫能快点吗？');
INSERT INTO `chatinfo` VALUES ('1003', '出啊，好牌都留着下蛋啊！');
INSERT INTO `chatinfo` VALUES ('1004', '地主，我和你拼了!');
INSERT INTO `chatinfo` VALUES ('1005', '和你搭档，真是伤不起啊！');
INSERT INTO `chatinfo` VALUES ('1006', '和你搭档，真是太愉快了！');
INSERT INTO `chatinfo` VALUES ('1007', '农民兄弟，不要欺负地主啊！');
INSERT INTO `chatinfo` VALUES ('1008', '兄弟，你打得这叫什么牌呀！');
INSERT INTO `chatinfo` VALUES ('1009', '咱们老百姓啊，今儿个真高兴！');
INSERT INTO `chatinfo` VALUES ('1010', '这副牌我赢定了！');
INSERT INTO `chatinfo` VALUES ('1011', '这牌发的，真是太郁闷了！');
INSERT INTO `chatinfo` VALUES ('1012', '这牌一好，心情就好！');

-- ----------------------------
-- Table structure for `configenvset`
-- ----------------------------
DROP TABLE IF EXISTS `configenvset`;
CREATE TABLE `configenvset` (
  `EnvKey` varchar(50) NOT NULL,
  `EnvValue` varchar(50) DEFAULT NULL,
  `EnvDesc` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`EnvKey`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of configenvset
-- ----------------------------
INSERT INTO `configenvset` VALUES ('Game.FleeMultipleNum', '10', '游戏逃跑最低倍数');
INSERT INTO `configenvset` VALUES ('Game.Table.AIFirstOutCardTime', '15000', '机器人一手出牌间隔时间');
INSERT INTO `configenvset` VALUES ('Game.Table.AIIntoTime', '10000', '机器人加入桌子时间间隔(毫秒)');
INSERT INTO `configenvset` VALUES ('Game.Table.AIOutCardTime', '5000', '机器人出牌间隔5000秒');
INSERT INTO `configenvset` VALUES ('Game.Table.MinTableCount', '10', '初始开出空桌数');
INSERT INTO `configenvset` VALUES ('Ranking.OfficeNumber', '100', '进胜率榜的总场数');
INSERT INTO `configenvset` VALUES ('User.DailyFreeNum', '3', '每日转盘活动免费次数');
INSERT INTO `configenvset` VALUES ('User.DailyGiffCoinTime', '1', '每是赠送金豆次数');
INSERT INTO `configenvset` VALUES ('User.GameCoin', '10000', '建角初始金豆');
INSERT INTO `configenvset` VALUES ('User.GiftGold', '50', '建角初始赠送元宝');
INSERT INTO `configenvset` VALUES ('User.Level', '1', '建角初始等级');
INSERT INTO `configenvset` VALUES ('User.MaxLength', '12', '昵称最大长度');
INSERT INTO `configenvset` VALUES ('User.MinLength', '4', '昵称最小长度');
INSERT INTO `configenvset` VALUES ('User.VipLv', '0', '建角初始VIP等级');

-- ----------------------------
-- Table structure for `dialinfo`
-- ----------------------------
DROP TABLE IF EXISTS `dialinfo`;
CREATE TABLE `dialinfo` (
  `ID` int(11) NOT NULL,
  `GameCoin` int(11) DEFAULT NULL,
  `HeadID` varchar(100) DEFAULT NULL,
  `Probability` double DEFAULT NULL,
  `ItemDesc` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of dialinfo
-- ----------------------------
INSERT INTO `dialinfo` VALUES ('1', '100', '', '0.3', '');
INSERT INTO `dialinfo` VALUES ('2', '200', '', '0.3', '');
INSERT INTO `dialinfo` VALUES ('3', '400', '', '0.2', '');
INSERT INTO `dialinfo` VALUES ('4', '600', '', '0.1', '');
INSERT INTO `dialinfo` VALUES ('5', '900', '', '0.05', '');
INSERT INTO `dialinfo` VALUES ('6', '1200', '', '0.05', '');
INSERT INTO `dialinfo` VALUES ('7', '2400', '', '0.04', '');
INSERT INTO `dialinfo` VALUES ('8', '4000', '', '0.03', '');
INSERT INTO `dialinfo` VALUES ('9', '6000', '', '0.02', '');
INSERT INTO `dialinfo` VALUES ('10', '10000', '', '0.01', '');

-- ----------------------------
-- Table structure for `pokerinfo`
-- ----------------------------
DROP TABLE IF EXISTS `pokerinfo`;
CREATE TABLE `pokerinfo` (
  `Id` int(11) NOT NULL,
  `Name` varchar(20) DEFAULT NULL,
  `Color` int(11) DEFAULT NULL,
  `Value` smallint(6) DEFAULT NULL,
  `HeadIcon` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of pokerinfo
-- ----------------------------
INSERT INTO `pokerinfo` VALUES ('103', '黑桃3', '1', '3', 'card_1066');
INSERT INTO `pokerinfo` VALUES ('104', '黑桃4', '1', '4', 'card_1070');
INSERT INTO `pokerinfo` VALUES ('105', '黑桃5', '1', '5', 'card_1074');
INSERT INTO `pokerinfo` VALUES ('106', '黑桃6', '1', '6', 'card_1078');
INSERT INTO `pokerinfo` VALUES ('107', '黑桃7', '1', '7', 'card_1082');
INSERT INTO `pokerinfo` VALUES ('108', '黑桃8', '1', '8', 'card_1086');
INSERT INTO `pokerinfo` VALUES ('109', '黑桃9', '1', '9', 'card_1090');
INSERT INTO `pokerinfo` VALUES ('110', '黑桃10', '1', '10', 'card_1094');
INSERT INTO `pokerinfo` VALUES ('111', '黑桃J', '1', '11', 'card_1098');
INSERT INTO `pokerinfo` VALUES ('112', '黑桃Q', '1', '12', 'card_1102');
INSERT INTO `pokerinfo` VALUES ('113', '黑桃K', '1', '13', 'card_1106');
INSERT INTO `pokerinfo` VALUES ('114', '黑桃A', '1', '14', 'card_1110');
INSERT INTO `pokerinfo` VALUES ('115', '黑桃2', '1', '15', 'card_1062');
INSERT INTO `pokerinfo` VALUES ('203', '梅花3', '2', '3', 'card_1068');
INSERT INTO `pokerinfo` VALUES ('204', '梅花4', '2', '4', 'card_1072');
INSERT INTO `pokerinfo` VALUES ('205', '梅花5', '2', '5', 'card_1076');
INSERT INTO `pokerinfo` VALUES ('206', '梅花6', '2', '6', 'card_1080');
INSERT INTO `pokerinfo` VALUES ('207', '梅花7', '2', '7', 'card_1084');
INSERT INTO `pokerinfo` VALUES ('208', '梅花8', '2', '8', 'card_1088');
INSERT INTO `pokerinfo` VALUES ('209', '梅花9', '2', '9', 'card_1092');
INSERT INTO `pokerinfo` VALUES ('210', '梅花10', '2', '10', 'card_1096');
INSERT INTO `pokerinfo` VALUES ('211', '梅花J', '2', '11', 'card_1100');
INSERT INTO `pokerinfo` VALUES ('212', '梅花Q', '2', '12', 'card_1104');
INSERT INTO `pokerinfo` VALUES ('213', '梅花K', '2', '13', 'card_1108');
INSERT INTO `pokerinfo` VALUES ('214', '梅花A', '2', '14', 'card_1112');
INSERT INTO `pokerinfo` VALUES ('215', '梅花2', '2', '15', 'card_1064');
INSERT INTO `pokerinfo` VALUES ('303', '方片3', '3', '3', 'card_1065');
INSERT INTO `pokerinfo` VALUES ('304', '方片4', '3', '4', 'card_1069');
INSERT INTO `pokerinfo` VALUES ('305', '方片5', '3', '5', 'card_1073');
INSERT INTO `pokerinfo` VALUES ('306', '方片6', '3', '6', 'card_1077');
INSERT INTO `pokerinfo` VALUES ('307', '方片7', '3', '7', 'card_1081');
INSERT INTO `pokerinfo` VALUES ('308', '方片8', '3', '8', 'card_1085');
INSERT INTO `pokerinfo` VALUES ('309', '方片9', '3', '9', 'card_1089');
INSERT INTO `pokerinfo` VALUES ('310', '方片10', '3', '10', 'card_1093');
INSERT INTO `pokerinfo` VALUES ('311', '方片J', '3', '11', 'card_1097');
INSERT INTO `pokerinfo` VALUES ('312', '方片Q', '3', '12', 'card_1101');
INSERT INTO `pokerinfo` VALUES ('313', '方片K', '3', '13', 'card_1105');
INSERT INTO `pokerinfo` VALUES ('314', '方片A', '3', '14', 'card_1109');
INSERT INTO `pokerinfo` VALUES ('315', '方片2', '3', '15', 'card_1061');
INSERT INTO `pokerinfo` VALUES ('403', '红桃3', '4', '3', 'card_1067');
INSERT INTO `pokerinfo` VALUES ('404', '红桃4', '4', '4', 'card_1071');
INSERT INTO `pokerinfo` VALUES ('405', '红桃5', '4', '5', 'card_1075');
INSERT INTO `pokerinfo` VALUES ('406', '红桃6', '4', '6', 'card_1079');
INSERT INTO `pokerinfo` VALUES ('407', '红桃7', '4', '7', 'card_1083');
INSERT INTO `pokerinfo` VALUES ('408', '红桃8', '4', '8', 'card_1087');
INSERT INTO `pokerinfo` VALUES ('409', '红桃9', '4', '9', 'card_1091');
INSERT INTO `pokerinfo` VALUES ('410', '红桃10', '4', '10', 'card_1095');
INSERT INTO `pokerinfo` VALUES ('411', '红桃J', '4', '11', 'card_1099');
INSERT INTO `pokerinfo` VALUES ('412', '红桃Q', '4', '12', 'card_1103');
INSERT INTO `pokerinfo` VALUES ('413', '红桃K', '4', '13', 'card_1107');
INSERT INTO `pokerinfo` VALUES ('414', '红桃A', '4', '14', 'card_1111');
INSERT INTO `pokerinfo` VALUES ('415', '红桃2', '4', '15', 'card_1063');
INSERT INTO `pokerinfo` VALUES ('518', '小王', '5', '18', 'card_1114');
INSERT INTO `pokerinfo` VALUES ('619', '大王', '6', '19', 'card_1113');

-- ----------------------------
-- Table structure for `roominfo`
-- ----------------------------
DROP TABLE IF EXISTS `roominfo`;
CREATE TABLE `roominfo` (
  `Id` int(11) NOT NULL,
  `Name` varchar(20) DEFAULT NULL,
  `AnteNum` int(11) DEFAULT NULL,
  `MultipleNum` smallint(6) DEFAULT NULL,
  `MinGameCion` int(11) DEFAULT NULL,
  `GiffCion` int(11) DEFAULT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `PlayerNum` int(11) DEFAULT NULL,
  `CardPackNum` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of roominfo
-- ----------------------------
INSERT INTO `roominfo` VALUES ('1001', '一倍积分房', '400', '1', '1000', '1000', '', '3', '1');
INSERT INTO `roominfo` VALUES ('1002', '二倍积分房', '400', '2', '10000', '1000', '', '3', '1');
INSERT INTO `roominfo` VALUES ('1003', '四倍积分房', '600', '4', '60000', '1000', '', '3', '1');
INSERT INTO `roominfo` VALUES ('1004', '十倍积分房', '800', '10', '200000', '1000', '', '3', '1');

-- ----------------------------
-- Table structure for `shopinfo`
-- ----------------------------
DROP TABLE IF EXISTS `shopinfo`;
CREATE TABLE `shopinfo` (
  `ShopID` int(11) NOT NULL,
  `ShopName` varchar(20) DEFAULT NULL,
  `HeadID` varchar(100) DEFAULT NULL,
  `ShopType` int(11) DEFAULT NULL,
  `SeqNO` smallint(6) DEFAULT NULL,
  `Price` int(11) DEFAULT NULL,
  `VipPrice` int(11) DEFAULT NULL,
  `GameCoin` int(11) DEFAULT NULL,
  `ShopDesc` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`ShopID`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of shopinfo
-- ----------------------------
INSERT INTO `shopinfo` VALUES ('1', '白鼠', 'head_1006', '1', '1', '20', '0', '0', '');
INSERT INTO `shopinfo` VALUES ('2', '水牛', 'head_1007', '1', '2', '20', '0', '0', '');
INSERT INTO `shopinfo` VALUES ('3', '蠢虎', 'head_1008', '1', '3', '20', '0', '0', '');
INSERT INTO `shopinfo` VALUES ('4', '玉兔', 'head_1009', '1', '4', '20', '0', '0', '');
INSERT INTO `shopinfo` VALUES ('5', '神龙', 'head_1010', '1', '5', '20', '0', '0', '');
INSERT INTO `shopinfo` VALUES ('6', '青蛇', 'head_1011', '1', '6', '20', '0', '0', '');
INSERT INTO `shopinfo` VALUES ('7', '憨马', 'head_1012', '1', '7', '20', '0', '0', '');
INSERT INTO `shopinfo` VALUES ('8', '羊羊', 'head_1013', '1', '8', '20', '0', '0', '');
INSERT INTO `shopinfo` VALUES ('9', '灵猴', 'head_1014', '1', '9', '20', '0', '0', '');
INSERT INTO `shopinfo` VALUES ('10', '雄鸡', 'head_1015', '1', '10', '20', '0', '0', '');
INSERT INTO `shopinfo` VALUES ('11', '小哈', 'head_1016', '1', '11', '20', '0', '0', '');
INSERT INTO `shopinfo` VALUES ('12', '睡猪', 'head_1017', '1', '12', '20', '0', '0', '');
INSERT INTO `shopinfo` VALUES ('13', '小丑', 'head_1018', '1', '13', '50', '0', '0', '');
INSERT INTO `shopinfo` VALUES ('14', '公主', 'head_1019', '1', '14', '50', '0', '0', '');
INSERT INTO `shopinfo` VALUES ('15', '王子', 'head_1020', '1', '15', '50', '0', '0', '');
INSERT INTO `shopinfo` VALUES ('16', '小金豆', 'head_1021', '2', '16', '10', '0', '1000', '');
INSERT INTO `shopinfo` VALUES ('17', '中金豆', 'head_1022', '2', '17', '100', '0', '10000', '');
INSERT INTO `shopinfo` VALUES ('18', '大金豆', 'head_1023', '2', '18', '600', '0', '60000', '');
INSERT INTO `shopinfo` VALUES ('19', '特大金豆', 'head_1024', '2', '19', '2000', '0', '200000', '');
INSERT INTO `shopinfo` VALUES ('20', '至尊金豆', 'head_1025', '2', '20', '5000', '0', '500000', '');
INSERT INTO `shopinfo` VALUES ('21', '女王', 'head_1026', '1', '21', '70', '0', '0', '');
INSERT INTO `shopinfo` VALUES ('22', '精灵', 'head_1027', '1', '22', '70', '0', '0', '');
INSERT INTO `shopinfo` VALUES ('23', '女神', 'head_1028', '1', '23', '70', '0', '0', '');
INSERT INTO `shopinfo` VALUES ('24', '学生妹', 'head_1029', '1', '24', '70', '0', '0', '');

-- ----------------------------
-- Table structure for `taskinfo`
-- ----------------------------
DROP TABLE IF EXISTS `taskinfo`;
CREATE TABLE `taskinfo` (
  `TaskID` int(11) NOT NULL,
  `TaskName` varchar(50) DEFAULT NULL,
  `TaskType` int(11) DEFAULT NULL,
  `TaskClass` int(11) DEFAULT NULL,
  `RestraintNum` int(11) DEFAULT NULL,
  `AchieveID` int(11) DEFAULT NULL,
  `GameCoin` int(11) DEFAULT NULL,
  `TaskDesc` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`TaskID`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of taskinfo
-- ----------------------------
INSERT INTO `taskinfo` VALUES ('1001', '萌妹首胜', '1', '2', '1', '0', '800', '初出茅庐赢得首场萌妹斗地主胜利');
INSERT INTO `taskinfo` VALUES ('1002', '正常发挥', '1', '2', '3', '0', '1200', '赢得3场萌妹斗地主的胜利');
INSERT INTO `taskinfo` VALUES ('1003', '胜利旗帜', '1', '2', '10', '0', '2000', '赢得10场萌妹斗地主的胜利');
INSERT INTO `taskinfo` VALUES ('1004', '胜利喜悦', '1', '2', '15', '0', '2500', '赢得15场萌妹斗地主的胜利');
INSERT INTO `taskinfo` VALUES ('1005', '斗战胜佛', '1', '2', '30', '0', '4500', '赢得30场萌妹斗地主的胜利');
INSERT INTO `taskinfo` VALUES ('2001', '萌妹铜牌', '1', '2', '50', '1001', '200', '获得萌妹铜牌成就（需赢得50胜利）');
INSERT INTO `taskinfo` VALUES ('2002', '萌妹银牌', '1', '2', '150', '1002', '400', '获得萌妹银牌成就（需赢得150胜利）');
INSERT INTO `taskinfo` VALUES ('2003', '萌妹金牌', '1', '2', '300', '1003', '600', '获得萌妹金牌成就（需赢得300胜利）');
INSERT INTO `taskinfo` VALUES ('2004', '萌妹白金', '1', '2', '600', '1004', '800', '获得萌妹白金成就（需赢得600胜利）');
INSERT INTO `taskinfo` VALUES ('2005', '萌妹铂金', '1', '2', '1000', '1005', '1000', '获得萌妹铂金成就（需赢得1000胜利）');
INSERT INTO `taskinfo` VALUES ('2006', '萌妹钻石', '1', '2', '2000', '1006', '1200', '获得萌妹钻石成就（需赢得2000胜利）');
INSERT INTO `taskinfo` VALUES ('2007', '萌妹大神', '1', '2', '3500', '1007', '1400', '获得萌妹大神成就（需赢得3500胜利）');
INSERT INTO `taskinfo` VALUES ('2008', '一星至尊', '1', '5', '500', '1008', '2000', '获得一星至尊成就（需累充值500元宝）');
INSERT INTO `taskinfo` VALUES ('2009', '二星至尊', '1', '5', '1500', '1009', '3000', '获得二星至尊成就（需累充值1500元宝）');
INSERT INTO `taskinfo` VALUES ('2010', '三星至尊', '1', '5', '3000', '1010', '4000', '获得三星至尊成就（需累充值3000元宝）');
INSERT INTO `taskinfo` VALUES ('2011', '四星至尊', '1', '5', '6000', '1011', '5000', '获得四星至尊成就（需累充值6000元宝）');
INSERT INTO `taskinfo` VALUES ('2012', '五星至尊', '1', '5', '8000', '1012', '7000', '获得五星至尊成就（需累充值8000元宝）');
INSERT INTO `taskinfo` VALUES ('2013', '六星至尊', '1', '5', '10000', '1013', '8000', '获得六星至尊成就（需累充值10000元宝）');
INSERT INTO `taskinfo` VALUES ('2014', '七星至尊', '1', '5', '20000', '1014', '9000', '获得七星至尊成就（需累充值20000元宝）');
INSERT INTO `taskinfo` VALUES ('3001', '每日一胜', '2', '2', '1', '0', '400', '赢得今日第一次胜利');

-- ----------------------------
-- Table structure for `titleinfo`
-- ----------------------------
DROP TABLE IF EXISTS `titleinfo`;
CREATE TABLE `titleinfo` (
  `Id` int(11) NOT NULL,
  `Name` varchar(20) DEFAULT NULL,
  `TargetNum` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of titleinfo
-- ----------------------------
INSERT INTO `titleinfo` VALUES ('1001', '萌妹青铜组I级', '49');
INSERT INTO `titleinfo` VALUES ('1002', '萌妹青铜组II级', '99');
INSERT INTO `titleinfo` VALUES ('1003', '萌妹青铜组III级', '199');
INSERT INTO `titleinfo` VALUES ('1004', '萌妹白银组I级', '399');
INSERT INTO `titleinfo` VALUES ('1005', '萌妹白银组II级', '599');
INSERT INTO `titleinfo` VALUES ('1006', '萌妹白银组III级', '999');
INSERT INTO `titleinfo` VALUES ('1007', '萌妹黄金组I级', '1999');
INSERT INTO `titleinfo` VALUES ('1008', '萌妹黄金组II级', '3999');
INSERT INTO `titleinfo` VALUES ('1009', '萌妹黄金组III级', '5999');
INSERT INTO `titleinfo` VALUES ('1010', '萌妹铂金组I级', '9999');
INSERT INTO `titleinfo` VALUES ('1011', '萌妹铂金组II级', '29999');
INSERT INTO `titleinfo` VALUES ('1012', '萌妹铂金组III级', '99999');
INSERT INTO `titleinfo` VALUES ('1013', '萌妹钻石组I级', '199999');
INSERT INTO `titleinfo` VALUES ('1014', '萌妹钻石组II级', '499999');
INSERT INTO `titleinfo` VALUES ('1015', '萌妹钻石组III级', '999999');
INSERT INTO `titleinfo` VALUES ('1016', '萌妹大神组I级', '2999999');
INSERT INTO `titleinfo` VALUES ('1017', '萌妹大神组II级', '4999999');
INSERT INTO `titleinfo` VALUES ('1018', '萌妹大神组III级', '9999999');

select 'create tables for ddzconfig successfull!';
