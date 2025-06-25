using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NamPhuThuy
{
    public static class ExtensionVector3
    {
        public static Vector3 GetRandomInRange(this Vector3 pos, float rangeX, float rangeY, float rangeZ)
        {
            float randX = UnityEngine.Random.Range(pos.x - rangeX, pos.x + rangeX);
            float randY = UnityEngine.Random.Range(pos.y - rangeY, pos.y + rangeY);
            float randZ = UnityEngine.Random.Range(pos.z - rangeZ, pos.z + rangeZ);
            return new Vector3(randX, randY, randZ);
        }

        public static Vector3 GetRandomInRange(this Vector3 pos, float range)
        {
            float randX = UnityEngine.Random.Range(pos.x - range, pos.x + range);
            float randY = UnityEngine.Random.Range(pos.y - range, pos.y + range);
            float randZ = UnityEngine.Random.Range(pos.y - range, pos.y + range);
            return new Vector3(randX, randY, randZ);
        }

        public static Vector3 Round(this Vector3 vector3, int decimalPlaces = 2)
        {
            return new Vector3(
                (float)Math.Round(vector3.x, decimalPlaces),
               (float)Math.Round(vector3.y, decimalPlaces),
               (float)Math.Round(vector3.z, decimalPlaces));
        }

        public static Vector3 GetProjectionPoint(this Vector3 point, Vector3 lineVect, Vector3 linePoint)
        {
            float t = 0;
            if (lineVect.sqrMagnitude > 0.0001f)
            {
                t = -((linePoint.x - point.x) * lineVect.x
                      + (linePoint.y - point.y) * lineVect.y
                      + (linePoint.z - point.z) * lineVect.z)
                        / lineVect.sqrMagnitude; ;
            }
            Vector3 result = new Vector3(
                linePoint.x + t * lineVect.x,
                linePoint.y + t * lineVect.y,
                linePoint.z + t * lineVect.z);
            return result;
        }

        public static Vector3 GetMiddlePoint(Vector3 pointA, Vector3 pointB)
        {
            return (pointA + pointB) / 2;
        }

        public static List<Vector3> GetConeDirections(this Vector3 direction, float angle, int numOfRay)
        {
            List<Vector3> vects = new List<Vector3>();
            var vector = Quaternion.AngleAxis(-angle, Vector3.up) * direction;
            vects.Add(vector);
            var vector2 = Quaternion.AngleAxis(angle, Vector3.up) * direction;
            vects.Add(vector2);
            float anglePerStep = angle * 2 / numOfRay;
            for (int i = 1; i <= numOfRay; i++)
            {
                var dir = Quaternion.AngleAxis(-angle + anglePerStep * i, Vector3.up) * direction;
                vects.Add(dir);
            }
            return vects;
        }

        public static bool IsInRange(this Vector3 position, Vector3 target, float range)
        {
            return Vector3.SqrMagnitude(position - target) <= range * range;
        }

        public static float TotalDistance(this IEnumerable<Vector3> list)
        {
            float distance = 0;
            bool getLast = false;
            Vector3 last = Vector3.zero;
            foreach (var item in list)
            {
                if (getLast)
                {
                    distance += Vector3.Distance(last, item);
                }
                last = item;
                getLast = true;
            }
            return distance;
        }

        public static List<Vector3> Split(this List<Vector3> list, float distance)
        {
            distance = Mathf.Max(distance, 0.015f);
            int pointCount = Mathf.RoundToInt(list.TotalDistance() / distance);
            return list.Split(pointCount);
        }

        public static List<Vector3> Split(this List<Vector3> list, int pointCount)
        {
            pointCount = Mathf.Max(list.Count, pointCount);
            float distance = list.TotalDistance() / (pointCount - 1);
            List<Vector3> result = new List<Vector3>();
            result.Add(list.FirstElement());
            float tempDistance = 0;
            if (list.Count == 0 || distance < 0.01f) return list;
            //
            for (int i = 0; i < list.Count - 1; i++)
            {
                Vector3 temp = list[i];
                while (true)
                {
                    temp = Vector3.MoveTowards(temp, list[i + 1], distance - tempDistance);
                    tempDistance = 0;
                    result.Add(new Vector3(temp.x, temp.y, temp.z));
                    float distanceBetween = Vector3.Distance(temp, list[i + 1]);
                    if (distanceBetween < distance)
                    {
                        tempDistance = distanceBetween;
                        break;
                    }
                }
            }
            result.Add(list.LastElement());
            return result;
        }

        public static float GetRightZAngle(this Vector3 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            return angle;
        }

        public static List<Vector3> ToPoints(this EdgeCollider2D edge)
        {
            List<Vector3> result = new List<Vector3>();
            foreach (var item in edge.points)
            {
                result.Add(item);
            }
            return result;
        }

        public static bool IsInRect(this Vector3 point, Vector3 rectPosition, Vector3 rectSize)
        {
            return Mathf.Abs(point.x - rectPosition.x) < rectSize.x
               && Mathf.Abs(point.y - rectPosition.y) < rectSize.y
               && Mathf.Abs(point.z - rectPosition.z) < rectSize.z;
        }

        public static Vector3 ClosestPointOnLineSegment(float px, float py, float pz, float ax, float ay, float az, float bx, float by, float bz)
        {
            float apx = px - ax;
            float apy = py - ay;
            float apz = pz - az;
            float abx = bx - ax;
            float aby = by - ay;
            float abz = bz - az;
            float abMag = abx * abx + aby * aby + abz * abz; // Sqr magnitude.
            if (abMag < Mathf.Epsilon) return new Vector3(ax, ay, az);
            // Normalize.
            abMag = Mathf.Sqrt(abMag);
            abx /= abMag;
            aby /= abMag;
            abz /= abMag;
            float mu = abx * apx + aby * apy + abz * apz; // Dot.
            if (mu < 0) return new Vector3(ax, ay, az);
            if (mu > abMag) return new Vector3(bx, by, bz);
            return new Vector3(ax + abx * mu, ay + aby * mu, az + abz * mu);
        }
        /// <summary>
        /// Closest point on a line segment from a given point in 3D.
        /// </summary>
        public static Vector3 ClosestPointOnLineSegment(Vector3 p, Vector3 a, Vector3 b)
        {
            return ClosestPointOnLineSegment(p.x, p.y, p.z, a.x, a.y, a.z, b.x, b.y, b.z);
        }
    }
}
