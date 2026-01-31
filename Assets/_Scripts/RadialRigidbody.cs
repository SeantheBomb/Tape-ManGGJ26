using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RadialRigidbody : MonoBehaviour
{

    public Vector2 gravity;
    public Vector2 velocity;
    public Vector2 position;
    public float radius = 0.5f;
    public float height = 1f;
    public bool isGrounded;
    public LayerMask collisionLayer;
    public LayerMask groundLayer;

    Rigidbody body;


    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {

        body.AddForce(gravity);
        position = RadialRigidibodyManager.instance.GetInverseRadialPosition(body.position);
        body.MovePosition(RadialRigidibodyManager.instance.GetRadialPosition(position));
        velocity = RadialRigidibodyManager.instance.GetInverseRadialVector(body.position, body.velocity);
        body.velocity = RadialRigidibodyManager.instance.GetRadialVector(position, velocity);
        
        isGrounded = Spherecast(new Ray(body.position + (Vector3.up * height / 2), Vector3.down), radius, height, groundLayer);
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
        Vector3 realForce = RadialRigidibodyManager.instance.GetRadialVector(position, force);
        body.AddForce(realForce, ForceMode.Force);
        Debug.DrawRay(transform.position, realForce, Color.yellow);
    }

    public void AddImpulse(Vector2 force)
    {
        Vector3 realForce = RadialRigidibodyManager.instance.GetRadialVector(position, force);
        body.AddForce(realForce, ForceMode.Impulse);
        Debug.DrawRay(transform.position, realForce, Color.yellow / 2f);
    }

    public void ConstrainRope(Vector2 anchorRadialPos, float ropeLength)
    {
        // Read current state from the real rigidbody (not cached fields)
        var mgr = RadialRigidibodyManager.instance;

        Vector2 p = mgr.GetInverseRadialPosition(body.position);
        Vector2 v = mgr.GetInverseRadialVector(body.position, body.velocity);

        Vector2 delta = anchorRadialPos - p;
        float dist = delta.magnitude;

        // Slack: rope does nothing (doesn't push)
        if (dist <= ropeLength || dist < 1e-6f)
            return;

        Vector2 dir = delta / dist; // from body -> anchor

        // --- 1) Position projection: snap back to exactly ropeLength ---
        float error = dist - ropeLength;
        p += dir * error;

        // --- 2) Velocity projection: remove only "moving away" component ---
        // (dir points toward the anchor, so vAlong < 0 means moving away)
        float vAlong = Vector2.Dot(v, dir);
        if (vAlong < 0f)
            v -= dir * vAlong;

        // Write back to rigidbody using the same conversions your system expects
        body.MovePosition(mgr.GetRadialPosition(p));
        body.velocity = mgr.GetRadialVector(p, v);

        // Keep cached fields consistent (optional but nice)
        position = p;
        velocity = v;
    }

}
