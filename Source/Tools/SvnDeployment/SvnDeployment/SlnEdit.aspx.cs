using System;
using SvnDeploymentHosting;
using ZyGames.SvnDeployment.Model;

namespace WebSite.SvnDeployment
{
    public partial class SlnEdit : BasePage
    {
        private bool bIsNew = false;
        private int nId = -1;

        private void ShowResult(string strReslut)
        {
            pResult.InnerText = strReslut;
            divMsgBox.InnerHtml = string.Format(
                "<script type=\"text/javascript\">alert(\"{0}\");</script>",
                strReslut
                );
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            DepProject depSln = null;
            string strId = Request.QueryString["id"];

            if ((!string.IsNullOrEmpty(strId)) && int.TryParse(strId, out nId))
            {
                depSln = SvnProcesser.Project(nId);

                if (null == depSln)
                {
                    bIsNew = true;
                    Response.Redirect("SlnEdit.aspx");
                    return;
                }
            }
            else
            {
                bIsNew = true;
            }

            if (bIsNew)
            {
                pTitle.InnerText = "项目部署创建";
            }
            else
            {
                pTitle.InnerText = depSln.Name;
            }

            if ("GET".Equals(Request.HttpMethod) && !Page.IsPostBack)
            {
                if (!bIsNew)
                {
                    tbSlnName.Text = depSln.Name;
                    tbSlnIP.Text = depSln.Ip;
                    tbSvnPath.Text = depSln.SvnPath;
                    txtSharePath.Text = depSln.SharePath;
                    txtExcludeFile.Text = depSln.ExcludeFile;
                    txtGameId.Text = depSln.GameId.ToString();
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {

                if (string.IsNullOrEmpty(tbSlnName.Text.Trim()))
                {
                    ShowResult("请输入方案名称。");
                    return;
                }

                if (string.IsNullOrEmpty(tbSlnIP.Text.Trim()))
                {
                    ShowResult("请输入机器IP。");
                    return;
                }
                if (string.IsNullOrEmpty(tbSvnPath.Text.Trim()))
                {
                    ShowResult("请输入SVN路径。");
                    return;
                }
                var project = new DepProject()
                {
                    Id = nId,
                    Name = tbSlnName.Text,
                    Ip = tbSlnIP.Text,
                    SvnPath = tbSvnPath.Text,
                    SharePath = txtSharePath.Text,
                    ExcludeFile = txtExcludeFile.Text,
                    GameId = int.Parse(txtGameId.Text)
                };
                if (bIsNew)
                {
                    if (IsExist(tbSlnName.Text))
                    {
                        ShowResult("方案名称已经存在。");
                        return;
                    }
                    SvnProcesser.AppendProject(project);
                }
                else
                {
                    SvnProcesser.UpateProject(project);
                }

                Response.Redirect("Default.aspx");
            }
            catch (Exception ex)
            {
                ShowResult(ex.Message);
            }
        }

        private bool IsExist(string name)
        {
            var list = SvnProcesser.ProjectList();
            return list.Exists(m => m.Name.ToLower().Equals(name.ToLower()));
        }
    }
}