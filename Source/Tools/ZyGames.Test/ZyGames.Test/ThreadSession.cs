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
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using ZyGames.Test.Net;

namespace ZyGames.Test
{
    public class ThreadSession
    {
        private NetProxy _netProxy;

        public ThreadSession(int id = 0)
        {
            Id = id;
            Context = new CaseContext();
        }

        public TaskSetting Setting { get; set; }

        /// <summary>
        /// connect not timer.
        /// </summary>
        public void InitConnect()
        {
            if (_netProxy == null)
            {
                _netProxy = NetProxy.Create(Setting.Url);
            }
            _netProxy.CheckConnect();
        }

        public NetProxy Proxy { get { return _netProxy; } }

        public int Id { get; set; }

        public CaseContext Context { get; set; }

    }
}