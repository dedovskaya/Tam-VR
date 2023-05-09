using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node  
{
    public string id;
    public string name;
    public int birthYear;
    public Vector3 position; // added field for position
    public Vector3 velocity;
    public List<Edge> edges = new List<Edge>();
    public string family;

    public Node(string id, string name, int birthYear)
    {
        this.id = id;
        this.name = name;
        this.birthYear = birthYear;
        this.position = new Vector3(UnityEngine.Random.Range(-5f, 5f),
                                    0f,
                                    UnityEngine.Random.Range(-5f, 5f)
                                    );
        this.velocity = Vector3.zero;
        this.edges = new List<Edge>();
    }

    public Node(Node n)
    {

    }
}
