
/*create databases*/
DROP database IF EXISTS `contractdb`;
create database `contractdb` DEFAULT CHARACTER SET gbk COLLATE gbk_chinese_ci;

select 'create databases:contractdb successfull!';

use contractdb;
/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50168
Source Host           : localhost:3306
Source Database       : contractdb

Target Server Type    : MYSQL
Target Server Version : 50168
File Encoding         : 65001

Date: 2013-12-16 15:38:28
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `agreementclass`
-- ----------------------------
DROP TABLE IF EXISTS `agreementclass`;
CREATE TABLE `agreementclass` (
  `AgreementID` int(11) NOT NULL AUTO_INCREMENT,
  `GameID` int(11) DEFAULT NULL,
  `Title` varchar(200) DEFAULT NULL,
  `Describe` longtext,
  `CreateDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`AgreementID`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of agreementclass
-- ----------------------------

-- ----------------------------
-- Table structure for `contract`
-- ----------------------------
DROP TABLE IF EXISTS `contract`;
CREATE TABLE `contract` (
  `ID` int(11) NOT NULL,
  `Descption` varchar(100) DEFAULT NULL,
  `ParentID` int(11) DEFAULT NULL,
  `SlnID` int(11) NOT NULL,
  `Complated` tinyint(4) DEFAULT NULL,
  `AgreementID` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`,`SlnID`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of contract
-- ----------------------------

-- ----------------------------
-- Table structure for `enuminfo`
-- ----------------------------
DROP TABLE IF EXISTS `enuminfo`;
CREATE TABLE `enuminfo` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `SlnID` int(11) DEFAULT NULL,
  `enumName` varchar(50) DEFAULT NULL,
  `enumDescription` varchar(200) DEFAULT NULL,
  `enumValueInfo` longtext,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of enuminfo
-- ----------------------------

-- ----------------------------
-- Table structure for `paraminfo`
-- ----------------------------
DROP TABLE IF EXISTS `paraminfo`;
CREATE TABLE `paraminfo` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `ContractID` int(11) DEFAULT NULL,
  `ParamType` smallint(6) DEFAULT NULL,
  `Field` varchar(30) DEFAULT NULL,
  `FieldType` smallint(6) DEFAULT NULL,
  `Descption` varchar(100) DEFAULT NULL,
  `FieldValue` varchar(100) DEFAULT NULL,
  `Required` tinyint(4) DEFAULT NULL,
  `Remark` varchar(255) DEFAULT NULL,
  `SortID` int(11) DEFAULT NULL,
  `Creator` int(11) DEFAULT NULL,
  `CreateDate` datetime NOT NULL,
  `Modifier` int(11) DEFAULT NULL,
  `ModifyDate` datetime NOT NULL,
  `SlnID` int(11) DEFAULT NULL,
  `MinValue` int(11) DEFAULT NULL,
  `MaxValue` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of paraminfo
-- ----------------------------

-- ----------------------------
-- Table structure for `solutions`
-- ----------------------------
DROP TABLE IF EXISTS `solutions`;
CREATE TABLE `solutions` (
  `SlnID` int(11) NOT NULL AUTO_INCREMENT,
  `SlnName` varchar(100) DEFAULT NULL,
  `Namespace` varchar(200) DEFAULT NULL,
  `RefNamespace` varchar(200) DEFAULT NULL,
  `Url` varchar(200) DEFAULT NULL,
  `GameID` int(11) DEFAULT NULL,
  PRIMARY KEY (`SlnID`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of solutions
-- ----------------------------


select 'create table successfully!';
