------------------------------------------------------------------
--ZyMaze.lua
-- Author     : JMChen
-- Version    : 1.0.0.0
-- Date       : 2011-10-15
-- Description:
------------------------------------------------------------------


module("ZyMaze", package.seeall)

local OpenList={}
local CloseList={}
local titleMap=nil
local OBLIQUE = 14;
local STEP = 10;

function initResource()
	 OpenList={}
	 CloseList={}
end;


--寻路算法
--起始点 终止点 是否忽略转角 障碍层（titleMap）
function  findPath(startPoint,endPoint,isIgnoreCorner,wall)
	initResource()
	titleMap=wall
	ZyTable.push_back(OpenList,startPoint)
	while  #OpenList~=0 do
		--找出F值最小的点
		local tempStart =FindMinPoint(OpenList);
		ZyTable.remove(OpenList, 1)
		ZyTable.push_back(CloseList,tempStart)
           --找出它相邻的点
               local  surroundPoints = SurrroundPoints(tempStart, IsIgnoreCorner);
               if surroundPoints and #surroundPoints>0 then
               	for k, v in pairs(surroundPoints) do
               		if ZyTable.Exists(v.x,v.y,OpenList) then
               		        --计算G值, 如果比原来的大, 就什么都不做, 否则设置它的父节点为当前点,并更新G和F
                        		FoundPoint(tempStart, v);
               		else
               			--如果它们不在开始列表里, 就加入, 并设置父节点,并计算GHF
               			 NotFoundPoint(tempStart ,endPoint,v )
               		end
               	end
               	--
               	 if    ZyTable.GetValue(endPoint.x,endPoint.y,OpenList) then
               	 	local endTable= ZyTable.GetValue(endPoint.x,endPoint.y,OpenList)
               	 	
               	 	return changeEndTabel(endTable)
               	 end        
               end
	end;	
	return ZyTable.GetValue(endPoint.x,endPoint.y,OpenList)
end;

--最后转换table表
function changeEndTabel(table)
	local endTable={}
	if table.ParentPoint then
		while  table.ParentPoint do
			local sonTable={}
			sonTable.x=table.x
			sonTable.y=table.y
			ZyTable.insert(endTable, 1, sonTable)
			table=table.ParentPoint
		end;
	end
	return endTable
end;

--
function FoundPoint(tempStart,  point)
	local G=CalcG(tempStart, point);
	if  (G < (point.G or 0)) then
                point.ParentPoint = tempStart;
                point.G = G;
                point.F=point.G+point.H
	end
end;



---找到最小的
function FindMinPoint(table) 
            points =  ZyTable.orderBy(table,"F")
            return points[1];
end

--获取某个点周围可以到达的点
function  SurrroundPoints(point,IsIgnoreCorner)
	local surroundPoints={}
	for x=point.x-1,point.x+1 do
		for y=point.y-1, point.y+1 do
			if CanReach(point,x,y,IsIgnoreCorner) then
				local table={}
				table.x=x
				table.y=y
				ZyTable.push_back(surroundPoints,table)
			end
		end
	end
	return surroundPoints
end;

--在二维数组对应的位置不为障碍物
function TileGIDAt(x,y)
	if x>0 and y>0 and titleMap:tileGIDAt(PT(x,y)) == 0 then
				return true
	end	
	return false
end;

--是否可到达
function CanReach(start,x,y,IsIgnoreCorner)
	 if not TileGIDAt(x, y)  or ZyTable.Exists(x,y,CloseList) then 
	 	return false
	 else
	 	if (math.abs(x - start.x) + math.abs(y - start.y) == 1) then
	 		return true
	 	else
	 		--如果是斜方向移动, 判断是否 "拌脚
	 		if (TileGIDAt(math.abs(x - 1), y) and TileGIDAt(x, math.abs(y - 1))) then
                        return true;
                    else
                        return IsIgnoreCorner;
                     end    
	 	end 	
	 end	  
end;


function NotFoundPoint(tempStart ,endPoint,point )
	point.ParentPoint=tempStart
 	point.G = CalcG(tempStart, point);
       point.H = CalcH(endPoint, point);
	point.F=point.G+point.H
	ZyTable.push_back(OpenList,point)
end;


----
function CalcG(start,point)
            local  G = (math.abs(point.x - start.x) + math.abs(point.y - start.y)) == 2 and STEP or OBLIQUE;
            local  parentG = point.ParentPoint ~= nil and  point.ParentPoint.G or 0;
            return G + parentG;
end;


function CalcH(endPoint,point)
	local step = math.abs(point.x - endPoint.x) + math.abs(point.y - endPoint.y);
	return step * STEP;
end;



