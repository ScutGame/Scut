LOCAL_PATH := $(call my-dir)
include $(CLEAR_VARS)

LOCAL_CPPFLAGS += -DSCUT_ANDROID
LOCAL_CFLAGS := -Wno-psabi -DUSE_FILE32API
LOCAL_EXPORT_CFLAGS := -Wno-psabi -DUSE_FILE32API
LOCAL_MODULE := scutsdk_static

LOCAL_MODULE_FILENAME := libscutsdk

LOCAL_SRC_FILES := \
../LuaScut.cpp \
../ScutExt.cpp \
../ScutDataLogic/iniEx.cpp \
../ScutDataLogic/LuaIni.cpp \
../ScutDataLogic/DataHandler.cpp \
../ScutDataLogic/DataRequest.cpp \
../ScutDataLogic/FileHelper.cpp \
../ScutDataLogic/IPluginInfo.cpp \
../ScutDataLogic/LuaHost.cpp \
../ScutDataLogic/NetStreamExport.cpp \
../ScutDataLogic/LuaString.cpp \
../ScutDataLogic/NetHelper.cpp \
../ScutDataLogic/Int64.cpp \
../ScutDataLogic/PackageUnzipHandler.cpp \
../ScutDataLogic/XmlDataHandler.cpp \
../ScutNetwork/HttpSession.cpp \
../ScutNetwork/HttpClientResponse.cpp \
../ScutNetwork/HttpClientRequest.cpp \
../ScutNetwork/HttpClient.cpp \
../ScutNetwork/TcpClient.cpp \
../ScutNetwork/NetClientBase.cpp \
../ScutNetwork/LuaHelper.cpp \
../ScutNetwork/TcpSceneManager.cpp \
../ScutNetwork/INetStatusNotify.cpp \
../ScutNetwork/ZipUtils.cpp \
../ScutUtility/android/ScutUtilityJni.cpp \
../ScutUtility/ScutLocale.cpp \
../ScutUtility/ScutUtils.cpp \
../ScutUtility/ScutLuaLan.cpp \
../ScutUtility/PlatformProcedure.cpp \
../ScutUtility/ScutLanFactory.cpp \
../ScutSystem/Stream.cpp \
../ScutSystem/ScutString.cpp \
../ScutSystem/Trace.cpp \
../ScutSystem/AutoGuard.cpp \
../ScutSystem/PathUtility.cpp \
../ScutSystem/ScutUtility.cpp \
../ScutSystem/md5.cpp \
../ScutSystem/WjcDes.cpp \
../ScutSystem/Des91Priv.cpp \
../ScutSystem/StdDES2.cpp \
../ScutSystem/tinyxml/tinystr.cpp \
../ScutSystem/tinyxml/tinyxml.cpp \
../ScutSystem/tinyxml/tinyxmlerror.cpp \
../ScutSystem/tinyxml/tinyxmlparser.cpp

LOCAL_EXPORT_C_INCLUDES := $(LOCAL_PATH) \
                    $(LOCAL_PATH)/include \
                    $(LOCAL_PATH)/kazmath/include \
                    $(LOCAL_PATH)/platform/android

LOCAL_C_INCLUDES := $(LOCAL_PATH)/../ \
$(LOCAL_PATH)/../ScutSystem \
$(LOCAL_PATH)/../ScutSystem/android \
$(LOCAL_PATH)/../ScutSystem/Markup \
$(LOCAL_PATH)/../ScutSystem/tinyxml \
$(LOCAL_PATH)/../ScutDataLogic \
$(LOCAL_PATH)/../ScutNetwork \
$(LOCAL_PATH)/../ScutUtility \
$(LOCAL_PATH)/../ScutUtility/android \
$(LOCAL_PATH)/../../../cocos2d-x/scripting/lua/lua \
$(LOCAL_PATH)/../../../cocos2d-x/scripting/lua/tolua \
$(LOCAL_PATH)/../../../cocos2d-x/scripting/lua/cocos2dx_support \
$(LOCAL_PATH)/../../../cocos2d-x/cocos2dx \
$(LOCAL_PATH)/../../../cocos2d-x/cocos2dx/script_support \
$(LOCAL_PATH)/../../../cocos2d-x/cocos2dx/layers_scenes_transitions_nodes \
$(LOCAL_PATH)/../../../cocos2d-x/cocos2dx/include \
$(LOCAL_PATH)/../../../cocos2d-x/cocos2dx/kazmath/include \
$(LOCAL_PATH)/../../../cocos2d-x/cocos2dx/platform \
$(LOCAL_PATH)/../../../cocos2d-x/cocos2dx/platform/android \
$(LOCAL_PATH)/../../../cocos2d-x/cocos2dx/platform/android/jni \
$(LOCAL_PATH)/../../../cocos2d-x/cocos2dx/support/image_support \
$(LOCAL_PATH)/../../../cocos2d-x/cocos2dx/support/data_support \
$(LOCAL_PATH)/../../../cocos2d-x/cocos2dx/platform/third_party/android \
$(LOCAL_PATH)/../../../cocos2d-x/cocos2dx/platform/third_party/android/prebuilt/libcurl/include/curl \
$(LOCAL_PATH)/../../../cocos2d-x/cocos2dx/platform/third_party/android/prebuilt/libcurl/include \
$(LOCAL_PATH)/../../../cocos2d-x/cocos2dx/platform/third_party/android/iconv

LOCAL_LDLIBS := -llog

include $(BUILD_STATIC_LIBRARY)
