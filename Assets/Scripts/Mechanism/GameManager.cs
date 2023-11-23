using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RectTransform itemInfoUI;
    [SerializeField] private Image itemInfoImage;
    [SerializeField] private RectTransform diaryUI;
    [SerializeField] private RectTransform clozeUI;
    [SerializeField] private CodeLock codeLockUI;
    [SerializeField] private RectTransform itemExchangeUI;
    [SerializeField] private ItemExchangeButton itemExchangeButton;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Material floorAndSkyMat;
    [SerializeField] private Color32 catViewColor;
    [SerializeField] private Color32 humanViewColor;
    [SerializeField] private Light[] lights;
    [SerializeField] private List<MeshRenderer> livingRoomWalls;
    [SerializeField] private List<MeshRenderer> storageWalls;
    [SerializeField] private List<MeshRenderer> bedroomWalls;
    [SerializeField] private Material hazeWallMat;
    [SerializeField] private Material BlackWallMat;
    private List<Material> livingRoomWallsMatStorer = new List<Material>();
    private List<Material> storageWallsMatStorer = new List<Material>();
    private List<Material> bedroomWallsMatStorer = new List<Material>();

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        LockCursor();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DispelLivingRoomHaze();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DispelBedroomHaze();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            DispelStorageHaze();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (itemInfoUI.gameObject.activeInHierarchy)
            {
                CloseItemInfoUI();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (diaryUI.gameObject.activeInHierarchy)
            {
                SetDiaryUIOpen(false);
            }
        }

        if (!GetIsAnyUIOpen())
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                UnlockCursor();
                playerController.canControl = false;
            }
            else
            {
                LockCursor();
                playerController.canControl = true;
            }
        }

        UpdateFloorAndSkyMat();
        UpdateLightsColor();
    }

    private void UpdateLightsColor()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            if (playerController.isCatView)
            {
                lights[i].color = (Color)catViewColor;
            }
            else
            {
                lights[i].color = (Color)humanViewColor;
            }
        }
    }

    private void UpdateFloorAndSkyMat()
    {
        if (playerController.isCatView)
        {
            floorAndSkyMat.color = (Color)catViewColor;
        }
        else
        {
            floorAndSkyMat.color = (Color)humanViewColor;
        }
    }

    public static Vector3 GetWorldMousePosition(Camera activeCamera)
    {
        Ray ray = activeCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue);
        return hitInfo.point;
    }

    public bool GetIsAnyUIOpen()
    {
        return (itemInfoUI.gameObject.activeInHierarchy || diaryUI.gameObject.activeInHierarchy || clozeUI.gameObject.activeInHierarchy || itemExchangeUI.gameObject.activeInHierarchy || codeLockUI.gameObject.activeInHierarchy);
    }

    public void OpenItemInfoUI(Sprite itemSprite)
    {
        itemInfoUI.gameObject.SetActive(true);
        itemInfoImage.sprite = itemSprite;
        FreezeTime();
    }

    private void CloseItemInfoUI()
    {
        itemInfoUI.gameObject.SetActive(false);
        UnfreezeTime();
    }

    public void SetItemExchangeUIOpen(bool isOpen)
    {
        itemExchangeUI.gameObject.SetActive(isOpen);
        itemExchangeButton.ResetButtonImage();
        if (isOpen)
        {
            FreezeTime();
            UnlockCursor();
        }
        else
        {
            UnfreezeTime();
            LockCursor();
        }
    }

    public void OpenCodeLock(int answer1, int answer2, int answer3, int answer4, bool isHumanFoot)
    {
        playerController.canControl = false;
        codeLockUI.gameObject.SetActive(true);
        codeLockUI.InitCodeLock(answer1, answer2, answer3, answer4, isHumanFoot);
    }

    public void CloseCodeLock()
    {
        if (!codeLockUI.gameObject.activeInHierarchy)
        {
            return;
        }
        playerController.canControl = true;
        codeLockUI.gameObject.SetActive(false);
    }


    public void SetDiaryUIOpen(bool isOpen)
    {
        diaryUI.gameObject.SetActive(isOpen);
        if (isOpen)
        {
            UnlockCursor();
            playerController.canControl = false;
        }
        else
        {
            LockCursor();
            playerController.canControl = true;
        }
    }

    public void SetClozeUIOpen(bool isOpen)
    {
        clozeUI.gameObject.SetActive(isOpen);
        if (isOpen)
        {
            FreezeTime();
            UnlockCursor();
        }
        else
        {
            UnfreezeTime();
            LockCursor();
        }
    }

    private void FreezeTime()
    {
        Time.timeScale = 0f;
    }

    private void UnfreezeTime()
    {
        Time.timeScale = 1f;
    }

    private static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private static void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void DispelLivingRoomHaze()
    {
        for (int i = 0; i < livingRoomWalls.Count; i++)
        {
            livingRoomWalls[i].material = livingRoomWallsMatStorer[i];
        }
    }

    private void DispelStorageHaze()
    {
        for (int i = 0; i < storageWalls.Count; i++)
        {
            storageWalls[i].material = storageWallsMatStorer[i];
        }
    }

    private void DispelBedroomHaze()
    {
        for (int i = 0; i < bedroomWalls.Count; i++)
        {
            bedroomWalls[i].material = bedroomWallsMatStorer[i];
        }
    }


}

public enum GameUIType
{
    ItemInfoUI, DiaryUI, ClozeUI
}
