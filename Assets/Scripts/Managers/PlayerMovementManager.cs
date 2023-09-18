using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.Animations.Rigging;

//  Summary
//  This class provides movement based on PlayerInputManager
//  Summary

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementManager : MonoBehaviour
{
    [HideInInspector] public static PlayerMovementManager Instance;

    private CharacterController characterController;
    private Camera camera;
    private PlayerInputManager inputManager;

    [Header("Movement Settings")]
    [SerializeField] private float crouchingSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float sprintingSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float aimMovementSpeed;
    [SerializeField] private float aimRotationSpeed;

    [Header("Character Rig Settings")]
    [SerializeField] private Rig aimRig;
    private float aimSmoothVelocity;
    [SerializeField] private float aimSmoothTime;
    private float aimRigWeight = 0f;

    private Vector3 moveDirection;

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

        characterController = GetComponent<CharacterController>();
        inputManager = GetComponent<PlayerInputManager>();
        camera = Camera.main;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        HandleAllMovement();

        //aimRig.weight = Mathf.Lerp(aimRig.weight, aimRigWeight, aimSmoothTime * Time.deltaTime);
        aimRig.weight = Mathf.SmoothDamp(aimRig.weight, aimRigWeight, ref aimSmoothVelocity, aimSmoothTime);
        
    }

    public void HandleAllMovement()
    {
        if (PlayerInputManager.Instance.IsAiming)
        {
            aimRigWeight = 1f;
            HandleAimingMovement();
        }
        else
        {
            aimRigWeight = 0f;
            HandleGroundMovement();
        }
    }

    //  Basic movement
    private void HandleGroundMovement()
    {
        if (PlayerInputManager.Instance.IsCrouching)
        {
            CrouchMovement();
            return;
        }

        StandMovement();
    }

    private void StandMovement()
    {
        CalculateMovementAxis();
        CalculateMovementRotation();

        moveDirection.y = Physics.gravity.y / 2;

        if (PlayerInputManager.Instance.IsSprinting)
        {
            characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
            return;
        }

        characterController.Move(moveDirection * movementSpeed * Time.deltaTime);
    }

    private void CalculateMovementRotation()
    {
        //  Rotate towards the movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion movementRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, movementRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void CalculateMovementAxis()
    {
        moveDirection =
                            new Vector3(camera.transform.forward.x, 0, camera.transform.forward.z) * inputManager.VerticalInput;
        moveDirection = moveDirection +
            new Vector3(camera.transform.right.x, 0, camera.transform.right.z) * inputManager.HorizontalInput;
        moveDirection.Normalize();
    }

    private void CrouchMovement()
    {
        CalculateMovementAxis();
        CalculateMovementRotation();

        moveDirection.y = Physics.gravity.y / 2;

        if (PlayerInputManager.Instance.IsSprinting)
        {
            PlayerInputManager.Instance.IsCrouching = false;
            return;
        }

        characterController.Move(moveDirection * crouchingSpeed * Time.deltaTime);
    }

    //  Aim movement

    private void HandleAimingMovement()
    {
        Aim();
        AimMovement();
    }

    private void Aim()
    {
        var (success, position) = PlayerInputManager.Instance.GetMousePosition();

        if (success)
        {
            //  Calculate direction
            var direction = position - transform.position;
            //  Ignore y rotation
            direction.y = 0;

            //  Rotate
            Quaternion aimRotation = Quaternion.LookRotation(direction, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, aimRotation, aimRotationSpeed * Time.deltaTime);
        }
    }

    private void AimMovement()
    {
        CalculateMovementAxis();
        characterController.Move(moveDirection * aimMovementSpeed * Time.deltaTime);
    }

    public float HorizontalSpeed
    {
        get
        {
            return transform.InverseTransformDirection(moveDirection).normalized.x;
        }
    }

    public float VerticalSpeed
    {
        get
        {
            return transform.InverseTransformDirection(moveDirection).normalized.z;

        }
    }
}
