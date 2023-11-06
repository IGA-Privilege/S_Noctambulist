using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeLockedObj : Interactable
{
    [SerializeField] private int _answer1;
    [SerializeField] private int _answer2;
    [SerializeField] private int _answer3;
    [SerializeField] private int _answer4;

    public override void OnPlayerInteract()
    {
        base.OnPlayerInteract();
        GameManager.Instance.OpenCodeLock(_answer1, _answer2, _answer3, _answer4);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.CloseCodeLock();
        }
    }
}
