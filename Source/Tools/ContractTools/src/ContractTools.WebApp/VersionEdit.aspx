<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="VersionEdit.aspx.cs" Inherits="ContractTools.WebApp.VersionEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function readTxt() {
            var x = document.getElementById("txtID");
            if (x == null) {
                alert('请填写带有*框');
            }
            return false;
        }
        function readTxts() {
            var x = document.getElementById("txtDescption");
            if (x == null) {
                alert('请填写带有*框');
            }
        }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main">
         <div class="content">
             <table style="width: 100%; text-align: center; background: #f0f0f0;padding: 15px 5px;" >
                <tr>
                    <td style="width: 45%;text-align: right">
                        版本：
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox><strong style="color: Red">*</strong>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td style="text-align: left;padding-top: 15px;">
                        <asp:Button ID="butSubmit" runat="server" Text="提交" OnClick="butSubmit_Click" CssClass="btn"/>
                    </td>
                </tr>
                 <tr>
                     <td colspan="2" style="padding-top: 15px;">
             <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="grid" 
                            DataKeyNames="ID,Title" OnRowCancelingEdit="GridView1_RowCancelingEdit"
                            OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" OnRowDeleting="GridView1_RowDeleting">
                            <Columns>
                                <asp:TemplateField HeaderText="ID" ItemStyle-Width="10%">
                                    <EditItemTemplate>
                                        <asp:Label ID="IDLabel" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                    </ItemTemplate>

                <ItemStyle Width="10%"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="版本" ItemStyle-Width="10%">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="itTitle" runat="server" Text='<%# Bind("Title") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblTitle" runat="server" Text='<%# Bind("Title") %>'></asp:Label>
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
                        </asp:GridView></td>
                 </tr>
            </table>
        </div>
    </div>
</asp:Content>
