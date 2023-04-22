using System.Collections.Generic;
using System;
using UnityEngine;
using Newtonsoft.Json;

public class DataLoader : MonoBehaviour
{
    public TextAsset jsonFile;
    public GraphData graphData;

    public GraphData LoadData()
    {
        // Read the JSON file as a string
        string jsonString = jsonFile.text;

        // Deserialize the JSON string into a dictionary of nodes and edges
        Dictionary<string, List<Dictionary<string, string>>> jsonDict = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary< string,string>>>>(jsonString);

        // Create a list to hold the Node and Edge objects
        List<Node> nodes = new List<Node>();
        List<Edge> edges = new List<Edge>();


        foreach (Dictionary<string, string>  jsonNode in jsonDict["nodes"]) 
        {
            string id = jsonNode["id"];
            string name = jsonNode["name"];
            int value = 0;
            try
            {
                value = Int32.Parse(jsonNode["value"]);
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to create node because value is ill defined '{jsonNode["value"]}'");
            }
            Node node = new Node(id,name,value);
            nodes.Add(node);
        };

        foreach (Dictionary<string, string> jsonEdge in jsonDict["links"])
        {
            string sourceId = jsonEdge["source"];
            string targetId = jsonEdge["target"];
            bool directed = Convert.ToBoolean(jsonEdge["directed"]); //nodes.Items[0].id

            Node sourceNode = nodes.Find(n => n.id == sourceId);
            Node targetNode = nodes.Find(n => n.id == targetId);

            // Create a new Edge object and add it to the edges list for both the source and target nodes
            Edge edge = new Edge(sourceNode, targetNode, directed);
            sourceNode.edges.Add(edge);
            targetNode.edges.Add(edge);
            edges.Add(edge);
        };

        // Construct a GraphData object from the parsed Node and Edge objects
        GraphData graphData = new GraphData(nodes, edges);

        return graphData;
       
    }


    void Start()
    {
        Debug.Log("DataLoader.Start() called");
        //GraphData graphData = LoadData();
        graphData = LoadData();
        Debug.Log($"graphData is created: {graphData}. Num nodes: {graphData.nodes.Count}. Num edges: {graphData.edges.Count}");
    }
}