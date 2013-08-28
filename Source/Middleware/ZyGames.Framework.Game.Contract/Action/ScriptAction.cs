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
