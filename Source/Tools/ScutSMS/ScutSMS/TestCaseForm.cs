using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Scut.SMS.Config;
using ZyGames.Framework.Common;
using ZyGames.Framework.Data;

namespace Scut.SMS
{
    public partial class TestCaseForm : BaseForm
    {
        private Action handle;
        private DataTable paramList;
        private int selectActionId;

        public TestCaseForm()
        {
            InitializeComponent();
            handle = OnAsyncLoad;

        }

        private void OnAsyncLoad()
        {
            try
            {
                OnBindGameList();
            }
            catch (Exception er)
            {
                ShowError("Loading error:" + er.Message);
            }
        }

        private void OnBindContractTree(int slnId)
        {
            var parent = new TreeNode("Interface");
            parent.Tag = 0;
            parent.Expand();
            parent.Nodes.AddRange(TemplateFactory.ReadContract(slnId));
            contractTreeView.Nodes.Clear();
            contractTreeView.Nodes.Add(parent);
            contractTreeView.Select();

            paramList = TemplateFactory.ReadContractParam(slnId);
            //设定默认接口
            var firstNode = parent.FirstNode;
            if (firstNode != null)
            {
                contractTreeView.SelectedNode = firstNode;

                int actionId;
                if (int.TryParse(firstNode.Tag.ToString(), out actionId))
                {
                    BindCaceView(actionId, firstNode.Text);
                }
            }
        }

        private void OnBindGameList()
        {
            DataTable dt = TemplateFactory.ReadSolutions();
            var row = dt.NewRow();
            row["SlnID"] = "0";
            row["SlnName"] = "Select Game";
            dt.Rows.InsertAt(row, 0);

            gameListComboBox.DataSource = dt;
            gameListComboBox.DisplayMember = "SlnName";
            gameListComboBox.ValueMember = "SlnID";
        }

        private void TestCaseForm_Load(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = false;
#if DEBUG
                OnAsyncLoad();
#else
                handle.BeginInvoke(null, null);
#endif
            }
            catch (Exception)
            {
            }
        }

        private void btnCloce_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                string gameId = gameListComboBox.Text;
                bool isReplace = ShowConfirm("Is replace testCase files?") == DialogResult.Yes;
                string outPath = TemplateFactory.BuildTestCase(gameId, contractTreeView.Nodes, paramList, isReplace);
                if (ShowConfirm("Generate testCase successfully, Is open the testCase output path?") == DialogResult.Yes)
                {
                    OpenDir(outPath);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void gameListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int slnId;
            if (int.TryParse(gameListComboBox.SelectedValue.ToString(), out slnId))
            {
                OnBindContractTree(slnId);
            }
        }

        private void contractTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                int actionId;
                if (int.TryParse(e.Node.Tag.ToString(), out actionId))
                {
                    BindCaceView(actionId, e.Node.Text);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void BindCaceView(int actionId, string actionName)
        {
            selectActionId = actionId;
            txtCase.Text = TemplateFactory.LoadTastCase(actionId, actionName, paramList);
            btnSave.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string gameId = gameListComboBox.Text;
                TemplateFactory.SaveTestCase(txtCase.Text, gameId, selectActionId);
                btnSave.Enabled = false;
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void txtCase_TextChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
        }

    }
}
