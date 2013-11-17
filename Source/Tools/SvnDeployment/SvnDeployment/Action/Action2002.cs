using System;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ZyGames.OA.BLL.Common;
using SvnDeploymentHosting;
using ZyGames.OA.BLL.Model;
using ZyGames.SimpleManager.Service;
using ZyGames.SvnDeployment.Model;

namespace ZyGames.OA.BLL.Action
{
    /// <summary>
    /// 部署子方案
    /// </summary>
    public class Action2002 : BaseAction
    {
        private bool hasDoAction;

        private class PythonResult
        {
            public string RunTime { get; set; }
            public object Result { get; set; }
        }

        public Action2002(string actionId, HttpContext context)
            : base(actionId, context)
        {
            hasDoAction = false;
        }

        protected override void DoAction()
        {
            if (hasDoAction)
            {
                return;
            }
            int projectId = Convert.ToInt32(GetParam("deployId"));
            List<DepProjectItem> projectList = SvnProcesser.ProjectItemList(projectId);

            JsonObject jsonContainer = new JsonObject();

            List<JsonObject> jsonList = new List<JsonObject>();
            foreach (var item in projectList)
            {
                JsonObject jItem = new JsonObject();
                jItem.Add("Id", item.Id);
                jItem.Add("Name", item.Name);
                jItem.Add("WebSite", item.WebSite);
                jItem.Add("DeployPath", item.DeployPath);
                jItem.Add("ExcludeFile", item.ExcludeFile);
                jItem.Add("CreateDate", item.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"));
                jItem.Add("ServerId", item.ServerId);
                jsonList.Add(jItem);
            }
            jsonContainer.Add("rows", jsonList.ToArray());
            string comboJson = jsonContainer.ToJson();

            _context.Response.Write(comboJson);
        }

        protected override bool ValidateRequest(string operation)
        {
            if (operation == "add")
            {
                int deployId = Convert.ToInt32(GetParam("deployId"));
                string name = GetParam("Name");
                if (IsExist(deployId, name))
                {
                    var comboJson = new JsonObject().Add("state", false).Add("message", "子项目名称已经存在！").ToJson();
                    _context.Response.Write(comboJson);
                    return false;
                }
            }
            return true;
        }

        protected override void DoAdd()
        {
            int deployId = Convert.ToInt32(GetParam("deployId"));
            int serverId = Convert.ToInt32(GetParam("serverId"));
            string name = GetParam("Name");
            string webSite = GetParam("WebSite");
            string deployPath = GetParam("DeployPath");
            string excludeFile = GetParam("ExcludeFile");
            var projectItem = new DepProjectItem()
            {
                Id = 0,
                DepId = deployId,
                Name = name,
                WebSite = webSite,
                DeployPath = deployPath,
                ExcludeFile = excludeFile,
                ServerId = serverId
            };

            SvnProcesser.AppendProjectItem(projectItem);
        }
        private bool IsExist(int deployId, string name)
        {
            var list = SvnProcesser.ProjectItemList(deployId);
            return list.Exists(m => m.Name.ToLower().Equals(name.ToLower()));
        }

        protected override void DoSave()
        {
            int id = Convert.ToInt32(GetParam("Id"));
            int deployId = Convert.ToInt32(GetParam("deployId"));
            int serverId = Convert.ToInt32(GetParam("ServerId"));
            string name = GetParam("Name");
            string webSite = GetParam("WebSite");
            string deployPath = GetParam("DeployPath");
            string excludeFile = GetParam("ExcludeFile");
            var projectItem = new DepProjectItem()
            {
                Id = id,
                DepId = deployId,
                Name = name,
                WebSite = webSite,
                DeployPath = deployPath,
                ExcludeFile = excludeFile,
                ServerId = serverId
            };
            SvnProcesser.UpateProjectItem(projectItem);
        }

        protected override void DoDelete()
        {
            int id = Convert.ToInt32(GetParam("Id"));
            SvnProcesser.DeleteProjectItem(id);
        }

        protected override void DoOperated(string op)
        {
            string[] ids = GetParam("Id").Split(',');

            int projectId = Convert.ToInt32(GetParam("deployId"));
            int verId = 0;
            int.TryParse(GetParam("verId"), out verId);
            DepProject project = SvnProcesser.Project(projectId);
            bool hasSchTask = false;
            int type = 0;
            int gameId = 0;
            int serverId = 0;
            switch (op)
            {
                case "startiis":
                    hasSchTask = GetSchTasks(ids);
                    type = hasSchTask ? 13 : 1;
                    DoProcess(type, ids, project, verId);
                    break;
                case "stopiis":
                    hasSchTask = GetSchTasks(ids);
                    type = hasSchTask ? 14 : 2;
                    DoProcess(type, ids, project, verId);
                    break;
                case "clearupsvn":
                    DoProcess(5, ids, project, verId);
                    break;
                case "updatesvn":
                    bool ischeck;
                    bool.TryParse(GetParam("Ischeck"), out ischeck);
                    if (ischeck)
                    {
                        DoProcess(6, ids, project, verId);
                    }
                    else
                    {
                        DoProcess(3, ids, project, verId);
                    }
                    break;
                case "deploy":
                    DoProcess(4, ids, project, verId);
                    break;
                case "deployPython":
                    DoProcess(8, ids, project, verId);
                    break;
                case "quickdeploy":
                    DoProcess(7, ids, project, verId);
                    break;
                case "runpy":
                    gameId = Convert.ToInt32(GetParam("gameId"));
                    serverId = Convert.ToInt32(GetParam("serverId"));
                    DoProcessPython(ids, gameId, serverId);
                    hasDoAction = true;
                    break;
                case "msmq":
                    gameId = Convert.ToInt32(GetParam("gameId"));
                    serverId = Convert.ToInt32(GetParam("serverId"));
                    SelectMSMQLog(projectId, gameId, serverId);
                    hasDoAction = true;
                    break;
                case "createSchtask":
                    DoProcess(11, ids, project, verId);
                    break;
                case "deleteSchtask":
                    DoProcess(12, ids, project, verId);
                    break;
                case "restart":
                    DoProcess(15, ids, project, verId);
                    break;
                case "socketcmd":
                    DoProcessCommand(ids, project, GetParam("cmd"));
                    break;
                default:
                    break;
            }

        }

        private void DoProcessCommand(string[] ids, DepProject project, string cmd)
        {
            string command = "";
            switch (cmd)
            {
                case "upcache":
                    command = "GM:cache";//游戏实现GM命令
                    break;
                default:
                    break;
            }
            if (string.IsNullOrEmpty(command))
            {
                return;
            }
            int gameId = project.GameId;
            string host = project.ExcludeFile;//配置成Socket地址
            //using (GameSocketClient client = new GameSocketClient())
            //{
            //    client.ReceiveHandle += new SocketCallback(client_ReceiveHandle);
            //    client.Connect(host);
            //    if (ids.Length > 0)
            //    {
            //        var item = SvnProcesser.ProjectItem(int.Parse(ids[0]));
            //        int serverId = item.ServerId;
            //        client.SendToServer(gameId, serverId, 1000, command);
            //    }
            //}
            hasDoAction = true;
        }

        void client_ReceiveHandle(int error, string errorInfo, byte[] buffer)
        {
            var comboJson = new JsonObject().Add("state", error != 10000).Add("message", errorInfo).ToJson();
            _context.Response.Write(comboJson);
        }

        private bool GetSchTasks(string[] ids)
        {
            if (ids.Length > 0)
            {
                var item = SvnProcesser.ProjectItem(int.Parse(ids[0]));
                if (item != null && !string.IsNullOrEmpty(item.ExcludeFile))
                {
                    return true;
                }
            }
            return false;
        }

        private void SelectMSMQLog(int projectId, int gameId, int serverId)
        {
            PythonResult pythonResult = new PythonResult();
            pythonResult.RunTime = DateTime.Now.ToLongTimeString();
            try
            {
                string messageQueuePath = "";
                //var gameSet = new CacheGameSet().Get(gameId, serverId);
                //if (gameSet != null)
                //{
                //    var dev = SvnProcesser.Project(projectId);
                //    messageQueuePath = string.Format(gameSet.MessageQueuePath, serverId, dev.Ip);
                //}
                if (!string.IsNullOrEmpty(messageQueuePath))
                {
                    SimplePlanManagerService simplePlanManagerService = new SimplePlanManagerService();//计划任务服务对象
                    var planList = simplePlanManagerService.GetSimplePlanInfoList(120).FindAll(m => m.Name.StartsWith(messageQueuePath));
                    pythonResult.Result = planList;
                }
                else
                {
                    pythonResult.Result = string.Format("服务器MessageQueuePath的GameId:{0},ServerId:{1}配置为空", gameId, serverId);
                }
            }
            catch (Exception ex)
            {
                pythonResult.Result = ex.Message;
            }
            var isodata = new IsoDateTimeConverter() { DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm" };
            _context.Response.Write(JsonConvert.SerializeObject(pythonResult, isodata));
        }

        private void DoProcessPython(string[] ids, int gameId, int serverId)
        {
            PythonResult pythonResult = new PythonResult();
            pythonResult.RunTime = DateTime.Now.ToLongTimeString();
            try
            {
                if (ids.Length > 0)
                {
                    int id = 0;
                    if (int.TryParse(ids[0], out id))
                    {
                        string codeText = "";
                        List<JsonParameter> paramList = new List<JsonParameter>();
                        paramList.Add(new JsonParameter() { Key = "_py_code", Value = codeText });
                        paramList.Add(new JsonParameter() { Key = "_py_func_arg", Value = "" });
                        //发送

                    }
                }
            }
            catch (Exception ex)
            {
                pythonResult.Result = ex.Message;
            }
            _context.Response.Write(JsonConvert.SerializeObject(pythonResult));
        }

        private void DoProcess(int type, string[] ids, DepProject project, int version)
        {
            foreach (var item in ids)
            {
                int id = 0;
                if (int.TryParse(item, out id))
                {
                    SvnProcesser.AppendDeploy(type, project.Ip, id, version);
                }
            }
        }
    }
}
