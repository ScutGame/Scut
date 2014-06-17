using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Doudizhu.Model;
using ZyGames.Framework.Game.Com.Rank;

namespace ZyGames.Doudizhu.Bll
{
    public class PythonHelper
    {
        /// <summary>
        /// 小数转换为百分比
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string TransformString(decimal num)
        {
            string currNum = Math.Round((num * 100), 1, MidpointRounding.AwayFromZero).ToString();
            currNum = currNum.Replace(".0", "") + "%";
            return currNum;
        }
    }
}
