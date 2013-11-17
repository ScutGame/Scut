<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgreementList.aspx.cs"
    Inherits="ZyGames.ContractTools.AgreementList" %>

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
         <asp:DropDownList ID="ddlSolution" runat="server" Width="236px" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlSolution_SelectedIndexChanged">
                    </asp:DropDownList>   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <a href="index.aspx"><strong>返回</strong></a>&nbsp;&nbsp;&nbsp;&nbsp;
            <a href="AgreementAdd.aspx?gameid=<%=ddlSolution.SelectedValue %>"><strong style=" color:Red;">游戏接口分类增加</strong></a>
        </div>
        <br>
        <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
            AutoGenerateColumns="False" Width="60%" DataKeyNames="AgreementID,Title" OnRowCancelingEdit="GridView1_RowCancelingEdit"
            OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" OnRowDeleting="GridView1_RowDeleting">
            <RowStyle BackColor="#E3EAEB" />
            <Columns>
                <asp:TemplateField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                    <EditItemTemplate>
                        <asp:Label ID="IDLabel" runat="server" Text='<%# Bind("AgreementID") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="IDLabel" runat="server" Text='<%# Bind("AgreementID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="项目方案" ItemStyle-Width="10%" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                    <EditItemTemplate>
                        
                         <asp:Label ID="SlnName" runat="server" Text='<%# Bind("SlnName") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                       <asp:Label ID="lblUrl" runat="server" Text='<%# Bind("SlnName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="项目分类" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                    <EditItemTemplate>
                        <asp:TextBox ID="Title" runat="server" Text='<%# Bind("Title") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblUrl" runat="server" Text='<%# Bind("Title") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="描述" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Center">
                    <EditItemTemplate>
                        <asp:TextBox ID="Describe" runat="server" Text='<%# Bind("Describe") %>' Width="300px"></asp:TextBox>
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
