<%@ Page Title="" Language="C#" MasterPageFile="WebSite.Master" AutoEventWireup="true"
	CodeBehind="Default.aspx.cs" Inherits="WebSite.SvnDeployment.Default" %>

<asp:Content ID="contentHead" ContentPlaceHolderID="contentPlaceHolderHead" runat="server">
	<title>首页</title>
</asp:Content>
<asp:Content ID="contentBody" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
	<div class="sub-nav">&gt;&gt;项目发布&gt;&gt;部署方案</div>
    <div class="container">
        <div class="search-box"></div>
        <div class="grid-box">
            <table id="gridTable"></table>
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        var actionUrl = '/Share/ActionService.aspx?action=2001';
        function resize(data){
	        if(data){
	            $('#gridTable').datagrid('loadData',data);
	        }
	    };
	    function getID(){
	        var record = $('#gridTable').datagrid('getSelected');
	        if(record){
	            return record.SlnId || 0;
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
					{field:'opt',title:'操作',width:200,align:'center',  
                        formatter:function(value,rec,index){    
                             var str = '<a href="javascript:void(0)" onclick="deploy('+ rec.SlnId +','+ rec.GameId +')">部署</a>&nbsp;&nbsp; ';
                             str += '<a href="javascript:void(0)" onclick="modify(\''+ rec.SlnId +'\')">修改</a>&nbsp;&nbsp;';
                            str += '<a href="javascript:void(0)" onclick="doDelete(\''+ rec.SlnId +'\')">删除</a>';
                            return str;  
                        }  
                    },
					{field:'SlnName',title:'项目名称',width:200,formatter:function(value, rowData, rowIndex){
					    return '<a href="ProjectItem.aspx?Id='+rowData.SlnId+'">'+$.fn.formatterHtml(value) +'</a>';
					}},
					{field:'Ip',title:'机器IP',width:120},
					{field:'SvnPath',title:'SVN路径',width:180},
					{field:'GameId',title:'游戏ID',width:80},
					{field:'SharePath',title:'共享目录',width:180},
					{field:'ExcludeFile',title:'游戏服Host',width:180},
					{field:'CreateDate',title:'创建日期',width:120}
				]],
				toolbar:[{
					id:'btnadd',
					text:'增加',
					iconCls:'icon-add',
					handler:function(){
					   location.href = 'SlnEdit.aspx';
					}
				}]
			});
			var params = {};
			$.fn.postJsonData(actionUrl, params, resize);
			
		});
		function modify(id){
		    if(id > 0){
			    location.href = 'SlnEdit.aspx?id=' + id;
			}
		}
		function deploy(id,gameId){
		    if(id > 0){
			    location.href = 'Deploy.aspx?id=' + id +'&GameId='+gameId;
			}
		}
		function doDelete(id){
		    if(confirm('确定要删除数据吗？')){
		        if(id > 0){
			        var params = {op:'delete', id:id};
			        $.fn.postJsonData(actionUrl, params, resize);
			    }
			}
		}
    </script>
</asp:Content>
