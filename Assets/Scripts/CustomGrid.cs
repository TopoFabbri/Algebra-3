using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomGrid : MonoBehaviour
{
    [Header("Drawing:")]
    public bool draw = false;
    public bool drawLines = false;
    public bool drawPoints = false;
    public bool drawSize = false;
    public float discSize = .1f;

    [Header("Dimensions:")]
    public float size = 10.0f;
    public float distance = 1.0f;
    public Vector3[] points;

    private int gridSize;
    private int pointIndex = 0;

    private void Awake()
    {
        gridSize = Mathf.FloorToInt(size / distance);
        points = new Vector3[gridSize * gridSize * gridSize];
        CreateGrid();
    }

    private void OnValidate()
    {
        gridSize = Mathf.FloorToInt(size / distance);
        points = new Vector3[gridSize * gridSize * gridSize];
        CreateGrid();
    }

    private void Update()
    {
        DrawGrid();
    }

    private void CreateGrid()
    {
        pointIndex = 0;
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                for (int k = 0; k < gridSize; k++)
                {
                    points[pointIndex] = new Vector3(i * distance, j * distance, k * distance);
                    pointIndex++;
                }
            }
        }
    }
    
    private void DrawGrid()
    {
        Vector3[] outerPoints = GetOuterWallPoints();
        Vector3 halfCell = new Vector3(distance / 2, distance / 2, distance / 2);

        for (int i = 0; i < points.Length; i++)
        {
            if (drawLines)
            {
                if (!outerPoints.Contains(points[i]))
                    DrawWireCube(points[i] + halfCell, Vector3.one * distance);
            }

            if (drawPoints)
                DrawPoint(points[i], discSize, Color.magenta);
        }
        
        if (drawSize)
        {
            Vector3 pos = new Vector3((size - 1) / 2, (size - 1) / 2, (size - 1) / 2);
            Vector3 vectorSize = pos * 2;
            DrawWireCube(pos, vectorSize);
        }
    }

    public void DrawWireCube(Vector3 center, Vector3 size)
    {
        Vector3 halfSize = size / 2f;

        Vector3 topLeftFront = center + new Vector3(-halfSize.x, halfSize.y, -halfSize.z);
        Vector3 topRightFront = center + new Vector3(halfSize.x, halfSize.y, -halfSize.z);
        Vector3 bottomRightFront = center + new Vector3(halfSize.x, -halfSize.y, -halfSize.z);
        Vector3 bottomLeftFront = center + new Vector3(-halfSize.x, -halfSize.y, -halfSize.z);
        Vector3 topLeftBack = center + new Vector3(-halfSize.x, halfSize.y, halfSize.z);
        Vector3 topRightBack = center + new Vector3(halfSize.x, halfSize.y, halfSize.z);
        Vector3 bottomRightBack = center + new Vector3(halfSize.x, -halfSize.y, halfSize.z);
        Vector3 bottomLeftBack = center + new Vector3(-halfSize.x, -halfSize.y, halfSize.z);

        Debug.DrawLine(topLeftFront, topRightFront, Color.white);
        Debug.DrawLine(topRightFront, bottomRightFront, Color.white);
        Debug.DrawLine(bottomRightFront, bottomLeftFront, Color.white);
        Debug.DrawLine(bottomLeftFront, topLeftFront, Color.white);

        Debug.DrawLine(topLeftFront, topLeftBack, Color.white);
        Debug.DrawLine(topRightFront, topRightBack, Color.white);
        Debug.DrawLine(bottomRightFront, bottomRightBack, Color.white);
        Debug.DrawLine(bottomLeftFront, bottomLeftBack, Color.white);

        Debug.DrawLine(topLeftBack, topRightBack, Color.white);
        Debug.DrawLine(topRightBack, bottomRightBack, Color.white);
        Debug.DrawLine(bottomRightBack, bottomLeftBack, Color.white);
        Debug.DrawLine(bottomLeftBack, topLeftBack, Color.white);
    }
    
    public void DrawPoint(Vector3 point, float size, Color color)
    {
        Debug.DrawLine(point - Vector3.up * size / 2f, point + Vector3.up * size / 2f, color);
        Debug.DrawLine(point - Vector3.left * size / 2f, point + Vector3.left * size / 2f, color);
        Debug.DrawLine(point - Vector3.forward * size / 2f, point + Vector3.forward * size / 2f, color);
    }

    public Vector3[] GetOuterWallPoints()
    {
        List<Vector3> outerPoints = new List<Vector3>();

        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].x == size - distance ||
                points[i].y == size - distance ||
                points[i].z == size - distance)
            {
                outerPoints.Add(points[i]);
            }
        }

        return outerPoints.ToArray();
    }
}