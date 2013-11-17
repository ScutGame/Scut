import clr, sys
clr.AddReference('ZyGames.Framework.Common');
clr.AddReference('ZyGames.Framework');
clr.AddReference('ZyGames.Framework.Game')
from ZyGames.Framework.Common.Log import *

class UrlParam():
    Result = True;
    Uid = 0;
    Sid = '';

def getUrlElement(httpGet):
    result = False;
    urlParam = UrlParam;
    result = httpGet.GetInt("Uid", 0);
    if result[0]:
        urlParam.Uid = result[1];
        result = httpGet.GetString("Pid", '');
        if result[0]:
            urlParam.Sid = result[1];
        else:
            urlParam.Result = False;
    else:
        urlParam.Result = False;
    return urlParam;


def takeAction(urlParam):
    #for key, value in urlParam.items():
    #    TraceLog.ReleaseWrite('{0}={1}',key,value);
    TraceLog.ReleaseWrite('1004 param Sid:{0},Uid:{1}', urlParam.Sid, urlParam.Uid);
    actionResult = ['he'];
    #处理结果存储在字典中
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    #输出
	return True;