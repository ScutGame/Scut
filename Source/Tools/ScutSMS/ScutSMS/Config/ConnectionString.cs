/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ProtoBuf;

namespace ScutServerManager.Config
{
    [TypeConverterAttribute(typeof(ProtertyObjectConverter)),
    Description("db connection string."),
    ProtoContract, Serializable]
    public class ConnectionString
    {
        private const string CatConnection = "Connection";

        public ConnectionString()
        {
            DataSource = DefaultConfig.DataSource;
            ProviderName = DbProviderType.SqlDataProvider;
        }

        [ProtoMember(1),
        Category(CatConnection),
        DescriptionAttribute("ConnectionString key name, is required.")]
        public string Name { get; set; }

        private DbProviderType _providerName;

        [ProtoMember(2),
        Category(CatConnection),
        DefaultValueAttribute(DbProviderType.SqlDataProvider),
        DescriptionAttribute("ProviderName of db.")]
        public DbProviderType ProviderName
        {
            get { return _providerName; }
            set
            {
                _providerName = value;
                if (_providerName == DbProviderType.MySqlDataProvider)
                {
                    Charset = DefaultConfig.MysqlCharset;
                    Port = DefaultConfig.MysqlPort;
                }
                else
                {
                    Charset = "";
                    Port = DefaultConfig.SqlPort;
                }
            }
        }

        [ProtoMember(3),
        Category(CatConnection),
        DefaultValue(DefaultConfig.DataSource),
        DescriptionAttribute("DataSource of db, is required.")]
        public string DataSource { get; set; }

        [ProtoMember(4),
        Category(CatConnection),
        DescriptionAttribute("Database of db, is required.")]
        public string Database { get; set; }

        [ProtoMember(5),
        Category(CatConnection),
        DescriptionAttribute("User of db.")]
        public string Uid { get; set; }

        [ProtoMember(6),
        Category(CatConnection),
        DescriptionAttribute("Password of db.")]
        public string Pwd { get; set; }

        [ProtoMember(7),
        Category(CatConnection),
        DefaultValue(DefaultConfig.MysqlCharset),
        DescriptionAttribute("Charset of MySql db, charset:gbk,utf8.")]
        public string Charset { get; set; }

        [ProtoMember(8),
        Category(CatConnection),
         DescriptionAttribute("Extend property of db.")]
        public string Extend { get; set; }

        [ProtoMember(9),
        Category(CatConnection),
        DescriptionAttribute("port of db.")]
        public int Port { get; set; }

        public string FormatString()
        {
            string str = string.Format("Data Source={0};Database={1};Uid={2};Pwd={3};{4}{5}{6}",
                ProviderName == DbProviderType.SqlDataProvider && Port != DefaultConfig.SqlPort ? string.Format("{0},{1}", DataSource, Port) : DataSource,
                Database,
                Uid,
                Pwd,
                ProviderName == DbProviderType.MySqlDataProvider && Port != DefaultConfig.MysqlPort ? string.Format("Port={0};", Port) : "",
                ProviderName == DbProviderType.MySqlDataProvider && Charset != DefaultConfig.MysqlCharset ? string.Format("Charset={0};", Charset) : "",
                (Extend ?? "").Trim());
            return str;
        }

        public bool HasConfig()
        {
            return !string.IsNullOrEmpty(Database) &&
                !string.IsNullOrEmpty(DataSource);
        }
        public bool IsDataLevel()
        {
            return (Database ?? "").IndexOf("Data", StringComparison.CurrentCultureIgnoreCase) != -1;
        }

        public ConnectionString CopyObject()
        {
            var conn = new ConnectionString();
            conn.Name = Name;
            conn.ProviderName = ProviderName;
            conn.DataSource = DataSource;
            conn.Database = Database;
            conn.Charset = Charset;
            conn.Extend = Extend;
            conn.Uid = Uid;
            conn.Pwd = Pwd;
            return conn;
        }
    }
}