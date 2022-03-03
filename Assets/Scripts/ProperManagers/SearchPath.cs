using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchPath : MonoBehaviour
{
    public SearchNode initialNode;
    public SearchNode Node { get; set; }
    public Enemy enemy { get; private set; }

    void Start()
    {
        var go = GameObject.FindGameObjectWithTag("firstCheckpoint");
        Debug.Log(go.name);
        Node = (initialNode == null) ? go.GetComponent<SearchNode>() : initialNode;
        enemy = GetComponent<Enemy>();
    }

    public void Move(Movement movement)
    {
        if (enemy != null && enemy.IsDead) { GetComponent<KinematicMovement>().Velocity = Vector3.zero; return; }
        if (Node != null)
        {
            movement.MoveTowards(Node.transform.position);
        }
    }
}
