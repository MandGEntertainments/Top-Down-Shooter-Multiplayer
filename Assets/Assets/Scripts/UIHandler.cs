using System;
using System.Collections;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public GameObject uiPanel;
    private bool isMenuOpened;
    public ScoreData data;
    private void Start()
    {
        uiPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !data.gameOver)
        {
            isMenuOpened = !isMenuOpened;
            if (!isMenuOpened)
            {
                OnPaused();
            }
            else
            {
                OnResume();
            }
        }
    }

    public void OnPaused()
    {
        uiPanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void OnResume()
    {
        uiPanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void OnLeave()
    {
        StartCoroutine(LeaveRoomCoRoutine());
    }

    IEnumerator LeaveRoomCoRoutine()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
        
       
        while (PhotonNetwork.InRoom)
        {
            yield return null;
        }
        PhotonNetwork.LoadLevel("FirstScene");
    }
    public void OnCloseButton()
    {
        uiPanel.SetActive(false);
        Time.timeScale = 1;
    }

    /*public void QuitMatch()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("FirstScene");
    }*/
}
