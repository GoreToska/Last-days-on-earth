using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Zenject;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] private Rig _rifleRig;
    [SerializeField] private Rig _twoHandedMeleeRig;

    [Header("Animations")]
    [SerializeField] private AnimationClip _rifleReloadingAnimation;
    [SerializeField] private float _rifleReloadingAnimationOffset = 0.75f;

    [Inject] private PlayerMovementManager _playerMovementManager;

    private Animator _animator;

    private const string RifleLightShotTrigger = "RifleLightShot";
    private const string RifleMediumShotTrigger = "RifleMediumShot";
    private const string RifleHeavyShotTrigger = "RifleHeavyShot";
    private const string PistolMediumShotTrigger = "PistolMediumShot";
    private const string RifleWalkTrigger = "RifleWalk";
    private const string DefaultWalkTrigger = "DefaultWalk";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
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
        _animator.SetFloat("Speed", PlayerInputManager.Instance.MoveAmount, 0.05f, Time.deltaTime);
        _animator.SetFloat("HorizontalSpeed", _playerMovementManager.HorizontalSpeed, 0.02f, Time.deltaTime);
        _animator.SetFloat("VerticalSpeed", _playerMovementManager.VerticalSpeed, 0.02f, Time.deltaTime);
    }

    public void SetWeaponAnimationPattern(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Range_Primary:
                _animator.SetTrigger(RifleWalkTrigger);
                SetRifleRig();
                break;
            case WeaponType.Range_Secondary:
                _animator.SetTrigger(RifleWalkTrigger);
                break;
            case WeaponType.None:
                _animator.SetTrigger(DefaultWalkTrigger);
                PlayerInputManager.Instance.IsAiming = false;
                _animator.SetBool("IsAiming", false);
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
        _animator.SetBool("IsCrouching", value);

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
        _animator.SetBool("IsAiming", value);

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
        _animator.SetTrigger(RifleLightShotTrigger);
    }

    public void PlayRifleMediumShot()
    {
        _animator.SetTrigger(RifleMediumShotTrigger);
    }

    public void PlayRifleHeavyShot()
    {
        _animator.SetTrigger(RifleHeavyShotTrigger);
    }

    public void PlayPistolMediumShot()
    {
        _animator.SetTrigger(PistolMediumShotTrigger);
    }

    public void PlayReloadAnimation(string animationName)
    {
        StartCoroutine(PlayRifleReloadAnimationCoroutine(animationName));
    }

    public void PlayHeavyAttackAnimation()
    {
        _animator.Play("Stable Sword Inward Slash (1)");
    }

    private IEnumerator PlayRifleReloadAnimationCoroutine(string animationName)
    {
        SetDefaultRig();
        PlayerInputManager.Instance.DisableCombatControls();
        _animator.CrossFade(animationName, 0.1f);

        yield return new WaitForSeconds(_rifleReloadingAnimation.length - _rifleReloadingAnimationOffset);

        PlayerInputManager.Instance.EnableCombatControls();
        SetRifleRig();

        yield break;
    }

    public void SetRifleRig()
    {
        // other rigs too
        SetDefaultRig();
        _rifleRig.weight = 1f;
    }

    public void SetTwoHandedMeleeRig()
    {
        SetDefaultRig();
        //twoHandedMeleeRig.weight = 1f;
    }

    public void SetDefaultRig()
    {
        // other rigs too
        _twoHandedMeleeRig.weight = 0f;
        _rifleRig.weight = 0f;
    }
}
