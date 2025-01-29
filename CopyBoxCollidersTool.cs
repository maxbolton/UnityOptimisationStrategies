using UnityEngine;

public class CopyBoxCollidersTool
{
    public static void CopyBoxColliders(GameObject[] sourceObjects, GameObject[] targetObjects)
    {
        if (sourceObjects.Length != targetObjects.Length || sourceObjects.Length != 16)
        {
            Debug.LogError("Source and Target objects arrays must both have exactly 16 elements.");
            return;
        }

        for (int i = 0; i < sourceObjects.Length; i++)
        {
            BoxCollider sourceCollider = sourceObjects[i].GetComponent<BoxCollider>();
            if (sourceCollider != null)
            {
                BoxCollider targetCollider = targetObjects[i].GetComponent<BoxCollider>();
                if (targetCollider == null)
                {
                    targetCollider = targetObjects[i].AddComponent<BoxCollider>();
                }

                targetCollider.center = sourceCollider.center;
                targetCollider.size = sourceCollider.size;
                targetCollider.isTrigger = sourceCollider.isTrigger;
            }
            else
            {
                Debug.LogWarning($"Source object at index {i} does not have a BoxCollider component.");
            }
        }
    }
}
