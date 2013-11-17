using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.IO;

namespace CustomerControl
{
    public class CustomTextBox : System.Web.UI.WebControls.TextBox
    {
        private TBType tbtype;
        public TBType TBTYPE
        {
            get { return tbtype; }
            set { tbtype = value; }
        }
        public enum TBType
        {
            onlyint = 1,
            onlyfloat = 2,
            onlyint2 = 3
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (tbtype == TBType.onlyint)
            {
                this.Attributes.Add("onkeypress", @"var k=event.keyCode; if((k<=57 && k>=48)){ if(k==46) { if(this.value.indexOf('.')>=0) { return false; } else { if(this.value==''){return false;}return true;} }}else return false;");
                this.Attributes.Add("onpaste", "return false");
                this.Style.Add("ime-mode", "disabled");
                this.ToolTip = "只能输入数字";
            }
            else if (tbtype == TBType.onlyfloat)
            {
                this.Attributes.Add("onkeypress", @"var k=event.keyCode; if((k==46)||(k<=57 && k>=48)){ if(k==46) { if(this.value.indexOf('.')>=0) { return false; } else { if(this.value==''){return false;}return true;} }}else return false;");
                this.Attributes.Add("onpaste", "return false");
                this.Style.Add("ime-mode", "disabled");
                this.ToolTip = "只能输入整数和小数";
            }
            else if (tbtype == TBType.onlyint2)
            {
                this.Attributes.Add("onKeyUp", "this.value=DBC2SBC(this.value);");
                this.ToolTip = "只能输入整数";
                string dopost = "";
                dopost += "<script type=text/javascript>\n";
                dopost += "      function DBC2SBC(str){\n";
                dopost += "          var i;var result='';for(i=0;i<str.length;i++) {if(str.charCodeAt(i)>65295 && str.charCodeAt(i)<65306) result+=String.fromCharCode(str.charCodeAt(i)-65248); else if(str.charCodeAt(i)>47 && str.charCodeAt(i)<58) result+=String.fromCharCode(str.charCodeAt(i)); } return result; } \n";
                dopost += "</script>\n";
                if (!Page.ClientScript.IsClientScriptIncludeRegistered(this.GetType(), "DBC2SBC"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "DBC2SBC", dopost);
                }
            }
            else
            { }
        }
    }

    public class AutoTextArea : System.Web.UI.WebControls.TextBox
    {
        [DefaultValue(200)]
        public int MaxHeight
        {
            get
            {
                object obj = ViewState["MaxHeight"];
                return obj == null ? 200 : (int)obj;
            }
            set
            {
                if (value >= MinHeight)
                    ViewState["MaxHeight"] = value;
            }
        }

        private const int min = 16;
        [DefaultValue(16)]
        public int MinHeight
        {
            get
            {
                object obj = ViewState["MinHeight"];
                return obj == null ? 16 : (int)obj;
            }
            set
            {
                if (value >= min)
                    ViewState["MinHeight"] = value;
            }
        }
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
        }

        protected override void OnInit(EventArgs e)
        {    
            base.OnInit(e);
            this.Attributes.Add("style", "display: block; overflow: visible;");
            this.Attributes["minHeight"] = this.MinHeight.ToString();
            this.Attributes["maxHeight"] = this.MaxHeight.ToString();

            base.Attributes["onpropertychange"] = "this.style.height=(this.scrollHeight>this.maxHeight)?this.maxHeight:Math.max(this.minHeight,this.scrollHeight-4)";
            base.Attributes["onchange"] = "this.style.height=(this.scrollHeight>this.maxHeight)?this.maxHeight:Math.max(this.minHeight,this.scrollHeight-4)";

            if (base.Rows == 0)
            {
                base.Rows = 1;
            }
            base.TextMode = TextBoxMode.MultiLine;
        }

        //protected override void OnPreRender(EventArgs e)
        //{
        //    //this.Attributes["minHeight"] = this.MinHeight.ToString();
        //    //this.Attributes["maxHeight"] = this.MaxHeight.ToString();
        //    //if (this.Height == Unit.Empty)
        //    //{
        //    //    this.Height = this.MinHeight;
        //    //}
        //    //else
        //    //{
        //    //    this.Height = (int)Math.Max(this.MinHeight, this.Height.Value);
        //    //}
        //    base.OnPreRender(e);
        //}

        //protected override void Render(HtmlTextWriter output)
        //{
        //    base.Attributes["onpropertychange"] = "this.style.height=(this.scrollHeight>this.maxHeight)?this.maxHeight:Math.max(this.minHeight,this.scrollHeight-4)";
        //    base.Attributes["onchange"] = "this.style.height=(this.scrollHeight>this.maxHeight)?this.maxHeight:Math.max(this.minHeight,this.scrollHeight-4)";

        //    if (base.Rows == 0)
        //    {
        //        base.Rows = 1;
        //    }
        //    base.TextMode = TextBoxMode.MultiLine;
        //    base.Render(output);
        //}
    }
}