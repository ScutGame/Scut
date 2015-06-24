<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ContractTools.WebApp.Default" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>协议工具平台</title>
    <link href="skin.css" rel="stylesheet" />
    <script language="javascript" type="text/javascript">
        //传入 event 对象
        function ShowPrompt(objEvent, text) {
            var divObj = document.getElementById("promptDiv");
            divObj.style.visibility = "visible";
            //使用这一行代码，提示层将出现在鼠标附近

            divObj.style.left = objEvent.clientX + 5 + "px";   //clientX 为鼠标在窗体中的 x 坐标值
            divObj.innerHTML = text;
            divObj.style.top = objEvent.clientY + 5 + "px";     //clientY 为鼠标在窗体中的 y 坐标值
        }
        //隐藏提示框

        function HiddenPrompt() {
            divObj = document.getElementById("promptDiv");
            divObj.style.visibility = "hidden";
        }
        
        function onCopyCode(id) {
            var t = document.getElementById(id);
            if (t == null) {
                return false;
            }
            t.select();
            if (window.clipboardData) {
                window.clipboardData.clearData();
                window.clipboardData.setData('Text', t.createTextRange().text);
            } else {
                alert('你的浏览器不支持复制功能, 请手工复制');
            }
        }

        function OnSearch() {
            var txtSearch = document.getElementById("txtSearch").valueOf || '';
            __doPostBack && __doPostBack("btnSerach", txtSearch);
        }

        function EnterPress(e) {
            var e = e || window.event;
            if(e.keyCode == 13) {
                OnSearch();
            }
        }
    </script>
</head>
<body>
    <div id="promptDiv" class="promptStyle" onmouseout="HiddenPrompt()"></div>
    <form id="form1" runat="server">
        <div class="layout">
            <% if (IsEdit){ %>
            <div class="topbar">
                <ul>
                    <li>
                        <asp:HyperLink ID="hlSolution" runat="server" Target="_top">项目管理</asp:HyperLink></li>
                    <li>
                        <asp:HyperLink ID="hlVersion" runat="server" Target="_top">版本管理</asp:HyperLink></li>
                    <li>
                        <asp:HyperLink ID="hlEnum" runat="server" Target="_top">枚举管理</asp:HyperLink></li>
                    <li>
                        <asp:HyperLink ID="hlAgreement" runat="server" Target="_top">类别管理</asp:HyperLink></li>
                    <li>
                        <asp:HyperLink ID="hlContract" runat="server" Target="_top">新增协议</asp:HyperLink></li>
                    <li>
                        <asp:HyperLink ID="hlContractEdit" runat="server" Target="_top">编辑协议</asp:HyperLink></li>
                    <li>
                        <asp:HyperLink ID="hlContractCopy" runat="server" Target="_top">Copy协议</asp:HyperLink></li>
                </ul>
            </div>
            <% } %>
            <div class="left">
                <div class="toolbar">
                    <span>
                        <asp:HyperLink ID="hlTopEdit" runat="server" CssClass="edit" ToolTip="开启或关闭编辑">项目</asp:HyperLink>:</span>
                    <asp:DropDownList ID="ddlSolution" runat="server" Width="120px" AutoPostBack="True" OnSelectedIndexChanged="OnSolutionChanged"></asp:DropDownList>
                    <span>版本:</span>
                    <asp:DropDownList ID="ddVersion" runat="server" Width="80px" AutoPostBack="True" OnSelectedIndexChanged="OnVersionChanged"></asp:DropDownList>
                    <span>协议:</span>
                    <asp:DropDownList ID="ddlAgreement" runat="server" Width="120px" AutoPostBack="True" OnSelectedIndexChanged="OnAgreementChanged"></asp:DropDownList>
                    <asp:DropDownList ID="ddContract" runat="server" Width="220px" AutoPostBack="True" OnSelectedIndexChanged="OnContractChanged"></asp:DropDownList>
                    <span>筛选:</span><input type="text" id="txtSearch" name="txtSearch" style="width: 120px" onkeypress="EnterPress(event)" onkeydown="EnterPress()"/>
                    <a class="btn-search" id="btnSerach" onclick="OnSearch()" ></a>
                </div>
                <% if (IsEdit){ %>
                <div class="content">
                    <table style="width: 100%; background: #f0f0f0; padding: 5px;" cellspacing="4">
                        <tr>
                            <td style="width: 25%; text-align: right;"><span class="space">参数类型:</span></td>
                            <td>
                                <asp:DropDownList ID="ddParamType" runat="server" AutoPostBack="True" Width="100px" OnSelectedIndexChanged="OnParamTypeChanged">
                                    <asp:ListItem Value="1">请求</asp:ListItem>
                                    <asp:ListItem Value="2" Selected="True">返回</asp:ListItem>
                                </asp:DropDownList><span>（Record：循环体开始标记，End：结束标记，Void：作废，Password：标记加密）</span>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;vertical-align: top;">
                                <span class="space">字段类型:</span></td>
                            <td>
                                <asp:DropDownList ID="ddFieldType" runat="server" Width="100px">
                                </asp:DropDownList>
                                <span>新增到位置:</span>
                                <asp:DropDownList ID="ddResponseParams" runat="server" Width="150px">
                                 </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <span class="space">字段名:</span></td>
                            <td>
                                <asp:TextBox ID="txtField" runat="server" Width="300px"></asp:TextBox>
                                <asp:CheckBox runat="server" ID="ckRequired" Text="必传参数" Checked="True" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <span class="space">字段说明:</span>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Height="30px" Width="300px"></asp:TextBox></td>
                        </tr>
                        <tr><td style="text-align: right">
                                <span class="space"></span>
                            </td>
                            <td style="padding-top: 5px;">
                                <asp:Button ID="btnParamAdd" runat="server" Text="新增参数" OnClick="btnParamAdd_Click" CssClass="btn" />
                                <span style="padding:0 5px"></span>
                                <asp:Button ID="btnParamCopy" runat="server" Text="复制参数" OnClick="btnParamCopy_Click" CssClass="btn" AutoPostBack="True" />
                                <span style="padding:0 5px"></span>
                                <asp:Button ID="btnParamRemove" runat="server" Text="删除参数" OnClick="btnParamRemove_Click" CssClass="btn"/>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <span>复制参数从位置:</span>
                            </td>
                            <td style="padding-top: 5px;">
                                <asp:DropDownList ID="ddParamCopyFrom" runat="server" Width="150px">
                                 </asp:DropDownList>
                                <span>到</span>
                                <asp:DropDownList ID="ddParamCopyTo" runat="server" Width="150px">
                                 </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
                <% } %>
                <div class="subnav">
                    <span class="title">请求参数</span>
                </div>
                <div class="content-grid">
                    <asp:GridView ID="gvReqParams" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="grid"
                        OnRowDataBound="OnGridRowDataBound" OnRowCancelingEdit="OnGridRowCancelingEdit"
                        OnRowEditing="OnGridRowEditing" OnRowUpdating="OnGridRowUpdating">
                        <Columns>
                            <asp:TemplateField Visible="False">
                                <EditItemTemplate>
                                    <asp:Label ID="IDLabel" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="IDLabel" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="参数名" ItemStyle-Width="12%" ItemStyle-HorizontalAlign="Left">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtField" runat="server" Text='<%# Bind("Field") %>' Width="90%"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LabField" runat="server" Text='<%# Bind("Field") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="10%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="参数类型" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Left">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="droFieldType" runat="server" SelectedValue='<%# Bind("FieldTypeId") %>'
                                        Width="90%"><asp:ListItem Value="-1">Password</asp:ListItem>
                                        <asp:ListItem Value="1">Int</asp:ListItem>
                                        <asp:ListItem Value="2">String</asp:ListItem>
                                        <asp:ListItem Value="3">Short</asp:ListItem>
                                        <asp:ListItem Value="4">Byte</asp:ListItem>
                                        <asp:ListItem Value="7">Void</asp:ListItem>
                                        <asp:ListItem Value="8">Long</asp:ListItem>
                                        <asp:ListItem Value="9">Bool</asp:ListItem>
                                        <asp:ListItem Value="10">Float</asp:ListItem>
                                        <asp:ListItem Value="11">Double</asp:ListItem>
                                        <asp:ListItem Value="12">Date</asp:ListItem>
                                        <asp:ListItem Value="13">UInt</asp:ListItem>
                                        <asp:ListItem Value="14">UShort</asp:ListItem>
                                        <asp:ListItem Value="15">ULong</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LabFieldType" runat="server" Text='<%# Bind("FieldTypeId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="5%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="必传参数" ItemStyle-Width="5%">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="droRequired" runat="server" SelectedValue='<%# Bind("Required") %>'
                                        Width="90%">
                                        <asp:ListItem Value="False">False</asp:ListItem>
                                        <asp:ListItem Value="True">True</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LabRrequired" runat="server" Text='<%# Bind("Required") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="5%"></ItemStyle>
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
                            <asp:TemplateField HeaderText="参数描述" ItemStyle-Width="16%" ItemStyle-HorizontalAlign="Left">
                                <EditItemTemplate>
                                    <asp:TextBox ID="hiDescption" runat="server" Text='<%# Bind("Descption") %>' Width="90%" Visible="False"></asp:TextBox>
                                    <asp:TextBox ID="txtDescption" runat="server" Text='<%# Bind("Remark") %>' Width="90%" TextMode="MultiLine"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LabDescption" runat="server" Text='<%# Bind("Descption") %>'></asp:Label>
                                    <asp:Label ID="LabRemark" runat="server" Text='<%# Bind("Remark") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="16%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:CommandField ShowEditButton="True" ItemStyle-Width="6%" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" Width="6%"></ItemStyle>
                            </asp:CommandField>
                            <asp:TemplateField ShowHeader="False" HeaderText="操作" ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" OnClientClick="return confirm('是否删除')"
                                        CommandArgument='<%#Eval("id")%>' Text="删除" OnCommand="OnRespGridDelete"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle Width="4%"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="grid-row-alt" />
                        <FooterStyle CssClass="grid-row" />
                        <HeaderStyle CssClass="grid-head" />
                        <RowStyle CssClass="grid-row" />
                        <SelectedRowStyle CssClass="grid-row-select" />
                        <EditRowStyle CssClass="grid-row-select" />
                    </asp:GridView>
                </div>
                <div class="subnav">
                    <span class="title">返回参数</span>
                </div>
                <div class="content-grid">
                    <asp:GridView ID="gvRespParams" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="grid"
                        OnRowDataBound="OnGridRowDataBound" OnRowCancelingEdit="OnGridRowCancelingEdit"
                        OnRowEditing="OnGridRowEditing" OnRowUpdating="OnGridRowUpdating">
                        <Columns>
                            <asp:TemplateField Visible="False">
                                <EditItemTemplate>
                                    <asp:Label ID="IDLabel" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="IDLabel" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="参数类型" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="droFieldType" runat="server" SelectedValue='<%# Bind("FieldTypeId") %>'
                                        Width="90%">
                                        <asp:ListItem Value="1">Int</asp:ListItem>
                                        <asp:ListItem Value="2">String</asp:ListItem>
                                        <asp:ListItem Value="3">Short</asp:ListItem>
                                        <asp:ListItem Value="4">Byte</asp:ListItem>
                                        <asp:ListItem Value="5">Record</asp:ListItem>
                                        <asp:ListItem Value="6">End</asp:ListItem>
                                        <asp:ListItem Value="7">Void</asp:ListItem>
                                        <asp:ListItem Value="8">Long</asp:ListItem>
                                        <asp:ListItem Value="9">Bool</asp:ListItem>
                                        <asp:ListItem Value="10">Float</asp:ListItem>
                                        <asp:ListItem Value="11">Double</asp:ListItem>
                                        <asp:ListItem Value="12">Date</asp:ListItem>
                                        <asp:ListItem Value="13">UInt</asp:ListItem>
                                        <asp:ListItem Value="14">UShort</asp:ListItem>
                                        <asp:ListItem Value="15">ULong</asp:ListItem>
                                        <asp:ListItem Value="16">SigleRecord</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LabFieldType" runat="server" Text='<%# Bind("FieldTypeId") %>' Visible="False"></asp:Label>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("DepthTypeDescp") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="5%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="参数名" ItemStyle-Width="12%" ItemStyle-HorizontalAlign="Left">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtField" runat="server" Text='<%# Bind("Field") %>' Width="90%"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LabField" runat="server" Text='<%# Bind("Field") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="10%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="参数描述" ItemStyle-Width="11%" ItemStyle-HorizontalAlign="Left">
                                <EditItemTemplate>
                                    <asp:TextBox ID="hiDescption" runat="server" Text='<%# Bind("Descption") %>' Width="90%" Visible="False"></asp:TextBox>
                                    <asp:TextBox ID="txtDescption" runat="server" Text='<%# Bind("Remark") %>' Width="90%" TextMode="MultiLine"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LabDescption" runat="server" Text='<%# Bind("Descption") %>'></asp:Label>
                                    <asp:Label ID="labRemark" runat="server" Text='<%# Bind("Remark") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="16%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="排序" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                <EditItemTemplate>
                                    <asp:Label ID="txtSortID" runat="server" Text='<%# Bind("SortID") %>' Width="90%"></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnSortAsc" runat="server" CausesValidation="False" CommandArgument='<%#Eval("id")+","+Eval("ParamType")+","+Eval("SortID")%>'
                                        Text="↑上移" OnCommand="btnSortAsc_Command"></asp:LinkButton>&nbsp;
                                    <asp:LinkButton ID="btnSortDes" runat="server" CausesValidation="False" CommandArgument='<%#Eval("id")+","+Eval("ParamType")+","+Eval("SortID")%>'
                                        Text="↓下移" OnCommand="btnSortDes_Command"></asp:LinkButton>&nbsp;
                                    <asp:LinkButton ID="btnRecordSortAsc" runat="server" CausesValidation="False" CommandArgument='<%#Eval("id")+","+Eval("FieldTypeId")+","+Eval("SortID")%>'
                                        Text="↑循环" OnCommand="btnRecordSortAsc_Command"></asp:LinkButton>&nbsp;
                                    <asp:LinkButton ID="btnRecordSortDes" runat="server" CausesValidation="False" CommandArgument='<%#Eval("id")+","+Eval("FieldTypeId")+","+Eval("SortID")%>'
                                        Text="↓循环" OnCommand="btnRecordSortDes_Command"></asp:LinkButton>&nbsp;
                                </ItemTemplate>
                                <ItemStyle Width="10%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:CommandField ShowEditButton="True" ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" Width="4%"></ItemStyle>
                            </asp:CommandField>
                            <asp:TemplateField ShowHeader="False" HeaderText="操作" ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" OnClientClick="return confirm('是否删除')"
                                        CommandArgument='<%#Eval("id")%>' Text="删除" OnCommand="OnRespGridDelete"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle Width="4%"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="grid-row-alt" />
                        <FooterStyle CssClass="grid-row" />
                        <HeaderStyle CssClass="grid-head" />
                        <RowStyle CssClass="grid-row" />
                        <SelectedRowStyle CssClass="grid-row-select" />
                        <EditRowStyle CssClass="grid-row-select" />
                    </asp:GridView>
                </div>
                <div class="bottombar">
                    <% if (IsEdit){ %>
                    <span>
                        <asp:Button runat="server" ID="btnNoCompalte" OnClick="OnNoCompaltedClick" Text="未完成" /></span>
                    <span>
                        <asp:Button runat="server" ID="btnCompalte" OnClick="OnCompaltedClick" Text="已完成" /></span>
                    <% } %>
                    <span>
                        <asp:CheckBox runat="server" ID="ckSelfAction"  Text="自定义的协议" AutoPostBack="True" OnCheckedChanged="OnSelfActionChanged"/></span>
                </div>
            </div>
            <div class="fx-space"></div>
            <div class="right">
                <div id="navbox">
                    <ul id="tab_nav">
                        <li><a href="#tab_1">服务端代码</a></li>
                        <li><a href="#tab_2">客户端代码</a></li>
                        <li><a href="#tab_3">枚举参数</a></li>
                        <li><a href="#tab_4">协议调试</a></li>
                        <%--<li><a href="#tab_5">协议配置</a></li>--%>
                    </ul>
                    <div id="tab_content">
                        <div id="tab_1" class="tab_item">
                            <div class="toolbar">
                                <span style="padding-left: 5px;">脚本类型:</span>
                                <asp:DropDownList ID="ddServerCodeType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="OnServerCodeTypeChanged">
                                    <asp:ListItem Value="C#"></asp:ListItem>
                                    <asp:ListItem Value="Python"></asp:ListItem>
                                    <asp:ListItem Value="Lua"></asp:ListItem>
                                </asp:DropDownList>
                                <span>&nbsp;</span>
                                <asp:Button id="btnExportServer" runat="server" Text="导出文件" OnClick="OnExportSererCode"/>
                                <span>&nbsp;</span>
                                <asp:Button id="btnExportAllServer" runat="server" Text="批量导出" OnClick="OnExportAllSererCode"/>
                                <span>&nbsp;</span>
                                <input type="button" id="btCopyServer" value="复制代码" onclick="onCopyCode('txtServerCode')"/>
                            </div>
                            <div style="width: 100%; height: 542px;">
                                <asp:TextBox ID="txtServerCode" runat="server" TextMode="MultiLine" CssClass="codeBox"></asp:TextBox>
                            </div>
                        </div>
                        <div id="tab_2" class="tab_item">
                            <div class="toolbar">
                                <span style="padding-left: 5px;">脚本类型:</span>
                                <asp:DropDownList ID="ddClientCodeType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="OnClientCodeTypeChanged">
                                    <asp:ListItem Value="Lua">Cocos2d-x of Lua</asp:ListItem>
                                    <asp:ListItem Value="Quick">Quick</asp:ListItem>
                                    <asp:ListItem Value="C#">Unity3d of C#</asp:ListItem>
                                </asp:DropDownList>
                                <span>&nbsp;</span>
                                <asp:Button id="btnExportClient" runat="server" Text="导出文件" OnClick="OnExportClientCode"/>
                                <span>&nbsp;</span>
                                <asp:Button id="btnExportAllClient" runat="server" Text="批量导出" OnClick="OnExportAllClientCode"/>
                                <span>&nbsp;</span>
                                <input type="button" id="btCopyClient" value="复制代码" onclick="onCopyCode('txtServerCode')"/>
                            </div>
                            <div style="width: 100%; height: 542px;">
                                <asp:TextBox ID="txtClientCode" runat="server" TextMode="MultiLine" CssClass="codeBox"></asp:TextBox>
                            </div>
                        </div>
                        <div id="tab_3" class="tab_item">
                            <div style="width: 100%; height: 542px;">
                                <div id="lblEnumDescp" runat="server"></div>
                            </div>
                        </div>
                        <div id="tab_4" class="tab_item">
                            <iframe id="ifrTest" runat="server" style="width: 100%; height: 100%; border: 0;"></iframe>
                        </div>
                        <div id="tab_5" class="tab_item">
                            <iframe id="ifrClientConfig" runat="server" style="width: 100%; height: 100%; border: 0;"></iframe>
                        </div>
                    </div>
                </div>

            </div>
            <div class="botbar">
                <span>Copyright &copy; 2014 Scut, Inc.</span>
            </div>
        </div>
    </form>
</body>
</html>
