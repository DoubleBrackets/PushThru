using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneTesselator : MonoBehaviour
{
    public int vertexCountZ;
    public int vertexCountX;

    public int lengthX;
    public int lengthZ;

    private Mesh mesh;

    [ContextMenu("TesselatePlane")]
    public void TesselatePlane()
    {
        mesh = GetComponent<MeshFilter>().mesh = new Mesh();
        GenerateVertices(mesh);
        CreateTriangles(mesh);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    private void GenerateVertices(Mesh mesh)
    {
        Vector3[] vertices = new Vector3[vertexCountX * vertexCountZ];
        Vector2[] UVs = new Vector2[vertexCountX * vertexCountZ];

        float intervalX = (float)lengthX / (vertexCountX-1);
        float intervalZ = (float)lengthZ / (vertexCountZ-1);

        for(int x = 0;x < vertexCountX;x++)
        {
            for(int z = 0;z < vertexCountZ;z++)
            {
                float xPos = (x - vertexCountX / 2f+0.5f)*intervalX;
                float zPos = (z - vertexCountZ / 2f+0.5f) * intervalZ;
                vertices[x * vertexCountZ + z] = new Vector3(xPos, 0, zPos);
                UVs[x * vertexCountZ + z] = new Vector2(x / ((float)vertexCountX - 1), z / ((float)vertexCountZ - 1));
            }
        }

        mesh.vertices = vertices;
        mesh.uv = UVs;
    }

    private void CreateTriangles(Mesh mesh)
    {
        int quadCount = (vertexCountX - 1) * (vertexCountZ - 1);
        int[] triangles = new int[quadCount * 6];

        for(int x = 0;x < quadCount;x++)
        {
            int triIndex = x * 6;
            int zColumn = x / (vertexCountZ - 1);
            int xRow = x % (vertexCountZ - 1);
            triangles[triIndex] = zColumn * vertexCountZ + xRow;
            triangles[triIndex + 1] = (zColumn + 1) * vertexCountZ + xRow + 1;
            triangles[triIndex + 2] = (zColumn + 1) * vertexCountZ + xRow;

            triangles[triIndex + 3] = zColumn * vertexCountZ + xRow;
            triangles[triIndex + 4] = zColumn * vertexCountZ + xRow + 1;
            triangles[triIndex + 5] = (zColumn + 1) * vertexCountZ + xRow + 1;
        }
        mesh.triangles = triangles;
    }
}
