using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class imposterEnable : MonoBehaviour
{

    // Define the squared distance threshold
    private static float distanceThresholdSquared = 200f * 200f;  // 200 units distance, squared
    public static void EnableImposters(Terrains terrains)
    {
        // get all active objects
        HashSet<GameObject> activeObjects = terrains.GetActiveObjects();
        foreach (GameObject obj in activeObjects)
        {

            // Use Vector3.sqrMagnitude for distance comparison
            Vector3 difference = terrains.player.transform.position - obj.transform.position;
            if (difference.sqrMagnitude > distanceThresholdSquared)
            {
                terrains.AddToImposters(obj);
            }
            else
            {
                terrains.RemoveFromImposters(obj);
            }


        }
    }
}
