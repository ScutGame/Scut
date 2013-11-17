using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZyGames.OA.BLL.Common;
using ZyGames.Common;
using SvnDeploymentHosting;
using ZyGames.SvnDeployment.Model;

namespace ZyGames.OA.BLL.Action
{
    /// <summary>
    /// 部署状态
    /// </summary>
    public class Action2003 : BaseAction
    {
        public Action2003(string actionId, HttpContext context)
            : base(actionId, context)
        {
        }

        protected override void DoAction()
        {
            int projectId = Convert.ToInt32(GetParam("deployId"));
            int pageSize;
            if (!int.TryParse(GetParam("pageSize"), out pageSize))
            {
                pageSize = 10;
            }
            List<DepProjectAction> projectList = SvnProcesser.DeployLog(projectId, pageSize);

            JsonObject jsonContainer = new JsonObject();

            List<JsonObject> jsonList = new List<JsonObject>();
            foreach (var item in projectList)
            {
                JsonObject jItem = new JsonObject();
                jItem.Add("Id", item.Id);
                jItem.Add("Ip", item.Ip);
                jItem.Add("DepId", item.DepId);
                jItem.Add("Type", item.Type);
                jItem.Add("Revision", item.Revision);
                jItem.Add("Status", item.Status);
                jItem.Add("ErrorMsg", FormatJson(item.ErrorMsg));
                jItem.Add("CreateDate", item.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"));
                jsonList.Add(jItem);
            }
            jsonContainer.Add("rows", jsonList.ToArray());
            string comboJson = jsonContainer.ToJson();

            _context.Response.Write(comboJson);
        }

        private string FormatJson(string str)
        {
            return str.Replace("\r\n", "<br>")
                .Replace("\n", "<br>")
                .Replace("\r", "<br>")
                .Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;")
                .Replace("----", "")
                .Replace("\\", "/")
                .Replace("'", "&nbsp;")
                .Replace("\"", "&nbsp;");
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
        }
    }
}
