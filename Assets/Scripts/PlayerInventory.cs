using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Item Nothing;
    [SerializeField] private Item CatsEye;
    [SerializeField] private Item Opal;
    [SerializeField] private Image itemSlotIcon01;
    [SerializeField] private Image itemSlotIcon02;

    private ItemWrapper item01;
    private ItemWrapper item02;

    private static PlayerInventory instance;

    public static PlayerInventory Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<PlayerInventory>();
            return instance;
        }
    }

    private void Awake()
    {
        item01 = new ItemWrapper(Nothing);
        item02 = new ItemWrapper(Nothing);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerGetsCatsEye();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerGetsOpal();
        }
        SyncInventoryUI();
    }

    public void PlayerGetsCatsEye()
    {
        item01 = new ItemWrapper(CatsEye);
    }

    public void PlayerGetsOpal()
    {
        item01 = new ItemWrapper(Opal);
    }

    private void SyncInventoryUI()
    {
        itemSlotIcon01.sprite = item01.spriteIcon;
        itemSlotIcon02.sprite = item02.spriteIcon;
    }
}

public struct ItemWrapper
{
    public string name;
    public Sprite spriteIcon;

    public ItemWrapper(Item item)
    {
        this.name = item.name;
        this.spriteIcon = item.icon;
    }
}
