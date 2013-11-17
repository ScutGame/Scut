using System;
using System.ComponentModel;


/// <summary>
/// EnumManage 的摘要说明
/// </summary>
public class EnumManage
{
	public EnumManage()
	{
	}

    /// <summary>
    /// 操作类型枚举
    /// </summary>
    public enum OpAction
    {
        /// <summary>
        /// 添加
        /// </summary>
        add = 0,
        /// <summary>
        /// 修改
        /// </summary>
        modify,
        /// <summary>
        /// 删除
        /// </summary>
        delete
    }

    public enum TaskInfo
    {
        /// <summary>
        /// 新手任务
        /// </summary>
        XS,
        /// <summary>
        /// 日常任务
        /// </summary>
        RC,
        /// <summary>
        /// 剧情任务
        /// </summary>
        JQ,
        /// <summary>
        /// 节日任务
        /// </summary>
        JR
    }

    /// <summary>
    /// 
    /// </summary>
    public enum GameType : short
    {
        /// <summary>
        /// 牧场
        /// </summary>
        [Description("疯狂牧场")]
        MC = 3,
        /// <summary>
        /// 花粉
        /// </summary>
        [Description("魔力花粉")]
        HF = 4,
        /// <summary>
        /// 军临
        /// </summary>
        [Description("军临城下")]
        Army = 5,
    }
}
