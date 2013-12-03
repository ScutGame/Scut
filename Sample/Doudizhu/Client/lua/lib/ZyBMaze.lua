------------------------------------------------------------------
--ZyMaze.lua
-- Author     : JMChen
-- Version    : 1.0.0.0
-- Date       : 2011-10-15
-- Description:
------------------------------------------------------------------



 ZyBMaze = {
	startPoint = nil,
	endPoint = nil,
	roadTable={},
	isIgnoreCorner=nil,
	finalRoad=nil,
 }
 
-- 0 没有方向  1 右 2 左 3 上 4 下

 -- 创建实例
function ZyBMaze:new(startPoint,endPoint,titleMap)
	local instance = {}
	setmetatable(instance, self)
	self.__index = self
	if endPoint.x== startPoint.x and endPoint.y== startPoint.y  then
		return 
	end
	startPoint={x=startPoint.x,y=startPoint.y}
	endPoint={x=endPoint.x,y=endPoint.y}	
	instance.finalRoad=nil
	instance.roadTable={}
	instance.startPoint = startPoint
	instance.endPoint = endPoint
	instance.titleMap = titleMap

	return instance
end


--初始化一条路
function ZyBMaze:findLoadBegin()
	local table={}
	table[1]=self.startPoint
	self:newLoadPath(table,nil,nil,self.startPoint)
	self:addAllRoadTile()
	--
	return self.finalRoad
end;


--新建一条寻路线 
function ZyBMaze:newLoadPath(table,dir,stickDir,point)
	local newTable={}
	newTable.roadTable=table
	--寻路类型
	newTable.dir=dir or 0
	newTable.stickDir=stickDir or 0
	self.roadTable[#self.roadTable+1]=newTable
end



function ZyBMaze:addAllRoadTile()
	while self.endPoint  do
		for k, v in pairs (self.roadTable) do
		    if  self.endPoint  then
               		 local point=v.roadTable[#v.roadTable]
              		 self:findNexTitle(v,point,k)	
		    end
		end
	end;
end;


--寻找下一个点
function ZyBMaze:findNexTitle(table,tempStart,k)
	if self.endPoint then
		--是否已经到了终点
		if tempStart.x==self.endPoint.x and tempStart.y==self.endPoint.y then
			self.endPoint=nil
			self.finalRoad=table
			return true
		else		
			--没有指定方向的表
			if table.dir==0 then
				local  surroundPoints =self:SurrroundPoints(tempStart);
				--循环周围的点找出最小的点
				local minPoint=nil
				for k, v in pairs(surroundPoints) do
					if not minPoint or v.F<minPoint.F then
						minPoint=v
					end
				end
				--这个点可以走
				if self:TileGIDAt(minPoint.x,minPoint.y) then
					ZyTable.push_back(table,minPoint)
				else	
					--遇到障碍 四个方向判断
					
	
	
				--这个点遇到障碍 重新找两个点
					 local surroundPoints=self:CanSurrroundPoints(tempStart,table)
					 surroundPoints=ZyTable.orderBy(surroundPoints,"F")
					 --新建两条路 把原来的路删除
					 for k, v in pairs(surroundPoints) do
					 	if k<=2 then
					 		local newTable=ZyTable.th_table_dup(table)
							newTable[#newTable+1]=v
					 		self:newLoadPath(newTable,v)	
					 	end
					 end 
					ZyTable.remove(self.roadTable, k)	 
					
					
					
					
					
				end			
			else
				
			
			end
		end
	end
end;







--计算点
function NotFoundPoint(tempStart ,endPoint,point )
	point.ParentPoint=tempStart
 	point.G = CalcG(tempStart, point);
       point.H = CalcH(endPoint, point);
	point.F=point.G+point.H
	return point
end;



----
function CalcG(start,point)
            local  G = (math.abs(point.x - start.x) + math.abs(point.y - start.y)) == 1 and 10 or 14;
            local  parentG = point.ParentPoint ~= nil and  point.ParentPoint.G or 0;
            return G + parentG;
end;


function CalcH(endPoint,point)
	local step = math.abs(point.x - endPoint.x) + math.abs(point.y - endPoint.y);
	return step * 10;
end;


--寻找最近的点
function ZyBMaze:SurrroundPoints(tempStart)
	if self.endPoint then
		local surroundPoints={}	
		--先判断x轴
		if self.endPoint.x>tempStart.x then
			for y=tempStart.y-1 , tempStart.y+1 do
				local table={}
				table.x=tempStart.x+1
				table.y=y
				table=NotFoundPoint(tempStart ,self.endPoint,table )
				
				ZyTable.push_back(surroundPoints,table)
			end
		elseif self.endPoint.x<tempStart.x then
			for y=tempStart.y-1 , tempStart.y+1 do
				local table={}
				table.x=tempStart.x-1
				table.y=y
				table=NotFoundPoint(tempStart ,self.endPoint,table )
				ZyTable.push_back(surroundPoints,table)
			end
		else
			--判断Y轴
			if self.endPoint.y>tempStart.y then
				local table={}
				table.y=tempStart.y+1
				table.x=tempStart.x
				table=NotFoundPoint(tempStart ,self.endPoint,table )
				
				ZyTable.push_back(surroundPoints,table)
				
			elseif self.endPoint.y<tempStart.y then
				local table={}
				table.y=tempStart.y-1
				table.x=tempStart.x
				table=NotFoundPoint(tempStart ,self.endPoint,table )
				
				ZyTable.push_back(surroundPoints,table)
			end			
		end
		return surroundPoints
	end
end;

--在二维数组对应的位置不为障碍物
function ZyBMaze:TileGIDAt(x,y)
	if x>=0 and y>=0 and y<self.titleMap:getLayerSize().height  and x<self.titleMap:getLayerSize().width and 
        self.titleMap:tileGIDAt(PT(x,y)) == 0 then
				return true
	end	
	return false
end;

--是否存在于原来的路里面
function isExit(table,x,y)
    for k=#table,1 ,-1 do
        if table[k].x==x and table[k].y==y then
            return false
        end
    end
    return true
end

--获取某个点周围可以到达的点
function  ZyBMaze:CanSurrroundPoints(point,roadTable)
	local surroundPoints={}
	for x=point.x-1,point.x+1 do
		for y=point.y-1, point.y+1 do
			if self:CanReach(point,x,y)  and isExit(roadTable,x,y) then
				local table={}
				table.x=x
				table.y=y
				table=NotFoundPoint(point ,self.endPoint,table )
				ZyTable.push_back(surroundPoints,table)
			end
		end
	end
	return surroundPoints
end;

--是否可到达
function ZyBMaze:CanReach(start,x,y)
	 if not self:TileGIDAt(x, y) or (start.x==x and start.y==y)  then 
	 	return false
	 else
	 	if (math.abs(x - start.x) + math.abs(y - start.y) == 1) then
	 		return true
	 	else
	 		--如果是斜方向移动, 判断是否 "拌脚
	 		if (self:TileGIDAt(math.abs(x - 1), y) and self:TileGIDAt(x, math.abs(y - 1))) then
                        return true;
                    else
                        return false;
                     end    
	 	end 	
	 end	  
end;

