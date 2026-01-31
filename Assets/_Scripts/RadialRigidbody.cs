using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RadialRigidbody : MonoBehaviour
{

    public Vector2 gravity;
    public Vector2 acceleration;
    public Vector2 velocity;
    public Vector2 position;
    public float drag;
    public float radius = 0.5f;
    public float height = 1f;
    public bool isGrounded;
    public LayerMask collisionLayer;
    public LayerMask groundLayer;
    Vector2 lastPosition;
    Vector2 lastVelocity;
    float deltaPosX;
    float deltaPosY;
    float deltaVelX;
    float deltaVelY;

    Rigidbody body;


    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        lastPosition = position;
        lastVelocity = velocity;
        acceleration += gravity;
        velocity += acceleration;
        velocity = ConstrainVelocityCollision(position, velocity);
        position += velocity * Time.fixedDeltaTime;
        body.MovePosition(RadialRigidibodyManager.instance.GetRadialPosition(position));
        isGrounded = Spherecast(new Ray(body.position, Vector3.down), radius, 0.1f, groundLayer);
        position = RadialRigidibodyManager.instance.GetInverseRadialPosition(body.position);
        deltaPosX = Mathf.Abs(lastPosition.x - position.x);
        deltaPosY = Mathf.Abs(position.y - lastPosition.y);
        velocity.x = Mathf.Clamp(velocity.x, -deltaPosX, deltaPosX);
        velocity.y = Mathf.Clamp(velocity.y, -deltaPosY, deltaPosY);
        deltaVelX = Mathf.Abs(velocity.x - lastVelocity.x);
        deltaVelY = Mathf.Abs(velocity.y - lastVelocity.y);
        acceleration = Vector2.zero;
    }

    public Vector2 ConstrainVelocityCollision(Vector2 position, Vector2 velocity)
    {
        Vector3 realPos = RadialRigidibodyManager.instance.GetRadialPosition(position);
        Vector3 realDest = RadialRigidibodyManager.instance.GetRadialPosition(position + (velocity * Time.fixedDeltaTime));
        Vector3 realVel = realDest - realPos;
        Vector2 result = velocity;
        Vector3 xVel = Vector3.ProjectOnPlane(realVel, Vector3.up);
        Ray xRay = new Ray(realPos + (Vector3.up * height / 2), xVel);
        if(Spherecast(xRay,height, out RaycastHit xHit,radius, collisionLayer))
        {
            Vector2 point = RadialRigidibodyManager.instance.GetInverseRadialPosition(xHit.point);
            Vector2 newVel = point - position;
            if (Vector2.Dot(xVel, newVel) > 0)
            {
                result.x = 0;
                Debug.DrawRay(xRay.origin, xRay.direction * radius, Color.red, 3f);
            }
        }
        else
            Debug.DrawRay(xRay.origin, xRay.direction * radius);
        Vector3 yVel = Vector3.Project(realVel, Vector3.up);
        Ray yRay = new Ray(realPos + (Vector3.up * height/2), yVel);
        float yDist = yRay.direction.y > 0 ? height : height / 2;
        if (Spherecast(yRay, radius, out RaycastHit yHit, yDist, collisionLayer))
        {
            Vector2 point = RadialRigidibodyManager.instance.GetInverseRadialPosition(yHit.point);
            Vector2 newVel = point - position;
            if(Vector2.Dot(yVel, newVel) > 0)
            {
                result.y = 0;
                Debug.DrawRay(yRay.origin, yRay.direction * yDist, Color.red, 3f);
            }
        }
        else
            Debug.DrawRay(yRay.origin, yRay.direction * yDist, Color.green);
         return result;
    }

    bool Spherecast(Ray ray, float radius, out RaycastHit result, float range, LayerMask layers)
    {
        ray.origin -= ray.direction * radius;
        return Physics.SphereCast(ray, radius, out result, range, layers);
    }

    bool Spherecast(Ray ray, float radius, float range, LayerMask layers)
    {
        ray.origin -= ray.direction * radius;
        return Physics.SphereCast(ray, radius, range, layers);
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
