using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerGun : MonoBehaviour
{
    public Transform muzzle;
    public GameObject Unit;
    public Bullet bullet;

    private Animator animator;

    public float timer = 100f;
    public float velocity = 100f;

    float coolTime;

    Vector3 basicGunHole;
    Vector3 squatGunHole;
    Vector3 jumpGunHole;

    public PhotonView PV;

    private void Start()
    {
        animator = Unit.GetComponent<Animator>();

        basicGunHole = new Vector3(0.047f, 0.7102f, 0.541f);
        squatGunHole = new Vector3(0.076f, 0.4039f, 0.379f);
    }

    private void Update()
    {
        float smoothTime = 0.024f;
        
        if (animator.GetNextAnimatorStateInfo(0).IsName("Wary_Aim") 
            || animator.GetNextAnimatorStateInfo(0).IsName("Fire_Wary")
            || animator.GetNextAnimatorStateInfo(0).IsName("Sneak_Aim")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Wary_Aim")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Fire_Wary")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Sneak_Aim"))
        {
            muzzle.localPosition = Vector3.MoveTowards(muzzle.localPosition, squatGunHole, smoothTime);
        }
        else
        {
            muzzle.localPosition = Vector3.MoveTowards(muzzle.localPosition, basicGunHole, smoothTime);
        }
    }

    public void Shoot()
    {
        if (Time.time > coolTime)
        {
            coolTime = Time.time + timer / 100;
            Bullet newBullet = Instantiate(bullet, muzzle.position, Unit.transform.rotation);

            //if (PV.IsMine)
            //{
            //    PV.RPC("BulletMaster", RpcTarget.AllBuffered);
            //}
        }
    }

    //[PunRPC]
    //void BulletMaster()
    //{
    //    if (PV.IsMine)
    //    {
    //        Bullet newBullet = PhotonNetwork.Instantiate("PlayerBullet", muzzle.position, Unit.transform.rotation).GetComponent<Bullet>();
    //        newBullet.mom = PV.gameObject;
    //    }
    //}
}
