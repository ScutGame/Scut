using System;

public class Action1000 : GameAction
{

    public Action1000()
        : base((int)ActionType.RankAdd)
    {
    }

    protected override void SendParameter(NetWriter writer, object userData)
    {
        writer.writeString("UserName", "Jon");
        writer.writeInt32("Score", 100);
    }

    protected override void DecodePackage(NetReader reader)
    {
    }

    protected override void Process(object userData)
    {
    }
}
