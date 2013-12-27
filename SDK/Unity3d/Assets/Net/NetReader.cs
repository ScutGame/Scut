using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.GZip;

public enum NetworkType
{
    Http = 0,
    Socket = 1,
}

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

    byte[] bytes = null;
    int streamPos = 0;
    Stack<RECORDINFO> RecordStack = new Stack<RECORDINFO>();
    public void SetByte(byte[] buf)
    {
        bytes = buf;
    }

    public int StatusCode
    {
        get;
        set;
    }

    public string Description
    {
        get;
        set;
    }

    public int ActionId
    {
        get;
        set;
    }

    public int RmId
    {
        get;
        set;
    }

    public string StrTime
    {
        get;
        set;
    }

    public NetReader()
    {

    }

    public bool pushNetStream(byte[] netBytes, NetworkType type)
    {
        if (null == netBytes)
        {
            return false;
        }

        // Decompression
        //netBytes = Decompression(netBytes);
        //Debug.Log("pushNetStream:length" + netBytes.Length);

        this.bytes = netBytes;
        this.streamPos = 0;
        this.RecordStack.Clear();

        int nStreamSize = getInt();

        if (nStreamSize != this.bytes.Length)
	    {
            Debug.Log(" Failed: NetReader: pushNetStream , nStreamSize error " + "nStreamSize" + nStreamSize + "this.bytes.Length" + this.bytes.Length);
	        return false;
	    }


        this.StatusCode = this.getInt();

        this.RmId = this.getInt();

        int nDescriptionLen = this.getInt();
        if (nDescriptionLen > 0)
        {
            this.Description = this.getString(nDescriptionLen);
        }
        else
        {
            this.Description = "";
        }

        this.ActionId = this.getInt();

        int nStrTimeLen = this.getInt();
        if (nStrTimeLen > 0)
        {
            this.StrTime = this.getString(nStrTimeLen);
        }

        return true;
    }

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

    public bool recordBegin()
    {
        int nLen = sizeof(int);
        if (streamPos + nLen > this.bytes.Length)
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
        byte bt = bytes[this.streamPos];
        this.streamPos += 1;
        return bt;
    }

    public byte getByte()
    {
        int nLen = sizeof(byte);
        if (this.streamPos + nLen > this.bytes.Length)
        {
            Debug.Log(" Failed: 长度越界  NetReader: getBYTE ");
            return 0;
        }

        byte bt = bytes[this.streamPos];
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
        if (this.streamPos + nLen > this.bytes.Length)
        {
            Debug.Log(" Failed: 长度越界  NetReader: getSByte ");
            return 0;
        }

        sbyte bt = Convert.ToSByte(bytes[this.streamPos]);
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
        if (streamPos + nLen > this.bytes.Length)
        {
            Debug.Log(" Failed: 长度越界 NetReader: getShort");
            return 0;
        }

        short val = BitConverter.ToInt16(this.bytes, this.streamPos);
        streamPos += nLen;

        if (this.RecordStack.Count > 0)
        {
            RECORDINFO info = RecordStack.Peek();

            if(info.RecordReadSize + nLen > info.RecordSize)
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
        if (streamPos + nLen > this.bytes.Length)
        {
            Debug.Log(" Failed: 长度越界 NetReader: getUShort");
            return 0;
        }

        ushort val = BitConverter.ToUInt16(this.bytes, this.streamPos);
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
        if (streamPos + nLen > this.bytes.Length)
        {
            Debug.Log(" Failed: 长度越界 NetReader: getInt" + ActionId);
            return 0;
        }

        int val = BitConverter.ToInt32(this.bytes, this.streamPos);
        streamPos += nLen;

        if (this.RecordStack.Count > 0)
        {
            RECORDINFO info = RecordStack.Peek();

            if (info.RecordReadSize + nLen > info.RecordSize)
            {
                Debug.Log(" Failed: 记录长度越界 NetReader: getInt" + ActionId);
                return 0;
            }

            info.RecordReadSize += nLen;
        }

        return val;
    }

    public uint getUInt()
    {
        int nLen = sizeof(uint);
        if (streamPos + nLen > this.bytes.Length)
        {
            Debug.Log(" Failed: 长度越界 NetReader: getUInt");
            return 0;
        }

        uint val = BitConverter.ToUInt32(this.bytes, this.streamPos);
        streamPos += nLen;

        if (this.RecordStack.Count > 0)
        {
            RECORDINFO info = RecordStack.Peek();

            if (info.RecordReadSize + nLen > info.RecordSize)
            {
                Debug.Log(" Failed: 记录长度越界 NetReader: getUInt");
                return 0;
            }

            info.RecordReadSize += nLen;
        }

        return val;
    }
    public float getFloat()
    {
        int nLen = sizeof(float);
        if (streamPos + nLen > this.bytes.Length)
        {
            Debug.Log(" Failed: 长度越界 NetReader: getFloat");
            return 0;
        }

        float val = BitConverter.ToSingle(this.bytes, this.streamPos);
        streamPos += nLen;

        if (this.RecordStack.Count > 0)
        {
            RECORDINFO info = RecordStack.Peek();

            if (info.RecordReadSize + nLen > info.RecordSize)
            {
                Debug.Log(" Failed: 记录长度越界 NetReader: getFloat");
                return 0;
            }

            info.RecordReadSize += nLen;
        }

        return val;
    }

    public double getDouble()
    {
        int nLen = sizeof(double);
        if (streamPos + nLen > this.bytes.Length)
        {
            Debug.Log(" Failed: 长度越界 NetReader: getDouble");
            return 0;
        }

        double val = BitConverter.ToDouble(this.bytes, this.streamPos);
        streamPos += nLen;

        if (this.RecordStack.Count > 0)
        {
            RECORDINFO info = RecordStack.Peek();

            if (info.RecordReadSize + nLen > info.RecordSize)
            {
                Debug.Log(" Failed: 记录长度越界 NetReader: getDouble");
                return 0;
            }

            info.RecordReadSize += nLen;
        }

        return val;
    }

    public ulong getULong()
    {
        int nLen = sizeof(ulong);
        if (streamPos + nLen > this.bytes.Length)
        {
            Debug.Log(" Failed: 长度越界 NetReader: getULong");
            return 0;
        }

        ulong val = BitConverter.ToUInt64(this.bytes, this.streamPos);
        streamPos += nLen;

        if (this.RecordStack.Count > 0)
        {
            RECORDINFO info = RecordStack.Peek();

            if (info.RecordReadSize + nLen > info.RecordSize)
            {
                Debug.Log(" Failed: 记录长度越界 NetReader: getULong");
                return 0;
            }

            info.RecordReadSize += nLen;
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
        if (streamPos + nLen > this.bytes.Length)
        {
            Debug.Log(" Failed: 长度越界 NetReader: readInt64");
            return 0;
        }

        Int64 val = BitConverter.ToInt64(this.bytes, this.streamPos);
        streamPos += nLen;

        if (this.RecordStack.Count > 0)
        {
            RECORDINFO info = RecordStack.Peek();

            if (info.RecordReadSize + nLen > info.RecordSize)
            {
                Debug.Log(" Failed: 记录长度越界 NetReader: readInt64");
                return 0;
            }

            info.RecordReadSize += nLen;
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
        if (streamPos + nLen > this.bytes.Length)
        {
            Debug.Log(" Failed: 长度越界 NetReader: getDateTime");
            return DateTime.MinValue;
        }

        long val = BitConverter.ToInt64(this.bytes, this.streamPos);
        streamPos += nLen;

        if (this.RecordStack.Count > 0)
        {
            RECORDINFO info = RecordStack.Peek();

            if (info.RecordReadSize + nLen > info.RecordSize)
            {
                Debug.Log(" Failed: 记录长度越界 NetReader: getDateTime");
                return DateTime.MinValue;
            }

            info.RecordReadSize += nLen;
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
        if (this.streamPos + nLen > this.bytes.Length)
        {
            Debug.Log(" Failed: 长度越界 NetReader: getString");
            return null;
        }
       
        string str = Encoding.UTF8.GetString(this.bytes, this.streamPos, nLen);
        this.streamPos += nLen;

        if (this.RecordStack.Count > 0)
        {
            RECORDINFO info = RecordStack.Peek();

            if (info.RecordReadSize + nLen > info.RecordSize)
            {
                Debug.Log(" Failed: 记录长度越界 NetReader: getString");
                return null;
            }

            info.RecordReadSize += nLen;
        }

        return str;
    }

    public string readString()
    {
        int nLen = this.getInt();
        return this.getString(nLen);
    }
}
