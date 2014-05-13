using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameRanking.Pack;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Sockets;

namespace Game.Script
{
public class CustomActionDispatcher : IActionDispatcher
{
    public bool TryDecodePackage(ConnectionEventArgs e, out RequestPackage package)
    {
        //这里解出头部信息根据ActionId来分发请求到相应的Action子类
        package = null;
        byte[] content;
        //todo test 1001 action
        //MessagePack packageHead = new MessagePack { MsgId = 1, ActionId = 1001, SessionId = "", UserId = 1380001 };
        //byte[] headBytes = ProtoBufUtils.Serialize(packageHead);
        //byte[] contentBytes = ProtoBufUtils.Serialize(new Request1001Pack() { PageIndex = 1, PageSize = 20 });
        //e.Data = BufferUtils.MergeBytes(BufferUtils.AppendHeadBytes(headBytes), contentBytes);

        MessagePack head = ReadMessageHead(e.Data, out content);
        if (head == null)
        {
            return false;
        }
        package = new RequestPackage(head.MsgId, head.SessionId, head.ActionId, head.UserId) { Message = content };

        return true;
    }

    /// <summary>
    /// 定义byte[]格式：headLength + headBytes + contentBytes
    /// </summary>
    /// <returns></returns>
    private MessagePack ReadMessageHead(byte[] data, out byte[] content)
    {
        MessagePack headPack = null;
        content = new byte[0];
        try
        {
            //解头部(解之前当然还需要对byte[]解密，这里跳过这步)
            int pos = 0;
            byte[] headLenBytes = new byte[4];
            Buffer.BlockCopy(data, pos, headLenBytes, 0, headLenBytes.Length);
            pos += headLenBytes.Length;
            int headSize = BitConverter.ToInt32(headLenBytes, 0);
            if (headSize < data.Length)
            {
                byte[] headBytes = new byte[headSize];
                Buffer.BlockCopy(data, pos, headBytes, 0, headBytes.Length);
                pos += headBytes.Length;
                headPack = ProtoBufUtils.Deserialize<MessagePack>(headBytes);

                //解消息的内容
                if (data.Length > pos)
                {
                    int len = data.Length - pos;
                    content = new byte[len];
                    Buffer.BlockCopy(data, pos, content, 0, content.Length);
                    //内容数据放到具体Action业务上处理
                }
            }
            else
            {
                //不支持的数据格式
            }
        }
        catch (Exception ex)
        {
            //不支持的数据格式
        }
        return headPack;
    }

    public ActionGetter GetActionGetter(RequestPackage package)
    {
        // 可以实现自定的ActionGetter子类
        return new ActionGetter(package);
    }

    public void ResponseError(BaseGameResponse response, ActionGetter actionGetter, int errorCode, string errorInfo)
    {
        //实现出错处理下发
        ResponsePack head = new ResponsePack()
        {
            MsgId = actionGetter.GetMsgId(),
            ActionId = actionGetter.GetActionId(),
            ErrorCode = errorCode,
            ErrorInfo = errorInfo,
            St = actionGetter.GetSt()
        };
        byte[] headBytes = ProtoBufUtils.Serialize(head);
        byte[] buffer = BufferUtils.AppendHeadBytes(headBytes);
        response.BinaryWrite(buffer);
    }
}
}
