using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightCollider : MonoBehaviour
{
    public PlayerController player;
    private SphereCollider sight;

    private bool change;

    private void Start()
    {
        sight = GetComponent<SphereCollider>();
        player = FindObjectOfType<PlayerController>();

    }

    void Update()
    {
        if(player.GetComponent<Animator>().GetBool("Squat"))
        {
            this.transform.position = new Vector3(player.transform.position.x,
                player.transform.position.y + 0.5023f, player.transform.position.z);
            this.transform.rotation = player.transform.rotation;
            sight.center = new Vector3(0.08f, 0f, -0.124f);
        }
        else{
            this.transform.position = new Vector3(player.transform.position.x,
                player.transform.position.y + 0.8059995231628f, player.transform.position.z);
            this.transform.rotation = player.transform.rotation;
            sight.center = new Vector3(0.046f, 0f, -0.047f);
        }
    }
}
