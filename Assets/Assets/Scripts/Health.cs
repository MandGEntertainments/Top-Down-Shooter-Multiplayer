using Photon.Pun;
using System.Collections;
using System.Text;
using ExitGames.Client.Photon.StructWrapping;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviourPunCallbacks, IPunObservable
{
    public float health = 100;
    private TopDownController controller;
    private SpawnHelper spawnHelper;
 //   private ScoreHandler scoreHandler;
    private Animator animator;
    public Slider healthBar;
    public Image healthBarImageForColorTransition;
    public GameObject spawnText;
    private Transform spawnTextContainer;

    Text healthText;
    Text reSpawnText;
    private bool execOnce;
    public bool Died;
    public float deaths;
    private void Start()
    {
        spawnTextContainer = GameObject.FindWithTag("MessageContainer").transform;
        controller = GetComponent<TopDownController>();
        animator = GetComponent<Animator>();
        spawnHelper = GameObject.Find("SpawnHelper").GetComponent<SpawnHelper>();
       // reSpawnText = GameObject.Find("PlayerOneText").GetComponent<Text>();
        //scoreHandler = GameObject.Find("ScoreHandler").GetComponent<ScoreHandler>();
    }


    private void OnEnable()
    {
        if (!GetComponent<TopDownController>().isBot)
        {
            healthText = GameObject.Find("HealthText").GetComponent<Text>();
            healthText.text = "Health: "+health+" %";
        }
    }
    /*private void Update()
    {
        if (Input.GetMouseButtonDown((0)))
        {
            GetDamage(10);
        }
    }*/


    public void GetDamage(float damage,ScoreHandler scoreHandler)
    {
        if (!Died)
        {
            health -= damage;
            if (!controller.isBot)
            {
                healthText.text = "Health: " + health + " %";
            }

            healthBar.value = health;
            if (healthBar.value >= 90)
            {
                healthBarImageForColorTransition.color = Color.green;
            }
            else if (healthBar.value <= 60f && healthBar.value >= 31f)
            {
                healthBarImageForColorTransition.color = Color.yellow;
            }
            else if (healthBar.value <= 30f && healthBar.value >= 0)
            {
                healthBarImageForColorTransition.color = Color.red;
            }

            if (health <= 0)
            {
                health = 0;
                Die(scoreHandler);
                Died = true;
            }
        }
    }

    void Die(ScoreHandler scoreHandler)
    {
        animator.SetBool("Shoot", false);
        animator.SetBool("ShootSingle", false);
        animator.SetBool("Reload", false);
        animator.SetBool("Die", true);

        scoreHandler.kills += 1;
        
        
        if (!controller.isBot)
        {
            StartCoroutine((RespawnPlayer(scoreHandler)));
            GameObject spawntext = Instantiate(spawnText);
            spawntext.transform.parent = spawnTextContainer;
            spawntext.transform.localScale = Vector3.one;
            spawntext.GetComponent<TextMeshProUGUI>().text = "You Are Killed By:" + scoreHandler.playerName;

        }
        else if (controller.isBot)
        {
            StartCoroutine((RespawnEnemy(scoreHandler)));
        }
    }

    IEnumerator RespawnPlayer(ScoreHandler scoreHandler)
    {
        deaths += 1;
        controller.enabled = false;

        yield return new WaitForSeconds(3f);
        int a = Random.Range(0, spawnHelper.spawnPosesPlayer.Length);
        controller.transform.position = spawnHelper.spawnPosesPlayer[a].position;
        controller.enabled = true;
        animator.SetBool("Die", false);
        health = 100f;
        healthText.text = "Health: " + health;
        healthBar.value = health;
        healthBarImageForColorTransition.color = Color.green;
        //reSpawnText.text = reSpawnText.text + "\n " + PhotonNetwork.NickName + " Respawned.";
        GameObject spawntext = Instantiate(spawnText);
        spawntext.transform.parent = spawnTextContainer;
        spawntext.transform.localScale = Vector3.one;
        spawntext.GetComponent<TextMeshProUGUI>().text = PhotonNetwork.NickName + " Respawned.";
        Died = false;
    }

    IEnumerator RespawnEnemy(ScoreHandler scoreHandler)
    {
        
       // scoreHandler.UpdateScore(1);
        //controller.points += 25;
        deaths += 1;
        controller.enabled = false;
        
        yield return new WaitForSeconds(3f);
        int t = Random.Range(0, spawnHelper.spawnPosesEnemy.Length);
        controller.transform.position = spawnHelper.spawnPosesEnemy[t].position;
        controller.enabled = true;
        animator.SetBool("Die", false);
        health = 100f;
        healthBar.value = health;
        healthBarImageForColorTransition.color = Color.green;
        GameObject spawntext = Instantiate(spawnText);
        spawntext.transform.parent = spawnTextContainer;
        spawntext.GetComponent<TextMeshProUGUI>().text = controller.playerName + " Respawned.";
        spawntext.transform.localScale = Vector3.one;
        Died = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else
        {
            health = (float)stream.ReceiveNext();
        }
    }
}