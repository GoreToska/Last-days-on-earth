using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WeaponActionSFX : MonoBehaviour
{
    [SerializeField] private WeaponData _weaponData;

    private AudioSource _audioSource;

	private void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	public void PlayMagazineOutSFX()
    {
		_audioSource.pitch = 1f;

		_audioSource.PlayOneShot(_weaponData.WeaponSFXConfig.MagazineOutSound);
	}

    public void PlayMagazineInSFX()
    {
		_audioSource.pitch = 1f;

		_audioSource.PlayOneShot(_weaponData.WeaponSFXConfig.MagazineInSound);
	}

	public void PlayChamberSFX()
    {
		_audioSource.pitch = 1f;

		_audioSource.PlayOneShot(_weaponData.WeaponSFXConfig.ChamberSound);
	}
}
