using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public float speed;
    public GameObject[] characters;
    public int selectedCharacter;

    public GameObject cameraGameObject;
    public bool swipedLeft,swipedRight;
    float x1, x2;
    public float valueHolder;

    public TextMeshProUGUI lockedStateText;

    //Weapons data

    public TextMeshProUGUI weaponName;
    public Slider overallStrength;
    public Slider firingRate;
    public Slider damagePower;
    public Slider reloadTime;
    public float sliderSpeed;
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("FirstScene");
        }
       
        if (Input.GetMouseButtonDown(0))
        {
            x1 = Input.mousePosition.x;
        }
        if (Input.GetMouseButtonUp(0))
        {
            x2 = Input.mousePosition.x;

            if(x1 > x2)
            {
                valueHolder -= 4;
                if(valueHolder <= 0)
                {
                    valueHolder = 0;
                }
                swipedLeft = true;
            }
            if (x2 > x1)
            {
                valueHolder += 4;
                if (valueHolder >= 20)
                {
                    valueHolder = 20;
                }
                swipedRight = true;
            }
        }
        HandleSwipes();
        HandleSlider();

    }
    void HandleSwipes()
    {
        if (swipedRight)
        {
            if (cameraGameObject.transform.position.x <= valueHolder)
            {
                cameraGameObject.transform.Translate(-Vector3.right * speed);
                if (cameraGameObject.transform.position.x == valueHolder)
                {
                    swipedRight = false;
                }
            }
        }
        if (swipedLeft)
        {
            if (cameraGameObject.transform.position.x >= valueHolder)
            {
                cameraGameObject.transform.Translate(Vector3.right * speed);
                if (cameraGameObject.transform.position.x == valueHolder)
                {
                    swipedLeft = false;
                }
            }
        }
    }
    void HandleSlider()
    {
        if(valueHolder == 0)
        {
            lockedStateText.text = "Unlocked.";
            lockedStateText.color = Color.green;
            weaponName.text = "Magnum";
            if(overallStrength.value < 0.5f)
            {
                overallStrength.value += sliderSpeed;
            }else if(overallStrength.value > 0.51f)
            {
                overallStrength.value -= sliderSpeed;
            }

            if (firingRate.value < 0.1f)
            {
                firingRate.value += sliderSpeed;
            }
            else if (firingRate.value > 0.11f)
            {
                firingRate.value -= sliderSpeed;
            }

            if (damagePower.value < 0.7f)
            {
                damagePower.value += sliderSpeed;
            }
            else if (damagePower.value > 0.71f)
            {
                damagePower.value -= sliderSpeed;
            }

            if (reloadTime.value < 0.1f)
            {
                reloadTime.value += sliderSpeed;
            }
            else if (reloadTime.value > 0.11f)
            {
                reloadTime.value -= sliderSpeed;
            }
        }
        if (valueHolder == 4)
        {
            lockedStateText.text = "Unlocked.";
            lockedStateText.color = Color.green;
            weaponName.text = "Ump";
            if (overallStrength.value < 0.5f)
            {
                overallStrength.value += sliderSpeed;
            }
            else if (overallStrength.value > 0.51f)
            {
                overallStrength.value -= sliderSpeed;
            }

            if (firingRate.value < 0.4f)
            {
                firingRate.value += sliderSpeed;
            }
            else if (firingRate.value > 0.41f)
            {
                firingRate.value -= sliderSpeed;
            }

            if (damagePower.value < 0.4f)
            {
                damagePower.value += sliderSpeed;
            }
            else if (damagePower.value > 0.41f)
            {
                damagePower.value -= sliderSpeed;
            }

            if (reloadTime.value < 0.5f)
            {
                reloadTime.value += sliderSpeed;
            }
            else if (reloadTime.value > .56f)
            {
                reloadTime.value -= sliderSpeed;
            }
        }
        if (valueHolder == 8)
        {
            lockedStateText.text = "Unlocked.";
            lockedStateText.color = Color.green;
            
            weaponName.text = "ShotGun";
            if (overallStrength.value < 0.7f)
            {
                overallStrength.value += sliderSpeed;
            }
            else if (overallStrength.value > 0.71f)
            {
                overallStrength.value -= sliderSpeed;
            }

            if (firingRate.value < 0f)
            {
                firingRate.value += sliderSpeed;
            }
            else if (firingRate.value > 0.01f)
            {
                firingRate.value -= sliderSpeed;
            }

            if (damagePower.value < .98f)
            {
                damagePower.value += sliderSpeed;
            }
            else if (damagePower.value > 0.99f)
            {
                damagePower.value -= sliderSpeed;
            }

            if (reloadTime.value < 0f)
            {
                reloadTime.value += sliderSpeed;
            }
            else if (reloadTime.value > .01f)
            {
                reloadTime.value -= sliderSpeed;
            }
        }
        if (valueHolder == 12)
        {
            lockedStateText.text = "Unlocked.";
            lockedStateText.color = Color.green;
            
            weaponName.text = "SMG-Auto";
            if (overallStrength.value < 0.7f)
            {
                overallStrength.value += sliderSpeed;
            }
            else if (overallStrength.value > 0.71f)
            {
                overallStrength.value -= sliderSpeed;
            }

            if (firingRate.value < 0.8f)
            {
                firingRate.value += sliderSpeed;
            }
            else if (firingRate.value > 0.81f)
            {
                firingRate.value -= sliderSpeed;
            }

            if (damagePower.value < 0.4f)
            {
                damagePower.value += sliderSpeed;
            }
            else if (damagePower.value > 0.41f)
            {
                damagePower.value -= sliderSpeed;
            }

            if (reloadTime.value < 0.5f)
            {
                reloadTime.value += sliderSpeed;
            }
            else if (reloadTime.value > .56f)
            {
                reloadTime.value -= sliderSpeed;
            }
        }
        if (valueHolder == 16)
        {
            lockedStateText.text = "Locked.";
            lockedStateText.color = Color.red;
            weaponName.text = "AK-47";
            if (overallStrength.value < .98f)
            {
                overallStrength.value += sliderSpeed;
            }
            else if (overallStrength.value > 0.99f)
            {
                overallStrength.value -= sliderSpeed;
            }

            if (firingRate.value < .7f)
            {
                firingRate.value += sliderSpeed;
            }
            else if (firingRate.value > 0.71f)
            {
                firingRate.value -= sliderSpeed;
            }

            if (damagePower.value < 0.8f)
            {
                damagePower.value += sliderSpeed;
            }
            else if (damagePower.value > 0.81f)
            {
                damagePower.value -= sliderSpeed;
            }

            if (reloadTime.value < 0.3f)
            {
                reloadTime.value += sliderSpeed;
            }
            else if (reloadTime.value > .31f)
            {
                reloadTime.value -= sliderSpeed;
            }
        }
        if (valueHolder == 20)
        {
            lockedStateText.text = "Locked.";
            lockedStateText.color = Color.red;
            weaponName.text = "AWM";
            if (overallStrength.value < 0.9f)
            {
                overallStrength.value += sliderSpeed;
            }
            else if (overallStrength.value > 0.91f)
            {
                overallStrength.value -= sliderSpeed;
            }

            if (firingRate.value < 0.08f)
            {
                firingRate.value += sliderSpeed;
            }
            else if (firingRate.value > 0.081f)
            {
                firingRate.value -= sliderSpeed;
            }

            if (damagePower.value < .98f)
            {
                damagePower.value += sliderSpeed;
            }
            else if (damagePower.value > 0.99f)
            {
                damagePower.value -= sliderSpeed;
            }

            if (reloadTime.value < 0.01f)
            {
                reloadTime.value += sliderSpeed;
            }
            else if (reloadTime.value > 0.012f)
            {
                reloadTime.value -= sliderSpeed;
            }
        }
    }


    public void SelectButton()
    {
        /*if(valueHolder== 16 || valueHolder == 20)
        {
            Debug.Log("Characters Are Locked. ");
            lockedStateText.text = "Locked.";
            lockedStateText.color = Color.red;
        }
        else
        {
            lockedStateText.text = "Unlocked.";
            lockedStateText.color = Color.green;
            GameManager.instance.selectedChar = (int)valueHolder;
            SceneManager.LoadScene("FirstScene");
        }*/
        lockedStateText.text = "Unlocked.";
        lockedStateText.color = Color.green;
        GameManager.instance.selectedChar = (int)valueHolder;
        SceneManager.LoadScene("FirstScene");
        
    }
    public void OnBackPressed()
    {
        SceneManager.LoadScene("FirstScene");
    }

}
