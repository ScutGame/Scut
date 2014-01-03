SyncSDK
=================

SyncSDK 使用
----------------
> 1.复制SyncSDK目录至游戏项目的Client/lua目录下；
> 2.在lua目录下的mainapp.lua文件中修改
<ul>
    <li>导入ScutDataSync.lua文件，require("SyncSDK.ScutDataSync")
    <li>在table变量strSubDirs增加"SyncSDK"搜索目录
    <li>注册服务器push回调，CCDirector:sharedDirector():RegisterSocketPushHandler("ScutDataSync.PushReceiverCallback");
    <li>文件结尾增加注册Sync通知监听回调处理，如：ScutDataSync.registerSceneCallback(ScutEntityListener.NotifySceneLayer)
</ul>