using System.Web;

namespace ZyGames.Framework.Game.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpGameResponse : IGameResponse
    {
        /// <summary>
        /// 输出类
        /// </summary>
        private System.Web.HttpResponse _response;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        public HttpGameResponse(HttpResponse response)
        {
            _response = response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public void BinaryWrite(byte[] buffer)
        {
            _response.BinaryWrite(buffer);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public void Write(byte[] buffer)
        {
            _response.OutputStream.Write(buffer, 0, buffer.Length);
        }
    }
}