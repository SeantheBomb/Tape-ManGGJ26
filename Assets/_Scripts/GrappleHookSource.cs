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


    private void Update()
    {
        if (Input.GetKeyDown(grappleKey))
        {
            if (target != null)
            {
                target = null;
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
            return;
        }

        Vector3 grapplePoint = target.transform.TransformPoint(localHitpoint);
        Vector2 radialGrapplePoint = RadialRigidibodyManager.instance.GetInverseRadialPosition(grapplePoint);
        Vector2 delta = radialGrapplePoint - radialBody.position;
        float stretch = delta.magnitude - length;
        Vector2 direction = delta.normalized;

        if(target.isFixed)
            radialBody.ConstrainRope(radialGrapplePoint, length);
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
}
