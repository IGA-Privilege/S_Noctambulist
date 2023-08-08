using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public DraggableIcon draggableIconPref;
    [HideInInspector] public DraggableIcon instantiatedDraggableIcon;

    [SerializeField] private Item Nothing;
    [SerializeField] private Item CatsEye;
    [SerializeField] public Item Opal;
    [SerializeField] private InventorySlotButton itemSlot01;
    [SerializeField] private InventorySlotButton itemSlot02;


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
        itemSlot01.SetItem(item01);
        itemSlot02.SetItem(item02);
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
