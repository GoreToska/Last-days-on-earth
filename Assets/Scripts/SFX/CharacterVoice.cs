using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CharacterVoice : MonoBehaviour
{
	[SerializeField] private List<AudioClip> _idleVoice;
	[SerializeField] private List<AudioClip> _agroVoice;
	[SerializeField] private List<AudioClip> _deadVoice;
	[SerializeField] private List<AudioClip> _painEffectVoice;
	[SerializeField] private float _idleVoiceTime;

	private IDamagable _damagable;
	private AudioSource _audioSource;

	private void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
		_damagable = GetComponent<IDamagable>();
		StartCoroutine(WaitCoroutine(_idleVoiceTime));
	}

	private void OnEnable()
	{
		_damagable.OnDeath += PlayDeadVoice;
	}

	private void OnDisable()
	{
		_damagable.OnDeath -= PlayDeadVoice;
		StopAllCoroutines();
	}

	public void PlayIdleVoice()
	{
		_audioSource.pitch = Random.Range(0.8f, 1.1f);
		_audioSource.PlayOneShot(_idleVoice[Random.Range(0, _idleVoice.Count)]);
	}

	public void PlayDeadVoice()
	{
		_audioSource.pitch = Random.Range(0.8f, 1.1f);
		_audioSource.PlayOneShot(_deadVoice[Random.Range(0, _deadVoice.Count)]);
	}

	private IEnumerator WaitCoroutine(float delay)
	{
		while (!_damagable.IsDead)
		{
			yield return new WaitForSeconds(delay + delay * Random.Range(0, 4));

			if(!_damagable.IsDead)
				PlayIdleVoice();
		}

		yield break;
	}
}
