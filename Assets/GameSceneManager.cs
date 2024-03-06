using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
	[HideInInspector] public static GameSceneManager Instance;

	[field: SerializeField] public int LoadingScene { get; private set; } = 0;
	[field: SerializeField] public int MainMenuScene { get; private set; } = 1;
	[field: SerializeField] public int GameplayScene { get; private set; } = 2;

	public static event UnityAction OnSceneLoadingStarted;
	public static event UnityAction OnSceneLoadingFinished;

	private AsyncOperation loadingSceneOperation;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	private void OnEnable()
	{
		//loadingSceneOperation.completed += i => OnSceneLoadingFinished?.Invoke();
	}

	private void OnDisable()
	{
		//loadingSceneOperation.completed -= i => OnSceneLoadingFinished.Invoke();
	}

	public void LoadScene(int index)
	{
		loadingSceneOperation = SceneManager.LoadSceneAsync(LoadingScene);
		loadingSceneOperation.completed += i => loadingSceneOperation = SceneManager.LoadSceneAsync(index);
		//loadingSceneOperation.allowSceneActivation = false;
	}

	public void EndLoadingAnimation()
	{
		loadingSceneOperation.allowSceneActivation = true;
	}

	public void LoadMainMenuScene()
	{
		LoadScene(MainMenuScene);
	}

	public void LoadGameplayScene()
	{
		LoadScene(GameplayScene);
	}

	public float LoadingProgress { get { return loadingSceneOperation.progress; } }
}
