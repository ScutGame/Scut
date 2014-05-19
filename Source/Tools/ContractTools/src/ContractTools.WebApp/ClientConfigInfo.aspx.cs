/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ContractTools.WebApp.Base;
using ContractTools.WebApp.Model;
using ZyGames.Framework.RPC.IO;

namespace ContractTools.WebApp
{
    public partial class ClientConfigInfo : System.Web.UI.Page
    {
        public class LuaConfig
        {
            public string Key { get; set; }
            public StringBuilder Builder { get; set; }
        }

        protected int SlnID
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString["slnID"]))
                {
                    return 0;
                }
                return Convert.ToInt32(Request.Params["slnID"]);
            }
        }
        protected int ContractID
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    return 0;
                }
                return Convert.ToInt32(Request.QueryString["ID"]);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                var slnModel = DbDataLoader.GetSolution(SlnID);
                this.txtServerUrl.Text = slnModel == null ? "" : slnModel.Url;
                ddlContract.Items.Clear();
                var contractList = DbDataLoader.GetContract(SlnID);
                if (contractList.Count > 0)
                {
                    ddlContract.DataSource = contractList;
                    ddlContract.DataTextField = "uname";
                    ddlContract.DataValueField = "ID";
                    ddlContract.DataBind();
                    ddlContract.SelectedValue = ContractID.ToString();
                }
            }
        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            int contractID = int.Parse(ddlContract.Text);
            MessageHead msg = new MessageHead();
            
            var paramList = DbDataLoader.GetParamInfo(SlnID, contractID);
            string requestParams = GetRequestParams(paramList, contractID, txtVersion.Text);
            string serverUrl = txtServerUrl.Text;
            string[] keyNames = txtKeyName.Text.Split(new char[] { ',' });
            var msgReader = NetHelper.Create(serverUrl, requestParams, out msg, false, contractID, Session.SessionID);
            if (msgReader != null)
            {
                try
                {
                    if (msg.ErrorCode != 0)
                    {
                        txtResponse.Text = msg.ErrorInfo;
                    }
                    else
                    {
                        txtResponse.Text = BuildLuaFile(paramList, msgReader, keyNames);
                    }
                }
                catch (Exception ex)
                {
                    txtResponse.Text = ex.ToString();
                }
            }
        }

        private static string GetRequestParams(List<ParamInfoModel> paramList, int contractId, string clientVersion, string sid = "", int uid = 0)
        {
            StringBuilder requestParams = new StringBuilder();
            requestParams.AppendFormat("Sid={0}&Uid={1}&ActionID={2}&ClientVersion={3}&rl=1", sid, uid, contractId, clientVersion);

            var reqParamList = paramList.Where(p => p.ParamType == 1).OrderBy(p => p.SortID);
            int i = 0;
            foreach (var record in reqParamList)
            {
                if (requestParams.Length > 0)
                {
                    requestParams.Append("&");
                }
                string fieldName = record.Field;
                string fieldValue = record.FieldValue;

                requestParams.AppendFormat("{0}={1}", fieldName, fieldValue);
                i++;
            }
            return requestParams.ToString();
        }

        public static string BuildLuaFile(List<ParamInfoModel> paramList, MessageStructure msgReader, string[] keyNames)
        {
            var respParams = paramList.Where(p => p.ParamType == 2).OrderBy(p => p.SortID);
            StringBuilder containerBuilder = new StringBuilder();
            int loopDepth = 0;//循环深度
            List<ParamInfoModel> recordQueue = new List<ParamInfoModel>();
            foreach (var record in respParams)
            {
                string fieldName = record.Field;
                FieldType fieldType = record.FieldType;
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
                    if (NetHelper.GetFieldValue(msgReader, fieldType, ref fieldValue))
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

        private static void ParseRecordEnd(StringBuilder itemBuilder, MessageStructure msgReader, List<ParamInfoModel> queue, int depth, int recordNum, string[] keyNames)
        {
            string keyValue = string.Empty;
            string keyName = keyNames.Length > depth ? keyNames[depth] : string.Empty;
            List<LuaConfig> builderList = new List<LuaConfig>();
            int recordCount = 0;
            try
            {
                recordCount = msgReader.ReadInt();
            }
            catch { }
            for (int i = 0; i < recordCount; i++)
            {
                try
                {
                    msgReader.RecordStart();
                    int loopDepth = 0; //循环深度
                    StringBuilder recordBuilder = new StringBuilder();
                    List<ParamInfoModel> recordQueue = new List<ParamInfoModel>();

                    int columnNum = 0;
                    int childNum = 0;

                    #region 遍历列取数据
                    for (int r = 1; r < queue.Count - 1; r++)
                    {
                        var record = queue[r];
                        string fieldName = record.Field;
                        FieldType fieldType = record.FieldType;
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
                                if (NetHelper.GetFieldValue(msgReader, fieldType, ref fieldValue))
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

        private static void FormatListToLua(StringBuilder itemBuilder, List<LuaConfig> builderList, string keyName, int depth, int recordNum)
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

        private static void FormatChildToLua(StringBuilder builder, StringBuilder childBuilder, int recordIndex)
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