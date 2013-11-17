LOCAL_PATH := $(call my-dir)

# compile scutsystem.so
include $(CLEAR_VARS)
LOCAL_MODULE := libScutSystem

LOCAL_CPPFLAGS +=	-DSCUT_ANDROID -DUSE_FILE32API -D__STDC_LIMIT_MACROS=1
LOCAL_SRC_FILES := \
../Stream.cpp \
../stdafx.cpp \
../ScutString.cpp \
../Trace.cpp \
../AutoGuard.cpp \
../PathUtility.cpp \
../ScutUtility.cpp \
../md5.cpp \
../Des91Priv.cpp \
../WjcDes.cpp \
../unzip.cpp \
../ZipUnZip.cpp	\
../Des91Priv.cpp \
../StdDES2.cpp \
../LUA_Export/LuaScutSystem.cpp \
../tinyxml/tinystr.cpp	\
../tinyxml/tinyxml.cpp	\
../tinyxml/tinyxmlerror.cpp	\
../tinyxml/tinyxmlparser.cpp

LOCAL_LDLIBS := -L$(call host-path, $(LOCAL_PATH)/../../libs/armeabi) \
                -llog \
                -L$(call host-path, $(LOCAL_PATH)/../../cocos2dx/platform/third_party/android/libraries) -liconv

LOCAL_C_INCLUDES := $(LOCAL_PATH)/ \
$(LOCAL_PATH)/../ \
$(LOCAL_PATH)/../android \
$(LOCAL_PATH)/../tinyxml \
$(LOCAL_PATH)/../../scripting/lua/lua	\
$(LOCAL_PATH)/../../scripting/lua/tolua \
$(LOCAL_PATH)/../../scripting/lua/cocos2dx_support \
$(LOCAL_PATH)/../../cocos2dx \
$(LOCAL_PATH)/../../cocos2dx/include \
$(LOCAL_PATH)/../../cocos2dx/platform \
$(LOCAL_PATH)/../../cocos2dx/platform/android \
$(LOCAL_PATH)/../../cocos2dx/support/image_support	\
$(LOCAL_PATH)/../../cocos2dx/support/data_support	\
$(LOCAL_PATH)/../../cocos2dx/platform/third_party/android \
$(LOCAL_PATH)/../../cocos2dx/platform/third_party/android/curl \
$(LOCAL_PATH)/../../cocos2dx/platform/third_party/android/iconv
                    
#LOCAL_CPPFLAGS	+=-fexceptions

# define the macro to compile through support/zip_support/ioapi.c                

                                 
#include $(BUILD_SHARED_LIBRARY)
include	$(BUILD_STATIC_LIBRARY)