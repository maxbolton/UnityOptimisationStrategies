using UnityEngine;
using UnityEditor;
using System.Linq;

public class LODToMesh : EditorWindow
{
    [MenuItem("Tools/Convert LODGroup to Mesh")]
    static void Init()
    {
        // Open the window
        LODToMesh window = (LODToMesh)EditorWindow.GetWindow(typeof(LODToMesh));
        window.Show();
    }

    void OnGUI()
    {
        if (GUILayout.Button("Convert Selected"))
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                LODGroup lodGroup = obj.GetComponent<LODGroup>();
                if (lodGroup != null)
                {
                    Transform[] children = obj.GetComponentsInChildren<Transform>(true).Where(t => t != obj.transform).ToArray();
                    foreach (Transform child in children)
                    {
                        MeshFilter meshFilter = child.GetComponent<MeshFilter>();
                        MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
                        if (meshFilter != null && meshRenderer != null)
                        {
                            // Copy Mesh Filter
                            MeshFilter parentMeshFilter = obj.AddComponent<MeshFilter>();
                            parentMeshFilter.sharedMesh = meshFilter.sharedMesh;

                            // Copy Mesh Renderer
                            MeshRenderer parentMeshRenderer = obj.AddComponent<MeshRenderer>();
                            parentMeshRenderer.sharedMaterials = meshRenderer.sharedMaterials;

                            // Break after the first valid child to avoid overwriting
                            break;
                        }
                    }

                    // Remove the LODGroup component
                    DestroyImmediate(lodGroup);

                    // Delete all child objects
                    foreach (Transform child in children)
                    {
                        DestroyImmediate(child.gameObject);
                    }
                }
            }
        }
    }
}
