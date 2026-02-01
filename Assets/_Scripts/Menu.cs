using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public IEnumerator Playh()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
    }
    public IEnumerator Quith()
    {
        yield return new WaitForSeconds(1);
        Application.Quit();
    }
    public void Playing()
    {
        StartCoroutine(Playh());
    }
    public void Quitting()
    {
        StartCoroutine(Quith());
    }

   
}
