using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public override void Enter(Agent owner)
    {
        Debug.Log(GetType().Name + " Enter");
    }

    public override void Execute(Agent owner)
    {
        SearchPath searchPath = owner.GetComponent<SearchPath>();
        searchPath.Move(owner.movement);
    }

    public override void Exit(Agent owner)
    {
        Debug.Log(GetType().Name + " Exit");
    }
}
