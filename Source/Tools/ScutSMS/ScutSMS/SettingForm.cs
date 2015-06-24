using System;
using System.Windows.Forms;
using Scut.SMS.Config;
using ZyGames.Framework.Common;

namespace Scut.SMS
{
    public enum SettingTabType
    {
        Entity = 0,
        Contract
    }

    public partial class SettingForm : BaseForm
    {
        private readonly Action<string> _errorOut;
        private AppSetting _setting;

        public SettingForm(SettingTabType tabType, Action<string> errorOut)
        {
            _setting = AppSetting.Current;
            _errorOut = errorOut;
            InitializeComponent();
            if (_setting.Entity.IsScript)
            {
                radioEntityPath.Checked = true;
            }
            else
            {
                radioEntityAsmPath.Checked = true;
            }
            LoadConfig();
            this.tabControl1.SelectTab((int)tabType);
        }

        private void LoadConfig()
        {

            txtEntityAsmPath.Text = _setting.Entity.AssemblyPath;
            txtEntityPath.Text = _setting.Entity.ScriptPath;

            ddDbType.Text = _setting.Contract.DBType.ToString();
            txtContractServer.Text = _setting.Contract.Server;
            txtPort.Text = _setting.Contract.Port.ToString();
            txtContractUid.Text = _setting.Contract.UserId;
            txtContractPwd.Text = _setting.Contract.Password;
            txtContractDatabase.Text = _setting.Contract.Database;
            txtCaseName.Text = _setting.Contract.CaseNameFormat;
            txtCaseOutPath.Text = _setting.Contract.CaseOutPath;

        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            SetEntityPath(_setting.Entity.IsScript);
        }

        private void SetEntityPath(bool isScript)
        {
            if (isScript)
            {
                txtEntityAsmPath.Enabled = false;
                txtEntityPath.Enabled = true;
                btnEntityOpen1.Enabled = true;
                btnEntityOpen2.Enabled = false;
            }
            else
            {
                txtEntityAsmPath.Enabled = true;
                txtEntityPath.Enabled = false;
                btnEntityOpen1.Enabled = false;
                btnEntityOpen2.Enabled = true;
            }
        }

        private void radioEntityPath_CheckedChanged(object sender, EventArgs e)
        {
            SetEntityPath(true);
        }

        private void radioEntityAsmPath_CheckedChanged(object sender, EventArgs e)
        {
            SetEntityPath(false);
        }

        private void btnEntityOpen1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtEntityPath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btnEntityOpen2_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Assembly|*.dll";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtEntityAsmPath.Text = openFileDialog.FileName;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                SaveConfig();
                Close();
            }
            catch (Exception ex)
            {
                ShowError("Save setting faild,Error:" + ex.Message);
            }
        }

        private void SaveConfig()
        {
            SaveEntityConfig();

            _setting.Contract.DBType = ddDbType.Text.ToEnum<DBType>();
            _setting.Contract.Server = txtContractServer.Text;
            _setting.Contract.Port = txtPort.Text.ToInt();
            _setting.Contract.UserId = txtContractUid.Text;
            _setting.Contract.Password = txtContractPwd.Text;
            _setting.Contract.Database = txtContractDatabase.Text;
            _setting.Contract.CaseNameFormat = txtCaseName.Text;
            _setting.Contract.CaseOutPath = txtCaseOutPath.Text;
            _setting.Save();
            TemplateFactory.Init();
        }

        private void SaveEntityConfig()
        {
            _setting.Entity.IsScript = radioEntityPath.Checked;
            _setting.Entity.AssemblyPath = txtEntityAsmPath.Text;
            _setting.Entity.ScriptPath = txtEntityPath.Text;
        }

        private void btnCaseOutpath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtCaseOutPath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btnEntityImport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveEntityConfig();
                _setting.Save();
                string error = AppSetting.LoadAssembly();
                if (!string.IsNullOrEmpty(error))
                {
                    if (_errorOut != null)
                    {
                        _errorOut(error);
                    }
                    ShowError("Import fail.");
                }
                else
                {
                    ShowInfo("Import successfully!");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }
    }
}
