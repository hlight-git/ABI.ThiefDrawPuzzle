using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HlightSDK
{
    namespace Tool
    {
        namespace Level
        {
            namespace Compress
            {
#if UNITY_EDITOR
                public static class Compresser
                {
                    const string EXTRACT_DESTINATION = "Assets/_Game/Resources/Data/Lv/DataSet";

                    [MenuItem("Hlight/Tool/Level/Compress")]
                    public static void Compress()
                    {
                        GameObject[] selections = Selection.gameObjects;
                        for (int i = 0; i < selections.Length; i++)
                        {
                            CompressLevel(selections[i]);
                        }
                    }

                    // Get prefab's root in hierarchy workflow
                    static GameObject GetPrefabRoot(GameObject obj) => PrefabUtility.GetOutermostPrefabInstanceRoot(obj);
                    // Get prefab's origin version
                    static GameObject GetPrefabSource(GameObject obj) => PrefabUtility.GetCorrespondingObjectFromSource(obj);
                    // Get PrefabData in a set of PrefabData
                    static PrefabData GetPrefabData(ref List<PrefabData> prefabDatas, GameObject gameObject)
                    {
                        PrefabData prefabData = null;

                        for (int i = 0; i < prefabDatas.Count; i++)
                        {
                            if (prefabDatas[i].prefab == gameObject)
                            {
                                prefabData = prefabDatas[i];
                            }
                        }
                        if (prefabData == null)
                        {
                            prefabData = new PrefabData() { prefab = gameObject };
                            prefabDatas.Add(prefabData);
                        }

                        return prefabData;
                    }
                    static void CompressLevel(GameObject levelGO)
                    {
                        List<PrefabData> prefabDatas = new List<PrefabData>();

                        var deeperSelection = Selection.gameObjects.SelectMany(go => levelGO.GetComponentsInChildren<Transform>(true))
                            .Select(t => t.gameObject);

                        GameObject parent = null;

                        HashSet<GameObject> hash = new HashSet<GameObject>();

                        foreach (var deeper in deeperSelection)
                        {
                            if (parent == null)
                            {
                                parent = deeper;
                            }

                            GameObject prefab = GetPrefabRoot(deeper);
                            if (prefab != null && prefab.activeSelf && !hash.Contains(prefab))
                            {
                                hash.Add(prefab);
                            }
                        }

                        foreach (var item in hash)
                        {
                            GameObject root = GetPrefabSource(item);

                            PrefabData prefabData = GetPrefabData(ref prefabDatas, root);

                            if (item.TryGetComponent(out IComplexLvGO complex))
                            {
                                prefabData.isComplex = true;
                                prefabData.json.Add(complex.CompressData);
                            }
                            else
                            {
                                prefabData.json.Add(new LvGOData().OnCompressed(item.transform));
                            }
                        }

                        SaveDataSet(EXTRACT_DESTINATION + "/" + parent.name, prefabDatas);
                    }

                    static void SaveDataSet(string path, List<PrefabData> prefabDatas)
                    {
                        DataSet dataSet = ScriptableObject.CreateInstance<DataSet>();
                        AssetDatabase.CreateAsset(dataSet, path + ".asset");

                        dataSet.Datas = prefabDatas;

                        Undo.RegisterCompleteObjectUndo(dataSet, "Save level data");
                        EditorUtility.SetDirty(dataSet);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();

                        Debug.Log($"!!! SAVE {path} COMPLETE !!! ");
                    }
                }
#endif
            }
        }
    }
}