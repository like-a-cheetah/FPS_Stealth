using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    public PlayerController otherPlayer;
    public Enemy[] enemyList;
    public GameObject slider;

    private void Start()
    {
        enemyList = FindObjectsOfType(typeof(Enemy)) as Enemy[];

        for (int i = 0; i<enemyList.Length; i++)
        {
            GameObject HPbar = Instantiate(slider) as GameObject;
            HPbar.transform.SetParent(gameObject.transform, true);

            HPbar.GetComponent<HPBar>().unit = enemyList[i].gameObject;
            HPbar.GetComponent<HPBar>().full = enemyList[i].gameObject;
        }
    }

    private void Update()
    {
        //if (otherPlayer == !this.gameObject.GetComponent<PlayerController>() && otherPlayer != null)
        //    otherPlayer = FindObjectOfType(typeof(PlayerController)) as PlayerController;
    }
}
