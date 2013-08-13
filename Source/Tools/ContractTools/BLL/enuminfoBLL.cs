using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using model;
using System.Data;

namespace BLL
{
    public class EnuminfoBLL
    {
        private readonly EnumInfoDAL dal = new EnumInfoDAL();
        /// <summary>
        /// 获取列表
        /// </summary>
        public DataSet GetList(int slnid)
        {
            return dal.GetList(slnid);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Add(model.EnumInfoModel model)
        {
            return dal.Add(model);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(EnumInfoModel model)
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
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public EnumInfoModel GetModel(int ID)
        {
            return dal.GetModel(ID);
        }
        public EnumInfoModel GetModel(string ID, string slnid)
        {
            return dal.GetModel(ID, slnid);
        }
    }

}
