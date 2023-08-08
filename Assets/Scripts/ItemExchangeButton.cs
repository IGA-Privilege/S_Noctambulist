using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ItemExchangeButton : Button
{
    private readonly string requiredItemName = "Cat's Eye";
    private bool isExchanging = false;
    public bool hasExchanged = false;

    protected override void Awake()
    {
        base.Awake();
        GetComponent<Image>().color = Color.clear;
    }

    private void Update()
    {
        if (!hasExchanged)
        {
            if (isExchanging)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    if (PlayerInventory.Instance.instantiatedDraggableIcon.ItemBeingDragged.name == requiredItemName)
                    {
                        SuccessfulExchange();
                    }
                }
            }
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (!hasExchanged)
        {
            if (PlayerInventory.Instance.instantiatedDraggableIcon != null)
            {
                if (PlayerInventory.Instance.instantiatedDraggableIcon.ItemBeingDragged.name == requiredItemName)
                {
                    StartCoroutine(StartMouseListener());
                }
            }
        }
    }

    private void SuccessfulExchange()
    {
        PlayerInventory.Instance.PlayerGetsOpal();
        GetComponent<Image>().color = Color.white;
        GetComponent<Image>().sprite = PlayerInventory.Instance.Opal.icon;
        hasExchanged = true;
    }

    private IEnumerator StartMouseListener()
    {
        isExchanging = true;
        yield return new WaitForSeconds(3);
        isExchanging = false;
    }
}
