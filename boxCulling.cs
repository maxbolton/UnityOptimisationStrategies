using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using UnityEditor.Build;
using UnityEngine;


public class boxCulling : MonoBehaviour
{

    // Define a layer mask for layer 8
    private static readonly int layerMask = 1 << 8;

    private static bool isTester = false;
    public static bool BoxOcclusion(Terrains terrains, GameObject player, Camera camera)
    {
        HashSet<ObjectGroup> activeGroups = null;

        try
        {
            activeGroups = terrains.GetActiveGroups();
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Error in retrieving groups: " + e.Message);
            return false; // Early exit due to failure
        }


        try
        {
            foreach (ObjectGroup group in activeGroups)
            {
                // append objectList to activeObjects
                HashSet<GameObject> activeObjects = new HashSet<GameObject>(group.objectList);

                foreach (GameObject obj in activeObjects)
                {
                    //Color debugColor = obj.name == "Cypress(tester)" ? Color.red : Color.green;
                    if (IsOccluded(obj, camera, terrains))
                    {
                        terrains.RemoveFromActiveObjects(obj);
                    }
                    else
                    {
                        terrains.AddToActiveObjects(obj);
                    }
                }
            }

        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Error processing active groups: " + e.Message);
        }

        return true;
    }


    private static bool IsOccluded(GameObject occludee, Camera camera, Terrains terrains)
    {
        // Try to retrieve the BoxCollider component from cache
        if (!terrains.Colliders.TryGetValue(occludee, out BoxCollider collider))
        {
            return false; // No collider found in the cache, assume not occluded
        }
        // Get the local space bounds (size and center) of the collider
        Bounds localBounds = new Bounds(collider.center, collider.size);
        // Retrieve the transform component of the object
        Transform transform = occludee.transform;

        UnityEngine.Vector3[] corners = new UnityEngine.Vector3[8];

        // Calculate the local space positions of the corners of the box collider
        for (int i = 0; i < 8; i++)
        {
            corners[i] = new UnityEngine.Vector3(
                (i & 1) == 0 ? localBounds.min.x : localBounds.max.x,
                (i & 2) == 0 ? localBounds.min.y : localBounds.max.y,
                (i & 4) == 0 ? localBounds.min.z : localBounds.max.z
            );
            // Transform the corners to world space
            corners[i] = transform.TransformPoint(corners[i]);
        }

        /* if object name is cypress(tester) then draw the box
        if (occludee.name == "Cypress(tester)")
        {
            isTester = true;
            for (int i = 0; i < 8; i++)
            {
                Debug.DrawLine(camera.transform.position, corners[i], Color.red);
            }
        }*/


        // Perform the raycasting to check for occlusion
        for (int i = 0; i < corners.Length; i++)
        {

            Ray ray = new Ray(camera.transform.position, corners[i] - camera.transform.position);

            // if the ray does not hit anything or hits the occludee itself, the object is not occluded
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask) || hit.collider.gameObject == occludee)
            {
                // If any corner ray hits something other than the occludee, it's not occluded
                return false;
            }
        }

        /*if (isTester)
        {
            Debug.Log("Cypress(tester) is occluded");
        }*/

        // If all corner rays hit the occludee, it is considered occluded
        return true;

    }




}
