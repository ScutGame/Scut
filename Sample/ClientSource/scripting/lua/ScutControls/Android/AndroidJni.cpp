#include "CCCommon.h"
#include "AndroidJni.h"

#include <android/log.h>

#if 1
#define  LOG_TAG    "AndroidJni"
#define  LOGD(...)  __android_log_print(ANDROID_LOG_DEBUG,LOG_TAG,__VA_ARGS__)
#else
#define  LOGD(...) 
#endif

static AndroidJni GANDROID_JNI;
static std::string ANDROID_IMSI;
static std::string ANDROID_IMEI;

extern JavaVM *gJavaVM;

JNIEXPORT void JNICALL Java_cn_com_nd_jni_JniInstance_setImsiAndImei( JNIEnv * env, jobject thisz ,jobject instance, jstring imsi, jstring imei)
{

	// init instance
	GANDROID_JNI.jAndroidObject = env->NewGlobalRef(instance);
	env->GetJavaVM(&GANDROID_JNI.jVM);
//	NDLog("init end.");

	const char* cimsi;
	const char* cimei;
	
	cimsi = env->GetStringUTFChars(imsi,NULL);
	cimei = env->GetStringUTFChars(imei,NULL);

	if (cimsi) 
	{
		ANDROID_IMSI = std::string(cimsi);
	}

	if (cimei)
	{
		ANDROID_IMEI = std::string(cimei);
	}

	env->ReleaseStringUTFChars(imsi,cimsi);
	env->ReleaseStringUTFChars(imei,cimei);
	
}

std::string AndroidJni::getImsi()
{
	return ANDROID_IMSI;
}

std::string AndroidJni::getImei()
{
	return ANDROID_IMEI;
}

std::string AndroidJni::getSimCardUUID()
{
	if (ANDROID_IMSI.length() < 9)
	{
		return "imei" + ANDROID_IMEI;
	}
	else 
	{
		return ANDROID_IMSI;
	}
}

void AndroidJni::startActivity( std::string action ,  std::string extra)
{
	//if (GANDROID_JNI.jAndroidObject != NULL && GANDROID_JNI.jVM != NULL)
	//{
		LOGD("startActivity begin gJavaVM : %d", gJavaVM);
		JNIEnv* pEnv = NULL;
		//GANDROID_JNI.jVM
		gJavaVM->AttachCurrentThread(&pEnv,NULL);
		if (pEnv == NULL)
		{
			return ;
		}
		static jmethodID mid = NULL;

		jclass ndrWndClass = pEnv->FindClass("com/nd/lib/NdUtilityJni");


		if (ndrWndClass == NULL)
		{
				return ;
		}

		if (mid == NULL)
		{
			mid = pEnv->GetStaticMethodID(ndrWndClass,"startActivity","(Ljava/lang/String;Ljava/lang/String;)V");
		}


		if (mid == NULL)
		{
			return ;
		} else 
		{	
			jstring jaction = pEnv->NewStringUTF(action.c_str());
			jstring jextra = pEnv->NewStringUTF(extra.c_str());
			pEnv->CallStaticVoidMethod(ndrWndClass,mid,jaction,jextra);
			pEnv->DeleteLocalRef(jaction);
			pEnv->DeleteLocalRef(jextra);
			pEnv->DeleteLocalRef(ndrWndClass);
		}
		LOGD("startActivity end");
	//	NDLog("startActivity() end.");
	//}

//	NDLog("startActivity() null.");
}

void AndroidJni::startWebView(std::string url, std::string title, int ox , int oy , int width , int height)
{
		JNIEnv* pEnv = NULL;
		//GANDROID_JNI.jVM
		LOGD("startWebView begin gJavaVM : %d", gJavaVM);
		gJavaVM->AttachCurrentThread(&pEnv,NULL);
		if (pEnv == NULL)
		{
			return ;
		}
		static jmethodID mid = NULL;

		jclass ndrWndClass = pEnv->FindClass("org/cocos2dx/lib/WebViewActivity");


		if (ndrWndClass == NULL)
		{
				return ;
		}

		if (mid == NULL)
		{
			mid = pEnv->GetStaticMethodID(ndrWndClass,"startUrl","(Ljava/lang/String;Ljava/lang/String;IIII)V");
		}


		if (mid == NULL)
		{
			return ;
		} else 
		{	
			jstring jurl = pEnv->NewStringUTF(url.c_str());
			jstring jtitle = pEnv->NewStringUTF(title.c_str());
			pEnv->CallStaticVoidMethod(ndrWndClass,mid,jurl, jtitle,ox,oy, width, height);
			pEnv->DeleteLocalRef(jurl);
			pEnv->DeleteLocalRef(jtitle);
			pEnv->DeleteLocalRef(ndrWndClass);
		}
}

void AndroidJni::endWebView()
{
		JNIEnv* pEnv = NULL;
		//GANDROID_JNI.jVM
		gJavaVM->AttachCurrentThread(&pEnv,NULL);
		if (pEnv == NULL)
		{
			return ;
		}
		static jmethodID mid = NULL;

		jclass ndrWndClass = pEnv->FindClass("org/cocos2dx/lib/WebViewActivity");


		if (ndrWndClass == NULL)
		{
				return ;
		}

		if (mid == NULL)
		{
			mid = pEnv->GetStaticMethodID(ndrWndClass,"endUrl","()V");
		}


		if (mid == NULL)
		{
			return ;
		} else 
		{	
			//jstring jurl = pEnv->NewStringUTF(url.c_str());
			pEnv->CallStaticVoidMethod(ndrWndClass,mid);
			pEnv->DeleteLocalRef(ndrWndClass);
		}
}

void AndroidJni::switchWebView(std::string url)
{
		JNIEnv* pEnv = NULL;
		//GANDROID_JNI.jVM
		gJavaVM->AttachCurrentThread(&pEnv,NULL);
		if (pEnv == NULL)
		{
			return ;
		}
		static jmethodID mid = NULL;

		jclass ndrWndClass = pEnv->FindClass("org/cocos2dx/lib/WebViewActivity");


		if (ndrWndClass == NULL)
		{
				return ;
		}

		if (mid == NULL)
		{
			mid = pEnv->GetStaticMethodID(ndrWndClass,"switchUrl","(Ljava/lang/String;)V");
		}


		if (mid == NULL)
		{
			return ;
		} else 
		{	
			jstring jurl = pEnv->NewStringUTF(url.c_str());
			pEnv->CallStaticVoidMethod(ndrWndClass,mid,jurl);
			pEnv->DeleteLocalRef(jurl);
			pEnv->DeleteLocalRef(ndrWndClass);
		}
}
/*
void AndroidJni::openFeedBack()
{	
	std::string url = "http://feedback.sj.91.com/Tribe/FeedBack.aspx?itemcode=2122&softversion=1.0&pt=0";
	if (!ANDROID_IMEI.empty())
	{
		url.append("&imei=");
		url.append(ANDROID_IMEI);
	}

	NdTribe::CMainUserInfo* pUser = NdTribe::CMainUserInfo::Instance();
	if (pUser)
	{
		CPlayerData* pdata = pUser->getUseInfoData();
		if(pdata)
		{
			url.append("&account=");
			url.append(pdata->strUserName91);
		}
	}

	startActivity("cn.com.nd.jni.feedback",url);
}
*/


bool AndroidWebView(std::string strUrl, std::string szTitle, int ox , int oy , int width , int height)
{
	//AndroidJni::startActivity("org.cocos2dx.lib.jni.show_webview",strUrl);
	LOGD("AndroidWebView begin");
	AndroidJni::startWebView(strUrl, szTitle, ox, oy, width, height);
	LOGD("AndroidWebView end");
	return true;
}

void CloseAndroidWebView()
{
	//AndroidJni::startActivity("org.cocos2dx.lib.jni.show_webview",strUrl);
	AndroidJni::endWebView();
}

void SwitchAndroidWebViewUrl(std::string strUrl)
{
	AndroidJni::switchWebView(strUrl);
}

void AndroidJni::openPayLayer( std::string name )
{
	AndroidJni::startActivity("cn.com.nd.jni.payment",name);
}

void AndroidJni::getAddressBook( std::map<std::string, std::string> &mapAddressBook )
{

	if (GANDROID_JNI.jVM == NULL || GANDROID_JNI.jAndroidObject == NULL)
	{
		return;
	}

	JNIEnv* pEnv = NULL;

	GANDROID_JNI.jVM->AttachCurrentThread(&pEnv, NULL);
	if (pEnv == NULL)
	{
		//NDLog("getAddressBook pEnv == NULL ");
		return;
	}

	static jmethodID mid = NULL;
	if (mid == NULL)
	{
		jclass mclass = pEnv->FindClass("cn/com/nd/jni/JniInstance");
		if (mclass == NULL)
		{
			return;
		}

		mid = pEnv->GetMethodID(mclass,"getContacts","()Ljava/lang/String;");
	}

	if (mid)
	{  
		jstring jstr = (jstring)pEnv->CallObjectMethod(GANDROID_JNI.jAndroidObject, mid);

		const char* chars;
		chars = pEnv->GetStringUTFChars(jstr,NULL);
		char *newstring;

		newstring = strdup( chars );
		if (chars != NULL) 
		{
			char* pChar = strtok(newstring,";");
			while (pChar)
			{
				std::string strTemp = pChar;
				int nFind = strTemp.find(",");
				if (nFind != std::string::npos)
				{
					std::string name = strTemp.substr(nFind+1, strTemp.length() - nFind - 1);
					mapAddressBook[strTemp.substr(0, nFind)] = strTemp.substr(nFind+1, strTemp.length() - nFind - 1);
				}

				pChar = strtok(NULL,";");
			}

		}

		free( newstring );
	}

}
