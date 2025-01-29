using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectCuller : MonoBehaviour
{



    [SerializeField]
    private GameObject cullingParent;
    [SerializeField]
    private GameObject highPolyTerrain;
    [SerializeField]
    private GameObject lowPolyTerrain;

    [SerializeField]
    private bool startCulled;
    [SerializeField]
    private bool toogleObjects;
    [SerializeField]
    private bool toggleHighPoly;
    [SerializeField]
    private bool toggleLowPoly;


    // Start is called before the first frame update
    void Start()
    {
        // enable all renderers on start
        foreach (MeshRenderer renderer in cullingParent.GetComponentsInChildren<MeshRenderer>())
        {
            renderer.enabled = !startCulled;
        }

        if (highPolyTerrain != null) // Check if the object was found
        {
            // Get all MeshRenderer components in children
            Terrain[] childMeshRenderers = highPolyTerrain.GetComponentsInChildren<Terrain>();

            // Toggle the enabled state of each MeshRenderer
            foreach (Terrain terrain in childMeshRenderers)
            {
                //terrain.drawHeightmap = !startCulled;
            }

        }

        if (lowPolyTerrain != null) // Check if the object was found
        {
            // Get all MeshRenderer components in children
            Terrain[] childMeshRenderers = lowPolyTerrain.GetComponentsInChildren<Terrain>();

            // Toggle the enabled state of each MeshRenderer
            foreach (Terrain terrain in childMeshRenderers)
            {
                //terrain.drawHeightmap = !startCulled;
            }

        }


    }

    // Update is called once per frame
    void Update()
    {
        #region Disable Objects

        if (Input.GetKeyDown(KeyCode.P))
        {
            // Find the parent object with the tag 'culling'

            if (cullingParent != null && toogleObjects) // Check if the object was found
            {
                // Get all MeshRenderer components in children
                MeshRenderer[] childMeshRenderers = cullingParent.GetComponentsInChildren<MeshRenderer>();

                // Toggle the enabled state of each MeshRenderer
                foreach (MeshRenderer renderer in childMeshRenderers)
                {
                    renderer.enabled = !renderer.enabled;
                }

                Debug.Log("P Pressed and child meshes toggled");
            }


            if (highPolyTerrain != null && toggleHighPoly) // Check if the object was found
            {
                // Get all MeshRenderer components in children
                Terrain[] childMeshRenderers = highPolyTerrain.GetComponentsInChildren<Terrain>();

                // Toggle the enabled state of each MeshRenderer
                foreach (Terrain terrain in childMeshRenderers)
                {
                    //terrain.drawHeightmap = !terrain.drawHeightmap;
                }

            }


            if (lowPolyTerrain != null && toggleLowPoly) // Check if the object was found
            {
                // Get all MeshRenderer components in children
                Terrain[] childMeshRenderers = lowPolyTerrain.GetComponentsInChildren<Terrain>();

                // Toggle the enabled state of each MeshRenderer
                foreach (Terrain terrain in childMeshRenderers)
                {
                    //terrain.drawHeightmap = !terrain.drawHeightmap;
                }

            }
        }
        #endregion

    }
}
