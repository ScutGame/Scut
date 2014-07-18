using UnityEngine;
using System.Collections;

public class Regist : MonoBehaviour
{
    public GUIStyle SelectBoxStype;
    public GUIStyle UnSelectBoxStype;

    string roleName = "";
    private bool role1;
    private bool role2;
    int width = 400;
    int height = 300;

    /// <summary>
    /// 只被调用一次
    /// </summary>
    void Start()
    {
        SelectBoxStype = new GUIStyle() { alignment = TextAnchor.MiddleCenter, normal = new GUIStyleState() { textColor = Color.yellow } };
        UnSelectBoxStype = new GUIStyle() { alignment = TextAnchor.MiddleCenter };
    }

    /// <summary>
    /// 每一帧被调用多次,enabled=false时禁用
    /// </summary>
    void OnGUI()
    {
        var boxPos = new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height);
        GUILayout.BeginArea(boxPos, "", "box");
        GUILayout.Label("");
        GUILayout.BeginHorizontal();
        GUILayout.Space(40);
        if (GUILayout.RepeatButton("Role1", role1 ? SelectBoxStype : UnSelectBoxStype, GUILayout.Width(150), GUILayout.Height(150)))
        {
            role1 = true;
            role2 = false;
        }
        GUILayout.Space(10);
        if (GUILayout.RepeatButton("Role2", role2 ? SelectBoxStype : UnSelectBoxStype, GUILayout.Width(150), GUILayout.Height(150)))
        {
            role2 = true;
            role1 = false;
        }
        GUILayout.EndHorizontal();
        GUILayout.Label("");
        GUILayout.BeginHorizontal();
        GUILayout.Space(75);
        GUILayout.Label("Role Name:", GUILayout.Width(100));
        roleName = GUILayout.TextField(roleName, GUILayout.Width(150));
        GUILayout.EndHorizontal();
        GUILayout.Label("");
        GUILayout.BeginHorizontal();
        GUILayout.Space(125);
        if (GUILayout.Button("Create Role", GUILayout.Width(150)))
        {
            Debug.Log("role1:" + role1 + ",role2" + role2);
            ActionParam actionParam = new ActionParam();
            actionParam["roleName"] = roleName;
            actionParam["Sex"] = (role1 ? 0 : 1).ToString();
            actionParam["HeadID"] = role1 ? "role1" : "role2";
            Net.Instance.Send((int)ActionType.CreateRote, CreateRoteCallback, actionParam);
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void CreateRoteCallback(ActionResult actionResult)
    {
        if (actionResult != null)
        {
            Application.LoadLevelAsync("MainScene");
        }
    }
}
