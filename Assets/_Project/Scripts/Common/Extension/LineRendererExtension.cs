using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NamPhuThuy.Common
{
    public static class LineRendererExtension
    {
        public static Vector3[] GetPoints(this LineRenderer line)
        {
            Vector3[] result = new Vector3[line.positionCount];
            line.GetPositions(result);
            return result;
        }

        public static List<Vector3> ListPosition(this LineRenderer line)
        {
            Vector3[] result = new Vector3[line.positionCount];
            line.GetPositions(result);
            return result.ToList();
        }
    }
}