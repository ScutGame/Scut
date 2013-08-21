<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addenum.aspx.cs" Inherits="ZyGames.ContractTools.addenum" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>增加/修改协议</title>
    <link rel="stylesheet" href="flashpeak.css">
  </head>
<body>
    <form id="form1" runat="server">
    <div>
    <center>
            <table style="height: 201px; width: 853px; margin-left: 0px;">
                <tr>
                    <td>
                        <a href="index.aspx?ID=<%=ContractID%>&slnID=<%=SlnID%>">返回</a>
                    </td>
                    <td>
                        <strong style="color: Red">
                       
                        <asp:Button ID="btRefreshCache" runat="server" Text="刷新缓存" 
                            OnClick="btRefreshCache_Click" />
                        </strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        枚举名称</td>
                    <td class="style3" align="left">
                        <asp:TextBox ID="txtName" runat="server" Width="160px"></asp:TextBox>
                        <strong style="color: Red">*</strong></td>
                </tr>
                <tr>
                    <td>
                        枚举说明</td>
                    <td class="style5" align="left">
                        <asp:TextBox ID="txtDescription" runat="server" Width="160px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        枚举描述<br />
                    </td>
                    <td class="style5" align="left">
                        <asp:TextBox ID="txtValueInfo" runat="server" Height="131px" TextMode="MultiLine"
                            Width="404px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Literal ID="EditKey" runat="server" Visible="False"></asp:Literal>
                        <strong style="color: Red">
                       
                        <asp:Button ID="btAddEnum" runat="server" Text="增加" OnClick="butSubmit_Click" />
                    &nbsp;<asp:Button ID="btCancelButton" runat="server" Text="取消" 
                            OnClick="btCancelButton_Click" />
                        </strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        枚举列表</td>
                    <td class="style5" align="left">
                        <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="False" 
                            DataKeyNames="id" onrowcommand="GridView_RowCommand" 
                            Width="753px">
                            <Columns>
                                <asp:BoundField DataField="enumName" HeaderText="名称">
                                <ControlStyle Width="5%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="enumDescription" HeaderText="枚举说明" />
                                <asp:TemplateField HeaderText="选择" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" 
                                            CommandName="sel" Text="选择"  CommandArgument='<%# Eval("id") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ControlStyle Width="30px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="删除" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="false" 
                                            CommandName="del"  CommandArgument='<%# Eval("id") %>'  Text="删除"  OnClientClick="return confirm('是否删除')"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ControlStyle Width="30px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <strong style="color: Red">
                    &nbsp;&nbsp;
                        </strong>
                    </td>
                </tr>
                <tr>
                    <td class="style1" colspan="2">
                        &nbsp;</td>
                </tr>
            </table>
    </center>
    </div>
    </form>
</body>
</html>
