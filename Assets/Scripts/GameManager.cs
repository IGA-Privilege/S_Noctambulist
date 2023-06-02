using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RectTransform itemInfoUI;
    [SerializeField] private Image itemInfoImage;
    [SerializeField] private RectTransform diaryUI;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        CloseAllUIs();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (itemInfoUI.gameObject.activeInHierarchy)
            {
                itemInfoUI.gameObject.SetActive(false);
            }
            else if (diaryUI.gameObject.activeInHierarchy)
            {
                // 后续做出日记后这里改为让日记本翻页
                diaryUI.gameObject.SetActive(false);
            }
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
        }
        return false;
    }

    public void OpenItemInfoUI(Sprite itemSprite)
    {
        itemInfoUI.gameObject.SetActive(true);
        itemInfoImage.sprite = itemSprite;
    }

    public void CloseAllUIs()
    {
        itemInfoUI.gameObject.SetActive(false);
        diaryUI.gameObject.SetActive(false);
    }

    public void OpenDiaryUI()
    {
        diaryUI.gameObject.SetActive(true);
    }
}

public enum GameUIType
{
    ItemInfoUI, DiaryUI
}
