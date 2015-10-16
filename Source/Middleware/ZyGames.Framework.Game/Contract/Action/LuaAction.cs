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

using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Service;

namespace ZyGames.Framework.Game.Contract.Action
{
    class JsonLuaAction : LuaAction
    {
        public JsonLuaAction(int actionId, ActionGetter actionGetter, object scriptScope, bool ignoreAuthorize)
            : base(actionId, actionGetter, scriptScope, ignoreAuthorize)
        {
            EnableWebSocket = true;
        }

        protected override string BuildJsonPack()
        {
            return _scriptScope["buildPacket"].Call(_scriptScope, _urlParam, _actionResult)[0];
        }
    }
    class LuaAction : ScriptAction
    {
        public LuaAction(int actionId, ActionGetter actionGetter, object scriptScope, bool ignoreAuthorize)
            : base(ScriptType.Lua, actionId, actionGetter, scriptScope, ignoreAuthorize)
        {
            //ScriptEngines.LuaRegister(actionGetter);
        }

        public override bool GetUrlElement()
        {
            var func = _scriptScope["getUrlElement"];
            _urlParam = func.Call(_scriptScope, actionGetter)[0];
            return _urlParam != null && _urlParam["Result"] ? true : false;
        }

        public override bool DoAction()
        {
            _actionResult = _scriptScope["takeAction"].Call(_scriptScope, _urlParam)[0];
            return _actionResult != null && _actionResult["Result"] ? true : false;
        }

        public override void BuildPacket()
        {
            bool result = _scriptScope["buildPacket"].Call(_scriptScope, dataStruct, _urlParam, _actionResult)[0];
            if (!result)
            {
                ErrorCode = Language.Instance.ErrorCode;
                if (!IsRealse)
                {
                    ErrorInfo = Language.Instance.ServerBusy;
                }
            }
        }
    }
}
