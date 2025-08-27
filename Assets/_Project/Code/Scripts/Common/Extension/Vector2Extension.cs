using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NamPhuThuy.Common
{
    public static class Vector2Extension
    {
        public static IEnumerable<Vector3> IEnumerableVector3(this IEnumerable<Vector2> vectors)
        {
            foreach (var item in vectors)
            {
                yield return item;
            }
        }
    }

}