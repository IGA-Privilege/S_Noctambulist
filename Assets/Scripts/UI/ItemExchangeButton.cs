using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ItemExchangeButton : Button
{
    private bool isExchanging = false;
    public bool hasSkullObtained = false;
    public bool hasToothObtained = false;

    protected override void Awake()
    {
        base.Awake();
        GetComponent<Image>().color = Color.clear;
    }

    private void Update()
    {
        if (!hasToothObtained || !hasSkullObtained)
        {
            if (isExchanging)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    if (PlayerInventory.Instance.instantiatedDraggableIcon.ItemBeingDragged.name == PlayerInventory.Instance.CatsEye.name)
                    {
                        SuccessfulExchangeForSkull();
                    }
                    else if (PlayerInventory.Instance.instantiatedDraggableIcon.ItemBeingDragged.name == PlayerInventory.Instance.Fish.name)
                    {
                        SuccessfulExchangeForTooth();
                    }
                }
            }
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (!hasToothObtained || !hasSkullObtained)
        {
            if (PlayerInventory.Instance.instantiatedDraggableIcon != null)
            {
                if (PlayerInventory.Instance.instantiatedDraggableIcon.ItemBeingDragged.name == PlayerInventory.Instance.CatsEye.name || PlayerInventory.Instance.instantiatedDraggableIcon.ItemBeingDragged.name == PlayerInventory.Instance.Fish.name)
                {
                    StartCoroutine(StartMouseListener());
                }
            }
        }
    }

    private void SuccessfulExchangeForSkull()
    {
        PlayerInventory.Instance.PlayerGetsHumanSkull();
        GetComponent<Image>().color = Color.white;
        GetComponent<Image>().sprite = PlayerInventory.Instance.HumanSkull.icon;
        hasSkullObtained = true;
    }

    private void SuccessfulExchangeForTooth()
    {
        PlayerInventory.Instance.PlayerGetsCatsTooth();
        GetComponent<Image>().color = Color.white;
        GetComponent<Image>().sprite = PlayerInventory.Instance.CatsTooth.icon;
        hasToothObtained = true;
    }

    public void ResetButtonImage()
    {
        GetComponent<Image>().color = Color.clear;
    }

    private IEnumerator StartMouseListener()
    {
        isExchanging = true;
        yield return new WaitForSeconds(3);
        isExchanging = false;
    }

}
