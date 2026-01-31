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
    public float deltaPosX;
    public float deltaPosY;
    public float deltaVelX;
    public float deltaVelY;


    private void FixedUpdate()
    {
        lastPosition = position;
        lastVelocity = velocity;
        acceleration += gravity;
        velocity += acceleration;
        position += velocity * Time.fixedDeltaTime;
        transform.position = RadialRigidibodyManager.instance.GetRadialPosition(position);
        position = RadialRigidibodyManager.instance.GetInverseRadialPosition(transform.position);
        deltaPosX = Mathf.Abs(lastPosition.x - position.x);
        deltaPosY = Mathf.Abs(position.y - lastPosition.y);
        velocity.x = Mathf.Clamp(velocity.x, -deltaPosX, deltaPosX);
        velocity.y = Mathf.Clamp(velocity.y, -deltaPosY, deltaPosY);
        deltaVelX = Mathf.Abs(velocity.x - lastVelocity.x);
        deltaVelY = Mathf.Abs(velocity.y - lastVelocity.y);
        acceleration.x = Mathf.Clamp(acceleration.x, -deltaVelX, deltaVelX);
        acceleration.y = Mathf.Clamp(acceleration.y, -deltaVelY, deltaVelY);
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
