//
//  ScutEditIphoneView.h
//  
//
//  Created by wyt218 on 11-4-22.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

namespace ScutCxControl
{
	class CScutEdit;
}

@interface ScutEditIphoneView : UIView<UITextFieldDelegate,UITextViewDelegate> {
	int     m_nMaxNum;
    int     m_nKeyBoardHeight;
	BOOL    m_bKeyboardShown;
	BOOL    m_bFirstResponder;
	BOOL    m_bInputNumType;
	CGRect  m_originFrame;
	NSString* m_lastValidStr;
	ScutCxControl::CScutEdit* m_pOwnerEdit;
}

-(NSString*)getTextString;
- (id)init:(BOOL)isPwStyle  andIsEditView:(BOOL)isEditView andBackColor:(UIColor*)backColor andForeColor:(UIColor*)foreColor;
- (BOOL)textFieldShouldReturn:(UITextField *)textField;
- (BOOL)textFieldShouldEndEditing:(UITextField *)textField;
- (void)setNdEditFrame:(CGRect)frame;
- (void)setMaxInputNum:(int)nNum;
- (void)setInputNum;
- (void)setEnabled:(BOOL)bEnable;
- (void)setTextString:(NSString*)str;
- (void)hiddenTextPanel;
- (void)setTextFontSize:(int)nSize;
- (void)setOwnerEdit:(ScutCxControl::CScutEdit*)pEdit;

- (void)keyboardWillShow:(NSNotification *)aNotification;
- (void)keyboardWillHide:(NSNotification *)aNotification;
- (void)textFieldDidChange:(NSNotification *)aNotification;
- (void)registerKeyboradNotification;
- (void)unregisterKeyboradNotification;
@end
