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
using System.Web;

namespace ZyGames.Tianjiexing.BLL.Base
{
    public class GetEnumCountHelper
    {
        /// <summary>
        /// 获取枚举值个数:使用参考
        /// GetEnumCountHelper.EnumCount('ZyGames.Tianjiexing.Model.Enum.MailType,ZyGames.Tianjiexing.Model')
        /// </summary>
        /// <param name="str">枚举所在的程序集和枚举名称</param>
        /// <returns>枚举数</returns>
        public static int EnumCount(string str)
        {
            Type type = Type.GetType(str,false,true);
            if (type==null)
            {
                return 0;
            }
            return Enum.GetValues(type).Length;
        }
    }
}