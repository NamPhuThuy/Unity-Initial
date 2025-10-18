using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NamPhuThuy.Common
{
    public static class ColorExtension
    {
        public static bool SameHexaCode(this Color color1, Color color2)
        {
            return color1.ToHexaCode().Equals(color2.ToHexaCode());
        }

        public static string ToHexaCode(this Color color)
        {
            return "#" + ColorUtility.ToHtmlStringRGBA(color);
        }

        public static Color ToColor(string hexaCode, Color defaultValue)
        {
            Color color;
            bool sucess = ColorUtility.TryParseHtmlString(hexaCode, out color);
            if (sucess)
            {
                return color;
            }
            return defaultValue;
        }
    }

}