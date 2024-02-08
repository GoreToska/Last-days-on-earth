using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FootstepsHandler : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] protected Transform _leftLeg;
    [SerializeField] protected Transform _rightLeg;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

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
            ImpactManager.Instance.HandleImpact(hit.collider.gameObject, hit.point, hit.normal, ImpactType.Footstep);
        }
    }
}
