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

namespace ZyGames.Framework.Game.Service
{
    /// <summary>
    /// 游戏输出接口
    /// </summary>
    public abstract class BaseGameResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public event Action<BaseGameResponse, ActionGetter, int, string> WriteErrorCallback;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
       public abstract void BinaryWrite(byte[] buffer);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
       public abstract void Write(byte[] buffer);

       internal void WriteError(ActionGetter actionGetter, int errorCode, string errorInfo)
       {
           Action<BaseGameResponse, ActionGetter, int, string> handler = WriteErrorCallback;

           if (handler != null) handler(this, actionGetter, errorCode, errorInfo);
       }

    }

}