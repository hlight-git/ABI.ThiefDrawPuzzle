using UnityEngine;

public interface IGuard
{
    Transform TF { get; }
    float VisionRange { get; }
    float VisionAngle { get; }
    void OnSawCat(Cat cat);
}