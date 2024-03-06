using System.Collections;
using UnityEngine;

namespace GoreToska
{
    [RequireComponent(typeof(AudioSource))]
    public class PoolableSFX : PoolableObject
    {
        private AudioSource _audioSource;

        public override void Awake()
        {
            base.Awake();

            _audioSource = GetComponent<AudioSource>();
        }

        public void SetSound(AudioClip audioClip, int minDistance, int maxDistance)
        {
            _audioSource.minDistance = minDistance;
            _audioSource.maxDistance = maxDistance;
            _audioSource.PlayOneShot(audioClip);
            StartCoroutine(DisableOnEndCoroutine(audioClip.length));
        }

        public void SetSound(AudioClip audioClip, int maxDistance)
        {
            _audioSource.maxDistance = maxDistance;
            _audioSource.PlayOneShot(audioClip);
            StartCoroutine(DisableOnEndCoroutine(audioClip.length));
        }

        public void SetPitch(float pitch)
        {
            _audioSource.pitch = pitch;
        }

        public void SetVolume(float volume)
        {
            _audioSource.volume = volume;
        }
    }
}
