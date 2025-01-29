using UnityEngine;
using UnityEditor;
using System.Linq;

public class PrefabAttacherTool : EditorWindow
{
    private GameObject targetGameObject;
    private GameObject prefabToAttach;

    [MenuItem("Tools/Prefab Attacher")]
    public static void ShowWindow()
    {
        GetWindow<PrefabAttacherTool>("Prefab Attacher");
    }

    void OnGUI()
    {
        GUILayout.Label("Attach Prefab to Game Objects by Name", EditorStyles.boldLabel);

        targetGameObject = (GameObject)EditorGUILayout.ObjectField("Target Game Object", targetGameObject, typeof(GameObject), true);
        prefabToAttach = (GameObject)EditorGUILayout.ObjectField("Prefab to Attach", prefabToAttach, typeof(GameObject), false);

        if (GUILayout.Button("Attach Prefab"))
        {
            AttachPrefab();
        }
    }

    private void AttachPrefab()
    {
        if (targetGameObject == null || prefabToAttach == null)
        {
            EditorUtility.DisplayDialog("Error", "Please assign both a target game object and a prefab.", "OK");
            return;
        }

        string targetName = targetGameObject.name;
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == targetName)
            {
                GameObject prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefabToAttach);
                prefabInstance.transform.SetParent(obj.transform, false);

                MeshRenderer meshRenderer = prefabInstance.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = false;
                }

                Undo.RegisterCreatedObjectUndo(prefabInstance, "Attach Prefab");
            }
        }

        Debug.Log("Prefab attached and MeshRenderer disabled for all matching game objects.");
    }


}
