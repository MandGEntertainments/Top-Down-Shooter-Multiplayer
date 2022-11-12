using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollowPlayer : MonoBehaviour
{
    public Transform playerObj;

    // Update is called once per frame
    void Update()
    {
        float xPOs = playerObj.position.x;
        float zPos = playerObj.position.z - 2;

        transform.position = new Vector3(xPOs, transform.position.y, zPos);
        
        transform.LookAt(playerObj.transform);
    }
}
