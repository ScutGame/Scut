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
using ZyGames.Framework.Model;

namespace FrameworkUnitTest.Cache.Model
{
    [Serializable, ProtoContract]
    [EntityTable(CacheType.Entity, MyDataConfigger.DbKey)]
    public class FieldTypeData : ShareEntity
    {
        public FieldTypeData()
            : base(false)
        {
        }

        [ProtoMember(1)]
        [EntityField(true)]
        public long Id { get; set; }


        [ProtoMember(2)]
        [EntityField]
        public int FieldInt { get; set; }

        [ProtoMember(3)]
        [EntityField]
        public short FieldShort { get; set; }

        [ProtoMember(4)]
        [EntityField]
        public byte[] FieldBytes { get; set; }

        [ProtoMember(5)]
        [EntityField]
        public byte FieldByte { get; set; }

        [ProtoMember(6)]
        [EntityField]
        public Guid FieldGuid { get; set; }

        [ProtoMember(7)]
        [EntityField]
        public string FieldStr { get; set; }

        [ProtoMember(8)]
        [EntityField]
        public ulong FieldUlong { get; set; }

        [ProtoMember(9)]
        [EntityField]
        public uint FieldUint { get; set; }

        [ProtoMember(10)]
        [EntityField]
        public ushort FieldUshort { get; set; }


        [ProtoMember(11)]
        [EntityField]
        public DateTime FieldDateTime { get; set; }

        [ProtoMember(12)]
        [EntityField]
        public decimal FieldDecimal { get; set; }

        [ProtoMember(13)]
        [EntityField]
        public double FieldDouble { get; set; }

        [ProtoMember(14)]
        [EntityField]
        public float FieldFloat { get; set; }

        [ProtoMember(14)]
        [EntityField]
        public bool FieldBool { get; set; }
    }
}
