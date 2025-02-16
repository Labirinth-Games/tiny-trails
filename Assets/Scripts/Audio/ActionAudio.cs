using System.Collections.Generic;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.Sounds
{
    public class ActionAudio : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] AudioSource audioSource;

        [Header("Musics")]
        [SerializeField] List<AudioClip> hitClip;
        [SerializeField] List<AudioClip> footStepClips;

        public ActionAudio Init()
        {
            return this;
        }

        public void Hit()
        {
            audioSource.clip = hitClip[Random.Range(0, hitClip.Count)];
            audioSource.Play();
        }

        public void PlayerMove()
        {
            audioSource.clip = footStepClips[Random.Range(0, footStepClips.Count)];
            audioSource.Play();
        }
    }
}