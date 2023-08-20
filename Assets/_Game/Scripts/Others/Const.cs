using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Const
{
    public static readonly Vector3 DRAWLINE_AND_FLOOR_OFFSET = new Vector3(0, .1f, 0);
    public const float ACCEPTABLE_DISTANCE_FROM_CAT_TO_HITPOINT = 1f;
    public const float MIN_DISTANCE_BETWEEN_2_DRAW_POINT = 0.1f;
    public const float DOG_CATCH_CAT_RANGE = 1f;
    public const int GUARD_SIGHT_RESOLUTION = 10;
    public struct Layer
    {
        public const string CATCH_RAY_OF_CAT = "CatchRayOfCat";
        public const string CATCH_RAY = "CatchRay";
        public const string OBSTACLE = "Obstacle";
    }

    public struct Anim
    {
        public struct Cat
        {
            public const string IDLE = "idle";
            public const string RUN = "run";
            public const string VICTORY = "victory";
            public const string DIE = "die";
        }
        public struct Dog
        {
            public const string IDLE = "idle";
            public const string WALK = "walk";
            public const string RUN = "run";
            public const string ATTACK = "attack";
            public const string VICTORY = "victory";
            public const string DIE = "die";
        }
        public struct Cabinet
        {
            public const string OPEN = "Open";
            public const string CLOSE = "Close";
        }
    }
    public struct Tag
    {
        public const string CAT = "Cat";
        public const string FLOOR = "Floor";
        public const string OBSTACLE = "Obstacle";
        public const string DESTINATION = "Destination";
        public const string COLLECTABLE_ITEM = "CollectableItem";
    }
}
