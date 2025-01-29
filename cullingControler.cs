using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class cullingControler : MonoBehaviour
{
    [SerializeField]
    private bool ViewFrustumCullingEnable;

    private bool ViewFrustumLastState;

    [SerializeField]
    private bool HierarchicalCullingEnable;

    private bool HierarchicalCullingLastState;

    [SerializeField]
    private bool BruteForceBoxCullingEnable;

    private bool BruteForceBoxCullingLastState;

    [SerializeField]
    private bool ImposterEnable;

    private bool ImposterLastState;

    [SerializeField]
    Camera cameraObject;

    Plane[] viewFrustum;

    private Terrains terrains;

    private GameObject player;

    private Vector3 LastPosition;
    private Quaternion LastRotation;


    // Start is called before the first frame update
    void Start()
    {
        cameraObject = Camera.main;
        terrains = Terrains.Instance;
        player = this.gameObject;

    }

    // Update is called once per frame
    void Update()
    {

        // if any of the culling methods change state, reset active objects
        if (ViewFrustumCullingEnable != ViewFrustumLastState || HierarchicalCullingEnable != HierarchicalCullingLastState || BruteForceBoxCullingEnable != BruteForceBoxCullingLastState || ImposterEnable != ImposterLastState)
        {
            terrains.resetStates();
            updateGroups();
            updateObjects();
            updateImposters();
            ViewFrustumLastState = ViewFrustumCullingEnable;
            HierarchicalCullingLastState = HierarchicalCullingEnable;
            BruteForceBoxCullingLastState = BruteForceBoxCullingEnable;
            ImposterLastState = ImposterEnable;
        }

        // if 'T' is pressed, toggle view frustum culling
        if (Input.GetKeyDown(KeyCode.T))
        {
            ViewFrustumCullingEnable = !ViewFrustumCullingEnable;
        }
        // if 'Y' is pressed, toggle hierarchical culling
        if (Input.GetKeyDown(KeyCode.Y))
        {
            HierarchicalCullingEnable = !HierarchicalCullingEnable;
        }
        // if 'U' is pressed, toggle brute force box culling
        if (Input.GetKeyDown(KeyCode.U))
        {
            BruteForceBoxCullingEnable = !BruteForceBoxCullingEnable;
        }
        // if 'I' is pressed, toggle imposter culling
        if (Input.GetKeyDown(KeyCode.I))
        {
            ImposterEnable = !ImposterEnable;
        }






        if (player.transform.rotation != LastRotation)
        {
            LastRotation = player.transform.rotation;
            viewFrustum = GeometryUtility.CalculateFrustumPlanes(cameraObject);
            cameraObject.transform.hasChanged = false;

            if (ViewFrustumCullingEnable)
            {
                ViewFrustumCulling.BasicCulling(cameraObject, viewFrustum, terrains);
            }
            else if (HierarchicalCullingEnable)
            {
                updateGroups();
                updateObjects();
            }

            if (BruteForceBoxCullingEnable)
            {
                BoxCulling();
                updateObjects();
            }

            if (ImposterEnable)
            {
                imposterEnable.EnableImposters(terrains);
                updateImposters();
            }

        }
    }
    private bool HierarchicalCulling()
    {
        return hierarchicalCulling.HierarchicalEnabled(cameraObject, viewFrustum, terrains);
    }

    private void BoxCulling()
    {
        bool result = boxCulling.BoxOcclusion(terrains, player, cameraObject);
    }


    // call after hierarchical cull
    private void updateGroups()
    {
        // foreach object group not in active object groups, cull
        foreach (ObjectGroup group in terrains.GetInactiveGroups())
        {
            group.disableGroup(terrains);
        }

        // foreach object group in active object groups, enable
        foreach (ObjectGroup group in terrains.GetActiveGroups())
        {
            group.enableGroup(terrains);
        }

    }


    // call after box cull
    private void updateObjects() 
    {

        // foreach object not in active objects, cull
        foreach (GameObject obj in terrains.GetInactiveObjects())
        {
            // get objects mesh renderer from cache
            terrains.ToggleRenderer(obj, false);
        }

        // foreach object in active objects, enable
        foreach (GameObject obj in terrains.GetActiveObjects())
        {
            terrains.ToggleRenderer(obj, true);

        }
    }

    private void updateImposters()
    {
        // if object is far enough, enable imposter and disable original object
        foreach (GameObject obj in terrains.GetImposters())
        {
            // if object has imposter child and is not in inactive objects
            if (obj.transform.childCount > 0)
            {

                GameObject imposter = obj.transform.GetChild(0).gameObject;
                if (terrains.GetActiveObjects().Contains(obj))
                {
                    // enable mesh of imposter child
                    imposter.GetComponent<MeshRenderer>().enabled = true;
                    // rotate imposter only around the Y axis to face the player
                    Vector3 lookDirection = player.transform.position - imposter.transform.position;
                    lookDirection.y = 0; // This nullifies any vertical difference in the look direction

                    // Create a rotation that only considers the Y-axis
                    Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
                    // Apply the Y-axis rotation to the imposter while preserving its original X and Z rotations
                    Vector3 currentAngles = imposter.transform.eulerAngles;
                    imposter.transform.rotation = Quaternion.Euler(currentAngles.x, lookRotation.eulerAngles.y, currentAngles.z);


                    // disable mesh of original object
                    terrains.ToggleRenderer(obj, false);
                }
                else
                {
                    imposter.GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
        // if object is close enough, disable imposter and enable original object
        foreach (GameObject obj in terrains.GetNullImposters())
        {
            if (obj.transform.childCount > 0)
            {
                if (terrains.GetActiveObjects().Contains(obj))
                {
                    // disable mesh of imposter child
                    obj.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                    // enable mesh of original object
                    terrains.ToggleRenderer(obj, true);
                }
                obj.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}
