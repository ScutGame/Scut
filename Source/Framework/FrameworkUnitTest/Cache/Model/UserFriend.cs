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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Model;

namespace FrameworkUnitTest.Cache.Model
{
    [Serializable]
    [ProtoContract]
    [EntityTable(CacheType.Dictionary, MyDataConfigger.DbKey)]
    public class UserFriendState : BaseEntity
    {
        [ProtoMember(1)]
        [EntityField(true)]
        public long UserId { get; set; }

        [ProtoMember(2)]
        [EntityField]
        public bool Enable { get; set; }

        protected override int GetIdentityId()
        {
            return (int) UserId;
        }
    }

    [Serializable]
    [ProtoContract]
    [EntityTable(CacheType.Dictionary, MyDataConfigger.DbKey)]
    public class UserFriend : BaseEntity
    {
        private string _name;
        private CacheList<FriendsData> _friends;
        private CacheList<FriendsData> _friends2;
        private CacheDictionary<int, FriendsData> _friendDict;

        public UserFriend()
        {
            Friends = new CacheList<FriendsData>();
            FriendDict = new CacheDictionary<int, FriendsData>();
        }

        [ProtoMember(1)]
        [EntityField(true)]
        [Cloneable]
        public long UserId { get; set; }

        [ProtoMember(2)]
        [EntityField]
        [Cloneable]
        public string Name
        {
            get { return _name; }
            set
            {
                if (!object.Equals(_name, value))
                {
                    _name = value;
                    base.BindAndChangeProperty(_name, "Name");
                }
            }
        }

        [ProtoMember(3)]
        [EntityField(true, ColumnDbType.LongText)]
        [Cloneable]
        public CacheList<FriendsData> Friends
        {
            get { return _friends; }
            set
            {
                if (!object.Equals(_friends, value))
                {
                    _friends = value;
                    base.BindAndChangeProperty(_friends, "Friends");
                }
            }
        }

        [ProtoMember(5)]
        [EntityField(true, ColumnDbType.LongText)]
        public CacheList<FriendsData> Friends2
        {
            get { return _friends2; }
            set
            {
                if (!object.Equals(_friends2, value))
                {
                    _friends2 = value;
                    base.BindAndChangeProperty(_friends2, "Friends2");
                }
            }
        }

        [ProtoMember(4)]
        [EntityField(true, ColumnDbType.LongText)]
        [Cloneable]
        public CacheDictionary<int, FriendsData> FriendDict
        {
            get { return _friendDict; }
            set
            {
                if (!object.Equals(_friendDict, value))
                {
                    _friendDict = value;
                    base.BindAndChangeProperty(_friendDict, "FriendDict");
                }
            }
        }

        /*
        private CacheList<FriendsData> _friends;
        private CacheDictionary<int, FriendsData> _friendDict;

        [ProtoMember(3)]
        [EntityField(true, ColumnDbType.LongText)]
        public CacheList<FriendsData> Friends
        {
            get { return _friends; }
            set
            {
                if (!object.Equals(this._friends, value))
                {
                    _friends = value;
                    base.BindAndChangeProperty(_friends, "Friends");
                }
            }
        }


        [ProtoMember(4)]
        [EntityField(true, ColumnDbType.LongText)]
        public CacheDictionary<int, FriendsData> FriendDict
        {
            get { return _friendDict; }
            set
            {
                if (!object.Equals(this._friendDict, value))
                {
                    _friendDict = value;
                    base.BindAndChangeProperty(_friendDict, "FriendDict");
                }
            }
        }*/
        public int[] Items { get; set; }
        public FriendsData[] ItemFriends { get; set; }

        public List<int> Lists { get; set; }
        public Queue<int> Queues { get; set; }
        public Dictionary<int, FriendsData> Dicts { get; set; }


        protected override int GetIdentityId()
        {
            return (int)UserId;
        }
    }
}
