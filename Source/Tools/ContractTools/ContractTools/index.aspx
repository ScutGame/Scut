<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="ZyGames.ContractTools.index"
    ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>协议生成器</title>
    <link rel="stylesheet" href="flashpeak.css"/>
    <style type="text/css">
        .promptStyle
        {
            border: solid 1px #eee;
            padding:3px;
            width: 200px;
            height: 200px;
            visibility: hidden;
            background-color: rgb(253,253,203);
            position: absolute;
            overflow:auto;
        }
        .style1
        {
            width: 515px;
        }
    </style>

    <script type="text/javascript">

　　        //传入 event 对象
        function ShowPrompt(objEvent, text) {
            var divObj = document.getElementById("promptDiv");
            divObj.style.visibility = "visible";
            //使用这一行代码，提示层将出现在鼠标附近

            divObj.style.left = objEvent.clientX + 5+"px";   //clientX 为鼠标在窗体中的 x 坐标值
            divObj.innerHTML = text;
            divObj.style.top = objEvent.clientY + 5 + "px";     //clientY 为鼠标在窗体中的 y 坐标值
        }
        //隐藏提示框

        function HiddenPrompt() {
            divObj = document.getElementById("promptDiv");
            divObj.style.visibility = "hidden";
        }


        function readTxt() {
            alert(window.clipboardData.getData("txtContent"));
        }
        function setTxt() {
            var t = document.getElementById("<% =txtContent.ClientID %>");
            if (t == null) {
                return false;
            }
            t.select();
            window.clipboardData.setData('Text', t.createTextRange().text);
        }
        function setTxtto() {
            var t = document.getElementById("<% =txtContentto.ClientID %>");
            if (t == null) {
                return false;
            }
            t.select();
            window.clipboardData.setData('Text', t.createTextRange().text);
        } 
    </script>

</head>
<body>
    <%--提示框--%>
    <div id="promptDiv" class="promptStyle" onmouseout="HiddenPrompt()">
        
    </div>

    <form id="form1" runat="server">
    <center>
        <table width="90%">
            <tr>
                <td align="left" colspan="2">
                    <asp:DropDownList ID="ddlSolution" runat="server" Width="236px" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlSolution_SelectedIndexChanged">
                    </asp:DropDownList>      
                    <span style="padding-left: 40px;">&nbsp;<a href="SolutionsList.aspx" target="_blank">项目方案</a></span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:HyperLink ID="AddEnumLink" runat="server">增加枚举</asp:HyperLink>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:HyperLink ID="SearchLink" runat="server">查找协议</asp:HyperLink>
                &nbsp;&nbsp;
                    </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:DropDownList ID="DropGetList" runat="server" Width="236px" AutoPostBack="True"
                        OnSelectedIndexChanged="DropGetList_SelectedIndexChanged">
                    </asp:DropDownList>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;<asp:HyperLink
                        ID="AddRecordLink" runat="server">增加字段</asp:HyperLink>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:HyperLink ID="AddProtocolLink" runat="server">增加协议</asp:HyperLink>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:HyperLink ID="UPRecordLink" runat="server">修改协议</asp:HyperLink>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:HyperLink ID="btnCopyContract" runat="server">复制协议</asp:HyperLink>
                    &nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="btnDeldte" runat="server" OnClick="btnDeldte_Click" OnClientClick="return confirm('是否删除')">删除</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="gvGetlist" runat="server" Width="100%" AutoGenerateColumns="False"
                        CellPadding="4" ForeColor="#333333" GridLines="Both" OnRowCancelingEdit="gvGetlist_RowCancelingEdit"
                        OnRowEditing="gvGetlist_RowEditing" OnRowUpdating="gvGetlist_RowUpdating" OnRowDataBound="gvGetlist_RowDataBound"
                        Style="margin-bottom: 0px">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="序号" ItemStyle-Width="4%">
                                <ItemTemplate>
                                    <asp:Label ID="lblRowID" runat="server" Text=''></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="4%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <EditItemTemplate>
                                    <asp:Label ID="IDLabel" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="IDLabel" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="协议ID" ItemStyle-Width="5%">
                                <EditItemTemplate>
                                    <asp:Label ID="LabContractID" runat="server" Text='<%# Bind("ContractID") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LabContractID" runat="server" Text='<%# Bind("ContractID") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="5%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="参数类型" ItemStyle-Width="5%">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="droParamType" runat="server" Width="100%" SelectedValue='<%# Bind("ParamType") %>'>
                                        <asp:ListItem Value="1">请求</asp:ListItem>
                                        <asp:ListItem Value="2">返回</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblParamType" runat="server" Text='<%# Bind("ParamType") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="5%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="字段" ItemStyle-Width="12%" ItemStyle-HorizontalAlign="Left">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtField" runat="server" Text='<%# Bind("Field") %>' Width="100%"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LabField" runat="server" Text='<%# Bind("Field") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="10%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="字段类型" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Left">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="droFieldType" runat="server" SelectedValue='<%# Bind("FieldType") %>'
                                        Width="100%">
                                        <asp:ListItem Value="1">Int</asp:ListItem>
                                        <asp:ListItem Value="2">String</asp:ListItem>
                                        <asp:ListItem Value="3">Word(Short,Smallint)</asp:ListItem>
                                        <asp:ListItem Value="4">Byte</asp:ListItem>
                                        <asp:ListItem Value="5">Record(循环开始)</asp:ListItem>
                                        <asp:ListItem Value="6">End(循环开始)</asp:ListItem>
                                        <asp:ListItem Value="7">Head</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LabFieldType" runat="server" Text='<%# Bind("FieldType") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="5%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="协议描述" ItemStyle-Width="16%" ItemStyle-HorizontalAlign="Left">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDescption" runat="server" Text='<%# Bind("Descption") %>' Width="100%"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LabDescption" runat="server" Text='<%# Bind("Descption") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="16%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="默认值" ItemStyle-Width="5%">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtFieldValue" runat="server" Text='<%# Bind("FieldValue") %>' Width="100%"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LabFieldValue" runat="server" Text='<%# Bind("FieldValue") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="5%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="最小值范围" ItemStyle-Width="5%">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtMinValue" runat="server" Text='<%# Bind("MinValue") %>' Width="100%"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LabMinValue" runat="server" Text='<%# Bind("MinValue") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="5%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="最大值范围" ItemStyle-Width="5%">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtMaxValue" runat="server" Text='<%# Bind("MaxValue") %>' Width="100%"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LabMaxValue" runat="server" Text='<%# Bind("MaxValue") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="5%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="是否必填" ItemStyle-Width="5%">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="droRequired" runat="server" SelectedValue='<%# Bind("Required") %>'
                                        Width="100%">
                                        <asp:ListItem Value="False">False</asp:ListItem>
                                        <asp:ListItem Value="True">True</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LabRrequired" runat="server" Text='<%# Bind("Required") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="5%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="说明" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtRemark" runat="server" Text='<%# Bind("Remark") %>' Width="100%"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LabRemark" runat="server" Text='<%# Bind("Remark") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="15%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="排序ID" ItemStyle-Width="8%">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtSortID" runat="server" Text='<%# Bind("SortID") %>' Width="100%"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LabSortID" runat="server" Text='<%# Bind("SortID") %>'></asp:Label>
                                    <asp:LinkButton ID="btnSortAsc" runat="server" CausesValidation="False" CommandArgument='<%#Eval("id")+","+Eval("ParamType")+","+Eval("SortID")%>'
                                        Text="上移" OnCommand="btnSortAsc_Command"></asp:LinkButton>
                                    <asp:LinkButton ID="btnSortDes" runat="server" CausesValidation="False" CommandArgument='<%#Eval("id")+","+Eval("ParamType")+","+Eval("SortID")%>'
                                        Text="下移" OnCommand="btnSortDes_Command"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle Width="8%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:CommandField ShowEditButton="True" ItemStyle-Width="6%" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField ShowHeader="False" HeaderText="操作" ItemStyle-Width="4%">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" OnClientClick="return confirm('是否删除')"
                                        CommandArgument='<%#Eval("id")%>' Text="删除" OnCommand="LinkButton1_Command"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle Width="4%"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="left" />
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#7C6F57" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left">
                    <hr />
                    <strong>请求示例:</strong>
                    <asp:Label ID="lblExample" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnNoCompalte" runat="server" Text="协议未完成" 
                        onclick="btnNoCompalte_Click" />
                    <span style="padding:0 20px"></span>
                    <asp:Button ID="btnCompalte" runat="server" Text="协议完成" 
                        onclick="btnCompalte_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr>
                <td style="width: 49%">
                    <asp:DropDownList ID="LangDropDownList" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="LangDropDownList_SelectedIndexChanged">
                        <asp:ListItem Value="C#">C#</asp:ListItem>
                        <asp:ListItem Value="Python" Selected="True">Python</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="btnCopy" OnClientClick="setTxt()" runat="server" Text="复制" Width="91px" />&nbsp;<asp:Button
                        ID="btnCopy0" runat="server" Text="生成文件" OnClick="btnCopy0_Click" 
                        Width="92px" />
                    &nbsp;
                    <asp:HyperLink
                        ID="UnitTestLink" runat="server" Target="_blank">单元测试</asp:HyperLink>
                </td>
                <td style="width: 49%">
                    <asp:Button ID="btnCopyto" OnClientClick="setTxtto()" runat="server" Text="复制" />&nbsp;<asp:Button
                        ID="btnCopy1" runat="server" Text="生成.lua文件" OnClick="btnCopy1_Click" Width="92px" />&nbsp;<asp:Button
                            ID="btnConfig" runat="server" Text="生成配置文件" Width="92px" OnClick="btnConfig_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtContent" runat="server" Height="577px" TextMode="MultiLine" Width="100%"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtContentto" runat="server" Height="577px" TextMode="MultiLine"
                        Width="100%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left">
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="txtActionDefine" runat="server" Height="250px" TextMode="MultiLine"
                        Width="100%"></asp:TextBox>
                </td>
            </tr>
        </table>
    </center>
    </form>
</body>
</html>
