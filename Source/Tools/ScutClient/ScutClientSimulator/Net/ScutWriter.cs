using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Scut.Client.Net
{
    public class ScutWriter
    {
        private static ScutWriter _instance = new ScutWriter();

        public static ScutWriter getInstance()
        {
            return _instance;
        }

        public static void resetData()
        {
            _instance._dictParam.Clear();
        }

        public static string generatePostData()
        {
            _instance.Init();
            return _instance.FormatToUrlParam();
        }

        private int _msgId;
        private string _sid;
        private int _uid;
        private ConcurrentDictionary<string, object> _dictParam;

        private ScutWriter()
        {
            _dictParam = new ConcurrentDictionary<string, object>();
        }

        public void Init()
        {
            Interlocked.Increment(ref _instance._msgId);
            if (_msgId < 0)
            {
                _msgId = 1;
            }
        }

        public void writeInt32(string name, int value)
        {
            Write(name, value);
        }

        public void writeWord(string name, short value)
        {
            Write(name, value);
        }

        public void writeString(string name, string value)
        {
            Write(name, value);
        }

        public void writeHead(string sid, int uid)
        {
            _sid = sid;
            _uid = uid;
        }

        private string FormatToUrlParam()
        {
            string param = string.Format("MsgId={0}&Sid={1}&Uid={2}&St=st", _msgId, _sid, _uid);
            List<string> keys = new List<string>(_dictParam.Keys);
            keys.Sort();
            foreach (var key in keys)
            {
                param += string.Format("&{0}={1}", key, _dictParam[key]);
            }
            return param;
        }

        private void Write(string name, object value)
        {
            if (_dictParam.ContainsKey(name))
            {
                _dictParam[name] = value;
            }
            else
            {
                _dictParam.TryAdd(name, value);
            }
        }
    }
}
