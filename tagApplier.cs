using UnityEngine;
using UnityEditor;

public class TagApplier : EditorWindow
{
    [MenuItem("Tools/Apply Tag To Prefab Instances")]
    public static void ShowWindow()
    {
        GetWindow<TagApplier>("Apply Tag");
    }

    private GameObject prefabToTag;
    private string tagToApply;

    void OnGUI()
    {
        prefabToTag = EditorGUILayout.ObjectField("Prefab:", prefabToTag, typeof(GameObject), true) as GameObject;
        tagToApply = EditorGUILayout.TagField("Tag to Apply:", tagToApply);

        if (GUILayout.Button("Apply Tag"))
        {
            ApplyTagToInstances();
        }
    }

    void ApplyTagToInstances()
    {
        if (prefabToTag == null || string.IsNullOrEmpty(tagToApply))
        {
            Debug.LogError("Prefab or Tag is not set.");
            return;
        }

        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (PrefabUtility.GetCorrespondingObjectFromSource(obj) == prefabToTag)
            {
                obj.tag = tagToApply;
                Debug.Log(obj.name + " tagged with " + tagToApply);
            }
        }
    }
}
