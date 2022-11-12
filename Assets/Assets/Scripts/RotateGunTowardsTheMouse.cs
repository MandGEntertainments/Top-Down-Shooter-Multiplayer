using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGunTowardsTheMouse : MonoBehaviour
{

   public float offset;
   public float x, z;
   private void Update()
   {
      Vector3 mousePos = Input.mousePosition;
      mousePos.z = 5.23f;
 
      Vector3 gunPos = Camera.main.WorldToScreenPoint(transform.position);
      mousePos.x -= gunPos.x;
      mousePos.y -= gunPos.y;
 
      float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg+offset;
      transform.rotation = Quaternion.Euler(new Vector3(0, -angle, 0f));
   }
}
