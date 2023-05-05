using UnityEngine;

public class Grid3D : MonoBehaviour
{
    [SerializeField] private float size = 10.0f;
    [SerializeField] private float distance = 1.0f;
    [SerializeField] private Vector3[] points;

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

    public Vector3[] GetPoints()
    {
        return points;
    }

    public float GetDistance()
    {
        return distance;
    }
}