
DROP database IF EXISTS `GameData`;
create database `GameData` DEFAULT CHARACTER SET gbk COLLATE gbk_chinese_ci;

use GameData;

/*grant user*/
Delete FROM mysql.user Where User='game_user';
grant select,insert,update,delete,create,alter,drop on *.* to game_user@"%" Identified by "123";
