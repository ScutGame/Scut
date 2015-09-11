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
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Scut.SMS.Config;
using ZyGames.Framework.Common;
using ZyGames.Framework.Data;

namespace Scut.SMS
{
    public static class TemplateFactory
    {
        private static string templateTestCase;
        private static string saveTempPath;
        private static DbBaseProvider dbProvider;

        static TemplateFactory()
        {
            LoadTemplate();
        }

        public static void Init()
        {
            var dbType = AppSetting.Current.Contract.DBType;
            string connectionString = string.Format("Data Source={0};Database={1};Uid={2};Pwd={3};",
                dbType == DBType.SQL && AppSetting.Current.Contract.Port > 0
                ? AppSetting.Current.Contract.Server + "," + AppSetting.Current.Contract.Port
                : AppSetting.Current.Contract.Server,
                AppSetting.Current.Contract.Database,
                AppSetting.Current.Contract.UserId,
                AppSetting.Current.Contract.Password);
            if (dbType == DBType.MySql && AppSetting.Current.Contract.Port > 0)
            {
                connectionString += string.Format("Port={0};", AppSetting.Current.Contract.Port);
            }
            string privodeType = AppSetting.Current.Contract.DBType == DBType.MySql ? "MySqlDataProvider" : "";
            dbProvider = DbConnectionProvider.CreateDbProvider("ContractData", privodeType, connectionString);
            saveTempPath = Path.Combine(MathUtils.RuntimePath, AppSetting.Current.Contract.CaseOutPath);
        }

        private static void LoadTemplate()
        {
            try
            {
                Init();
                System.Reflection.Assembly appDll = System.Reflection.Assembly.GetExecutingAssembly();
                Stream stream = appDll.GetManifestResourceStream("Scut.SMS.Template.TestCase.txt");
                using (var sr = new StreamReader(stream))
                {
                    templateTestCase = sr.ReadToEnd();
                }
            }
            catch (Exception)
            {
            }
        }

        public static string LoadTastCase(int actionId, string actionName, DataTable paramList)
        {
            string fileName = Path.Combine(saveTempPath, string.Format(AppSetting.Current.Contract.CaseNameFormat + ".cs", actionId));
            if (File.Exists(fileName))
            {
                return File.ReadAllText(fileName);
            }
            var tempTable = paramList.Select("Required=true and ContractID=" + actionId);
            return GenerateTestCate(actionId, actionName, tempTable);
        }

        public static string BuildTestCase(string gameName, TreeNodeCollection nodes, DataTable paramList, bool isReplace)
        {
            string path = Path.Combine(saveTempPath, gameName);
            foreach (TreeNode node in nodes)
            {
                string actionName = node.Text;
                int actionId;
                if (!int.TryParse(node.Tag.ToString(), out actionId) || actionId == 0)
                {
                    BuildTestCase(gameName, node.Nodes, paramList, isReplace);
                    continue;
                }
                var tempTable = paramList.Select("Required=true and ContractID=" + actionId);
                var content = GenerateTestCate(actionId, actionName, tempTable);
                SaveTestCase(content, gameName, actionId, isReplace);
            }

            return Path.GetFullPath(path);
        }

        private static string GenerateTestCate(int actionId, string actionName, DataRow[] paramList)
        {
            StringBuilder sbParam = new StringBuilder();
            foreach (var dataRow in paramList)
            {
                int type = dataRow["FieldType"].ToInt();
                string value = type == 2
                    ? string.Format("\"{0}\"", dataRow["FieldValue"].ToNotNullString())
                    : dataRow["FieldValue"].ToNotNullString();
                sbParam.AppendLine();
                sbParam.AppendFormat("            SetRequestParam(\"{0}\", {1});", dataRow["Field"], value);
            }
            return templateTestCase
                .Replace("#Desp#", actionName)
                .Replace("#Step#", string.Format(AppSetting.Current.Contract.CaseNameFormat, actionId))
                .Replace("#SetUrlElement#", sbParam.ToString());
        }

        public static DataTable ReadSolutions()
        {
            var command = dbProvider.CreateCommandStruct("Solutions", CommandMode.Inquiry, "SlnID,SlnName");
            command.OrderBy = "SlnID asc";
            command.Parser();
            using (var reader = dbProvider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
            {
                DataTable dt = new DataTable();
                dt.Load(reader);
                return dt;
            }
        }

        public static TreeNode[] ReadContract(int slnId)
        {
            var command = dbProvider.CreateCommandStruct("Contract", CommandMode.Inquiry);
            command.Columns = "ID,Descption,SlnID,AgreementID";
            command.OrderBy = "SlnID asc,ID asc";
            command.Filter = dbProvider.CreateCommandFilter();
            command.Filter.Condition = dbProvider.FormatFilterParam("SlnID");
            command.Filter.AddParam("SlnID", slnId);
            command.Parser();

            var contractList = new List<TreeNode>();
            using (var reader = dbProvider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
            {
                while (reader.Read())
                {
                    string id = reader["ID"].ToString();
                    var item = new TreeNode();
                    item.Text = String.Format("{0}-{1}", id, reader["Descption"]);
                    item.Tag = id;
                    contractList.Add(item);
                }
            }
            return contractList.ToArray();
        }

        public static DataTable ReadContractParam(int slnId)
        {
            var command = dbProvider.CreateCommandStruct("ParamInfo", CommandMode.Inquiry);
            command.Columns = "ID,ContractID,Field,FieldValue,FieldType,Required";
            command.OrderBy = "ContractID asc,SortID asc ,ID asc";
            command.Filter = dbProvider.CreateCommandFilter();
            command.Filter.Condition = string.Format("ParamType=1 and {0}", dbProvider.FormatFilterParam("SlnID"));
            command.Filter.AddParam("SlnID", slnId);
            command.Parser();

            using (var reader = dbProvider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
            {
                DataTable dt = new DataTable();
                dt.Load(reader);
                return dt;
            }
        }

        public static string SaveTestCase(string text, string gameName, int actionId, bool isReplace = true)
        {
            string path = Path.Combine(saveTempPath, gameName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = Path.Combine(path, string.Format(AppSetting.Current.Contract.CaseNameFormat + ".cs", actionId));
            if (!isReplace && File.Exists(fileName))
            {
                return fileName;
            }
            using (var sr = File.CreateText(fileName))
            {
                sr.Write(text);
                sr.Flush();
            }
            return fileName;
        }
    }
}