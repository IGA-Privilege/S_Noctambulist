using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventorySlotButton : Button
{
    private ItemWrapper _itemInfo;
    private Image _iconImage;

    protected override void Awake()
    {
        base.Awake();
        _iconImage = GetComponent<Image>();
    }

    public bool IsButtonDown()
    {
        return IsPressed();
    }

    public void SetItem(ItemWrapper itemInfo)
    {
        if (_iconImage == null)
        {
            _iconImage = GetComponent<Image>();
        }
        _itemInfo = itemInfo;
        _iconImage.sprite = itemInfo.spriteIcon;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        if (IsPressed())
        {
            DraggableIcon draggableIcon = Instantiate(PlayerInventory.Instance.draggableIconPref, transform);
            draggableIcon.Init(_itemInfo, this);
            PlayerInventory.Instance.instantiatedDraggableIcon = draggableIcon;
        }
    }
}
