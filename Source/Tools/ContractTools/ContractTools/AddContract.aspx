<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddContract.aspx.cs" Inherits="ZyGames.ContractTools.AddContract" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script>
function readTxt() 
{ 
var x=document.getElementById("txtID");
if(x==null)
{
alert('请填写带有*框');
}
return false;
}
function readTxts() 
{
var x=document.getElementById("txtDescption");
if(x==null)
{
alert('请填写带有*框');
}
}
return false;
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>增加修改协议</title>
    <link rel="stylesheet" href="flashpeak.css">
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <center>
            <table style="height: 201px; width: 484px">
                <tr>
                    <td>
                        <a href="index.aspx?ID=<%=ContractID%>&slnID=<%=SlnID%>">返回</a>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                 <tr>
                    <td>
                        接口分类
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAgreement" runat="server" Width="236px" AutoPostBack="True" >
                    </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        协议ID
                    </td>
                    <td class="style3" align="left">
                        <asp:TextBox ID="txtID" runat="server" Width="160px"></asp:TextBox>
                        <strong style="color: Red">*<asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                            runat="server" ControlToValidate="txtID" ErrorMessage="协议ID不能为空"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtID"
                                ErrorMessage="协议ID必须为数字" ValidationExpression="\d{1,10}"></asp:RegularExpressionValidator>
                        </strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        协议描述
                    </td>
                    <td class="style5" align="left">
                        <asp:TextBox ID="txtDescption" runat="server" Height="60px" TextMode="MultiLine"
                            Width="164px"></asp:TextBox>
                        <strong style="color: Red">*<strong style="color: Red"><asp:RequiredFieldValidator
                            ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDescption"
                            ErrorMessage="描述不能为空"></asp:RequiredFieldValidator>
                        </strong></strong>
                    </td>
                </tr>
                <tr>
                    <td class="style1" colspan="2">
                        <asp:Button ID="butSubmit" runat="server" Text="提交" OnClick="butSubmit_Click" />
                    </td>
                </tr>
            </table>
        </center>
    </div>
    </form>
</body>
</html>
