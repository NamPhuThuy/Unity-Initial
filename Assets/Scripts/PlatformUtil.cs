using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace NamPhuThuy.Common
{
    public static class PlatformUtil
    {

#if UNITY_IOS
    private static readonly Dictionary<string, int> s_devicePpiMapping = new Dictionary<string, int>
{
    // iPhone models
    { "iPhone1,1", 163 },  // iPhone 1
    { "iPhone1,2", 163 },  // iPhone 3G
    { "iPhone2,1", 163 },  // iPhone 3GS
    { "iPhone3,1", 326 },  // iPhone 4
    { "iPhone3,2", 326 },  // iPhone 4
    { "iPhone3,3", 326 },  // iPhone 4 (CDMA)
    { "iPhone4,1", 326 },  // iPhone 4S
    { "iPhone5,1", 326 },  // iPhone 5
    { "iPhone5,2", 326 },  // iPhone 5
    { "iPhone5,3", 326 },  // iPhone 5C
    { "iPhone5,4", 326 },  // iPhone 5C
    { "iPhone6,1", 326 },  // iPhone 5S
    { "iPhone6,2", 326 },  // iPhone 5S
    { "iPhone7,1", 401 },  // iPhone 6 Plus
    { "iPhone7,2", 326 },  // iPhone 6
    { "iPhone8,1", 326 },  // iPhone 6S
    { "iPhone8,2", 401 },  // iPhone 6S Plus
    { "iPhone8,4", 326 },  // iPhone SE (1st generation)
    { "iPhone9,1", 326 },  // iPhone 7
    { "iPhone9,2", 401 },  // iPhone 7 Plus
    { "iPhone9,3", 326 },  // iPhone 7
    { "iPhone9,4", 401 },  // iPhone 7 Plus
    { "iPhone10,1", 326 }, // iPhone 8
    { "iPhone10,2", 401 }, // iPhone 8 Plus
    { "iPhone10,3", 458 }, // iPhone X
    { "iPhone10,4", 326 }, // iPhone 8
    { "iPhone10,5", 401 }, // iPhone 8 Plus
    { "iPhone10,6", 458 }, // iPhone X
    { "iPhone11,2", 458 }, // iPhone XS
    { "iPhone11,4", 458 }, // iPhone XS Max
    { "iPhone11,6", 458 }, // iPhone XS Max
    { "iPhone11,8", 326 }, // iPhone XR
    { "iPhone12,1", 326 }, // iPhone 11
    { "iPhone12,3", 458 }, // iPhone 11 Pro
    { "iPhone12,5", 458 }, // iPhone 11 Pro Max
    { "iPhone12,8", 326 }, // iPhone SE (2nd generation)
    { "iPhone13,1", 476 }, // iPhone 12 mini
    { "iPhone13,2", 460 }, // iPhone 12
    { "iPhone13,3", 460 }, // iPhone 12 Pro
    { "iPhone13,4", 458 }, // iPhone 12 Pro Max
    { "iPhone14,2", 460 }, // iPhone 13 Pro
    { "iPhone14,3", 458 }, // iPhone 13 Pro Max
    { "iPhone14,4", 476 }, // iPhone 13 mini
    { "iPhone14,5", 460 }, // iPhone 13
    { "iPhone14,6", 326 }, // iPhone SE (3rd generation)
    { "iPhone14,7", 460 }, // iPhone 14
    { "iPhone14,8", 476 }, // iPhone 14 Plus
    { "iPhone15,2", 460 }, // iPhone 14 Pro
    { "iPhone15,3", 460 }, // iPhone 14 Pro Max
    { "iPhone15,4", 460 }, // iPhone 15
    { "iPhone15,5", 460 }, // iPhone 15 Plus
    { "iPhone16,1", 460 }, // iPhone 15 Pro
    { "iPhone16,2", 460 }, // iPhone 15 Pro Max
    { "iPhone17,1", 460 }, // iPhone 16 Pro
    { "iPhone17,2", 460 }, // iPhone 16 Pro Max
    { "iPhone17,3", 460 }, // iPhone 16
    { "iPhone17,4", 460 }, // iPhone 16 Plus

    // iPad models
    { "iPad1,1", 132 },    // iPad 1
    { "iPad2,1", 132 },    // iPad 2
    { "iPad2,2", 132 },    // iPad 2
    { "iPad2,3", 132 },    // iPad 2
    { "iPad2,4", 132 },    // iPad 2
    { "iPad2,5", 163 },    // iPad mini gen 1
    { "iPad3,1", 264 },    // iPad 3
    { "iPad3,2", 264 },    // iPad 3
    { "iPad3,3", 264 },    // iPad 3
    { "iPad3,4", 264 },    // iPad 4
    { "iPad3,5", 264 },    // iPad 4
    { "iPad3,6", 264 },    // iPad 4
    { "iPad4,1", 264 },    // iPad Air
    { "iPad4,2", 264 },    // iPad Air
    { "iPad4,3", 264 },    // iPad Air
    { "iPad5,1", 264 },    // iPad mini 4
    { "iPad5,2", 264 },    // iPad mini 4
    { "iPad6,3", 264 },    // iPad Pro 9.7
    { "iPad6,4", 264 },    // iPad Pro 9.7
    { "iPad6,7", 264 },    // iPad Pro 12.9
    { "iPad6,8", 264 },    // iPad Pro 12.9
    { "iPad6,11", 264 },   // iPad 5
    { "iPad6,12", 264 },   // iPad 5
    { "iPad7,1", 264 },    // iPad Pro 12.9 2nd Gen
    { "iPad7,2", 264 },    // iPad Pro 12.9 2nd Gen
    { "iPad7,3", 264 },    // iPad Pro 10.5
    { "iPad7,4", 264 },    // iPad Pro 10.5
    { "iPad7,5", 264 },    // iPad 6
    { "iPad7,6", 264 },    // iPad 6
    { "iPad8,1", 264 },    // iPad Pro 11 (1st Gen)
    { "iPad8,2", 264 },    // iPad Pro 11 (1st Gen)
    { "iPad8,3", 264 },    // iPad Pro 11 (1st Gen)
    { "iPad8,4", 264 },    // iPad Pro 11 (1st Gen)
    { "iPad8,5", 264 },    // iPad Pro 12.9 (3rd Gen)
    { "iPad8,6", 264 },    // iPad Pro 12.9 (3rd Gen)
    { "iPad8,7", 264 },    // iPad Pro 12.9 (3rd Gen)
    { "iPad8,8", 264 },    // iPad Pro 12.9 (3rd Gen)
    { "iPad8,9", 264 },    // iPad Pro 11 (2nd Gen)
    { "iPad8,10", 264 },   // iPad Pro 11 (2nd Gen)
    { "iPad8,11", 264 },   // iPad Pro 12.9 (4th Gen)
    { "iPad8,12", 264 },   // iPad Pro 12.9 (4th Gen)
    { "iPad11,1", 264 },   // iPad mini (5th Gen)
    { "iPad11,2", 264 },   // iPad mini (5th Gen)
    { "iPad11,3", 264 },   // iPad Air (3rd Gen)
    { "iPad11,4", 264 },   // iPad Air (3rd Gen)
    { "iPad13,1", 264 },   // iPad Air (4th Gen)
    { "iPad13,2", 264 },   // iPad Air (4th Gen)
    { "iPad13,4", 264 },   // iPad Pro 11 (3rd Gen)
    { "iPad13,5", 264 },   // iPad Pro 11 (3rd Gen)
    { "iPad13,6", 264 },   // iPad Pro 11 (3rd Gen)
    { "iPad13,7", 264 },   // iPad Pro 11 (3rd Gen)
    { "iPad13,8", 264 },   // iPad Pro 12.9 (5th Gen)
    { "iPad13,9", 264 },   // iPad Pro 12.9 (5th Gen)
    { "iPad13,10", 264 },  // iPad Pro 12.9 (5th Gen)
    { "iPad13,11", 264 },  // iPad Pro 12.9 (5th Gen)
    { "iPad14,1", 264 },   // iPad mini (6th Gen)
    { "iPad14,3", 264 },   // iPad Air (5th Gen)
    { "iPad14,4", 264 },   // iPad Air (5th Gen)
    { "iPad14,2", 264 },   // iPad Air (5th Gen)
    { "iPad14,6", 264 },   // iPad Air (5th Gen)
    { "iPad14,7", 264 },   // iPad Air (5th Gen)
    { "iPad14,8", 264 },   // iPad Air (5th Gen)
    { "iPad12,1", 264 },   // iPad (9th Gen)
    { "iPad12,2", 264 },   // iPad (9th Gen)
    
    // iPod models
    { "iPod1,1", 163 },    // iPod touch 1
    { "iPod2,1", 163 },    // iPod touch 2
    { "iPod3,1", 163 },    // iPod touch 3
    { "iPod4,1", 326 },    // iPod touch 4
    { "iPod5,1", 326 },    // iPod touch 5
    { "iPod7,1", 326 },    // iPod touch 6
    { "iPod9,1", 326 },    // iPod touch 7
};
    [DllImport("__Internal")]
    public static extern void NativeOpenAppSettings();
    
    [DllImport("__Internal")]
    public static extern void PauseGame(bool pause);

    [DllImport("__Internal")]
    public static extern int GetSafeAreaTopPadding();
    
    [DllImport("__Internal")]
    public static extern int GetSafeAreaBottomPadding();

    [DllImport("__Internal")]
    public static extern bool VpnChecker_isVpnActive();
#endif

#if UNITY_ANDROID
        private static string PLATFORM_UTIL_CLASS_NAME = "com.libhub.netzero.unity.PlatformUtil";
        private static string IS_VPN_ACTIVE_METHOD_NAME = "isVpnActive"; 
        private static AndroidJavaClass s_platformUtilClass;
#endif

        public static void OpenAppSettings()
        {
#if !UNITY_EDITOR
#if UNITY_ANDROID
        try 
        {
            using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivityObject = unityClass.GetStatic<AndroidJavaObject>("currentActivity")) 
            {                
                using (var uriClass = new AndroidJavaClass("android.net.Uri"))
                using (AndroidJavaObject uriObject =
 uriClass.CallStatic<AndroidJavaObject>("fromParts", "package", Application.identifier, null))
                using (var intentObject =
 new AndroidJavaObject("android.content.Intent", "android.settings.SETTINGS", uriObject)) 
                {
                    intentObject.Call<AndroidJavaObject>("addCategory", "android.intent.category.DEFAULT");
                    intentObject.Call<AndroidJavaObject>("setFlags", 0x10000000);
                    currentActivityObject.Call("startActivity", intentObject);
                }
            }
        } catch (Exception e) 
        {
            Debug.Log(e);
        }
#elif UNITY_IOS
        NativeOpenAppSettings();
#endif
#endif
        }

        public static int getDpi()
        {
#if UNITY_IOS
        string deviceModel = SystemInfo.deviceModel;
        if (s_devicePpiMapping.ContainsKey(deviceModel)) 
        {
            return s_devicePpiMapping[deviceModel];
        }
#endif
            return (int)Screen.dpi;
        }

        public static float GetDeviceScale()
        {
#if UNITY_ANDROID
            return Screen.dpi / 160f;
#else
            return Mathf.Round(getDpi() / 160f);
#endif
        }

        /// <summary>
        /// Check if the VPN is active on the device.
        /// VPNs may affect data for analytics 
        /// </summary>
        /// <returns></returns>
        public static bool IsVpnActive()
        {
            bool ret = false;
#if !UNITY_EDITOR
    #if UNITY_ANDROID
        if (s_platformUtilClass == null) 
        {
            s_platformUtilClass = new AndroidJavaClass(PLATFORM_UTIL_CLASS_NAME);
        } 
        ret = s_platformUtilClass.CallStatic<bool>(IS_VPN_ACTIVE_METHOD_NAME);
    #elif UNITY_IOS
        ret = VpnChecker_isVpnActive();
    #endif
#endif
            return ret;
        }
    }

}