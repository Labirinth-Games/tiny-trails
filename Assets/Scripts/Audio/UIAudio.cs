using System.Collections.Generic;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.Sounds
{
    public class UIAudio : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] AudioSource audioSource;

        [Header("Musics")]
        [SerializeField] List<AudioClip> outResourcesAudio;

        public UIAudio Init()
        {
            return this;
        }

        public void OutResources()
        {
            audioSource.clip = outResourcesAudio[Random.Range(0, outResourcesAudio.Count)];
            audioSource.Play();
        }
    }
}