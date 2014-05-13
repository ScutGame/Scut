using System;
using System.Collections.Generic;
using GameRanking.Pack;

public class Action1001 : GameAction
{
    private Response1001Pack _responseData;

    public Action1001()
        : base((int)ActionType.RankSelect)
    {
    }
    
    protected override void SendParameter(NetWriter writer, object userData)
    {
        writer.writeInt32("PageIndex", 1);
        writer.writeInt32("PageSize", 10);
    }

    protected override void DecodePackage(NetReader reader)
    {
        if (reader.StatusCode == 0)
        {
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

    protected override void Process(object userData)
    {
        if (_responseData != null)
        {
            UnityEngine.Debug.Log(string.Format("ok, count:{0}", _responseData.Items.Count));
        }
    }
}
