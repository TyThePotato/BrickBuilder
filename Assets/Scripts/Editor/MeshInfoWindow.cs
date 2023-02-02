using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MeshInfoWindow : EditorWindow {
    
    private Mesh MeshFieldObject;
    
    [MenuItem("Window/Mesh Info")]
    static void Init() {
        MeshInfoWindow window = (MeshInfoWindow)EditorWindow.GetWindow(typeof(MeshInfoWindow));
        window.minSize = new Vector2(300, 200);
        window.maxSize = new Vector2(300, 200);
        window.Show();
    }

    private void OnGUI() {
        MeshFieldObject = EditorGUILayout.ObjectField("Mesh", MeshFieldObject, typeof(Mesh), false) as Mesh;

        if (MeshFieldObject != null) {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Mesh Info", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"Name: {MeshFieldObject.name}");
            EditorGUILayout.LabelField($"Vertex Count: {MeshFieldObject.vertexCount}");
            EditorGUILayout.LabelField($"Sub Mesh Count: {MeshFieldObject.subMeshCount}");
            EditorGUILayout.LabelField($"Index Format: {MeshFieldObject.indexFormat}");
            EditorGUILayout.EndVertical();
        }
    }
}
