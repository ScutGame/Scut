
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

select 'create table successfully!';
