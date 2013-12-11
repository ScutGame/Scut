LOCAL_PATH := $(call my-dir)

# compile ndsystem.so
include $(CLEAR_VARS)
LOCAL_MODULE := libScutDataLogic

LOCAL_CPPFLAGS +=	-DSCUT_ANDROID -DUSE_FILE32API -D__STDC_LIMIT_MACROS=1
LOCAL_SRC_FILES := \
../iniEx.cpp \
../LuaIni.cpp \
../DataHandler.cpp \
../DataRequest.cpp \
../FileHelper.cpp \
../IPluginInfo.cpp \
../LuaHost.cpp \
../NetStreamExport.cpp \
../Utility.cpp \
../zip_support/ioapi.cpp \
../zip_support/unzip.cpp \
../zip_support/ZipUtils.cpp \
../LUA_Export/LuaScutDataLogic.cpp \
../LuaString.cpp \
../NetHelper.cpp \
../Int64.cpp \
../PackageUnzipHandler.cpp

LOCAL_LDLIBS := -L$(call host-path, $(LOCAL_PATH)/../../libs/armeabi) -lNdNetwork \
                -lGLESv1_CM -llog -lz \
                -L$(call host-path, $(LOCAL_PATH)/../../cocos2dx/platform/third_party/android/libraries) -liconv

LOCAL_C_INCLUDES := $(LOCAL_PATH)/ \
$(LOCAL_PATH)/../ \
$(LOCAL_PATH)/../zip_support \
$(LOCAL_PATH)/../LUA_Export \
$(LOCAL_PATH)/../../ScutSystem \
$(LOCAL_PATH)/../../ScutSystem/android \
$(LOCAL_PATH)/../../ScutSystem/tinyxml \
$(LOCAL_PATH)/../../ScutNetwork \
$(LOCAL_PATH)/../../ScutUtility \
$(LOCAL_PATH)/../../scripting/lua/lua	\
$(LOCAL_PATH)/../../scripting/lua/tolua \
$(LOCAL_PATH)/../../scripting/lua/cocos2dx_support \
$(LOCAL_PATH)/../../cocos2dx \
$(LOCAL_PATH)/../../cocos2dx/layers_scenes_transitions_nodes \
$(LOCAL_PATH)/../../cocos2dx/include \
$(LOCAL_PATH)/../../cocos2dx/kazmath/include \
$(LOCAL_PATH)/../../cocos2dx/platform \
$(LOCAL_PATH)/../../cocos2dx/platform/android \
$(LOCAL_PATH)/../../cocos2dx/support/image_support	\
$(LOCAL_PATH)/../../cocos2dx/support/data_support	\
$(LOCAL_PATH)/../../cocos2dx/platform/third_party/android \
$(LOCAL_PATH)/../../cocos2dx/platform/third_party/android/prebuilt/libcurl/include/curl \
$(LOCAL_PATH)/../../cocos2dx/platform/third_party/android/iconv

#LOCAL_CPPFLAGS	+=-fexceptions

# define the macro to compile through support/zip_support/ioapi.c                

                                 
#include $(BUILD_SHARED_LIBRARY)
include	$(BUILD_STATIC_LIBRARY)