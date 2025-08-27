using UnityEngine;

namespace NamPhuThuy.Common
{
    public static class CastExtension
{
    static CastResultData result;

    static CastResultData Result
    {
        get
        {
            if (result == null) result = new CastResultData();
            return result;
        }
    }
    static Vector3 GetStartPoint(Transform target, Vector3 offset)
    {
        Vector3 start = target.position;
        start += target.up * offset.y;
        start += target.right * offset.x;
        start += target.forward * offset.z;
        return start;
    }

    public static void DrawRay(this Transform target, Vector3 direction, Vector3 offset, float distance, Color color)
    {
        Vector3 castDirection = target.TransformDirection(direction);
        Vector3 start = GetStartPoint(target, offset);
        Vector3 end = start + castDirection * distance;
        Gizmos.color = color;
        Gizmos.DrawLine(start, end);
    }

    public static void DrawSphere(this Transform target, float radius, Vector3 direction, Vector3 offset, float distance, Color color)
    {
        Vector3 castDirection = target.TransformDirection(direction);
        Vector3 start = GetStartPoint(target, offset);
        Vector3 end = start + castDirection * distance;
        Gizmos.color = color;
        GizmosExtension.DrawCapsule(start, end, radius, color);
    }

    public static CastResultData SphereCast(this Transform target, Vector3 direction, Vector3 offset, float radius, float distance, int layer = 0, bool ignoreTrigger = true)
    {
        Vector3 castDirection = target.TransformDirection(direction);
        Vector3 start = GetStartPoint(target, offset);
        Result.IsHit = Physics.SphereCast(start, radius, castDirection, out Result.Hit, distance, layer, queryTriggerInteraction: (ignoreTrigger ? QueryTriggerInteraction.Ignore : QueryTriggerInteraction.UseGlobal));
        return Result;
    }

    public static CastResultData RayCast(this Transform target, Vector3 direction, Vector3 offset, float distance, int layer = 0)
    {
        Vector3 castDirection = target.TransformDirection(direction);
        Vector3 start = GetStartPoint(target, offset);
        Result.IsHit = Physics.Raycast(start, castDirection, out Result.Hit, distance, layer, queryTriggerInteraction: QueryTriggerInteraction.Ignore);
        return Result;
    }
}

[System.Serializable]
public class CastResultData
{
    public RaycastHit Hit;

    public bool IsHit;
}

[System.Serializable]
public class CastConfig
{
    public LayerMask Layer;

    public float Distance;

    public Vector3 StartOffset;

    public Vector3 Direction;

    public Color GizmosColor = Color.white;
}

[System.Serializable]
public class SphereCastConfig : CastConfig
{
    public float Radius;
}
}