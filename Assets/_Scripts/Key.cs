using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    // Start is called before the first frame update
    public Lock pair;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == pair.gameObject & collision.gameObject.CompareTag("Locks"))
        {
            gameObject.transform.position = collision.gameObject.transform.position;
            gameObject.transform.rotation = collision.gameObject.transform.rotation;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            gameObject.isStatic = true;
        }
        return;
    }
}
