using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour 
{
    public NPC saveme;
    public Transform lockPoint;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Key key))
        {
            if (key.pair == this)
            {
                StartCoroutine(saveme.Happy());
                key.LockTo(this);
            }
            else
                StartCoroutine(saveme.Mad());
        }
    }
}
