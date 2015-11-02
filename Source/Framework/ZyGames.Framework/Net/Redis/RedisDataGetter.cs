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

using System.Collections.Generic;
using ZyGames.Framework.Model;
using ZyGames.Framework.Redis;

namespace ZyGames.Framework.Net.Redis
{
    class RedisDataGetter : IDataReceiver
    {
        private string _redisKey;
        private readonly SchemaTable _table;

        public RedisDataGetter(string redisKey, SchemaTable table)
        {
            _redisKey = redisKey;
            _table = table;
        }

        #region IDataReceiver 成员

        public bool TryReceive<T>(out List<T> dataList) where T : ISqlEntity, new()
        {
            return RedisConnectionPool.TryGetEntity(_redisKey, _table, out dataList);
        }

        public void Dispose()
        {

        }

        #endregion
    }
}