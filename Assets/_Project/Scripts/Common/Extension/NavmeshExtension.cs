using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace NamPhuThuy.Common
{
    public static class NavmeshExtension
    {
        public static Vector3 SamplePosition(this Vector3 position, float distance)
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(position, out hit, distance, 1);
            Vector3 finalPosition = hit.position;
            return finalPosition;
        }

        public static float TotalNavmeshPathMagnitude(Vector3 target, Vector3 destination, float sampleDistance)
        {
            var path = GetPath(target, destination, sampleDistance);
            float total = 0;
            for (int i = 1; i < path.Length; i++)
            {
                total += (path[i] - path[i - 1]).magnitude;
            }
            return total;
        }

        public static Vector3[] GetPath(this Vector3 point, Vector3 target, float sampleDistance)
        {
            Vector3[] defaultResult = new Vector3[0];
            Vector3 pointSamplePosition = SamplePosition(point, sampleDistance);
            Vector3 targetSamplePostition = SamplePosition(target, sampleDistance);
            if (targetSamplePostition.magnitude > 9999999 || pointSamplePosition.magnitude > 9999999)
            {
                return defaultResult;
            }
            var path = new NavMeshPath();
            NavMesh.CalculatePath(pointSamplePosition, targetSamplePostition, NavMesh.AllAreas, path);
            return path.corners;
        }

        public static List<Vector3> GetMovablePoints(this Vector3 position, float height, int numRay = 20, float range = 15f)
        {
            Vector3 straightDirection = Vector3.forward;
            Vector3 destination = position.GetMovablePoint(straightDirection, range);
            List<Vector3> destinations = new List<Vector3>();
            destinations.Add(destination);
            float angleStep = 180f / numRay;
            for (int i = 1; i <= numRay; i++)
            {
                var directionLeft = Quaternion.AngleAxis(-angleStep * i, Vector3.up) * straightDirection;
                var directionRight = Quaternion.AngleAxis(angleStep * i, Vector3.up) * straightDirection;
                destinations.Add(position.GetMovablePoint(directionLeft, range));
                destinations.Add(position.GetMovablePoint(directionRight, range));
            }
            return destinations;
        }

        public static Vector3 GetFurthestInNav(this IEnumerable<Vector3> temp, Vector3 target, float sampleDistance)
        {
            float maxMagnitue = -1;
            Vector3 finalDestination = Vector3.zero;
            foreach (var item in temp)
            {
                var magnitude = TotalNavmeshPathMagnitude(target, item, sampleDistance);
                if (magnitude > maxMagnitue)
                {
                    maxMagnitue = magnitude;
                    finalDestination = item;
                }
            }
            return finalDestination;
        }

        public static T GetNearestInNav<T>(this IEnumerable<T> list, Vector3 target, float sampleDistance) where T : Component
        {
            T nearest = null;
            float bestDistance = float.MaxValue;
            foreach (var item in list)
            {
                float distance = TotalNavmeshPathMagnitude(target, item.transform.position, sampleDistance);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    nearest = item;
                }
            }
            return nearest;
        }

        public static Vector3 GetMovablePoint(this Vector3 position, Vector3 direction, float maxDistance)
        {
            Vector3 infor = position + direction.normalized * maxDistance;
            var point = SamplePosition(infor, 20f);
            NavMeshHit hit;
            bool isHitted = NavMesh.Raycast(position, point, out hit, NavMesh.AllAreas);
            if (isHitted)
            {
                infor = hit.position;
                return infor;
            }
            else
            {
                return point;
            }
        }

        public static bool HasPath(Vector3 position, Vector3 target)
        {
            var path = new NavMeshPath();
            NavMesh.CalculatePath(position, target, NavMesh.AllAreas, path);
            return path.status == NavMeshPathStatus.PathComplete;
        }
    }
}