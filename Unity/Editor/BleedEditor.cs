using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Bleed))]
public class BleedEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var t = target as Bleed;
        
        DrawDefaultInspector();

        if (GUILayout.Button("Create")) t.Process();
    }
}
