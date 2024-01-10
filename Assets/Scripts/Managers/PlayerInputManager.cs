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

    private PlayerInput playerInput;
    private PlayerStatusManager playerStatusManager;
    private Camera camera;

    private Transform crosshair;

    //  Movement
    private Vector2 movement;
    private float verticalInput;
    private float horizontalInput;
    private float moveAmount;

    //  Sprint
    private bool isSprinting = false;

    //  Crouch
    private bool isCrouching = false;

    //  Mouse scroll
    private float scroll;

    //  Aiming
    private bool isAiming = false;
    [SerializeField] public LayerMask aimMask;
    public RaycastHit hitInfo;

    public bool isShooting = false;
    public event UnityAction AttackEvent = delegate { };

    //  Inventory
    public event UnityAction OpenInventoryEvent = delegate { };
    public event UnityAction AlternativeCloseInventoryEvent = delegate { };
    public event UnityAction CloseInventoryEvent = delegate { };
    public event UnityAction RotateItem = delegate { };

    //  Pickup
    public event UnityAction PickUpEvent = delegate { };

    // reload
    public event UnityAction ReloadEvent = delegate { };

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

        camera = Camera.main;
        crosshair = GameObject.FindGameObjectWithTag("Crosshair").transform;
        playerStatusManager = GetComponent<PlayerStatusManager>();
    }

    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = new PlayerInput();

            //  All the input actions should be evented here
            playerInput.PlayerMovement.Movement.performed += i => movement = i.ReadValue<Vector2>();
            playerInput.PlayerMovement.Movement.canceled += i => movement = i.ReadValue<Vector2>();

            playerInput.CameraMovement.Zoom.performed += i => scroll = i.ReadValue<float>();

            playerInput.PlayerMovement.Sprint.performed += i => isSprinting = true;
            playerInput.PlayerMovement.Sprint.canceled += i => isSprinting = false;

            playerInput.PlayerActions.Crouch.performed += i => isCrouching = !isCrouching;

            playerInput.PlayerCombat.Aim.performed += i => isAiming = !isAiming;

            playerInput.PlayerCombat.Attack.performed += i => isShooting = true;
            playerInput.PlayerCombat.Attack.canceled += i => isShooting = false;
            playerInput.PlayerCombat.Attack.performed += i => AttackEvent?.Invoke();

            playerInput.PlayerActions.OpenInventory.performed += i => OpenInventoryEvent?.Invoke();
            playerInput.PlayerActions.CloseInventory.performed += i => AlternativeCloseInventoryEvent?.Invoke();
            playerInput.Inventory.CloseInventory.performed += i => CloseInventoryEvent?.Invoke();
            playerInput.Inventory.Rotate.performed += i => RotateItem?.Invoke();

            OpenInventoryEvent += () => { DisablePlayerControls(); EnableInventoryControls(); };
            CloseInventoryEvent += () => { EnablePlayerControls(); DisableInventoryControls(); };
            AlternativeCloseInventoryEvent += () => { EnablePlayerControls(); DisableInventoryControls(); };

            playerInput.PlayerActions.PickUp.performed += i => PickUpEvent?.Invoke();

            playerInput.PlayerActions.Reload.performed += i => ReloadEvent?.Invoke();
        }

        EnablePlayerControls();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Update()
    {
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movement.y;
        horizontalInput = movement.x;

        //  absolute movement amount
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        if (isSprinting && playerStatusManager.stamina > 1)
        {
            moveAmount *= 2;
        }
        else
        {
            isSprinting = false;
        }
    }

    public (bool success, Vector3 position) GetMousePosition()
    {
        var ray = camera.ScreenPointToRay(Mouse.current.position.value);

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, aimMask))
        {
            crosshair.transform.position = hitInfo.point;
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
        playerInput.Disable();
    }

    public void EnableInput()
    {
        playerInput.Enable();
    }

    public void DisablePlayerControls()
    {
        playerInput.PlayerMovement.Disable();
        playerInput.PlayerCombat.Disable();
        playerInput.PlayerActions.Disable();
        playerInput.CameraMovement.Disable();

        movement = Vector2.zero;
    }

    public void EnablePlayerControls()
    {
        playerInput.PlayerMovement.Enable();
        playerInput.PlayerCombat.Enable();
        playerInput.PlayerActions.Enable();
        playerInput.CameraMovement.Enable();
    }

    public void DisableInventoryControls()
    {
        playerInput.Inventory.Disable();
    }

    public void EnableInventoryControls()
    {
        playerInput.Inventory.Enable();
    }

    public void DisableCombatControls()
    {
        isAiming = false;
        playerInput.PlayerCombat.Disable();
    }

    public void EnableCombatControls()
    {
        playerInput.PlayerCombat.Enable();
    }

    public float VerticalInput { get { return verticalInput; } }

    public float HorizontalInput { get { return horizontalInput; } }

    public float MoveAmount { get { return moveAmount; } }

    public float Scroll { get { return scroll; } }

    public bool IsSprinting { get { return isSprinting; } }

    public bool IsCrouching { get { return isCrouching; } set { isCrouching = value; } }

    public bool IsAiming { get { return isAiming; } set { isCrouching = value; } }
}
