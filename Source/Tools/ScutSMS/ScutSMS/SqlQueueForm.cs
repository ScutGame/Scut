using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Scut.SMS.Comm;
using ServiceStack.Redis;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Data;

namespace Scut.SMS
{
    public partial class SqlQueueForm : BaseForm
    {
        private readonly string errorKey = "__GLOBAL_SQL_STATEMENT_*_error";
        private readonly string hashErrorKey = "__QUEUE_SQL_SYNC_ERROR";
        private readonly MainForm _parent;
        private byte[][] _buffers;
        private string _setId;
        private int _currentIndex;
        private int _completedCount;
        private int _errorCount;
        private DbBaseProvider provider;

        public SqlQueueForm(MainForm parent)
        {
            _parent = parent;
            InitializeComponent();
            DoRefresh();
        }

        private void DoRefresh()
        {
            try
            {
                lblCount.Text = "0";
                _parent.ServerManager.TryExecute(redisClient =>
                {
                    long errorQueueNum = 0;
                    errorQueueNum += redisClient.ZRange(hashErrorKey, 0, int.MaxValue).Length;
                    var setList = redisClient.SearchKeys(errorKey).ToArray();
                    if (setList.Length > 0)
                    {
                        _setId = setList[0];
                        _buffers = redisClient.ZRange(_setId, 0, int.MaxValue);
                        errorQueueNum += _buffers.Length;
                    }
                    lblCount.Text = errorQueueNum.ToString("D");
                    return true;
                });
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            btnRefresh.Enabled = false;
            DoRefresh();
            _currentIndex = 0;
            _completedCount = 0;
            _errorCount = 0;
            RefreshWorkBar(0);
            btnRefresh.Enabled = true;
        }


        private void btnExecute_Click(object sender, EventArgs e)
        {
            btnExecute.Enabled = false;
            _currentIndex = 0;
            _completedCount = 0;
            _errorCount = 0;
            RefreshWorkBar(0);
            msgListBox.Items.Clear();
            executeQueueWorker.RunWorkerAsync();
            btnExecute.Enabled = true;
        }

        private void executeQueueWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _parent.ServerManager.TryExecute(redisClient =>
                {
                    for (int i = _currentIndex; i < _buffers.Length; i++)
                    {
                        if (DoWriteToDb(redisClient, _buffers[i]))
                        {
                            _completedCount++;
                        }
                        _currentIndex++;
                        int value = (int)(_currentIndex * 100) / _buffers.Length;
                        RefreshWorkBar(value);
                    }
                    return true;
                });
            }
            catch (Exception)
            {

            }
        }

        private void executeQueueWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _buffers = new byte[0][];
            btnExecute.Enabled = true;
        }

        private void RefreshWorkBar(int value)
        {
            try
            {
                lblErrorCount.Text = _errorCount.ToString("D");
                lblCompleted.Text = _completedCount.ToString("D");
                queueProgressBar.Value = value;
                lblBar.Text = string.Format("{0}%", value);
            }
            catch (Exception ex)
            {
            }
        }

        private bool DoWriteToDb(RedisClient redisClient, byte[] buffer)
        {
            SqlStatement statement = null;
            try
            {
                statement = ProtoBufUtils.Deserialize<SqlStatement>(buffer);
                if (statement != null)
                {
                    var dbProvider = DbConnectionProvider.CreateDbProvider("", statement.ProviderType, statement.ConnectionString);
                    if (dbProvider != null)
                    {
                        var paramList = ConvertParam(dbProvider, statement.Params);
                        dbProvider.ExecuteQuery(statement.CommandType, statement.CommandText, paramList);
                        redisClient.ZRem(_setId, buffer);
                        return true;
                    }
                }
                _errorCount++;
                return false;
            }
            catch (Exception ex)
            {
                _errorCount++;
                try
                {
                    if (removeCheckBox.Checked)
                    {
                        redisClient.ZRem(_setId, buffer);
                    }
                }
                catch
                {
                }
                if (statement != null)
                {
                    try
                    {
                        msgListBox.Items.Add(string.Format("pos:{0}, error:{1}", _currentIndex, ex.Message));
                        TraceLog.WriteError("WriteToDb:{0}\r\n{1}\r\nParam:{2}",
                            ex.Message,
                            statement.CommandText,
                            GetParamToString(statement.Params));
                    }
                    catch
                    {
                    }
                }
                return false;
            }
        }

        private IDataParameter[] ConvertParam(DbBaseProvider dbProvider, SqlParam[] paramList)
        {
            IDataParameter[] list = new IDataParameter[paramList.Length];
            for (int i = 0; i < paramList.Length; i++)
            {
                SqlParam param = paramList[i];
                list[i] = dbProvider.CreateParameter(param.ParamName, param.DbTypeValue, param.Size, param.Value.Value);
            }
            return list;
        }

        private string GetParamToString(SqlParam[] list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.AppendLine(string.Format("    {0}:{1}", item.ParamName, item.Value));
            }
            return sb.ToString();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }
    }
}
