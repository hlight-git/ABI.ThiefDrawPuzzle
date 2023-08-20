using UnityEngine;

public class GameUnit : MonoBehaviour
{
    private Transform tf;

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
public class PoolUnit : GameUnit
{
    public PoolType poolType;
}