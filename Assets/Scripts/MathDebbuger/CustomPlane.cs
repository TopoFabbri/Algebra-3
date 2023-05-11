using System.Collections;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;

public struct CustomPlane
{
    public Vec3 normal;
    public float distance;

    public CustomPlane(Vec3 inNormal, Vec3 inPoint)
    {
        normal = inNormal.normalized;
        distance = -Vec3.Dot(inNormal, inPoint);
    }

    public CustomPlane(Vec3 a, Vec3 b, Vec3 c)
    {
        normal = Vec3.Normalize(Vec3.Cross(b - a, c - a));
        distance = -Vec3.Dot(normal, a);
    }

    public void SetNormalAndPosition(Vec3 inNormal, Vec3 inPoint)
    {
        normal = inNormal.normalized;
        distance = -Vec3.Dot(inNormal, inPoint);
    }

    public void Set3Points(Vec3 a, Vec3 b, Vec3 c)
    {
        normal = Vec3.Cross(b - a, c - a).normalized;
        distance = -Vec3.Dot(normal, a);
    }

    public void Flip()
    {
        normal *= -1;
        distance *= -1;
    }

    public float GetDistanceToPoint(Vec3 point)
    {
        return Vec3.Dot(normal, point) + distance;
    }
    
    public bool GetSide(Vec3 point)
    {
        return GetDistanceToPoint(point) > 0;
    }

    public bool SameSide(Vec3 inPt0, Vec3 inPt1)
    {
        float d0 = GetDistanceToPoint(inPt0);
        float d1 = GetDistanceToPoint(inPt1);
        return (d0 > 0 && d1 > 0) || (d0 <= 0 && d1 <= 0);
    }

    public Vec3 ClosestPointOnPlane(Vec3 point)
    {
        float dist = GetDistanceToPoint(point);
        return point - (normal * dist);
    }

    public void Translate(Vec3 translation)
    {
        distance += Vec3.Dot(normal, translation);
    }

    public void Translate(float distance)
    {
        this.distance += distance;
    }

    public bool Raycast(Ray ray, out float enter)
    {
        float a = Vector3.Dot(ray.direction, this.normal);
        float num = -Vector3.Dot(ray.origin, this.normal) - this.distance;
        if (Mathf.Approximately(a, 0.0f))
        {
            enter = 0.0f;
            return false;
        }
        enter = num / a;
        return (double) enter > 0.0;
    }
}
