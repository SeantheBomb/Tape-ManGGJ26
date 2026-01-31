using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RadialRigidbody))]
public class PlayerController : MonoBehaviour
{

    public Vector2 movement;

    public float moveSpeed = 1f;
    public float jumpStrength = 1f;

    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode jump = KeyCode.Space;
    public KeyCode leftGrapple = KeyCode.Mouse0;
    public KeyCode rightGrapple = KeyCode.Mouse1;

    RadialRigidbody radialBody;

    // Start is called before the first frame update
    void Start()
    {
        radialBody = GetComponent<RadialRigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        movement = Vector2.zero;
        movement.x += Input.GetKey(moveRight) ? moveSpeed : 0;
        movement.x -= Input.GetKey(moveLeft) ? moveSpeed : 0;
        movement.y += Input.GetKeyDown(jump) ? jumpStrength : 0;
    }

    private void FixedUpdate()
    {
        radialBody.AddImpulse(movement);
    }
}
