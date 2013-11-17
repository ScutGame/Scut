<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientConfigInfo.aspx.cs" Inherits="ZyGames.ContractTools.ClientConfigInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>服务配置数据下发</title>
    <link rel="stylesheet" href="flashpeak.css">
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align:center">
        <h1>服务配置数据下发(ID=<%=ContractID%>)</h1>
    </div>
    <div style="margin: 0 auto; width: 1024px; padding: 5px 0; border: solid 0px #000;">
        <div>
            <a href="index.aspx?ID=<%=ContractID%>&slnID=<%=SlnID%>"><h3>返回首页</h3></a>
        </div>
        <div style="text-align:center">
            <table style="width:90%; padding:5px;" border="0" cellpadding="8" cellspacing="0">
                <tr>
                    <td style="width:25%; text-align:right">服务器地址：</td>
                    <td style="width:75%; text-align:left">
                        <asp:TextBox ID="txtServerUrl" runat="server" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">协议ID：</td>
                    <td align="left">
                        <asp:DropDownList ID="ddlContract" runat="server" Width="98%">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right">版本号</td>
                    <td>
                        <asp:TextBox ID="txtVersion" runat="server" Width="98%" Text="0"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="text-align:right">主键ID</td>
                    <td>
                        <asp:TextBox ID="txtKeyName" runat="server" Width="98%"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="center"  colspan="2">
                        <asp:Button ID="btnTest" runat="server" Text="生成Lua" onclick="btnTest_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"><hr /></td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <asp:TextBox ID="txtResponse" runat="server" Height="452px" Width="99%" 
                            TextMode="MultiLine" Wrap="False"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
