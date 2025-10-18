using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NamPhuThuy.Common
{
    public static class GameObjectExtension
    {
        public static void SetActive<T>(this IEnumerable<T> list, bool isActive) where T : MonoBehaviour
        {
            foreach (var item in list)
            {
                item.gameObject.SetActive(isActive);
            }
        }
        public static IEnumerator IEActive<T>(this IEnumerable<T> list, float time = 0.1f) where T : MonoBehaviour
        {
            foreach (var item in list)
            {
                item.gameObject.SetActive(true);
                yield return new WaitForSeconds(time);
            }
        }

        public static T AddUniqueComponent<T>(this GameObject obj) where T : Component
        {
            T comp = obj.GetComponent<T>();
            if (!comp)
            {
                comp = obj.AddComponent<T>();
            }
            return comp;
        }
    }
}
