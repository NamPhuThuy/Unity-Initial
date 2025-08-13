using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NamPhuThuy.EditorHelpers
{
    /// <summary>
    /// This class provides a menu item to remove all GameObjects with missing scripts in the project.
    /// </summary>
    public class RemoveMissingScripts : Editor
    {
        [MenuItem("Tools/TrinhNam/Remove Missing Scripts")]
        public static void Remove()
        {
            var objs = Resources.FindObjectsOfTypeAll<GameObject>();
            int count = objs.Sum(GameObjectUtility.RemoveMonoBehavioursWithMissingScript);
            Debug.Log($"Removed {count} missing scripts");
        }
    }

}