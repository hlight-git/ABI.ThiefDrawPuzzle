using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsManager : Singleton<InsManager>
{
    [SerializeReference] PlayerLineOfSight playerLineOfSight;
    [SerializeReference] Player player;
    [SerializeReference] Transform destination;

    public PlayerLineOfSight PlayerLineOfSight { get => playerLineOfSight; set => playerLineOfSight = value; }
    public Player Player { get => player; set => player = value; }
    public Transform Destination { get => destination; set => destination = value; }
}
