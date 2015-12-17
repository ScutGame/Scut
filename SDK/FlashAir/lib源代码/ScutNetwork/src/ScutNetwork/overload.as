package ScutNetwork
{	
	import flash.utils.getQualifiedClassName;
	import flash.utils.getQualifiedSuperclassName;
	import flash.utils.getDefinitionByName;
	
	/**
	 * 匹配规则：先进行精确匹配，若无则进行模糊匹配
	 * 
	 * 数值类判断规则：
	 * 非负整型数据将优先判断成int，并同时匹配uint，若超出int大小将匹配为uint，超出uint大小则匹配为Number
	 * 负整型数据将优先判断为int，超出int大小则匹配为Number
	 * 带小数点数据将匹配为Number，要指定为Number型数据请写上小数点，比如2.0
	 * Boolean将模糊匹配为所有数值类
	 * 所有数值类都能匹配Boolean,NaN或0为false，其他为true
	 * 模糊匹配将在不超出数值表示范围的情况下进行转换
	 * 
	 * 类类型匹配规则：
	 * 将精确匹配自己的类，模糊匹配父类
	 * 
	 * 所有类型都能模糊匹配为String，但在模糊匹配时优先级低于Boolean
	 * 
	 * null为通配*，会做为最低匹配方式
	 * 
	 * 有多个模糊匹配将调用assert抛出异常
	 * 
	 * XML和XMLList类型调用可能有问题
	 * 
	 * 调用方式overload(functionNumber, 
	 			[function1, [paramType1], defaultParamNumber],
				[function3, [paramType1, paramType2], defaultParamNumber],
				[function2, [paramType1, paramType2, paramType3], defaultParamNumber],
				paramToPassIn);
	 * 例子： overload(2, [addChild1, [CCNode], 0],
				[addChild2, [CCNode, int, int], 1],
				args);
	 */
	public function overload(...args):*
	{
		//一系列合法判断
		assert("number" == typeof(args[0]))
		var nArg:int = args[0];
		
		assert(nArg >= 1 && args.length == nArg + 2);
		
		var per:int = 1;
		while(per <= nArg
			&& "Array" == getQualifiedClassName(args[per])
			&& "function" == typeof(args[per][0])
			&& "Array" == getQualifiedClassName(args[per][1])
			&& "number" == typeof(args[per++][2]))
		{}
		
		assert(per == nArg + 1);
		assert("Array" == getQualifiedClassName(args[nArg]), "[overload] param error");
		
		
		//结果函数
		var func:Function = null;
		//函数列表
		var funcArr:Array = args.concat();
		//传入参数列表
		var params:Array = args[nArg+1];
		funcArr.pop();
		funcArr.shift();
		
		//***************内部函数begin****************************************************
		
		//去除参数长度不符合的情况，item[0]为函数，item[1]为函数须传入的参数，item[2]为默认参数个数
		var filterByParamNum:Function = function (item:*, index:int, array:Array):Boolean
		{		
			if(this.length < item[1].length - item[2]
				|| this.length > item[1].length)
			{
				return false;
			}
			
			return true;
		}
		
		//去除参数类型不符合的情况
		var filterByParamType:Function = function (item:*, index:int, array:Array):Boolean
		{
			var ret:Boolean = false;
			var i:int = 0;
			
			for(; i<this.length; i++)
			{
				var retTmp:Boolean = false;
				var stype:* = item[1][i];
				
				switch(typeof(this[i]))
				{
					//int, uint, Number
					case "number":
					{
						if(this[i] is stype)
						{
							retTmp = true;
						}else if(params[i]<int.MAX_VALUE && params[i]>int.MIN_VALUE)
						{
							retTmp = true;
						}
						else if(params[i]<uint.MAX_VALUE && params[i]>uint.MIN_VALUE)
						{
							retTmp = true;
						}
						break;
					}
						//Boolean
					case "boolean":
					{
						if(Boolean == stype || int == stype || uint == stype || Number == stype || String == stype)
							retTmp = true;
						break;
					}
						//Object,Array
					case "object":
					{
						if(null === this[i])
						{
							retTmp = true;
						}
						else if("Array" == getQualifiedClassName(this[i]))
						{
							if(Array == stype)
								retTmp = true;
						}else if(this[i] is stype || String == stype)
						{
							retTmp = true;
						}
						
						break;
					}
						//String,Function,XML,XMLList
					case "string":
					case "function":
					case "xml":
					{
						if(null === this[i] || this[i] is stype || String == stype)
							retTmp = true;
						break;
					}
					default:
						break;
				}
				
				if(!retTmp)
					break;
			}
			
			if(i == this.length)
				ret = true;
			
			return ret;
		}
		//***************内部函数end****************************************************
		
		funcArr = funcArr.filter(filterByParamNum, params);
		
		funcArr = funcArr.filter(filterByParamType, params);
		
		assert(funcArr.length >= 1, "[overload] no func matching");
		
		if(funcArr.length == 1)
		{
			func = funcArr[0][0];
		}else
		{
			//更佳匹配选择
			var moreFit:Array = new Array(funcArr.length);
			//是否已存在更佳匹配
			var moreFitChoose:Boolean = false;
			for(var ar:int=0; ar<funcArr.length; ar++)
			{
				moreFit[ar] = false;
			}
			//逐个参数对比
			var allSame:Boolean = true;
			endParmFor: for(var i:int=0; i<params.length; i++)
			{
				//过滤函数提供的参数相同的情况
				var parTypeComp:* = funcArr[0][1][i];
				var j:int=1;
				
				for(; j<funcArr.length; j++)
				{
					if(parTypeComp != funcArr[j][1][i])
					{
						break;
					}
				}
				
				if(j == funcArr.length)
				{
					continue;
				}
				
				allSame = false;
				
				//选择匹配
				var fitType:* = parTypeComp;
				var bestFitType:* = null;
				var oldFitType:* = fitType;
				switch(typeof(params[i]))
				{
					//可选择的匹配(从高到低)：int uint Number Boolean String
					case "number":
					{
						//是否匹配int
						var bint:Boolean = (params[i]<int.MAX_VALUE) && (params[i]>int.MIN_VALUE);
						//是否匹配uint
						var buint:Boolean = (params[i]<uint.MAX_VALUE) && (params[i]>uint.MIN_VALUE);
						
						//非整数最佳匹配Number，整数按范围选择最佳匹配
						bestFitType = (params[i] is int || params[i] is uint) ? (bint ? int : (buint ? uint : Number)) : Number;
						if(fitType == bestFitType)
							break;
						
						numLoop: for(var ni:int=j; ni<funcArr.length; ni++)
						{
							//相同则无需判断
							if(funcArr[ni][1][i] == fitType)
								continue;
							
							//找到最佳匹配
							if(bestFitType == funcArr[ni][1][i])
							{
								fitType = bestFitType;
								break;
							}
							
							switch(funcArr[ni][1][i])
							{
								case int:
								{
									if(!bint)
										continue numLoop;
									
									if(params[i] is Number)
									{
										fitType = int;
									}

									break;
								}
								case uint:
								{
									if(!buint)
										continue numLoop;
									//此种情况只可能是bestFitType=int
									if(fitType != int)
									{
										fitType = uint;
									}
									break;
								}
								case Number:
								{
									if(fitType != uint)
									{
										fitType = Number;
									}
									break;
								}
								case Boolean:
								{
									if(fitType == String || fitType == null)
									{
										fitType = Boolean;
									}
									break;
								}
								case String:
								{
									if(fitType == null)
									{
										fitType = String;
									}
									break;
								}
								case null:
								{
									break;
								}
								default:
									break;
							}//end switch(funcArr[s][1][i])
							break;
						}//end numLoop: for
						break;
					}//end case "number"
					//可选择的匹配(从高到低)：Boolean int uint Number String
					case "boolean":
					{
						bestFitType = Boolean;
						if(fitType == bestFitType)
							break;
						
						for(var bi:int=j; bi<funcArr.length; bi++)
						{
							var tBlTmp:* = funcArr[bi][1][i];
							//相同则无需判断
							if(tBlTmp == fitType)
								continue;
							
							//找到最佳匹配
							if(tBlTmp == bestFitType)
							{
								fitType = bestFitType;
								break;
							}
							
							if(tBlTmp == int)
								fitType = int;
							else if(tBlTmp==uint && fitType!=int)
								fitType = uint;
							else if(tBlTmp==Number && fitType!=int && fitType!=uint)
								fitType = Number;
							else if(tBlTmp==String && fitType!=int && fitType!=uint && fitType !=Number)
								fitType = String;
							
							//tBlTmp为null时fitType为null
							
						}//end for(var bi:int=j; bi<funcArr.length; bi++)
						break;
					}//end case "boolean":
					case "object":
					{
						if(null === params[i])
						{
							bestFitType = null;
							fitType = null;
						}
						else if("Array" == getQualifiedClassName(params[i]))
						{
							bestFitType = Array;
							for(var ai:int=j; ai<funcArr.length; ai++)
							{
								var tAlTmp:* = funcArr[ai][1][i];
								//相同则无需判断
								if(tAlTmp == fitType)
									continue;
								
								//找到最佳匹配
								if(tAlTmp == bestFitType)
								{
									fitType = bestFitType;
									break;
								}
								
								if(tAlTmp == String)
								{
									fitType = String;
								}
								//tAlTmp为null时fitType为null
							}//end for(var ai:int=j; ai<funcArr.length; ai++)
						}else
						{
							bestFitType = Object;
							var bestFitTypeString:String = getQualifiedClassName(params[i]);
							var fitTypeString:String = getQualifiedClassName(Object);
							
							for(var oi:int=j; oi<funcArr.length; oi++)
							{
								var tOlTmp:* = funcArr[oi][1][i];
								
								//相同则无需判断
								if(tOlTmp == fitType)
									continue;
								
								//找到最佳匹配
								if(getQualifiedClassName(tOlTmp) == bestFitTypeString)
								{
									fitType = tOlTmp;
									break;
								}
								
								
								for(var supObjStr:String = getQualifiedSuperclassName(params[i]);
									supObjStr != fitTypeString;
									supObjStr = getQualifiedSuperclassName(getDefinitionByName(supObjStr)))
								{
									if(getQualifiedClassName(tOlTmp) == supObjStr)
									{
										fitType = tOlTmp;
										break;
									}
								}
							}//end for(var oi:int=j; oi<funcArr.length; oi++)
						}
						break;
					}
					case "string":
					{
						bestFitType = String;
						
						for(var si:int=j; si<funcArr.length; si++)
						{
							if(funcArr[si][1][i] == null)
								continue;
							
							if(funcArr[si][1][i] == String)
							{
								fitType = String;
								break;
							}
						}
						break;
					}
					case "function":
					{
						bestFitType = Function;
						for(var fi:int=j; fi<funcArr.length; fi++)
						{
							if(funcArr[fi][1][i] == null)
								continue;
							
							if(funcArr[fi][1][i] == Function)
							{
								fitType = Function;
								break;
							}else if(funcArr[fi][1][i] == String)
							{
								fitType = String;
							}
						}
						break;
					}
					case "xml":
					{
						if(getQualifiedClassName(params[i]) == "XML")
							bestFitType = XML;
						else
							bestFitType = XMLList;
						break;
						
						for(var xi:int=j; xi<funcArr.length; xi++)
						{
							var tXlTmp:* = funcArr[xi][1][i];
							
							//相同则无需判断
							if(tXlTmp == fitType)
								continue;
							
							//找到最佳匹配
							if(tXlTmp == bestFitType)
							{
								fitType = bestFitType;
								break;
							}
							
							if(tXlTmp != null)
							{
								fitType = tXlTmp;
							}
						}
					}
				}//end switch(typeof(params[i]))
				
				//选出匹配
				if(fitType == null && bestFitType == null)
				{//空值处理
					continue;
				}else
				{
					var bFitTmp:Boolean = false;
					for(var chi:int=0; chi<funcArr.length; chi++)
					{
						if(funcArr[chi][1][i] == fitType)
						{
							if(moreFitChoose)
							{
								if(!moreFit[chi])
									break endParmFor;
							}else
							{
								moreFit[chi] = true;
								bFitTmp = true;
							}
						}else
							moreFit[chi] = false;
					}
					
					if(bFitTmp)
						moreFitChoose = true;						
				}
			}//end for(var i:int=0; i<params.length; i++)
			
			if(allSame)
			{
				assert(false, "[overload]: cann't choose whitch function to loaded.");
			}else
			{
				for(var funi:int=0; funi<funcArr.length; funi++)
				{
					if(moreFit[funi])
					{
						if(func == null)
						{
							func = funcArr[funi][0];
						}else
						{
							func = null;
							assert(false, "[overload]: cann't choose whitch function to loaded.");
						}
					}
				}				
			}
			
			
		}
		
		if(func != null)
		{
			return func.apply(func, params);
		}
		
		assert(false, "[overload] func is null");
		return null;
	}
}