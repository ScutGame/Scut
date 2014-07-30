using System;
using System.Collections.Generic;
using UnityEngine;

public class Action1005 : BaseAction
{
    private ActionResult actionResult;

    public Action1005()
        : base((int)ActionType.CreateRote)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {
        //default url param
        writer.writeString("UserName", actionParam.Get<string>("roleName"));
        writer.writeString("Sex",  actionParam.Get<string>("Sex"));
        writer.writeString("HeadID",  actionParam.Get<string>("HeadID"));
        writer.writeString("RetailID", GameSetting.Instance.RetailID);
        writer.writeString("Pid", GameSetting.Instance.Pid);
        writer.writeInt32("MobileType", GameSetting.Instance.MobileType);
        writer.writeInt32("ScreenX", GameSetting.Instance.ScreenX);
        writer.writeInt32("ScreenY", GameSetting.Instance.ScreenY);
        writer.writeString("ClientAppVersion", GameSetting.Instance.ClientAppVersion);
        writer.writeInt32("GameType", GameSetting.Instance.GameID);
        writer.writeInt32("ServerID", GameSetting.Instance.ServerID);
    }

    protected override void DecodePackage(NetReader reader)
    {
        actionResult = new ActionResult();
    }

    public override ActionResult GetResponseData()
    {
        return actionResult;
    }
}
