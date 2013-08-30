using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.RPC.IO;

namespace Scut.Client.Net
{
    public class ScutReader
    {
        private static ScutReader instance = new ScutReader();

        public static ScutReader GetIntance()
        {
            return instance;
        }

        private MessageStructure _dataReader;
        private MessageHead _head;

        private ScutReader()
        {
        }

        public MessageStructure Reader
        {
            get { return _dataReader; }
            set { _dataReader = value; }
        }

        #region read method

        public bool getResult()
        {
            if (Reader.Offset == 0)
            {
                _head = instance.Reader.ReadHeadGzip();
            }
            return !_head.Faild;
        }

        public int readAction()
        {
            return _head.Action;
        }

        public int readErrorCode()
        {
            return _head.ErrorCode;
        }

        public string readErrorMsg()
        {
            return _head.ErrorInfo;
        }

        public int getWORD()
        {
            return Reader.ReadShort();
        }

        public int getInt()
        {
            return Reader.ReadInt();
        }

        public int getByte()
        {
            return Reader.ReadByte();
        }

        public long getLong()
        {
            return Reader.ReadLong();
        }

        public double getDouble()
        {
            return Reader.ReadDouble();
        }

        public string readString()
        {
            return Reader.ReadString();
        }

        public void recordBegin()
        {
            Reader.RecordStart();
        }

        public void recordEnd()
        {
            Reader.RecordEnd();
        }

        #endregion

    }
}
