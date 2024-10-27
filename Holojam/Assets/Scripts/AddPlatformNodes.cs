using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPlatformNodes : MonoBehaviour
{
    [SerializeField] GameObject Node;

    private void Start()
    {
        List<GameObject> nodes = new List<GameObject>();
        nodes.Add(transform.GetChild(0).gameObject);
        //print(nodes[0].name);
        nodes.Add(transform.GetChild(1).gameObject);
        float length = GetComponent<SpriteRenderer>().bounds.size.x;
        if(length > 10)
        {
            GameObject n = Instantiate(Node, new Vector2(), new Quaternion(), transform);
            n.transform.localPosition = new Vector2(0,1);
            n.name = "Middle Node";
            nodes.Add(n);
        }
        if(length > 15)
        {
            GameObject n = Instantiate(Node, new Vector2(), new Quaternion(), transform);
            n.transform.localPosition = new Vector2((nodes[0].transform.localPosition.x - nodes[2].transform.localPosition.x) / 2, 1);
            n.name = "Left Middle Node";

            GameObject m = Instantiate(Node, new Vector2(), new Quaternion(), transform);
            m.transform.localPosition = new Vector2((nodes[1].transform.localPosition.x - nodes[2].transform.localPosition.x) / 2, 1);
            m.name = "Right Middle Node";
        }
    }
}
