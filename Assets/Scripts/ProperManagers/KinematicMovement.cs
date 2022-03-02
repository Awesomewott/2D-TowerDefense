using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicMovement : Movement
{
    private void LateUpdate()
    {
        Velocity += Acceleration * Time.deltaTime;
        Velocity = Vector3.ClampMagnitude(Velocity, speedMax);
        transform.position += Velocity * Time.deltaTime;

        if (orientToMovement && Direction.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(Direction);
        }

        Acceleration = Vector3.zero;
    }

    public override void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        ApplyForce(direction * accelerationMax);
    }

    public override void ApplyForce(Vector3 force)
    {
        Acceleration += force;
        Acceleration = Vector3.ClampMagnitude(Acceleration, accelerationMax);
    }

    public override void Stop()
    {
        Velocity = Vector3.zero;
    }

    public override void Resume()
    {
        //
    }
}
