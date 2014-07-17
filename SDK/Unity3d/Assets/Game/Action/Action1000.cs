using System;

public class Action1000 : GameAction
{

    public Action1000()
        : base((int)ActionType.RankAdd)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {
        writer.writeString("UserName", "Jon");
        writer.writeInt32("Score", 100);
    }

    protected override void DecodePackage(NetReader reader)
    {
    }

    public override ActionResult GetResponseData()
    {
		return null;
    }
}
