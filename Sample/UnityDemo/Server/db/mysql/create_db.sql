
DROP database IF EXISTS `PHData`;
create database `PHData` DEFAULT CHARACTER SET gbk COLLATE gbk_chinese_ci;

use PHData;

/*grant user*/
Delete FROM mysql.user Where User='game_user';
grant select,insert,update,delete,create,alter,drop on *.* to game_user@"%" Identified by "123";
