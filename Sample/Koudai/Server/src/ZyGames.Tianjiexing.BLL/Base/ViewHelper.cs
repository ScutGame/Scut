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
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Base
{
    public class ViewHelper
    {
        /// <summary>
        /// 查看玩家功能是否开启
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static List<UserFunction> GetFunctionList(string userID)
        {
            List<UserFunction> functionsList = new List<UserFunction>();
            //命运水晶
            UserFunction function_Crystal = GetUserFunction(userID, FunctionEnum.Mingyunshuijing);
            if (function_Crystal != null)
            {
                functionsList.Add(function_Crystal);
            }

            //装备封灵
            UserFunction function_Spare = GetUserFunction(userID, FunctionEnum.Fengling);
            if (function_Spare != null)
            {
                functionsList.Add(function_Spare);
            }

            //武器附魔
            UserFunction function_Enchant = GetUserFunction(userID, FunctionEnum.Enchant);
            if (function_Enchant != null)
            {
                functionsList.Add(function_Enchant);
            }
            return functionsList;
        }

        /// <summary>
        /// 已开启的功能
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="funEnum"></param>
        /// <returns></returns>
        private static UserFunction GetUserFunction(string userID, FunctionEnum funEnum)
        {
            var cacheSet = new GameDataCacheSet<UserFunction>();
            UserFunction function1 = cacheSet.FindKey(userID, funEnum);
            if (function1 != null)
            {
                return function1;
            }
            return null;
        }

    }
}