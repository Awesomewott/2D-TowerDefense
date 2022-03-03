using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchPath : MonoBehaviour
{
    public SearchNode initialNode;
    public SearchNode Node { get; set; }

    void Start()
    {
        var go = GameObject.FindGameObjectWithTag("firstCheckpoint");
        Debug.Log(go.name);
        Node = (initialNode == null) ? go.GetComponent<SearchNode>() : initialNode;       
    }

    public void Move(Movement movement)
    {
        if (Node != null)
        {
            movement.MoveTowards(Node.transform.position);
        }
    }
}
