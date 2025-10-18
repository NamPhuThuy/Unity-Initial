using UnityEngine;

using System.IO;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build.Reporting;

public class WindowsBuildScript : EditorWindow
{
    // Build configuration variables
    private static string productName = "YourGameName";
    private static BuildTarget buildTarget = BuildTarget.StandaloneWindows64;
    private static BuildOptions buildOptions = BuildOptions.None;

    // Path for build output
    private static string outputPath = Path.Combine(Application.dataPath, "..", "Builds", "Windows2");

    [MenuItem("Tools/TrinhNam/Build/Build Windows")]
    public static void BuildWindows()
    {
        // Ensure build directory exists
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        // Prepare scenes for build
        string[] scenesToBuild = GetBuildScenes();

        // Full path with executable name
        string exePath = Path.Combine(outputPath, $"{productName}.exe");

        // Perform the build
        BuildReport buildReport = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = scenesToBuild,
            locationPathName = exePath,
            target = buildTarget,
            options = buildOptions
        });

        // Check build result
        if (buildReport.summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"Windows Build Success: {exePath}");
            
            // Optional: Open build folder after successful build
            EditorUtility.RevealInFinder(exePath);
        }
        else
        {
            Debug.LogError("Windows Build Failed");
        }
    }

    // Get all scenes included in build settings
    private static string[] GetBuildScenes()
    {
        return EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();
    }

    // Advanced build configurations
    [MenuItem("Tools/TrinhNam/Build/Build Windows (Development)")]
    public static void BuildWindowsDevelopment()
    {
        buildOptions = BuildOptions.Development | BuildOptions.AllowDebugging;
        BuildWindows();
    }

    [MenuItem("Tools/TrinhNam/Build/Build Windows (Compressed)")]
    public static void BuildWindowsCompressed()
    {
        buildOptions = BuildOptions.CompressWithLz4;
        BuildWindows();
    }

    // Optional: Custom build configuration window
    [MenuItem("Tools/TrinhNam/Build/Build Configuration Window")]
    public static void ShowBuildWindow()
    {
        GetWindow<BuildConfigWindow>("Build Configuration");
    }
}

// Optional: Custom build configuration window
public class BuildConfigWindow : EditorWindow
{
    private string customProductName;
    private BuildTarget selectedTarget = BuildTarget.StandaloneWindows64;

    void OnGUI()
    {
        GUILayout.Label("Build Configuration", EditorStyles.boldLabel);

        // Product Name Input
        customProductName = EditorGUILayout.TextField("Product Name", customProductName);

        // Target Platform Selection
        selectedTarget = (BuildTarget)EditorGUILayout.EnumPopup("Build Target", selectedTarget);

        // Build Button
        if (GUILayout.Button("Build Game"))
        {
            // Update build script's product name
            // WindowsBuildScript.productName = customProductName;
            WindowsBuildScript.BuildWindows();  
        }
    }
}
#endif

