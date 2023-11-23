using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour
{
    public DraggableIcon draggableIconPref;
    [HideInInspector] public DraggableIcon instantiatedDraggableIcon;

    [SerializeField] private Item Nothing;
    [SerializeField] public Item CatsEye;
    [SerializeField] private Item CatsPaw;
    [SerializeField] public Item CatsTooth;
    [SerializeField] public Item Fish;
    [SerializeField] public Item HumanSkull;
    [SerializeField] private Item HumanHand;
    [SerializeField] private Item HumanFoot;
    [SerializeField] private Item CatsTail;
    [SerializeField] private Item PowderyMoonlight;
    [SerializeField] private RectTransform itemSlotPref;
    [SerializeField] private RectTransform itemSlotParent;

    public List<ItemWrapper> Items = new List<ItemWrapper>();
    private List<InventorySlotButton> itemSlots;

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
        instance = this;
        itemSlots = new List<InventorySlotButton>(itemSlotParent.GetComponentsInChildren<InventorySlotButton>());
    }


    private void Update()
    {
        SyncInventoryUI();
    }

    public void PlayerGetsCatsEye()
    {
        ObtainNewItem(new ItemWrapper(CatsEye));
    }

    public void PlayerGetsPowderyMoonlight()
    {
        ObtainNewItem(new ItemWrapper(PowderyMoonlight));
    }

    public void PlayerGetsHumanFoot()
    {
        ObtainNewItem(new ItemWrapper(HumanFoot));
    }

    public void PlayerGetsHumanHand()
    {
        ObtainNewItem(new ItemWrapper(HumanHand));
    }
    public void PlayerGetsCatTail()
    {
        ObtainNewItem(new ItemWrapper(CatsTail));
    }

    public void PlayerGetsHumanSkull()
    {
        ExchangeItem(CatsEye.name, new ItemWrapper(HumanSkull));
    }

    public void PlayerGetsCatsTooth()
    {
        ExchangeItem(Fish.name, new ItemWrapper(CatsTooth));
    }

    private void ObtainNewItem(ItemWrapper newItem)
    {
        Items.Add(newItem);
    }

    private void ExchangeItem(string nameOfItemExchanged, ItemWrapper newItem)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].name == nameOfItemExchanged)
            {
                Items[i] = newItem;
                break;
            }
        }
    }

    private void SyncInventoryUI()
    {
        if (itemSlots.Count < Items.Count)
        {
            Debug.LogError("Incorrect Slots Number!");
        }

        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (Items.Count > i)
            {
                itemSlots[i].SetItem(Items[i]);
            }
        }
    }

    private void AddNewItemSlot()
    {
        itemSlots.Add(Instantiate(itemSlotPref, itemSlotParent.transform).GetComponent<InventorySlotButton>());
    }

    public void PlayerGetsFish()
    {
        ObtainNewItem(new ItemWrapper(Fish));
    }

    public void PlayerGetsCatsPaw()
    {
        ObtainNewItem(new ItemWrapper(CatsPaw));
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
