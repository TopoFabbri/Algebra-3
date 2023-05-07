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
    }
}
#endif