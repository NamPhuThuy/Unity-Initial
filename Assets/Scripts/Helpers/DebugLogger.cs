using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

/*
 Why should keep the suffix of class is "Logger" and prefix of method is "Log"
 - Reddit: https://www.reddit.com/r/Unity3D/comments/17eikh0/i_found_a_way_to_go_to_the_right_line_in_your/
 - Sample gist: https://gist.github.com/AnatoleCF/9f4b28750ebd8c30bebd3cd04f04e520
 - Any class whose name ends with "Logger" that implements a method starting with "Log" is ignored by the console's double click, unless it is the last call in the stack trace.
 - 
 */

namespace NamPhuThuy.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class DebugLogger
    {
        public static bool enableLog = true;
        private static readonly Color defaultColor = Color.white;

        #region Log Error

        public static void LogError(string content, Color color, bool setBold = false)
        {
            if (!enableLog)
                return;
            Debug.LogError(ColorizedText(content, color, setBold));
            return;
        }
        
        public static void LogError(string content, bool setBold = false)
        {
            if (!enableLog)
                return;
            Debug.LogError(ColorizedText(content, setBold));
            return;
        }
        
        /// <summary>
        /// Log error only if condition is true
        /// </summary>
        public static void LogErrorIf(bool condition, string content, Color color = default, bool setBold = false)
        {
            if (!enableLog)
                return;
        
            if (condition)
            {
                if (color == default)
                    LogError(content, setBold);
                else
                    LogError(content, color, setBold);
            }
        }

        #endregion

        #region Log Warning
        
        public static void LogWarning(string content, Color color, bool setBold = false)
        {
            if (!enableLog)
                return;
            Debug.LogWarning(ColorizedText(content, color, setBold));
            return;
        }
        
        public static void LogWarning(string content, bool setBold = false)
        {
            if (!enableLog)
                return;
            Debug.LogWarning(ColorizedText(content, setBold));
            return;
        }

        /// <summary>
        /// Log warning only if condition is true
        /// </summary>
        public static void LogWarningIf(bool condition, string content, Color color = default, bool setBold = false)
        {
            if (!enableLog)
                return;
        
            if (condition)
            {
                if (color == default)
                    LogWarning(content, setBold);
                else
                    LogWarning(content, color, setBold);
            }
        }
        #endregion
        
        #region Log 

        public static void Log(string content, Color color, bool setBold = false, Object context = null)
        {
            if (!enableLog)
                return;
            Debug.Log(ColorizedText(content, color, setBold), context: context);
            return;
        }
        
        public static void Log(string content, bool setBold = false)
        {
            if (!enableLog)
                return;
            Debug.Log(ColorizedText(content, setBold));
            return;
        }
        
        /// <summary>
        /// Log only if condition is true
        /// </summary>
        public static void LogIf(bool condition, string content, Color color = default, bool setBold = false)
        {
            if (!enableLog)
                return;
        
            if (condition)
            {
                if (color == default)
                    Log(content, setBold);
                else
                    Log(content, color, setBold);
            }
        }
        
        public static void LogWithFrame(string content, Color color = default, bool setBold = false)
        {
            string frameInfo = $"[Frame {Time.frameCount}] ";
            Debug.Log(ColorizedText($"{frameInfo} - {content}", color, setBold));
        }
        
        public static void LogSimple(
            [CallerLineNumber] int line = 0
            , [CallerMemberName] string memberName = ""
            , [CallerFilePath] string filePath = "", string message = "", Color color = default, Object context = null
            /*, [CallerArgumentExpression("message")] string expression = ""*/
        )
        {
            string className = Path.GetFileNameWithoutExtension(filePath);

            Color currentColor = color == default ? Color.cyan : color;
            Log($"{className}().{memberName}()::{line}: {message}", currentColor, context: context);
            /*Can replace UnityEngine.Debug.Log with any logging API you want

            Usage is simple: just put a LogCaller(); at any line you want. The compiler will pass in the 3 parameters for you.*/
        }
        
        

        #endregion

        #region Break

        
        /// <summary>
        /// Breaks execution in the editor and logs a message
        /// </summary>
        public static void LogBreak(string content, Color color = default, bool setBold = false,
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "")
        {
            if (!enableLog)
                return;

            string className = Path.GetFileNameWithoutExtension(filePath);
            string location = $"{className}.{memberName}()::{line}";
            string message = $"[BREAK] {location} - {content}";

            if (color == default)
                color = Color.red;

            Debug.LogError(ColorizedText(message, color, setBold));

#if UNITY_EDITOR
            Debug.Break();
#endif
        }

        /// <summary>
        /// Conditional break - only breaks if condition is true
        /// </summary>
        public static void LogBreakIf(bool condition, string content, Color color = default, bool setBold = false,
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "")
        {
            if (condition)
            {
                LogBreak(content, color, setBold, line, memberName, filePath);
            }
        }

        /// <summary>
        /// Assert with break - breaks if condition is false
        /// </summary>
        public static void LogAssert(bool condition, string content, Color color = default, bool setBold = false,
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "")
        {
            if (!condition)
            {
                LogBreak($"ASSERTION FAILED: {content}", color, setBold, line, memberName, filePath);
            }
        }

        #endregion

        #region Try-catch 

        /// <summary>
        /// Try-catch wrapper with logging
        /// </summary>
        public static void LogTry(System.Action action, string context = "Unknown operation", Object contextObject = null)
        {
            try
            {
                action?.Invoke();
            }
            catch (System.Exception ex)
            {
                LogException(ex, context, contextObject);
            }
        }
        
        /// <summary>
        /// Log exception with full details
        /// </summary>
        public static void LogException(System.Exception ex, string context = "", Object contextObject = null)
        {
            if (!enableLog)
                return;

            string message = string.IsNullOrEmpty(context) 
                ? $"EXCEPTION: {ex.Message}\nStackTrace: {ex.StackTrace}" 
                : $"EXCEPTION in {context}: {ex.Message}\nStackTrace: {ex.StackTrace}";
    
            Debug.LogError(ColorizedText(message, Color.red, true), contextObject);
        }

        #endregion

        #region Helper

        /// <summary>
        /// Get full hierarchy path of GameObject
        /// </summary>
        private static string GetGameObjectPath(GameObject go)
        {
            if (go == null)
                return "NULL";

            string path = go.name;
            Transform parent = go.transform.parent;
    
            while (parent != null)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }
    
            return path;
        }
        
        /// <summary>
        /// Format array/collection for logging
        /// </summary>
        public static string FormatCollection<T>(System.Collections.Generic.IEnumerable<T> collection, string separator = ", ")
        {
            if (collection == null)
                return "NULL";
    
            return string.Join(separator, collection);
        }

        /// <summary>
        /// Format Vector3 for logging
        /// </summary>
        public static string FormatVector3(Vector3 vector, int decimals = 2)
        {
            return $"({vector.x.ToString($"F{decimals}")}, {vector.y.ToString($"F{decimals}")}, {vector.z.ToString($"F{decimals}")})";
        }
        
        public static string ColorizedText(string content, Color color, bool setBold = false)
        {
            if (setBold)
                return $"<b><color=#{ColorUtility.ToHtmlStringRGB(color)}>{content}</color></b>";
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{content}</color>";
        }
        
        public static string ColorizedText(string content, bool setBold = false)
        {
            if (setBold)
                return $"<b>{content}</b>";
            return $"{content}";
        }

        #endregion
    }
}