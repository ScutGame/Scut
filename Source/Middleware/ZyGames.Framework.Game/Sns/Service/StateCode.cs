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
namespace ZyGames.Framework.Game.Sns.Service
{
    /// <summary>
    /// 
    /// </summary>
    public enum StateCode
    {
        /// <summary>
        /// 
        /// </summary>
        OK = 0,
        /// <summary>
        /// 
        /// </summary>
        Error = 100,
        /// <summary>
        /// 
        /// </summary>
        SignError = 101,
        /// <summary>
        /// 
        /// </summary>
        NoHandler = 102,
        /// <summary>
        /// 
        /// </summary>
        PassworkError = 103,
        /// <summary>
        /// 
        /// </summary>
        NoToken = 105,
        /// <summary>
        /// 
        /// </summary>
        TokenExpired = 106,
        /// <summary>
        /// Request account server Timeout.
        /// </summary>
        Timeout = 107,
        /// <summary>
        /// Parse json error.
        /// </summary>
        ParseError = 108
    }
}