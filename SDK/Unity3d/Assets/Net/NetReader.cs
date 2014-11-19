using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.GZip;

public class NetReader
{
    class RECORDINFO
    {
        public int RecordSize { get; set; }
        public int RecordReadSize { get; set; }
        public RECORDINFO(int _RecordSize, int _RecordReadSize)
        {
            RecordSize = _RecordSize;
            RecordReadSize = _RecordReadSize;
        }
    }

    private PackageHead _head;
    private IHeadFormater _formater;
    private byte[] _bytes = null;
    private int streamPos = 0;
    Stack<RECORDINFO> RecordStack = new Stack<RECORDINFO>();


    //public NetReader()
    //    : this(new DefaultHeadFormater())
    //{
    //}

    public NetReader(IHeadFormater formater)
    {
        _formater = formater;
    }

    public bool Success
    {
        get { return StatusCode == 0; }
    }

    public int StatusCode
    {
        get { return _head == null ? 10000 : _head.StatusCode; }
    }

    public string Description
    {
        get { return _head == null ? "" : _head.Description; }
    }

    public int ActionId
    {
        get { return _head == null ? 0 : _head.ActionId; }
    }

    public int RmId
    {
        get { return _head == null ? 0 : _head.MsgId; }
    }

    public string StrTime
    {
        get { return _head == null ? "" : _head.StrTime; }
    }

    public void SetBuffer(byte[] buf)
    {
        _bytes = buf;
        streamPos = 0;
        RecordStack.Clear();
    }

    /// <summary>
    /// 设置字节流，并解开包的头部信息
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool pushNetStream(byte[] buffer, NetworkType type)
    {
        byte[] data;
        if (!_formater.TryParse(buffer, out _head, out data))
        {
            Debug.LogError(" Failed: NetReader's pushNetStream parse head error: buffer Length " + buffer.Length);
            return false;
        }
        SetBuffer(data);
        return true;
    }

    /// <summary>
    /// Gzip解压
    /// </summary>
    /// <param name="buf"></param>
    /// <returns></returns>
    public static byte[] Decompression(byte[] buf)
    {
        MemoryStream ms = new MemoryStream();
        int count = 0;

        GZipInputStream zip = new GZipInputStream(new MemoryStream(buf));
        byte[] data = new byte[256];

        while ((count = zip.Read(data, 0, data.Length)) != 0)
        {
            ms.Write(data, 0, count);
        }

        byte[] result = ms.ToArray();
        ms.Close();

        return result;
    }

    public byte[] Buffer
    {
        get { return _bytes ?? new byte[0]; }
    }

    public bool recordBegin()
    {
        int nLen = sizeof(int);
        if (streamPos + nLen > this._bytes.Length)
        {
            Debug.Log(" Failed: 长度越界 NetReader: recordBegin ");
            return false;
        }

        int nRecoredSize = getInt();

        RECORDINFO info = new RECORDINFO(nRecoredSize, nLen);
        RecordStack.Push(info);

        return nRecoredSize > 4;
    }

    public void recordEnd()
    {
        if (RecordStack.Count > 0)
        {
            RECORDINFO info = RecordStack.Pop();
            this.streamPos += (info.RecordSize - info.RecordReadSize);
            if (RecordStack.Count > 0)
            {
                RECORDINFO parent = RecordStack.Peek();
                parent.RecordReadSize += info.RecordSize - 4;
            }
        }
    }

    public byte getRecordNumber()
    {
        byte bt = _bytes[this.streamPos];
        this.streamPos += 1;
        return bt;
    }

    public byte getByte()
    {
        int nLen = sizeof(byte);
        if (this.streamPos + nLen > this._bytes.Length)
        {
            Debug.Log(" Failed: 长度越界  NetReader: getBYTE ");
            return 0;
        }

        byte bt = _bytes[this.streamPos];
        this.streamPos += nLen;

        if (this.RecordStack.Count > 0)
        {
            RECORDINFO info = RecordStack.Peek();

            if (info.RecordReadSize + nLen > info.RecordSize)
            {
                Debug.Log(" Failed: 记录长度越界 NetReader: getBYTE");
                return 0;
            }

            info.RecordReadSize += nLen;
        }

        return bt;
    }

    public sbyte getSByte()
    {
        int nLen = sizeof(sbyte);
        if (this.streamPos + nLen > this._bytes.Length)
        {
            Debug.Log(" Failed: 长度越界  NetReader: getSByte ");
            return 0;
        }

        sbyte bt = Convert.ToSByte(_bytes[this.streamPos]);
        this.streamPos += nLen;

        if (this.RecordStack.Count > 0)
        {
            RECORDINFO info = RecordStack.Peek();

            if (info.RecordReadSize + nLen > info.RecordSize)
            {
                Debug.Log(" Failed: 记录长度越界 NetReader: getSByte");
                return 0;
            }

            info.RecordReadSize += nLen;
        }

        return bt;
    }

    public short getShort()
    {
        int nLen = sizeof(short);
        if (streamPos + nLen > this._bytes.Length)
        {
            Debug.Log(" Failed: 长度越界 NetReader: getShort");
            return 0;
        }

        short val = BitConverter.ToInt16(this._bytes, this.streamPos);
        streamPos += nLen;

        if (this.RecordStack.Count > 0)
        {
            RECORDINFO info = RecordStack.Peek();

            if (info.RecordReadSize + nLen > info.RecordSize)
            {
                Debug.Log(" Failed: 记录长度越界 NetReader: getShort");
                return 0;
            }

            info.RecordReadSize += nLen;
        }

        return val;
    }

    public ushort getUShort()
    {
        int nLen = sizeof(ushort);
        if (streamPos + nLen > this._bytes.Length)
        {
            Debug.Log(" Failed: 长度越界 NetReader: getUShort");
            return 0;
        }

        ushort val = BitConverter.ToUInt16(this._bytes, this.streamPos);
        streamPos += nLen;

        if (this.RecordStack.Count > 0)
        {
            RECORDINFO info = RecordStack.Peek();

            if (info.RecordReadSize + nLen > info.RecordSize)
            {
                Debug.Log(" Failed: 记录长度越界 NetReader: getUShort");
                return 0;
            }

            info.RecordReadSize += nLen;
        }

        return val;
    }

    public int getInt()
    {
        int nLen = sizeof(int);
        if (streamPos + nLen > this._bytes.Length)
        {
            Debug.Log(" Failed: 长度越界 NetReader: getInt" + ActionId);
            return 0;
        }

        int val = BitConverter.ToInt32(this._bytes, this.streamPos);
        streamPos += nLen;

        if (CheckRecordSize(nLen))
        {
            Debug.Log(" Failed: 记录长度越界 NetReader: getInt" + ActionId);
            return 0;
        }
        return val;
    }

    private bool CheckRecordSize(int nLen)
    {
        if (this.RecordStack.Count > 0)
        {
            RECORDINFO info = RecordStack.Peek();
            if (info.RecordReadSize + nLen > info.RecordSize)
            {
                return true;
            }
            info.RecordReadSize += nLen;
        }
        return false;
    }

    public uint getUInt()
    {
        int nLen = sizeof(uint);
        if (streamPos + nLen > this._bytes.Length)
        {
            Debug.Log(" Failed: 长度越界 NetReader: getUInt");
            return 0;
        }

        uint val = BitConverter.ToUInt32(this._bytes, this.streamPos);
        streamPos += nLen;

        if (CheckRecordSize(nLen))
        {
            Debug.Log(" Failed: 记录长度越界 NetReader: getUInt");
            return 0;
        }

        return val;
    }
    public float getFloat()
    {
        int nLen = sizeof(float);
        if (streamPos + nLen > this._bytes.Length)
        {
            Debug.Log(" Failed: 长度越界 NetReader: getFloat");
            return 0;
        }

        float val = BitConverter.ToSingle(this._bytes, this.streamPos);
        streamPos += nLen;

        if (CheckRecordSize(nLen))
        {
            Debug.Log(" Failed: 记录长度越界 NetReader: getFloat");
            return 0;
        }

        return val;
    }

    public double getDouble()
    {
        int nLen = sizeof(double);
        if (streamPos + nLen > this._bytes.Length)
        {
            Debug.Log(" Failed: 长度越界 NetReader: getDouble");
            return 0;
        }

        double val = BitConverter.ToDouble(this._bytes, this.streamPos);
        streamPos += nLen;

        if (CheckRecordSize(nLen))
        {
            Debug.Log(" Failed: 记录长度越界 NetReader: getDouble");
            return 0;
        }
        return val;
    }

    public ulong getULong()
    {
        int nLen = sizeof(ulong);
        if (streamPos + nLen > this._bytes.Length)
        {
            Debug.Log(" Failed: 长度越界 NetReader: getULong");
            return 0;
        }

        ulong val = BitConverter.ToUInt64(this._bytes, this.streamPos);
        streamPos += nLen;

        if (CheckRecordSize(nLen))
        {
            Debug.Log(" Failed: 记录长度越界 NetReader: getULong");
            return 0;
        }
        return val;
    }
    public Int64 getLong()
    {
        return readInt64();
    }
    public Int64 readInt64()
    {
        int nLen = sizeof(Int64);
        if (streamPos + nLen > this._bytes.Length)
        {
            Debug.Log(" Failed: 长度越界 NetReader: readInt64");
            return 0;
        }

        Int64 val = BitConverter.ToInt64(this._bytes, this.streamPos);
        streamPos += nLen;

        if (CheckRecordSize(nLen))
        {
            Debug.Log(" Failed: 记录长度越界 NetReader: readInt64");
            return 0;
        }
        return val;
    }

    public int ReverseInt()
    {
        int data = getInt();
        byte[] array = System.BitConverter.GetBytes(data);
        Array.Reverse(array);
        return System.BitConverter.ToInt32(array, 0);
    }
    public string ReadReverseString()
    {
        int nLen = this.ReverseInt();
        return this.getString(nLen);
    }
    public DateTime getDateTime()
    {
        int nLen = sizeof(long);
        if (streamPos + nLen > this._bytes.Length)
        {
            Debug.Log(" Failed: 长度越界 NetReader: getDateTime");
            return DateTime.MinValue;
        }

        long val = BitConverter.ToInt64(this._bytes, this.streamPos);
        streamPos += nLen;

        if (CheckRecordSize(nLen))
        {
            Debug.Log(" Failed: 记录长度越界 NetReader: getDateTime");
            return DateTime.MinValue;
        }
        return FromUnixTime(Convert.ToDouble(val));
    }

    private const long UnixEpoch = 621355968000000000L;
    private static readonly DateTime UnixEpochDateTime = new DateTime(UnixEpoch);
    private static DateTime FromUnixTime(double unixTime)
    {
        return (UnixEpochDateTime + TimeSpan.FromSeconds(unixTime)).ToLocalTime();
    }

    public string getString(int nLen)
    {
        if (this.streamPos + nLen > this._bytes.Length)
        {
            Debug.Log(" Failed: 长度越界 NetReader: getString");
            return null;
        }

        string str = Encoding.UTF8.GetString(this._bytes, this.streamPos, nLen);
        this.streamPos += nLen;

        if (CheckRecordSize(nLen))
        {
            Debug.Log(" Failed: 记录长度越界 NetReader: getString");
            return null;
        }
        return str;
    }

    public string readString()
    {
        int nLen = this.getInt();
        return this.getString(nLen);
    }

    public byte[] readBytes()
    {
        int nLen = this.getInt();
        return this.getString(nLen);
    }

    public byte[] readBytes(int nLen)
    {
        if (this.streamPos + nLen > this._bytes.Length)
        {
            Debug.Log(" Failed: 长度越界 NetReader: getString");
            return null;
        }
        bytep[] buffer = new[nLen];
        Array.Copy(this._bytes, this.streamPos, buffer, 0, buffer.Length);
        this.streamPos += nLen;

        if (CheckRecordSize(nLen))
        {
            Debug.Log(" Failed: 记录长度越界 NetReader: readBytes");
            return new Byte[0];
        }
        return buffer;
    }
}
