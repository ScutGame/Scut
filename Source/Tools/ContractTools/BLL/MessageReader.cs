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
        public static MessageReader Create(string serverUrl, string requestParams, Message header)
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
            MessageReader msgReader = null;
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
