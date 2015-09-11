<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ContractCopy.aspx.cs" Inherits="ContractTools.WebApp.ContractCopy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main">
        <div class="content">
             <table class="table" style="width: 100%; text-align: center; background: #f0f0f0;padding: 15px 5px;">
                <tr>
                    <td style="text-align: right; width: 40%">
                        当前项目
                    </td>
                    <td style="text-align: left;">
                        <asp:Label ID="lblSlnName" runat="server" Style="text-decoration: underline;font-weight: bold;" />
                        <asp:TextBox ID="txtSlnID" runat="server" Visible="false"></asp:TextBox>
                        <asp:TextBox ID="txtVerID" runat="server" Visible="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        协议编号
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList ID="ddContract" runat="server" Width="260px" 
                            AutoPostBack="True" onselectedindexchanged="ddContract_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        复制到项目
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList ID="ddlSolution" runat="server" Width="260px" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        新协议编号
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtCopyID" runat="server" Width="260px"></asp:TextBox>
                        <span style="padding: 0 10px"></span>
                        <asp:Button ID="btnRefesh" runat="server" Text="刷新" OnClick="btnRefesh_Click" CssClass="btn"/>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        新增到位置
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList ID="ddResponseParams" runat="server" Width="260px"></asp:DropDownList>
                    </td>
                </tr>
                <tr><td>&nbsp;</td>
                    <td style="text-align: left;padding-top: 15px;">
                        <asp:Button ID="butSubmit" runat="server" Text="复制协议" OnClick="butSubmit_Click" CssClass="btn"/>
                        <span style="padding: 0 10px"></span>
                        <asp:Button ID="btnCopyParam" runat="server" Text="复制参数" OnClick="btnCopyParam_Click" CssClass="btn"/>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <span>复制参数从位置</span>
                    </td>
                    <td  style="text-align: left;">
                        <asp:DropDownList ID="ddParamCopyFrom" runat="server" Width="120px"></asp:DropDownList>
                        <span>到</span>
                        <asp:DropDownList ID="ddParamCopyTo" runat="server" Width="120px"></asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
