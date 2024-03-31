using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LoadingProgressBar : MonoBehaviour
{
	[Inject] private GameSceneManager _gameSceneManager;
	[SerializeField] private Image _progressImage;

	private void Awake()
	{
		_progressImage.fillAmount = 0;
	}

	private void Update()
	{
		_progressImage.fillAmount = _gameSceneManager.LoadingProgress;
	}
}
