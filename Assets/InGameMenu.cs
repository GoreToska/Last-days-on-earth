using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class InGameMenu : MonoBehaviour
{
	[SerializeField] private Volume _menuVolume;
	[SerializeField] private Canvas _menuCanvas;
	[SerializeField] private CanvasGroup _menuCanvasGroup;
	[SerializeField] private Canvas _playerHUD;

	private void Awake()
	{
		_menuCanvas.enabled = false;
		_menuCanvasGroup.alpha = 0f;
	}

	private void OnEnable()
	{
		PlayerInputManager.MenuOpen += OpenMenu;
		PlayerInputManager.MenuClose += CloseMenu;
	}

	private void OnDisable()
	{
		PlayerInputManager.MenuOpen -= OpenMenu;
		PlayerInputManager.MenuClose -= CloseMenu;
	}

	public void OpenMenu()
	{
		_menuCanvas.enabled = true;
		_menuCanvasGroup.alpha = 1f;
		_menuVolume.weight = 1f;
		_playerHUD.enabled = false;

		PlayerInputManager.Instance.DisablePlayerControls();
		PlayerInputManager.Instance.EnableMenuControls();
	}

	public void CloseMenu()
	{
		_menuCanvas.enabled = false;
		_menuCanvasGroup.alpha = 0f;
		_menuVolume.weight = 0f;
		_playerHUD.enabled = true;

		PlayerInputManager.Instance.DisableMenuControls();
		PlayerInputManager.Instance.EnablePlayerControls();
	}
}
