<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ContractEdit.aspx.cs" Inherits="ContractTools.WebApp.ContractEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main">
        <div class="content">
            <table style="width: 100%; text-align: center; background: #f0f0f0;padding: 15px 5px;" >
                 <tr>
                    <td style="width: 45%; text-align: right">
                        类别
                    </td>
                    <td style="text-align: left">
                        <asp:DropDownList ID="ddlAgreement" runat="server" Width="160px" ></asp:DropDownList>
                    </td>
                </tr>
                 <tr>
                    <td style="text-align: right">
                        版本
                    </td>
                    <td style="text-align: left">
                        <asp:DropDownList ID="ddVersion" runat="server" Width="160px" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        协议ID
                    </td>
                    <td  style="text-align: left">
                        <asp:TextBox ID="txtID" runat="server" Width="160px"></asp:TextBox>
                        <strong style="color: Red">*</strong>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        协议说明
                    </td>
                    <td  style="text-align: left">
                        <asp:TextBox ID="txtDescption" runat="server" Width="164px"></asp:TextBox>
                        <strong style="color: Red">*</strong>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding-top: 15px;">
                        <asp:Button ID="butSubmit" runat="server" Text="提交" OnClick="butSubmit_Click" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btnDelete" runat="server" Text="删除" OnClick="btnDelete_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
