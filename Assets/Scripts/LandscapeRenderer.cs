using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LandscapeRenderer : MonoBehaviour
{
    public int gridSize = 5000;
    public float scale = 5f;
    public float heightMultiplier = 10f;
    private List<Vector3> coordinates = new List<Vector3>();
    public GraphRenderer graphRenderer;

    private void Update()
    {
        if (graphRenderer.graphController.exportData)
        {
            GetCoordinates();

            List<float> minMaxY = GetMinMaxY(coordinates);
            Debug.Log(minMaxY[0]);
            // Generate mesh
            Mesh mesh = GenerateMesh();

            // Create a new GameObject to hold the mesh
            GameObject meshObject = new GameObject("Perlin Noise Mesh");
            meshObject.transform.position = Vector3.zero;

            // Add MeshFilter and MeshRenderer components to the GameObject
            MeshFilter meshFilter = meshObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;
            MeshRenderer meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshRenderer.material = new Material(Shader.Find("Standard"));

            // Set the height of the mesh based on the Perlin noise values


            Vector3[] vertices = mesh.vertices;
            /*
            for (int i = 0; i < vertices.Length; ++i)
            {
                Vector3 vert = vertices[i];
                //float height = Mathf.PerlinNoise(vertices[i].x / gridSize * scale, vertices[i].z / gridSize * scale) * heightMultiplier;
                //vertices[i].y = height;
                foreach (Vector3 vec in coordinates)
                {
                    if (Mathf.Abs(vec.x - vert.x) < 1.0f & Mathf.Abs(vec.z - vert.z) < 1.0f)
                    {
                        //vertices[i].y = 10.0f;
                        vertices[i].y = (vec.y-1553)/10;
                    }
                }
            }
            mesh.vertices = vertices;
            */
            for (int i = 0; i < vertices.Length; ++i)
            {
                Vector3 vert = vertices[i];
                float totalHeight = 0.0f;

                foreach (Vector3 vec in coordinates)
                {
                    if (Mathf.Abs(vec.x - vert.x) < 2.0f & Mathf.Abs(vec.z - vert.z) < 2.0f)
                    {
                        vertices[i].y = (vec.y - 1553) / 10;
                        totalHeight += vertices[i].y;
                    }
                }

                // Smooth the vertex height
                float averageHeight = totalHeight / vertices.Length;

                Debug.Log(averageHeight);
                vertices[i].y = Mathf.Lerp(vertices[i].y, averageHeight, 0.5f);
            }

            mesh.vertices = vertices;

            // Recalculate the normals and bounds of the mesh
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            graphRenderer.graphController.exportData = false;
        }
    }

    private void GetCoordinates() {
        foreach (GameObject node in graphRenderer.nodes)
        {
            // TextMesh textMesh = node.GetComponentInChildren<TextMesh>();
            TextMesh secondTextMesh = node.transform.GetChild(1).GetComponent<TextMesh>();
            float y = float.Parse(secondTextMesh.text);
            Vector3 vec = new Vector3(node.transform.position.x, y, node.transform.position.z);
            coordinates.Add(vec);
        }
    }

    private List<float> GetMinMaxY(List<Vector3> coordinatesList)
    {
        List<float> minMaxList = new List<float>();
        float minY = Mathf.Infinity;
        float maxY = Mathf.NegativeInfinity;

        foreach (Vector3 coordinate in coordinatesList)
        {
            if (coordinate.y < minY)
            {
                minY = coordinate.y;
            }
            if (coordinate.y > maxY)
            {
                maxY = coordinate.y;
            }
        }
        minMaxList.Add(minY);
        minMaxList.Add(maxY);
        return minMaxList;

    }

    Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();

        // Generate vertices
        Vector3[] vertices = new Vector3[(gridSize + 1) * (gridSize + 1)];
        for (int x = 0; x <= gridSize; x++)
        {
            for (int z = 0; z <= gridSize; z++)
            {
                float xPos = x - gridSize / 2f;
                float zPos = z - gridSize / 2f;
                vertices[x * (gridSize + 1) + z] = new Vector3(xPos, 0f, zPos);
            }
        }
        mesh.vertices = vertices;

        // Generate triangles
        int[] triangles = new int[gridSize * gridSize * 6];
        int triangleIndex = 0;
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                int vertexIndex = x * (gridSize + 1) + z;
                triangles[triangleIndex] = vertexIndex;
                triangles[triangleIndex + 1] = vertexIndex + 1;
                triangles[triangleIndex + 2] = vertexIndex + gridSize + 1;
                triangles[triangleIndex + 3] = vertexIndex + gridSize + 1;
                triangles[triangleIndex + 4] = vertexIndex + 1;
                triangles[triangleIndex + 5] = vertexIndex + gridSize + 2;
                triangleIndex += 6;
            }
        }
        mesh.triangles = triangles;

        // Generate UVs
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x / gridSize, vertices[i].z / gridSize);
        }
        mesh.uv = uvs;

        return mesh;
    }
}