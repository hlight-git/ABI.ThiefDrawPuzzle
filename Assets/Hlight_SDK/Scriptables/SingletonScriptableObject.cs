using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
{
    static T ins;
    public static T Ins
    {
        get
        {
            if (ins == null)
            {
                //T[] ins_founds = Resources.FindObjectsOfTypeAll<T>();   -> Can't found the instance
                T[] ins_founds = Resources.LoadAll<T>("");
                if (ins_founds.Length == 0)
                {
                    Debug.Log($"{typeof(T)}: Not found any instance!");
                    return null;
                }
                if (ins_founds.Length > 1)
                {
                    Debug.Log($"{typeof(T)}: Found {ins_founds.Length} instances!");
                }
                ins = ins_founds[0];
            }
            return ins;
        }
    }
}
