using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;

//  Summary
//  This class is reading input from the input devices
//  Summary
[RequireComponent(typeof(PlayerMovementManager))]
public class PlayerInputManager : MonoBehaviour
{
    [HideInInspector] public static PlayerInputManager Instance;

    private PlayerInput playerInput;
    private Camera camera;

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
    [SerializeField] private LayerMask aimMask;

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

        camera = Camera.main;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = new PlayerInput();

            //  All the input actions should be evented here
            playerInput.PlayerMovement.Movement.performed += i => movement = i.ReadValue<Vector2>();

            playerInput.CameraMovement.Zoom.performed += i => scroll = i.ReadValue<float>();

            playerInput.PlayerMovement.Sprint.performed += i => isSprinting = true;
            playerInput.PlayerMovement.Sprint.canceled += i => isSprinting = false;

            playerInput.PlayerActions.Crouch.performed += i => isCrouching = !isCrouching;

            playerInput.PlayerCombat.Aim.performed += i => isAiming = !isAiming;
        }

        playerInput.Enable();
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

        if (IsSprinting)
        {
            moveAmount *= 2;
        }
    }

    public (bool success, Vector3 position) GetMousePosition()
    {
        var ray = camera.ScreenPointToRay(Mouse.current.position.value);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, aimMask))
        {
            Debug.DrawRay(camera.transform.position, ray.direction, Color.red);
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

    public float VerticalInput { get { return verticalInput; } }

    public float HorizontalInput { get { return horizontalInput; } }

    public float MoveAmount { get { return moveAmount; } }

    public float Scroll { get { return scroll; } }

    public bool IsSprinting { get { return isSprinting; } }

    public bool IsCrouching { get { return isCrouching; } set { isCrouching = value; } }

    public bool IsAiming { get { return isAiming; } set { isCrouching = value; } }
}
