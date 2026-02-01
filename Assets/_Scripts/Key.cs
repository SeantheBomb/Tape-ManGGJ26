using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    // Start is called before the first frame update
    public Lock pair;

    public void LockTo(Lock lockToMe)
    {
        if(lockToMe == pair)
        { 
            gameObject.transform.position = lockToMe.lockPoint.position;
            gameObject.transform.rotation = lockToMe.gameObject.transform.rotation;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
