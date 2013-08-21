using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using model;
using System.Data;

namespace BLL
{
    public class SolutionBLL
    {
        private readonly SolutionDAL dal = new SolutionDAL();
        /// <summary>
        /// 添加一条数据
        /// </summary>
        public int Add(SolutionModel model)
        {
            return dal.Add(model);
        }
        /// <summary>
        /// 获取全部数据
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }
        /// <summary>
        /// 获取分服
        /// </summary>
        public DataSet GetServerList(string strWhere)
        {
            return dal.GetServerList(strWhere);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(SolutionModel model)
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
            DataTable table = GetList("SlnID=" + snlId).Tables[0];
            if (table.Rows.Count > 0)
            {
                return table.Rows[0]["Url"].ToString();
            }
            return string.Empty;
        }

        public string GetGameID(int snlId)
        {
            DataTable table = GetList("SlnID=" + snlId).Tables[0];
            if (table.Rows.Count > 0)
            {
                return table.Rows[0]["GameID"].ToString();
            }
            return string.Empty;
        }
    }
}
