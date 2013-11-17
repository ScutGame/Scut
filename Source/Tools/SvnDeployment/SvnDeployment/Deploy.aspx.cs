using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using ZyGames.Core.Util;
using ZyGames.SvnDeployment.Model;
using SvnDeploymentHosting;

namespace WebSite.SvnDeployment
{
    public partial class Deploy : BasePage
    {
        public int DeployId
        {
            get { return Request["Id"] == null ? 0 : Convert.ToInt32(Request["Id"]); }
        }

        public int GameId
        {
            get { return Request["GameId"] == null ? 0 : Convert.ToInt32(Request["GameId"]); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var depSln = SvnProcesser.Project(DeployId);
            pTitle.Text = depSln.Name;
        }

    }
}
