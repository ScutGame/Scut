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
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Xml;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Timing;
using ZyGames.Framework.Plugin.PythonScript;

namespace ZyGames.Framework.Game.Script
{
    /// <summary>
    /// 路由项
    /// </summary>
    public class RouteItem
    {
        /// <summary>
        /// 
        /// </summary>
        public RouteItem()
        {
            IgnoreAuthorize = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public int ActionId { get; set; }
        /// <summary>
        /// 相对路径
        /// </summary>
        public string ScriptPath { get; set; }
        /// <summary>
        /// 映射C#类名
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 忽略授权
        /// </summary>
        public bool IgnoreAuthorize { get; set; }
    }

    /// <summary>
    /// 脚本管理
    /// </summary>
    public sealed class PythonScriptManager
    {
        private static PythonScriptManager _instance;
        /// <summary>
        /// 
        /// </summary>
        public static PythonScriptManager Current
        {
            get { return _instance; }
        }

        static PythonScriptManager()
        {
            _instance = new PythonScriptManager();
        }


        private ConcurrentDictionary<int, RouteItem> _routePyDict = new ConcurrentDictionary<int, RouteItem>();
        private ConcurrentDictionary<string, string> _libPyDict = new ConcurrentDictionary<string, string>();
        private string _routeConfigFileName;

        private string _runtimePath;
        /// <summary>
        /// 自带类库路径
        /// </summary>
        private string LibPath { get; set; }
        /// <summary>
        /// 脚本根路径
        /// </summary>
        public string PythonRootPath
        {
            get;
            private set;
        }

        private PythonScriptManager()
        {

        }

        internal void Intialize()
        {
            try
            {
                LibPath = "";
                PythonRootPath = ConfigUtils.GetSetting("PythonRootPath", "PyScript");
                _runtimePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                _runtimePath = Path.Combine(_runtimePath, PythonRootPath);
                _routeConfigFileName = Path.Combine(_runtimePath, "Route.config.xml");
                LoadRouteConfig(_routeConfigFileName);
                bool isDebug = ConfigUtils.GetSetting("Python_IsDebug", false);
                PythonScriptHost.Initialize(_runtimePath, LibPath, isDebug);

                CacheListener routeListener = new CacheListener("__ACTION_ROUTES", 0, (key, value, reason) =>
                {
					if (reason == CacheRemovedReason.Changed)
                    {
                        LoadRouteConfig(_routeConfigFileName);
                        //重新设置查找路径
						//todo remark
                        PythonScriptHost.LibPath = LibPath;
                        PythonScriptHost.SetPythonSearchPath(_runtimePath);
                    }
                }, _routeConfigFileName);
                routeListener.Start();
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("ActionFactory load route config:{0} error{1}", _routeConfigFileName, ex);
            }
        }

        private void LoadRouteConfig(string routeConfigFileName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(routeConfigFileName);
            SetLib(xmlDoc);
            SetRoutes(xmlDoc);
            xmlDoc = null;
        }

        private void SetLib(XmlDocument xmlDoc)
        {
            XmlElement rootNode = (XmlElement)xmlDoc.SelectSingleNode("config/lib");
            if (rootNode == null)
            {
                return;
            }
            LibPath = FormatLibPath(rootNode.GetAttribute("path"));
            _libPyDict.Clear();
            foreach (XmlNode childNode in rootNode.ChildNodes)
            {
                XmlElement element = childNode as XmlElement;
                if (element != null && !_libPyDict.ContainsKey(element.Name))
                {
                    string path = FormatPythonPath(element.GetAttribute("path"));
                    if (!string.IsNullOrEmpty(path))
                    {
                        _libPyDict.TryAdd(childNode.Name, path);
                    }
                    else
                    {
                        TraceLog.WriteError("Python lib name:{0} path is empyt.", childNode.Name);
                    }
                }
            }
        }

        private void SetRoutes(XmlDocument xmlDoc)
        {
            XmlNode rootNode = xmlDoc.SelectSingleNode("config/route-list");
            if (rootNode == null)
            {
                return;
            }
            _routePyDict.Clear();
            XmlNodeList childList = rootNode.SelectNodes("route");
            if (childList == null)
            {
                return;
            }
            foreach (XmlElement routeElement in childList)
            {
                int actionId = routeElement.GetAttribute("action").ToInt();
                string routePath = routeElement.GetAttribute("path");
                string typeName = routeElement.GetAttribute("type");
                bool ignoreAuthorize = routeElement.GetAttribute("ignoreAuthorize").ToBool();
                if (!string.IsNullOrEmpty(routePath))
                {
                    RouteItem routeItem = new RouteItem()
                    {
                        ActionId = actionId,
                        ScriptPath = FormatPythonPath(routePath),
                        TypeName = typeName,
                        IgnoreAuthorize = ignoreAuthorize
                    };
                    if (!_routePyDict.ContainsKey(actionId))
                    {
                        _routePyDict.TryAdd(actionId, routeItem);
                    }
                }
                else
                {
                    TraceLog.WriteError("Python route actionId:{0} path: is empyt.", actionId);
                }
            }
        }


        private string FormatLibPath(string path)
        {
			string rootPath = _runtimePath.Replace( @"\","/").TrimEnd('\\').TrimEnd('/') + "/";
			path = (path ?? "").Replace(@"\", "/").Replace("$ROOT_PATH/", rootPath);

            if (Directory.Exists(path))
            {
                return new DirectoryInfo(path).FullName;
            }
            return path;
        }

        /// <summary>
        /// 处理相对路径
        /// </summary>
        /// <param name="routePath"></param>
        /// <returns></returns>
        private static string FormatPythonPath(string routePath)
        {
            return (routePath ?? "").Replace("/", @"\").Replace("$ROOT_PATH\\", "");
        }

        /// <summary>
        /// 尝试获取Action
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="routeItem"></param>
        /// <returns></returns>
        public bool TryGetAction(int actionId, out RouteItem routeItem)
        {
            return _routePyDict.TryGetValue(actionId, out routeItem);
        }

        /// <summary>
        /// 尝试获取Lib
        /// </summary>
        /// <param name="key"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool TryGetLib(string key, out string path)
        {
            return _libPyDict.TryGetValue(key, out path);
        }

        /// <summary>
        /// 尝试加载脚本
        /// </summary>
        /// <param name="moduleName">相对路径</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool TryLoadPython(string moduleName, out PythonContext context)
        {
            context = null;
            try
            {
                context = PythonContext.CreateInstance(moduleName);
                return context != null;
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Load script setting file {0} error:{1}", moduleName, ex);
            }
            return false;
        }
    }
}