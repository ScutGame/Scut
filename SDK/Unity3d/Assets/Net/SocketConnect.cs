using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.IO.Compression;
public class SocketConnect
{
    private Socket socket;
    private string host;
    private int port;
    private bool isDisposed;
    private List<SocketPackage> sendList;
    private Queue<SocketPackage> receiveQueue;
    static private List<SocketPackage> pushActionList = new List<SocketPackage>();//push Action的请求
    private const int TimeOut = 30;//30秒的超时时间
    private Thread thread = null;
    enum ErrorCode
    {
        eSuccess = 0,
        eConnectError = -1,
        eTimeOutError = -2,
    }
    public SocketConnect(string host, int port)
    {
        this.host = host;
        this.port = port;
        sendList = new List<SocketPackage>();
        receiveQueue = new Queue<SocketPackage>();
    }

    static public void AddPushActionID(int actionID, INetCallback callback)
    {
        RemovePushActionID(actionID);
        SocketPackage package = new SocketPackage();
        package.ActionId = actionID;
        package.FuncCallback = callback;
        pushActionList.Add(package);
    }

    static public void RemovePushActionID(int actionID)
    {
        foreach (SocketPackage pack in pushActionList)
        {
            if (pack.ActionId == actionID)
            {
                pushActionList.Remove(pack);
                break;
            }
        }
    }
    public SocketPackage Dequeue()
    {
        lock (receiveQueue)
        {
            if (receiveQueue.Count == 0)
            {
                return null;
            }
            else
            {
                return receiveQueue.Dequeue();
            }
        }
    }
    private void CheckReceive()//(object state)
    {
        while (true)
        {
            if (socket == null) return;
            try
            {
                if (socket.Poll(5, SelectMode.SelectRead))
                {
                    if (socket.Available == 0)
                    {
                        UnityEngine.Debug.Log("Close Socket");
                        Close();
                        break;
                    }
                    byte[] prefix = new byte[4];
                    int recnum = socket.Receive(prefix);

                    if (recnum == 4)
                    {
                        int datalen = BitConverter.ToInt32(prefix, 0);
                        byte[] data = new byte[datalen];
                        int startIndex = 0;
                        recnum = 0;
                        do
                        {
                            int rev = socket.Receive(data, startIndex, datalen - recnum, SocketFlags.None);
                            recnum += rev;
                            startIndex += rev;
                        } while (recnum != datalen);
                        if (data[0] == 0x1f && data[1] == 0x8b && data[2] == 0x08 && data[3] == 0x00)
                        {
                            data = NetReader.Decompression(data);  
                        }
                        NetReader reader = new NetReader();
                        reader.pushNetStream(data, NetworkType.Http);
                        SocketPackage findPackage = null;
                        UnityEngine.Debug.Log("socket.Poll ok" + recnum + "actionId:" + reader.ActionId + " " + reader.RmId.ToString() + "len" + datalen.ToString());
                        lock (sendList)
                        {
                            foreach (SocketPackage package in sendList)
                            {
                                if (package.MsgId == reader.RmId)
                                {
                                    package.Reader = reader;
                                    package.ErrorCode = (int)ErrorCode.eSuccess;
                                    package.ErrorMsg = "success";
                                    findPackage = package;
                                    break;
                                }

                            }
                        }
                        foreach (SocketPackage package in pushActionList)
                        {
                            if (package.ActionId == reader.ActionId)
                            {
                                package.Reader = reader;
                                package.ErrorCode = (int)ErrorCode.eSuccess;
                                package.ErrorMsg = "success";
                                findPackage = package;
                            }
                        }
                        if (findPackage != null)
                        {
                            lock (receiveQueue)
                            {
                                receiveQueue.Enqueue(findPackage);
                            }
                            lock (sendList)
                            {
                                sendList.Remove(findPackage);
                            }
                        }

                    }

                }
                else if (socket.Poll(5, SelectMode.SelectError))
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
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(host, port);
            }
            catch
            {
                //socket.Dispose();
                socket = null;
                throw;
            }
            thread = new Thread(new ThreadStart(CheckReceive));
            thread.Start();
        }

    }

    private void EnsureConnected()
    {
        if (socket == null)
        {
            Open();
        }
      
    }

    /// <summary>
    /// 关闭连接
    /// </summary>
    public void Close()
    {
        if (socket == null) return;
        try
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            socket = null;
        }
        catch (Exception ex)
        {
            socket = null;
        }
    }

    /// <summary>
    /// 发送数据
    /// </summary>
    /// <param name="data"></param>
    public bool Send(byte[] data)
    {
        EnsureConnected();
        if (socket != null)
        {
             //socket.Send(data);
            IAsyncResult asyncSend = socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(sendCallback), socket);
            bool success = asyncSend.AsyncWaitHandle.WaitOne(5000, true);
            if (!success)
            {
                UnityEngine.Debug.Log("asyncSend error close socket");
                Close();
                return false;
            }
          
        }
        return true;

    }
    private void sendCallback (IAsyncResult asyncSend)
    {
       

    }
    public void Request(byte[] data, SocketPackage package)
    {
        if (data == null)
        {
            return;
        }
        lock (sendList)
        {
            sendList.Add(package);
        }

        try
        {
            bool bRet = Send(data);
            UnityEngine.Debug.Log("Socket Request Id:" + package.ActionId.ToString() + " " + package.MsgId.ToString() + "send:" + bRet.ToString());
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.Log("Request error" + ex.ToString());
            package.ErrorCode = (int)ErrorCode.eConnectError;
            package.ErrorMsg = ex.ToString();
            lock (receiveQueue)
            {
                receiveQueue.Enqueue(package);
            }
            lock (sendList)
            {
                sendList.Remove(package);
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
            if (!this.isDisposed)
            {
                if (isDisposing)
                {
                    //if (socket != null) socket.Dispose(true);
                }
            }
        }
        finally
        {
            this.isDisposed = true;
        }
    }

    public void ProcessTimeOut()
    {
        SocketPackage findPackage = null;
        lock (sendList)
        {
            foreach (SocketPackage package in sendList)
            {
                if (DateTime.Now.Subtract(package.SendTime).TotalSeconds > TimeOut)
                {
                    package.ErrorCode = (int)ErrorCode.eTimeOutError;
                    package.ErrorMsg = "TimeOut";
                    findPackage = package;
                    break;
                }
            }
        }
        if (findPackage != null)
        {
            lock (receiveQueue)
            {
                receiveQueue.Enqueue(findPackage);
            }
            lock (sendList)
            {
                sendList.Remove(findPackage);
            }
        }
    }


}

