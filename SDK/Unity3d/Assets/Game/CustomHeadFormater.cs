using System;
using GameRanking.Pack;
using ZyGames.Framework.Common.Serialization;

/// <summary>
/// 定制的头部结构解析
/// </summary>
public class CustomHeadFormater : IHeadFormater
{
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

    private int GetInt(byte[] data, ref int pos)
    {
        int val = BitConverter.ToInt32(data, pos);
        pos += sizeof(int);
        return val;
    }
}
