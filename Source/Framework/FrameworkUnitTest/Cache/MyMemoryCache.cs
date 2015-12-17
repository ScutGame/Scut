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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameworkUnitTest.Cache.Model;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common.Reflect;
using ZyGames.Framework.Data;
using ZyGames.Framework.Model;
using ZyGames.Framework.Net;
using ZyGames.Framework.Redis;

namespace FrameworkUnitTest.Cache
{
    public class MyMemoryCache : MemoryCacheStruct<MemoryData>
    {
        protected override bool InitCache(bool isReplace)
        {
            var schema = EntitySchemaSet.Get<MemoryData>();
            List<MemoryData> list;
            if (DataSyncManager.TryReceiveSql(schema, new DbDataFilter(100), OnSetProperty, out list))
            {
                foreach (var data in list)
                {
                    AddOrUpdate(data.Id.ToString(), data);
                }
                return true;
            };
            return false;
            //List<MemoryData> list;
            //RedisConnectionPool.TryGetEntity(ContainerKey, null, out list);
        }

        public void Save(MemoryData data)
        {
            if (AddOrUpdate(data.Id.ToString(), data))
            {
                //DataSyncQueueManager.SendToDb(GetPropertyValue, null, data);
                DataSyncManager.SendSql(new MemoryData[] { data }, true, GetPropertyValue);
            }
        }

        protected override object GetPropertyValue(MemoryData entity, SchemaColumn column)
        {
            return ObjectAccessor.Create(entity)[column.Name];
        }

        protected void OnSetProperty(MemoryData entity, SchemaColumn column, object value)
        {
            ObjectAccessor.Create(entity)[column.Name] = value;
        }

    }
}
