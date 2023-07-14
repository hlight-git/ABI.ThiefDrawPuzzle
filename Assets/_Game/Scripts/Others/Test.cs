using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform[] pos;
    Tween tween;
    public Vector3[] GetPath => pos.Select(tf => tf.position).ToArray();

    private void Start()
    {
        tween = transform.DOPath(GetPath, 3).SetLoops(-1, LoopType.Yoyo).SetSpeedBased(true).SetEase(Ease.Linear)
            .OnWaypointChange(OnWayPointChange);
    }

    void OnWayPointChange(int i)
    {
        if (i == 0 || i == pos.Length - 1)
        {
            tween.Pause();
            Invoke(nameof(Play), 1f);
        }
    }
    void Play()
    {
        tween.Play();
    }
}
