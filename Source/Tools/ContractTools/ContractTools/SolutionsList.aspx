<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolutionsList.aspx.cs"
    Inherits="ZyGames.ContractTools.SolutionsList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>项目方案</title>
    <link rel="stylesheet" href="flashpeak.css">
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin: 0 auto; width: 90%; padding: 5px 0; border: solid 0px #000;">
        <div>
            <a href="SolutionAdd.aspx">增加</a>
        </div>
        <br>
        <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
            AutoGenerateColumns="False" Width="100%" DataKeyNames="SlnID,SlnName" OnRowCancelingEdit="GridView1_RowCancelingEdit"
            OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" OnRowDeleting="GridView1_RowDeleting">
            <RowStyle BackColor="#E3EAEB" />
            <Columns>
                <asp:TemplateField ItemStyle-Width="10%">
                    <EditItemTemplate>
                        <asp:Label ID="IDLabel" runat="server" Text='<%# Bind("SlnID") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="IDLabel" runat="server" Text='<%# Bind("SlnID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="项目方案" ItemStyle-Width="10%">
                    <EditItemTemplate>
                        <asp:TextBox ID="SlnName" runat="server" Text='<%# Bind("SlnName") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblSlnName" runat="server" Text='<%# Bind("SlnName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="游戏ID" ItemStyle-Width="10%">
                    <EditItemTemplate>
                        <asp:TextBox ID="gameid" runat="server" Text='<%# Bind("gameid") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblgameid" runat="server" Text='<%# Bind("gameid") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Url" ItemStyle-Width="10%">
                    <EditItemTemplate>
                        <asp:TextBox ID="Url" runat="server" Text='<%# Bind("Url") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblUrl" runat="server" Text='<%# Bind("Url") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="命名空间" ItemStyle-Width="10%">
                    <EditItemTemplate>
                        <asp:TextBox ID="Namespace" runat="server" Text='<%# Bind("Namespace") %>' Width="300px"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblNamespace" runat="server" Text='<%# Bind("Namespace") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="引用命名空间" ItemStyle-Width="10%">
                    <EditItemTemplate>
                        <asp:TextBox ID="RefNamespace" runat="server" TextMode="MultiLine" Text='<%# Bind("RefNamespace") %>' Width="300px" Height="50"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblRefNamespace" runat="server" Text='<%# Bind("RefNamespace") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField HeaderText="操作" ItemStyle-HorizontalAlign="Center" ShowDeleteButton="True"
                    ShowEditButton="True">
                    <ItemStyle Width="10%" />
                </asp:CommandField>
            </Columns>
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#7C6F57" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
    </div>
    </form>
</body>
</html>
