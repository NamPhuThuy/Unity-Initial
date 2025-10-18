using UnityEngine;

namespace NamPhuThuy.Common
{
    /// <summary>
    /// You can use this script to capture screenshots in Unity.
    /// If you want to capture screenshots in the game, attach this script to any GameObject in your scene. Then press the 'G' key to capture a screenshot.
    /// </summary>
    public class ScreenShot : MonoBehaviour
    {
        public string fileType = "jpg";
        public KeyCode captureKey = KeyCode.G;
        public KeyCode resetIndexKey = KeyCode.P;
        public int index = 0;

        #region MonoBehaviour Callbacks

        void Update()
        {
            if (Input.GetKeyDown(captureKey))
            {
                index++;
                Capture();
            }

            if (Input.GetKeyDown(resetIndexKey))
            {
                index = 0;
            }
        }   

        #endregion

        #region Private Methods

        private void Capture()
        {
            string fullPath = Application.persistentDataPath + "/" + Screen.width + "_" + Screen.height + "_" + index +"." + fileType;

            ScreenCapture.CaptureScreenshot(fullPath);

            Debug.Log("Screenshot captured: " + fullPath);
        }

        #endregion
    }
}
