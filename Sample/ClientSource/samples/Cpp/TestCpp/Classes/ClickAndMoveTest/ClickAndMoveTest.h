#ifndef _CLICK_AScut_MOVE_TEST_H_
#define _CLICK_AScut_MOVE_TEST_H_

#include "../testBasic.h"

class ClickAScutMoveTestScene : public TestScene
{
public:
    virtual void runThisTest();
};

class MainLayer : public CCLayer
{
public:
    MainLayer();
    virtual void ccTouchesended(CCSet *pTouches, CCEvent *pEvent);
};

#endif
