using UnityEngine;

public class HttpPackage : NetPackage
{
    public WWW WwwObject { get; set; }

    public string error
    {
        get
        {
            if (IsOverTime)
            {
                return "http request over time";
            }
            else
            {
                return WwwObject.error;
            }
        }
    }

    public byte[] GetResponse()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return WwwObject.bytes;
        }
        string strEncoding = null;
        if (WwwObject.responseHeaders.ContainsKey("CONTENT-ENCODING"))
        {
            strEncoding = WwwObject.responseHeaders["CONTENT-ENCODING"];
        }
        if (strEncoding != null && strEncoding == "gzip")
        {
            return NetReader.Decompression(WwwObject.bytes);
        }
        return WwwObject.bytes;
    }

}