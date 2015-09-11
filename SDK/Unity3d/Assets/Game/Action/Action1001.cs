using System;
using System.Collections.Generic;
using GameRanking.Pack;
using ZyGames.Framework.Common.Serialization;

public class Action1001 : BaseAction//GameAction
{
    private Response1001Pack _responseData;
    private bool isCustom;

    public Action1001()
        : base((int)ActionType.RankSelect)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {
        if (actionParam.HasValue)
        {
            //自定对象参数格式
            isCustom = true;
            Request1001Pack requestPack = actionParam.GetValue<Request1001Pack>();
            byte[] data = ProtoBufUtils.Serialize(requestPack);
            writer.SetBodyData(data);
        }
        else
        {
            isCustom = false;
            //默认url参数格式
            actionParam.Foreach((k, v) =>
            {
                writer.writeString(k, v.ToString());
                return true;
            });
        }
    }

    protected override void DecodePackage(NetReader reader)
    {
        if (reader != null && reader.StatusCode == 0)
        {
            if (isCustom)
            {
                //自定对象格式解包
                _responseData = ProtoBufUtils.Deserialize<Response1001Pack>(reader.Buffer);
            }
            else
            {
                //默认Scut流格式解包
                _responseData = new Response1001Pack();
                _responseData.PageCount = reader.getInt();
                int nNum = reader.getInt();
                _responseData.Items = new List<RankData>();
                for (int i = 0; i < nNum; i++)
                {
                    reader.recordBegin();
                    RankData item = new RankData();
                    item.UserName = reader.readString();
                    item.Score = reader.getInt();
                    reader.recordEnd();
                    _responseData.Items.Add(item);
                }
            }
        }
    }

    public override ActionResult GetResponseData()
    {
        if (_responseData != null)
        {
            UnityEngine.Debug.Log(string.Format("The action{0} receive ok, record count:{1}", ActionId, _responseData.Items == null ? 0 : _responseData.Items.Count));
        }
        return new ActionResult(_responseData);
    }
}
