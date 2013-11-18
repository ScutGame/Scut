<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UnitTest.aspx.cs" Inherits="ZyGames.ContractTools.UnitTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>服务端单元测试</title>
    <link rel="stylesheet" href="flashpeak.css">
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align:center">
        <h1>单元测试(ID=<%=ContractID%>)</h1>
    </div>
    <div style="margin: 0 auto; width: 1024px; padding: 5px 0; border: solid 0px #000;">
        <div>
            <a href="index.aspx?ID=<%=ContractID%>&slnID=<%=SlnID%>"><h3>返回首页</h3></a>
        </div>
        <div style="text-align:center">
            <table style="width:100%; padding:5px;" border="0" cellpadding="5" cellspacing="0">
                <tr>
                    <td style="width:15%; text-align:left">服务器地址：</td>
                    <td style="width:85%; text-align:left">
                        <asp:TextBox ID="txtServerUrl" runat="server" Width="80%"></asp:TextBox>(socket:IP+端口或域名+端口如192.168.1.102:9500或xy.36you.net:9500)
                    </td>
                </tr>
                <tr>
                    <td align="left">前置执行协议ID：</td>
                    <td align="left">
                       <asp:TextBox ID="txtMoreContrats" runat="server" Width="80%"></asp:TextBox>(多个以,分隔)
                    </td>
                </tr>
                <tr>
                    <td align="left">协议ID：</td>
                    <td align="left">
                        <asp:DropDownList ID="ddlContract" runat="server" Width="80%" 
                            AutoPostBack="True" onselectedindexchanged="ddlContract_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                  <tr>
                    <td align="left">游戏ID：</td>
                    <td align="left">
                           
                        <asp:Label ID="lbtGmeID" runat="server" Text="1"></asp:Label>
                    </td>
                </tr>
                  <tr>
                    <td align="left">游戏分服列表：</td>
                    <td align="left">
                        <asp:DropDownList ID="ddlServerID" runat="server" Width="80%" >
                        </asp:DropDownList>(非socket无视)
                    </td>
                </tr>
                <tr>
                    <td align="left">参数：</td>
                    <td align="left">
                        <asp:TextBox ID="ParamListTextBox" runat="server" Width="80%"></asp:TextBox>(多个以,分隔)
                    </td>
                </tr>
                <tr>
                    <td align="left">PassportID：</td>
                    <td align="left">
                        <asp:TextBox ID="txtPassport" runat="server" Width="80%"></asp:TextBox>(必填)
                    </td>
                </tr>
                <tr>
                    <td align="left">Password：</td>
                    <td align="left">
                        <asp:TextBox ID="txtPassword" runat="server" Width="80%"></asp:TextBox>(必填)
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="left" style=" font-size:15px;">
                        <asp:Button ID="btnTest" runat="server" Text="http测试" Width="100px" onclick="btnTest_Click"/>
                         <asp:Button ID="socketBtn" runat="server" Text="socket测试" Width="100px" 
                            onclick="socketBtn_Click" />
                        &nbsp;&nbsp;
                        <asp:CheckBox ID="ckResponse" runat="server" Text="输出全部结果" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="left">
                        <asp:Literal ID="LinkUrlLiteral" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"><hr /></td>
                </tr>
                <tr>
                    <td align="left">UserID：</td>
                    <td align="left">
                        <table border="0" width="90%">
                            <tr>
                                <td style="width:30%"><asp:TextBox ID="txtUserID" runat="server" Width="90%"></asp:TextBox></td>
                                <td style="width:10%" align="right">SessionID：</td>
                                <td style="width:30%" align="left">
                                    <asp:TextBox ID="txtSessionID" runat="server" Width="100%"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        
                    </td>
                </tr>
                <tr>
                    <td colspan="2"><hr /></td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <asp:Label ID="lblResponse" runat="server" Text="" Width="100%"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
