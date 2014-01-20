using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Net.Sockets;
using System.Net;

public partial class ServerResponse
{
    private static ServerResponse s_instance = null;
    public static ServerResponse Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new ServerResponse();
            }
            return s_instance;
        }
    }
    public class ResponseData
    {
        public IResponse Resonse { get; set; }
        public string ErrorMsg { get; set; }
        public int ErrorCode { get; set; }
        public int ActionId { get; set; }
    }

    public ResponseData GetData(NetReader reader)
    {

        ResponseData data = new ResponseData();
        data.ActionId = reader.ActionId;
        data.ErrorCode = reader.StatusCode;
        data.ErrorMsg = reader.Description;
        IResponse ret = null;
        if (data.ErrorCode == Net.Instance.NetSuccess)
        {
             ret = GetResponse(reader, data.ActionId);
        }
        data.Resonse = ret;
        return data;
    }

}


public class Net : MonoBehaviour, IHttpCallback
{
    public enum Status
    {
        eStartRequest = 0,
        eEndRequest = 1,
    }
    public delegate void RequestNotifyDelegate(Status eStatus);
    static Net s_instance = null;
    private const int NETSUCCESS = 0;
    private string strUrl;
    private SocketConnect mSocket = null;
    public delegate bool CanRequestDelegate(int actionId, object userData);
   
    public enum eNetError
    {
        eConnectFailed = 0,
        eTimeOut = 1,
    }

    public delegate bool CommonDataCallback(NetReader reader);
    public delegate void NetError(eNetError nType, int ActionId, string strMsg);

    public NetError NetErrorCallback
    {
        get;
        set;
    }
    public CommonDataCallback CommonCallback
    {
        get;
        set;
    }

    
    public int NetSuccess
    {
        get { return NETSUCCESS; }
    }
    void Start()
    {
        CommonCallback = NetResponse.Instance.CommonData;
        NetErrorCallback = NetResponse.Instance.NetError;
       
    }
    public void RequestDelegate(Net.Status eState)
    {
       // RequestDelegate(eState, null);
        //todo user loading
        if (eState == Net.Status.eStartRequest)
        {
        }
        else//Net.Status.eEndRequest
        {

        }
    }
   /* public LoginLoadingLogo mLoadingLogo = null;
    public void RequestDelegate(Net.Status eState)
    {
        RequestDelegate(eState, null);
    }

    public void RequestDelegate(Net.Status eState, string strText)
    {
        if (eState == Net.Status.eStartRequest)
        {
            if (mLoadingLogo != null)
            {
                mLoadingLogo.nCounter++;
            }
            else
            {
                mLoadingLogo = LoginLoadingLogo.Create(strText);
            }
        }
        else
        {
            if (mLoadingLogo != null)
            {
                mLoadingLogo.nCounter--;
                if (mLoadingLogo.nCounter <= 0)
                {
                    mLoadingLogo.CloseWindow();
                    mLoadingLogo = null;
                }
            }
        }
    }
	*/
    void Awake()
    {
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);

    }
    void Update()
    {
        if (mSocket != null)
        {
            mSocket.ProcessTimeOut();
            SocketPackage data = mSocket.Dequeue();
            if (data != null)
            {
                OnSocketRespond(data);
            }
        }
    }

    public static Net Instance
    {
        get
        {
            s_instance = UnityEngine.Object.FindObjectOfType(typeof(Net)) as Net;
            if (s_instance == null)
            {
                GameObject obj2 = new GameObject("net");
                s_instance = obj2.AddComponent(typeof(Net)) as Net;
                Http.RequestNotify = s_instance.RequestDelegate;
            }
            return s_instance;
        }
    }
    /// <summary>
    /// CallBack的函数要保证它在网络回来时的生命周期依然可用
    /// </summary>
    /// <param name="actionId"></param>
    /// <param name="callback"></param>
    /// <param name="userData"></param>
    public void Request(int actionId, INetCallback callback, object userData)
    {
        Request(actionId, callback, userData, true);
    }
    public void Request(int actionId, INetCallback callback, object userData, bool bShowLoading)
    {
       if (NetWriter.IsSocket())
        {
            SocketRequest(actionId, callback, userData, bShowLoading);
        }
        else
        {
            HttpRequest(actionId, callback, userData, bShowLoading);
        }
    }
    //
    //NetWriter.Instance.writeInt32()
    //发送请求
    public void HttpRequest(int actionId, INetCallback callback, object userData)
    {
        HttpRequest(actionId, callback, userData, true);
    }

    public void HttpRequest(int actionId, INetCallback callback, object userData, bool bShowLoading)
    {
        NetWriter writer = NetWriter.Instance;
        writer.writeInt32("actionId", actionId);
        StartCoroutine(Http.GetRequest(writer.generatePostData(), userData, actionId, callback, this, bShowLoading));
        NetWriter.resetData();
    }

    /// <summary>
    /// parse input data
    /// </summary>
    /// <param name="url"></param>
    /// <param name="callback"></param>
    /// <param name="ud"></param>
    public void SocketRequest(int actionId, INetCallback callback, object userData, bool bShowLoading)
    {
        
        if (mSocket == null)
        {
            string strUrl = NetWriter.GetUrl();
			Debug.Log("url" + strUrl);
            string[] arr = strUrl.Split(new char[] { ':' });
            int nPort = int.Parse(arr[1]);
            mSocket = new SocketConnect(arr[0], nPort);
        }
        NetWriter writer = NetWriter.Instance;
        writer.writeInt32("actionId", actionId);
        byte[] data = NetWriter.Instance.PostData();
        SocketPackage package = new SocketPackage();
        package.FuncCallback = callback;
        package.UserData = userData;
        package.MsgId = NetWriter.MsgId - 1;
        package.ActionId = actionId;
        package.HasLoading = bShowLoading;
        package.SendTime = DateTime.Now;
        NetWriter.resetData();
       
        if (bShowLoading)
        {
            RequestDelegate(Status.eStartRequest);
        }
        mSocket.Request(data, package);
    }
    public void SocketRequest(int actionId, INetCallback callback, object userData)
    {
        SocketRequest(actionId, callback, userData, true);
    }


    /// <summary>
    /// socket respond
    /// </summary>
    /// <param name="package"></param>
    /// <param name="userdata"></param>
    public void OnSocketRespond(SocketPackage package)
    {
        if (package.HasLoading)
        {
            RequestDelegate(Status.eEndRequest);
        }
        if (package.ErrorCode != 0)
        {
            if (package.ErrorCode == -2)
            {
                OnNetTimeOut(package.ActionId);
            }
            else
            {
                OnNetError(package.ActionId, package.ErrorMsg);
            }
           
        }
        else
        {
            ServerResponse.ResponseData data = null;
            NetReader reader = package.Reader;
            bool bRet = true;

            if (CommonCallback != null)
            {
                bRet = CommonCallback(reader);
            }
            
            if (bRet)
            {
                data = ServerResponse.Instance.GetData(reader);
                if (package.FuncCallback != null)
                {
                    ProcessBodyData(data, package.UserData, package.FuncCallback);
                }
                else
                {
                    Debug.Log("poll message ");
                }
                
            }
        }
    }

    /// <summary>
    /// http respond
    /// </summary>
    /// <param name="package"></param>
    /// <param name="userdata"></param>
    public void OnHttpRespond(HttpPackage package, object userdata)
    {
        if (package.error != null)
        {
            OnNetError(package.Tag, package.error.ToString());
        }
        else if (package.overTime)
        {
            OnNetTimeOut(package.Tag);
        }
        else
        {
            NetReader reader = new NetReader();
            reader.pushNetStream(package.buf, NetworkType.Http);
            ServerResponse.ResponseData data = null;
            bool bRet = true;
            if (reader.ActionId != 0)//获取服务器列表比较特殊没有协议头
            {
                if (CommonCallback != null)
                {
                    bRet = CommonCallback(reader);
                }
            }
            if (bRet)
            {
                data = ServerResponse.Instance.GetData(reader);
                ProcessBodyData(data, userdata, package.FuncCallback);
            }
            reader = null;
        }
    }

    public void OnNetError(int nActionId, string str)
    {
        if (NetErrorCallback != null)
        {
            NetErrorCallback(eNetError.eConnectFailed, nActionId, str);
        }
    }
    public void OnNetTimeOut(int nActionId)
    {
        if (NetErrorCallback != null)
        {
            NetErrorCallback(eNetError.eTimeOut, nActionId, null);
        }

    }

    public void ProcessBodyData(ServerResponse.ResponseData data, object userdata, INetCallback callback)
    {
        Debug.Log("Net ProcessBodyData " + data.ActionId + " ErrorCode " + data.ErrorCode + " ErrorMsg " + data.ErrorMsg);
        callback(data, userdata);
    }
}
