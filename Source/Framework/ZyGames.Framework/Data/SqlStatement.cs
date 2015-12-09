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
using System.Data;
using System.Text;
using ProtoBuf;

namespace ZyGames.Framework.Data
{
    /// <summary>
    /// Sql语句对象
    /// </summary>
    [ProtoContract, Serializable]
    public class SqlStatement
    {
        /// <summary>
        /// init
        /// </summary>
        public SqlStatement()
        {
            
        }

        /// <summary>
        /// 标识ID
        /// </summary>
        [ProtoMember(1)]
        public int IdentityID { get; set; }

        /// <summary>
        /// 数据库连接串设置
        /// </summary>
        [ProtoMember(2)]
        public string ConnectionString { get; set; }

        /// <summary>
        /// 数据驱动连接提供者类型
        /// </summary>
        [ProtoMember(3)]
        public string ProviderType{ get; set; }

        /// <summary>
        /// 命令类型
        /// </summary>
        [ProtoMember(4)]
        public CommandType CommandType { get; set; }

        /// <summary>
        /// 语句
        /// </summary>
        [ProtoMember(5)]
        public string CommandText { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        [ProtoMember(6)]
        public SqlParam[] Params { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        [ProtoMember(7)]
        public string Table { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sql = new StringBuilder();
            sql.AppendLine(CommandText);
            foreach (var sqlParam in Params)
            {
                sql.AppendLine(string.Format("{0}:{1}", sqlParam.ParamName, sqlParam.Value));
            }
            return sql.ToString();
        }
    }
}