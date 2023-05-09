using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GraphRenderer : MonoBehaviour

{
    public GameObject arrowheadPrefab;
    public GameObject nodePrefab;
    public GameObject edgePrefab;
    public DataLoader dataLoader;

    public GraphController graphController;

    private List<TextMesh> yearsTextMesh;
    private List<GameObject> nameText;
    public List<GameObject> nodes;
    public List<GameObject> edges;
    private List<GameObject> arrowheads;
    private List<TextMesh> nameTextMesh;

    void Start()
    {
        yearsTextMesh = new List<TextMesh>();
        nameTextMesh = new List<TextMesh>();
        nameText = new List<GameObject>();
        nodes = new List<GameObject>();
        edges = new List<GameObject>();
        arrowheads = new List<GameObject>();
        RenderGraph(dataLoader.LoadData());
    }

    void RenderGraph(GraphData graphData)
    {
        // Create a parent game object for the graph
        GameObject graphObj = new GameObject("Graph");
        GameObject nodesSceneObj = new GameObject("Nodes");
        nodesSceneObj.transform.SetParent(graphObj.transform, false);

        // Create GameObjects for each node in the graph and set their positions
        foreach (Node nodeData in graphData.nodes)
        {
            GameObject nodeObj = Instantiate(nodePrefab, Vector3.zero, Quaternion.identity, nodesSceneObj.transform);
            nodeObj.name = nodeData.id;
            nodeObj.transform.position = new Vector3(UnityEngine.Random.Range(-5f, 5f),
                                                     0f,
                                                     UnityEngine.Random.Range(-5f, 5f)
                                                     ); // Assign random position
            nodes.Add(nodeObj);

            //////////////////////////////////////////////////////////////////////////////////////////////////// 
            // Add text
            GameObject textNameObj = new GameObject("NamePerson");
            textNameObj.transform.SetParent(nodeObj.transform, false);
            TextMesh textNameMesh = textNameObj.AddComponent<TextMesh>();
            textNameMesh.text = nodeData.name;
            textNameMesh.fontSize = 14;
            textNameMesh.color = Color.black;
            textNameMesh.anchor = TextAnchor.MiddleCenter;
            Vector3 textOffset = new Vector3(0f, textNameMesh.GetComponent<Renderer>().bounds.size.y / 2f + 0.1f, 0f);
            textNameMesh.transform.position = nodeObj.transform.position + textOffset;
            nameText.Add(textNameObj);
            nameTextMesh.Add(textNameMesh);

            //////////////////////////////////////////////////////////////////////////////////////////////////// 
            // Add year (height)
            GameObject textYearObj = new GameObject("BirthYear");
            textYearObj.transform.SetParent(nodeObj.transform, false);
            TextMesh textYearMesh = textYearObj.AddComponent<TextMesh>();
            textYearMesh.text = nodeData.birthYear.ToString();
            yearsTextMesh.Add(textYearMesh);
        }

        GameObject edgesSceneObj = new GameObject("Edges");
        edgesSceneObj.transform.SetParent(graphObj.transform, false);
        // Create GameObjects for each edge in the graph and set their positions
        foreach (Edge edgeData in graphData.edges)
        {
            GameObject edgeObj = Instantiate(edgePrefab, edgesSceneObj.transform);
            edgeObj.name = edgeData.source.id + " to " + edgeData.target.id;
            // Get the LineRenderer component of the edge
            LineRenderer lineRenderer = edgeObj.GetComponent<LineRenderer>();

            // Set the positions of the LineRenderer to the center positions of the source and target nodes
            lineRenderer.SetPosition(0, nodes.Find(n => n.name == edgeData.source.id).transform.position);
            lineRenderer.SetPosition(1, nodes.Find(n => n.name == edgeData.target.id).transform.position);

            edges.Add(edgeObj);

            //////////////////////////////////////////////////////////////////////////////////////////////////// 
            // Add an arrowhead to the edge
            GameObject arrowheadObj = Instantiate(arrowheadPrefab);
            arrowheadObj.name = "Arrowhead";
            arrowheadObj.transform.SetParent(edgeObj.transform);
            arrowheadObj.transform.localPosition = Vector3.zero;
            arrowheadObj.transform.LookAt(lineRenderer.GetPosition(1));
            arrowheadObj.transform.Rotate(0, 180, 0);
        }
    }
    void Update()
    {
        // Apply spring forces to nodes
        foreach (GameObject nodeObj in nodes)
        {
            Node node = dataLoader.graphData.nodes.Find(n => n.id == nodeObj.name);
            Vector3 force = Vector3.zero;

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            // Find the TextMesh component in the node object
            TextMesh textMesh = nodeObj.GetComponentInChildren<TextMesh>();
            // Set the TextMesh rotation to face the camera
            if (textMesh != null)
            {
                //textMesh.transform.position = node.position;
                textMesh.transform.rotation = Quaternion.LookRotation(textMesh.transform.position - Camera.main.transform.position);
            }

            TextMesh secondTextMesh = nodeObj.transform.GetChild(1).GetComponent<TextMesh>();

            if (secondTextMesh != null)
            {
                //textMesh.transform.position = node.position;
                secondTextMesh.transform.rotation = Quaternion.LookRotation(textMesh.transform.position - Camera.main.transform.position);
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            // Calculate spring forces between connected nodes
            foreach (Edge edge in node.edges)
            {
                Node otherNode = (edge.source == node) ? edge.target : edge.source;
                Vector3 direction = otherNode.position - node.position;
                float distance = direction.magnitude;
                float displacement = distance - graphController.springLength;
                float springForce = displacement * graphController.springStiffness;
                force += direction.normalized * springForce;

                // Update edge position
                GameObject edgeObj = edges.Find(e => e.name == edge.source.id + " to " + edge.target.id);
                LineRenderer lineRenderer = edgeObj.GetComponent<LineRenderer>();
                lineRenderer.SetPosition(0, nodeObj.transform.position);
                lineRenderer.SetPosition(1, nodes.Find(n => n.name == otherNode.id).transform.position);

                // Update arrowhead rotation
                GameObject arrowheadObj = edgeObj.transform.Find("Arrowhead").gameObject;
                Vector3 edgeDirection = (otherNode.position - node.position).normalized;
                float angle = Vector3.Angle(Vector3.right, edgeDirection);
                Vector3 rotationAxis = Vector3.forward;
                if (edgeDirection.y < 0) // Edge is pointing down
                {
                    angle = -angle;
                }
                Quaternion rotation = Quaternion.AngleAxis(angle, rotationAxis);
                arrowheadObj.transform.rotation = rotation;

                // Update arrowhead position
                Transform arrowhead = edgeObj.transform.Find("Arrowhead");
                arrowhead.position = nodes.Find(n => n.name == edge.target.id).transform.position;
                arrowhead.LookAt(nodeObj.transform.position);
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            // Calculate repulsion forces between all nodes
            foreach (Node otherNode in dataLoader.graphData.nodes)
            {
                if (otherNode == node) continue;
                Vector3 direction = otherNode.position - node.position;
                float distance = direction.magnitude;
                float repulsionForce = graphController.nodeRepulsion / (distance * distance);
                force -= direction.normalized * repulsionForce;
            }

            // Apply damping to current velocity
            node.velocity *= graphController.damping;

            // Apply force to current velocity
            node.velocity += force * Time.fixedDeltaTime;

            if (!graphController.freezeGraph)
            {
                nodeObj.transform.position = node.position;
                node.position += node.velocity * Time.fixedDeltaTime;
                textMesh.transform.position = node.position;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////// hideYear
        // Hide name
        if (graphController.hideName)
        {
            foreach (TextMesh textMesh in nameTextMesh)
            {
                // Set opacity to 0
                Color color = textMesh.color;
                color.a = 0f;
                textMesh.color = color;
            }
        }
        else
        {
            foreach (TextMesh textMesh in nameTextMesh)
            {
                // Set opacity back to 1
                Color color = textMesh.color;
                color.a = 1f;
                textMesh.color = color;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////// hideYear
        // Hide year
        if (graphController.hideYear)
        {
            foreach (TextMesh textMesh in yearsTextMesh)
            {
                // Set opacity to 0
                Color color = textMesh.color;
                color.a = 0f;
                textMesh.color = color;
            }
        }
        else
        {
            foreach (TextMesh textMesh in yearsTextMesh)
            {
                // Set opacity back to 1
                Color color = textMesh.color;
                color.a = 1f;
                textMesh.color = color;
            }
        }

        /////////////////////
        // Raise graph
        if (graphController.raiseGraph)
        {
            foreach (GameObject node in nodes)
            {
                TextMesh secondTextMesh = node.transform.GetChild(1).GetComponent<TextMesh>();
                float y = float.Parse(secondTextMesh.text);

                // Get the current position of the GameObject
                Vector3 pos = node.transform.position;
                // Update the y-coordinate of the position
                pos.y = (y - 1553) / 10;
                // Update the position of the GameObject
                node.transform.position = pos;
            }
            graphController.raiseGraph = false;
        }
    }
}


