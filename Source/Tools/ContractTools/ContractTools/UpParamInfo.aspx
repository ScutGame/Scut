<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpParamInfo.aspx.cs" Inherits="ZyGames.ContractTools.UpParamInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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
                        <asp:DropDownList ID="ddlAgreement" runat="server" Width="236px" >
                    </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        协议ID
                    </td>
                    <td class="style5">
                        <asp:Label ID="IdLabel" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        协议描述
                    </td>
                    <td class="style5">
                        <asp:TextBox ID="txtDescption" runat="server" Height="67px" TextMode="MultiLine"
                            Width="173px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style1" colspan="2">
                        <asp:Button ID="butSubmit" runat="server" Text="修改" OnClick="butSubmit_Click" />
                    </td>
                </tr>
            </table>
        </center>
    </div>
    </form>
</body>
</html>
