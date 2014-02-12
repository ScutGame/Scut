using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public partial class NetWriter
{

    static NetWriter s_isntance;
    public static NetWriter Instance
    {
        get
        {
            if (s_isntance == null)
            {
                s_isntance = new NetWriter();
            }
            return s_isntance;
        }

    }

    public NetWriter()
    {
        resetData();
    }

    public static void resetData()
    {
        s_strPostData = "";

        s_strUserData = string.Format("MsgId={0}&Sid={1}&Uid={2}&St={3}", s_Counter, s_strSessionID, s_userID, s_strSt);

        s_Counter++;
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
        Debug.Log("SetUrl " + szUrl);
        s_strUrl = szUrl;
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

    public static string getMd5String(byte []buf)
    {
        System.Security.Cryptography.MD5 alg = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] data = alg.ComputeHash(buf);

        StringBuilder sBuilder = new StringBuilder();

        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        return sBuilder.ToString();
    }
    public static string getMd5String(string input)
    {
        return getMd5String(Encoding.Default.GetBytes(input));
    }

    public string generatePostData()
    {
        s_strPostData = s_strUrl + "?d=";

        //md5
        string str = s_strUserData + "&sign="
            + getMd5String(s_strUserData + s_md5Key);

        s_strPostData += url_encode(str);

        Debug.Log(s_strPostData);

        return s_strPostData;
    }

    public byte[] PostData()
    {
        s_strPostData = "?d=";
        string str = s_strUserData + "&sign="
            + getMd5String(s_strUserData + s_md5Key);

        s_strPostData += url_encode(str);

        //Debug.Log(s_strPostData);

        byte[] data = Encoding.ASCII.GetBytes(s_strPostData);
        byte[] len = BitConverter.GetBytes(data.Length);

        byte[] send = new byte[data.Length + len.Length];

        Buffer.BlockCopy(len, 0, send, 0, len.Length);
        Buffer.BlockCopy(data, 0, send, len.Length, data.Length);

        return send;
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

    public static int MsgId { 
        get {
            return s_Counter; 
        } 
    }
    public static void SetMd5Key(string value)
    {
        s_md5Key = value;
    }
    private static ulong s_userID = 0;
    private static string s_strSessionID = "";
    private static string s_strSt = "";

    private static string s_strUrl = "";
    private static string s_strPostData = "";
    private static string s_strUserData = "";
    private static int s_Counter = 1;
    private static string s_md5Key = "";
    // Key value
    private byte[] key = new byte[8] { 0x70, 0x06, 0x09, 0x21, 0x3A, 0x8B, 0x4F, 0x1D };
}