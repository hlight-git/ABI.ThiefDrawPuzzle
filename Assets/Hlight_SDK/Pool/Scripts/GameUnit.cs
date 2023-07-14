using UnityEngine;

public class GameUnit : MonoBehaviour
{
    private Transform tf;

    public PoolType poolType;
    public Transform TF
    {
        get
        {
            if (tf == null)
            {
                tf = transform;
            }
            return tf;
        }
    }
}