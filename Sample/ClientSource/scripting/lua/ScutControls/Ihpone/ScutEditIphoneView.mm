//
//  NdEditIphoneView.m
//  91Tribe
//
//  Created by wyt218 on 11-4-22.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "ScutEditIphoneView.h"
#include "EAGLView.h"
#include "../ScutEdit.h"
#define TextTag 4356
#define KEYBOARD_HEIGHT_MARGIN 5

using namespace ScutCxControl;

@implementation ScutEditIphoneView
- (id)init:(BOOL)isPwStyle  andIsEditView:(BOOL)isEditView andBackColor:(UIColor*)backColor andForeColor:(UIColor*)foreColor
{
	if(self=[super init])
	{
		if (isEditView) {
			UITextView *textView= [[[UITextView alloc]init]autorelease];
			[self addSubview:textView];
			textView.tag = TextTag;
			textView.delegate = self;
			textView.backgroundColor = backColor;
			textView.textColor = foreColor;
		//	textView.m_textColor   = [UIColor whiteColor];
			textView.returnKeyType = UIReturnKeyDefault;			
		}
		else {
			UITextField *textFild= [[[UITextField alloc]init]autorelease];
			
			[self addSubview:textFild];
			textFild.tag = TextTag;
			textFild.delegate = self;
			textFild.backgroundColor = backColor;
			textFild.textColor = foreColor;
			textFild.borderStyle = UITextBorderStyleNone;
			textFild.contentVerticalAlignment = UIControlContentVerticalAlignmentCenter;
			CGColorRef colorRef = [backColor CGColor];
			int numComponents = CGColorGetNumberOfComponents(colorRef);
			if (numComponents == 4)
			{
				const CGFloat* components = CGColorGetComponents(colorRef);
				CGFloat A = components[3];
				if (A < 1)
					textFild.borderStyle = UITextBorderStyleNone;
			}
			//textFild.backgroundColor = [UIColor colorWithWhite:0.0 alpha:0.0];
			if (isPwStyle) {
				textFild.secureTextEntry = YES;
			}
			textFild.returnKeyType = UIReturnKeyDone;
			
		}
		//self.backgroundColor = [UIColor redColor];
		m_nMaxNum = -1;
		m_pOwnerEdit = NULL;
	}
	return self;
}

- (void)dealloc {
	if(m_lastValidStr)
	{
		[m_lastValidStr release];
		m_lastValidStr = nil;
	}	
	[self hiddenTextPanel];
	[self unregisterKeyboradNotification];
    [super dealloc];
}


-(void)setMaxInputNum:(int)nNum
{
	if(nNum<=0)
		return;
	m_nMaxNum = nNum;
}

- (void)setInputNum
{
	m_bInputNumType = YES;
}

- (void)setTextFontSize:(int)nSize
{

	UIView *vi = [self viewWithTag:TextTag];
	if ([vi isKindOfClass:[UITextField class]]) {
		UITextField *textField = (UITextField*)vi;
		[textField setFont:[UIFont systemFontOfSize:nSize]];		 
	}
	else if ([vi isKindOfClass:[UITextView class]]){
		UITextView *textView = (UITextView*)vi;
		//[textView setFont:[UIFont fontWithName:@"Arial" size:32]];	
		[textView setFont:[UIFont systemFontOfSize:nSize]];	
	//	[textView setFont:[UIFont fontWithOfSize:32]];		 
	}


}

- (void)setOwnerEdit:(ScutCxControl::CScutEdit*)pEdit
{
	m_pOwnerEdit = pEdit;
}

-(void)setEnabled:(BOOL)bEnable
{
	UIView *vi = [self viewWithTag:TextTag];
	if ([vi isKindOfClass:[UITextField class]]) {
		UITextField *textField = (UITextField*)vi;
		textField.enabled = bEnable;
	}
	else if ([vi isKindOfClass:[UITextView class]]){
		UITextView *textView = (UITextView*)vi;
		textView.editable = bEnable;
	}
}

-(void)setNdEditFrame:(CGRect)frame
{
	self.frame = frame;
	UIView *vi = [self viewWithTag:TextTag];

	vi.frame=CGRectMake(0, 0, frame.size.width, frame.size.height);
}

- (void)setTextString:(NSString*)str
{
	UIView *vi = [self viewWithTag:TextTag];
	if ([vi isKindOfClass:[UITextField class]]) {
		UITextField *textField = (UITextField*)vi;
		textField.text = str;
	}
	else if ([vi isKindOfClass:[UITextView class]]){
		UITextView *textView = (UITextView*)vi;
		textView.text = str;
	}
}

-(NSString*)getTextString
{	
	UIView *vi = [self viewWithTag:TextTag];
	if ([vi isKindOfClass:[UITextField class]]) {
		UITextField *textField = (UITextField*)vi;
		return textField.text;
	}
	else if ([vi isKindOfClass:[UITextView class]]){
		UITextView *textView = (UITextView*)vi;
		return textView.text;
	}
	return NULL;
}

-(void)hiddenTextPanel
{
	UIView *vi = [self viewWithTag:TextTag];
	if ([vi isKindOfClass:[UITextField class]]) {
		UITextField *textField = (UITextField*)vi;
		[textField resignFirstResponder];
	}
	else if ([vi isKindOfClass:[UITextView class]]){
		UITextView *textView = (UITextView*)vi;
		[textView resignFirstResponder];
	}
}
#pragma mark textView delegate
- (BOOL)textView:(UITextView *)textView shouldChangeTextInRange:(NSRange)range replacementText:(NSString *)text
{
	/*
	if([text length]>=range.length&&[textView.text length]-range.length+[text length]>m_nMaxNum) {
		return NO;
	}
	*/
	if(m_bInputNumType && [text length] > 0)
	{
		const char* pszText = [text UTF8String];
		int count = strlen(pszText);
		for (int i = 0; i < count; i++) {
			if (*pszText < '0' || *pszText > '9')
			{
				return NO;
			}
		}
	}
	
	return YES;
}

#pragma mark textField delegate
- (BOOL)textField:(UITextField *)textField shouldChangeCharactersInRange:(NSRange)range replacementString:(NSString *)string
{
	/*
	if([string length]>=range.length&&[textField.text length]-range.length+[string length]>m_nMaxNum) {
		return NO;
	}
	 */
	if(m_bInputNumType && [string length] > 0)
	{
		const char* pszText = [string	UTF8String];
		int count = strlen(pszText);
		for (int i = 0; i < count; i++) {
			if (*pszText < '0' || *pszText > '9')
			{
				return NO;
			}
		}
	}
	 
	return YES;
}

- (BOOL)textFieldShouldReturn:(UITextField *)textField
{
	[textField resignFirstResponder];
	return YES;
}

- (BOOL)textFieldShouldEndEditing:(UITextField *)textField
{
	if (m_bInputNumType) {
		UIView *vi = [self viewWithTag:TextTag];
		if ([vi isKindOfClass:[UITextField class]]) {
			UITextField *textField = (UITextField*)vi;
			NSString *text = [textField text];
			const char* pszText = [text	UTF8String];
			int count = strlen(pszText);
			if (count > 0) {
				int i = 0;
				for (; i < count; i++) {
					if (pszText[i] < '0' || pszText[i] > '9')
					{
						break;
					}
				}	
				if (i < count) {
					NSString* b = [text substringToIndex:i];
					textField.text = b;
				}
			}
			
		}		
	}

	return YES;
}


// Only override drawRect: if you perform custom drawing.
// An empty implementation adversely affects performance during animation.
- (void)drawRect:(CGRect)rect {
    // Drawing code
//	CGContextRef context = UIGraphicsGetCurrentContext();
//	CGContextSaveGState(context);
//	
//	int nHeight = rect.size.height;
//	int nWidht = rect.size.width;
//	CGContextSetRGBFillColor( context, 1, 1, 1, 1 );
//	CGContextBeginPath(context);
//	CGContextMoveToPoint(context, nWidht/2, 0);
//	
//	CGContextAddArcToPoint(context, nWidht, 0, nWidht, nHeight, 20);
//	CGContextAddArcToPoint(context, nWidht, nHeight, nWidht/2, nHeight, 20);
//	CGContextAddArcToPoint(context, nWidht/2, nHeight, 0, nHeight, 20);
//	CGContextAddArcToPoint(context, 0, nHeight, 0, 0, 20);
//	
//
//	CGContextClosePath(context);
//	CGContextFillPath(context);
//	
//	
//	CGContextRestoreGState(context);
}



/*
- (void)textFieldDidBeginEditing:(UITextField *)textField
{
	EAGLView *eglView = (EAGLView *)[self superview];
	[[eglView class] beginAnimations:nil context:NULL];
	[[eglView class] setAnimationDuration:0.3];
	
	m_originFrame = eglView.frame;

	CGRect viewFrame = m_originFrame;
	CGFloat delta = m_originFrame.size.width - self.frame.origin.y - self.frame.size.height - KEYBOARD_HEIGHT > 0 ? 0 : KEYBOARD_HEIGHT - m_originFrame.size.width + self.frame.origin.y + self.frame.size.height;
	switch ([[UIApplication sharedApplication] statusBarOrientation])
    {
		case UIInterfaceOrientationPortrait:
			viewFrame.origin.y -= delta;
            break;
            
        case UIInterfaceOrientationPortraitUpsideDown:
			viewFrame.origin.y += delta;
            break;
            
        case UIInterfaceOrientationLandscapeLeft:
			viewFrame.origin.x -= delta;
            break;
            
        case UIInterfaceOrientationLandscapeRight:
			viewFrame.origin.x += delta;
            break;
            
        default:
            break;
	}
	eglView.frame = viewFrame;
	
	[[eglView class] commitAnimations];
	
	
	[eglView setNeedsDisplay];
}

-(void)textFieldDidEndEditing:(UITextField *)textField
{
	EAGLView *eglView = (EAGLView *)[self superview];
	[[eglView class] beginAnimations:nil context:NULL];
	[[eglView class] setAnimationDuration:0.3];
	
	
	eglView.frame = m_originFrame;
	
	[[eglView class] commitAnimations];
	
	[eglView setNeedsDisplay];
}
*/
- (void)textFieldDidBeginEditing:(UITextField *)textField
{
	if ([self viewWithTag:TextTag] == textField) {
		m_bFirstResponder = YES;
		m_bKeyboardShown = NO;
        m_nKeyBoardHeight = 0;
	}
}

- (BOOL)textViewShouldBeginEditing:(UITextView *)textView
{
	if ([self viewWithTag:TextTag] == textView) 
	{
		m_bFirstResponder = YES;
		m_bKeyboardShown = NO;
        m_nKeyBoardHeight = 0;
	}
	
	return YES;
}

- (void)keyboardWillShow:(NSNotification *)aNotification
{
	if (m_bFirstResponder == NO) {
		return;
	}
	if (m_bKeyboardShown == YES) {
		return;
	}
	NSDictionary *info = [aNotification userInfo];
	NSValue *aValue = [info objectForKey:UIKeyboardBoundsUserInfoKey];
	CGRect rect;
	[aValue getValue:&rect];
    m_nKeyBoardHeight = rect.size.height;
	
	EAGLView *eglView = (EAGLView *)[self superview];
	[[eglView class] beginAnimations:nil context:NULL];
	[[eglView class] setAnimationDuration:0.3];
	
	CGRect originFrame = eglView.frame;

	CGRect viewFrame = originFrame;
	CGFloat delta = 0.0;
	bool isPortrait = false;
	switch ([[UIApplication sharedApplication] statusBarOrientation])
    {
		case UIInterfaceOrientationPortrait:
			isPortrait = true;
            break;
            
        case UIInterfaceOrientationPortraitUpsideDown:
			isPortrait = true;
            break;
            
        default:
            break;
	}	
	if (!isPortrait)
		delta = originFrame.size.width - self.frame.origin.y - self.frame.size.height - rect.size.height > 0 ? 0 : rect.size.height - originFrame.size.width + self.frame.origin.y + self.frame.size.height;
 	else
		delta = originFrame.size.height - self.frame.origin.y - self.frame.size.height - rect.size.height > 0 ? 0 : rect.size.height - originFrame.size.height + self.frame.origin.y + self.frame.size.height;
	if (delta < 0.001)
	{
		[[eglView class] commitAnimations];
		return;
	}

	m_originFrame = self.frame;
	if (m_originFrame.origin.y <= delta)
	{
		delta = m_originFrame.origin.y - KEYBOARD_HEIGHT_MARGIN * 2;
	}
	NSLog(@"keyboardWillShow vi.frame y %f !!", m_originFrame.origin.y);
	NSLog(@"keyboardWillShow delta %f !!", delta);
	delta += KEYBOARD_HEIGHT_MARGIN;
	switch ([[UIApplication sharedApplication] statusBarOrientation])
    {
		case UIInterfaceOrientationPortrait:
			viewFrame.origin.y -= delta;
            break;
            
        case UIInterfaceOrientationPortraitUpsideDown:
			viewFrame.origin.y += delta;
            break;
            
        case UIInterfaceOrientationLandscapeLeft:
			viewFrame.origin.x -= delta;
            break;
            
        case UIInterfaceOrientationLandscapeRight:
			viewFrame.origin.x += delta;
            break;
            
        default:
            break;
	}
	eglView.frame = viewFrame;
	
	m_bKeyboardShown = YES;
	
	[[eglView class] commitAnimations];
	
	[eglView setNeedsDisplay];
}

- (void)keyboardWillHide:(NSNotification *)aNotification
{
	if (m_bFirstResponder == NO) {
		m_bKeyboardShown = NO;
        m_nKeyBoardHeight = 0;
		return;
	}
	EAGLView *eglView = (EAGLView *)[self superview];
	[[eglView class] beginAnimations:nil context:NULL];
	[[eglView class] setAnimationDuration:0.3];
	
	eglView.frame = eglView.originalRect_;//m_originFrame;

	m_bKeyboardShown = NO;
	m_bFirstResponder = NO;
    m_nKeyBoardHeight = 0;
	
	[[eglView class] commitAnimations];
	
	[eglView setNeedsDisplay];
}

- (void)keyboardDidShow:(NSNotification *)aNotification
{
    /*
    NSDictionary *info = [aNotification userInfo];
    CGSize kbSize = [[info objectForKey:UIKeyboardFrameBeginUserInfoKey] CGRectValue].size;
    NSLog(@"keyboardDidShow y %f !!", kbSize.height);
    NSLog(@"keyboardDidShow x %f !!", kbSize.width);
    CGSize kbSize1 = [[info objectForKey:UIKeyboardBoundsUserInfoKey] CGRectValue].size;
    NSLog(@"keyboardDidShow1 y %f !!", kbSize1.height);
    NSLog(@"keyboardDidShow1 x %f !!", kbSize1.width);
     */
    NSDictionary *info = [aNotification userInfo];
	CGSize kbSize1 = [[info objectForKey:UIKeyboardBoundsUserInfoKey] CGRectValue].size;
	
    if (kbSize1.height != m_nKeyBoardHeight && m_nKeyBoardHeight != 0) {
        float delta = kbSize1.height - m_nKeyBoardHeight;
        EAGLView *eglView = (EAGLView *)[self superview];
        [[eglView class] beginAnimations:nil context:NULL];
        [[eglView class] setAnimationDuration:0.3];
        
        CGRect viewFrame = eglView.frame;
        
        switch ([[UIApplication sharedApplication] statusBarOrientation])
        {
            case UIInterfaceOrientationPortrait:
                viewFrame.origin.y -= delta;
                break;
                
            case UIInterfaceOrientationPortraitUpsideDown:
                viewFrame.origin.y += delta;
                break;
                
            case UIInterfaceOrientationLandscapeLeft:
                viewFrame.origin.x -= delta;
                break;
                
            case UIInterfaceOrientationLandscapeRight:
                viewFrame.origin.x += delta;
                break;
                
            default:
                break;
        }
        m_nKeyBoardHeight = kbSize1.height;
        eglView.frame = viewFrame;
        
        [[eglView class] commitAnimations];
        
        [eglView setNeedsDisplay];
    }
}

- (void)keyboardDidHide:(NSNotification *)aNotification
{
    
}

- (void)textFieldDidChange:(NSNotification *)aNotification
{
	UIView *vi = [self viewWithTag:TextTag];
	if ([vi isKindOfClass:[UITextField class]]) {
		UITextField *textField = (UITextField*)vi;
		NSString *text = [textField text];
		//NSLog(@"text length is %d", [text length]);
		//NSLog(@"text: %@", text);
		if([text length] > m_nMaxNum)
		{
			if (m_nMaxNum == 1)
			{
				textField.text = [text substringFromIndex:m_nMaxNum];
			}
			else if (m_lastValidStr)
			{
				NSString* str = [m_lastValidStr copy];
				textField.text =  str;
				[str release];
			}
			else {
				textField.text = @"";
			}

			
				//textField.text = [text substringToIndex:m_nMaxNum];
			//NSLog(@"textField.text length is %d", [textField.text length]);
			//NSLog(@"textField.text: %@", textField.text);			
		}
		else {
			if(m_lastValidStr != text)
			{
				[m_lastValidStr release];
				
				m_lastValidStr = [text copy];
			}
		}

	}
	else if ([vi isKindOfClass:[UITextView class]]){
		UITextView *textView = (UITextView*)vi;
		NSString *text = [textView text];
		if([text length] > m_nMaxNum)
		{
			if (m_nMaxNum == 1)
			{
				textView.text = [text substringFromIndex:m_nMaxNum];
			}
			else
				textView.text = [text substringToIndex:m_nMaxNum];
		}
	}
	if (m_pOwnerEdit)
	{
		m_pOwnerEdit->OnTextChanged();
	}
}

- (void)textViewDidChange:(UITextView *)textView
{
	NSString *text = [textView text];
	if([text length] > m_nMaxNum)
	{
		if (m_nMaxNum == 1)
		{
			textView.text = [text substringFromIndex:m_nMaxNum];
		}
		else
			textView.text = [text substringToIndex:m_nMaxNum];
	}
}

- (void)registerKeyboradNotification
{
	[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(keyboardWillShow:) name:UIKeyboardWillShowNotification object:nil];
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(keyboardDidShow:) name:UIKeyboardDidShowNotification object:nil];
    //[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(keyboardDidHide:) name:UIKeyboardDidHideNotification object:nil];
	[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(keyboardWillHide:) name:UIKeyboardWillHideNotification object:nil];
	[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(textFieldDidChange:) name:UITextFieldTextDidChangeNotification object:nil];
}

- (void)unregisterKeyboradNotification
{
	[[NSNotificationCenter defaultCenter] removeObserver:self name:UIKeyboardWillShowNotification object:nil];
	[[NSNotificationCenter defaultCenter] removeObserver:self name:UIKeyboardDidShowNotification object:nil];
	[[NSNotificationCenter defaultCenter] removeObserver:self name:UIKeyboardWillHideNotification object:nil];
	[[NSNotificationCenter defaultCenter] removeObserver:self name:UITextFieldTextDidChangeNotification object:nil];
}

@end

