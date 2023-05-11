using System;
using System.Collections.Generic;
using System.Linq;
using CustomMath;
using UnityEngine;
using UnityEngine.Serialization;

public class CustomGrid : MonoBehaviour
{
    [Header("Drawing:")] public bool draw = false;
    public bool drawLines = false;
    public bool drawPoints = false;
    public bool drawSize = false;
    [FormerlySerializedAs("discSize")] public float pointSize = .1f;

    [Header("Dimensions:")] public float size = 10.0f;
    public float distance = 1.0f;
    public Vec3[,,] points;

    private int gridSize;
    
    private void Awake()
    {
        gridSize = Mathf.FloorToInt(size / distance);
        points = new Vec3[gridSize, gridSize, gridSize];
        CreateGrid();
    }

    private void OnValidate()
    {
        gridSize = Mathf.FloorToInt(size / distance);
        points = new Vec3[gridSize, gridSize, gridSize];
        CreateGrid();
    }

    private void Update()
    {
        DrawGrid();
    }

    private void CreateGrid()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                for (int k = 0; k < gridSize; k++)
                {
                    points[i, j, k] = new Vec3(i * distance, j * distance, k * distance);
                }
            }
        }
    }

    private void DrawGrid()
    {
        Vec3 halfCell = new Vec3(distance / 2, distance / 2, distance / 2);

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                for (int k = 0; k < gridSize; k++)
                {
                    Vec3 point = points[i, j, k];

                    if (drawLines)
                    {
                        bool isOuter = i == 0 || j == 0 || k == 0 || i == gridSize - 1 || j == gridSize - 1 ||
                                       k == gridSize - 1;

                        if (!isOuter)
                            DrawWireCube(point + halfCell, Vec3.One * distance);
                    }

                    if (drawPoints)
                        DrawPoint(point, pointSize, Color.magenta);
                }
            }
        }

        if (drawSize)
        {
            Vec3 pos = new Vec3((size) / 2, (size) / 2, (size) / 2);
            Vec3 vectorSize = pos * 2;
            DrawWireCube(pos, vectorSize);
        }
    }

    public void DrawWireCube(Vec3 center, Vec3 size)
    {
        Vec3 halfSize = size / 2f;

        Vec3 topLeftFront = center + new Vec3(-halfSize.x, halfSize.y, -halfSize.z);
        Vec3 topRightFront = center + new Vec3(halfSize.x, halfSize.y, -halfSize.z);
        Vec3 bottomRightFront = center + new Vec3(halfSize.x, -halfSize.y, -halfSize.z);
        Vec3 bottomLeftFront = center + new Vec3(-halfSize.x, -halfSize.y, -halfSize.z);
        Vec3 topLeftBack = center + new Vec3(-halfSize.x, halfSize.y, halfSize.z);
        Vec3 topRightBack = center + new Vec3(halfSize.x, halfSize.y, halfSize.z);
        Vec3 bottomRightBack = center + new Vec3(halfSize.x, -halfSize.y, halfSize.z);
        Vec3 bottomLeftBack = center + new Vec3(-halfSize.x, -halfSize.y, halfSize.z);

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

    public static void DrawPoint(Vec3 point, float size, Color color)
    {
        Debug.DrawLine(point - Vec3.Up * size / 2f, point + Vec3.Up * size / 2f, color);
        Debug.DrawLine(point - Vec3.Left * size / 2f, point + Vec3.Left * size / 2f, color);
        Debug.DrawLine(point - Vec3.Forward * size / 2f, point + Vec3.Forward * size / 2f, color);
    }

    public float GetSize()
    {
        return size;
    }

    public Vec3[] GetOuterWallPoints()
    {
        List<Vec3> outerPoints = new List<Vec3>();

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                for (int k = 0; k < gridSize; k++)
                {
                    Vec3 point = points[i, j, k];

                    if (i == 0 || j == 0 || k == 0 || i == gridSize - 1 || j == gridSize - 1 || k == gridSize - 1)
                    {
                        outerPoints.Add(point);
                    }
                }
            }
        }

        return outerPoints.ToArray();
    }
}