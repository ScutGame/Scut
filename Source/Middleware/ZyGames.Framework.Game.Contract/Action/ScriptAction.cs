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
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Service;

namespace ZyGames.Framework.Game.Contract.Action
{
    /// <summary>
    /// 提供脚本支持
    /// </summary>
    public class ScriptAction : AuthorizeAction//AuthorizeAction
    {
        private readonly ScriptRoute _scriptRoute;
        private readonly bool _ignoreAuthorize;

        /// <summary>
        /// /
        /// </summary>
        /// <param name="aActionId"></param>
        /// <param name="httpGet"></param>
        /// <param name="scriptRoute"></param>
        /// <param name="ignoreAuthorize">忽略授权</param>
        public ScriptAction(short aActionId, HttpGet httpGet, ScriptRoute scriptRoute, bool ignoreAuthorize)
            : base(aActionId, httpGet)
        {
            _scriptRoute = scriptRoute;
            _ignoreAuthorize = ignoreAuthorize;
        }

        protected override void InitChildAction()
        {
        }

        public override bool GetUrlElement()
        {
            return _scriptRoute.GetUrlElement(httpGet, this);
        }

        public override bool TakeAction()
        {
            return _scriptRoute.TakeAction(this);
        }

        public override void BuildPacket()
        {
            if (!_scriptRoute.BuildPacket(dataStruct))
            {
                ErrorCode = LanguageHelper.GetLang().ErrorCode;
                if (IsRealse)
                {
                    ErrorInfo = LanguageHelper.GetLang().ServerBusy;
                }
            }
        }

        protected override bool IgnoreActionId
        {
            get { return _ignoreAuthorize; }
        }
    }
}