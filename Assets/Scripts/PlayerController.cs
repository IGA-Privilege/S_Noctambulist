using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform eyePoint;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private MeshRenderer playerMeshRenderer;
    [SerializeField] private Material playerFrontIdleMat;
    [SerializeField] private Material playerBackIdleMat;
    [SerializeField] private Material playerLeftIdleMat;
    [SerializeField] private Material playerRightIdleMat;
    [SerializeField] private MeshRenderer catMeshRenderer;
    [SerializeField] private Material catFrontIdleMat;
    [SerializeField] private Material catBackIdleMat;
    [SerializeField] private Material catLeftIdleMat;
    [SerializeField] private Material catRightIdleMat;
    [SerializeField] private Transform playerShadow;
    [SerializeField] private Transform catManifest;
    [SerializeField] private Image viewSwitchingCurtain;

    private Vector3 playerManifestPos;
    private Vector3 playerManifestRotation;
    private Vector3 catShadowPos;
    private Vector3 catShadowRotation;
    public bool isCatView = false;
    private bool isLookingBehind = false;
    private float switchViewSecCounter = 0f;
    private float lookRotationX = 0;
    private float lookRotationY = 0;
    private float lookSpeed = 800f;
    private float lookXLimit = 45f;
    private float walkSpeed = 2f;
    private Vector3 moveDirection = Vector3.zero;
    float maxInteractDistance = 2.5f;

    private void Awake()
    {
        
        MarkOriginalPosition();
        //SetMouseLocked();
    }

    private void MarkOriginalPosition()
    {
        playerManifestPos = playerMeshRenderer.transform.localPosition;
        playerManifestRotation = playerMeshRenderer.transform.localEulerAngles;
        catShadowPos = catMeshRenderer.transform.localPosition;
        catShadowRotation = catMeshRenderer.transform.localEulerAngles;
    }


    private void Update()
    {
        KeepEyesTrackingMouse();
        AlignCameraToEyes();
        HandleMovement();
        DetectSwitchView();
        FacePlayerToCamera();

        if (Input.GetMouseButtonUp(0))
        {
            TryInteractWithObj();
        }
    }



    private void HandleMovement()
    {
        Vector3 forward = new Vector3(eyePoint.forward.x, 0, eyePoint.forward.z).normalized;
        float curSpeedX = walkSpeed * Input.GetAxis("Vertical");
        Vector3 right = new Vector3(eyePoint.right.x, 0, eyePoint.right.z).normalized;
        float curSpeedZ = walkSpeed * Input.GetAxis("Horizontal");
        if (curSpeedX > 0.7f)
        {
            playerMeshRenderer.material = playerBackIdleMat;
            catMeshRenderer.material = catBackIdleMat;
            isLookingBehind = false;
        }
        else if (curSpeedX < -0.7f)
        {
            playerMeshRenderer.material = playerFrontIdleMat;
            catMeshRenderer.material = catFrontIdleMat;
            isLookingBehind = true;
        }
        else if (curSpeedZ > 0.7f)
        {
            if (isCatView)
            {
                playerMeshRenderer.material = playerLeftIdleMat;
                catMeshRenderer.material = catRightIdleMat;
            }
            else
            {
                playerMeshRenderer.material = playerRightIdleMat;
                catMeshRenderer.material = catLeftIdleMat;
            }
            isLookingBehind = false;
        }
        else if (curSpeedZ < -0.7f)
        {
            if (isCatView)
            {
                playerMeshRenderer.material = playerRightIdleMat;
                catMeshRenderer.material = catLeftIdleMat;
            }
            else
            {
                playerMeshRenderer.material = playerLeftIdleMat;
                catMeshRenderer.material = catRightIdleMat;
            }
            isLookingBehind = false;
        }
        moveDirection = Mathf.Abs(curSpeedX) > 0.7f ? forward * curSpeedX : right * curSpeedZ;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void DetectSwitchView()
    {
        if (isLookingBehind)
        {
            if (lookRotationX > lookXLimit - 1f)
            {
                switchViewSecCounter += Time.deltaTime;
                float waitSec = 2f;
                viewSwitchingCurtain.color = new Color(viewSwitchingCurtain.color.r, viewSwitchingCurtain.color.g, viewSwitchingCurtain.color.b, switchViewSecCounter / waitSec);

                if (switchViewSecCounter > waitSec)
                {
                    SwitchView();
                }
                return;
            }
        }

        if (switchViewSecCounter > 0f)
        {
            switchViewSecCounter = 0f;
            viewSwitchingCurtain.color = new Color(viewSwitchingCurtain.color.r, viewSwitchingCurtain.color.g, viewSwitchingCurtain.color.b, 0);
        }
    }


    private void SwitchView()
    {
        lookRotationX = 0f;
        switchViewSecCounter = 0f;
        StartCoroutine(SwitchViewFadeIn());
        if (isCatView)
        {
            isCatView = false;
            playerMeshRenderer.transform.localPosition = playerManifestPos;
            playerMeshRenderer.transform.localEulerAngles = playerManifestRotation;
            catMeshRenderer.transform.localPosition = catShadowPos;
            catMeshRenderer.transform.localEulerAngles = catShadowRotation;
        }
        else
        {
            isCatView = true;
            playerMeshRenderer.transform.localPosition = playerShadow.localPosition;
            playerMeshRenderer.transform.localEulerAngles = playerShadow.localEulerAngles;
            catMeshRenderer.transform.localPosition = catManifest.localPosition;
            catMeshRenderer.transform.localEulerAngles = catManifest.localEulerAngles;
        }
    }

    private IEnumerator SwitchViewFadeIn()
    {
        for (float i = 1f; i > 0f; i = i - 0.05f)
        {
            viewSwitchingCurtain.color = new Color(viewSwitchingCurtain.color.r, viewSwitchingCurtain.color.g, viewSwitchingCurtain.color.b, i);
            yield return new WaitForFixedUpdate();
        }
        viewSwitchingCurtain.color = new Color(viewSwitchingCurtain.color.r, viewSwitchingCurtain.color.g, viewSwitchingCurtain.color.b, 0);
    }

    private void KeepEyesTrackingMouse()
    {
        if (playerCamera.isActiveAndEnabled)
        {
            lookRotationX += -Input.GetAxis("Mouse Y") * lookSpeed * Time.deltaTime;
            lookRotationX = Mathf.Clamp(lookRotationX, -lookXLimit, lookXLimit);
            lookRotationY += Input.GetAxis("Mouse X") * lookSpeed * Time.deltaTime;
            eyePoint.rotation = Quaternion.Euler(lookRotationX, lookRotationY, 0);
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

    private void FacePlayerToCamera()
    {
        transform.eulerAngles = new Vector3(0, eyePoint.eulerAngles.y, 0);
    }

}

