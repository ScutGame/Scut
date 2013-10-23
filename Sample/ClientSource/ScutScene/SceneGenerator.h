#pragma once
#include "base_nodes/CCNode.h"
#include "layers_scenes_transitions_nodes/CCLayer.h"
#include "label_nodes/CCLabelTTF.h"
#include "menu_nodes/CCMenu.h"
#include "menu_nodes/CCMenuItem.h"
#include "sprite_nodes/CCSprite.h"
#include "tinyxml.h"

USING_NS_CC;

namespace NdCxControl
{
	class NdScene;	
	class CNdEdit;
	class NdLayer;
	class NdMenu;
	class NdCxListItem;
	class NdCxList;
	class CNdButton;
	class CSceneGenerator
	{
	private:
		CSceneGenerator(void);
		virtual ~CSceneGenerator(void);
	public:
		static CSceneGenerator* Instance();

		//获取指定名称的场景
		NdScene* AcquireScene(const char* lpszSceneName);

		//根据指定的父节点，儿子名称获取对应的儿子节点对象
		CCNode* GetChildByName(CCNode* pParent, const char* lpszChildName);

	protected:
		bool LoadScene(NdScene* pDest, const char* lpszSceneName);

		//加载场景及其子节点
		bool LoadLayer(NdTiXmlElement* markupXml, CCNode* pParentNode);
		bool LoadChildNode(NdTiXmlElement* markupXml, CCNode* pParentNode);

		//创建节点并添加到父节点
		CCNode* CreateNodeAddToParent(NdTiXmlElement* markupXml, const char* pszType, CCNode* pParentNode);
		void DoActionAfterAddToParent(NdTiXmlElement* markupXml, const char* pszType, CCNode* pNode, CCNode* pParentNode = NULL);
		//创建Cocos2d对象
		//创建节点
		CCNode* CreateNodeByProperty(NdTiXmlElement* markupXml, CCNode* pParentNode);

		//创建layer
		CCLayer* CreateLayerByProperty(NdTiXmlElement* markupXml, CCNode* pNode = NULL);
		NdLayer* CreateNdLayerByProperty(NdTiXmlElement* markupXml);

		//创建列表
		NdCxList* CreateNdCxListByProperty(NdTiXmlElement* markupXml, CCNode* pNode = NULL);
		NdCxListItem* CreateNdCxListItemByProperty(NdTiXmlElement* markupXml, CCNode* pNode = NULL);

		//创建label
		CCLabelTTF* CreateLabelTTFByProperty(NdTiXmlElement* markupXml);

		//创建精灵
		CCSprite* CreateSpriteByProperty(NdTiXmlElement* markupXml);

		//创建菜单
		CCMenu* CreateMenuByProperty(NdTiXmlElement* markupXml, CCNode* pNode = NULL);
		NdMenu* CreateNdMenuByProperty(NdTiXmlElement* markupXml);

		//创建菜单项
		CCMenuItem* CreateMenuItemByProperty(NdTiXmlElement* markupXml, CCNode* pNode = NULL);
		CCMenuItemLabel* CreateMenuItemLabelByProperty(NdTiXmlElement* markupXml);
		CCMenuItemSprite* CreateMenuItemSpriteByProperty(NdTiXmlElement* markupXml);

		//创建编辑框
		CNdEdit*  CreateNdEditor(NdTiXmlElement* markupXml);

		//创建按钮
		CNdButton* CreateNdButtonByProperty(NdTiXmlElement* markupXml, CCNode* pNode = NULL);
	private:
		int HashString(const char* lpszValue);
	};
}