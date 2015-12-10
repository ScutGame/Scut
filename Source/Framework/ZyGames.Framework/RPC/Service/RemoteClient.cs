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
using System.Text;
using System.Threading.Tasks;

namespace ZyGames.Framework.RPC.Service
{
    /// <summary>
    /// Remote EventArgs
    /// </summary>
    public class RemoteEventArgs : EventArgs
    {
        /// <summary>
        /// data
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// user data
        /// </summary>
        public object UserData { get; set; }
    }


    /// <summary>
    /// RemoteCallback delegate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void RemoteCallback(object sender, RemoteEventArgs e);

    /// <summary>
    /// Remote client
    /// </summary>
    public abstract class RemoteClient
    {
        /// <summary>
        /// Remote Target
        /// </summary>
        public object RemoteTarget { get; set; }

        /// <summary>
        /// callback event.
        /// </summary>
        public event RemoteCallback Callback;

        /// <summary>
        /// Is socket client
        /// </summary>
        public bool IsSocket { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public string LocalAddress { get; protected set; }

        /// <summary>
        /// Send
        /// </summary>
        /// <param name="data"></param>
        public abstract Task Send(string data);

        /// <summary>
        /// Send
        /// </summary>
        /// <param name="data"></param>
        public abstract Task Send(byte[] data);

        /// <summary>
        /// 
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCallback(RemoteEventArgs e)
        {
            RemoteCallback handler = Callback;
            if (handler != null) handler(RemoteTarget, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        protected byte[] ReadStream(Stream stream, Encoding encoding)
        {
            List<byte> data = new List<byte>();
            BinaryReader readStream;
            using (readStream = new BinaryReader(stream, encoding))
            {
                int size = 0;
                while (true)
                {
                    var buffer = new byte[1024];
                    size = readStream.Read(buffer, 0, buffer.Length);
                    if (size == 0)
                    {
                        break;
                    }
                    byte[] temp = new byte[size];
                    Buffer.BlockCopy(buffer, 0, temp, 0, size);
                    data.AddRange(temp);
                }
                return data.ToArray();
            }
        }

    }
}
