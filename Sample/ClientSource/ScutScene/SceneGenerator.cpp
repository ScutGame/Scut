#include "SceneGenerator.h"
#include "NDScene.h"
#include "NdEdit.h"
#include "NdLayer.h"
#include "NdMenu.h"
#include "NdCxList.h"
#include "NdCxListItem.h"
#include "NdButton.h"
#include "FileHelper.h"
#include "Trace.h"

USING_NS_CC;
using namespace NdDataLogic;

namespace NdCxControl
{
	CSceneGenerator::CSceneGenerator(void)
	{
	}

	CSceneGenerator::~CSceneGenerator(void)
	{
	}

	CSceneGenerator* CSceneGenerator::Instance()
	{
		static CSceneGenerator g_Instance;
		return &g_Instance;
	}

	NdScene* CSceneGenerator::AcquireScene( const char* lpszSceneName )
	{
		NdScene* pRet = NdScene::node();
		//尝试从描述文件中加载
		LoadScene(pRet, lpszSceneName);
		return pRet;
	}

	bool CSceneGenerator::LoadScene( NdScene* pDest, const char* lpszSceneName )
	{
		//TODO:加载描述文件
		TiXmlDocument xmlDoc;
		if (!xmlDoc.LoadFile(lpszSceneName))
		{
			return false;
		}

		NdTiXmlElement* pRootElement = xmlDoc.RootElement();
		if (pRootElement && strcmp(pRootElement->Value(), "CC2IDE.Scene") == 0)
		{
			return LoadLayer(pRootElement, (CCNode*)pDest);
		}

		return false;
	}

	bool CSceneGenerator::LoadLayer(NdTiXmlElement* markupXml, CCNode* pParentNode)
	{
		return LoadChildNode(markupXml, pParentNode);
	}

	bool CSceneGenerator::LoadChildNode(NdTiXmlElement* markupXml, CCNode* pParentNode)
	{
		NdTiXmlElement* pChild = markupXml->FirstChildElement();
		while (pChild)
		{
			const char* lpszClassName = pChild->Value();
			if (!lpszClassName || strlen(lpszClassName) == 0)
			{
				continue;
			}

			CCNode*	pNode = (CCNode*)CreateNodeAddToParent(pChild, lpszClassName, pParentNode);
			if (pNode)
			{
				LoadChildNode(pChild, (CCNode*)pNode);
			}	

			DoActionAfterAddToParent(pChild, lpszClassName, pNode);				

			pChild = pChild->NextSiblingElement();
		}

		return true;
	}

	CCNode* CSceneGenerator::CreateNodeAddToParent( NdTiXmlElement* markupXml, const char* pszType, CCNode* pParentNode )
	{
		if (!pszType)
		{
			return NULL;
		}

		bool bMenuItem = false;
		CCNode* pNode = NULL;
		if (strcmp(pszType, "CC2Layer") == 0)
		{
			pNode = (CCLayer*)CreateLayerByProperty(markupXml);
		}
		else if (strcmp(pszType, "CC2NdLayer") == 0)
		{
			pNode = (CCLayer*)CreateNdLayerByProperty(markupXml);
		}
		else if (strcmp(pszType, "CC2NdCxList") == 0)
		{
			pNode = (CCNode*)CreateNdCxListByProperty(markupXml);
		}
		else if (strcmp(pszType, "CC2NdCxListItem") == 0)
		{
			bool bScrollToView = true;
			NdTiXmlElement* pParentElem = (NdTiXmlElement*)markupXml->Parent();
			if (pParentElem)
			{
				bScrollToView = strcmp(pParentElem->Attribute("ScrollToView"), "True") == 0;
			}
			pNode = (CCNode*)CreateNdCxListItemByProperty(markupXml);
			if (pParentNode && pNode)
			{
				int nTag = atoi(markupXml->Attribute("Tag"));
				int nZorder = atoi(markupXml->Attribute("ZOrder"));
				//重置Tag
				std::string Name = markupXml->Attribute("Name");
				if (!Name.empty())
				{
					nTag = HashString(Name.c_str());
				}
				((NdCxList*)pParentNode)->addChild((NdCxListItem*)pNode, bScrollToView);
			}

			return pNode;
		}
		else if (strcmp(pszType, "CC2Menu") == 0)
		{
			pNode = (CCMenu*)CreateMenuByProperty(markupXml);
		}
		else if (strcmp(pszType, "CC2NdMenu") == 0)
		{
			pNode = (CCMenu*)CreateNdMenuByProperty(markupXml);
		}
		else if (strcmp(pszType, "CC2MenuItem") == 0)
		{
			pNode = (CCMenuItem*)CreateMenuItemByProperty(markupXml);
		}
		else if (strcmp(pszType, "CC2MenuItemLabel") == 0)
		{
			pNode = (CCMenuItem*)CreateMenuItemLabelByProperty(markupXml);
		}
		else if (strcmp(pszType, "CC2MenuItemSprite") == 0)
		{
			pNode = (CCMenuItem*)CreateMenuItemSpriteByProperty(markupXml);
		}
		else if (strcmp(pszType, "CC2Sprite") == 0)
		{
			pNode = (CCSprite*)CreateSpriteByProperty(markupXml);
		}
		else if (strcmp(pszType, "CC2Node") == 0)
		{
			pNode = CreateNodeByProperty(markupXml, pNode);
		}
		else if (strcmp(pszType, "CC2LabelTTF") == 0)
		{
			pNode = (CCLabelTTF*)CreateLabelTTFByProperty(markupXml);
		}
		else if (strcmp(pszType, "CC2NdEdit") == 0)
		{
			CreateNdEditor(markupXml);
			pNode = NULL;
		}
		else if (strcmp(pszType, "CC2NdButton") == 0)
		{
			pNode = CreateNdButtonByProperty(markupXml);
		}

		if (pParentNode && pNode)
		{
			int nTag = atoi(markupXml->Attribute("Tag"));
			int nZorder = atoi(markupXml->Attribute("ZOrder"));
			//重置Tag
			std::string Name = markupXml->Attribute("Name");
			if (!Name.empty())
			{
				nTag = HashString(Name.c_str());
			}
			pParentNode->addChild(pNode, nZorder, nTag);
		}

		return pNode;
	}

	void CSceneGenerator::DoActionAfterAddToParent( NdTiXmlElement* markupXml, const char* pszType, CCNode* pNode, CCNode* pParentNode )
	{
		if (!pNode)
		{
			return;
		}

		if (strcmp(pszType, "CC2Menu") == 0
			|| strcmp(pszType, "CC2NdMenu") == 0)
		{
			bool bIsVerticallayout = strcmp(markupXml->Attribute("IsItemsVertically"), "True") == 0;
			float itemPadding = (float)atof(markupXml->Attribute("ItemsPadding"));

			if (strcmp(pszType, "CC2NdMenu") == 0)
			{
				std::string strSelectedItem = markupXml->Attribute("SelectedItem");
				((NdMenu*)pNode)->setSelectItem((CCMenuItem*)GetChildByName(pNode, strSelectedItem.c_str()));
			}
			
			if (bIsVerticallayout)
			{
				((CCMenu*)pNode)->alignItemsVerticallyWithPadding(itemPadding);
			}
			else
			{
				((CCMenu*)pNode)->alignItemsHorizontallyWithPadding(itemPadding);
			}
		}
	}

	CCNode* CSceneGenerator::CreateNodeByProperty( NdTiXmlElement* markupXml, CCNode* pNode )
	{
		if (!pNode)
		{
			return NULL;
		}
		float ScaleXf = (float)atof(markupXml->Attribute("ScaleX"));
		float ScaleYf = (float)atof(markupXml->Attribute("ScaleY"));
		float Leftf = (float)atof(markupXml->Attribute("Left"));
		float Bottomf = (float)atof(markupXml->Attribute("Bottom"));
		float AnchorPointXf = (float)atof(markupXml->Attribute("AnchorPointX"));
		float AnchorPointYf = (float)atof(markupXml->Attribute("AnchorPointY"));
		float Widthf = (float)atof(markupXml->Attribute("Width"));
		float Heightf = (float)atof(markupXml->Attribute("Height"));
		float Opacityf = (float)atof(markupXml->Attribute("Opacity"));
		bool bIsRelativeAnchorPoint = strcmp(markupXml->Attribute("IsRelativeAnchorPoint"), "True") == 0 ? true : false;
		bool bIsVisible = strcmp(markupXml->Attribute("Visibility"), "Visible") == 0;
		pNode->setScaleX(ScaleXf);
		pNode->setScaleY(ScaleYf);
		pNode->setIsRelativeAnchorPoint(bIsRelativeAnchorPoint);
		pNode->setAnchorPoint(CCPointMake(AnchorPointXf, AnchorPointYf));
		pNode->setPosition(CCPointMake(Leftf, Bottomf));
		pNode->setContentSize(CCSizeMake(Widthf, Heightf));
		pNode->setIsVisible(bIsVisible);
		
		return pNode;
	}

	CCLayer* CSceneGenerator::CreateLayerByProperty( NdTiXmlElement* markupXml, CCNode* pNode)
	{
		CCLayer* pLayer = NULL;
		if (pNode)
		{
			pLayer = (CCLayer*)pNode;
		}
		else
		{
			bool bIsTransparent = true;
			unsigned long bkClr = 0;
			if (strcmp(markupXml->Attribute("Background"), "Transparent") != 0)
			{
				bIsTransparent = false;
				bkClr = atol(markupXml->Attribute("Background"));
			}

			if (!bIsTransparent)
			{
				ccColor4B color4B;
				color4B.a = (GLubyte)(bkClr >> 0x18) & 0xffL;
				color4B.r = (GLubyte)(bkClr >> 0x10) & 0xffL;
				color4B.g = (GLubyte)(bkClr >> 8) & 0xffL;
				color4B.b = (GLubyte)(bkClr & 0xff);
				pLayer = CCLayerColor::layerWithColor(color4B);
			}
			else
			{
				pLayer = CCLayer::node();
			}
		}
		
		return (CCLayer*)CreateNodeByProperty(markupXml, (CCNode*)pLayer);
	}

	NdLayer* CSceneGenerator::CreateNdLayerByProperty( NdTiXmlElement* markupXml )
	{
		std::string ScriptOnEnterFunc = markupXml->Attribute("ScriptOnEnter");
		std::string ScriptOnExitFunc = markupXml->Attribute("ScriptOnExit");

		NdLayer* pNdlayer = NdLayer::node();
		pNdlayer->registerOnEnter(ScriptOnEnterFunc.c_str());
		pNdlayer->registerOnExit(ScriptOnExitFunc.c_str());

		return (NdLayer*)CreateLayerByProperty(markupXml, (CCNode*)pNdlayer);
	}

	NdCxList* CSceneGenerator::CreateNdCxListByProperty( NdTiXmlElement* markupXml, CCNode* pNode /*= NULL*/ )
	{
		unsigned long color = atol(markupXml->Attribute("LineColor"));
		ccColor3B LineColorl_cl_;
		LineColorl_cl_.r = (GLubyte)(color >> 0x10) & 0xffL;
		LineColorl_cl_.g = (GLubyte)(color >> 8) & 0xffL;
		LineColorl_cl_.b = (GLubyte)(color & 0xff);

		color = atol(markupXml->Attribute("ItemEndColor"));
		ccColor3B ItemEndColor_cl_;
		ItemEndColor_cl_.r = (GLubyte)(color >> 0x10) & 0xffL;
		ItemEndColor_cl_.g = (GLubyte)(color >> 8) & 0xffL;
		ItemEndColor_cl_.b = (GLubyte)(color & 0xff);

		color = atol(markupXml->Attribute("ItemStartColor"));
		ccColor3B ItemStartColor_cl_;
		ItemStartColor_cl_.r = (GLubyte)(color >> 0x10) & 0xffL;
		ItemStartColor_cl_.g = (GLubyte)(color >> 8) & 0xffL;
		ItemStartColor_cl_.b = (GLubyte)(color & 0xff);

		float fRowWidth = (float)atof(markupXml->Attribute("RowWidth"));
		float fRowHeight = (float)atof(markupXml->Attribute("RowHeight"));
		int nRecodeNumPerPage = atoi(markupXml->Attribute("RecordNumPerPage"));
		bool bLayoutMode = strcmp(markupXml->Attribute("LayoutMode"), "True") == 0;
		bool bCanPageTurned = strcmp(markupXml->Attribute("CanPageTurned"), "True") == 0;
		bool bScrollToView = strcmp(markupXml->Attribute("ScrollToView"), "True") == 0;
		std::string LoadEventFunc = markupXml->Attribute("LoadEventFunc");
		std::string UnLoadEventFunc = markupXml->Attribute("UnLoadEventFunc");

		unsigned long bkClr = atol(markupXml->Attribute("Background"));
		ccColor4B color4B;
		color4B.a = (GLubyte)(bkClr >> 0x18) & 0xffL;
		color4B.r = (GLubyte)(bkClr >> 0x10) & 0xffL;
		color4B.g = (GLubyte)(bkClr >> 8) & 0xffL;
		color4B.b = (GLubyte)(bkClr & 0xff);

		float Widthf = (float)atof(markupXml->Attribute("Width"));
		float Heightf = (float)atof(markupXml->Attribute("Height"));

		NdCxList* pNdCxList = NULL;
		if (pNode)
		{
			pNdCxList = (NdCxList*)pNode;
		}
		else
			pNdCxList = NdCxList::node(fRowHeight, color4B, CCSizeMake(Widthf, Heightf));

		pNdCxList->setHorizontal(bLayoutMode);
		pNdCxList->setLineColor(LineColorl_cl_);
		pNdCxList->setPageTurnEffect(bCanPageTurned);
		pNdCxList->setRecodeNumPerPage(nRecodeNumPerPage);
		pNdCxList->setRowWidth(fRowWidth);
		pNdCxList->setSelectedItemColor(ItemStartColor_cl_, ItemEndColor_cl_);

		return (NdCxList*)CreateLayerByProperty(markupXml, (CCNode*)pNdCxList);
	}

	NdCxListItem* CSceneGenerator::CreateNdCxListItemByProperty( NdTiXmlElement* markupXml, CCNode* pNode /*= NULL*/ )
	{
		unsigned long bkClr = atol(markupXml->Attribute("Background"));
		ccColor3B color3B;
		color3B.r = (GLubyte)(bkClr >> 0x10) & 0xffL;
		color3B.g = (GLubyte)(bkClr >> 8) & 0xffL;
		color3B.b = (GLubyte)(bkClr & 0xff);

		bool bIsSelected = strcmp(markupXml->Attribute("IsSelected"), "True") == 0;
		bool bIsDrawTopLine = strcmp(markupXml->Attribute("IsDrawTopLine"), "True") == 0;
		bool bIsDrawBottomLine = strcmp(markupXml->Attribute("IsDrawBottomLine"), "True") == 0;
		bool bIsDrawSelected = strcmp(markupXml->Attribute("IsDrawSelected"), "True") == 0;
		float fHorizontalMargin = (float)atof(markupXml->Attribute("HorizontalMargin"));
		float fVerticalMargin = (float)atof(markupXml->Attribute("VerticalMargin"));

		NdCxListItem *item = NULL;
		if (pNode)
		{
			item = (NdCxListItem*)pNode;
		}
		else
			item = NdCxListItem::itemWithColor(color3B);
		item->setDrawBottomLine(bIsDrawBottomLine);
		item->setDrawSelected(bIsDrawSelected);
		item->setDrawTopLine(bIsDrawTopLine);
		item->setItemColor(color3B);
		item->setMargin(CCSizeMake(fHorizontalMargin, fVerticalMargin));

		return (NdCxListItem*)CreateLayerByProperty(markupXml, (CCNode*)item);
	}

	CCLabelTTF* CSceneGenerator::CreateLabelTTFByProperty( NdTiXmlElement* markupXml )
	{
		std::string TextAlignment = markupXml->Attribute("TextAlignment");
		std::string FontFamily = markupXml->Attribute("FontFamily");
		std::string Text = markupXml->Attribute("Text");
		float FontSizef = (float)atof(markupXml->Attribute("FontSize"));
		float Widthf = (float)atof(markupXml->Attribute("Width"));
		float Heightf = (float)atof(markupXml->Attribute("Height"));
		unsigned long color = atol(markupXml->Attribute("Foreground"));

		CCTextAlignment alignment = CCTextAlignmentLeft;
		if (TextAlignment.compare("Right") == 0)
		{
			alignment = CCTextAlignmentRight;
		}
		else if (TextAlignment.compare("Center") == 0)
		{
			alignment = CCTextAlignmentCenter;
		}

		CCLabelTTF* pLabelTTF = CCLabelTTF::labelWithString(Text.c_str(), CCSizeMake(Widthf, Heightf), alignment, FontFamily.c_str(), FontSizef);
		ccColor3B label_cl_;
		label_cl_.r = (GLubyte)(color >> 0x10) & 0xffL;
		label_cl_.g = (GLubyte)(color >> 8) & 0xffL;
		label_cl_.b = (GLubyte)(color & 0xff);
		pLabelTTF->setColor(label_cl_);

		return (CCLabelTTF*)CreateNodeByProperty(markupXml, (CCNode*)pLabelTTF);
	}

	CCSprite* CSceneGenerator::CreateSpriteByProperty( NdTiXmlElement* markupXml )
	{
		std::string ImagePath = markupXml->Attribute("ImagePath");		
		if (!ImagePath.empty())
		{
			std::string strFileName = NdDataLogic::CFileHelper::getPath(ImagePath.c_str());			
			CCSprite* pSprite = CCSprite::spriteWithFile(strFileName.c_str());			
			(CCSprite*)CreateNodeByProperty(markupXml, (CCNode*)pSprite);
			return pSprite;	
		}	
		else
			return NULL;		
	}

	CCMenu* CSceneGenerator::CreateMenuByProperty( NdTiXmlElement* markupXml, CCNode* pNode )
	{
		CCMenu* pMenu = NULL;
		if (pNode)
		{
			pMenu = (CCMenu*)pNode;
		}
		else
			pMenu = CCMenu::menuWithItem(NULL);

		return (CCMenu*)CreateNodeByProperty(markupXml, (CCNode*)pMenu);
	}

	NdMenu* CSceneGenerator::CreateNdMenuByProperty( NdTiXmlElement* markupXml)
	{
		NdMenu* pMenu = NdMenu::menuWithItem(NULL);

		return (NdMenu*)CreateMenuByProperty(markupXml, (CCNode*)pMenu);
	}

	CCMenuItem* CSceneGenerator::CreateMenuItemByProperty( NdTiXmlElement* markupXml, CCNode* pNode )
	{
		bool bIsEnabled = strcmp(markupXml->Attribute("Enabled"), "True") != 0 ? false : true;
		bool bIsSelected = strcmp(markupXml->Attribute("IsSelected"), "True") != 0 ? false : true;

		CCMenuItem* pMenuItem = NULL;
		if (!pNode)
		{
			pMenuItem = CCMenuItem::itemWithTarget(NULL, NULL);
		}
		else
		{
			pMenuItem = (CCMenuItem*)pNode;
		}

		pMenuItem->setIsEnabled(bIsEnabled);
		if (bIsSelected)
		{
			pMenuItem->selected();
		}
		else
		{
			pMenuItem->unselected();
		}

		return (CCMenuItem*)CreateNodeByProperty(markupXml, (CCNode*)pMenuItem);
	}

	CCMenuItemLabel* CSceneGenerator::CreateMenuItemLabelByProperty( NdTiXmlElement* markupXml )
	{
		unsigned long color = atol(markupXml->Attribute("DisabledColor"));
		ccColor3B label_cl_;
		label_cl_.r = (GLubyte)(color >> 0x10) & 0xffL;
		label_cl_.g = (GLubyte)(color >> 8) & 0xffL;
		label_cl_.b = (GLubyte)(color & 0xff);
		std::string Text = markupXml->Attribute("Text");
		float Leftf = (float)atof(markupXml->Attribute("Left"));
		float Bottomf = (float)atof(markupXml->Attribute("Bottom"));
		float Widthf = (float)atof(markupXml->Attribute("Width"));
		float Heightf = (float)atof(markupXml->Attribute("Height"));

		CCLabelTTF* pLabelTTF = CCLabelTTF::labelWithString(Text.c_str(), "Arial", 24);
		ccColor3B label_ttf_;
		label_ttf_.r = (GLubyte)(color >> 0x10) & 0xffL;
		label_ttf_.g =  (GLubyte)(color >> 8) & 0xffL;
		label_ttf_.b =  (GLubyte)(color & 0xff);
		pLabelTTF->setColor(label_ttf_);

		CCMenuItemLabel* pMenuItemLabel = CCMenuItemLabel::itemWithLabel(pLabelTTF);
		pMenuItemLabel->setDisabledColor(label_cl_);
		pMenuItemLabel->setPosition(CCPointMake(Leftf, Bottomf));
		pMenuItemLabel->setContentSize(CCSizeMake(Widthf, Heightf));

		return (CCMenuItemLabel*)CreateMenuItemByProperty(markupXml, (CCNode*)pMenuItemLabel);
	}

	CCMenuItemSprite* CSceneGenerator::CreateMenuItemSpriteByProperty( NdTiXmlElement* markupXml )
	{
		std::string normalImagePath = markupXml->Attribute("NormalImagePath");
		std::string selectedImagePath = markupXml->Attribute("SelectedImagePath");
		std::string disabledImagePath = markupXml->Attribute("DisabledImagePath");

		CCSprite* pNormalSprite = NULL;
		CCSprite* pSelectedSprite = NULL;
		CCSprite* pDisabledSprite = NULL;
		if (!normalImagePath.empty())
		{
			pNormalSprite = CCSprite::spriteWithFile(NdDataLogic::CFileHelper::getPath(normalImagePath.c_str()).c_str());
		}
		if (!selectedImagePath.empty())
		{
			pSelectedSprite = CCSprite::spriteWithFile(NdDataLogic::CFileHelper::getPath(selectedImagePath.c_str()).c_str());
		}	
		if (!disabledImagePath.empty())
		{
			pDisabledSprite = CCSprite::spriteWithFile(NdDataLogic::CFileHelper::getPath(disabledImagePath.c_str()).c_str());
		}
		if (pNormalSprite)
		{
			CCMenuItemSprite* pMenuItemSprite = CCMenuItemSprite::itemFromNormalSprite(pNormalSprite, pSelectedSprite);
			pMenuItemSprite->setDisabledImage(pDisabledSprite);

			return (CCMenuItemSprite*)CreateMenuItemByProperty(markupXml, (CCNode*)pMenuItemSprite);
		}		
		return NULL;
	}

	CNdEdit* CSceneGenerator::CreateNdEditor( NdTiXmlElement* markupXml )
	{
		float Leftf = (float)atof(markupXml->Attribute("Left"));
		float Bottomf = (float)atof(markupXml->Attribute("Bottom"));
		bool bMultiline = strcmp(markupXml->Attribute("Multiline"), "True") != 0 ? false : true;
		bool bPwdMode = strcmp(markupXml->Attribute("PwdMode"), "True") != 0 ? false : true;
		float Widthf = (float)atof(markupXml->Attribute("Width"));
		float Heightf = (float)atof(markupXml->Attribute("Height"));
		bool bIsVisible = strcmp(markupXml->Attribute("Visibility"), "Visible") == 0;
		std::string Text = markupXml->Attribute("DefaultText");
		bool bIsEnabled = strcmp(markupXml->Attribute("Enabled"), "True") != 0 ? false : true;
		float FontSizef = (float)atof(markupXml->Attribute("FontSize"));
		int nMaxLimit = atoi(markupXml->Attribute("MaxLimit"));

		CNdEdit* pEdit = new CNdEdit();
		pEdit->init(bMultiline, bPwdMode);
		pEdit->setText(Text);
		pEdit->setEnabled(bIsEnabled);
		pEdit->setMaxText(nMaxLimit);
		pEdit->setVisible(bIsVisible);
#ifndef ND_WIN32
		pEdit->SetTextSize(nMaxLimit);
#endif
		pEdit->setRect(CCRectMake(Leftf, Bottomf, Widthf, Heightf));

		return NULL;
	}

	CNdButton* CSceneGenerator::CreateNdButtonByProperty( NdTiXmlElement* markupXml, CCNode* pNode /*= NULL*/ )
	{
		float Widthf = (float)atof(markupXml->Attribute("Width"));
		float Heightf = (float)atof(markupXml->Attribute("Height"));
		const char* pszNormalImagePath = markupXml->Attribute("NormalImagePath");
		const char* pszSelectedImagePath = markupXml->Attribute("SelectedImagePath");
		const char* pszDisabledImagePath = markupXml->Attribute("DisabledImagePath");
		const char* pszText = markupXml->Attribute("Text");
		const char* pszListenerFunc = markupXml->Attribute("ListenerFunc");
		bool bIsEnabled = strcmp(markupXml->Attribute("Enabled"), "True") != 0 ? false : true;
		bool bIsSelected = strcmp(markupXml->Attribute("IsSelected"), "True") != 0 ? false : true;
		std::string TextAlignment = markupXml->Attribute("TextAlignment");
		const char* pszFontFamily = markupXml->Attribute("FontFamily");
		float FontSizef = (float)atof(markupXml->Attribute("FontSize"));
		unsigned long color = atol(markupXml->Attribute("Foreground"));
		ccColor3B label_cl_;
		label_cl_.r = (GLubyte)(color >> 0x10) & 0xffL;
		label_cl_.g = (GLubyte)(color >> 8) & 0xffL;
		label_cl_.b = (GLubyte)(color & 0xff);

		CCTextAlignment alignment = CCTextAlignmentLeft;
		if (TextAlignment.compare("Right") == 0)
		{
			alignment = CCTextAlignmentRight;
		}
		else if (TextAlignment.compare("Center") == 0)
		{
			alignment = CCTextAlignmentCenter;
		}
		std::string strTemp = pszNormalImagePath;
		if (strTemp.empty())
		{
			pszNormalImagePath = NULL;
		}
		strTemp = pszSelectedImagePath;
		if (strTemp.empty())
		{
			pszSelectedImagePath = NULL;
		}
		strTemp = pszDisabledImagePath;
		if (strTemp.empty())
		{
			pszDisabledImagePath = NULL;
		}
		CNdButton* pButton = CNdButton::buttonWithString(pszNormalImagePath, pszSelectedImagePath, pszDisabledImagePath, pszText, alignment, pszFontFamily, FontSizef, CCSizeMake(Widthf, Heightf));
		pButton->setLabelColor(label_cl_);
		pButton->setEnabled(bIsEnabled);
		pButton->setSelected(bIsSelected);
		pButton->setListener(pszListenerFunc);

		return (CNdButton*)CreateMenuByProperty(markupXml, (CCNode*)pButton);
	}

	CCNode* CSceneGenerator::GetChildByName( CCNode* pParent, const char* lpszChildName )
	{
		if (pParent && lpszChildName)
		{
			int nTag = HashString(lpszChildName);
			return pParent->getChildByTag(nTag);
		}
		return NULL;
	}

	int CSceneGenerator::HashString( const char* lpszValue )
	{
		int h = 0;
		char *p = (char*)lpszValue;

		while ( *p != '\0' )
		{
			int v = (*p);
			if ((*p) >= 'a' && (*p) <= 'z')
			{
				v -= 32;
			}
			h = (h << 2) + v;
			p++;
		}

		return h;
	}
}