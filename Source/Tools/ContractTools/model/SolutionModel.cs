using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace model
{
    public class SolutionModel
    {
        public string Url { get; set; }

        public int SlnID
        {
            get;
            set;
        }
        public string SlnName
        {
            get;
            set;
        }
        public string Namespace
        {
            get;
            set;
        }
        /// <summary>
        /// 引用空间
        /// </summary>
        public string RefNamespace
        {
            get;
            set;
        }
        public int GameID
        {
            get;
            set;
        }
    }
}
