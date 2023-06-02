using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Transform eyePoint;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private GameObject debuggingVisualSphere;
    [SerializeField] private Animator animator;

    private float lookRotationX = 0;
    private float lookRotationY = 0;
    private float lookSpeed = 2f;
    private float lookXLimit = 45f;
    private bool canMove;
    private float walkSpeed = 2f;
    private Vector3 moveDirection = Vector3.zero;
    float maxInteractDistance = 2.5f;

    private void Awake()
    {
        canMove = true;
        SetMouseLocked();
    }

    private static void SetMouseLocked()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (canMove)
        {
            KeepEyesTrackingMouse();
            AlignCameraToEyes();
            HighlightLookPos();
            HandleMovement();
            if (Input.GetMouseButtonUp(0))
            {
                TryInteractWithObj();
            }
        }
        FaceSpriteToCamera();
    }



    private void HandleMovement()
    {
        Vector3 forward = transform.TransformDirection(new Vector3(eyePoint.forward.x, 0, eyePoint.forward.z).normalized);
        float curSpeedX = walkSpeed * Input.GetAxis("Vertical");
        moveDirection = forward * curSpeedX;
        characterController.Move(moveDirection * Time.deltaTime);
        animator.SetBool("isMoving", moveDirection.magnitude > 0.01f);
    }

    private void KeepEyesTrackingMouse()
    {
        if (playerCamera.isActiveAndEnabled)
        {
            lookRotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            lookRotationX = Mathf.Clamp(lookRotationX, -lookXLimit, lookXLimit);
            lookRotationY += Input.GetAxis("Mouse X") * lookSpeed;
            eyePoint.rotation = Quaternion.Euler(lookRotationX, lookRotationY, 0);
        }
    }

    private void HighlightLookPos()
    {
        Ray ray = new Ray(eyePoint.position, eyePoint.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxInteractDistance))
        {
            debuggingVisualSphere.transform.position = hitInfo.point;
        }
        else
        {
            debuggingVisualSphere.transform.position = eyePoint.position + eyePoint.forward * maxInteractDistance;
        }
    }

    private void TryInteractWithObj()
    {
        Ray ray = new Ray(eyePoint.position, eyePoint.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxInteractDistance))
        {
            if (hitInfo.collider.TryGetComponent<Interactable>(out Interactable interactableObj))
            {
                interactableObj.OnPlayerInteract();
            }
        }
    }

    private void AlignCameraToEyes()
    {
        float cameraDistanceFromPlayer = 1.0f;

        Ray ray = new Ray(eyePoint.position, -eyePoint.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, cameraDistanceFromPlayer))
        {
            playerCamera.transform.position = hitInfo.point;
        }
        else
        {
            playerCamera.transform.position = eyePoint.position - eyePoint.forward * cameraDistanceFromPlayer;
        }

        playerCamera.transform.LookAt(eyePoint);
    }

    private void FaceSpriteToCamera()
    {
        playerSprite.transform.eulerAngles = new Vector3(0, eyePoint.eulerAngles.y, 0);
    }

}
