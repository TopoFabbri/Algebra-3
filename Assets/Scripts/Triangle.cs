using CustomMath;
using Unity.VisualScripting;
using UnityEngine;
using Color = UnityEngine.Color;

struct Triangle
{
    private Vec3[] verts;
    private CustomPlane plane;

    public Triangle(Vec3[] verts, CustomPlane plane)
    {
        this.verts = verts;
        this.plane = plane;
    }

    public bool LineCollides(Vec3 start, Vec3 dir)
    {
        bool doesIntersect = false;
        float dis;
        Ray ray = new Ray(start, dir);

        if (plane.Raycast(ray, out dis))
            doesIntersect = TriangleCollides(ray.GetPoint(dis));

        return doesIntersect;
    }

    private bool TriangleCollides(Vec3 intersection)
    {
        Vec3 v1 = verts[1] - verts[0];
        Vec3 v2 = verts[2] - verts[0];

        Vec3 p = intersection - verts[0];
        float dot11 = Vec3.Dot(v1, v1);
        float dot12 = Vec3.Dot(v1, v2);
        float dot1P = Vec3.Dot(v1, p);
        float dot22 = Vec3.Dot(v2, v2);
        float dot2P = Vec3.Dot(v2, p);

        // u = (Dot(v2, v2) * Dot(v1, p) - Dot(v1, v2) * Dot(v2, p)) / Dot(v1, v1) * Dot(v2, v2) - Dot(v1, v2) * Dot(v1, v2)
        // v = (Dot(v1, v1) * Dot(v2, p) - Dot(v1, v2) * Dot(v1, p)) / Dot(v1, v1) * Dot(v2, v2) - Dot(v1, v2) * Dot(v1, v2)

        float u = (dot22 * dot1P - dot12 * dot2P) / (dot11 * dot22 - dot12 * dot12);
        float v = (dot11 * dot2P - dot12 * dot1P) / (dot11 * dot22 - dot12 * dot12);

        if (u >= 0 && v >= 0 && u + v < 1)
            return true;
        else
            return false;

    }
}