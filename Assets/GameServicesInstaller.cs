using GoreToska;
using UnityEngine;
using Zenject;

public class GameServicesInstaller : MonoInstaller
{
	[HideInInspector] public static GameServicesInstaller Instance;

	[SerializeField] private PlayerMovementManager _playerMovement;
	[SerializeField] private PlayerAnimationManager _playerAnimationManager;
	[SerializeField] private ImpactManager _impactManager;
	[SerializeField] private NoiseManager _noiseManager;
	[SerializeField] private SFXManager _sfxManager;
	[SerializeField] private HUDManager _hudManager;
	[SerializeField] private DescriptionManager _descriptionManager;
	[SerializeField] private HotbarDisplay _hotbarDisplay;

	private void Awake()
	{
		Instance = this;
	}

	private void OnDestroy()
	{
		Instance = null;
	}

	public override void InstallBindings()
	{
		// Player
		Container.BindInstance(_playerMovement).AsSingle().NonLazy();
		Container.BindInstance(_playerAnimationManager).AsSingle().NonLazy();

		// Impacts
		Container.BindInstance(_impactManager).AsSingle().NonLazy();

		// Noise 
		Container.BindInstance(_noiseManager).AsSingle().NonLazy();

		// SFX and music
		Container.BindInstance(_sfxManager).AsSingle().NonLazy();

		//UI
		Container.BindInstance(_hudManager).AsSingle().NonLazy();
		Container.BindInstance(_descriptionManager).AsSingle().NonLazy();
		Container.BindInstance(_hotbarDisplay).AsSingle().NonLazy();
	}

	public PlayerMovementManager PlayerMovementManager { get { return _playerMovement; } }
	public PlayerAnimationManager PlayerAnimationManager { get { return _playerAnimationManager; } }
	public ImpactManager ImpactManager { get { return _impactManager; } }
	public NoiseManager NoiseManager { get { return _noiseManager; } }
	public SFXManager SFXManager { get { return _sfxManager; } }
	public HUDManager HUDManager { get { return _hudManager; } }
	public DescriptionManager DescriptionManager { get { return _descriptionManager; } }
	public HotbarDisplay HotbarDisplay { get { return _hotbarDisplay; } }
}
