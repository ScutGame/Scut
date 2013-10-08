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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Model;
using ZyGames.Framework.Net;

namespace ZyGames.Framework.Game.Runtime
{
    /// <summary>
    /// 游戏配置数据加载器
    /// </summary>
    public class GameConfigLoader<T> : DataLoader where T : ShareEntity, new()
    {
        private readonly bool _isAuto;
        private readonly ConfigCacheSet<T> _cacheSet;
        private const int MaxCount = int.MaxValue;

        public GameConfigLoader()
            : this(true)
        {
        }

        public GameConfigLoader(bool isAuto)
        {
            _cacheSet = new ConfigCacheSet<T>();
            _isAuto = isAuto;
        }

        public override string LoadTypeName
        {
            get { return typeof(T).FullName; }
        }

        protected override bool IsExistData()
        {
            return _cacheSet.Count > 0;
        }

        public override bool Load()
        {
            if (!_isAuto || !IsExistData())
            {
                return new ConfigCacheSet<T>().AutoLoad(new DbDataFilter(MaxCount));
            }
            return true;
        }
    }
}