using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RadialRigidbody))]
public class PlayerController : MonoBehaviour
{

    public Vector2 movement;

    public float moveForce = 10f;
    public float initialJumpForce = 100f;
    public float sustainJumpForce = 25f;
    public float sustainJumpDuration = 2f;
    public bool isJumping = false;
    public bool isGrounded = false;

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
        movement.x += Input.GetKey(moveRight) ? moveForce : 0;
        movement.x -= Input.GetKey(moveLeft) ? moveForce : 0;
        movement.y += Input.GetKeyDown(jump) ? initialJumpForce : 0;
    }

    private void FixedUpdate()
    {

        radialBody.AddImpulse(movement);
        movement = Vector2.zero;
    }
}
