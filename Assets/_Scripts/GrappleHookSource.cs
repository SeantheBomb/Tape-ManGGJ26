using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHookSource : MonoBehaviour
{

    public RadialRigidbody radialBody;
    public GrappleHookTarget target;
    public KeyCode grappleKey;
    public LineRenderer grappleLine;
    public float minLength = 1f;

    Vector3 localHitpoint;
    float length;
    public bool isDownswing, isUpswing;

    public bool isGrappled => target != null;


    private void Update()
    {
        if (Input.GetKeyDown(grappleKey))
        {
            if (target != null)
            {
                StopGrapple();
            }
            else if (MouseRaycastUtil.TryGetMouseColliderHit(out RaycastHit hit) && hit.collider.TryGetComponent(out GrappleHookTarget target))
            {
                this.target = target;
                localHitpoint = target.transform.InverseTransformPoint(hit.point);
                
                length = Vector2.Distance(radialBody.position, RadialRigidibodyManager.instance.GetInverseRadialPosition(hit.point));
                if(length < minLength) 
                    length = minLength;
            }
        }
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            if(grappleLine)grappleLine.enabled = false;
            isDownswing = false;
            isUpswing = false;
            return;
        }

        Vector3 grapplePoint = target.transform.TransformPoint(localHitpoint);
        Vector2 radialGrapplePoint = RadialRigidibodyManager.instance.GetInverseRadialPosition(grapplePoint);
        Vector2 delta = radialGrapplePoint - radialBody.position;
        float stretch = delta.magnitude - length;
        Vector2 direction = delta.normalized;
        float gravity = radialBody.gravity.magnitude;

        if (target.isFixed)
        {
            // How "vertical" the rope is (1 = straight down, 0 = perfectly horizontal)
            float ropeDownDot = Vector3.Dot(direction, Vector3.down);
            float ropeVerticality = Mathf.Abs(ropeDownDot);

            // Vertical speed (positive = moving up, negative = moving down)
            float verticalSpeed = Vector3.Dot(radialBody.velocity, Vector3.up);

            // Only consider swing boost when rope isn't nearly vertical (tweak threshold)
            bool ropeIsSwinging = ropeVerticality < 0.9f;

            isDownswing = ropeIsSwinging && verticalSpeed < -0.1f;
            isUpswing = ropeIsSwinging && verticalSpeed > 0.1f;

            //if (isDownswing)
            //{
            //    // Increase downward acceleration (tweak multiplier)
            //    radialBody.AddForce(Vector3.down * gravity * 0.5f);
            //}
            //else if (isUpswing)
            //{
            //    // Decrease downward acceleration by pushing upward a bit
            //    radialBody.AddForce(Vector3.up * gravity * 0.5f);
            //}
            radialBody.ConstrainRope(radialGrapplePoint, length);
        }
        else
            target.radialBody.ConstrainRope(radialBody.position, length);

        if (stretch < 0)
            length = Mathf.Max(delta.magnitude, minLength);

        Debug.DrawRay(radialGrapplePoint, -direction * length, Color.blue);
        Debug.DrawRay(radialBody.position, direction * stretch, stretch > 0 ? Color.red : Color.green);

        if (grappleLine)
        {
            grappleLine.enabled = true;
            grappleLine.SetPositions(new Vector3[] { transform.position, grapplePoint });
        }
    }

    public void StopGrapple()
    {
        target = null;
    }
}
