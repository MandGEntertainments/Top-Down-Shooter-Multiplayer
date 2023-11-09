using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fps : MonoBehaviour
{
    private float count;
    public Text fpsText;
    
    
    void Awake(){
        Application.targetFrameRate = 60;
    }
    
    
    private IEnumerator Start()
    {
        GUI.depth = 2;
        while (true)
        {
            count = 1f / Time.unscaledDeltaTime;
            yield return new WaitForSeconds(0.1f);
            fpsText.text = Mathf.Round(count).ToString("00");
        }
    }
    
    /*private void OnGUI()
    {
        GUI.Label(new Rect(5, 40, 100, 25), "FPS: " + Mathf.Round(count));
    }*/
}