using System;
using UnityEngine;

public class Login : MonoBehaviour
{
    string user = "";
    string pwd = "";
    /// <summary>
    /// 只被调用一次,变量声明
    /// </summary>
    void Awake()
    {

    }

    /// <summary>
    /// 只被调用一次
    /// </summary>
    void Start()
    {
        NetWriter.SetUrl("127.0.0.1:9001");
    }

    /// <summary>
    /// 每一帧调用
    /// </summary>
    void Update()
    {

    }

    /// <summary>
    /// 每一帧调用,处理Rigdibody时代替Update方法
    /// </summary>
    void FixedUpdate()
    {

    }

    /// <summary>
    /// 每一帧调用,Update方法全部执行完后执行
    /// </summary>
    void LateUpdate()
    {

    }

    /// <summary>
    /// 每一帧被调用多次,enabled=false时禁用
    /// </summary>
    void OnGUI()
    {
        int cwidth = Screen.width / 2;
        int cheight = Screen.height / 2;
        var boxPos = new Rect(cwidth - 110, cheight - 160, 240, 120);

        GUI.Box(boxPos, "");
        GUI.Label(new Rect(cwidth - 100, cheight - 150, 100, 22), "User:");
        user = GUI.TextField(new Rect(cwidth - 10, cheight - 150, 120, 22), user, 20);
        GUI.Label(new Rect(cwidth - 100, cheight - 120, 100, 22), "Password:");
        pwd = GUI.PasswordField(new Rect(cwidth - 10, cheight - 120, 120, 22), pwd, '*', 20);

        if (GUI.Button(new Rect(cwidth - 100, cheight - 80, 80, 22), "Regist"))
        {
            Net.Instance.Send((int)ActionType.Regist, RegistCallback, null);
        }

        if (GUI.Button(new Rect(cwidth, cheight - 80, 80, 22), "Login"))
        {
            GameSetting.Instance.Pid = user;
            GameSetting.Instance.Password = pwd;
            Net.Instance.Send((int)ActionType.Login, LoginCallback, null);
        }

    }

    private void RegistCallback(ActionResult actionResult)
    {
        if (actionResult != null)
        {
            user = actionResult.Get<string>("passportID");
            pwd = actionResult.Get<string>("password");
        }
    }

    private void LoginCallback(ActionResult actionResult)
    {
        if (actionResult != null && actionResult.Get<int>("GuideID") == (int)ActionType.CreateRote)
        {
            Application.LoadLevelAsync("RoleScene");
            return;
        }
       Application.LoadLevelAsync("MainScene");
    }

    /// <summary>
    /// 不销毁对象,场景切换时对象依然存在.
    /// </summary>
    void DontDestroyOnLoad()
    {

    }
}
