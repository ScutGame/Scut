package ScutDataLogic
{
	import flash.utils.ByteArray;
	import flash.utils.Dictionary;
	
	import ScutDataLogic.CDataHandler;
	import ScutDataLogic.RequestInfo;
	import ScutDataLogic.overload;
	
	import ScutNetwork.AsyncInfo;
	import ScutNetwork.CHttpClient;
	import ScutNetwork.CHttpClientResponse;
	import ScutNetwork.CHttpSession;
	import ScutNetwork.CMemoryStream;
	import ScutNetwork.CStream;
	import ScutNetwork.INetStatusNotify;

	public class CDataRequest implements INetStatusNotify
	{
		
		private static var g_DataRequestInst:CDataRequest=null;
		
		private var m_pOnlyOneClient:CHttpClient;
		private var m_pOnlyOneSession:CHttpSession;
		private var m_RequestInfos:Vector.<RequestInfo>=new Vector.<RequestInfo>();
		private var m_pMapRequestInfos:Dictionary=new Dictionary();
		
		public static var g_bUseSequenceNetRequest:Boolean=false;
		public static const TICK_ACTIONLIMIT:int = 1000; //1秒
		public static const MSG_DATAREQUEST_TERMINATED:int = 0x1080;
		
		private var m_pLuaDataHandleCallBack:Function;
		
		public function CDataRequest()
		{
			if (g_bUseSequenceNetRequest)
			{
				m_pOnlyOneSession = new CHttpSession();
				m_pOnlyOneClient = new CHttpClient(m_pOnlyOneSession);
			}
		}
		
		public function Dispose():void
		{
			//释放剩下的请求
//			AUTO_GUARD(m_ThreadMutex);
//			if (m_pMapRequestInfos)
//			{
//				map<CHttpClient*, PRequestInfo>::iterator it = m_pMapRequestInfos->begin();
//				for (; it != m_pMapRequestInfos->end(); it++)
//				{
//					FreeRequestInfo(it->second);
//				}
//				delete m_pMapRequestInfos;
//			}
			
			if(m_pMapRequestInfos)
			{
				m_pMapRequestInfos = null;
//				for each(var key:* in m_pMapRequestInfos)
//				{
//					m_pMapRequestInfos[key]=null;
//					delete m_pMapRequestInfos[key];
//				}
			}
			
			if (m_pOnlyOneClient)
			{
				 m_pOnlyOneClient=null;
			}
			if (m_pOnlyOneSession)
			{
				 m_pOnlyOneSession=null;
			}
			
		}
		
		
		public function OnNotify(pAi:AsyncInfo):void
		{
			ProcAsyncInfo(pAi);
		}
		
		public static function Instance():CDataRequest
		{
			if(g_DataRequestInst==null)
			{
				g_DataRequestInst=new CDataRequest();
			}
			return g_DataRequestInst;
		}
		
		public function ExecRequest(...args):Boolean
		{
			return overload(2,[ExecRequest1,[CDataHandler,String],0],[ExecRequest2,[Object],0],args)	
		}
//		public function AsyncExecRequest(...args):Number
//		{
//			return overload(2,[AsyncExecRequest1,[CDataHandler,int,String,ByteArray,Object],1],[AsyncExecRequest2,[Object,String,int,ByteArray],0],args);
//		}
		
		public function AsyncExecRequest(evtName:String,lpszUrl:String,nTag:int,lpData:ByteArray,pScene:Object=null):Number
		{
			var dwRet:Number = 0;
			if (/*pHandler && ShouldRequest(pHandler, nTag, lpszUrl)*/1)
			{
				//加入请求列表
				//				AUTO_GUARD(m_ThreadMutex);
				var pRi:RequestInfo = new RequestInfo();
				if (g_bUseSequenceNetRequest && !m_pOnlyOneClient.IsBusy() && m_RequestInfos.length == 0)
				{
					pRi.HttpClient = m_pOnlyOneClient;
					pRi.HttpClient.Reset();
					pRi.HttpClient.AddHeader("Connection", "Keep-Alive");
				}
				else
				{
					pRi.HttpSession = new CHttpSession();
					pRi.HttpClient = new CHttpClient(pRi.HttpSession);
					pRi.HttpClient.AddHeader("Connection", "Keep-Alive");
				}	
				pRi.EventName = evtName;
				pRi.Handler = null;
				pRi.HandlerTag = nTag;
				pRi.HttpResponse = new CHttpClientResponse();
				pRi.HttpResponse.SetTarget(new CMemoryStream());
				pRi.Url = lpszUrl;
				pRi.LPData = lpData;
				pRi.pScene = pScene;
				pRi.HttpClient.SetNetStautsNotify(this);
				m_pMapRequestInfos[pRi.HttpClient] = pRi;
				//				(*m_pMapRequestInfos)[pRi.HttpClient] = pRi;
				//执行异步网络请求
				if (pRi.HttpClient.AsyncHttpGet(pRi.Url, pRi.HttpResponse) == 0)
				{
					//					dwRet = pRi.HttpClient;
				}
				CurrentRequstInfo=pRi;
			}
			return dwRet;
		}
		
		///////////////////////////C++ 模式相关函数//////////////////////
		
		//同步执行请求
		public function ExecRequest1(pHandler:CDataHandler, lpszUrl:String):Boolean
		{
			return false;	
		}
		//异步执行请求
		public function AsyncExecRequest1(pHandler:CDataHandler, nTag:int, lpszUrl:String, lpData:ByteArray, pScene:Object=null):Number
		{
			var dwRet:Number = 0;
			if (/*pHandler && ShouldRequest(pHandler, nTag, lpszUrl)*/1)
			{
				//加入请求列表
//				AUTO_GUARD(m_ThreadMutex);
				var pRi:RequestInfo = new RequestInfo();
				if (g_bUseSequenceNetRequest && !m_pOnlyOneClient.IsBusy() && m_RequestInfos.length == 0)
				{
					pRi.HttpClient = m_pOnlyOneClient;
					pRi.HttpClient.Reset();
					pRi.HttpClient.AddHeader("Connection", "Keep-Alive");
				}
				else
				{
					pRi.HttpSession = new CHttpSession();
					pRi.HttpClient = new CHttpClient(pRi.HttpSession);
					pRi.HttpClient.AddHeader("Connection", "Keep-Alive");
				}		
				pRi.Handler = pHandler;
				pRi.HandlerTag = nTag;
				pRi.HttpResponse = new CHttpClientResponse();
				pRi.HttpResponse.SetTarget(new CMemoryStream());
				pRi.Url = lpszUrl;
				pRi.LPData = lpData;
				pRi.pScene = pScene;
				pRi.HttpClient.SetNetStautsNotify(this);
				m_pMapRequestInfos[pRi.HttpClient] = pRi;
//				(*m_pMapRequestInfos)[pRi.HttpClient] = pRi;
				//执行异步网络请求
				if (pRi.HttpClient.AsyncHttpGet(pRi.Url, pRi.HttpResponse) == 0)
				{
//					dwRet = pRi.HttpClient;
				}
				CurrentRequstInfo=pRi;
			}
			return dwRet;
		}
		
		/********测试用*******************************************/
		private var currentRequstInfo:RequestInfo=new RequestInfo();
		public function set CurrentRequstInfo(info:RequestInfo):void
		{
			this.currentRequstInfo=info;
		}
		public function get CurrentRequstInfo():RequestInfo
		{
			return this.currentRequstInfo;
		}
		
		/*********************************************************/
		//取消异步请求，暂不支持
		public function CancelAsyncRequest(dwRequestID:Number):Boolean
		{
			return true;	
		}
		
		///////////////////////////LUA 模式相关函数//////////////////////
		
		//同步执行请求
		public function ExecRequest2(pScene:Object):Boolean
		{
			return true;	
		}
		//添加数据的处理者 目前默认是NdScene
		public function AsyncExecRequest2(pScene:Object, szUrl:String, nTag:int,  lpData:ByteArray):Number
		{
			var nRet:Number =  this.AsyncExecRequest1(null, nTag, szUrl, lpData, pScene);
			
			return nRet;
		}
		
		//注册数据处理回调函数
		public function RegisterLUACallBack(pCallBack:Function):void
		{
			m_pLuaDataHandleCallBack = pCallBack;
		}
		//取一笔LUA数据并进行处理
		public function PeekLUAData():void
		{
//			AUTO_GUARD(m_ThreadMutex);
			for each(var info:RequestInfo in m_RequestInfos)
			{
//				trace("PeekLUAData 这个时候有下发数据 m_RequestInfos.length="+m_RequestInfos.length)
				var pRi:RequestInfo = info;
				//处理状态值
				var nStatus:int = pRi.HttpResponse.GetStatusCode();
				if (nStatus == 200)
				{
					nStatus = pRi.Status;			
				}
//				LuaHandleData(pRi.pScene, pRi.HandlerTag, nStatus, pRi.HttpResponse.GetTarget(), pRi.LPData);
				LuaHandleData(pRi.EventName, pRi.HandlerTag, nStatus, pRi.HttpResponse.GetTarget(), pRi.LPData);
				pRi=null;
//				FreeRequestInfo(pRi);
			}
			m_RequestInfos.length=0;
		}
		
		private function LuaHandleData(evtName:String,nTag:int,nNetRet:int,lpData:CStream,lpExternal:ByteArray):void
		{
			if (m_pLuaDataHandleCallBack!=null)
			{
				m_pLuaDataHandleCallBack(evtName, nTag, nNetRet, lpData, lpExternal);
			}
		}
				
		
		
		private function ProcAsyncInfo(pAi:AsyncInfo):void
		{
			if (!pAi)
			{
				return;
			}
//			map<CHttpClient*, PRequestInfo>::iterator it;
//			it = m_pMapRequestInfos->find(pAi.Sender); 
//			if (it != m_pMapRequestInfos->end())
//			{
//				PRequestInfo pRi = it->second;
//				m_pMapRequestInfos.erase(it);
//				pRi.Status = pAi.Status;
//				m_RequestInfos.push(pRi);
//			}
			if(m_pMapRequestInfos[pAi.Sender])
			{
				var pRi:RequestInfo=m_pMapRequestInfos[pAi.Sender];
				pRi.Status = pAi.Status;
				m_RequestInfos.push(pRi);
				m_pMapRequestInfos[pAi.Sender]=null;
				delete m_pMapRequestInfos[pAi.Sender];
			}
			
		}
		
		
		public function SetUseKeepAliveMode(bKeepAlive:Boolean):void
		{
			
		}
		
		public function GetUseKeepAliveMode():Boolean
		{
			return false;
		}
		
	}
}