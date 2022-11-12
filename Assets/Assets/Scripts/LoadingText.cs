using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingText : MonoBehaviour
{
    public TextMeshProUGUI text;
    private bool play = true;
    private bool playAgain;
    private void Start()
    {
        StartCoroutine(PlayAnime());
    }

    IEnumerator PlayAnime()
    {
        while (play)
        {
                text.text = "L";
                yield return new WaitForSeconds(0.1f);
                text.text = "Lo";
                yield return new WaitForSeconds(0.1f);
                text.text = "Loa";
                yield return new WaitForSeconds(0.1f);
                text.text = "Load";
                yield return new WaitForSeconds(0.1f);
                text.text = "Loadi";
                yield return new WaitForSeconds(0.1f);
                text.text = "Loadin";
                yield return new WaitForSeconds(0.1f);
                text.text = "Loading";
                yield return new WaitForSeconds(0.1f);
                text.text = "Loading.";
                yield return new WaitForSeconds(0.1f);
                text.text = "Loading..";
                yield return new WaitForSeconds(0.1f);
                text.text = "Loading...";
                yield return new WaitForSeconds(0.1f);
            }
    }
}
