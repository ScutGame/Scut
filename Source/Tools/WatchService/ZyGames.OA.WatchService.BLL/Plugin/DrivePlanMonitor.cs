using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using ZyGames.Core.Util;
using ZyGames.Core.Web;
using ZyGames.OA.WatchService.BLL.Tools;
using ZyGames.SimpleManager.Model;
using ZyGames.SimpleManager.Service;
using ZyGames.SimpleManager.Service.Common;

namespace ZyGames.OA.WatchService.BLL.Plugin
{
    class DrivePlanMonitor
    {
        private const string DriveInfoKey = "DriveInfoManager";
        private string _serverIP = string.Empty;
        private string _serverUrl;

        public DrivePlanMonitor(string serverUrl, string serverIp)
        {
            this._serverIP = serverIp;
            _serverUrl = serverUrl;
        }

        /// <summary>
        /// 检查服务器磁盘空间容量
        /// </summary>
        public void CheckDriveInfo()
        {
            try
            {
                DriveInfo[] alldrive = DriveInfo.GetDrives();
                List<string> listWaring = new List<string>();
                foreach (DriveInfo d in alldrive)
                {
                    if (d.DriveType == DriveType.Fixed)
                    {
                        DriveWaring checkItem = null;
                        foreach (DriveWaring driveItem in ConfigContext.GetInstance().DriveWaringSet)
                        {
                            if (d.Name.IndexOf(driveItem.DriveName) != -1)
                            {
                                checkItem = driveItem;
                                break;
                            }
                        }

                        int curFreeSpace = Convert.ToInt32(d.TotalFreeSpace / 1048576); //计算剩余多少M
                        int waringSize = 10000;
                        if (checkItem != null)
                        {
                            waringSize = checkItem.WaringSize;
                        }

                        PostDataToServer(_serverIP, d.Name, curFreeSpace);

                        string waringKey = DriveInfoKey + _serverIP + d.Name;
                        if (curFreeSpace < waringSize)
                        {

                            if (ContinuousManage.GetInstance().GetIsWaring(waringKey))
                            {
                                string content = string.Format("磁盘：{0}，剩余空间：{1}MB，低于警戒值：{2}MB<br />", d.Name,
                                                               curFreeSpace, waringSize);
                                listWaring.Add(content);

                                //Modify post trace
                                string planName = string.Format("{0},磁盘：{1}低于警戒值", _serverIP, d.Name);
                                string planValue = string.Format("{0}/{1}MB", curFreeSpace, waringSize);
                                OaSimplePlanHelper.PostDataToServer(planName, planValue);
                            }

                        }
                        else
                        {
                            ContinuousManage.GetInstance().Reset(waringKey);
                        }
                    }
                }
                if (listWaring.Count > 0)
                {
                    if (ContinuousManage.GetInstance().GetIsWaring(DriveInfoKey + _serverIP))
                    {
                        string title = _serverIP + "磁盘空间不足";
                        string content = string.Format("共{0}个磁盘空间不足<br />", listWaring.Count);
                        foreach (string item in listWaring)
                        {
                            content += item;
                        }

                        LogHelper.WriteException("Disk monitoring [" + _serverIP + "]>>", new Exception(content));
                        Mail139Helper.SendMail(title, content, ConfigContext.GetInstance().SendTo139Mail, true);
                    }
                }
                else
                {
                    ContinuousManage.GetInstance().Reset(DriveInfoKey + _serverIP);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteException("Check server disk space remaining capacity error", ex);
            }
        }

        /// <summary>
        /// 提交数据到远程
        /// </summary>
        public void PostDataToServer(string ip, string name, int size)
        {
            string postUrl = string.Format("{0}?type=drive&ip={1}&name={2}&size={3}", _serverUrl, ip, name, size);
            Console.WriteLine(postUrl);
            HttpHelper.GetReponseText(postUrl);
        }
    }
}
