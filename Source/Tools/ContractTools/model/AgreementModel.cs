using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace model
{
    public class AgreementModel
    {
        public int GameID { get; set; }

        public int AgreementID
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public string Describe
        {
            get;
            set;
        }

        public DateTime CreateDate
        {
            get;
            set;
        }
    }
}
