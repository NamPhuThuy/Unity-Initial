using UnityEngine;

namespace NamPhuThuy.Common
{
    [RequireComponent(typeof(SpriteRenderer))]

    public class SpriteAutoScaler : MonoBehaviour
    {
        #region MonoBehaviour Callbacks

        private void Start()
        {
            ScaleSpriteToScreen();
        }

        #endregion

        #region Private Methods

        private void ScaleSpriteToScreen()
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();

            if (sr.sprite == null)
            {
                Debug.LogWarning("SpriteAutoScaler: No sprite assigned to SpriteRenderer.");
                return;
            }

            // Maintain aspect ratio 
            Vector2 spriteSize = sr.sprite.bounds.size;

            float screenHeight = Camera.main.orthographicSize * 2f;
            float screenWidth = screenHeight * Screen.width / Screen.height;

            float scaleX = screenWidth / spriteSize.x;
            float scaleY = screenHeight / spriteSize.y;

            float finalScale = Mathf.Max(scaleX, scaleY);

            transform.localScale = new Vector3(finalScale, finalScale, 1f);
        }

        #endregion
    }
}
