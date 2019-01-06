using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{
    private Mesh m_hexMesh;
    private List<Vector3> m_vertices;
    private List<int> m_triangles;
    private MeshCollider m_meshCollider;
    private List<Color> m_colors;

    private void Awake ()
    {
        this.GetComponent<MeshFilter>().mesh = this.m_hexMesh = new Mesh();
        this.m_meshCollider = this.gameObject.AddComponent<MeshCollider>();
        this.m_hexMesh.name = "Hex Mesh";
        this.m_vertices = new List<Vector3>();
        this.m_triangles = new List<int>();
        this.m_colors = new List<Color>();
    }

    public void Triangulate (HexCell[] cells)
    {
        this.m_hexMesh.Clear();
        this.m_vertices.Clear();
        this.m_triangles.Clear();
        this.m_colors.Clear();

        for (int i = 0; i < cells.Length; i++)
        {
            this.Triangulate(cells[i]);
        }

        this.m_hexMesh.vertices = this.m_vertices.ToArray();
        this.m_hexMesh.triangles = this.m_triangles.ToArray();
        this.m_hexMesh.colors = this.m_colors.ToArray();
        this.m_hexMesh.RecalculateNormals();

        this.m_meshCollider.sharedMesh = this.m_hexMesh;
    }

    private void Triangulate (HexCell cell)
    {
        Vector3 center = cell.transform.localPosition;

        for (int i = 0; i < 6; i++)
        {
            this.AddTriangle(
                center,
                center + HexMetrics.corners[i],
                center + HexMetrics.corners[i + 1]    
            );
            this.AddTriangleColor(cell.color);
        }
    }

    private void AddTriangle (Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = this.m_vertices.Count;
        this.m_vertices.Add(v1);
        this.m_vertices.Add(v2);
        this.m_vertices.Add(v3);
        this.m_triangles.Add(vertexIndex);
        this.m_triangles.Add(vertexIndex + 1);
        this.m_triangles.Add(vertexIndex + 2);
    }

    private void AddTriangleColor (Color color)
    {
        this.m_colors.Add(color);
        this.m_colors.Add(color);
        this.m_colors.Add(color);
    }
}
