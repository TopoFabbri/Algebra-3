using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMeshCollider : MonoBehaviour
{
    [Header("Drawing:")]
    [SerializeField] private bool drawTris = false;
    [SerializeField] private bool drawNormals = false;

    private int[] tris;
    private Vector3[] vertices;
    private List<Plane> faces;

    private Vector3 prevPos;

    private void OnValidate()
    {
        faces = new List<Plane>();
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        tris = mesh.triangles;
        vertices = mesh.vertices;
        prevPos = Vector3.zero;
    }

    private void OnDrawGizmos()
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
        Gizmos.color = Color.blue;

        for (int i = 0; i < tris.Length; i += 3)
        {
            Vector3[] verts = { vertices[tris[i]], vertices[tris[i + 1]], vertices[tris[i + 2]] };
            Gizmos.DrawLine(transform.TransformPoint(verts[0]), transform.TransformPoint(verts[1]));
            Gizmos.DrawLine(transform.TransformPoint(verts[1]), transform.TransformPoint(verts[2]));
        }
    }

    private void PositionUpdated()
    {
        faces.Clear();
        
        for (int i = 0; i < tris.Length; i += 3)
        {
            Plane plane = new Plane(
                transform.TransformPoint(vertices[tris[i]]), 
                transform.TransformPoint(vertices[tris[i + 1]]), 
                transform.TransformPoint(vertices[tris[i + 2]])
            );
            
            faces.Add(plane);
        }
    }

    private void DrawNormals()
    {
        foreach (var face in faces)
        {
            var position = transform.position;
            Vector3 faceCenter = face.ClosestPointOnPlane(position);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(faceCenter, faceCenter + face.normal * .2f);

            if (!face.GetSide(transform.position))
                face.Flip();
        }
    }
}