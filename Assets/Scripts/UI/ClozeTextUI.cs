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
    [SerializeField] TMP_InputField inputField04;

    private readonly string firstAnswer01 = "cat";
    private readonly string firstAnswer02 = "eye";
    private readonly string secondAnswer01 = "FI";
    private readonly string secondAnswer02 = "H";

    [SerializeField] private RectTransform firstRiddle;
    [SerializeField] private RectTransform secondRiddle;
    private bool _hasFirstRiddleSolved = false;
    private bool _hasSecondRiddleSolved = false;

    public bool HasFinished { get { return _hasFirstRiddleSolved && _hasSecondRiddleSolved; } }

    private void Start()
    {
        firstRiddle.gameObject.SetActive(true);
        secondRiddle.gameObject.SetActive(false);
        ClearText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnPlayerSubmitAnswer();
        }
    }


    public void OnPlayerSubmitAnswer()
    {
        if (inputField01.text.ToLower() == firstAnswer01.ToLower() && inputField02.text.ToLower() == firstAnswer02.ToLower())
        {
            SolveFirstRiddle();
        }

        if (inputField03.text.ToLower() == secondAnswer01.ToLower() && inputField04.text.ToLower() == secondAnswer02.ToLower())
        {
            SolveSecondRiddle();
        }

        if (!_hasFirstRiddleSolved || !_hasSecondRiddleSolved)
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
        ShowSecondRiddle();
    }

    private void SolveSecondRiddle()
    {
        if (_hasSecondRiddleSolved)
        {
            return;
        }
        PlayerInventory.Instance.PlayerGetsFish();
        _hasSecondRiddleSolved = true;
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
            inputField04.text = null;
        }
    }

    public void ShowSecondRiddle()
    {
        firstRiddle.gameObject.SetActive(false);
        secondRiddle.gameObject.SetActive(true);
    }
}
