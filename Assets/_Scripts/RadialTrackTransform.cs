using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialTrackTransform : MonoBehaviour
{

    public RadialRigidbody trackedObject;
    public float distance = 50;
    public float heightOffset = 10;
    public float xMoveSpeed = 9f;
    public float yMoveSpeed = 1f;
    public float yRotSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void LateUpdate()
    {
        Vector3 destination = RadialRigidibodyManager.instance.GetRadialPosition(trackedObject.position, distance)
                            + Vector3.up * heightOffset;

        transform.position = Vector3.Lerp(transform.position, destination, 1f - Mathf.Exp(-xMoveSpeed * Time.deltaTime));

        Quaternion lookRot = Quaternion.LookRotation(trackedObject.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, 1f - Mathf.Exp(-yRotSpeed * Time.deltaTime));
    }

    // Update is called once per frame
    //void Update()
    //{
    //    Vector3 destination = transform.position;
    //    destination = RadialRigidibodyManager.instance.GetRadialPosition(trackedObject.position, distance);
    //    destination += Vector3.up * heightOffset;
    //    transform.position = destination;
    //    transform.LookAt(trackedObject.transform.position);
    //    //Vector3 newPos = transform.position;
    //    //newPos.x = Mathf.MoveTowards(newPos.x, destination.x, xMoveSpeed * Time.deltaTime);
    //    //newPos.z = Mathf.MoveTowards(newPos.z, destination.z, xMoveSpeed * Time.deltaTime);
    //    //newPos.y = Mathf.MoveTowards(newPos.y, destination.y, yMoveSpeed * Time.deltaTime);
    //    //transform.position = newPos;
    //    //Quaternion lookRot = Quaternion.LookRotation(trackedObject.transform.position - transform.position);
    //    //transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot, yRotSpeed * Time.deltaTime);
    //}
}
