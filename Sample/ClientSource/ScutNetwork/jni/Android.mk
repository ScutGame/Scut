LOCAL_PATH := $(call my-dir)

# compile scutnet.so
include $(CLEAR_VARS)

MY_CURL :=curl-7.21.4
MY_ND_SYSTEM :=ScutSystem
MY_NDR_BULD :=android_builder

LOCAL_MODULE := libScutNetwork
LOCAL_CPPFLAGS +=	-DSCUT_ANDROID

LOCAL_SRC_FILES := \
../HttpSession.cpp \
../HttpClientResponse.cpp \
../HttpClientRequest.cpp \
../HttpClient.cpp \
../TcpClient.cpp \
../NetClientBase.cpp \
../ZipUtils.cpp	\
../LuaHelper.cpp	\
../TcpSceneManager.cpp	\
../INetStatusNotify.cpp

LOCAL_LDLIBS := -L$(call host-path, $(LOCAL_PATH)/../../libs/armeabi) \
                -llog

LOCAL_C_INCLUDES := $(LOCAL_PATH)/ \
$(LOCAL_PATH)/../ \
$(LOCAL_PATH)/../../cocos2dx \
$(LOCAL_PATH)/../../cocos2dx/kazmath/include \
$(LOCAL_PATH)/../../cocos2dx/cocoa \
$(LOCAL_PATH)/../../cocos2dx/include \
$(LOCAL_PATH)/../../cocos2dx/platform \
$(LOCAL_PATH)/../../cocos2dx/platform/android \
$(LOCAL_PATH)/../../cocos2dx/platform/third_party/android \
$(LOCAL_PATH)/../../cocos2dx/platform/third_party/android/prebuilt/libcurl/include \
$(LOCAL_PATH)/../../cocos2dx/platform/third_party/android/prebuilt/libcurl/include/curl \
$(LOCAL_PATH)/../../scripting/lua/lua \
$(LOCAL_PATH)/../../scripting/lua/tolua \
$(LOCAL_PATH)/../../scripting/lua/cocos2dx_support \
$(LOCAL_PATH)/../../scripting/lua/cocos2dx_support/platform/android \
$(LOCAL_PATH)/../../ScutDataLogic/zip_support \
$(LOCAL_PATH)/../../ScutSystem

LOCAL_LDLIBS := -L$(call host-path, $(LOCAL_PATH)/../../libs/armeabi) \
                -llog \
				
#it is used for ndk-r4
#LOCAL_LDLIBS := -L$(LOCAL_PATH)/../$(MY_CURL)/lib/.libs \
#                -lcurl 


# it is used for ndk-r5    
# because the new Windows toolchain doesn't support Cygwin's drive
# mapping (i.e /cygdrive/c/ instead of C:/)  
#LOCAL_LDLIBS := -L$(call host-path, $(LOCAL_PATH)/../$(MY_NDR_BULD)/backup) \
#                -lcurl
                
#LOCAL_LDLIBS := -L$(call host-path, $(LOCAL_PATH)/../$(MY_NDR_BULD)/backup)	\
#								-lScutSystem	\
#								-lcurl	

# define the macro to compile through support/zip_support/ioapi.c                
LOCAL_CFLAGS := -DUSE_FILE32API
                                 
#include $(BUILD_SHARED_LIBRARY)
include $(BUILD_STATIC_LIBRARY)

#=====
include $(CLEAR_VARS)
LOCAL_MODULE := preScutNetwork.a
EXPORT_LOCAL_C_INCLUDES	:=$(LOCAL_PATH)/ \
$(LOCAL_PATH)/../ \
$(LOCAL_PATH)/../../cocos2dx/platform/third_party/android \
$(LOCAL_PATH)/../../cocos2dx/platform/third_party/android/curl \
$(LOCAL_PATH)/../../ScutDataLogic/zip_support \
$(LOCAL_PATH)/../../ScutSystem
include $(PREBUILD_STATIC_LIBRARY)
#=====