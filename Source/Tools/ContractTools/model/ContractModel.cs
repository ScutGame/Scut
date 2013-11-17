using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace model
{
    public class ContractModel
    {
        public ContractModel()
        { }
        #region Model
        public int SlnID
        {
            get;
            set;
        }

        private int _id;
        private string _descption;
        private int _parentid;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Descption
        {
            set { _descption = value; }
            get { return _descption; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ParentID
        {
            set { _parentid = value; }
            get { return _parentid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Complated
        {
            set; get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int AgreementID
        {
            set;
            get;
        }
        #endregion Model
    }

    public class EnumInfoModel
    {
        public EnumInfoModel()
        { }
        #region Model
        public int ID
        {
            set;
            get;
        }
        public int SlnID
        {
            get;
            set;
        }
        public string enumName
        {
            get;
            set;
        }
        public string enumDescription
        {
            get;
            set;
        }
        public string enumValueInfo
        {
            get;
            set;
        }

        
        #endregion Model
    }
}
