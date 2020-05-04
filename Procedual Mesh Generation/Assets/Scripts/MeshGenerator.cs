using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    public int xSize = 20;
    public int zSize = 20;

    public bool useRandomSeed = true;
    public string seed = "Hi";

    public Mesh mesh;

    private Vector3[] vertices;
    private int[] triangles;

    private void OnValidate()
    {
        CreateMeshData();
        CreateMesh();
    }

    private void CreateMeshData()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        int i = 0;
        for (float z = 0; z <= zSize; z++)
        {
            for (float x = 0; x <= xSize; x++)
            {
                float y = CalculateHeight(x, z);
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < zSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    private void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    private float CalculateHeight(float x, float z)
    {
        if (useRandomSeed)
            seed = Time.time.ToString();
        System.Random random = new System.Random(seed.GetHashCode());

        return Mathf.PerlinNoise(x / 16 * 1.5f + 0.001f + random.Next(1000, 100000), z / 16 * 1.5f + 0.001f + random.Next(1000, 100000)) * 5;
    }

    private void OnDrawGizmos()
    {
        if (vertices != null)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Gizmos.DrawSphere(vertices[i], 0.1f);
            }
        }
    }
}
