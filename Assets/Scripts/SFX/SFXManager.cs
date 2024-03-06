using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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

		public void PlaySoundEffect(Vector3 point, AudioClip audioClip, int minDistance, int maxDistance, float volume = 1f, bool randomPitch = true)
		{
			ObjectPool pool = ObjectPool.CreateInstance(_sfxPrefab, 10);
			PoolableSFX instance = pool.GetObject(point, Quaternion.identity) as PoolableSFX;
			instance.SetSound(audioClip, minDistance, maxDistance);

			if (randomPitch)
			{
				instance.SetPitch(Random.Range(0.75f, 1.25f));
			}

			if (volume != 1f)
			{
				instance.SetVolume(volume);
			}
		}

		public void PlaySoundEffect(Vector3 point, AudioClip audioClip, int maxDistance, float volume = 1f, bool randomPitch = true)
		{
			ObjectPool pool = ObjectPool.CreateInstance(_sfxPrefab, 10);
			PoolableSFX instance = pool.GetObject(point, Quaternion.identity) as PoolableSFX;
			instance.SetSound(audioClip, maxDistance);
			instance.SetVolume(volume);

			if (randomPitch)
			{
				instance.SetPitch(Random.Range(0.75f, 1.25f));
			}
		}
	}
}

