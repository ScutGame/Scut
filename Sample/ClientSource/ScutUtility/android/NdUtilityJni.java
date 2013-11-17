package com.nd.lib;

import java.io.File;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;

import android.R.string;
import android.app.AlarmManager;
import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.pm.ActivityInfo;
import android.content.pm.ApplicationInfo;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.content.pm.PackageManager.NameNotFoundException;
import android.net.Uri;
import android.os.Bundle;
import android.os.Environment;
import android.telephony.TelephonyManager;
import android.text.ClipboardManager;
import android.util.Log;

import com.nd.application.R;

public class NdUtilityJni {
	
	public static final String EXTR_STRING = "cn.com.nd.jni.extr_key_string";
	public static final String INTENT_SHOW_WEB_VIEW = "org.cocos2dx.lib.jni.show_webview";
	private static int mAlarmId = 0;

	/**
	 * @param args
	 */
	/*
	 * 锟斤拷锟斤拷锟揭拷玫锟絀MSI锟接匡拷  锟斤拷要锟斤拷Activity锟叫碉拷锟斤拷 initJni锟斤拷锟斤拷锟斤拷锟叫筹拷始锟斤拷
	 */
	private static Context mContext = null;
	public static String getLanguage()
	{
		return Locale.getDefault().getCountry();
	}
	public static int isSDCardExist()
	{
		int nRet = 0;
		if(Environment.getExternalStorageState().equals(Environment.MEDIA_MOUNTED))
		{
			nRet = 1;
		}
		return nRet;
	}
	public static String getImsi()
	{
		String IMSI = new String();
		if(mContext != null)
		{
			TelephonyManager gTeleManager = (TelephonyManager)mContext.getSystemService(Context.TELEPHONY_SERVICE);
			
			String IMEI = new String();;
			if (gTeleManager.getSimState() == TelephonyManager.SIM_STATE_READY) {
				IMSI = gTeleManager.getSubscriberId();
				//IMSI ="136380234553560";
				IMEI = gTeleManager.getDeviceId();
				if(IMSI == null || IMSI.length()<15){
					IMSI=IMEI;
				}
			}
			
		}
		return IMSI;
	}
	
	public static String getImei()
	{
		String IMEI = new String();
		if(mContext != null)
		{
			TelephonyManager gTeleManager = (TelephonyManager)mContext.getSystemService(Context.TELEPHONY_SERVICE);

			IMEI = gTeleManager.getDeviceId();
		}
		return IMEI;
	}
	
	//public static native void initTest();
	public static void initJni(Context con)
	{
		mContext = con;
	}
	
	public static void startActivity(String action, String extra) {
		Log.i("startActivity java", action);
		Intent i = new Intent();
		i.setAction(action);
		i.putExtra(EXTR_STRING, extra);
		mContext.startActivity(i);
	}

	public static void localNotification(
			int iconResId , 
			String msgTitle , 
			String msgContent, 
			int notifyFlag, 
			int intentFlag , 
			long when
			) 
	{
		if (mContext != null)
		{
			try {
				NotificationManager nm = (NotificationManager) mContext.getSystemService(Context.NOTIFICATION_SERVICE); 
				Notification notification = new Notification(iconResId, msgTitle, when);
				Intent intent = new Intent(mContext, Class.forName("com.nd.application.MainApplication"));
				intent.setFlags(intentFlag);
				PendingIntent pendingIntent = PendingIntent.getActivity(mContext, 0, intent, 0);
				long[] vibrate = {1000, 1000, 1000, 1000, 1000, 1000, 1000};
				notification.defaults |= Notification.DEFAULT_VIBRATE;
				notification.vibrate = vibrate;
				notification.setLatestEventInfo(mContext, msgTitle, msgContent, pendingIntent); 
				notification.flags |= notifyFlag;
				nm.notify(1, notification);
			} 
			catch (Exception e) {
				Log.e("JNIMsg", e.toString());
			}
		}
	}

	
	//本地消息通知功能
	//	soundName	：声音文件路径
	//	iconResId	：显示在消息上图标资源的id
	//	msgTitle	：消息标题
	//	msgContent	：消息内容
	//	when		：显示在消息末尾的时间戳，如System.currentTimeMillis()
	//	hasAction	：是否振动
	//	repeatInterval	：提示间隔（暂无）
	public static void scheduleLocalNotification(
			String soundName,
			int iconResId , 
			String msgTitle , 
			String msgContent, 
			long when,
			boolean hasAction,
			int repeatInterval
			) 
	{
		Log.i("JNIMsg", "enter scheduleLocalNotification");
		if (mContext != null)
		{		
			Log.i("JNIMsg", "mContext != null");

			Intent alarmIntent = new Intent(mContext.getApplicationContext(), RepeatNotificationReceiver.class);
			alarmIntent.putExtra("soundName", soundName);
			alarmIntent.putExtra("msgTitle", msgTitle);
			alarmIntent.putExtra("msgContent", msgContent);
			alarmIntent.putExtra("hasAction", hasAction);
			alarmIntent.putExtra("iconResId", iconResId);		
			alarmIntent.putExtra("packageName", mContext.getPackageName());
			int id = ++mAlarmId;//(int)System.currentTimeMillis();
			alarmIntent.putExtra("id", id);
			PendingIntent mPendingIntent = PendingIntent.getBroadcast(mContext.getApplicationContext(), id, alarmIntent, PendingIntent.FLAG_UPDATE_CURRENT);
			
			AlarmManager mAlarmManager = (AlarmManager)mContext.getSystemService(Context.ALARM_SERVICE);
			mAlarmManager.setRepeating(AlarmManager.RTC_WAKEUP, when, repeatInterval, mPendingIntent);
		}
		Log.i("JNIMsg", "end scheduleLocalNotification");		
	}
	
	public static class RepeatNotificationReceiver extends BroadcastReceiver {
		@Override
		public void onReceive(Context mContext, Intent srcIntent) {
			// TODO Auto-generated method stub
			Log.i("JNIMsg", "RepeatNotificationReceiver.onReceive()");
			if (mContext == null) {
				Log.i("JNIMsg", "current context is null!!");
				return;
			}
			if (srcIntent == null) {
				Log.i("JNIMsg", "current intent is null!!");
				return;
			}
						
			boolean hasAction = srcIntent.getBooleanExtra("hasAction", true);
			String soundName = srcIntent.getStringExtra("soundName");
			String msgTitle = srcIntent.getStringExtra("msgTitle");
			String msgContent = srcIntent.getStringExtra("msgContent");
			int iconResId = srcIntent.getIntExtra("iconResId", 1);
			String srcPackageName = srcIntent.getStringExtra("packageName");
			int id = srcIntent.getIntExtra("id", 1);
			iconResId = R.drawable.icon;

			Log.i("JNIMsg", "mContext.getPackageManager()");
			
            String packageName = mContext.getApplicationContext().getPackageName();
        	Log.i("JNIMsg", "current package name = " + packageName);
        	Log.i("JNIMsg", "source package name = " + srcPackageName);
            if (srcPackageName != null && !packageName.toLowerCase().equals(srcPackageName.toLowerCase())) {
            	Log.i("JNIMsg", "package not equal!");
				return;
			} 
            
            String mainActivityName = "MainApplication";
            PackageManager pm = mContext.getPackageManager();  
            PackageInfo info = pm.getPackageArchiveInfo(mContext.getPackageCodePath(), PackageManager.GET_ACTIVITIES);  
            if(info != null){  
                ActivityInfo[] activityInfos = info.activities;  
                if (activityInfos.length == 1) {
                	mainActivityName = activityInfos[0].name;
                	Log.i("JNIMsg", "activityInfos.length == 1");
				}
                else {
                	for (ActivityInfo activityInfo : activityInfos) {
                        if (activityInfo.labelRes == R.string.app_name) {
                        	mainActivityName = activityInfo.name;
                        	break;
        				}
					}
				}
                
                Log.i("JNIMsg", "mainActivityName=" + mainActivityName);
            }  
			
			String className = mainActivityName;
			Log.i("JNIMsg", "ClassName=" + className);

			try {
				Log.i("JNIMsg", "msgTitle=" + msgTitle);	

				int notifyFlag = Notification.FLAG_AUTO_CANCEL; 
				int intentFlag = Notification.FLAG_AUTO_CANCEL;
				NotificationManager nm = (NotificationManager) mContext.getSystemService(Context.NOTIFICATION_SERVICE); 
				Notification notification = new Notification(iconResId, msgTitle, System.currentTimeMillis());
				
				if (soundName != null && soundName.trim() != "") {
					Log.i("JNIMsg", "soundName=" + soundName);
					notification.defaults |= Notification.DEFAULT_SOUND; 
					notification.audioStreamType = android.media.AudioManager.ADJUST_RAISE;
					notification.sound = Uri.fromFile(new File(soundName)); 
				}
				
				if (hasAction) 
				{
					notification.defaults |= Notification.DEFAULT_VIBRATE; 
					long[] vibrate = {1000,1000,1000,1000,1000}; 
					notification.vibrate = vibrate; 
				}
				
				Log.i("Java", "Before Class.forName(className)");
				Intent intent = new Intent(mContext, Class.forName(className));
				Log.i("Java", "After Class.forName(className)");
				intent.setFlags(intentFlag);
				PendingIntent pendingIntent = PendingIntent.getActivity(mContext, 0, intent, 0);
				notification.setLatestEventInfo(mContext, msgTitle, msgContent, pendingIntent); 
				notification.flags |= notifyFlag;
				nm.notify(id, notification);//如果希望新的提示直接覆盖上一条提示的话，只需要将id改成1
			} 
			catch (Exception e) {
				Log.e("JNIMsg", e.toString());
			}			
		}
	}
	
	//取消所有消息通知
	public static void cancelAllNotifications() {
		Log.i("JNIMsg", "enter cancelAllNotifications");
		if (mContext != null)
		{		
			Log.i("JNIMsg", "mContext != null");
			try {
				NotificationManager nm = (NotificationManager) mContext.getSystemService(Context.NOTIFICATION_SERVICE); 
				nm.cancelAll();				

				Intent alarmIntent = new Intent(mContext.getApplicationContext(), RepeatNotificationReceiver.class);
				AlarmManager mAlarmManager = (AlarmManager)mContext.getSystemService(Context.ALARM_SERVICE);
				for (int i = 1; i <= 10; i++)
				{
					PendingIntent mPendingIntent = PendingIntent.getBroadcast(mContext.getApplicationContext(), i, alarmIntent, 0);
					if (mPendingIntent != null) {
						mAlarmManager.cancel(mPendingIntent);
					}
				}
				mAlarmId = 0;
			} 
			catch (Exception e) {
				Log.e("JNIMsg", e.toString());
			}
		}
		Log.i("JNIMsg", "end cancelAllNotifications");	
	}
	
	//取消指定id的消息通知
	public static void cancelNotification(int id) {
		Log.i("JNIMsg", "enter cancelNotification");
		if (mContext != null)
		{		
			Log.i("JNIMsg", "mContext != null");
			try {
				NotificationManager nm = (NotificationManager) mContext.getSystemService(Context.NOTIFICATION_SERVICE); 
				nm.cancel(id);				

				Intent alarmIntent = new Intent(mContext.getApplicationContext(), RepeatNotificationReceiver.class);
				PendingIntent mPendingIntent = PendingIntent.getBroadcast(mContext.getApplicationContext(), id, alarmIntent, 0);
				if (mPendingIntent != null) {
					AlarmManager mAlarmManager = (AlarmManager)mContext.getSystemService(Context.ALARM_SERVICE);
					mAlarmManager.cancel(mPendingIntent);
				}
			} 
			catch (Exception e) {
				Log.e("JNIMsg", e.toString());
			}
		}
		Log.i("JNIMsg", "end cancelNotification");	
	}
	
	//复制文本到剪贴板
	public static void copyTextToClipBoard(String content) {
		if (mContext == null) {
			return ;
		}
        final ClipboardManager myClipBoard = (ClipboardManager) mContext.getSystemService(mContext.CLIPBOARD_SERVICE);
        myClipBoard.setText(content);
	}
	
	//从剪贴板复制文本
	public static String getTextFromClipBoard()
	{
		if (mContext == null) {
			return null;
		}
        final ClipboardManager myClipBoard = (ClipboardManager) mContext.getSystemService(mContext.CLIPBOARD_SERVICE);
        CharSequence cs = myClipBoard.getText();
        return cs == null ? null : cs.toString();
	}

    //启动app：packageName--包名
	public static void launchApp(String packageName) {
		if (mContext != null) {
	    	Intent intent = mContext.getPackageManager().getLaunchIntentForPackage(packageName);
	    	if(intent != null) 
	    	{
	    		mContext.startActivity(intent);
	    	}
		}
	}

	//安装apk包： apkFilePath--apk包路径
    public static void installPackage(String apkFilePath) {
		if (mContext != null) {
	    	Intent intent = new Intent(Intent.ACTION_VIEW);
	    	intent.setDataAndType(Uri.fromFile(new File(apkFilePath)), "application/vnd.android.package-archive");
	    	mContext.startActivity(intent);
		}
	}
 
    private static List<PackageInfo> mInstalledPackagesInfos = null;
    private static List<PackageInfo> getUserInstalledPackages() {
    	if (mContext != null) {
            PackageManager manager = mContext.getPackageManager();
    		List<PackageInfo> infos = manager.getInstalledPackages(0);
    		List<PackageInfo> retInfos = new ArrayList<PackageInfo>();
    		for (PackageInfo packageInfo : infos) {
    			if ((packageInfo.applicationInfo.flags & ApplicationInfo.FLAG_UPDATED_SYSTEM_APP) != 0 
    					|| (packageInfo.applicationInfo.flags & ApplicationInfo.FLAG_SYSTEM) != 0)
    			{
    				continue;
    			}
    			retInfos.add(packageInfo);
    		}
    		
    		return retInfos;
		}
    	
    	return null;
	}
    
    //查看App是否已安装：packageName--包名，bForceUpdate--是否强制刷新
    public static boolean checkAppInstalled(String packageName, boolean bForceUpdate) {
		if (bForceUpdate) {
			if (mInstalledPackagesInfos != null) {
				mInstalledPackagesInfos.clear();
				mInstalledPackagesInfos = null;
			}
		}
		
		if (mInstalledPackagesInfos == null) {			
			mInstalledPackagesInfos = getUserInstalledPackages();
		}
		
		if (mInstalledPackagesInfos != null && mInstalledPackagesInfos.size() > 0) {
			for (PackageInfo packageInfo : mInstalledPackagesInfos) {
				Log.d("Installed App Name", packageInfo.packageName + " , " + packageInfo.versionName);
				if (packageInfo.packageName.equals(packageName)) {
					Log.d("find app:", packageInfo.packageName + " , " + packageInfo.versionName);
					return true;
					//break;
				}
			}
		}
		
		return false;
	}
	
	public static String getInstalledApps() {		
		StringBuilder strAppsBuilder = new StringBuilder();	
        List<PackageInfo> apps = getUserInstalledPackages();
        
		if (apps != null && apps.size() > 0) {
			for (PackageInfo packageInfo : apps) {
				Log.d("Installed App Name", packageInfo.packageName + " , " + packageInfo.versionName);
				strAppsBuilder.append(packageInfo.packageName);
				strAppsBuilder.append("_");
			}
		}
        
        if (strAppsBuilder.length() > 0)
        {
			return strAppsBuilder.toString();
        }
        
        return "";
	}
	
	public static class ManifestMetaData {

		private static Object readKey(Context context, String keyName) {
			try {	
				ApplicationInfo appi =  context.getPackageManager().getApplicationInfo(context.getPackageName(), PackageManager.GET_META_DATA);	
				Bundle bundle = appi.metaData;	
				Object value = bundle.get(keyName);	
				return value;	
			} catch (NameNotFoundException e) {	
				return null;	
			}
		}

		public static int getInt(Context context, String keyName) {
			return (Integer)readKey(context, keyName);
		}


		public static String getString(Context context, String keyName ) {
			return (String ) readKey(context, keyName);
		}
		
		public static Boolean getBoolean(Context context, String keyName) {
			return (Boolean) readKey(context, keyName);
		}

		public static Object get(Context context, String keyName) {
			return readKey(context, keyName);
		}
	}
	
}
