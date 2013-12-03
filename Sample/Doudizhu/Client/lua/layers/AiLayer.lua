------------------------------------------------------------------
--AiLayer.lua
-- Author     : JMChen
-- Version    : 1.0.0.0
-- Date       : 2011-10-15
-- Description:
------------------------------------------------------------------


module("AiLayer", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local mCardTable=nil
local bombTable=nil
local planeTable=nil
local singleTable=nil
local doubleTable=nil
local doubleTable1={}
local planeTable1={}

--
function getCards(table,type,cardTable,value)
mCardTable={}
mCardTable=table
--[[
1 = 单牌,2 = 对牌,3 = 王炸,4 = 三张,5 = 三带一,6 = 三带二,7 = 炸弹,8 = 顺子,
9 = 四带二,10 = 连对,11 = 飞机,12 = 飞机带二,13 = 二连飞机带二对,
--]]
local returnTable={}
bombTable=nil
doubleTable={}
planeTable={}
singleTable={}
doubleTable1={}
planeTable1={}
getSameCard()
if type==1 then
	returnTable= getLargeVal(value)
elseif type==2 then
	returnTable=getTwoLargeVal(value)
elseif type==4 then
	returnTable=getThreeLargeVal(value)
elseif type==5 then
	returnTable=getThreeLargeVal(value,1)
elseif type==6 then
	returnTable=getThreeLargeVal(value,2)
elseif type==7 then
	returnTable=getBombLargeVal(value,2)
elseif type==8 then
	returnTable=getShunLargeVal(cardTable,value)
elseif type==9 then
	returnTable=getSiDaiEr(cardTable,value)
elseif type==10 then
	returnTable=getLianDui(cardTable)
elseif type==11 then
	returnTable=getPlane(cardTable)
elseif type==12 then
	returnTable=getPlane(cardTable,1)
elseif type==13 then
	returnTable=getPlane(cardTable,2)
end

--如果没有的话出 有炸弹出炸弹
if #returnTable<=0 and bombTable  and type~=7 then
	local bombTable1=nil
	for k,v in pairs(bombTable) do
		bombTable1=v
		break
	end
	returnTable=bombTable1
end

--王炸
if #returnTable<=0  and singleTable and singleTable[18] and  singleTable[19] then
		ZyTable.push_back(returnTable,singleTable[18][1])	
		ZyTable.push_back(returnTable,singleTable[19][1])	
end

if type==3 then
	returnTable={}
end

return returnTable or {}
end;


--获取单张大牌
function getLargeVal(cardValue,limitValue)
	local returnTable={}	
	--
	limitValue=limitValue or 0
	local cardTable={}
	for k, v in pairs(singleTable) do
		ZyTable.push_back(cardTable,k)
	end
	table.sort(cardTable,function(a,b) return a<b end)
	
	local index=nil
	for k, v in pairs(cardTable) do
		if  v> cardValue  and limitValue ~=v then
			index=v
		--	ZyTable.push_back(returnTable,v)	
			break	
		end
	end
	if index and singleTable[index] then
		local info= singleTable[index]
		ZyTable.push_back(returnTable,info[1])	
	end
	--如果没有单张 拆
	if #returnTable<=0 then	
		for k=#mCardTable,1,-1 do
		local v=mCardTable[k]
		if   v.CardId %100> cardValue and   limitValue ~=v.CardId %100  then
			returnTable[#returnTable+1]=k	
			break			
		end
	end
	end
	return returnTable or {}
end;

--获取三张牌
function getThreeLargeVal(cardValue,type,limitValue)
	local value=cardValue
	local currentValue=nil
	limitValue=limitValue or 0
	local outTable={}
	local cardTable={}
	for k, v in pairs(planeTable1) do
		ZyTable.push_back(cardTable,k)
	end	
	table.sort(cardTable,function(a,b) return a<b end)
	
	local index=nil
	for k, v in pairs(cardTable) do
		if  v> cardValue  and v~=limitValue then
			currentValue=v
			index=v
			break	
		end
	end
	if index and planeTable1[index] then
		local	returnTable=planeTable1[index]
		local id=1
		for k, v in pairs(returnTable) do
			if id<=3 then
				ZyTable.push_back(outTable,v)		
				id=id+1
			end
		end
	end
	--取炸弹
	if #outTable<=0 then
		local returnTable={}
		for k, v in pairs(planeTable) do
			if k >cardValue and k~=limitValue  then
				currentValue=k
				returnTable =v
				break
			end
		end
		local index=1
		for k, v in pairs(returnTable) do
			if index<=3 then
				ZyTable.push_back(outTable,v)		
				index=index+1	
			end
		end	
	end
	--3带
	if #outTable>0 then
	if type==1 then		
			local singTabel=getLargeVal(2,currentValue)
			for k ,v in pairs(singTabel) do
				ZyTable.push_back(outTable,v)	
			end
			if #outTable<4 then
				outTable={}
			end		
	elseif type==2 then
		local returnTable=getTwoLargeVal(2,currentValue)
		local index=1
		for k, v in ipairs(returnTable) do
			if index<=2 then
				ZyTable.push_back(outTable,v)	
				index=index+1
			end
		end
		if #outTable<5 then
			outTable={}
		end
	end
	end
	return outTable
end;

--两倍
function getTwoLargeVal(cardValue,limitValue)
	local value=cardValue
	limitValue=limitValue or 0
	local outTable={}	
	local cardTable={}
	for k, v in pairs(doubleTable1) do
		ZyTable.push_back(cardTable,k)
	end
	
	table.sort(cardTable,function(a,b) return a<b end)
	
	local index=nil
	for k, v in pairs(cardTable) do
		if  v> cardValue and v~=limitValue then
			index=v
			break	
		end
	end
	--对子没有拆三
	if index and doubleTable1[index] then
		local	returnTable=doubleTable1[index]
		local id=1
		for k, v in pairs(returnTable) do
			if id<=2 then
				ZyTable.push_back(outTable,v)		
				id=id+1
			end
		end
	else
		local table= getThreeLargeVal(cardValue,nil,limitValue)
		local id=1	
		for k, v in pairs(table) do
			if id<=2 then
				ZyTable.push_back(outTable,v)		
				id=id+1
			end
		end
	end
	return outTable
end;

--炸弹
function getBombLargeVal(cardValue)
   
	local value=cardValue
	local outTable={}
	if bombTable then
        for k, v in pairs(bombTable) do
            if k>value then
                outTable=v
                break
            end
        end	
	end
	return outTable
end;

--顺子
function getShunLargeVal(cardTable,cardValue)
	local value=cardValue
	local shunLeng=#cardTable
	for k=#mCardTable,1,-1 do
		local v=mCardTable[k]
        if v.CardId %100>value and k>1 then
            local i=k
            local j=i-1
            local length=1
            local outTable={} 
            outTable[1]=k
            while mCardTable[j].CardId %100-mCardTable[i].CardId %100 <2 and j>1 do
                --连续
                if mCardTable[j].CardId %100-mCardTable[i].CardId %100 ==1 and mCardTable[j].CardId %100~=15 then
                    ZyTable.push_back(outTable,j)		
                    i=j									
                end
                j=j-1			
            end;
            if 	#outTable>=shunLeng then
            	     local returnTable={}
            	    for k, v in pairs(outTable) do
            	    	   if k<=shunLeng then
                    		ZyTable.push_back(returnTable,v) 	
            	    	   end
            	    end
            	    return  returnTable               
            end		
        end
	end
	return {}
end;

--连对
function  getLianDui(cardTable)
	local firstValue=cardTable[1].CardId %100
	local endValue=cardTable[#cardTable].CardId %100
	local cardLenth=endValue-firstValue+1

	local cardValue={}
	for k, v in pairs(doubleTable) do
		ZyTable.push_back(cardValue,k)
	end
	table.sort(cardValue,function(a,b) return a<b end)
	local num=#cardValue
	if num<cardLenth then
		return {}
	end	
	local   outTable={} 
	for k, v in pairs(cardValue) do
		if v>firstValue and k<#cardValue then
	            local i=k
	            local j=i+1
	            local   outTable1={} 
	            outTable1[1]=v
	            while j<=#cardValue and cardValue[j]-cardValue[i] <2   do
	                --连续
	                if cardValue[j]-cardValue[i] ==1 then
	                    ZyTable.push_back(outTable1,cardValue[j])		
	                    i=j									
	                end
	                j=j+1			
	            end;
	            if 	#outTable1>=cardLenth then
	         	outTable=outTable1
	            	  break           
		     else
				outTable={}
	         end           
		end
	end	
	local returnTable={}
		local id=1
	for k, v in pairs(outTable) do	
		if id>cardLenth then
		    break
		end
		if doubleTable[v] then
			local index=1
			for m, n in pairs(doubleTable[v]) do
				if index<=2 then
				ZyTable.push_back(returnTable,n)
				index=index+1
				end				
			end
		end
		 id= id+1
	end	
	return returnTable
end;

--获取飞机
function getPlane(cardTable,type)
	local firstValue=cardTable[1].CardId %100
	local cardLenth=#cardTable/3
	if type==1 then
		cardLenth=#cardTable/4
	elseif type==2 then
		cardLenth=#cardTable/5	
	end
	local endValue=firstValue+cardLenth-1
	local cardValue={}
	for k, v in pairs(planeTable) do
		ZyTable.push_back(cardValue,k)
	end
	if #cardValue<cardLenth then
		return {}
	end	
	local   outTable={} 
	for k, v in pairs(cardValue) do
		if v>firstValue and k<#cardValue then
	            local i=k
	            local j=i+1
	            local   outTable={} 
	            outTable[1]=v
	            while j<=#cardValue and cardValue[j]-cardValue[i] <2   do
	                --连续
	                if cardValue[j]-cardValue[i] ==1 then
	                    ZyTable.push_back(outTable,cardValue[j])		
	                    i=j									
	                end
	                j=j+1			
	            end;
	            if 	#outTable>=cardLenth then
	            	   break           
		     else
				outTable={}
	            end           
		end
	end
	
	local returnTable={}
	for k, v in pairs(outTable) do
		if planeTable[v] then
			local index=1
			for m, n in pairs(planeTable[v]) do
				if index<=3 then
				ZyTable.push_back(returnTable,n)	
				index=index+1
				end				
			end
		end
	end	
	
	if type==1 then
		if #mCardTable>cardLenth*3+cardLenth then
			for k=#mCardTable,1,-1 do
				local v=mCardTable[k]
				if v.CardId %100<firstValue and  v.CardId %100>endValue then
					ZyTable.push_back(returnTable,k)	
					break					
				end				
			end
		else
			returnTable={}
		end	
	elseif type==2 then
		local table={}
		local index=1
		for k, v in pairs(doubleTable) do	
			if k<firstValue and k>endValue then
				index=index+1
				ZyTable.push_back(table,v)		
			end
			if index>=cardLenth then
				break
			end
		end	
		
		
		if #table>0 then
			for k, v in pairs(table) do
				for m, n in pairs(v) do
					ZyTable.push_back(returnTable,n)	
				end
			end
		else
			returnTable={}
		end
	end	
	return returnTable
end;

--4 带2
function getSiDaiEr(cardTable,cardValue)
    if bombTable then
	local value=cardValue
	local length=#cardTable
	local type=0
	local outTable={}
	for k, v in pairs(bombTable) do
		if k>value then
			outTable=v
			break
		end
	end	
	local value=0
	local count=0
	for k, v in pairs(cardTable) do
		if v.CardId %100~=value then
			count=count+1
			value=v.CardId %100
		end
	end
	
	--
	local index=1
	local maxIndex=1
	local type=1
	if length>6 then
		maxIndex=2
		type=2
	else
		if count<3 then
			type=2
		end
	end
	
	if type==2 then
		for k, v in pairs(doubleTable) do
			if index<=maxIndex then
				local id=1
				for m, n in pairs(v) do
					if id<=2 then
					ZyTable.push_back(outTable,n)
					id=id+1
					end
				end
			else
				break
			end	
		end	
	else
		local index=1
		for k, v in pairs(singleTable) do
			if index<=2 then
					ZyTable.push_back(outTable,v[1])		
					index=index+1
			end
		end		
		if #outTable<6 then
			local value=0
			for k, v in pairs(mCardTable) do
				if v.CardId %100~=value and v.CardId %100~=cardValue then
					ZyTable.push_back(outTable,k)		
					value=v.CardId %100
					if #outTable>=6 then
					 break
					end
				end
			end
		end	
	end
	
	
	if #outTable<(4+maxIndex*2) then
		outTable={}
	end	
	return outTable
	end
end;
--]]
--
function getSameCard()
	local table={}
	for k=#mCardTable,1,-1 do
		local v=mCardTable[k]
		local index=v.CardId %100
		if not table[index] then
			table[index]={}
		end
		ZyTable.push_back(table[index],k)	
	end	
	for k, v in pairs(table) do
		if #v>3 then		
			if not bombTable then
				bombTable={}
			end
			bombTable[k]=v
			planeTable[k]=v
			
		elseif  #v>2 then
			planeTable[k]=v
			planeTable1[k]=v
			doubleTable[k]=v
		elseif  #v>1 then	
			doubleTable[k]=v
			doubleTable1[k]=v
		else 
			singleTable[k]=v
		end
	end	
end;





