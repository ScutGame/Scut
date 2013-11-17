using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using ZyGames.Core.Util;
using ZyGames.Core.Web;
using ZyGames.OA.WatchService.BLL.Tools;
using ZyGames.SimpleManager.Model;
using ZyGames.SimpleManager.Service.Common;
using ZyGames.SimpleManager.Service;

namespace ZyGames.OA.WatchService.BLL.Plugin
{
    /// <summary>
    /// CPU监控数据给指定服务器
    /// </summary>
    public class CpuPlanMonitor
    {
        private const string CpuMonitorKey = "CpuMonitor";
        /// <summary>
        /// 远程接收数据服务地址
        /// </summary>
        private string _serverUrl;
        private string saveDir = ConfigHelper.GetSetting("OAPlan_CpuSaveDir");
        private string monitorProcessString = ConfigHelper.GetSetting("OAPlan_CpuProcess");
        public List<string> monitorProcess = new List<string>();
        private PerformanceCounter PC = null;
        private string _serverIP;

        /// <summary>
        /// 提交性能监控数据
        /// </summary>
        public CpuPlanMonitor(string serverUrl, string serverIp)
        {
            _serverUrl = serverUrl;
            _serverIP = serverIp;
            PC = new PerformanceCounter();
            string[] arrary = monitorProcessString.Split(new char[] { '|' });
            foreach (string pro in arrary)
            {
                monitorProcess.Add(pro);
            }
        }

        /// <summary>
        /// 执行性能监控
        /// </summary>
        public void DoMonitor()
        {
            try
            {
                MonitorAll();
                MonitorProcess();
            }
            catch (Exception ex)
            {
                LogHelper.WriteException("CPU Monitoring error", ex);
            }
        }

        /// <summary>
        /// 监控系统整体性能
        /// </summary>
        public void MonitorAll()
        {
            PC.CategoryName = "Processor";//指定获取计算机进程信息  如果传Processor参数代表查询计算机CPU       
            PC.CounterName = "% Processor Time";//占有率
            PC.InstanceName = "_Total";
            float cpuValue = PC.NextValue();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            cpuValue = PC.NextValue();
            PostDataToServer(Math.Round(cpuValue, 2).ToString(), saveDir, "");

            if (ConfigContext.GetInstance().MaxCPU < cpuValue)
            {
                if (ContinuousManage.GetInstance().GetIsWaring(CpuMonitorKey + _serverIP + PC.CounterName))
                {
                    string title = _serverIP + "机器CPU过高";
                    string content = string.Format("IP：{2} CPU：{0}，高于警戒值：{1}", cpuValue.ToString(), ConfigContext.GetInstance().MaxCPU.ToString(), _serverIP);

                    //Modify post trace
                    string planName = string.Format("{0}，CPU高于警戒值", _serverIP);
                    string planValue = string.Format("{0}/{1}", Math.Round(cpuValue, 2), ConfigContext.GetInstance().MaxCPU);
                    OaSimplePlanHelper.PostDataToServer(planName, planValue);
                    LogHelper.WriteException("CPU监视[" + _serverIP + "]>>", new Exception(content));
                    Mail139Helper.SendMail(title, content, ConfigContext.GetInstance().SendTo139Mail, true);
                }
            }
            else
            {
                ContinuousManage.GetInstance().Reset(CpuMonitorKey + _serverIP + PC.CounterName);
            }
        }

        /// <summary>
        /// 监控系统某些进程
        /// </summary>
        public void MonitorProcess()
        {
            Process[] process = Process.GetProcesses();
            if (process.Length > 0)
            {
                PC.CategoryName = "Process";
                foreach (Process pr in process)
                {
                    if (monitorProcess.Contains(pr.ProcessName.ToLower()))
                    {
                        PC.InstanceName = pr.ProcessName;
                        float processValue = PC.NextValue();
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                        PostDataToServer(Math.Round(PC.NextValue(), 2).ToString(), saveDir, pr.ProcessName);
                    }
                }
            }
        }

        /// <summary>
        /// 提交数据到远程
        /// </summary>
        public void PostDataToServer(string value, string dir, string filename)
        {
            string postUrl = string.Format("{0}?type=cpu{1}", _serverUrl, ConstructParame(value, dir, filename));
            Console.WriteLine(postUrl);
            HttpHelper.GetReponseText(postUrl);
        }

        /// <summary>
        /// 构造要提交数据的参数信息
        /// </summary>
        /// <returns></returns>
        private string ConstructParame(string value, string dir, string filename)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("&savedir={0}", dir);
            sb.AppendFormat("&name={0}", filename);
            sb.AppendFormat("&data={0}", value);
            return sb.ToString();
        }

    }
}
