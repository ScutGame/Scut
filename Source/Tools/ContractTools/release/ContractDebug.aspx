<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractDebug.aspx.cs" Inherits="ContractTools.WebApp.ContractDebug" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>协议调试</title>
    <link href="skin.css" rel="stylesheet" />
    <style type="text/css">
        .table {
            width: 100%;
        }

            .table span, .table input[type="text"], .table textarea {
                padding: 0 2px;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="padding: 10px 5px;">
            <table class="table">
                <tr>
                    <td style="width: 15%; text-align: right;"><span>服务地址:</span></td>
                    <td>
                        <asp:TextBox ID="txtServerUrl" runat="server" Width="99%"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;"><span>协议:</span></td>
                    <td>
                        <asp:DropDownList ID="ddlContract" runat="server" Width="99%" AutoPostBack="True" OnSelectedIndexChanged="OnContractSelectedIndexChanged"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;"><span>测试参数:</span></td>
                    <td>
                        <asp:TextBox ID="txtParams" runat="server" Width="99%" Height="100px" TextMode="MultiLine"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="text-align: right;"><span>Sid:</span></td>
                    <td>
                        <asp:TextBox ID="txtSid" runat="server" Width="99%" OnTextChanged="OnRefresh" AutoPostBack="True"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="text-align: right;"><span>Uid:</span></td>
                    <td>
                        <asp:TextBox ID="txtUid" runat="server" Width="99%" Text="0" OnTextChanged="OnRefresh" AutoPostBack="True"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="text-align: right;"><span>MsgId:</span></td>
                    <td>
                        <asp:TextBox ID="txtMsgId" runat="server" Width="99%" Text="0" OnTextChanged="OnRefresh" AutoPostBack="True"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="text-align: right;"><span>St:</span></td>
                    <td>
                        <asp:TextBox ID="txtSt" runat="server" Width="99%" Text="" OnTextChanged="OnRefresh" AutoPostBack="True"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="text-align: right;"><span>协议版本:</span></td>
                    <td>
                      <asp:DropDownList ID="txtPrtl" runat="server" Width="99%" OnSelectedIndexChanged="OnRefresh">
                          <asp:ListItem Value="1" Selected="True">1.1</asp:ListItem>
                          <asp:ListItem Value="0">1.0</asp:ListItem>
                      </asp:DropDownList>    
                    </td>
                </tr>
                 <tr>
                    <td style="text-align: right; vertical-align: top;"><span>扩展头属性协议:</span></td>
                    <td>
                        <asp:DropDownList ID="ddHeadProperty" runat="server" Width="99%"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;"><span>Post参数:</span></td>
                    <td>
                        <asp:TextBox ID="txtPostParam" runat="server" TextMode="MultiLine" ReadOnly="True" Height="50px" Width="99%" Wrap="true"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="text-align: right;"><span>输出格式:</span></td>
                    <td>
                        <asp:DropDownList ID="ddResponseShowType" runat="server">
                            <asp:ListItem Value="1" Selected="True">Table</asp:ListItem>
                            <asp:ListItem Value="2">Json</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center; padding-top: 20px;">
                        <asp:Button ID="btnSend" runat="server" Text="发送" CssClass="btn" OnClick="OnSendClick" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center; padding-top: 20px;">
                        <div id="dvResult" runat="server" style="text-align: center; width: 100%; max-width: 730px; overflow: auto;"></div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
