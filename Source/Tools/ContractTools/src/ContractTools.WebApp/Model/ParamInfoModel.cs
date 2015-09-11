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
using System.Web;

namespace ContractTools.WebApp.Model
{
    public class ParamInfoModel
    {
        public int ID
        {
            get;
            set;
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
            get;
            set;
        }
        /// <summary>
        /// 1:Request; 2:Response
        /// </summary>
        public int ParamType
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Field
        {
            get;
            set;
        }

        public int FieldTypeId
        {
            get { return (int)FieldType; }
        }

        /// <summary>
        /// 
        /// </summary>
        public FieldType FieldType
        {
            get;
            set;
        }
        /// <summary>
        /// 不使用,与Remark合并
        /// </summary>
        public string Descption
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>
        public string FieldValue
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Required
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int SortID
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Creator
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Modifier
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ModifyDate
        {
            get;
            set;
        }

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
        /// <summary>
        /// 版本号
        /// </summary>
        public int VerID
        {
            get;
            set;
        }

        /// <summary>
        /// 层级深度
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// List显示
        /// </summary>
        public string ComboxDescp
        {
            get
            {
                string str = string.Format("{0}<{1}>{2}",
                    FormatSpace(),
                    FieldType,
                    string.IsNullOrEmpty(Remark) ? Field : Remark
                );

                return str;
            }
        }

        public string DepthTypeDescp { get { return FormatSpace() + FieldType; } }

        private string FormatSpace()
        {
            string preStr = "";
            string spaceStr = HttpUtility.HtmlDecode("&nbsp;");
            int count = (Depth - 1) * 4;
            for (int i = 0; i < count; i++)
            {
                preStr += spaceStr;
            }
            return Depth > 0
                ? preStr + "├" + spaceStr
                : "";
        }
    }
}