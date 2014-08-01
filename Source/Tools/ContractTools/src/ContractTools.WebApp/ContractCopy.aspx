<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ContractCopy.aspx.cs" Inherits="ContractTools.WebApp.ContractCopy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main">
        <div class="content">
             <table style="width: 100%; text-align: center; background: #f0f0f0;padding: 15px 5px;">
                <tr>
                    <td style="text-align: right; width: 40%">
                        项目方案
                    </td>
                    <td style="text-align: left;">
                        <asp:Label ID="lblSlnName" runat="server" Style="text-decoration: underline;font-weight: bold;" />
                        <asp:TextBox ID="txtSlnID" runat="server" Visible="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        协议ID
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList ID="ddContract" runat="server" Width="236px" 
                            AutoPostBack="True" onselectedindexchanged="ddContract_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Copy至项目
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList ID="ddlSolution" runat="server" Width="236px" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Copy至项目协议ID
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtCopyID" runat="server" Width="236px"></asp:TextBox>
                    </td>
                </tr>
                <tr><td>&nbsp;</td>
                    <td style="text-align: left;padding-top: 15px;">
                        <asp:Button ID="butSubmit" runat="server" Text="Copy" OnClick="butSubmit_Click" CssClass="btn"/>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
