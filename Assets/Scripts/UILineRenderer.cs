using UnityEngine;
using UnityEngine.UI;

public class UILineRenderer : Graphic
{
    public Vector2[] points = null;
    public float thickness = 10f;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (points == null || points.Length == 0) return;

        for(int i = 0; i < points.Length; i++)
        {
            DrawPoint(points[i], i, vh);
        }

        for (int i = 0; i < points.Length - 1; i++)
        {
            DrawLine(i, vh);
        }
    }

    protected void DrawPoint(Vector2 point, int index, VertexHelper vh)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = point + Vector2.up * thickness / 2f;
        vh.AddVert(vertex);

        vertex.position = point + Vector2.right * thickness / 2f;
        vh.AddVert(vertex);

        vertex.position = point + Vector2.down * thickness / 2f;
        vh.AddVert(vertex);

        vertex.position = point + Vector2.left * thickness / 2f;
        vh.AddVert(vertex);

        int offset = index * 4;

        vh.AddTriangle(offset, offset + 1, offset + 2);
        vh.AddTriangle(offset + 2, offset + 3, offset);
    }

    protected void DrawLine(int index, VertexHelper vh)
    {
        int offset = index * 4;

        //Vertical
        vh.AddTriangle(offset, offset + 4, offset + 2);
        vh.AddTriangle(offset + 4, offset + 6, offset + 2);
        vh.AddTriangle(offset + 1, offset + 5, offset + 7);
        vh.AddTriangle(offset + 3, offset + 1, offset + 7);
    }

    private void Update()
    {
        SetVerticesDirty();
    }
}
