using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
    Grid _target;
    bool gizmos;

    private void OnEnable()
    {
        _target = (Grid)target;

    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Create"))
        {
            _target.GenerateGrid();
        }
    }
}
