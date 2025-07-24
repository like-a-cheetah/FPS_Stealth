using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    [Serializable]
    public class SoundClip
    {
        public AudioClip shot;
        public AudioClip walk;
        public AudioClip scream;
        public AudioClip run;
    }
    
    void Update()
    {
        
    }
}
