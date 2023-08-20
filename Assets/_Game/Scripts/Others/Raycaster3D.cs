using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster3D : GameUnit
{
    public RaycastHit? Raycast(Vector3 direction, float maxDistance = float.PositiveInfinity, int layerMask = Physics.AllLayers)
    {
        if (Physics.Raycast(TF.position, direction, out RaycastHit hit, maxDistance, layerMask))
        {
            return hit;
        }
        return null;
    }

    public RaycastHit? Linecast(Vector3 endPoint, int layerMask = Physics.AllLayers)
    {
        if (Physics.Linecast(TF.position, endPoint, out RaycastHit hit, layerMask))
        {
            return hit;
        }
        return null;
    }
}
