using System;
using System.Collections.Generic;
using GameRanking.Pack;
using ZyGames.Framework.Common.Serialization;

public class Action1001 : BaseAction//GameAction
{
    private Response1001Pack _responseData;
    private bool isCustom = true;//todo 启用自定的结构

    public Action1001()
        : base((int)ActionType.RankSelect)
    {
    }

    protected override void SendParameter(NetWriter writer, object userData)
    {
        if (isCustom)
        {
            //自定对象参数格式
            Request1001Pack requestPack = new Request1001Pack()
            {
                PageIndex = 1,
                PageSize = 10
            };
            byte[] data = ProtoBufUtils.Serialize(requestPack);
            writer.SetBodyData(data);
        }
        else
        {
            //默认url参数格式
            writer.writeInt32("PageIndex", 1);
            writer.writeInt32("PageSize", 10);
        }


    }

    protected override void DecodePackage(NetReader reader)
    {
        if (reader.StatusCode == 0)
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

    protected override void Process(object userData)
    {
        if (_responseData != null)
        {
            UnityEngine.Debug.Log(string.Format("ok, count:{0}", _responseData.PageCount));
        }
    }
}
