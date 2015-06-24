using System;

/// <summary>
/// 网络包
/// </summary>
public abstract class NetPackage
{
    public int ActionId
    {
        get;
        set;
    }
    public GameAction Action
    {
        set;
        get;
    }

    public bool IsOverTime
    {
        get;
        set;
    }

    public string ErrorMsg
    {
        set;
        get;
    }

    /// <summary>
    /// 错误码 0代码成功 非0失败
    /// </summary>
    public int ErrorCode
    {
        set;
        get;
    }
    public object UserData { set; get; }

    public NetReader Reader { get; set; }

}
