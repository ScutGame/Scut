using System;
using System.Collections.Generic;
using System.Web;
using ZyGames.OA.BLL.Common;
using SvnDeploymentHosting;
using ZyGames.SvnDeployment.Model;

namespace ZyGames.OA.BLL.Action
{
    /// <summary>
    /// 部署方案
    /// </summary>
    public class Action2001 : BaseAction
    {
        public Action2001(string actionId, HttpContext context)
            : base(actionId, context)
        {
        }

        protected override void DoAction()
        {
            List<DepProject> projectList = SvnProcesser.ProjectList();

            JsonObject jsonContainer = new JsonObject();

            List<JsonObject> jsonList = new List<JsonObject>();
            foreach (var item in projectList)
            {
                JsonObject jItem = new JsonObject();
                jItem.Add("SlnId", item.Id);
                jItem.Add("SlnName", item.Name);
                jItem.Add("Ip", item.Ip);
                jItem.Add("SvnPath", item.SvnPath);
                jItem.Add("SharePath", item.SharePath);
                jItem.Add("ExcludeFile", item.ExcludeFile);
                jItem.Add("CreateDate", item.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"));
                jItem.Add("GameId", item.GameId);
                jsonList.Add(jItem);
            }
            jsonContainer.Add("rows", jsonList.ToArray());
            string comboJson = jsonContainer.ToJson();

            _context.Response.Write(comboJson);
        }

        protected override bool ValidateRequest(string operation)
        {
            return true;
        }

        protected override void DoAdd()
        {
        }

        protected override void DoSave()
        {
        }

        protected override void DoDelete()
        {
            int depId = Convert.ToInt32(GetParam("Id"));
            var list = SvnProcesser.ProjectItemList(depId);
            foreach (var depProjectItem in list)
            {
                SvnProcesser.DeleteProjectItem(depProjectItem.Id);
            }
            SvnProcesser.DeleteProject(depId);
        }
    }
}
