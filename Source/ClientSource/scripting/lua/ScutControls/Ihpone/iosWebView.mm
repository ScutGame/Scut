//
//  NDWebView.m
//  91Tribe
//
//  Created by wyt218 on 11-5-12.
//  modify: by yay 
//  1.  set backbar img
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "iosWebView.h"
#import "CCLuaEngine.h"
#import "script_support/CCScriptSupport.h"
#import "ScutUtils.h"

@implementation iosWebView

@synthesize m_WebView,m_strUrl;

static NSString *s_strWebviewCallback = nil;

/*
-(id)setBackBarimg:(NSString*)strNormal andPushDownimg:(CNSString*)strPushDown
{
	m_strNormal = [NSString stringWithUTF8String:strNormal.c_str()];
	strPushDown = [NSString stringWithUTF8String:strPushDown.c_str()];
}
*/

-(id)initWithUrl:(NSString*)strUrl andScreenFrame:(CGRect)screenFrame andTitle:(NSString*)title andNormalimg:(NSString*)NormalImg andPushimg:(NSString*)PushImg
{
	if (self=[super initWithFrame:screenFrame]) {
		bool bAddTitleBar = true;
		if ([title length] == 0)
		{
			bAddTitleBar = false;
		}
		int nImageHeight = 0;
		UIImage *imageNormal = NULL;
		UIImage *imageSel = NULL;
		if (bAddTitleBar)
		{
			imageNormal = [UIImage imageNamed:NormalImg];
			imageSel = [UIImage imageNamed:PushImg];
			float imageSpaceX = 80;
			float imageSpaceY = 80;
			if (imageNormal) {
				imageSpaceX=2*(imageNormal.size.width+5);
				imageSpaceY=2*(imageNormal.size.height+5);
			}
			nImageHeight = imageNormal.size.height;
		}
		CGRect rcWeb = CGRectMake(0, 0 + nImageHeight, screenFrame.size.width , screenFrame.size.height-nImageHeight);
		self.m_WebView = [[[UIWebView alloc] initWithFrame:rcWeb] autorelease];
		self.m_strUrl = strUrl;
		self.m_WebView.delegate = self;
		//self.m_WebView.center = CGPointMake(screenFrame.size.width/2,  screenFrame.size.height/2+nImageHeight/2);
		[self addSubview:self.m_WebView];
		self.backgroundColor = [UIColor colorWithRed:86/255.0 green:49/255.0 blue:10/255.0 alpha:1];//[UIColor colorWithRed:0 green:0 blue:0 alpha:0.6];
		if (bAddTitleBar)
		{
			UIButton *btn=[UIButton buttonWithType:UIButtonTypeCustom];
			[self addSubview:btn];
			
			[btn addTarget:self action:@selector(closeCurView) forControlEvents:UIControlEventTouchUpInside];
			
			UILabel*lbtitel = [[UILabel alloc] initWithFrame:CGRectMake(0, 0, screenFrame.size.width, nImageHeight)];
			[self addSubview:lbtitel];
			lbtitel.text=title;
			lbtitel.textColor = [UIColor whiteColor];
			lbtitel.backgroundColor=[UIColor clearColor];
			lbtitel.textAlignment = UITextAlignmentCenter;
			CGPoint btnCenter;
			if (imageNormal) {
				float fbtnX = screenFrame.size.width -imageNormal.size.width/2-3;
				float fbtnY = (imageNormal.size.height/2 + 3);
				
				btnCenter=CGPointMake(fbtnX, fbtnY);
				btn.frame = CGRectMake(0, 0, imageNormal.size.width, imageNormal.size.height);
				[btn setBackgroundImage:imageNormal forState:UIControlStateNormal];
			}
			if (imageSel) {
				[btn setBackgroundImage:imageSel forState:UIControlStateSelected];
			}
			btn.center = btnCenter;
		}
		[self.m_WebView loadRequest:[NSURLRequest requestWithURL:[NSURL URLWithString:m_strUrl] cachePolicy:NSURLRequestReloadIgnoringLocalCacheData timeoutInterval:60]];
		
		m_loadingActivity = [[UIActivityIndicatorView alloc]
							 initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleGray];
		[self addSubview:m_loadingActivity];
		[m_loadingActivity sizeToFit];
        m_loadingActivity.autoresizingMask = (UIViewAutoresizingFlexibleLeftMargin |
                                        UIViewAutoresizingFlexibleRightMargin |
                                        UIViewAutoresizingFlexibleTopMargin |
                                        UIViewAutoresizingFlexibleBottomMargin);
		

		m_loadingActivity.frame = CGRectMake(0, 0, 50, 50);
		m_loadingActivity.center = m_WebView.center ;
		[m_loadingActivity startAnimating];
	}
	return self;
}
-(void)dealloc
{
	self.m_WebView.delegate = nil;
	self.m_WebView = nil;
	self.m_strUrl = nil;
	[m_loadingActivity release];
	[super dealloc];
}

-(void)close
{
	[self removeFromSuperview];
}

-(void)switchUrl:(NSString *)strUrl
{
	self.m_strUrl = strUrl;
	[self.m_WebView loadRequest:[NSURLRequest requestWithURL:[NSURL URLWithString:m_strUrl] cachePolicy:NSURLRequestReloadIgnoringLocalCacheData timeoutInterval:60]];
}

-(void)closeCurView
{
	[self removeFromSuperview];
}
- (void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
{
}


+(void)setWebviewCallback:(NSString*)strFun
{
    s_strWebviewCallback = strFun;
}

#pragma mark -
#pragma mark UIWebViewDelegate
- (BOOL)webView:(UIWebView *)webView shouldStartLoadWithRequest:(NSURLRequest *)request navigationType:(UIWebViewNavigationType)navigationType
{
    NSString *url = [request.URL.absoluteURL absoluteString];
    
    if ([url rangeOfString:@"NDGAction:DownloadApp(" options:NSCaseInsensitiveSearch].length > 0) {
        NSString *destStr = [url substringFromIndex:(@"NDGAction:DownloadApp(".length + 1)];
        destStr = [destStr substringToIndex:(destStr.length - 2)];
                                             
        //\u5224\u65ad\u7f51\u7edc\u7c7b\u578b
        
        //cocos2d::CCLuaScriptModule::sharedLuaScriptModule()->executeListloader(destStr.lossyCString, destStr.lossyCString);
        
        //if (s_strWebviewCallback != nil && s_strWebviewCallback.length > 0) {
            //cocos2d::CCLuaEngine* pEngine = (cocos2d::CCLuaEngine*)cocos2d::CCScriptEngineManager::sharedScriptEngineManager()->getScriptEngine();
            //pEngine->executeWebviewEvent([s_strWebviewCallback cStringUsingEncoding:NSASCIIStringEncoding], 0, [destStr cStringUsingEncoding:NSASCIIStringEncoding]);
        //}
        
        return ![ [ UIApplication sharedApplication ] openURL: [NSURL URLWithString:destStr] ]; 
    }
    else if([url rangeOfString:@"NDGAction:LaunchApp(" options:NSCaseInsensitiveSearch].length > 0) {
            NSString *destStr = [url substringFromIndex:(@"NDGAction:LaunchApp(".length + 1)];
            destStr = [destStr substringToIndex:(destStr.length - 2)];
        //cocos2d::CCLuaScriptModule::sharedLuaScriptModule()->executeListloader(destStr.lossyCString, destStr.lossyCString);
        
        //if (s_strWebviewCallback != nil && s_strWebviewCallback.length > 0) {
            //cocos2d::CCLuaEngine* pEngine = (cocos2d::CCLuaEngine*)cocos2d::CCScriptEngineManager::sharedScriptEngineManager()->getScriptEngine();
            //pEngine->executeWebviewEvent([s_strWebviewCallback cStringUsingEncoding:NSASCIIStringEncoding], 1, [destStr cStringUsingEncoding:NSASCIIStringEncoding]);
        //}
        
        //\u542f\u52a8app
        ScutUtility::ScutUtils::launchApp([destStr UTF8String]);
    }
    else if([url rangeOfString:@"NDGAction:JumpTo(" options:NSCaseInsensitiveSearch].length > 0) {
        NSString *destStr = [url substringFromIndex:(@"NDGAction:JumpTo(".length + 1)];
        destStr = [destStr substringToIndex:(destStr.length - 2)];
        
        return ![ [ UIApplication sharedApplication ] openURL: [NSURL URLWithString:destStr] ];
    }
    
	return YES;
}

- (void)webViewDidStartLoad:(UIWebView *)webView
{

	if (![m_loadingActivity isAnimating]) {
		[m_loadingActivity startAnimating];
	}
	
}

- (void)webViewDidFinishLoad:(UIWebView *)webView
{
	[m_loadingActivity stopAnimating];
}

- (void)webView:(UIWebView *)webView didFailLoadWithError:(NSError *)error
{
//	NSString* errorString = [NSString stringWithFormat:
//							 @"<html><center><font size=+5 color='red'>An error occurred:<br>%@</font></center></html>",
//							 error.localizedDescription];
//	[self.m_WebView loadHTMLString:errorString baseURL:nil];
}

@end
