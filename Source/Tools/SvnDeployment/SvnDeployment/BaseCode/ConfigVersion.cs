using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// ConfigVersion 的摘要说明
/// </summary>
public class ConfigVersion
{
	public ConfigVersion()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    /// <summary>
    /// 版本配置表对应配置类型定义
    /// </summary>
    public enum ConfigVersionType
    {
        /// <summary>
        /// 经验配置表
        /// </summary>
        ConfigExpLevel=0,
        /// <summary>
        /// 窝棚信息配置表
        /// </summary>
        ConfigHouseInfo,
        /// <summary>
        /// 动物基础信息配置表
        /// </summary>
        ConfigAnimalBasic,
        /// <summary>
        /// 动物阶段配置表
        /// </summary>
        ConfigAnimalCurStatu,
        /// <summary>
        /// 出产物配置表
        /// </summary>
        ConfigOutPut,
        /// <summary>
        /// 食物配置表
        /// </summary>
        ConfigFood,
        /// <summary>
        /// 蚊子参数配置表
        /// </summary>
        ConfigMosquito,
        /// <summary>
        /// 动物粪便相关参数配置表
        /// </summary>
        ConfigRubbish,
        /// <summary>
        /// 资源下载路径配置表
        /// </summary>
        ConfigSourcePath,
        /// <summary>
        /// 头像资源配置表
        /// </summary>
        ConfigHeadInfo,
        /// <summary>
        /// 模型资源配置表
        /// </summary>
        ConfigModelInfo,
        /// <summary>
        /// 动物对话资源配置表
        /// </summary>
        ConfigAnimalSpeak

    }
    public bool UpCVersion(int vertype)
    {
        try
        {
            DateTime dt = DateTime.Now;
            string UpSql = "update ConfigVersion set CurVersion=CurVersion+1,ModifyTime='" + dt.ToString() + "' where VersionType='" + vertype + "'";
            Common.ExecuteSql(UpSql);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
