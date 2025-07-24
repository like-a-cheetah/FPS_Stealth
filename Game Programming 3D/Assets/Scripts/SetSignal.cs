using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSignal : MonoBehaviour
{
    Enemy sender;

    void Awake()
    {
        sender = GetComponentInParent<Enemy>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy" && !other.GetComponentInParent<Enemy>().canAttack)
        {
            other.GetComponentInParent<Enemy>().trace = true;
            other.GetComponentInParent<Enemy>().target = sender.target;
        }
    }
}
