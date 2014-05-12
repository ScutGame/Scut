using UnityEngine;

public class HttpPackage
{
    public string error
    {
        get
        {
            if (overTime)
            {
                return "http request over time";
            }
            else
            {
                return w.error;
            }
        }
    }

    public byte[] Buffer
    {
        get
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return w.bytes;
            }
            string strEncoding = null;
            if (w.responseHeaders.ContainsKey("CONTENT-ENCODING"))
            {
                strEncoding = w.responseHeaders["CONTENT-ENCODING"];
            }
            if (strEncoding != null && strEncoding == "gzip")
            {
                return NetReader.Decompression(w.bytes);
            }
            return w.bytes;
        }
    }

    public int Tag
    {
        get;
        set;
    }
    public INetCallback FuncCallback
    {
        set;
        get;
    }
    public WWW w { get; set; }
    public bool overTime { get; set; }
}