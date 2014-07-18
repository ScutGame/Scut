using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;


enum ErrorCode
{
    Success = 0,
    ConnectError = -1,
    TimeOutError = -2,
}

/// <summary>
/// 
/// </summary>
/// <param name="package"></param>
public delegate void NetPushCallback(SocketPackage package);
/// <summary>
/// 
/// </summary>
public class SocketConnect
{
    /// <summary>
    /// push Action的请求
    /// </summary>
    private static readonly List<SocketPackage> ActionPools = new List<SocketPackage>();
    private Socket _socket;
    private readonly string _host;
    private readonly int _port;
    private readonly IHeadFormater _formater;
    private bool _isDisposed;
    private readonly List<SocketPackage> _sendList;
    private readonly Queue<SocketPackage> _receiveQueue;
    private const int TimeOut = 30;//30秒的超时时间
    private Thread _thread = null;
    private const int HearInterval = 10000;
    private Timer _heartbeatThread = null;

    /// <summary>
    /// 注册网络Push回调方法
    /// </summary>
    public event NetPushCallback PushCallback;

    protected virtual void OnPushCallback(SocketPackage package)
    {
        try
        {
            NetPushCallback handler = PushCallback;
            if (handler != null) handler.BeginInvoke(package, null, null);
        }
        catch (Exception)
        {
        }
    }

    public SocketConnect(string host, int port, IHeadFormater formater)
    {
        this._host = host;
        this._port = port;
        _formater = formater;
        _sendList = new List<SocketPackage>();
        _receiveQueue = new Queue<SocketPackage>();
    }

    static public void PushActionPool(int actionId, GameAction action)
    {
        RemoveActionPool(actionId);
        SocketPackage package = new SocketPackage();
        package.ActionId = actionId;
        package.Action = action;
        ActionPools.Add(package);
    }

    static public void RemoveActionPool(int actionId)
    {
        foreach (SocketPackage pack in ActionPools)
        {
            if (pack.ActionId == actionId)
            {
                ActionPools.Remove(pack);
                break;
            }
        }
    }
    /// <summary>
    /// 取出回返消息包
    /// </summary>
    /// <returns></returns>
    public SocketPackage Dequeue()
    {
        lock (_receiveQueue)
        {
            if (_receiveQueue.Count == 0)
            {
                return null;
            }
            else
            {
                return _receiveQueue.Dequeue();
            }
        }
    }
    private void CheckReceive()
    {
        while (true)
        {
            if (_socket == null) return;
            try
            {
                if (_socket.Poll(5, SelectMode.SelectRead))
                {
                    if (_socket.Available == 0)
                    {
                        Debug.Log("Close Socket");
                        Close();
                        break;
                    }
                    byte[] prefix = new byte[4];
                    int recnum = _socket.Receive(prefix);

                    if (recnum == 4)
                    {
                        int datalen = BitConverter.ToInt32(prefix, 0);
                        byte[] data = new byte[datalen];
                        int startIndex = 0;
                        recnum = 0;
                        do
                        {
                            int rev = _socket.Receive(data, startIndex, datalen - recnum, SocketFlags.None);
                            recnum += rev;
                            startIndex += rev;
                        } while (recnum != datalen);
                        //判断流是否有Gzip压缩
                        if (data[0] == 0x1f && data[1] == 0x8b && data[2] == 0x08 && data[3] == 0x00)
                        {
                            data = NetReader.Decompression(data);
                        }

                        NetReader reader = new NetReader(_formater);
                        reader.pushNetStream(data, NetworkType.Socket);
                        SocketPackage findPackage = null;

                        //todo socket trace
                        Debug.Log("Socket receive ok, revLen:" + recnum
                            + ", actionId:" + reader.ActionId
                            + ", msgId:" + reader.RmId
                            + ", error:" + reader.StatusCode + reader.Description
                            + ", packLen:" + reader.Buffer.Length);
                        lock (_sendList)
                        {
                            //find pack in send queue.
                            foreach (SocketPackage package in _sendList)
                            {
                                if (package.MsgId == reader.RmId)
                                {
                                    package.Reader = reader;
                                    package.ErrorCode = reader.StatusCode;
                                    package.ErrorMsg = reader.Description;
                                    findPackage = package;
                                    break;
                                }

                            }
                        }
                        if (findPackage == null)
                        {
                            lock (_receiveQueue)
                            {
                                //find pack in receive queue.
                                foreach (SocketPackage package in ActionPools)
                                {
                                    if (package.ActionId == reader.ActionId)
                                    {
                                        package.Reader = reader;
                                        package.ErrorCode = reader.StatusCode;
                                        package.ErrorMsg = reader.Description;
                                        findPackage = package;
                                        break;
                                    }
                                }
                            }
                        }
                        if (findPackage != null)
                        {
                            lock (_receiveQueue)
                            {
                                _receiveQueue.Enqueue(findPackage);
                            }
                            lock (_sendList)
                            {
                                _sendList.Remove(findPackage);
                            }
                        }
                        else
                        {
                            //server push pack.
                            SocketPackage package = new SocketPackage();
                            package.MsgId = reader.RmId;
                            package.ActionId = reader.ActionId;
                            package.ErrorCode = reader.StatusCode;
                            package.ErrorMsg = reader.Description;
                            package.Reader = reader;
                            OnPushCallback(package);
                        }

                    }

                }
                else if (_socket.Poll(5, SelectMode.SelectError))
                {
                    Close();
                    UnityEngine.Debug.Log("SelectError Close Socket");
                    break;

                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("catch" + ex.ToString());

            }

            Thread.Sleep(5);

        }

    }

    //public void CheckNetState()
    //{
    //    if (socket == null)
    //    {
    //        return;
    //    }
    //    //DateTime start = DateTime.Now;
    //    UnityEngine.NetworkReachability state = UnityEngine.Application.internetReachability;
    //    if (state == UnityEngine.NetworkReachability.NotReachable)
    //    {
    //        IsNetStateChange = true;
    //        UnityEngine.Debug.Log("IsNetStateChange = true" + state.ToString());
    //    }
    //    else if (NetState != state)//处理3G 2G的情况
    //    {
    //        UnityEngine.Debug.Log("IsNetStateChange = true" + state.ToString());
    //        IsNetStateChange = true;
    //    }
    //    //UnityEngine.Debug.Log("CheckTime" + DateTime.Now.Subtract(start).TotalMilliseconds );
    //}

    /// <summary>
    /// 打开连接
    /// </summary>
    public void Open()
    {
        UnityEngine.NetworkReachability state = UnityEngine.Application.internetReachability;
        if (state != UnityEngine.NetworkReachability.NotReachable)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                _socket.Connect(_host, _port);
            }
            catch
            {
                //socket.Dispose();
                _socket = null;
                throw;
            }
            if (_heartbeatThread == null)
            {
                _heartbeatThread = new Timer(SendHeartbeatPackage, null, HearInterval, HearInterval);
            }
            _thread = new Thread(new ThreadStart(CheckReceive));
            _thread.Start();
        }

    }

    private void SendHeartbeatPackage(object state)
    {
        try
        {
            NetWriter writer = NetWriter.Instance;
            writer.writeInt32("actionId", 1);
            byte[] data = writer.PostData();
            NetWriter.resetData();
            if (!PostSend(data))
            {
                Debug.Log("send heartbeat paketage fail");
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private void EnsureConnected()
    {
        if (_socket == null)
        {
            Open();
        }

    }

    /// <summary>
    /// 关闭连接
    /// </summary>
    public void Close()
    {
        if (_socket == null) return;
        try
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            _socket = null;
        }
        catch (Exception)
        {
            _socket = null;
        }
    }

    /// <summary>
    /// 发送数据
    /// </summary>
    /// <param name="data"></param>
    private bool PostSend(byte[] data)
    {
        EnsureConnected();
        if (_socket != null)
        {
            //socket.Send(data);
            IAsyncResult asyncSend = _socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(sendCallback), _socket);
            bool success = asyncSend.AsyncWaitHandle.WaitOne(5000, true);
            if (!success)
            {
                Debug.Log("asyncSend error close socket");
                Close();
                return false;
            }
            return true;
        }
        return false;

    }
    private void sendCallback(IAsyncResult asyncSend)
    {
    }
    public void Send(byte[] data, SocketPackage package)
    {
        if (data == null)
        {
            return;
        }
        lock (_sendList)
        {
            _sendList.Add(package);
        }

        try
        {
            PostSend(data);
            //UnityEngine.Debug.Log("Socket send actionId:" + package.ActionId + ", msgId:" + package.MsgId + ", send result:" + bRet);
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.Log("Socket send actionId: " + package.ActionId + " error" + ex);
            package.ErrorCode = (int)ErrorCode.ConnectError;
            package.ErrorMsg = ex.ToString();
            lock (_receiveQueue)
            {
                _receiveQueue.Enqueue(package);
            }
            lock (_sendList)
            {
                _sendList.Remove(package);
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool isDisposing)
    {
        try
        {
            if (!this._isDisposed)
            {
                if (isDisposing)
                {
                    //if (socket != null) socket.Dispose(true);
                }
            }
        }
        finally
        {
            this._isDisposed = true;
        }
    }

    public void ProcessTimeOut()
    {
        SocketPackage findPackage = null;
        lock (_sendList)
        {
            foreach (SocketPackage package in _sendList)
            {
                if (DateTime.Now.Subtract(package.SendTime).TotalSeconds > TimeOut)
                {
                    package.ErrorCode = (int)ErrorCode.TimeOutError;
                    package.ErrorMsg = "TimeOut";
                    findPackage = package;
                    break;
                }
            }
        }
        if (findPackage != null)
        {
            lock (_receiveQueue)
            {
                _receiveQueue.Enqueue(findPackage);
            }
            lock (_sendList)
            {
                _sendList.Remove(findPackage);
            }
        }
    }


}

