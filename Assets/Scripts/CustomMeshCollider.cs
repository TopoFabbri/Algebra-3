using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMeshCollider : MonoBehaviour
{
    [Header("Drawing:")] [SerializeField] private bool drawTris = false;
    [SerializeField] private bool drawNormals = false;

    public List<Vector3> pointsInMesh = new List<Vector3>();

    private int[] tris;
    private Vector3[] vertices;
    private List<Plane> faces = new List<Plane>();
    private List<Triangle> triangles = new List<Triangle>();
    private Color trisColor = Color.blue;
    private CustomGrid grid;

    private Vector3 prevPos;

    private void OnValidate()
    {
        faces = new List<Plane>();
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        tris = mesh.triangles;
        vertices = mesh.vertices;
        prevPos = Vector3.zero;
        grid = FindObjectOfType<CustomGrid>();
    }

    private void Update()
    {
        if (transform.position != prevPos)
        {
            prevPos = transform.position;
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
            Vector3[] verts = { vertices[tris[i]], vertices[tris[i + 1]], vertices[tris[i + 2]] };
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
            Vector3[] verts = new Vector3[3];
            verts[0] = transform.TransformPoint(vertices[tris[i]]);
            verts[1] = transform.TransformPoint(vertices[tris[i + 1]]);
            verts[2] = transform.TransformPoint(vertices[tris[i + 2]]);

            Plane plane = new Plane(
                transform.TransformPoint(verts[0]),
                transform.TransformPoint(verts[1]),
                transform.TransformPoint(verts[2])
            );

            faces.Add(plane);
            triangles.Add(new Triangle(verts, plane));
        }
    }

    private void MakeGridpointsInMesh()
    {
        pointsInMesh.Clear();
        Vector3[] gridPoints = grid.points;

        for (var i = 0; i < gridPoints.Length; i++)
        {
            int faceColCount = 0;

            for (var j = 0; j < faces.Count; j++)
            {
                if (triangles[j].LineCollides(gridPoints[i], Vector3.up))
                    faceColCount++;
            }

            if (faceColCount % 2 != 0)
                pointsInMesh.Add(gridPoints[i]);
        }
    }

    public bool CollidesWith(List<Vector3> points)
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
    }

    public void NoCollision()
    {
        trisColor = Color.blue;
    }

    private void DrawNormals()
    {
        foreach (var face in faces)
        {
            var position = transform.position;
            Vector3 faceCenter = face.ClosestPointOnPlane(position);

            Debug.DrawLine(faceCenter, faceCenter + face.normal * .2f, trisColor);

            if (!face.GetSide(transform.position))
                face.Flip();
        }
    }
}