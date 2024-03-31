using System;
using System.Linq;
using UnityEngine;

public class NoiseManager : MonoBehaviour
{
	[SerializeField] private LayerMask _targetLayers;
	private Collider[] _targets = new Collider[30];

	public void MakeNouse(float radius, GameObject noiseTarget)
	{
		Array.Clear(_targets, 0, _targets.Length);
		Physics.OverlapSphereNonAlloc(noiseTarget.transform.position, radius, _targets, _targetLayers);
		
		if (_targets[0] == null)
		{
			return;
		}

		foreach (Collider target in _targets)
		{
			if(target == null) continue;

			if (target.transform.root.TryGetComponent<BaseAIAgent>(out var component))
			{
				component.AddTarget(noiseTarget, 100f);
			}
		}
	}
}
