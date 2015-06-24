package ScutDataLogic
{
	import flash.utils.ByteArray;
	public class StringUtils
	{
		/**
		 * 格式化字符串
		 * @param str   
		 * @param rest
		 * @return 
		 * 
		 */		
		public static function Format(str:String, ... rest):String
		{
			// Replace all of the parameters in the msg string.
			var len:uint = rest.length;
			
			if(len==0)return str;
			
			var args:Array;
			if (len == 1 && rest[0] is Array)
			{
				args = rest[0] as Array;
				len = args.length;
			}
			else
			{
				args = rest;
			}
//			var re:RegExp=/\{(\d+)\}/g;
			for (var i:int = 0; i < len; i++)
			{
				str = str.replace(new RegExp("\\{"+i+"\\}", "g"), args[i]);
			}
			return str;
		}
		
		
		public static function FormatTo(obj:Object):String{
			if(obj.toString().length==1){
				return " "+obj.toString();
			}
			return obj.toString();
		}
		
		/**
		 * 去左右空格;
		 * @param	char
		 * @return
		 */
		public static function trim(char:String):String{
			if(char == null){
				return null;
			}
			return char.replace(/^\s* | \s*$/g,"");
			//return rtrim(ltrim(char));
		}
		/**
		 * 去左空格;
		 * @param	char
		 * @return
		 */
		public static function ltrim(char:String):String{
			if(char == null){
				return null;
			}
			return char.replace(/^\s*/,"");
		}
		
		/**
		 * 去右空格
		 * @param	char
		 * @return
		 */
		public static function rtrim(char:String):String{
			if(char == null){
				return null;
			}
			return char.replace(/\s*$/,"");
		}
		
		
		/**
		 * 是否为空白;
		 * @param	char
		 * @return
		 */
		public static function isWhitespace(char:String):Boolean{
			switch (char){
				case "":
				case " ":
				case "\t":
				case "\r":
				case "\n":
				case "\f":
					return true;	
				default:
					return false;
			}
		}
		
//		/**
//		 * 返回随机标识符
//		 * @param	length
//		 * @param	radix
//		 * @return  String
//		 */
//		public static function createRandomIdentifier(length:uint, radix:uint = 61):String {
//			const characters:Array = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 
//				'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 
//				'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 
//				'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 
//				'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 
//				'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 
//				'y', 'z'];
//			const id:Array         = [];
//			radix                  = (radix > 61) ? 61 : radix;
//			
//			while (length--) {
//				id.push(characters[NumberUtil.randomIntegerWithinRange(0, radix)]);
//			}
//			
//			return id.join('');
//		}
		
		
		/**
		 * 是否是数值字符串;
		 * @param	char
		 * @return
		 */
		public static function isNumber(char:String):Boolean{
			if(char == null) return false;
			var trimmed:String = StringUtils.trim(char);
			if (trimmed.length < char.length || char.length == 0)
				return false;
			
			return !isNaN(Number(char));
		}
		
		//是否为Double型数据;
		public static function isDouble(char:String):Boolean{
			char = trim(char);
			var pattern:RegExp = /^[-\+]?\d+(\.\d+)?$/; 
			var result:Object = pattern.exec(char);
			if(result == null) {
				return false;
			}
			return true;
		}
		//Integer;
		public static function isInteger(char:String):Boolean{
			if(char == null){
				return false;
			}
			char = trim(char);
			var pattern:RegExp = /^[-\+]?\d+$/; 
			var result:Object = pattern.exec(char);
			if(result == null) {
				return false;
			}
			return true;
		}
		//English;
		public static function isEnglish(char:String):Boolean{
			if(char == null){
				return false;
			}
			char = trim(char);
			var pattern:RegExp = /^[A-Za-z]+$/; 
			var result:Object = pattern.exec(char);
			if(result == null) {
				return false;
			}
			return true;
		}
		//中文;
		public static function isChinese(char:String):Boolean{
			if(char == null){
				return false;
			}
			char = trim(char);
			var pattern:RegExp = /^[\u0391-\uFFE5]+$/; 
			var result:Object = pattern.exec(char);
			if(result == null) {
				return false;
			}
			return true;
		}
		
		/**
		 * 是否双字节
		 * @param	char
		 * @return
		 */
		public static function isDoubleChar(char:String):Boolean{
			if(char == null){
				return false;
			}
			char = trim(char);
			var pattern:RegExp = /^[^\x00-\xff]+$/; 
			var result:Object = pattern.exec(char);
			if(result == null) {
				return false;
			}
			return true;
		}
		
		/**
		 * 是否含有中文字符
		 * @param	char
		 * @return
		 */
		public static function hasChineseChar(char:String):Boolean{
			if(char == null){
				return false;
			}
			char = trim(char);
			var pattern:RegExp = /[^\x00-\xff]/; 
			var result:Object = pattern.exec(char);
			if(result == null) {
				return false;
			}
			return true;
		}
		
		/**
		 * 注册字符;
		 * @param	char
		 * @param	len
		 * @return
		 */
		public static function hasAccountChar(char:String,len:uint=15):Boolean{
			if(char == null){
				return false;
			}
			if(len < 10){
				len = 15;
			}
			char = trim(char);
			var pattern:RegExp = new RegExp("^[a-zA-Z0-9][a-zA-Z0-9_-]{0,"+len+"}$", ""); 
			var result:Object = pattern.exec(char);
			if(result == null) {
				return false;
			}
			return true;
		}
		/**
		 * 去除标点符号(包括空格\回车\换行)
		 * @param	source
		 * @return
		 */
		public static function getLettersFromString(source:String):String {
			var pattern:RegExp = /[[:digit:]|[:punct:]|\s]/g;
			return source.replace(pattern, '');
		}
		/**
		 * 删除所有换行
		 * @param text
		 * @return 
		 * 
		 */
		public static function removeBR(text:String):String
		{
			return text.replace(/\r|\n|<br>/g,"");
		}
		
		/**
		 * 插入换行符使得字体可以竖排
		 *  
		 * @param str
		 * @return 
		 * 
		 */
		public static function vertical(str:String):String
		{
			var result:String = "";
			for (var i:int = 0;i < str.length;i++)
			{
				result += str.charAt(i);
				if (i < str.length - 1)
					result += "\r";
			}
			return result;
		}
		
		/**
		 * 获得ANSI长度（中文按两个字符计算）
		 * @param data
		 * @return 
		 * 
		 */
		public static function getANSILength(data:String):int
		{
			var byte:ByteArray = new ByteArray();
			byte.writeMultiByte(data,"gb2312");
			return byte.length;
		}
		/**
		 * 添加新字符到指定位置
		 **/        
		public static function addAt(char:String, value:String, position:int):String {  
			if (position > char.length) {  
				position = char.length;  
			}  
			var firstPart:String = char.substring(0, position);  
			var secondPart:String = char.substring(position, char.length);  
			return (firstPart + value + secondPart);  
		} 
		/**
		 * 根据字符串 手动换行 
		 * @param str
		 * @return 
		 * 
		 */		
		public static function getStringWidth(str:String):String
		{
			var strLength:int;
			var totalWidth:int;
			var singleWidth:int;
			
			var index:int;
			var strAry:Array=str.split("");
			var strResult:String;
			
			strLength = str.length;
			for (var i:int = 0; i < strLength; i++ )
			{
				if (str.substr(i,1).search(/[A-Z@％。@【\？]/g) > -1){//匹配大写字母以及圆角标点
					singleWidth = 1;
				}
				if (str.substr(i, 1).search(/[\u4E00-\u9FA5\uf900-\ufa2d]/g) > -1) {//匹配汉字
					singleWidth = 2;
				}
				if (str.substr(i, 1).search(/[0-9a-z]/g) > -1) {//匹配小写字母 数字 以及半角标点
					singleWidth = 1;
				}
				totalWidth += singleWidth;
				if(totalWidth * 6>230){//230
					index=i;
					strResult=addAt(strResult,"\n",index);
					break;
				}else{
					strResult=str;
				}
			}
			return strResult;
		}
	}
}