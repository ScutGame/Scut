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
using System.Threading.Tasks;

namespace ZyGames.Framework.RPC.Sockets.WebSocket
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CloseStatusCode
    {
        /// <summary>
        /// 
        /// </summary>
        protected CloseStatusCode()
        {
            NormalClosure = 1000;
            GoingAway = 1001;
            ProtocolError = 1002;
            UnexpectedCondition = 1003;
            Reserved = 1004;
            NoStatusRcvd = 1005;
            AbnormalClosure = 1006;
            InvalidUTF8 = 1007;
            PolicyViolation = 1008;
            MessageTooBig = 1009;
            MandatoryExt = 1010;
        }

        /// <summary>
        /// 1000
        /// </summary>
        public int NormalClosure { get; protected set; }
        /// <summary>
        /// 1001
        /// </summary>
        public int GoingAway { get; protected set; }
        /// <summary>
        /// 1002
        /// </summary>
        public int ProtocolError { get; protected set; }
        /// <summary>
        /// 1003
        /// </summary>
        public int UnexpectedCondition { get; protected set; }
        /// <summary>
        /// 1004
        /// </summary>
        public int Reserved { get; protected set; }
        /// <summary>
        /// 1005
        /// </summary>
        public int NoStatusRcvd { get; protected set; }

        /// <summary>
        /// 1006
        /// </summary>
        public int AbnormalClosure { get; protected set; }
        /// <summary>
        /// 1007
        /// </summary>
        public int InvalidUTF8 { get; protected set; }
        /// <summary>
        /// 1008
        /// </summary>
        public int PolicyViolation { get; protected set; }
        /// <summary>
        /// 1009
        /// </summary>
        public int MessageTooBig { get; protected set; }
        /// <summary>
        /// 1010
        /// </summary>
        public int MandatoryExt { get; protected set; }
    }
}
