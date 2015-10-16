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
using System.Collections.Concurrent;
using System.Collections.Generic;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.RPC.IO;

namespace ZyGames.Framework.Game.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class SocketGameResponse : BaseGameResponse
    {
        static SocketGameResponse()
        {
            var setting = GameEnvironment.Setting;
            if (setting != null)
            {
                MessageStructure.EnableGzip = setting.ActionEnableGZip;
                MessageStructure.EnableGzipMinByte = setting.ActionGZipOutLength;
            }
        }

        private MessageStructure _buffers;
        /// <summary>
        /// 
        /// </summary>
        public SocketGameResponse()
        {
            _buffers = new MessageStructure();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public override void BinaryWrite(byte[] buffer)
        {
            DoWrite(buffer);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public override void Write(byte[] buffer)
        {
            DoWrite(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] ReadByte()
        {
            return _buffers.PosGzipBuffer();
        }

        private void DoWrite(byte[] buffer)
        {
            _buffers.WriteByte(buffer);
        }

    }
}