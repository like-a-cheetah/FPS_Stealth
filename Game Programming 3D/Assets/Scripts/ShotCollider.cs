using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotCollider : MonoBehaviour
{
    public Transform player;
    //Transform parentPosition;

    void Awake()
    {
        //parentPosition = GetComponentInParent<Transform>();
    }

    private void Update()
    {
        this.transform.position = player.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy" && !other.GetComponentInParent<Enemy>().canAttack)
        {
            other.GetComponentInParent<Enemy>().goalTime = 0f;
            other.GetComponentInParent<Enemy>().patrol = false;
            other.GetComponentInParent<Enemy>().shotSound = true;

            other.GetComponentInParent<Enemy>().pathFinder.SetDestination(transform.position);
        }
    }
}
