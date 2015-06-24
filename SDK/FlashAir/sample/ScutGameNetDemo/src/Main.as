import ScutDataLogic.CNetReader;
import ScutDataLogic.CNetWriter;
import ScutDataLogic.FrameManager;
import ScutDataLogic.registerCallback;

import flash.events.Event;
import flash.events.MouseEvent;

public static var ZyReader:CNetReader = ScutDataLogic.CNetReader.getInstance();
public static var ZyWriter:CNetWriter = ScutDataLogic.CNetWriter.getInstance();

public static var EVENT_GET_RANK:String = "Event_Get_Rank";		
public static var EVENT_SUBMIT_RANK:String = "Event_Submit_Rank";		

private function GameInit():void
{
	// 游戏初始化  || 固定添加即可。
	//        1. 注册事件派发机制  
	//       2. 初始化FrameManager   
	Init();
	
	// 注册侦听事件（即通讯成功回调函数）
	registerCallback(EVENT_GET_RANK,onGetRankHandler);
	registerCallback(EVENT_SUBMIT_RANK,onSubmitHandler);
	
	// 清除侦听器 通常在场景退出后调用
	//			removeCallback(EVENT_GET_RANK,onGetRankHandler);
}

private function Init():void
{
	//注册派发事件
	ScutDataLogic.CDataRequest.Instance().RegisterLUACallBack(ScutDataLogic.DataDispatch.netDataDispatch);
	//强制调用框架管理器，以执行初始化操作
	FrameManager.Instance().init(this.stage);
	
	// 设置服务器URL。 只需设置一次。如有更改，重新设置即可。
	ScutDataLogic.CNetWriter.setUrl("http://ph.scutgame.com/service.aspx");
}

protected function onClickRequestRank(event:MouseEvent):void
{
	rankList.text = "数据加载中...";
	
	//写入协议字段
	ZyWriter.writeString("ActionId","1001");
	ZyWriter.writeString("PageIndex",rankPage.text);
	ZyWriter.writeString("PageSize","25");
	
	// 游戏发起请求 
	//parm: String  该协议名称（可自定义）
	ScutDataLogic.ZyExecRequest.request(EVENT_GET_RANK);
}

protected function onClickSubmit(event:MouseEvent):void
{
	if(nameLabel.text == ""){
		tipLabel.text = "名字不能为空";
		return;
	}
	if(scoreLabel.text == ""){
		tipLabel.text = "分数不能为空";
		return;
	}
	
	//写入协议字段
	ZyWriter.writeString("ActionId","1000");
	ZyWriter.writeString("UserName",nameLabel.text);
	ZyWriter.writeString("Score",scoreLabel.text);
	
	ScutDataLogic.ZyExecRequest.request(EVENT_SUBMIT_RANK);
}

protected function onGetRankHandler(event:Event):void
{
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
		}else{
			rankList.text = "没有这么多数据";
			return;
		}
	}
	
	var len:int = dataAry.length;
	var rankInfo:String = "";
	for(var i:int = 0;i<len;i++){
		rankInfo += "排行榜::第"+(25*(int(rankPage.text)-1)+(i+1))+"名是"+dataAry[i].UserName+",,分数为【"+dataAry[i].Score+"】\n";
	}
	rankList.text = rankInfo;
}
protected function onSubmitHandler(event:Event):void
{
	if(ZyReader.getResult() == 0){
		tipLabel.text = "分数提交成功了~~~";
		nameLabel.text = "";
		scoreLabel.text = "";
	}
}
