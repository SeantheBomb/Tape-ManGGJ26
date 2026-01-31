using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialRigidbody : MonoBehaviour
{

    public Vector2 gravity;
    public Vector2 acceleration;
    public Vector2 velocity;
    public Vector2 position;
    public float drag;
    public Vector2 lastPosition;
    public Vector2 lastVelocity;

    private void FixedUpdate()
    {
        lastPosition = position;
        lastVelocity = velocity;
        acceleration += gravity;
        velocity += acceleration * Time.fixedDeltaTime;
        position += velocity * Time.fixedDeltaTime;
        transform.position = RadialRigidibodyManager.instance.GetRadialPosition(position);
        position = RadialRigidibodyManager.instance.GetInverseRadialPosition(transform.position);
    }

    public void AddForce(Vector2 force)
    {
        acceleration += force;
    }

    public void AddImpulse(Vector2 force)
    {
        if(force.x != 0)
        {
            if(Mathf.Sign(force.x) == Mathf.Sign(velocity.x))
            {
                velocity.x += force.x;
            }
            else
            {
                velocity.x = force.x;
            }
        }
        if (force.y != 0)
        {
            if (Mathf.Sign(force.y) == Mathf.Sign(velocity.y))
            {
                velocity.y += force.y;
            }
            else
            {
                velocity.y = force.y;
            }
        }
    }


}
