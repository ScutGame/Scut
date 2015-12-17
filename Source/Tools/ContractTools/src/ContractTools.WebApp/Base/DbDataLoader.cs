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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ContractTools.WebApp.Model;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Data;

namespace ContractTools.WebApp.Base
{
    /// <summary>
    /// The db data loader
    /// </summary>
    public static class DbDataLoader
    {
        private static DbBaseProvider _dbBaseProvider;

        static DbDataLoader()
        {
            _dbBaseProvider = DbConnectionProvider.CreateDbProvider("Contract");
            _dbBaseProvider.CheckConnect();
        }

        #region SolutionModel


        public static int Add(SolutionModel model)
        {
            var command = _dbBaseProvider.CreateCommandStruct("Solutions", CommandMode.Insert);
            command.AddParameter("SlnName", model.SlnName);
            command.AddParameter("Namespace", model.Namespace);
            command.AddParameter("RefNamespace", model.RefNamespace);
            command.AddParameter("Url", model.Url);
            command.AddParameter("GameID", model.GameID);
            command.AddParameter("SerUseScript", model.SerUseScript);
            command.AddParameter("CliUseScript", model.CliUseScript);
            command.AddParameter("IsDParam", model.IsDParam);
            command.AddParameter("RespContentType", model.RespContentType);
            command.ReturnIdentity = true;
            command.Parser();
            return _dbBaseProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters);

        }
        public static bool Update(SolutionModel model)
        {
            var command = _dbBaseProvider.CreateCommandStruct("Solutions", CommandMode.Modify);
            command.AddParameter("SlnName", model.SlnName);
            command.AddParameter("Namespace", model.Namespace);
            command.AddParameter("RefNamespace", model.RefNamespace);
            command.AddParameter("Url", model.Url);
            command.AddParameter("GameID", model.GameID);
            command.AddParameter("SerUseScript", model.SerUseScript);
            command.AddParameter("CliUseScript", model.CliUseScript);
            command.AddParameter("IsDParam", model.IsDParam);
            command.AddParameter("RespContentType", model.RespContentType);
            command.Filter = _dbBaseProvider.CreateCommandFilter();
            command.Filter.Condition = _dbBaseProvider.FormatFilterParam("SlnID");
            command.Filter.AddParam("SlnID", model.SlnID);
            command.Parser();
            return _dbBaseProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters) > 0;

        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public static bool Delete(SolutionModel model)
        {
            var command = _dbBaseProvider.CreateCommandStruct("Solutions", CommandMode.Delete);
            command.Filter = _dbBaseProvider.CreateCommandFilter();
            command.Filter.Condition = _dbBaseProvider.FormatFilterParam("SlnID");
            command.Filter.AddParam("SlnID", model.SlnID);
            command.Parser();
            return _dbBaseProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters) > 0;
        }

        public static List<SolutionModel> GetSolution()
        {
            return GetSolution(f =>
            {
            });
        }

        public static SolutionModel GetSolution(int slnId)
        {
            return GetSolution(f =>
            {
                f.Condition = f.FormatExpression("SlnID");
                f.AddParam("SlnID", slnId);
            }).FirstOrDefault();
        }

        public static List<SolutionModel> GetSolution(Action<CommandFilter> match)
        {
            var command = _dbBaseProvider.CreateCommandStruct("Solutions", CommandMode.Inquiry);
            command.Columns = "SlnID,SlnName,Namespace,RefNamespace,Url,GameID,SerUseScript,CliUseScript,IsDParam,RespContentType";
            command.OrderBy = "SlnID ASC";
            command.Filter = _dbBaseProvider.CreateCommandFilter();
            if (match != null)
            {
                match(command.Filter);
            }
            command.Parser();
            var list = new List<SolutionModel>();
            using (var reader = _dbBaseProvider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
            {
                while (reader.Read())
                {
                    SolutionModel model = new SolutionModel();
                    model.SlnID = reader["SlnID"].ToInt();
                    model.GameID = reader["GameID"].ToInt();
                    model.SlnName = reader["SlnName"].ToNotNullString();
                    model.Namespace = reader["Namespace"].ToNotNullString();
                    model.RefNamespace = reader["RefNamespace"].ToNotNullString();
                    model.Url = reader["Url"].ToNotNullString();
                    model.SerUseScript = reader["SerUseScript"].ToNotNullString();
                    model.CliUseScript = reader["CliUseScript"].ToNotNullString();
                    model.IsDParam = reader["IsDParam"].ToBool();
                    model.RespContentType = reader["RespContentType"].ToInt();
                    list.Add(model);
                }
            }
            return list;
        }
        #endregion

        #region AgreementModel
        public static int Add(AgreementModel model)
        {
            var command = _dbBaseProvider.CreateCommandStruct("AgreementClass", CommandMode.Insert);
            command.AddParameter("GameID", model.GameID);
            command.AddParameter("Title", model.Title);
            command.AddParameter("Describe", model.Describe);
            command.ReturnIdentity = true;
            command.Parser();
            using (var reader = _dbBaseProvider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
            {
                if (reader.Read())
                {
                    model.AgreementID = reader[0].ToInt();
                }
            }
            return model.AgreementID;
        }

        public static bool Update(AgreementModel model)
        {
            var command = _dbBaseProvider.CreateCommandStruct("AgreementClass", CommandMode.Modify);
            command.AddParameter("Title", model.Title);
            command.AddParameter("Describe", model.Describe);
            command.Filter = _dbBaseProvider.CreateCommandFilter();
            command.Filter.Condition = _dbBaseProvider.FormatFilterParam("AgreementID");
            command.Filter.AddParam("AgreementID", model.AgreementID);
            command.Parser();
            return _dbBaseProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters) > 0;
        }

        public static bool Delete(AgreementModel model)
        {
            var command = _dbBaseProvider.CreateCommandStruct("AgreementClass", CommandMode.Delete);
            command.Filter = _dbBaseProvider.CreateCommandFilter();
            command.Filter.Condition = _dbBaseProvider.FormatFilterParam("AgreementID");
            command.Filter.AddParam("AgreementID", model.AgreementID);
            command.Parser();
            return _dbBaseProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters) > 0;
        }

        public static List<AgreementModel> GetAgreement(int gameId)
        {
            return GetAgreement(f =>
            {
                f.Condition = f.FormatExpression("GameID");
                f.AddParam("GameID", gameId);
            });
        }

        public static List<AgreementModel> GetAgreement(Action<CommandFilter> match)
        {
            var command = _dbBaseProvider.CreateCommandStruct("AgreementClass", CommandMode.Inquiry);
            command.Columns = string.Format("AgreementID,GameID,Title,Describe,CreateDate,(SELECT SLNNAME FROM SOLUTIONS WHERE SLNID={0}.GAMEID) SlnName", command.TableName);
            command.OrderBy = "AgreementID ASC";
            command.Filter = _dbBaseProvider.CreateCommandFilter();
            if (match != null)
            {
                match(command.Filter);
            }
            command.Parser();
            var list = new List<AgreementModel>();
            using (var reader = _dbBaseProvider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
            {
                while (reader.Read())
                {
                    AgreementModel model = new AgreementModel();
                    model.AgreementID = reader["AgreementID"].ToInt();
                    model.GameID = reader["GameID"].ToInt();
                    model.Title = reader["Title"].ToNotNullString();
                    model.Describe = reader["Describe"].ToNotNullString();
                    model.CreateDate = reader["CreateDate"].ToDateTime();
                    model.SlnName = reader["SlnName"].ToNotNullString();

                    list.Add(model);
                }
            }
            return list;
        }

        #endregion

        #region ContractModel
        public static int Add(ContractModel model)
        {
            var command = _dbBaseProvider.CreateCommandStruct("Contract", CommandMode.Insert);
            command.AddParameter("ID", model.ID);
            command.AddParameter("Descption", model.Descption);
            command.AddParameter("ParentID", model.ParentID);
            command.AddParameter("SlnID", model.SlnID);
            command.AddParameter("Complated", model.Complated);
            command.AddParameter("AgreementID", model.AgreementID);
            command.AddParameter("VerId", model.VerID);
            command.Parser();
            return _dbBaseProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters);
        }

        public static bool UpdateContractStatus(int id, int slnId, bool complated)
        {
            var command = _dbBaseProvider.CreateCommandStruct("Contract", CommandMode.Modify);
            command.AddParameter("Complated", complated);
            command.Filter = _dbBaseProvider.CreateCommandFilter();
            command.Filter.Condition = string.Format("{0} AND {1}",
                _dbBaseProvider.FormatFilterParam("ID"),
                _dbBaseProvider.FormatFilterParam("SlnID"));
            command.Filter.AddParam("ID", id);
            command.Filter.AddParam("SlnID", slnId);
            command.Parser();
            return _dbBaseProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters) > 0;
        }

        public static bool Update(ContractModel model)
        {
            var command = _dbBaseProvider.CreateCommandStruct("Contract", CommandMode.Modify);
            command.AddParameter("Descption", model.Descption);
            command.AddParameter("ParentID", model.ParentID);
            command.AddParameter("Complated", model.Complated);
            if (model.VerID > 0)
            {
                command.AddParameter("VerId", model.VerID);
            }
            if (model.AgreementID > 0)
            {
                command.AddParameter("AgreementID", model.AgreementID);
            }
            command.Filter = _dbBaseProvider.CreateCommandFilter();
            command.Filter.Condition = string.Format("{0} AND {1}",
                _dbBaseProvider.FormatFilterParam("ID"),
                _dbBaseProvider.FormatFilterParam("SlnID"));
            command.Filter.AddParam("ID", model.ID);
            command.Filter.AddParam("SlnID", model.SlnID);
            command.Parser();
            return _dbBaseProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters) > 0;
        }

        public static bool Delete(ContractModel model)
        {
            if (Delete(new ParamInfoModel() { ContractID = model.ID, SlnID = model.SlnID }))
            {
                var command = _dbBaseProvider.CreateCommandStruct("Contract", CommandMode.Delete);
                command.Filter = _dbBaseProvider.CreateCommandFilter();
                command.Filter.Condition = string.Format("{0} AND {1}",
                                                         _dbBaseProvider.FormatFilterParam("ID"),
                                                         _dbBaseProvider.FormatFilterParam("SlnID"));
                command.Filter.AddParam("ID", model.ID);
                command.Filter.AddParam("SlnID", model.SlnID);
                command.Parser();
                return _dbBaseProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters) > 0;
            }
            return false;
        }

        public static List<ContractModel> GetContract(int slnId, int versionId)
        {
            return GetContract(f =>
            {
                if (versionId > 0)
                {
                    f.Condition = string.Format("{0} AND ", f.FormatExpression("VerId", "<="));
                    f.AddParam("VerId", versionId);
                }
                f.Condition += f.FormatExpression("SlnID");
                f.AddParam("SlnID", slnId);
            });
        }
        public static List<ContractModel> GetContract(int slnId, string filter)
        {
            return GetContract(f =>
            {
                f.Condition = string.Format("({0} OR {1}) AND ", f.FormatExpression("ID", "LIKE", "filter"), f.FormatExpression("Descption", "LIKE", "filter"));
                f.AddParam("filter", "%" + filter + "%");
                f.Condition += f.FormatExpression("SlnID");
                f.AddParam("SlnID", slnId);
            });
        }

        public static ContractModel GetContract(int slnId, int contractId, int versionId)
        {
            return GetContract(f =>
            {
                if (versionId > 0)
                {
                    f.Condition = string.Format("{0} AND ", f.FormatExpression("VerId", "<="));
                    f.AddParam("VerId", versionId);
                }
                f.Condition += string.Format("{0} AND {1}", f.FormatExpression("ID"), f.FormatExpression("SlnID"));
                f.AddParam("ID", contractId);
                f.AddParam("SlnID", slnId);
            }).FirstOrDefault();
        }

        public static List<ContractModel> GetContractByAgreement(int slnId, int agreementId, int versionId)
        {
            return GetContract(f =>
            {
                if (versionId > 0)
                {
                    f.Condition = string.Format("{0} AND ", f.FormatExpression("VerId", "<="));
                    f.AddParam("VerId", versionId);
                }
                f.Condition += f.FormatExpression("SlnID");
                f.AddParam("SlnID", slnId);
                if (agreementId > 0)
                {
                    f.Condition += " AND " + f.FormatExpression("AgreementID");
                    f.AddParam("AgreementID", agreementId);
                }
            });
        }


        public static List<ContractModel> GetContract(Action<CommandFilter> match)
        {
            var command = _dbBaseProvider.CreateCommandStruct("Contract", CommandMode.Inquiry);
            command.Columns = "ID,Descption,ParentID,SlnID,Complated,AgreementID,VerId";
            command.OrderBy = "SlnID ASC,Complated DESC,ID ASC";
            command.Filter = _dbBaseProvider.CreateCommandFilter();
            if (match != null)
            {
                match(command.Filter);
            }
            command.Parser();
            var list = new List<ContractModel>();
            using (var reader = _dbBaseProvider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
            {
                while (reader.Read())
                {
                    ContractModel model = new ContractModel();
                    model.ID = reader["ID"].ToInt();
                    model.Descption = reader["Descption"].ToNotNullString();
                    model.ParentID = reader["ParentID"].ToInt();
                    model.SlnID = reader["SlnID"].ToInt();
                    model.Complated = reader["Complated"].ToBool();
                    model.AgreementID = reader["AgreementID"].ToInt();
                    model.VerID = reader["VerId"].ToInt();
                    list.Add(model);
                }
            }
            return list;
        }

        public static bool CopyContract(int slnID, int contractID, int copySlnID, int copyContractID)
        {
            var contract = GetContract(slnID, contractID, 0);
            if (contract == null)
            {
                return false;
            }

            var contractcopy = new ContractModel()
            {
                ID = copyContractID,
                SlnID = copySlnID,
                AgreementID = contract.AgreementID,
                Complated = false,
                Descption = contract.Descption,
                ParentID = contract.ParentID
            };

            if (Add(contractcopy) > 0)
            {
                var paramList = GetParamInfo(slnID, contractID, 0);
                foreach (var paramInfo in paramList)
                {
                    var info = new ParamInfoModel()
                    {
                        ContractID = contractcopy.ID,
                        SlnID = contractcopy.SlnID,
                        CreateDate = MathUtils.Now,
                        Creator = paramInfo.Creator,
                        Descption = paramInfo.Descption,
                        Field = paramInfo.Field,
                        FieldType = paramInfo.FieldType,
                        FieldValue = paramInfo.FieldValue,
                        MinValue = paramInfo.MinValue,
                        MaxValue = paramInfo.MaxValue,
                        Remark = paramInfo.Remark,
                        Required = paramInfo.Required,
                        ParamType = paramInfo.ParamType,
                        SortID = paramInfo.SortID
                    };
                    Add(info);
                }
                return true;
            }

            return false;
        }

        #endregion

        #region ParamInfoModel
        public static int Add(ParamInfoModel model)
        {
            var command = _dbBaseProvider.CreateCommandStruct("ParamInfo", CommandMode.Insert);
            command.ReturnIdentity = true;
            command.AddParameter("SlnID", model.SlnID);
            command.AddParameter("ContractID", model.ContractID);
            command.AddParameter("ParamType", model.ParamType);
            command.AddParameter("Field", model.Field);
            command.AddParameter("FieldType", model.FieldType);
            command.AddParameter("Descption", model.Descption);
            command.AddParameter("FieldValue", model.FieldValue);
            command.AddParameter("Required", model.Required);
            command.AddParameter("Remark", model.Remark);
            command.AddParameter("SortID", model.SortID);
            command.AddParameter("Creator", model.Creator);
            command.AddParameter("CreateDate", model.CreateDate);
            command.AddParameter("Modifier", model.Modifier);
            if (model.CreateDate == DateTime.MinValue)
            {
                model.CreateDate = MathUtils.Now;
            }
            if (model.ModifyDate == DateTime.MinValue)
            {
                model.ModifyDate = MathUtils.SqlMinDate;
            }
            command.AddParameter("ModifyDate", model.ModifyDate);
            command.AddParameter("MinValue", model.MinValue);
            command.AddParameter("MaxValue", model.MaxValue);
            command.AddParameter("VerID", model.VerID);
            command.Parser();
            return _dbBaseProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters);
        }

        public static bool UpdateParamSort(int paramID, int sortID)
        {
            var command = _dbBaseProvider.CreateCommandStruct("ParamInfo", CommandMode.Modify);
            command.AddParameter("SortID", sortID);
            command.Filter = _dbBaseProvider.CreateCommandFilter();
            command.Filter.Condition = _dbBaseProvider.FormatFilterParam("ID");
            command.Filter.AddParam("ID", paramID);
            command.Parser();
            return _dbBaseProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters) > 0;
        }

        public static bool Update(ParamInfoModel model)
        {
            var command = _dbBaseProvider.CreateCommandStruct("ParamInfo", CommandMode.Modify);
            if (model.SlnID > 0)
            {
                command.AddParameter("SlnID", model.SlnID);
            }
            command.AddParameter("ContractID", model.ContractID);
            command.AddParameter("ParamType", model.ParamType);
            command.AddParameter("Field", model.Field);
            command.AddParameter("FieldType", model.FieldType);
            command.AddParameter("Descption", model.Descption);
            command.AddParameter("FieldValue", model.FieldValue);

            command.AddParameter("Required", model.Required);
            command.AddParameter("Remark", model.Remark);
            if (model.SortID > -1)
            {
                command.AddParameter("SortID", model.SortID);
            }
            command.AddParameter("Creator", model.Creator);
            command.AddParameter("Modifier", model.Modifier);
            command.AddParameter("MinValue", model.MinValue);
            command.AddParameter("MaxValue", model.MaxValue);
            command.AddParameter("ModifyDate", model.ModifyDate);
            if (model.VerID > 0)
            {
                command.AddParameter("VerID", model.VerID);
            }
            command.Filter = _dbBaseProvider.CreateCommandFilter();
            command.Filter.Condition = _dbBaseProvider.FormatFilterParam("ID");
            command.Filter.AddParam("ID", model.ID);
            command.Parser();
            return _dbBaseProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters) > 0;
        }

        public static bool Delete(ParamInfoModel model)
        {
            var command = _dbBaseProvider.CreateCommandStruct("ParamInfo", CommandMode.Delete);
            command.Filter = _dbBaseProvider.CreateCommandFilter();
            if (model.ID > 0)
            {
                command.Filter.Condition = _dbBaseProvider.FormatFilterParam("ID");
                command.Filter.AddParam("ID", model.ID);
            }
            else if (model.SlnID > 0 && model.ContractID > 0)
            {
                command.Filter.Condition = string.Format("{0} AND {1}",
                    _dbBaseProvider.FormatFilterParam("ContractID"),
                    _dbBaseProvider.FormatFilterParam("SlnID"));
                command.Filter.AddParam("ContractID", model.ContractID);
                command.Filter.AddParam("SlnID", model.SlnID);
            }
            else
            {
                throw new ArgumentException("参数异常");
            }
            command.Parser();
            _dbBaseProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters);
            return true;
        }

        public static List<ParamInfoModel> GetParamInfo(int slnId, int contractId, int paramType, int versionId)
        {
            return GetParamInfo(f =>
            {
                if (versionId > 0)
                {
                    f.Condition = string.Format("{0} AND ", f.FormatExpression("VerID", "<="));
                    f.AddParam("VerID", versionId);
                }
                f.Condition += string.Format("{0} AND {1} AND {2}",
                    f.FormatExpression("ContractID"),
                    f.FormatExpression("SlnID"),
                    f.FormatExpression("ParamType"));
                f.AddParam("ContractID", contractId);
                f.AddParam("SlnID", slnId);
                f.AddParam("ParamType", paramType);
            });
        }

        public static List<ParamInfoModel> GetParamInfo(int slnId, int contractId, int versionId)
        {
            var result = GetParamInfo(f =>
            {
                if (versionId > 0)
                {
                    f.Condition = string.Format("{0} AND ", f.FormatExpression("VerID", "<="));
                    f.AddParam("VerID", versionId);
                }
                f.Condition += string.Format("{0} AND {1}",
                    f.FormatExpression("ContractID"),
                    f.FormatExpression("SlnID"));
                f.AddParam("ContractID", contractId);
                f.AddParam("SlnID", slnId);
            });

            return TemplateHelper.InitParamDepth(result);
        }

        public static List<ParamInfoModel> GetParamInfo(Action<CommandFilter> match)
        {
            var command = _dbBaseProvider.CreateCommandStruct("ParamInfo", CommandMode.Inquiry);
            command.Columns = "ID,SlnID,ContractID,ParamType,Field,FieldType,Descption,FieldValue,Required,Remark,SortID,Creator,CreateDate,Modifier,ModifyDate,MinValue,MaxValue,VerID";
            command.OrderBy = "PARAMTYPE ASC,SORTID ASC,ID ASC";
            command.Filter = _dbBaseProvider.CreateCommandFilter();
            if (match != null)
            {
                match(command.Filter);
            }
            command.Parser();
            var list = new List<ParamInfoModel>();
            using (var reader = _dbBaseProvider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
            {
                while (reader.Read())
                {
                    ParamInfoModel model = new ParamInfoModel();
                    model.ID = reader["ID"].ToInt();
                    model.SlnID = reader["SlnID"].ToInt();
                    model.ContractID = reader["ContractID"].ToInt();
                    model.ParamType = reader["ParamType"].ToInt();
                    model.Field = reader["Field"].ToNotNullString();
                    model.FieldType = reader["FieldType"].ToEnum<FieldType>();
                    model.Descption = reader["Descption"].ToNotNullString();
                    model.FieldValue = reader["FieldValue"].ToNotNullString();
                    model.Required = reader["Required"].ToBool();
                    model.Remark = reader["Remark"].ToNotNullString();
                    model.SortID = reader["SortID"].ToInt();
                    model.Creator = reader["Creator"].ToInt();
                    model.CreateDate = reader["CreateDate"].ToDateTime();
                    model.Modifier = reader["Modifier"].ToInt();
                    model.ModifyDate = reader["ModifyDate"].ToDateTime();
                    model.MinValue = reader["MinValue"].ToInt();
                    model.MaxValue = reader["MaxValue"].ToInt();
                    model.VerID = reader["VerID"].ToInt();
                    list.Add(model);
                }
            }
            return list;
        }

        #endregion

        #region EnumInfoModel
        public static int Add(EnumInfoModel model)
        {
            var command = _dbBaseProvider.CreateCommandStruct("EnumInfo", CommandMode.Insert);
            command.AddParameter("SlnID", model.SlnID);
            command.AddParameter("enumName", model.enumName);
            command.AddParameter("enumDescription", model.enumDescription);
            command.AddParameter("enumValueInfo", model.enumValueInfo);
            command.Parser();
            return _dbBaseProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters);
        }

        public static bool Update(EnumInfoModel model)
        {
            var command = _dbBaseProvider.CreateCommandStruct("EnumInfo", CommandMode.Modify);
            command.AddParameter("enumName", model.enumName);
            command.AddParameter("enumDescription", model.enumDescription);
            command.AddParameter("enumValueInfo", model.enumValueInfo);
            command.Filter = _dbBaseProvider.CreateCommandFilter();
            command.Filter.Condition = _dbBaseProvider.FormatFilterParam("ID");
            command.Filter.AddParam("ID", model.ID);
            command.Parser();
            return _dbBaseProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters) > 0;
        }

        public static bool Delete(EnumInfoModel model)
        {
            var command = _dbBaseProvider.CreateCommandStruct("EnumInfo", CommandMode.Delete);
            command.Filter = _dbBaseProvider.CreateCommandFilter();
            command.Filter.Condition = _dbBaseProvider.FormatFilterParam("ID");
            command.Filter.AddParam("ID", model.ID);
            command.Parser();
            return _dbBaseProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters) > 0;
        }

        public static List<EnumInfoModel> GetEnumInfo(int slnId, string enumName = "")
        {
            return GetEnumInfo(f =>
            {
                f.Condition = f.FormatExpression("SlnID");
                f.AddParam("SlnID", slnId);
                if (!string.IsNullOrEmpty(enumName))
                {
                    f.Condition += " AND " + f.FormatExpression("enumName");
                    f.AddParam("enumName", enumName);
                }
            });
        }

        public static List<EnumInfoModel> GetEnumInfo(Action<CommandFilter> match)
        {
            var command = _dbBaseProvider.CreateCommandStruct("EnumInfo", CommandMode.Inquiry);
            command.Columns = "Id,SlnID,enumName,enumDescription,enumValueInfo";
            command.OrderBy = "enumName ASC,Id ASC";
            command.Filter = _dbBaseProvider.CreateCommandFilter();
            if (match != null)
            {
                match(command.Filter);
            }
            command.Parser();
            var list = new List<EnumInfoModel>();
            using (var reader = _dbBaseProvider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
            {
                while (reader.Read())
                {
                    EnumInfoModel model = new EnumInfoModel();
                    model.ID = reader["ID"].ToInt();
                    model.SlnID = reader["SlnID"].ToInt();
                    model.enumName = reader["enumName"].ToNotNullString();
                    model.enumDescription = reader["enumDescription"].ToNotNullString();
                    model.enumValueInfo = reader["enumValueInfo"].ToNotNullString();

                    list.Add(model);
                }
            }
            return list;
        }

        #endregion

        #region version
        public static int Add(VersionMode model)
        {
            var command = _dbBaseProvider.CreateCommandStruct("ContractVersion", CommandMode.Insert);
            command.AddParameter("SlnID", model.SlnID);
            command.AddParameter("Title", model.Title);
            command.ReturnIdentity = true;
            command.Parser();
            return _dbBaseProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters);

        }
        public static bool Update(VersionMode model)
        {
            var command = _dbBaseProvider.CreateCommandStruct("ContractVersion", CommandMode.Modify);
            if (model.SlnID > 0)
            {
                command.AddParameter("SlnID", model.SlnID);
            }
            command.AddParameter("Title", model.Title);
            command.Filter = _dbBaseProvider.CreateCommandFilter();
            command.Filter.Condition = _dbBaseProvider.FormatFilterParam("ID");
            command.Filter.AddParam("ID", model.ID);
            command.Parser();
            return _dbBaseProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters) > 0;
        }

        public static bool Delete(VersionMode model)
        {
            var command = _dbBaseProvider.CreateCommandStruct("ContractVersion", CommandMode.Delete);
            command.Filter = _dbBaseProvider.CreateCommandFilter();
            command.Filter.Condition = _dbBaseProvider.FormatFilterParam("ID");
            command.Filter.AddParam("ID", model.ID);
            command.Parser();
            return _dbBaseProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters) > 0;
        }

        public static List<VersionMode> GetVersion(int gameId)
        {
            var command = _dbBaseProvider.CreateCommandStruct("ContractVersion", CommandMode.Inquiry);
            command.Columns = "ID,SlnID,Title";
            command.OrderBy = "SlnID ASC,ID ASC";
            command.Filter = _dbBaseProvider.CreateCommandFilter();
            command.Filter.Condition = _dbBaseProvider.FormatFilterParam("SlnID");
            command.Filter.AddParam("SlnID", gameId);
            command.Parser();
            var list = new List<VersionMode>();
            using (var reader = _dbBaseProvider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
            {
                while (reader.Read())
                {
                    VersionMode model = new VersionMode();
                    model.ID = reader["ID"].ToInt();
                    model.SlnID = reader["SlnID"].ToInt();
                    model.Title = reader["Title"].ToNotNullString();
                    list.Add(model);
                }
            }
            return list;
        }
        #endregion
    }
}