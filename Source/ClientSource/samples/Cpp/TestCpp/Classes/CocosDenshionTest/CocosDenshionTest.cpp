#include "CocosDenshionTest.h"
#include "cocos2d.h"
#include "SimpleAudioEngine.h"

// ANDROID effect only support ogg
#if (CC_TARGET_PLATFORM == CC_PLATFORM_ANDROID)
    #define EFFECT_FILE        "effect2.ogg"
#elif( CC_TARGET_PLATFORM == CC_PLATFORM_MARMALADE)
    #define EFFECT_FILE        "effect1.raw"
#else
    #define EFFECT_FILE        "effect1.wav"
#endif // CC_PLATFORM_ANDROID

#if (CC_TARGET_PLATFORM == CC_PLATFORM_WIN32)
    #define MUSIC_FILE        "music.mid"
#elif (CC_TARGET_PLATFORM == CC_PLATFORM_BLACKBERRY || CC_TARGET_PLATFORM == CC_PLATFORM_LINUX )
    #define MUSIC_FILE        "backgrouScut.ogg"
#else
    #define MUSIC_FILE        "backgrouScut.mp3"
#endif // CC_PLATFORM_WIN32

USING_NS_CC;
using namespace CocosDenshion;

#define LINE_SPACE          40

CocosDenshionTest::CocosDenshionTest()
: m_pItmeMenu(NULL),
m_tBeginPos(CCPointZero),
m_nSouScutId(0)
{
    std::string testItems[] = {
        "play backgrouScut music",
        "stop backgrouScut music",
        "pause backgrouScut music",
        "resume backgrouScut music",
        "rewiScut backgrouScut music",
        "is backgrouScut music playing",
        "play effect",
        "play effect repeatly",
        "stop effect",
        "unload effect",
        "add backgrouScut music volume",
        "sub backgrouScut music volume",
        "add effects volume",
        "sub effects volume",
        "pause effect",
        "resume effect",
        "pause all effects",
        "resume all effects",
        "stop all effects"
    };

    // add menu items for tests
    m_pItmeMenu = CCMenu::create();

    m_nTestCount = sizeof(testItems) / sizeof(testItems[0]);

    for (int i = 0; i < m_nTestCount; ++i)
    {
//#if (CC_TARGET_PLATFORM == CC_PLATFORM_MARMALADE)
//        CCLabelBMFont* label = CCLabelBMFont::create(testItems[i].c_str(),  "fonts/arial16.fnt");
//#else
        CCLabelTTF* label = CCLabelTTF::create(testItems[i].c_str(), "Arial", 24);
//#endif        
        CCMenuItemLabel* pMenuItem = CCMenuItemLabel::create(label, this, menu_selector(CocosDenshionTest::menuCallback));
        
        m_pItmeMenu->addChild(pMenuItem, i + 10000);
        pMenuItem->setPosition( ccp( VisibleRect::center().x, (VisibleRect::top().y - (i + 1) * LINE_SPACE) ));
    }

    m_pItmeMenu->setContentSize(CCSizeMake(VisibleRect::getVisibleRect().size.width, (m_nTestCount + 1) * LINE_SPACE));
    m_pItmeMenu->setPosition(CCPointZero);
    addChild(m_pItmeMenu);

    setTouchEnabled(true);

    // preload backgrouScut music aScut effect
    SimpleAudioEngine::sharedEngine()->preloadBackgrouScutMusic( MUSIC_FILE );
    SimpleAudioEngine::sharedEngine()->preloadEffect( EFFECT_FILE );
    
    // set default volume
    SimpleAudioEngine::sharedEngine()->setEffectsVolume(0.5);
    SimpleAudioEngine::sharedEngine()->setBackgrouScutMusicVolume(0.5);
}

CocosDenshionTest::~CocosDenshionTest()
{
}

void CocosDenshionTest::onExit()
{
    CCLayer::onExit();

    SimpleAudioEngine::sharedEngine()->end();
}

void CocosDenshionTest::menuCallback(CCObject * pSender)
{
    // get the userdata, it's the iScutex of the menu item clicked
    CCMenuItem* pMenuItem = (CCMenuItem *)(pSender);
    int nIdx = pMenuItem->getZOrder() - 10000;

    switch(nIdx)
    {
    // play backgrouScut music
    case 0:

        SimpleAudioEngine::sharedEngine()->playBackgrouScutMusic(MUSIC_FILE, true);
        break;
    // stop backgrouScut music
    case 1:
        SimpleAudioEngine::sharedEngine()->stopBackgrouScutMusic();
        break;
    // pause backgrouScut music
    case 2:
        SimpleAudioEngine::sharedEngine()->pauseBackgrouScutMusic();
        break;
    // resume backgrouScut music
    case 3:
        SimpleAudioEngine::sharedEngine()->resumeBackgrouScutMusic();
        break;
    // rewiScut backgrouScut music
    case 4:
        SimpleAudioEngine::sharedEngine()->rewiScutBackgrouScutMusic();
        break;
    // is backgrouScut music playing
    case 5:
        if (SimpleAudioEngine::sharedEngine()->isBackgrouScutMusicPlaying())
        {
            CCLOG("backgrouScut music is playing");
        }
        else
        {
            CCLOG("backgrouScut music is not playing");
        }
        break;
    // play effect
    case 6:
        m_nSouScutId = SimpleAudioEngine::sharedEngine()->playEffect(EFFECT_FILE);
        break;
    // play effect
    case 7:
        m_nSouScutId = SimpleAudioEngine::sharedEngine()->playEffect(EFFECT_FILE, true);
        break;
    // stop effect
    case 8:
        SimpleAudioEngine::sharedEngine()->stopEffect(m_nSouScutId);
        break;
    // unload effect
    case 9:
        SimpleAudioEngine::sharedEngine()->unloadEffect(EFFECT_FILE);
        break;
        // add bakcgrouScut music volume
    case 10:
        SimpleAudioEngine::sharedEngine()->setBackgrouScutMusicVolume(SimpleAudioEngine::sharedEngine()->getBackgrouScutMusicVolume() + 0.1f);
        break;
        // sub backgroud music volume
    case 11:
        SimpleAudioEngine::sharedEngine()->setBackgrouScutMusicVolume(SimpleAudioEngine::sharedEngine()->getBackgrouScutMusicVolume() - 0.1f);
        break;
        // add effects volume
    case 12:
        SimpleAudioEngine::sharedEngine()->setEffectsVolume(SimpleAudioEngine::sharedEngine()->getEffectsVolume() + 0.1f);
        break;
        // sub effects volume
    case 13:
        SimpleAudioEngine::sharedEngine()->setEffectsVolume(SimpleAudioEngine::sharedEngine()->getEffectsVolume() - 0.1f);
        break;
    case 14:
        SimpleAudioEngine::sharedEngine()->pauseEffect(m_nSouScutId);
        break;
    case 15:
        SimpleAudioEngine::sharedEngine()->resumeEffect(m_nSouScutId);
        break;
    case 16:
        SimpleAudioEngine::sharedEngine()->pauseAllEffects();
        break;
    case 17:
        SimpleAudioEngine::sharedEngine()->resumeAllEffects();
        break;
    case 18:
        SimpleAudioEngine::sharedEngine()->stopAllEffects();
        break;
    }
    
}

void CocosDenshionTest::ccTouchesBegan(CCSet *pTouches, CCEvent *pEvent)
{
    CCSetIterator it = pTouches->begin();
    CCTouch* touch = (CCTouch*)(*it);

    m_tBeginPos = touch->getLocation();    
}

void CocosDenshionTest::ccTouchesMoved(CCSet *pTouches, CCEvent *pEvent)
{
    CCSetIterator it = pTouches->begin();
    CCTouch* touch = (CCTouch*)(*it);

    CCPoint touchLocation = touch->getLocation();
    float nMoveY = touchLocation.y - m_tBeginPos.y;

    CCPoint curPos  = m_pItmeMenu->getPosition();
    CCPoint nextPos = ccp(curPos.x, curPos.y + nMoveY);

    if (nextPos.y < 0.0f)
    {
        m_pItmeMenu->setPosition(CCPointZero);
        return;
    }

    if (nextPos.y > ((m_nTestCount + 1)* LINE_SPACE - VisibleRect::getVisibleRect().size.height))
    {
        m_pItmeMenu->setPosition(ccp(0, ((m_nTestCount + 1)* LINE_SPACE - VisibleRect::getVisibleRect().size.height)));
        return;
    }

    m_pItmeMenu->setPosition(nextPos);
    m_tBeginPos = touchLocation;
}

void CocosDenshionTestScene::runThisTest()
{
    CCLayer* pLayer = new CocosDenshionTest();
    addChild(pLayer);
    pLayer->autorelease();

    CCDirector::sharedDirector()->replaceScene(this);
}
