package ScutNetwork
{
	
	import flash.events.Event;
	import flash.events.EventDispatcher;
	import flash.events.HTTPStatusEvent;
	import flash.events.IOErrorEvent;
	import flash.events.ProgressEvent;
	import flash.events.SecurityErrorEvent;
	import flash.net.URLRequest;
	import flash.net.URLRequestMethod;
	import flash.utils.ByteArray;
	import flash.utils.Endian;
	
	import ccloader.loadingtypes.LoadingItem;
	import ccloader.loadingtypes.URLItem;
	
	import net.HttpManager;

	public class CHttpClient
	{
		/**
		 * 单个 HTTP 请求中的所有信息
		 */		
		private var urlRequest:URLRequest;
		
		private var bUseHttpProxy:Boolean;
		private var httpProxyHost:String="";
		private var httpProxyPort:uint;
		private var bUseHttpsProxy:Boolean;
		private var httpsProxyHost:String;
		private var httpsProxyPort:uint;
		private var pResponsePage:String;
		private var responsePageLen:uint;
		private var headers:SingleLinkList;
		private var 	session:CHttpSession;
		private var 	host:String;//[256]
		private var 	m_sgThreadID:uint;
		private var 	m_sAsyncInfo:AsyncInfo;
		public var 	m_bIsBusy:Boolean;
		public var 	m_bUseProgressReport:Boolean;
		public var 	m_bAsyncProcessing:Boolean;
		public var  m_nTimeOut:int;
		public var referer:String;
		public var 	m_pNetNotify:INetStatusNotify;	//网络状态通知接口
		
		public function CHttpClient( s:CHttpSession )
		{
			pResponsePage   = null;
			headers         = new SingleLinkList();
			host	        = '\0';
			httpProxyHost = '\0';
			httpsProxyHost = '\0';
			session			= s;
			referer         = null;
			m_sgThreadID = 0;
			m_bIsBusy = false;
			m_bUseProgressReport = false;
			m_bAsyncProcessing = false;
			m_nTimeOut = 15;		//默认15s超时
			m_pNetNotify = null;
			
			Initialize();
		}
		
		public function Dispose():void
		{
			m_sAsyncInfo.Reset();
		}
		
		/**** 同步Get与Post ****/
		public function HttpGet( url:String,  resp:CHttpClientResponse):int
		{
			return 0;
		}
		public function HttpPost( url:String,  postData:ByteArray, nPostDataSize:int, resp:CHttpClientResponse, formflag:Boolean=false):Boolean
		{
			return true;
		}
		
		/**** 同步Get与Post ****/
		//由于异步处理，因此resp参数必须一直有效
		public function AsyncHttpGet( url:String,  resp:CHttpClientResponse):int
		{
			m_sAsyncInfo.Reset();
			m_sAsyncInfo.Url = url;
			m_sAsyncInfo.Response = resp;
			m_sAsyncInfo.Sender = this;
			m_sAsyncInfo.Mode = AsyncInfo.amGet;
			
			urlRequest.method = URLRequestMethod.GET;
			urlRequest.url=url;
			if(HttpManager.httpLoad.hasItem(url))
			{
				HttpManager.httpLoad.reload(url);
			}
			else
			{
				trace("GET请求的URL="+urlRequest.url)
				HttpManager.httpLoad.add(urlRequest,{id:url});
				HttpManager.httpLoad.start();
			}
			
			/**当前的加载项**/
			var currentLoadItem:LoadingItem = HttpManager.httpLoad.get(url);
			addEventListeners(currentLoadItem);
			currentLoadItem=null;
			
			return 1;
		}
		
		public function AsyncHttpPost( url:String,  postData:ByteArray, nPostDataSize:int, resp:CHttpClientResponse, formflag:Boolean=false):int
		{
			m_sAsyncInfo.Reset();
			m_sAsyncInfo.Url = url;
			m_sAsyncInfo.Response = resp;
			m_sAsyncInfo.Sender = this;
			m_sAsyncInfo.PostData = postData;
			m_sAsyncInfo.PostDataSize = nPostDataSize;
			m_sAsyncInfo.FormFlag = formflag;
			m_sAsyncInfo.Mode = AsyncInfo.amPost;
			
			urlRequest.method = URLRequestMethod.POST;
			urlRequest.url=url;
//			urlRequest.dat
			if(HttpManager.httpLoad.hasItem(url))
			{
				HttpManager.httpLoad.reload(url);
			}
			else
			{
				trace("POST请求的URL="+urlRequest.url)
				HttpManager.httpLoad.add(urlRequest,{id:url});
				HttpManager.httpLoad.start();
			}
			/**当前的加载项**/
			var currentLoadItem:LoadingItem=HttpManager.httpLoad.get(url);
			addEventListeners(currentLoadItem);
			currentLoadItem=null;
			
			return 1;
		}
		
		private function addEventListeners(dispatch:EventDispatcher):void
		{
			dispatch.addEventListener(Event.COMPLETE,onCurrentComplete);
			dispatch.addEventListener(HTTPStatusEvent.HTTP_STATUS,onHttpStatus,false,0,true);
			dispatch.addEventListener(Event.OPEN, openHandler,false,0,true);
			dispatch.addEventListener(ProgressEvent.PROGRESS, progressHandler,false,0,true);
			dispatch.addEventListener(SecurityErrorEvent.SECURITY_ERROR, securityErrorHandler,false,0,true);
			dispatch.addEventListener(IOErrorEvent.IO_ERROR, ioErrorHandler,false,0,true);
		}
		
		private var _httpResult:ByteArray=new ByteArray();

		/********测试用*******************************************/
		public function get httpResult():ByteArray
		{
			return _httpResult;
		}

		/**
		 * @private
		 */
		public function set httpResult(value:ByteArray):void
		{
			_httpResult = value;
		}
		
		
		/***************************************************/
		
		
		public function onCurrentComplete(e:Event):void
		{
			var urlItem:URLItem = URLItem(e.target);
			var byteAry:ByteArray=HttpManager.httpLoad.getBinary(urlItem.id, true);
			byteAry.position=0;
			//byteAry.endian=Endian.LITTLE_ENDIAN;
//			trace("下发数据长度="+byteAry.length+",,URL="+decodeURIComponent(urlItem.url.url));
			/*** 如果长度大于512 或者请求头Content-Encoding = gzip  则需要解压 ***/
//			if(byteAry.length == 104 )//&& byteAry.length>512
//			{
//				byteAry.uncompress();
//				var position : uint = byteAry.position;
//				var bit1 : uint = byteAry.readUnsignedByte();
//				var bit2 : uint = byteAry.readUnsignedByte();
//				var bit3 : uint = byteAry.readUnsignedByte();
//				var bit4 : uint = byteAry.readUnsignedByte();
				
//				var buf:ByteArray;
//				var gzip:GZIPBytesEncoder = new GZIPBytesEncoder();
//				byteAry.endian = Endian.LITTLE_ENDIAN;
//				buf = gzip.uncompressToByteArray(byteAry);
//				buf.position = 0;
//				byteAry.endian = Endian.LITTLE_ENDIAN;
//				trace("解压后的数据="+buf)
//			}
			byteAry.endian=Endian.LITTLE_ENDIAN;
			httpResult=byteAry;
			var nRet:int=-1;
			var pAi:AsyncInfo=GetAsyncInfo();
			if (pAi.Sender==null)
			{
				throw new Error("pAi.Sender==null");
			}
			/****将获取的数据复制给response***/
			pAi.Response.SetStatusCode(urlItem.httpStatus);
			if(urlItem.httpStatus == 200)
			{
				pAi.Status=AsyncInfo.aisSucceed;
			}
			else if(urlItem.httpStatus == 408)
			{
				pAi.Status=AsyncInfo.aisTimeOut;
			}
			else
			{
				pAi.Status=AsyncInfo.aisFailed;
			}
			pAi.Response.SetRequestUrl(urlItem.url.url);
			
			var stream:CStream=pAi.Response.GetTarget();
			stream.WriteBuffer(byteAry,byteAry.length);

			pAi.Sender.m_bAsyncProcessing = true;
			pAi.Sender.m_bIsBusy = true;
			pAi.Sender.m_bAsyncProcessing = false;
			//通知				
			if (pAi.Sender.m_pNetNotify)
			{
				pAi.Sender.m_pNetNotify.OnNotify(pAi);
			}		
			pAi.Sender.m_bIsBusy = false;
			
			urlItem=null;
		}
		
		public function getInt64(byteAry:ByteArray):Number
		{
			var long_b:int=byteAry.readInt();
			var long_l:uint=byteAry.readUnsignedInt();
			return Number (long_b) * (1<<32) +  long_l ;
		}
		
		public function onHttpStatus(e:HTTPStatusEvent):void
		{
//			trace("返回HTTP请求状态="+e.status);
		}
		private function openHandler(event:Event):void {
//			trace("openHandler: " + event);
		}
		private function progressHandler(event:ProgressEvent):void {
//			trace("progressHandler loaded:" + event.bytesLoaded + " total: " + event.bytesTotal);
		}
		private function securityErrorHandler(event:SecurityErrorEvent):void {
			trace("securityErrorHandler: " + event);
		}
		private function ioErrorHandler(event:IOErrorEvent):void {
			trace("ioErrorHandler: " + event);
		}
		
		
		//异步情况下，需要设定网络状态通知接口
		public function SetNetStautsNotify( pNetNotify:INetStatusNotify ):void
		{
			m_pNetNotify = pNetNotify;
		}
		
		public function GetNetType():int
		{
			return 1;
		}

		//头信息与代理服务器设置
		public function AddHeader( name:String,  value:String):Boolean
		{
//			var urlHeader:URLRequestHeader=new URLRequestHeader(name,value);
//			urlRequest.requestHeaders.push(urlHeader);
			
			
//			var header:String;
//			
//			if(name == null || value == null)
//				header = "\r\n";
//			else
//				header.Format("%s: %s", name, value);
//			
//			// If Cookie, replace existing line, if any
//			var found:int = 0;
//			if (headers != null && name == "Cookie" )
//			{
//				var h:SingleLinkList=headers;
////				struct curl_slist *h = headers;
//				
//			var node:SingleLinkNode=h.firstNode;
//			while(node!=null)
//			{
//				if(node.data.slice(0,6) == name.slice(0,6) && node.data.charAt(6) == ":" )
//				{
////					SAFE_FREE(h.data);
//					node.data="";
//					var dup = header.toString().concat();
//					node.data = dup;
//					found++;
//				}
//				node=node.next;
//			}
//				for (; h; h = h.next)
//				{
//					if (strncmp(h.data, name, 6) == 0 && h.data[6] == ':')
//					{
//						SAFE_FREE(h.data);
//						var dup = header.toString().concat();
//						h.data = dup;
//						found++;
//					}
//				}
//			}
			
//			if (found == 0)
//				headers = curl_slist_append(headers, header);
//			
			return true;
		}
		
		public function UseHttpProxy( bUseProxy:Boolean ):void
		{
			bUseHttpProxy = bUseProxy;
		}
		
		public function SetHttpProxy( proxyHost:String, proxyPort:uint ):void
		{
			httpProxyHost = proxyHost.concat();
			httpProxyPort = proxyPort;
		}
		
		public function UseHttpsProxy( bUseProxy:Boolean ):void
		{
			bUseHttpsProxy = bUseProxy;
		}
		
		
		public function SetHttpsProxy( proxyHost:String, proxyPort:uint ):void
		{
			httpsProxyHost=proxyHost.concat();
			httpsProxyPort = proxyPort;
		}
		
		//获取和设定超时时间，以秒为单位，0或INFINTE表示无超时时间
		public function GetTimeOut():int
		{
			return m_nTimeOut;
		}
		public function SetTimeOut( nTimeOut:int ):void
		{
			if (nTimeOut != m_nTimeOut && nTimeOut >= 0)
			{
				m_nTimeOut = nTimeOut;
			}
		}
		
		
		//是否使用进度通知
		public function GetUseProgressReport():Boolean
		{
			return m_bUseProgressReport;
		}
		public function SetUseProgressReport( bReport:Boolean ):void
		{
			m_bUseProgressReport = bReport;
		}
		
		
		//重设状态
		public function FullReset():void
		{
//			if (curl_handle != null)
//			{
				//		curl_easy_cleanup(curl_handle); // cleanup curl stuff
				//        curl_handle = null;
				
//			}
			Reset();
//			referer = null;
		}
		
		public function Reset():void
		{
			pResponsePage="";
			responsePageLen = 0;
			m_bAsyncProcessing = false;
			
//			if(headers != null)
//				curl_slist_free_all(headers);
//			headers = null;
			
//			host[0]	= '\0';
//			
//			if (curl_handle == null)
//				Initialize();
//			
//			if (referer != "")
//				curl_easy_setopt(curl_handle, CURLOPT_REFERER, referer.c_str());
		}
		
		public function GetHost():String
		{
			return this.host;
		}
		
		
		//是否处于繁忙状态，如果是那么不可再调用Get或Post
		//iPhone平台也可通过此判断异步请求是否已经完成
		public function IsBusy():Boolean
		{
			return m_bIsBusy;
		}
		
		public function GetAsyncInfo():AsyncInfo
		{
			return m_sAsyncInfo;
		}
		
		
		public static function GetUrlHost( url:String, host:String ):uint
		{
			return 0;
		}
		
		private function AsyncThreadProc( puCmd:* ):*
		{
			return null;
		}
		
		private function ProgressReportProc( ptr:Object, dlTotal:Number, dlNow:Number, ulTotal:Number, ulNow:Number ):int
		{
			return 1;
		}
		
		
		private function Initialize():void
		{
			m_sAsyncInfo=new AsyncInfo();
			bUseHttpProxy	= false;
			bUseHttpsProxy  = false;
			host		= '\0';
			pResponsePage="";
			responsePageLen = 0;
			
			urlRequest=new URLRequest();
			
			
			/* init the curl session */
			//curl_handle = curl_easy_init();
//			curl_handle = CurlHandlePool::Instance()->GetHandle();
		
		}
		
		private function DoGetInternal( url:String, resp:CHttpClientResponse ):int
		{
			return 1;
		}
		
		private function DoPostInternal( url:String, postData:ByteArray, nPostDataSize:int, resp:CHttpClientResponse, formflag:Boolean=false ):int
		{
			return 1;
		}
		
		private function DoProgressReport( dlTotal:Number, dlNow:Number, ulTotal:Number, ulNow:Number ):int
		{
			return 1;
		}
		
		private function DuplicateStr( lpszValue:String ):String
		{
			return "";
		}
		
	}
}