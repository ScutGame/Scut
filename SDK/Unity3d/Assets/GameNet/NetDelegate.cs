using System;

/*
      data.ActionId;
      data.ErrorCode;
      data.ErrorMsg;
      data.Resonse;
      data.Resonse
*/
public delegate void INetCallback(ServerResponse.ResponseData data, object userdata);