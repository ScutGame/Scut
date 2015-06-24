using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using LuaInterface;
using Scut.Client.Net;
using Scut.Client.Runtime;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Security;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Sockets;

namespace ScutClientSimulator
{
    public partial class Main : Form
    {
        private ResponseLog _responseLog = new ResponseLog(1000);
        private Thread _watchThread;
        private UserToken _token = new UserToken();

        public Main()
        {
            InitializeComponent();
            rtxtResponse.ReadOnly = true;
            LuaRuntime.GetContext().RegisterFunc("LogWriteLine", _responseLog, _responseLog.GetType().GetMethod("WriteLine"));
            LuaRuntime.GetContext().RegisterFunc("LogWriteTable", _responseLog, _responseLog.GetType().GetMethod("WriteTable"));
            InitConfig();
            TcpRequest.Instance().ReceiveCompleted += OnReceiveCompleted;

            _watchThread = new Thread(obj =>
            {
                var instance = obj as Main;
                while (true)
                {

                    Thread.Sleep(100);
                }
            });
        }

        private void InitConfig()
        {
            txtHost.Text = ConfigUtils.GetSetting("User.Host");
            txtPort.Text = ConfigUtils.GetSetting("User.Port");
            txtAction.Text = ConfigUtils.GetSetting("User.ActionId");
            txtGameType.Text = ConfigUtils.GetSetting("User.GameType");
            txtServerID.Text = ConfigUtils.GetSetting("User.ServerID");
            txtRetailID.Text = ConfigUtils.GetSetting("User.RetailID");
            txtPid.Text = ConfigUtils.GetSetting("User.Pid");
            txtPwd.Text = ConfigUtils.GetSetting("User.Pwd");
            _token.DeviceID = ConfigUtils.GetSetting("User.DeviceID", "00-00-00-00-01");
            _token.MobileType = ConfigUtils.GetSetting("User.MobileType", "5").ToInt();
        }

        private void SetConfig(string host, int port, int actionId, UserToken token, string pwd)
        {
            ConfigurationUtils.GetInstance().updateSeeting("User.Host", host);
            ConfigurationUtils.GetInstance().updateSeeting("User.Port", port.ToString());
            ConfigurationUtils.GetInstance().updateSeeting("User.ActionId", actionId.ToString());
            ConfigurationUtils.GetInstance().updateSeeting("User.GameType", token.GameType.ToString());
            ConfigurationUtils.GetInstance().updateSeeting("User.ServerID", token.ServerID.ToString());
            ConfigurationUtils.GetInstance().updateSeeting("User.RetailID", token.RetailID);
            ConfigurationUtils.GetInstance().updateSeeting("User.Pid", token.Pid);
            ConfigurationUtils.GetInstance().updateSeeting("User.Pwd", pwd);
            ConfigurationUtils.GetInstance().updateSeeting("User.DeviceID", token.DeviceID);
            ConfigurationUtils.GetInstance().updateSeeting("User.MobileType", token.MobileType.ToString());
            ConfigurationUtils.GetInstance().save();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //_watchThread.Start(this);
        }

        private void btnLuaRunPath_Click(object sender, EventArgs e)
        {
            try
            {
                string host = txtHost.Text;
                int port = txtPort.Text.ToInt();
                int actionId = txtAction.Text.ToInt();
                string pwd = txtPwd.Text.Trim();
                _token.GameType = txtGameType.Text.ToInt();
                _token.ServerID = txtServerID.Text.ToInt();
                _token.RetailID = txtRetailID.Text.Trim();
                _token.Pid = txtPid.Text.Trim();
                _token.Pwd = EncodePwdAndUrlEncode(pwd);
                SetConfig(host, port, actionId, _token, pwd);

                string funName = string.Format("Action{0}", actionId);
                ScutWriter.getInstance().writeHead(_token.Sid, _token.Uid.ToInt());
                ScutWriter.getInstance().writeInt32("ActionId", actionId);
                if (!LuaRuntime.GetContext().TryCall<Action<UserToken>>(funName, _token))
                {
                    _responseLog.WriteFormatLine("请求出错:The {0} function is not exist in lua file.", funName);
                }
                var sendData = ScutWriter.generatePostData();
                TcpRequest.Instance().Send(host, port, sendData, null);
                ScutWriter.resetData();
            }
            catch (Exception ex)
            {
                _responseLog.WriteFormatLine("请求出错:{0}", ex);
            }
            finally
            {
                ResponseReflesh();
            }
        }

        private string EncodePwdAndUrlEncode(string pwd)
        {
            pwd = new DESAlgorithmNew().EncodePwd(pwd, ConfigUtils.GetSetting("User.Password.EncodeKey"));
            return System.Web.HttpUtility.UrlEncode(pwd);
        }

        private void OnReceiveCompleted(SocketClient sender, byte[] data)
        {
            try
            {
                ScutReader.GetIntance().Reader = new MessageStructure(data);
                bool result = ScutReader.GetIntance().getResult();
                int actionId = ScutReader.GetIntance().readAction();
                string funName = string.Format("_{0}Callback", actionId);
                LuaTable table;
                if (!LuaRuntime.GetContext().TryCall<Func<UserToken, LuaTable>, LuaTable>(funName, out table, _token))
                {
                    _responseLog.WriteFormatLine("接收出错:The {0} function is not exist in lua file.", funName);
                }
                else if (result)
                {
                    _responseLog.WriteTable("接收Action:" + actionId + "成功!Error:" + ScutReader.GetIntance().readErrorCode(), table);
                }
            }
            catch (Exception ex)
            {
                _responseLog.WriteFormatLine("接收出错:{0}", ex);
            }
            finally
            {
#if DEBUG
#else
                ResponseReflesh();
#endif
            }
        }

        private void ResponseReflesh()
        {
            rtxtResponse.Lines = _responseLog.GetInfo();
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            _responseLog.Clear();
            ResponseReflesh();
        }

    }
}
