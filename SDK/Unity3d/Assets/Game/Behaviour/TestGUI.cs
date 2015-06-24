using System.Collections.Generic;
using GameRanking.Pack;
using UnityEngine;

public class TestGUI : MonoBehaviour
{
    private List<RankData> rankList;
    //todo 启用自定的结构
    bool useCustomAction = true;
    private int index = 1;

    // Use this for initialization
    void Start()
    {
        if (useCustomAction)
        {
            Net.Instance.HeadFormater = new CustomHeadFormater();
        }

    }

    ActionParam GetReuqest1001()
    {
        if (Net.Instance.HeadFormater is CustomHeadFormater)
        {
            Request1001Pack requestPack = new Request1001Pack() { PageIndex = 1, PageSize = 20 };
            return  new ActionParam(requestPack);
        }
        var actionParam = new ActionParam();
        actionParam["PageIndex"] = "1";
        actionParam["PageSize"] = "20";
        return actionParam;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 500, 100));
        GUILayout.BeginHorizontal();
        // Now create any Controls you like, and they will be displayed with the custom Skin
        if (GUILayout.Button("Get ranking for Http"))
        {
            NetWriter.SetUrl("http://ph.scutgame.com/service.aspx");
            Net.Instance.Send((int)ActionType.RankSelect, OnRankingCallback, GetReuqest1001());
        }
        GUILayout.Space(20);
        // Any Controls created here will use the default Skin and not the custom Skin
        if (GUILayout.Button("Get ranking for Socket"))
        {
            NetWriter.SetUrl("ph.scutgame.com:9001");
            Net.Instance.Send((int)ActionType.RankSelect, OnRankingCallback, GetReuqest1001());
        }
        GUILayout.Space(20);
        if (GUILayout.Button("Add ranking for Socket"))
        {
            NetWriter.SetUrl("ph.scutgame.com:9001");
            var actionParam = new ActionParam(new RankData() { Score = 90, UserName = "Cocos" + index });
            index++;
            Net.Instance.Send((int)ActionType.RankAdd, OnRankAddCallback, actionParam);
        }

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        OnRankGUI();
    }


    private void OnRankGUI()
    {
        if (rankList == null) return;

        GUILayout.BeginArea(new Rect(20, 100, 200, 200));
        GUILayout.BeginHorizontal();
        GUILayout.Label("UserName", GUILayout.Width(100));
        GUILayout.Label("Score", GUILayout.Width(100));
        GUILayout.EndHorizontal();

        foreach (var data in rankList)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(data.UserName, GUILayout.Width(100));
            GUILayout.Label(data.Score.ToString(), GUILayout.Width(100));
            GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();
    }

    void OnRankingCallback(ActionResult actionResult)
    {
        Response1001Pack pack = actionResult.GetValue<Response1001Pack>();
        if (pack == null)
        {
            return;
        }
        rankList = pack.Items;
    }

    private void OnRankAddCallback(ActionResult actionResult)
    {
    }
}