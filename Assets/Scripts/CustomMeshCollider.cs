using System;
using System.Collections;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;

public class CustomMeshCollider : MonoBehaviour
{
    [Header("Drawing:")] [SerializeField] private bool drawTris = false;
    [SerializeField] private bool drawNormals = false;
    
    public List<Vec3> pointsInMesh = new List<Vec3>();

    private int[] tris;
    private Vec3[] vertices;
    private List<CustomPlane> faces = new List<CustomPlane>();
    private List<Triangle> triangles = new List<Triangle>();
    private Color trisColor = Color.blue;
    private CustomGrid grid;
    private MeshRenderer renderer;

    private Vec3 prevPos;

    private void OnValidate()
    {
        faces = new List<CustomPlane>();
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        tris = mesh.triangles;
        vertices = Vec3.ToVec3(mesh.vertices);
        prevPos = Vec3.Zero;
        grid = FindObjectOfType<CustomGrid>();
        renderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (transform.position != prevPos)
        {
            prevPos = Vec3.ToVec3(transform.position);
            PositionUpdated();
        }

        if (drawNormals)
            DrawNormals();

        if (drawTris)
            DrawTris();
    }

    private void DrawTris()
    {
        for (int i = 0; i < tris.Length; i += 3)
        {
            Vec3[] verts = { vertices[tris[i]], vertices[tris[i + 1]], vertices[tris[i + 2]] };
            Debug.DrawLine(transform.TransformPoint(verts[0]), transform.TransformPoint(verts[1]), trisColor);
            Debug.DrawLine(transform.TransformPoint(verts[1]), transform.TransformPoint(verts[2]), trisColor);
        }
    }

    private void PositionUpdated()
    {
        MakeTrianglesList();

        MakeGridpointsInMesh();
    }

    void MakeTrianglesList()
    {
        faces.Clear();
        triangles.Clear();

        for (int i = 0; i < tris.Length; i += 3)
        {
            Vec3[] verts = new Vec3[3];
            verts[0] = Vec3.ToVec3(transform.TransformPoint(vertices[tris[i]]));
            verts[1] = Vec3.ToVec3(transform.TransformPoint(vertices[tris[i + 1]]));
            verts[2] = Vec3.ToVec3(transform.TransformPoint(vertices[tris[i + 2]]));

            CustomPlane plane = new CustomPlane(
                Vec3.ToVec3(verts[0]),
                Vec3.ToVec3(verts[1]),
                Vec3.ToVec3(verts[2])
            );

            faces.Add(plane);
            triangles.Add(new Triangle(verts, plane));
        }
    }

    private void MakeGridpointsInMesh()
    {
        pointsInMesh.Clear();
        int closestI = 0;
        int closestJ = 0;
        int closestK = 0;

        for (int i = 0; i < grid.points.GetLength(0); i++)
        {
            for (int j = 0; j < grid.points.GetLength(1); j++)
            {
                for (int k = 0; k < grid.points.GetLength(2); k++)
                {
                    float dis = Vec3.Distance(grid.points[i, j, k], Vec3.ToVec3(transform.position));
                    float closestDis = Vec3.Distance(grid.points[closestI, closestJ, closestK],
                        Vec3.ToVec3(transform.position));

                    if (dis < closestDis)
                    {
                        closestI = i;
                        closestJ = j;
                        closestK = k;
                    }
                }
            }
        }

        PointInMesh(closestI, closestJ, closestK);
    }

    private void PointInMesh(int i, int j, int k)
    {
        if (pointsInMesh.Contains(grid.points[i, j, k]))
            return;

        int faceColCount = 0;

        for (int l = 0; l < faces.Count; l++)
        {
            if (triangles[l].LineCollides(grid.points[i, j, k], Vec3.Up))
                faceColCount++;
        }

        if (faceColCount % 2 != 0)
        {
            pointsInMesh.Add(grid.points[i, j, k]);
            
            if (i > 0)
                PointInMesh(i - 1, j, k);

            if (i < grid.points.GetLength(0) - 1)
                PointInMesh(i + 1, j, k);

            if (j > 0)
                PointInMesh(i, j - 1, k);

            if (j < grid.points.GetLength(1) - 1)
                PointInMesh(i, j + 1, k);
                
            if (k > 0)
                PointInMesh(i, j, k - 1);
            
            if (k < grid.points.GetLength(2) - 1)
                PointInMesh(i, j, k + 1);
        }
    }

    public bool CollidesWith(List<Vec3> points)
    {
        foreach (var point in points)
        {
            if (pointsInMesh.Contains(point))
                return true;
        }

        return false;
    }

    public void CollisionEnter(CustomMeshCollider other)
    {
        trisColor = Color.red;
        renderer.material.color = Color.red;
    }

    public void NoCollision()
    {
        trisColor = Color.blue;
        renderer.material.color = Color.white;
    }

    private void DrawNormals()
    {
        foreach (var face in faces)
        {
            var position = transform.position;
            Vec3 faceCenter = face.ClosestPointOnPlane(Vec3.ToVec3(position));

            Debug.DrawLine(faceCenter, faceCenter + face.normal * .2f, trisColor);

            if (!face.GetSide(Vec3.ToVec3(transform.position)))
                face.Flip();
        }
    }
}