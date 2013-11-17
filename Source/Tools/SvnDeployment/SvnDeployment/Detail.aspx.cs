using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebSite.SvnDeployment
{
    public partial class Detail : BasePage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            //string strId = Request.QueryString["id"];
            //string strVer = Request.QueryString["ver"];
            //int nId = - 1;
            //int nVer = - 1;

            //if(
            //    (!string.IsNullOrEmpty(strId)) &&
            //    int.TryParse(strId, out nId) &&
            //    (!string.IsNullOrEmpty(strVer)) &&
            //    int.TryParse(strVer, out nVer)
            //    )
            //{
            //    DepSln depSln = Database.GetDepSlnById(Context, nId);

            //    if(null == depSln)
            //    {
            //        Global.HttpResponse403(Response);
            //        return;
            //    }

            //    DepVersion depVer = Database.GetDepVersionBySlnGuidAndVerNum(Context, depSln.SlnGuid, nVer);

            //    if(null == depVer)
            //    {
            //        Global.HttpResponse403(Response);
            //        return;
            //    }

            //    pTitle.InnerText = string.Format("{0} - Ver.{1}", depSln.SlnName, nVer);
            //    divVersionNumber.InnerText = depVer.VersionNumber.ToString();
            //    divVersionDescription.InnerText = depVer.VersionDescription;
            //    divRevision.InnerText = ((0 >= depVer.Revision) ? "最新" : depVer.Revision.ToString());
            //    divPublishedAt.InnerText = depVer.PublishedAt.ToString();
            //    divStatus.InnerText = Database.GetStatusString(depVer.Status);
            //    divStatusXt.InnerText = depVer.StatusXt;
            //}
            //else
            //{
            //    Global.HttpResponse403(Response);
            //}
		}
	}
}