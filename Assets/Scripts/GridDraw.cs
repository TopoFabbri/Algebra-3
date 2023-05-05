#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomGrid))]
public class GridDraw : Editor
{
    private void OnSceneGUI()
    {
        CustomGrid grid = (CustomGrid)target;

        if (grid.draw)
            DrawGrid(grid);
    }

    private void DrawGrid(CustomGrid grid)
    {
        Vector3[] outerPoints = grid.GetOuterWallPoints();
        Vector3 halfCell = new Vector3(grid.distance / 2, grid.distance / 2, grid.distance / 2);

        for (int i = 0; i < grid.points.Length; i++)
        {
            if (grid.drawLines)
            {
                if (!outerPoints.Contains(grid.points[i]))
                    Handles.DrawWireCube(grid.points[i] + halfCell, Vector3.one * grid.distance);
            }

            if (grid.drawDiscs)
                Handles.DrawWireDisc(grid.points[i], Vector3.up, grid.discSize);
        }

    }
}
#endif