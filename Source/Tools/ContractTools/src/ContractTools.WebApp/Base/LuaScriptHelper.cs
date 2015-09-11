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
using System.Data;
using System.Linq;
using System.Text;
using ContractTools.WebApp.Model;
using ZyGames.Framework.RPC.IO;

namespace ContractTools.WebApp.Base
{
    public static class LuaScriptHelper
    {
        class LuaConfig
        {
            public string Key { get; set; }
            public StringBuilder Builder { get; set; }
        }

        public static string BuildLuaFile(DataTable table, MessageStructure msgReader, string[] keyNames)
        {
            StringBuilder containerBuilder = new StringBuilder();
            int loopDepth = 0;//ѭ�����
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
                    //����ѭ����¼
                    ParseRecordEnd(containerBuilder, msgReader, recordQueue, loopDepth, 0, keyNames);
                    recordQueue.Clear();
                }

                if (loopDepth == 0)
                {
                    if (NetHelper.GetFieldValue(msgReader, fieldType, ref fieldValue))
                    {
                        //�Զ���¼
                    }
                    if (fieldType == FieldType.Record || fieldType == FieldType.SigleRecord)
                    {
                        loopDepth++;
                        recordQueue.Add(record);
                    }
                }
                else if (fieldType != FieldType.End)
                {
                    if (fieldType == FieldType.Record || fieldType == FieldType.SigleRecord)
                    {
                        loopDepth++;
                    }
                    recordQueue.Add(record);
                }
            }
            return containerBuilder.ToString();
        }

        private static void ParseRecordEnd(StringBuilder itemBuilder, MessageStructure reader, List<DataRow> queue, int depth, int recordNum, string[] keyNames)
        {
            MessageStructure msgReader = reader;
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
                    int loopDepth = 0; //ѭ�����
                    StringBuilder recordBuilder = new StringBuilder();
                    List<DataRow> recordQueue = new List<DataRow>();

                    int columnNum = 0;
                    int childNum = 0;

                    #region ������ȡ����
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
                                //����ѭ����¼
                                childNum++;
                                var childBuilder = new StringBuilder();
                                ParseRecordEnd(childBuilder, msgReader, recordQueue, depth + 1, childNum, keyNames);
                                //
                                recordQueue.Clear();
                                //ѡ�������ʽ
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
                                if (fieldType == FieldType.Record || fieldType == FieldType.SigleRecord)
                                {
                                    loopDepth++;
                                    recordQueue.Add(record);
                                }
                            }
                            else if (fieldType != FieldType.End)
                            {
                                if (fieldType == FieldType.Record || fieldType == FieldType.SigleRecord)
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
                    //��ȡ�н���
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
                //ѡ�������ʽ
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