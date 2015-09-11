using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scut.SMS.Config;
using ServiceStack.Redis;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Model;
using ZyGames.Framework.Net;
using ZyGames.Framework.Redis;

namespace Scut.SMS
{
    public partial class CacheUpdateQueueForm : BaseForm
    {
        private static string[] cacheRedisKeys = new string[] { "__GLOBAL_CHANGE_KEYS_NEW", "__GLOBAL_CHANGE_KEYS", "__QUEUE_REDIS_SYNC" };
        private readonly MainForm _parent;
        private long waitingCount;
        private long runningCount;
        private long currentIndex;
        private bool isContinue;
        private ManualResetEvent singal = new ManualResetEvent(false);
        private Thread subThread;

        public CacheUpdateQueueForm(MainForm parent)
        {
            isContinue = true;
            _parent = parent;
            InitializeComponent();
            subThread = new Thread(OnCheckComplete);
            subThread.Start();
            OnRefresh();
        }

        private void OnCheckComplete()
        {
            try
            {
                while (isContinue)
                {
                    singal.WaitOne();
                    int value = 0;
                    while (true)
                    {
                        if (value >= 100) break;
                        value = (int)(currentIndex * 100) / (runningCount == 0 ? 1 : (int)runningCount);
                        RefreshWorkBar(value);
                        Thread.Sleep(100);
                    }
                    singal.Reset();
                }
            }
            catch
            {
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            OnRefresh();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
                isContinue = false;
                singal.Set();
                singal.Dispose();
                subThread.Abort();
            }
            catch (Exception er)
            {
                ShowError(er.Message);
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            try
            {
                var dymincEntity = AppSetting.Current.Entity.DymincEntity;
                if (dymincEntity == null)
                {
                    ShowError("Not generating entity assembly!");
                    return;
                }
                btnExecute.Enabled = false;
                currentIndex = 0;
                singal.Set();
                this.backgroundWorker1.RunWorkerAsync();
                //btnExecute.Enabled = true;
            }
            catch (Exception er)
            {
                ShowError(er.Message);
            }
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _parent.ServerManager.TryExecute(client =>
                {
                    string key = cacheRedisKeys[1];
                    string setId = key + "_temp";
                    byte[][] buffers = client.ZRange(setId, 0, int.MaxValue);

                    if (buffers != null && buffers.Length > 0)
                    {
                        KeyValuePair<string, byte[]> pair;
                        foreach (var buffer in buffers)
                        {
                            try
                            {
                                pair = ProtoBufUtils.Deserialize<KeyValuePair<string, byte[]>>(buffer);
                                DoUpdateChangeKey(client, setId, pair.Key, pair.Value, false, buffer);
                            }
                            catch
                            {
                            }
                            currentIndex++;
                        }
                    }
                    //New key
                    key = cacheRedisKeys[0];
                    setId = key + "_temp";
                    byte[][] keyBytes = client.HKeys(setId);
                    if (keyBytes != null && keyBytes.Length > 0)
                    {
                        buffers = client.HMGet(setId, keyBytes);
                        int index = 0;
                        foreach (var buffer in buffers)
                        {
                            DoUpdateChangeKey(client, setId, keyBytes[index], buffer, true, null);
                            currentIndex++;
                            index++;
                        }
                    }
                    return true;
                });
            }
            finally
            {
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OnRefresh();
            btnExecute.Enabled = true;
        }

        private void RefreshWorkBar(int value)
        {
            try
            {
                progressBar1.Value = value;
                lblProcessBar.Text = string.Format("{0}%", value);
            }
            catch (Exception ex)
            {
            }
        }

        private void OnRefresh()
        {
            try
            {
                _parent.ServerManager.TryExecute(client =>
                {
                    string key = cacheRedisKeys[0];
                    string setId = key + "_temp";
                    byte[][] keyBytes = client.HKeys(setId);
                    if (keyBytes == null || keyBytes.Length <= 0)
                    {
                        try
                        {
                            if (client.ContainsKey(key))
                            {
                                client.Remove(setId);
                                client.Rename(key, setId);
                            }
                        }
                        catch { }
                    }

                    waitingCount = client.HLen(key);
                    runningCount = client.HLen(setId);

                    key = cacheRedisKeys[1];
                    setId = key + "_temp";
                    byte[][] buffers = client.ZRange(setId, 0, int.MaxValue);
                    if (buffers == null || buffers.Length == 0)
                    {
                        try
                        {
                            if (client.ContainsKey(key))
                            {
                                client.Remove(setId);
                                client.Rename(key, setId);
                                buffers = client.ZRange(setId, 0, int.MaxValue);
                            }
                        }
                        catch { }
                    }

                    waitingCount += client.ZRange(key, 0, int.MaxValue).Length;
                    if (buffers != null) runningCount += buffers.Length;
                    //hash queue key
                    key = cacheRedisKeys[2];
                    var keyList = client.SearchKeys(key + "*");
                    var waitQueueKeys = keyList.Where(p => !p.EndsWith("_temp")).ToArray();
                    var runQueueKeys = keyList.Where(p => p.EndsWith("_temp")).ToArray();
                    foreach (var waitQueueKey in waitQueueKeys)
                    {
                        waitingCount += client.HLen(waitQueueKey);
                    }
                    foreach (var queueKey in runQueueKeys)
                    {
                        runningCount += client.HLen(queueKey);
                    }

                    lblWaitingQueue.Text = waitingCount.ToString();
                    lblRunningQueue.Text = runningCount.ToString();
                    return true;
                });
            }
            catch (Exception er)
            {
                ShowError(er.Message);
            }
        }

        private void DoUpdateChangeKey(RedisClient client, string setId, object keyByte, byte[] values, bool isHash, byte[] buffer)
        {
            string key = isHash
                ? Encoding.UTF8.GetString((byte[])keyByte)
                : keyByte.ToString();
            dynamic entity = null;
            try
            {
                entity = CovertEntityObject(key, values);
                CacheType cacheType = CacheType.None;
                if (entity != null)
                {
                    SchemaTable schema;
                    Type entityType = entity.GetType();
                    if (!EntitySchemaSet.TryGet(entityType, out schema))
                    {
                        EntitySchemaSet.InitSchema(entityType);
                    }
                    if (schema != null || EntitySchemaSet.TryGet(entityType, out schema))
                    {
                        cacheType = schema.CacheType;
                    }
                    if (cacheType != CacheType.None)
                    {
                        string redisKey = cacheType == CacheType.Dictionary
                            ? key.Split('|')[0]
                            : key.Split('_')[0];

                        using (IDataSender sender = DataSyncManager.GetRedisSender(schema, redisKey))
                        {
                            sender.Send(entity);

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("ChangeKey:{0} error:{1}", key, ex);
            }
            if (isHash)
            {
                client.HDel(setId, (byte[])keyByte);
            }
            else
            {
                client.ZRem(setId, buffer);
            }
            if (entity != null)
            {
                Type type = entity.GetType();
                string entityKey = string.Format("{0},{1}", key, type.Assembly.GetName().Name);
                client.HSet("__GLOBAL_SQL_CHANGE_KEYS_NEW", Encoding.UTF8.GetBytes(entityKey), new byte[] { 1 });
            }

        }

        private dynamic CovertEntityObject(string key, byte[] value)
        {
            string typeName;
            string asmName;
            bool isEntityType;
            string redisKey;
            Type type = null;
            GetEntityTypeFromKey(key, out typeName, ref type, out asmName, out isEntityType, out redisKey);
            if (type != null)
            {
                try
                {
                    return ProtoBufUtils.Deserialize(value, type);
                }
                catch
                {
                    try
                    {
                        type = Type.GetType(string.Format("{0},{1}", typeName, asmName), false, true);
                        return ProtoBufUtils.Deserialize(value, type);
                    }
                    catch
                    {
                    }
                }
            }
            return null;
        }

        private static string GetEntityTypeFromKey(string key, out string typeName, ref Type type, out string asmName, out bool isEntityType, out string redisKey)
        {
            int index = key.IndexOf(',');
            var arr = (index > -1 ? key.Substring(0, index) : key).Split('_');
            typeName = arr[0];
            asmName = index == -1 ? "" : key.Substring(index + 1, key.Length - index - 1);
            string persionKey = string.Empty;
            string entityKey = string.Empty;
            if (arr.Length > 1)
            {
                entityKey = arr[1];
                var tempArr = entityKey.Split('|');
                if (tempArr.Length > 1)
                {
                    persionKey = tempArr[0];
                    entityKey = tempArr[1];
                }
            }
            isEntityType = false;
            if (string.IsNullOrEmpty(persionKey))
            {
                isEntityType = true;
                redisKey = string.Format("{0}_{1}", typeName, entityKey);
            }
            else
            {
                //私有类型
                redisKey = string.Format("{0}_{1}", typeName, persionKey);
            }
            string formatString = AppSetting.DictTypeNameFormat;
            if (isEntityType)
            {
                formatString = "{0},{1}";
            }
            if (type == null)
            {
                type = Type.GetType(string.Format(formatString, typeName, asmName), false, true);
                if (Equals(type, null))
                {
                    var enitityAsm = AppSetting.Current.Entity.DymincEntity;
                    if (enitityAsm != null)
                    {
                        asmName = enitityAsm.GetName().Name;
                        type = Type.GetType(string.Format(formatString, typeName, asmName), false, true);
                    }
                }
            }
            return entityKey;
        }
    }
}
