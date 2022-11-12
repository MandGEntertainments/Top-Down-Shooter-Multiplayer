using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletDestruction : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject,2f);
    }

    void Update()
    {
        transform.Translate(Vector3.forward);
    }
}
