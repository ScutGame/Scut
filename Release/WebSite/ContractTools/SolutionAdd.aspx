<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolutionAdd.aspx.cs" Inherits="ZyGames.ContractTools.SolutionAdd" %>

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
    <title>增加项目</title>
    <link rel="stylesheet" href="flashpeak.css">
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <center>
            <table style="height: 201px; width: 484px">
                <tr>
                    <td>
                        <a href="SolutionsList.aspx">返回</a>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        项目名称
                    </td>
                    <td class="style5" align="left">
                        <asp:TextBox ID="txtDescption" runat="server" Width="164px"></asp:TextBox>
                        <strong style="color: Red">*<strong style="color: Red"><asp:RequiredFieldValidator
                            ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDescption"
                            ErrorMessage="项目名称不能为空"></asp:RequiredFieldValidator>
                        </strong></strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        单元测试URL
                    </td>
                    <td class="style5" align="left">
                        <asp:TextBox ID="txtUrl" runat="server" Width="385px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        命名空间
                    </td>
                    <td class="style5" align="left">
                        <asp:TextBox ID="txtNamespace" runat="server" Width="385px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        引用空间
                    </td>
                    <td class="style5" align="left">
                        <asp:TextBox ID="txtRefNamespace" runat="server" Width="387px" Height="194px" 
                            TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        游戏ID
                    </td>
                    <td class="style5" align="left">
                        <asp:TextBox ID="txtGameID" runat="server" Width="387px" Height="194px" 
                            TextMode="MultiLine"></asp:TextBox>
                            <strong style="color: Red">*<strong style="color: Red"><asp:RequiredFieldValidator
                            ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtGameID"
                            ErrorMessage="游戏ID不能为空"></asp:RequiredFieldValidator>
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
