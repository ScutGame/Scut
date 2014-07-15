using System.Collections.Generic;
using GameRanking.Pack;
using UnityEngine;

public class TestGUI : MonoBehaviour
{
    private List<RankData> rankList;
    // Use this for initialization
    void Start()
    {
        //todo 启用自定的结构
        Net.Instance.HeadFormater = new CustomHeadFormater();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {

        // Now create any Controls you like, and they will be displayed with the custom Skin
        if (GUILayout.Button("Click Http"))
        {
            //NetWriter.SetUrl("http://127.0.0.1:8036/service.aspx");
            NetWriter.SetUrl("http://ph.scutgame.com/service.aspx");
            Net.Instance.Send((int)ActionType.RankSelect, OnRankingCallback, null);
        }

        // Any Controls created here will use the default Skin and not the custom Skin
        if (GUILayout.Button("Click Socket"))
        {
            NetWriter.SetUrl("ph.scutgame.com:9001");
            Net.Instance.Send((int)ActionType.RankSelect, OnRankingCallback, null);
        }

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

    void OnRankingCallback(object responseData)
    {
        Response1001Pack pack = responseData as Response1001Pack;
        if (pack == null)
        {
            return;
        }
        rankList = pack.Items;
    }
}