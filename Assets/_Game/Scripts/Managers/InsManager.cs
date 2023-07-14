using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsManager : Singleton<InsManager>
{
    [SerializeReference] PressHandler pressHandler;
    [SerializeReference] PlayerLineOfSight playerLineOfSight;
    [SerializeReference] Player player;
    [SerializeReference] Level level;

    public PressHandler PressHandler { get => pressHandler; set => pressHandler = value; }
    public PlayerLineOfSight PlayerLineOfSight { get => playerLineOfSight; set => playerLineOfSight = value; }
    public Player Player { get => player; set => player = value; }
    public Level Level { get => level; set => level = value; }
}
