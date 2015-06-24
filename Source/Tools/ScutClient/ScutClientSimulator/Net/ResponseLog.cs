using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using LuaInterface;

namespace Scut.Client.Net
{
    public class ResponseLog
    {
        private string[] _segmentArray;
        private readonly int _length;
        private int _offset;
        private int _bufferCount;

        public ResponseLog(int length)
        {
            _length = length;
            _bufferCount = (length + 3) / 4;
            _segmentArray = new string[_length + _bufferCount];
            _dateFormat = "HH:mm:ss:ms";
        }

        private string _dateFormat;

        public string DateFormat
        {
            set { _dateFormat = value; }
        }

        public int Offset
        {
            get { return _offset; }
        }

        private int OffsetStart
        {
            get { return _offset > _length ? _offset - _length : 0; }
        }

        public int Length
        {
            get { return _offset > _length ? _length : _offset; }
        }

        public void Clear()
        {
            _offset = 0;
            Array.Clear(_segmentArray, 0, _segmentArray.Length);
        }

        public void Test(string str)
        {
            WriteLine(str);
        }

        public void WriteLine(string str)
        {
            Write(str + Environment.NewLine);
        }

        public void WriteFormatLine(string format, params object[] args)
        {
            WriteFormat(format + Environment.NewLine, args);
        }

        public void Write(string str)
        {
            CheckSegment();
            _segmentArray[_offset++] = str;
        }

        public void WriteFormat(string format, params object[] args)
        {
            CheckSegment();
            _segmentArray[_offset++] = string.Format("{0}>>{1}",
                DateTime.Now.ToString(_dateFormat),
                string.Format(format, args));
        }

        public void WriteTable(string message, LuaTable table)
        {
            WriteFormatLine("{0}{1}", message, FormatLuaTable(table));
        }

        public string[] GetInfo()
        {
            string[] data = new string[Length];
            Array.Copy(_segmentArray, OffsetStart, data, 0, data.Length);
            return data;
        }

        private void CheckSegment()
        {
            int offstart = OffsetStart;
            if (offstart + 1 > _bufferCount)
            {
                //回收缓存
                string[] copySegment = new string[_segmentArray.Length];
                Array.Copy(_segmentArray, offstart, copySegment, 0, Length);
                Interlocked.CompareExchange(ref _offset, _offset - offstart - 1, _offset);
                Interlocked.CompareExchange(ref _segmentArray, copySegment, _segmentArray);
            }
        }


        private static string FormatLuaTable(LuaTable table, int depth = 1)
        {
            string formatStr = "";
            if (table == null)
            {
                return formatStr;
            }
            string preChar = "".PadLeft(depth * 4, ' ');
            int index = 0;
            int count = table.Keys.Count;
            if (count > 0) formatStr += "\r\n" + preChar;
            formatStr += "{";
            foreach (var key in table.Keys)
            {
                object value = table[key];
                if (index > 0)
                {
                    formatStr += ",";
                }
                if (value is LuaTable)
                {
                    formatStr += FormatLuaTable((LuaTable)value, depth + 1);

                }
                else
                {
                    formatStr += string.Format("{0}={1}", key, value);
                }
                index++;
            }
            if (count > 1) formatStr += "\r\n" + preChar;
            formatStr += "}";
            return formatStr;
        }

    }
}
