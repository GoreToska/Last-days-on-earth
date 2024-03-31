using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Animator))]
public class FootstepsHandler : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] protected Transform _leftLeg;
    [SerializeField] protected Transform _rightLeg;

    [Inject] private ImpactManager _impactManager;

    public void PlayStepSound(string leg)
    {
        Vector3 startPoint;

        if (leg == "Left")
        {
            startPoint = _leftLeg.position;
        }
        else
        {
            startPoint = _rightLeg.position;
        }

        RaycastHit hit;

        Debug.DrawLine(startPoint, startPoint + Vector3.down, Color.red, 2f);
        if (Physics.Linecast(startPoint, startPoint + Vector3.down, out hit, _groundLayer))
        {
			_impactManager.HandleImpact(hit.collider.gameObject, hit.point, hit.normal, ImpactType.Footstep);
        }
    }
}
