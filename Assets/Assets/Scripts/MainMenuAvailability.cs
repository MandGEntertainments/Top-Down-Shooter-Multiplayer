using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuAvailability : MonoBehaviour
{
    public TextMeshProUGUI availabilityText;

    public void UnAvailable()
    {
        availabilityText.text = "Currently UnAvailable.";
        availabilityText.color = Color.red;
        StartCoroutine(HideIt());
    }

    IEnumerator HideIt()
    {
        yield return new WaitForSeconds(2f);
        availabilityText.text = " ";
    }
}