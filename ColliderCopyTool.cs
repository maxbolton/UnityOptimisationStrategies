using UnityEngine;
using UnityEditor;

public class ColliderCopierTool : EditorWindow
{
    [MenuItem("Tools/Collider Copier")]
    public static void ShowWindow()
    {
        GetWindow<ColliderCopierTool>("Collider Copier");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Copy BoxCollider Values"))
        {
            CopyBoxColliderValues();
        }
    }

    private void CopyBoxColliderValues()
    {
        GameObject selectedObject = Selection.activeGameObject;

        if (selectedObject == null)
        {
            Debug.LogWarning("No game object selected. Please select a game object with a BoxCollider.");
            return;
        }

        BoxCollider sourceCollider = selectedObject.GetComponent<BoxCollider>();
        if (sourceCollider == null)
        {
            Debug.LogWarning("The selected game object does not have a BoxCollider component.");    
            return;
        }

        // Use FindObjectsOfType to get all instances of Transform, which every GameObject has
        Transform[] allTransforms = FindObjectsOfType<Transform>();
        foreach (Transform transform in allTransforms)
        {
            // Check if the current transform's GameObject has the same name as the selected object
            if (transform.gameObject.name == selectedObject.name)
            {
                BoxCollider collider = transform.gameObject.GetComponent<BoxCollider>();
                if (collider == null)
                {
                    collider = transform.gameObject.AddComponent<BoxCollider>();
                }

                collider.center = sourceCollider.center;
                collider.size = sourceCollider.size;
                collider.isTrigger = sourceCollider.isTrigger;
            }
        }

        Debug.Log("BoxCollider values copied successfully.");
    }
}
