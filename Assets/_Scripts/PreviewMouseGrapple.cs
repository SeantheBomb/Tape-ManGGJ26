using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PreviewMouseGrapple : MonoBehaviour
{

    public PlayerController player;
    public GrappleHookSource[] grapples => player.grapples;
    public LineRenderer lineRenderer;
    public SpriteRenderer spriteRenderer;

    public bool allGrappled => grapples.All((g) => g.isGrappled);


    // Update is called once per frame
    void Update()
    {
        if (allGrappled)
        {
            lineRenderer.enabled = false;
            spriteRenderer.enabled = false;
            return;
        }
        Vector2 radialPlayerPos = player.radialBody.position;
        Vector3 playerPos = player.transform.position;
        if(RadialRigidibodyManager.instance.TryGetRadialPositionFromScreen(Input.mousePosition, out Vector2 radialMousePos))
        {
            Vector3 realMousePos = RadialRigidibodyManager.instance.GetRadialPosition(radialMousePos);
            Vector3 delta = realMousePos - playerPos;

            if(delta.magnitude > player.maxGrappleLength)
            {
                Vector3 newEndPos = playerPos + (delta.normalized * player.maxGrappleLength);
                Vector2 newRadialEndPos = RadialRigidibodyManager.instance.GetInverseRadialPosition(newEndPos);
                realMousePos = RadialRigidibodyManager.instance.GetRadialPosition(newRadialEndPos);
                SetCanGrapple(false);
            }
            else
            {
                SetCanGrapple(true);
            }

            lineRenderer.SetPositions(new Vector3[] { playerPos, realMousePos });
            spriteRenderer.transform.position = realMousePos;
            spriteRenderer.transform.LookAt(Camera.main.transform);
            lineRenderer.enabled = true;
            spriteRenderer.enabled = true;
        }
    }

    void SetCanGrapple(bool value)
    {
        foreach (var g in grapples)
        {
            g.canGrapple = value;
        }
    }
}
