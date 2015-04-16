using System;
using System.Text;
using Newtonsoft.Json;

/// <summary>
/// 
/// </summary>
class ResponseBody
{
    /// <summary>
    /// 
    /// </summary>
    public ResponseBody()
    {
        StateCode = 0;
        StateDescription = "";
        Vesion = "1.0";
        Handler = "";
    }
    /// <summary>
    /// 
    /// </summary>
    public int StateCode { get; set; }
    /// <summary>
    /// /
    /// </summary>
    public string StateDescription { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Vesion { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Handler { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public object Data { get; set; }
}
/// <summary>
/// 
/// </summary>
class DefaultHeadFormater : IHeadFormater
{

    public bool TryParse(string data, NetworkType type, out PackageHead head, out object body)
    {
        body = null;
        head = null;
        try
        {
            ResponseBody result = JsonConvert.DeserializeObject<ResponseBody>(data);
            if (result == null) return false;

            head = new PackageHead();
            head.StatusCode = result.StateCode;
            head.Description = result.StateDescription;
            body = result.Data;
            return true;

        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public bool TryParse(byte[] data, out PackageHead head, out byte[] bodyBytes)
    {
        bodyBytes = null;
        head = null;
        int pos = 0;
        if (data == null || data.Length == 0)
        {
            return false;
        }

        int nStreamSize = GetInt(data, ref pos);

        if (nStreamSize != data.Length)
        {
            return false;
        }
        head = new PackageHead();
        head.StatusCode = GetInt(data, ref pos);
        head.MsgId = GetInt(data, ref pos);
        head.Description = GetString(data, ref pos);
        head.ActionId = GetInt(data, ref pos);
        head.StrTime = GetString(data, ref pos);
        int bodyLen = data.Length - pos;
        if (bodyLen > 0)
        {
            bodyBytes = new byte[bodyLen];
            Buffer.BlockCopy(data, pos, bodyBytes, 0, bodyLen);
        }
        else
        {
            bodyBytes = new byte[0];
        }
        return true;
    }

    public byte[] BuildHearbeatPackage()
    {
        NetWriter writer = NetWriter.Instance;
        writer.writeInt32("actionId", 1);
        byte[] data = writer.PostData();
        NetWriter.resetData();
        return data;
    }

    private string GetString(byte[] data, ref int pos)
    {
        string val = string.Empty;
        int len = GetInt(data, ref pos);
        if (len > 0)
        {
            val = Encoding.UTF8.GetString(data, pos, len);
            pos += len;
        }
        return val;
    }

    private int GetInt(byte[] data, ref int pos)
    {
        int val = BitConverter.ToInt32(data, pos);
        pos += sizeof(int);
        return val;
    }
}
