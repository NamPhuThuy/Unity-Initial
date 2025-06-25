using System.Collections.Generic;
using UnityEngine;

namespace NamPhuThuy
{
    public static class ExtensionTransform
    {
        //local pos
        public static void SetLocalPositionX(this Transform trans, float value)
        {
            var v = trans.localPosition;
            v.x = value;
            trans.localPosition = v;
        }
        public static void SetLocalPositionY(this Transform trans, float value)
        {
            var v = trans.localPosition;
            v.y = value;
            trans.localPosition = v;
        }
        public static void SetLocalPositionZ(this Transform trans, float value)
        {
            var v = trans.localPosition;
            v.z = value;
            trans.localPosition = v;
        }
        // pos
        public static void SetPositionX(this Transform trans, float value)
        {
            var v = trans.position;
            v.x = value;
            trans.position = v;
        }
        public static void SetPositionY(this Transform trans, float y)
        {
            var v = trans.position;
            v.y = y;
            trans.position = v;
        }
        public static void SetPositionZ(this Transform trans, float value)
        {
            var v = trans.position;
            v.z = value;
            trans.position = v;
        }
        // localscale
        public static void SetLocalScaleX(this Transform trans, float value)
        {
            var v = trans.localScale;
            v.x = value;
            trans.localScale = v;
        }
        public static void SetLocalScaleY(this Transform trans, float value)
        {
            var v = trans.localScale;
            v.y = value;
            trans.localScale = v;
        }
        public static void SetLocalScaleZ(this Transform trans, float value)
        {
            var v = trans.localScale;
            v.z = value;
            trans.localScale = v;
        }

        // rotation
        public static void SetEulerAngleZ(this Transform trans, float value)
        {
            var v = trans.eulerAngles;
            v.z = value;
            trans.eulerAngles = v;
        }
        public static void SetEulerAngleY(this Transform trans, float value)
        {
            var v = trans.eulerAngles;
            v.y = value;
            trans.eulerAngles = v;
        }
        public static void SetEulerAngleX(this Transform trans, float value)
        {
            var v = trans.eulerAngles;
            v.x = value;
            trans.eulerAngles = v;
        }

        public static void SetLocalEulerAngleX(this Transform trans, float value)
        {
            var v = trans.localEulerAngles;
            v.x = value;
            trans.localEulerAngles = v;
        }

        public static void SetLocalEulerAngleY(this Transform trans, float value)
        {
            var v = trans.localEulerAngles;
            v.y = value;
            trans.localEulerAngles = v;
        }
        public static void SetLocalEulerAngleZ(this Transform trans, float value)
        {
            var v = trans.localEulerAngles;
            v.z = value;
            trans.localEulerAngles = v;
        }
        public static float TotalNavmeshPathMagnitude(this Transform target, Vector3 destination)
        {
            UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
            UnityEngine.AI.NavMesh.CalculatePath(target.position, destination, UnityEngine.AI.NavMesh.AllAreas, path);
            float total = 0;
            for (int i = 1; i < path.corners.Length; i++)
            {
                total += (path.corners[i] - path.corners[i - 1]).magnitude;
            }
            return total;
        }
        public static List<GameObject> FindObjectsByName(this Transform transform, string name)
        {
            List<GameObject> taggedGameObjects = new List<GameObject>();

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.name.Equals(name))
                {
                    taggedGameObjects.Add(child.gameObject);
                }
                if (child.childCount > 0)
                {
                    taggedGameObjects.AddRange(FindObjectsByName(child, name));
                }
            }
            return taggedGameObjects;
        }

        public static List<GameObject> FindObjectsByNameContain(this Transform transform, List<string> names)
        {
            List<GameObject> taggedGameObjects = new List<GameObject>();

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                bool check = false;
                foreach (var item in names)
                {
                    if (child.name.Contains(item))
                    {
                        check = true;
                        break;
                    }
                }
                if (check)
                {
                    taggedGameObjects.Add(child.gameObject);
                }
                if (child.childCount > 0)
                {
                    taggedGameObjects.AddRange(FindObjectsByNameContain(child, names));
                }
            }
            return taggedGameObjects;
        }
        public static List<GameObject> FindObjectsByNameContain(this Transform transform, List<string> names, List<string> exceptNames)
        {
            List<GameObject> taggedGameObjects = new List<GameObject>();

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);

                bool check = false;
                foreach (var item in names)
                {
                    if (child.name.Contains(item))
                    {
                        check = true;
                        break;
                    }
                }
                foreach (var item in exceptNames)
                {
                    if (child.name.Contains(item))
                    {
                        check = false;
                        break;
                    }
                }
                if (check)
                {
                    taggedGameObjects.Add(child.gameObject);
                }
                if (child.childCount > 0)
                {
                    taggedGameObjects.AddRange(FindObjectsByNameContain(child, names));
                }
            }
            return taggedGameObjects;
        }
        public static List<GameObject> FindObjectsByNameContain(this Transform transform, string name)
        {
            List<GameObject> taggedGameObjects = new List<GameObject>();

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.name.Contains(name))
                {
                    taggedGameObjects.Add(child.gameObject);
                }
                if (child.childCount > 0)
                {
                    taggedGameObjects.AddRange(FindObjectsByNameContain(child, name));
                }
            }
            return taggedGameObjects;
        }
        public static GameObject FindObjectByNameContain(this Transform transform, string name)
        {
            Queue<Transform> check = transform.ChildsQueue();
            while (check.Count > 0)
            {
                var child = check.Dequeue();
                if (child.name.Contains(name))
                {
                    return child.gameObject;
                }
                else
                {
                    check.AddRange(child.ChildsQueue());
                }
            }
            return null;
        }
        public static GameObject FindObjectByName(this Transform transform, string name)
        {
            Queue<Transform> check = transform.ChildsQueue();
            while (check.Count > 0)
            {
                var child = check.Dequeue();
                if (child.name.Equals(name))
                {
                    return child.gameObject;
                }
                else
                {
                    check.AddRange(child.ChildsQueue());
                }
            }
            return null;
        }
        public static GameObject FindObjectWithTag(this Transform transform, string tag)
        {
            Queue<Transform> check = transform.ChildsQueue();
            while (check.Count > 0)
            {
                var child = check.Dequeue();
                if (child.CompareTag(tag))
                {
                    return child.gameObject;
                }
                else
                {
                    check.AddRange(child.ChildsQueue());
                }
            }
            return null;
        }
        public static Queue<Transform> ChildsQueue(this Transform transform)
        {
            Queue<Transform> result = new Queue<Transform>();

            for (int i = 0; i < transform.childCount; i++)
            {
                result.Enqueue(transform.GetChild(i));
            }
            return result;
        }
        public static List<Transform> ChildsList(this Transform transform)
        {
            List<Transform> result = new List<Transform>();

            for (int i = 0; i < transform.childCount; i++)
            {
                result.Add(transform.GetChild(i));
            }
            return result;
        }
        public static List<GameObject> FindObjectsWithTag(this Transform parent, string tag)
        {
            List<GameObject> taggedGameObjects = new List<GameObject>();

            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                if (child.CompareTag(tag))
                {
                    taggedGameObjects.Add(child.gameObject);
                }
                if (child.childCount > 0)
                {
                    taggedGameObjects.AddRange(FindObjectsWithTag(child, tag));
                }
            }

            return taggedGameObjects;
        }
        public static List<Transform> FindTransformsWithTag(this Transform parent, string tag)
        {
            List<Transform> taggedGameObjects = new List<Transform>();

            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                if (child.CompareTag(tag))
                {
                    taggedGameObjects.Add(child.transform);
                }
                if (child.childCount > 0)
                {
                    taggedGameObjects.AddRange(FindTransformsWithTag(child, tag));
                }
            }
            return taggedGameObjects;
        }

        public static bool IsInRange(this Transform transform, Vector3 target, float range)
        {
            return transform.position.IsInRange(target, range);
        }

        public static List<Vector3> ToVectors(this List<Transform> transform)
        {
            List<Vector3> points = new List<Vector3>();
            foreach (var item in transform)
            {
                if (item)
                {
                    points.Add(item.position);
                }
            }
            return points;
        }

        public static List<Transform> ToTransforms(this List<GameObject> transform)
        {
            List<Transform> points = new List<Transform>();
            foreach (var item in transform)
            {
                if (item)
                {
                    points.Add(item.transform);
                }
            }
            return points;
        }
        public static IEnumerable<Transform> ToTransforms<T>(this IEnumerable<T> list) where T : MonoBehaviour
        {
            foreach (var item in list)
            {
                yield return item.transform;
            }
        }

        public static List<Transform> OutRange(this List<Transform> transforms, Vector3 position, float distance)
        {
            List<Transform> result = new List<Transform>();
            foreach (var item in transforms)
            {
                var inRange = (item.position - position).sqrMagnitude > distance * distance;
                if (inRange)
                {
                    result.Add(item);
                }
            }
            return result;
        }

        public static List<Transform> InRange(this List<Transform> transforms, Vector3 position, float minDistance, float maxDistance)
        {
            List<Transform> result = new List<Transform>();
            foreach (var item in transforms)
            {
                float distanceSq = (item.position - position).sqrMagnitude;
                float minDistanceSq = minDistance * minDistance;
                float maxDistanceSq = maxDistance * maxDistance;
                if (distanceSq > minDistanceSq && distanceSq < maxDistanceSq)
                {
                    result.Add(item);
                }
            }
            return result;
        }

        public static void CharacterLook(this Transform transform, Transform target)
        {
            transform.rotation = transform.GetCharacterLookRotation(target);
        }

        public static Quaternion GetCharacterLookRotation(this Transform transform, Transform target)
        {
            return GetCharacterLookRotation(transform, target.position);
        }

        public static Quaternion GetCharacterLookRotation(this Transform transform, Vector3 target)
        {
            var targetPos = target;
            var currentPos = transform.position;
            targetPos.y = 0;
            currentPos.y = 0;
            var dir = targetPos - currentPos;
            var targetRotation = Quaternion.LookRotation(dir);
            return targetRotation;
        }

        public static T Nearest<T>(this IEnumerable<T> list, Vector3 target) where T : Component
        {
            T best = null;
            float bestDistance = float.MaxValue;
            foreach (var item in list)
            {
                float distance = Vector3.Distance(target, item.transform.position);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    best = item;
                }
            }
            return best;
        }

        public static T Furthest<T>(this IEnumerable<T> list, Vector3 target) where T : Component
        {
            T best = null;
            float bestDistance = float.MaxValue;
            foreach (var item in list)
            {
                float distance = Vector3.Distance(target, item.transform.position);
                if (distance > bestDistance)
                {
                    bestDistance = distance;
                    best = item;
                }
            }
            return best;
        }

    }
}
