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
using System.Diagnostics;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Plugin.Test
{
    /// <summary>
    /// 
    /// </summary>
    public static class CaseManager
    {
        private static event CaseEventHandle Casehandle;

        static CaseManager()
        {
            Casehandle += new CaseEventHandle(ProcessCase);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseCase"></param>
        public static void Add(BaseCase baseCase)
        {
            var args = new CaseEventArgs() { Case = baseCase };

            CaseEventHandle temp = Casehandle;
            if (temp != null)
            {
                foreach (CaseEventHandle handler in temp.GetInvocationList())
                {
                    handler.BeginInvoke(null, args, new AsyncCallback(EndAsync), null);
                }
            }
        }

        private static void EndAsync(IAsyncResult ar)
        {
        }

        private static void ProcessCase(object sender, CaseEventArgs args)
        {
            if (args == null || args.Case == null)
            {
                return;
            }

            try
            {
                args.Case.TestCase();
            }
            catch (Exception ex)
            {
                string msg = string.Format("\"{0}\"用例>>测试失败:{1}", args.Case.Name, ex);
                TraceLog.WriteLine(msg);
            }
        }
    }
}