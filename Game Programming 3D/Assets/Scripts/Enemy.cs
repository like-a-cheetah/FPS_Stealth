using UnityEngine;
using System.Collections;
using System;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    public Transform rightGunBone;
    public Transform leftGunBone;
    public Arsenal[] arsenal;

    private Animator animator;

    [HideInInspector]
    public NavMeshAgent pathFinder;
    public PlayerController target;
    private EnemyGun gun;

    private Vector3 startPosition;
    public Transform searchPlace;
    
    public bool canAttack;
    public bool canSee;
    public bool trace;
    public bool patrol;

    public bool walkSound;
    public bool shotSound;

    [HideInInspector]
    public float goalTime;
    [HideInInspector]
    public float traceTime;

    private SphereCollider signalObj;

    public EnemySight sight;

    public AudioSource sound;
    public SoundClip clip;

    float walkSpeed = 2f;
    float runSpeed = 4f;

    void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        gun = GetComponent<EnemyGun>();

        target = null;

        signalObj = GetComponentInChildren<SphereCollider>();

        pathFinder.SetDestination(searchPlace.transform.position);
        startPosition = transform.position;

        if (arsenal.Length > 0)
            SetArsenal(arsenal[0].name);

        patrol = true;
        goalTime = 0f;
    }

    public void SetArsenal(string name)
    {
        foreach (Arsenal hand in arsenal)
        {
            if (hand.name == name)
            {
                if (rightGunBone.childCount > 0)
                    Destroy(rightGunBone.GetChild(0).gameObject);
                if (leftGunBone.childCount > 0)
                    Destroy(leftGunBone.GetChild(0).gameObject);
                if (hand.rightGun != null)
                {
                    GameObject newRightGun = (GameObject)Instantiate(hand.rightGun);
                    newRightGun.transform.parent = rightGunBone;
                    newRightGun.transform.localPosition = Vector3.zero;
                    newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
                }
                if (hand.leftGun != null)
                {
                    GameObject newLeftGun = (GameObject)Instantiate(hand.leftGun);
                    newLeftGun.transform.parent = leftGunBone;
                    newLeftGun.transform.localPosition = Vector3.zero;
                    newLeftGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
                }
                animator.runtimeAnimatorController = hand.controller;
                return;
            }
        }
    }

    private void Update()
    {
        {
            //else
            //{
            //    sound.clip = null;
            //}

            if (sight.visibleTargets.Count > 0)
            {
                target = sight.visibleTargets[0].GetComponent<SightCollider>().player.GetComponent<PlayerController>();
            }

            if (canAttack)
            {
                pathFinder.isStopped = true;
                sound.clip = clip.shot;
                if (!sound.isPlaying)
                    sound.Play();

                sound.volume = 0.1f;
                sound.pitch = 3f; ;

                animator.SetBool("Ismove", false);
                animator.SetBool("Isrun", false);
                animator.SetBool("Isshooting", true);

                patrol = false;
                shotSound = false;
                walkSound = false;

                Vector3 direct = transform.position - target.transform.position;
                transform.eulerAngles = new Vector3(0, (Mathf.Atan2(direct.x, direct.z) * Mathf.Rad2Deg) - 180f, 0);

                gun.Shoot();

                trace = true;
                traceTime = 0f;

                signalObj.enabled = true;
                return;
            }
            else if (trace)
            {
                sound.clip = clip.walk;
                if (!sound.isPlaying)
                    sound.Play();
                sound.volume = 1;
                sound.pitch = 1.75f;

                pathFinder.isStopped = false;
                pathFinder.speed = runSpeed;
                pathFinder.angularSpeed = runSpeed;

                animator.SetBool("Isshooting", false);

                shotSound = false;
                walkSound = false;
                patrol = false;

                pathFinder.stoppingDistance = 0f;
                pathFinder.SetDestination(target.transform.position);

                if (Vector3.Distance(transform.position, pathFinder.destination) > 3f)
                {
                    Vector3 direct = pathFinder.steeringTarget - transform.position;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direct), 7f * Time.deltaTime);
                }
                else
                {
                    Vector3 direct = pathFinder.destination - transform.position;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direct), 7f * Time.deltaTime);
                }

                animator.SetBool("Isrun", true);
                signalObj.enabled = false;

                traceTime += Time.deltaTime;
                if (traceTime >= 10f)
                {
                    target = null;
                    trace = false;
                    patrol = true;
                    pathFinder.SetDestination(searchPlace.position);
                    traceTime = 0f;
                }
                return;
            }
        }

        {
            if (shotSound)
            {
                sound.clip = clip.walk;
                if (!sound.isPlaying)
                    sound.Play();
                sound.volume = 1;
                sound.pitch = 1.75f;

                pathFinder.speed = runSpeed;
                pathFinder.angularSpeed = runSpeed;
                animator.SetBool("Isrun", true);
            }
            else
            {
                sound.clip = clip.walk;
                if (!sound.isPlaying)
                    sound.Play();
                sound.volume = 1;
                sound.pitch = 1.5f;

                pathFinder.speed = walkSpeed;
                pathFinder.angularSpeed = walkSpeed;
                animator.SetBool("Isrun", false);
            }

            if (patrol)
            {
                animator.SetBool("Ismove", true);

                if ((Vector3.SqrMagnitude(transform.position - searchPlace.position) < 0.05f))
                {
                    Vector3 temp = startPosition;
                    startPosition = searchPlace.position;
                    searchPlace.position = new Vector3(temp.x, searchPlace.position.y, temp.z);
                    pathFinder.SetDestination(searchPlace.position);
                }

                Vector3 direct = pathFinder.steeringTarget - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direct), 5f * Time.deltaTime);
                return;
            }

            if (shotSound)
            {
                walkSound = false;
                if (Vector3.SqrMagnitude(transform.position - pathFinder.destination) < 0.05f)
                {
                    sound.clip = null;
                    animator.SetBool("Isrun", false);
                    animator.SetBool("Ismove", false);
                    if (goalTime <= 0.5f)
                    {
                        transform.localRotation *= Quaternion.Euler(new Vector3(0f, 2f, 0f));
                    }
                    else if (goalTime > 1f && goalTime <= 2f)
                    {
                        transform.localRotation *= Quaternion.Euler(new Vector3(0f, -2f, 0f));
                    }
                    else if (goalTime >= 2.5f)
                    {
                        shotSound = false;
                        patrol = true;
                        goalTime = 0f;
                        pathFinder.SetDestination(searchPlace.position);
                        return;
                    }
                    goalTime += Time.deltaTime;
                }
                else
                {
                    animator.SetBool("Ismove", true);

                    if (Vector3.Distance(transform.position, pathFinder.destination) > 5f)
                    {
                        Vector3 direct = pathFinder.steeringTarget - transform.position;
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direct), 7f * Time.deltaTime);
                    }
                    else
                    {
                        Vector3 direct = pathFinder.destination - transform.position;
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direct), 7f * Time.deltaTime);
                    }
                }
            }
            else if (walkSound)
            {
                if (Vector3.SqrMagnitude(transform.position - pathFinder.destination) < 0.05f)
                {
                    sound.clip = null;
                    animator.SetBool("Ismove", false);
                    if (goalTime <= 0.5f)
                    {
                        transform.localRotation *= Quaternion.Euler(new Vector3(0f, 2f, 0f));
                    }
                    else if (goalTime > 1f && goalTime <= 2f)
                    {
                        transform.localRotation *= Quaternion.Euler(new Vector3(0f, -2f, 0f));
                    }
                    else if (goalTime >= 2.5f)
                    {
                        walkSound = false;
                        patrol = true;
                        goalTime = 0f;
                        pathFinder.SetDestination(searchPlace.position);
                        return;
                    }
                    goalTime += Time.deltaTime;
                }
                else
                {
                    animator.SetBool("Ismove", true);

                    if (Vector3.Distance(transform.position, pathFinder.destination) > 5f)
                    {
                        Vector3 direct = pathFinder.steeringTarget - transform.position;
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direct), 7f * Time.deltaTime);
                    }
                    else
                    {
                        Vector3 direct = pathFinder.destination - transform.position;
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direct), 7f * Time.deltaTime);
                    }
                }
                return;
            }
        }

    }

    //[PunRPC]
    //public void Trace()
    //{
    //    if (sight.visibleTargets.Count > 0)
    //    {
    //        target = sight.visibleTargets[0].GetComponent<SightCollider>().player.GetComponent<PlayerController>();
    //    }

    //    if (canAttack)
    //    {
    //        pathFinder.isStopped = true;

    //        animator.SetBool("Ismove", false);
    //        animator.SetBool("Isrun", false);
    //        animator.SetBool("Isshooting", true);

    //        patrol = false;
    //        shotSound = false;
    //        walkSound = false;

    //        Vector3 direct = transform.position - target.transform.position;
    //        transform.eulerAngles = new Vector3(0, (Mathf.Atan2(direct.x, direct.z) * Mathf.Rad2Deg) - 180f, 0);

    //        gun.Shoot();

    //        trace = true;
    //        traceTime = 0f;

    //        signalObj.enabled = true;
    //        return;
    //    }
    //    else if (trace)
    //    {
    //        pathFinder.isStopped = false;
    //        pathFinder.speed = runSpeed;
    //        pathFinder.angularSpeed = runSpeed;

    //        animator.SetBool("Isshooting", false);

    //        shotSound = false;
    //        walkSound = false;
    //        patrol = false;

    //        pathFinder.stoppingDistance = 0f;
    //        pathFinder.SetDestination(target.transform.position);

    //        if (Vector3.Distance(transform.position, pathFinder.destination) > 3f)
    //        {
    //            Vector3 direct = pathFinder.steeringTarget - transform.position;
    //            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direct), 7f * Time.deltaTime);
    //        }
    //        else
    //        {
    //            Vector3 direct = pathFinder.destination - transform.position;
    //            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direct), 7f * Time.deltaTime);
    //        }

    //        animator.SetBool("Isrun", true);
    //        signalObj.enabled = false;

    //        traceTime += Time.deltaTime;
    //        if (traceTime >= 10f)
    //        {
    //            target = null;
    //            trace = false;
    //            patrol = true;
    //            pathFinder.SetDestination(searchPlace.position);
    //            traceTime = 0f;
    //        }
    //        return;
    //    }
    //}

    //[PunRPC]
    //public void Sound() {
    //    if (shotSound)
    //    {
    //        pathFinder.speed = runSpeed;
    //        pathFinder.angularSpeed = runSpeed;
    //        animator.SetBool("Isrun", true);
    //    }
    //    else
    //    {
    //        pathFinder.speed = walkSpeed;
    //        pathFinder.angularSpeed = walkSpeed;
    //        animator.SetBool("Isrun", false);
    //    }

    //    if (patrol)
    //    {
    //        animator.SetBool("Ismove", true);

    //        if ((Vector3.SqrMagnitude(transform.position - searchPlace.position) < 0.05f))
    //        {

    //            Vector3 temp = startPosition;
    //            startPosition = searchPlace.position;
    //            searchPlace.position = new Vector3(temp.x, searchPlace.position.y, temp.z);
    //            pathFinder.SetDestination(searchPlace.position);
    //        }

    //        Vector3 direct = pathFinder.steeringTarget - transform.position;
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direct), 5f * Time.deltaTime);
    //        return;
    //    }

    //    if (shotSound)
    //    {
    //        walkSound = false;
    //        if (Vector3.SqrMagnitude(transform.position - pathFinder.destination) < 0.05f)
    //        {
    //            animator.SetBool("Isrun", false);
    //            animator.SetBool("Ismove", false);
    //            if (goalTime <= 0.5f)
    //            {
    //                transform.localRotation *= Quaternion.Euler(new Vector3(0f, 2f, 0f));
    //            }
    //            else if (goalTime > 1f && goalTime <= 2f)
    //            {
    //                transform.localRotation *= Quaternion.Euler(new Vector3(0f, -2f, 0f));
    //            }
    //            else if (goalTime >= 2.5f)
    //            {
    //                shotSound = false;
    //                patrol = true;
    //                goalTime = 0f;
    //                pathFinder.SetDestination(searchPlace.position);
    //                return;
    //            }
    //            goalTime += Time.deltaTime;
    //        }
    //        else
    //        {
    //            animator.SetBool("Ismove", true);

    //            if (Vector3.Distance(transform.position, pathFinder.destination) > 5f)
    //            {
    //                Vector3 direct = pathFinder.steeringTarget - transform.position;
    //                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direct), 7f * Time.deltaTime);
    //            }
    //            else
    //            {
    //                Vector3 direct = pathFinder.destination - transform.position;
    //                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direct), 7f * Time.deltaTime);
    //            }
    //        }
    //    }
    //    else if (walkSound)
    //    {
    //        if (Vector3.SqrMagnitude(transform.position - pathFinder.destination) < 0.05f)
    //        {
    //            animator.SetBool("Ismove", false);
    //            if (goalTime <= 0.5f)
    //            {
    //                transform.localRotation *= Quaternion.Euler(new Vector3(0f, 2f, 0f));
    //            }
    //            else if (goalTime > 1f && goalTime <= 2f)
    //            {
    //                transform.localRotation *= Quaternion.Euler(new Vector3(0f, -2f, 0f));
    //            }
    //            else if (goalTime >= 2.5f)
    //            {
    //                walkSound = false;
    //                patrol = true;
    //                goalTime = 0f;
    //                pathFinder.SetDestination(searchPlace.position);
    //                return;
    //            }
    //            goalTime += Time.deltaTime;
    //        }
    //        else
    //        {
    //            animator.SetBool("Ismove", true);

    //            if (Vector3.Distance(transform.position, pathFinder.destination) > 5f)
    //            {
    //                Vector3 direct = pathFinder.steeringTarget - transform.position;
    //                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direct), 7f * Time.deltaTime);
    //            }
    //            else
    //            {
    //                Vector3 direct = pathFinder.destination - transform.position;
    //                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direct), 7f * Time.deltaTime);
    //            }
    //        }
    //        return;
    //    }
    //}
    
    [Serializable]
    public class SoundClip
    {
        public AudioClip shot;
        public AudioClip walk;
        public AudioClip scream;
        public AudioClip run;
        public AudioClip hit;
    }

    [System.Serializable]
    public struct Arsenal
    {
        public string name;
        public GameObject rightGun;
        public GameObject leftGun;
        public RuntimeAnimatorController controller;
    }
}
