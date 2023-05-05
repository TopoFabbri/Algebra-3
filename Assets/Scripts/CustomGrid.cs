using System.Collections.Generic;
using UnityEngine;

public class CustomGrid : MonoBehaviour
{
    [Header("Drawing:")]
    public bool draw = false;
    public bool drawLines = false;
    public bool drawDiscs = false;
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