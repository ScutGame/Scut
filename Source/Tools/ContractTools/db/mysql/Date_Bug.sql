use contractdb;

alter table `ParamInfo` modify column `CreateDate` datetime NOT NULL;
alter table `ParamInfo` modify column `ModifyDate`  datetime NOT NULL;