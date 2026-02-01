using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour 
{
    public NPC saveme;
    public Transform lockPoint;
    bool done = false;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Key key) && !done)
        {
            if (key.pair == this)
            {
                StartCoroutine(saveme.Happy());
                key.LockTo(this);
                done = true;
            }
            else
                StartCoroutine(saveme.Mad());
        }
    }
}
