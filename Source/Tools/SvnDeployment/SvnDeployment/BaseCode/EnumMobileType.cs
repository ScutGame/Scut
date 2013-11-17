using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;

namespace CitGame
{ 
    public enum MobileType
    {
        //[Description("无")]
        None =0,
        //[Description("无")]
        IPhone,
        WM,
        SYMBIAN,
        ANDROID,
        OMS,
        M8
    }

    /// <summary>
    /// 奖励类型
    /// </summary>
    public enum ContainType
    {
        /// <summary>
        /// 物品-1
        /// </summary>
        物品 = 1,
        /// <summary>
        /// 经验-2
        /// </summary>
        经验,
        /// <summary>
        /// 金币-3
        /// </summary>
        金币,
        /// <summary>
        /// 米币-4
        /// </summary>
        米币,
        /// <summary>
        /// 随机种子-5
        /// </summary>
        随机种子
    }

    /// <summary>
    /// 住所类型
    /// </summary>
    public enum HouseType
    {
        /// <summary>
        /// 窝 - 1
        /// </summary>
        Nest = 1,
        /// <summary>
        /// 棚 - 2
        /// </summary>
        Cote = 2,
        /// <summary>
        /// 加工厂 - 3
        /// </summary>
        Factory
    }
    /// <summary>
    /// 经验获取类型枚举
    /// </summary>


    /// <summary>
    /// 牧场中动物可偷状态
    /// </summary>
    public enum ListAnimalStatus
    {
        /// <summary>
        /// 无 0
        /// </summary>
        Nothing=0,
        /// <summary>
        /// 可赶产 1
        /// </summary>
        PreYield,
        /// <summary>
        /// 可收获 2
        /// </summary>
        Autumn
    }
    /// <summary>
    /// 收获类型
    /// </summary>
    public enum HarvestType
    {
        /// <summary>
        /// 饲养收获
        /// </summary>
        feed=0,
        /// <summary>
        /// 偷取收获
        /// </summary>
        steal
    }
    /// <summary>
    /// 是否收获
    /// </summary>
    public enum HarvestMark
    {
        /// <summary>
        /// 未收获
        /// </summary>
        NoHarvest=0,
        /// <summary>
        /// 已经收获
        /// </summary>
        YesHarvest
    }


    /// <summary>
    /// 动物的饥饿状态
    /// </summary>
    public enum HungryStatus
    {
        /// <summary>
        /// 未饥饿
        /// </summary>
        Full = 0,
        /// <summary>
        /// 饥饿状态
        /// </summary>
        Hungry
    }

    /// <summary>
    /// 是否已收获  
    /// </summary>
    public enum IsGot
    {
        No = 0,
        Yes
    }

    /// <summary>
    /// 是否有蚊子
    /// </summary>
    public enum MosquitoStatus
    { 
        /// <summary>
        /// 无
        /// </summary>
      Nothing=0,
      /// <summary>
      /// 有蚊子
      /// </summary>
      HasMosquito
    }
    /// <summary>
    /// 是否有粪便
    /// </summary>
    public enum ShitStatus
    {
        /// <summary>
        /// 无
        /// </summary>
      Nothing=0,
        /// <summary>
        /// 有粪便
        /// </summary>
      HasShit
    }
    public enum GotExpType
    {
        /// <summary>
        /// 任务  0
        /// </summary>
        Task =0,
        /// <summary>
        /// 放养动物  1
        /// </summary>
        RaiseAnimal,
        /// <summary>
        /// 收获动物  2
        /// </summary>
        GotAnimal,
        /// <summary>
        /// 自己赶产  3
        /// </summary>
        YieldBySelf,
        /// <summary>
        /// 好友赶产 4
        /// </summary>
        YieldByFriend,
        /// <summary>
        /// 替好友赶产5
        /// </summary>
        YieldToFriend,
        /// <summary>
        /// 清理蚊子6
        /// </summary>
        CleanMosquito,
        /// <summary>
        /// 替好友加牧草7
        /// </summary>
        AddCribToFriend,
        /// <summary>
        /// 收获出产物8
        /// </summary>
        GotOutPutProdcut,
        /// <summary>
        /// 清理垃圾9
        /// </summary>
        CleanRubbish,
        /// <summary>
        /// 偷取（收获）好友出产物 10
        /// </summary>
        StealOutPutProduct, 
        /// <summary>
        /// 给自己牧场加牧草 11
        /// </summary>
        AddCribToSelf


    }
    /// <summary>
    /// 仓库中物品的类型定义
    /// </summary>
    public enum StorageType
    {
        /// <summary>
        /// 食物与其他道具 - 1
        /// </summary>
        ItemFood = 1,
        /// <summary>
        /// 出产物 - 2
        /// </summary>
        ItemOutput,
        /// <summary>
        /// 垃圾 - 3
        /// </summary>
        ItemRubbish,
        /// <summary>
        /// 成熟体 - 4
        /// </summary>
        ItemAnimalBasic,
        /// <summary>
        /// 未定义 - 99
        /// </summary>
        Undefined=99
    }
    /// <summary>
    /// 食物道具减少方式
    /// </summary>
    public enum EnumFoodUseType
    {
        SellToShop = 0,
        AddToCrib,
        Other
    }

    public enum FoodAddType
    {
        self = 0,
        friend
    }

    public enum SourcePathType
    {
        Head = 0,
        Model
    }

    /// <summary>
    /// 动物成长状态枚举
    /// </summary>
    public enum EnumAnimalGrowingStatus
    {
        /// <summary>
        ///  幼仔期
        /// </summary>
        Baby = 0,
        /// <summary>
        /// 成长期
        /// </summary>
        Growing,
        /// <summary>
        /// 待产期
        /// </summary>
        WaitYield,
        /// <summary>
        /// 赶产期
        /// </summary>
        PreYield,
        /// <summary>
        /// 生产期
        /// </summary>
        Yield,
        /// <summary>
        /// 成熟期，可收获
        /// </summary>
        CanGot
    }

    /// <summary>
    /// 金币增减记录类型
    /// </summary>
    public enum EnumGoldListType
    {
        /// <summary>
        /// 金币增加
        /// </summary>
        Raise =0,
        /// <summary>
        /// 金币减少
        /// </summary>
        Consume
    }

    public enum EnumGoldRaiseType
    {
        /// <summary>
        /// 任务获得 0
        /// </summary>
        Task=0,
        /// <summary>
        /// 出售仓库物品 1
        /// </summary>
        SellStorage,
        /// <summary>
        /// 放养动物所得金币 2
        /// </summary>
        RaiseAnimal,
        /// <summary>
        /// 赶产动物所得 3
        /// </summary>
        YieldAnimal,
        /// <summary>
        /// 清理垃圾所得 4
        /// </summary>
        CleanRubbish,
        /// <summary>
        /// 清理蚊子所得 5
        /// </summary>
        CleanMosquito,
        /// <summary>
        /// 给好友牧场添加食物所得 6
        /// </summary>
        AddCribToFriend,
        /// <summary>
        /// 收获成熟动物所得 7
        /// </summary>
        GotRaiseAnimal,
        /// <summary>
        /// 收获自己的出产物所得 8
        /// </summary>
        HavrestOutPut, 
        /// <summary>
        /// 收获（偷取）好友出产物 9
        /// </summary>
        StealFriendOutPut,
        /// <summary>
        /// 给自己牧场加牧草食物 10
        /// </summary>
        AddCribToSelf,
        /// <summary>
        /// 交易所出售
        /// </summary>
        SellExchange
    }
    /// <summary>
    /// 金币代币消费方式
    /// </summary>
    public enum EnumGoldConsumeType
    {
        /// <summary>
        /// 购买动物幼仔 - 0
        /// </summary>
        BuyAnimalBaby=0,
        /// <summary>
        /// 购买道具（食物、卡片） - 1
        /// </summary>
        BuyFood,
        /// <summary>
        /// 升级窝棚 - 2
        /// </summary>
        UpgradeHouse,
        /// <summary>
        /// 学习公式 - 3
        /// </summary>
        StudyFormula,
        /// <summary>
        ///交易所购买 - 4 
        /// </summary>
        BuyExchange,
        /// <summary>
        /// 升级加工厂 - 5
        /// </summary>
        UpgradeFactory

    }
    /// <summary>
    /// 是否加锁
    /// </summary>
    public enum islock
    {
        /// <summary>
        /// 没有加锁
        /// </summary>
        NoLock=0,
         /// <summary>
         /// 加锁
         /// </summary>
        YesLock
    }
    /// <summary>
    /// 退出登录状态
    /// </summary>
    public enum LogStat
    {
        /// <summary>
        /// 退出成功
        /// </summary>
       OutSuccess=0,
        /// <summary>
        /// 退出失败
        /// </summary>
        OutLost=1

    }

    /// <summary>
    /// 登录退出日志记录类型
    /// </summary>
    public enum LoginLogoutType
    {
        /// <summary>
        /// 登录
        /// </summary>
        Login =1,
        /// <summary>
        /// 退出
        /// </summary>
        Logout
    }

    /// <summary>
    /// 用户是否已经登陆
    /// </summary>
    public enum IsLogStat
    {
        /// <summary>
        /// 退出登录 离线
        /// </summary>
        NoLog=0,
        /// <summary>
        /// 已经登陆 在线
        /// </summary>
        YesLog

    }
    /// <summary>
    /// 提交留言状态
    /// </summary>
    public enum LeaveStat
    {
        /// <summary>
        /// 提交留言成功
        /// </summary>
        LeaveSuccess=0,
        /// <summary>
        /// 提交留言失败
        /// </summary>
        LeaveLost
    }
    /// <summary>
    /// 修改个人信息接口状态
    /// </summary>
    public enum ModifyUserDataStat
    {
        /// <summary>
        /// 修改成功
        /// </summary>
        Success=0,
        /// <summary>
        /// 修改失败
        /// </summary>
        Fail
    }
    /// <summary>
    /// 出产物是否可偷
    /// </summary>
    public enum OutPutStealStatus
    {
        /// <summary>
        ///  不可偷
        /// </summary>
       NoSteal=0,
        /// <summary>
        /// 可偷
        /// </summary>
        YesSteal
    }
    /// <summary>
    /// 食物增加类型
    /// </summary>
    public enum AddType
    {
        /// <summary>
        /// 商店购买
        /// </summary>
      ShopType=0,
       /// <summary>
       /// 其他
       /// </summary>
      OtherType
    }
    /// <summary>
    /// 出产物类型定义
    /// </summary>
    public enum OutPutType
    {
        /// <summary>
        /// 动物幼仔
        /// </summary>
        AnimalBaby=1,
        /// <summary>
        /// 动物副产品
        /// </summary>
        AnimalProduct

    }
    /// <summary>
    /// 用户范围
    /// </summary>
    public enum UserType
    {
        /// <summary>
        /// 全部用户
        /// </summary>
        AllUser=0,
        /// <summary>
        /// 同城非好友用户
        /// </summary>
        SameCityUser,
        /// <summary>
        /// 非同城非好友用户
        /// </summary>
        UnSameCityUser,
        /// <summary>
        /// 认识我的人
        /// </summary>
        LinkMeUser
    }
    /// <summary>
    /// 是否好友
    /// </summary>
    public enum IsFriendType
    {
        /// <summary>
        /// 非好友
        /// </summary>
      NoFriend=0,
        /// <summary>
        /// 好友
        /// </summary>
      YesFriend
    }
    /// <summary>
    /// 添加好友状态
    /// </summary>
    public enum AddFriendStat
    {
        /// <summary>
        /// 添加成功
        /// </summary>
      AddSucess=0,
        /// <summary>
        /// 添加失败
        /// </summary>
      AddFail
    }
    /// <summary>
    /// 上传通讯录是否成功
    /// </summary>
    public enum AddTelPhone
    {
        /// <summary>
        /// 上传成功
        /// </summary>
        AddSucess=0,
        /// <summary>
        /// 上传失败
        /// </summary>
        AddFail
    }
    /// <summary>
    /// 是否已经注册
    /// </summary>
    public enum MobileStat
    {
        /// <summary>
        /// 未注册过的手机
        /// </summary>
        NoRegPhone=0,
        /// <summary>
        /// 已经注册过的手机
        /// </summary>
        YesRegPhone
    }

    /// <summary>
    /// 商品售价类型
    /// </summary>
    public enum PriceType
    {
        /// <summary>
        /// 普通系统金币
        /// </summary>
        NormalGold = 1,
        /// <summary>
        /// 人民币支付
        /// </summary>
        RMB
    }

    public enum HelpType
    {
        /// <summary>
        /// 新手帮助
        /// </summary>
        [Description("新手帮助")]
        Novice=1,
        /// <summary>
        /// 切屏提示语
        /// </summary>
        [Description("切屏提示语")]
        Cut_Screen
    }

    /// <summary>
    /// 好友留言是否已读的枚举，2010-05-07
    /// </summary>
    public enum LevelMessageFlag
    {
        /// <summary>
        /// 未阅读
        /// </summary>
        unread = 0,
        /// <summary>
        /// 已阅读
        /// </summary>
        read
    }

    /// <summary>
    /// 留言记录类型
    /// </summary>
    public enum LevelMessageType
    {
        /// <summary>
        /// 系统留言
        /// </summary>
        System=0,
        /// <summary>
        /// 好友留言
        /// </summary>
        Friend,
        /// <summary>
        /// 自己留言
        /// </summary>
        Self
    }

    /// <summary>
    /// 删除好友留言模式枚举
    /// </summary>
    public enum LeaveMessageDeleteType
    {
        /// <summary>
        /// 删除单条记录
        /// </summary>
        single = 0,
        /// <summary>
        /// 删除全部记录
        /// </summary>
        all
    }
    /// <summary>
    /// 好友留言记录是否删除枚举
    /// </summary>
    public enum LeaveMessageDeleteFlag
    {
        /// <summary>
        /// 未被删除
        /// </summary>
        None =0,
        /// <summary>
        /// 已被删除
        /// </summary>
        Deleted
    }

    /// <summary>
    /// 版本检测表中，资源的版本号是否下发
    /// </summary>
    public enum CfgVersionStat
    {
        /// <summary>
        /// 不下发
        /// </summary>
        DoNotPush = 0,
        /// <summary>
        /// 下发版本号
        /// </summary>
        Push
    }
    /// <summary>
    /// 好友关系类型枚举
    /// </summary>
    public enum FriendRelativeType
    {
        /// <summary>
        /// 我的好友
        /// </summary>
        MyFriend =0,
        /// <summary>
        /// 认识我的人
        /// </summary>
        LineMeUser,
        /// <summary>
        /// 非好友
        /// </summary>
        UnFriend
    }
    /// <summary>
    /// Add / Delete FriendLink
    /// </summary>
    public enum FriendRelativeOpType
    {
        /// <summary>
        /// add friend - 0
        /// </summary>
        AddRelative = 0,
        /// <summary>
        /// delete friend - 1
        /// </summary>
        DeleteRelative
    }

    /// <summary>
    /// 状态枚举值
    /// </summary>
    public enum EStat
    {
        /// <summary>
        /// 成功 - 0
        /// </summary>
        Success = 0,
        /// <summary>
        /// 失败 - 1
        /// </summary>
        Fail
    }

    /// <summary>
    /// 操作日志是否已读标识（在主屏幕是否下发）
    /// </summary>
    public enum ActionOpLogReadStat
    {
        /// <summary>
        /// 未读 - 0
        /// </summary>
        UnRead =0,
        /// <summary>
        /// 已读 - 1
        /// </summary>
        Read
    }
    /// <summary>
    /// 请求是否来自主界面
    /// </summary>
    public enum ERequestFrom
    {
        /// <summary>
        /// 其他界面 - 0
        /// </summary>
        OtherScreen = 0,
        MainScreen
    }
    /// <summary>
    /// 食槽状态
    /// </summary>
    public enum ECribHeadId
    {
        /// <summary>
        /// 充足
        /// </summary>
        Full = 4001,
        /// <summary>
        /// 半满
        /// </summary>
        UnFull = 4002,
        /// <summary>
        /// 空槽
        /// </summary>
        Empty = 4003
    }
}