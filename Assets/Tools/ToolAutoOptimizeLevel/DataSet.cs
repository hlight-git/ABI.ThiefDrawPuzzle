using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace HlightSDK
{
    namespace Tool
    {
        namespace Level
        {
            namespace Compress
            {
                public class DataSet : ScriptableObject
                {
                    [SerializeField] List<PrefabData> datas;
                    internal List<PrefabData> Datas { get => datas; set => datas = value; }

                    public void ExtractTo(Transform parent)
                    {
                        parent.position = Vector3.zero;
                        parent.rotation = Quaternion.identity;
                        parent.localScale = Vector3.one;

                        for (int i = 0; i < datas.Count; i++)
                        {
                            for (int j = 0; j < datas[i].json.Count; j++)
                            {
                                Transform tf = Instantiate(datas[i].prefab).transform;
                                LvGOData LvGOData;
                                if (tf.TryGetComponent(out IComplexLvGO goAsLevelGO))
                                {
                                    LvGOData = goAsLevelGO.OnExtract(datas[i].json[j]);
                                }
                                else
                                {
                                    LvGOData = JsonUtility.FromJson<LvGOData>(datas[i].json[j]);
                                }
                                tf.SetPositionAndRotation(LvGOData.position, LvGOData.rotation);
                                tf.localScale = LvGOData.scale;
                                tf.SetParent(parent);
                            }
                        }
                        Debug.Log("Extracted");
                    }
                }

                [System.Serializable]
                public class TransformData
                {
                    public Vector3 position;
                    public Quaternion rotation;
                    public Vector3 scale;
                }

                [System.Serializable]
                class PrefabData
                {
                    [ReadOnly] public bool isComplex;
                    public GameObject prefab;
                    public List<string> json = new List<string> ();
                }
                public class LvGOData
                {
                    [ShowIf("IsLvGOData")] public Vector3 position;
                    [ShowIf("IsLvGOData")] public Quaternion rotation;
                    [ShowIf("IsLvGOData")] public Vector3 scale;
                    public bool IsLvGOData => GetType() == typeof(LvGOData);
                    protected void UpdateTransformData(Transform tf)
                    {
                        position = tf.position;
                        rotation = tf.rotation;
                        scale = tf.lossyScale;
                    }
                    public string OnCompressed(Transform tf)
                    {
                        UpdateTransformData(tf);
                        return JsonUtility.ToJson(this);
                    }
                }

                public interface IComplexLvGO
                {
                    string CompressData { get; }
                    LvGOData OnExtract(string json);
                }
            }
        }
    }
}