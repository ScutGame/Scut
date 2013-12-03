------------------------------------------------------------------
-- GuideConfigInfo.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 
------------------------------------------------------------------

module("GuideConfigInfo", package.seeall)


--
--
--1001	第一次副本战斗	点击副本，进入副本指引界面，挑战第一个副本	0	NULL	1	1
--1002	商城招募佣兵	进入酒馆界面后，指引其招募第一个佣兵	1001	NULL	2	1
--1003	布置阵型	招募完第一个佣兵后，指引其进入阵型界面，并选择佣兵上阵	1002	NULL	3	1
--1004	通过副本2	指引其进入副本界面，进入第二个副本	1003	NULL	1	2
--1005	通过副本3	指引其进入后山3进入下一个副本，并获得一件特定武器（根据职业送）	1004	NULL	1	3
--1006	装备武器	指引进入佣兵界面给第一个佣兵装备武器。	1005	NULL	4	1
--1007	强化武器	装备上武器后，开启强化功能，并指引其进入强化界面强化武器。	1006	NULL	5	1
--1008	通过副本4	点击副本，进入副本指引界面，挑战第四个副本	1007	NULL	1	4
--1009	通过副本5	点击副本，进入副本指引界面，挑战第五个副本	1008	[{"Reward":3,"Type":7,"ItemID":1223,"Num":1}]	1	5
--1010	佣兵升级	指引其对第一个佣兵进行升级	1009	NULL	6	1
--1011	通过副本6	点击副本，进入副本指引界面，挑战第六个副本	1010	NULL	1	6
--1012	新手引导完成	完成引导	1011	NULL	7	1




function getConfigInfo(taskId, step)
	if GUIDEINFO[taskId] and GUIDEINFO[taskId][step] then
		return GUIDEINFO[taskId][step]
	else
		return false
	end
end


function getJudgeInfo(taskId, step)
	if GuideId[taskId] then
		if GuideId[taskId][step]  then
			return GuideId[taskId][step]
		end
		
	else
		return false
	end
	
end;

GUIDEINFO = {}
--
GUIDEINFO[1001]={
	[1] = {isHaveMan=true, pos_x=pWinSize.width*0, pos_y=pWinSize.height*0.145, strMsg="恶魔克鲁斯的黑暗统治使得亚鲁尼斯所有生灵走向毁灭。现在你需要证明自己拥有拯救这片大陆的力量。"},
	[2] = {isHaveMan=true, pos_x=pWinSize.width*0, pos_y=pWinSize.height*0.145, strMsg="你需要进入梦幻森林1杀死1只猫妖来证明自己的力量。"},
	
	[3] = {isHaveMan=true, pos_x=pWinSize.width*0, pos_y=pWinSize.height*0.145, strMsg="你已经证明了自己的力量，副本挑战可以获得荣誉，金币，晶石和装备，这是提高实力的主要途径。"}
}

GUIDEINFO[1002]={
	[1] = {isHaveMan=true, pos_x=pWinSize.width*0, pos_y=pWinSize.height*0.145, strMsg="一个人的力量终究有限，你需要去酒馆招募更多志同道合的伙伴一起战斗！"},
}

GUIDEINFO[1003]={
	[1] = {isHaveMan=true, pos_x=pWinSize.width*0, pos_y=pWinSize.height*0.145, strMsg="点击进入阵型！"},
	[2] = {isHaveMan=false, pos_x=pWinSize.width*0, pos_y=pWinSize.height*0.145, strMsg="将刚招募的佣兵拖到阵型中"},	
	[3] = {isHaveMan=true, pos_x=pWinSize.width*0, pos_y=pWinSize.height*0.145, strMsg="点击保存阵法。"},	
}

GUIDEINFO[1004]={
	[1] = {isHaveMan=true, pos_x=pWinSize.width*0, pos_y=pWinSize.height*0.145, strMsg="点击进入副本，进入梦幻森林2。"},
--	[2] = {isHaveMan=true, pos_x=pWinSize.width*0, pos_y=pWinSize.height*0.145, strMsg="敌人越来越强大，你需要更快的增强自己的实力！"},	
}

GUIDEINFO[1005]={
	[1] = {isHaveMan=true, pos_x=pWinSize.width*0, pos_y=pWinSize.height*0.145, strMsg="敌人越来越强大，你需要更快的增强自己的实力！"},	
}

GUIDEINFO[1006]={
	[0] = {isHaveMan=true, pos_x=pWinSize.width*0, pos_y=pWinSize.height*0.145, strMsg="宝剑配英雄，回到主界面，去给佣兵装备上刚获得的武器！"},
	[1] = {isHaveMan=true, pos_x=pWinSize.width*0, pos_y=pWinSize.height*0.145, strMsg="这把武器将会使你的实力更强上一分，快点把武器装备上吧！"},
}

GUIDEINFO[1007]={
	[1] = {isHaveMan=true, pos_x=pWinSize.width*0, pos_y=pWinSize.height*0.145, strMsg="装备上武器后，是否感觉自己的实力更加强劲了！一把好的武器需要不断的锻造才会更加锋利，现在你需要强化自己的第一把武器！"},
}

GUIDEINFO[1008]={
	[1] = {isHaveMan=true, pos_x=pWinSize.width*0, pos_y=pWinSize.height*0.145, strMsg="实力又增加了一分，想必现在再遇到敌人能够更加轻松应对！"},
}

GUIDEINFO[1010]={
	[1] = {isHaveMan=true, pos_x=pWinSize.width*0, pos_y=pWinSize.height*0.145, strMsg="佣兵是会长不可缺少的力量，为此必须时刻对其进行升级才能保证佣兵的实力。现在请会长进入佣兵升级界面进行佣兵升级。"},
}



GUIDEINFO[1011]={
	[1] = {isHaveMan=true, pos_x=pWinSize.width*0, pos_y=pWinSize.height*0.145, strMsg="你已经基本掌握了自己的力量，现在是最后考验你的时候，前往神秘洞穴3击败 那里的半兽人！"},
}

GUIDEINFO[1012]={
	[1] = {isHaveMan=true, pos_x=pWinSize.width*0, pos_y=pWinSize.height*0.145, strMsg="恭喜你通过了最终的试炼！"},
}

--1	主界面
--2	副本界面
--3	副本评价界面
--4	酒馆
--5	布阵
--6	佣兵阵营
--7	装备选择界面
--8	装备详情
--9 装备强化界面
--10 佣兵升级界面
--11 佣兵选择界面
--12 经验卡选择界面

GuideId = {}
--1001	第一次副本战斗	点击副本，进入副本指引界面，挑战第一个副本
GuideId[1001] = {
	[1] = { 1 },
	[2] = { 2 },
	[3] = { 3 },
}
--1002	商城招募佣兵	进入酒馆界面后，指引其招募第一个佣兵	
GuideId[1002] = {
	[1] = { 1,3 },
	[2] = { 4 },
	[3] = { 4 },
	[4] = { 4 },
}
--1003	布置阵型	招募完第一个佣兵后，指引其进入阵型界面，并选择佣兵上阵
GuideId[1003] = {
	[1] = { 1,4 },
	[2] = { 5 },
	[3] = { 5 },
	[4] = { 5 },
	[5] = { 5 },	
}
--1004	通过副本2	指引其进入副本界面，进入第二个副本
GuideId[1004] = {
	[1] = { 1,5 },
	[2] = { 2 },
	[3] = { 3 },
}
--1005	通过副本3	指引其进入后山3进入下一个副本，并获得一件特定武器（根据职业送）
GuideId[1005] = {
	[1] = { 3 },
	[2] = { 2 },
	[3] = { 3 },
}
--1006	装备武器	指引进入佣兵界面给第一个佣兵装备武器。
GuideId[1006] = {
	[1] = { 1,3 },--到主界面
	[2] = { 1 },--点击佣兵头像
	[3] = { 6 },--点击装备框
	[4] = { 7 },--选择装备界面 选择装备
	[5] = { 7 },--点击确认
	[6] = { 6 },--结束
}
--1007	强化武器	装备上武器后，开启强化功能，并指引其进入强化界面强化武器。
GuideId[1007] = {
	[1] = { 6 },--点击装备
	[2] = { 8 },--点击强化按钮
	[3] = { 9 },--强化
	[4] = { 9 },--强化结束	
}
--1008	通过副本4	点击副本，进入副本指引界面，挑战第四个副本
GuideId[1008] = {
	[1] = { 1,9 },
	[2] = { 2 },
	[3] = { 3 },
}
--1009	通过副本5	点击副本，进入副本指引界面，挑战第五个副本
GuideId[1009] = {
	[1] = { 3 },
	[2] = { 2 },
	[3] = { 3 },
}
--1010	佣兵升级	指引其对第一个佣兵进行升级
GuideId[1010] = {
	[1] = { 1,3 },
	[2] = { 1 },
	[3] = { 10 },
	[4] = { 11 },
	[5] = { 11 },
	[6] = { 10 },
	[7] = { 12 },
	[8] = { 12 },
	[9] = { 10 },
	
}


--1011	通过副本6	点击副本，进入副本指引界面，挑战第六个副本
GuideId[1011] = {
	[1] = { 1, 10 },
	[2] = { 2 },
	[3] = { 3 },
}
--1012	新手引导完成	完成引导	
GuideId[1012] = {
	[1] = { 1, 3},
}