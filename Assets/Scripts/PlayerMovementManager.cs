using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    }

    public void HandleAllMovement()
    {
        HandleGroundMovement();
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
        CalculateRotation();

        moveDirection.y = Physics.gravity.y / 2;

        if (PlayerInputManager.Instance.IsSprinting)
        {
            characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
            return;
        }

        characterController.Move(moveDirection * movementSpeed * Time.deltaTime);
    }

    private void CalculateRotation()
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
        CalculateRotation();

        moveDirection.y = Physics.gravity.y / 2;

        if (PlayerInputManager.Instance.IsSprinting)
        {
            PlayerInputManager.Instance.IsCrouching = false;
            return;
        }

        characterController.Move(moveDirection * crouchingSpeed * Time.deltaTime);
    }
}
