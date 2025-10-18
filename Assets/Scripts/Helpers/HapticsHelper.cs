using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles haptics behaviour with static methods
/// </summary>

namespace NamPhuThuy
{
    public class HapticsHelper : MonoBehaviour
    {
        public static int vibrationFx
        {
            get { return PlayerPrefs.GetInt("vibrationFx", 1); }
            set { PlayerPrefs.SetInt("vibrationFx", value); }
        }
        
        #region Haptic
    #if UNITY_ANDROID && !UNITY_EDITOR
        public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
    #else
        public static AndroidJavaClass unityPlayer;
        public static AndroidJavaObject currentActivity;
        public static AndroidJavaObject vibrator;
    #endif
    
        public static void Vibrate(long milliseconds = 25)
        {
            if (vibrationFx == 0)
                return;
    
            if (IsAndroid())
            {
                vibrator.Call("vibrate", milliseconds);
                Debug.Log("vibrate andoroid: " + milliseconds);
            }
            else
            {
                // Handheld.Vibrate();
                Debug.Log("vibrate else: " + milliseconds);
            }
        }
    
        public static void LowVibrate()
        {
            Vibrate(35);
        }
    
        public static void MediumVibrate()
        {
            Vibrate(50);
        }
    
        public static void HardVibrate()
        {
            Vibrate(100);
        }
    
        public static void Cancel()
        {
            if (IsAndroid())
            {
                vibrator.Call("cancel");
            }
        }
    
        public static bool IsAndroid()
        {
    #if UNITY_ANDROID && !UNITY_EDITOR
            return true;
    #else
            return false;
    #endif
        }
        #endregion
    }
}
