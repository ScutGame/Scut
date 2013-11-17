------------------------------------------------------------------
-- RoomConfig.lua
-- Author     : chenjp

-- Version    : 1.0
-- Date       :
-- Description: 
------------------------------------------------------------------

module("RoomConfig", package.seeall)

function getRoomInfo()
	return roomInfo
end;

roomInfo={}

roomInfo[1]={
content="<label>本房起始倍数1倍，基本单位400金豆，1000金豆准入，每局游戏消耗300金豆，本房同时输赢积分。</label>"
}
roomInfo[2]={
content="<label>本房起始倍数2倍，基本单位400金豆，6000金豆准入，每局游戏消耗400金豆，本房同时输赢积分。</label>"
}
roomInfo[3]={
content="<label>本房起始倍数4倍，基本单位600金豆，50000金豆准入，每局游戏消耗1200金豆，本房同时输赢积分。</label>"
}
roomInfo[4]={
content="<label>本房起始倍数10倍，基本单位600金豆，20000金豆准入，每局游戏消耗5000金豆，本房同时输赢积分。</label>"
}
