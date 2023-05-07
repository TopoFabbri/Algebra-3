using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

struct Triangle
{
    private Vector3[] verts;
    private Plane plane;

    public Triangle(Vector3[] verts, Plane plane)
    {
        this.verts = verts;
        this.plane = plane;
    }
    
    public bool LineCollides(Vector3 start, Vector3 dir)
    {
        bool doesIntersect = false;
        float dis;
        Ray ray = new Ray(start, dir);

        if(plane.Raycast(ray, out dis))
            doesIntersect = TriangleCollides(ray.GetPoint(dis));
        
        return doesIntersect;
    }

    private bool TriangleCollides(Vector3 intersection)
    {
        Plane[] planes = new Plane[3];
        
        planes[0] = new Plane(verts[0], verts[1], verts[0] + plane.normal);
        if (!planes[0].GetSide(verts[2]))
            planes[0].Flip();

        planes[1] = new Plane(verts[1], verts[2], verts[1] + plane.normal);
        if (!planes[1].GetSide(verts[0]))
            planes[1].Flip();

        planes[2] = new Plane(verts[2], verts[0], verts[2] + plane.normal);
        if (!planes[2].GetSide(verts[1]))
            planes[2].Flip();

        int sideCounter = 0;
    
        foreach (var side in planes)
        {
            if (side.GetSide(intersection))
                sideCounter++;
        }

        return sideCounter >= 3;
    }
    
    public void DrawPlane(Vector3 position, Vector3 normal)
    {
        Vector3 v3;
        if (normal.normalized != Vector3.forward)
            v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
        else
            v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude; ;
        var corner0 = position + v3;
        var corner2 = position - v3;
        var q = Quaternion.AngleAxis(90.0f, normal);
        v3 = q * v3;
        var corner1 = position + v3;
        var corner3 = position - v3;
        Debug.DrawLine(corner0, corner2, Color.green);
        Debug.DrawLine(corner1, corner3, Color.green);
        Debug.DrawLine(corner0, corner1, Color.green);
        Debug.DrawLine(corner1, corner2, Color.green);
        Debug.DrawLine(corner2, corner3, Color.green);
        Debug.DrawLine(corner3, corner0, Color.green);
        Debug.DrawRay(position, normal, Color.red);
    }
}