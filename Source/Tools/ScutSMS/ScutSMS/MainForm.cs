using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Scut.SMS.Config;
using ScutServerManager.Config;
using ZyGames.Framework.Common;

namespace Scut.SMS
{
    public partial class MainForm : BaseForm
    {
        private ServerManager _serverManager;
        private string _configFileName;
        private string searchDefaultPattern = "Use * match key";

        public MainForm()
        {
            InitializeComponent();
            saveConfigToolStrip.Enabled = false;
            _serverManager = new ServerManager();
        }

        public ServerManager ServerManager
        {
            get { return _serverManager; }
        }

        #region Method

        private void OnLoadConfig()
        {
            try
            {
                string currPath = MathUtils.RuntimePath;
                var configs = Directory.GetFiles(currPath, "*.config");
                if (configs.Length == 0)
                {
                    var dir = new DirectoryInfo(currPath);
                    if (dir.Name.EndsWith("bin", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (dir.Parent != null)
                        {
                            currPath = dir.Parent.FullName;
                            configs = Directory.GetFiles(currPath, "*.config");
                        }
                    }
                    if (configs.Length == 0)
                    {
                        return;
                    }
                }
                string first = configs.Where(p => p.EndsWith(".exe.config") || p.StartsWith("Web.config", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                _configFileName = string.IsNullOrEmpty(first) ? configs[0] : first;
                if (configs.Length > 1 && _configFileName.EndsWith("nlog.config", StringComparison.CurrentCultureIgnoreCase))
                {
                    _configFileName = configs[1];
                }
                var setting = _serverManager.LoadFile(_configFileName);
                if (setting != null)
                {
                    if (!_serverManager.RedisConfig.ConnectionString.HasConfig())
                    {
                        ConnectionString conn = setting.Connections.Where(p => p.IsDataLevel()).FirstOrDefault();
                        if (conn != null)
                        {
                            _serverManager.RedisConfig.ConnectionString = conn.CopyObject();
                        }
                    }
                    BindConfigGrid(setting);
                }
            }
            catch
            {
            }
        }

        private void BindConfigGrid(GameSetting setting)
        {
            this.configToolStripStatus.Text = string.IsNullOrEmpty(_configFileName)
                ? "New Config"
                : Path.GetFullPath(_configFileName);
            this.configPropertyGrid.SelectedObject = setting;
            ShowConfigSample(setting);
        }

        private void ShowConfigSample(GameSetting setting)
        {
            saveConfigToolStrip.Enabled = setting.IsModify;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Config:");
            sb.AppendLine(_serverManager.ToXml(setting));
            txtConfigSample.Text = sb.ToString();
        }

        private void SaveConfig(GameSetting setting)
        {
            if (string.IsNullOrEmpty(_configFileName))
            {
                var result = configSaveFileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _configFileName = configSaveFileDialog.FileName;
                }
                else
                {
                    return;
                }
            }
            _serverManager.SaveFile(setting, _configFileName);
            BindConfigGrid(setting);
        }

        private bool CheckSettingStatus()
        {
            GameSetting setting = configPropertyGrid.SelectedObject as GameSetting;
            if (setting == null) return true;
            if (setting.IsModify)
            {
                string msg = "The current configuration is not saved, if you need to save click \"Yes\"";
                var result = MessageBox.Show(msg, "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    SaveConfig(setting);
                    return false;
                }
            }
            return true;
        }

        private void BindRedisKeys(int top)
        {
            try
            {
                _serverManager.RedisSearchTop = top;
                switch (top)
                {
                    case 10:
                        getTop10KeyToolStripMenuItem.Checked = true;
                        getTop50KeyToolStripMenuItem.Checked = false;
                        getTop100KeyToolStripMenuItem.Checked = false;
                        break;
                    case 50:
                        getTop10KeyToolStripMenuItem.Checked = false;
                        getTop50KeyToolStripMenuItem.Checked = true;
                        getTop100KeyToolStripMenuItem.Checked = false;
                        break;
                    case 100:
                        getTop10KeyToolStripMenuItem.Checked = false;
                        getTop50KeyToolStripMenuItem.Checked = false;
                        getTop100KeyToolStripMenuItem.Checked = true;
                        break;
                    default:
                        break;
                }
                List<string> keys;
                if (_serverManager.TrySearchRedisKeys(out keys, top))
                {
                    redisToolStrip.ToolTipText = string.Format("Redis Server [{0}-{1}]",
                        _serverManager.RedisConfig.ReadOnlyHost,
                        _serverManager.RedisConfig.Db);
                    if (keys.Count == 0)
                    {
                        redisKeysToolStrip.ComboBox.Text = searchDefaultPattern;
                    }
                    redisKeysToolStrip.ComboBox.DataSource = keys;
                }
                else
                {
                    redisKeysToolStrip.ComboBox.Text = searchDefaultPattern;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
                redisToolStrip.ToolTipText = string.Format("Redis Server [{0}-{1}] not started",
                        _serverManager.RedisConfig.ReadOnlyHost,
                        _serverManager.RedisConfig.Db);
            }
        }

        private void WriteRedisResutl(string str)
        {
            textBoxBody.Text = str;
            resultTabControl.SelectTab(0);
        }

        private void WriteProtobufResutl(string str)
        {
            webBrowserBody.DocumentStream = new MemoryStream(Encoding.UTF8.GetBytes(str));
            resultTabControl.SelectTab(1);
        }

        #endregion

        #region Event

        private void MainForm_Load(object sender, EventArgs e)
        {
            OnLoadConfig();
            //BindRedisKeys(10);
        }

        private void redisToolStrip_Click(object sender, EventArgs e)
        {
            RedisForm redisForm = new RedisForm(_serverManager.RedisConfig, () =>
            {
                _serverManager.ReLoadRedis();
                BindRedisKeys(10);
            });
            redisForm.ShowDialog();
        }

        private void aboutToolStrip_Click(object sender, EventArgs e)
        {
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
            try
            {
                GC.Collect();
            }
            catch (Exception)
            {
            }
        }

        private void openToolStrip_ButtonClick(object sender, EventArgs e)
        {
            this.openToolStrip.ShowDropDown();
        }

        private void newConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckSettingStatus()) return;
                _configFileName = "";
                var setting = new GameSetting();
                BindConfigGrid(setting);

            }
            catch (Exception ex)
            {
                ShowError("New config error:" + ex.Message);
            }
        }

        private void openConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                if (!CheckSettingStatus()) return;
                var result = this.configOpenFileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _configFileName = configOpenFileDialog.FileName;
                    var setting = _serverManager.LoadFile(_configFileName);
                    if (setting != null)
                    {
                        BindConfigGrid(setting);
                    }
                    else
                    {
                        string name = Path.GetFileName(_configFileName);
                        ShowError(string.Format("The \"{0}\" is not a valid configuration file", name));
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Open config error:" + ex.Message);
            }
        }

        private void saveConfig_Click(object sender, EventArgs e)
        {
            try
            {
                SaveConfig(configPropertyGrid.SelectedObject as GameSetting);
            }
            catch (Exception ex)
            {
                ShowError("Save config error:" + ex.Message);
            }
        }

        private void saveToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _configFileName = "";
                SaveConfig(configPropertyGrid.SelectedObject as GameSetting);
            }
            catch (Exception ex)
            {
                ShowError("Save to config error:" + ex.Message);
            }
        }


        private void configPropertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            try
            {
                GameSetting setting = configPropertyGrid.SelectedObject as GameSetting;
                if (setting == null) return;
                setting.IsModify = true;
                ShowConfigSample(setting);
            }
            catch (Exception ex)
            {
                ShowError("Changed config error:" + ex.Message);
            }
        }

        private void settingToolStrip_Click(object sender, EventArgs e)
        {
        }

        private void ShowSettingForm(SettingTabType tabType)
        {
            var form = new SettingForm(tabType, WriteRedisResutl);
            form.ShowDialog();
        }

        #endregion

        private void getTop10KeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BindRedisKeys(10);
        }

        private void getTop50KeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BindRedisKeys(50);
        }

        private void getTop100KeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BindRedisKeys(100);
        }

        private void getMoreKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var redisKey = new RedisKeyForm(this);
            redisKey.ShowDialog();
        }

        public void SetRedisKeySelectBox(string key)
        {
            this.redisKeysToolStrip.Text = key;
        }

        private void redisGetValueToolStrip_ButtonClick(object sender, EventArgs e)
        {
            try
            {
                string key = redisKeysToolStrip.ComboBox.Text;
                if (string.IsNullOrEmpty(key) || key == searchDefaultPattern)
                {
                    return;
                }
                string msg;
                int error;
                if (_serverManager.TryGetRedisKeyValue(key, out error, out msg))
                {
                    WriteRedisResutl(msg);
                }
                else
                {
                    if (error == 102 &&
                        ShowConfirm("Entity is not set, setting whether to go?") == DialogResult.Yes)
                    {
                        ShowSettingForm(SettingTabType.Entity);
                        return;
                    }
                    ShowError("Redis connection fail.");
                }
            }
            catch (Exception ex)
            {
                ShowError("Get redis value error:" + ex.Message);
            }
        }


        private void removeKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string key = redisKeysToolStrip.ComboBox.Text;
            if (!string.IsNullOrEmpty(key) && ShowConfirm("Are you soure removed redis key?") == DialogResult.Yes)
            {
                if (_serverManager.TryRemoveRedisKey(key))
                {
                    BindRedisKeys(_serverManager.RedisSearchTop);
                    MessageBox.Show("Remove redis key successfully!");
                }
                else
                {
                    ShowError("Remove redis key faild!");
                }
            }
        }

        private void removeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ShowConfirm("Are you soure removed redis all key?") == DialogResult.Yes)
            {
                if (_serverManager.TryRemoveAllRedisKey())
                {
                    BindRedisKeys(_serverManager.RedisSearchTop);
                    MessageBox.Show("Remove redis key successfully!");
                }
                else
                {
                    ShowError("Remove redis key faild!");
                }
            }

        }

        private void redisInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string msg;
            if (_serverManager.TryRedisInfo(out msg))
            {
                WriteRedisResutl(msg);
            }
            else
            {
                ShowError("Redis connection fail.");
            }
        }

        private void redisKeysToolStrip_Enter(object sender, EventArgs e)
        {
            ToolStripComboBox searchBox = sender as ToolStripComboBox;
            if (searchBox.Text == searchDefaultPattern)
            {
                searchBox.Text = "";
            }
        }

        private void protoTolStrip_Click(object sender, EventArgs e)
        {
            try
            {
                var dymincEntity = AppSetting.Current.Entity.DymincEntity;
                if (dymincEntity == null)
                {
                    ShowSettingForm(SettingTabType.Entity);
                    return;
                }
                string result = ProtobufChecker.Check(dymincEntity);
                WriteProtobufResutl(result);
            }
            catch (Exception ex)
            {
                ShowError("Protobuf check error:" + ex.Message);
            }
        }

        private void testCaseToolStripLabel_Click(object sender, EventArgs e)
        {
            if (!AppSetting.Current.Contract.IsProxyServer)
            {
                ShowSettingForm(SettingTabType.Contract);
                return;
            }
            var test = new TestCaseForm();
            test.ShowDialog();
        }

        private void contractToolStripLabel_Click(object sender, EventArgs e)
        {
            var contract = new ContractForm();
            contract.ShowDialog();
        }

        private void changeKeyQueueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new CacheUpdateQueueForm(this);
            form.ShowDialog();
        }

        private void generateSqlQueueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new DatabaseUpdateQueueForm();
            form.ShowDialog();
        }

        private void sqlQueueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new SqlQueueForm(this);
            form.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var aboutForm = new AboutBox();
            aboutForm.ShowDialog();
        }

        private void settingTtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSettingForm(SettingTabType.Entity);
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = @"ScutSMS Readme.chm";
                var myProcess = Process.Start(fileName);
                //myProcess.WaitForExit(); 
            }
            catch (Exception ex)
            {
                ShowError("Open API error:" + ex.Message);
            }
        }

        private void backupRDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_serverManager.TryBackupRedis("RDB"))
                {
                    ShowInfo("Backup redis successfully");
                }
                else
                {
                    ShowInfo("Backup redis fail");
                }
            }
            catch (Exception ex)
            {
                ShowError("Backup redis of RDB error:" + ex.Message);
            }
        }

        private void backupAOFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_serverManager.TryBackupRedis("AOF"))
                {
                    ShowInfo("Backup redis successfully");
                }
                else
                {
                    ShowInfo("Backup redis fail");
                }
            }
            catch (Exception ex)
            {
                ShowError("Backup redis of AOF error :" + ex.Message);
            }
        }

        private void textBoxBody_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                this.textBoxBody.SelectAll();
            }
        }
        private void textBoxBody_Enter(object sender, EventArgs e)
        {
            this.textBoxBody.SelectAll();
        }

        private void txtConfigSample_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                this.txtConfigSample.SelectAll();
            }

        }


    }
}
