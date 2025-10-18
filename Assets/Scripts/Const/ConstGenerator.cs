using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditorInternal;
#endif

#if UNITY_EDITOR
namespace NamPhuThuy
{
    public class ConstGenerator : EditorWindow
    {
        // Preferences (stored in EditorPrefs)
        private const string PrefNamespace = "ConstGenerator.Namespace";
        private const string PrefOutFolder = "ConstGenerator.OutFolder";

        private string _namespace;
        private string _outFolder;

        [MenuItem("Tools/TrinhNam/Const Generator")]
        public static void ShowWindow()
        {
            var w = GetWindow<ConstGenerator>("Const Generator");
            w.minSize = new Vector2(520, 320);
        }

        #region Callbacks

        private void OnEnable()
        {
            _namespace = EditorPrefs.GetString(PrefNamespace, "NamPhuThuy.Common");
            _outFolder = EditorPrefs.GetString(PrefOutFolder, "Assets/Scripts/Generated");
        }

        private void OnGUI()
        {
            GUILayout.Label("Constants Generator", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // Settings
            EditorGUI.BeginChangeCheck();
            _namespace = EditorGUILayout.TextField(new GUIContent("Namespace"), _namespace);
            
            EditorGUILayout.BeginHorizontal();
            var folderObj = AssetDatabase.LoadAssetAtPath<DefaultAsset>(_outFolder);
            var newFolderObj = (DefaultAsset)EditorGUILayout.ObjectField(
                new GUIContent("Output Folder"), 
                folderObj, 
                typeof(DefaultAsset), 
                false);

            if (newFolderObj != folderObj && newFolderObj != null)
            {
                string path = AssetDatabase.GetAssetPath(newFolderObj);
                if (AssetDatabase.IsValidFolder(path))
                {
                    _outFolder = path; // store as "Assets/..." relative path
                    EditorPrefs.SetString(PrefOutFolder, _outFolder);
                }
            }

            ButtonReset();
            EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetString(PrefNamespace, _namespace);
                EditorPrefs.SetString(PrefOutFolder, _outFolder);
            }

            ButtonOpenOutputFolder();

            ButtonFindRef();

            EditorGUILayout.Space();
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Generate All", GUILayout.Height(28)))
                {
                    GenerateLayers();
                    GenerateTags();
                    GenerateSortingLayers();
                    GenerateScenes();
                    GenerateScriptingDefines();
                    AssetDatabase.Refresh();
                    EditorUtility.DisplayDialog("Const Generator", "Generated all constant files.", "OK");
                }

                if (GUILayout.Button("Refresh Assets", GUILayout.Height(28)))
                {
                    AssetDatabase.Refresh();
                }
            }

            EditorGUILayout.Space();
            DrawSection("Layers", GenerateLayers);
            DrawSection("Tags", GenerateTags);
            DrawSection("Sorting Layers", GenerateSortingLayers);
            DrawSection("Scenes (Build Settings)", GenerateScenes);
            DrawSection("Scripting Define Symbols", GenerateScriptingDefines);

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox(
                "Tip: Use these constants to avoid string-typos and magic numbers. " +
                "Edit namespace/output to fit your project. Re-run after you change layers/tags/scenes/defines.",
                MessageType.Info);

            void ButtonReset()
            {
                if (GUILayout.Button("Reset", GUILayout.Width(60)))
                {
                    _outFolder = "Assets/Scripts/Generated";
                    EditorPrefs.SetString(PrefOutFolder, _outFolder);
                }
            }

            void ButtonOpenOutputFolder()
            {
                if (GUILayout.Button("Open Output Folder"))
                {
                    var abs = MakeFolder(_outFolder);
                    EditorUtility.RevealInFinder(abs);
                }
            }

            void ButtonFindRef()
            {
                if (GUILayout.Button("Go to Script", GUILayout.Width(180)))
                {
                    var script = MonoScript.FromScriptableObject(this);
                    if (script != null)
                    {
                        EditorGUIUtility.PingObject(script);
                    }
                }
            }
        }

        #endregion

        private void DrawSection(string currentTitle, System.Action gen)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                GUILayout.Label(currentTitle, EditorStyles.boldLabel);
                if (GUILayout.Button($"Generate {currentTitle}"))
                {
                    gen?.Invoke();
                    AssetDatabase.Refresh();
                }
            }
        }

        #region Generator Methods

        private void GenerateLayers()
        {
            var sb = new StringBuilder();
            BeginFile(sb, "LayerConst");

            // Strings
            sb.AppendLine("        // Layer names");
            for (int i = 0; i < 32; i++)
            {
                string name = LayerMask.LayerToName(i);
                if (string.IsNullOrEmpty(name)) continue;
                string constName = ToConstName(name);
                sb.AppendLine($"        public const string {constName} = \"{name}\";");
            }
            sb.AppendLine();

            // Indices
            sb.AppendLine("        // Layer indices");
            for (int i = 0; i < 32; i++)
            {
                string name = LayerMask.LayerToName(i);
                if (string.IsNullOrEmpty(name)) continue;
                string constName = ToConstName(name) + "_INDEX";
                sb.AppendLine($"        public const int {constName} = {i};");
            }

            EndFile(sb);
            WriteFile("LayerConst.cs", sb.ToString());
        }

        private void GenerateTags()
        {
            var tags = InternalEditorUtility.tags;

            var sb = new StringBuilder();
            BeginFile(sb, "TagConst");

            sb.AppendLine("        // Unity Tags");
            foreach (var t in tags)
            {
                if (string.IsNullOrEmpty(t)) continue;
                sb.AppendLine($"        public const string {ToConstName(t)} = \"{t}\";");
            }

            EndFile(sb);
            WriteFile("TagConst.cs", sb.ToString());
        }

        private void GenerateSortingLayers()
        {
            var layers = SortingLayer.layers; // struct[] with .id and .name

            var sb = new StringBuilder();
            BeginFile(sb, "SortingLayerConst");

            sb.AppendLine("        // Sorting Layer names");
            foreach (var l in layers)
            {
                sb.AppendLine($"        public const string {ToConstName(l.name)} = \"{l.name}\";");
            }
            sb.AppendLine();
            sb.AppendLine("        // Sorting Layer IDs");
            foreach (var l in layers)
            {
                sb.AppendLine($"        public const int {ToConstName(l.name)}_ID = {l.id};");
            }

            EndFile(sb);
            WriteFile("SortingLayerConst.cs", sb.ToString());
        }

        private void GenerateScenes()
        {
            var scenes = EditorBuildSettings.scenes;

            var sb = new StringBuilder();
            BeginFile(sb, "SceneConst");

            sb.AppendLine("        // Scenes included in Build Settings");
            for (int i = 0; i < scenes.Length; i++)
            {
                var s = scenes[i];
                if (!s.enabled) continue;

                string name = Path.GetFileNameWithoutExtension(s.path);
                string safe = ToConstName(name);
                sb.AppendLine($"        public const string {safe} = \"{name}\";");
            }

            sb.AppendLine();
            sb.AppendLine("        // Build indices");
            int buildIndex = 0;
            for (int i = 0; i < scenes.Length; i++)
            {
                var s = scenes[i];
                if (!s.enabled) continue;
                string name = Path.GetFileNameWithoutExtension(s.path);
                string safe = ToConstName(name);
                sb.AppendLine($"        public const int {safe}_INDEX = {buildIndex};");
                buildIndex++;
            }

            EndFile(sb);
            WriteFile("SceneConst.cs", sb.ToString());
        }

        private void GenerateScriptingDefines()
        {
            // By platform group of current active target
            var group = EditorUserBuildSettings.selectedBuildTargetGroup;
#if UNITY_2021_1_OR_NEWER
            string defines = PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.FromBuildTargetGroup(group));
#else
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
#endif
            var defineArray = defines.Split(new[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);

            var sb = new StringBuilder();
            BeginFile(sb, "ScriptingDefineConst");

            sb.AppendLine("        // PlayerSettings Scripting Define Symbols for current target group");
            foreach (var d in defineArray)
            {
                var trimmed = d.Trim();
                if (string.IsNullOrEmpty(trimmed)) continue;
                sb.AppendLine($"        public const string {ToConstName(trimmed)} = \"{trimmed}\";");
            }

            EndFile(sb);
            WriteFile("ScriptingDefineConst.cs", sb.ToString());
        }

        #endregion



        #region Helper Methods

        private void BeginFile(StringBuilder sb, string className)
        {
            sb.AppendLine("/* This file is auto-generated. Do not edit by hand. */");
            sb.AppendLine($"namespace {_namespace}");
            sb.AppendLine("{");
            sb.AppendLine($"    public static class {className}");
            sb.AppendLine("    {");
        }

        private void EndFile(StringBuilder sb)
        {
            sb.AppendLine("    }");
            sb.AppendLine("}");
        }

        private void WriteFile(string fileName, string content)
        {
            string absFolder = MakeFolder(_outFolder);
            string path = Path.Combine(absFolder, fileName);
            File.WriteAllText(path, content, Encoding.UTF8);
        }

        private static string MakeFolder(string projectRelative)
        {
            // Ensure under Assets
            if (string.IsNullOrEmpty(projectRelative) || !projectRelative.StartsWith("Assets"))
                projectRelative = "Assets/Scripts/Generated";

            if (!AssetDatabase.IsValidFolder(projectRelative))
            {
                // Create nested folders
                var parts = projectRelative.Split('/');
                string cur = "Assets";
                for (int i = 1; i < parts.Length; i++)
                {
                    string next = $"{cur}/{parts[i]}";
                    if (!AssetDatabase.IsValidFolder(next))
                        AssetDatabase.CreateFolder(cur, parts[i]);
                    cur = next;
                }
            }

            return Path.GetFullPath(projectRelative);
        }

        private static string ToConstName(string raw)
        {
            // Uppercase, replace invalid with underscore, collapse repeats, trim edges
            string upper = raw.ToUpperInvariant();
            upper = Regex.Replace(upper, @"[^A-Z0-9_]", "_");
            upper = Regex.Replace(upper, @"_+", "_");
            upper = upper.Trim('_');

            // Ensure first char is a letter or underscore
            if (!Regex.IsMatch(upper, @"^[A-Z_]")) upper = "_" + upper;

            return upper;
        }

        #endregion
        
    }
}

#endif