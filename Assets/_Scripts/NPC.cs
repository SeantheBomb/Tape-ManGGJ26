using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
    public TMP_Text happyText;
    public TMP_Text madText;
    public GameObject textbox;
    public AudioSource happySound;
    public AudioSource madSound;
    public AudioSource chatter;
    public TensionTracker tension;
    public Sprite happylook;
    public Sprite madlook;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = madlook; 
        happyText.gameObject.SetActive(false);
        madText.gameObject.SetActive(false);
        textbox.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Happy()
    {
        if (this.gameObject.activeSelf)
        {
            yield return new WaitForSeconds(2);
            GetComponent<SpriteRenderer>().sprite = happylook;
            madText.gameObject.SetActive(false);
            happyText.gameObject.SetActive(true);
            textbox.gameObject.SetActive(true);
            happySound.Play();
            chatter.Play();
            yield return new WaitForSeconds(5);
            happyText.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            textbox.gameObject.SetActive(false);
            tension.Up_tension();
        }

    }

    public IEnumerator Mad()
    {
        if (this.gameObject.activeSelf)
        {
            GetComponent<SpriteRenderer>().sprite = madlook;
            happyText.gameObject.SetActive(false);
            madText.gameObject.SetActive(true);
            textbox.gameObject.SetActive(true);
            madSound.Play();
            chatter.Play();
            yield return new WaitForSeconds(5);
            madText.gameObject.SetActive(false);
            textbox.gameObject.SetActive(false);
        }
    }
}
