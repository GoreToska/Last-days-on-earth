using System;
using System.Linq;
using UnityEngine;

public class NoiseManager : MonoBehaviour
{
	[field: HideInInspector] public static NoiseManager Instance { get; private set; }
	[SerializeField] private LayerMask _targetLayers;
	private Collider[] _targets = new Collider[20];

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Debug.Log("Only one NoiseManager can be in this scene.");
			Destroy(gameObject);
		}
	}

	public void MakeNouse(float radius, GameObject noiseTarget)
	{
		Array.Clear(_targets, 0, 20);
		Physics.OverlapSphereNonAlloc(noiseTarget.transform.position, radius, _targets, _targetLayers);
		
		if (_targets[0] == null)
		{
			return;
		}

		foreach (Collider target in _targets)
		{
			if (target.transform.root.TryGetComponent<BaseAIAgent>(out var component))
			{
				component.AddTarget(noiseTarget, 100f);
			}
		}
	}
}
