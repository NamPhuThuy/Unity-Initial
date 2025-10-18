using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace NamPhuThuy.Common
{
    public class ImageDownloader : MonoBehaviour
    {
        [Header("UI References")]
        public Image displayImage;
        public Button downloadButton;
        public Slider progressSlider;
        public Text statusText;
        public InputField urlInputField;
        
        [Header("Download Settings")]
        public string downloadFolder = "DownloadedImages";
        public bool saveToGallery = true;
        public bool cacheImages = true;
        public int maxCacheSize = 100; // MB
        
        [Header("Image Settings")]
        public int maxImageWidth = 2048;
        public int maxImageHeight = 2048;
        public TextureFormat textureFormat = TextureFormat.RGB24;
        
        private Dictionary<string, Texture2D> imageCache;
        private Queue<string> cacheOrder;
        private long currentCacheSize;
        
        #region MonoBehaviour Callbacks
        void Start()
        {
            InitializeDownloader();
        }
        
        void OnDestroy()
        {
            ClearCache();
        }
        
        
        #endregion
        
        void InitializeDownloader()
        {
            imageCache = new Dictionary<string, Texture2D>();
            cacheOrder = new Queue<string>();
            currentCacheSize = 0;
            
            if (downloadButton != null)
                downloadButton.onClick.AddListener(OnDownloadButtonClicked);
            
            // Create download directory
            CreateDownloadDirectory();
            
            UpdateUI("Ready to download images");
            
            void CreateDownloadDirectory()
            {
                string path = GetDownloadPath();
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    // Debug.Log($"Created download directory: {path}");
                }
            }
        }
        
        
        
        string GetDownloadPath()
        {
            return Path.Combine(Application.persistentDataPath, downloadFolder);
        }
        
        public void OnDownloadButtonClicked()
        {
            if (urlInputField != null && !string.IsNullOrEmpty(urlInputField.text))
            {
                DownloadImage(urlInputField.text);
            }
            else
            {
                UpdateUI("Please enter a valid URL");
            }
        }
        
        // Main download method with multiple options
        public void DownloadImage(string url, Action<Texture2D> onSuccess = null, Action<string> onError = null)
        {
            // Check cache first
            if (cacheImages && imageCache.ContainsKey(url))
            {
                // Debug.Log("Loading image from cache");
                var cachedTexture = imageCache[url];
                DisplayImage(cachedTexture);
                onSuccess?.Invoke(cachedTexture);
                return;
            }
            
            StartCoroutine(DownloadImageCoroutine(url, onSuccess, onError));
        }
        
        IEnumerator DownloadImageCoroutine(string url, Action<Texture2D> onSuccess, Action<string> onError)
        {
            UpdateUI("Starting download...", 0f);
            
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
            {
                // Set timeout
                request.timeout = 30;
                
                // Start download
                var operation = request.SendWebRequest();
                
                // Track progress
                while (!operation.isDone)
                {
                    UpdateUI($"Downloading... {(operation.progress * 100):F0}%", operation.progress);
                    yield return null;
                }
                
                if (request.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(request);
                        
                        if (downloadedTexture != null)
                        {
                            // Process the image
                            var processedTexture = ProcessDownloadedImage(downloadedTexture);
                            
                            // Cache the image
                            if (cacheImages)
                            {
                                CacheImage(url, processedTexture);
                            }
                            
                            // Save to device
                            StartCoroutine(SaveImageToDevice(processedTexture, GetFileNameFromUrl(url)));
                            
                            // Display the image
                            DisplayImage(processedTexture);
                            
                            UpdateUI("Download completed successfully!", 1f);
                            onSuccess?.Invoke(processedTexture);
                        }
                        else
                        {
                            string error = "Failed to process downloaded image";
                            UpdateUI(error);
                            onError?.Invoke(error);
                        }
                    }
                    catch (Exception e)
                    {
                        string error = $"Error processing image: {e.Message}";
                        UpdateUI(error);
                        onError?.Invoke(error);
                    }
                }
                else
                {
                    string error = $"Download failed: {request.error}";
                    UpdateUI(error);
                    onError?.Invoke(error);
                }
            }
        }
        
        Texture2D ProcessDownloadedImage(Texture2D originalTexture)
        {
            // Resize if necessary
            if (originalTexture.width > maxImageWidth || originalTexture.height > maxImageHeight)
            {
                return ResizeTexture(originalTexture, maxImageWidth, maxImageHeight);
            }
            
            return originalTexture;
        }
        
        Texture2D ResizeTexture(Texture2D original, int maxWidth, int maxHeight)
        {
            float aspectRatio = (float)original.width / original.height;
            int newWidth, newHeight;
            
            if (aspectRatio > 1) // Landscape
            {
                newWidth = Mathf.Min(maxWidth, original.width);
                newHeight = Mathf.RoundToInt(newWidth / aspectRatio);
            }
            else // Portrait or Square
            {
                newHeight = Mathf.Min(maxHeight, original.height);
                newWidth = Mathf.RoundToInt(newHeight * aspectRatio);
            }
            
            RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
            Graphics.Blit(original, rt);
            
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = rt;
            
            Texture2D resizedTexture = new Texture2D(newWidth, newHeight, textureFormat, false);
            resizedTexture.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
            resizedTexture.Apply();
            
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(rt);
            
            return resizedTexture;
        }
        
        void CacheImage(string url, Texture2D texture)
        {
            // Calculate texture size in bytes
            long textureSize = texture.width * texture.height * 4; // Assuming RGBA32
            
            // Check if cache needs cleanup
            while (currentCacheSize + textureSize > maxCacheSize * 1024 * 1024 && cacheOrder.Count > 0)
            {
                string oldestUrl = cacheOrder.Dequeue();
                if (imageCache.ContainsKey(oldestUrl))
                {
                    var oldTexture = imageCache[oldestUrl];
                    currentCacheSize -= oldTexture.width * oldTexture.height * 4;
                    Destroy(oldTexture);
                    imageCache.Remove(oldestUrl);
                }
            }
            
            // Add to cache
            imageCache[url] = texture;
            cacheOrder.Enqueue(url);
            currentCacheSize += textureSize;
            
            // Debug.Log($"Cached image. Cache size: {currentCacheSize / (1024 * 1024)}MB");
        }
        
        IEnumerator SaveImageToDevice(Texture2D texture, string fileName)
        {
            try
            {
                byte[] imageData = texture.EncodeToPNG();
                string filePath = Path.Combine(GetDownloadPath(), fileName);
                
                // Save to app directory
                File.WriteAllBytes(filePath, imageData);
                // Debug.Log($"Image saved to: {filePath}");
                
                // Save to gallery (platform-specific)
                if (saveToGallery)
                {
                    StartCoroutine(SaveToGallery(imageData, fileName));
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save image: {e.Message}");
            }

            yield return null;
        }
        
        IEnumerator SaveToGallery(byte[] imageData, string fileName)
        {
    #if UNITY_ANDROID && !UNITY_EDITOR
            yield return StartCoroutine(SaveToGalleryAndroid(imageData, fileName));
    #elif UNITY_IOS && !UNITY_EDITOR
            SaveToGalleryIOS(imageData);
            yield return null;
    #else
            // Debug.Log("Gallery save not supported in editor");
            yield return null;
    #endif
        }
        
    #if UNITY_ANDROID && !UNITY_EDITOR
        IEnumerator SaveToGalleryAndroid(byte[] imageData, string fileName)
        {
            // Request permission first
            if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.ExternalStorageWrite))
            {
                UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.ExternalStorageWrite);
                
                // Wait for permission response
                float timeout = 10f;
                while (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.ExternalStorageWrite) && timeout > 0)
                {
                    timeout -= Time.deltaTime;
                    yield return null;
                }
                
                if (timeout <= 0)
                {
                    Debug.LogWarning("Permission timeout");
                    yield break;
                }
            }
            
            // Save using NativeGallery plugin or custom Android implementation
            try
            {
                string tempPath = Path.Combine(Application.temporaryCachePath, fileName);
                File.WriteAllBytes(tempPath, imageData);
                
                // Use Android MediaScannerConnection to add to gallery
                using (AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (AndroidJavaObject currentActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity"))
                using (AndroidJavaClass mediaClass = new AndroidJavaClass("android.media.MediaScannerConnection"))
                {
                    mediaClass.CallStatic("scanFile", currentActivity, new string[] { tempPath }, null, null);
                }
                
                // Debug.Log("Image saved to Android gallery");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save to Android gallery: {e.Message}");
            }
        }
    #endif

    #if UNITY_IOS && !UNITY_EDITOR
        void SaveToGalleryIOS(byte[] imageData)
        {
            try
            {
                // Save using iOS native functionality
                // This requires iOS native plugin or NativeGallery asset
                // Debug.Log("Saving to iOS Photos app");
                // Implementation depends on your iOS plugin
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save to iOS gallery: {e.Message}");
            }
        }
    #endif
        
        void DisplayImage(Texture2D texture)
        {
            if (displayImage != null && texture != null)
            {
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                displayImage.sprite = sprite;
                
                // Fit image to display area
                FitImageToDisplay();
            }
        }
        
        void FitImageToDisplay()
        {
            if (displayImage != null && displayImage.sprite != null)
            {
                displayImage.preserveAspect = true;
                displayImage.type = Image.Type.Simple;
            }
        }
        
        string GetFileNameFromUrl(string url)
        {
            try
            {
                Uri uri = new Uri(url);
                string fileName = Path.GetFileName(uri.LocalPath);
                
                if (string.IsNullOrEmpty(fileName) || !fileName.Contains("."))
                {
                    fileName = $"image_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                }
                
                return fileName;
            }
            catch
            {
                return $"image_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            }
        }
        
        void UpdateUI(string message, float progress = -1f)
        {
            if (statusText != null)
                statusText.text = message;
            
            if (progressSlider != null)
            {
                if (progress >= 0f)
                {
                    progressSlider.gameObject.SetActive(true);
                    progressSlider.value = progress;
                }
                else
                {
                    progressSlider.gameObject.SetActive(false);
                }
            }
            
            // Debug.Log($"ImageDownloader: {message}");
        }
        
        // Public API methods
        public void DownloadFromClipboard()
        {
            string clipboardText = GUIUtility.systemCopyBuffer;
            if (Uri.IsWellFormedUriString(clipboardText, UriKind.Absolute))
            {
                DownloadImage(clipboardText);
            }
            else
            {
                UpdateUI("Clipboard doesn't contain a valid URL");
            }
        }
        
        public void ClearCache()
        {
            foreach (var texture in imageCache.Values)
            {
                if (texture != null)
                    Destroy(texture);
            }
            
            imageCache.Clear();
            cacheOrder.Clear();
            currentCacheSize = 0;
            
            UpdateUI("Cache cleared");
        }
        
        public void LoadLocalImage(string fileName)
        {
            string filePath = Path.Combine(GetDownloadPath(), fileName);
            if (File.Exists(filePath))
            {
                StartCoroutine(LoadLocalImageCoroutine(filePath));
            }
            else
            {
                UpdateUI($"Local file not found: {fileName}");
            }
        }
        
        IEnumerator LoadLocalImageCoroutine(string filePath)
        {
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture("file://" + filePath))
            {
                yield return request.SendWebRequest();
                
                if (request.result == UnityWebRequest.Result.Success)
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(request);
                    DisplayImage(texture);
                    UpdateUI($"Loaded local image: {Path.GetFileName(filePath)}");
                }
                else
                {
                    UpdateUI($"Failed to load local image: {request.error}");
                }
            }
        }
        
        
    }
}