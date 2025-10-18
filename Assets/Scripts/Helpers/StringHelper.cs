using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace NamPhuThuy
{
    
    public class StringHelper : MonoBehaviour
    {
        #region Private Serializable Fields

        #endregion

        #region Private Fields

        #endregion

        

        #region Private Methods
        #endregion

        #region Public Methods
        
        /// <summary>
        /// Capitalizes the first letter of each word in a string.
        /// Words are separated by spaces, underscores, or hyphens.
        /// </summary>
        public static string CapitalizeEachWord(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            // Replace underscores and hyphens with spaces for better readability
            string normalized = Regex.Replace(input, @"[_-]+", " ");

            // Use TextInfo for proper culture-aware capitalization
            TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
            return textInfo.ToTitleCase(normalized.ToLower());
        }
        
        /// <summary>
        /// Converts a string into all-uppercase letters, leaving underscores intact.
        /// Example: "sfx_button" → "SFX_BUTTON"
        /// </summary>
        public static string CapitalizeEachCharacter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return input.ToUpperInvariant();
        }
        
        #endregion
    }
}