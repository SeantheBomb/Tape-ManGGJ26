using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TensionTracker : MonoBehaviour
{
    public AudioSource myAudioSource;
    public AudioSource myAudioSource2;
    public AudioSource myAudioSource3;
    int change = 0;

    public int tense = 0;
    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        myAudioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (tense == 1 && change < 1)
        {
            myAudioSource.Stop();
            myAudioSource2.Play();
            change = change++;
        }

        if(tense == 3 && change < 2)
        {
            myAudioSource2.Stop();
            myAudioSource3.Play();
            change = change++;
        }
    }

    public void Up_tension()
    {
        tense = tense++;
    }

}
