using System;
using ProtoBuf;
using ZyGames.Framework.Common;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Model;


namespace ZyGames.Doudizhu.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable, ProtoContract]
    [EntityTable(AccessLevel.ReadOnly, DbConfig.Config)]
    public class ConfigEnvSet : ShareEntity
    {

        public static int GetInt(string key, int defValue = 0)
        {
            var item = new ConfigCacheSet<ConfigEnvSet>().FindKey(key);
            return item == null ? defValue : item.EnvValue.ToInt();
        }

        public static decimal GetDecimal(string key, decimal defValue = 0)
        {
            var item = new ConfigCacheSet<ConfigEnvSet>().FindKey(key);
            return item == null ? defValue : item.EnvValue.ToDecimal();
        }
        public static double GetDouble(string key, double defValue = 0)
        {
            var item = new ConfigCacheSet<ConfigEnvSet>().FindKey(key);
            return item == null ? defValue : item.EnvValue.ToDouble();
        }

        public static string GetString(string key, string defValue = "")
        {
            var item = new ConfigCacheSet<ConfigEnvSet>().FindKey(key);
            return item == null ? defValue : item.EnvValue.ToNotNullString();
        }

        /// <summary>
        /// </summary>
        public ConfigEnvSet()
            : base(true)
        {

        }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(1)]
        [EntityField(true)]
        public string EnvKey{ get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(2)]
        [EntityField]
        public string EnvValue{ get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(3)]
        [EntityField]
        public string EnvDesc{ get; set; }


    }
}