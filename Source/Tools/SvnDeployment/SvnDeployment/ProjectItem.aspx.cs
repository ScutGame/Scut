using System;
using SvnDeploymentHosting;

namespace ZyGames.OA.SvnDeployment
{
    public partial class ProjectItem : BasePage
    {
        public int DeployId
        {
            get { return Request["Id"] == null ? 0 : Convert.ToInt32(Request["Id"]); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lblSubTitle.Text = SvnProcesser.Project(DeployId).Name;
            }
        }
    }
}
