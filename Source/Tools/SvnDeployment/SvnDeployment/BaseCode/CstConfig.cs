using System;
namespace CitGame
{
    /// <summary>
    /// CstConfig 的摘要说明
    /// </summary>
    public class CstConfig
    {
        /// <summary>
        /// 错误的用户ID
        /// </summary>
        public const Int32 UserIDErr = -999;

        /// <summary>
        /// 保留数据长度
        /// </summary>
        public const Int16 CstHoldDataDefValue = 0;
        public const byte GotStat_NotGot = 0;
        public const byte GotStat_IsGot = 1;

        public const int AddFriendCribGotNum = 10;
        public const int AddFriendCribGotExpNum = 1;

        public const string CstReplaceKey = "|";

        public const Byte Cst_Byte_YieldAction = 0;
        public const Byte Cst_Byte_YieldAction_Later = 1;

        /// <summary>
        /// 清理垃圾成功
        /// </summary>
        public const Byte Cst_Byte_RubbishCleanStat_Suc = 0;
        /// <summary>
        /// 清理垃圾失败
        /// </summary>
        public const Byte Cst_Byte_RubbishCleanStat_Fail = 1;


        /// <summary>
        /// 获得牧场操作日志时，提取多少天之内的数据
        /// </summary>
        public const int iOpLogGetDays = -3;
        /// <summary>
        /// 获得牧场操作日志时，对多少分钟内的相同Action进行合并
        /// </summary>
        public const int iOpLogCombinMinute = 10;

        /// <summary>
        /// 动物基本信息缓存类日志目录
        /// </summary>
        public const string CacheAnimalErrorLog = "LCacheAniaml";
        public const string CacheMosErrorLog = "LCacheMosquito";
        /// <summary>
        /// 是否保存正常的日志
        /// </summary>
        public const bool CstBSaveNormalLog = true;
        /// <summary>
        /// 代币的名称
        /// </summary>
        public const string RmbCoinName = "米币";
        /// <summary>
        /// 系统金币名称;
        /// </summary>
        public const string SysCoinName = "金币";
        /// <summary>
        /// 物品
        /// </summary>
        public const string GoodsName = "物品";
        /// <summary>
        /// 经验
        /// </summary>
        public const string ExperienceName = "经验";
        /// <summary>
        /// 随机种子
        /// </summary>
        public const string RndSeed = "随机种子";

        /// <summary>
        /// 新手帮助
        /// </summary>
        public const string NoviceHelp="新手帮助";
        /// <summary>
        /// 切屏提示语帮助
        /// </summary>
        public const string Cut_ScreenHelp = "切屏提示语";

        /// <summary>
        /// Gm号
        /// </summary>
        public const int GMUserID = 1373721;

        /// <summary>
        /// Version：0.1.0
        /// </summary>
        public const string sCstAppVersion_v_0_1_0 = "0.1.0";
        /// <summary>
        /// Version：1.1.0
        /// </summary>
        public const string sCstAppVersion_v_1_1_0 = "1.1.0";
        /// <summary>
        /// Version: 0.1.0
        /// </summary>
        public const short iCstAppVersion_v_0_1_0 = 1;
        /// <summary>
        /// Version: 1.10.0
        /// </summary>
        public const short iCstAppVersion_v_1_10_0 = 2;

        /// <summary>
        /// VIP的加成值的除数（百分比）
        /// </summary>
        public const int VipPercent = 100;

        public const int iLowestStarLv = 1;
        public const int iStarLv2 = 2;
        public const int iStarLv3 = 3;
        public const int iStarLv4 = 4;
        public const int iHighestStarLv = 5;
        public const int iStarLvCreateNewRecord = 6;

        /// <summary>
        /// 任务类型 日常任务
        /// </summary>
        public const string RC = "日常任务";
        /// <summary>
        /// 任务类型 新手任务
        /// </summary>
        public const string XS = "新手任务";
        /// <summary>
        /// 任务类型 剧情任务
        /// </summary>
        public const string JQ = "剧情任务";
        /// <summary>
        /// 任务类型 节日任务
        /// </summary>
        public const string JR = "节日任务";

        public const int Dialog_Undifine = -1;
        public const string Dialog_Ranch_Task = "1";
        public const int Dialog_Ranch_MarketItem = 2;
        public const int Dialog_Ranch_Animal = 3;
    }
}