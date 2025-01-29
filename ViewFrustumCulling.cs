using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class ViewFrustumCulling : MonoBehaviour
{
    public static void BasicCulling(Camera cameraObject, Plane[] viewFrustum, Terrains terrains)
    {
        foreach (TerrainCell terrain in terrains.GetTerrainCells())
        {
            foreach(ObjectGroup group in terrain.objectGroups)
            {
                foreach (GameObject obj in group.objectList)
                {
                    if (terrains.Colliders.TryGetValue(obj, out BoxCollider collider))
                    {
                        if (GeometryUtility.TestPlanesAABB(viewFrustum, collider.bounds))
                        {
                            terrains.ToggleRenderer(obj, true);
                        }
                        else
                        {
                            terrains.ToggleRenderer(obj, false);
                        }
                    }
                    
                }
            }


        }
    }
}
