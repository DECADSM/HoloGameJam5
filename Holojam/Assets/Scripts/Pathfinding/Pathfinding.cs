using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    //based off of A*
    public Node Start;
    public Enemy_Base Agent;

    public List<Node> OpenList;
    public List<Node> ClosedList;
    public List<Node> PlatformNodes;
    List<Node> ClosestNodes;

    void SetAgent(Enemy_Base e) { Agent = e; }

    public void PathFinding()
    {
        Start.g = Vector2.Distance(Agent.player.transform.position, Agent.transform.position);
        ClosestNodes = new List<Node>();

        Vector2 DirectionToPlayer = Agent.player.transform.position - transform.position;

        if (PlatformNodes is { Count: 0 })
            PlatformNodes = FindObjectsOfType<Node>().ToList();
        OpenList = new List<Node>();
        ClosedList = new List<Node>();
        OpenList.Add(Start);
        while(OpenList.Count > 0)
        {
            Node current = OpenList[0];
            bool openCheck = true, closedCheck = true;
            FindClosestNodes(current);

            foreach (Node n in ClosestNodes)
            {
                if (n.transform.parent != null)
                {
                    print(n.transform.parent.name + " " + n.name);
                }
                if(OpenList.Contains(n))
                    continue;
                if (ClosedList.Contains(n))
                    continue;
                Vector2 DirectiontoN = n.transform.position - transform.position;
                //if (Vector2.Dot(DirectiontoN, DirectionToPlayer) > 90)
                  //  continue;
                float dist = Vector2.Distance(n.transform.position, Agent.player.transform.position);
                if (dist <= 2)
                {
                    print(n.transform.parent.name + " " + n.name + " is the closest node");
                    n.parent = current;
                    ClosedList.Add(n);
                    return;
                }
                //make end condition to exit pathfinding
                //idea, use two raycasts, one the hits everything, walls, platforms ..etc then one that ignores platforms
                //The one the ignores platforms means that the enemy can still see the player the other one will check for if it's true LOS
                n.g = current.g + Vector2.Distance(n.transform.position, current.transform.position);
                n.h = Vector2.Distance(n.transform.position, Agent.player.transform.position);
                n.f = n.g + n.f;

                

                foreach (Node c in OpenList)
                {
                    if (c.Equals(n) && c.f < n.f)
                    {
                        openCheck = false;
                        break;
                    }
                }
                
                foreach (Node c in ClosedList)
                {
                    if (c.Equals(n) && c.f < n.f)
                    {
                        closedCheck = false;
                        break;
                    }
                }
                if (closedCheck && openCheck)
                {
                    n.parent = current;
                    OpenList.Add(n);
                }

            }
            ClosedList.Add(OpenList[0]);
            OpenList.RemoveAt(0);
            OpenList.TrimExcess();
        }
    }

    void FindClosestNodes(Node current)
    {
        if (ClosestNodes.Count > 0)
            ClosestNodes.Clear();
        foreach(Node n in PlatformNodes)
        {
            if (n.Equals(current))
                continue;
            float dist = Vector2.Distance(n.transform.position, current.transform.position);
            if ( dist <= Agent.JumpDistance)
            {
                ClosestNodes.Add(n);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (ClosedList.Count > 0)
        {
            for (int i = 0; i < ClosedList.Count - 1; i++)
                Gizmos.DrawLine(ClosedList[i].transform.position, ClosedList[i + 1].transform.position);
        }
    }
}
