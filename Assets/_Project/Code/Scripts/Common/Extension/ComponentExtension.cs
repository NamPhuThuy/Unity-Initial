using System;
using System.Collections.Generic;
using UnityEngine;

namespace NamPhuThuy.Common
{
    public static class ComponentExtension
    {
        public static bool SameID<T>(this T comp, T other) where T : Component
        {
            return comp.GetInstanceID() == other.GetInstanceID();
        }

        public static void RemoveComponent<T>(this List<T> list, T item) where T : Component
        {
            list.RemoveAll(x => x.GetInstanceID() == item.GetInstanceID());
        }

        public static void RemoveComponent<T>(this List<T> list, int instanceID) where T : Component
        {
            list.RemoveAll(x => x.GetInstanceID() == instanceID);
        }

        public static T PopComponent<T>(this List<T> list, T item) where T : Component
        {
            var index = list.FindIndex(x => x.GetInstanceID() == item.GetInstanceID());
            T result = list[index];
            list.RemoveAt(index);
            return result;
        }

        public static T FindComponent<T>(this List<T> list, T item) where T : Component
        {
            return list.Find(x => x.GetInstanceID() == item.GetInstanceID());
        }
        public static bool ContainComponent<T>(this IEnumerable<T> list, T findItem) where T : Component
        {
            foreach (var item in list)
            {
                if (item.GetInstanceID() == findItem.GetInstanceID())
                {
                    return true;
                }
            }
            return false;
        }
        public static void AddUniqueComponent<T>(this List<T> list, T item) where T : Component
        {
            if (!list.ContainComponent(item))
            {
                list.Add(item);
            }
        }

        public static T GetNearest<T>(this List<T> list, Vector3 position) where T : Component
        {
            return GetBestByDistance(list, position, (distance, current) => { return distance < current; });
        }
        public static List<Transform> ToTransforms<T>(this List<T> list) where T : Component
        {
            List<Transform> transforms = new List<Transform>();
            foreach (var item in list)
            {
                transforms.Add(item.transform);
            }
            return transforms;
        }


        public static T GetFurthest<T>(this List<T> list, Vector3 position) where T : Component
        {
            return GetBestByDistance(list, position, (distance, current) => { return distance > current; });
        }

        static T GetBestByDistance<T>(this List<T> list, Vector3 position, Func<float, float, bool> predicate) where T : Component
        {
            if (list.Count == 0) return null;
            T nearest = list[0];
            float bestDistance = Vector3.SqrMagnitude(position - nearest.transform.position);
            for (int i = 1; i < list.Count; i++)
            {
                float distance = Vector3.SqrMagnitude(position - list[i].transform.position);
                if (predicate(distance, bestDistance))
                {
                    bestDistance = distance;
                    nearest = list[i];
                }
            }
            return nearest;
        }

    }
}