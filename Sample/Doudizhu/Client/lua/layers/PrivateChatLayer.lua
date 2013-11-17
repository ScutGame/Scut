--
-- PrivateChatLayer.lua
-- Author     : Chensy
-- Version    : 1.0
-- Date       :
-- Description: 聊天接口管理
--

module("PrivateChatLayer", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local _chatMessage = {};
local _allMessage={}
--
---------------公有接口(以下两个函数固定不用改变)--------
--

-- 函数名以小写字母开始，每个函数都应注释

-- 场景入口
function pushScene()
	initResource()
	createScene()
	CCDirector:sharedDirector():pushScene(_scene)
end
-- 退出场景
function popScene()
	releaseResource()
	CCDirector:sharedDirector():popScene()
end

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
end
-- 释放资源
function releaseResource()
end
-- 发即时消息
function sendMessage(Scene,chatId)
	ZyWriter:writeString("ActionId", 9002)
	ZyWriter:writeString("ChatID",chatId)
	ZyExecRequest(Scene,nil, false)
end
-- 获取消息
function getMessage(Scene)
	ZyWriter:writeString("ActionId", 9003)
 	ZyExecRequest(Scene, nil, false)
end
-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionID = ZyReader:getActionID()
	local userData = ZyRequestParam:getParamData(lpExternalData)
	if actionID ==9002 then----9002_聊天发送接口（ID=9002）
        if ZyReader:getResult() == eScutNetSuccess then
            getMessage(pScutScene)
            ChatLayer.setIsClick(true)
            ChatLayer.closeBtnActon();
            sendState=true
        else         
		    ChatLayer.setIsClick(true) 
            ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.2)
        end
     elseif actionID ==9003 then--9003_聊天记录列表接口【完成】（ID=9003）
         --[[  local table=actionLayer._9003Callback(pScutScene, lpExternalData)
           if table and #table.RecordTabel>0 then
             if ChatLayer.getIsClick2() then----用来判断是否是切换Tab的时候到聊天记录那边
                ChatLayer.setIsClick2(false);
                ChatLayer.updateChatTypeLayer(table)
             else
                if sendState then--用来判断现在是否在聊天界面--true未不在聊天界面
                    sendState=nil;
                    for k,v in ipairs(table.RecordTabel) do
                        if MainDesk.getLayer() then
                            local layer=MainDesk.getLayer()
                            ZyToast.show2(layer,v.Content,1,0.3)
                        end
                    end
                else
                    for k,v in ipairs(table.RecordTabel) do 
                        ChatLayer.addMsg(v,k)
                        if MainDesk.getLayer() then
                            local layer=MainDesk.getLayer()
                            ZyToast.show2(layer,v.Content,1,0.3)
                        end
                    end
                end
             end
           end]]
	 end
end
--增加一条消息
function appendLocalMessage(messageType,message)
	if _chatMessage[messageType] == nil then
		_chatMessage[messageType]={};
	end
	if _allMessage[messageType]==nil then--说有的消息
		_allMessage[messageType]={};
	end
	--
	local messageInfo={};
	if message then
        ----
            for i,v in pairs(message) do 
	            messageInfo[i]=v;
	        end
	        messageInfo.index=#_allMessage[messageType]+1;
            table.insert(_allMessage[messageType],messageInfo)
        ---
	end
        if  messageType==1 then--超过100 删掉第一条 类型为综合的时候
            if #_chatMessage[messageType] >=100 then
                removeMessage(messageType,1)
            end
        else---超过30 删掉第一条 类型不是综合的时候
	   end
	table.insert(_chatMessage[messageType],messageInfo)
end
--删除一条消息 --30条数据
function removeMessage(messageType,index)
	if _chatMessage[messageType] then
		table.remove(_chatMessage[messageType],index);
	end
end
--消息维护
function getLocalMessage(messageType)
	if messageType then
		if _chatMessage[messageType] then
			return _chatMessage[messageType];
		else
			return {};
		end
	else
		return _chatMessage;
    end
end

function getChatMessage()
    return _chatMessage;
end

function  clearMessage()
    _chatMessage={};
    _allMessage={}
    personalInfo=nil;
end