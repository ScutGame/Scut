using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CitGame
{
    /// <summary>
    /// Action7015调用的商城道具类
    /// </summary>
    public class ConfigMarket:BaseMarketItem
    {
      
        /// <summary>
        /// 配置的保留数据
        /// </summary>
        public short HoldData;

        public ConfigMarket()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        public override void initData(SqlDataReader aReader)
        {
            if (aReader == null) throw new Exception();
            
            this.PutData(aReader);
            this.HoldData = CstConfig.CstHoldDataDefValue;
        }

        /// <summary>
        /// 输入的版本号是否在该对象的可使用的版本号范围内
        /// </summary>
        /// <param name="aExistVersion">验证版本号</param>        
        public bool SetHidden(short aExistVersion)
        {
            if (this.IsHidden)
            {
                return this.IsHidden;
            }
            else
            {
                if (this._DataForClientVersions.IndexOf(aExistVersion.ToString()) == -1)
                {
                    return true;
                }
                else
                {
                    return this.IsHidden;
                }
            }
        }
    }
}