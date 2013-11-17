<%@ Page Language="C#" MasterPageFile="WebSite.Master" AutoEventWireup="true" CodeBehind="ProjectItem.aspx.cs" Inherits="ZyGames.OA.SvnDeployment.ProjectItem" Title="无标题页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHead" runat="server">
<title>部署子方案</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
<div class="sub-nav">&gt;&gt;项目发布&gt;&gt;<a href="Default.aspx">部署方案</a>&gt;&gt;<asp:Label ID="lblSubTitle" runat="server"></asp:Label></div>
    <div class="container">
        <div class="search-box"></div>
        <div class="grid-box">
            <table id="gridTable"></table>
        </div>
    </div>
    <div id="winAdd" class="easyui-window" iconCls="icon-save" title="增加/编辑" collapsible="false" closed="true" modal="true" minimizable="false" maximizable="false" style="width:560px;height:280px;padding:1px;">
		<div class="easyui-layout" fit="true">
		    <div region="center" border="false" style="padding:10px;background:#fafafa;border:1px solid #ccc;">
			<form id="formAdd" method="post">
			    <table width="100%">
			        <tr>
			            <td style="width:8%"><label for="GameID">子项目:</label></td>
			            <td style="width:42%"><input class="easyui-validatebox" type="text" size="30" name="Name" required="true" missingMessage="必需填写"/></td>
			            <td style="width:50%"></td>
			        </tr>
			        <tr>
			            <td><label for="GameName">IIS站点/任务计划名:</label></td>
			            <td><input class="easyui-validatebox" type="text" size="30" name="WebSite" required="true"  missingMessage="必需填写"/></td>
			            <td>IIS站点名称，在开启和停止网站使用</td>
			        </tr>
			        <tr>
			            <td><label for="GameName">部署路径:</label></td>
			            <td><input class="easyui-validatebox" type="text" size="30" name="DeployPath" required="true"  missingMessage="必需填写" /></td>
			            <td>部署的位置必需与SVN的路径一致</td>
			        </tr>
			        <tr>
			            <td><label for="ServerId">游戏服ID:</label></td>
			            <td><input class="easyui-validatebox" type="text" size="30" name="ServerId" required="true" value="1" missingMessage="必需填写"/></td>
			            <td></td>
			        </tr>
			        <tr>
			            <td><label for="GameName">任务启动文件:</label></td>
			            <td><input class="easyui-validatebox" type="text" size="30" name="ExcludeFile" /></td>
			            <td>任务计划启动程序</td>
			        </tr>
			        <tr>
			            <td colspan="3">
			            </td>
			        </tr>
			    </table>                    
	        </form>
		    </div>
		    <div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
			    <a class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="saveData()">确认</a>
			    <span class="btn-padding"></span>
			    <a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="closeWin('winAdd')">取消</a>
		    </div>
		</div>
	</div>
    <script language="javascript" type="text/javascript">
        var depId = '<%=DeployId %>';
        var actionUrl = '/Share/ActionService.aspx?action=2002&deployId=' + depId;
        function resize(data){
	        if(data){
	            $('#gridTable').datagrid('loadData',data);
	        }
	    };
        function closeWin(id){
            $('#'+id).window('close')
        }
	    function queryData(gridTable, op, params, callback){
	       
            $.fn.postJsonData(actionUrl+'&op=' + op, params, function(data){
            data  = data === null || data === "" ? '{"total": 0, "rows":[]}' : data;
		        if(data){
	                $('#'+gridTable).datagrid('loadData',data);
	                if($.isFunction(callback)){
	                    callback(data, op);
	                }
	            }
		    });
	    };
	    function saveData(){
	        var op = $('#formAdd').attr('op');
	        var param = $.fn.getFormParam('#winAdd :input');
	        var result = $('#formAdd').form('validate');
	        if(!result) return false;
	        param.Id = getID();
	        
	        queryData('gridTable', op, param, function(data, op){
//	            if(op==='add'){
//	                alert('增加成功');
//	            } else if(op==='save'){
//	                alert('修改成功');
//	            }
	            closeWin('winAdd');
	        });
	    }
	    function getID(){
	        var record = $('#gridTable').datagrid('getSelected');
	        if(record){
	            return record.Id || 0;
	        }
	        return 0;
	    }
	    
        $(function(){
			$('#gridTable').datagrid({
				//url: actionUrl,
				loadMsg: '正在加载数据...',
				width: $.fn.getAutoWidth(0.98),
				height: $.fn.getAutoHeight(),
				nowrap: false,
				striped: true,
		        singleSelect: true,
		        rownumbers: true,
				fitColumns: true,
				onLoadError: function(){
				    alert('加载出错！');
				},
				columns:[[
					{field:'Name',title:'子项目名称',width:200},
					{field:'WebSite',title:'IIS站点/任务计划名',width:120},
					{field:'DeployPath',title:'部署路径',width:180},
					{field:'ServerId',title:'游戏服ID',width:80},
					{field:'ExcludeFile',title:'任务启动文件',width:180},
					{field:'CreateDate',title:'创建日期',width:120}
				]],
				toolbar:[{
					id:'btnadd',
					text:'增加',
					iconCls:'icon-add',
					handler:function(){
					    $('#formAdd').form('clear');
					    $('#formAdd').attr('op', 'add');
						$('#winAdd').window('open');
					}
				},{
					id:'btnedit',
					text:'修改',
					disabled:false,
					iconCls:'icon-edit',
					handler:function(){
					    $('#formAdd').form('clear');
					    $('#formAdd').attr('op', 'save');
					    var record = $('#gridTable').datagrid('getSelected');
	                    if(record){
            	            $.fn.bindFormData('#winAdd :input', record);
	                    }else{
	                        alert('没有选择数据行！');
	                        return false;
	                    }
						$('#winAdd').window('open');
					}
				},{
					id:'btndelete',
					text:'删除',
					disabled:false,
					iconCls:'icon-cancel',
					handler:function(){
					    var record = $('#gridTable').datagrid('getSelected');
	                    if(record){
	                        var param = {Id: record.Id}
	                        queryData('gridTable', 'delete', param, function(){
	                             alert('删除成功');
	                        });
	                    }else{
	                        alert('没有选择数据行！');
	                    }
					}
				},{
					id:'btnadd',
					text:'增加游服任务',
					iconCls:'icon-add',
					handler:function(){
					    var record = $('#gridTable').datagrid('getSelected');
	                    if(record){
	                        var param = {Id: record.Id}
	                        queryData('gridTable', 'createSchtask', param, function(){
	                             alert('增加游服任务成功');
	                        });
	                    }else{
	                        alert('没有选择数据行！');
	                    }
					}
				},{
					id:'btnadd',
					text:'删除游服任务',
					iconCls:'icon-cancel',
					handler:function(){
					    var record = $('#gridTable').datagrid('getSelected');
	                    if(record){
	                        var param = {Id: record.Id}
	                        queryData('gridTable', 'deleteSchtask', param, function(){
	                             alert('删除游服任务成功');
	                        });
	                    }else{
	                        alert('没有选择数据行！');
	                    }
					}
				}]
			});
			var params = {};
			$.fn.postJsonData(actionUrl, params, resize);
		});
    </script>
</asp:Content>
