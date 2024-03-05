using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingProgressBar : MonoBehaviour
{
	[SerializeField] private Image _progressImage;

	private void Awake()
	{
		_progressImage.fillAmount = 0;
	}

	private void Update()
	{
		_progressImage.fillAmount = GameSceneManager.Instance.LoadingProgress;
	}
}
