using System.Collections.Generic;
using TinyTrails.Sounds;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.Managers
{
    public class AudioManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] AudioSource audioSource;
        [SerializeField] AmbientAudio ambientAudio;
        [SerializeField] ActionAudio actionAudio;
        [SerializeField] UIAudio uiAudio;

        public AmbientAudio AmbientAudio { get; private set; }
        public ActionAudio ActionAudio { get; private set; }
        public UIAudio UIAudio { get; private set; }

        public void Init()
        {
            AmbientAudio = ambientAudio.Init();
            ActionAudio = actionAudio.Init();
            UIAudio = uiAudio.Init();

            // play awake
            AmbientAudio.Play(ContextGameType.Explore);

            // subsicribers
            GameManager.Instance.EventManager.Subscriber<ContextGameType>(EventChannelType.OnContextGameChangeStatus, AmbientAudio.Play);
        }

        public void Play(AudioClip clip)
        {
            audioSource.clip = clip;
            audioSource.loop = false;
            audioSource.Play();
        }
    }
}