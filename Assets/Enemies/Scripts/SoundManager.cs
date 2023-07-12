using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Enemies
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        public AudioClip[] sounds;

        private AudioSource source;

        private void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        public void PlayRandomSound()
        {
            source.clip = sounds[(int)Random.Range(0, sounds.Length)];
            source.Play();
        }
    }


}