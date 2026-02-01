using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
    public TMP_Text happyText;
    public TMP_Text madText;
    public TensionTracker tension;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponentInChildren<Renderer>().material.color = Color.grey;
        happyText.gameObject.SetActive(false);
        madText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Happy()
    {
        if (this.gameObject.activeSelf)
        {
            gameObject.GetComponentInChildren<Renderer>().material.color = Color.green;
            madText.gameObject.SetActive(false);
            happyText.gameObject.SetActive(true);
            yield return new WaitForSeconds(5);
            happyText.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            tension.Up_tension();
        }

    }

    public IEnumerator Mad()
    {
        if (this.gameObject.activeSelf)
        {
            gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
            happyText.gameObject.SetActive(false);
            madText.gameObject.SetActive(true);
            yield return new WaitForSeconds(5);
            madText.gameObject.SetActive(false);
        }
    }
}
