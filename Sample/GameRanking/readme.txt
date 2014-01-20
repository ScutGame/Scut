《游戏排行榜Demo》提交玩家名字与分数到游戏服务器，并获取所有玩家分数排行列表

目录构成：

Client：客户端源代码（lua）
    release: http版本，对应server目录下的release版本
    release_socket: socket版本，对应server目录下的release_socket版本，scutsdk默认例子
                    外网布有该例子，可以直接运行

Server：服务器源代码
    
    release: http版本，对应client目录下的release版本，
             使用：架设IIS站点"ph.scutgame.com"，并设置Host：127.0.0.1 ph.scutgame.com
    release_socket: socket版本，对应client目录下的release_socket版本
