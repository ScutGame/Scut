using System;
using System.ComponentModel;
using ProtoBuf;

namespace ScutServerManager.Config
{
    [TypeConverterAttribute(typeof(ProtertyObjectConverter)),
    ProtoContract, Serializable,
    DescriptionAttribute("Redis setting.")]
    public class RedisSetting
    {
        public RedisSetting()
        {
            Host = DefaultConfig.RedisHost;
            ReadOnlyHost = DefaultConfig.ReadOnlyHost;
            Db = DefaultConfig.RedisDb;
            MaxReadPoolSize = DefaultConfig.MaxReadPoolSize;
            MaxWritePoolSize = DefaultConfig.MaxWritePoolSize;
            ConnectTimeout = DefaultConfig.ConnectTimeout;
            PoolTimeOut = DefaultConfig.PoolTimeOut;
            ConnectionString = new ConnectionString();
            Serializer = DefaultConfig.CacheSerializer;
        }

        //DefaultValueAttribute("1.0"),BrowsableAttribute(false),ReadOnlyAttribute(true)]
        [ProtoMember(1), 
        Category("Redis"),
        DefaultValue(DefaultConfig.RedisHost),
        Description("Redis server ip address(ex:password@ip:port).")]
        public string Host { get; set; }

        [ProtoMember(2), 
        CategoryAttribute("Redis")]
        [DefaultValueAttribute(DefaultConfig.ReadOnlyHost)]
        [DescriptionAttribute("Redis server port(ex:password@ip:port).")]
        public string ReadOnlyHost { get; set; }

        [ProtoMember(4), 
        CategoryAttribute("Redis")]
        [DefaultValueAttribute(DefaultConfig.RedisDb)]
        [DescriptionAttribute("Redis server db index.")]
        public int Db { get; set; }

        [ProtoMember(5), CategoryAttribute("Redis"),
        DefaultValue(DefaultConfig.MaxReadPoolSize),
        DescriptionAttribute("Redis connection read pool min size.")]
        public int MaxReadPoolSize { get; set; }

        [ProtoMember(6), CategoryAttribute("Redis"),
        DefaultValue(DefaultConfig.MaxWritePoolSize),
        DescriptionAttribute("Redis connection write pool max size.")]
        public int MaxWritePoolSize { get; set; }


        [ProtoMember(8), CategoryAttribute("Redis"),
        DefaultValue(DefaultConfig.ConnectTimeout),
        DescriptionAttribute("Redis connection connect timeout(ms).")]
        public int ConnectTimeout { get; set; }

        [ProtoMember(9), CategoryAttribute("Redis"),
        DefaultValue(DefaultConfig.PoolTimeOut),
        DescriptionAttribute("Redis connection pool timeout(ms).")]
        public int PoolTimeOut { get; set; }


        [ProtoMember(7), 
        CategoryAttribute("Database"),
        DescriptionAttribute("Redis move to databases connectionstring.")]
        public ConnectionString ConnectionString { get; set; }

        [ProtoMember(10), CategoryAttribute("Redis"),
        DefaultValue(DefaultConfig.CacheSerializer),
        DescriptionAttribute("Redis serialize storage mode.")]
        public StorageMode Serializer { get; set; }
    }
}
