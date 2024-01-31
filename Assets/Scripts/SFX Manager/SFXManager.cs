using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoreToska
{
    public class SFXManager : MonoBehaviour
    {
        [HideInInspector] public static SFXManager Instance;

        [SerializeField] private PoolableSFX _sfxPrefab;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogAssertion("Only one SFXManager can be in this scene.");
                Destroy(gameObject);
            }
        }

        public void PlaySoundEffect(Vector3 point, AudioClip audioClip, int minDistance, int maxDistance, bool randomPitch = true)
        {
            ObjectPool particlePool = ObjectPool.CreateInstance(_sfxPrefab, 50);
            PoolableSFX instance = particlePool.GetObject(point, Quaternion.identity) as PoolableSFX;
            instance.SetSound(audioClip, minDistance, maxDistance);

            if(randomPitch )
            {
                instance.SetPitch(Random.Range(0.75f, 1.25f));
            }
        }

        public void PlaySoundEffect(Vector3 point, AudioClip audioClip, int maxDistance, bool randomPitch = true)
        {
            ObjectPool particlePool = ObjectPool.CreateInstance(_sfxPrefab, 50);
            PoolableSFX instance = particlePool.GetObject(point, Quaternion.identity) as PoolableSFX;
            instance.SetSound(audioClip, maxDistance);

            if (randomPitch)
            {
                instance.SetPitch(Random.Range(0.75f, 1.25f));
            }
        }
    }
}

