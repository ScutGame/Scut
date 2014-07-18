using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameSetting : MonoBehaviour
{
    public const string ClientPasswordKey = "j6=9=1ac";
    private static GameSetting setting;

    static GameSetting()
    {
        setting = FindObjectOfType(typeof(GameSetting)) as GameSetting;
        if (setting == null)
        {
            GameObject obj2 = new GameObject("GameSetting");
            setting = obj2.AddComponent(typeof(GameSetting)) as GameSetting;
        }
        if (setting == null) return;

        setting.MobileType = 1;
        setting.DeviceID = "00-E1-4C-36-F5-C8";//更换会重新注册账号
        setting.ScreenX = 860;
        setting.ScreenY = 460;
        setting.RetailID = "0000";
        setting.GameID = 1;
        setting.ServerID = 1;
        setting.ClientAppVersion = "1.0";
    }

    public static GameSetting Instance { get { return setting; } }


    public string Pid { get; set; }

    public string Password { get; set; }

    public int MobileType { get; set; }
    public string DeviceID { get; set; }
    public int ScreenX { get; set; }
    public int ScreenY { get; set; }
    public string RetailID { get; set; }
    public int GameID { get; set; }
    public int ServerID { get; set; }
    public string ClientAppVersion { get; set; }

    public string RoleName { get; set; }
}
