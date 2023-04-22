using System.Collections.Generic;

public class GraphData
{
    public List<Node> nodes;
    public List<Edge> edges;

    public GraphData()
    {

    }

    public GraphData(List<Node> nodes, List<Edge> edges)
    {
        this.nodes = nodes;
        this.edges = edges;
    }
}