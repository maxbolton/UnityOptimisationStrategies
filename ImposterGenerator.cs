using System.Collections;
using UnityEngine;

public class ImpostorBaker : MonoBehaviour
{
    public GameObject targetObject;
    public Camera renderCamera;
    public RenderTexture impostorTexture;

    void Start()
    {
        if (targetObject == null || renderCamera == null || impostorTexture == null)
        {
            Debug.LogError("Missing components, please assign them in the inspector.");
            return;
        }

        // Set up the render camera
        SetupRenderCamera();

        // Bake the impostor
        BakeImpostor();

        // Save as prefab (you need to handle prefab creation, see Unity Editor scripting)
        // PrefabUtility.SaveAsPrefabAsset(impostorObject, "Assets/Impostors/Impostor.prefab");
    }

    void SetupRenderCamera()
    {
        // Position the camera to face the target object
        renderCamera.transform.position = targetObject.transform.position - targetObject.transform.forward * 10;
        renderCamera.transform.LookAt(targetObject.transform);
        renderCamera.targetTexture = impostorTexture;
    }

    void BakeImpostor()
    {
        // Render the target object to the render texture
        renderCamera.Render();

        // Create the impostor object
        GameObject impostorObject = new GameObject("Impostor");
        MeshRenderer meshRenderer = impostorObject.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = impostorObject.AddComponent<MeshFilter>();
        meshFilter.mesh = CreateQuadMesh();

        // Assign the texture to a material
        Material mat = new Material(Shader.Find("HDRP/Lit"));
        mat.mainTexture = impostorTexture;
        meshRenderer.material = mat;

        // Optionally, adjust scale and orientation
        impostorObject.transform.localScale = new Vector3(5, 5, 1); // Scale as needed
    }

    Mesh CreateQuadMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[] {
            new Vector3(-1, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(-1, -1, 0),
            new Vector3(1, -1, 0)
        };
        mesh.uv = new Vector2[] {
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(0, 0),
            new Vector2(1, 0)
        };
        mesh.triangles = new int[] { 0, 2, 1, 2, 3, 1 };
        return mesh;
    }
}
