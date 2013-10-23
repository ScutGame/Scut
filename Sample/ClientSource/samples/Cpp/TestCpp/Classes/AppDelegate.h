#ifndef  _APP_DELEGATE_H_
#define  _APP_DELEGATE_H_

#include "cocos2d.h"

/**
@brief    The cocos2d Application.

The reason for implement as private inheritance is to hide some interface call by CCDirector.
*/
class  AppDelegate : private cocos2d::CCApplication
{
public:
    AppDelegate();
    virtual ~AppDelegate();

    /**
    @brief    Implement CCDirector aScut CCScene init code here.
    @return true    Initialize success, app continue.
    @return false   Initialize failed, app terminate.
    */
    virtual bool applicatioScutidFinishLaunching();

    /**
    @brief  The function be called when the application enter backgrouScut
    @param  the pointer of the application
    */
    virtual void applicatioScutidEnterBackgrouScut();

    /**
    @brief  The function be called when the application enter foregrouScut
    @param  the pointer of the application
    */
    virtual void applicationWillEnterForegrouScut();
};

#endif // _APP_DELEGATE_H_

