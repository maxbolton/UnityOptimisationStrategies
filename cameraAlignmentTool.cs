using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PositionChildAtHalfHeight : MonoBehaviour
{
    // This will create a new menu item in Unity's top menu bar.
    [MenuItem("Tools/Position Child At Half Height of Box Collider")]
    private static void PositionChildren()
    {
        // Find all GameObjects with the 'mesh' tag in the scene.
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("mesh");
        foreach (GameObject obj in objectsWithTag)
        {
            // Check if the object has a BoxCollider component.
            BoxCollider boxCollider = obj.GetComponent<BoxCollider>();
            if (boxCollider != null && obj.transform.childCount > 0)
            {
                // Assuming the first child should be repositioned.
                Transform child = obj.transform.GetChild(0);

                // Calculate the new position, which is half the height of the BoxCollider.
                Vector3 newPosition = new Vector3(child.localPosition.x, boxCollider.size.y / 2, child.localPosition.z);

                // Set the child's localPosition to the new position.
                child.localPosition = newPosition;

                Debug.Log($"Child of {obj.name} repositioned to {newPosition}");
            }
        }
    }
}
