using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    //based off of A*
    public Node Start;
    public Enemy_Base Agent;

    public Queue<Node> OpenList;
    public List<Node> ClosedList;
    public List<Node> PlatformNodes;

    void SetAgent(Enemy_Base e) { Agent = e; }

    public void AStarPathFinding()
    {
        Start.g = Vector2.Distance(Agent.player.transform.position, Agent.transform.position);

        if (PlatformNodes is { Count: 0 })
            PlatformNodes = FindObjectsOfType<Node>().ToList();
        OpenList = new Queue<Node>();
        ClosedList = new List<Node>();
        OpenList.Enqueue(Start);
        while(OpenList.Count > 0)
        {
            Node current = OpenList.Peek();
            
            foreach(Node n in PlatformNodes)
            {
                //make end condition to exit pathfinding
                //idea, use two raycasts, one the hits everything, walls, platforms ..etc then one that ignores platforms
                //The one the ignores platforms means that the enemy can still see the player the other one will check for if it's true LOS
                n.g = current.g + Vector2.Distance(n.transform.position, current.transform.position);
                n.h = Vector2.Distance(n.transform.position, Agent.player.transform.position);
                n.f = n.g + n.f;

                if (OpenList.Contains(n))
                    continue;
                if (ClosedList.Contains(n))
                    continue;
                OpenList.Enqueue(n);

            }
            ClosedList.Add(OpenList.Peek());
            OpenList.Dequeue();
        }
    }
}
