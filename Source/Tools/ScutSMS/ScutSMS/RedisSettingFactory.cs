using System;
using System.IO;
using System.Text;
using ScutServerManager.Config;
using ServiceStack.Text;
using ServiceStack.Text.Json;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;

namespace Scut.SMS
{
    internal static class RedisSettingFactory
    {
        private static readonly string ConfigPath;
        private static string _fileName;

        static RedisSettingFactory()
        {
            _fileName = "redisInfo.tmp";
            ConfigPath = Path.GetTempPath();
            ConfigPath = Path.Combine(ConfigPath, "ScutSM");
        }

        public static RedisSetting Load()
        {
            var tempPath = Path.Combine(ConfigPath, _fileName);
            RedisSetting setting = new RedisSetting();
            try
            {
                if (File.Exists(tempPath))
                {
                    byte[] data = File.ReadAllBytes(tempPath);
                    setting = ProtoBufUtils.Deserialize<RedisSetting>(data);
                }
                else
                {
                    Save(setting);
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("RedisSetting load error:{0}", ex);
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
            }
            return setting;
        }

        public static void Save(RedisSetting setting)
        {
            if (!Directory.Exists(ConfigPath))
            {
                Directory.CreateDirectory(ConfigPath);
            }
            var tempPath = Path.Combine(ConfigPath, _fileName);
            var buffer = ProtoBufUtils.Serialize(setting);
            using (var sw = File.Open(tempPath, FileMode.OpenOrCreate))
            {
                sw.Write(buffer, 0, buffer.Length);
                sw.Flush();
            }
        }

        private static string ParseBase64String(string data)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(data));
        }

        private static string ConvertBase64String(string data)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        }
    }
}