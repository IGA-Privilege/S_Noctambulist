using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeLockedObj : Interactable
{
    [SerializeField] private int _answer1;
    [SerializeField] private int _answer2;
    [SerializeField] private int _answer3;
    [SerializeField] private int _answer4;

    [SerializeField] private GameObject humanViewModel;
    [SerializeField] private GameObject catViewModel;

    [SerializeField] private bool isHumanFoot;

    public override void OnPlayerInteract()
    {
        base.OnPlayerInteract();
        if (!PlayerController.Instance.isCatView)
        {
            GameManager.Instance.OpenCodeLock(_answer1, _answer2, _answer3, _answer4, isHumanFoot);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.CloseCodeLock();
        }

        if (humanViewModel != null && catViewModel != null)
        {
            if (PlayerController.Instance.isCatView)
            {
                catViewModel.SetActive(true);
                humanViewModel.SetActive(false);
            }
            else
            {
                catViewModel.SetActive(false);
                humanViewModel.SetActive(true);
            }
        }
    }
}
