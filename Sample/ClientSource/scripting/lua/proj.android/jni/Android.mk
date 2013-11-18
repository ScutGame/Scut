LOCAL_PATH := $(call my-dir)
include $(CLEAR_VARS)

LOCAL_MODULE    := libcontrols

LOCAL_MODULE_FILENAME := libcontrols

LOCAL_SRC_FILES := ../../cocos2dx_support/CCLuaEngine.cpp \
          ../../cocos2dx_support/CCLuaStack.cpp \
          ../../cocos2dx_support/CCLuaValue.cpp \
          ../../cocos2dx_support/Cocos2dxLuaLoader.cpp \
          ../../cocos2dx_support/LuaCocos2d.cpp \
		  ../../cocos2dx_support/tolua_fix.c \
		  ../../cocos2dx_support/platform/android/CCLuaJavaBridge.cpp \
		  ../../cocos2dx_support/platform/android/org_cocos2dx_lib_Cocos2dxLuaJavaBridge.cpp \
          ../../ScutControls/ImageHelper.cpp \
		  ../../ScutControls/ScutCxList.cpp \
		  ../../ScutControls/ScutCxListItem.cpp \
		  ../../ScutControls/ScutDrawPrimitives.cpp \
		  ../../ScutControls/ScutListLoaderListener.cpp \
		  ../../ScutControls/ScutWebView.cpp \
		  ../../ScutControls/Android/AndroidJni.cpp \
		  ../../ScutControls/Android/AndroidWindow.cpp \
		  ../../ScutControls/Android/ScutEdit_android.cpp
          
LOCAL_EXPORT_C_INCLUDES := $(LOCAL_PATH)/../../lua/jit/include \
                           $(LOCAL_PATH)/../../tolua \
						   $(LOCAL_PATH)/../../lua \
                           $(LOCAL_PATH)/../cocos2dx_support
          
          
LOCAL_C_INCLUDES := $(LOCAL_PATH)/ \
                    $(LOCAL_PATH)/../../lua/jit/include \
					$(LOCAL_PATH)/../../lua \
					$(LOCAL_PATH)/../../ScutControls \
					$(LOCAL_PATH)/../../cocos2dx_support \
					$(LOCAL_PATH)/../../cocos2dx_support/platform/android \
                    $(LOCAL_PATH)/../../tolua \
					$(LOCAL_PATH)/../../../../ScutDataLogic \
                    $(LOCAL_PATH)/../../../../cocos2dx \
					$(LOCAL_PATH)/../../../../cocos2dx/script_support \
					$(LOCAL_PATH)/../../../../cocos2dx/textures \
					$(LOCAL_PATH)/../../../../cocos2dx/draw_nodes \
					$(LOCAL_PATH)/../../../../cocos2dx/base_nodes \
                    $(LOCAL_PATH)/../../../../cocos2dx/include \
                    $(LOCAL_PATH)/../../../../cocos2dx/platform \
                    $(LOCAL_PATH)/../../../../cocos2dx/platform/android \
                    $(LOCAL_PATH)/../../../../cocos2dx/kazmath/include \
                    $(LOCAL_PATH)/../../../../CocosDenshion/include

#LOCAL_WHOLE_STATIC_LIBRARIES := luajit_static
#LOCAL_WHOLE_STATIC_LIBRARIES += cocos_extension_static

LOCAL_CFLAGS += -Wno-psabi
LOCAL_EXPORT_CFLAGS += -Wno-psabi

include $(BUILD_STATIC_LIBRARY)

#$(call import-module,scripting/lua/luajit)
#$(call import-module,extensions)
