#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using System.IO;


public class OpenPersistentDataPath : EditorWindow
{
    [MenuItem("Tools/TrinhNam/Open Persistent Data Path")]
    static void RunOpenPersistentPath()
    {
        string path = Application.persistentDataPath;

        // Ensure the directory exists
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

#if UNITY_EDITOR_WIN
        Process.Start("explorer.exe", path.Replace("/", "\\"));
#elif UNITY_EDITOR_OSX
            Process.Start("open", path);
#elif UNITY_EDITOR_LINUX
            Process.Start("xdg-open", path);
#else
            UnityEngine.Debug.LogWarning("This platform is not supported for opening paths.");
#endif
    }
}
#endif
