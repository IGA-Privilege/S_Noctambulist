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
    [SerializeField] private GameObject playerModel;
    [SerializeField] private MeshRenderer playerShadow;
    [SerializeField] private GameObject catModel;
    [SerializeField] private MeshRenderer catShadow;
    [SerializeField] private Material playerFrontIdleMat;
    [SerializeField] private Material playerBackIdleMat;
    [SerializeField] private Material playerLeftIdleMat;
    [SerializeField] private Material playerRightIdleMat;
    [SerializeField] private Material catFrontIdleMat;
    [SerializeField] private Material catBackIdleMat;
    [SerializeField] private Material catLeftIdleMat;
    [SerializeField] private Material catRightIdleMat;
    [SerializeField] private Image viewSwitchingCurtain;
    //[SerializeField] private Material theaterBGMaterial;
    //[SerializeField] private Color theaterCatColor;
    //[SerializeField] private Color theaterGirlColor;
    [SerializeField] private GameObject debugSphere;
    [SerializeField] private Animator girlAnimator;
    [SerializeField] private Animator catAnimator;

    private bool _canMove = true;
    private Vector3 floorPos;
    private Vector3 playerManifestPos;
    private Vector3 playerManifestRotation;
    private Vector3 catShadowPos;
    private Vector3 catShadowRotation;
    public bool isCatView = false;
    public bool canControl { get; set; }
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
        canControl = true;
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
    }

    public void MoveSingleDistance(float moveDistance)
    {
        StartCoroutine(MoveSingleDistance(moveDirection * moveDistance));
        StartCoroutine(SwitchViewFadeIn());
    }

    public void TeleportPlayerTo(Vector3 position)
    {
        characterController.enabled = false;
        transform.position = new Vector3(position.x, transform.position.y, position.z);
        StartCoroutine(Teleport());
    }

    private IEnumerator Teleport()
    {
        yield return new WaitForFixedUpdate();
        characterController.enabled = true;
        yield return StartCoroutine(SwitchViewFadeIn());
    }

    public IEnumerator MoveSingleDistance(Vector3 movement)
    {
        yield return new WaitForEndOfFrame();
        characterController.Move(movement);
        StartCoroutine(SwitchViewFadeIn());
    }

    private void Update()
    {
        if (canControl)
        {
            KeepEyesTrackingMouse();
            HandleMovement();
            AlignCameraToEyes();
            DetectSwitchView();
        }

        FacePlayerToCamera();

        if (Input.GetMouseButtonUp(0))
        {
            TryInteractWithObj();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(TeleportToFloor());
        }
    }

    private void HandleMovement()
    {
        Vector3 forward = new Vector3(eyePoint.forward.x, 0, eyePoint.forward.z).normalized;
        float curSpeedX = walkSpeed * Input.GetAxisRaw("Vertical");
        Vector3 right = new Vector3(eyePoint.right.x, 0, eyePoint.right.z).normalized;
        float curSpeedZ = walkSpeed * Input.GetAxisRaw("Horizontal");
        if (curSpeedX > 0.7f)
        {
            playerModel.transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
            catModel.transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
            playerShadow.material = playerBackIdleMat;
            catShadow.material = catBackIdleMat;
            isLookingBehind = false;
        }
        else if (curSpeedX < -0.7f)
        {
            playerModel.transform.rotation = Quaternion.LookRotation(-1f * transform.forward, Vector3.up);
            catModel.transform.rotation = Quaternion.LookRotation(-1f * transform.forward, Vector3.up);
            playerShadow.material = playerFrontIdleMat;
            catShadow.material = catFrontIdleMat;
            isLookingBehind = true;
        }
        else if (curSpeedZ > 0.7f)
        {
            if (isCatView)
            {
                playerShadow.material = playerLeftIdleMat;
                catModel.transform.rotation = Quaternion.LookRotation(transform.right, Vector3.up);
            }
            else
            {
                playerModel.transform.rotation = Quaternion.LookRotation(transform.right, Vector3.up);
                catShadow.material = catLeftIdleMat;
            }
            isLookingBehind = false;
        }
        else if (curSpeedZ < -0.7f)
        {
            if (isCatView)
            {
                playerShadow.material = playerRightIdleMat;
                catModel.transform.rotation = Quaternion.LookRotation(-1f * transform.right, Vector3.up);
            }
            else
            {
                playerModel.transform.rotation = Quaternion.LookRotation(-1f * transform.right, Vector3.up);
                catShadow.material = catRightIdleMat;
            }
            isLookingBehind = false;
        }
        moveDirection = Mathf.Abs(curSpeedX) > 0.7f ? forward * curSpeedX : right * curSpeedZ;
        moveDirection.y = 0f;

        if (_canMove)
        {
            characterController.Move(moveDirection * Time.deltaTime);
            if (moveDirection.magnitude != 0)
            {
                girlAnimator.SetBool("isMoving", true);
                catAnimator.SetBool("isMoving", true);
            }
            else
            {
                girlAnimator.SetBool("isMoving", false);
                catAnimator.SetBool("isMoving", false);
            }
        }
        else
        {
            girlAnimator.SetBool("isMoving", false);
            catAnimator.SetBool("isMoving", false);
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
            playerModel.gameObject.SetActive(true);
            catModel.gameObject.SetActive(false);
            catShadow.gameObject.SetActive(true);
            playerShadow.gameObject.SetActive(false);
        }
        else
        {
            isCatView = true;
            playerModel.gameObject.SetActive(false);
            catModel.gameObject.SetActive(true);
            catShadow.gameObject.SetActive(false);
            playerShadow.gameObject.SetActive(true);
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

    private IEnumerator TeleportToFloor()
    {
        yield return StartCoroutine(SwitchViewFadeOut());
        transform.DOJump(new Vector3(transform.position.x, floorPos.y, transform.position.z), 0f, 1, 0.1f);
        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(SwitchViewFadeIn());
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
        debugSphere.transform.position = camLookPoint;
    }

    private void FacePlayerToCamera()
    {
        transform.eulerAngles = new Vector3(0, eyePoint.eulerAngles.y, 0);
    }
}

