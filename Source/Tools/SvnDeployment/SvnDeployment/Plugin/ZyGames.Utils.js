//扩展工具
(function (){
    $.extend($.fn, {
        editTab: function(){
            var code, sel, tmp, r
	        var tabs=""
	        event.returnValue = false
	        sel =event.srcElement.document.selection.createRange()
	        r = event.srcElement.createTextRange()

	        switch (event.keyCode)
	        {
		        /*case (8):
			        if (!(sel.getClientRects().length > 1))
			        {
				        event.returnValue = true
				        return
			        }
			        code = sel.text
			        tmp = sel.duplicate()
			        tmp.moveToPoint(r.getBoundingClientRect().left, sel.getClientRects()[0].top)
			        sel.setEndPoint("startToStart", tmp)
			        sel.text = sel.text.replace(/^\t/gm, "")
			        code = code.replace(/^\t/gm, "").replace(/\r\n/g, "\r")
			        r.findText(code)
			        r.select()
			        break*/
		        case (9):
			        if (sel.getClientRects().length > 1)
			        {
				        code = sel.text
				        tmp = sel.duplicate()
				        tmp.moveToPoint(r.getBoundingClientRect().left, sel.getClientRects()[0].top)
				        sel.setEndPoint("startToStart", tmp)
				        sel.text = "\t"+sel.text.replace(/\r\n/g, "\r\t")
				        code = code.replace(/\r\n/g, "\r\t")
				        r.findText(code)
				        r.select()
			        }
			        else
			        {
				        sel.text = "\t"
				        sel.select()
			        }
			        break
		        /*case (13):
			        tmp = sel.duplicate()
			        tmp.moveToPoint(r.getBoundingClientRect().left, sel.getClientRects()[0].top)
			        tmp.setEndPoint("endToEnd", sel)

			        for (var i=0; tmp.text.match(/^[\t]+/g) && i<tmp.text.match(/^[\t]+/g)[0].length; i++)	tabs += "\t"
			        sel.text = "\r\n"+tabs
			        sel.select()
			        break*/
		        default:
			        event.returnValue = true
			        break
	        }
        },
        toExcel: function(tableId, title){
            function exporttoxls(inTbl) 
            { 
                //初始化控件； 
                var ExcelApp = new ActiveXObject("Excel.Application"); 
                if(ExcelApp == null || ExcelApp === ""){ 
                    confirm('输出到Excel需要启用ActiveX控件，请检查是否进行正确设置？'); 
                    return; 
                } 
                //ExcelSheet = new ActiveXObject("Excel.Sheet");
                ExcelApp.DefaultSaveFormat = 56; //xlExcel8s
                var oWorkbooks = ExcelApp.Workbooks.Add();
                var ExcelSheet = oWorkbooks.ActiveSheet;
                ExcelSheet.Application.Visible = true; 
                //从表格获取数据导出到控件中； 
                var gridFields = $(inTbl).datagrid('getColumnFields');
                var gridRows = $(inTbl).datagrid('getRows');
                var rowIndex = 1;
                //列头
                for (var i=0; i < gridFields.length; i++){
                    var column = gridFields[i] || {};
                    var field = $(inTbl).datagrid('getColumnOption', column);
                    ExcelSheet.Cells(rowIndex, i+1).Value = field.title;                    
                    ExcelSheet.Cells(rowIndex, i+1).WrapText=true;
                    ExcelSheet.Columns(i+1).ColumnWidth = field.width * 0.119;
                }
                rowIndex++;
                for (var i=0; i < gridRows.length; i++){
                    var rows = gridRows[i];
                    for (var j=0; j < gridFields.length; j++){
                        ExcelSheet.Cells(rowIndex, j+1).Value = rows[gridFields[j]];
                    }
                    rowIndex++;
                }
                //输出到excel； 
                window.blur(); 
                ExcelSheet.Activate(); 
                window.blur();
                //ExcelSheet.Application.print(); 
            }
            function getXlsFromTbl(inTblId, title)
            {
                try {
                    var allStr = "";
                    var curStr = "";
                    if (inTblId) {
                        curStr = getTblData(inTblId, title);
                    }
                    if (curStr != null) {
                        allStr += curStr;
                    }
                    else {
                        alert("你要导出的表不存在");
                        return;
                    }
                    var fileName = getExcelFileName(title);
                    doFileExport(fileName, allStr);
                }
                catch(e) {
                    alert("导出发生异常:" + e.name + "->" + e.description + "!");
                }
            }
            function getTblData(inTbl, title) {            
                var outStr = "";
                var gridFields = $(inTbl).datagrid('getColumnFields');
                var gridRows = $(inTbl).datagrid('getRows');
                var rownumber = 0;
                for (var i=0; i < gridFields.length; i++){
                    var column = gridFields[i] || {};
                    var field = $(inTbl).datagrid('getColumnOption', column);
                    
                    if (i == 0 && rownumber > 0) {
                        outStr += " \t";
                        rownumber -= 1;
                    }
                    outStr += field.title + "\t";
                    if (field.colSpan > 1) {
                        for (var k = 0; k < field.colSpan - 1; k++) {
                            outStr += " \t";
                        }
                    }
                    if (i == 0) {
                        if (rownumber == 0 && field.rowSpan > 1) {
                            rownumber = field.rowSpan - 1;
                        }
                    }
                }
                outStr += "\r\n";
                for (var i=0; i < gridRows.length; i++){
                    var rows = gridRows[i];
                    for (var j=0; j<gridFields.length; j++){
                        outStr += rows[gridFields[j]] + "\t";
                    }
                    outStr += "\r\n";
                }
                return outStr;
            }
            function getExcelFileName(title) {
                var d = new Date();
                var curYear = d.getYear();
                var curMonth = "" + (d.getMonth() + 1);
                var curDate = "" + d.getDate();
                var curHour = "" + d.getHours();
                var curMinute = "" + d.getMinutes();
                var curSecond = "" + d.getSeconds();
                if (curMonth.length == 1) {
                    curMonth = "0" + curMonth;
                }
                if (curDate.length == 1) {
                    curDate = "0" + curDate;
                }
                if (curHour.length == 1) {
                    curHour = "0" + curHour;
                }
                if (curMinute.length == 1) {
                    curMinute = "0" + curMinute;
                }
                if (curSecond.length == 1) {
                    curSecond = "0" + curSecond;
                }
                var fileName = (title || "table") + "_" + curYear + curMonth + curDate + "_"
                        + curHour + curMinute + curSecond + ".csv";
                return fileName;
            }
            function doFileExport(inName, inStr) {
                var xlsWin = null;
                if (!!document.all("glbHideFrm")) {
                    xlsWin = glbHideFrm;
                }
                else {
                    var width = 6;
                    var height = 4;
                   var openPara = "left=" + (window.screen.width / 2 - width / 2)
                            + ",top=" + (window.screen.height / 2 - height / 2)
                            + ",scrollbars=no,width=" + width + ",height=" + height;
                    xlsWin = window.open("", "_blank", openPara);
                }
                xlsWin.document.write(inStr);
                xlsWin.document.close();
                xlsWin.document.execCommand('Saveas', true, inName);
                xlsWin.close();
            }
            exporttoxls(tableId);
            //getXlsFromTbl(tableId,title);
        },    
        formatterJson: function(val, format) {
            if($.isFunction(format)){
                val = format(val);
            }else{
                val = val.replace(/\\r\\n/g,'\r\n');
                val = val.replace(/\\n/g,'\n');
                val = val.replace(/\\u0027/g,'\'');
            }
            return val;
        },
        formatterHtml: function(val, format) {
            if($.isFunction(format)){
                val = format(val);
            }else{
                val = val.replace(/\\r\\n/g,'<br>');
                val = val.replace(/\\n/g,'<br>');
                val = val.replace(/\\u0027/g,'\'');
            }
            return val;
        },
        getFormParam: function(express){
            var formParam = {};
            $(express).each(function(index, param){
                if($(this).attr('type') == 'checkbox'){
                    var val = this.checked  ? true : false;
                    formParam[param.name] = val;
                } else{
                    formParam[param.name] = param.value;
                }
            });
            return formParam;
        },
        bindFormData: function(express, record){
            $(express).each(function(index, param){
                var name = param.name;
                if(name){
                    var val = $.fn.formatterJson(record[name] || '');
                    if($(this).attr('type') == 'checkbox'){
                        if(val === true || val === 'true' || val === '1'){
                            $(this).attr('checked', true);
                        }else{
                            $(this).attr('checked', false);
                        }
                    } else{
                        $(this).val(val);
                    }
                }
            });
        },
        getJsonData: function(url, params, callback, errorback){
            $.getJSON(url, params, function(data){
                if(data!==undefined && data.rows){
                    if(callback) callback(data);
                }else{
                    if(errorback) errorback(data);
                }
            });
        },
        postJsonData: function(url, params, callback){
            $.post(
                url, 
                params, 
                function(data){
                     data  = data === null || data === "" ? '{"total": 0, "rows":[]}' : data;     
                    jsonData = $.parseJSON(data);               
                    if(callback){
                        callback(jsonData);
                    }
                }
            )
            .error(function(XMLHttpRequest, textStatus, errorThrown){
                //alert('Error:' + XMLHttpRequest.responseText);
                alert('Error:' + textStatus);
            });
            return true;
        },
        getAutoWidth: function(percent){ 
            return document.body.clientWidth*percent; 
        },
        getAutoHeight: function(){ 
            return $(window).height()-document.body.clientHeight; 
        },
        //转换JSON时间
        parseJsonDate: function (dateStr) {
             function scanDate(obj, dateParser) {
                 for (var key in obj) {
                     obj[key] = dateParser(key, obj[key]);
                     if (typeof (obj[key]) === 'object') {
                         scanDate(obj[key], dateParser);
                     }
                 }
             }
             function jsonDateParser(key, value) {
     
                if (typeof value === 'string') {
                     var a = (/^\/Date\((\d+)(([\+\-])(\d\d)(\d\d))?\)\//gi).exec(value);
                     if (a) {
                         var utcMilliseconds = parseInt(a[1], 10);
                         //utcMilliseconds += ((a[3] == '-') ? -1 : 1) * (parseInt(a[4], 10) + (parseInt(a[5], 10) / 60.0)) * 60 * 60 * 1000;
                         var date = new Date(utcMilliseconds);
                         value = RE.formatDate(date, 'yyyy-MM-dd hh:mm:ss');
                     }
                 }
                 return value;
             }
             var obj = eval('({Date:"' + dateStr + '"})');
             scanDate(obj, jsonDateParser);
             var dateValue = obj["Date"];
             return dateValue;
         },
     
        formatDate: function (date, format) {
             var o = {
                 "M+": date.getMonth() + 1, //month
                 "d+": date.getDate(),    //day
                 "h+": date.getHours(),   //hour
                 "m+": date.getMinutes(), //minute
                 "s+": date.getSeconds(), //second
                 "q+": Math.floor((date.getMonth() + 3) / 3),  //quarter
                 "S": date.getMilliseconds() //millisecond
             }
             if (/(y+)/.test(format)) {
                 format = format.replace(RegExp.$1, (date.getFullYear() + "").substr(4 - RegExp.$1.length));
             }
             for (var k in o) {
                 if (new RegExp("(" + k + ")").test(format)) {
                     format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
                 }
             }
             return format;
        },
         
        ClientSideStrongPassword: function(sPW) {
            function CharMode(iN){  
                if (iN>=48 && iN <=57) //数字  
                    return 1;  
                if (iN>=65 && iN <=90) //大写字母  
                    return 2;  
                if (iN>=97 && iN <=122) //小写  
                    return 4;  
                else
                    return 8; //特殊字符  
            }  
              //bitTotal函数  
              //计算出当前密码当中一共有多少种模式  
            function bitTotal(num){  
                modes=0;  
                for (i=0;i<4;i++){  
                    if (num & 1) modes++;  
                    num>>>=1;  
                }  
                return modes;  
            }
            if (sPW.length<=4) return 0; //密码太短  
            Modes=0;  
            for (i=0;i<sPW.length;i++){  
                //测试每一个字符的类别并统计一共有多少种模式.  
                Modes|=CharMode(sPW.charCodeAt(i));  
            }  
            return bitTotal(Modes);  
        }

    });

    $.extend($.fn.datagrid.methods, {
        //显示遮罩
        loading: function(jq){
            return jq.each(function(){
                $(this).datagrid("getPager").pagination("loading");
                var opts = $(this).datagrid("options");
                var wrap = $.data(this,"datagrid").panel;
                if(opts.loadMsg)
                {
                    $("<div class=\"datagrid-mask\"></div>").css({display:"block",width:wrap.width(),height:wrap.height()}).appendTo(wrap);
                    $("<div class=\"datagrid-mask-msg\"></div>").html(opts.loadMsg).appendTo(wrap).css({display:"block",left:(wrap.width()-$("div.datagrid-mask-msg",wrap).outerWidth())/2,top:(wrap.height()-$("div.datagrid-mask-msg",wrap).outerHeight())/2});
                }
            });
        },
        //隐藏遮罩
        loaded: function(jq){
            return jq.each(function(){
                $(this).datagrid("getPager").pagination("loaded");
                var wrap = $.data(this,"datagrid").panel;
                wrap.find("div.datagrid-mask-msg").remove();
                wrap.find("div.datagrid-mask").remove();
            });
        }
    });
})(jQuery);