<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="ZyGames.ContractTools.Search" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>查找结果</title>
    <link rel="stylesheet" href="flashpeak.css">
    <style type="text/css">
        .style1
        {
            height: 96px;
        }
        .style2
        {
            height: 9px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
     &nbsp;<div>
    
            <table style="height: 201px; width: 853px; margin-left: 0px;">
                <tr>
                    <td class="style2">
                        <a href="index.aspx?ID=<%=ContractID%>&slnID=<%=SlnID%>">返回</a>
                    </td>
                    <td class="style2" align="center">
                    <asp:TextBox ID="SearchTextBox" runat="server" Width="589px"></asp:TextBox>
                    <asp:Button ID="SearchButton" runat="server" Text="查找" 
                            onclick="SearchButton_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="style1" colspan="2">
                        <asp:Literal ID="ResultLiteral" runat="server"></asp:Literal>
    <center>
                        <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="False" 
                            ShowHeader="False" Width="80%">
                            <Columns>
                                <asp:TemplateField HeaderText="链接">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# string.Format("index.aspx?ID={0}&slnID={1}", Eval("ID"),Eval("slnid"))%>' Text='<%# string.Format("{0}_{1}", Eval("ID"), Eval("descption")) %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView></center>
                    </td>
                </tr>
            </table>
    
    </div>
    </form>
</body>
</html>
