using System;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject Magnum, Ump, SMG, Shotgun, Ak47, Sniper;
    public GameObject enemyOne, enemyTwo, enemyThree, enemyFour;
    public Transform[] poses;

    public Transform enemyPosOne, enemyPosTwo, enemyPosThree, enemyPosFour;
    public GameObject instantiatedPrefab;

    private void Awake()
    {
        SpawPlayer();
        SpawBots();
    }

    public void SpawPlayer()
    {
        if (GameManager.instance.selectedChar == 0)
        {
            instantiatedPrefab = PhotonNetwork.Instantiate(Magnum.name, poses[Random.Range(0, poses.Length)].position,
                Quaternion.identity);
            instantiatedPrefab.name = "Player";
            instantiatedPrefab.GetComponent<TopDownController>().index = 0;
        }
        else if (GameManager.instance.selectedChar == 4)
        {
            instantiatedPrefab = PhotonNetwork.Instantiate(Ump.name, poses[Random.Range(0, poses.Length)].position,
                Quaternion.identity);
            instantiatedPrefab.name = "Player";
            instantiatedPrefab.GetComponent<TopDownController>().index = 0;
        }
        else if (GameManager.instance.selectedChar == 8)
        {
            instantiatedPrefab = PhotonNetwork.Instantiate(Shotgun.name, poses[Random.Range(0, poses.Length)].position,
                Quaternion.identity);
            instantiatedPrefab.name = "Player";
            instantiatedPrefab.GetComponent<TopDownController>().index = 0;
        }
        else if (GameManager.instance.selectedChar == 12)
        {
            instantiatedPrefab = PhotonNetwork.Instantiate(SMG.name, poses[Random.Range(0, poses.Length)].position,
                Quaternion.identity);
            instantiatedPrefab.name = "Player";
            instantiatedPrefab.GetComponent<TopDownController>().index = 0;
        }
        else if (GameManager.instance.selectedChar == 16)
        {
            instantiatedPrefab = PhotonNetwork.Instantiate(Ak47.name, poses[Random.Range(0, poses.Length)].position,
                Quaternion.identity);
            instantiatedPrefab.name = "Player";
            instantiatedPrefab.GetComponent<TopDownController>().index = 0;
        }
        else if (GameManager.instance.selectedChar == 20)
        {
            instantiatedPrefab = PhotonNetwork.Instantiate(Sniper.name, poses[Random.Range(0, poses.Length)].position,
                Quaternion.identity);
            instantiatedPrefab.name = "Player";
            instantiatedPrefab.GetComponent<TopDownController>().index = 0;
        }
    }

    public void SpawBots()
    {
        GameObject enemyOneObj = PhotonNetwork.Instantiate(enemyOne.name, enemyPosOne.position, Quaternion.identity);
        enemyOneObj.GetComponent<TopDownController>().index = 1;
        GameObject enemyTwoObj = PhotonNetwork.Instantiate(enemyTwo.name, enemyPosTwo.position, Quaternion.identity);
        enemyTwoObj.GetComponent<TopDownController>().index = 2;
        GameObject enemyThreeObj = PhotonNetwork.Instantiate(enemyThree.name, enemyPosThree.position, Quaternion.identity);
        enemyThreeObj.GetComponent<TopDownController>().index = 3;
        GameObject enemyFourObj = PhotonNetwork.Instantiate(enemyFour.name, enemyPosFour.position, Quaternion.identity);
        enemyFourObj.GetComponent<TopDownController>().index = 4;
    }
}