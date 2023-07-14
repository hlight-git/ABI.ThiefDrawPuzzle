using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSight : GameUnit
{
    [SerializeReference] Transform guardTF;
    [SerializeReference] MeshFilter meshFilter;
    [SerializeField] float visionRange;
    [SerializeField] float visionAngle;

    readonly int visionConeResolution = 10;
    IGuard guard;

    public LayerMask VisionObstructingLayer;

    Mesh visionConeMesh;
    void Awake()
    {
        visionConeMesh = new Mesh();
        visionAngle *= Mathf.Deg2Rad;
        guard = guardTF.GetComponent<IGuard>();
    }


    void Update()
    {
        DrawVisionCone();
    }

    void DrawVisionCone()
    {
        int[] triangles = new int[(visionConeResolution - 1) * 3];
        Vector3[] Vertices = new Vector3[visionConeResolution + 1];
        Vertices[0] = Vector3.zero;
        float Currentangle = -visionAngle / 2;
        float angleIcrement = visionAngle / (visionConeResolution - 1);
        float Sine;
        float Cosine;

        for (int i = 0; i < visionConeResolution; i++)
        {
            Sine = Mathf.Sin(Currentangle);
            Cosine = Mathf.Cos(Currentangle);
            Vector3 RaycastDirection = (transform.forward * Cosine) + (transform.right * Sine);
            Vector3 VertForward = (Vector3.forward * Cosine) + (Vector3.right * Sine);
            if (Physics.Raycast(transform.position, RaycastDirection, out RaycastHit hit, visionRange, VisionObstructingLayer))
            {
                Vertices[i + 1] = VertForward * hit.distance;
                OnRaycastHit(hit);
            }
            else
            {
                Vertices[i + 1] = VertForward * visionRange;
            }


            Currentangle += angleIcrement;
        }
        for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j + 2;
        }
        visionConeMesh.Clear();
        visionConeMesh.vertices = Vertices;
        visionConeMesh.triangles = triangles;
        meshFilter.mesh = visionConeMesh;
    }
    void OnRaycastHit(RaycastHit hit)
    {
        if (hit.collider.CompareTag(Constant.Tag.PLAYER) && !InsManager.Ins.Player.IsDisguising)
        {
            guard.OnSawThief();
        }
    }
}
