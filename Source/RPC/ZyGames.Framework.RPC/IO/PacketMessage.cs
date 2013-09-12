using System;
using System.Text;
using ZyGames.Framework.RPC.Sockets;

namespace ZyGames.Framework.RPC.IO
{
    /// <summary>
    /// 包消息
    /// </summary>
    public class PacketMessage
    {
        /// <summary>
        /// 格式：head + ( len(4) + data)
        /// head: RequestType(1)+MsgType(1) + uid(4) + gameId(4) + serverId(4) + ssid(4+string)+ address(4+string)  + gzip(1)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static PacketMessage Parse(byte[] data)
        {
            var reader = new BufferReader(data);
            int length = reader.ReadInt32();
            var reqType = (ConnectType)reader.ReadByte();
            var msgType = (PacketMsgType)reader.ReadByte();
            PacketHead head = new PacketHead(reqType, msgType);
            head.Length = length;
            head.MsgId = reader.ReadInt32();
            head.Uid = reader.ReadInt32();
            head.GameId = reader.ReadInt32();
            head.ServerId = reader.ReadInt32();
            head.ActionId = reader.ReadInt32();
            head.SSID = reader.ReadPacketString();
            head.Address = reader.ReadPacketString();
            head.EnableGzip = reader.ReadBool();
            PacketMessage message = new PacketMessage();
            message.Head = head;
            message.Content = reader.ReadToEnd();
            return message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packHead"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public static PacketMessage CreateError(PacketHead packHead, MessageError errorCode, string errorInfo)
        {
            PacketMessage packet = new PacketMessage();
            packet.Head = packHead;
            MessageStructure ms = new MessageStructure();
            MessageHead msgHead = new MessageHead(packHead.MsgId, packHead.ActionId, "st", (int)errorCode, errorInfo);
            ms.WriteBuffer(msgHead);
            packet.Content = ms.ReadBuffer();
            return packet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] ToByte()
        {
            byte[] data = BufferUtils.MergeBytes(
                BufferUtils.GetBytes((char)Head.ConnectType),
                BufferUtils.GetBytes((char)Head.MsgType),
                BufferUtils.GetBytes(Head.MsgId),
                BufferUtils.GetBytes(Head.Uid),
                BufferUtils.GetBytes(Head.GameId),
                BufferUtils.GetBytes(Head.ServerId),
                BufferUtils.GetBytes(Head.ActionId),
                BufferUtils.GetBytes(Head.SSID),
                BufferUtils.GetBytes(Head.Address),
                BufferUtils.GetBytes(Head.EnableGzip),
                Content);
            return BufferUtils.AppendHeadBytes(data);
        }

        /// <summary>
        /// 
        /// </summary>
        public PacketMessage()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public PacketHead Head { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Content { get; set; }
    }
}
