using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerSpriteHandler : MonoBehaviour
{
    public PlayerController player;
    public SpriteRenderer spriteRenderer;
    public Sprite grounded;
    public Sprite inAir;
    bool isLeft;
    //float scale;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sprite = grounded;
        //scale = spriteRenderer.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 delta = spriteRenderer.transform.position - Camera.main.transform.position;
        if (Input.GetKey(player.moveRight))
        {
            spriteRenderer.transform.rotation = Quaternion.LookRotation(delta);
            isLeft = false;
        }
        else if (Input.GetKey(player.moveLeft))
        {
            spriteRenderer.transform.rotation = Quaternion.LookRotation(-delta);
            isLeft=true;
        }
        else
        {
            spriteRenderer.transform.rotation = Quaternion.LookRotation(delta * (isLeft ? -1 : 1));
        }

        if (player.radialBody.isGrounded)
        {
            spriteRenderer.sprite = grounded;
        }
        else
        {
            spriteRenderer.sprite = inAir;
        }
    }
}
