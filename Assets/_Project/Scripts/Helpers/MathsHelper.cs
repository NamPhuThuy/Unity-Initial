using UnityEngine;

namespace NamPhuThuy.Common
{
    public static class MathsHelper
    {
        //Map a value from scale-A to scale-B
        #region MapValue
        public static float Map(float value, float originMin, float originMax, float newMin, float newMax, bool clamp)
        {
            float newValue = (value - originMin) / (originMax - originMin) * (newMax - newMin) + newMin;

            if (clamp)
            {
                newValue = Mathf.Clamp(newValue, newMin, newMax);
            }

            return newValue;
        }

        public static float ReverseMap(float value, float originMin, float originMax, float newMin, float newMax,
            bool clamp)
        {
            float newValue = (1f - ((value - originMin) / (originMax - originMin))) * (newMax - newMin) + newMin;

            if (clamp)
            {
                newValue = Mathf.Clamp(newValue, newMin, newMax);
            }

            return newValue;
        }
        #endregion

        #region Iterpolation

        /// <summary>
        /// Linear interpolation.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="percentage">Percentage.</param>
        public static float LinearInterpolate (float from, float to, float percentage)
        {
            return (to - from) * percentage + from;
        }

        /// <summary>
        /// Linear interpolation.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="percentage">Percentage.</param>
        public static Vector2 LinearInterpolate (Vector2 from, Vector2 to, float percentage)
        {
            return new Vector2 (LinearInterpolate (from.x, to.x, percentage), LinearInterpolate (from.y, to.y, percentage));
        }

        /// <summary>
        /// Linear interpolation.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="percentage">Percentage.</param>
        public static Vector3 LinearInterpolate (Vector3 from, Vector3 to, float percentage)
        {
            return new Vector3 (LinearInterpolate (from.x, to.x, percentage), LinearInterpolate (from.y, to.y, percentage), LinearInterpolate (from.z, to.z, percentage));
        }

        /// <summary>
        /// Linear interpolation.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="percentage">Percentage.</param>
        public static Vector4 LinearInterpolate (Vector4 from, Vector4 to, float percentage)
        {
            return new Vector4 (LinearInterpolate (from.x, to.x, percentage), LinearInterpolate (from.y, to.y, percentage), LinearInterpolate (from.z, to.z, percentage), LinearInterpolate (from.w, to.w, percentage));
        }

        /// <summary>
        /// Linear interpolation.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="percentage">Percentage.</param>
        public static Rect LinearInterpolate (Rect from, Rect to, float percentage)
        {
            return new Rect (LinearInterpolate (from.x, to.x, percentage), LinearInterpolate (from.y, to.y, percentage), LinearInterpolate (from.width, to.width, percentage), LinearInterpolate (from.height, to.height, percentage));
        }

        /// <summary>
        /// Linear interpolation.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="percentage">Percentage.</param>
        public static Color LinearInterpolate (Color from, Color to, float percentage)
        {
            return new Color (LinearInterpolate (from.r, to.r, percentage), LinearInterpolate (from.g, to.g, percentage), LinearInterpolate (from.b, to.b, percentage), LinearInterpolate (from.a, to.a, percentage));
        }

        #endregion
        
        //animation curves:
        /// <summary>
        /// Evaluates the curve.
        /// </summary>
        /// <returns>The value evaluated at the percentage of the clip.</returns>
        /// <param name="curve">Curve.</param>
        /// <param name="percentage">Percentage.</param>
        public static float EvaluateCurve (AnimationCurve curve, float percentage)
        {
            return curve.Evaluate ((curve [curve.length - 1].time) * percentage);
        }

        #region Random
        //To get a truly consistent (repeatable) random sequence, you need to provide a specific seed:
        //private static System.Random mRandom = new System.Random(12345); // example fixed seed
        private static System.Random mRandom = new System.Random();
        public static int EasyRandom(int range)
        {
            return mRandom.Next(range);
        }

        public static int EasyRandom(int min, int max)
        {
            return mRandom.Next(min, max);
        }

        public static float EasyRandom(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }
        #endregion

        #region Convert

        /// <summary>
        /// Turn number to text
        /// <para>12.345f -> 12</para>
        /// <para>1234 -> 1234</para>
        /// <para>1234567 -> 1.23M</para>
        /// <para>1234567890 -> 1.23B</para>
        /// </summary>
        /// <param name="_number"></param>
        /// <returns></returns>
        public static string NumberCustomToString(float _number)
        {
            string str = "";
            if (_number < 10000)
                str = _number.ToString("00");
            else if (10000 <= _number && _number < 1000000)
                str = (_number / 1000).ToString("0.#") + "K";
            else if (1000000 <= _number && _number < 1000000000)
                str = (_number / 1000000).ToString("0.##") + "M";
            else
                str = (_number / 1000000000).ToString("0.##") + "B";
            return str;
        }

        #endregion
    }

}