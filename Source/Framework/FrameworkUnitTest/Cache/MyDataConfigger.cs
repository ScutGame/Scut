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
#define IS_MYSQ
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Config;
using ZyGames.Framework.Redis;


namespace FrameworkUnitTest.Cache
{
    public class MyDataConfigger : DataConfigger
    {
        public const string DbKey = "Conn";
        protected override void LoadConfigData()
        {
            this.AddNodeData(new RedisSection() { DbIndex = 10, Host = "127.0.0.1:6379", ClientVersion = RedisStorageVersion.HashMutilKeyMap });
            this.AddNodeData(new MessageQueueSection() { SqlSyncInterval = 1000 });
            this.AddNodeData(new CacheSection() { IsStorageToDb = true });
            this.AddNodeData(new EntitySection() { EnableModifyTimeField = true });

#if IS_MYSQ
            this.AddNodeData(new ConnectionSection(DbKey, "MySqlDataProvider", "Data Source=localhost;Database=FrameTestDB;Uid=root;Pwd=123456;"));
#else
                this.AddNodeData(new ConnectionSection(DbKey, "SqlDataProvider", "Data Source=localhost;Database=FrameTestDB;Uid=sa;Pwd=123;"));
#endif
        }
    }
}
