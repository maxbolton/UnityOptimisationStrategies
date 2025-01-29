using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrustumController : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;

    private Camera objectCamera;
    private GameObject childCamera;
    private BoxCollider objectCollider;

    // Minimum and maximum FOV values
    public float minFOV = 5f;
    public float maxFOV = 50f;

    // Adjust these values to scale distance effect on FOV
    public float minDistance = 1f;
    public float maxDistance = 10f;

    void Start()
    {
        childCamera = GetComponentInChildren<Camera>().gameObject;
        objectCamera = childCamera.GetComponent<Camera>();
        objectCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        Vector3 cameraPosition = childCamera.transform.position;
        Vector3 playerPosition = playerTransform.position;

        // Calculate the direction vector from the camera to the player
        Vector3 directionToPlayer = (playerPosition - cameraPosition).normalized;

        // Make the camera look at the player first
        Quaternion lookAtPlayer = Quaternion.LookRotation(directionToPlayer);

        // Then, rotate the camera to face the opposite direction
        Quaternion oppositeDirection = lookAtPlayer * Quaternion.Euler(0, 180, 0);

        // Apply the calculated rotation to the camera
        childCamera.transform.rotation = oppositeDirection;

        /* Adjust the FOV based on the player's proximity
        float distanceToPlayer = Vector3.Distance(cameraPosition, playerPosition);
        float normalizedDistance = (distanceToPlayer - minDistance) / (maxDistance - minDistance);
        normalizedDistance = Mathf.Clamp01(normalizedDistance);
        float targetFOV = Mathf.Lerp(maxFOV, minFOV, normalizedDistance);
        objectCamera.fieldOfView = targetFOV;*/

        AdjustFOVBasedOnPlayerDistance();


    }
    void AdjustFOVBasedOnPlayerDistance()
    {
        if (playerTransform == null || objectCollider == null)
            return;

        // Calculate distance from the player to this object
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Assuming the collider's size along its local Y-axis represents the object's "height"
        float objectHeight = objectCollider.size.y * transform.localScale.y;

        // Calculate the required vertical FOV to fit the object's height at the current distance
        float fov = 2.0f * Mathf.Atan(objectHeight / (2.0f * distanceToPlayer)) * Mathf.Rad2Deg;

        // Optionally, add a margin to the FOV to ensure the object is not too tightly framed
        float fovMargin = 5.0f; // Adjust this value as needed
        fov += fovMargin;

        // Update the camera's FOV
        objectCamera.fieldOfView = fov;
    }

}
