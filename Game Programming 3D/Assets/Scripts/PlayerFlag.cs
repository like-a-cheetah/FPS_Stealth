using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFlag : MonoBehaviour
{
    public PlayerController[] playerList;
    public Text P1;
    public Text P2;

    void Update()
    {
        playerList = FindObjectsOfType(typeof(PlayerController)) as PlayerController[];

        if (playerList.Length == 2)
        {
            //if (playerList[0].PV.ViewID < playerList[1].PV.ViewID)
            //{
            //    P1.transform.position = new Vector3(playerList[0].transform.position.x, playerList[0].transform.position.y + 1.5f, playerList[0].transform.position.z);
            //    P2.transform.position = new Vector3(playerList[1].transform.position.x, playerList[1].transform.position.y + 1.5f, playerList[1].transform.position.z);
            //}
            //else
            //{
            //    P2.transform.position = new Vector3(playerList[0].transform.position.x, playerList[0].transform.position.y + 1.5f, playerList[0].transform.position.z);
            //    P1.transform.position = new Vector3(playerList[1].transform.position.x, playerList[1].transform.position.y + 1.5f, playerList[1].transform.position.z);
            //}
        }
    }
}
