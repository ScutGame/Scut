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
using System.Threading;

namespace ZyGames.Framework.Common
{
    /// <summary>
    /// 版本信息
    /// </summary>
    public class VersionConfig
    {
        private readonly int _minVersion;
        private readonly int _maxVersion;
        private int _currversion;
        /// <summary>
        /// 
        /// </summary>
        public VersionConfig()
            : this(0, int.MaxValue)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="minVersion"></param>
        /// <param name="maxVersion"></param>
        public VersionConfig(int minVersion, int maxVersion)
        {
            _minVersion = minVersion;
            _maxVersion = maxVersion;
            _currversion = _minVersion;
        }

        /// <summary>
        /// 取下一个版本号
        /// </summary>
        public int NextId
        {
            get
            {
                Interlocked.Exchange(ref _currversion, _currversion + 1 < _maxVersion ? _currversion + 1 : _minVersion);
                return _currversion;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            get
            {
                return _currversion;
            }
        }
    }
}