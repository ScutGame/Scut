using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ZyGames.Framework.Model
{
    /// <summary>
    /// Supported sql entity object.
    /// </summary>
    public interface ISqlEntity
    {

        /// <summary>
        /// Message queue group id.
        /// </summary>
        /// <returns></returns>
        int GetMessageQueueId();

        /// <summary>
        /// 
        /// </summary>
        bool IsDelete { get; }

        /// <summary>
        /// 
        /// </summary>
        void ResetState();
    }
}
