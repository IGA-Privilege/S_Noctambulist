using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Collider bedCollider;

    [SerializeField] private GameObject catViewModel;
    [SerializeField] private GameObject humanViewModel;

    private void Update()
    {
        UpdateCollider();
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

    private void UpdateCollider()
    {
        if (player.isCatView)
        {
            bedCollider.enabled = false;
        }
        else
        {
            bedCollider.enabled = true;
        }
    }
}
