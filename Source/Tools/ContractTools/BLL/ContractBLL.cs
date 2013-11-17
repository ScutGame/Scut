using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using System.Data;
using model;
namespace BLL
{
    public class ContractBLL
    {
        private readonly ContractDAL dal = new ContractDAL();
        /// <summary>
        /// 获取列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }

        public bool Copy(int slnID, int contractID, int copySlnID, int copyContractID)
        {
            return dal.Copy(slnID, contractID, copySlnID, copyContractID);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Add(ContractModel model)
        {
            return dal.Add(model);
        }

        public bool UpdateStatus(int id, int slnId, bool complated)
        {
            return dal.UpdateStatus(id, slnId, complated);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(ContractModel model)
        {
            return dal.Update(model);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID, int SlnID)
        {
            return dal.Delete(ID, SlnID);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ContractModel GetModel(int ID, int SlnID)
        {
            return dal.GetModel(ID, SlnID);
        }


        public DataSet Search(int slnID, string p)
        {
            return dal.Search(slnID, p);
        }
    }
}
