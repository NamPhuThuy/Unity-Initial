using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NamPhuThuy.Common
{
    public static class CanvasUIExtension
    {
        public static void SetSprite(this Image image, Sprite sprite)
        {
            image.sprite = sprite;
            image.SetNativeSize();
        }
        public static void SetAlpha(this Graphic image, float alpha)
        {
            var color = image.color;
            color.a = alpha;
            image.color = color;
        }
        public static void SetAlpha(this SpriteRenderer image, float alpha)
        {
            var color = image.color;
            color.a = alpha;
            image.color = color;
        }
        public static void SetPivot(this RectTransform rectTransform, Vector2 pivot)
        {
            if (rectTransform == null) return;
            Vector2 size = rectTransform.rect.size;
            Vector2 deltaPivot = rectTransform.pivot - pivot;
            Vector3 deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);
            rectTransform.pivot = pivot;
            rectTransform.localPosition -= deltaPosition;
        }

        public static Vector2 ConvertToCanvasAnchorPos(this Vector3 worldPosition, Camera camera, RectTransform canvasRect)
        {
            Vector2 ViewportPosition = camera.WorldToViewportPoint(worldPosition);
            Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));
            //now you can set the position of the ui element
            return WorldObject_ScreenPosition;
        }
    }

}   