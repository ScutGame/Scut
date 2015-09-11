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

using System.Collections.Generic;
using ZyGames.Framework.Game.Pay.Section;
using ZyGames.Framework.Game.Service;

namespace ZyGames.Framework.Game.Contract.Action
{
	/// <summary>
	/// App store detail action.
	/// </summary>
    public class AppStoreDetailAction : BaseStruct
    {
        private List<AppStorePayElement> appList = new List<AppStorePayElement>();
        private MobileType MobileType = 0;
		/// <summary>
		/// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Contract.Action.AppStoreDetailAction"/> class.
		/// </summary>
		/// <param name="aActionId">A action identifier.</param>
		/// <param name="httpGet">Http get.</param>
        public AppStoreDetailAction(short aActionId, ActionGetter httpGet)
            : base(aActionId, httpGet)
        {
        }
		/// <summary>
		/// 创建返回协议内容输出栈
		/// </summary>
        public override void BuildPacket()
        {
            PushIntoStack(appList.Count);
            foreach (var item in appList)
            {
                DataStruct ds = new DataStruct();
                ds.PushIntoStack(item.Dollar.ToString());
                ds.PushIntoStack(item.ProductId);
                ds.PushIntoStack(item.SilverPiece);
                PushIntoStack(ds);
            }
        }
		/// <summary>
		/// 接收用户请求的参数，并根据相应类进行检测
		/// </summary>
		/// <returns></returns>
        public override bool GetUrlElement()
        {
            if (actionGetter.GetEnum("MobileType", ref MobileType))
            {
                return true;
            }
            return false;
        }
		/// <summary>
		/// 子类实现Action处理
		/// </summary>
		/// <returns></returns>
        public override bool TakeAction()
        {
            var paySection = AppStoreFactory.GetPaySection();
            var rates = paySection.Rates;

            foreach (AppStorePayElement rate in rates)
            {
                if (rate.MobileType == MobileType.Normal || rate.MobileType == MobileType)
                {
                    appList.Add(rate);
                }
            }
            return true;
        }
    }
}