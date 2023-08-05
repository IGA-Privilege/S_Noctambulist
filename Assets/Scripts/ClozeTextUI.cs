using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClozeTextUI : MonoBehaviour
{
    [SerializeField] Interactable trigger;
    [SerializeField] TMP_InputField inputField01;
    [SerializeField] TMP_InputField inputField02;
    [SerializeField] TMP_InputField inputField03;
    public bool HasFinished = false;

    private readonly string answer01 = "cat";
    private readonly string answer02 = "eye";
    private readonly string answer03 = "Fish";

    public void OnPlayerSubmitAnswer()
    {
        if (inputField01.text == answer01 && inputField02.text == answer02 && inputField03.text == answer03)
        {
            PlayerInventory.Instance.PlayerGetsCatsEye();
            Debug.Log("Answer Correct!");
            HasFinished = true;
        }
        else
        {
            ClearText();
            trigger.StartCoroutine(trigger.ResetObjState());
            Debug.Log("Not the right answer!");
        }

        GameManager.Instance.SetClozeUIOpen(false);
    }

    public void ClearText()
    {
        inputField01.text = null;
        inputField02.text = null;
        inputField03.text = null;
    }
}
