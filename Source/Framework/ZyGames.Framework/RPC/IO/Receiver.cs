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

namespace ZyGames.Framework.RPC.IO
{
    /// <summary>
    /// 接收消息
    /// </summary>
    public abstract class Receiver : IDisposable
    {
        /// <summary>
        /// 对消息解码
        /// </summary>
        public abstract void Decode();

        /// <summary>
        /// 较验签名
        /// </summary>
        /// <returns></returns>
        public abstract bool CheckSign();

        /// <summary>
        /// 处理
        /// </summary>
        /// <returns></returns>
        public abstract void Process();
		/// <summary>
		/// Releases all resource used by the <see cref="ZyGames.Framework.RPC.IO.Receiver"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="ZyGames.Framework.RPC.IO.Receiver"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="ZyGames.Framework.RPC.IO.Receiver"/> in an unusable state.
		/// After calling <see cref="Dispose"/>, you must release all references to the
		/// <see cref="ZyGames.Framework.RPC.IO.Receiver"/> so the garbage collector can reclaim the memory that the
		/// <see cref="ZyGames.Framework.RPC.IO.Receiver"/> was occupying.</remarks>
        public void Dispose()
        {
            DoDispose(true);
        }
		/// <summary>
		/// Dos the dispose.
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected virtual void DoDispose(bool disposing)
        {
            if (disposing)
            {
                //清理托管对象
                GC.SuppressFinalize(this);
            }
            //清理非托管对象
        }
    }
}