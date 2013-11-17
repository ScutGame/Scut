using System;
using System.Configuration;
using System.Net;
using System.Threading;
using SvnDeploymentHosting;
using ZyGames.GameService.BaseService.LogService;
using ZyGames.OA.WatchService.BLL.Tools;
using ZyGames.SvnDeployment.Model;
using System.IO;
using ZyGames.Core.Util;

namespace ZyGames.OA.WatchService.BLL.Watch
{
    /// <summary>
    /// 项目部署
    /// </summary>
    public class SvnDeploymentWatch : BaseWatch
    {
        private static int WatchInterval = Int32.Parse(ConfigurationManager.AppSettings["SvnDeploy_Interval"]);
        private static readonly IIsVersion IIS_VERSION = (IIsVersion)Enum.Parse(typeof(IIsVersion), ConfigHelper.GetSetting("IIS_VERSION", "IIS6"));
        private static readonly string SVN_ROOT = ConfigurationManager.AppSettings["SVN_ROOT"];
        private static readonly string SVN_USER = ConfigurationManager.AppSettings["SVN_USER"];
        private static readonly string SVN_PASSWORD = ConfigHelper.GetConnectionString("SVN_PASSWORD");
        private static readonly string SVN_UPDATE_ROOT = ConfigurationManager.AppSettings["SVN_UPDATE_ROOT"];
        private static readonly string DEPLOY_ROOT = ConfigurationManager.AppSettings["DEPLOY_ROOT"];
        private static readonly string DEPLOY_PublishType = ConfigHelper.GetSetting("DEPLOY_PublishType", "Release");

        private static readonly int SVN_QDEPLOY_WAIT = Convert.ToInt32(ConfigHelper.GetSetting("SVN_QDEPLOY_WAIT", "90"));
        private static string ip = ConfigHelper.GetSetting("DEPLOY_IP");
        private const int SUCCESS = 1;
        private const int ERROR = 2;

        public SvnDeploymentWatch()
        {
            Interval = WatchInterval;
        }

        public bool IsRelease
        {
            get
            {
                return "Release".Equals(DEPLOY_PublishType);
            }
        }
        protected override bool DoProcess(object obj)
        {
            try
            {

                if (!Directory.Exists(SVN_UPDATE_ROOT))
                {
                    Directory.CreateDirectory(SVN_UPDATE_ROOT);
                }
                if (string.IsNullOrEmpty(ip))
                {
                    ip = GetServerIP();
                }
                var list = SvnProcesser.DeployList(ip);
                foreach (var projectAction in list)
                {
                    Logger.SaveLog(projectAction.Id + "Deploy the project begins...");
                    try
                    {
                        switch (projectAction.Type)
                        {
                            case 1:
                                StartIIS(projectAction, IIS_VERSION);
                                break;
                            case 2:
                                StopIIS(projectAction, IIS_VERSION);
                                break;
                            case 3:
                                SvnUpdate(projectAction);
                                break;
                            case 4:
                                Deploy(projectAction);
                                break;
                            case 5:
                                Cleanup(projectAction);
                                break;
                            case 6:
                                SvnCheckOut(projectAction);
                                break;
                            case 7:
                                QuickDeploy(projectAction, IIS_VERSION);
                                break;
                            case 8:
                                DeployPython(projectAction);
                                break;
                            case 11:
                                //创建计划任务
                                CreateSchTasks(projectAction);
                                break;
                            case 12:
                                //删除计划任务
                                DeleteSchTasks(projectAction);
                                break;
                            case 13:
                                //创建计划任务
                                StartSchTasks(projectAction);
                                break;
                            case 14:
                                StopSchTasks(projectAction);
                                break;
                            case 15:
                                //重启IIS或任务计划
                                ReStart(projectAction, IIS_VERSION);
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SaveLog(string.Format("SvnDeploy Execution [{0}] command error：", projectAction.Id), ex);
                        SvnProcesser.UpdateDeployStatus(projectAction.Id, 2, ex.ToString());
                    }
                    Logger.SaveLog(projectAction.Id + " the end of the project deployment");
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.SaveLog(ex);
                return false;
            }
        }
        /// <summary>
        /// 回收IIS pool或重启任务计划
        /// </summary>
        /// <param name="projectAction"></param>
        /// <param name="version"></param>
        private void ReStart(DepProjectAction projectAction, IIsVersion version)
        {
            if (CheckSchTaskOperate(projectAction))
            {
                ReStartSchTasks(projectAction);
            }
            else
            {
                RecycleIISPool(projectAction, version);
            }
        }

        /// <summary>
        /// 创建任务计划
        /// </summary>
        /// <param name="projectAction"></param>
        private void CreateSchTasks(DepProjectAction projectAction)
        {
            DepProjectItem projectItem = SvnProcesser.ProjectItem(projectAction.DepId);
            string targetPath = string.Empty;
            targetPath = projectItem.DeployPath.Replace("/", @"\");
            if (targetPath.IndexOf(":") == -1)
            {
                targetPath = CommandExeHelper.CombineDir(DEPLOY_ROOT, targetPath);
            }
            targetPath = CommandExeHelper.CombineDir(targetPath, projectItem.ExcludeFile);
            string strOutput = SchTasksOperate.Create(projectItem.WebSite, targetPath);
            Logger.SaveLog(strOutput);
            SvnProcesser.UpdateDeployStatus(projectAction.Id, SUCCESS, strOutput);

        }
        /// <summary>
        /// 删除计划任务
        /// </summary>
        /// <param name="projectAction"></param>
        private void DeleteSchTasks(DepProjectAction projectAction)
        {
            DepProjectItem projectItem = SvnProcesser.ProjectItem(projectAction.DepId);
            string strOutput = SchTasksOperate.Delete(projectItem.WebSite);
            Logger.SaveLog(strOutput);
            SvnProcesser.UpdateDeployStatus(projectAction.Id, SUCCESS, strOutput);
        }
        /// <summary>
        /// 停止计划任务
        /// </summary>
        /// <param name="projectAction"></param>
        private void StopSchTasks(DepProjectAction projectAction)
        {
            DepProjectItem projectItem = SvnProcesser.ProjectItem(projectAction.DepId);
            string strOutput = SchTasksOperate.StopRun(projectItem.WebSite);
            Logger.SaveLog(strOutput);
            SvnProcesser.UpdateDeployStatus(projectAction.Id, SUCCESS, strOutput);
        }
        /// <summary>
        /// 开启计划任务
        /// </summary>
        /// <param name="projectAction"></param>
        private void StartSchTasks(DepProjectAction projectAction)
        {
            DepProjectItem projectItem = SvnProcesser.ProjectItem(projectAction.DepId);
            string strOutput = SchTasksOperate.StartRun(projectItem.WebSite);
            Logger.SaveLog(strOutput);
            SvnProcesser.UpdateDeployStatus(projectAction.Id, SUCCESS, strOutput);
        }
        /// <summary>
        /// 重启
        /// </summary>
        /// <param name="projectAction"></param>
        private void ReStartSchTasks(DepProjectAction projectAction)
        {
            DepProjectItem projectItem = SvnProcesser.ProjectItem(projectAction.DepId);
            string strOutput = SchTasksOperate.StopRun(projectItem.WebSite);
            strOutput += SchTasksOperate.StartRun(projectItem.WebSite);
            Logger.SaveLog(strOutput);
            SvnProcesser.UpdateDeployStatus(projectAction.Id, SUCCESS, strOutput);
        }

        /// <summary>
        /// 清理SVN
        /// </summary>
        /// <param name="projectAction"></param>
        private void Cleanup(DepProjectAction projectAction)
        {
            DepProjectItem projectItem = SvnProcesser.ProjectItem(projectAction.DepId);
            DepProject depProject = SvnProcesser.Project(projectItem.DepId);
            string checkOutPath = Path.Combine(SVN_UPDATE_ROOT, depProject.Id.ToString());
            string svnArguments = string.Format(
                        "cleanup {0} --username {1} --password {2} --no-auth-cache --non-interactive --trust-server-cert",
                        checkOutPath,
                        SVN_USER,
                        SVN_PASSWORD
                    );

            Logger.SaveLog(string.Format("SvnDeploy执行svn.exe {0}", svnArguments));
            string strOutput = CommandExeHelper.Run("svn.exe", svnArguments);
            if (string.IsNullOrEmpty(strOutput))
            {
                strOutput = "cleanup end.";
            }
            SvnProcesser.UpdateDeployStatus(projectAction.Id, 1, strOutput);
        }

        private void SvnCheckOut(DepProjectAction projectAction)
        {
            SvnUpdate(projectAction, true);
        }


        /// <summary>
        /// 开启IIS
        /// </summary>
        /// <param name="projectAction"></param>
        /// <param name="iisVersion"></param>
        private void StartIIS(DepProjectAction projectAction, IIsVersion iisVersion)
        {
            DepProjectItem projectItem = SvnProcesser.ProjectItem(projectAction.DepId);
            string strOutput;
            IISHelper.GetIIS(iisVersion, projectItem.WebSite).Start(out strOutput);
            SvnProcesser.UpdateDeployStatus(projectAction.Id, SUCCESS, strOutput);
        }
        /// <summary>
        /// 停止IIS
        /// </summary>
        /// <param name="projectAction"></param>
        /// <param name="iisVersion"></param>
        private void StopIIS(DepProjectAction projectAction, IIsVersion iisVersion)
        {
            DepProjectItem projectItem = SvnProcesser.ProjectItem(projectAction.DepId);
            string strOutput;
            IISHelper.GetIIS(iisVersion, projectItem.WebSite).Stop(out strOutput);
            SvnProcesser.UpdateDeployStatus(projectAction.Id, SUCCESS, strOutput);

        }
        /// <summary>
        /// 回收IIS应用程序池
        /// </summary>
        /// <param name="projectAction"></param>
        /// <param name="iisVersion"></param>
        private void RecycleIISPool(DepProjectAction projectAction, IIsVersion iisVersion)
        {
            DepProjectItem projectItem = SvnProcesser.ProjectItem(projectAction.DepId);
            string strOutput;
            IISHelper.GetIIS(iisVersion, projectItem.WebSite).RecycleAppPool(out strOutput);
            SvnProcesser.UpdateDeployStatus(projectAction.Id, SUCCESS, strOutput);

        }

        private void SvnUpdate(DepProjectAction projectAction)
        {
            SvnUpdate(projectAction, false);
        }

        /// <summary>
        /// 更新SVN
        /// </summary>
        /// <param name="projectAction"></param>
        private void SvnUpdate(DepProjectAction projectAction, bool isCheckout)
        {
            DepProjectItem projectItem = SvnProcesser.ProjectItem(projectAction.DepId);
            DepProject depProject = SvnProcesser.Project(projectItem.DepId);
            string svnUrl = depProject.SvnPath.ToLower().StartsWith("http") ? depProject.SvnPath : SVN_ROOT + "/" + depProject.SvnPath;
            string checkOutPath = Path.Combine(SVN_UPDATE_ROOT, depProject.Id.ToString());
            string svnArguments = string.Empty;
            Exception error = null;
            
            //判断是否检出
            if (isCheckout || !Directory.Exists(checkOutPath))
            {
                //if (Directory.Exists(checkOutPath))
                //{
                //    try
                //    {
                //        Directory.Delete(checkOutPath, true);
                //    }
                //    catch (Exception ex)
                //    {
                //        error = ex;
                //    }
                //}
                if (projectAction.Revision <= 0)
                {
                    svnArguments = string.Format(
                        "checkout {0} {1} --username {2} --password {3} --no-auth-cache --non-interactive --trust-server-cert --force",
                        svnUrl,
                        checkOutPath,
                        SVN_USER,
                        SVN_PASSWORD
                    );
                }
                else
                {
                    svnArguments = string.Format(
                        "checkout {0} {1} -r {2} --username {3} --password {4} --no-auth-cache --non-interactive --trust-server-cert --force",
                        svnUrl,
                        checkOutPath,
                        projectAction.Revision,
                        SVN_USER,
                        SVN_PASSWORD
                    );
                }
            }
            else
            {
                if (projectAction.Revision <= 0)
                {
                    svnArguments = string.Format(
                       "update {0} --username {1} --password {2} --no-auth-cache --non-interactive --trust-server-cert --force",
                       checkOutPath,
                       SVN_USER,
                       SVN_PASSWORD
                    );
                }
                else
                {
                    svnArguments = string.Format(
                       "update {0} -r {1} --username {2} --password {3} --no-auth-cache --non-interactive --trust-server-cert --force",
                       checkOutPath,
                       projectAction.Revision,
                       SVN_USER,
                       SVN_PASSWORD
                    );
                }
            }
            if (error != null)
            {
                Logger.SaveLog(string.Format("SvnDeploy执行svn.exe {0}\r\n清除目录出错：", svnArguments), error);
            }
            else
            {
                Logger.SaveLog(string.Format("SvnDeploy执行svn.exe {0}", svnArguments));
            }

            string strOutput = CommandExeHelper.Run("svn.exe", svnArguments);
            if (error != null)
            {
                strOutput = "清除目录出错：" + error.Message + "\r\n" + strOutput;
            }
            SvnProcesser.UpdateDeployStatus(projectAction.Id, SUCCESS, strOutput);

        }

        /// <summary>
        /// 检查是否是任务计划操作
        /// </summary>
        /// <param name="projectAction"></param>
        /// <returns></returns>
        private bool CheckSchTaskOperate(DepProjectAction projectAction)
        {
            DepProjectItem projectItem = SvnProcesser.ProjectItem(projectAction.DepId);
            return !string.IsNullOrEmpty(projectItem.ExcludeFile) &&
                   projectItem.ExcludeFile.EndsWith(".exe");
        }

        private void QuickDeploy(DepProjectAction projectAction, IIsVersion iisVersion)
        {
            //停止IIS
            DepProjectItem projectItem = SvnProcesser.ProjectItem(projectAction.DepId);
            if (CheckSchTaskOperate(projectAction))
            {
                DoSchTaskQuickDeploy(projectAction, projectItem);
            }
            else
            {
                DoIIsQuickDeploy(projectAction, iisVersion, projectItem);
            }
        }

        /// <summary>
        /// 快速部署计划任务
        /// </summary>
        private void DoSchTaskQuickDeploy(DepProjectAction projectAction, DepProjectItem projectItem)
        {
            //更新缓存
            DepProject project = SvnProcesser.Project(projectItem.DepId);
            int gameId = project.GameId;
            int serverId = projectItem.ServerId;
            string host = project.ExcludeFile;//配置成Socket地址
            using (GameSocketClient client = new GameSocketClient())
            {
                string msg = string.Empty;
                client.ReceiveHandle += new SocketCallback((error, errorInfo, buffer) =>
                {
                    try
                    {

                        msg += string.Format("cache cmd:{0}-{1}\r\n", error, errorInfo);
                        //停止计划任务
                        msg += SchTasksOperate.StopRun(projectItem.WebSite);
                        string deploymsg = string.Empty;
                        if (Deploy(projectAction, ref deploymsg))
                        {
                            msg += deploymsg;
                            int sleepTime = SVN_QDEPLOY_WAIT + new Random().Next(1, 10);
                            Thread.Sleep(sleepTime * 1000);
                            //开启计划任务
                            msg += SchTasksOperate.StartRun(projectItem.WebSite);
                        }
                        SvnProcesser.UpdateDeployStatus(projectAction.Id, SUCCESS, msg);
                    }
                    catch (Exception ex)
                    {
                        new BaseLog().SaveLog("DoSchTaskQuickDeploy", ex);
                        SvnProcesser.UpdateDeployStatus(projectAction.Id, ERROR, ex.ToString());
                    }
                });
                client.Connect(host);
                string command = "GM:cache";
                client.SendToServer(gameId, serverId, 1000, command);
            }
        }

        private void DoIIsQuickDeploy(DepProjectAction projectAction, IIsVersion iisVersion, DepProjectItem projectItem)
        {
            string msg = string.Empty;
            string iisstopMsg;
            IISHelper.GetIIS(iisVersion, projectItem.WebSite).Stop(out iisstopMsg);
            msg += iisstopMsg;

            string deploymsg = string.Empty;
            string iisstartMsg = string.Empty;
            if (Deploy(projectAction, ref deploymsg))
            {
                int sleepTime = SVN_QDEPLOY_WAIT + new Random().Next(1, 10);
                Thread.Sleep(sleepTime * 1000);
                IISHelper.GetIIS(iisVersion, projectItem.WebSite).Start(out iisstartMsg);
            }

            msg += deploymsg;
            msg += iisstartMsg;
            SvnProcesser.UpdateDeployStatus(projectAction.Id, SUCCESS, msg);
        }

        private void Deploy(DepProjectAction projectAction)
        {
            string msg = string.Empty;
            Deploy(projectAction, ref msg);
            SvnProcesser.UpdateDeployStatus(projectAction.Id, SUCCESS, msg);
        }
        /// <summary>
        /// 部署项目
        /// </summary>
        /// <param name="projectAction"></param>
        private bool Deploy(DepProjectAction projectAction, ref string msg)
        {
            return DoDeploy(projectAction, ref msg);
        }

        private bool DoDeploy(DepProjectAction projectAction, ref string msg)
        {
            bool result = true;
            DepProjectItem projectItem = SvnProcesser.ProjectItem(projectAction.DepId);
            DepProject project = SvnProcesser.Project(projectItem.DepId);
            string checkSharePath = CommandExeHelper.CombineDir(SVN_UPDATE_ROOT, project.Id.ToString());
            string targetPath = string.Empty;
            targetPath = projectItem.DeployPath.Replace("/", @"\");
            if (targetPath.IndexOf(":") == -1)
            {
                targetPath = CommandExeHelper.CombineDir(DEPLOY_ROOT, targetPath);
            }
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
            if (!string.IsNullOrEmpty(project.SharePath.Trim()))
            {
                checkSharePath = CommandExeHelper.CombineDir(checkSharePath, project.SharePath);

                string exclude = "";//string.IsNullOrEmpty(project.ExcludeFile) ? "" : string.Format("/xf {0}", project.ExcludeFile);
                //更新共享的目录,如Code目录
                string robarguments = string.Format("{0} {1} /MIR {2}", checkSharePath, targetPath, exclude);
                Logger.SaveLog(string.Format("SvnDeploy shared directory robocopy.exe {0}", robarguments));
                msg += CommandExeHelper.Run("robocopy.exe", robarguments);
            }

            //差异目录,如天界行1服目录下的Web.Config文件
            string checkOutPath = CommandExeHelper.CombineDir(SVN_UPDATE_ROOT, project.Id.ToString());
            checkOutPath = CommandExeHelper.CombineDir(checkOutPath, projectItem.Name);
            bool isExist = Directory.Exists(checkOutPath);
            if (isExist)
            {
                //合并Debug与Release目录文件
                string arguments = string.Empty;
                if (IsRelease)
                {
                    arguments = string.Format("{0} {1} /e/y", checkOutPath, targetPath);
                }
                else
                {
                    arguments = string.Format("{0} {1} /e/y", Path.Combine(checkOutPath, "Debug"), targetPath);
                }

                Logger.SaveLog(string.Format("SvnDeploy differences in directory xcopy.exe {0}", arguments));
                msg += CommandExeHelper.Run("xcopy.exe", arguments);
            }
            //支持以前目录结构
            else if (string.IsNullOrEmpty(project.SharePath.Trim()))
            {
                checkOutPath = CommandExeHelper.CombineDir(SVN_UPDATE_ROOT, project.Id.ToString());
                string arguments = string.Format("{0} {1} /e/y", checkOutPath, targetPath);
                Logger.SaveLog(string.Format("SvnDeploy old directory xcopy.exe {0}", arguments));
                msg += CommandExeHelper.Run("xcopy.exe", arguments);
            }
            else
            {
                result = false;
                msg += string.Format("Directory error：{0} {1}", checkOutPath, targetPath);
            }
            return result;
        }

        /// <summary>
        /// 部署Python脚本
        /// </summary>
        /// <param name="projectAction"></param>
        private bool DeployPython(DepProjectAction projectAction)
        {
            string msg = "";
            bool result = true;
            string pythonPath = ConfigHelper.GetSetting("DEPLOY_Python_Path", "PyScript");
            DepProjectItem projectItem = SvnProcesser.ProjectItem(projectAction.DepId);
            DepProject project = SvnProcesser.Project(projectItem.DepId);
            //目标路径
            string targetPath = projectItem.DeployPath.Replace("/", @"\");
            if (targetPath.IndexOf(":") == -1)
            {
                targetPath = CommandExeHelper.CombineDir(DEPLOY_ROOT, targetPath);
                //Python目录
                targetPath = CommandExeHelper.CombineDir(targetPath, pythonPath);
            }
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
            string checkSharePath = CommandExeHelper.CombineDir(SVN_UPDATE_ROOT, project.Id.ToString());
            checkSharePath = CommandExeHelper.CombineDir(checkSharePath, project.SharePath);
            //Python目录
            checkSharePath = CommandExeHelper.CombineDir(checkSharePath, pythonPath);

            string exclude = "";//string.IsNullOrEmpty(project.ExcludeFile) ? "" : string.Format("/xf {0}", project.ExcludeFile);
            //更新共享的目录,如Code目录
            string robarguments = string.Format("{0} {1} /MIR {2}", checkSharePath, targetPath, exclude);
            Logger.SaveLog(string.Format("SvnDeploy Python shared directory robocopy.exe {0}", robarguments));
            msg += CommandExeHelper.Run("robocopy.exe", robarguments);

            SvnProcesser.UpdateDeployStatus(projectAction.Id, SUCCESS, msg);
            return result;
        }


    }
}
