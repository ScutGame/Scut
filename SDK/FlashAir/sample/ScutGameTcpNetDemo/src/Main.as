/*********************************************************************************
 *			ScutGame TCP GameRank Demo
 * 
 *  连接socket  
 *  ScutDataLogic.CTcpClient.Ins().Connect(地址,端口,成功回调函数);
 *  ex: ScutDataLogic.CTcpClient.Ins().Connect("ph.scutgame.com",9001,onConnectHandler);
 * 
 * 
 * 	 发送请求
 * 	ScutDataLogic.ZyExecRequest.request(请求事件名称,服务器地址);
 * ex: ScutDataLogic.ZyExecRequest.request(EVENT_GET_RANK,"ph.scutgame.com:9001");
 * 
 *  
 * !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
 * TCP模式与HTTP模式差别只在于:
 * 
 * 1.要先连接socket
 * 2.request时第二个参数带上服务器地址
 *  
 *********************************************************************************/



import ScutDataLogic.CNetReader;
import ScutDataLogic.CNetWriter;
import ScutDataLogic.CTcpClient;
import ScutDataLogic.FrameManager;
import ScutDataLogic.registerCallback;

import flash.events.Event;
import flash.events.MouseEvent;

public static var ZyReader:CNetReader = ScutDataLogic.CNetReader.getInstance();
public static var ZyWriter:CNetWriter = ScutDataLogic.CNetWriter.getInstance();

public static var EVENT_GET_RANK:String = "Event_Get_Rank";

private function GameInit():void
{
	closeBtn.enabled = false;
	statusInfo.text += "Socket还没连接。\n";
	
	// 游戏初始化  || 固定添加即可。
	// 1. 注册事件派发机制  
	// 2. 初始化FrameManager   
	FrameWorkInit();
	
	// 注册侦听事件（即通讯成功回调函数）
	registerCallback(EVENT_GET_RANK,onGetRankHandler);
}

private function FrameWorkInit():void
{
	//注册派发事件
	ScutDataLogic.CDataRequest.Instance().RegisterLUACallBack(ScutDataLogic.DataDispatch.netDataDispatch);
	//强制调用框架管理器，以执行初始化操作
	FrameManager.Instance().init(this.stage);
	
	// 设置服务器URL。 只需设置一次。如有更改，重新设置即可。
	//HTTP模式需要设置服务器地址头
	//			ScutDataLogic.CNetWriter.setUrl("http://ph.scutgame.com/service.aspx");
	//TCP模式 需要启动连接
//	ScutDataLogic.CTcpClient.Ins().Connect("ph.scutgame.com",9001,onConnectHandler);
}


protected function onClickConnect(event:MouseEvent):void
{
	//游戏中一开始用这句话启动socket连接
	//parm   地址:String  端口:int  连接成功的回调函数:Function
	ScutDataLogic.CTcpClient.Ins().Connect("ph.scutgame.com",9001,onConnectHandler);
}

private function onConnectHandler():void
{
	connectBtn.enabled = false;
	closeBtn.enabled = true;
	statusInfo.text += "Socket连接成功!!\n";
	sendRequest();
}

protected function onClickClose(event:MouseEvent):void
{
	ScutDataLogic.CTcpClient.Ins().Close();
	closeBtn.enabled = false;
	connectBtn.enabled = true;
}

/**
 * 发送请求 
 */
private function sendRequest():void
{
	//写入协议字段
	ZyWriter.writeString("ActionId","1001");
	ZyWriter.writeString("PageIndex","1");
	ZyWriter.writeString("PageSize","10");
	// 游戏发起请求  tcp请求需要加上地址与端口
	ScutDataLogic.ZyExecRequest.request(EVENT_GET_RANK,"ph.scutgame.com:9001");
}

private var packageIndex:int = 0;

/**
 * 解包函数 
 * @param event
 * 
 */
protected function onGetRankHandler(event:Event):void
{
	packageIndex++;
	statusInfo.text += "成功接收【"+packageIndex+"】次排行榜数据\n";
	dataInfo.text += "第"+ packageIndex + "次数据====================\n";
	var ZyReader:CNetReader = CNetReader.getInstance();
	if(ZyReader.getResult() == 0){
		var pageCount:int = ZyReader.getInt();
		var RecordNums_1:int = ZyReader.getInt();
		var dataAry:Array = [];
		if(RecordNums_1 != 0){
			for(var k:int=0;k<RecordNums_1;k++){
				var mRecordTabel_1:Object = {};
				ZyReader.recordBegin()
				mRecordTabel_1.UserName= ZyReader.readString();
				mRecordTabel_1.Score= ZyReader.getInt()
				ZyReader.recordEnd();
				dataAry.push(mRecordTabel_1);
			}
		}
		var len:int = dataAry.length;
		for(var i:int = 0;i<len;i++){
			dataInfo.text += ("排行榜::第"+(i+1)+"名是"+dataAry[i].UserName+",,分数为["+dataAry[i].Score+"]\n");
		}
		dataInfo.text += "\n";
	}
}