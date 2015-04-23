using System;
using System.Collections;
using System.Text;
using UnityEngine;


public class Net : MonoBehaviour, IHttpCallback
{
    public delegate bool CanRequestDelegate(int actionId, ActionParam actionParam);
    public delegate void RequestNotifyDelegate(Status eStatus);
    /// <summary>
    /// 网络请求回调统一处理方法
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public delegate bool CommonDataCallback(NetReader reader);

    /// <summary>
    /// 网络请求出错回调方法
    /// </summary>
    /// <param name="nType"></param>
    /// <param name="actionId"></param>
    /// <param name="strMsg"></param>
    public delegate void NetError(eNetError nType, int actionId, string strMsg);


    public enum Status
    {
        eStartRequest = 0,
        eEndRequest = 1,
    }

    protected static readonly int OVER_TIME = 30;
    private static Net s_instance = null;
    private const int NETSUCCESS = 0;
    private string strUrl;
    private SocketConnect mSocket = null;

    public enum eNetError
    {
        eConnectFailed = 0,
        eTimeOut = 1,
    }

    /// <summary>
    /// 请求代理通知
    /// </summary>
    public RequestNotifyDelegate RequestNotify { set; get; }

    /// <summary>
    /// 注册网络请求出错回调方法
    /// </summary>
    public NetError NetErrorCallback { get; set; }

    /// <summary>
    /// 注册网络请求回调统一处理方法
    /// </summary>
    public CommonDataCallback CommonCallback { get; set; }

    public IHeadFormater HeadFormater { get; set; }

    public int NetSuccess
    {
        get { return NETSUCCESS; }
    }

    public void OnPushCallback(SocketPackage package)
    {
        try
        {
            if (package == null) return;
            //do Heartbeat package
            if (package.ActionId == 1) return;

            GameAction gameAction = ActionFactory.Create(package.ActionId);
            if (gameAction == null)
            {
                throw new ArgumentException(string.Format("Not found {0} of GameAction object.", package.ActionId));
            }
            NetReader reader = package.Reader;
            bool result = true;
            if (CommonCallback != null)
            {
                result = CommonCallback(reader);
            }

            if (result && gameAction.TryDecodePackage(reader))
            {
                ActionResult actionResult = gameAction.GetResponseData();
                gameAction.OnCallback(actionResult);
            }
            else
            {
                Debug.Log("Decode package fail.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public void RequestDelegate(Net.Status eState)
    {
        //todo user implement loading method
        if (eState == Net.Status.eStartRequest)
        {
        }
        else
        {
            //Net.Status.eEndRequest

        }
    }

    void Start()
    {

    }

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
            data = mSocket.DequeuePush();
            if (data != null)
            {
                OnPushCallback(data);
            }
        }
    }

    void OnApplicationQuit()
    {
        if (mSocket != null)
        {
            mSocket.Close();
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
                if (s_instance != null)
                {
                    s_instance.RequestNotify = s_instance.RequestDelegate;
                    s_instance.HeadFormater = new DefaultHeadFormater();
                    s_instance.NetErrorCallback = (type, id, msg) => Debug.LogError(string.Format("Net error:{0}-{1}", id, msg));
                }
            }
            return s_instance;
        }
    }

    public void ReBuildHearbeat()
    {
        if (mSocket != null)
        {
            mSocket.ReBuildHearbeat();
        }
    }

    /// <summary>
    /// Send
    /// </summary>
    /// <param name="actionId"></param>
    /// <param name="callback"></param>
    /// <param name="actionParam"></param>
    /// <param name="bShowLoading"></param>
    public void Send(int actionId, Action<ActionResult> callback, ActionParam actionParam, bool bShowLoading = true)
    {
        GameAction gameAction = ActionFactory.Create(actionId);
        if (gameAction == null)
        {
            throw new ArgumentException(string.Format("Not found {0} of GameAction object.", actionId));
        }
        //if (SceneChangeManager.mInstance != null)
        //    SceneChangeManager.mInstance.ShowLoading(true);

        gameAction.Callback += callback;
        if (NetWriter.IsSocket())
        {
            SocketRequest(gameAction, actionParam, HeadFormater, bShowLoading);
        }
        else
        {
            HttpRequest(gameAction, actionParam, HeadFormater, bShowLoading);
        }
    }

    /// <summary>
    /// parse input data
    /// </summary>
    /// <param name="gameAction"></param>
    /// <param name="actionParam"></param>
    /// <param name="formater"></param>
    /// <param name="bShowLoading"></param>
    private void SocketRequest(GameAction gameAction, ActionParam actionParam, IHeadFormater formater, bool bShowLoading)
    {
        if (mSocket == null)
        {
            string strUrl = NetWriter.GetUrl();
            string[] arr = strUrl.Split(new char[] { ':' });
            int nPort = int.Parse(arr[1]);
            mSocket = new SocketConnect(arr[0], nPort, formater);

        }
        gameAction.Head.MsgId = NetWriter.MsgId - 1;

        SocketPackage package = new SocketPackage();
        package.MsgId = gameAction.Head.MsgId;
        package.ActionId = gameAction.ActionId;
        package.Action = gameAction;
        package.HasLoading = bShowLoading;
        package.SendTime = DateTime.Now;
        byte[] data = gameAction.Send(actionParam);
        NetWriter.resetData();
        if (bShowLoading)
        {
            RequestDelegate(Status.eStartRequest);
        }
        mSocket.Send(data, package);
    }

    /// <summary>
    /// socket respond
    /// </summary>
    /// <param name="package"></param>
    private void OnSocketRespond(SocketPackage package)
    {
        if (package.HasLoading)
        {
            RequestDelegate(Status.eEndRequest);
        }
        if (package.ErrorCode >= 10000)
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
            OnRespond(package);
        }
    }

    private void HttpRequest(GameAction gameAction, ActionParam actionParam, IHeadFormater formater, bool bShowLoading)
    {
        StartCoroutine(HttpGetRequest(gameAction, actionParam, formater, bShowLoading));
        NetWriter.resetData();
    }

    private IEnumerator HttpGetRequest(GameAction gameAction, ActionParam actionParam, IHeadFormater formater, bool showLoading)
    {
        string url = NetWriter.GetUrl();
        byte[] postData = gameAction.Send(actionParam);
        DateTime start = DateTime.Now;
        HttpPackage httpPackage = new HttpPackage();
        httpPackage.WwwObject = NetWriter.IsGet ? new WWW(string.Format("{0}?{1}", url, Encoding.UTF8.GetString(postData))) : new WWW(url, postData);
        httpPackage.ActionId = gameAction.ActionId;
        httpPackage.Action = gameAction;
        httpPackage.Reader = new NetReader(formater);

        if (RequestNotify != null && showLoading)
        {
            RequestNotify(Net.Status.eStartRequest);
        }

        yield return httpPackage.WwwObject;

        if (RequestNotify != null && showLoading)
        {
            RequestNotify(Net.Status.eEndRequest);
        }
        TimeSpan tsStart = new TimeSpan(start.Ticks);
        TimeSpan tsEnd = new TimeSpan(DateTime.Now.Ticks);
        TimeSpan ts = tsEnd.Subtract(tsStart).Duration();

        if (ts.Seconds > OVER_TIME)
        {
            httpPackage.IsOverTime = true;
        }
        OnHttpRespond(httpPackage);
    }

    /// <summary>
    /// http respond
    /// </summary>
    /// <param name="package"></param>
    public void OnHttpRespond(HttpPackage package)
    {
        if (package.error != null)
        {
            OnNetError(package.ActionId, package.error);
        }
        else if (package.IsOverTime)
        {
            OnNetTimeOut(package.ActionId);
        }
        else
        {
            NetReader reader = package.Reader;
            byte[] buffBytes = package.GetResponse();
            if (reader.pushNetStream(buffBytes, NetworkType.Http, NetWriter.ResponseContentType))
            {
                if (reader.Success)
                {
                    OnRespond(package);
                }
                else
                {
                    OnNetError(package.ActionId, reader.Description);
                }
            }
        }
    }

    private void OnRespond(NetPackage package)
    {
        NetReader reader = package.Reader;
        bool result = true;
        if (CommonCallback != null)
        {
            result = CommonCallback(reader);
        }

        if (result && package.Action != null && package.Action.TryDecodePackage(reader))
        {
            ActionResult actionResult = package.Action.GetResponseData();
            package.Action.OnCallback(actionResult);
        }
        else
        {
            Debug.Log("Decode package fail.");
        }
    }

    private void OnNetError(int nActionId, string str)
    {
        if (NetErrorCallback != null)
        {
            NetErrorCallback(eNetError.eConnectFailed, nActionId, str);
        }
    }
    private void OnNetTimeOut(int nActionId)
    {
        if (NetErrorCallback != null)
        {
            NetErrorCallback(eNetError.eTimeOut, nActionId, "timeout.");
        }

    }

}
