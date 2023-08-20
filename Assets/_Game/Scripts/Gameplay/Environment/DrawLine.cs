using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class DrawLine : PoolUnit
{
    [SerializeReference] LineRenderer lineRenderer;

    public List<Vector3> Points { get; private set; } = new List<Vector3>();
    public Vector3 LastPoint => Points.Last();
    public bool IsDrawn => Points.Count > 0;
    public Material Material
    {
        get => lineRenderer.material;
        set => lineRenderer.material = value;
    }
    void AddPoint(Vector3 point)
    {
        Points.Add(point);

        lineRenderer.positionCount++;
        lineRenderer.SetPosition(Points.Count - 1, point);
    }
    public float DistanceToLastPoint(Vector3 position) => Vector3.Distance(Points.Last(), position);
    public void UpdateLine(Vector3 position, bool addDefaultOffset = true)
    {
        if (addDefaultOffset)
        {
            position += Const.DRAWLINE_AND_FLOOR_OFFSET;
        }
        if (Points.Count == 0 || DistanceToLastPoint(position) > Const.MIN_DISTANCE_BETWEEN_2_DRAW_POINT)
        {
            AddPoint(position);
        }
    }

    public void Remove()
    {
        Points.Clear();
        lineRenderer.positionCount = 0;
    }
}
