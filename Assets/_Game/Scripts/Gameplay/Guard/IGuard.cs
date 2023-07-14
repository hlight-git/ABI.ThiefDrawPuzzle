using UnityEngine;

public interface IGuard
{
    Transform TF { get; }
    void OnSawThief();
}