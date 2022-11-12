using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionAnimation : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //StartCoroutine(PlayAnimations(other));
            other.GetComponent<Animator>().SetBool("Victory", true);
            other.GetComponent<Animator>().SetBool("Victory", false);
        }
    }

    IEnumerator PlayAnimations(Collider other)
    {
        other.GetComponent<Animator>().SetBool("Victory", true);
        yield return new WaitForSeconds(2.5f);
        other.GetComponent<Animator>().SetBool("Victory", false);
        other.GetComponent<Animator>().SetBool("Knockout", true);
        yield return new WaitForSeconds(2.5f);
        other.GetComponent<Animator>().SetBool("Knockout", false);
        other.GetComponent<Animator>().SetBool("GetUp", true);
        yield return new WaitForSeconds(2f);
        other.GetComponent<Animator>().SetBool("GetUp", false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Animator>().SetBool("Victory", false);
        }
    }
}
