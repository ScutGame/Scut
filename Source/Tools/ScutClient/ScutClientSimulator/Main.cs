using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Sockets;

namespace ScutClientSimulator
{
    public partial class Main : Form
    {
        private ResponseLog _responseLog = new ResponseLog(100);
        private Thread _watchThread;
        private UserToken _token = new UserToken();

        public Main()
        {
            InitializeComponent();
            rtxtResponse.ReadOnly = true;
            LuaRuntime.GetContext().RegisterFunc("LogWriteLine", _responseLog, _responseLog.GetType().GetMethod("WriteLine"));
            LuaRuntime.GetContext().RegisterFunc("LogWriteTable", _responseLog, _responseLog.GetType().GetMethod("WriteTable"));

            TcpRequest.Instance().ReceiveCompleted += OnReceiveCompleted;
            _token.Pid = "z633140";
            _token.Pwd = "%25A9%25F0%2506m%2508%25D9%25EB%2528O%25BDYR9%25AA%2583%25D0";
            _token.DeviceID = "00-00-00-00-01";
            _token.MobileType = 4;
            _token.RetailID = "0000";

            _watchThread = new Thread(obj =>
            {
                var instance = obj as Main;
                while (true)
                {

                    Thread.Sleep(100);
                }
            });
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
                _token.GameType = txtGameType.Text.ToInt();
                _token.ServerID = txtServerID.Text.ToInt();
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
