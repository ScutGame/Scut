using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using model;
using System.Data;

namespace BLL
{
    public class AgreementBLL
    {
        private readonly AgreementDAL dal = new AgreementDAL();
        /// <summary>
        /// 添加一条数据
        /// </summary>
        public int Add(AgreementModel model)
        {
            return dal.Add(model);
        }
        /// <summary>
        /// 获取全部数据
        /// </summary>
        public DataSet GetList(string strWhere, string SlnID)
        {
            return dal.GetList(strWhere, SlnID);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(AgreementModel model)
        {
            return dal.Update(model);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            return dal.Delete(ID);
        }

        public string GetUrl(int snlId)
        {
            DataTable table = GetList("GameID=" + snlId, snlId.ToString()).Tables[0];
            if (table.Rows.Count > 0)
            {
                return table.Rows[0]["Url"].ToString();
            }
            return string.Empty;
        }
    }
}
