using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LoseGameMenu : MonoBehaviour
{
    [SerializeField] private Canvas _deadCanvas;
	[SerializeField] private Volume _deadVolume;
	[SerializeField] private Canvas _HUD;
	[SerializeField] private Canvas _menuCanvas;
	[SerializeField] private Volume _menuVolume;

	private void Awake()
	{
		HideLoseCanvas();
	}

	private void OnEnable()
	{
		PlayerStatusManager.DeathEvent += ShowLoseCanvas;
	}

	private void OnDisable()
	{
		PlayerStatusManager.DeathEvent -= ShowLoseCanvas;
	}

	public void ShowLoseCanvas()
	{
		_HUD.enabled = false;
		_menuCanvas.enabled = false;
		_menuVolume.weight = 0f;
		_deadCanvas.enabled = true;
		_deadVolume.enabled = true;
		_deadVolume.weight = 1f;
	}

	public void HideLoseCanvas()
	{
		_HUD.enabled = true;
		_menuCanvas.enabled = false;
		_menuVolume.weight = 0f;
		_deadCanvas.enabled = false;
		_deadVolume.enabled = false;
		_deadVolume.weight = 0f;
	}
}
