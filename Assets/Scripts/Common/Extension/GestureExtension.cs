using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NamPhuThuy.Common
{
    public static class GestureExtension
    {

        public static Touch GetTouchByFingerID(int id)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                var touch = Input.GetTouch(i);
                if (touch.fingerId == id)
                {
                    return touch;
                }
            }
            return default(Touch);
        }
        public static Vector3 TouchPosition
        {
            get
            {
#if (UNITY_ANDROID||UNITY_IOS)&&!UNITY_EDITOR
                return Input.GetTouch(0).position;
#endif
                return Input.mousePosition;
            }
        }

        public static bool IsMouseOverGameObject()
        {
#if (UNITY_ANDROID||UNITY_IOS)&&!UNITY_EDITOR
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId)) return true;
            }
            return false;
#endif
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}