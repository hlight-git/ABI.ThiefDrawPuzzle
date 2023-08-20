using System.Collections.Generic;
using UnityEngine;

public class Wait
{
    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

    static readonly Dictionary<float, WaitForSeconds> cacheDict = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds ForSec(float key)
    {
        if (!cacheDict.ContainsKey(key))
        {
            cacheDict.Add(key, new WaitForSeconds(key));
        }
        return cacheDict[key];
    }
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