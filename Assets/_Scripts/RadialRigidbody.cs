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
    public float radius = 0.5f;
    public float height = 1f;
    Vector2 lastPosition;
    Vector2 lastVelocity;
    float deltaPosX;
    float deltaPosY;
    float deltaVelX;
    float deltaVelY;


    private void FixedUpdate()
    {
        lastPosition = position;
        lastVelocity = velocity;
        acceleration += gravity;
        velocity += acceleration;
        velocity = ConstrainVelocityCollision(position, velocity);
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

    public Vector2 ConstrainVelocityCollision(Vector2 position, Vector2 velocity)
    {
        Vector3 realPos = RadialRigidibodyManager.instance.GetRadialPosition(position);
        Vector3 realDest = RadialRigidibodyManager.instance.GetRadialPosition(position + (velocity * Time.fixedDeltaTime));
        Vector3 realVel = realDest - realPos;
        Vector2 result = velocity;
        Ray xRay = new Ray(realPos, Vector3.ProjectOnPlane(realVel, Vector3.up));
        if(Physics.Raycast(xRay, out RaycastHit xHit, realVel.magnitude + radius))
        {
            Vector2 point = RadialRigidibodyManager.instance.GetInverseRadialPosition(xHit.point);
            Vector2 newVel = point - position;
            result.x = newVel.x;
        }
        Ray yRay = new Ray(realPos, Vector3.Project(realVel, Vector3.up));
        if (Physics.Raycast(yRay, out RaycastHit yHit, realVel.magnitude + (yRay.direction.y > 0 ? height : 0)))
        {
            Vector2 point = RadialRigidibodyManager.instance.GetInverseRadialPosition(xHit.point);
            Vector2 newVel = point - position;
            result.y = newVel.y;
        }
        return result;
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
