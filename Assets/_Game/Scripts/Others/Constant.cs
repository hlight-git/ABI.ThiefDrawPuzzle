using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant : MonoBehaviour
{
    public class Layer
    {
        public const string FLOOR = "Floor";
    }

    public const float NEAR_DISTANCE = 1f;
    public class Anim
    {
        public class Player
        {
            public const string IDLE = "Idle";
            public const string SNEAK = "Sneak";
            public const string FIGHT = "Fight";
            public const string KNOCKEDOUT = "KnockedOut";
            public const string SURRENDER = "Surrender";

            public const float IDLE_DURATION = 4f;
            public const float SNEAK_DURATION = 1.667f;
            public const float FIGHT_DURATION = 2.967f;
            public const float KNOCKEDOUT_DURATION = 4.833f;
            public const float SURRENDER_DURATION = 4.8f;
        }
        public class Guard
        {
            public const string IDLE = "Idle";
            public const string PATROL = "Patrol";
            public const string CHASE = "Chase";
            public const string PUNCH = "Punch";

            public const float IDLE_DURATION = 1.967f;
            public const float PATROL_DURATION = 1.017f;
            public const float CHASE_DURATION = 0.7f;
            public const float PUNCH_DURATION = 2f;
        }
    }
    public class Tag
    {
        public const string PLAYER = "Player";
        public const string COLLECTABLE_ITEM = "CollectableItem";
    }
}
