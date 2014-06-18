LOCAL_PATH := $(call my-dir)

include $(CLEAR_VARS)

LOCAL_MODULE := ScutUitility
LOCAL_CPPFLAGS +=	-DSCUT_ANDROID  -DUSE_FILE32API -D__STDC_LIMIT_MACROS=1

LOCAL_SRC_FILES := \
../ScutUtilityJni.cpp \
../../ScutLocale.cpp \
../../ScutUtils.cpp \
../../ScutLuaLan.cpp \
../../PlatformProcedure.cpp \
../../ScutLanFactory.cpp \
../../LUA_Export/LuaScutUtility.cpp

LOCAL_LDLIBS := -L$(call host-path, $(LOCAL_PATH)/../../libs/armeabi) \
                -llog
				
LOCAL_C_INCLUDES := $(LOCAL_PATH)/ \
$(LOCAL_PATH)/../../../cocos2dx/platform/android \
$(LOCAL_PATH)/../../../cocos2dx/platform/android/jni \
$(LOCAL_PATH)/../../../cocos2dx/platform \
$(LOCAL_PATH)/../../../cocos2dx/include \
$(LOCAL_PATH)/../../../scripting/lua/lua	\
$(LOCAL_PATH)/../../../scripting/lua/tolua \
$(LOCAL_PATH)/../../../cocos2dx/platform/android	\
$(LOCAL_PATH)/../../../scripting/lua/lua	\
$(LOCAL_PATH)/../../../scripting/lua/tolua \
$(LOCAL_PATH)/../../../cocos2dx \
$(LOCAL_PATH)/../../../cocos2dx/kazmath/include \
$(LOCAL_PATH)/../../../cocos2dx/include \
$(LOCAL_PATH)/../../../cocos2dx/platform \
$(LOCAL_PATH)/../../../cocos2dx/script_support \
$(LOCAL_PATH)/../../../scripting/lua/cocos2dx_support \
$(LOCAL_PATH)/../../../cocos2dx/support/image_support	\
$(LOCAL_PATH)/../../../cocos2dx/platform/third_party/android \
$(LOCAL_PATH)/../../../cocos2dx/platform/third_party/android/prebuilt/libcurl/include/curl \
$(LOCAL_PATH)/../../../scripting/lua/ScutControls \
$(LOCAL_PATH)/../../../scripting/lua/ScutControls/Android \
$(LOCAL_PATH)/../../../cocos2dx/platform/third_party/android/iconv \
$(LOCAL_PATH)/../ \
$(LOCAL_PATH)/../../ \
$(LOCAL_PATH)/../../../ScutDataLogic \
$(LOCAL_PATH)/../../../ScutSystem \
$(LOCAL_PATH)/../../LUA_Export

#include $(BUILD_SHARED_LIBRARY)
include $(BUILD_STATIC_LIBRARY)