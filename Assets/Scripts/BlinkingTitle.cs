using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingTitle : MonoBehaviour
{

    public Text titleText;

    public void Start()
    {
        StartBlinking();
    }
    public IEnumerator Blink()
    {
        while (true)
        {
            titleText.color = Color.green;
            yield return new WaitForSeconds(0.1f);
            titleText.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            titleText.color = Color.blue;
            yield return new WaitForSeconds(0.1f);
            titleText.color = Color.magenta;
            yield return new WaitForSeconds(0.1f);
            titleText.color = Color.cyan;
            yield return new WaitForSeconds(0.1f);
            titleText.color = Color.yellow;
            yield return new WaitForSeconds(0.1f);

        }
    }

    void StartBlinking()
    {
        StartCoroutine("Blink");
    }  
}

