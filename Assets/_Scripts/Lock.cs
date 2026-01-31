using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour 
{
    public NPC saveme;
    public Key pair;
   // Start is called before the first frame update
   void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject  == pair.gameObject)
        {
            StartCoroutine(saveme.Happy());
        }
        else
        {
            if (collision.gameObject.tag == "Keys")
            {
                StartCoroutine(saveme.Mad());
            }
        }
    }
}
