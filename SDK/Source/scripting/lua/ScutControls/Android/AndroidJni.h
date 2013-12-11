#ifndef _ANDROID_JNI_H_
#define _ANDROID_JNI_H_

#include <jni.h>
#include <string>
#include <map>

class AndroidJni
{
public :
	static std::string getImsi();
	static std::string getImei();
	static std::string getSimCardUUID();
	static void startActivity(std::string action, std::string extra);
	static void openPayLayer(std::string name);
	static void openFeedBack();
	static void sendMsg(std::string msg, std::string telphones);
	static void getAddressBook(std::map<std::string, std::string> &mapAddressBook);
	static void startWebView(std::string url, std::string title, int ox , int oy , int width , int height);
	static void endWebView();
	static void switchWebView(std::string url);
public :

	jobject jAndroidObject;
	JavaVM* jVM;
};




#ifdef __cplusplus
extern "C" {
#endif
/*
 * Class:     cn_com_nd_jni_JniInstance
 * Method:    setImsiAndImei
 * Signature: (Ljava/lang/String;Ljava/lang/String;)V
 */
	JNIEXPORT void JNICALL Java_cn_com_nd_jni_JniInstance_setImsiAndImei(JNIEnv *, jobject,  jobject instance, jstring, jstring);
	bool AndroidWebView(std::string strUrl, std::string szTitle, int ox , int oy , int width , int height);
	void CloseAndroidWebView();
	void SwitchAndroidWebViewUrl(std::string strUrl);
#ifdef __cplusplus
}
#endif

#endif