
using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviourPunCallbacks
{
    private Rigidbody rb;
    public float speed;
    private PhotonView view;
    public float damagePower;
    private AudioSource bulletImpactAuidio;
    public AudioClip bloodImpactClip;
    public AudioClip otherImpactClip;
    public ScoreHandler scoreHandler;

    void Awake()
    {
        bulletImpactAuidio = GetComponent<AudioSource>();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        view = GetComponent<PhotonView>();
        Destroy(gameObject, 2);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
          /*  if (collision.gameObject.GetComponent<Bots>())
            {
                Debug.Log("I am Returning");
                return;
            }*/
          bulletImpactAuidio.PlayOneShot(bloodImpactClip);
          collision.gameObject.GetComponent<Health>().GetDamage(damagePower,scoreHandler);
            Destroy(gameObject);
        }
        else
        {
            bulletImpactAuidio.PlayOneShot(otherImpactClip);
            Destroy(gameObject);
        }
    }
}