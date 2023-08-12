using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExchangeUI : MonoBehaviour
{
    [SerializeField] private ItemExchangeButton exchangeButton;
    [SerializeField] private Interactable catSkullMerchant;
    [SerializeField] private Interactable catToothMerchant;

    public void CloseUI()
    {
        GameManager.Instance.SetItemExchangeUIOpen(false);

        if (!exchangeButton.hasToothObtained)
        {
            catToothMerchant.StartCoroutine(catToothMerchant.ResetObjState());
        }

        if (!exchangeButton.hasSkullObtained)
        {
            catSkullMerchant.StartCoroutine(catSkullMerchant.ResetObjState());
        }
    }
}
