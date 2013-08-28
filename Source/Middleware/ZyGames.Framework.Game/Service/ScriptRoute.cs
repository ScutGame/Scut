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
