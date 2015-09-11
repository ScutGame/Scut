LOCAL_PATH := $(call my-dir)

include $(CLEAR_VARS)

LOCAL_MODULE := cocos2dlua_shared

LOCAL_MODULE_FILENAME := libcocos2dlua

LOCAL_SRC_FILES := hellolua/main.cpp \
                   ../../Classes/AppDelegate.cpp

LOCAL_C_INCLUDES := $(LOCAL_PATH)/../../Classes \
        $(LOCAL_PATH)/../../../../SDK/ScutSDK/

LOCAL_STATIC_LIBRARIES := curl_static_prebuilt

LOCAL_WHOLE_STATIC_LIBRARIES := quickcocos2dx
LOCAL_WHOLE_STATIC_LIBRARIES += scutsdk_static

include $(BUILD_SHARED_LIBRARY)

$(call import-module,SDK/ScutSDK/proj.android)
$(call import-module,lib/proj.android)