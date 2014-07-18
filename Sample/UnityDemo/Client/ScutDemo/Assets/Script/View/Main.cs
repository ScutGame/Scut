using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0,0,80,80));
        if (GUILayout.Button(GameSetting.Instance.RoleName, GUILayout.Width(80), GUILayout.Height(80)))
        {
            Debug.Log("show user info...");
            Net.Instance.Send((int)ActionType.World, OnWorldCallback, null);
        }
        GUILayout.EndArea();

    }

    private void OnWorldCallback(ActionResult obj)
    {
        Debug.Log("OnWorldCallback...");
    }
}
