using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DraggableIcon : MonoBehaviour
{
    [HideInInspector] public InventorySlotButton OriginalButton;
    [HideInInspector] public ItemWrapper ItemBeingDragged;

    public void Init(ItemWrapper itemInfo, InventorySlotButton originalButton)
    {
        ItemBeingDragged = itemInfo;
        OriginalButton = originalButton;
        GetComponent<Image>().sprite = itemInfo.spriteIcon;
        UpdateLocalPosition();
    }


    private void Update()
    {
        if (!OriginalButton.IsButtonDown())
        {
            StartCoroutine(SelfDestory());
        }
        else
        {
            UpdateLocalPosition();

        }
    }

    private void UpdateLocalPosition()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform)
        {
            Vector2 vecMouse = this.CurrMousePosition(transform);
            rectTransform.localPosition = new Vector3(vecMouse.x, vecMouse.y, 0);
        }
    }

    public Vector2 CurrMousePosition(Transform thisTrans)
    {
        Vector2 vecMouse;
        RectTransform parentRectTrans = thisTrans.parent.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTrans, Input.mousePosition, null, out vecMouse);
        return vecMouse;
    }


    private IEnumerator SelfDestory()
    {
        yield return new WaitForEndOfFrame();
        Destroy(this.gameObject);
    }
}
