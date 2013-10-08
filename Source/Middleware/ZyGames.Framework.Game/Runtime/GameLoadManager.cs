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
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Game.Runtime
{
    /// <summary>
    /// 游戏加载管理
    /// </summary>
    public static class GameLoadManager
    {
        class LoadEventArgs : EventArgs
        {
            public bool IgnoreError
            {
                get;
                set;
            }
            public DataLoader Loader
            {
                get;
                set;
            }
        }
        delegate bool LoadEventHandle(LoadEventArgs args);

        private static event LoadEventHandle LoadEvent;

        static GameLoadManager()
        {
            LoadEvent += new LoadEventHandle(ProcessLoadEvent);
        }

        private static int _isLoadError;

        public static bool IsLoadError
        {
            get { return _isLoadError > 0; }
        }

        public static bool Add(DataLoader dataLoader, bool ignoreError)
        {
            var args = new LoadEventArgs() { Loader = dataLoader, IgnoreError = ignoreError };

            LoadEventHandle callbackFunc = LoadEvent;
            if (callbackFunc != null)
            {
                return callbackFunc(args);
            }
            return false;
        }

        private static bool ProcessLoadEvent(LoadEventArgs args)
        {
            if (args == null || args.Loader == null)
            {
                return false;
            }
            try
            {
                if (!args.Loader.Load())
                {
                    Interlocked.Increment(ref _isLoadError);
                    if (args.IgnoreError)
                    {
                        TraceLog.WriteError("GameLoader[{0}] data:\"{1}\" failure.", _isLoadError, args.Loader.LoadTypeName);
                        return false;
                    }
                    throw new Exception(string.Format("GameLoader[{0}] data:\"{1}\" failure.", _isLoadError, args.Loader.LoadTypeName));
                }
                return true;
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref _isLoadError);
                if (args.IgnoreError)
                {
                    TraceLog.WriteError("GameLoader[{0}] error:{1}", _isLoadError, ex);
                    return false;
                }
                throw new Exception("GameLoader error:", ex);
            }
        }

    }
}