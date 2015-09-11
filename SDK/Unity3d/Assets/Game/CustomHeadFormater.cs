using System;
using GameRanking.Pack;
using Newtonsoft.Json;
using ZyGames.Framework.Common.Serialization;

/// <summary>
/// 定制的头部结构解析
/// </summary>
public class CustomHeadFormater : IHeadFormater
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
        int headSize = GetInt(data, ref pos);
        byte[] headBytes = new byte[headSize];
        Buffer.BlockCopy(data, pos, headBytes, 0, headBytes.Length);
        pos += headSize;
        ResponsePack resPack = ProtoBufUtils.Deserialize<ResponsePack>(headBytes);

        head = new PackageHead();
        head.StatusCode = resPack.ErrorCode;
        head.MsgId = resPack.MsgId;
        head.Description = resPack.ErrorInfo;
        head.ActionId = resPack.ActionId;
        head.StrTime = resPack.St;

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

        //UnityEngine.Debug.Log(string.Format("ActionId:{0}, ErrorCode:{1}, len:{2}", resPack.ActionId, resPack.ErrorCode, bodyBytes.Length));

        return true;
    }

    public byte[] BuildHearbeatPackage()
    {
        var writer = NetWriter.Instance;
        MessagePack headPack = new MessagePack()
        {
            MsgId = NetWriter.MsgId,
            ActionId = 1,
            SessionId = NetWriter.SessionID,
            UserId = (int)NetWriter.UserID
        };
        byte[] headBytes = ProtoBufUtils.Serialize(headPack);
        writer.SetHeadBuffer(headBytes);
        writer.SetBodyData(new byte[0]);
        var data = writer.PostData();
        NetWriter.resetData();
        return data;
    }

    private int GetInt(byte[] data, ref int pos)
    {
        int val = BitConverter.ToInt32(data, pos);
        pos += sizeof(int);
        return val;
    }
}
