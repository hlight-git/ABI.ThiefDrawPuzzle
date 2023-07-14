using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AnyStateAnimSet", menuName = "Scriptable Objects/AnyStateAnimSet")]
public class AnyStateAnimSet : ScriptableObject
{
    const int BASE_LAYER_INDEX = 0;
    const int TRANSITION_CONDITION_INDEX = 0;
    const string CONST_STRING_PREFIX = "public const string ";
    const string CONST_FLOAT_PREFIX = "public const float ";
    [SerializeField] AnimatorController controller;
    [SerializeField] List<AnyStateAnim> animSet;
#if UNITY_EDITOR
    [SerializeField, TextArea] string EnumValues;
    [SerializeField, TextArea] string ConstantValues;
#endif
    public AnyStateAnim Get<EnumT>(EnumT animType) where EnumT : System.Enum
    {
        return animSet[(int)(object)animType];
    }
    public void LoadResources()
    {
        animSet = new List<AnyStateAnim>();
        foreach (AnimatorStateTransition t in controller.layers[BASE_LAYER_INDEX].stateMachine.anyStateTransitions)
        {
            animSet.Add(new AnyStateAnim(t.conditions[TRANSITION_CONDITION_INDEX].parameter, t.destinationState.motion.averageDuration));
        }

        CreateEnumValues();
        CreateConstantValues();
    }
    void CreateEnumValues()
    {
        EnumValues = animSet[0].trigger + " = 0";
        for (int i = 1; i < animSet.Count; i++)
        {
            EnumValues += ",\n" + animSet[i].trigger + " = " + i;
        }
    }
    void CreateConstantValues()
    {
        ConstantValues = "";
        for (int i = 0; i < animSet.Count; i++)
        {
            ConstantValues += CONST_STRING_PREFIX + animSet[i].trigger.ToUpper() + " = " + "\"" + animSet[i].trigger + "\";\n";
        }
        for (int i = 0; i < animSet.Count; i++)
        {
            ConstantValues += "\n" + CONST_FLOAT_PREFIX + animSet[i].trigger.ToUpper() + "_DURATION = " + animSet[i].duration.ToString("0.###") + "f;";
        }
    }
}

[System.Serializable]
public class AnyStateAnim
{
    public string trigger;
    public float duration;
    public AnyStateAnim(string trigger, float duration)
    {
        this.trigger = trigger;
        this.duration = duration;
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(AnyStateAnimSet))]
public class AnyStateAnimSetEditor : Editor
{
    protected AnyStateAnimSet resourceRepository;
    protected virtual void OnEnable()
    {
        resourceRepository = (AnyStateAnimSet)target;
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