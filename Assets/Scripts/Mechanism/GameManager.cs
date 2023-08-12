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
    [SerializeField] private RectTransform itemExchangeUI;
    [SerializeField] private ItemExchangeButton itemExchangeButton;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Material floorAndSkyMat;
    [SerializeField] private Color32 catViewColor;
    [SerializeField] private Color32 humanViewColor;
    [SerializeField] private Light[] lights;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        LockCursor();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (itemInfoUI.gameObject.activeInHierarchy)
            {
                CloseItemInfoUI();
            }
            else if (diaryUI.gameObject.activeInHierarchy)
            {
                // 后续做出日记后这里改为让日记本翻页
                diaryUI.gameObject.SetActive(false);
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

    public bool GetIsUIOpen(GameUIType ui)
    {
        switch (ui)
        {
            case GameUIType.ItemInfoUI:
                {
                    return itemInfoUI.gameObject.activeInHierarchy;
                }
            case GameUIType.DiaryUI:
                {
                    return diaryUI.gameObject.activeInHierarchy;
                }
            case GameUIType.ClozeUI:
                {
                    return clozeUI.gameObject.activeInHierarchy;
                }
        }
        return false;
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


    public void SetDiaryUIOpen(bool isOpen)
    {
        diaryUI.gameObject.SetActive(isOpen);
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
}

public enum GameUIType
{
    ItemInfoUI, DiaryUI, ClozeUI
}
