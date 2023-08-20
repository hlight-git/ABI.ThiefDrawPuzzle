using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Pool Attacher", menuName = "HlighTool/Pool Attacher")]
public class PoolAttacher : ScriptableObject
{
    public string path;
    public string prefix;
    public int startValue;
    [TextArea(10, 20)] public string result;
    [Button]
    void GenerateEnum()
    {
        GameObject[] units = Resources.LoadAll<GameObject>(path);
        List<string> res = new List<string>();
        for(int i = 0; i < units.Length; i++)
        {
            res.Add(prefix + "_" + units[i].name.Replace(" ", "") + " = " + (startValue + i));
        }
        result = string.Join(",\n", res);
    }

    [Button]
    void Attach()
    {
        GameObject[] units = Resources.LoadAll<GameObject>(path);
        for (int i = 0; i < units.Length; i++)
        {
            if (!units[i].TryGetComponent(out PoolUnit u))
            {
                u = units[i].AddComponent<PoolUnit>();
            }
            u.poolType = (PoolType)(startValue + i);
        }
    }
}
