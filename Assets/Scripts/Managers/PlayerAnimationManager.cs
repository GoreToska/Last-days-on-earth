using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationManager : MonoBehaviour
{
    [HideInInspector] public static PlayerAnimationManager Instance;

    [SerializeField] private Rig rifleRig;

    private Animator animator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

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
        animator.SetBool("IsAiming", PlayerInputManager.Instance.IsAiming);
        animator.SetFloat("HorizontalSpeed", PlayerMovementManager.Instance.HorizontalSpeed, 0.1f, Time.deltaTime);
        animator.SetFloat("VerticalSpeed", PlayerMovementManager.Instance.VerticalSpeed, 0.1f, Time.deltaTime);
    }

    public void SetWeaponAnimationPattern(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Primary:
                animator.Play("Walk Rifle Blend Tree", 0);
                SetRifleRig();
                break;
            case WeaponType.Secondary:
                break;
            case WeaponType.None:
                animator.Play("Walk Blend Tree", 0);
                SetDefaultRig();
                break;
            default:
                break;
        }
    }

    private void SetRifleRig()
    {
        // other rigs too
        rifleRig.weight = 1f;
    }

    private void SetDefaultRig()
    {
        // other rigs too
        rifleRig.weight = 0f;
    }
}
