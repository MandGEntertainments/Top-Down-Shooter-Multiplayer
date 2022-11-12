using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowScript : MonoBehaviour
{
    public int health = 100;
    public Transform target;
    public ParticleSystem psBlood;

    private void Start()
    {
        psBlood = transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (health !=0)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime*2);
        }

        if (health==0)
        {
            Destroy(gameObject);
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            health -= 10;
            psBlood.Play();
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            target.gameObject.GetComponent<PlayerGunController>().enabled = false;
            target.gameObject.GetComponent<PlayerMovement>().enabled = false;
        }
    }
}
