using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class LandscapeRenderer : MonoBehaviour
{
    public int gridSize = 5000;
    //public float scale = 5f;
    public float heightMultiplier = 10f;
    private List<Vector3> coordinates = new List<Vector3>();
    public List<LineRenderer> lines = new List<LineRenderer>();
    public GraphRenderer graphRenderer;

    /////////////////////////////////////////////
    public int size = 2048;
    public float maxDistance = 48f;
    private Camera cam;

    private void Update()
    {
        if (graphRenderer.graphController.createHeightMap3)
        {
            GetCoordinatesNodes();
            GetCoordinatesEdges();

            // Create a 1024x1024 texture
            Texture2D texture = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);

            // Set the pixel at (512, 512) to white with a value of 0.58
            texture.SetPixel(512, 512, new Color32(148, 148, 148, 255)); // abs(0.58*255)

            // Set the pixel at (600, 700) to white with a value of 0.22
            texture.SetPixel(600, 700, new Color32(56, 56, 56, 255));

            // Draw an interpolated line between the two pixels
            int x0 = 512;
            int y0 = 512;
            int x1 = 600;
            int y1 = 700;
            float height0 = 0.58f;
            float height1 = 0.22f;
            float dx = Mathf.Abs(x1 - x0);
            float dy = Mathf.Abs(y1 - y0);
            float dz = Mathf.Abs(height1 - height0);
            float slope = dz / Mathf.Sqrt(dx * dx + dy * dy);

            if (dx > dy)
            {
                if (x0 > x1)
                {
                    int temp = x0;
                    x0 = x1;
                    x1 = temp;
                    temp = y0;
                    y0 = y1;
                    y1 = temp;
                    float tempHeight = height0;
                    height0 = height1;
                    height1 = tempHeight;
                }
                float y = y0;
                float z = height0;
                float ystep = dy / dx;
                for (int x = x0; x <= x1; x++)
                {
                    texture.SetPixel(x, Mathf.RoundToInt(y), new Color32((byte)(z * 255), (byte)(z * 255), (byte)(z * 255), 255));
                    y += ystep;
                    z += slope;
                }
            }
            else
            {
                if (y0 > y1)
                {
                    int temp = x0;
                    x0 = x1;
                    x1 = temp;
                    temp = y0;
                    y0 = y1;
                    y1 = temp;
                    float tempHeight = height0;
                    height0 = height1;
                    height1 = tempHeight;
                }
                float x = x0;
                float z = height0;
                float xstep = dx / dy;
                for (int y = y0; y <= y1; y++)
                {
                    texture.SetPixel(Mathf.RoundToInt(x), y, new Color32((byte)(z * 255), (byte)(z * 255), (byte)(z * 255), 255));
                    x += xstep;
                    z += slope;
                }
            }

            // Apply the changes to the texture and save it to disk
            texture.Apply();
            // Save the texture to disk
            byte[] bytes = texture.EncodeToPNG();
            string filePath = "Assets/heightmap2.png";
            File.WriteAllBytes(filePath, bytes);

            graphRenderer.graphController.createHeightMap3 = false;
        }
        if (graphRenderer.graphController.createHeightMap2)
        {
            //int width = 1024;
            //int height = 1024;

            //// Create new texture
            //Texture2D texture = new Texture2D(width, height);

            //// Set all pixels to black
            //for (int x = 0; x < 1024; x++)
            //{
            //    for (int y = 0; y < 1024; y++)
            //    {
            //        texture.SetPixel(x, y, Color.black);
            //    }
            //}


            //// Set center pixel to white
            //int centerX = width / 2;
            //int centerY = height / 2;
            //texture.SetPixel(centerX, centerY, Color.white);

            //// Apply changes to texture
            //texture.Apply();
            // ///////////////////////////////////////////////////////////////////

            // Create a 1024x1024 texture
            Texture2D texture = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);

            // Set the pixel at (512, 512) to white with a value of 0.58
            texture.SetPixel(512, 512, new Color32(148, 148, 148, 255)); // abs(0.58*255)

            // Set the pixel at (600, 700) to white with a value of 0.22
            texture.SetPixel(600, 700, new Color32(56, 56, 56, 255));

            // Draw an interpolated line between the two pixels
            int x0 = 512;
            int y0 = 512;
            int x1 = 600;
            int y1 = 700;
            float height0 = 0.58f;
            float height1 = 0.22f;
            float dx = Mathf.Abs(x1 - x0);
            float dy = Mathf.Abs(y1 - y0);
            float dz = Mathf.Abs(height1 - height0);
            float slope = dz / Mathf.Sqrt(dx * dx + dy * dy);

            if (dx > dy)
            {
                if (x0 > x1)
                {
                    int temp = x0;
                    x0 = x1;
                    x1 = temp;
                    temp = y0;
                    y0 = y1;
                    y1 = temp;
                    float tempHeight = height0;
                    height0 = height1;
                    height1 = tempHeight;
                }
                float y = y0;
                float z = height0;
                float ystep = dy / dx;
                for (int x = x0; x <= x1; x++)
                {
                    texture.SetPixel(x, Mathf.RoundToInt(y), new Color32((byte)(z * 255), (byte)(z * 255), (byte)(z * 255), 255));
                    y += ystep;
                    z += slope;
                }
            }
            else
            {
                if (y0 > y1)
                {
                    int temp = x0;
                    x0 = x1;
                    x1 = temp;
                    temp = y0;
                    y0 = y1;
                    y1 = temp;
                    float tempHeight = height0;
                    height0 = height1;
                    height1 = tempHeight;
                }
                float x = x0;
                float z = height0;
                float xstep = dx / dy;
                for (int y = y0; y <= y1; y++)
                {
                    texture.SetPixel(Mathf.RoundToInt(x), y, new Color32((byte)(z * 255), (byte)(z * 255), (byte)(z * 255), 255));
                    x += xstep;
                    z += slope;
                }
            }

            // Apply the changes to the texture and save it to disk
            texture.Apply();
            // Save the texture to disk
            byte[] bytes = texture.EncodeToPNG();
            string filePath = "Assets/heightmap2.png";
            File.WriteAllBytes(filePath, bytes);

            graphRenderer.graphController.createHeightMap2 = false;
        }
        if (graphRenderer.graphController.createMiniMap)
        {
            //// Add Mesh Renderer component to this object
            //MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();

            //// Set material to use for rendering
            //renderer.material = new Material(Shader.Find("Standard"));

            // Create camera for rendering
            cam = new GameObject("Camera").AddComponent<Camera>();
            cam.cullingMask = 1 << LayerMask.NameToLayer("MyLayer"); // Only render objects on the "MyLayer" layer
            cam.transform.position = new Vector3(0, maxDistance * 2f, 0);
            cam.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            cam.orthographic = true;
            cam.orthographicSize = maxDistance;
            cam.nearClipPlane = 0.1f;
            cam.farClipPlane = 10000f;

            // Create render texture for capturing camera output
            RenderTexture renderTexture = new RenderTexture(size, size, 24);
            cam.targetTexture = renderTexture;

            // Create heightmap texture
            Texture2D heightmap = new Texture2D(size, size, TextureFormat.RGBA32, false);

            // Render scene and read pixel values
            cam.Render();
            RenderTexture.active = renderTexture;
            heightmap.ReadPixels(new Rect(0, 0, size, size), 0, 0);
            heightmap.Apply();

            // Cleanup camera and render texture
            cam.targetTexture = null;
            RenderTexture.active = null;
            Destroy(cam.gameObject);
            Destroy(renderTexture);

            // Assign heightmap to material
            GetComponent<MeshRenderer>().material.mainTexture = heightmap;
            Debug.Log(heightmap);
            

            // Save the texture to disk
            byte[] bytes = heightmap.EncodeToPNG();
            string filePath = "Assets/heightmap.png";
            File.WriteAllBytes(filePath, bytes);

            graphRenderer.graphController.createMiniMap = false;
        }
        if (graphRenderer.graphController.exportData)
        {
            GetCoordinatesNodes();
            GetCoordinatesEdges();

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

    private void GetCoordinatesNodes() {
        foreach (GameObject node in graphRenderer.nodes)
        {
            // TextMesh textMesh = node.GetComponentInChildren<TextMesh>();
            TextMesh secondTextMesh = node.transform.GetChild(1).GetComponent<TextMesh>();
            float y = float.Parse(secondTextMesh.text);
            Vector3 vec = new Vector3(node.transform.position.x, y, node.transform.position.z);
            coordinates.Add(vec);
        }
    }
    private void GetCoordinatesEdges()
    {
        foreach (GameObject edge in graphRenderer.edges)
        {
            LineRenderer line = edge.transform.GetComponent<LineRenderer>();
            lines.Add(line);
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