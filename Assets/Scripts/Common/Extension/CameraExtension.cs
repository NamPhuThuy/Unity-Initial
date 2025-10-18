using UnityEngine;

namespace NamPhuThuy.Common
{
    public static class CameraExtension
    {
        public static Vector2 GetSize(this Camera camera)
        {
            float height = 2f * camera.orthographicSize;
            float width = height * camera.aspect;
            return new Vector2(width, height);
        }

        public static float GetSizeRatioByScreen(AutoSizeMatchType matchType, Vector2 defaultScreen)
        {
            return GetCurrentRatio(matchType) / GetDefaultRatio(matchType, defaultScreen);
        }

        static float GetDefaultRatio(AutoSizeMatchType matchType, Vector2 defaultScreen)
        {
            switch (matchType)
            {
                case AutoSizeMatchType.Width:
                    return defaultScreen.x / defaultScreen.y;
                case AutoSizeMatchType.Height:
                default:
                    return defaultScreen.y / defaultScreen.x;
            }
        }
        static float GetCurrentRatio(AutoSizeMatchType matchType)
        {
            switch (matchType)
            {
                case AutoSizeMatchType.Width:
                    return Screen.width * 1f / Screen.height;
                case AutoSizeMatchType.Height:
                default:
                    return Screen.height * 1f / Screen.width;
            }
        }
    }

    public enum AutoSizeMatchType
    {
        Width,
        Height
    }

}