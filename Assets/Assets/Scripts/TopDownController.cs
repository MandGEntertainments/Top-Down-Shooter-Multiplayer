using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MiscUtil.Collections.Extensions;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class TopDownController : MonoBehaviourPunCallbacks, IPunObservable
{
    //Ai
    public bool isBot;
    public float attackRange;
    public float sightRange;
    public LayerMask whatIsPlayer;
    GameObject TargetTransform;
    bool shootAi;
    bool playerIsInRange;
    NavMeshAgent agent;
    NavmeshTargets navMeshAgentTarget;
    public Transform targetForNavMesh;
    int numberOfTarget;
    public List<GameObject> allPlayersList;
    private GameObject[] allPlayers;
    public bool isBotDead;
    public GameObject spawnText;
    private Transform spawnTextContainer;


    private PhotonView view;

    public bool isDead;

    public Animator animator;

    //Player Camera variables
    public enum CameraDirection
    {
        x,
        z
    }

    public CameraDirection cameraDirection = CameraDirection.x;
    public float cameraHeight = 20f;
    public float cameraDistance = 7f;
    public Camera playerCamera;

    public GameObject targetIndicatorPrefab;

    //Player Controller variables
    public float speed = 5.0f;
    public float gravity = 14.0f;
    public float maxVelocityChange = 10.0f;
    public bool canJump = true;

    public float jumpHeight = 2.0f;

    //Private variables
    bool grounded = false;
    Rigidbody rb;

    GameObject targetObject;

    //Mouse cursor Camera offset effect
    Vector2 playerPosOnScreen;
    Vector2 cursorPosition;

    Vector2 offsetVector;

    //Plane that represents imaginary floor that will be used to calculate Aim target position
    Plane surfacePlane = new Plane();

    // Bullet vars

    public GameObject bulletPrefab;
    public Transform bulletInstantiationPos;
    private int ammo;
    [SerializeField] private int maxAmmo;


    //Weapon vars
    public GunFireType chooseType;
    private float fireRateTimer = 0;
    public float gunFireRate;
    public float reloadTime;
    private AudioSource gunAudioSource;
    public AudioClip gunClip;
    public AudioClip noAmmoClip;
    public AudioClip reloadingClip;
    public float damagePower;

    public string playerName;

    Text ammoText;
    Text reloadingText;
    private bool reloadingP, reloadingE;

    // Score
    public int points;
    private ScoreHandler scoreHandler;
    private Health health;
    public int index;
    private ScoreData scoreData;
    public GameObject instantiateScore;
    private Transform scoreboardContainer;

    public enum GunFireType
    {
        Single,
        Auto
    }


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = false;

        scoreData = GameObject.Find("ScoreData").GetComponent<ScoreData>();
        spawnTextContainer = GameObject.FindWithTag("MessageContainer").transform;
            health = GetComponent<Health>();
        if (isBot)
        {
            rb.freezeRotation = false;
            rb.useGravity = true;
            agent = GetComponent<NavMeshAgent>();
            navMeshAgentTarget = GameObject.Find("NavmeshTargets").GetComponent<NavmeshTargets>();
            targetForNavMesh =
                navMeshAgentTarget.targets[UnityEngine.Random.Range(0, navMeshAgentTarget.targets.Length - 1)];
        }
        else
        {
            playerName = PhotonNetwork.NickName;
        }
        //Instantiate aim target prefab


        //Hide the cursor
        Cursor.visible = false;

        gunAudioSource = GetComponent<AudioSource>();
        ammo = maxAmmo;
        view = GetComponent<PhotonView>();

        if (!isBot)
        {
            if (targetIndicatorPrefab)
            {
                targetObject = Instantiate(targetIndicatorPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            }
        }
        else
        {
            Destroy(targetObject);
        }
    }

    void OnEnable()
    {
        /*if (isBot)
        {
            //Text t = GameObject.Find("PlayerOneText").GetComponent<Text>();
            //t.text = t.text + "\n Player" + GetComponent<PhotonView>().GetInstanceID().ToString() + " Joined Game.";
            GameObject spawnTextObj = Instantiate(this.spawnText);
            spawnTextObj.transform.parent = spawnTextContainer;
            spawnTextObj.GetComponent<TextMeshProUGUI>().text = playerName + " Joined Game.";
        }
        else
        {
            playerName = PhotonNetwork.NickName;
            GameObject spawnTextObj = Instantiate(this.spawnText);
            spawnTextObj.transform.parent = spawnTextContainer;
            spawnTextObj.GetComponent<TextMeshProUGUI>().text = playerName + " Joined Game.";
            ammoText = GameObject.Find("AmmoText").GetComponent<Text>();
            ammoText.text = "Ammo: " + ammo;
            reloadingText = GameObject.Find("ReloadingText").GetComponent<Text>();
        }*/
    }

    private void Start()
    {
        if (view.IsMine)
        {
            playerCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            scoreHandler = GetComponent<ScoreHandler>();
        }

        else
        {
        }

        if (isBot)
        {
            //Text t = GameObject.Find("PlayerOneText").GetComponent<Text>();
            //t.text = t.text + "\n Player" + GetComponent<PhotonView>().GetInstanceID().ToString() + " Joined Game.";
            GameObject spawnTextObj = Instantiate(this.spawnText);
            spawnTextObj.transform.parent = spawnTextContainer;
            spawnTextObj.GetComponent<TextMeshProUGUI>().text = playerName + " Joined Game.";
            spawnTextObj.transform.localScale = Vector3.one;
            FindAllEnemies();
        }
        else
        {
           
            GameObject spawnTextObj = Instantiate(this.spawnText);
            spawnTextObj.transform.parent = spawnTextContainer;
            spawnTextObj.GetComponent<TextMeshProUGUI>().text = playerName + " Joined Game.";
            spawnTextObj.transform.localScale = Vector3.one;
            ammoText = GameObject.Find("AmmoText").GetComponent<Text>();
            ammoText.text = "Ammo: " + ammo;
            reloadingText = GameObject.Find("ReloadingText").GetComponent<Text>();
        }
    }

    void FindAllEnemies()
    {
        allPlayers = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < allPlayers.Length; i++)
        {
            if (!(allPlayers[i].GetComponent<TopDownController>().playerName == playerName))
            {
                allPlayersList.Add(allPlayers[i]);
            }
        }
    }

    void FixedUpdate()
    {
        if (!isBot)
        {
            if (view.IsMine)
            {
                Vector3 cameraOffset = Vector3.zero;
                if (cameraDirection == CameraDirection.x)
                {
                    cameraOffset = new Vector3(cameraDistance, cameraHeight, 0);
                }
                else if (cameraDirection == CameraDirection.z)
                {
                    cameraOffset = new Vector3(0, cameraHeight, cameraDistance);
                }

                if (grounded)
                {
                    Vector3 targetVelocity = Vector3.zero;
                    // Calculate how fast we should be moving
                    if (cameraDirection == CameraDirection.x)
                    {
                        targetVelocity = new Vector3(Input.GetAxis("Vertical") * (cameraDistance >= 0 ? -1 : 1), 0,
                            Input.GetAxis("Horizontal") * (cameraDistance >= 0 ? 1 : -1));
                    }
                    else if (cameraDirection == CameraDirection.z)
                    {
                        targetVelocity = new Vector3(Input.GetAxis("Horizontal") * (cameraDistance >= 0 ? -1 : 1), 0,
                            Input.GetAxis("Vertical") * (cameraDistance >= 0 ? -1 : 1));
                    }

                    targetVelocity *= speed;

                    // Apply a force that attempts to reach our target velocity
                    Vector3 velocity = rb.velocity;
                    Vector3 velocityChange = (targetVelocity - velocity);
                    velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                    velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                    velocityChange.y = 0;
                    rb.AddForce(velocityChange, ForceMode.VelocityChange);

                    // Jump
                    if (canJump && Input.GetButton("Jump"))
                    {
                        rb.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
                    }
                }

                // We apply gravity manually for more tuning control
                rb.AddForce(new Vector3(0, -gravity * rb.mass, 0));

                grounded = false;

                //Mouse cursor offset effect
                playerPosOnScreen = playerCamera.WorldToViewportPoint(transform.position);
                cursorPosition = playerCamera.ScreenToViewportPoint(Input.mousePosition);
                offsetVector = cursorPosition - playerPosOnScreen;

                //Camera follow
                playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position,
                    transform.position + cameraOffset, Time.deltaTime * 7.4f);
                playerCamera.transform.LookAt(transform.position +
                                              new Vector3(-offsetVector.y * 2, 0, offsetVector.x * 2));

                //Aim target position and rotation
                if (targetObject != null)
                {
                    targetObject.transform.position = GetAimTargetPos();
                    targetObject.transform.LookAt(new Vector3(transform.position.x, targetObject.transform.position.y,
                        transform.position.z));
                }


                transform.LookAt(new Vector3(targetObject.transform.position.x, transform.position.y,
                    targetObject.transform.position.z));
                //Player rotation
                //   view.RPC("RotateInAllPlayers",RpcTarget.All,transform.eulerAngles.y);
            }
        }
        else
        {
            // Ai
            HandleAIMovement();
        }


        //Setup camera offset
    }

    private void Update()
    {
        if (scoreData.gameOver)
        {
            scoreData.SubmitScore(index,playerName,scoreHandler.kills,health.deaths.ToString(),points.ToString());
            scoreData.stopAllControllers = true;
            Cursor.visible = true;
        }
        if (!isBot)
        {
            if (view.IsMine)
            {
                if (Input.GetMouseButton(0))
                {
                    if (Time.time > fireRateTimer)
                    {
                        if (ammo > 0)
                        {
                            //view.RPC("Shoot", RpcTarget.All);
                            Shoot();
                        }
                        else
                        {
                            fireRateTimer = Time.time + (gunFireRate * 3);
                            if (chooseType == GunFireType.Single)
                            {
                                animator.SetBool("ShootSingle", false);
                            }
                            else if (chooseType == GunFireType.Auto)
                            {
                                animator.SetBool("Shoot", false);
                            }

                            animator.SetBool("Reload", true);
                            gunAudioSource.PlayOneShot(noAmmoClip);
                            if (!reloadingP)
                            {
                                reloadingP = true;
                                StartCoroutine(ReloadGun(false));
                            }
                        }
                    }
                    else
                    {
                    }
                }
                else
                {
                    if (chooseType == GunFireType.Single)
                    {
                        animator.SetBool("ShootSingle", false);
                    }
                    else if (chooseType == GunFireType.Auto)
                    {
                        animator.SetBool("Shoot", false);
                    }
                }
            }
        }
        else
        {
            //Controll With Ai Controller
            if (shootAi)
            {
                transform.LookAt(new Vector3(TargetTransform.transform.position.x, transform.position.y,
                    TargetTransform.transform.position.z));
                if (Time.time > fireRateTimer)
                {
                    if (ammo > 0)
                    {
                        //view.RPC("Shoot", RpcTarget.All);
                        Shoot();
                    }
                    else
                    {
                        fireRateTimer = Time.time + (gunFireRate * 3);
                        if (chooseType == GunFireType.Single)
                        {
                            animator.SetBool("ShootSingle", false);
                        }
                        else if (chooseType == GunFireType.Auto)
                        {
                            animator.SetBool("Shoot", false);
                        }

                        animator.SetBool("Reload", true);
                        gunAudioSource.PlayOneShot(noAmmoClip);
                        StartCoroutine(ReloadGun(true));
                    }
                }
            }
        }
    }

    IEnumerator ReloadGun(bool isBotis)
    {
        if (!isBotis)
        {
            reloadingText.enabled = true;
            yield return new WaitForSeconds(reloadTime / 3);
            reloadingText.text = "Reloading...3";
            yield return new WaitForSeconds(reloadTime / 3);
            reloadingText.text = "Reloading...2";
            yield return new WaitForSeconds(reloadTime / 3);
            reloadingText.text = "Reloading...1";
            yield return new WaitForSeconds(reloadTime / 3);

            reloadingText.text = " ";
            reloadingText.enabled = false;
            animator.SetBool("Reload", false);
            ammo = maxAmmo;
            if (!isBot)
            {
                ammoText.text = "Ammo: " + ammo.ToString();
            }

            reloadingP = false;
        }
        else if (isBotis)
        {
            yield return new WaitForSeconds(reloadTime);
            animator.SetBool("Reload", false);
            ammo = maxAmmo;
        }
    }

    void Shoot()
    {
        fireRateTimer = Time.time + gunFireRate;
        if (chooseType == GunFireType.Single)
        {
            animator.SetBool("ShootSingle", true);
        }
        else if (chooseType == GunFireType.Auto)
        {
            animator.SetBool("Shoot", true);
        }

        gunAudioSource.PlayOneShot(gunClip);
        ammo -= 1;
        if (!isBot)
        {
            ammoText.text = "Ammo: " + ammo.ToString();
        }

        Bullet b = PhotonNetwork.Instantiate(bulletPrefab.name, bulletInstantiationPos.position, transform.rotation)
            .GetComponent<Bullet>();
        b.damagePower = damagePower;
        b.scoreHandler = scoreHandler;
    }


    /*[PunRPC]
    void RotateInAllPlayers(float t)
    {
        transform.LookAt(new Vector3(targetObject.transform.position.x, t,
            targetObject.transform.position.z));
    }*/

    Vector3 GetAimTargetPos()
    {
        //Update surface plane
        surfacePlane.SetNormalAndPosition(Vector3.up, transform.position);

        //Create a ray from the Mouse click position
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        //Initialise the enter variable
        float enter = 0.0f;

        if (surfacePlane.Raycast(ray, out enter))
        {
            //Get the point that is clicked
            Vector3 hitPoint = ray.GetPoint(enter);

            //Move your cube GameObject to the point where you clicked
            return hitPoint;
        }

        //No raycast hit, hide the aim target by moving it far away
        return new Vector3(-5000, -5000, -5000);
    }

    void OnCollisionStay()
    {
        grounded = true;
    }

    public void Die()
    {
    }

    void HandleAIMovement()
    {
        //playerIsInRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (Vector3.Distance(transform.position, allPlayersList[0].transform.position) <
            15f) //if (Vector3.Distance(transform.position, PlayerTransform.position) < 15)
        {
            shootAi = true;
            TargetTransform = allPlayersList[0].gameObject;
            agent.destination = TargetTransform.transform.position;
        }
        else if (Vector3.Distance(transform.position, allPlayersList[1].transform.position) <
                 15f) //if (Vector3.Distance(transform.position, PlayerTransform.position) < 15)
        {
            shootAi = true;
            TargetTransform = allPlayersList[1].gameObject;
            agent.destination = TargetTransform.transform.position;
        }
        else if (Vector3.Distance(transform.position, allPlayersList[2].transform.position) <
                 15f) //if (Vector3.Distance(transform.position, PlayerTransform.position) < 15)
        {
            shootAi = true;
            TargetTransform = allPlayersList[2].gameObject;
            agent.destination = TargetTransform.transform.position;
        }
        else if (Vector3.Distance(transform.position, allPlayersList[3].transform.position) <
                 15f) //if (Vector3.Distance(transform.position, PlayerTransform.position) < 15)
        {
            shootAi = true;
            TargetTransform = allPlayersList[3].gameObject;
            agent.destination = TargetTransform.transform.position;
        }
        else
        {
            shootAi = false;
            TargetTransform = null;

            //targetForNavMesh = navMeshAgentTarget.targets[numberOfTarget];
            agent.destination = targetForNavMesh.position;

            if (chooseType == GunFireType.Single)
            {
                animator.SetBool("ShootSingle", false);
            }
            else if (chooseType == GunFireType.Auto)
            {
                animator.SetBool("Shoot", false);
            }
        }

        if (Vector3.Distance(transform.position, targetForNavMesh.position) < 2f)
        {
            numberOfTarget++;

            /*if (numberOfTarget >= (navMeshAgentTarget.targets.Length - 1))
            {
                numberOfTarget = 0;
            }*/
            targetForNavMesh =
                navMeshAgentTarget.targets[UnityEngine.Random.Range(0, navMeshAgentTarget.targets.Length - 1)];
            agent.destination = targetForNavMesh.position;
        }
    }

    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.eulerAngles.y);
        }
        else
        {
            transform.eulerAngles = (Vector3)stream.ReceiveNext();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}