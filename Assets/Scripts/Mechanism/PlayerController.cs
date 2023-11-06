using DG.Tweening;
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
    public static PlayerController Instance { get; private set; }

    [SerializeField] private Camera playerCamera;
    [SerializeField] private Camera theaterCamera;
    [SerializeField] private TheaterBox theaterBox;
    [SerializeField] private Transform eyePoint;
    [SerializeField] private RectTransform inventoryUI;
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
    //[SerializeField] private Material theaterBGMaterial;
    //[SerializeField] private Color theaterCatColor;
    //[SerializeField] private Color theaterGirlColor;

    private bool _canMove = true;
    private Vector3 floorPos;
    private Vector3 playerManifestPos;
    private Vector3 playerManifestRotation;
    private Vector3 catShadowPos;
    private Vector3 catShadowRotation;
    public bool isCatView = false;
    public bool canControl = true;
    private bool isLookingBehind = false;
    private float switchViewSecCounter = 0f;
    private float lookRotationX = 0;
    private float lookRotationY = 0;
    private float lookSpeed = 800f;
    private float lookXLimit = 20f;
    private float walkSpeed = 2f;
    private Vector3 moveDirection = Vector3.zero;
    float maxInteractDistance = 2.5f;
    Vector3 camLookPoint { get { return eyePoint.position + new Vector3(0, 0.25f, 0); } }


    private void Awake()
    {
        Instance = this;
        MarkOriginalPosition();
        playerCamera.gameObject.SetActive(true);
        theaterCamera.gameObject.SetActive(false);
    }

    private void Start()
    {
        theaterBox.PlayGirlAnimation();
    }

    private void MarkOriginalPosition()
    {
        floorPos = transform.position;
        playerManifestPos = playerMeshRenderer.transform.localPosition;
        playerManifestRotation = playerMeshRenderer.transform.localEulerAngles;
        catShadowPos = catMeshRenderer.transform.localPosition;
        catShadowRotation = catMeshRenderer.transform.localEulerAngles;
    }


    private void Update()
    {
        if (canControl)
        {
            KeepEyesTrackingMouse();
            AlignCameraToEyes();
            HandleMovement();
            DetectSwitchView();
        }

        FacePlayerToCamera();

        if (Input.GetMouseButtonUp(0))
        {
            TryInteractWithObj();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(JumpToFloor());
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

        if (_canMove)
        {
            characterController.Move(moveDirection * Time.deltaTime);
        }
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
                    StartCoroutine(SwitchView());
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


    private IEnumerator SwitchView()
    {
        lookRotationX = 0f;
        switchViewSecCounter = 0f;
        playerCamera.gameObject.SetActive(false);
        theaterCamera.gameObject.SetActive(true);
        inventoryUI.gameObject.SetActive(false);

        yield return StartCoroutine(SwitchViewFadeIn());

        if (isCatView)
        {
            theaterBox.PlayGirlAnimation();
        }
        else
        {
            theaterBox.PlayCatAnimation();
        }

        yield return new WaitForSeconds(5.5f);
        yield return StartCoroutine(SwitchViewFadeOut());

        playerCamera.gameObject.SetActive(true);
        theaterCamera.gameObject.SetActive(false);
        inventoryUI.gameObject.SetActive(true);

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

        yield return StartCoroutine(SwitchViewFadeIn());
    }

    private IEnumerator SwitchViewFadeIn()
    {
        for (float i = 1f; i > 0f; i = i - 0.05f)
        {
            viewSwitchingCurtain.color = new Color(viewSwitchingCurtain.color.r, viewSwitchingCurtain.color.g, viewSwitchingCurtain.color.b, Mathf.Clamp(i, 0f, 1f));
            yield return new WaitForFixedUpdate();
        }
        viewSwitchingCurtain.color = new Color(viewSwitchingCurtain.color.r, viewSwitchingCurtain.color.g, viewSwitchingCurtain.color.b, 0);
    }

    private IEnumerator SwitchViewFadeOut()
    {
        for (float i = 1f; i > 0f; i = i - 0.05f)
        {
            viewSwitchingCurtain.color = new Color(viewSwitchingCurtain.color.r, viewSwitchingCurtain.color.g, viewSwitchingCurtain.color.b, 1 - Mathf.Clamp(i, 0f, 1f));
            yield return new WaitForFixedUpdate();
        }
        viewSwitchingCurtain.color = new Color(viewSwitchingCurtain.color.r, viewSwitchingCurtain.color.g, viewSwitchingCurtain.color.b, viewSwitchingCurtain.color.a);
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
        //Ray ray = new Ray(eyePoint.position, eyePoint.forward);
        Ray ray = new Ray(camLookPoint, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxInteractDistance))
        {
            if (hitInfo.collider.TryGetComponent<Interactable>(out Interactable interactableObj))
            {
                interactableObj.OnPlayerInteract();
            }
            else if (hitInfo.collider.TryGetComponent<MusicBoxButton>(out MusicBoxButton musicBoxButton))
            {
                musicBoxButton.OnPlayerPress();
            }
            else if (hitInfo.collider.TryGetComponent<JumpPlatform>(out JumpPlatform jumpPlatform))
            {
                transform.DOJump(jumpPlatform.transform.position + new Vector3(0, 0.482f, 0), 0.6f, 1, 1f);
                SetPlayerCanMove(false);
                jumpPlatform.OnPlayerJumpOnto();
                if (!jumpPlatform.isRightPlaform)
                {
                    StartCoroutine(JumpToFloor());
                }
            }
        }
    }

    public IEnumerator JumpToFloor()
    {
        yield return new WaitForSeconds(1.1f);
        transform.DOJump(new Vector3(transform.position.x, floorPos.y, transform.position.z), 0f, 1, 1f);
        SetPlayerCanMove(true);
    }

    private void SetPlayerCanMove(bool canMove)
    {
        _canMove = canMove;
    }

    private void AlignCameraToEyes()
    {
        float cameraDistanceFromPlayer = 1.8f;

        Ray ray = new Ray(eyePoint.position, -eyePoint.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, cameraDistanceFromPlayer))
        {
            playerCamera.transform.position = hitInfo.point;
        }
        else
        {
            playerCamera.transform.position = camLookPoint - eyePoint.forward * cameraDistanceFromPlayer;
        }

        playerCamera.transform.LookAt(camLookPoint);
    }



    private void FacePlayerToCamera()
    {
        transform.eulerAngles = new Vector3(0, eyePoint.eulerAngles.y, 0);
    }



}

