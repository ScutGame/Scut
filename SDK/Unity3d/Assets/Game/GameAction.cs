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
        Head = new PackageHead() { ActionId = actionId };
    }

    public int ActionId
    {
        get { return Head.ActionId; }
    }

    public PackageHead Head { get; private set; }

    public byte[] Send(object userData)
    {
        NetWriter writer = NetWriter.Instance;
        SetActionHead(writer);
        SendParameter(writer, userData);
        return writer.PostData();
    }

    /// <summary>
    /// 尝试解Body包
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public bool TryDecodePackage(NetReader reader)
    {
        try
        {
            DecodePackage(reader);
            return true;
        }
        catch (Exception ex)
        {
            Debug.Log(string.Format("Action {0} decode package error:{1}", ActionId, ex));
            return false;
        }
    }

    public void Callback(object userData)
    {
        try
        {
            Process(userData);
        }
        catch (Exception ex)
        {
            Debug.Log(string.Format("Action {0} callback process error:{1}", ActionId, ex));
        }
    }


    protected virtual void SetActionHead(NetWriter writer)
    {
        writer.writeInt32("actionId", ActionId);
    }

    protected abstract void SendParameter(NetWriter writer, object userData);

    protected abstract void DecodePackage(NetReader reader);

    protected abstract void Process(object userData);

}
