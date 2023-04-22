using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphController : MonoBehaviour
{
    public bool freezeGraph = false;
    public bool hideName = false;
    public bool hideYear = false;
    public bool startOver = false;
    public float springLength = 5f;
    public float springStiffness = 5f;
    public float nodeRepulsion = 100f;
    public float damping = 0.8f;
    public bool exportData = false;


    // Start is called before the first frame update
    void Update()
    {

        }
}
