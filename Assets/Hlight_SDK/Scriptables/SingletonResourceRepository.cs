using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public interface IResourceRepository
{
    bool LoadResources();
}

public class SingletonResourceRepository<SelfT, KeyT, ValueT> : SingletonScriptableObject<SelfT>, IResourceRepository
    where SelfT : SingletonResourceRepository<SelfT, KeyT, ValueT>
    where ValueT : Object
{
    [SerializeField] protected List<ValueT> resources;

#if UNITY_EDITOR
    [Header("Editor:")]
    [SerializeField] protected string dataPath;

    [Header("Only for key's type is enum:")]
    [SerializeField] protected int firstEnumValue = 0;
    [SerializeField, TextArea] string enumValues;
#endif

    public virtual bool LoadResources()
    {
        ValueT[] data = Resources.LoadAll<ValueT>(dataPath);
        if (data.Length == 0)
        {
            Debug.Log("Not found data at current `dataPath`.");
            return false;
        }
        resources = new List<ValueT>(data);

        CreateEnumValues();

        return true;
    }
    void CreateEnumValues()
    {
        enumValues = "";
        for (int i = 0; i < resources.Count; i++)
        {
            enumValues += $"{resources[i].name} = {i + firstEnumValue},\n";
        }
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(SingletonResourceRepository<,,>), true)]
public class SingletonResourceRepositoryEditor : Editor
{
    protected IResourceRepository resourceRepository;
    protected virtual void OnEnable()
    {
        resourceRepository = (IResourceRepository)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Load Resources"))
        {
            resourceRepository.LoadResources();
            EditorUtility.SetDirty((Object)resourceRepository);
        }
    }
}
#endif