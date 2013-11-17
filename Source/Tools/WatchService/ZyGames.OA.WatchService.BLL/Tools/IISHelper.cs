using System;

namespace ZyGames.OA.WatchService.BLL.Tools
{
    public enum IIsVersion
    {
        IIS6,
        IIS7
    }

    public abstract class IISHelper
    {
        public static IISHelper GetIIS(IIsVersion version, string siteName)
        {
            switch (version)
            {
                case IIsVersion.IIS6:
                    return new IIS6Helper(siteName);
                    break;
                case IIsVersion.IIS7:
                    return new IIS7Helper(siteName);
                    break;
                default:
                    break;
            }
            throw new NotImplementedException("未实现IIs版本操作");
        }

        public abstract bool Start(out string msg);

        public abstract bool Stop(out string msg);

        /// <summary>
        /// 回收程序池
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public abstract bool RecycleAppPool(out string msg);
    }

    internal class IIS6Helper : IISHelper
    {
        private string _siteName;

        public IIS6Helper( string siteName)
        {
            this._siteName = siteName;
        }

        public override bool Start(out string msg)
        {
            string arguments = string.Format("//Nologo iisweb.vbs /start \"{0}\"", _siteName);
            msg = CommandExeHelper.Run("cscript.exe", arguments);
            return true;
        }

        public override bool Stop(out string msg)
        {
            string arguments = string.Format("//Nologo iisweb.vbs /stop \"{0}\"", _siteName);
            msg = CommandExeHelper.Run("cscript.exe", arguments);
            return true;
        }

        public override bool RecycleAppPool(out string msg)
        {
            //回收程序池
            string arguments = string.Format(" iisapp.vbs /a \"{0}\" /r", _siteName);
            msg = CommandExeHelper.Run("cscript.exe", arguments);
            return true;
        }
    }

    internal class IIS7Helper : IISHelper
    {
        private string _siteName;

        public IIS7Helper( string siteName)
        {
            this._siteName = siteName;
        }

        public override bool Start(out string msg)
        {
            string arguments = string.Format(" start site \"{0}\"", _siteName);
            msg = CommandExeHelper.Run("inetsrv/appcmd.exe", arguments);
            return true;
        }

        public override bool Stop(out string msg)
        {
            string arguments = string.Format(" stop site \"{0}\"", _siteName);
            msg = CommandExeHelper.Run("inetsrv/appcmd.exe", arguments);
            return true;
        }

        public override bool RecycleAppPool(out string msg)
        {
            string arguments = string.Format(" recycle apppool /apppool.name:\"{0}\"", _siteName);
            msg = CommandExeHelper.Run("inetsrv/appcmd.exe", arguments);
            return true;
        }
    }
}
