<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SyncModelInfo.aspx.cs" Inherits="ContractTools.WebApp.SyncModelInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>实体同步协议</title>
    <link rel="stylesheet" href="flashpeak.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width:1400px; float:left;padding-left:0px; margin-left:20px;">
            <table style="width: 100%; margin-left: 0px;">
                <tr>
                    <td style="width:140px"><label>实体Schema配置文件:</label></td>
                    <td>
                        <asp:FileUpload ID="FileUpload1" runat="server"/>
                        <span style="padding-left:5px"></span>
                        <asp:Button ID="BtnUp" runat="server" onclick="BtnUp_Click" Text="上传Schema" />
                        <span style="padding-left:20px"></span>
                        <asp:Button ID="btnDown" runat="server" Text="下载Schema" onclick="btnDown_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                <td colspan="2">
                    <asp:TextBox ID="txtContent" runat="server" Height="577px" TextMode="MultiLine" Width="100%"></asp:TextBox>
                </td>
                </tr>
            </table>   
    </div>
    </form>
</body>
</html>
