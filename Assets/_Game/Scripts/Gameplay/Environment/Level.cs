using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeReference] Transform startPointTF;
    [SerializeReference] Transform destinationTF;

    public Vector3 StartPoint => startPointTF.position;
    public Vector3 Destination => destinationTF.position;
}
