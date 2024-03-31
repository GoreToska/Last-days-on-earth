using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SceneManagerInstaller : MonoInstaller
{
	[SerializeField] private GameSceneManager _gameSceneManager;

	public override void InstallBindings()
	{
		Container.BindInstance(_gameSceneManager).AsSingle().NonLazy();
	}
}
