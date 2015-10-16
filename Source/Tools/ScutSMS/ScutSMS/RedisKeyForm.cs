using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Scut.SMS
{
    public partial class RedisKeyForm : BaseForm
    {
        private readonly MainForm _parent;
        private DataTable dtKey = new DataTable();

        public RedisKeyForm(MainForm parent)
        {
            _parent = parent;
            InitializeComponent();

            dtKey.Columns.Add(new DataColumn("Id", typeof(int)));
            dtKey.Columns.Add(new DataColumn("Key", typeof(string)));
            dtKey.Columns.Add(new DataColumn("Identity", typeof(string)));

            IsReplaceOption(ckFindOrReplace.Checked);
            txtKey.Text = _parent.ServerManager.RedisSearchPattern;
            waitPanel.Visible = false;
            //LoadRedisKey();
        }

        private void LoadRedisKey()
        {
            try
            {

#if DEBUG
                OnAsyncLoad();
#else
                waitPanel.Visible = true;
                OnAsyncLoad();
#endif
            }
            catch (Exception er)
            {
                ShowError("Load redis keys error:" + er.Message);
            }
        }

        private void OnAsyncLoad()
        {
            try
            {
                dtKey.Clear();
                List<string> keys;
                if (_parent.ServerManager.TrySearchRedisKeys(out keys))
                {
                    this.lblKeyCount.Text = keys.Count.ToString();
                    int rowId = 0;
                    foreach (var key in keys)
                    {
                        rowId++;
                        var dr = dtKey.NewRow();
                        dr[0] = rowId;
                        dr[1] = key;
                        var arr = key.Split('_');
                        dr[2] = arr.Length > 1 ? arr[1] : "0";
                        dtKey.Rows.Add(dr);
                    }
                }
                if (keyDataGridView != null)
                {
                    keyDataGridView.DataSource = dtKey;
                }
                waitPanel.Visible = false;
            }
            catch (Exception er)
            {
                try
                {
                    ShowError("Load redis keys error:" + er.Message);
                }
                catch
                {
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (keyDataGridView.CurrentRow != null)
            {
                string key = keyDataGridView.CurrentRow.Cells["Key"].Value.ToString();
                if (!string.IsNullOrEmpty(key))
                {
                    _parent.SetRedisKeySelectBox(key);
                }
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                btnSearch.Enabled = false;
                _parent.ServerManager.RedisSearchPattern = txtKey.Text;
                OnAsyncLoad();
            }
            catch (Exception er)
            {
                ShowError("Load redis keys error:" + er.Message);
            }
            finally
            {
                btnSearch.Enabled = true;
            }
        }

        private void keyDataGridView_CellContentDoubleClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                txtKey.Text = keyDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
        }

        private void ckFindOrReplace_CheckedChanged(object sender, EventArgs e)
        {
            IsReplaceOption(ckFindOrReplace.Checked);
        }

        private void IsReplaceOption(bool isReplace)
        {
            this.Text = isReplace ? "Replace Redis Key" : "Find Redis Key";
            this.btnReplace.Visible = isReplace;
            this.btnReplaceAll.Visible = isReplace;
            this.replacePanel.Visible = isReplace;
            topPanel.Height = isReplace ? 87 : 47;
            middlePanel.Top = isReplace ? 87 : 47;
            middlePanel.Height = isReplace ? 318 : 358;
            //middlePanel.Refresh();
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            try
            {
                string fromReplace = this.txtKey.Text;
                string toReplace = this.txtReplace.Text;
                var row = keyDataGridView.CurrentRow;
                if (row != null)
                {
                    string key = row.Cells["Key"].Value.ToString();
                    string replaceKey = DoReplace(key, fromReplace, toReplace, ckMatchCase.Checked);
                    if (_parent.ServerManager.TryRenameKey(key, replaceKey))
                    {
                        row.Cells["Key"].Value = replaceKey;
                        return;
                    }
                }
                ShowInfo("Replace count 0!");
            }
            catch (Exception er)
            {
                ShowError("Replace error:" + er.Message);
            }
        }
        private void btnReplaceAll_Click(object sender, EventArgs e)
        {
            try
            {
                string fromReplace = this.txtKey.Text;
                string toReplace = this.txtReplace.Text;
                var dict = new Dictionary<string, string>();
                foreach (DataGridViewRow row in keyDataGridView.Rows)
                {
                    string key = row.Cells["Key"].Value.ToString();
                    string replaceKey = DoReplace(key, fromReplace, toReplace, ckMatchCase.Checked);
                    dict[key] = replaceKey;
                    row.Cells["Key"].Value = replaceKey;
                }
                if (_parent.ServerManager.TryRenameKey(dict))
                {
                }
                ShowInfo("Replace count " + dict.Count);
            }
            catch (Exception er)
            {
                ShowError("Replace all error:" + er.Message);
            }
        }

        private void btnMoveToDb_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowConfirm(string.Format("Are you sure move the key from the result view count {0} to the databases?", keyDataGridView.Rows.Count)) == DialogResult.No)
                {
                    return;
                }
                IEnumerable<string> keys = (from DataGridViewRow row in keyDataGridView.Rows select row.Cells["Key"].Value.ToString());
                int success;
                int count = keyDataGridView.Rows.Count;
                string info = "";
                var conn = _parent.ServerManager.RedisConfig.ConnectionString;
                if (_parent.ServerManager.TryMoveKeyToDb(conn, keys, out success))
                {
                    info = success + " key to move successfully into the database";
                }
                else
                {
                    info = "key to move failure into the database";
                }
                ShowInfo(info);
            }
            catch (Exception ex)
            {
                ShowError("Move to db error:" + ex.Message);
            }
        }


        private string DoReplace(string key, string fromReplace, string toReplace, bool matchCase)
        {
            if (matchCase)
            {
                return (key ?? "").Replace(fromReplace, toReplace);
            }
            return Regex.Replace(key ?? "", fromReplace, toReplace, RegexOptions.IgnoreCase);
        }

    }
}
