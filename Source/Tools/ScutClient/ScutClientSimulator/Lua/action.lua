
function Action1004(token)
   	ScutWriter:writeString("MobileType", token.MobileType)
    ScutWriter:writeString("Pid", token.Pid)
    ScutWriter:writeString("Pwd", token.Pwd)
    ScutWriter:writeString("DeviceID", token.DeviceID)
    ScutWriter:writeString("GameType", token.GameType)
    ScutWriter:writeString("ScreenX", "400")
    ScutWriter:writeString("ScreenY", "300")
    ScutWriter:writeString("RetailID", token.RetailID)
    ScutWriter:writeString("ServerID", token.ServerID)
    ScutWriter:writeString("RetailUser", "")
    ScutWriter:writeString("ClientAppVersion", "1.0")
    ScutWriter:writeString("Code","")

end

function _1004Callback(token)
    local DataTabel=nil

    if ScutReader:getResult() then
        DataTabel={}
        DataTabel.SessionID= ScutReader:readString()
        DataTabel.UserID= ScutReader:readString()
        DataTabel.UserType= ScutReader:getInt()
        DataTabel.LoginTime= ScutReader:readString()
        DataTabel.GuideID= ScutReader:getInt()
        DataTabel.PassportId= ScutReader:readString()
        DataTabel.RefeshToken= ScutReader:readString()
        DataTabel.QihooUserID= ScutReader:readString()
        DataTabel.Scope= ScutReader:readString()
        
        token.Uid = DataTabel.UserID
        token.Sid = DataTabel.SessionID
    else
        LogWriteLine("ÇëÇóAction:"..ScutReader:readAction()..",³ö´í:"..ScutReader:readErrorCode() .. "-"..ScutReader:readErrorMsg())
    end
    return DataTabel
end
