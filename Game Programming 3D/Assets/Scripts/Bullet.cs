using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Bullet : MonoBehaviour
{
    float speed = 30f;
    public float activeTime;
    private float time;

    private float damage = 13;

    public GameObject mom;

    public AudioSource sound;
    //public AudioClip hitClip;

    private void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    public void setSpeed(float changeValue)
    {
        speed = changeValue;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);

        time += Time.deltaTime;
        if (time >= activeTime)
        {
            damage -= Time.deltaTime;
            Debug.Log("사라짐");
          /*  PhotonNetwork.*/Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            Destroy(this.gameObject);
            return;
        }

        if (this.name == "EnemyBullet(Clone)")
        {
            if (other.tag == "Player")
            {
                if(other.GetComponentInParent<UnitCondition>().HP >= 0)
                    Debug.Log("플레이어 적중");
                other.GetComponentInParent<UnitCondition>().HP -= (int)damage;
                other.GetComponentInParent<UnitCondition>().coolTime = 3.5f;
                other.GetComponentInParent<Animator>().SetTrigger("Damage");
                other.GetComponentInParent<Animator>().SetInteger("DamageID", Random.Range(0, 3));
                //other.GetComponentInParent<AudioSource>().clip = hitClip;
                //other.GetComponentInParent<AudioSource>().volume = 5f;
                //other.GetComponentInParent<AudioSource>().pitch = 1f;
                //other.GetComponentInParent<AudioSource>().clip = hitClip;
                //other.GetComponentInParent<AudioSource>().PlayOneShot(hitClip);
                /*   PhotonNetwork.*/
                Destroy(this.gameObject);
            }
        }
        else if(this.name == "PlayerBullet(Clone)")
        {
            if (other.tag == "Enemy")
            {
                sound.Play();
                Debug.Log("AI 적중");
                other.GetComponentInParent<UnitCondition>().HP -= (int)damage;
                other.GetComponentInParent<Animator>().SetTrigger("Damage");
                other.GetComponentInParent<Animator>().SetInteger("DamageID", Random.Range(0, 3));
                //other.GetComponentInParent<Enemy>().trace = true;
                //other.GetComponentInParent<Enemy>().target = ;

                sound.Play();
                //other.GetComponentInParent<AudioSource>().PlayOneShot(hitClip);
                /*   PhotonNetwork.*/
                Destroy(this.gameObject);
            }
        }
    }
}
