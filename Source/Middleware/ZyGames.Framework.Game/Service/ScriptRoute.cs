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
using System.IO;
using System.Linq;
using System.Text;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Script;
using ZyGames.Framework.Plugin.PythonScript;

namespace ZyGames.Framework.Game.Service
{
    /// <summary>
    /// 脚本路由
    /// </summary>
    public class ScriptRoute
    {
        private readonly int _actionId;
        private PythonContext _pyContext;
        private dynamic _urlParam;
        private dynamic _actionResult;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionId"></param>
        public ScriptRoute(int actionId)
        {
            _actionId = actionId;
        }

        /// <summary>
        /// 尝试加载
        /// </summary>
        /// <returns></returns>
        public bool TryLoadAction(string routeName)
        {
            if (string.IsNullOrEmpty(routeName))
            {
                routeName = string.Format("Action\\Action{0}.py", _actionId);
            }
            return PythonScriptManager.Current.TryLoadPython(routeName, out _pyContext);
        }


        /// <summary>
        /// 读取参数
        /// </summary>
        /// <returns></returns>
        public bool GetUrlElement(HttpGet httpGet, BaseStruct baseStruct)
        {
            if (_pyContext != null)
            {
                try
                {
                    _urlParam = _pyContext.Scope.getUrlElement(httpGet, baseStruct);
                    return _urlParam != null && _urlParam.Result ? true : false;
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("Action{0}.GetUrlElement method:{1}", _actionId, ex);
                }
            }
            return false;
        }

        /// <summary>
        /// 处理请求
        /// </summary>
        /// <returns></returns>
        public bool TakeAction(BaseStruct baseStruct)
        {
            if (_pyContext != null)
            {
                try
                {
                    _actionResult = _pyContext.Scope.takeAction(_urlParam, baseStruct);
                    return _actionResult != null && _actionResult.Result ? true : false;
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("Action{0}.TakeAction method:{1}", _actionId, ex);
                }
            }
            return false;
        }

        /// <summary>
        /// 输出回返结果
        /// </summary>
        public bool BuildPacket(DataStruct writer)
        {
            if (_pyContext != null)
            {
                try
                {
                    return _pyContext.Scope.buildPacket(writer, _urlParam, _actionResult);
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("Action{0}.BuildPacket method:{1}", _actionId, ex);
                }
            }
            return false;
        }

    }
}