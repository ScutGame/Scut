<%@ Page Title="" Language="C#" MasterPageFile="WebSite.Master" AutoEventWireup="true"
    CodeBehind="Deploy.aspx.cs" Inherits="WebSite.SvnDeployment.Deploy" %>

<asp:Content ID="contentHead" ContentPlaceHolderID="contentPlaceHolderHead" runat="server">
    <title>版本部署</title>
    <style type="text/css">
        .deploy_op{padding: 0;}
        .deploy_op a{padding:0 3px; text-decoration:none;}
    </style>
</asp:Content>
<asp:Content ID="contentBody" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
    <div class="sub-nav">
        &gt;&gt;项目发布&gt;&gt;<a href="Default.aspx">部署方案</a>&gt;&gt;<asp:Label ID="pTitle" runat="server"></asp:Label></div>
    <div class="container">
        <div class="search-box">
        </div>
        <div class="grid-box">
            <table id="gridTable">
            </table>
        </div>
        
    </div>
    
    <div id="winAdd" class="easyui-window" iconCls="icon-save" title="操作结果" collapsible="false" closed="true" modal="true" minimizable="false" maximizable="false" style="width:860px;height:560px;padding:1px;">
		<div class="easyui-layout" fit="true">
		    <div region="center" border="false" style="padding:10px;background:#fafafa;border:1px solid #ccc;">
		        <div style="text-align:center; padding-bottom:5px;">
		        <a href="#" id="btReoad" class="easyui-linkbutton" plain="true" iconCls="icon-reload">刷新</a><span id="spReoad"></span></div>
			    <div><table id="gridResult"></table></div>
		    </div>
		</div>
	</div>
    <div id="winUpdate" class="easyui-window" iconCls="icon-save" title="Svn更新至版本" collapsible="false" closed="true" modal="true" minimizable="false" maximizable="false" style="width:260px;height:140px;padding:2px;">
        
        <div class="easyui-layout" fit="true">
			<div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
				<table border="0" width="98%">
                    <tr>
                        <td align="center" style="width:30%">版本:</td>
                        <td style="width:70%"><input id="itUpdateTo" size="20" class="easyui-numberbox"/></td>
                    </tr>
                </table>
			</div>
			<div region="south" border="false" style="text-align:center;height:30px;line-height:30px;">
				<a class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="winUpdateOk()">确认</a>
				<span style="padding-left:5px"></span>
				<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#winUpdate').window('close')">取消</a>
			</div>
		</div>
    </div>
    
	<div id="winMessage" class="easyui-window" iconCls="icon-save" title="消息窗口" collapsible="false" closed="true" modal="true" minimizable="false" maximizable="false" style="width:600px;height:400px;padding:2px;">
        <div class="easyui-layout" fit="true">
            <div class="content" region="center" border="false" style="padding:10px;background:#fafafa;border:1px solid #ccc;overflow:auto">
                <p>正在执行中...</p>
            </div>
        </div>
	</div>
    <input type="hidden" id="hiGameId" value="<%=GameId %>"/>
    <script language="javascript" type="text/javascript">
        var actionUrl = '/Share/ActionService.aspx?action=2002&deployId=<%=DeployId %>';       
	    
        $(function(){
			$('#gridTable').datagrid({
			    title: '部暑流程：执行Python / 停IIS / 部署 / 开启IIS（注:操作会延迟15秒，点结果查看操作进度）',
				url: actionUrl,
				loadMsg: '正在加载数据...',				
				width: 860,
				height: 'auto',
				nowrap: false,
				striped: true,
		        rownumbers: true,                
		        //singleSelect:true,
				//fitColumns: true,
				onLoadError: function(){
				    alert('加载出错！');
				},
				onLoadSuccess: function(){
                    function bindRowsEvent(){
                        var panel = $('#gridTable').datagrid('getPanel');
                        var rows = panel.find('tr[datagrid-row-index]');
                        rows.unbind('click').bind('click',function(e){
                            return false;
                        });
                        rows.find('div.datagrid-cell-check input[type=checkbox]').unbind().bind('click', function(e){
                            var index = $(this).parent().parent().parent().attr('datagrid-row-index');
                            if ($(this).attr('checked')){
                                $('#gridTable').datagrid('selectRow', index);
                            } else {
                                $('#gridTable').datagrid('unselectRow', index);
                            }
                            e.stopPropagation();
                        });
                    }
                    setTimeout(function(){
                        bindRowsEvent();
                    }, 10);    
                },
				columns:[[
				    {field:'ck', checkbox:true},
					{field:'Name',title:'子项目名称',width:130},
					{field:'ServerId',title:'游戏服ID',width:80},
//					{field:'ExcludeFile',title:'版本号',width:100, formatter:function(value,rec,index){
//					    var str = '<input type="text" id="itVerId'+rec.Id+'" name="verId" size="5" value="0">';
//					    return str;③④⑤⑥⑦⑧⑨
//					}},
					{field:'opt',title:'操作',width:140,align:'center',  
                        formatter:function(value,rec,index){  
                            var str = '<div class="deploy_op">';
                            str += '<a href="javascript:void(0)"onclick="updateCache(\''+ rec.Id + '\')" title="更新缓存">①更新缓存</a>';
                            str += '<a href="javascript:void(0)" onclick="deployPython(\''+ rec.Id +'\')" title="部署Python脚本">②PY部署</a>';
                            str += "</div>";
                            
                            return str;  
                        }  
                    },  
                    {field:'opt2',title:'操作结果',width:100,align:'center',  
                        formatter:function(value,rec,index){  
                            var str = '<a href="javascript:void(0)" onclick="refreshResult(\''+ rec.Id + '\')">查看</a>&nbsp;&nbsp;';                                
                            return str;  
                        }  
                    },
					{field:'opt3',title:'其它操作',width:320,align:'center',  
                        formatter:function(value,rec,index){  
                            var str = '<div class="deploy_op">';
                            str += '<a href="javascript:void(0)" onclick="runPy('+ rec.Id + ','+ rec.ServerId + ')" title="执行Python脚本">①执行PY</a>';  
                            str += '<a href="javascript:void(0)" onclick="selectLog('+ rec.Id + ','+ rec.ServerId + ')" title="消息队列数">②队列数</a>';
                            str += '<a href="javascript:void(0)" onclick="stopIIs(\''+ rec.Id + '\')" title="停IIS/任务">③停止</a>';   
                            str += '<a href="javascript:void(0)" onclick="deploy(\''+ rec.Id +'\')" title="部署全部文件">④部署</a> ';
                            str += '<a href="javascript:void(0)"onclick="startIIs(\''+ rec.Id + '\')" title="开启IIS/任务">⑥开启</a>';  
                            str += '<a href="javascript:void(0)"onclick="reStartIIs(\''+ rec.Id + '\')" title="回收程序池/任务">⑦回收/重启</a>';  
                            str += "</div>";
                            
                            return str;  
                        }  
                    }
				]],
				toolbar:[{
					id:'btnSvnUpdate',
					text:'SVN检出',
					//iconCls:'icon-edit',
					handler:function(){
					    updateSvn(true);
					}
				},{
					id:'btnSvnClearup',
					text:'SVN清理',
					//iconCls:'icon-edit',
					handler:function(){
					    clearupSvn();
					}
				}/*,{
					id:'btnSvnUpdate',
					text:'SVN更新',
					//iconCls:'icon-edit',
					handler:function(){
					    updateSvn(false);
					}
				}*/,{
					id:'btnSvnUpdate',
					text:'SVN更新至',
					//iconCls:'icon-edit',
					handler:function(){
					    updateToSvn();
					}
				},'-',{
					id:'btnStopIIs',
					text:'停止IIS',
					iconCls:'icon-edit',
					handler:function(){
					    stopIIs(getSelections());
					}
				},{
					id:'btnStartIIs',
					text:'开启IIS',
					iconCls:'icon-edit',
					handler:function(){
					    startIIs(getSelections());
					}
				},'-',{
					id:'btnDeploy',
					text:'部署',
					iconCls:'icon-edit',
					handler:function(){
					    deploy(getSelections());
					}
				},'-',{
					id:'btnDeploy',
					text:'Py部署',
					iconCls:'icon-edit',
					handler:function(){
					    deployPython(getSelections());
					}
				},'-',{
					id:'btnquickDeploy',
					text:'快速部署',
					iconCls:'icon-edit',
					handler:function(){
					    quickDeploy(getSelections());
					}
				}]

			});
			
			var intervalId;
			$('#winAdd').window({
			    onOpen: function(){
			        if(intervalId === undefined){
			            var coldTime = 10;
			            intervalId = setInterval(function() {
                            $('#spReoad').text('('+ coldTime +'s)');
                            if(coldTime <= 0){
                                coldTime = 10;
                                $('#btReoad').click();
                            }
                            coldTime--;
                        }, 1000);
			        }
			    }, 
			    onClose: function(){	        
                    clearInterval(intervalId);
                    intervalId = undefined;
			    }
			});
			$('#winUpdate').window();
            var isLoading = false;
			$('#btReoad').click(function(){
			    if(isLoading) return;
			    isLoading = true;
			    var depId = $('#winAdd').attr("depId");
			    var params = {deployId: depId};
			    $.fn.postJsonData('/Share/ActionService.aspx?action=2003&op=', params, function(data){
			        if(data){
	                    $('#gridResult').datagrid('loadData',data);
	                    isLoading = false;
	                }
			    });
			});
			$('#gridResult').datagrid({
				//url: '/Share/ActionService.aspx?action=2003',
				loadMsg: '正在加载数据...',
				nowrap: false,
				striped: true,
		        rownumbers: true,
				fitColumns: true,
		        singleSelect:true,
				onLoadError: function(){
				    alert('加载出错！');
				},
				columns:[[
					{field:'Type',title:'命令类型',width:80, formatter:function(value){
					    if(value === '1') return '开始IIS';
					    if(value === '2') return '停止IIS';
					    if(value === '3') return 'Svn更新';
					    if(value === '4') return '部署';
					    if(value === '5') return 'Svn清理';
					    if(value === '6') return 'Svn检出';
					    if(value === '7') return '快速部署';
					    if(value === '8') return '部署Python';
					    if(value === '11') return '创建任务';
					    if(value === '12') return '删除任务';
					    if(value === '13') return '开启任务';
					    if(value === '14') return '停止任务';
					    if(value === '15') return '重启任务';
					    return value;
					}},
					{field:'Revision',title:'Svn版本',width:80},
					{field:'Status',title:'状态',width:80, formatter:function(value){
					    if(value === '0') return '等待执行';
					    if(value === '1') return '执行结束';
					    if(value === '2') return '执行失败';
					    return value;
					}},
					{field:'ErrorMsg',title:'详情',width:350},
					{field:'CreateDate',title:'时间',width:120}
				]]
			});
			
		});
		function getSelections(){
		    var ids = [];
		    var rows = $('#gridTable').datagrid('getSelections');
		    for(var i=0;i<rows.length;i++){
			    ids.push(rows[i].Id);
		    }
		    return ids.join(',');
	    }
        function stopIIs(depId) 
        {
            var params = {op:'stopiis',Id: depId};
		    $.fn.postJsonData(actionUrl, params, resize);
        }
        
        function startIIs(depId)  
        {
            var params = {op:'startiis',Id: depId};
		    $.fn.postJsonData(actionUrl, params, resize);
        }
        function reStartIIs(depId){
            var params = {op:'restart',Id: depId};
		    $.fn.postJsonData(actionUrl, params, resize);
        }
        function clearupSvn(){
            var data = $('#gridTable').datagrid('getData');
			if(data && data.rows && data.rows.length>0){
			    var depId = data.rows[0].Id || 0;
                var params = {
                    op: 'clearupsvn',
                    Id: depId,
                    verId: 0
                };
		        $.fn.postJsonData(actionUrl, params);
			    refreshResult(depId);
		    }else{
		        alert('没有数据！')
		    }
        }
        function updateSvn(ischeck, verId)  
        {
			var data = $('#gridTable').datagrid('getData');
			if(data && data.rows && data.rows.length>0){
			    var depId = data.rows[0].Id || 0;
                var params = {
                    op: 'updatesvn',
                    Id: depId,
                    Ischeck: ischeck,
                    verId: verId||0
                };
		        $.fn.postJsonData(actionUrl, params);
			    refreshResult(depId);
		    }else{
		        alert('没有数据！')
		    }
        }
        
        function updateToSvn(){
            $('#winUpdate').window("open");
        }
        
        function winUpdateOk(){
            var verId = $('#itUpdateTo').val();
            if(verId < 0){
                alert('版本超出范围，不能为负数！');
                $('#itUpdateTo').val('');
                $('#itUpdateTo').focus();
                return;
            }
            updateSvn(false, verId);
            $('#winUpdate').window("close");
        }

        function deploy(depId)  
        {
            var verId = $('#itUpdateTo').val();
            verId = verId < 0 ? 0 : verId;
            var params = {op:'deploy',
                Id: depId,
                verId: verId||0
            };
		    $.fn.postJsonData(actionUrl, params, resize);
        }
        
        function deployPython(depId){
            var verId = $('#itUpdateTo').val();
            verId = verId < 0 ? 0 : verId;
            var params = {op:'deployPython',
                Id: depId,
                verId: verId||0
            };
		    $.fn.postJsonData(actionUrl, params, resize);
        }
        
        function quickDeploy(depId)  
        {
            var verId = $('#itUpdateTo').val();
            verId = verId < 0 ? 0 : verId;
            var params = {op:'quickdeploy',
                Id: depId,
                verId: verId||0
            };
		    $.fn.postJsonData(actionUrl, params, resize);
        }
        
        function updateCache(depId){
            var params = {op:'socketcmd',
                Id: depId,
                verId: 0,
                cmd: 'upcache'
            };
		    $.fn.postJsonData(actionUrl, params, function(data){
		        if(data.state && (data.state=='1'||data.state=='true')){
		            alert('提交成功!');
		        }else{
		            alert('出错:'+data.message || '未知错误');
		        }
		    });
        }
        
        function refreshResult(depId){
			$('#winAdd').window('open');
			$('#winAdd').attr("depId",depId);
			var params = {deployId: depId};
			$.fn.postJsonData('/Share/ActionService.aspx?action=2003&op=', params, function(data){
			    if(data){
	                $('#gridResult').datagrid('loadData',data);
	            }
			});
			
        }
        function selectLog(depId,serverId){
            var params = {
                op:'msmq',
                Id: depId,
                gameId: $('#hiGameId').val(),
                serverId:serverId
            };
            $('#winMessage .content').empty().html('<p>正在执行中...<p>');
		    $('#winMessage').window('open');
		    $.fn.postJsonData(actionUrl, params, function(data){
		        if(data){
		            var msg = '消息队列执行 ['+data.RunTime +']:<br>';
		            if($.isArray(data.Result)){
		                $.each(data.Result, function(i, item){
		                    msg += item.Name + ', 队列数:' + item.Value + ', 刷新时间:' + item.UpdateTime +'<br>';
		                });
		            }else{
		                msg += data.Result;
		            }
		            $('#winMessage .content').empty().html(msg);
		        }
		        else{
		            $('#winMessage .content').empty().html('error');
		        }
		    });
        }
	    function runPy(depId, serverId){
            var params = {
                op:'runpy',
                Id: depId,
                gameId: $('#hiGameId').val(),
                serverId:serverId
            };
            $('#winMessage .content').empty().html('<p>正在执行中...<p>');
		    $('#winMessage').window('open');
		    $.fn.postJsonData(actionUrl, params, function(data){
		        if(data){
		            var msg = 'Python执行 ['+data.RunTime +']:<br>'+data.Result;
		            $('#winMessage .content').empty().html(msg);
		        }
		        else{
		           $('#winMessage .content').empty().html('error');
		        }
		    });
	    }
        function resize(data){
            alert('操作成功！')
        }
        
    </script>

</asp:Content>
