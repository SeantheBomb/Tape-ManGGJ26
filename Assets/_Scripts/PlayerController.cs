using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(RadialRigidbody))]
public class PlayerController : MonoBehaviour
{

    public Vector2 movement;

    public float moveForce = 1f;
    public float maxSpeed = 10f;
    public float jumpHeight = 1f;
    public float maxJumpDuration = 1f;
    public bool isJumping = false;
    bool isJumpFirstFrame = false;

    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode jump = KeyCode.Space;

    GrappleHookSource[] grapples;

    RadialRigidbody radialBody;
    float jumpTimer;

    bool isGrappled => grapples.Any((g) => g.isGrappled);

    float jumpImpulse => Mathf.Sqrt(2f * radialBody.gravity.magnitude * jumpHeight);

    // Start is called before the first frame update
    void Start()
    {
        radialBody = GetComponent<RadialRigidbody>();
        grapples = GetComponentsInChildren<GrappleHookSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        movement.x += Input.GetKey(moveRight) ? 1 : 0;
        movement.x -= Input.GetKey(moveLeft) ? 1 : 0;

        if (Input.GetKey(jump) && isJumping == false)
            isJumpFirstFrame = true;
        isJumping = Input.GetKey(jump);
        if (jumpTimer > maxJumpDuration)
            isJumping = false;
        if(isJumping && isJumpFirstFrame)
        {
            movement.y = jumpImpulse;
            BreakFixedGrapples();
            //Debug.Log($"Player: Jump impulse {jumpImpulse} set movement {movement.y}");
        }
        if (isJumping)
        {
            jumpTimer += Time.deltaTime;
        }
        if(radialBody.isGrounded == false && isJumping == false && Input.GetKey(jump) == false && isGrappled == false)
            movement.y = -jumpImpulse / 2f;

        if ((radialBody.isGrounded || isGrappled) && Input.GetKey(jump) == false)
            jumpTimer = 0;
        isJumpFirstFrame = false;


        if (movement.x != 0)
        {
            float targetSpeed = Mathf.Sign(movement.x) * maxSpeed;
            float acceleration = Mathf.Clamp(targetSpeed - radialBody.velocity.x, -moveForce, moveForce);
            movement.x = acceleration;
        }
    }

    private void FixedUpdate()
    {
        radialBody.AddImpulse(movement);
        movement = Vector2.zero;
    }

    private void BreakFixedGrapples()
    {
        foreach(var g in grapples)
        {
            if (g.isGrappled && g.target.isFixed)
                g.StopGrapple();
        }
    }
}
