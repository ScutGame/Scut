using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum ResponseContentType
{
    Stream = 0,
    Json
}
public class NetWriter
{
    private static ulong s_userID = 0;
    private static string s_strSessionID = "";
    private static string s_strSt = "";
    private static ResponseContentType _respContentType = ResponseContentType.Stream;
    private static string s_strUrl = "";
    private static string s_strPostData = "";
    private static string s_strUserData = "";
    private static int s_Counter = 1;
    private static string s_md5Key = "";
    private static byte[] _bodyBuffer;
    private static byte[] _headBuffer;
    private static readonly NetWriter s_isntance = new NetWriter();

    public static NetWriter Instance
    {
        get { return s_isntance; }
    }

    public static bool IsGet { get; private set; }
        
    public static ResponseContentType ResponseContentType
    {
        get
        {
            return _respContentType;
        }
    }

    public static int MsgId
    {
        get { return s_Counter; }
    }

    public static ulong UserID
    {
        get { return s_userID; }
    }

    public static string SessionID
    {
        get { return s_strSessionID; }
    }

    public static string St
    {
        get { return s_strSt; }
    }

    public static void SetMd5Key(string value)
    {
        s_md5Key = value;
    }

    public static void resetData()
    {
        _headBuffer = null;
        _bodyBuffer = null;
        s_strPostData = "";
        s_strUserData = string.Format("MsgId={0}&Sid={1}&Uid={2}&St={3}", s_Counter, s_strSessionID, s_userID, s_strSt);
        s_Counter++;
    }

    public static void setSessionID(string pszSessionID)
    {
        if (pszSessionID != null)
        {
            s_strSessionID = pszSessionID;
            resetData();
        }
    }

    public static void setUserID(ulong value)
    {
        s_userID = value;
        resetData();
    }

    public static void setStime(string pszTime)
    {
        if (pszTime != null)
        {
            s_strSt = pszTime;
            resetData();
        }
    }

    public void SetHeadBuffer(byte[] buffer)
    {
        _headBuffer = buffer;
    }

    public void SetBodyData(byte[] buffer)
    {
        _bodyBuffer = buffer ?? new byte[0];
    }

    private byte[] GetDataBuffer()
    {
        if (_headBuffer == null || _headBuffer.Length == 0 || _bodyBuffer == null)
        {
            return new byte[0];
        }
        //加头长度,合并body
        byte[] lenBytes = BitConverter.GetBytes(_headBuffer.Length);
        byte[] buffer = new byte[_headBuffer.Length + lenBytes.Length + _bodyBuffer.Length];
        Buffer.BlockCopy(lenBytes, 0, buffer, 0, lenBytes.Length);
        Buffer.BlockCopy(_headBuffer, 0, buffer, lenBytes.Length, _headBuffer.Length);
        Buffer.BlockCopy(_bodyBuffer, 0, buffer, lenBytes.Length + _headBuffer.Length, _bodyBuffer.Length);
        return buffer;
    }


    private NetWriter()
    {
        resetData();
    }


    public void writeInt32(string szKey, int nValue)
    {
        s_strUserData += string.Format("&{0}={1}", szKey, nValue);
    }

    public void writeFloat(string szKey, float fvalue)
    {
        s_strUserData += string.Format("&{0}={1}", szKey, fvalue);
    }

    public void writeString(string szKey, string szValue)//
    {
        if (szValue == null)
        {
            return;
        }
        s_strUserData += string.Format("&{0}=", szKey);

        s_strUserData += url_encode(szValue);
    }

    public void writeInt64(string szKey, UInt64 nValue)
    {
        s_strUserData += string.Format("&{0}={1}", szKey, nValue);
    }

    public void writeWord(string szKey, UInt16 sValue)
    {
        s_strUserData += string.Format("&{0}={1}", szKey, sValue);
    }
    public void writeBuf(string szKey, byte[] buf, int nSize)
    {
        System.Diagnostics.Debug.Assert(false);
    }

    public static void SetUrl(string szUrl)
    {
        SetUrl(szUrl, ResponseContentType.Stream);
    }

    public static void SetUrl(string szUrl, ResponseContentType respContentType, bool isGet = false)
    {
        s_strUrl = szUrl;
        IsGet = isGet;
        _respContentType = respContentType;
    }

    public static string GetUrl()
    {
        return s_strUrl;
    }

    public static bool IsSocket()
    {
        if (s_strUrl != null && !s_strUrl.Contains("http"))
        {
            return true;
        }
        return false;
    }


    public string url_encode(string str)
    {
        return WWW.EscapeURL(str);
    }

    public static string getMd5String(byte[] buf)
    {
        return MD5Utils.Encrypt(buf);
    }
    public static string getMd5String(string input)
    {
        return getMd5String(Encoding.Default.GetBytes(input));
    }

    public byte[] PostData()
    {
        byte[] data;
        if (_headBuffer != null && _headBuffer.Length > 0)
        {
            //支持自定义结构
            data = GetDataBuffer();
        }
        else if (_respContentType == ResponseContentType.Json)
        {
            s_strPostData = s_strUserData + "&sign=" + getMd5String(s_strUserData + s_md5Key);
            Debug.Log("request param:" + s_strPostData);
            data = Encoding.UTF8.GetBytes(s_strPostData);
        }
        else
        {
            s_strPostData = IsSocket() ? "?d=" : "d=";
            string str = s_strUserData + "&sign=" + getMd5String(s_strUserData + s_md5Key);
            s_strPostData += url_encode(str);
            data = Encoding.ASCII.GetBytes(s_strPostData);
        }
        if (!IsSocket()) return data;

        //加包长度，拆包时使用
        byte[] len = BitConverter.GetBytes(data.Length);
        byte[] sendBytes = new byte[data.Length + len.Length];
        Buffer.BlockCopy(len, 0, sendBytes, 0, len.Length);
        Buffer.BlockCopy(data, 0, sendBytes, len.Length, data.Length);
        return sendBytes;
    }

}