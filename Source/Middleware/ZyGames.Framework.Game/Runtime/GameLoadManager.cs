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
