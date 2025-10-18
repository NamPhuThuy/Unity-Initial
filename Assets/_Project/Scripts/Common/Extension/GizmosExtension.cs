using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NamPhuThuy.Common
{
    public static class GizmosExtension
    {
        public static void DrawCapsule(Vector3 startPos, Vector3 endPos, float radius, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(startPos, radius);
            Gizmos.DrawWireSphere(endPos, radius);
            var v1 = Vector3.Cross(Vector3.left, startPos - endPos).normalized * radius;
            var v2 = Vector3.Cross(Vector3.forward, startPos - endPos).normalized * radius;
            Gizmos.DrawLine(startPos + v1, endPos + v1);
            Gizmos.DrawLine(startPos - v1, endPos - v1);
            Gizmos.DrawLine(startPos + v2, endPos + v2);
            Gizmos.DrawLine(startPos - v2, endPos - v2);
        }
    }
}
