namespace ZyGames.Framework.RPC.Wcf
{

    /// <summary>
    /// 通道协议绑定类型
    /// </summary>
    public enum BindingType
    {
        BasicHttpBinding = 0,
        NetNamedPipeBinding,
        NetPeerTcpBinding,
        NetTcpBinding,
        WsDualHttpBinding,
        WsFederationHttpBinding,
        WsHttpBinding
    }
}