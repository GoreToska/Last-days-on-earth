using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationManager : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        animator.SetFloat("Speed", PlayerInputManager.Instance.MoveAmount, 0.1f, Time.deltaTime);
        animator.SetBool("IsCrouching", PlayerInputManager.Instance.IsCrouching);
    }
}
