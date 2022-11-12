using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunController : MonoBehaviour
{
   public Vector3 screenPoint;
   public Vector3 worldPoint;
   
   public GameObject spere;
   
   public GameObject player;
   
   
   public float distance = 30;
   private void Update()
   {
      screenPoint = Input.mousePosition;
      screenPoint.z = Camera.main.nearClipPlane + distance;
      
      worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);
      
      spere.transform.position = new Vector3(worldPoint.x,1, worldPoint.z);
        
      player.transform.LookAt(spere.transform);
      
     
   }   
}
 