using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    public Node Start;
    public Enemy_Base Agent;

    public List<Vector2> OpenList;
    public List<Vector2> ClosedList;
    public List<Node> PlatformNodes;

    void SetAgent(Enemy_Base e) { Agent = e; }

    public void AStarPathFinding()
    {
        if (PlatformNodes is { Count: 0 })
            PlatformNodes = FindObjectsOfType<Node>().ToList();
        OpenList = new List<Vector2>();
        ClosedList = new List<Vector2>();
        OpenList.Add(Agent.transform.position);
        while(OpenList.Count > 0)
        {

        }
    }
}
