using System;
using System.Text;
using DAL;
using model;
using System.Data;
using System.Collections.Generic;

namespace BLL
{
    public class LuaConfig
    {
        public string Key { get; set; }
        public StringBuilder Builder { get; set; }
    }
    public class ParamInfoBLL
    {
        private readonly ParamInfoDAL dal = new ParamInfoDAL();
        /// <summary>
        /// 添加一条数据
        /// </summary>
        public int Add(ParamInfoModel model)
        {
            FieldType fieldType = (FieldType)Enum.ToObject(typeof(FieldType), model.FieldType);
            if (FieldType.Record == fieldType || fieldType == FieldType.End || fieldType == FieldType.Head)
            {
                model.Field = string.Empty;
            }
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
        /// 更新一条数据
        /// </summary>
        public bool Update(ParamInfoModel model)
        {
            FieldType fieldType = (FieldType)Enum.ToObject(typeof(FieldType), model.FieldType);
            if (FieldType.Record == fieldType || fieldType == FieldType.End || fieldType == FieldType.Head)
            {
                model.Field = string.Empty;
            }
            return dal.Update(model);
        }

        public bool UpdateSort(int paramID, int sortID)
        {
            return dal.UpdateSort(paramID, sortID);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            return dal.Delete(ID);
        }
        /// <summary>
        /// 获得ID
        /// </summary>
        public DataSet GetID(string strWhere)
        {
            return dal.GetID(strWhere);
        }

        public string LuaConfig(int slnId, int contractId, string[] keyNames, MessageReader reader)
        {
            DataTable table = GetList("paramType=2 and SlnID=" + slnId + " and ContractID=" + contractId).Tables[0];

            return BuildLuaFile(table, reader, keyNames);
        }

        private string BuildLuaFile(DataTable table, MessageReader msgReader, string[] keyNames)
        {
            StringBuilder containerBuilder = new StringBuilder();
            int loopDepth = 0;//循环深度
            List<DataRow> recordQueue = new List<DataRow>();
            foreach (DataRow record in table.Rows)
            {
                string fieldName = record["Field"].ToString();
                FieldType fieldType = (FieldType)Enum.Parse(typeof(FieldType), record["FieldType"].ToString());
                string fieldValue = "";

                if (loopDepth > 0 && fieldType == FieldType.End)
                {
                    loopDepth--;
                    recordQueue.Add(record);
                }
                if (loopDepth == 0 && recordQueue.Count > 0)
                {
                    //处理循环记录
                    ParseRecordEnd(containerBuilder, msgReader, recordQueue, loopDepth, 0, keyNames);
                    recordQueue.Clear();
                }

                if (loopDepth == 0)
                {
                    if (msgReader.GetFieldValue(fieldType, ref fieldValue))
                    {
                        //自动登录
                    }
                    if (fieldType == FieldType.Record)
                    {
                        loopDepth++;
                        recordQueue.Add(record);
                    }
                }
                else if (fieldType != FieldType.End)
                {
                    if (fieldType == FieldType.Record)
                    {
                        loopDepth++;
                    }
                    recordQueue.Add(record);
                }
            }
            return containerBuilder.ToString();
        }

        private void ParseRecordEnd(StringBuilder itemBuilder, MessageReader reader, List<DataRow> queue, int depth, int recordNum, string[] keyNames)
        {
            MessageReader msgReader = reader;
            string keyValue = string.Empty;
            string keyName = keyNames.Length > depth ? keyNames[depth] : string.Empty;
            List<LuaConfig> builderList = new List<LuaConfig>();
            int recordCount = 0;
            try
            {
                recordCount = msgReader.RecordCount();
            }
            catch { }
            for (int i = 0; i < recordCount; i++)
            {
                try
                {
                    msgReader.RecordStart();
                    int loopDepth = 0; //循环深度
                    StringBuilder recordBuilder = new StringBuilder();
                    List<DataRow> recordQueue = new List<DataRow>();

                    int columnNum = 0;
                    int childNum = 0;

                    #region 遍历列取数据
                    for (int r = 1; r < queue.Count - 1; r++)
                    {
                        DataRow record = queue[r];
                        string fieldName = record["Field"].ToString();
                        FieldType fieldType = (FieldType)Enum.Parse(typeof(FieldType), record["FieldType"].ToString());
                        string fieldValue = "";
                        try
                        {
                            if (loopDepth > 0 && fieldType == FieldType.End)
                            {
                                loopDepth--;
                                recordQueue.Add(record);
                            }
                            if (loopDepth == 0 && recordQueue.Count > 0)
                            {
                                //处理循环记录
                                childNum++;
                                var childBuilder = new StringBuilder();
                                ParseRecordEnd(childBuilder, msgReader, recordQueue, depth + 1, childNum, keyNames);
                                //
                                recordQueue.Clear();
                                //选择输出格式
                                FormatChildToLua(recordBuilder, childBuilder, columnNum);
                            }

                            if (loopDepth == 0)
                            {
                                if (msgReader.GetFieldValue(fieldType, ref fieldValue))
                                {
                                    if (columnNum > 0)
                                    {
                                        recordBuilder.Append(",");
                                    }
                                    if (fieldName.Trim().ToLower() == keyName.Trim().ToLower())
                                    {
                                        keyValue = fieldValue;
                                    }
                                    if (fieldType == FieldType.Byte || fieldType == FieldType.Short || fieldType == FieldType.Int)
                                    {
                                        recordBuilder.AppendFormat("{0}={1}", fieldName, fieldValue);
                                    }
                                    else
                                    {
                                        recordBuilder.AppendFormat("{0}=\"{1}\"", fieldName, fieldValue);
                                    }
                                    columnNum++;
                                }
                                if (fieldType == FieldType.Record)
                                {
                                    loopDepth++;
                                    recordQueue.Add(record);
                                }
                            }
                            else if (fieldType != FieldType.End)
                            {
                                if (fieldType == FieldType.Record)
                                {
                                    loopDepth++;
                                }
                                recordQueue.Add(record);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(string.Format("recordindex:{0},fieldName:{1} error:", i, fieldName), ex);
                        }
                    }

                    #endregion
                    //读取行结束
                    msgReader.RecordEnd();
                    builderList.Add(new LuaConfig { Key = keyValue, Builder = recordBuilder });
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("recordindex:{0}error:", i), ex);
                }
            }

            FormatListToLua(itemBuilder, builderList, keyName, depth, recordNum);

        }

        private void FormatListToLua(StringBuilder itemBuilder, List<LuaConfig> builderList, string keyName, int depth, int recordNum)
        {
            if (depth == 0)
            {
                //选择输出格式
                if (keyName.Length > 0)
                {
                    foreach (var recordBuilder in builderList)
                    {
                        itemBuilder.AppendFormat("RecordInfos[{0}]=", recordBuilder.Key);
                        itemBuilder.Append("{");
                        itemBuilder.Append(recordBuilder.Builder);
                        itemBuilder.Append("}");
                        itemBuilder.AppendLine();
                        itemBuilder.AppendLine();
                    }
                }
                else
                {
                    itemBuilder.Append("RecordInfos={");
                    int index = 0;
                    foreach (var recordBuilder in builderList)
                    {
                        if (index > 0)
                        {
                            itemBuilder.Append(",");
                        }
                        itemBuilder.AppendLine();
                        itemBuilder.Append("{");
                        itemBuilder.Append(recordBuilder.Builder);
                        itemBuilder.Append("}");
                        index++;
                    }
                    itemBuilder.AppendLine();
                    itemBuilder.Append("}");
                }

            }
            else
            {
                itemBuilder.AppendFormat("{0}SubInfos{1}_{2}=",
                                           string.Empty.PadLeft(depth * 4),
                                           depth,
                                           recordNum);
                if (keyName.Length > 0)
                {
                    itemBuilder.Append("{");
                    int index = 0;
                    foreach (var subBuilder in builderList)
                    {
                        if (index > 0)
                        {
                            itemBuilder.Append(",");
                        }
                        itemBuilder.AppendLine();
                        itemBuilder.Append(string.Empty.PadLeft((depth + 1) * 4));
                        itemBuilder.AppendFormat("[{0}]=", subBuilder.Key);
                        itemBuilder.Append("{");
                        itemBuilder.Append(subBuilder.Builder);
                        itemBuilder.Append("}");
                        index++;
                    }
                    if (index > 0)
                    {
                        itemBuilder.AppendLine();
                        itemBuilder.Append(string.Empty.PadLeft(depth * 4));
                    }
                    itemBuilder.Append("}");
                }
                else
                {
                    itemBuilder.Append("{");
                    int index = 0;
                    foreach (var subBuilder in builderList)
                    {
                        if (index > 0)
                        {
                            itemBuilder.Append(",");
                        }
                        itemBuilder.AppendLine();
                        itemBuilder.Append(string.Empty.PadLeft((depth + 1) * 4));
                        itemBuilder.Append("{");
                        itemBuilder.Append(subBuilder.Builder);
                        itemBuilder.Append("}");
                        index++;
                    }
                    if (index > 0)
                    {
                        itemBuilder.AppendLine();
                        itemBuilder.Append(string.Empty.PadLeft(depth * 4));
                    }
                    itemBuilder.Append("}");
                }
            }

        }

        private void FormatChildToLua(StringBuilder builder, StringBuilder childBuilder, int recordIndex)
        {
            bool isFirst = recordIndex == 0;
            if (!isFirst)
            {
                builder.Append(",");
                builder.AppendLine();
            }
            builder.Append(childBuilder);
        }
    }
}
