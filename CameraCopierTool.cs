using UnityEngine;
using UnityEditor;
using System.Linq;
using Unity.VisualScripting;

public class AdjustCameraChildPosition : EditorWindow
{
    [MenuItem("Tools/Adjust Camera Position Relative to BoxCollider")]
    public static void ShowWindow()
    {
        GetWindow<AdjustCameraChildPosition>("Adjust Camera Position");
    }

    GameObject sourceObject;

    void OnGUI()
    {
        GUILayout.Label("Source Object", EditorStyles.boldLabel);
        sourceObject = (GameObject)EditorGUILayout.ObjectField(sourceObject, typeof(GameObject), true);

        if (GUILayout.Button("Adjust Camera Position"))
        {
            if (sourceObject != null)
            {
                AdjustCameraChildPositions();
            }
            else
            {
                Debug.LogError("Source object is not selected!");
            }
        }
    }

    private void AdjustCameraChildPositions()
    {
        var sourceFrustumController = sourceObject.GetComponent("frustumController");
        var sourceCameraChild = sourceObject.transform.GetComponentsInChildren<Camera>(true).FirstOrDefault();


        //sourceFrustumController == null || 
        if (sourceCameraChild == null)
        {
            // log specific error message depending on which component is missing inline if statement
            if (sourceFrustumController == null)
                Debug.LogError("Source object is missing frustumController component.");
            if (sourceCameraChild == null)
                Debug.LogError("Source object is missing Camera child object.");

            
            return;
        }

        var targetObjects = GameObject.FindGameObjectsWithTag("mesh");
        foreach (var target in targetObjects)
        {
            // Ensure or update frustumController on the parent object
            AdjustFrustumController(sourceObject, target);

            // Handle or update the camera child, including its position adjustment
            var targetCameraChild = AdjustOrAddCameraChild(sourceCameraChild, target);

            // Update camera properties
            var newCameraComp = targetCameraChild.GetComponent<Camera>();
            CopyCameraProperties(sourceCameraChild, newCameraComp);

            // Adjust the child object's position to half the height of the BoxCollider
            AdjustChildPositionRelativeToBoxCollider(target, targetCameraChild);
        }
    }

    void AdjustFrustumController(GameObject source, GameObject target)
    {
        var sourceFrustumController = source.GetComponent("frustumController");
        var targetFrustumController = target.GetComponent("frustumController");
        if (targetFrustumController == null)
        {
            UnityEditorInternal.ComponentUtility.CopyComponent(sourceFrustumController as Component);
            UnityEditorInternal.ComponentUtility.PasteComponentAsNew(target);
        }
        else
        {
            UnityEditorInternal.ComponentUtility.CopyComponent(sourceFrustumController as Component);
            UnityEditorInternal.ComponentUtility.PasteComponentValues(targetFrustumController as Component);
        }
    }

    GameObject AdjustOrAddCameraChild(Camera sourceCamera, GameObject target)
    {
        var targetCameraChild = target.transform.GetComponentsInChildren<Camera>(true).FirstOrDefault()?.gameObject;
        if (targetCameraChild == null)
        {
            targetCameraChild = Instantiate(sourceCamera.gameObject, target.transform);
            targetCameraChild.name = sourceCamera.gameObject.name;
        }
        else
        {
            targetCameraChild.SetActive(sourceCamera.gameObject.activeSelf);
            targetCameraChild.GetComponent<Camera>().enabled = sourceCamera.enabled;
        }

        // Ensure no frustumController on the camera child
        var cameraFrustumController = targetCameraChild.GetComponent("frustumController");
        if (cameraFrustumController != null)
        {
            DestroyImmediate(cameraFrustumController as Component);
        }

        return targetCameraChild;
    }

    void CopyCameraProperties(Camera sourceCamera, Camera targetCamera)
    {
        targetCamera.fieldOfView = sourceCamera.fieldOfView;
        targetCamera.nearClipPlane = sourceCamera.nearClipPlane;
        targetCamera.farClipPlane = sourceCamera.farClipPlane;
        targetCamera.backgroundColor = sourceCamera.backgroundColor;
        // Add other camera properties as needed
    }

    void AdjustChildPositionRelativeToBoxCollider(GameObject target, GameObject cameraChild)
    {
        var boxCollider = target.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            var halfHeight = boxCollider.size.y * 0.5f;
            cameraChild.transform.localPosition = new Vector3(0, halfHeight, 0) + boxCollider.center;
        }
        else
        {
            Debug.LogWarning($"No BoxCollider found on {target.name}, camera child position not adjusted.", target);
        }
    }
}
