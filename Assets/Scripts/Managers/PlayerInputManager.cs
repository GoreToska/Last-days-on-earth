using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public interface IInputController
{
    public void DisablePlayerControls();
    public void EnablePlayerControls();
    public void EnableInventoryControls();
    public void DisableInventoryControls();
}

//  Summary
//  This class is reading input from the input devices
//  Summary
[RequireComponent(typeof(PlayerMovementManager))]
public class PlayerInputManager : MonoBehaviour, IInputController
{
    [HideInInspector] public static PlayerInputManager Instance;

    private PlayerInput _playerInput;
    private PlayerStatusManager _playerStatusManager;
    private Camera _camera;

    private Transform _crosshair;

    //  Movement
    private Vector2 _movement;
    private float _verticalInput;
    private float _horizontalInput;
    private float _moveAmount;

    //  Sprint
    private bool _isSprinting = false;

    //  Crouch
    private bool _isCrouching = false;
    public static event UnityAction<bool> ToggleCrouch = delegate { };

    //  Mouse scroll
    public static event UnityAction<float> MouseScroll = delegate { };
    private float scroll;

    // Camera changing
    public static event UnityAction NextCamera = delegate { };
    public static event UnityAction PreviousCamera = delegate { };

    // Mouse scroll with tab pressed
    public static event UnityAction<float> ChangeInteractable = delegate { };

    //  Aiming
    private bool _isAiming = false;
    [field: SerializeField] public LayerMask AimMask { get; private set; }
    public RaycastHit hitInfo;

    public bool IsShooting = false;

    public static event UnityAction AttackEvent = delegate { };

    //  Inventory
    public static event UnityAction OpenInventoryEvent = delegate { };
    public static event UnityAction AlternativeCloseInventoryEvent = delegate { };
    public static event UnityAction CloseInventoryEvent = delegate { };
    public static event UnityAction RotateItem = delegate { };

    //  Pickup
    public static event UnityAction PickUpEvent = delegate { };

    // reload
    public static event UnityAction ReloadEvent = delegate { };

    // Slots
    public static event UnityAction<int> Hotbar1Event = delegate { };
    public static event UnityAction<int> Hotbar2Event = delegate { };
    public static event UnityAction<int> Hotbar3Event = delegate { };
    public static event UnityAction<int> Hotbar4Event = delegate { };
    public static event UnityAction<int> Hotbar5Event = delegate { };

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

        _camera = Camera.main;
        _crosshair = GameObject.FindGameObjectWithTag("Crosshair").transform;
        _playerStatusManager = GetComponent<PlayerStatusManager>();
    }

    private void OnEnable()
    {
        if (_playerInput == null)
        {
            _playerInput = new PlayerInput();

            //  All the input actions should be evented here
            _playerInput.PlayerMovement.Movement.performed += i => _movement = i.ReadValue<Vector2>();
            _playerInput.PlayerMovement.Movement.canceled += i => _movement = i.ReadValue<Vector2>();

            _playerInput.CameraMovement.Zoom.performed += i =>
            {
                if (!_playerInput.PlayerActions.SwitchInteractable.IsInProgress())
                {
                    scroll = i.ReadValue<float>();
                    MouseScroll?.Invoke(i.ReadValue<float>());
                }
            };

            _playerInput.CameraMovement.NextCamera.performed += i => NextCamera?.Invoke();
            _playerInput.CameraMovement.PreviousCamera.performed += i => PreviousCamera?.Invoke();

            _playerInput.PlayerActions.SwitchInteractable.performed += i => ChangeInteractable?.Invoke(i.ReadValue<float>());

            _playerInput.PlayerMovement.Sprint.performed += i => _isSprinting = true;
            _playerInput.PlayerMovement.Sprint.canceled += i => _isSprinting = false;

            _playerInput.PlayerActions.Crouch.performed += i => _isCrouching = !_isCrouching;
            _playerInput.PlayerActions.Crouch.performed += i => ToggleCrouch?.Invoke(_isCrouching);

            _playerInput.PlayerCombat.Aim.performed += i => _isAiming = !_isAiming;

            _playerInput.PlayerCombat.Attack.performed += i => IsShooting = true;
            _playerInput.PlayerCombat.Attack.canceled += i => IsShooting = false;
            _playerInput.PlayerCombat.Attack.performed += i => AttackEvent?.Invoke();

            _playerInput.PlayerActions.OpenInventory.performed += i => OpenInventoryEvent?.Invoke();
            _playerInput.PlayerActions.CloseInventory.performed += i => AlternativeCloseInventoryEvent?.Invoke();
            _playerInput.Inventory.CloseInventory.performed += i => CloseInventoryEvent?.Invoke();
            _playerInput.Inventory.Rotate.performed += i => RotateItem?.Invoke();

            OpenInventoryEvent += () => { DisablePlayerControls(); EnableInventoryControls(); };
            CloseInventoryEvent += () => { EnablePlayerControls(); DisableInventoryControls(); };
            AlternativeCloseInventoryEvent += () => { EnablePlayerControls(); DisableInventoryControls(); };

            _playerInput.PlayerActions.PickUp.performed += i => PickUpEvent?.Invoke();

            _playerInput.PlayerActions.Reload.performed += i => ReloadEvent?.Invoke();

            _playerInput.PlayerActions.Hotbar1.performed += i => Hotbar1Event?.Invoke(0);
            _playerInput.PlayerActions.Hotbar2.performed += i => Hotbar2Event?.Invoke(1);
            _playerInput.PlayerActions.Hotbar3.performed += i => Hotbar3Event?.Invoke(2);
            _playerInput.PlayerActions.Hotbar4.performed += i => Hotbar4Event?.Invoke(3);
            _playerInput.PlayerActions.Hotbar5.performed += i => Hotbar5Event?.Invoke(4);
        }

        EnablePlayerControls();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void Update()
    {
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        _verticalInput = _movement.y;
        _horizontalInput = _movement.x;

        //  absolute movement amount
        _moveAmount = Mathf.Clamp01(Mathf.Abs(_verticalInput) + Mathf.Abs(_horizontalInput));

        if (_isSprinting && _playerStatusManager.stamina > 1)
        {
            _moveAmount *= 2;
        }
        else
        {
            _isSprinting = false;
        }
    }

    public (bool success, Vector3 position) GetMousePosition()
    {
        var ray = _camera.ScreenPointToRay(Mouse.current.position.value);

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, AimMask))
        {
            _crosshair.transform.position = hitInfo.point;
            //  If raycact hit something, return true and hit point
            return (success: true, position: hitInfo.point);
        }
        else
        {
            //  If raycast didn't hit anything, return false and zero vector
            return (success: false, position: Vector3.zero);
        }
    }

    public void DisableInput()
    {
        _playerInput.Disable();
    }

    public void EnableInput()
    {
        _playerInput.Enable();
    }

    public void DisablePlayerControls()
    {
        _playerInput.PlayerMovement.Disable();
        _playerInput.PlayerCombat.Disable();
        _playerInput.PlayerActions.Disable();
        _playerInput.CameraMovement.Disable();

        _movement = Vector2.zero;
    }

    public void EnablePlayerControls()
    {
        _playerInput.PlayerMovement.Enable();
        _playerInput.PlayerCombat.Enable();
        _playerInput.PlayerActions.Enable();
        _playerInput.CameraMovement.Enable();
    }

    public void DisableInventoryControls()
    {
        _playerInput.Inventory.Disable();
    }

    public void EnableInventoryControls()
    {
        _playerInput.Inventory.Enable();
    }

    public void DisableCombatControls()
    {
        _isAiming = false;
        _playerInput.PlayerCombat.Disable();
    }

    public void EnableCombatControls()
    {
        _playerInput.PlayerCombat.Enable();
    }

    public float VerticalInput { get { return _verticalInput; } }

    public float HorizontalInput { get { return _horizontalInput; } }

    public float MoveAmount { get { return _moveAmount; } }

    public float Scroll { get { return scroll; } }

    public bool IsSprinting { get { return _isSprinting; } }

    public bool IsCrouching { get { return _isCrouching; } set { _isCrouching = value; } }

    public bool IsAiming { get { return PlayerEquipment.Instance._currentRangeWeapon != null? _isAiming : false; } set { _isAiming = value; } }
}
