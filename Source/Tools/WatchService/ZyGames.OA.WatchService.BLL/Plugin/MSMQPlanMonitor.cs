using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Messaging;
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
    public class MSMQPlanMonitor
    {
        private const string SimplePlanInfoKey = "SimplePlanMSMQ";
        private string _serverUrl;
        private string _serverIP;
        private static int MaxErrorQueue = 100;

        static MSMQPlanMonitor()
        {
            int.TryParse(ConfigHelper.GetSetting("messageErrorMQMaxCount"), out MaxErrorQueue);
        }
        public MSMQPlanMonitor(string serverUrl, string serverIp)
        {
            _serverUrl = serverUrl;
            _serverIP = serverIp;
        }

        public void SearchSimplePlanInfo()
        {
            try
            {
                string pcName = Dns.GetHostName();
                var messageList = MessageQueue.GetPrivateQueuesByMachine(pcName);
                foreach (var queue in messageList)
                {
                    int queueCount = -1;
                    try
                    {
                        queueCount = queue.GetAllMessages().Length;
                    }
                    catch (Exception ex)
                    {

                    }
                    OaSimplePlanHelper.PostDataToServer(PlanType.msmq, queue.QueueName, queueCount);

                    if ((queueCount > MaxErrorQueue && queue.QueueName.ToLower().IndexOf("errorlogcmdsql") != -1) ||
                        (queueCount > ConfigContext.GetInstance().CitQueueMaxCount))
                    {
                        if (ContinuousManage.GetInstance().GetIsWaring(SimplePlanInfoKey + _serverIP + queue.QueueName))
                        {
                            string title = _serverIP + "服务器消息队列异常";
                            string content = string.Format("队列服务器：{0}<br />队列名称：{1}<br />队列总数：{2}<br />",
                                _serverIP,
                                queue.QueueName,
                                queueCount
                            );
                            int maxQueueCount = ConfigContext.GetInstance().CitQueueMaxCount;
                            if (queue.QueueName.ToLower().IndexOf("errorlogcmdsql") != -1)
                            {
                                maxQueueCount = MaxErrorQueue;
                            }
                            //Modify post trace
                            string planName = string.Format("{0}，消息队列{1}高于警戒值", _serverIP, queue.QueueName);
                            string planValue = string.Format("{0}/{1}", queueCount, maxQueueCount);
                            OaSimplePlanHelper.PostDataToServer(planName, planValue);
                            LogHelper.WriteException("服务器消息队列监视[" + _serverIP + "]>>", new Exception(content));

                            Mail139Helper.SendMail(title, content, ConfigContext.GetInstance().SendTo139Mail, true);

                        }
                    }
                    else
                    {
                        ContinuousManage.GetInstance().Reset(SimplePlanInfoKey + _serverIP + queue.QueueName);
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteException("MSMQ information error", ex);
            }
        }

    }
}
