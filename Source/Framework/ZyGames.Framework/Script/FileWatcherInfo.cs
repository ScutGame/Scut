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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace ZyGames.Framework.Script
{

    internal class FileWatcherInfo
    {
        private int isUpdating;
        public string Path { get; set; }
        public string Filter { get; set; }
        public FileSystemWatcher Watcher { get; set; }
        public bool IsPython { get; set; }
        public int CompileLevel { get; set; }
        public string[] ReferenceKeys { get; set; }
        public bool IsInMemory { get; set; }
        public string AssemblyOutPath { get; set; }

        public event Action<Assembly> ChangedBefore;
        public event Action<Assembly> ChangedAfter;

        private Assembly _assembly;
        public Assembly Assembly
        {
            get
            {
                //modify reason:Get value when the cycle
                return _assembly;
            }
            set
            {
                //初始运行不检查更新Cache
                if (_assembly == null)
                {
                    _assembly = value;
                    return;
                }
                if (!Equals(_assembly, value))
                {
                    if (Interlocked.CompareExchange(ref isUpdating, 1, 0) == 0)
                    {
                        try
                        {
                            OnChangedBefore(_assembly);
                            _assembly = value;
                            OnChangedAfter(_assembly);
                        }
                        finally
                        {
                            Interlocked.Exchange(ref isUpdating, 0);
                        }
                    }
                }
            }
        }

        public bool IsUpdating()
        {
            return isUpdating == 1;
        }

        private void OnChangedBefore(Assembly assembly)
        {
            Action<Assembly> handler = ChangedBefore;
            if (handler != null) handler(assembly);
        }

        private void OnChangedAfter(Assembly assembly)
        {
            Action<Assembly> handler = ChangedAfter;
            if (handler != null) handler(assembly);
        }
    }

}