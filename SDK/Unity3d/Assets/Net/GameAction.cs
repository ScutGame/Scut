using System;
using UnityEngine;

/// <summary>
/// 游戏Action接口
/// </summary>
public abstract class GameAction
{
    private readonly int _actionId;

    protected GameAction(int actionId)
    {
        _actionId = actionId;
    }

    public int ActionId
    {
        get { return _actionId; }
    }

    public void Send()
    {
        
    }

    public void Test()
    {
        //todo test method
        UnityEngine.Debug.Log(_actionId + " test...");
    }
}
