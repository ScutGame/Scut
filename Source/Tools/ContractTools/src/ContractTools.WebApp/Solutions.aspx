<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Solutions.aspx.cs" Inherits="ContractTools.WebApp.Solutions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main">
        <div class="content">
            <table style="width: 100%; text-align: center; background: #f0f0f0;padding: 15px 5px;" >
                <tr>
                    <td style="width: 40%;text-align: right">
                        项目名称
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtDescption" runat="server" Width="385px"></asp:TextBox>
                        <strong style="color: Red">*</strong>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        项目命名空间
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtNamespace" runat="server" Width="385px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        游戏ID
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtGameID" runat="server" Width="385px"></asp:TextBox>
                            <strong style="color: Red">*</strong>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        调试地址
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtUrl" runat="server" Width="385px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        命名空间引用
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtRefNamespace" runat="server" Width="387px" Height="50px" 
                            TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td style="text-align: left;padding-top: 15px;">
                        <asp:Button ID="butSubmit" runat="server" Text="提交" OnClick="butSubmit_Click"  CssClass="btn"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding-top: 15px;">
                        
                         <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="grid" 
                            DataKeyNames="SlnID,SlnName" OnRowCancelingEdit="GridView1_RowCancelingEdit"
                            OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" OnRowDeleting="GridView1_RowDeleting">
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="10%">
                                    <EditItemTemplate>
                                        <asp:Label ID="IDLabel" runat="server" Text='<%# Bind("SlnID") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("SlnID") %>'></asp:Label>
                                    </ItemTemplate>

                <ItemStyle Width="10%"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="项目方案" ItemStyle-Width="10%">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="SlnName" runat="server" Text='<%# Bind("SlnName") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSlnName" runat="server" Text='<%# Bind("SlnName") %>'></asp:Label>
                                    </ItemTemplate>

                <ItemStyle Width="10%"></ItemStyle>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="游戏ID" ItemStyle-Width="10%">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="gameid" runat="server" Text='<%# Bind("gameid") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblgameid" runat="server" Text='<%# Bind("gameid") %>'></asp:Label>
                                    </ItemTemplate>

                <ItemStyle Width="10%"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="调试地址" ItemStyle-Width="10%">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="Url" runat="server" Text='<%# Bind("Url") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblUrl" runat="server" Text='<%# Bind("Url") %>'></asp:Label>
                                    </ItemTemplate>

                <ItemStyle Width="10%"></ItemStyle>
                                </asp:TemplateField>
                
                                <asp:TemplateField HeaderText="命名空间" ItemStyle-Width="10%">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="Namespace" runat="server" Text='<%# Bind("Namespace") %>' Width="300px"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblNamespace" runat="server" Text='<%# Bind("Namespace") %>'></asp:Label>
                                    </ItemTemplate>

                <ItemStyle Width="10%"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="命名空间引用" ItemStyle-Width="10%">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="RefNamespace" runat="server" TextMode="MultiLine" Text='<%# Bind("RefNamespace") %>' Width="300px" Height="50"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRefNamespace" runat="server" Text='<%# Bind("RefNamespace") %>'></asp:Label>
                                    </ItemTemplate>

                <ItemStyle Width="10%"></ItemStyle>
                                </asp:TemplateField>
                                <asp:CommandField HeaderText="操作" ItemStyle-HorizontalAlign="Center" ShowDeleteButton="True"
                                    ShowEditButton="True">
                                    <ItemStyle Width="10%" />
                                </asp:CommandField>
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
