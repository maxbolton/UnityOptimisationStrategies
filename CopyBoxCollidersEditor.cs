using UnityEditor;
using UnityEngine;

public class CopyBoxCollidersEditor : EditorWindow
{
    private GameObject[] sourceObjects = new GameObject[16];
    private GameObject[] targetObjects = new GameObject[16];

    [MenuItem("Tools/Copy Box Colliders")]
    public static void ShowWindow()
    {
        GetWindow<CopyBoxCollidersEditor>("Copy Box Colliders");
    }

    void OnGUI()
    {
        GUILayout.Label("Source Objects", EditorStyles.boldLabel);
        for (int i = 0; i < 16; i++)
        {
            sourceObjects[i] = (GameObject)EditorGUILayout.ObjectField(sourceObjects[i], typeof(GameObject), true);
        }

        GUILayout.Label("Target Objects", EditorStyles.boldLabel);
        for (int i = 0; i < 16; i++)
        {
            targetObjects[i] = (GameObject)EditorGUILayout.ObjectField(targetObjects[i], typeof(GameObject), true);
        }

        if (GUILayout.Button("Copy Box Colliders"))
        {
            CopyBoxCollidersTool.CopyBoxColliders(sourceObjects, targetObjects);
        }
    }
}
