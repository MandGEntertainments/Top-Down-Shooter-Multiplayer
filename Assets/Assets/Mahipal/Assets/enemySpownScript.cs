using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpownScript : MonoBehaviour
{
    public GameObject enemay;
    public Transform[] spownPoint;
    private int rand;

    public Transform limmitARea;
    void Start()
    {
        InvokeRepeating("enemySp",2,1);
    }

    public void enemySp()
    {
        if (limmitARea.childCount < 10)
        {
            rand = Random.Range(0, spownPoint.Length);
            Instantiate(enemay, spownPoint[rand].position, spownPoint[rand].rotation,limmitARea);
        }
  
    }
}
