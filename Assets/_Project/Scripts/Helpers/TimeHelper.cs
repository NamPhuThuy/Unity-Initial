using System;
using UnityEngine;

namespace NamPhuThuy.Common
{
    /// <summary>
    /// Retrieve information of time in the real world
    /// </summary>
    public class TimeHelper : MonoBehaviour
    {
        #region Static Time Helper

        public static double ConvertToUnixTime(DateTime time)
        {
            DateTime epoch = new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            return (time - epoch).TotalSeconds;
        }

        public static DateTime ConvertFromUnixTime(double timeStamp)
        {
            DateTime epoch = new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime time = epoch.AddSeconds(timeStamp);
            return time;
        }

        public static int GetHours(float totalSeconds)
        {
            return (int)(totalSeconds / 3600f);
        }

        public static int GetMinutes(float totalSeconds)
        {
            return (int)((totalSeconds / 60) % 60);
        }

        public static int GetSeconds(float totalSeconds)
        {
            return (int)(totalSeconds % 60);
        }

        #endregion
    }
}
