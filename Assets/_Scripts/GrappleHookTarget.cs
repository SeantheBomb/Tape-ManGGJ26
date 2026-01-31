using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHookTarget : MonoBehaviour
{
    public bool isFixed => radialBody == null;
    public RadialRigidbody radialBody;

}
