using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] private GameObject catViewModel;
    [SerializeField] private GameObject humanViewModel;

    private void Update()
    {
        if (PlayerController.Instance.isCatView)
        {
            catViewModel.gameObject.SetActive(true);
            humanViewModel.gameObject.SetActive(false);
        }
        else
        {
            catViewModel.gameObject.SetActive(false);
            humanViewModel.gameObject.SetActive(true);
        }
    }
}
