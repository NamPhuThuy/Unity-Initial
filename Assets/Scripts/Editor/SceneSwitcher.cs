using System.Collections;
using System.Collections.Generic;
using NamPhuThuy.Common;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

#if UNITY_EDITOR
namespace NamPhuThuy.EditorTools
{
    public class SceneSwitcher : Editor
    {
        private const string SWITCH_SCENE_MENU_NAME = "Tools/TrinhNam/Switch Scene";
        private const string ALT = "&";

        #region LoadSceneShortCut

        [MenuItem(SWITCH_SCENE_MENU_NAME + "/Scene 0 " + ALT + "1")]
        static void LoadScene0()
        {
            LoadSceneByIndex(0);
        }

        [MenuItem(SWITCH_SCENE_MENU_NAME + "/Scene 1 " + ALT + "2")]
        static void LoadScene1()
        {
            LoadSceneByIndex(1);
        }

        [MenuItem(SWITCH_SCENE_MENU_NAME + "/Scene 2 " + ALT + "3")]
        static void LoadScene2()
        {
            LoadSceneByIndex(2);
        }

        [MenuItem(SWITCH_SCENE_MENU_NAME + "/Scene 3 " + ALT + "4")]
        static void LoadScene3()
        {
            LoadSceneByIndex(3);
        }

        static void LoadSceneByIndex(int buildIndex)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

            // Get path for the scene by build index
            string scenePath = GetScenePathByIndex(buildIndex);

            if (!string.IsNullOrEmpty(scenePath))
            {
                EditorSceneManager.OpenScene(scenePath);
            }
            else
            {
                Debug.LogError($"No scene found at build index {buildIndex}. Please check your build settings.");
            }
        }

        private static string GetScenePathByIndex(int buildIndex)
        {
            // Validate index
            if (buildIndex < 0 || buildIndex >= EditorBuildSettings.scenes.Length)
            {
                Common.DebugLogger.LogError($"Build index {buildIndex} is out of range.", Color.black);
                return null;
            }

            // Get scene path from build settings
            var scene = EditorBuildSettings.scenes[buildIndex];
            return scene.enabled ? scene.path : null;
        }

        #endregion
    }
}
#endif