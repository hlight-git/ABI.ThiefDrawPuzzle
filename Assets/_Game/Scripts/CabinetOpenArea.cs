using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetOpenArea : MonoBehaviour
{
    [SerializeReference] Animator animator;
    AnimTrigger animTrigger;
    private void Awake()
    {
        animTrigger = new AnimTrigger(animator);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Const.Tag.CAT))
        {
            animTrigger.TriggerAnim(Const.Anim.Cabinet.OPEN);
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag(Const.Tag.CAT))
        {
            animTrigger.TriggerAnim(Const.Anim.Cabinet.CLOSE);
        }
    }
}
