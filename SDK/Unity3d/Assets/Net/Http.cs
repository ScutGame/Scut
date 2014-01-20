using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

public class HttpPackage {
	public string error {
		get {
			if(overTime) {
				return "http request over time";
			} else {
				return w.error;
			}
		}
	}
	
	public byte[] buf {
		get {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return w.bytes;
            }
            string strEncoding = null;
            if (w.responseHeaders.ContainsKey("CONTENT-ENCODING"))
            {
                strEncoding = w.responseHeaders["CONTENT-ENCODING"];
            }
            if (strEncoding != null && strEncoding == "gzip")
            {
                return NetReader.Decompression(w.bytes);
            }
            return w.bytes;
		}
	}

    public int Tag
    {
        get;
        set;
    }
    public INetCallback FuncCallback
    {
        set;
        get;
    }
	public WWW w { get; set; }
	public bool overTime { get; set; }
}

public class Http {
	
	protected static readonly int OVER_TIME = 30;
	
	protected static Http s_inst = new Http();

    public static Net.RequestNotifyDelegate RequestNotify { set; get; }

	protected Http() {
		NdWriter = NetWriter.Instance;
	}
	
	
	public static NetWriter NdWriter {
		get;
		private set;
	}
	public static IEnumerator GetRequest(string url, object ud, int tag, INetCallback callback, IHttpCallback httpCallback, bool showLoading) {
		DateTime start = DateTime.Now;
		
		HttpPackage hp = new HttpPackage();
		hp.w = new WWW(url);
        hp.Tag = tag;
        hp.FuncCallback = callback;
        if (RequestNotify != null && showLoading)
        {
            RequestNotify(Net.Status.eStartRequest);
        }
        
		yield return hp.w;

        if (RequestNotify != null && showLoading)
        {
            RequestNotify(Net.Status.eEndRequest);
        }
		if(null != httpCallback) {
			TimeSpan tsStart = new TimeSpan(start.Ticks);
	        TimeSpan tsEnd = new TimeSpan(DateTime.Now.Ticks);
	        TimeSpan ts = tsEnd.Subtract(tsStart).Duration();
			
			if(ts.Seconds > OVER_TIME) {
				hp.overTime = true;
			}
			httpCallback.OnHttpRespond(hp, ud);
		} else {
			Debug.Log("no http callback");
		}
	}
}
