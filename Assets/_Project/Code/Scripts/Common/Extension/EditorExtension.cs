#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using NamPhuThuy;
using UnityEditor;
using UnityEngine;

namespace NamPhuThuy.Common
{
    public static class EditorExtension
    {
        public static List<T> LoadAssets<T>(string path, string fileExtension) where T : Object
        {
            if (path != "")
            {
                if (path.EndsWith("/"))
                {
                    path = path.TrimEnd('/');
                }
            }
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            FileInfo[] fileInf = dirInfo.GetFiles($"*.{fileExtension}", SearchOption.AllDirectories);
            List<T> assets = new List<T>();
            foreach (FileInfo fileInfo in fileInf)
            {
                string fullPath = fileInfo.FullName.Replace(@"\", "/");
                string assetPath = "Assets" + fullPath.Replace(Application.dataPath, "");
                T prefab = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (prefab)
                {
                    assets.Add(prefab);
                }
            }
            return assets;
        }

        public static List<T> LoadAssets<T>(Object folder, string fileExtension) where T : Object
        {
            string path = AssetDatabase.GetAssetPath(folder);
            return LoadAssets<T>(path, fileExtension);
        }

        public static Texture GetTexture(Object folder, string name, string imageType = "png")
        {
            string path = AssetDatabase.GetAssetPath(folder);
            var textures = LoadAssets<Object>(path, imageType);
            foreach (var item in textures)
            {
                if (item.name.Contains(name))
                {
                    return item as Texture;
                }
            }
            return null;
        }
        public static Sprite FindSprite(Object folder, string name, string imageType = "png")
        {
            string path = AssetDatabase.GetAssetPath(folder);
            var textures = LoadAssets<Object>(path, imageType);
            foreach (var item in textures)
            {
                Object[] data = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(item));
                foreach (var sprite in data)
                {
                    if (sprite as Sprite)
                    {
                        if (sprite.name.Equals(name))
                        {
                            return sprite as Sprite;
                        }
                    }
                }
            }
            return null;
        }

        public static T CreateAsset<T>(Object originalAsset, Object saveFolder, string saveName) where T : Object
        {
            string savedRootPath = AssetDatabase.GetAssetPath(saveFolder);
            string moveConfigOriginPath = AssetDatabase.GetAssetPath(originalAsset);
            string savePath = $"{savedRootPath}/{saveName}";
            //
            var existedAsset = AssetDatabase.LoadAssetAtPath<Object>(savePath);
            if (existedAsset && existedAsset is T)
            {
                return existedAsset as T;
            }
            //
            AssetDatabase.CopyAsset(moveConfigOriginPath, savePath);
            Object savedAsset = AssetDatabase.LoadAssetAtPath<Object>(savePath);
            return savedAsset as T;
        }

        public static void SetLoop(Object fbx, bool isLoop)
        {
            string path = AssetDatabase.GetAssetPath(fbx);
            ModelImporter modelImporter = AssetImporter.GetAtPath(path) as ModelImporter;
            ModelImporterClipAnimation[] clipAnimations = new ModelImporterClipAnimation[modelImporter.defaultClipAnimations.Length];
            for (int i = 0; i < modelImporter.defaultClipAnimations.Length; i++)
            {
                clipAnimations[i] = new ModelImporterClipAnimation();
                clipAnimations[i].cycleOffset = modelImporter.defaultClipAnimations[i].cycleOffset;
                clipAnimations[i].events = modelImporter.defaultClipAnimations[i].events;
                clipAnimations[i].heightFromFeet = modelImporter.defaultClipAnimations[i].heightFromFeet;
                clipAnimations[i].heightOffset = modelImporter.defaultClipAnimations[i].heightOffset;
                clipAnimations[i].keepOriginalOrientation = modelImporter.defaultClipAnimations[i].keepOriginalOrientation;
                clipAnimations[i].keepOriginalPositionXZ = modelImporter.defaultClipAnimations[i].keepOriginalPositionXZ;
                clipAnimations[i].keepOriginalPositionY = modelImporter.defaultClipAnimations[i].keepOriginalPositionY;
                clipAnimations[i].lockRootHeightY = modelImporter.defaultClipAnimations[i].lockRootHeightY;
                clipAnimations[i].lockRootPositionXZ = modelImporter.defaultClipAnimations[i].lockRootPositionXZ;
                clipAnimations[i].lockRootRotation = modelImporter.defaultClipAnimations[i].lockRootRotation;
                clipAnimations[i].loopPose = modelImporter.defaultClipAnimations[i].loopPose;
                clipAnimations[i].maskSource = modelImporter.defaultClipAnimations[i].maskSource;
                clipAnimations[i].maskType = modelImporter.defaultClipAnimations[i].maskType;
                clipAnimations[i].mirror = modelImporter.defaultClipAnimations[i].mirror;
                clipAnimations[i].rotationOffset = modelImporter.defaultClipAnimations[i].rotationOffset;
                clipAnimations[i].takeName = modelImporter.defaultClipAnimations[i].takeName;
                clipAnimations[i].curves = modelImporter.defaultClipAnimations[i].curves;
                clipAnimations[i].name = modelImporter.defaultClipAnimations[i].name;
                clipAnimations[i].firstFrame = modelImporter.defaultClipAnimations[i].firstFrame;
                clipAnimations[i].lastFrame = modelImporter.defaultClipAnimations[i].lastFrame;
                clipAnimations[i].loop = isLoop;
                clipAnimations[i].loopTime = isLoop;
                clipAnimations[i].wrapMode = WrapMode.Loop;
            }
            modelImporter.clipAnimations = clipAnimations;
            modelImporter.SaveAndReimport();
        }
        public static void FindAnimationClips(this AnimatorOverrideController animationOverride, Object fbxAssetsFolder, List<AnimatorOverrideConfig> configs)
        {
            string path = AssetDatabase.GetAssetPath(fbxAssetsFolder);
            var fbxs = LoadAssets<Object>(path, "fbx");
            foreach (var fbx in fbxs)
            {
                var itemPath = AssetDatabase.GetAssetPath(fbx);
                var all = AssetDatabase.LoadAllAssetsAtPath(itemPath);
                foreach (var t in all)
                {
                    if (t is AnimationClip && !t.name.Contains("__preview__"))
                    {
                        if (fbx.name.Contains(animationOverride.name))
                        {
                            foreach (var config in configs)
                            {
                                if (t.name.Equals(config.AnimationClipName))
                                {
                                    animationOverride[config.Key] = t as AnimationClip;
                                }
                            }
                        }
                    }
                }
            }
            EditorUtility.SetDirty(animationOverride);
            AssetDatabase.SaveAssets();
        }

        public static void Rename(this Object obj, string newName)
        {
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(obj), newName);
            EditorUtility.SetDirty(obj);
            AssetDatabase.SaveAssets();
        }

        public static void SetDirtyAndSave(this Object obj)
        {
            EditorUtility.SetDirty(obj);
            AssetDatabase.SaveAssetIfDirty(obj);
        }
        public static void MarkDirty(this Object obj)
        {
            EditorUtility.SetDirty(obj);
        }
        public static void DestroyAllInChildren<T>(this Component comp) where T : Component
        {
            var comps = comp.GetComponentsInChildren<T>();
            foreach (var item in comps)
            {
                Object.DestroyImmediate(item);
            }
            comp.SetDirtyAndSave();
        }

        public static IEnumerable<Transform> AllChilds(this Transform transform)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                yield return transform.GetChild(i);
                foreach (var item in transform.GetChild(i).AllChilds())
                {
                    yield return item;
                }
            }
        }

        public static void SetUpDefineSymbolsForGroup(string key, bool enable)
        {
            //Debug.Log(enable);
            BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            // Only if not defined already.
            if (defines.Contains(key))
            {
                if (enable)
                {
                    Debug.LogWarning("Selected build target (" + EditorUserBuildSettings.activeBuildTarget.ToString() + ") already contains <b>" + key + "</b> <i>Scripting Define Symbol</i>.");
                    return;
                }
                else
                {
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, (defines.Replace(key, "")));

                    return;
                }
            }
            else
            {
                // Append
                if (enable)
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, (defines + ";" + key));
            }
        }

        public static void SetShader(this Material material, Shader shader)
        {
            var texture = material.mainTexture;
            var color = material.color;
            if (!texture)
            {
                texture = material.GetTexture("_Albeto");
            }
            material.shader = shader;
            material.mainTexture = texture;
            material.color = color;
            material.SetDirtyAndSave();
        }

        public static GameObject CreateObject(this Transform parent, string name)
        {
            var mapObj = new GameObject(name);
            mapObj.transform.SetParent(parent);
            mapObj.transform.localPosition = Vector3.zero;
            mapObj.transform.localEulerAngles = Vector3.zero;
            return mapObj;
        }

        public static void LoadPrefabIfNotExist(this Transform parent, string path, string name)
        {
            var obj = parent.FindObjectByNameContain(name);
            if (obj)
            {
                return;
            }
            string pathFile = $"{path}/{name}.{FileNameExtension.PREFAB}";
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(pathFile);
            if (prefab)
            {
                var newObj = PrefabUtility.InstantiatePrefab(prefab, parent);
                parent.SetDirtyAndSave();
            }
        }

        public static void DestroyObjectByNameContain(this Transform parent, string name)
        {
            var obj = parent.FindObjectsByNameContain(name);
            foreach (var item in obj)
            {
                GameObject.DestroyImmediate(item.gameObject);
            }
        }
    }

    [System.Serializable]
    public class AnimatorOverrideConfig
    {
        public string Key;

        public string AnimationClipName;
    }

    public class FileNameExtension
    {
        public const string FBX = "fbx";

        public const string SCRIPTABLE_OBJECT = "asset";

        public const string PREFAB = "prefab";

        public const string MATERIAL = "mat";

        public const string ANIMATOR_OVERRIDE = "overrideController";
    }
}
#endif