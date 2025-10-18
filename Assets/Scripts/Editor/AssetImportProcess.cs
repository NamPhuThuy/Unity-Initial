using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;


//automatically sets import presets based on asset format
namespace NamPhuThuy
{
    public class AssetImportProcess : AssetPostprocessor
    {
        // This method is called automatically when any asset is imported
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, 
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string assetPath in importedAssets)
            {
                /*ApplyPreset(assetPath);
                CheckImageDimensions(assetPath);*/
            }
        }

        static void ApplyPreset(string assetPath)
        {
            string extension = Path.GetExtension(assetPath).ToLower();
            string presetPath = GetPresetPathForExtension(extension);
            
            if (!string.IsNullOrEmpty(presetPath))
            {
                // Load the preset
                var preset = AssetDatabase.LoadAssetAtPath<UnityEditor.Presets.Preset>(presetPath);
                // var preset = AssetDatabase.LoadAssetAtPath(presetPath, typeof(Preset));
                
                if (preset != null)
                {
                    // Get the importer for the asset
                    var importer = AssetImporter.GetAtPath(assetPath);
                
                    if (importer != null)
                    {
                        // Apply the preset to the importer
                        preset.ApplyTo(importer);
                    }
                }
            }
        }
        
        static void CheckImageDimensions(string assetPath)
        {
            // Only process image files
            string[] imageExtensions = { ".png", ".jpg", ".jpeg", ".tga", ".bmp", ".tiff" };
            string extension = Path.GetExtension(assetPath).ToLower();
        
            if (!System.Array.Exists(imageExtensions, ext => ext == extension))
                return;
            
            // Get the texture importer
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer == null)
            {
                Debug.LogWarning("Texture impoter is empty");
                return;
            }
            
            // Read the actual texture dimensions
            int width, height;
            GetTextureSize(assetPath, out width, out height);
                    
            // Check if both dimensions are divisible by 4
            bool isDivisibleByFour = (width % 4 == 0) && (height % 4 == 0);

            if (!isDivisibleByFour)
            {
                // Log a warning
                Debug.LogWarning($"Texture '{Path.GetFileName(assetPath)}' dimensions are not divisible by 4. " +
                                 $"size: {width}x{height}.");

                // Optional: You may automatically resize the texture here
            }
        }
        
        static void GetTextureSize(string assetPath, out int width, out int height)
        {
            width = height = 0;
            
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            if (texture != null)
            {
                width = texture.width;
                height = texture.height;
            }
        }
        
     
        /// <summary>
        /// Return the path of desired preset for each extension file
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        static string GetPresetPathForExtension(string extension)
        {
            // Define preset paths for different file types
            // Make sure these preset files exist in your project
            switch (extension)
            {
                // Texture Presets
                case ".png":
                    return "Assets/_GamePrototype2D/Preset/PixelArtPreset.preset";
                case ".jpg":
                case ".jpeg":
                case ".tga":
                case ".bmp":
                case ".tiff":
                    return "Assets/ImportPresets/TextureImportPreset.preset";

                // Sprite Presets
                case ".sprite":
                    return "Assets/ImportPresets/SpriteImportPreset.preset";

                // Audio Presets
                case ".wav":
                case ".mp3":
                case ".ogg":
                    return "Assets/ImportPresets/AudioImportPreset.preset";

                // Model Presets
                case ".fbx":
                case ".obj":
                    return "Assets/ImportPresets/ModelImportPreset.preset";

                // Add more extensions as needed
                default:
                    return null;
            }
        }
    }
}
#endif
