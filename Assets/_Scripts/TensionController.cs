using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TensionTracker : MonoBehaviour
{
    public AudioSource myAudioSource;
    public AudioSource myAudioSource2;
    public AudioSource myAudioSource3;
    public int change = 0;

    public int tense = 0;
    // Start is called before the first frame update
    void Start()
    {
        myAudioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (tense == 1 && change == 0)
        {
            Fade(myAudioSource, myAudioSource2);
        }

        if(tense == 3 && change == 1)
        {
            Fade(myAudioSource2, myAudioSource3);
        }
        if(tense == 4)
        {
            StartCoroutine(End_game());
        }
    }
    void Fade(AudioSource a, AudioSource b)
    {
        if (a.volume <= .1f)
        {
            a.Stop();
            b.Play();
            b.volume = .1f;
            while (b.volume < 1f)
            {
                float newVolume = b.volume + (.1f * Time.deltaTime);

                if (newVolume > 1f)
                {
                    newVolume = 1f;
                }
                b.volume = newVolume;
            }
            change += 1;
        }
        else
        {
            float newVolume = a.volume - (.1f * Time.deltaTime);
            if (newVolume < 0f)
            {
                newVolume = 0f;
            }
            a.volume = newVolume;
         }
    }

   
    public void Up_tension()
    {
        tense += 1;
    }

    IEnumerator End_game()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(2);
    }
}
