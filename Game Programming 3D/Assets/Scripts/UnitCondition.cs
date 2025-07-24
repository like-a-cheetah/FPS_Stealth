using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCondition : MonoBehaviour
{
    public float HP;
    public bool heal;

    public float coolTime = 4f;

    public Camera mainCamera;
    public Canvas EndCanvas;
    public SphereCollider PlayerCollider;
    public Canvas AimCanvas;

    private void Start()
    {
        if(this.name == "P1")
            EndCanvas.enabled = false;
    }

    private void Update()
    {
        if (HP <= 0)
        {
            if (this.name == "P1")
            {
                //Destroy(this.gameObject);
                mainCamera.enabled = true;
                EndCanvas.enabled = true;
                AimCanvas.enabled = false;
                PlayerCollider.enabled = false;
                //Destroy(this.gameObject);
            }
            else
                Destroy(this.gameObject);
        }

        if (name == "P1")
        {
            if (HP >= 100)
            {
                HP = 100;
                return;
            }
            else
            {
                if (coolTime >= 0f)
                    coolTime -= Time.deltaTime;
                else
                    coolTime = 0f;
            }

            if (coolTime <= 0f)
            {
                HP += 0.25f;
            }
        }
    }
}
