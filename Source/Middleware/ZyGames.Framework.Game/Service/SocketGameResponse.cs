using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ZyGames.Framework.Game.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class SocketGameResponse : IGameResponse
    {
        private ConcurrentQueue<byte> _buffers;

        /// <summary>
        /// 
        /// </summary>
        public SocketGameResponse()
        {
            _buffers = new ConcurrentQueue<byte>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public void BinaryWrite(byte[] buffer)
        {
            DoWrite(buffer);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public void Write(byte[] buffer)
        {
            DoWrite(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] ReadByte()
        {
            var buffer = new List<byte>();
            while (_buffers.Count > 0)
            {
                byte b;
                if (_buffers.TryDequeue(out b))
                {
                    buffer.Add(b);
                }
            }
            return buffer.ToArray();
        }

        private void DoWrite(IEnumerable<byte> buffer)
        {
            foreach (var b in buffer)
            {
                _buffers.Enqueue(b);
            }
        }

    }
}
