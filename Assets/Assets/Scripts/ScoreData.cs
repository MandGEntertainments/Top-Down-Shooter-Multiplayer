using System;
using System.Collections;
using System.Collections.Generic;
using MiscUtil.Collections.Extensions;
using Photon.Pun.Demo.Cockpit;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class ScoreData : MonoBehaviour
{
   public List<Text> PlayerOneData;
   public List<Text> PlayerTwoData;
   public List<Text> PlayerThreeData;
   public List<Text> PlayerFourData;
   public List<Text> PlayerFiveData;

   public List<Data> data;

   public GameObject scorePanel;
   public Button pauseButton;

   public Text timerText;
   public float secs = 60f;
   public float mins = 0f;
   public int index;
   private float lastIndex;
   public bool gameOver;
   private bool execOnce;
   private int[] scoreArray = new int[]{0,0,0,0,0};

   public bool stopAllControllers;

   public GameObject[] allPlayers;
   private float maxKills;
   private float first, sec, third, fourth, fifth;

   private void Start()
   {
      StartCoroutine(FindAllPlayers());
      scorePanel.SetActive(false);
   }

   IEnumerator FindAllPlayers()
   {
      yield return new WaitForSeconds(0.5f);
      allPlayers = GameObject.FindGameObjectsWithTag("Player");
   }
   private void Update()
   {
      secs -= Time.deltaTime;
      if (secs < 0)
      {
         mins -= 1;
         secs = 60;
       
      }
      if (mins < 0)
      {
         mins = 0;
         secs = 0;
         gameOver = true;
         pauseButton.interactable = false;
      }
      timerText.text = mins +":"+ secs.ToString("00");
      if (gameOver)
      {
         if (!execOnce)
         {
            if (stopAllControllers)
            {
               for (int i = 0; i < allPlayers.Length; i++)
               {
                  allPlayers[i].GetComponent<Health>().StopAllCoroutines();
                  allPlayers[i].GetComponent<Health>().enabled = false;
                  allPlayers[i].GetComponent<TopDownController>().enabled = false;
                  //  allPlayers[i].GetComponent<NavMeshAgent>().enabled = false;
               }

               scorePanel.SetActive(true);
               execOnce = true;
               stopAllControllers = false;
               
            }
            
           
         }
      }
   }

   public void SubmitScore(int index,string playerName,int kills, string deaths,string points)
   {
     // maxKills = float.Parse(kills);
    NormalData(index,playerName,kills.ToString(),deaths,points);

  //DifferentData(index,playerName,kills,deaths,points);

   }

   private void DifferentData(int i, string playerName, int kills, string deaths, string points)
   {
      
      data[i].kills = kills;
      data[i].playerName = playerName;
      data[i].deaths = deaths;
      data[i].points = points;

   }

   void NormalData(int index,string playerName,string kills, string deaths,string points)
   {
      if (index == 0)
      {
         PlayerOneData[0].text = playerName;
         PlayerOneData[1].text = kills;
         PlayerOneData[2].text = deaths;
         PlayerOneData[3].text = points;
      }
      else if (index == 1)
      {
         PlayerTwoData[0].text = playerName;
         PlayerTwoData[1].text = kills;
         PlayerTwoData[2].text = deaths;
         PlayerTwoData[3].text = points;
      }
      else if (index == 2)
      {
         PlayerThreeData[0].text = playerName;
         PlayerThreeData[1].text = kills;
         PlayerThreeData[2].text = deaths;
         PlayerThreeData[3].text = points;
      }
      else if (index == 3)
      {
         PlayerFourData[0].text = playerName;
         PlayerFourData[1].text = kills;
         PlayerFourData[2].text = deaths;
         PlayerFourData[3].text = points;
      }
      else if (index == 4)
      {
         PlayerFiveData[0].text = playerName;
         PlayerFiveData[1].text = kills;
         PlayerFiveData[2].text = deaths;
         PlayerFiveData[3].text = points;
      }
   }
   
}



[Serializable]
public class Data
{
   public string playerName;
   public int kills;
   public string deaths;
   public string points;
}
