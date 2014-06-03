using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZyGames.Framework.Common;

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
            _currDomain = AppDomain.CreateDomain(name, null, setup);
        }

        /// <summary>
        /// 
        /// </summary>
        public ScriptDomainContext DomainContext
        {
            get { return _context; }
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
        public object MainInstance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PrivateBinPath
        {
            get { return _currDomain.SetupInformation.PrivateBinPath; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ScriptDomainContext InitDomainContext()
        {
            var type = typeof(ScriptDomainContext);
            _context = (ScriptDomainContext)_currDomain.CreateInstanceFromAndUnwrap(type.Assembly.GetName().CodeBase, type.FullName);
            return _context;
        }

        /// <summary>
        /// 
        /// </summary>
        public ScriptRuntimeScope CreateScope(ScriptSettupInfo settupInfo)
        {
            var type = typeof(ScriptRuntimeScope);
            string amsKey = type.Assembly.GetName().Name;
            _scope = _context.GetInstance(amsKey, type.FullName, settupInfo) as ScriptRuntimeScope;
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
                AppDomain.Unload(_currDomain);
            }
            catch (Exception)
            {
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
