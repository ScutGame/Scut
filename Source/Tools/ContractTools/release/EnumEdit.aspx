<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="EnumEdit.aspx.cs" Inherits="ContractTools.WebApp.EnumEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main">
        <div class="content">
            <table style="width: 100%; text-align: center; background: #f0f0f0;padding: 15px 5px;">
                <tr>
                    <td colspan="2">
                        <strong style="color: Red">
                       
                        <asp:Button ID="btRefreshCache" runat="server" Text="刷新缓存" 
                            OnClick="btRefreshCache_Click" CssClass="btn"/>
                        </strong>
                    </td>
                </tr>
                <tr>
                    <td style="width: 40%; text-align: right;">
                        枚举名称</td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtName" runat="server" Width="160px"></asp:TextBox>
                        <strong style="color: Red">*</strong></td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        枚举说明</td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtDescription" runat="server" Width="160px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        枚举项
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtValueInfo" runat="server" Height="131px" TextMode="MultiLine"
                            Width="404px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Literal ID="EditKey" runat="server" Visible="False"></asp:Literal>
                       
                        <asp:Button ID="btAddEnum" runat="server" Text="增加" OnClick="butSubmit_Click" CssClass="btn"/>
                    &nbsp;&nbsp;<asp:Button ID="btCancelButton" runat="server" Text="取消" 
                            OnClick="btCancelButton_Click" CssClass="btn"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding-top: 15px;">
                        <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="grid" 
                            DataKeyNames="id" onrowcommand="GridView_RowCommand" >
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
                            <AlternatingRowStyle CssClass="grid-row-alt" />
                            <FooterStyle CssClass="grid-row" />
                            <HeaderStyle CssClass="grid-head" />
                            <RowStyle CssClass="grid-row" />
                            <SelectedRowStyle CssClass="grid-row-select" />
                            <EditRowStyle CssClass="grid-row-select"/>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
