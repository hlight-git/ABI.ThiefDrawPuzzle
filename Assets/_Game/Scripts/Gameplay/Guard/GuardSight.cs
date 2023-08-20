using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSight : GameUnit
{
    [SerializeReference] MeshFilter meshFilter;
    [SerializeField] bool rotateWhenGuardIdle;
    [SerializeReference, ShowIf("rotateWhenGuardIdle")] Transform rotateBaseTF;

    int triangleCount = Const.GUARD_SIGHT_RESOLUTION - 1;
    IGuard guard;
    float visionRange;
    float visionAngle;
    int[] triangles;


    public LayerMask VisionObstructingLayer;

    Mesh visionConeMesh;
    void Awake()
    {
        visionConeMesh = new Mesh();

        triangles = new int[triangleCount * 3];
        for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j + 2;
        }
    }
    public void OnInit(IGuard guard)
    {
        this.guard = guard;
        visionRange = guard.VisionRange;
        visionAngle = guard.VisionAngle * Mathf.Deg2Rad;
    }

    void Update()
    {
        DrawVisionCone();
        if (rotateWhenGuardIdle)
        {
            TF.rotation = Quaternion.Euler(0, rotateBaseTF.rotation.eulerAngles.y, 0);
        }
    }
    void DrawVisionCone()
    {
        Vector3[] vertices = new Vector3[Const.GUARD_SIGHT_RESOLUTION + 1];
        vertices[0] = Vector3.zero;
        float curAngle = -visionAngle / 2;
        float angleInc = visionAngle / triangleCount;
        float sine;
        float cosine;

        for (int i = 0; i < Const.GUARD_SIGHT_RESOLUTION; i++, curAngle += angleInc)
        {
            sine = Mathf.Sin(curAngle);
            cosine = Mathf.Cos(curAngle);
            Vector3 raycastDir = (TF.forward * cosine) + (TF.right * sine);
            Vector3 vertForward = (Vector3.forward * cosine) + (Vector3.right * sine);
            if (Physics.Raycast(TF.position, raycastDir, out RaycastHit hit, visionRange, VisionObstructingLayer))
            {
                vertices[i + 1] = vertForward * hit.distance;
                OnRaycastHit(hit);
            }
            else
            {
                vertices[i + 1] = vertForward * visionRange;
            }
        }
        visionConeMesh.Clear();
        visionConeMesh.vertices = vertices;
        visionConeMesh.triangles = triangles;
        meshFilter.mesh = visionConeMesh;
    }
    void OnRaycastHit(RaycastHit hit)
    {
        if (hit.collider.CompareTag(Const.Tag.CAT))
        {
            Cat cat = hit.collider.GetComponent<Cat>();
            guard.OnSawCat(cat);
            //if (cat.IsMoving)
            //{
            //    guard.OnSawCat(cat);
            //}
        }
    }
}
