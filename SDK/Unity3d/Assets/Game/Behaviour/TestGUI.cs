using UnityEngine;

public class TestGUI : MonoBehaviour
{

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
            NetWriter.SetUrl("http://ph.scutgame.com/service.aspx");
            Net.Instance.Send((int)ActionType.RankSelect, null);
        }

        // Any Controls created here will use the default Skin and not the custom Skin
        if (GUILayout.Button("Click Socket"))
        {
            NetWriter.SetUrl("ph.scutgame.com:9001");
            Net.Instance.Send((int)ActionType.RankSelect, null);
        }
    }
}