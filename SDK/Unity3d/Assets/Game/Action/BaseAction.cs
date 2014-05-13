using System;
using GameRanking.Pack;
using ZyGames.Framework.Common.Serialization;

/// <summary>
/// 自定结构Action代理基类
/// </summary>
public abstract class BaseAction : GameAction
{
    protected BaseAction(int actionId)
        : base(actionId)
    {
    }

    protected override void SetActionHead(NetWriter writer)
    {
        MessagePack headPack = new MessagePack()
        {
            MsgId = Head.MsgId,
            ActionId = ActionId,
            SessionId = Head.SessionId,
            UserId = Head.UserId
        };
        byte[] data = ProtoBufUtils.Serialize(headPack);
        writer.SetHeadBuffer(data);
        writer.SetBodyData(null);
    }
}
