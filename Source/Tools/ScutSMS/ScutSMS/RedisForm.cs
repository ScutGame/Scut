using System;
using System.Windows.Forms;
using ScutServerManager;
using ScutServerManager.Config;

namespace Scut.SMS
{
    public partial class RedisForm : Form
    {
        private readonly Action _callback;

        public RedisForm(RedisSetting setting, Action callback)
        {
            _callback = callback;
            InitializeComponent();

            settingPropertyGrid.SelectedObject = setting;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Save();
            if (_callback != null)
            {
                _callback.BeginInvoke(null, null);
            }
            Close();
        }

        private void Save()
        {
            RedisSettingFactory.Save(settingPropertyGrid.SelectedObject as RedisSetting);
        }

    }
}
