<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractList.aspx.cs" Inherits="ZyGames.ContractTools.ContractList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>复制协议</title>
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
                        项目方案
                    </td>
                    <td class="style3" align="left">
                        <asp:Label ID="lblSlnName" runat="server" Style="text-decoration: underline" />
                        <asp:TextBox ID="txtSlnID" runat="server" Visible="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        协议ID
                    </td>
                    <td class="style3" align="left">
                        <asp:DropDownList ID="ddContract" runat="server" Width="236px" 
                            AutoPostBack="True" onselectedindexchanged="ddContract_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Copy至项目
                    </td>
                    <td class="style5" align="left">
                        <asp:DropDownList ID="ddlSolution" runat="server" Width="236px" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Copy至项目协议ID
                    </td>
                    <td class="style3" align="left">
                        <asp:TextBox ID="txtCopyID" runat="server" Width="236px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style1" colspan="2">
                        <asp:Button ID="butSubmit" runat="server" Text="复制" OnClick="butSubmit_Click" Width="81px" />
                    </td>
                </tr>
            </table>
        </center>
    </div>
    </form>
</body>
</html>
