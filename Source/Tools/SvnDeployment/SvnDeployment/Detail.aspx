<%@ Page Title="" Language="C#" MasterPageFile="WebSite.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="WebSite.SvnDeployment.Detail" %>

<asp:Content ID="contentHead" ContentPlaceHolderID="contentPlaceHolderHead" runat="server">
	<title>详情</title>
</asp:Content>
<asp:Content ID="contentBody" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
<div class="sub-nav">&gt;&gt;项目发布&gt;&gt;部署方案</div>
	<div style="text-align:center">
	<p id="pTitle" class="title" runat="server">
	</p>
	<div class="FixedWidth480">
		<p class="Left">
			<b>版本号</b>
		</p>
		<div id="divVersionNumber" class="FixedWidth440" runat="server">
		</div>
		<p class="Left">
			<b>版本说明</b>
		</p>
		<div id="divVersionDescription" style="text-align: left;" class="FixedWidth440" runat="server">
		</div>
		<p class="Left">
			<b>SVN Revision</b>
		</p>
		<div id="divRevision" class="FixedWidth440" runat="server">
		</div>
		<p class="Left">
			<b>提交时间</b>
		</p>
		<div id="divPublishedAt" class="FixedWidth440" runat="server">
		</div>
		<p class="Left">
			<b>当前状态</b>
		</p>
		<div id="divStatus" class="FixedWidth440" runat="server">
		</div>
		<p class="Left">
			<b>其它信息</b>
		</p>
		<div><input type="button" value="刷新" onclick="top.window.location = location;" /></div>
		<div id="divStatusXt" style="text-align: left; font-size: x-small;" class="FixedWidth440" runat="server">
		</div>
	</div>
	</div>
</asp:Content>
