//
//  NDWebView.h
//  91Tribe
//
//  Created by wyt218 on 11-5-12.
//  add backbar img set : by yay 2011-7-30
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>


@interface iosWebView : UIView<UIWebViewDelegate> {
	UIWebView *m_WebView;
	NSString *m_strUrl;
	NSString *m_strNormal;
	NSString *m_strPushDown;
	UIActivityIndicatorView *m_loadingActivity;
}
-(id)initWithUrl:(NSString*)strUrl andScreenFrame:(CGRect)screenFrame andTitle:(NSString*)title andNormalImg:(NSString*)Normalimg andPushImg:(NSString*)Pushimg;
-(void)close;
-(void)switchUrl:(NSString*)strUrl;

+(void)setWebviewCallback:(NSString*)strFun;

//-(id)setBackBarimg:(NSString*)strNormal andPushDownimg:(CNSString*)strPushDown;
@property (nonatomic, retain) UIWebView	*m_WebView;
@property (nonatomic, retain) NSString *m_strUrl;
//@property (nonatomic, retain) NSString *m_strNormal;
//@property (nonatomic, retain) NSString *m_strPushDown;
@end
