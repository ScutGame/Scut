#pragma once
#ifndef __ND_FRAMEMANAGER_H__
#define __ND_FRAMEMANAGER_H__

#include "CCObject.h"

using namespace cocos2d;

class CFrameManager: public CCObject
{
public:
	CFrameManager(void);
	virtual ~CFrameManager(void);
public:
	static CFrameManager* Instance();
protected:
	//将定时调用此函数处理分发网络数据
	virtual void update(float dt);
};

#endif
