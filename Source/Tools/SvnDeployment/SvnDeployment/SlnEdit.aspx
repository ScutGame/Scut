<%@ Page Title="" Language="C#" MasterPageFile="WebSite.Master" AutoEventWireup="true"
    CodeBehind="SlnEdit.aspx.cs" Inherits="WebSite.SvnDeployment.SlnEdit" %>

<asp:Content ID="contentHead" ContentPlaceHolderID="contentPlaceHolderHead" runat="server">
    <title>编辑部署方案</title>
</asp:Content>
<asp:Content ID="contentBody" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
    <div class="sub-nav">
        &gt;&gt;项目发布&gt;&gt;部署方案</div>
    <div style="text-align: center">
        <p id="pTitle" class="title" runat="server">
        </p>
        <div style="margin: auto; width: 1000px;">
            <form runat="server">
            <table class="e">
                <tr>
                    <td class="e">
                        方案名称
                    </td>
                    <td class="e">
                        <asp:TextBox ID="tbSlnName" runat="server" Width="200px" Wrap="False" MaxLength="30"></asp:TextBox>
                    </td>
                    <td class="Red">
                        *
                    </td>
                    <td class="e">
                        <span class="Small">必填。部署方案的名称。</span>
                    </td>
                </tr>
                <tr>
                    <td class="e">
                        机器IP
                    </td>
                    <td class="e">
                        <asp:TextBox ID="tbSlnIP" runat="server" Width="200px" Wrap="False" MaxLength="100"></asp:TextBox>
                    </td>
                    <td class="Red">
                        *
                    </td>
                    <td class="e">
                        <span class="Small">必填。项目所在机器IP。</span>
                    </td>
                </tr>
                <tr>
                    <td class="e">
                        SVN路径
                    </td>
                    <td class="e">
                        <asp:TextBox ID="tbSvnPath" runat="server" Width="200px" Wrap="False" MaxLength="1500"></asp:TextBox>
                    </td>
                    <td class="Red">
                        *
                    </td>
                    <td class="e">
                        <span class="Small">必填。SVN相对路径（注：相对/svn/36you/目录下）。</span>
                    </td>
                </tr>
                <tr>
                    <td class="e">
                        共享目录
                    </td>
                    <td class="e">
                        <asp:TextBox ID="txtSharePath" runat="server" Width="200px" Wrap="False" MaxLength="100"></asp:TextBox>
                    </td>
                    <td class="Red">
                    </td>
                    <td class="e">
                        <span class="Small">多个子项目共享该目录。</span>
                    </td>
                </tr>
                <tr>
                    <td class="e">
                        游戏ID
                    </td>
                    <td class="e">
                        <asp:TextBox ID="txtGameId" runat="server" Width="200px" Wrap="False" MaxLength="100">0</asp:TextBox>
                    </td>
                    <td class="Red">
                    </td>
                    <td class="e">
                        <span class="Small"></span>
                    </td>
                </tr>
                <tr>
                    <td class="e">游戏服Host</td>
                    <td class="e">
                        <asp:TextBox ID="txtExcludeFile" runat="server" Width="200px" Wrap="False" MaxLength="100"></asp:TextBox>
                    </td>
                    <td class="Red">
                    </td>
                    <td class="e">
                        <span class="Small">Socket分发器地址(例：192.168.1.101:9257)。</span>
                    </td>
                </tr>
                <tr>
                    <td class="e" colspan="4">
                        <p id="pResult" runat="server" style="color: red;">
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="e" colspan="4" align="center">
                        <asp:Button ID="btnSubmit" runat="server" Text="提交" OnClick="btnSubmit_Click" />
                        &#160;&#160;
                        <input type="button" value="取消" onclick="location.href='Default.aspx'" />
                    </td>
                </tr>
            </table>
            </form>
        </div>
        <div id="divMsgBox" runat="server">
        </div>
    </div>
    
</asp:Content>
