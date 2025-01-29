using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class hierarchicalCulling : MonoBehaviour
{
    private static int groupsChanged = 0;
    // Update is called once per frame
    public static bool HierarchicalEnabled(Camera cameraObject, Plane[] viewFrustum, Terrains terrains)
    {
        TerrainCell[] terrainCells = terrains.GetTerrainCells();
        foreach (var terrain in terrainCells)
        {
            Collider terrainCollider = terrain.TerrainsCollider;
            if (terrainCollider != null && !GeometryUtility.TestPlanesAABB(viewFrustum, terrainCollider.bounds))
            {
                terrain.cullTerrain();  // If terrain is out of view, cull it
                continue;  // Skip further processing for this terrain
            }

            terrain.enableTerrain();
            foreach (var group in terrain.objectGroups)
            {
                Collider groupCollider = group.groupsCollider;
                if (groupCollider != null && GeometryUtility.TestPlanesAABB(viewFrustum, groupCollider.bounds))
                {
                    if (terrains.AddToActiveGroups(group)){
                        groupsChanged++;
                    }
                }
                else
                {
                    if (terrains.RemoveFromActiveGroups(group))
                    {
                        groupsChanged++;
                    }
                }
            }
        }
        if (groupsChanged > 0)
        {
            return true;
        }
        return false;
    }



}
