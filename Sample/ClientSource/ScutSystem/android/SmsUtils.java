package com.nd.lib;

import java.util.ArrayList;

import android.app.PendingIntent;
import android.os.Build;

@SuppressWarnings("deprecation")
public class SmsUtils {
	
	public static boolean sendMsg(String msg, String tels) {
		if (msg == null || tels == null) {
			return false;
		}
		ArrayList<String> msgs = divideMessage(msg);
		String[] telphons = tels.split(",");
		
		for (int i = 0; i < telphons.length; i++) {
			//Log.e(TAG, "send:" + msgs + "tel:" + telphons[i]);
			sendNultipartSms(telphons[i], msgs, null);
		}
		return true;
	}
	
	public static boolean sendSms(String phone, String msg, PendingIntent pi) {
		try {
			if (isNormalVersion()) {
				Sms.instance.sendSms(phone, msg, pi);
			} else {
				android.telephony.gsm.SmsManager manager = android.telephony.gsm.SmsManager
						.getDefault();
				manager.sendTextMessage(phone, null, msg, pi, null);
			}
		} catch (Throwable e) {
			e.printStackTrace();
			try {
				android.telephony.gsm.SmsManager manager = android.telephony.gsm.SmsManager
						.getDefault();
				manager.sendTextMessage(phone, null, msg, pi, null);
			} catch (Throwable t) {
				t.printStackTrace();
				return false;
			}
		}
		return true;
	}

	public static ArrayList<String> divideMessage(String text) {
		try {
			if (isNormalVersion()) {
				return Sms.instance.dividerMessage(text);
			} else {
				android.telephony.gsm.SmsManager manager = android.telephony.gsm.SmsManager
				.getDefault();
				return manager.divideMessage(text);
			}
		} catch(Throwable e) {
			try {
				android.telephony.gsm.SmsManager manager = android.telephony.gsm.SmsManager
				.getDefault();
				return manager.divideMessage(text);
				
			}catch(Throwable t) {
				t.printStackTrace();
				return null;
			}
		}
	}

	public static boolean sendNultipartSms(String phone,
			ArrayList<String> msgs, ArrayList<PendingIntent> pis) {
		try {
			if (isNormalVersion()) {
				Sms.instance.sendNultipartSms(phone, msgs, pis);
			} else {
				android.telephony.gsm.SmsManager manager = android.telephony.gsm.SmsManager
						.getDefault();
				manager.sendMultipartTextMessage(phone, null, msgs, pis, null);
			}
		} catch (Throwable e) {
			e.printStackTrace();
			try {
				android.telephony.gsm.SmsManager manager = android.telephony.gsm.SmsManager
						.getDefault();
				manager.sendMultipartTextMessage(phone, null, msgs, pis, null);
			} catch (Throwable t) {
				t.printStackTrace();
				return false;
			}
		}
		return true;
	}

	private static class Sms {
		public static final Sms instance = new Sms();

		public void sendSms(String phone, String msg, PendingIntent pi) {
			
			android.telephony.SmsManager sm = android.telephony.SmsManager
					.getDefault();
			sm.sendTextMessage(phone, null, msg, pi, null);
		}

		public ArrayList<String> dividerMessage(String text) {
			return android.telephony.SmsManager.getDefault()
					.divideMessage(text);
		}

		public void sendNultipartSms(String phone, ArrayList<String> msgs,
				ArrayList<PendingIntent> pi) {
			android.telephony.SmsManager sm = android.telephony.SmsManager
					.getDefault();
			sm.sendMultipartTextMessage(phone, null, msgs, pi, null);
		}
	}
	
	public static boolean isNormalVersion() {
		try {
			 int VERSION = Integer.valueOf(Build.VERSION.SDK);
			 return VERSION > 3;
		}catch (Exception e) {
			// TODO: handle exception
			return false;
		}
	}
	
}
