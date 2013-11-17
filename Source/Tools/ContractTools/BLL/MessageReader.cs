using System;
using System.Text;
using System.IO;
using model;
using System.Net;
using System.Web.Security;

namespace BLL
{
    public class Message
    {
        public bool Success
        {
            get
            {
                return ErrorCode == 0;
            }
        }
        public int Length
        {
            get;
            set;
        }

        public int ErrorCode
        {
            get;
            set;
        }

        public int MsgID
        {
            get;
            set;
        }
        public int Action
        {
            get;
            set;
        }

        public string ErrorInfo
        {
            get;
            set;
        }

        public string St
        {
            get;
            set;
        }

    }

    public class MessageReader : IDisposable
    {
        public static byte[] _buffer;
        public static int GameID { get; set; }
        public static int ServerID { get; set; }

        public Message ReadHead()
        {
            var header = new Message();
            header.Length = ReadInt();
            header.ErrorCode = ReadInt();
            header.MsgID = ReadInt();
            header.ErrorInfo = ReadString();
            header.Action = ReadInt();
            header.St = ReadString();
            return header;
        }
        public static MessageReader Create(string serverUrl, string requestParams,ref Message header, bool IsSocket)
        {
            MessageReader msgReader = null;
            if (IsSocket)
            {

                SocketAction socketAction = new SocketAction();
                socketAction.DoSocket(serverUrl, requestParams);
                msgReader = socketAction.result;
                header = socketAction._head;
                //MemoryStream ms = new MemoryStream(socketAction.result.ReadByte());
                //BinaryReader reader = new BinaryReader(ms, Encoding.UTF8);
                //msgReader = new MessageReader(reader);
            }
            else
            {
                Encoding encode = Encoding.GetEncoding("utf-8");
                string postData = "d=" + GetSign(requestParams);
                byte[] bufferData = encode.GetBytes(postData);

                HttpWebRequest serverRequest = (HttpWebRequest)WebRequest.Create(serverUrl);
                serverRequest.Method = "POST";
                serverRequest.ContentType = "application/x-www-form-urlencoded";
                serverRequest.ContentLength = bufferData.Length;
                Stream requestStream = serverRequest.GetRequestStream();
                requestStream.Write(bufferData, 0, bufferData.Length);
                requestStream.Close();

                //返回流

                WebResponse serverResponse = serverRequest.GetResponse();
                Stream responseStream = serverResponse.GetResponseStream();
                if (responseStream != null)
                {
                    BinaryReader reader = new BinaryReader(responseStream, Encoding.UTF8);
                    msgReader = new MessageReader(reader);
                    header.Length = msgReader.ReadInt();
                    header.ErrorCode = msgReader.ReadInt();
                    header.MsgID = msgReader.ReadInt();
                    header.ErrorInfo = msgReader.ReadString();
                    header.Action = msgReader.ReadInt();
                    header.St = msgReader.ReadString();

                }
            }
            return msgReader;
        }
        static void client_ReceiveHandle(int error, string errorInfo, byte[] buffer)
        {
            _buffer = buffer;
        }
        private void DoXuanYuanPy()
        {
            //List<JsonParameter> paramList = new List<JsonParameter>();
            ////paramList.Add(new JsonParameter() { Key = "_py_code", Value = Uri.EscapeDataString(GetForm("codeText")) });
            //paramList.Add(new JsonParameter() { Key = "_py_func_arg", Value = "" });
            //paramList.Add(new JsonParameter() { Key = "GetData", Value = "PythonScript" });
            //string strParamList = JsonConvert.SerializeObject(paramList);
            //DepProject project = SvnProcesser.ProjectList().Find(u => u.GameId == GameID);

            //string host = project.ExcludeFile;//配置成Socket地址
            //using (GameSocketClient client = new GameSocketClient())
            //{
            //    client.ReceiveHandle += new SocketCallback(client_ReceiveHandle);
            //    client.Connect(host);
            //    client.SendToServer(GameID, ServerID, 1000, strParamList, string.Empty, Uri.EscapeDataString(GetForm("codeText")));
            //}
        }


        public static MessageReader Create(byte[] buffer)
        {
            MessageReader msgReader = null;

            return msgReader;
        }

        public static string GetSign(string requestParams)
        {
            string sign = FormsAuthentication.HashPasswordForStoringInConfigFile(requestParams + "44CAC8ED53714BF18D60C5C7B6296000", "MD5").ToLower();
            return Uri.EscapeDataString(string.Format("{0}&sign={1}", requestParams, sign));
        }

        private BinaryReader reader;
        private int recordLength = 0;

        public MessageReader(BinaryReader reader)
        {
            this.reader = reader;
        }
        public MessageReader(byte[] buffer)
        {
            reader = new BinaryReader(new MemoryStream(buffer));
        }
        public string ReadString()
        {
            Int32 length = reader.ReadInt32();
            if (length >= 0)
            {
                return Encoding.UTF8.GetString(reader.ReadBytes(length));
            }
            else
            {
                throw new Exception("读取字符串溢出");
            }
        }

        public int ReadInt()
        {
            return reader.ReadInt32();
        }

        public short ReadShort()
        {
            return reader.ReadInt16();
        }

        public byte ReadByte()
        {
            return reader.ReadByte();
        }

        public char ReadChar()
        {
            return reader.ReadChar();
        }
        /// <summary>
        /// 记录总数
        /// </summary>
        /// <returns></returns>
        public int RecordCount()
        {
            return reader.ReadInt32();
        }

        /// <summary>
        /// 循环开始
        /// </summary>
        public void RecordStart()
        {
            Int32 length = reader.ReadInt32();
            recordLength = length;
        }
        /// <summary>
        /// 循环结束
        /// </summary>
        public void RecordEnd()
        {
            //Int32 length = reader.ReadInt32();
            recordLength = 0;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            reader.Close();
        }

        #endregion

        public bool GetFieldValue(FieldType fieldType, ref string val)
        {
            bool result = false;
            switch (fieldType)
            {
                case FieldType.Int:
                    val = ReadInt().ToString();
                    result = true;
                    break;
                case FieldType.String:
                    val = ReadString().ToString();
                    result = true;
                    break;
                case FieldType.Short:
                    val = ReadShort().ToString();
                    result = true;
                    break;
                case FieldType.Byte:
                    val = ReadByte().ToString();
                    result = true;
                    break;
                case FieldType.Record:
                    break;
                case FieldType.End:
                    break;
                case FieldType.Head:
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
