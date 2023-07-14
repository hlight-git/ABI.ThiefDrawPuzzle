using System.Collections.Generic;
using UnityEngine;

public class WaiterCache
{
    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
    public static readonly WaitForSeconds WaitFor100ms = new WaitForSeconds(0.1f);
    public static readonly WaitForSeconds WaitForSec = new WaitForSeconds(1f);
}
public class ComponentCache<TValue>
{
    static readonly Dictionary<int, TValue> cacheDict = new Dictionary<int, TValue>();
    public static TValue Get<TKey>(TKey key) where TKey : Component
    {
        int hashCode = key.GetHashCode();
        if (!cacheDict.ContainsKey(hashCode))
        {
            cacheDict.Add(hashCode, key.GetComponent<TValue>());
        }
        return cacheDict[hashCode];
    }
    public static void Clear()
    {
        cacheDict.Clear();
    }
}