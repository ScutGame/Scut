using System;
using System.Collections.Generic;
using UnityEngine;

public class Action1008 : BaseAction
{
    private ActionResult actionResult;

    public Action1008()
        : base((int)ActionType.World)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {
        //default url param
    }

    protected override void DecodePackage(NetReader reader)
    {
        actionResult = new ActionResult();
        //默认Scut流格式解包
        actionResult["RoleID"] = reader.getInt();
        actionResult["RoleName"] = reader.readString();
        actionResult["HeadImg"] = reader.readString();
        actionResult["Sex"] = reader.getByte();
        actionResult["LvNum"] = reader.getInt();
        actionResult["ExperienceNum"] = reader.getInt();
        actionResult["LifeNum"] = reader.getInt();
        actionResult["LifeMaxNum"] = reader.getInt();
        GameSetting.Instance.RoleName = actionResult.Get<string>("RoleName");
    }

    public override ActionResult GetResponseData()
    {
        return actionResult;
    }
}
