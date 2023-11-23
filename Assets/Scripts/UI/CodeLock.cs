using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeLock : MonoBehaviour
{
    [HideInInspector] public CodeLockUnit SelectedUnit;
    [SerializeField] private CodeLockUnit unit1;
    [SerializeField] private CodeLockUnit unit2;
    [SerializeField] private CodeLockUnit unit3;
    [SerializeField] private CodeLockUnit unit4;
    private int[] _answer;
    private bool isRewardHumanFoot;

    private void Start()
    {
        SelectedUnit = unit1;
    }

    public void InitCodeLock(int answer1, int answer2, int answer3, int answer4, bool isHumanFoot)
    {
        _answer = new int[]{ answer1, answer2, answer3, answer4 };
        unit1.Number = 0;
        unit2.Number = 0;
        unit3.Number = 0;
        unit4.Number = 0;
        isRewardHumanFoot = isHumanFoot;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            RollToNextUnit(false);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            RollToNextUnit(true);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            SelectedUnit.NumberIncrease();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SelectedUnit.NumberDecrease();
        }

        if (unit1.Number == _answer[0] && unit2.Number == _answer[1] && unit3.Number == _answer[2] && unit4.Number == _answer[3])
        {
            StartCoroutine(Unlock());
        }
    }

    private IEnumerator Unlock()
    {
        Debug.Log("Solved Code Puzzle!");
        yield return new WaitForSeconds(0.5f);
        if (isRewardHumanFoot)
        {
            PlayerInventory.Instance.PlayerGetsHumanFoot();
        }
        else
        {
            PlayerInventory.Instance.PlayerGetsCatTail();
        }
        GameManager.Instance.CloseCodeLock();
    }


    private void RollToNextUnit(bool isLeftward)
    {
        if (isLeftward)
        {
            if (SelectedUnit == unit1)
            {
                SelectedUnit = unit4;
            }
            else if (SelectedUnit == unit2)
            {
                SelectedUnit = unit1;
            }
            else if (SelectedUnit == unit3)
            {
                SelectedUnit = unit2;
            }
            else if (SelectedUnit == unit4)
            {
                SelectedUnit = unit3;
            }
        }
        else
        {
            if (SelectedUnit == unit1)
            {
                SelectedUnit = unit2;
            }
            else if (SelectedUnit == unit2)
            {
                SelectedUnit = unit3;
            }
            else if (SelectedUnit == unit3)
            {
                SelectedUnit = unit4;
            }
            else if (SelectedUnit == unit4)
            {
                SelectedUnit = unit1;
            }
        }
    }
}
