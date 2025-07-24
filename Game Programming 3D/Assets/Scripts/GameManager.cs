using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    //public PlayerController[] playerList;
    //public List<GameObject> enemies;

    public FieldOfView[] FOV;

    public PlayerController P1;

    //public GameObject p1Sight;
    //public GameObject p2Sight;

    public Canvas escPanel;
    public Canvas AimPanel;

    private bool active;

    private void Awake()
    {
        FOV = FindObjectsOfType(typeof(FieldOfView)) as FieldOfView[];
        //for (int i = 0; i < enemies.Count; i++)
        //{
        //    enemies[i].SetActive(false);
        //}

        escPanel.enabled = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        for (int i = 0; i < FOV.Length; i++)
        {
            if(FOV[i].visibleTargets.Count == 1)
            {
                P1.canSee = true;
                break;
            }
            if(i == FOV.Length-1)
                P1.canSee = false;
        }

        //if (!active)
        //{
        //    Debug.Log("321");
        //    playerList = FindObjectsOfType(typeof(PlayerController)) as PlayerController[];
        //    ActiveObject();
        //}

        if (Input.GetKeyDown(KeyCode.Escape) && escPanel.enabled)
        {
            AimPanel.enabled = true;
            escPanel.enabled = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && !escPanel.enabled)
        {
            AimPanel.enabled = false;
            escPanel.enabled = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void ActiveObject()
    {
        //if (playerList.Length == 2)
        //{
        //    //if (playerList[0].PV.ViewID < playerList[1].PV.ViewID)
        //    //{
        //    //    playerList[0].name = "P1";
        //    //    playerList[0].playerSight.name = "P1sight";
        //    //    playerList[1].name = "P2";
        //    //    playerList[1].playerSight.name = "P2sight";
        //    //}
        //    //else
        //    //{
        //    //    playerList[0].name = "P2";
        //    //    playerList[0].playerSight.name = "P2sight";
        //    //    playerList[1].name = "P1";
        //    //    playerList[1].playerSight.name = "P1sight";
        //    //}

        //    //p1Sight.SetActive(true);
        //    //p2Sight.SetActive(true);

        //    //p1Sight.GetComponent<SightCollider>().player = GameObject.Find("P1").GetComponent<PlayerController>();
        //    //p2Sight.GetComponent<SightCollider>().player = GameObject.Find("P2").GetComponent<PlayerController>();

        //    for (int i = 0; i < enemies.Count; i++)
        //        {
        //            enemies[i].SetActive(true);
        //            enemies[i].GetComponentInChildren<EnemySight>().P1 = playerList[1];
        //            enemies[i].GetComponentInChildren<EnemySight>().P2 = playerList[0];
        //            enemies[i].GetComponentInChildren<FieldOfView>().P1 = playerList[1];
        //            enemies[i].GetComponentInChildren<FieldOfView>().P2 = playerList[0];
        //        }

        //    active = true;
        //}
    }
}
