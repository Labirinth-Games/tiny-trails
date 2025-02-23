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
        [SerializeField] List<AmbientAudioSettings> ambientAudioSettings;

        public AmbientAudio Init()
        {
            return this;
        }

        public void Play(ContextGameType contextGameType)
        {
            audioSource.clip = ambientAudioSettings.Find(f => f.audioType == contextGameType).audioClip;
            audioSource.loop = true;

            audioSource.Play();
        }
    }

    [System.Serializable]
    public struct AmbientAudioSettings {
        public ContextGameType audioType;
        public AudioClip audioClip;
    }
}