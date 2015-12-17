/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FrameworkUnitTest.Cache.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Data;
using ZyGames.Framework.Model;
using ZyGames.Framework.Redis;

namespace FrameworkUnitTest.Cache
{
    [TestClass]
    public class JsonCacheUnitTest
    {
        [TestInitialize]
        public void Init()
        {
            var ser = new JsonCacheSerializer(Encoding.UTF8);
            ConfigManager.GetConfigger<MyDataConfigger>();
            RedisConnectionPool.Initialize(ser);
            DbConnectionProvider.Initialize();
            EntitySchemaSet.LoadAssembly(typeof(SingleData).Assembly);
            var setting = new CacheSetting();
            CacheFactory.Initialize(setting, ser);

        }

        #region redis
        [TestMethod]
        public void RedisConnect()
        {
            Assert.IsTrue(RedisConnectionPool.Ping("127.0.0.1"));
            Assert.IsTrue(RedisConnectionPool.CheckConnect());
        }

        [TestMethod]
        public void RedisTestConnect()
        {
            var date = DateTime.Now;
            var tasks = new Task[1000];
            for (int i = 0; i < tasks.Count(); i++)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    while (date.AddMinutes(1) > DateTime.Now)
                    {
                        RedisConnectionPool.Process(c => c.HLen("$GameServer.Model.GameUser"));
                        Thread.Sleep(100);
                    }
                });
            }
            Task.WaitAll(tasks);
        }
        [TestMethod]
        public void RedisPirpe()
        {
            RedisConnectionPool.ProcessPipeline(p =>
            {
                p.QueueCommand(c => c.Set("_test", "1"));
                p.Flush();
            });

            RedisConnectionPool.ProcessPipeline(client =>
            {
                Assert.AreEqual(client.Get<string>("_test"), "1");
            }, p =>
            {
                long result = 0;
                p.QueueCommand(c =>
                {
                    result = c.Set("_test", "2") ? 1 : 0;
                });
                p.Flush();
                return result;
            });
        }

        [TestMethod]
        public void FindRedis()
        {
            RedisConnectionPool.Process(cli =>
            {

                /*
                var watch = Stopwatch.StartNew();
                key = "$FrameworkUnitTest.Cache.Model.UserKeyData";//"$FrameworkUnitTest.Cache.Model.QuestProcess";//
                //var keys = cli.HKeys(key);
                //Trace.WriteLine("load time:" + watch.ElapsedMilliseconds + "ms." + "keys:" + keys.Length);
                //var valueBytes = cli.HGet(key, keys);
                //Trace.WriteLine("load time:" + watch.ElapsedMilliseconds + "ms." + "vals:" + valueBytes.Length);
                byte[] suBytes = Encoding.Default.GetBytes("1380000");
                var script = @"
                    local userId = KEYS[2]
                    local key = KEYS[1]
                    local keyBytes = redis.call('HKeys',key)
                    print('keys:'.. table.getn(keyBytes))
                    local valueBytes = {}
                    for i,k in pairs(keyBytes) do
                        if string.find(k, userId) then
                            local val = redis.call('HGet',key, k)
                            table.insert(valueBytes, val)
                        end
                    end
                    return valueBytes
                ";
                var keybytes = Encoding.Default.GetBytes(key);
                var valueBytes = cli.Eval(script, 2, keybytes, suBytes);
                Trace.WriteLine("load time:" + watch.ElapsedMilliseconds + "ms." + "vals:" + (valueBytes.Length > 0 && valueBytes[0] != null ? Encoding.Default.GetString(valueBytes[0]) + "count:" + valueBytes.Length : "count:" + valueBytes.Length));
            
                 */
            });
        }

        [TestMethod]
        public void RedisListTest()
        {
            var watch = Stopwatch.StartNew();
            var keys = new List<string> { "__List:1000:1", "__List:1000:2" };
            var script = @"
local result={}
local index = 0
local len = table.getn(KEYS)
for i=1, len do
    local key = KEYS[i]
    local values = redis.call('lrange',key,0,-1)
    local l = table.getn(values)
    result[i] = table.concat(values,',')
end
return result
";
            List<string> valueList = null;
            RedisConnectionPool.Process(c =>
            {
                valueList = c.ExecLuaAsList(script, keys.ToArray(), new string[0]);
            });
            Trace.WriteLine("value count:" + valueList.Count + "\r\n" + string.Join("\r\n", valueList));

            WaitEnd(watch);
        }

        #endregion

        #region cache
        [TestMethod]
        public void AddFieldAll()
        {
            var watch = Stopwatch.StartNew();
            var cache = new ShareCacheStruct<FieldTypeData>();
            var t = cache.FindKey(1);
            if (t != null)
            {

            }
            Assert.IsTrue(cache.Add(new FieldTypeData()
            {
                Id = cache.GetNextNo(),
                FieldBool = true,
                FieldByte = 98,
                FieldBytes = new byte[] { 100, 102 },
                FieldDateTime = DateTime.Now,
                FieldDecimal = 1.0001m,
                FieldDouble = 1.000001d,
                FieldFloat = 1.0000001f,
                FieldGuid = Guid.NewGuid(),
                FieldInt = -1000001,
                FieldShort = -2555,
                FieldStr = "hello",
                FieldUint = 10000,
                FieldUlong = 100000000,
                FieldUshort = 6222,
            }));

            WaitEnd(watch);
        }

        [TestMethod]
        public void AddSingleField()
        {
            var watch = Stopwatch.StartNew();
            var cache = new ShareCacheStruct<SingleData>();
            Assert.IsTrue(cache.Add(new SingleData() { ChildId = 1 }));
            WaitEnd(watch);
        }

        [TestMethod]
        public void AddExtendField()
        {
            var watch = Stopwatch.StartNew();
            var cache = new ShareCacheStruct<ChildData>();
            Assert.IsTrue(cache.Add(new ChildData() { ChildId = 1, Age = 20 }));
            cache.UnLoad();
            Assert.IsNotNull(cache.FindKey(1));

            WaitEnd(watch);
        }

        [TestMethod]
        public void AddMail()
        {
            var watch = Stopwatch.StartNew();
            int userId = -1000;

            var cache = new PersonalCacheStruct<UserMail>();
            Assert.IsTrue(cache.Add(new UserMail() { UserId = userId, MailId = Guid.NewGuid() }));

            WaitEnd(watch);
        }


        [TestMethod]
        public void RemoveAndLoadMail()
        {
            var watch = Stopwatch.StartNew();
            int userId = -1000;

            var list = PersonalCacheStruct.TakeOrLoad<UserMail>(userId.ToString());
            var keys = new List<string>();
            foreach (var userMail in list)
            {
                string key = userMail.GetKeyCode();
                Trace.WriteLine("create id:" + key);
                keys.Add(key);
            }
            CacheFactory.RemoveToDatabase(new KeyValuePair<Type, IList<string>>(typeof(UserMail), keys));

            var cache = new PersonalCacheStruct<UserMail>();
            cache.UnLoad();

            list = PersonalCacheStruct.TakeOrLoad<UserMail>(userId.ToString());
            Trace.WriteLine("Count:" + keys.Count + ", reLoad count:" + list.Count());
            Assert.IsTrue(list.Count() == keys.Count);
            WaitEnd(watch);

        }

        [TestMethod]
        public void ChangedKeyOfChar()
        {
            var watch = Stopwatch.StartNew();
            int fuserId = 1000;
            var cache = new PersonalCacheStruct<UserFriend>();
            long userId = -cache.GetNextNo();
            var t = PersonalCacheStruct.Get<UserFriend>(userId.ToString(), true);
            if (t == null)
            {
                Assert.IsTrue(cache.Add(new UserFriend() { UserId = userId, Name = "kaikai" }));
                Trace.WriteLine("create id:" + userId);
            }
            Assert.IsNotNull(t = cache.Find(userId.ToString(), m => true));
            Assert.IsNotNull(cache.FindAll(userId.ToString(), m => true));
            Assert.IsNotNull(cache.FindKey(userId.ToString()));

            t.Friends.Add(new FriendsData() { UserId = fuserId, Num = 1 });
            t.FriendDict.Add(fuserId, new FriendsData() { UserId = fuserId, Num = 1 });
            UserFriend entity;
            Assert.IsTrue(cache.TryFindKey(userId.ToString(), out entity) == LoadingStatus.Success);
            Assert.IsNotNull(cache.TryFind(userId.ToString(), m => true, out entity) == LoadingStatus.Success);
            cache.ReLoad(userId.ToString());
            Assert.IsTrue(cache.ChildrenItem.Any());

            WaitEnd(watch);
        }

        [TestMethod]
        public void FindChangedCache()
        {
            var watch = Stopwatch.StartNew();
            int userId = 1001;
            var cache = new PersonalCacheStruct<UserFriend>();
            UserFriend Jim = cache.FindKey(userId.ToString());
            if (Jim == null)
            {
                Jim = new UserFriend() { UserId = userId, Friends2 = new CacheList<FriendsData>() };
                Jim.Friends2.Add(new FriendsData() { UserId = 1000 });
                Assert.IsTrue(cache.AddOrUpdate(Jim));
            }
            else
            {
                Assert.IsFalse(Jim.HasChanged);
                Assert.IsTrue(Jim.IsInCache);
            }

            foreach (var f in Jim.Friends2)
            {
                f.Num++;
            }

            Assert.IsTrue(Jim.HasChanged);
            Assert.IsTrue(Jim.IsInCache);
            WaitEnd(watch);
        }


        [TestMethod]
        public void ChangedCache()
        {
            var watch = Stopwatch.StartNew();
            int fuserId = 1000;
            var cache = new PersonalCacheStruct<UserFriend>();
            long userId = cache.GetNextNo();
            cache.FindKey(userId.ToString());

            UserFriend Jim = PersonalCacheStruct.GetOrAdd<UserFriend>(userId.ToString(), new Lazy<UserFriend>(() =>
            {
                var f = new UserFriend() { UserId = userId, Name = "Jim" };
                f.Friends.Add(new FriendsData() { UserId = fuserId, Num = 1 });
                f.FriendDict.Add(fuserId, new FriendsData() { UserId = fuserId, Num = 1 });
                return f;
            }));
            //Jim.SetChangeProperty("Name");
            Assert.IsTrue(cache.AddOrUpdate(Jim));
            Assert.IsTrue(Jim.IsInCache);

            var temp = cache.FindKey(userId.ToString());
            FriendsData fdata = null;
            //list
            fdata = temp.Friends[0];
            if (fdata != null)
            {
                Trace.WriteLine("num:" + fdata.Num);
                fdata.Num++;
                Assert.IsTrue(Jim.Friends.HasChanged, "list error");
                Assert.IsTrue(Jim.HasChangePropertys, "list error");
            }
            //dict
            if (temp.FriendDict.TryGetValue(fuserId, out fdata))
            {
                fdata.Num++;
                Assert.IsTrue(Jim.FriendDict.HasChanged, "dict error");
                Assert.IsTrue(Jim.HasChangePropertys, "entity error");
            }
            WaitEnd(watch);
        }


        [TestMethod]
        public void AddCache()
        {
            var watch = Stopwatch.StartNew();
            var cache = new PersonalCacheStruct<UserFriend>();
            int count = cache.ChildrenItem.Length;
            var Jim = new UserFriend() { UserId = cache.GetNextNo(), Name = "Jim" };
            var Tom = new UserFriend() { UserId = cache.GetNextNo(), Name = "Tom" };
            var Amriy = new UserFriend() { UserId = cache.GetNextNo(), Name = "Amriy" };
            var Cathy = new UserFriend() { UserId = cache.GetNextNo(), Name = "Cathy" };

            Jim.Friends.Add(new FriendsData() { UserId = Tom.UserId });
            Jim.Friends.Add(new FriendsData() { UserId = Amriy.UserId });
            Jim.Friends.Add(new FriendsData() { UserId = Cathy.UserId });

            Tom.Friends.Add(new FriendsData() { UserId = Jim.UserId });

            Amriy.Friends.Add(new FriendsData() { UserId = Jim.UserId });
            Amriy.Friends.Add(new FriendsData() { UserId = Cathy.UserId });

            Cathy.Friends.Add(new FriendsData() { UserId = Jim.UserId });
            Cathy.Friends.Add(new FriendsData() { UserId = Amriy.UserId });
            Trace.WriteLine("Jim:" + Jim.GetHashCode());
            Assert.IsTrue(cache.Add(Jim));
            UserFriend friend;
            Assert.IsNotNull((friend = cache.FindKey(Jim.PersonalId)));
            Trace.WriteLine("cache Jim:" + friend.GetHashCode());
            //Assert.IsTrue(cache.Add(Jim));
            Assert.AreEqual(friend, Jim);
            Assert.IsTrue(cache.Add(Tom));
            Assert.IsTrue(cache.Add(Amriy));
            Assert.IsTrue(cache.Add(Cathy));
            Assert.AreEqual(cache.ChildrenItem.Length - count, 4);
            //WaitEnd();

            Jim.Name = "JimCopy";
            Assert.IsTrue(Jim.HasChanged);
            cache.ReLoad();
            Assert.IsFalse(Jim.HasChanged);
            Assert.IsNotNull((friend = cache.FindKey(Jim.PersonalId, Jim.UserId)));
            Assert.AreEqual("JimCopy", friend.Name);
            Assert.IsNotNull(cache.FindKey(Tom.PersonalId, Tom.UserId));
            Assert.IsNotNull(cache.FindKey(Amriy.PersonalId, Amriy.UserId));
            Assert.IsNotNull(cache.FindKey(Cathy.PersonalId, Cathy.UserId));

            WaitEnd(watch);
        }

        [TestMethod]
        public void UnLoad()
        {
            var watch = Stopwatch.StartNew();
            var cache = new ShareCacheStruct<SingleData>();
            cache.UnLoad();
            for (int i = 0; i < 10; i++)
            {
                cache.AddOrUpdate(new SingleData() { ChildId = cache.GetNextNo().ToInt() });
                Thread.Sleep(100);
            }
            WaitEnd(watch);
        }

        [TestMethod]
        public void AddandGet()
        {
            var watch = Stopwatch.StartNew();
            int userId = 138001;
            var cache = new ShareCacheStruct<KeyData>();
            cache.AddOrUpdate(new KeyData() { Key = userId.ToString(), Value = "aa" });
            var list = cache.FindKey(userId.ToString());
            Assert.IsNotNull(list);

            var key = userId + cache.GetNextNo();
            cache.Add(new KeyData() { Key = key.ToString(), Value = "aa" });
            Assert.IsNotNull(cache.FindKey(key.ToString()));

            key = userId + cache.GetNextNo();
            cache.AddOrUpdate(new KeyData() { Key = key.ToString(), Value = "aa" });
            Assert.IsNotNull(cache.FindKey(key.ToString()));
            WaitEnd(watch);
        }

        [TestMethod]
        public void GetMutilTakeOrLoad()
        {
            var watch = Stopwatch.StartNew();
            var keys = new string[] { "4", "5", "7", "8" };
            int index = 0;
            foreach (var e in PersonalCacheStruct.TakeOrLoad<UserFriend>(keys))
            {
                Assert.AreEqual(e.PersonalId, keys[index]);
                index++;
            }

            keys = new string[] { "5", "9", "7", "10" };
            index = 0;
            foreach (var e in PersonalCacheStruct.TakeOrLoad<UserFriend>(keys))
            {
                Assert.AreEqual(e.PersonalId, keys[index]);
                index++;
            }

            WaitEnd(watch);
        }


        [TestMethod]
        public void GetAndAddOfMutiKey()
        {
            var watch = Stopwatch.StartNew();
            int userId = 138001;
            var cache = new PersonalCacheStruct<UserKeyData>();

            cache.AddOrUpdate(new UserKeyData() { UserId = userId, Key = "4", Value = "aa" });
            var list = cache.FindAll(userId.ToString());
            Assert.IsTrue(list.Count > 0);
            Trace.WriteLine("data count:" + list.Count);

            var key = userId + cache.GetNextNo();
            cache.Add(new UserKeyData() { UserId = key.ToInt(), Key = "4", Value = "aa" });
            Assert.IsNotNull(cache.FindKey(key.ToString(), key, "4"));
            WaitEnd(watch);
        }

        [TestMethod]
        public void GetCacheKey()
        {
            var watch = Stopwatch.StartNew();
            int userId = 1157;
            CheckInitData(userId.ToString(), () => new UserKeyData() { UserId = userId, Key = "111" }, userId, "111");
            var time = watch.ElapsedMilliseconds;
            Trace.WriteLine("process run time:" + time + "ms");
            Assert.IsTrue(time < 80);

            new PersonalCacheStruct<UserKeyData>().UnLoad();
            watch.Restart();
            userId = 1158;
            CheckInitData(userId.ToString(), () => new UserKeyData() { UserId = userId, Key = "111" }, userId, "111");
            time = watch.ElapsedMilliseconds;
            Trace.WriteLine("unload process run time:" + time + "ms");
            Assert.IsTrue(time < 10);
        }

        private T CheckInitData<T>(string persId, Func<T> createFactory, params object[] keys) where T : BaseEntity, new()
        {
            return PersonalCacheStruct.GetOrAdd(persId, new Lazy<T>(createFactory, true), keys);
        }

        [TestMethod]
        public void AddProcessCache()
        {
            var watch = Stopwatch.StartNew();
            int userId = 1157;
            int questIdx = 345;
            var questProcessCache = new ShareCacheStruct<QuestProcess>();

            var list = new List<QuestProcess>();
            int repNum = 0;
            long no = questProcessCache.GetNextNo(true);
            Trace.WriteLine("running time:" + watch.ElapsedMilliseconds);
            uint count = 20000;
            for (int i = 0; i < count; i++)
            {
                QuestProcess qProcess = new QuestProcess();
                qProcess.Suoyin = Convert.ToInt32(no);
                qProcess.Character = userId;
                qProcess.QuestIndex = questIdx;
                qProcess.RegisterDate = DateTime.Now;

                if (qProcess.Suoyin >= 10000 && qProcess.Suoyin < 10001)
                {
                    Trace.WriteLine("GetNextNo:" + no);
                    Trace.WriteLine("json:" + JsonUtils.SerializeCustom(qProcess));
                    repNum++;
                }
                list.Add(qProcess);
                no++;
            }
            questProcessCache.SetNoAddCount(count);
            Trace.WriteLine("running time:" + watch.ElapsedMilliseconds);
            Assert.IsTrue(questProcessCache.AddRange(list));
            WaitEnd(watch);
            //Assert.IsTrue(repNum > 1);
        }


        [TestMethod]
        public void FindProcessCache()
        {
            var watch = Stopwatch.StartNew();
            var questProcessCache = new ShareCacheStruct<QuestProcess>();
            Trace.WriteLine(questProcessCache.ChildrenItem.Length);
            var t = questProcessCache.FindKey(10000);
            //Assert.IsTrue(watch.ElapsedMilliseconds < 10000, "load timeout:" + watch.ElapsedMilliseconds + "ms.");
            WaitEnd(watch);
            Trace.WriteLine("json:" + JsonUtils.SerializeCustom(t));
        }

        [TestMethod]
        public void FindCache()
        {
            var cache = new ShareCacheStruct<ChildData>();
            var result = cache.FindAll();
            Trace.WriteLine("child data count:" + result.Count);
            Assert.IsTrue(cache.LoadingStatus == LoadingStatus.Success, "ChildData load fail.");

            var friendCache = new PersonalCacheStruct<UserFriend>();
            friendCache.LoadFrom(t => true);
            Assert.IsTrue(!friendCache.IsEmpty, "UserFriend is null.");
        }

        [TestMethod]
        public void GetAllEntity()
        {
            PersonalCacheStruct.AddRange(new[] { new UserFriendState() { UserId = 2 } });
            int UserId = 1;
            PersonalCacheStruct.GetOrAdd(UserId.ToString(), new Lazy<UserFriendState>(() => new UserFriendState() { UserId = UserId }), UserId);
            UserFriend userFriend;
            UserFriendState userFriendState;
            PersonalCacheStruct.Get(UserId.ToString(), out userFriend, out userFriendState, true);
            Assert.IsTrue(userFriend != null && userFriendState != null, "UserFriend is null.");
        }


        [TestMethod]
        public void UpdateCache()
        {
            var cache = new ShareCacheStruct<ChildData>();
            var data = cache.FindKey(1);
            if (data == null)
            {
                data = new ChildData() { ChildId = 1, Age = 20 };
                Assert.IsTrue(cache.Add(data), "add cache faild.");
            }
            int age = data.Age;
            age++;
            data.Age = age;
            cache.Add(data);
            Assert.IsFalse(data.HasChanged);
            cache.UnLoad();
            data = cache.FindKey(1);
            Assert.IsTrue(data != null && data.Age == age, "update data fail.");
        }

        [TestMethod]
        public void RemoveeCache()
        {
            var watch = Stopwatch.StartNew();
            var cache = new ShareCacheStruct<ChildData>();
            int key = (int)cache.GetNextNo();
            var data = cache.FindKey(key);
            if (data == null)
            {
                data = new ChildData() { ChildId = key, Age = 20 };
                Assert.IsTrue(cache.Add(data), "add cache faild.");
            }
            Assert.IsTrue(cache.Delete(data), "delete cache faild");
            cache.UnLoad();
            data = cache.FindKey(key);
            Assert.IsTrue(data == null, "delete cache faild.");
            WaitEnd(watch);
        }

        [TestMethod]
        public void MemoryCache()
        {
            var watch = Stopwatch.StartNew();
            var cache = new MyMemoryCache();
            var data = new MemoryData()
            {
                Id = 1,
                Name = "Heo"
            };
            cache.Save(data);
            MemoryData temp;
            Assert.IsTrue(cache.TryGet(data.Id.ToString(), out temp));
            WaitEnd(watch);
        }

        [TestMethod]
        public void KeyCharTest()
        {
            var watch = Stopwatch.StartNew();
            string key = "Test_1_B|A@#-$~'%";
            var cache = new ShareCacheStruct<KeyData>();
            var data = ShareCacheStruct.Get<KeyData>(key, true);
            if (data == null)
            {
                cache.Add(new KeyData() { Key = key, Value = key });
                WaitEnd(watch);
                cache.UnLoad();
                data = ShareCacheStruct.Get<KeyData>(key, true);

                Assert.IsNotNull(data, "find key faild");
            }
            int userId = 1380000;
            UserKeyData userData;
            var userCache = new PersonalCacheStruct<UserKeyData>();
            if (userCache.TryFindKey(userId.ToString(), out userData, userId, key) == LoadingStatus.Success)
            {
                if (userData == null)
                {
                    userCache.Add(new UserKeyData() { UserId = userId, Key = key, Value = key });
                    WaitEnd(watch);
                    userCache.UnLoad();
                    userData = userCache.FindKey(userId.ToString(), userId, key);
                    Assert.IsNotNull(userData, "find key faild");
                }
            }

        }

        [TestMethod]
        public void MutilKeyLoadTest()
        {
            int userId = 1380001;
            var userCache = new PersonalCacheStruct<UserKeyData>();
            var key = userCache.GetNextNo();
            userCache.Add(new UserKeyData() { UserId = userId, Key = key.ToString(), Value = "test" });
            var list = userCache.FindAll(userId.ToString());
            int count = list.Count;
            Trace.WriteLine("find count:" + count);

            userCache.UnLoad();
            key = userCache.GetNextNo();
            userCache.Add(new UserKeyData() { UserId = userId, Key = key.ToString(), Value = "test" });
            list = userCache.FindAll(userId.ToString());
            int count2 = list.Count;
            Trace.WriteLine("find2 count:" + count2);
            Assert.IsTrue(count + 1 == count2);
            //PersonalCacheStruct.GetOrAdd(userId.ToString(), new Lazy<UserKeyData>(() => new UserKeyData() { UserId = userId, Key = key }), userId, key);
        }

        [TestMethod]
        public void MutilKeyAddTest()
        {
            var watch = Stopwatch.StartNew();
            int userId = 1380000;
            var userCache = new PersonalCacheStruct<UserKeyData>();
            var list = new List<UserKeyData>();
            for (int i = 0; i < 10000; i++)
            {
                list.Add(new UserKeyData() { UserId = userId, Key = userCache.GetNextNo().ToString(), Value = "test" });
            }
            Trace.WriteLine("init list time:" + watch.ElapsedMilliseconds + "ms.");
            Assert.IsTrue(userCache.AddRange(list));
            WaitEnd(watch);
        }

        [TestMethod]
        public void MutilEntityLoadTest()
        {
            int userId = 1380000;
            Task[] tasks = new Task[10];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    var watch = Stopwatch.StartNew();
                    var t = PersonalCacheStruct.Get<UserKeyData>(userId.ToString(), true, userId, 10017);
                    Trace.WriteLine("Running time:" + watch.ElapsedMilliseconds + "ms.");
                    Assert.IsNotNull(t);
                });
            }
            Task.WaitAll(tasks);
        }

        [TestMethod]
        public void MutilEntityKeyLoadTest()
        {
            var r = PersonalCacheStruct.TakeOrLoad<UserFriend>(new[] { "45", "46", "53", "54" });
            Assert.IsTrue(r.Count() == 4);
            r = PersonalCacheStruct.TakeOrLoad<UserFriend>(new[] { "45", "46", "53", "54" });
            Assert.IsTrue(r.Count() == 4);
            r = PersonalCacheStruct.TakeOrLoad<UserFriend>(new[] { "45", "46", "53", "66" });
            Assert.IsTrue(r.Count() == 4);
        }

        [TestMethod]
        public void CheckCompletedTest()
        {
            var keys = new[] { "testkey1", "testkey2" };
            var vals = new[] { "1", "2" };
            RedisConnectionPool.SetExpire(keys, vals, 300);
            RedisConnectionPool.Process(c =>
            {
                var values = new List<string>();
                using (var p = c.CreatePipeline())
                {
                    foreach (var key in keys)
                    {
                        string k = key;
                        p.QueueCommand(cli => cli.Get<string>(k), v =>
                        {
                            values.Add(v);
                        });
                    }
                    p.Flush();
                }
                Trace.WriteLine(string.Join(",", values));
            });
            var watch = Stopwatch.StartNew();
            var data = ShareCacheStruct.Get<ChildData>(1);
            data.Age++;
            bool result = CacheFactory.CheckCompleted();
            Trace.WriteLine("Queue result:" + result);
            WaitEnd(watch);
        }

        [TestMethod]
        public void ChatAddTest()
        {
            var watch = Stopwatch.StartNew();
            var cache = new RankCacheStruct<ChatData>();
            var addlist = new List<ChatData>();
            for (int i = 0; i < 10; i++)
            {
                var chat = new ChatData(ChatType.World);
                chat.Message = "hello" + i;
                chat.SendTime = DateTime.Now;
                chat.ExpiredTime = DateTime.Now.AddDays(1);
                chat.Score = cache.GetNextNo();
                addlist.Add(chat);
            }
            cache.AddRank(ChatType.World.ToString(), addlist.ToArray());
            WaitEnd(watch);
        }

        [TestMethod]
        public void ChatExchangeTest()
        {
            var watch = Stopwatch.StartNew();
            var cache = new RankCacheStruct<ChatData>();
            var list = cache.TakeRank(ChatType.World.ToString(), 50).ToList();
            var t1 = list[5];
            Trace.WriteLine("rank:" + list[5].Message);
            Assert.IsTrue(cache.ExchangeRank(ChatType.World.ToString(), list[list.Count - 1], list[5]));
            list = cache.TakeRank(ChatType.World.ToString(), 50).ToList();
            var t2 = list[5];
            Trace.WriteLine("rank:" + list[5].Message);
            Assert.AreNotEqual(t1, t2);
            WaitEnd(watch);
        }

        [TestMethod]
        public void ChatTest()
        {
            var watch = Stopwatch.StartNew();
            var cache = new RankCacheStruct<ChatData>();
            var list = cache.TakeRank(ChatType.World.ToString(), 50);
            Trace.WriteLine("list ms:" + watch.ElapsedMilliseconds);
            list = cache.TakeRank(ChatType.World.ToString(), 50);
            Trace.WriteLine("list count:" + list.Count());
            WaitEnd(watch);
        }

        [TestMethod]
        public void ChatRemoveest()
        {
            var watch = Stopwatch.StartNew();
            var cache = new RankCacheStruct<ChatData>();
            Assert.IsTrue(cache.RemoveRankByScore(ChatType.World.ToString(), 0, -1));
            WaitEnd(watch);
        }

        [TestMethod]
        public void TakeOrLoadCache()
        {
            long userId = 1002;
            var count = PersonalCacheStruct.TakeOrLoad<UserFriend>(userId.ToString()).Count();
            Trace.WriteLine("UserFriend count:" + count);

            PersonalCacheStruct.TakeOrLoad<UserFriend>(userId.ToString());

            count = ShareCacheStruct.TakeOrLoad<FieldTypeData>().Count();
            Trace.WriteLine("FieldTypeData count:" + count);
            ShareCacheStruct.TakeOrLoad<FieldTypeData>();

        }


        [TestMethod]
        public void LoadTimeoutCache()
        {
            var watch = Stopwatch.StartNew();
            long userId = 1002;
            PersonalCacheStruct.Load(userId.ToString(), typeof(UserFriend));
            PersonalCacheStruct.Load(userId.ToString(), typeof(UserFriend));
            Trace.WriteLine("Running time:" + watch.ElapsedMilliseconds + "ms.");

            var waitTimes = new[] { 10 * 1000, 60 * 1000, 300 * 1000, 600 * 1000 };
            for (int i = 0; i < waitTimes.Length; i++)
            {
                watch.Restart();
                UserFriend Jim = PersonalCacheStruct.Get<UserFriend>(userId.ToString());
                Trace.WriteLine("Running time:" + watch.ElapsedMilliseconds + "ms.");
                //Thread.Sleep(waitTimes[i]);
                Thread.Sleep(1000);
            }
        }

        private object clone(object source)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(stream);
            }
        }

        [TestMethod]
        public void CopyEntityAddCache()
        {
            var watch = Stopwatch.StartNew();
            long userId = 1002;
            var t = PersonalCacheStruct.GetOrAdd<UserFriend>(userId.ToString(), new Lazy<UserFriend>(() => new UserFriend() { UserId = userId }));
            t.Friends.Add(new FriendsData() { UserId = 1001, Num = 1 });
            t.FriendDict[1001] = new FriendsData() { UserId = 1001, Num = 1 };
            WaitEnd(watch);
        }


        [TestMethod]
        public void CopyEntityCache()
        {
            long userId = 1002;
            var t = PersonalCacheStruct.TakeOrLoad<UserFriend>(userId.ToString()).FirstOrDefault();
            t.Items = new[] { 1, 2, 3 };
            t.ItemFriends = new[] { new FriendsData() { UserId = 1, Num = 100 } };
            t.Queues = new Queue<int>(new[] { 1, 2, 3 });
            t.Lists = new List<int>() { 1, 2, 3 };
            t.Dicts = new Dictionary<int, FriendsData>() { { 1, new FriendsData() { UserId = 1, Num = 100 } }, { 2, new FriendsData() { UserId = 1, Num = 100 } } };

            UserFriend c = null;
            c = t.Clone<UserFriend>();
            var watch = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                c = t.Clone<UserFriend>();
            }
            Trace.WriteLine("Running time:" + watch.ElapsedMilliseconds + "ms.");

            watch.Restart();
            for (int i = 0; i < 100; i++)
            {
                c = (UserFriend)clone(t);
            }
            Trace.WriteLine("Running time:" + watch.ElapsedMilliseconds + "ms.");


            Assert.IsNotNull(c);
            Assert.AreNotSame(t, c);
            Assert.AreNotSame(t.Friends[0], c.Friends[0]);
            Assert.AreNotSame(t.ItemFriends[0], c.ItemFriends[0]);
            Assert.AreNotSame(t.FriendDict[1001], c.FriendDict[1001]);
            //var t = new SingleData(){ChildId = 100};
            //var c = t.Clone();

        }

        //[TestMethod]
        public void CheckTimeoutCache()
        {
            Trace.WriteLine("Wait long time start.");

            var waitTimes = new[] { 10 * 1000, 60 * 1000, 300 * 1000, 600 * 1000 };
            for (int i = 0; i < waitTimes.Length; i++)
            {
                var watch = Stopwatch.StartNew();
                int fuserId = 1000;
                long userId = 1002;
                UserFriend Jim = PersonalCacheStruct.GetOrAdd<UserFriend>(userId.ToString(), new Lazy<UserFriend>(() =>
                {
                    var f = new UserFriend() { UserId = userId, Name = "Jim" };
                    f.Friends.Add(new FriendsData() { UserId = fuserId, Num = 1 });
                    f.FriendDict.Add(fuserId, new FriendsData() { UserId = fuserId, Num = 1 });
                    return f;
                }));

                Trace.WriteLine("Running time:" + watch.ElapsedMilliseconds + "ms.");
                Thread.Sleep(waitTimes[i]);
            }
        }

        [TestMethod]
        public void CacheRemAndAdd()
        {
            var list = new CacheList<UserFriend>();
            list.AddRange(new[]
            {
                new UserFriend(){UserId = 1001}, 
                new UserFriend(){UserId = 1002}, 
            });

            var temp = list.ToArray();
            list.Clear();
            list.AddRange(temp);

        }

        [TestMethod]
        public void RankTest()
        {
            var cache = new RankCacheStruct<LvRank>();
            LvRank rank = new LvRank() { UserId = 10000, Score = 100 };
            Assert.IsTrue(cache.AddRank(rank));
            Assert.IsTrue(cache.RemoveRankByScore(LvRank.Lv, 0, null));


            for (int i = 0; i < 100; i++)
            {
                LvRank lvRank = new LvRank() { UserId = 10001 + i, Score = i + 1 };
                int no = (int)cache.GetRankNo(lvRank.Key, lvRank);
                if (no < 1)
                {
                    Assert.IsTrue(cache.AddRank(lvRank));
                }
            }

            int count;
            Assert.IsTrue(cache.TryGetRankCount(LvRank.Lv, out count));
            Assert.IsTrue(count == 100);

            var userList = cache.TakeRank(LvRank.Lv, 3).ToList();
            Trace.WriteLine("Top 3:" + string.Join(",", userList.Select(t => t.UserId)));
            Assert.IsTrue(userList[0].UserId == 10100);

            Assert.IsTrue(cache.ExchangeRank(LvRank.Lv, userList[0], userList[1]));
            userList = cache.TakeRank(LvRank.Lv, 3).ToList();
            Assert.IsTrue(userList[0].UserId == 10099);
            Assert.IsTrue(cache.ExchangeRank(LvRank.Lv, userList[0], userList[1]));
            userList = cache.TakeRank(LvRank.Lv, 3).ToList();
            Assert.IsTrue(userList[0].UserId == 10100);

            var three = userList[userList.Count - 1];
            three.Score = 101;
            cache.Sort(LvRank.Lv, three);
            userList = cache.TakeRank(LvRank.Lv, 3).ToList();
            Assert.IsTrue(userList[0].UserId == 10098);
            Trace.WriteLine("10100 RankNo:" + cache.GetRankNo(LvRank.Lv, t => t.UserId == 10100).FirstOrDefault());
            three.Score = 98;
            cache.Sort(LvRank.Lv, three);
            //clear
            foreach (var item in cache.TakeRank(LvRank.Lv))
            {
                cache.Remove(item);
            }

        }

        //todo: 单独测试缓存过期临界点加载数据问题
        //[TestMethod]
        /*public void CacheExpired()
        {
            //测试时间5分钟
            long userId = 1002;
            var date = DateTime.Now;
            EntitySchemaSet.Get<UserFriend>().PeriodTime = 1;
            EntitySchemaSet.Get<KeyData>().PeriodTime = 1;
                
            var min = 5;
            var task1 = Task.Factory.StartNew(() =>
            {
                while (date.AddMinutes(min) > DateTime.Now)
                {
                    UserFriend Jim = PersonalCacheStruct.Get<UserFriend>(userId.ToString());
                    Jim.Friends[0].Num++;
                    Thread.Sleep(1003);
                }
            });
            var task2 = Task.Factory.StartNew(() =>
            {
                int l = 0;
                while (date.AddMinutes(min) > DateTime.Now)
                {
                    var t = ShareCacheStruct.Get<KeyData>("138003");
                    Assert.IsNotNull(t);
                    if (l % 4 == 0)
                    {
                        t = ShareCacheStruct.Get<KeyData>("138002");
                        Assert.IsNotNull(t);

                    }
                    var info = ShareCacheStruct.Get<DataInfo>(1);

                    Thread.Sleep(1003);
                    l++;
                }
            });

            var task3 = Task.Factory.StartNew(() =>
            {
                while (date.AddMinutes(min) > DateTime.Now)
                {
                    CacheFactory.TestDisposeCache();
                    Thread.Sleep(100);
                }
            });
            Task.WaitAll(task1, task2, task3);
        }*/



        #endregion

        #region sql

        [TestMethod]
        public void SendToDb()
        {
            int id = 1380001;
            var watch = Stopwatch.StartNew();
            for (int i = 0; i < 10; i++)
            {
                var log = new LoginLog();
                log.UserId = id + i;
                log.NickName = "cocos";
                log.RoleLv = 1;
                log.RetailId = "000";
                log.CreateTime = DateTime.Now;

                DataSyncQueueManager.SendToDb(log);
                Trace.WriteLine("Running time:" + watch.ElapsedMilliseconds + "ms.");
            }
            WaitEnd(watch);
        }

        [TestMethod]
        public void DbTest()
        {
            var dbProvider = DbConnectionProvider.FindFirst().Value;
            var cmd = dbProvider.CreateCommandStruct("Table", CommandMode.Delete);
            cmd.Filter = dbProvider.CreateCommandFilter();
        }

        [TestMethod]
        public void DbSelectInTest()
        {
            var dbProvider = DbConnectionProvider.FindFirst().Value;
            var cmd = dbProvider.CreateCommandStruct("userfriend", CommandMode.Inquiry, "Name");
            cmd.Filter.Condition = cmd.Filter.FormatExpressionByIn("UserId", 13, 14, 15);
            var names = new List<string>();
            using (var dr = dbProvider.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    names.Add(dr["Name"].ToString());
                }
            }
            Assert.IsTrue(names.Count > 0);
        }

        #endregion

        [TestCleanup]
        public void Cleanup()
        {
        }

        private void WaitEnd(Stopwatch watch, int ms = 2000)
        {
            Trace.WriteLine("Running time:" + watch.ElapsedMilliseconds + "ms.");
            do
            {
                Thread.Sleep(ms);
            } while (!DataSyncQueueManager.IsRunCompleted);
        }

    }
}
