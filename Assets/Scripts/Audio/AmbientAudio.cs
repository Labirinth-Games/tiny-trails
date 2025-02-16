using System.Collections.Generic;
using TinyTrails.Managers;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.Sounds
{
    public class AmbientAudio : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] AudioSource audioSource;

        [Header("Musics")]
        [SerializeField] AudioClip exploreAmbientAudio;
        [SerializeField] AudioClip battleAmbientAudio;

        public AmbientAudio Init()
        {
            return this;
        }

        public void Play(ContextGameType contextGameType)
        {
            audioSource.clip = contextGameType == ContextGameType.Explore ? exploreAmbientAudio : battleAmbientAudio;
            audioSource.loop = true;

            audioSource.Play();
        }
    }
}