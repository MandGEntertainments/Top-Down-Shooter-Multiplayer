using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour
{
    public int kills;

    public int deaths;

    public int points;
    public Text killsText;
    public string playerName;
    private bool exexOnce;
    private TopDownController _controller;

    private void Start()
    {
        _controller = GetComponent<TopDownController>();
        playerName =_controller.playerName;
    }
    public void UpdateScore(int score)
    {
        kills += score;
        //killsText.text = "Kills: " + kills;
    }
}
