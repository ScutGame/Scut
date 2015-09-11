using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 游戏Action处理工厂
/// </summary>
public abstract class ActionFactory
{
    private static Hashtable lookupType = new Hashtable();
    private static string ActionFormat = "Action{0}";

    public static GameAction Create(object actionType)
    {
        return Create((int)actionType);
    }

    public static GameAction Create(int actionId)
    {
        GameAction gameAction = null;
        try
        {
            string name = string.Format(ActionFormat, actionId);
            var type = (Type)lookupType[name];
            lock (lookupType)
            {
                if (type == null)
                {
                    type = Type.GetType(name);
                    lookupType[name] = type;
                }
            }
            if (type != null)
            {
                gameAction = Activator.CreateInstance(type) as GameAction;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("GameAction create error:" + ex);
        }
        return gameAction;
    }
}
