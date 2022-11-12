using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public GameObject bulletObj;
    public Transform bulletLocation;
    public Transform parObj;

    private bool autoFire;
    public int bulletCout = 150;
    public float fireRate;
    private float firerateLimit;


    [Header("TextArea")]
    public Text bulletCount;

    [Space] public GameObject rImg;
    public GameObject reload;
    public GameObject reloading;
    
    void Update()
    {
        bulletCount.text = "" + bulletCout;
        
        
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0,0,0.1f);
        }
        if (Input.GetKey(KeyCode.S))
        { 
            transform.Translate(0,0,-0.1f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-0.1f,0,0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(0.1f,0,0);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (bulletCout!=30)
            {
                reloading.SetActive(true);
                reload.SetActive(false);
                StartCoroutine(reloadBullet());
            }
        }
        if (bulletCout==30 && reloading.activeInHierarchy)
        {
            reloading.SetActive(false);
            rImg.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            fireTypesChanger();
        }
        
        fireMeth();
    }

    IEnumerator reloadBullet()
    {
        yield return new WaitForSeconds(2);
        bulletCout = 150;
    }
    
    public void fireTypesChanger()
    {
        autoFire = !autoFire;
    }


    public void fireMeth()
    {
        if (autoFire) {
            if (Input.GetMouseButton(0)) {
                if (bulletCout != 0) 
                {
                    firerateLimit += fireRate;
                    if (100 < firerateLimit)
                    {
                        firerateLimit = 0;
                        Instantiate(bulletObj, bulletLocation.position, bulletLocation.rotation, parObj);
                        bulletCout--;
                    }
                }
                if (bulletCout==0 && !rImg.activeInHierarchy)
                {
                    rImg.SetActive(true);
                    reload.SetActive(true);
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (bulletCout !=0 )
                {
                    bulletCout--;
                    Instantiate(bulletObj, bulletLocation.position, bulletLocation.rotation, parObj);
                }
                if (bulletCout==0 && !rImg.activeInHierarchy)
                {
                    rImg.SetActive(true);
                    reload.SetActive(true);
                }
            }
        }
    }
}
