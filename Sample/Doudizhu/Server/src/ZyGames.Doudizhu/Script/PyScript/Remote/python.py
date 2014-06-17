""""python.py 执行PY脚本"""
import ReferenceLib
from System import *
from System.Collections.Generic import *
from ZyGames.Framework.Net import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Com.Rank import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Plugin.PythonScript import *
from ZyGames.Xuanyuan.Model import *
from ZyGames.Xuanyuan.Model.LogModel import *
from ZyGames.Xuanyuan.Model.DataModel import *
from ZyGames.Xuanyuan.Model.Config import *
from ZyGames.Xuanyuan.Model.Enum import *
from ZyGames.Xuanyuan.BLL import *
from ZyGames.Xuanyuan.BLL.OAService import *

def ExecutePython(httpGet, head, writer):
    pythonCode = httpGet.GetStringValue("pythonCode")
    pythonFunc = "Main"
    pyParams = List[PythonParam]()
    writer.PushIntoStack(MathUtils.ToNotNullString(PythonUtils.CallFunc(pythonFunc, "", pyParams, pythonCode)))