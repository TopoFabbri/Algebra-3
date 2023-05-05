#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Grid3D))]
public class Grid3DEditor : Editor
{
    private void OnSceneGUI()
    {
        Grid3D grid = (Grid3D)target;

        for (int i = 0; i < grid.GetPoints().Length; i++)
        {
            Handles.DrawWireCube(grid.GetPoints()[i], Vector3.one * grid.distance);
        }
    }
}
#endif