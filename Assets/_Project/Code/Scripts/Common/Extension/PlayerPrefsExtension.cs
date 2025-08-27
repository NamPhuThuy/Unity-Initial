using System;
using System.Collections;
using System.Collections.Generic;
using NamPhuThuy.Common;
using UnityEngine;

namespace NamPhuThuy.Common
{
    public static class PlayerPrefsExtension
    {
        public static void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        public static bool GetBool(string key, bool defaultValue)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }

        public static void SetDateTime(string key, DateTime value)
        {
            var json = JsonUtility.ToJson((JsonDateTime)value);
            PlayerPrefs.SetString(key, json);
            DateTime test = JsonUtility.FromJson<JsonDateTime>(json);
        }
        public static DateTime GetDateTime(string key, DateTime defaultValue)
        {
            string json = PlayerPrefs.GetString(key, "");
            if (string.IsNullOrEmpty(json))
            {
                return defaultValue;
            }
            DateTime value = JsonUtility.FromJson<JsonDateTime>(json);
            return value;
        }

        public static void SetVector3(string key, Vector3 vect)
        {
            PlayerPrefs.SetFloat($"{key}_x", vect.x);
            PlayerPrefs.SetFloat($"{key}_y", vect.y);
            PlayerPrefs.SetFloat($"{key}_z", vect.z);
        }

        public static Vector3 GetVector3(string key, Vector3 defaultVector)
        {
            float x = PlayerPrefs.GetFloat($"{key}_x", defaultVector.x);
            float y = PlayerPrefs.GetFloat($"{key}_y", defaultVector.y);
            float z = PlayerPrefs.GetFloat($"{key}_z", defaultVector.z);
            return new Vector3(x, y, z);
        }

        public static void SetValue<T>(string key, T value)
        {
            string saveStr = JsonUtility.ToJson(value);
            PlayerPrefs.SetString(key, saveStr);
        }

        public static T GetValue<T>(string key, T defaultValue)
        {
            string savedStr = PlayerPrefs.GetString(key, "");
            if (string.IsNullOrEmpty(savedStr))
            {
                return defaultValue;
            }
            else
            {
                return JsonUtility.FromJson<T>(savedStr);
            }
        }

        public static Color GetColor(string key, Color defaultValue)
        {
            string hexaCode = PlayerPrefs.GetString(key, "");
            return ColorExtension.ToColor(hexaCode, defaultValue);
        }

        public static void SetColor(string key, Color value)
        {
            PlayerPrefs.SetString(key, value.ToHexaCode());
        }
    }
    [Serializable]
    struct JsonDateTime
    {
        public long value;
        public static implicit operator DateTime(JsonDateTime jdt)
        {
            return DateTime.FromBinary(jdt.value);
        }
        public static implicit operator JsonDateTime(DateTime dt)
        {
            JsonDateTime jdt = new JsonDateTime();
            jdt.value = dt.ToBinary();
            return jdt;
        }
    }
}