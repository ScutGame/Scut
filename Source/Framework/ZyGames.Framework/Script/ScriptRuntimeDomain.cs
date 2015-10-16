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
using System.Reflection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Script
{
    /// <summary>
    /// 
    /// </summary>
    public class ScriptRuntimeDomain : IDisposable
    {
        private AppDomain _currDomain;
        private ScriptDomainContext _context;
        private ScriptRuntimeScope _scope;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="privateBinPaths"></param>
        /// <returns></returns>
        public ScriptRuntimeDomain(string name, string[] privateBinPaths)
        {
            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationName = name;
            setup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            setup.PrivateBinPath = string.Join(";", privateBinPaths);
            setup.CachePath = setup.ApplicationBase;
            setup.ShadowCopyFiles = "true";
            setup.ShadowCopyDirectories = setup.ApplicationBase;
            InitDomain(name, setup);
        }

        private void InitDomain(string name, AppDomainSetup setup)
        {
#if STATIC

#else
            _currDomain = AppDomain.CreateDomain(name, null, setup);
            var type = typeof(ScriptDomainContext);
            _context = (ScriptDomainContext)_currDomain.CreateInstanceFromAndUnwrap(type.Assembly.GetName().CodeBase, type.FullName);
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public ScriptRuntimeScope Scope
        {
            get { return _scope; }
        }

        /// <summary>
        /// Main function args.
        /// </summary>
        public string[] MainArgs { get; set; }

        /// <summary>
        /// IMainScript
        /// </summary>
        public IMainScript MainInstance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal string PrivateBinPath
        {
            get { return _currDomain.SetupInformation.PrivateBinPath; }
        }

        internal void LoadAssembly(string key, string assemblyName)
        {
#if STATIC
            Assembly.LoadFrom(assemblyName);
#else
            _context.LoadAssembly(key, assemblyName);
#endif
        }
        private ScriptRuntimeScope CreateRuntimeScope(ScriptSettupInfo settupInfo, string amsKey, Type type)
        {
#if STATIC
            return type.CreateInstance<ScriptRuntimeScope>(settupInfo);
#else
            return _context.GetInstance(amsKey, type.FullName, settupInfo) as ScriptRuntimeScope;
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public ScriptRuntimeScope CreateScope(ScriptSettupInfo settupInfo)
        {
            var type = typeof(ScriptRuntimeScope);
            string amsKey = type.Assembly.GetName().Name;
            _scope = CreateRuntimeScope(settupInfo, amsKey, type);
            if (_scope != null)
            {
                _scope.Init();
            }
            return _scope;
        }


        /// <summary>
        /// 
        /// </summary>
        public void Unload()
        {
            try
            {
                if (_currDomain != null)
                {
                    AppDomain.Unload(_currDomain);
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Script domain error:{0}", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Unload();
            _currDomain = null;
            _scope.Dispose();
            _scope = null;
            _context = null;
            GC.SuppressFinalize(this);
        }

    }
}
