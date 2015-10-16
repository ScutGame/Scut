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

namespace ZyGames.Framework.Common.Locking
{
    /// <summary>
    /// 过期不使用
    /// </summary>
    [Obsolete]
    class WriteLockSlim : ILocking
    {
        private readonly Func<int, bool> _enterCallback;
        private readonly Action _exitCallback;
        private readonly int _timeOut;

        public WriteLockSlim(Func<int, bool> enterCallback, Action exitCallback, int timeOut = 1000)
        {
            if (enterCallback == null)
            {
                throw new ArgumentNullException("enterCallback");
            }
            if (exitCallback == null)
            {
                throw new ArgumentNullException("exitCallback");
            }
            _enterCallback = enterCallback;
            _exitCallback = exitCallback;
            _timeOut = timeOut;
        }

        public bool IsLocked
        {
            get;
            private set;
        }

        public bool TryEnterLock()
        {
            IsLocked = _enterCallback(_timeOut);
            return IsLocked;
        }

        public void Dispose()
        {
            if (IsLocked && _exitCallback != null)
            {
                _exitCallback();
            }
        }


    }
}