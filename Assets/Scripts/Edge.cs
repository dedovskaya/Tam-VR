using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public string id;
    public Node source;
    public Node target;
    public bool directed;

    public Edge(Node source, Node target, bool directed)
    {
        this.source = source;
        this.target = target;
        this.directed = directed;
        this.id = $"{source.id}-{target.id}";
    }
}
