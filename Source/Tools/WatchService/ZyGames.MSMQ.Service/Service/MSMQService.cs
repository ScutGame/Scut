using System;
using System.Collections.Generic;
using System.Messaging;
using System.Net;
using System.Threading;
using ZyGames.Core.Util;
using ZyGames.MSMQ.Model;
using ZyGames.MSMQ.Service.Common;
using ZyGames.MSMQ.Service.Util;

namespace ZyGames.MSMQ.Service.Service
{
    /// <summary>
    /// 消息队列服务
    /// </summary>
    public class MSMQService
    {
        public void Run()
        {
            MessageConfig config = ConfigContext.GetInstance().SysMessageConfig;
            string pcName = Dns.GetHostName();
            HashSet<string> queuqHash = new HashSet<string>();
            new Thread(() =>
            {
                while (true)
                {
                    var messageList = MessageQueue.GetPrivateQueuesByMachine(pcName);
                    foreach (var messageQueue in messageList)
                    {
                        if (messageQueue.QueueName.ToLower().IndexOf("errorlogcmdsql") != -1)
                        {
                            continue;
                        }

                        if (!queuqHash.Contains(messageQueue.QueueName))
                        {
                            queuqHash.Add(messageQueue.QueueName);

                            MessageConfig tempConfig = new MessageConfig();
                            tempConfig.ManagerName = config.ManagerName;
                            tempConfig.ManagerThreadNumber = config.ManagerThreadNumber;
                            tempConfig.ManagerThreadTimeOut = config.ManagerThreadTimeOut;
                            tempConfig.ManagerMessagePath = messageQueue.Path;
                            tempConfig.ManagerErrorPath = config.ManagerErrorPath;
                            new Thread(new MSMQDistribution().Receive).Start(tempConfig);
                        }
                    }
                    Thread.Sleep(60000);
                }
            }).Start();
        }

        public void RunError()
        {
            MessageConfig config = ConfigContext.GetInstance().SysMessageConfig;
            string pcName = Dns.GetHostName();
            HashSet<string> queuqHash = new HashSet<string>();
            new Thread(() =>
            {
                while (true)
                {
                    var messageList = MessageQueue.GetPrivateQueuesByMachine(pcName);
                    foreach (var messageQueue in messageList)
                    {
                        if (messageQueue.QueueName.ToLower().IndexOf("errorlogcmdsql") != -1)
                        {
                            if (!queuqHash.Contains(messageQueue.QueueName))
                            {
                                queuqHash.Add(messageQueue.QueueName);

                                MessageConfig tempConfig = new MessageConfig();
                                tempConfig.IsErrorQueue = true;
                                tempConfig.ManagerName = config.ManagerName;
                                tempConfig.ManagerThreadNumber = config.ManagerThreadNumber;
                                tempConfig.ManagerThreadTimeOut = config.ManagerThreadTimeOut;
                                tempConfig.ManagerMessagePath = messageQueue.Path;
                                tempConfig.ManagerErrorPath = config.ManagerErrorPath;
                                new Thread(new MSMQDistribution().Receive).Start(tempConfig);
                            }
                        }
                    }
                    Thread.Sleep(60000);
                }
            }).Start();
        }
    }
}
