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
    private bool _hasFirstRiddleSolved = false;
    private bool _hasSecondRiddleSolved = false;

    public void OnPlayerSubmitAnswer()
    {
        if (inputField01.text == answer01 && inputField02.text == answer02)
        {
            SolveFirstRiddle();
        }

        if (inputField03.text == answer03)
        {
            SolveSecondRiddle();
        }

        if (!HasFinished)
        {
            ClearText();
            trigger.StartCoroutine(trigger.ResetObjState());
        }

        GameManager.Instance.SetClozeUIOpen(false);
    }

    private void SolveFirstRiddle()
    {
        if (_hasFirstRiddleSolved)
        {
            return;
        }
        PlayerInventory.Instance.PlayerGetsCatsEye();
        _hasFirstRiddleSolved = true;
        if (_hasSecondRiddleSolved)
        {
            HasFinished = true;
        }
    }

    private void SolveSecondRiddle()
    {
        if (_hasSecondRiddleSolved)
        {
            return;
        }
        PlayerInventory.Instance.PlayerGetsFish();
        _hasSecondRiddleSolved = true;
        if (_hasFirstRiddleSolved)
        {
            HasFinished = true;
        }
    }

    public void ClearText()
    {
        if (!_hasFirstRiddleSolved)
        {
            inputField01.text = null;
            inputField02.text = null;
        }

        if (!_hasSecondRiddleSolved)
        {
            inputField03.text = null;
        }
    }
}
