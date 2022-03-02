using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNode : SearchNode
{
    public WaypointNode nextWaypoint;

    private void OnTriggerEnter(Collider other)
    {
        Agent agent = other.GetComponent<Agent>();
        if (agent != null)
        {
            SearchPath searchPath = agent.GetComponent<SearchPath>();

            if (searchPath != null)
            {
                if (searchPath.Node == this)
                {
                    searchPath.Node = nextWaypoint;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Agent agent = collision.GetComponent<Agent>();
        if (agent != null)
        {
            SearchPath searchPath = agent.GetComponent<SearchPath>();

            if (searchPath != null)
            {
                if (searchPath.Node == this)
                {
                    searchPath.Node = nextWaypoint;
                    agent.GetComponent<KinematicMovement>().Velocity = Vector3.zero;
                }
            }
        }
    }
}
