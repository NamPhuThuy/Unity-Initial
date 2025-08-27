using UnityEngine;

namespace NamPhuThuy.Common
{
    public static class NumberExtension
    {
        public static bool InRange(this float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        public static int GetMbSize(this long byteSize)
        {
            int mbSize = Mathf.CeilToInt(byteSize / (1024f * 1024f));
            return mbSize;
        }

        public static float CompareAngle(this float sourceAngle, float otherAngle)
        {
            // sourceAngle and otherAngle should be in the range -180 to 180
            float difference = otherAngle - sourceAngle;
            if (difference < -180.0f)
                difference += 360.0f;
            if (difference > 180.0f)
                difference -= 360.0f;
            if (difference > 0.0f)
                return 1;
            if (difference < 0.0f)
                return -1;
            return 0;
        }
    }
}
