using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
    [Serializable]
    public class SoundClip
    {
        public AudioClip shot;
        public AudioClip walk;
        public AudioClip scream;
        public AudioClip run;
        public AudioClip hit;
    }

    public SoundClip clip;
    public AudioSource sound;
    public Canvas escPanel;

    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(1).IsName("Damage1")
        ||animator.GetCurrentAnimatorStateInfo(1).IsName("Damage2")
        || animator.GetCurrentAnimatorStateInfo(1).IsName("Damage3"))
        {
            sound.clip = clip.scream;
            if (!sound.isPlaying)
                sound.Play();
            sound.volume = 0.7f;
            sound.pitch = 1f;
            return;
        }
        //else
        //{
        //    sound.clip = null;
        //}

        //sound.clip = clip.scream;
        //sound.maxDistance = 10f;
        if (escPanel.enabled)
        {
            sound.clip = null;
            return;
        }

        if (Input.GetMouseButton(0))
        {
            sound.clip = clip.shot;
            if (!sound.isPlaying)
                sound.Play();

            sound.volume = 0.1f;
            sound.pitch = 3f;
            return;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            sound.clip = null;
        }

        if((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) 
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("Aim_Jump"))
        {
            sound.clip = clip.walk;
            if (!sound.isPlaying)
                sound.Play();

            sound.volume = 1;

            if (Input.GetKey(KeyCode.LeftShift))
                sound.pitch = 1.75f;
            else if (animator.GetBool("Squat"))
                sound.pitch = 0.7f;
            else
                sound.pitch = 1.5f;
        }
        else if (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0)
        {
            sound.clip = null;
        }

    }
}
