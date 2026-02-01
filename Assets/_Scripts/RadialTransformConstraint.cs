using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RadialTransformConstraint : MonoBehaviour
{
    public Vector2 position;

    private void Update()
    {
        if (Application.isPlaying)
            return;
        if (transform.hasChanged)
            OnValidate();
        transform.hasChanged = false;
    }

    private void OnValidate()
    {
        if (RadialRigidibodyManager.instance == null) return;
        position = RadialRigidibodyManager.instance.GetInverseRadialPosition(transform.position);
        transform.position = RadialRigidibodyManager.instance.GetRadialPosition(position);
        Vector3 delta = Vector3.ProjectOnPlane(transform.position - RadialRigidibodyManager.instance.transform.position, Vector3.up);
        transform.rotation = Quaternion.LookRotation(delta, Vector3.up);
    }
}
