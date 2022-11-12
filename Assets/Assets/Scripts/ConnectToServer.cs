using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public byte maxPlayers;
    public List<RoomInfo> rooms;
    public InputField username;
    public Text buttonText;
    private AudioSource uiAudio;
    public AudioClip uiClip;

    private string gameVersion = "1.0";
    private bool autoReconnect;


    public GameObject loadingIndicator;
    public GameObject loadingPanel;

    //Exit UI
    public GameObject exitUi;

    private bool loadNow;
    class Bot
    {
        public string name;				// bot name
        public Vector3 scores; 			// x = kills, y = deaths, z = other scores
        public int characterUsing;		// the chosen character of the bot (index only)
        public int hat;
    }
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        autoReconnect = true;
       
    }
    private void Start()
    {
        uiAudio = GetComponent<AudioSource>();
        exitUi.SetActive(false);
        loadingIndicator.SetActive(false);

        if (PhotonNetwork.IsConnectedAndReady)
        {
            loadingPanel.SetActive(false);
        }
        StartCoroutine(Reconnection());
        if (PlayerPrefs.HasKey("username"))
        {
            username.text = PlayerPrefs.GetString("username");
        }
        else
        {
            username.text = string.Empty;
        }

    }

    // This will automatically connect the client to the server every 2 seconds if not connected:
    IEnumerator Reconnection(){
        while(autoReconnect){
            yield return new WaitForSeconds(2f);

            if (!PhotonNetwork.IsConnected || PhotonNetwork.NetworkClientState == ClientState.ConnectingToMasterServer){
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }
    }
    public void OnClickConnect()
    {
        uiAudio.PlayOneShot(uiClip);
        if (username.text.Length >= 1)
        {
            PhotonNetwork.NickName = username.text;
            buttonText.text = "Connecting....";
            PhotonNetwork.ConnectUsingSettings();
            StartCoroutine(ChangeText());
        }
    }

    private void Update()
    {
        if (PhotonNetwork.InRoom)
        { 
            if (loadNow){
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                    PhotonNetwork.LoadLevel("Level1");
                    loadNow = false;
                }
            }
        }
    }

    public void QuickPlay()
    {
        Hashtable h = new Hashtable();
        h.Add("IsInMatchMaking",true);
        PhotonNetwork.NickName = username.text;
        PlayerPrefs.SetString("username", username.text);
        PhotonNetwork.JoinRandomRoom(h, 0);
        loadNow = true;
    }

    public void OnValueChanged()
    {
        PlayerPrefs.SetString("username", username.text);
    }
    
    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.IsConnectedAndReady) PhotonNetwork.JoinLobby();

        loadingPanel.SetActive(false);
        Debug.Log("We Are Connected To Master....");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("<color=red>" + message + "</color>");
        // Create a new room if we failed to find one:
        if (PhotonNetwork.IsConnectedAndReady)
        {
            // Prepare the room properties:
            Hashtable h = new ExitGames.Client.Photon.Hashtable();
            h.Add("started", false);
            //h.Add("map", Random.Range(0, maps.Length));
            h.Add("isInMatchmaking", true);

            // Then create the room, with the prepared room properties in the RoomOptions argument:
            PhotonNetwork.CreateRoom(null, new RoomOptions()
            {
                MaxPlayers = maxPlayers,
                CleanupCacheOnLeave = true,
                IsVisible = true,
                CustomRoomProperties = h,
                CustomRoomPropertiesForLobby = new string[] {"isInMatchmaking" }
            });
        }
    }
    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //DataCarrier.message = message;
       PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Hashtable p = new Hashtable();
        p.Add("kills", 0);
        p.Add("deaths", 0);
        p.Add("otherScore", 0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(p); 
        Debug.Log("We Are Joined Room....");
    }
    IEnumerator ChangeText()
    {
        yield return new WaitForSeconds(1);
        buttonText.text = "Please Wait...";
        yield return new WaitForSeconds(1);
        buttonText.text = "Connecting...";
    }
    IEnumerator Go()
    {
        buttonText.text = "Let's Go";
        yield return new WaitForSeconds(1f);
      //  SceneManager.LoadScene("Lobby");
    }
    public void SelectCharacter()
    {
        SceneManager.LoadScene("ChooseCharacter");
    }

    public void ShowExitUI()
    {
        exitUi.SetActive(true);
    }
    public void WhenClickedNo()
    {
        exitUi.SetActive(false);
    }
    public void WhenClickedYes()
    {
        Application.Quit();
    }
}