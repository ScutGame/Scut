using UnityEngine;
using System.Collections;

public class TestGUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void httpCallback(ServerResponse.ResponseData data, object userdata)
    {
        ServerResponse.Resopnse_1001 res = (ServerResponse.Resopnse_1001)data.Resonse;
		
		string ret = "Page " + res.PageCount;
		for(int i = 0; i <res.items.Count; i++)
		{
			ServerResponse.Item item = res.items[i];
			ret +=item.UserName;
			ret += ",";
			ret += item.Score;
			ret += ":";
		}
        Debug.Log("value" + ret);

    }
    void OnGUI () {
	
		
	    // Now create any Controls you like, and they will be displayed with the custom Skin
        if ( GUILayout.Button ("Click Http"))
        {
            NetWriter.SetUrl("http://ph.scutgame.com/service.aspx");
            NetWriter writer =  NetWriter.Instance;
            writer.writeString("PageIndex", "1");
            writer.writeInt32("PageSize", 10);
            Net.Instance.Request(1001,httpCallback,null );
        }
	   


	    // Any Controls created here will use the default Skin and not the custom Skin
	    if ( GUILayout.Button ("Click Socket"))
	    {
			
			///////
			Debug.Log("set url first");
			NetWriter.SetUrl("ddz.36you.net:9700");
            NetWriter writer =  NetWriter.Instance;
            writer.writeString("PageIndex", "1");
            writer.writeInt32("PageSize", 10);
            Net.Instance.Request(1001,httpCallback,null );
	    }
	    
    }


}
