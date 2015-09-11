<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AgreementEdit.aspx.cs" Inherits="ContractTools.WebApp.AgreementEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main">
        <div class="content">
            <table style="width: 100%; text-align: center; background: #f0f0f0;padding: 15px 5px;" >
                <tr>
                    <td style="width: 40%;text-align: right">类别名</td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtTitle" runat="server" Width="164px"></asp:TextBox>
                        <strong style="color: Red">*</strong>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">描述</td>
                    <td style="text-align: left;">
                       
                        <asp:TextBox ID="txtDescribe" runat="server" Width="387px" Height="100px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="butSubmit" runat="server" Text="提交" OnClick="butSubmit_Click" CssClass="btn"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding-top: 15px;">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"  Width="100%" CssClass="grid" 
                             DataKeyNames="AgreementID,Title" OnRowCancelingEdit="GridView1_RowCancelingEdit"
                            OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" OnRowDeleting="GridView1_RowDeleting">
                            <Columns>
                                <asp:TemplateField  HeaderText="ID" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                    <EditItemTemplate>
                                        <asp:Label ID="lblID" runat="server" Text='<%# Bind("AgreementID") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="IDLabel" runat="server" Text='<%# Bind("AgreementID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="类别名" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="Title" runat="server" Text='<%# Bind("Title") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblTitle" runat="server" Text='<%# Bind("Title") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="描述" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Center">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="Describe" runat="server" Text='<%# Bind("Describe") %>' Width="300px" TextMode="MultiLine"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                      <asp:Label ID="Describe" runat="server" Text='<%# Bind("Describe") %>'></asp:Label>
                       
                                    </ItemTemplate>
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
