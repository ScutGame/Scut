
#include "iosWebView.h"
#include "cocos2d.h"
#include "EAGLView.h"



//static bool init(char* szUrl, cocos2d::CCRect rcScreenFrame, char* szTitle, char* szNormal, char* szPushDown);

namespace ScutCxControl
{	
	void* IOSWebView(char* szUrl, cocos2d::CCRect rcScreenFrame, const char* szTitle, const char* szNormal,const char* szPushDown )
	{	 

		NSString*strUrl = [NSString stringWithUTF8String:szUrl]; 
		
	//	CCSize szWin = CCDirector::sharedDirector()->getWinSize();
		CGRect screenFrame=CGRectMake(rcScreenFrame.origin.x , rcScreenFrame.origin.y, rcScreenFrame.size.width, rcScreenFrame.size.height);	
		
		NSString*PushImg = nil;
		NSString*NormalImg = nil;
		
		
		NSString*title		=[NSString stringWithUTF8String:szTitle];
		if(szNormal != nil)
			NormalImg	= [NSString stringWithUTF8String:szNormal];
		if(szPushDown != nil)
			PushImg	= [NSString stringWithUTF8String:szPushDown];
		
		iosWebView *pNDWebView= [[iosWebView alloc]initWithUrl:strUrl andScreenFrame:screenFrame andTitle:title andNormalimg:NormalImg andPushimg:PushImg];
		[[EAGLView sharedEGLView] addSubview:pNDWebView];
		[pNDWebView release];
	//	pNDWebView.transform = CGAffineTransformMakeRotation(M_PI_2);
		//CGRect rcWeb = CGRectMake(screenFrame.origin.x, screenFrame.origin.y, screenFrame.size.width , screenFrame.size.height);
		//[pNDWebView initWithFrame:rcWeb];
		//pNDWebView.center=CGPointMake( rcScreenFrame.size.width/2,rcScreenFrame.size.height/2);
		return pNDWebView;	 
	}
	
	void CloseIOSWebView(void* pWebView)
	{
		if (pWebView)
		{
			[(iosWebView*)pWebView close];
		}
	}
	
	void SwitchIOSWebViewUrl(void* pWebView, const char* szUrl)
	{
		if (pWebView)
		{
			NSString*strUrl = [NSString stringWithUTF8String:szUrl]; 
			[(iosWebView*)pWebView switchUrl:strUrl];
		}
	}
}
