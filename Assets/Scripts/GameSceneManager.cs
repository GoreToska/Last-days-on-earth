using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
	[field: SerializeField] public int LoadingScene { get; private set; } = 0;
	[field: SerializeField] public int MainMenuScene { get; private set; } = 1;
	[field: SerializeField] public int GameplayScene { get; private set; } = 2;

	public static event UnityAction OnSceneLoadingStarted;
	public static event UnityAction OnSceneLoadingFinished;

	private static AsyncOperation loadingSceneOperation;

	public void LoadScene(int index)
	{
		loadingSceneOperation = SceneManager.LoadSceneAsync(LoadingScene);
		loadingSceneOperation.completed += i => loadingSceneOperation = SceneManager.LoadSceneAsync(index);
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
