<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddParamInfo.aspx.cs" Inherits="ZyGames.ContractTools.AddParamInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>增加字段</title>
    <link rel="stylesheet" href="flashpeak.css">
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table align="center" style="height: 300px; width: 577px">
            <tr>
                <td style="text-align: left" colspan="2">
                    &nbsp;<a href="index.aspx?ID=<%=ContractID%>&slnID=<%=SlnID%>">返回</a>
                </td>
            </tr>
            <tr>
                <td style="text-align: left">
                    协议ID
                </td>
                <td style="text-align: left">
                    <asp:Label ID="LabType" runat="server" Text="LabType"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: left">
                    <p style="text-align: left">
                        参数类型</p>
                </td>
                <td style="text-align: left">
                    <asp:DropDownList ID="droParamType" runat="server" Height="22px" Width="150px">
                        <asp:ListItem Value="1">请求</asp:ListItem>
                        <asp:ListItem Value="2" Selected="True">返回</asp:ListItem>
                    </asp:DropDownList>
                    <strong style="color: Red">*</strong>
                </td>
            </tr>
            <tr>
                <td style="text-align: left" class="style1">
                    字段
                </td>
                <td style="text-align: left" class="style1">
                    <asp:TextBox ID="txtField" runat="server" Width="150px"></asp:TextBox>
                    <strong style="color: Red">*<asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                        runat="server" ControlToValidate="txtField" ErrorMessage="字段名要符合变量命名规则" ValidationExpression="[_a-zA-Z][_a-zA-Z0-9]*"></asp:RegularExpressionValidator>
                    </strong>
                </td>
            </tr>
            <tr>
                <td style="text-align: left">
                    参数描述
                </td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtDescption" runat="server" TextMode="MultiLine" Height="64px"
                        Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: left">
                    字段类型
                </td>
                <td style="text-align: left">
                    <asp:DropDownList ID="droFieldType" runat="server" Height="23px" Width="150px">
                        <asp:ListItem Value="1">Int</asp:ListItem>
                        <asp:ListItem Value="2">String</asp:ListItem>
                        <asp:ListItem Value="3">Word(Short,Smallint)</asp:ListItem>
                        <asp:ListItem Value="4">Byte</asp:ListItem>
                        <asp:ListItem Value="5">Record(循环开始)</asp:ListItem>
                        <asp:ListItem Value="6">End(循环开始)</asp:ListItem>
                        <asp:ListItem Value="7">Head</asp:ListItem>
                    </asp:DropDownList>
                    <strong style="color: Red">*</strong>
                </td>
            </tr>
            <tr>
                <td style="text-align: left">
                    是否必填
                </td>
                <td style="text-align: left">
                    <asp:DropDownList ID="droRrequired" runat="server" Height="20px" Width="150px">
                        <asp:ListItem Value="false">否</asp:ListItem>
                        <asp:ListItem Value="true" Selected>是</asp:ListItem>
                    </asp:DropDownList>
                    <strong style="color: Red">*</strong>
                </td>
            </tr>
            <tr>
                <td style="text-align: left">
                    默认值
                </td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtFieldValue" runat="server" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: left">
                    最小值范围
                </td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtMinValue" runat="server" Width="150px" Text="0"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: left">
                    最大值范围
                </td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtMaxValue" runat="server" Width="150px" Text="0"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: left">
                    说明
                </td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Height="79px" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: left" colspan="2">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnSubmit" runat="server" Text="提交" Width="75px" OnClick="btnSubmit_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnEmpty" runat="server" Text="清空" Width="64px" OnClick="btnEmpty_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
