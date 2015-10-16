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
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Cache.Generic
{
    /// <summary>
    /// 默认缓存对象,负责通用更新事件通知
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class DefaultCacheStruct<T> : BaseCacheStruct<T> where T : AbstractEntity, new()
    {
        protected override bool LoadFactory(bool isReplace)
        {
            return true;
        }

        protected override bool LoadItemFactory(string key, bool isReplace)
        {
            return true;
        }

        public override bool Update(bool isChange, string changeKey = null)
        {
            SchemaTable schema;
            if (EntitySchemaSet.TryGet<T>(out schema) &&
                schema.AccessLevel == AccessLevel.ReadWrite)
            {
                if (schema.CacheType == CacheType.Entity)
                {
                  return  UpdateEntity(isChange, changeKey);
                }
                if (schema.CacheType == CacheType.Queue)
                {
                    return UpdateQueue(isChange, changeKey);
                }
                return UpdateGroup(isChange, changeKey);
            }
            return false;
        }
    }
}