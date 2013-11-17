using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace model
{
   public class ParamInfoModel
    {

        public ParamInfoModel()
        { }
        #region Model
        private int _id;
        private int _contractid;
        private int _paramtype;
        private string _field;
        private int _fieldtype;
        private string _descption;
        private string _fieldvalue;
        private bool _required;
        private string _remark;
        private int _sortid;
        private int _creator;
        private DateTime _createdate;
        private int _modifier;
        private DateTime _modifydate;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        public int SlnID
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int ContractID
        {
            set { _contractid = value; }
            get { return _contractid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ParamType
        {
            set { _paramtype = value; }
            get { return _paramtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Field
        {
            set { _field = value; }
            get { return _field; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int FieldType
        {
            set { _fieldtype = value; }
            get { return _fieldtype; }
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
        public string FieldValue
        {
            set { _fieldvalue = value; }
            get { return _fieldvalue; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Required
        {
            set { _required = value; }
            get { return _required; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SortID
        {
            set { _sortid = value; }
            get { return _sortid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Creator
        {
            set { _creator = value; }
            get { return _creator; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            set { _createdate = value; }
            get { return _createdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Modifier
        {
            set { _modifier = value; }
            get { return _modifier; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ModifyDate
        {
            set { _modifydate = value; }
            get { return _modifydate; }
        }
        #endregion Model

        public int MinValue
        {
            get;
            set;
        }
        public int MaxValue
        {
            get;
            set;
        }
    }
}
