using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExchangeUI : MonoBehaviour
{
    [SerializeField] private ItemExchangeButton exchangeButton;
    [SerializeField] private Interactable trigger;

    public void CloseUI()
    {
        GameManager.Instance.SetItemExchangeUIOpen(false);
        if (!exchangeButton.hasExchanged)
        {
            trigger.StartCoroutine(trigger.ResetObjState());
        }
    }
}
