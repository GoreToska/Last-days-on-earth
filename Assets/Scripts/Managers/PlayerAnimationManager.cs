using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationManager : MonoBehaviour
{
    [HideInInspector] public static PlayerAnimationManager Instance;

    [SerializeField] private Rig rifleRig;
    [SerializeField] private Rig twoHandedMeleeRig;

    [Header("Animations")]
    [SerializeField] private AnimationClip rifleReloadingAnimation;
    [SerializeField] private float rifleReloadingAnimationOffset = 0.75f;

    private Animator animator;

    private const string RifleLightShotTrigger = "RifleLightShot";
    private const string RifleMediumShotTrigger = "RifleMediumShot";
    private const string RifleHeavyShotTrigger = "RifleHeavyShot";
    private const string PistolMediumShotTrigger = "PistolMediumShot";
    private const string RifleWalkTrigger = "RifleWalk";
    private const string DefaultWalkTrigger = "DefaultWalk";

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

        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (PlayerEquipment.Instance.GetCurrentWeapon() == null)
        {
            SetWeaponAnimationPattern(WeaponType.None);
            return;
        }

        if (PlayerEquipment.Instance.GetCurrentWeapon())
            SetWeaponAnimationPattern(PlayerEquipment.Instance.GetCurrentWeapon().WeaponData.WeaponType);
    }

    private void OnEnable()
    {
        PlayerInputManager.ToggleCrouch += CrouchAnimationHandler;
        PlayerInputManager.ToggleAim += AimAnimationHandler;
    }

    private void OnDisable()
    {
        PlayerInputManager.ToggleCrouch -= CrouchAnimationHandler;
        PlayerInputManager.ToggleAim -= AimAnimationHandler;
    }

    private void Update()
    {
        // change update to events
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        animator.SetFloat("Speed", PlayerInputManager.Instance.MoveAmount, 0.1f, Time.deltaTime);
        animator.SetFloat("HorizontalSpeed", PlayerMovementManager.Instance.HorizontalSpeed, 0.1f, Time.deltaTime);
        animator.SetFloat("VerticalSpeed", PlayerMovementManager.Instance.VerticalSpeed, 0.1f, Time.deltaTime);
    }

    public void SetWeaponAnimationPattern(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Range_Primary:
                animator.SetTrigger(RifleWalkTrigger);
                SetRifleRig();
                break;
            case WeaponType.Range_Secondary:
                animator.SetTrigger(RifleWalkTrigger);
                break;
            case WeaponType.None:
                animator.SetTrigger(DefaultWalkTrigger);
                PlayerInputManager.Instance.IsAiming = false;
                animator.SetBool("IsAiming", false);
                SetDefaultRig();
                break;
            case WeaponType.Melee_Primary:
                //animator.CrossFade("Walk Rifle Blend Tree", 0.1f);
                SetTwoHandedMeleeRig();
                //animator.CrossFade("Melee_2hand_Idle_01", 0.1f);
                break;
            case WeaponType.Melee_Secondary:
                break;
            default:
                break;
        }
    }

    public void CrouchAnimationHandler(bool value)
    {
        animator.SetBool("IsCrouching", value);

        if (value)
        {
            SetWeaponAnimationPattern(WeaponType.None);
        }
        else
        {
            SetWeaponAnimationPattern();
        }
    }

    public void AimAnimationHandler(bool value)
    {
        animator.SetBool("IsAiming", value);

        if(value)
        {

        }
        else
        {
            SetWeaponAnimationPattern();
        }
    }

    private void SetWeaponAnimationPattern()
    {
        if (PlayerEquipment.Instance.GetCurrentWeapon())
            SetWeaponAnimationPattern(PlayerEquipment.Instance.GetCurrentWeapon().WeaponData.WeaponType);
        else
        {
            SetWeaponAnimationPattern(WeaponType.None);
        }
    }

    public void PlayRifleLightShot()
    {
        animator.SetTrigger(RifleLightShotTrigger);
    }

    public void PlayRifleMediumShot()
    {
        animator.SetTrigger(RifleMediumShotTrigger);
    }

    public void PlayRifleHeavyShot()
    {
        animator.SetTrigger(RifleHeavyShotTrigger);
    }

    public void PlayPistolMediumShot()
    {
        animator.SetTrigger(PistolMediumShotTrigger);
    }

    public void PlayReloadAnimation(string animationName)
    {
        StartCoroutine(PlayRifleReloadAnimationCoroutine(animationName));
    }

    public void PlayHeavyAttackAnimation()
    {
        animator.Play("Stable Sword Inward Slash (1)");
    }

    private IEnumerator PlayRifleReloadAnimationCoroutine(string animationName)
    {
        SetDefaultRig();
        PlayerInputManager.Instance.DisableCombatControls();
        animator.CrossFade(animationName, 0.1f);

        yield return new WaitForSeconds(rifleReloadingAnimation.length - rifleReloadingAnimationOffset);

        PlayerInputManager.Instance.EnableCombatControls();
        SetRifleRig();

        yield break;
    }

    public void SetRifleRig()
    {
        // other rigs too
        SetDefaultRig();
        rifleRig.weight = 1f;
    }

    public void SetTwoHandedMeleeRig()
    {
        SetDefaultRig();
        //twoHandedMeleeRig.weight = 1f;
    }

    public void SetDefaultRig()
    {
        // other rigs too
        twoHandedMeleeRig.weight = 0f;
        rifleRig.weight = 0f;
    }
}
